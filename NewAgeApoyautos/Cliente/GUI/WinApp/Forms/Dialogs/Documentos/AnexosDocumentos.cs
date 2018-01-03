using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using NewAge.DTO.UDT;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Collections;
using SentenceTransformer;
using System.IO;
using System.Diagnostics;
using NewAge.DTO.Resultados;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de anexos para documentos
    /// </summary>
    public partial class AnexosDocumentos : Form
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();
        private int documentID = 0;
        private int numeroDoc = 0;
        private ModulesPrefix modulo;
        private bool canEdit = false;

        protected string unboundPrefix = "Unbound_";
        private List<DTO_glDocAnexoControl> data;
        private DTO_glDocAnexoControl anexo;

        #endregion

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="docID">Identificador del documento (AppDocuments)</param>
        /// <param name="numDoc">Identificador del documento control</param>
        public AnexosDocumentos(int numDoc, ModulesPrefix mod)
        {
            try
            {
                //Variables
                this.documentID = AppForms.ModalAnexosDocumentos;
                this.numeroDoc = numDoc;
                this.modulo = mod;
                
                //Inicializa el formulario
                InitializeComponent();

                FormProvider.LoadResources(this, this.documentID);
                this.LoadData(true);

                if (!this.canEdit)
                    this.btnSave.Enabled = false;

            }
            catch(Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AnexosDocumentos.cs", "AnexosDocumentos"));
            }
        }

        #region Funciones privadas

        /// <summary>
        /// Asigna la lista de columnas a la grilla
        /// </summary>
        private void AddGridCols()
        {
            if (this.canEdit)
            {
                //Actualizar
                GridColumn act = new GridColumn();
                act.FieldName = this.unboundPrefix + "Actualizar";
                act.Caption = "√";
                act.UnboundType = UnboundColumnType.Boolean;
                act.VisibleIndex = 0;
                act.Width = 25;
                act.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Actualizar");
                act.AppearanceHeader.ForeColor = Color.Lime;
                act.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                act.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                act.AppearanceHeader.Options.UseTextOptions = true;
                act.AppearanceHeader.Options.UseFont = true;
                act.AppearanceHeader.Options.UseForeColor = true;
                this.gvAnexos.Columns.Add(act);

                //Eliminar
                GridColumn eliminar = new GridColumn();
                eliminar.FieldName = this.unboundPrefix + "Eliminar";
                eliminar.Caption = "X";
                eliminar.UnboundType = UnboundColumnType.Boolean;
                eliminar.VisibleIndex = 1;
                eliminar.Width = 25;
                eliminar.Visible = true;
                eliminar.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Eliminar");
                eliminar.AppearanceHeader.ForeColor = Color.Red;
                eliminar.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                eliminar.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                eliminar.AppearanceHeader.Options.UseTextOptions = true;
                eliminar.AppearanceHeader.Options.UseFont = true;
                eliminar.AppearanceHeader.Options.UseForeColor = true;
                this.gvAnexos.Columns.Add(eliminar);
                
                //Nuevo
                GridColumn nuevo = new GridColumn();
                nuevo.FieldName = this.unboundPrefix + "Nuevo";
                nuevo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Nuevo");
                nuevo.UnboundType = UnboundColumnType.Boolean;
                nuevo.VisibleIndex = 2;
                nuevo.Width = 30;
                nuevo.Visible = true;
                nuevo.OptionsColumn.AllowEdit = false;
                nuevo.OptionsColumn.ReadOnly = true;
                this.gvAnexos.Columns.Add(nuevo);

            }

            //Descriptivo
            GridColumn descriptivo = new GridColumn();
            descriptivo.FieldName = this.unboundPrefix + "Descriptivo";
            descriptivo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descriptivo");
            descriptivo.UnboundType = UnboundColumnType.String;
            descriptivo.VisibleIndex = 3;
            descriptivo.Width = 210;
            descriptivo.OptionsColumn.AllowEdit = false;
            this.gvAnexos.Columns.Add(descriptivo);

            //Ver
            GridColumn ver = new GridColumn();
            ver.FieldName = this.unboundPrefix + "Ver";
            ver.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Ver");
            ver.UnboundType = UnboundColumnType.String;
            ver.VisibleIndex = 4;
            ver.Width = 75;
            ver.ColumnEdit = this.linkVer;
            this.gvAnexos.Columns.Add(ver);

            if (this.canEdit)
            {
                //Actualizar
                GridColumn mod = new GridColumn();
                mod.FieldName = this.unboundPrefix + "Modificar";
                mod.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Modificar");
                mod.UnboundType = UnboundColumnType.String;
                mod.VisibleIndex = 5;
                mod.Width = 100;
                mod.ColumnEdit = this.linkActualizar;
                this.gvAnexos.Columns.Add(mod);
            }

            this.gvAnexos.OptionsView.ColumnAutoWidth = true;
        }

        /// <summary>
        /// Asigna la información a la grilla
        /// </summary>
        private void LoadData(bool firstTime)
        {
            try
            {
                this.data = _bc.AdministrationModel.glDocAnexoControl_GetAnexosByNumeroDoc(this.numeroDoc);

                if(this.data.Count > 0)
                {
                    int docID = this.data.First().DocumentoID.Value.Value;
                    this.canEdit = SecurityManager.HasAccess(docID, FormsActions.Edit);
                }

                if(firstTime)
                    this.AddGridCols();

                this.gcAnexos.DataSource = this.data;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AnexosDocumentos.cs", "LoadData"));
            }
        }

        #endregion

        #region Eventos Formulario

        /// <summary>
        /// Evento que salva los datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.data != null && this.data.Count > 0)
                {
                    int count = this.data.Where(x => x.Actualizar.Value.Value).Count();
                    if (count > 0)
                    {
                        bool update = true;

                        count = this.data.Where(x => x.Eliminar.Value.Value).Count();
                        if (count > 0)
                        {
                            string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
                            string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Data);

                            //Revisa si desea eliminar el registro
                            if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.No)
                                update = false;
                        }


                        if (update)
                        {
                            DTO_TxResult result = _bc.AdministrationModel.glDocAnexoControl_Update(this.modulo, this.data);
                            MessageForm frm = new MessageForm(result);
                            frm.ShowDialog();

                            this.LoadData(false);

                            if (this.gvAnexos.FocusedRowHandle >= 0 && this.gvAnexos.FocusedRowHandle < this.data.Count)
                            {
                                this.anexo = this.data[this.gvAnexos.FocusedRowHandle];
                                bool allowEdit = string.IsNullOrWhiteSpace(this.anexo.ArchivoNombre.Value) ? false : true;

                                //Solo deja editar si tiene archivo
                                this.gvAnexos.Columns[this.unboundPrefix + "Ver"].OptionsColumn.AllowEdit = allowEdit;
                                if (this.canEdit)
                                    this.gvAnexos.Columns[this.unboundPrefix + "Eliminar"].OptionsColumn.AllowEdit = allowEdit;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AnexosDocumentos.cs", "btnSave_Click"));
            }
        }

        #endregion

        #region Eventos Grilla

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvAnexos_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

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
                        UDT udtProp = (UDT)fi.GetValue(dto);
                        udtProp.SetValueFromString(e.Value.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// Asigna texto por defecto para la columna de archivos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void gvAnexos_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            
            if (fieldName == "Ver")
            {
                e.DisplayText = string.IsNullOrWhiteSpace(this.data[e.ListSourceRowIndex].ArchivoNombre.Value) ? string.Empty : _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Ver");
            }
            if (fieldName == "Modificar")
            {
                e.DisplayText = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Modificar");
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>  
        protected virtual void gvAnexos_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
                if (fieldName == "Eliminar" && this.data[e.RowHandle].Eliminar.Value.Value)
                {
                    this.data[e.RowHandle].Actualizar.Value = true;
                    this.gcAnexos.RefreshDataSource();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AnexosDocumentos.cs", "gvAnexos_CellValueChanging"));
            }
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvAnexos_BeforeLeaveRow(object sender, RowAllowEventArgs e)
        {
            try
            {
                if (e.RowHandle < this.data.Count)
                {
                    #region Valida si es nuevo
                    if (this.data[e.RowHandle].Nuevo.Value.Value && !this.data[e.RowHandle].Actualizar.Value.Value)
                    {
                        this.data[e.RowHandle].Archivo = null;
                        this.data[e.RowHandle].Nuevo.Value = false;

                        this.gcAnexos.RefreshDataSource();
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AnexosDocumentos.cs", "gvAnexos_BeforeLeaveRow"));
            } 
        }

        /// <summary>
        /// Se ejecuta cuando se cambia la fila de los documentos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvAnexos_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle >= 0 && e.FocusedRowHandle < this.data.Count)
                {
                    this.anexo = this.data[e.FocusedRowHandle];
                    bool allowEdit = string.IsNullOrWhiteSpace(this.anexo.ArchivoNombre.Value) ? false : true;

                    //Solo deja editar si tiene archivo
                    this.gvAnexos.Columns[this.unboundPrefix + "Ver"].OptionsColumn.AllowEdit = allowEdit;
                    if (this.canEdit)
                        this.gvAnexos.Columns[this.unboundPrefix + "Eliminar"].OptionsColumn.AllowEdit = allowEdit;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AnexosDocumentos.cs", "gvAnexos_FocusedRowChanged"));
            }
        }

        #endregion

        #region Eventos controles de grilla internos

        /// <summary>
        /// Evento que se ejecuta para ver un anexo existente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkVer_Click(object sender, EventArgs e)
        {
            try
            {
                DTO_glDocAnexoControl anexo = this.data[this.gvAnexos.FocusedRowHandle];

                string fileFormat = _bc.GetControlValue(AppControl.NombreAnexoDocumento);
                string fileName = modulo.ToString() + "&" + anexo.ArchivoNombre.Value;

                string url = _bc.UrlDocumentFile(TipoArchivo.AnexosDocumentos, null, null, fileName);

                ProcessStartInfo sInfo = new ProcessStartInfo(url);
                Process.Start(sInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AnexosDocumentos.cs", "gvAnexos_FocusedRowChanged"));
            }

        }

        /// <summary>
        /// Evento que se ejecuta para ver seleccionar un nuevo anexo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog fDialog = new OpenFileDialog();
                //fDialog.Filter = "Word|*.xml|Excel|*.uml|Img1|*.uml|Img2|*.uml";

                if (fDialog.ShowDialog() == DialogResult.OK)
                {
                    string path = Path.GetDirectoryName(fDialog.FileName);
                    string filename = Path.GetFileNameWithoutExtension(fDialog.FileName);
                    string ext = Path.GetExtension(fDialog.FileName);

                    string filePath = path + "\\" + filename + ext;
                    byte[] arr = System.IO.File.ReadAllBytes(filePath);

                    int fila = this.gvAnexos.FocusedRowHandle;
                    this.data[fila].Archivo = arr;
                    this.data[fila].Extension = ext;

                    this.data[fila].Actualizar.Value = true;
                    if (string.IsNullOrWhiteSpace(this.data[fila].ArchivoNombre.Value))
                        this.data[fila].Nuevo.Value = true;
                   
                    this.gcAnexos.RefreshDataSource();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AnexosDocumentos.cs", "gvAnexos_FocusedRowChanged"));
            }
        }

        #endregion

    }
}
