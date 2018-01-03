﻿using System;
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

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de documentos
    /// </summary>
    public partial class ConsultaProyectoForm : FormWithToolbar
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
        private int _numeroDoc = 0;
        //Variables de datos
        private DTO_pyProyectoDocu _proyectoDocu = new DTO_pyProyectoDocu();
        private DTO_glDocumentoControl _ctrlProyecto = null;
        private DTO_pyProyectoTarea _rowTarea = new DTO_pyProyectoTarea();
        private DTO_pyProyectoDeta _rowDetalle = new DTO_pyProyectoDeta();
        private List<DTO_pyProyectoTarea> _listTareasAll = new List<DTO_pyProyectoTarea>();
        private List<DTO_pyProyectoDeta> _listRecursosXTareaAll = new List<DTO_pyProyectoDeta>();
        private List<DTO_pyProyectoMvto> _listMvtos = new List<DTO_pyProyectoMvto>();
        #endregion

        #region Delegados

        private delegate void SaveGrid();
        private SaveGrid saveDelegate;
        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        private  void SaveMethod() 
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudServicio.cs", "RefreshGridMethod"));
            }             
        }

        #endregion

        ///<summary>
        /// Constructor 
        /// </summary>
        public ConsultaProyectoForm()
        {
            try
            {
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());

                this.LoadDocumentInfo(true);
                this.frmModule = ModulesPrefix.py;

                FormProvider.Master.Form_Load(this, this.frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaProyectoForm.cs", "ConsultaProyectoForm"));
            }
        }

        #region Funciones Privadas

        /// <summary>
        /// Add Columns in Grid
        /// </summary>
        protected virtual void AddGridCols()
        {
           
        }

        /// <summary>
        /// Inicializar controles
        /// </summary>
        protected virtual void InitControls()
        {

        }

        /// <summary>
        /// Loads the document main info
        /// </summary>
        private void LoadDocumentInfo(bool firstTime)
        {
           
        }

        /// <summary>
        /// Carga la información
        /// </summary>
        /// <param name="firstTime"></param>
        protected virtual void LoadData(string prefijoID, int? docNro, int? numeroDoc, string proyectoID, bool actaTrabajoExist = false)
        {
            try
            {
                DTO_SolicitudTrabajo transaccion = this._bc.AdministrationModel.SolicitudProyecto_Load(AppDocuments.Proyecto, prefijoID, docNro, numeroDoc, string.Empty, proyectoID, false,true,false,false);

                if (transaccion != null)
                {
                    if (transaccion.DocCtrl.Estado.Value != (byte)EstadoDocControl.Aprobado)
                    {
                        MessageBox.Show("El Proyecto no se encuentra Aprobado");
                        return;
                    }

                    this._proyectoDocu = transaccion.HeaderProyecto;
                    this._listTareasAll = transaccion.DetalleProyecto;
                    this._listMvtos = transaccion.Movimientos;
                    this._ctrlProyecto = transaccion.DocCtrl;

                    #region Calcula Dias de Atraso
                    //(Tarea)
                    foreach (var tarea in this._listTareasAll.FindAll(x => x.FechaFin.Value != null))
                    {
                        if (tarea.FechaFin.Value.Value < DateTime.Today)
                            tarea.DiasAtraso.Value = tarea.FechaTermina.Value != null ? Math.Abs(tarea.FechaFin.Value.Value.Day - tarea.FechaTermina.Value.Value.Day) : Math.Abs(tarea.FechaFin.Value.Value.Day - DateTime.Today.Day);
                        //(Recurso)
                        foreach (var det in tarea.Detalle.FindAll(x => x.FechaFin.Value != null))
                        {
                            if (tarea.FechaFin.Value.Value < DateTime.Today)
                                det.DiasAtraso.Value = det.FechaTermina.Value != null ? Math.Abs(det.FechaFin.Value.Value.Day - det.FechaTermina.Value.Value.Day) : Math.Abs(det.FechaFin.Value.Value.Day - DateTime.Today.Day);
                        }
                    } 
                    #endregion

                    this.masterProyecto.Value = transaccion.DocCtrl.ProyectoID.Value;
                    this.masterPrefijo.Value = transaccion.DocCtrl.PrefijoID.Value;
                    this.txtNro.Text = transaccion.DocCtrl.DocumentoNro.Value.ToString();
                    this.masterCliente.Value = transaccion.HeaderProyecto.ClienteID.Value;
                    this.txtLicitacion.Text = transaccion.HeaderProyecto.Licitacion.Value;
                    this.txtDescripcion.Text = transaccion.HeaderProyecto.DescripcionSOL.Value;                
                    this.LoadGrids();
                }
                else
                {
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidDocument));
                    this._ctrlProyecto = new DTO_glDocumentoControl();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCompras", "LoadData"));
            }
        }

        /// <summary>
        /// Carga la información del detalle
        /// </summary>
        /// <param name="firstTime"></param>
        protected virtual void LoadGrids()
        {
            try
            {
                this.gcHeader.DataSource = this._listTareasAll;
                this.gcHeader.RefreshDataSource();
                this.gcDetalle.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionTiempo", "LoadData"));
            }
        }

        /// <summary>
        /// Refrescar Formulario
        /// </summary>
        protected virtual void RefreshForm()
        {

            this.masterProyecto.Value = string.Empty;
            this.masterPrefijo.Value = string.Empty; 
            this.txtNro.Text = string.Empty;
            this.masterCliente.Value = string.Empty;
            this.txtLicitacion.Text = string.Empty;
            this.txtDescripcion.Text =string.Empty;      

            this._ctrlProyecto = null;
            this._numeroDoc = 0;
            this._proyectoDocu = new DTO_pyProyectoDocu();
            this._rowTarea = new DTO_pyProyectoTarea();
            this._listTareasAll = new List<DTO_pyProyectoTarea>();
            this._listRecursosXTareaAll = new List<DTO_pyProyectoDeta>();
            this.gcHeader.DataSource = this._listTareasAll;
            this.gcHeader.RefreshDataSource();

            this.gcDetalle.DataSource = null;
            this.gcDetalle.RefreshDataSource();

            this.masterProyecto.Focus();
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected virtual void SetInitParameters()
        {
            InitializeComponent();
            this.frmModule = ModulesPrefix.py;
            this.documentID = AppDocuments.Entregables;
            this.AddGridCols();
            this.InitControls();

            this.empresaID = _bc.AdministrationModel.Empresa.ID.Value;
            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;
        }

        /// <summary>
        /// Valida una fila
        /// </summary>
        /// <param name="fila">Indice de la fila a validar</param>
        /// <returns>Retorna si la info de la fila es valida o no</returns>
        protected virtual bool ValidateRow(int fila)
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
                FormProvider.Master.itemUpdate.Visible = true;
                FormProvider.Master.itemUpdate.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaProyectoForm", "Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaProyectoForm", "Form_Leave"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaProyectoForm", "Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaProyectoForm", "Form_FormClosed"));
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
        /// Verifica si hay un documento Existente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNro_Leave(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNro.Text) && !string.IsNullOrEmpty(this.masterPrefijo.Value))
            {
                int docNro = Convert.ToInt32(this.txtNro.Text);
                DTO_glDocumentoControl docCtrl = this._bc.AdministrationModel.glDocumentoControl_GetInternalDoc(AppDocuments.Proyecto, this.masterPrefijo.Value, docNro);
                if (docCtrl != null)
                    this.LoadData(this.masterPrefijo.Value, docNro, null, string.Empty);
            }
        }
 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdGroupVer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //if (this.rdGroupVer.SelectedIndex == 0)
                //{
                //    #region Muestras las Cantidades
                //    //Grilla Tareas
                //    this.gvTareas.Columns[this.unboundPrefix + "TareaCliente"].VisibleIndex = 1;
                //    this.gvTareas.Columns[this.unboundPrefix + "TareaDesc"].VisibleIndex = 2;
                //    this.gvTareas.Columns[this.unboundPrefix + "CantPresupuestado"].VisibleIndex = 3;
                //    this.gvTareas.Columns[this.unboundPrefix + "CantSolicitado"].VisibleIndex = 4;
                //    this.gvTareas.Columns[this.unboundPrefix + "CantComprado"].VisibleIndex = 5;
                //    this.gvTareas.Columns[this.unboundPrefix + "CantRecibido"].VisibleIndex = 6;
                //    this.gvTareas.Columns[this.unboundPrefix + "CantConsumido"].VisibleIndex = 7;
                //    this.gvTareas.Columns[this.unboundPrefix + "CantFacturado"].VisibleIndex = 8;

                //    this.gvTareas.Columns[this.unboundPrefix + "CantPresupuestado"].Visible = true;
                //    this.gvTareas.Columns[this.unboundPrefix + "CantSolicitado"].Visible = true;
                //    this.gvTareas.Columns[this.unboundPrefix + "CantComprado"].Visible = true;
                //    this.gvTareas.Columns[this.unboundPrefix + "CantRecibido"].Visible = true;
                //    this.gvTareas.Columns[this.unboundPrefix + "CantConsumido"].Visible = true;
                //    this.gvTareas.Columns[this.unboundPrefix + "CantFacturado"].Visible = true;

                //    this.gvTareas.Columns[this.unboundPrefix + "VlrPresupuestado"].Visible = false;
                //    this.gvTareas.Columns[this.unboundPrefix + "VlrSolicitado"].Visible = false;
                //    this.gvTareas.Columns[this.unboundPrefix + "VlrComprado"].Visible = false;
                //    this.gvTareas.Columns[this.unboundPrefix + "VlrRecibido"].Visible = false;
                //    this.gvTareas.Columns[this.unboundPrefix + "VlrConsumido"].Visible = false;
                //    this.gvTareas.Columns[this.unboundPrefix + "VlrFacturado"].Visible = false;

                //    //Grilla Recursos
                //    this.gvRecurso.Columns[this.unboundPrefix + "RecursoID"].VisibleIndex = 1;
                //    this.gvRecurso.Columns[this.unboundPrefix + "RecursoDesc"].VisibleIndex = 2;
                //    this.gvRecurso.Columns[this.unboundPrefix + "UnidadInvID"].VisibleIndex = 3;
                //    this.gvRecurso.Columns[this.unboundPrefix + "CantPresupuestado"].VisibleIndex = 4;
                //    this.gvRecurso.Columns[this.unboundPrefix + "CantSolicitado"].VisibleIndex = 5;
                //    this.gvRecurso.Columns[this.unboundPrefix + "CantComprado"].VisibleIndex = 6;
                //    this.gvRecurso.Columns[this.unboundPrefix + "CantRecibido"].VisibleIndex = 7;
                //    this.gvRecurso.Columns[this.unboundPrefix + "CantConsumido"].VisibleIndex = 8;
                //    this.gvRecurso.Columns[this.unboundPrefix + "CantFacturado"].VisibleIndex = 9;
                //    this.gvRecurso.Columns[this.unboundPrefix + "ViewDoc"].VisibleIndex = 10;

                //    this.gvRecurso.Columns[this.unboundPrefix + "CantPresupuestado"].Visible = true;
                //    this.gvRecurso.Columns[this.unboundPrefix + "CantSolicitado"].Visible = true;
                //    this.gvRecurso.Columns[this.unboundPrefix + "CantComprado"].Visible = true;
                //    this.gvRecurso.Columns[this.unboundPrefix + "CantRecibido"].Visible = true;
                //    this.gvRecurso.Columns[this.unboundPrefix + "CantConsumido"].Visible = true;
                //    this.gvRecurso.Columns[this.unboundPrefix + "CantFacturado"].Visible = true;
                //    //this.gvRecurso.Columns[this.unboundPrefix + "ViewDoc"].Visible = true;

                //    this.gvRecurso.Columns[this.unboundPrefix + "VlrPresupuestado"].Visible = false;
                //    this.gvRecurso.Columns[this.unboundPrefix + "VlrSolicitado"].Visible = false;
                //    this.gvRecurso.Columns[this.unboundPrefix + "VlrComprado"].Visible = false;
                //    this.gvRecurso.Columns[this.unboundPrefix + "VlrRecibido"].Visible = false;
                //    this.gvRecurso.Columns[this.unboundPrefix + "VlrConsumido"].Visible = false;
                //    this.gvRecurso.Columns[this.unboundPrefix + "VlrFacturado"].Visible = false;
                //    #endregion
                //}
                //else if (this.rdGroupVer.SelectedIndex == 1)
                //{
                //    #region Muestra Valores
                //    //Grilla Tareas
                //    this.gvTareas.Columns[this.unboundPrefix + "TareaCliente"].VisibleIndex = 1;
                //    this.gvTareas.Columns[this.unboundPrefix + "TareaDesc"].VisibleIndex = 2;
                //    this.gvTareas.Columns[this.unboundPrefix + "VlrPresupuestado"].VisibleIndex = 10;
                //    this.gvTareas.Columns[this.unboundPrefix + "VlrSolicitado"].VisibleIndex = 11;
                //    this.gvTareas.Columns[this.unboundPrefix + "VlrComprado"].VisibleIndex = 12;
                //    this.gvTareas.Columns[this.unboundPrefix + "VlrRecibido"].VisibleIndex = 13;
                //    this.gvTareas.Columns[this.unboundPrefix + "VlrConsumido"].VisibleIndex = 14;
                //    this.gvTareas.Columns[this.unboundPrefix + "VlrFacturado"].VisibleIndex = 15;

                //    this.gvTareas.Columns[this.unboundPrefix + "VlrPresupuestado"].Visible = true;
                //    this.gvTareas.Columns[this.unboundPrefix + "VlrSolicitado"].Visible = true;
                //    this.gvTareas.Columns[this.unboundPrefix + "VlrComprado"].Visible = true;
                //    this.gvTareas.Columns[this.unboundPrefix + "VlrRecibido"].Visible = true;
                //    this.gvTareas.Columns[this.unboundPrefix + "VlrConsumido"].Visible = true;
                //    this.gvTareas.Columns[this.unboundPrefix + "VlrFacturado"].Visible = true;

                //    this.gvTareas.Columns[this.unboundPrefix + "CantPresupuestado"].Visible = false;
                //    this.gvTareas.Columns[this.unboundPrefix + "CantSolicitado"].Visible = false;
                //    this.gvTareas.Columns[this.unboundPrefix + "CantComprado"].Visible = false;
                //    this.gvTareas.Columns[this.unboundPrefix + "CantRecibido"].Visible = false;
                //    this.gvTareas.Columns[this.unboundPrefix + "CantConsumido"].Visible = false;
                //    this.gvTareas.Columns[this.unboundPrefix + "CantFacturado"].Visible = false;

                //    //Grilla Recursos
                //    this.gvRecurso.Columns[this.unboundPrefix + "RecursoID"].VisibleIndex = 1;
                //    this.gvRecurso.Columns[this.unboundPrefix + "RecursoDesc"].VisibleIndex = 2;
                //    this.gvRecurso.Columns[this.unboundPrefix + "UnidadInvID"].VisibleIndex = 3;
                //    this.gvRecurso.Columns[this.unboundPrefix + "VlrPresupuestado"].VisibleIndex = 10;
                //    this.gvRecurso.Columns[this.unboundPrefix + "VlrSolicitado"].VisibleIndex = 11;
                //    this.gvRecurso.Columns[this.unboundPrefix + "VlrComprado"].VisibleIndex = 12;
                //    this.gvRecurso.Columns[this.unboundPrefix + "VlrRecibido"].VisibleIndex = 13;
                //    this.gvRecurso.Columns[this.unboundPrefix + "VlrConsumido"].VisibleIndex = 14;
                //    this.gvRecurso.Columns[this.unboundPrefix + "VlrFacturado"].VisibleIndex = 15;
                //    this.gvRecurso.Columns[this.unboundPrefix + "ViewDoc"].VisibleIndex = 16;

                //    this.gvRecurso.Columns[this.unboundPrefix + "VlrPresupuestado"].Visible = true;
                //    this.gvRecurso.Columns[this.unboundPrefix + "VlrSolicitado"].Visible = true;
                //    this.gvRecurso.Columns[this.unboundPrefix + "VlrComprado"].Visible = true;
                //    this.gvRecurso.Columns[this.unboundPrefix + "VlrRecibido"].Visible = true;
                //    this.gvRecurso.Columns[this.unboundPrefix + "VlrConsumido"].Visible = true;
                //    this.gvRecurso.Columns[this.unboundPrefix + "VlrFacturado"].Visible = true;
                //    //this.gvRecurso.Columns[this.unboundPrefix + "ViewDoc"].Visible = true;

                //    this.gvRecurso.Columns[this.unboundPrefix + "CantPresupuestado"].Visible = false;
                //    this.gvRecurso.Columns[this.unboundPrefix + "CantSolicitado"].Visible = false;
                //    this.gvRecurso.Columns[this.unboundPrefix + "CantComprado"].Visible = false;
                //    this.gvRecurso.Columns[this.unboundPrefix + "CantRecibido"].Visible = false;
                //    this.gvRecurso.Columns[this.unboundPrefix + "CantConsumido"].Visible = false;
                //    this.gvRecurso.Columns[this.unboundPrefix + "CantFacturado"].Visible = false;
                //    #endregion
                //}
                //else if (this.rdGroupVer.SelectedIndex == 2)
                //{
                //    #region Muestras las Cantidades y Valores
                //    //Grilla Tareas
                //    this.gvTareas.Columns[this.unboundPrefix + "TareaCliente"].VisibleIndex = 1;
                //    this.gvTareas.Columns[this.unboundPrefix + "TareaDesc"].VisibleIndex = 2;
                //    this.gvTareas.Columns[this.unboundPrefix + "CantPresupuestado"].VisibleIndex = 3;
                //    this.gvTareas.Columns[this.unboundPrefix + "CantSolicitado"].VisibleIndex = 4;
                //    this.gvTareas.Columns[this.unboundPrefix + "CantComprado"].VisibleIndex = 5;
                //    this.gvTareas.Columns[this.unboundPrefix + "CantRecibido"].VisibleIndex = 6;
                //    this.gvTareas.Columns[this.unboundPrefix + "CantConsumido"].VisibleIndex = 7;
                //    this.gvTareas.Columns[this.unboundPrefix + "CantFacturado"].VisibleIndex = 8;
                //    this.gvTareas.Columns[this.unboundPrefix + "VlrPresupuestado"].VisibleIndex = 9;
                //    this.gvTareas.Columns[this.unboundPrefix + "VlrSolicitado"].VisibleIndex = 10;
                //    this.gvTareas.Columns[this.unboundPrefix + "VlrComprado"].VisibleIndex = 11;
                //    this.gvTareas.Columns[this.unboundPrefix + "VlrRecibido"].VisibleIndex = 12;
                //    this.gvTareas.Columns[this.unboundPrefix + "VlrConsumido"].VisibleIndex = 13;
                //    this.gvTareas.Columns[this.unboundPrefix + "VlrFacturado"].VisibleIndex = 14;

                //    this.gvTareas.Columns[this.unboundPrefix + "CantPresupuestado"].Visible = true;
                //    this.gvTareas.Columns[this.unboundPrefix + "CantSolicitado"].Visible = true;
                //    this.gvTareas.Columns[this.unboundPrefix + "CantComprado"].Visible = true;
                //    this.gvTareas.Columns[this.unboundPrefix + "CantRecibido"].Visible = true;
                //    this.gvTareas.Columns[this.unboundPrefix + "CantConsumido"].Visible = true;
                //    this.gvTareas.Columns[this.unboundPrefix + "CantFacturado"].Visible = true;
                //    this.gvTareas.Columns[this.unboundPrefix + "VlrPresupuestado"].Visible = true;
                //    this.gvTareas.Columns[this.unboundPrefix + "VlrSolicitado"].Visible = true;
                //    this.gvTareas.Columns[this.unboundPrefix + "VlrComprado"].Visible = true;
                //    this.gvTareas.Columns[this.unboundPrefix + "VlrRecibido"].Visible = true;
                //    this.gvTareas.Columns[this.unboundPrefix + "VlrConsumido"].Visible = true;
                //    this.gvTareas.Columns[this.unboundPrefix + "VlrFacturado"].Visible = true;

                //    //Grilla Recursos
                //    this.gvRecurso.Columns[this.unboundPrefix + "RecursoID"].VisibleIndex = 1;
                //    this.gvRecurso.Columns[this.unboundPrefix + "RecursoDesc"].VisibleIndex = 2;
                //    this.gvRecurso.Columns[this.unboundPrefix + "UnidadInvID"].VisibleIndex = 3;
                //    this.gvRecurso.Columns[this.unboundPrefix + "CantPresupuestado"].VisibleIndex = 4;
                //    this.gvRecurso.Columns[this.unboundPrefix + "CantSolicitado"].VisibleIndex = 5;
                //    this.gvRecurso.Columns[this.unboundPrefix + "CantComprado"].VisibleIndex = 6;
                //    this.gvRecurso.Columns[this.unboundPrefix + "CantRecibido"].VisibleIndex = 7;
                //    this.gvRecurso.Columns[this.unboundPrefix + "CantConsumido"].VisibleIndex = 8;
                //    this.gvRecurso.Columns[this.unboundPrefix + "CantFacturado"].VisibleIndex = 9;
                //    this.gvRecurso.Columns[this.unboundPrefix + "VlrPresupuestado"].VisibleIndex = 10;
                //    this.gvRecurso.Columns[this.unboundPrefix + "VlrSolicitado"].VisibleIndex = 12;
                //    this.gvRecurso.Columns[this.unboundPrefix + "VlrComprado"].VisibleIndex = 12;
                //    this.gvRecurso.Columns[this.unboundPrefix + "VlrRecibido"].VisibleIndex = 13;
                //    this.gvRecurso.Columns[this.unboundPrefix + "VlrConsumido"].VisibleIndex = 14;
                //    this.gvRecurso.Columns[this.unboundPrefix + "VlrFacturado"].VisibleIndex = 15;
                //    this.gvRecurso.Columns[this.unboundPrefix + "ViewDoc"].VisibleIndex = 16;

                //    this.gvRecurso.Columns[this.unboundPrefix + "CantPresupuestado"].Visible = true;
                //    this.gvRecurso.Columns[this.unboundPrefix + "CantSolicitado"].Visible = true;
                //    this.gvRecurso.Columns[this.unboundPrefix + "CantComprado"].Visible = true;
                //    this.gvRecurso.Columns[this.unboundPrefix + "CantRecibido"].Visible = true;
                //    this.gvRecurso.Columns[this.unboundPrefix + "CantConsumido"].Visible = true;
                //    this.gvRecurso.Columns[this.unboundPrefix + "CantFacturado"].Visible = true;
                //    this.gvRecurso.Columns[this.unboundPrefix + "VlrPresupuestado"].Visible = true;
                //    this.gvRecurso.Columns[this.unboundPrefix + "VlrSolicitado"].Visible = true;
                //    this.gvRecurso.Columns[this.unboundPrefix + "VlrComprado"].Visible = true;
                //    this.gvRecurso.Columns[this.unboundPrefix + "VlrRecibido"].Visible = true;
                //    this.gvRecurso.Columns[this.unboundPrefix + "VlrConsumido"].Visible = true;
                //    this.gvRecurso.Columns[this.unboundPrefix + "VlrFacturado"].Visible = true;
                //    //this.gvRecurso.Columns[this.unboundPrefix + "ViewDoc"].Visible = true;

                //    #endregion
                //}
            }
            catch (Exception ex)
            {
                 MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaProyectoForm", "rdGroupVer_SelectedIndexChanged"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterProyecto_Leave(object sender, EventArgs e)
        {
            if (this.masterProyecto.ValidID)
            {
                this.LoadData(string.Empty, null, null, this.masterProyecto.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryDoc_Click(object sender, EventArgs e)
        {
            try
            {
                List<int> docs = new List<int>();
                docs.Add(AppDocuments.Proyecto);
                ModalFindDocSolicitud getDocControl = new ModalFindDocSolicitud(docs, false, true);
                getDocControl.ShowDialog();
                if (getDocControl.DocumentoControl != null)
                    this.LoadData(getDocControl.DocumentoControl.PrefijoID.Value, getDocControl.DocumentoControl.DocumentoNro.Value, null, string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Eventos Grilla

        #region Header

        /// <summary>
        /// Se ejecutar cuando se selecciona un registro de la Grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDocument_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle >= 0)
                {
                    this._rowTarea = (DTO_pyProyectoTarea)this.gvHeader.GetRow(e.FocusedRowHandle);
                    this.gcDetalle.DataSource = this._rowTarea.Detalle;
                    this.gcDetalle.RefreshDataSource();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaProyectoForm.cs", "gvDocument_FocusedRowChanged"));
            }
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (pi != null)
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" ||
                        pi.PropertyType.Name == "Double")
                        e.Value = pi.GetValue(dto, null);
                    else
                        e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                else
                {
                    FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (fi != null)
                    {
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" ||
                            pi.PropertyType.Name == "Double")
                            e.Value = fi.GetValue(dto);
                        else
                            e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                    }
                }
                if (e.Value == null && pi != null && pi.PropertyType.Name == "UDT_Cantidad")
                    e.Value = 0;
            }
            if (e.IsSetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (e.Value == null)
                    e.Value = string.Empty;
                if (pi != null)
                {
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" ||
                        pi.PropertyType.Name == "Double")
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
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" ||
                            pi.PropertyType.Name == "Double")
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
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (fieldName == "DiasAtraso" && e.RowHandle >= 0)
            {

                decimal cellvalue = Convert.ToDecimal(e.CellValue, CultureInfo.InvariantCulture);
                if (cellvalue > 0)
                    e.Appearance.ForeColor = Color.Red;
                else
                    e.Appearance.ForeColor = Color.Black;
            }
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
                DTO_pyProyectoTarea currentRow = (DTO_pyProyectoTarea)this.gvHeader.GetRow(e.RowHandle);
                if (currentRow != null)
                {
                    if (currentRow.DetalleInd.Value.Value)
                        e.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    else
                        e.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Detalle

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvRecurso_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle >= 0)
                    this._rowDetalle = (DTO_pyProyectoDeta)this.gvDetalle.GetRow(e.FocusedRowHandle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaProyectoForm.cs", "gvRecurso_FocusedRowChanged"));
            }
        }

        /// <summary>
        /// Asigna mascaras
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvRecurso_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == this.unboundPrefix + "TipoRecurso" && e.IsForGroupRow)
                {
                    //double rowValue = Convert.ToDouble(this.gvRecurso.GetGroupRowValue(e.GroupRowHandle, e.Column));
                    if (Convert.ToByte(e.Value) == 1)
                        e.DisplayText = "MATERIALES";
                    else if (Convert.ToByte(e.Value) == 2)
                        e.DisplayText = "EQUIPO-HERRAMIENTA";
                    else if (Convert.ToByte(e.Value) == 3)
                        e.DisplayText = "MANO DE OBRA";
                    else if (Convert.ToByte(e.Value) == 4)
                        e.DisplayText = "TRANSPORTES";
                    else if (Convert.ToByte(e.Value) == 5)
                        e.DisplayText = "HERRAMIENTA";
                    else if (Convert.ToByte(e.Value) == 6)
                        e.DisplayText = "SOFTWARE";
                }
                else if (e.Column.FieldName == this.unboundPrefix + "ViewDoc")
                    e.DisplayText = _bc.GetResource(LanguageTypes.Messages, "Ver Doc. Anexos");

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaProyectoForm.cs", "gvRecurso_CustomColumnDisplayText"));
            }
        }

        #endregion        

        /// <summary>
        /// Al entrar al link de la grilla para ver la descripcin de documento
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void editLink_Click(object sender, EventArgs e)
        {
            try
            {             
                //List<DTO_glDocumentoControl> ctrlsAnexos = this._bc.AdministrationModel.pyProyectoMvto_GetDocsAnexo(this._rowDetalle.ConsecMvto.Value);
                //ModalViewDocuments viewDocs = new ModalViewDocuments(ctrlsAnexos,Convert.ToByte(this.rdGroupVer.SelectedIndex));
                //viewDocs.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaProyectoForm.cs", "editLink_Click"));
            }
        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Salvar
        /// </summary>
        public override void TBSave()
        {
            this.gvHeader.PostEditor();
          
            Thread process = new Thread(this.SaveThread);
            process.Start();
        }

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
                this.LoadData(string.Empty, null, null, this.masterProyecto.Value, false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Guarda la información del proceso
        /// </summary>
        public void SaveThread()
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.NOK;

            try
            {
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoOpConjuntas(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                if (this._proyectoDocu.NumeroDoc.Value != null && this._proyectoDocu.NumeroDoc.Value != 0)
                    this._numeroDoc = this._proyectoDocu.NumeroDoc.Value.Value;


                FormProvider.Master.StopProgressBarThread(this.documentID);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudProye.cs", "SaveThread"));
            }
            finally
            {
                //if (result.Result != ResultValue.NOK)
                //    this.Invoke(this.saveDelegate);
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion   
    }
}