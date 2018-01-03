using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Resultados;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using DevExpress.XtraGrid.Views.Grid;
using System.Threading;
using System.Globalization;
using NewAge.DTO.Attributes;
using System.Drawing;

using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using NewAge.DTO.Resultados;
using System.Threading;


namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de documentos
    /// </summary>
    public partial class ConsultaIndicadores : FormWithToolbar
    {
        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        //Variables Privadas
        private FormTypes _frmType = FormTypes.Document;
        private string _frmName;
        private int userID = 0;
        //Para manejo de propiedades
        private string empresaID = string.Empty;
        private int documentID;
        private ModulesPrefix frmModule;
        //Variables para importar
        private string unboundPrefix = "Unbound_";
        // Variables Formulario
        //Variables de datos
        private DTO_QueryIndicadores _rowCurrent = new DTO_QueryIndicadores();        
        private List<DTO_QueryIndicadores> _listProyectos = new List<DTO_QueryIndicadores>();
 
 
        private string semana = string.Empty;
        DateTime fecha = DateTime.Now;
        #endregion        

        ///<summary>
        /// Constructor 
        /// </summary>
        public ConsultaIndicadores()
        {
            try
            {
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());

                this.frmModule = ModulesPrefix.py;

                FormProvider.Master.Form_Load(this, this.frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaIndicadores.cs", "ConsultaIndicadores"));
            }
        }

        #region Funciones Privadas

        /// <summary>
        /// Add Columns in Grid
        /// </summary>
        private void AddGridCols()
        {
            #region Grilla Proyectos
            GridColumn ReporteID = new GridColumn();
            ReporteID.FieldName = this.unboundPrefix + "Grupo";
            ReporteID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ReporteID");
            ReporteID.UnboundType = UnboundColumnType.String;
            ReporteID.AppearanceCell.Options.UseTextOptions = true;
            ReporteID.AppearanceCell.Options.UseFont = true;
            ReporteID.VisibleIndex = 1;
            ReporteID.Width = 150;
            ReporteID.Visible = true;
            ReporteID.OptionsColumn.AllowEdit = false;
            this.gvProyectos.Columns.Add(ReporteID);


            GridColumn DescReporte = new GridColumn();
            DescReporte.FieldName = this.unboundPrefix + "DescReporte";
            DescReporte.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DescReporte");
            DescReporte.UnboundType = UnboundColumnType.Decimal;
            DescReporte.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            DescReporte.VisibleIndex = 2;
            DescReporte.AppearanceCell.Options.UseTextOptions = true;
            DescReporte.Width = 200;
            DescReporte.Visible = true;
            DescReporte.OptionsColumn.AllowEdit = false;
            this.gvProyectos.Columns.Add(DescReporte);


            GridColumn Formula = new GridColumn();
            Formula.FieldName = this.unboundPrefix + "Formula";
            Formula.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Formula");
            Formula.UnboundType = UnboundColumnType.Decimal;
            Formula.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            Formula.AppearanceCell.Options.UseTextOptions = true;
            Formula.VisibleIndex = 3;
            Formula.Width = 200;
            Formula.Visible = true;

            Formula.OptionsColumn.AllowEdit = false;
            this.gvProyectos.Columns.Add(Formula);

            GridColumn Indicador = new GridColumn();
            Indicador.FieldName = this.unboundPrefix + "Dato";
            Indicador.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Indicador");
            Indicador.UnboundType = UnboundColumnType.Decimal;
            Indicador.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Indicador.AppearanceCell.Options.UseTextOptions = true;
            Indicador.VisibleIndex = 4;
            Indicador.Width = 150;
            Indicador.Visible = true;
            Indicador.OptionsColumn.AllowEdit = false;
            //Indicador.ColumnEdit = this.editSpinPorcen;
            this.gvProyectos.Columns.Add(Indicador);

            GridColumn Unidad = new GridColumn();
            Unidad.FieldName = this.unboundPrefix + "cUnidad";
            Unidad.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Unidad");
            Unidad.UnboundType = UnboundColumnType.Decimal;
            Unidad.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            Unidad.AppearanceCell.Options.UseTextOptions = true;
            Unidad.VisibleIndex = 5;
            Unidad.Width = 100;
            Unidad.Visible = true;
            Unidad.OptionsColumn.AllowEdit = false;

            this.gvProyectos.Columns.Add(Unidad);

            //this.gvProyectos.OptionsView.ColumnAutoWidth = true;

            #endregion

        }

        /// <summary>
        /// Carga la información
        /// </summary>
        /// <param name="firstTime"></param>
        private void LoadData()
        {
            try
            {
                this.dtFechaCorte.Enabled = true;
                

                this._listProyectos = this._bc.AdministrationModel.plIndicadores(this.dtFechaCorte.DateTime);

                this.LoadGrids();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaIndicadores", "LoadData"));
            }
        }

        /// <summary>
        /// Carga la información del detalle
        /// </summary>
        /// <param name="firstTime"></param>
        private void LoadGrids()
        {
            try
            {
                this.gcProyectos.DataSource = null;
                this.gcProyectos.DataSource = this._listProyectos;
                this.gcProyectos.RefreshDataSource();

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionTiempo", "LoadData"));
            }
        }


        /// <summary>
        /// Refrescar Formulario
        /// </summary>
        private void RefreshForm()
        {
            this.gcProyectos.DataSource = null;

            this._rowCurrent = new DTO_QueryIndicadores();
            this._listProyectos = new List<DTO_QueryIndicadores>();

            this._listProyectos = this._bc.AdministrationModel.plIndicadores(this.dtFechaCorte.DateTime);
            this.LoadGrids();

        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            InitializeComponent();
            this.frmModule = ModulesPrefix.py;
            this.documentID = AppQueries.QueryIndicadores;
            this.AddGridCols();
            this.dtFechaCorte.DateTime = DateTime.Now;            
            

            this.LoadData();

            this.empresaID = _bc.AdministrationModel.Empresa.ID.Value;
            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;
        }

        /// <summary>
        /// Permite obtener el nombre del mes
        /// </summary>
        /// <param name="numeroMes"></param>
        /// <returns></returns>
        private string obtenerNombreMesNumero(int numeroMes)
        {
            try
            {
                DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
                string nombreMes = formatoFecha.GetMonthName(numeroMes);
                return nombreMes;
            }
            catch
            {
                return "Desconocido";
            }
        } 

        /// <summary>
        /// Valida una fila
        /// </summary>
        /// <param name="fila">Indice de la fila a validar</param>
        /// <returns>Retorna si la info de la fila es valida o no</returns>
        private bool ValidateRow(int fila)
        {
            return true;
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
                FormProvider.Master.itemNew.Visible = true;
                FormProvider.Master.itemDelete.Visible = false;
                FormProvider.Master.itemCopy.Visible = false;
                FormProvider.Master.itemPaste.Visible = false;
                FormProvider.Master.itemRevert.Visible = false;
                FormProvider.Master.itemFilter.Visible = false;
                FormProvider.Master.itemFilterDef.Visible = false;
                FormProvider.Master.itemGenerateTemplate.Visible = false;
                FormProvider.Master.itemImport.Visible = false;
                FormProvider.Master.itemSendtoAppr.Visible = false;
                FormProvider.Master.itemExport.Visible = false;
                FormProvider.Master.itemImport.Visible = false;
                FormProvider.Master.itemSave.Visible = false;
                FormProvider.Master.itemPrint.Visible = false;
                FormProvider.Master.itemUpdate.Visible = true;
                FormProvider.Master.itemUpdate.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaIndicadores", "Form_Enter"));
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
                ;
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
            }
            catch (Exception ex)
            {
                ;
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
                ;
            }
        }

        #endregion

        #region Eventos Header Superior

        /// <summary>
        /// Evento que se ejecuta al salir del numero de documento (glDocumentoControl - NumeroDoc)
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtNumeroDoc_Leave(object sender, EventArgs e) { }

        /// <summary>
        /// Evento que se ejecuta al pararse sobre el control de fecha
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void dtFecha_Enter(object sender, EventArgs e) { }
       
        /// <summary>
        /// Valida que solo ingrese numeros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNumPrefix_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        #endregion

        #region Eventos Header
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtFechaCorte_EditValueChanged(object sender, EventArgs e)
        {
            if (this.gvProyectos.DataRowCount > 0)
            {

            }
        }

        #endregion

        #region Eventos Grilla

        #region Proyectos


        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        
        private void gvDocument_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName =!e.Column.FieldName.Equals("Editar")? e.Column.FieldName.Substring(this.unboundPrefix.Length) : e.Column.FieldName;

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
        /// Cambia estylo del campo dependiendo del valor
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
           // string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            //if (fieldName == "DiasAtraso" && e.RowHandle >= 0)
            //{

            //    decimal cellvalue = Convert.ToDecimal(e.CellValue, CultureInfo.InvariantCulture);
            //    if (cellvalue > 0)
            //        e.Appearance.ForeColor = Color.Red;
            //    else
            //        e.Appearance.ForeColor = Color.Black;
            //}
        }

        /// <summary>
        /// Cambia estylo del campo dependiendo del valor
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_RowStyle(object sender, RowStyleEventArgs e)
        {
            try
            {
                //DTO_pyProyectoTarea currentRow = (DTO_pyProyectoTarea)this.gvProyectos.GetRow(e.RowHandle);
                //if (currentRow != null)
                //{
                //    if (currentRow.DetalleInd.Value.Value)
                //        e.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                //    else
                //        e.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
        


        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Nuevo
        /// </summary>
        public override void TBNew()
        {
            this.RefreshForm();
        }

        /// <summary>
        /// Boton para actualizar la lista de documentos
        /// </summary>
        public override void TBUpdate()
        {
            try
            {
                this.LoadData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        private void btnReporte_Click(object sender, EventArgs e)
        {

            try
            {
                //this.gcProyectos.DataSource = null;

                //this._rowCurrent = new DTO_QueryIndicadores();
                //this._listProyectos = new List<DTO_QueryIndicadores>();

                //this._listProyectos = this._bc.AdministrationModel.plIndicadores(this.dtFechaCorte.DateTime);
                //this.LoadGrids();

                string fileURl;
                string reportName = this._bc.AdministrationModel.Reportes_py_Indicadores(this.dtFechaCorte.DateTime);

                fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                //Process.Start(fileURl);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryLibranzas.cs", "gvGenerales_FocusedRowChanged"));
            }
        

        }


    }
}
