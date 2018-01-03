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
using System.Diagnostics;
using System.Net;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using System.Reflection;
using DevExpress.Data;
using System.Threading;
using SentenceTransformer;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class QueryFiltersForm : FormWithToolbar, IFiltrable
    {
        #region Variables

        protected BaseController _bc = BaseController.GetInstance();

        //Para manejo de propiedades
        private FormTypes _frmType = FormTypes.DocumentAprob;
        private string _frmName;
        private bool newSearch = false;
        private Dictionary<string, string> columnsRsx = new Dictionary<string, string>();
        private SaveFileDialog sfdGuardaDoc;
        private Excel.Application app;

        //Variables protected
        protected int userId;
        protected int documentID;
        protected int documenFiltroID;
        protected ModulesPrefix frmModule;
        protected string viewName;
        protected Type viewType;
        protected DataTable results;
        protected DTO_glConsulta viewFilter;
        protected MasterQuery mq;
        protected Dictionary<int, string> docs = new Dictionary<int, string>();

        #endregion

        #region Propiedades

        /// <summary>
        /// Indica si un formulario modal esta abierto
        /// </summary>
        protected bool IsModalFormOpened
        {
            get;
            set;
        }

        #endregion

        #region Constructor

        ///// <summary>
        ///// Constructor por defecto
        ///// </summary>
        ///// <param name="mod"></param>
        //public QueryFiltersForm()
        //{
        //    this.InitializeComponent();
        //}

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public QueryFiltersForm(string mod = null)
        {
            InitializeComponent();
            try
            {
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());

                if (!string.IsNullOrWhiteSpace(mod))
                    this.frmModule = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), mod);

                FormProvider.Master.Form_Load(this, this.frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryFiltersForm.cs", "QueryFiltersForm.cs-QueryFiltersForm"));
            }
        }

        #endregion

        #region Funciones privadas

        /// <summary>
        /// Agrega las columnas a las grillas
        /// </summary>
        private void LoadData()
        {
            try
            {
                string msgTitleSearch = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_GetData));
                string msgNewSearch = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewSearch);

                if (this.newSearch)
                {
                    if (MessageBox.Show(msgNewSearch, msgTitleSearch, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        this.newSearch = false;
                        this.GetResults();
                    }
                }
                else
                {
                    this.newSearch = true;
                    this.GetResults();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "QueryFiltersForm.cs", "LoadData"));
            }
        }

        /// <summary>
        /// Agrega las columnas a las grillas
        /// </summary>
        private void GetResults()
        {
            try
            {
                this.results = _bc.AdministrationModel.ConsultasGenerales(this.viewName, this.viewType, this.viewFilter);
                this.FormatResults();

                this.gcQuery.DataSource = results;
                this.gvQuery.PopulateColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "QueryFiltersForm.cs", "GetResults"));
            }
        }

        /// <summary>
        /// Funcion que se encarga de Traer los recuros
        /// </summary>
        private void FormatResults()
        {
            //Clear columns 
            this.gvQuery.Columns.Clear();

            //Colums names
            for (int i = 0; i < this.results.Columns.Count; i++)
            {
                DataColumn col = this.results.Columns[i];
                if (!this.columnsRsx.ContainsKey(col.ColumnName))
                    this.columnsRsx[col.ColumnName] = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + col.ColumnName);

                col.Caption = this.columnsRsx[col.ColumnName];
            }

            //Trim data
            foreach (DataRow dr in this.results.Rows)
            {
                foreach (DataColumn dc in this.results.Columns)
                {
                    if (dc.DataType == typeof(string))
                    {
                        object o = dr[dc];
                        if (!Convert.IsDBNull(o) && o != null)
                        {
                            dr[dc] = o.ToString().Trim();
                        }
                    }
                }
            }
        }

        #endregion

        #region Funciones Protected

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected virtual void SetInitParameters()
        {
            this.userId = _bc.AdministrationModel.User.ReplicaID.Value.Value;
            this.lkpConsulta.Properties.DataSource = this.docs;

            this.sfdGuardaDoc = new SaveFileDialog();
            this.sfdGuardaDoc.Filter = "Archivos Excel | *.XLS";
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
                FormProvider.Master.Form_Enter(this, this.documentID, this._frmType, this.frmModule);
                FormProvider.Master.itemSearch.Visible = true;
                FormProvider.Master.itemSearch.Enabled = true;
                FormProvider.Master.itemFilter.Visible = true;
                FormProvider.Master.itemFilter.Enabled = true;
                FormProvider.Master.itemExport.Visible = true;
                FormProvider.Master.itemExport.Enabled = true;

                FormProvider.Master.itemSave.Visible = false;
                FormProvider.Master.itemUpdate.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryFiltersForm.cs", "Form_Enter"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Leave(object sender, EventArgs e)
        {
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryFiltersForm.cs", "Form_Closing"));
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
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this.frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryFiltersForm.cs", "Form_FormClosed"));
            }
        }

        #endregion Eventos MDI

        #region Eventos Formulario

        /// <summary>
        /// Evento para lista los documetnos asociados por glDocumento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void lkpConsulta_EditValueChanged(object sender, System.EventArgs e)
        {
            this.viewFilter = null;
            this.results = null;
        }

        #endregion Eventos Formulario

        #region Eventos Barra Herramientas

        /// <summary>
        /// Boton para crear buscar
        /// </summary>
        public override void TBSearch()
        {
            try
            {
                if (this.lkpConsulta.EditValue != null)
                {
                    if(this.viewFilter == null)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.QueryNoFilter));
                        return;
                    }

                    string msgTitleSearch = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_GetData));
                    string msgNewSearch = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewSearch);

                    if (this.newSearch)
                    {
                        if (MessageBox.Show(msgNewSearch, msgTitleSearch, MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            this.newSearch = false;
                            this.LoadData();
                        }
                    }
                    else
                    {
                        this.newSearch = true;
                        this.LoadData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryFiltersForm.cs", "TBSearch"));
            }
        }

        /// <summary>
        /// Filtra información de la grilla
        /// </summary>
        public override void TBFilter()
        {
            try
            {
                if (this.lkpConsulta.EditValue != null)
                {
                    this.mq.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryFiltersForm.cs", "TBFilter"));
            }
        }

        /// <summary>
        /// Boton para Exportar A Excel
        /// </summary>
        public override void TBExport()
        {
            try
            {
                if (this.results != null && this.results.Rows.Count > 0)
                {
                    if (this.sfdGuardaDoc.ShowDialog(this) == DialogResult.OK)
                    {
                        this.gvQuery.ExportToXls(this.sfdGuardaDoc.FileName);

                        this.app = new Excel.Application();
                        this.app.Visible = true;

                        this.app.Workbooks.Open(System.IO.Path.GetFullPath(this.sfdGuardaDoc.FileName));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryFiltersForm.cs", "TBExport"));
            }
        }

        #endregion Eventos Barra Herramientas

        #region Filtros

        /// <summary>
        /// Asigna una consulta desde MasterQuery para hacer el filtrado de datos
        /// </summary>
        /// <param name="consulta"></param>
        /// <param name="fields"></param>
        public virtual void SetConsulta(DTO_glConsulta consulta, List<ConsultasFields> fields)
        {
            try
            {
                this.viewFilter = consulta;
                this.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryFiltersForm.cs", "SetConsulta"));
            }
        }

        #endregion

    }
}
