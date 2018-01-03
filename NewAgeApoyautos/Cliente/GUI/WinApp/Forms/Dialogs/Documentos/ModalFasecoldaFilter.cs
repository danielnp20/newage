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
    public partial class ModalFasecoldaFilter : Form
    {
        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        //Variables basicas
        private int _documentID;
        private string _unboundPrefix = "Unbound_";
        private int _pageSize = 50;
        private bool _filterActive = false;
        //Variables de data
        private DTO_ccFasecolda _rowCurrent = new DTO_ccFasecolda();
        private List<DTO_ccFasecolda> _listCurrent = null;

        #endregion

        #region Propiedades

        /// <summary>
        /// Documentos Control Seleccionados
        /// </summary>
        public string IDSelected
        {
            get { return this._rowCurrent.ID.Value; }
        }

        #endregion

       /// <summary>
       /// Constructor
       /// </summary>
       /// <param name="bodegaIni">Bodega para consultar las existencias</param>
        public ModalFasecoldaFilter(int document)
        {
            this.InitializeComponent();
            try
            {
                this._documentID = document;
                this.SetInitParameters();
                _bc.Pagging_Init(this.pgGrid, _pageSize);
                _bc.Pagging_SetEvent(this.pgGrid, this.pagging_Click);
                this.AddGridCols();              
                this.LoadGridData();
                FormProvider.LoadResources(this, this._documentID);
                this.Text = this._bc.GetResource(LanguageTypes.Forms, "FaseColda");
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinAppModalFasecoldaFilter.cs", "ModalFasecoldaFilter: " + ex.Message));
            }
        }

        #region Funciones privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            #region Inicializa Controles
            #endregion            
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddGridCols()
        {
            GridColumn ID = new GridColumn();
            ID.FieldName = this._unboundPrefix + "ID";
            ID.Caption = _bc.GetResource(LanguageTypes.Forms, "Fasecolda");
            ID.UnboundType = UnboundColumnType.String;
            ID.VisibleIndex = 0;
            ID.Width = 70;
            ID.Visible = true;
            ID.OptionsColumn.AllowEdit = false;
            this.gvData.Columns.Add(ID);

            GridColumn Descriptivo = new GridColumn();
            Descriptivo.FieldName = this._unboundPrefix + "Descriptivo";
            Descriptivo.Caption = _bc.GetResource(LanguageTypes.Forms, "Descriptivo");
            Descriptivo.UnboundType = UnboundColumnType.String;
            Descriptivo.VisibleIndex = 1;
            Descriptivo.Width = 200;
            Descriptivo.Visible = true;
            Descriptivo.OptionsColumn.AllowEdit = false;
            this.gvData.Columns.Add(Descriptivo);           
        }

        /// <summary>
        /// Carga los datos de la grilla
        /// </summary>
        private void LoadGridData()
        {
            try
            {
                long count = 0;
                DTO_glConsulta consulta = new DTO_glConsulta();
              
                List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();  
                if (!string.IsNullOrEmpty(this.txtMarca.Text))
                {
                    filtros.Add(new DTO_glConsultaFiltro()
                    {
                        CampoFisico = "Marca",
                        OperadorFiltro = OperadorFiltro.Contiene,
                        ValorFiltro = this.txtMarca.Text,
                        OperadorSentencia = "AND"
                    });
                }
                if (!string.IsNullOrEmpty(this.txtMarca.Text))
                {
                    filtros.Add(new DTO_glConsultaFiltro()
                    {
                        CampoFisico = "Clase",
                        OperadorFiltro = OperadorFiltro.Contiene,
                        ValorFiltro = this.txtClase.Text,
                        OperadorSentencia = "AND"
                    });
                }
                if (!string.IsNullOrEmpty(this.txtCodigo.Text))
                {
                    filtros.Add(new DTO_glConsultaFiltro()
                    {
                        CampoFisico = "FasecoldaID",
                        OperadorFiltro = OperadorFiltro.Contiene,
                        ValorFiltro = this.txtCodigo.Text,
                        OperadorSentencia = "AND"
                    });
                }
                if (!string.IsNullOrEmpty(this.txtDesc.Text))
                {
                    filtros.Add(new DTO_glConsultaFiltro()
                    {
                        CampoFisico = "Descriptivo",
                        OperadorFiltro = OperadorFiltro.Contiene,
                        ValorFiltro = this.txtDesc.Text,
                        OperadorSentencia = "AND"
                    });
                }
                consulta.Filtros = filtros; 
                count = this._bc.AdministrationModel.MasterSimple_Count(AppMasters.ccFasecolda, consulta, null, true);
                List<DTO_ccFasecolda> _dtoList = this._bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.ccFasecolda, count, 1, consulta, null, null).Cast<DTO_ccFasecolda>().ToList();
                
                this._listCurrent = _dtoList;
                this.pgGrid.UpdatePageNumber(this._listCurrent.Count, true, true, false);
                var tmp = this._listCurrent.Skip((this.pgGrid.PageNumber - 1) * this._pageSize).Take(this._pageSize).ToList<DTO_ccFasecolda>();
                this.gvData.MoveFirst();
                this.gcData.DataSource = tmp;
                this.gcData.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalFasecoldaFilter.cs", "ModalFasecoldaFilter(" + this._documentID + ")-LoadGridData: " + ex.Message));
            }
        }
    
        #endregion

        #region Eventos Controles

        /// <summary>
        /// Evento que se ejecuta al darle click al paginador
        /// </summary>
        /// <param name="sender">Objeto que inicia el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void pagging_Click(object sender, System.EventArgs e)
        {
            try
            {              
                var tmp = this._filterActive ?  this._listCurrent.Skip((this.pgGrid.PageNumber - 1) * this._pageSize).Take(this._pageSize).ToList<DTO_ccFasecolda>() :
                                                this._listCurrent.Skip((this.pgGrid.PageNumber - 1) * this._pageSize).Take(this._pageSize).ToList<DTO_ccFasecolda>(); ;
                this.pgGrid.UpdatePageNumber(this._listCurrent.Count, false, false, false);
                this.gvData.MoveFirst();
                this.gcData.DataSource = tmp;

                if (this.gvData.DataRowCount > 0)
                    this._rowCurrent = (DTO_ccFasecolda)this.gvData.GetRow(this.gvData.FocusedRowHandle);
                else
                    this._rowCurrent = new DTO_ccFasecolda();

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalFasecoldaFilter.cs", "pagging_Click: " + ex.Message)); 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">Objeto que inicia el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void btnFilter_Click(object sender, EventArgs e)
        {
            this.LoadGridData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">Objeto que inicia el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.btnFilter_Click(this.btnFilter, e);
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">Objeto que inicia el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void btnAccept_Click(object sender, EventArgs e)
        {
            if(this.gvData.DataRowCount > 0)
                this._rowCurrent = (DTO_ccFasecolda)this.gvData.GetRow(this.gvData.FocusedRowHandle);
            this.Close();
        }

        /// <summary>
        /// Al hacer click para cancelar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this._rowCurrent = new DTO_ccFasecolda();
            this.Close();
        }

        #endregion        

        #region Eventos Grillas

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvTarea_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

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
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvTarea_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle >= 0)
                    this._rowCurrent = (DTO_ccFasecolda)this.gvData.GetRow(e.FocusedRowHandle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalFasecoldaFilter.cs", "gvTarea_FocusedRowChanged: " + ex.Message));
            }
        }

        /// <summary>
        /// Cuando selecciona un item de la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvTarea_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            try
            {
                if (e.RowHandle >= 0)
                    this._rowCurrent = (DTO_ccFasecolda)this.gvData.GetRow(e.RowHandle);               
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalFasecoldaFilter.cs", "gvTarea_RowClick: " + ex.Message));
            }
        }

        /// <summary>
        /// Abre la modal para detalle
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvTarea_DoubleClick(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion          
    }
}
