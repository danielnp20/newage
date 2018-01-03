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
using NewAge.Librerias.Project;
using DevExpress.XtraGrid.Columns;
using System.Reflection;
using NewAge.DTO.UDT;
using DevExpress.Data;
using DevExpress.XtraVerticalGrid.Events;
using NewAge.DTO.Resultados;
using DevExpress.XtraEditors;
using System.Threading;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class PlanillaDiariaTrabajo : DocumentNominaForm
    {

        #region Variables

        BaseController _bc = BaseController.GetInstance();
        private List<DTO_noPlanillaDiariaTrabajo> _planillas = null;
        private int origen;
        private Int16 contratoNo = 0;

        #endregion

        #region Delegados

        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected override void SaveMethod()
        {
            this.RefreshDocument();
        }

        private delegate void AgregarPlanilla();
        private AgregarPlanilla agregarPlanillaDelegate;

        private void AgregarPlanillaMethod()
        {
            //agrega las planillas al sistema
            var result = _bc.AdministrationModel.Nomina_AddPlanillaDiaria(_planillas);
            if (result.Result == ResultValue.OK)
            {
                this.RefreshDocument();
                this.FieldsEnabled(false);
            }

            //Recarga la grilla de novedades
            this.contratoNo = !string.IsNullOrEmpty(this.txtContrato.Text) ? Convert.ToInt16(this.txtContrato.Text) : (Int16)0;
            this._planillas = _bc.AdministrationModel.Nomina_GetPlanillaDiaria(this.uc_Empleados.empleadoActivo.ID.Value, this.dtPeriod.DateTime, contratoNo);
            gcDocument.DataSource = _planillas;

            MessageForm frm = new MessageForm(result);
            frm.ShowDialog();
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Inicializa las controles
        /// </summary>
        private void InitControls()
        {
            this.uc_Empleados.Init();
            this.uc_Empleados.SelectRowEmpleado_Click += new UC_Empleados.EventHandler(uc_Empleados_SelectRowEmpleado_Click);
            this.editBtnGrid.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(editBtnGrid_ButtonClick);
            //TablesResources.GetTableResources(this.cmbPeriodo, typeof(PeriodoPago));

            this.FieldsEnabled(false);
        }

        /// <summary>
        /// Campos para habilitar o deshabilitar los controles
        /// </summary>
        /// <param name="estado">estado true o false</param>
        private void FieldsEnabled(bool estado)
        {

        }

        /// <summary>
        /// lista las novedades del empleado seleccionado
        /// </summary>
        /// <returns></returns>
        private List<DTO_noPlanillaDiariaTrabajo> GetNovedades()
        {
            this.contratoNo = !string.IsNullOrEmpty(this.txtContrato.Text) ? Convert.ToInt16(this.txtContrato.Text) : (Int16)0;
            _planillas = _bc.AdministrationModel.Nomina_GetPlanillaDiaria(this.uc_Empleados.empleadoActivo.ID.Value, this.dtPeriod.DateTime, this.contratoNo);
            return _planillas;
        }

        /// <summary>
        /// Limpia los campos y objetos del documento
        /// </summary>
        private void RefreshDocument()
        {
            this.gcDocument.DataSource = null;
            this.dtFecha.Enabled = false;
            FormProvider.Master.itemPrint.Enabled = false;
            FormProvider.Master.itemSave.Enabled = false;
            this.FieldsEnabled(true);
            this.LoadData(true);
        }

        private void ValoresDefecto()
        {
            DTO_noConceptoPlaTra dto = (DTO_noConceptoPlaTra)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noConceptoPlaTra, false, this.gvDocument.EditingValue.ToString(), true);
            _planillas[this.gvDocument.FocusedRowHandle].HorasEXTDiu = dto.HED;
            _planillas[this.gvDocument.FocusedRowHandle].HorasEXTNoc = dto.HEN;
            _planillas[this.gvDocument.FocusedRowHandle].HorasNORDiu = dto.HD;
            _planillas[this.gvDocument.FocusedRowHandle].HorasRECNoc = dto.HRN;
            this.gcDocument.DataSource = _planillas;
            this.gvDocument.PostEditor();
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
            if (firstTime)
            {
                
            }

        }

        /// <summary>
        /// Trae el documento id de una maestra de acuerdo al nombre de un campo
        /// </summary>
        /// <param name="colName">Nombre del campo</param>
        /// <returns>Retorna el documento id de una maestra</returns>
        protected override int GetMasterDocumentID(string colName)
        {
            return 0;
        }

        /// <summary>
        /// Evento que obliga que se ejecute una funcion al cambiar de fila
        /// <param name="fila">Fila que se debe actualizar</param>
        /// <param name="oper">Indica si se debe ejecutar una segunda operacion</param>
        /// </summary>
        protected override void RowIndexChanged(int fila, bool oper)
        {

        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            InitializeComponent();
            this.documentID = AppDocuments.PlanillaDiariaTrabajo;

            base.SetInitParameters();

            this.agregarPlanillaDelegate = new AgregarPlanilla(this.AgregarPlanillaMethod);

            //Inicia las variables del formulario
            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;
            this.areaFuncionalID = _bc.AdministrationModel.User.AreaFuncionalID.Value;
            //Asigna las propiedades al documento
            this.frmModule = ModulesPrefix.no;
            this.LoadData(true);

            this.InitControls();
            this.AddGridCols();
            this.AfterInitialize();
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected override void AddGridCols()
        {
            try
            {
                #region Planilla diaria de trabajo

                GridColumn FechaPlanilla = new GridColumn();
                FechaPlanilla.FieldName = this.unboundPrefix + "FechaPlanilla";
                FechaPlanilla.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaPlanilla");
                FechaPlanilla.UnboundType = UnboundColumnType.String;
                FechaPlanilla.VisibleIndex = 0;
                FechaPlanilla.Width = 100;
                FechaPlanilla.Visible = true;
                FechaPlanilla.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(FechaPlanilla);

                GridColumn ConceptoNOPlanillaID = new GridColumn();
                ConceptoNOPlanillaID.FieldName = this.unboundPrefix + "ConceptoNOPlanillaID";
                ConceptoNOPlanillaID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ConceptoNOPlanillaID");
                ConceptoNOPlanillaID.UnboundType = UnboundColumnType.String;
                ConceptoNOPlanillaID.VisibleIndex = 0;
                ConceptoNOPlanillaID.Width = 100;
                ConceptoNOPlanillaID.Visible = true;
                ConceptoNOPlanillaID.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(ConceptoNOPlanillaID);

                GridColumn conceptoTipo = new GridColumn();
                conceptoTipo.FieldName = this.unboundPrefix + "TipoConceptoPlanilla";
                conceptoTipo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TipoConceptoPlanilla");
                conceptoTipo.UnboundType = UnboundColumnType.String;
                conceptoTipo.VisibleIndex = 0;
                conceptoTipo.Width = 100;
                conceptoTipo.Visible = true;
                conceptoTipo.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(conceptoTipo);

                GridColumn HorasNORDiu = new GridColumn();
                HorasNORDiu.FieldName = this.unboundPrefix + "HorasNORDiu";
                HorasNORDiu.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_HorasNORDiu");
                HorasNORDiu.UnboundType = UnboundColumnType.String;
                HorasNORDiu.VisibleIndex = 0;
                HorasNORDiu.Width = 100;
                HorasNORDiu.Visible = true;
                HorasNORDiu.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(HorasNORDiu);

                GridColumn HorasEXTDiu = new GridColumn();
                HorasEXTDiu.FieldName = this.unboundPrefix + "HorasEXTDiu";
                HorasEXTDiu.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_HorasEXTDiu");
                HorasEXTDiu.UnboundType = UnboundColumnType.String;
                HorasEXTDiu.VisibleIndex = 0;
                HorasEXTDiu.Width = 100;
                HorasEXTDiu.Visible = true;
                HorasEXTDiu.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(HorasEXTDiu);

                GridColumn HorasEXTNoc = new GridColumn();
                HorasEXTNoc.FieldName = this.unboundPrefix + "HorasEXTNoc";
                HorasEXTNoc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_HorasEXTNoc");
                HorasEXTNoc.UnboundType = UnboundColumnType.String;
                HorasEXTNoc.VisibleIndex = 0;
                HorasEXTNoc.Width = 100;
                HorasEXTNoc.Visible = true;
                HorasEXTNoc.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(HorasEXTNoc);

                GridColumn HorasRECNoc = new GridColumn();
                HorasRECNoc.FieldName = this.unboundPrefix + "HorasRECNoc";
                HorasRECNoc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_HorasRECNoc");
                HorasRECNoc.UnboundType = UnboundColumnType.String;
                HorasRECNoc.VisibleIndex = 0;
                HorasRECNoc.Width = 100;
                HorasRECNoc.Visible = true;
                HorasRECNoc.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(HorasRECNoc);

              

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlanillaTrabajo.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Funcion que se ejecuta despues de inicilizar el documento
        /// </summary>
        protected override void AfterInitialize()
        {
           
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

            FormProvider.Master.tbBreak.Visible = false;
            FormProvider.Master.itemFilter.Visible = false;
            FormProvider.Master.itemFilterDef.Visible = false;
            FormProvider.Master.tbBreak0.Visible = false;
            FormProvider.Master.itemGenerateTemplate.Visible = false;
            FormProvider.Master.itemCopy.Visible = false;
            FormProvider.Master.itemPaste.Visible = false;
            FormProvider.Master.itemExport.Visible = false;
            FormProvider.Master.tbBreak1.Visible = false;
            FormProvider.Master.itemImport.Visible = true;
            FormProvider.Master.itemRevert.Visible = false;

            if (FormProvider.Master.LoadFormTB)
            {
                FormProvider.Master.itemSave.Enabled = false;
                FormProvider.Master.itemPrint.Enabled = false;
                FormProvider.Master.itemImport.Enabled = true;
            }
        }

        #endregion

        #region Eventos Header

        private void txtContrato_Leave(object sender, EventArgs e)
        {
            this.gcDocument.DataSource = this.GetNovedades();
            FormProvider.Master.itemSave.Enabled = true;
        }
      
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
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32")
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
                            if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32")
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
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32")
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
                            if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32")
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
            if (fieldName == "ConceptoNOPlanillaID")
            {
                e.RepositoryItem = this.editBtnGrid;                
   
            }
            if (fieldName == "TipoConceptoPlanilla")
            {
                e.RepositoryItem = this.editCmb;

            }
        }


        private void editBtnGrid_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            string colName = this.gvDocument.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);
            ButtonEdit origin = (ButtonEdit)sender;
            this.ValoresDefecto();  
         }
        
       
        #endregion

        #region Eventos Control Emmpleados

        /// <summary>
        /// Evento que se ejecuta cuando se selecciona un empleado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uc_Empleados_SelectRowEmpleado_Click(object sender, EventArgs e)
        {
         
        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para recargar los controles del formulario
        /// </summary>
        public override void TBNew()
        {
            this.RefreshDocument();
        }

        /// <summary>
        /// Boton para actualizar la lista de documentos
        /// </summary>
        public override void TBSave()
        {
            Thread process = new Thread(this.SaveThread);
            process.Start();
        }

        /// <summary>
        /// Boton para importir datos
        /// </summary>
        public override void TBImport()
        {
            try
            {
                try
                {
                    
                }
                catch (Exception e)
                {
                    throw e;
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
        /// Hilo de salvar
        /// </summary>
        public override void SaveThread()
        {
            try
            {
                this.Invoke(this.agregarPlanillaDelegate); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlanillaTrabajo.cs", "SaveThread"));
            }
        }

        #endregion

      
    }
}
