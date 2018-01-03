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
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using NewAge.Librerias.Project;
using System.Reflection;
using NewAge.DTO.UDT;
using DevExpress.XtraEditors;

namespace NewAge.Forms
{
    public partial class FindActivosModal : Form
    {
        public FindActivosModal(bool isContenedor = false, bool isMulSelection = false)
        {
            InitializeComponent();
            if (isContenedor)
            {
                this.chkContenedor.Checked = true;
                this.chkContenedor.Properties.ReadOnly = true;
            }
            if (isMulSelection)
                this.MultipleSelection = isMulSelection;
            this.pageSize = Convert.ToInt32(this._bc.GetControlValue(AppControl.PaginadorMaestra));
            this._bc.Pagging_Init(this.uc_Paginador, this.pageSize);
            this._bc.Pagging_SetEvent(this.uc_Paginador, this.pagging_Click);
            this.SetInitParameters();
        }

        #region Variables

        BaseController _bc = BaseController.GetInstance();
        List<DTO_acActivoControl> lActivos = null;
        List<DTO_acActivoControl> _lActivosSel = null;
        private DTO_acActivoControl _activo = null;
        private string unboundPrefix = "Unbound_";
        private int documentID;
        private int pageSize;  
        private int pageNum;

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Inicializa las controles
        /// </summary>
        private void InitControls()
        {
            this.documentID = AppQueries.QueryActivos;
            this.chkSelectAll.Visible = false;
            this._bc.InitMasterUC(this.uc_LocFisica, AppMasters.glLocFisica, true, true, true, false);
            this._bc.InitMasterUC(this.uc_Referencia, AppMasters.inReferencia, true, true, true, false);
            this._bc.InitMasterUC(this.uc_CentroCosto, AppMasters.coCentroCosto, true, true, true, false);
            this._bc.InitMasterUC(this.uc_Proyecto, AppMasters.coProyecto, true, true, true, false);
            this._bc.InitMasterUC(this.uc_Tipo, AppMasters.acTipo, true, true, true, false);
            this._bc.InitMasterUC(this.uc_Grupo, AppMasters.acGrupo, true, true, true, false);
            this._bc.InitMasterUC(this.uc_Clase, AppMasters.acClase, true, true, true, false);
            this._bc.InitMasterUC(this.uc_Responsable, AppMasters.coTercero, false, true, true, false);
            this.editChek.CheckedChanged += new EventHandler(editChek_CheckedChanged);
            this.AddGridCols();
        }       

        /// <summary>
        /// Limpia los campos y objetos del documento
        /// </summary>
        private void RefreshDocument()
        {           
            this.InitControls();
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>             
        private void SetInitParameters()
        {
            this._lActivosSel = new List<DTO_acActivoControl>();
            this.InitControls();
            FormProvider.LoadResources(this, this.documentID);
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                if (this.MultipleSelection)
                {
                    GridColumn marca = new GridColumn();
                    marca.FieldName = this.unboundPrefix + "Marca";
                    marca.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Marca");
                    marca.UnboundType = UnboundColumnType.Boolean;
                    marca.VisibleIndex = 0;
                    marca.Width = 40;
                    marca.Visible = true;
                    marca.OptionsColumn.AllowEdit = true;
                    this.gvContent.Columns.Add(marca);

                    this.chkSelectAll.Visible = true;
                    this.btnCargar.Visible = true;
                }

                GridColumn activoID = new GridColumn();
                activoID.FieldName = this.unboundPrefix + "ActivoID";
                activoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ActivoID");
                activoID.UnboundType = UnboundColumnType.Integer;
                activoID.VisibleIndex = 0;
                activoID.Width = 50;
                activoID.Visible = true;
                activoID.OptionsColumn.AllowEdit = false;
                this.gvContent.Columns.Add(activoID);

                GridColumn serialID = new GridColumn();
                serialID.FieldName = this.unboundPrefix + "SerialID";
                serialID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_SerialID");
                serialID.UnboundType = UnboundColumnType.String;
                serialID.VisibleIndex = 0;
                serialID.Width = 100;
                serialID.Visible = true;
                serialID.OptionsColumn.AllowEdit = false;
                this.gvContent.Columns.Add(serialID);

                GridColumn plaquetaID = new GridColumn();
                plaquetaID.FieldName = this.unboundPrefix + "PlaquetaID";
                plaquetaID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PlaquetaID");
                plaquetaID.UnboundType = UnboundColumnType.String;
                plaquetaID.VisibleIndex = 0;
                plaquetaID.Width = 100;
                plaquetaID.Visible = true;
                plaquetaID.OptionsColumn.AllowEdit = false;
                this.gvContent.Columns.Add(plaquetaID);

                GridColumn locFisicaID = new GridColumn();
                locFisicaID.FieldName = this.unboundPrefix + "LocFisicaID";
                locFisicaID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_LocFisicaID");
                locFisicaID.UnboundType = UnboundColumnType.String;
                locFisicaID.VisibleIndex = 0;
                locFisicaID.Width = 100;
                locFisicaID.Visible = true;
                locFisicaID.OptionsColumn.AllowEdit = false;
                this.gvContent.Columns.Add(locFisicaID);

                GridColumn ActivoClaseID = new GridColumn();
                ActivoClaseID.FieldName = this.unboundPrefix + "ActivoClaseID";
                ActivoClaseID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ActivoClaseID");
                ActivoClaseID.UnboundType = UnboundColumnType.String;
                ActivoClaseID.VisibleIndex = 0;
                ActivoClaseID.Width = 100;
                ActivoClaseID.Visible = true;
                ActivoClaseID.OptionsColumn.AllowEdit = false;
                this.gvContent.Columns.Add(ActivoClaseID);

                GridColumn ActivoGrupoID = new GridColumn();
                ActivoGrupoID.FieldName = this.unboundPrefix + "ActivoGrupoID";
                ActivoGrupoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ActivoGrupoID");
                ActivoGrupoID.UnboundType = UnboundColumnType.String;
                ActivoGrupoID.VisibleIndex = 0;
                ActivoGrupoID.Width = 100;
                ActivoGrupoID.Visible = true;
                ActivoGrupoID.OptionsColumn.AllowEdit = false;
                this.gvContent.Columns.Add(ActivoGrupoID);


                GridColumn ActivoTipoID = new GridColumn();
                ActivoTipoID.FieldName = this.unboundPrefix + "ActivoTipoID";
                ActivoTipoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ActivoTipoID");
                ActivoTipoID.UnboundType = UnboundColumnType.String;
                ActivoTipoID.VisibleIndex = 0;
                ActivoTipoID.Width = 100;
                ActivoTipoID.Visible = true;
                ActivoTipoID.OptionsColumn.AllowEdit = false;
                this.gvContent.Columns.Add(ActivoTipoID);

                GridColumn inReferenciaID = new GridColumn();
                inReferenciaID.FieldName = this.unboundPrefix + "Observacion";
                inReferenciaID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Observacion");
                inReferenciaID.UnboundType = UnboundColumnType.String;
                inReferenciaID.VisibleIndex = 0;
                inReferenciaID.Width = 250;
                inReferenciaID.Visible = true;
                inReferenciaID.OptionsColumn.AllowEdit = false;
                this.gvContent.Columns.Add(inReferenciaID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-FindActivosModal", "AddGridCols"));
            }
        }

        /// <summary>
        /// Funcion que se ejecuta despues de inicilizar el documento
        /// </summary>
        private void AfterInitialize()
        {

        }

        /// <summary>
        /// Carga la informacion en la grilla
        /// </summary>
        private void LoadData()
        { 
            this.lActivos = this._bc.AdministrationModel.acActivoControl_GetFilters(this.txtSerial.Text, 
                                                                                    this.txtPlaqueta.Text,                                                                                    this.uc_LocFisica.Value, 
                                                                                    this.uc_Referencia.Value,
                                                                                    this.uc_CentroCosto.Value,
                                                                                    this.uc_Proyecto.Value,  
                                                                                    this.uc_Clase.Value,
                                                                                    this.uc_Tipo.Value, 
                                                                                    this.uc_Grupo.Value,
                                                                                    this.uc_Responsable.Value,
                                                                                    this.chkContenedor.Checked,
                                                                                    this.uc_Paginador.PageSize,
                                                                                    1
                                                                                    ).Where(x => x.EstadoInv.Value == (byte)EstadoInv.Activo).ToList();
            if (this.lActivos != null && this.lActivos.Count > 0)
            {
                this.gcContent.DataSource = this.lActivos;
                this._activo = this.lActivos[0];
            }      
            
        }

        #endregion    
               
        #region Eventos

        /// <summary>
        /// Busca activo de acuerdo a los filtros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            this.LoadData();
        }

        /// <summary>
        /// Carga los activos cuando el control es de multiple seleccion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCargar_Click(object sender, EventArgs e)
        {
            if (this._lActivosSel != null && this._lActivosSel.Count > 0)
                this.Close();
            else
                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ac_NoActivoSelect));
        }

        /// <summary>
        /// Selecciona todos los items de la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckEdit)sender).Checked)
            {
                this.editChek.ValueChecked = true;                
            }
            else
            {
                this.editChek.ValueChecked = false;
            }

            this.gvContent.RefreshData();
            this.gcContent.RefreshDataSource();
        }


        #endregion       

        #region Eventos Grilla

        /// <summary>
        /// Carga la Data en la Grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvContent_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
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
            if (e.IsSetData)
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

        /// <summary>
        /// Cierra el modal y retorna el activo seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvContent_DoubleClick(object sender, EventArgs e)
        {
            if(!this.MultipleSelection)
                this.Close();
        }

        /// <summary>
        /// Selecciona el activo 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvContent_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (this.lActivos != null && this.lActivos.Count > 0)
            {
                this._activo = this.lActivos[e.FocusedRowHandle];
            }
        }

        /// <summary>
        /// Aplica formato a las celdas 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvContent_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (fieldName == "Marca")
            {
                e.RepositoryItem = this.editChek;
            }
        }

        /// <summary>
        /// Se ejecuta al seleccionar registro de la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editChek_CheckedChanged(object sender, EventArgs e)
        {
            int index = this.gvContent.FocusedRowHandle;
            if (((CheckEdit)sender).Checked)
            {
                this._lActivosSel.Add(this.lActivos[index]);
            }
            else
            { 
                this._lActivosSel.Remove(this.lActivos[index]);
            }
        }
        
        #endregion

        #region Eventos Paginador

           
        /// <summary>
        /// Evento que se ejecuta al darle click al paginador
        /// </summary>
        /// <param name="sender">Objeto que inicia el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void pagging_Click(object sender, System.EventArgs e)
        {
            this.uc_Paginador.UpdatePageNumber(this.lActivos.Count , false, false, false);      
        }
        
        #endregion

        #region Propiedades

        /// <summary>
        /// Activo Seleccionado
        /// </summary>
        public DTO_acActivoControl Activo
        {
            get { return this._activo; }
        }

        /// <summary>
        /// Activo Seleccionado
        /// </summary>
        public List<DTO_acActivoControl> ActivosSel
        {
            get { return this._lActivosSel; }
        }

        /// <summary>
        /// Activo Seleccionado
        /// </summary>
        public Boolean MultipleSelection
        {
            get;
            set;
        }

        #endregion
                  
    }
}
