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
    public partial class UC_DatosGenerales : UserControl
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();
        private DTO_glDocumentoControl _ctrl = null;
        private DTO_ccSolicitudDocu _solicitud = null;
        private int _numeroDoc = 0;
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
        public UC_DatosGenerales()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor control
        /// </summary>
        /// <param name="empleado">empleado</param>
        public UC_DatosGenerales(int numeroDoc)
        {
            InitializeComponent();
            this.documentID = AppDocuments.PreProyecto;
            this._numeroDoc = numeroDoc;
            this.InitControls();
            this.LoadData(numeroDoc);



        }
        #region Funciones Públicas

        /// <summary>
        /// Restaura los valores iniciales
        /// </summary>
        /// <param name="p"></param>
        public void CleanControl()
        {
            try
            { 
                //this.txtNro.EditValue = string.Empty;
                //this.txtDescripcion.Text = string.Empty;
                //this.txtLicitacion.Text = string.Empty;
                //this.ProyectoInfo = null;
                //this.ProyectoID = string.Empty;
                //this.PrefijoID = string.Empty;
                //this.DocumentoNro= null;
                //this.ClienteID = string.Empty;
                //this.btnQueryDoc.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-UC_DatosGenerales.cs", "CleanHeader"));
            }
        }

        #endregion

        #region Funciones privadas

        /// <summary>
        /// Inicializa los controles 
        /// </summary>
        private void InitControls()
        {
            this._bc.InitMasterUC(this.masterCiudadGeneral, AppMasters.glLugarGeografico, true, false, true, true);
            this._bc.InitMasterUC(this.masterVitrina, AppMasters.ccConcesionario, true, true, true, true);
            this._bc.InitMasterUC(this.masterZona, AppMasters.glZona, true, true, true, false);    
        }

        /// <summary>
        /// Carga la información
        /// </summary>
        /// <param name="firstTime"></param>
        public void LoadData(int numeroDoc)
        {
            try
            {
                //this._solicitud = this._bc.AdministrationModel.SolicitudProyecto_Load(AppDocuments.Proyecto, prefijoID, docNro, numeroDoc, string.Empty, proyectoID, this._loadPreproyecto,this._loadMvtos,this._loadActasTrabajo,false,this._loadTrazabilidad);

                //if (this._solicitud != null)
                //{
                //    if (this._solicitud.DocCtrl.Estado.Value != (byte)EstadoDocControl.Aprobado)
                //    {
                //        MessageBox.Show("El Proyecto no se encuentra Aprobado");
                //        return;
                //    }

                //    this.masterProyecto.Value = this._solicitud.DocCtrl.ProyectoID.Value;
                //    this.masterPrefijo.Value = this._solicitud.DocCtrl.PrefijoID.Value;
                //    this.txtNro.Text = this._solicitud.DocCtrl.DocumentoNro.Value.ToString();
                //    this.masterCliente.Value = this._solicitud.HeaderProyecto.ClienteID.Value;
                //    this.txtLicitacion.Text = this._solicitud.HeaderProyecto.Licitacion.Value;
                //    this.txtDescripcion.Text = this._solicitud.HeaderProyecto.DescripcionSOL.Value;                 
                //}
                //else
                //{
                //    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidDocument));
                //    this._ctrl = new DTO_glDocumentoControl();
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCompras", "LoadData"));
            }
        }  

        #endregion

        #region Propiedades

        ///// <summary>
        ///// Obtiene El  ClienteID
        ///// </summary>
        //public DTO_SolicitudTrabajo ProyectoInfo
        //{
        //    get { return this._solicitud; }
        //    set { this._solicitud = value; }
        //}

        #endregion  
    }
}
