using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.DTO.Negocio;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using NewAge.DTO.UDT;
using System.Reflection;
using DevExpress.XtraEditors;
using SentenceTransformer;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid;
using DevExpress.Utils;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class UC_Proyecto : UserControl
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();
        private DTO_glDocumentoControl _ctrl = null;
        private DTO_SolicitudTrabajo _proyectoInfo = null;
        private int documentID; 
        private bool _loadPreproyecto = false;
        private bool _loadMvtos = false;
        private bool _loadActasTrabajo = false;
        private bool _loadTrazabilidad = false;

        #endregion

        #region Handlers

        // Declaración de delagados y evento click
        public delegate void EventHandler(object sender, System.EventArgs e);
        EventHandler handlerLoadProyectoInfo;        

        /// <summary>
        /// Asigna y remueve los handlers
        /// </summary>
        new public event EventHandler LoadProyectoInfo_Leave
        {
            add { this.handlerLoadProyectoInfo += value; }
            remove { this.handlerLoadProyectoInfo -= value; }
        }


        #endregion

        /// <summary>
        /// Constructor control
        /// </summary>
        /// <param name="empleado">empleado</param>
        public UC_Proyecto()
        {
            InitializeComponent();
        }

        #region Funciones Públicas

        /// <summary>
        /// Inicializa el control
        /// </summary>
        public void Init(bool loadPreproyectoInd,bool loadMvtos, bool loadActasTrabajo, bool loadTrazabilidad)
        {
            this.documentID = AppDocuments.PreProyecto;                    
            this.InitControls();
            this._loadPreproyecto = loadPreproyectoInd;
            this._loadMvtos = loadMvtos;
            this._loadActasTrabajo = loadActasTrabajo;
            this._loadTrazabilidad = loadTrazabilidad;
        }

        /// <summary>
        /// Restaura los valores iniciales
        /// </summary>
        /// <param name="p"></param>
        public void CleanControl()
        {
            try
            { 
                this.txtNro.EditValue = string.Empty;
                this.txtDescripcion.Text = string.Empty;
                this.txtLicitacion.Text = string.Empty;
                this.ProyectoInfo = null;
                this.ProyectoID = string.Empty;
                this.PrefijoID = string.Empty;
                this.DocumentoNro= null;
                this.ClienteID = string.Empty;
                this.btnQueryDoc.Enabled = true;

                this.masterPrefijo.Value = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-PlaneacionTiempo.cs", "CleanHeader"));
            }
        }

        #endregion

        #region Funciones privadas

        /// <summary>
        /// Inicializa los controles 
        /// </summary>
        private void InitControls()
        {
            this._bc.InitMasterUC(this.masterPrefijo, AppMasters.glPrefijo, true, false, true, true);
            this._bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true, true, true, true);
            this._bc.InitMasterUC(this.masterCliente, AppMasters.faCliente, true, true, true, false);
            this.masterPrefijo.Value = this._bc.GetControlValueByCompany(ModulesPrefix.co,AppControl.co_PrefijoXDefecto);
        }

        /// <summary>
        /// Carga la información
        /// </summary>
        /// <param name="firstTime"></param>
        public void LoadData(string prefijoID, int? docNro, int? numeroDoc, string proyectoID)
        {
            try
            {
                this._proyectoInfo = this._bc.AdministrationModel.SolicitudProyecto_Load(AppDocuments.Proyecto, prefijoID, docNro, numeroDoc, string.Empty, proyectoID, this._loadPreproyecto,this._loadMvtos,this._loadActasTrabajo,false,this._loadTrazabilidad);

                if (this._proyectoInfo != null)
                {
                    if (this._proyectoInfo.DocCtrl.Estado.Value != (byte)EstadoDocControl.Aprobado)
                    {
                        MessageBox.Show("El Proyecto no se encuentra Aprobado");
                        return;
                    }

                    this.masterProyecto.Value = this._proyectoInfo.DocCtrl.ProyectoID.Value;
                    this.masterPrefijo.Value = this._proyectoInfo.DocCtrl.PrefijoID.Value;
                    this.txtNro.Text = this._proyectoInfo.DocCtrl.DocumentoNro.Value.ToString();
                    this.masterCliente.Value = this._proyectoInfo.HeaderProyecto.ClienteID.Value;
                    this.txtLicitacion.Text = this._proyectoInfo.HeaderProyecto.Licitacion.Value;
                    this.txtDescripcion.Text = this._proyectoInfo.HeaderProyecto.DescripcionSOL.Value;                 
                }
                else
                {
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidDocument));
                    this._ctrl = new DTO_glDocumentoControl();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCompras", "LoadData"));
            }
        }  

        #endregion

        #region Eventos 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void masterProyecto_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.masterProyecto.ValidID)
                {
                    this.LoadData(string.Empty, null, null, this.masterProyecto.Value);
                }
                else
                {
                    this.CleanControl();
                }

                if (this.handlerLoadProyectoInfo != null)
                    this.handlerLoadProyectoInfo(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCompras", "masterProyecto_Leave"));
            }
        }

        /// <summary>
        /// Al dejar el control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        public void txtNro_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtNro.Text) && !string.IsNullOrEmpty(this.masterPrefijo.Value))
                {
                    int docNro = Convert.ToInt32(this.txtNro.Text);
                    DTO_glDocumentoControl docCtrl = this._bc.AdministrationModel.glDocumentoControl_GetInternalDoc(AppDocuments.Proyecto, this.masterPrefijo.Value, docNro);
                    if (docCtrl != null)
                        this.LoadData(this.masterPrefijo.Value, docNro, null, string.Empty);
                }
                else
                {
                    this.CleanControl();
                }

                if (this.handlerLoadProyectoInfo != null)
                    this.handlerLoadProyectoInfo(sender, e);
            }
            catch (Exception ex)
            {                
               MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCompras", "txtNro_Leave"));
            }
        }

        /// <summary>
        /// Al dejar el control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnQueryDoc_Click(object sender, EventArgs e)
        {
            try
            {
                List<int> docs = new List<int>();
                if(this._loadPreproyecto)
                    docs.Add(AppDocuments.PreProyecto);
                else
                    docs.Add(AppDocuments.Proyecto);
                ModalFindDocSolicitud getDocControl = new ModalFindDocSolicitud(docs, false, true);
                getDocControl.ShowDialog();
                if (getDocControl.DocumentoControl != null)
                {
                    this.txtNro.Enabled = true;
                    this.txtNro.Text = getDocControl.DocumentoControl.DocumentoNro.Value.ToString();
                    this.masterPrefijo.Value = getDocControl.DocumentoControl.PrefijoID.Value;
                    this.txtNro.Focus();
                    this.btnQueryDoc.Focus();
                    this.btnQueryDoc.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostos", "btnQueryDoc_Click"));
            }
        }
        #endregion

        #region Propiedades

        /// <summary>
        /// Obtiene El  proyecto
        /// </summary>
        public string ProyectoID
        {
            get { return this.masterProyecto.Value; }
            set { this.masterProyecto.Value = value; }
        }

        /// <summary>
        /// Obtiene El  PrefijoID
        /// </summary>
        public string PrefijoID
        {
            get { return this.masterPrefijo.Value; }
            set { this.masterPrefijo.Value = value; }
        }

        /// <summary>
        /// Obtiene El  PrefijoID
        /// </summary>
        public int? DocumentoNro
        {
            get 
            { 
                int? docNro = null;
                if (!string.IsNullOrEmpty(this.txtNro.Text)) 
                    docNro = Convert.ToInt32(this.txtNro.Text);
                return docNro; 
            }
            set 
            {
                int? docNro = null;
                if (!string.IsNullOrEmpty(this.txtNro.Text))
                    docNro = Convert.ToInt32(this.txtNro.Text);
                docNro = value; 
            }
        }

        /// <summary>
        /// Obtiene El  ClienteID
        /// </summary>
        public string ClienteID
        {
            get { return this.masterCliente.Value; }
            set { this.masterCliente.Value = value; }
        }

        /// <summary>
        /// Obtiene El  ClienteID
        /// </summary>
        public DTO_SolicitudTrabajo ProyectoInfo
        {
            get { return this._proyectoInfo; }
            set { this._proyectoInfo = value; }
        }

        #endregion  
    }
}
