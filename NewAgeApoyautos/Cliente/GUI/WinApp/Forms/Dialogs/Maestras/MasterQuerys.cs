using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Librerias.Project;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Resultados;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using DevExpress.Data;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using NewAge.Librerias.ExceptionHandler;
using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Transactions;
using SentenceTransformer;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario Maestra de Consultas
    /// </summary>
    public partial class MasterQuery : Form
    {
        #region Variables

        private Dictionary<string, object> _fks = new Dictionary<string, object>();

        private BaseController _bc = BaseController.GetInstance();
        private int _frmCode;
        private string IdSelected;

        //Indicador para ver si se esta insertando un registro
        private bool _insertando;

        //Títulos de la grilla de datos
        private string _col_glConsultaFiltroGrupoID = string.Empty;
        private string _col_Nombre = string.Empty;
        private string _col_FormaID = string.Empty;
        private string _col_seUsuarioID = string.Empty;
        private string _col_Seleccion = string.Empty;
        private string _col_Filtro = string.Empty;
        private string _col_Distincion = string.Empty;
        private string _col_Activo = string.Empty;
        private string _col_CtrlVersion = string.Empty;
        private string _col_FechaCreacion = string.Empty;
        private string _col_UsuarioCreacion = string.Empty;
        private string _col_FechaAct = string.Empty;
        private string _col_UsuarioAct = string.Empty;
        private string _col_Prefijada = string.Empty;
        //Titulos de las columnas de otras grillas
        private string _col_field = string.Empty;
        private string _col_met = string.Empty;
        private string _col_type = string.Empty;
        private string _col_apselec = string.Empty;
        private string _col_ord = string.Empty;
        private string _col_ordix = string.Empty;
        private string _col_oper = string.Empty;
        private string _col_val = string.Empty;
        private string _col_opersent = string.Empty;
        //Textos para los tipos de datos
        private string _col_string = string.Empty;
        private string _col_int16 = string.Empty;
        private string _col_int32 = string.Empty;
        private string _col_int64 = string.Empty;
        private string _col_decimal = string.Empty;
        private string _col_double = string.Empty;
        private string _col_bool = string.Empty;
        private string _col_datetime = string.Empty;

        private bool _allowColumnSelect = true;
        #endregion

        #region Propiedades

        /// <summary>
        /// Encapsula el estado de insertando en el formulario 
        /// </summary>
        protected virtual bool Insertando
        {
            get
            {
                return this._insertando;
            }
            set
            {
                this._insertando = value;
                this.tpRecordEdit.Text = value ? this._bc.GetResource(LanguageTypes.Forms, "1003_tpRecordInsert") : this._bc.GetResource(LanguageTypes.Forms, "1003_tpRecordEdit");
            }
        }

        /// <summary>
        /// Forma Id
        /// </summary>
        private int DocumentoID
        {
            get;
            set;
        }

        /// <summary>
        /// User ID
        /// </summary>
        private int UserID
        {
            get;
            set;
        }

        /// <summary>
        /// Fields de la maestra actual
        /// </summary>
        private List<ConsultasFields> FieldsToFilter
        {
            get;
            set;
        }

        /// <summary>
        /// Variantes
        /// </summary>
        private List<DTO_glConsulta> Variantes
        {
            get;
            set;
        }

        ///// <summary>
        ///// VariantesID, DTO_glConsultaSelecciones
        ///// </summary>
        //public Dictionary<int, List<DTO_glConsultaSeleccion>> VarSelecciones
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// VariantesID, DTO_glConsultaFiltro
        ///// </summary>
        //public Dictionary<int, List<DTO_glConsultaFiltro>> VarFiltros
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// Id de los filtros a actualizar
        ///// </summary>
        //public Dictionary<int, List<int>> VarFiltrosToUpdate
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// Id de los filtros a insertar
        ///// </summary>
        //public Dictionary<int, List<int>> VarFiltrosToInsert
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// Id de los filtros a borrar
        ///// </summary>
        //public Dictionary<int, List<int>> VarFiltrosToDelete
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// Id de las selecciones a actualizar
        ///// </summary>
        //public Dictionary<int, List<int>> VarSeleccionesToUpdate
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// Id de las selecciones a insertar
        ///// </summary>
        //public Dictionary<int, List<int>> VarSeleccionesToInsert
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// Id de las selecciones a borrar
        ///// </summary>
        //public Dictionary<int, List<int>> VarSeleccionesToDelete
        //{
        //    get;
        //    set;
        //}
        private List<ConsultasFields> VariantesFields
        {
            get;
            set;
        }

        /// <summary>
        /// Fuente del filtro
        /// </summary>
        private IFiltrable SourceForm
        {
            get;
            set;
        }

        /// <summary>
        /// Indica si se puede seleccionar las columnas de las colsultas
        /// </summary>
        private bool AllowColumnSelect
        {
            get
            {
                return this._allowColumnSelect;
            }
            set
            {
                this._allowColumnSelect = value;
            }
        }

        #endregion

        #region Estaticas

        public static string ASC = "ASC";

        public static string DESC = "DESC";

        #endregion

        /// <summary>
        /// Constructor de la maestra con todos los datos requeridos 
        /// </summary>
        /// <param name="formaId">id de la maestra actual</param>
        /// <param name="userId">id del usuario actual</param>
        /// <param name="fields">Lista de campos de la maestra actual</param>
        public MasterQuery(IFiltrable ms, int documentID, int userId, bool allowSelect, List<ConsultasFields> fields, int? rsxID = null)
        {
            this.AllowColumnSelect = allowSelect;
            this.Start(ms, documentID, userId, fields);
        }

        /// <summary>
        /// Crea una ventana de consulta a partir de un tipo de dto
        /// </summary>
        /// <param name="ms">Objeto original a filtrar</param>
        /// <param name="formaId">id del documento</param>
        /// <param name="userId">id del usuario</param>
        /// <param name="dtoType">tipo del dto</param>
        /// <param name="ignoreFields">lista de campos a ignorar</param>
        public MasterQuery(IFiltrable ms, int documentID, int userId, bool allowSelect, Type dtoType, List<string> ignoreFields = null, int? rsxID = null)
        {
            if (ignoreFields==null)
                ignoreFields=new List<string>();

            this.AllowColumnSelect = allowSelect;
            if (!rsxID.HasValue)
                rsxID = documentID;

            List<ConsultasFields> fields = this.ConsultasFieldsFromDTOType(dtoType, rsxID.Value, ignoreFields);
            this.Start(ms, documentID, userId, fields);
        }

        /// <summary>
        /// Crea una ventana de consulta a partir de un tipo de dto
        /// </summary>
        /// <param name="ms">Objeto original a filtrar</param>
        /// <param name="formaId">id del documento</param>
        /// <param name="userId">id del usuario</param>
        /// <param name="dtoType">tipo del dto</param>
        /// <param name="atributeForGettingProperties">Atributo con el que se buscaran las propiedades a utilizar en los filtros</param>
        public MasterQuery(IFiltrable ms, int documentID, int userId, bool allowSelect, Type dtoType, Type atributeForGettingProperties, int? rsxID = null)
        {
            this.AllowColumnSelect = allowSelect;
            if (!rsxID.HasValue)
                rsxID = documentID;

            List<ConsultasFields> fields = this.ConsultasFieldsFromDTOType(dtoType, rsxID.Value, atributeForGettingProperties);
            this.Start(ms, documentID, userId, fields);
        }

        #region Funciones Auxiliares

        /// <summary>
        /// Crea ConsultaFields dinámicamente dado un dto.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private List<ConsultasFields> ConsultasFieldsFromDTOType(Type dtoType, int documentRsxId, List<string> ignoreFields)
        {
            List<ConsultasFields> res = new List<ConsultasFields>();
            PropertyInfo[] propiedades = dtoType.GetProperties();
            foreach (PropertyInfo pi in propiedades)
            {
                if (!ignoreFields.Contains(pi.Name))
                {
                    Type t=pi.PropertyType;
                    if (Transformer.IsSubclassOfRawGeneric(typeof(UDT), t))
                    {
                        PropertyInfo piUdt = pi.PropertyType.GetProperty("Value");
                        if (piUdt != null)
                            t = piUdt.PropertyType;
                    }
                    ConsultasFields cf = new ConsultasFields(pi.Name, _bc.GetResource(LanguageTypes.Forms, documentRsxId.ToString() + "_" + pi.Name), t);
                    res.Add(cf);
                }
            }
            FieldInfo[] fields = dtoType.GetFields();
            foreach (FieldInfo fi in fields)
            {
                if (!ignoreFields.Contains(fi.Name))
                {
                    Type t = fi.FieldType;
                    if (Transformer.IsSubclassOfRawGeneric(typeof(UDT), t))
                    {
                        PropertyInfo piUdt = fi.FieldType.GetProperty("Value");
                        if (piUdt != null)
                            t = piUdt.PropertyType;
                    }
                    ConsultasFields cf = new ConsultasFields(fi.Name, _bc.GetResource(LanguageTypes.Forms, documentRsxId.ToString() + "_" + fi.Name), t);
                    res.Add(cf);
                }
            }
            return res;
        }

        /// <summary>
        /// Crea ConsultaFields dinámicamente dado un dto.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private List<ConsultasFields> ConsultasFieldsFromDTOType(Type dtoType, int documentId, Type att)
        {
            List<ConsultasFields> res = new List<ConsultasFields>();
            PropertyInfo[] propiedades = dtoType.GetProperties();
            foreach (PropertyInfo pi in propiedades)
            {
                if (Attribute.IsDefined(pi, att))
                {
                    Type t = pi.PropertyType;
                    if (Transformer.IsSubclassOfRawGeneric(typeof(UDT), t))
                    {
                        PropertyInfo piUdt = pi.PropertyType.GetProperty("Value");
                        if (piUdt != null)
                            t = piUdt.PropertyType;
                    }
                    ConsultasFields cf = new ConsultasFields(pi.Name, _bc.GetResource(LanguageTypes.Forms, documentId.ToString() + "_" + pi.Name), t);
                    res.Add(cf);
                }
            }
            FieldInfo[] fields = dtoType.GetFields();
            foreach (FieldInfo fi in fields)
            {
                if (Attribute.IsDefined(fi, att))
                {
                    Type t = fi.FieldType;
                    if (Transformer.IsSubclassOfRawGeneric(typeof(UDT), t))
                    {
                        PropertyInfo piUdt = fi.FieldType.GetProperty("Value");
                        if (piUdt != null)
                            t = piUdt.PropertyType;
                    }
                    ConsultasFields cf = new ConsultasFields(fi.Name, _bc.GetResource(LanguageTypes.Forms, documentId.ToString() + "_" + fi.Name), t);
                    res.Add(cf);
                }
            }
            return res;
        }
        
        /// <summary>
        /// Actualiza la lista de selecciones de un filtro
        /// </summary>
        /// <param name="consulta">Identificador de la consulta</param>
        private void Update_Selections(int consulta)
        {
            try
            {
                List<DTO_glConsultaSeleccion> selecciones = new List<DTO_glConsultaSeleccion>();

                DTO_glConsulta cons = this.Variantes.Where(x => x.glConsultaID == consulta).First();
                cons.Selecciones = new List<DTO_glConsultaSeleccion>();
                foreach (ConsultasFields r in (List<ConsultasFields>)this.grlControlSelection.DataSource)
                {
                    int totalSels = 1;
                    int lastId = 1;

                    if (cons.Selecciones.Count > 0)
                    {
                        totalSels = cons.Selecciones.Count;
                        lastId = (int)cons.Selecciones.Last().glConsultaSeleccionID + 1;
                    }

                    if (r.A_Seleccion == true) // agregarlo si no existe, actualizarlo DLC
                    {
                        //agregarlo
                        DTO_glConsultaSeleccion nSel = new DTO_glConsultaSeleccion();
                        nSel.CampoDesc = r.Field;
                        nSel.CampoFisico = r.Field;
                        nSel.glConsultaID = consulta;
                        nSel.glConsultaSeleccionID = lastId;
                        nSel.GroupBy = r.A_GroupBy;
                        nSel.Idx = totalSels;
                        nSel.OrdenIdx = r.OrdenIndex;
                        nSel.OrdenTipo = r.Orden;
                        nSel.Tipo = r.Tipo.ToString();
                        selecciones.Add(nSel);
                    }
                }
                cons.Selecciones = selecciones;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Buscar el field en las selecciones si lo encuentra retorna el index en la lsita de lo controario -1
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private int GetIndexOnVarSelecciones(int consulta, string field)
        {
            int res = -1;

            DTO_glConsulta cons = this.Variantes.Where(x => x.glConsultaID == consulta).First();
            for (int i = 0; i < cons.Selecciones.Count; i++)
            {
                if (cons.Selecciones[i].CampoFisico == field)
                {
                    return i;
                }
            }

            return res;
        }

        /// <summary>
        /// Funcion para actualizar un filtro
        /// </summary>
        /// <param name="consulta">identificador de la consulta (filtro)</param>
        private void Update_Filters(int consulta)
        {
            try
            {
                DTO_glConsulta cons = this.Variantes.Where(x => x.glConsultaID == consulta).First();
                foreach (DTO_glConsultaFiltro r in (List<DTO_glConsultaFiltro>)this.grlControlFilter.DataSource)
                {
                    int index = this.GetIndexOnVarFiltros(consulta, (int)r.glConsultaFiltroID);

                    if (index != -1)
                    {
                        //actualizarlo
                        cons.Filtros[index].Idx = r.Idx;
                        cons.Filtros[index].OperadorFiltro = r.OperadorFiltro;
                        cons.Filtros[index].OperadorSentencia = r.OperadorSentencia;
                        cons.Filtros[index].ValorFiltro = r.ValorFiltro;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Buscar el filtro en la lista de filtros si lo encuentra retorna el index en la lsita de lo controario -1
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private int GetIndexOnVarFiltros(int consulta, int filterId)
        {
            int res = -1;

            DTO_glConsulta cons = this.Variantes.Where(x => x.glConsultaID == consulta).First();
            for (int i = 0; i < cons.Filtros.Count; i++)
            {
                if (cons.Filtros[i].glConsultaFiltroID == filterId)
                {
                    return i;
                }
            }

            return res;
        }

        /// <summary>
        /// Funcion auxiliar para la creación de una nueva variante
        /// </summary>
        private void New()
        {
            try
            {
                this.Insertando = true;

                //Cargar la data de la grilla de edicion
                this.LoadEditGridData(true, 0);

                //Setea el foco
                this.gvRecordEdit.Focus();
                this.gvRecordEdit.FocusedRowHandle = 0;

                //Asignar un consecutivo a la nueva variante
                int lastVarCode = 0;

                if (this.gvQuery.DataRowCount > 0)
                    lastVarCode = ((DTO_glConsulta)this.gvQuery.GetRow(this.gvQuery.DataRowCount - 1)).glConsultaID;

                List<GridProperty> grillaEdicion = (List<GridProperty>)grlControlRecordEdit.DataSource;

                grillaEdicion.Where(x => x.Campo.Equals(_col_Distincion)).First().Valor = "False";
                grillaEdicion.Where(x => x.Campo.Equals(_col_Activo)).First().Valor = "True";
                grillaEdicion.Where(x => x.Campo.Equals(_col_Prefijada)).First().Valor = "False";
                grillaEdicion.Where(x => x.Campo.Equals(_col_Distincion)).First().Valor = lastVarCode + 1;

                //Asinar el nuevo id a la variante
                this.groupSelect.Enabled = false;
                this.groupfilter.Enabled = false;
                this.groupSelect.Enabled = false;

                //Desactiva botones 
                this.btnNewQuery.Enabled = false;
                this.btnDeleteQuery.Enabled = false;
                this.btnEditQuery.Enabled = false;
                this.btnUseQuery.Enabled = false;

                //limpiar las grillas
                this.grlControlFilter.DataSource = null;
                this.gvcontrolGroup.DataSource = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Funciones privadas

        /// <summary>
        /// Se realiza al iniciar el formulario, 
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="formaId"></param>
        /// <param name="userId"></param>
        /// <param name="fields"></param>
        private void Start(IFiltrable ms, int documentID, int userId, List<ConsultasFields> fields)
        {
            try
            {
                this.VariantesFields = new List<ConsultasFields>();

                this.DocumentoID = documentID;
                this.UserID = userId;
                this._frmCode = AppForms.MasterQuery;

                //Fields
                this.FieldsToFilter = fields;

                //Inicializa el formulario
                this.InitializeComponent();

                //Setear los parametros inciales
                this.GetResourceGridColumns();
                this.GetResourceDataTypes();

                //Trae la info del Midtier
                this.LoadData();

                //Carga la grilla con la info
                this.LoadGridQueryData();

                //Carga la estructura de la grilla
                this.LoadGridQueryStructure();

                //Carga los recursos principales
                FormProvider.LoadResources(this, this._frmCode);    

                this.Text = this.lblTitle.Text;

                this.SourceForm = ms;
                if (this.Variantes.Count == 0)
                    this.New();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MasterQuery.cs", "Start"));
            }
        }

        /// <summary>
        /// Carga la info desde el MidTier
        /// </summary>
        private void LoadData()
        {
            try
            {
                // Traer la info
                DTO_glConsulta variante = new DTO_glConsulta();
                variante.DocumentoID = this.DocumentoID;
                this.Variantes = _bc.AdministrationModel.glConsulta_GetAll(variante, this.UserID).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MasterQuery.cs", "LoadData"));
            }
        }

        /// <summary>
        /// Carga los datos a la grilla
        /// </summary>
        private void LoadGridQueryData()
        {
            try
            {
                //Cargar la grilla
                this.grlControlQuery.DataSource = this.Variantes;
                this.LoadEditGridData(false, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MasterQuery.cs", "LoadGridDataQueryData"));
            }
        }

        /// <summary>
        /// Genera la estructura de la grilla
        /// </summary>
        private void LoadGridQueryStructure()
        {
            try
            {
                #region Organiza la grilla datos

                //Ocultar las columnas innecesarias
                this.gvQuery.Columns["glConsultaID"].Visible = false;
                this.gvQuery.Columns["FormaDesc"].Visible = false;
                this.gvQuery.Columns["UsuarioID"].Visible = false;
                this.gvQuery.Columns["Seleccion"].Visible = false;
                this.gvQuery.Columns["Filtro"].Visible = false;
                this.gvQuery.Columns["Distincion"].Visible = false;
                this.gvQuery.Columns["Activo"].Visible = false;
                this.gvQuery.Columns["CtrlVersion"].Visible = false;
                this.gvQuery.Columns["FechaCreacion"].Visible = false;
                this.gvQuery.Columns["UsuarioCreacion"].Visible = false;
                this.gvQuery.Columns["FechaAct"].Visible = false;
                this.gvQuery.Columns["UsuarioAct"].Visible = false;
                this.gvQuery.Columns["DocumentoID"].Visible = false;
                this.gvQuery.Columns["seUsuarioID"].Visible = false;
                this.gvQuery.Columns["Prefijada"].Visible = false;


                this.gvQuery.Columns["Nombre"].Caption = this._col_Nombre;

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MasterQuery.cs", "LoadGridStructure"));
            }
        }

        /// <summary>
        /// Actualiza los campos de la grilla de edición
        /// </summary>
        /// <param name="isNew">Indica si se va agregar un nuevo registro</param>
        /// <param name="rowIndex">Indice de la fila</param>
        private void LoadEditGridData(bool isNew, int rowIndex)
        {
            try
            {
                List<GridProperty> fillGridEdit = new List<GridProperty>();

                this.IdSelected = (isNew || gvQuery.Columns["glConsultaID"] == null || gvQuery.RowCount == 0) ? string.Empty : this.gvQuery.GetRowCellValue(rowIndex, gvQuery.Columns["glConsultaID"]).ToString();

                string val_glConsultaID = (isNew || gvQuery.Columns["glConsultaID"] == null || gvQuery.RowCount == 0) ? string.Empty : this.gvQuery.GetRowCellValue(rowIndex, gvQuery.Columns["glConsultaID"]).ToString();
                string val_Nombre = (isNew || gvQuery.Columns["Nombre"] == null || gvQuery.RowCount == 0) ? string.Empty : this.gvQuery.GetRowCellValue(rowIndex, gvQuery.Columns["Nombre"]).ToString();
                string val_FormaID = (isNew || gvQuery.Columns["DocumentoID"] == null || gvQuery.RowCount == 0) ? string.Empty : this.gvQuery.GetRowCellValue(rowIndex, gvQuery.Columns["DocumentoID"]).ToString();
                string val_seUsuarioID = (isNew || gvQuery.Columns["seUsuarioID"] == null || gvQuery.RowCount == 0) ? string.Empty : this.gvQuery.GetRowCellValue(rowIndex, gvQuery.Columns["seUsuarioID"]).ToString();
                string val_Seleccion = (isNew || gvQuery.Columns["Seleccion"] == null || gvQuery.RowCount == 0) ? string.Empty : this.gvQuery.GetRowCellValue(rowIndex, gvQuery.Columns["Seleccion"]).ToString();
                string val_Filtro = (isNew || gvQuery.Columns["Filtro"] == null || gvQuery.RowCount == 0) ? string.Empty : this.gvQuery.GetRowCellValue(rowIndex, gvQuery.Columns["Filtro"]).ToString();
                string val_Distincion = (isNew || gvQuery.Columns["Distincion"] == null || gvQuery.RowCount == 0) ? string.Empty : this.gvQuery.GetRowCellValue(rowIndex, gvQuery.Columns["Distincion"]).ToString();
                string val_Activo = (isNew || gvQuery.Columns["Activo"] == null || gvQuery.RowCount == 0) ? string.Empty : this.gvQuery.GetRowCellValue(rowIndex, gvQuery.Columns["Activo"]).ToString();
                string val_CtrlVersion = (isNew || gvQuery.Columns["CtrlVersion"] == null || gvQuery.RowCount == 0) ? string.Empty : this.gvQuery.GetRowCellValue(rowIndex, gvQuery.Columns["CtrlVersion"]).ToString();
                string val_FechaCreacion = (isNew || gvQuery.Columns["FechaCreacion"] == null || gvQuery.RowCount == 0) ? string.Empty : this.gvQuery.GetRowCellValue(rowIndex, gvQuery.Columns["FechaCreacion"]).ToString();
                string val_UsuarioCreacion = (isNew || gvQuery.Columns["UsuarioCreacion"] == null || gvQuery.RowCount == 0) ? string.Empty : this.gvQuery.GetRowCellValue(rowIndex, gvQuery.Columns["UsuarioCreacion"]).ToString();
                string val_FechaAct = (isNew || gvQuery.Columns["FechaAct"] == null || gvQuery.RowCount == 0) ? string.Empty : this.gvQuery.GetRowCellValue(rowIndex, gvQuery.Columns["FechaAct"]).ToString();
                string val_UsuarioAct = (isNew || gvQuery.Columns["UsuarioAct"] == null || gvQuery.RowCount == 0) ? string.Empty : this.gvQuery.GetRowCellValue(rowIndex, gvQuery.Columns["UsuarioAct"]).ToString();
                string val_Prefijada = (isNew || gvQuery.Columns["Prefijada"] == null || gvQuery.RowCount == 0) ? string.Empty : this.gvQuery.GetRowCellValue(rowIndex, gvQuery.Columns["Prefijada"]).ToString();

                //Llena la lista que envia los datos a la grilla de edición
                fillGridEdit.AddRange(new GridProperty[] 
                {                
                    new GridProperty(this._col_Nombre, val_Nombre.Trim()),
                    new GridProperty(this._col_Distincion, val_Distincion.Trim(), false),
                    new GridProperty(this._col_Activo, val_Activo.Trim(), false),
                    new GridProperty(this._col_Prefijada, val_Prefijada.Trim(), false), 
                    new GridProperty(this._col_glConsultaFiltroGrupoID, val_glConsultaID.Trim(), false),
                    new GridProperty(this._col_seUsuarioID, val_seUsuarioID.Trim(), false),
                    new GridProperty(this._col_Seleccion, val_Seleccion.Trim(), false),
                    new GridProperty(this._col_Filtro, val_Filtro.Trim(), false),                              
                    new GridProperty(this._col_FormaID, val_FormaID.Trim(), false),
                    new GridProperty(this._col_CtrlVersion, val_CtrlVersion.Trim(), false),
                    new GridProperty(this._col_FechaCreacion, val_FechaCreacion.Trim(), false),
                    new GridProperty(this._col_UsuarioCreacion, val_UsuarioCreacion.Trim(), false),
                    new GridProperty(this._col_FechaAct, val_FechaAct.Trim(), false),
                    new GridProperty(this._col_UsuarioAct, val_UsuarioAct.Trim(), false)
                });

                //Personaliza la primera columna 
                this.grlControlRecordEdit.DataSource = fillGridEdit;
                this.gvRecordEdit.OptionsCustomization.AllowSort = false;
                this.gvRecordEdit.OptionsCustomization.AllowRowSizing = false;
                this.gvRecordEdit.OptionsCustomization.AllowFilter = false;
                this.gvRecordEdit.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;
                this.gvRecordEdit.Columns["Campo"].Width = 70;
                this.gvRecordEdit.Columns["Campo"].OptionsColumn.AllowEdit = false;
                this.gvRecordEdit.Columns["Campo"].OptionsColumn.AllowFocus = false;
                this.gvRecordEdit.Columns["Campo"].AppearanceCell.BackColor = Color.GhostWhite;
                this.gvRecordEdit.Columns["Valor"].Width = 150;
                foreach(GridColumn col in gvRecordEdit.Columns){
                    if (!col.FieldName.Equals("Campo") && !col.FieldName.Equals("Valor"))
                        col.Visible = false;
                }
                this.gvRecordEdit.ActiveFilterString = "[Visible] = true";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Carga los datos a la grilla de selección
        /// </summary>
        private void LoadGridSelectionData()
        {
            try
            {
                //tomo los fiels y los selected de la grilla
                DTO_glConsulta selected = (DTO_glConsulta)this.gvQuery.GetFocusedRow();

                if (this.FieldsToFilter.Count > 0)
                {
                    GridColumn add = new GridColumn();
                    add.FieldName = "+";

                    //Carga la grilla
                    this.grlControlSelection.DataSource = this.VariantesFields;

                    //Campo
                    if (this.gvSelection.Columns["Field"]!=null)
                        this.gvSelection.Columns["Field"].Visible = false;

                    //Tipo
                    if (this.gvSelection.Columns["Tipo"] != null)
                        this.gvSelection.Columns["Tipo"].Visible = false;

                    //MetaData
                    if (this.gvSelection.Columns["MetaData"] != null)
                        this.gvSelection.Columns["MetaData"].Visible = false;

                    //A_GroupBy
                    if (this.gvSelection.Columns["A_GroupBy"] != null)
                        this.gvSelection.Columns["A_GroupBy"].Visible = false;

                    //Agrega una columnna a la data para adicionar campo a los filtros
                    if (gvSelection.Columns.ColumnByFieldName("+") == null)
                        gvSelection.Columns.AddVisible("+");

                    this.LoadGridSelectionStructure();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MasterQuerys.cs", "LoadGridSelectionData"));
            }
        }

        /// <summary>
        /// Genera la estructura de la grilla de selección
        /// </summary>
        private void LoadGridSelectionStructure()
        {
            try
            {
                #region Organiza la grilla de Seleccion

                this.gvSelection.Columns["Field"].OptionsColumn.ReadOnly = true;
                this.gvSelection.Columns["Field"].Caption = this._col_field;

                this.gvSelection.Columns["FieldShown"].Caption = this._col_field;
                this.gvSelection.Columns["FieldShown"].OptionsColumn.ReadOnly = true;

                this.gvSelection.Columns["MetaData"].OptionsColumn.ReadOnly = true;
                this.gvSelection.Columns["MetaData"].Caption = this._col_met;
                
                this.gvSelection.Columns["Tipo"].OptionsColumn.ReadOnly = true;
                this.gvSelection.Columns["Tipo"].Caption = this._col_type;

                this.gvSelection.Columns["TipoRsx"].OptionsColumn.ReadOnly = true;
                this.gvSelection.Columns["TipoRsx"].Caption = this._col_type;

                this.gvSelection.Columns["A_Seleccion"].Width = 100;
                this.gvSelection.Columns["A_Seleccion"].Caption = this._col_apselec;

                this.gvSelection.Columns["Orden"].Width = 60;
                this.gvSelection.Columns["Orden"].Caption = this._col_ord;
                this.gvSelection.Columns["Orden"].ColumnEdit = cmbSelection;

                this.gvSelection.Columns["OrdenIndex"].Width = 60;
                this.gvSelection.Columns["OrdenIndex"].Caption = this._col_ordix;
                this.gvSelection.Columns["OrdenIndex"].ColumnEdit = cmbSelection;

                //Envia el Index correspondiente al campo OrdenIndex
                this.gvSelection.Columns["A_Seleccion"].Visible = this.AllowColumnSelect;
                this.gvSelection.Columns["Orden"].Visible = this.AllowColumnSelect;
                this.gvSelection.Columns["OrdenIndex"].Visible = this.AllowColumnSelect;

                #endregion

                this.gvSelection.Columns["+"].Width = 30;
                this.gvSelection.Columns["+"].Caption = " ";
                this.gvSelection.Columns["+"].ColumnEdit = this.btnAddFieldToGrid;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MasterQuerys.cs", "LoadGridSelectionStructure"));
            }
        }

        /// <summary>
        /// Carga los datos a la grilla de filtros
        /// </summary>
        private void LoadGridFilterData()
        {
            try
            {
                //tomo los fiels y los selected de la grilla
                DTO_glConsulta selectedVariant = (DTO_glConsulta)this.gvQuery.GetFocusedRow();

                this.grlControlFilter.DataSource = null;
                if (this.gvGroup.DataRowCount > 0)
                {
                    var selectedGroup = "";
                    if (this.gvGroup.GetFocusedRow() != null)
                        //tomo los fiels y los selected de la grilla
                        selectedGroup = ((GridProperty)this.gvGroup.GetFocusedRow()).Campo;
                    else
                        selectedGroup = ((GridProperty)this.gvGroup.GetRow(this.gvGroup.RowCount - 1)).Campo;

                    //remover el G del grupo
                    int grupo = Convert.ToInt32(selectedGroup.ToString().Replace('G', ' ').Trim());
                    var filtrosGrupo = selectedVariant.Filtros.Where(x => x.glConsultaFiltroGrupo == grupo).ToList();
                    if (filtrosGrupo.Count > 0)
                    {
                        GridColumn del = new GridColumn();
                        del.FieldName = "x";

                        grlControlFilter.DataSource = filtrosGrupo;

                        //Agrega una columnna a la data para borrar el filtro actual
                        if (gvFilter.Columns.ColumnByFieldName("x") == null)
                            gvFilter.Columns.AddVisible("x");

                        this.LoadGridFilterStructure();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MasterQuerys.cs", "LoadGridFilterData"));
            }
        }

        /// <summary>
        /// Genera la estructura de la grilla de filtros
        /// </summary>
        private void LoadGridFilterStructure()
        {
            try
            {
                #region Organiza la grilla de Filtro

                this.gvFilter.Columns["glConsultaFiltroID"].Visible = false;
                this.gvFilter.Columns["glConsultaID"].Visible = false;
                this.gvFilter.Columns["CampoDesc"].Visible = false;
                this.gvFilter.Columns["Idx"].Visible = false;

                this.gvFilter.Columns["glConsultaFiltroGrupo"].OptionsColumn.ReadOnly = true;
                this.gvFilter.Columns["glConsultaFiltroGrupo"].Caption = this._col_glConsultaFiltroGrupoID;

                this.gvFilter.Columns["CampoFisico"].Width = 120;
                this.gvFilter.Columns["CampoFisico"].OptionsColumn.ReadOnly = true;
                this.gvFilter.Columns["CampoFisico"].Caption = this._col_field;

                this.gvFilter.Columns["OperadorFiltro"].Caption = this._col_oper;
                this.gvFilter.Columns["OperadorFiltro"].ColumnEdit = this.cmbFilter;

                this.gvFilter.Columns["ValorFiltro"].OptionsColumn.ReadOnly = false;
                this.gvFilter.Columns["ValorFiltro"].Caption = this._col_val;

                this.gvFilter.Columns["OperadorSentencia"].Caption = this._col_opersent;
                this.gvFilter.Columns["OperadorSentencia"].ColumnEdit = this.cmbFilter;

                #endregion

                this.gvFilter.Columns["x"].Width = 30;
                this.gvFilter.Columns["x"].Caption = " ";
                this.gvFilter.Columns["x"].ColumnEdit = this.btnDeleteRow;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MasterQuerys.cs", "LoadGridFilterStructure"));
            }
        }

        /// <summary>
        /// Carga los datos a la grilla de grupos de filtro
        /// </summary>
        private void LoadGridGroupData()
        {
            try
            {
                //tomo los fiels y los selected de la grilla
                DTO_glConsulta selected = (DTO_glConsulta)this.gvQuery.GetFocusedRow();

                if (selected!=null && selected.Filtros!=null && selected.Filtros.Count > 0)
                {
                    GridColumn del = new GridColumn();
                    del.FieldName = "x";

                    List<GridProperty> grupos = new List<GridProperty>();
                    //List<Tuple<string, string>> grupos = new List<Tuple<string, string>>();

                    var tempG = selected.Filtros.GroupBy(x => x.glConsultaFiltroGrupo).ToList();
                    for (int i = 0; i < tempG.Count; i++)
                    {
                        var element = tempG[i].First();
                        grupos.Add(new GridProperty("G" + element.glConsultaFiltroGrupo, ((DTO_glConsultaFiltro)element).OperadorSentencia));

                        
                    }
                    //Carga la grilla
                    this.gvcontrolGroup.DataSource = grupos;

                    if (gvGroup.Columns.ColumnByFieldName("x") == null)
                        gvGroup.Columns.AddVisible("x");

                    if (gvGroup.Columns.ColumnByFieldName("Campo") == null)
                        gvGroup.Columns.ColumnByFieldName("Campo");
                    this.LoadGridGroupStructure();

                    //Cargar los filtros del grupo seleccionado
                    this.LoadGridFilterData();
                }
                else
                {
                    this.gvcontrolGroup.DataSource = null;
                    this.grlControlFilter.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MasterQuerys.cs", "LoadGridGroupData"));
            }
        }

        /// <summary>
        /// Genera la estructura de la grilla de selección
        /// </summary>
        private void LoadGridGroupStructure()
        {
            try
            {
                #region Organiza la grilla de Seleccion
                this.gvGroup.Columns["Valor"].ColumnEdit = this.cmbFilter;
                this.gvGroup.Columns["x"].Width = 15;
                this.gvGroup.Columns["x"].ColumnEdit = this.btnDeleteRow;
                this.gvGroup.FocusedRowHandle = this.gvGroup.DataRowCount - 1;

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MasterQuerys.cs", "LoadGridSelectionStructure"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadDataForSelectedVariant()
        {
            try
            {
                //tomo los fiels y los selected de la grilla
                DTO_glConsulta selected = (DTO_glConsulta)this.gvQuery.GetFocusedRow();

                //Aqui llena el objeto VariantesFields ya transformado para mostrarlo en la grilla
                this.VariantesFields = new List<ConsultasFields>();

                foreach (ConsultasFields f in this.FieldsToFilter)
                {
                    ConsultasFields fl = new ConsultasFields();
                    fl.Field = f.Field;
                    fl.FieldShown = f.FieldShown;
                    fl.Tipo = f.Tipo;

                    if(fl.Tipo == typeof(string))
                        fl.TipoRsx = this._col_string;
                    else if (fl.Tipo == typeof(short) || fl.Tipo == typeof(short?))
                        fl.TipoRsx = this._col_int16;
                    else if (fl.Tipo == typeof(int) || fl.Tipo == typeof(int?))
                        fl.TipoRsx = this._col_int32;
                    else if (fl.Tipo == typeof(long) || fl.Tipo == typeof(long?))
                        fl.TipoRsx = this._col_int64;
                    else if (fl.Tipo == typeof(decimal) || fl.Tipo == typeof(decimal?))
                        fl.TipoRsx = this._col_decimal;
                    else if (fl.Tipo == typeof(double) || fl.Tipo == typeof(double?))
                        fl.TipoRsx = this._col_double;
                    else if (fl.Tipo == typeof(bool) || fl.Tipo == typeof(bool?))
                        fl.TipoRsx = this._col_bool;
                    else if (fl.Tipo == typeof(DateTime) || fl.Tipo == typeof(DateTime?))
                        fl.TipoRsx = this._col_datetime;
                    else
                        fl.TipoRsx = fl.Tipo.ToString();

                    //////Mirar si tiene Seleccion
                    if (selected != null && selected.Selecciones!=null)
                    {
                        var sels = selected.Selecciones.Where(x => x.CampoFisico == f.Field);
                        if (sels.Count() > 0)
                        {
                            fl.A_Seleccion = true;
                            if (sels.First().GroupBy != null)
                            {
                                if (sels.First().GroupBy == true)
                                    fl.A_GroupBy = true;
                                else
                                    fl.A_GroupBy = false;
                            }
                            fl.Orden = sels.First().OrdenTipo;
                            if (fl.Orden.Equals(ASC) || fl.Orden.Equals(DESC))
                                fl.OrdenIndex = sels.First().OrdenIdx;
                            else
                                fl.OrdenIndex = 99;
                        }
                        else
                            fl.A_Seleccion = false;
                    }
                    else
                        fl.A_Seleccion = false;

                    //Agrega
                    this.VariantesFields.Add(fl);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MasterQuerys.cs", "LoadDataForSelectedVariant"));
            }
        }

        /// <summary>
        /// Obtiene los recursos para las columna de las grillas
        /// </summary>
        private void GetResourceGridColumns()
        {
            try
            {
                this._col_glConsultaFiltroGrupoID = _bc.GetResource(LanguageTypes.Forms, this._frmCode + "_glConsultaFiltroGrupo");
                this._col_Nombre = _bc.GetResource(LanguageTypes.Forms, this._frmCode + "_Nombre");
                this._col_Distincion = _bc.GetResource(LanguageTypes.Forms, this._frmCode + "_Distincion");
                this._col_Activo = _bc.GetResource(LanguageTypes.Forms, this._frmCode + "_Activo");
                this._col_field = _bc.GetResource(LanguageTypes.Forms, this._frmCode + "_Field");
                this._col_met = _bc.GetResource(LanguageTypes.Forms, this._frmCode + "_Metadata");
                this._col_type = _bc.GetResource(LanguageTypes.Forms, this._frmCode + "_Tipo");
                this._col_apselec = _bc.GetResource(LanguageTypes.Forms, this._frmCode + "_A_Seleccion");
                this._col_ord = _bc.GetResource(LanguageTypes.Forms, this._frmCode + "_Orden");
                this._col_ordix = _bc.GetResource(LanguageTypes.Forms, this._frmCode + "_OrdenIndex");
                this._col_oper = _bc.GetResource(LanguageTypes.Forms, this._frmCode + "_OperadorFiltro");
                this._col_val = _bc.GetResource(LanguageTypes.Forms, this._frmCode + "_ValorFiltro");
                this._col_opersent = _bc.GetResource(LanguageTypes.Forms, this._frmCode + "_OperadorSentencia");
                this._col_Prefijada = _bc.GetResource(LanguageTypes.Forms, this._frmCode + "_Prefijada");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene los recursos para los tipos de datos
        /// </summary>
        private void GetResourceDataTypes()
        {
            try
            {
                this._col_string = _bc.GetResource(LanguageTypes.Forms, this._frmCode + "_" + typeof(string).ToString());
                this._col_int16 = _bc.GetResource(LanguageTypes.Forms, this._frmCode + "_" + typeof(short).ToString());
                this._col_int32 = _bc.GetResource(LanguageTypes.Forms, this._frmCode + "_" + typeof(int).ToString());
                this._col_int64 = _bc.GetResource(LanguageTypes.Forms, this._frmCode + "_" + typeof(long).ToString());
                this._col_decimal = _bc.GetResource(LanguageTypes.Forms, this._frmCode + "_" + typeof(decimal).ToString());
                this._col_double = _bc.GetResource(LanguageTypes.Forms, this._frmCode + "_" + typeof(double).ToString());
                this._col_bool = _bc.GetResource(LanguageTypes.Forms, this._frmCode + "_" + typeof(bool).ToString());
                this._col_datetime = _bc.GetResource(LanguageTypes.Forms, this._frmCode + "_" + typeof(DateTime).ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void FillSelected()
        {
            try
            {
                this.LoadDataForSelectedVariant();
                this.LoadGridSelectionData();

                //Traer los grupos y filtros del selected
                this.LoadGridGroupData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Retorna una variante de la lista de variantes
        /// </summary>
        /// <param name="id">id de la variante</param>
        /// <returns>Devuelve variante</returns>
        private DTO_glConsulta GetVariante(int id)
        {
            try
            {
                DTO_glConsulta v = null;

                foreach (DTO_glConsulta b in this.Variantes)
                {
                    if (b.glConsultaID.Equals(id))
                    {
                        v = b;
                        break;
                    }
                }
                return v;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Retorna una selección a partir de un id para una variante Dada
        /// </summary>
        /// <param name="varID">id de la variante dada</param>
        /// <param name="id">id de la selección</param>
        /// <returns>Devuelve index de la selección</returns>
        private int GetIndexSeleccionForVariant(int varID, int id)
        {
            try
            {
                int response = -1;

                List<DTO_glConsultaSeleccion> sels = GetVariante(varID).Selecciones;

                for (int i = 0; i < sels.Count(); i++)
                {
                    if (sels[i].glConsultaSeleccionID.Equals(id))
                    {
                        response = i;
                        break;
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Retorna un filtro a partir de un id para una variante Dada
        /// </summary>
        /// <param name="varID">id de la variante dada</param>
        /// <param name="id">id del filtro</param>
        /// <returns>Devuelve index del filtro</returns>
        private int GetIndexFiltroForVariant(int varID, int id)
        {
            try
            {
                int response = -1;

                List<DTO_glConsultaFiltro> fils = GetVariante(varID).Filtros;
                for (int i = 0; i < fils.Count(); i++)
                {
                    if (fils[i].glConsultaFiltroID.Equals(id))
                    {
                        response = i;
                        break;
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Actualiza las fechas de una busqueda
        /// </summary>
        private void ActualizarFechas()
        {
            try
            {
                DTO_glConsulta selectedVar = (DTO_glConsulta)this.gvQuery.GetFocusedRow();
                foreach (DTO_glConsultaFiltro filtro in selectedVar.Filtros)
                {
                    ConsultasFields r = this.FieldsToFilter.Where(x => x.Field.Equals(filtro.CampoFisico)).First();
                    if (r.Tipo == typeof(DateTime) || r.Tipo == typeof(DateTime?) && !string.IsNullOrWhiteSpace(filtro.ValorFiltro))
                        filtro.ValorFiltro = DateTime.Parse(filtro.ValorFiltro).ToString(FormatString.Date);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Guarda las consultas
        /// </summary>
        private void Save(DTO_glConsulta consult)
        {
            try
            {
                //variante seleccionada (revisar que el ultimo grupo no tenga un and o un or de ser asi no dejar continuar)
                DTO_glConsulta selectedVar = (DTO_glConsulta)this.gvQuery.GetFocusedRow();

                this.Update_Selections(selectedVar.glConsultaID);

                //Grupo
                int grupo = -1;
                if (this.gvGroup.GetFocusedRow() != null)
                {
                    var selectedRow = ((GridProperty)this.gvGroup.GetFocusedRow()).Campo;
                    grupo = Convert.ToInt32(selectedRow.ToString().Substring(1, 1));
                }
                string msgfilterConnection = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FilterConnection);
                string msggroupValidation = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.GroupValidation);

                //determinar que todos los filtros esten conectados ente si por un operador y que el ultimo no tenga
                bool valid = true;
                List<DTO_glConsultaFiltro> listF = new List<DTO_glConsultaFiltro>();
                try
                {
                    listF = selectedVar.Filtros;
                }
                catch (Exception e) {}

                if (listF.Count > 1)
                {//Valida que tengan un conector asignado
                    for (int i = 0; i < listF.Count; i++)
                    {
                        if (!listF[i].OperadorSentencia.Equals("AND") && !listF[i].OperadorSentencia.Equals("OR"))
                            valid = false;
                    }
                }

                if (valid == false)
                {
                    MessageBox.Show(msgfilterConnection);
                    return;
                }
                this._bc.AdministrationModel.glConsulta_Update(consult);
                this.groupSelect.Enabled = false;
                this.groupfilter.Enabled = false;
                this.pnlGroupFilter.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MasterQueries.cs", "Save"));
            }
        }

        #endregion

        #region Eventos grillas

        /// <summary>
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvQuery_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                this.Insertando = false;
                this.LoadEditGridData(false, e.FocusedRowHandle);
                this.btnDeleteQuery.Enabled = true;
                this.btnNewQuery.Enabled = true;
                this.btnEditQuery.Enabled = true;
                this.btnUseQuery.Enabled = true;
                this.groupSelect.Enabled = false;
                this.groupfilter.Enabled = false;
                this.pnlGroupFilter.Enabled = false;

                // Set the Filelds 
                this.FillSelected();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MasterQuerys.cs", "gvQuery_FocusedRowChanged"));
            }
        }

        /// <summary>
        /// Coloca en la celda booleana el editor(check) requerido
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvRecordEdit_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            try
            {
                if ((e.CellValue.Equals("True") || e.CellValue.Equals("False")))
                {
                    //Asigna una caja de chequeo a las celdas que contengan True o False 
                    this.chkRecordEdit.ReadOnly = false;
                    e.RepositoryItem = chkRecordEdit;
                    this.chkRecordEdit.ValueChecked = "True";
                    this.chkRecordEdit.ValueUnchecked = "False";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Asigna un editor (button, check, textbox..) a la celda relacionada del index
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvRecordEdit_CustomRowCellEditForEditing(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            try
            {
                //Asigna una caja de texto al campo Descripción
                if (e.RowHandle.Equals(0))
                {
                    e.RepositoryItem = txtRecordEdit;
                    this.txtRecordEdit.MaxLength = 255;
                }
                if (this.gvQuery.DataRowCount == 0)
                {
                    this.New();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Identifica la tecla presionada para cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void grlRecordEdit_ProcessGridKey(object sender, KeyEventArgs e)
        {

        }

        /// <summary>
        /// Bloquea la accion de la rueda del mouse
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvRecordEdit_MouseWheel(object sender, MouseEventArgs e)
        {
            (e as DevExpress.Utils.DXMouseEventArgs).Handled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvSelection_CustomRowCellForEditing(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            try
            {
                ConsultasFields r = (ConsultasFields)this.gvSelection.GetFocusedRow();
                if (e.Column.FieldName == "Orden")
                {
                    this.cmbSelection.Items.Clear();
                    this.cmbSelection.Items.Add(ASC);
                    this.cmbSelection.Items.Add(DESC);
                    this.cmbSelection.Items.Add("Unsorted");
                }
                if (e.Column.FieldName == "OrdenIndex")
                {
                    if (r.Orden.Equals(ASC) || r.Orden.Equals(DESC))
                    {
                        this.cmbSelection.Items.Clear();
                        List<ConsultasFields> dataFil = (List<ConsultasFields>)this.grlControlSelection.DataSource;
                        dataFil = dataFil.Where(x => (x.Orden.Equals(ASC) || x.Orden.Equals(DESC))).ToList();
                        List<int> disponibles = new List<int>();
                        for (int i = 1; i <= (dataFil.Count + 1); i++)
                            disponibles.Add(i);
                        
                        foreach (ConsultasFields cf in dataFil)
                            if (!cf.Field.Equals(r.Field))
                                disponibles.Remove(cf.OrdenIndex);

                        foreach (int i in disponibles)
                            this.cmbSelection.Items.Add(i);
                    }
                    else
                    {
                        this.cmbSelection.Items.Clear();
                        this.cmbSelection.Items.Add(99);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvFilter_CustomRowCellEditForEditing(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            try
            {
                this.ActualizarFechas();
                DTO_glConsultaFiltro filtro = ((this.gvFilter.GetFocusedRow()) as DTO_glConsultaFiltro);
                ConsultasFields r = this.FieldsToFilter.Where(x => x.Field.Equals(filtro.CampoFisico)).First();
                var t = r.Tipo;

                List<string> Operators = new List<string>();
                Operators.Add(OperadorFiltro.Mayor);
                Operators.Add(OperadorFiltro.MayorIgual);
                Operators.Add(OperadorFiltro.Menor);
                Operators.Add(OperadorFiltro.MenorIgual);
                Operators.Add(OperadorFiltro.Igual);
                Operators.Add(OperadorFiltro.Diferente);

                //Verifica el tipo de dato en OperadorFiltro y agrega una lista
                if (e.Column.FieldName == "OperadorFiltro")
                {
                    this.cmbFilter.Items.Clear();
                    if (t == typeof(string))
                    {
                        //Operators.Add("Like");
                        //cmbFilter.Items.AddRange(Operators);
                        cmbFilter.Items.Add(OperadorFiltro.Contiene); 
                        cmbFilter.Items.Add(OperadorFiltro.Comienza);
                        cmbFilter.Items.Add(OperadorFiltro.Igual);
                    }
                    else if (t == typeof(bool))
                    {
                        cmbFilter.Items.Add(OperadorFiltro.Igual);
                        cmbFilter.Items.Add(OperadorFiltro.Diferente);
                    }
                    else
                        cmbFilter.Items.AddRange(Operators);
                }
                //Agrega una lista en OperadorSentencia
                if (e.Column.FieldName == "ValorFiltro" && (t == typeof(bool) || t == typeof(bool?)))
                {
                    e.RepositoryItem = cmbFilter;
                    this.cmbFilter.Items.Clear();
                    this.cmbFilter.Items.Add("True");
                    this.cmbFilter.Items.Add("False");
                }
                //Fechas
                if (e.Column.FieldName == "ValorFiltro" && (t == typeof(DateTime) || t == typeof(DateTime?)))
                {
                    dateedit.EditMask = FormatString.Date;
                    dateedit.EditFormat.FormatString = FormatString.Date;
                    dateedit.DisplayFormat.FormatString = FormatString.Date;
                    e.RepositoryItem = dateedit;
                }
                //Agrega una lista en OperadorSentencia
                if (e.Column.FieldName == "ValorFiltro" && this._fks.ContainsKey(r.Field))
                {
                    object config = this._fks[r.Field];
                    if (config is Tuple<int, ButtonEditFKConfiguration>)
                    {
                        Tuple<int, ButtonEditFKConfiguration> c = config as Tuple<int, ButtonEditFKConfiguration>;
                        string empGrupo = _bc.GetMaestraEmpresaGrupoByDocumentID(c.Item1);
                        Tuple<int, string> tup = new Tuple<int, string>(c.Item1, empGrupo);
                        DTO_glTabla table=null;
                        if (_bc.AdministrationModel.Tables.TryGetValue(tup, out table))
                        {
                            btnEdit.CharacterCasing = c.Item2.Casing;
                            e.RepositoryItem = btnEdit;

                        }
                    }
                    if (config is Dictionary<string, string>)
                    {
                        Dictionary<string, string> c = config as Dictionary<string, string>; 
                        e.RepositoryItem = cmbFilter;
                        this.cmbFilter.Items.Clear();
                        foreach (string key in c.Keys)
                        {
                            this.cmbFilter.Items.Add(key);
                        }
                    }
                }
                if (e.Column.FieldName == "OperadorSentencia")
                {
                    this.cmbFilter.Items.Clear();
                    this.cmbFilter.Items.Add("AND");
                    this.cmbFilter.Items.Add("OR");
                    if (this.gvFilter.IsLastRow)
                        this.cmbFilter.Items.Add("N/A");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvGroup_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                //Cargar los filtros del grupo seleccionado
                this.LoadGridFilterData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvGroup_CustomRowCellEditForEditing(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == "Valor")
                {
                    this.cmbFilter.Items.Clear();
                    if (this.gvGroup.IsLastRow)
                    {
                        this.cmbFilter.Items.Add("N/A");
                    }
                    else
                    {
                        this.cmbFilter.Items.Add("AND");
                        this.cmbFilter.Items.Add("OR");
                    }
                }
                if (e.Column.FieldName == "Campo")
                {
                    txtRecordEdit.ReadOnly = true;
                    e.RepositoryItem=txtRecordEdit;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Los filtros que tienen listas de valores para mostrar descripción
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbFilter_DrawItem(object sender, ListBoxDrawItemEventArgs e)
        {
            ComboBoxEdit cbe = (ComboBoxEdit)sender;
            DTO_glConsultaFiltro filtro = ((this.gvFilter.GetFocusedRow()) as DTO_glConsultaFiltro);
            bool columnaFiltro = this.gvFilter.FocusedColumn.FieldName.Equals("ValorFiltro");
            ConsultasFields r = this.FieldsToFilter.Where(x => x.Field.Equals(filtro.CampoFisico)).First();
            if (columnaFiltro && _fks.ContainsKey(filtro.CampoFisico) && _fks[filtro.CampoFisico] is Dictionary<string, string>)
            {
                Dictionary<string, string> values = (Dictionary<string, string>)_fks[filtro.CampoFisico];
                string desc = values[e.Item.ToString()];
                if ((e.State & DrawItemState.Selected) > 0)
                {
                    Font boldFont = new Font(cbe.Font.FontFamily, cbe.Font.Size, FontStyle.Bold);
                    e.Graphics.DrawString(e.Item.ToString() + "-" + desc, boldFont, new SolidBrush(cbe.ForeColor),
                      e.Bounds);
                    e.Handled = true;
                }
                else
                {
                    e.Graphics.DrawString(e.Item.ToString() + "-" + desc, cbe.Font, new SolidBrush(cbe.ForeColor),
                      e.Bounds);
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Click del boton de FK en un filtro
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            ButtonEdit be = (ButtonEdit)sender;
            DTO_glConsultaFiltro fil = (DTO_glConsultaFiltro)this.gvFilter.GetFocusedRow();
            if (this._fks.ContainsKey(fil.CampoFisico))
            {
                object con = this._fks[fil.CampoFisico];
                if (con is Tuple<int, ButtonEditFKConfiguration>)
                {
                    Tuple<int, ButtonEditFKConfiguration> config = con as Tuple<int, ButtonEditFKConfiguration>;
                    string empGrupo = _bc.GetMaestraEmpresaGrupoByDocumentID(config.Item1);
                    Tuple<int, string> tup = new Tuple<int, string>(config.Item1, empGrupo);
                    DTO_glTabla table = null;
                    if (_bc.AdministrationModel.Tables.TryGetValue(tup, out table))
                    {
                        if (table.Jerarquica.Value != null && table.Jerarquica.Value.Value)
                        {
                            //Modal Jerarquica
                            //MasterHierarchyFind modal = new MasterHierarchyFind();
                            //modal.InitControl(table, config.Item2.FkConfig,0);
                            //modal.ShowDialog();
                            //if (modal.DialogResult == DialogResult.OK)
                            //    be.Text = modal.ResultCode;

                            ModalMaster modal = new ModalMaster(be, config.Item1.ToString(), config.Item2.FkConfig.CountMethod, config.Item2.FkConfig.DataMethod, config.Item2.FkConfig.args, config.Item2.FkConfig.KeyField, config.Item2.FkConfig.DescField, true);
                            modal.ShowDialog();
                        }
                        else
                        {
                            ModalMaster modal = new ModalMaster(be, config.Item1.ToString(), config.Item2.FkConfig.CountMethod, config.Item2.FkConfig.DataMethod, config.Item2.FkConfig.args, config.Item2.FkConfig.KeyField, config.Item2.FkConfig.DescField, false);
                            modal.ShowDialog();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Btn de los filtros de FK
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            
        }

        #endregion

        #region Eventos Botones

        /// <summary>
        /// Crea la instancia para ingresar un nuevo registro
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnNew_Click(object sender, EventArgs e)
        {
            New();
        }

        /// <summary>
        /// Borra el(los) registro(s) seleccionado(s)
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string msgDeleteQuery = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.QueryDelete);
                string msgTitleDelete = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Delete));

                List<GridProperty> grillaEdicion = (List<GridProperty>)this.gvRecordEdit.DataSource;

                string nombre = Convert.ToString(grillaEdicion.Where(x => x.Campo.Equals(this._col_glConsultaFiltroGrupoID)).First().Valor);

                if (this.Variantes == null || this.Variantes.Count == 0 || grillaEdicion.Where(x => x.Campo.Equals(this._col_glConsultaFiltroGrupoID)).Count() == 0)
                    return;

                //Obtener el id de la variante de la grilla
                int id = Convert.ToInt32(grillaEdicion.Where(x => x.Campo.Equals(this._col_glConsultaFiltroGrupoID)).First().Valor);
                //Obtener la version
                int ver = Convert.ToInt32(grillaEdicion.Where(x => x.Campo.Equals(this._col_CtrlVersion)).First().Valor);

                //obtener la variante
                DTO_glConsulta selectedVar = (DTO_glConsulta)this.gvQuery.GetFocusedRow();

                try
                {
                    DialogResult result = MessageBox.Show(msgDeleteQuery, msgTitleDelete, MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        //Borrar la variante
                        this._bc.AdministrationModel.glConsulta_Delete(selectedVar);

                        //Borrar en la grilla
                        this.Variantes.RemoveAll(x => x.glConsultaID == selectedVar.glConsultaID);

                        //Agregarlo en la grilla y posicionar la grilla en el que acabo de adicionar
                        try
                        {
                            this.grlControlQuery.DataSource = new List<DTO_glConsulta>();
                            this.grlControlQuery.DataSource = this.Variantes;
                        }
                        catch (Exception ex11)
                        {
                        }

                        if (this.Variantes.Count > 0)
                            this.gvQuery.MoveFirst();
                    }

                }
                catch (Exception ex)
                {
                    //no se pudo borrar
                }
                this.groupSelect.Enabled = false;
                this.groupfilter.Enabled = false;
                this.pnlGroupFilter.Enabled = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Edita la consulta actual
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnEditQuery_Click(object sender, EventArgs e)
        {
            if (this.btnNewQuery.Enabled == false)
            {
                List<GridProperty> grillaEdicion = (List<GridProperty>)this.gvRecordEdit.DataSource;

                //agregar la consulta
                DTO_glConsulta newC = new DTO_glConsulta();
                newC.Activo = true;
                newC.CtrlVersion = 1;
                newC.Distincion = null; //sacarlo 
                newC.DocumentoID = this.DocumentoID;
                newC.FechaAct = System.DateTime.Now;
                newC.FechaCreacion = System.DateTime.Now;
                newC.Filtro = "";
                newC.FormaDesc = this.DocumentoID.ToString();
                newC.glConsultaID = 1;
                newC.Nombre = Convert.ToString(grillaEdicion.Where(x => x.Campo.Equals(this._col_glConsultaFiltroGrupoID)).First().Valor);
                newC.Prefijada = Convert.ToBoolean(grillaEdicion.Where(x => x.Campo.Equals(this._col_Prefijada)).First().Valor);
                newC.Seleccion = "";
                newC.seUsuarioID = this.UserID;
                newC.UsuarioAct = this.UserID;
                newC.UsuarioCreacion = this.UserID;
                newC.UsuarioID = this.UserID.ToString();

                //agregar al dic
                this.Variantes.Add(newC);

                //agregarlo en la grilla y posicionar la grilla en el que acabo de adicionar
                this.grlControlQuery.DataSource = null;
                this.grlControlQuery.DataSource = this.Variantes;
            }

            this.groupSelect.Enabled = true;
            this.groupfilter.Enabled = true;
            this.pnlGroupFilter.Enabled = true;
        }

        /// <summary>
        /// Guarda solo consulta con o sin filtros
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnSaveQuery_Click(object sender, EventArgs e)
        {
            try
            {
                List<GridProperty> grillaEdicion = (List<GridProperty>)this.gvRecordEdit.DataSource;

                string nombre = Convert.ToString(grillaEdicion.Where(x => x.Campo.Equals(this._col_Nombre)).First().Valor);
                if (string.IsNullOrEmpty(nombre))
                {
                    string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.QueryNameRequired);
                    MessageBox.Show(msg);
                }
                else
                {
                    DTO_glConsulta query = new DTO_glConsulta();
                    
                    if (this.btnNewQuery.Enabled == true)
                    {
                        query = (DTO_glConsulta)this.gvQuery.GetFocusedRow();
                        query.Activo = Convert.ToBoolean(grillaEdicion.Where(x => x.Campo.Equals(this._col_Activo)).First().Valor);
                        query.Nombre = nombre;
                        this.Save(query);
                    }
                    else
                    {
                        //agregar la consulta
                        query.Activo = Convert.ToBoolean(grillaEdicion.Where(x => x.Campo.Equals(this._col_Activo)).First().Valor);
                        query.CtrlVersion = 1;
                        query.Distincion = null; //sacarlo 
                        query.DocumentoID = this.DocumentoID;
                        query.FechaAct = System.DateTime.Now;
                        query.FechaCreacion = System.DateTime.Now;
                        query.Filtro = "";
                        query.FormaDesc = this.DocumentoID.ToString();
                        query.glConsultaID = 1;
                        query.Nombre = nombre;
                        query.Prefijada = Convert.ToBoolean(grillaEdicion.Where(x => x.Campo.Equals(this._col_Prefijada)).First().Valor);
                        query.Seleccion = "";
                        query.seUsuarioID = this.UserID;
                        query.UsuarioAct = this.UserID;
                        query.UsuarioCreacion = this.UserID;
                        query.UsuarioID = this.UserID.ToString();

                        DTO_glConsulta resp = this._bc.AdministrationModel.glConsulta_Add(query);

                        //agregar al dic
                        this.Variantes.Add(resp);

                        //agregarlo en la grilla y posicionar la grilla en el que acabo de adicionar
                        this.grlControlQuery.DataSource = null;
                        this.grlControlQuery.DataSource = this.Variantes;
                        this.gvQuery.FocusedRowHandle = this.gvQuery.DataRowCount - 1;

                        //activeme todo
                        this.groupSelect.Enabled = true;
                        this.groupfilter.Enabled = true;
                        this.pnlGroupFilter.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Llamar la clase que retorna el linq
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnUseQuery_Click(object sender, EventArgs e)
        {
            try
            {
                //variante seleccionada (revisar que el ultimo grupo no tenga un and o un or de ser asi no dejar continuar)
                DTO_glConsulta selectedVar = (DTO_glConsulta)this.gvQuery.GetFocusedRow();
                List<GridProperty> grillaEdicion = (List<GridProperty>)gvRecordEdit.DataSource;
                if (this.Variantes == null || this.Variantes.Count == 0 || grillaEdicion.Where(x => x.Campo.Equals(this._col_glConsultaFiltroGrupoID)).Count() == 0 || grillaEdicion.Where(x => x.Campo.Equals(this._col_CtrlVersion)).Count() == 0 || selectedVar == null)
                    return;

                this.Update_Selections(selectedVar.glConsultaID);

                //Grupo
                GridProperty selectedGroup = (GridProperty)this.gvGroup.GetFocusedRow();
                if (selectedGroup != null)
                {
                    var selectedRow = ((GridProperty)this.gvGroup.GetFocusedRow()).Campo;
                    int grupo = Convert.ToInt32(selectedRow.ToString().Substring(1, 1));
                }
                //Mensajes 
                string msgfilterConnection = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FilterConnection);
                string msggroupValidation = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.GroupValidation);
                string msgqueryEmpty = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.QueryEmpty);
                string msgquerySave = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.QuerySave);

                ////determinar que todos los filtros esten conectados ente si por un operador y que el ultimo no tenga
                bool valid = true;
                var listF = selectedVar.Filtros;

                for (int i = 0; i < listF.Count; i++)
                {
                    if (i != listF.Count - 1)
                        if (listF[i].OperadorSentencia != "AND" && listF[i].OperadorSentencia != "OR")
                            valid = false;
                    else
                        if (listF[i].ValorFiltro == "")
                            valid = false;
                }

                if (valid == false)
                {
                    MessageBox.Show(msgfilterConnection);
                    return;
                }

                int id = selectedVar.glConsultaID;

                //Verificar q tenga selescciones y filtros
                List<DTO_glConsultaSeleccion> selToTransform = selectedVar.Selecciones;//this.VarSelecciones[id];
                List<DTO_glConsultaFiltro> filToTransform = selectedVar.Filtros;//new List<DTO_glConsultaFiltro>();

                //preguntar si se quiere guardar la consulta
                if (this.groupSelect.Enabled)
                {
                    DialogResult result = MessageBox.Show(msgquerySave, "Información", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                        this.Save(selectedVar);
                }

                //usarla
                this.SourceForm.SetConsulta(selectedVar, this.FieldsToFilter);
                this.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Cierra el formulario de variantes
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Eventos Botones Grillas

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteRow_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvFilter.IsFocusedView)
                {
                    //variante seleccionada
                    DTO_glConsulta selectedVar = (DTO_glConsulta)this.gvQuery.GetFocusedRow();

                    DTO_glConsultaFiltro selectedRow = (DTO_glConsultaFiltro)this.gvFilter.GetFocusedRow();

                    this.gvFilter.DeleteSelectedRows();

                    //aqui borrar el campo de la lista y borralos de los filtros
                    selectedVar.Filtros.RemoveAll(x => x.glConsultaFiltroID == selectedRow.glConsultaFiltroID);

                }
                else if (gvGroup.IsFocusedView)
                {
                    //variante seleccionada
                    DTO_glConsulta selectedVar = (DTO_glConsulta)this.gvQuery.GetFocusedRow();

                    var selectedRow = ((GridProperty)this.gvGroup.GetFocusedRow()).Campo;

                    int grupo = Convert.ToInt32(selectedRow.ToString().Substring(1, 1));

                    selectedVar.Filtros.RemoveAll(x => x.glConsultaFiltroGrupo == grupo);

                    this.LoadGridGroupData();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Fires when the btn add field to grid grid is pressed
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event</param>
        private void btnAddFieldToGrid_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int grupo = 1;

                //Obtener el grupo actual
                if (this.gvGroup.RowCount > 0)
                {
                    var selectedRow = ((GridProperty)this.gvGroup.GetFocusedRow()).Campo;
                    grupo = Convert.ToInt32(selectedRow.ToString().Substring(1, 1));
                }

                //adicionar un filtro nuevo
                ConsultasFields rowF = (ConsultasFields)this.gvSelection.GetFocusedRow();

                DTO_glConsulta selectedVar = (DTO_glConsulta)this.gvQuery.GetFocusedRow();

                int lastFil = 0;
                int index = 0;

                if (selectedVar.Filtros.Count > 0)
                {
                    var last = selectedVar.Filtros.Last();
                    lastFil = (int)last.glConsultaFiltroID;
                }

                DTO_glConsultaFiltro fil = new DTO_glConsultaFiltro();
                fil.glConsultaID = selectedVar.glConsultaID;
                fil.glConsultaFiltroID = lastFil + 1;
                fil.glConsultaFiltroGrupo = grupo;
                fil.CampoFisico = rowF.Field;
                fil.CampoDesc = rowF.Field;
                fil.Idx = index + 1;
                fil.OperadorFiltro = "=";
                fil.OperadorSentencia = "N/A";
                fil.ValorFiltro = "";

                if (selectedVar.Filtros.Count > 0)
                    fil.OperadorSentencia = selectedVar.Filtros.Last().OperadorSentencia;

                selectedVar.Filtros.Add(fil);

                this.LoadGridGroupData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Identifica la sentencia seleccionada para agregar o no una nueva linea de filtro
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void cmbFilter_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                var item = ((DevExpress.XtraEditors.ComboBoxEdit)(sender)).SelectedItem;
                string msgfilterconnection = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FilterConnection);

                //variante seleccionada
                DTO_glConsulta selectedVar = (DTO_glConsulta)this.gvQuery.GetFocusedRow();

                if (gvGroup.IsFocusedView)
                {
                    if (item.ToString() == "AND" || item.ToString() == "OR")
                    {
                        //determinar que todos los filtros esten conectados ente si por un operador y que el ultimo no tenga
                        bool valid = true;
                        var listF = selectedVar.Filtros;// this.VarFiltros[selectedVar.glConsultaID].ToList();


                        for (int i = 0; i < listF.Count; i++)
                        {
                            if (i != listF.Count - 1)
                                if (listF[i].OperadorSentencia != "AND" || listF[i].OperadorSentencia != "OR")// && listF[i].ValorFiltro == "")
                                    valid = false;
                            else
                                if (listF[i].ValorFiltro == "")
                                    valid = false;
                        }

                        if (valid)
                        {
                            //Grupo
                            var selectedRow = ((GridProperty)this.gvGroup.GetFocusedRow()).Campo;
                            int grupo = Convert.ToInt32(selectedRow.ToString().Substring(1, 1));

                            //Aqui asignar al ultimo filtro el operador
                            selectedVar.Filtros.Where(x => x.glConsultaFiltroGrupo == grupo).Last().OperadorSentencia = item.ToString();

                            this.grlControlFilter.DataSource = null;
                            this.grlControlSelection.Focus();
                        }
                        else
                        {
                            MessageBox.Show(msgfilterconnection);
                            return;
                        }
                    }
                }
                else if (gvFilter.IsFocusedView)
                {
                    if (item.ToString() == "AND" || item.ToString() == "OR")
                    {
                        //grabar el operdor en el filtro correspondiente, si es el ultimo no hacer nada

                        var selectedRow = ((GridProperty)this.gvGroup.GetFocusedRow()).Campo;
                        int grupo = Convert.ToInt32(selectedRow.ToString().Substring(1, 1));

                        //Aqui asignar al ultimo filtro el operador
                        var idUltimo = selectedVar.Filtros.Where(x => x.glConsultaFiltroGrupo == grupo).Last().glConsultaFiltroID;

                        //Filtrer en cuestio
                        var varianteFiltroID = ((DTO_glConsultaFiltro)this.gvFilter.GetFocusedRow()).glConsultaFiltroID;

                        //Aqui asignar al ultimo filtro el operador
                        foreach (DTO_glConsultaFiltro fil in selectedVar.Filtros)
                        {
                            if (fil.glConsultaFiltroGrupo == grupo)
                                fil.OperadorSentencia = item.ToString();
                        }

                        //pintar
                        this.LoadGridFilterData();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MasterQuerys.cs", "cmbFilter_SelectedValueChanged"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta con un cambio de fechas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DateEdit_DateTimeChanged(object sender, EventArgs e)
        {
            DateEdit de = (DateEdit)sender;
            DTO_glConsultaFiltro filtro = (DTO_glConsultaFiltro)gvFilter.GetFocusedRow();
            filtro.ValorFiltro = de.DateTime.ToString(FormatString.Date);
        }

        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Configura un campo como FK
        /// </summary>
        /// <param name="fieldName">nombre del campo</param>
        /// <param name="documentoId">numero del documento al que apunta</param>
        /// <param name="config">configuración</param>
        public void SetFK(string fieldName, int documentoId, ButtonEditFKConfiguration config)
        {
            if (this._fks.ContainsKey(fieldName))
                this._fks[fieldName] = new Tuple<int, ButtonEditFKConfiguration>(documentoId, config);
            else
                this._fks.Add(fieldName, new Tuple<int, ButtonEditFKConfiguration>(documentoId, config));
        }

        /// <summary>
        /// Asigna un campo como una dicionario de llave, valor
        /// </summary>
        /// <param name="fieldName">nombre del campo</param>
        /// <param name="values">valores posibles para el campo</param>
        public void SetValueDictionary(string fieldName, Dictionary<string, string> values)
        {
            if (this._fks.ContainsKey(fieldName))
                this._fks[fieldName] = values;
            else
                this._fks.Add(fieldName, values);
        }

        #endregion

    }//clase
}//namespace
