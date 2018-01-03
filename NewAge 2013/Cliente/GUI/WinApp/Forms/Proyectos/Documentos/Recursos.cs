using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using NewAge.Librerias.Project;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using System.Reflection;
using NewAge.DTO.UDT;
using System.Threading;
using NewAge.DTO.Resultados;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class Recursos : DocumentAnalisisForm
    {
        #region Variables

        BaseController _bc = BaseController.GetInstance();
        List<DTO_pyPreProyectoDeta> lPersonal = new List<DTO_pyPreProyectoDeta>();
        List<DTO_pyPreProyectoDeta> lEquipos = new List<DTO_pyPreProyectoDeta>();
        List<DTO_pyPreProyectoDeta> lServicios = new List<DTO_pyPreProyectoDeta>();
        List<DTO_pyPreProyectoDeta> lInventarios = new List<DTO_pyPreProyectoDeta>();
        List<DTO_pyPreProyectoDeta> lUpdates = null;

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Initcialize System Parameters
        /// </summary>
        protected override void SetInitParameters()
        {
            InitializeComponent();
            this.frmModule = ModulesPrefix.py;
            this.documentID = 0;
            base.SetInitParameters();
            this.AddGridCols();
            this.InitControls();
        }

        /// <summary>
        /// Carga la informacion en las grillas
        /// </summary>
        /// <param name="firstTime"></param>
        protected override void LoadData(bool firstTime)
        {
            if (this._lista != null)
            {
                base.LoadData(firstTime);
                if (firstTime)
                {
                    this.CargarDatos();
                }
            }
        }       

        /// <summary>
        /// Add Columns in Grid
        /// </summary>
        protected override void AddGridCols()
        {
            base.AddGridCols();
            
            #region Personal
            this.tabPage1.Text = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_lblPersonal");
            GridColumn trabajoID = new GridColumn();
            trabajoID.FieldName = this.unboundPrefix + "TrabajoID";
            trabajoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TrabajoID");
            trabajoID.UnboundType = UnboundColumnType.String;
            trabajoID.VisibleIndex = 0;
            trabajoID.Width = 70;
            trabajoID.Visible = true;
            trabajoID.OptionsColumn.AllowEdit = false;
            this.gvPersonal.Columns.Add(trabajoID);

            GridColumn trabajoDesc = new GridColumn();
            trabajoDesc.FieldName = this.unboundPrefix + "TrabajoIDDesc";
            trabajoDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TrabajoIDDesc");
            trabajoDesc.UnboundType = UnboundColumnType.String;
            trabajoDesc.VisibleIndex = 0;
            trabajoDesc.Width = 200;
            trabajoDesc.Visible = true;
            trabajoDesc.OptionsColumn.AllowEdit = true;
            this.gvPersonal.Columns.Add(trabajoDesc);

            GridColumn unidadInvID = new GridColumn();
            unidadInvID.FieldName = this.unboundPrefix + "UnidadInvID";
            unidadInvID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_UnidadInvID");
            unidadInvID.UnboundType = UnboundColumnType.String;
            unidadInvID.VisibleIndex = 0;
            unidadInvID.Width = 100;
            unidadInvID.Visible = true;
            unidadInvID.OptionsColumn.AllowEdit = true;
            this.gvPersonal.Columns.Add(unidadInvID);   

            GridColumn cantidad = new GridColumn();
            cantidad.FieldName = this.unboundPrefix + "Cantidad";
            cantidad.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Cantidad");
            cantidad.UnboundType = UnboundColumnType.Integer;
            cantidad.VisibleIndex = 0;
            cantidad.Width = 100;
            cantidad.Visible = true;
            cantidad.OptionsColumn.AllowEdit = true;
            this.gvPersonal.Columns.Add(cantidad);

            GridColumn cargoEmpID = new GridColumn();
            cargoEmpID.FieldName = this.unboundPrefix + "CodigoBSID";
            cargoEmpID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CodigoBSID");
            cargoEmpID.UnboundType = UnboundColumnType.String;
            cargoEmpID.VisibleIndex = 0;
            cargoEmpID.Width = 70;
            cargoEmpID.Visible = true;
            cargoEmpID.OptionsColumn.AllowEdit = false;
            this.gvPersonal.Columns.Add(cargoEmpID);

            GridColumn cargoEmpDesc = new GridColumn();
            cargoEmpDesc.FieldName = this.unboundPrefix + "CodigoBSIDDesc";
            cargoEmpDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CodigoBSIDDesc");
            cargoEmpDesc.UnboundType = UnboundColumnType.String;
            cargoEmpDesc.VisibleIndex = 0;
            cargoEmpDesc.Width = 200;
            cargoEmpDesc.Visible = true;
            cargoEmpDesc.OptionsColumn.AllowEdit = false;
            this.gvPersonal.Columns.Add(cargoEmpDesc);            

            #endregion

            #region Servicios

            this.tabPage2.Text = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_lblServicios");
            GridColumn trabajoIDServ = new GridColumn();
            trabajoIDServ.FieldName = this.unboundPrefix + "TrabajoID";
            trabajoIDServ.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TrabajoID");
            trabajoIDServ.UnboundType = UnboundColumnType.String;
            trabajoIDServ.VisibleIndex = 0;
            trabajoIDServ.Width = 70;
            trabajoIDServ.Visible = true;
            trabajoIDServ.OptionsColumn.AllowEdit = false;
            this.gvServicios.Columns.Add(trabajoIDServ);

            GridColumn trabajoServDesc = new GridColumn();
            trabajoServDesc.FieldName = this.unboundPrefix + "TrabajoIDDesc";
            trabajoServDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TrabajoIDDesc");
            trabajoServDesc.UnboundType = UnboundColumnType.String;
            trabajoServDesc.VisibleIndex = 0;
            trabajoServDesc.Width = 200;
            trabajoServDesc.Visible = true;
            trabajoServDesc.OptionsColumn.AllowEdit = true;
            this.gvServicios.Columns.Add(trabajoServDesc);

            //Unidad InvID
            GridColumn unidadInvIDServ = new GridColumn();
            unidadInvIDServ.FieldName = this.unboundPrefix + "UnidadInvID";
            unidadInvIDServ.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_UnidadInvID");
            unidadInvIDServ.UnboundType = UnboundColumnType.String;
            unidadInvIDServ.VisibleIndex = 0;
            unidadInvIDServ.Width = 100;
            unidadInvIDServ.Visible = true;
            unidadInvIDServ.OptionsColumn.AllowEdit = true;
            this.gvServicios.Columns.Add(unidadInvIDServ);

            //Cantidad
            GridColumn cantidadServ = new GridColumn();
            cantidadServ.FieldName = this.unboundPrefix + "Cantidad";
            cantidadServ.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Cantidad");
            cantidadServ.UnboundType = UnboundColumnType.Integer;
            cantidadServ.VisibleIndex = 0;
            cantidadServ.Width = 100;
            cantidadServ.Visible = true;
            cantidadServ.OptionsColumn.AllowEdit = true;
            this.gvServicios.Columns.Add(cantidadServ);

            GridColumn codigoBSID = new GridColumn();
            codigoBSID.FieldName = this.unboundPrefix + "CodigoBSID";
            codigoBSID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CodigoBSID");
            codigoBSID.UnboundType = UnboundColumnType.String;
            codigoBSID.VisibleIndex = 0;
            codigoBSID.Width = 70;
            codigoBSID.Visible = true;
            codigoBSID.OptionsColumn.AllowEdit = false;
            this.gvServicios.Columns.Add(codigoBSID);

            GridColumn codigoBSIDDesc = new GridColumn();
            codigoBSIDDesc.FieldName = this.unboundPrefix + "CodigoBSIDDesc";
            codigoBSIDDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CodigoBSIDDesc");
            codigoBSIDDesc.UnboundType = UnboundColumnType.String;
            codigoBSIDDesc.VisibleIndex = 0;
            codigoBSIDDesc.Width = 200;
            codigoBSIDDesc.Visible = true;
            codigoBSIDDesc.OptionsColumn.AllowEdit = false;
            this.gvServicios.Columns.Add(codigoBSIDDesc);
          

            #endregion

            #region Alquiler
            this.tabPage3.Text = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_lblEquiposYDireridos");
            GridColumn trabajoIDAlq = new GridColumn();
            trabajoIDAlq.FieldName = this.unboundPrefix + "TrabajoID";
            trabajoIDAlq.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TrabajoID");
            trabajoIDAlq.UnboundType = UnboundColumnType.String;
            trabajoIDAlq.VisibleIndex = 0;
            trabajoIDAlq.Width = 70;
            trabajoIDAlq.Visible = true;
            trabajoIDAlq.OptionsColumn.AllowEdit = false;
            this.gvAlquiler.Columns.Add(trabajoIDAlq);

            GridColumn trabajoAlqDesc = new GridColumn();
            trabajoAlqDesc.FieldName = this.unboundPrefix + "TrabajoIDDesc";
            trabajoAlqDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TrabajoIDDesc");
            trabajoAlqDesc.UnboundType = UnboundColumnType.String;
            trabajoAlqDesc.VisibleIndex = 0;
            trabajoAlqDesc.Width = 200;
            trabajoAlqDesc.Visible = true;
            trabajoAlqDesc.OptionsColumn.AllowEdit = true;
            this.gvAlquiler.Columns.Add(trabajoAlqDesc);           
 

            GridColumn inReferenciaID = new GridColumn();
            inReferenciaID.FieldName = this.unboundPrefix + "inReferenciaID";
            inReferenciaID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_inReferenciaID");
            inReferenciaID.UnboundType = UnboundColumnType.String;
            inReferenciaID.VisibleIndex = 0;
            inReferenciaID.Width = 70;
            inReferenciaID.Visible = true;
            inReferenciaID.OptionsColumn.AllowEdit = false;
            this.gvAlquiler.Columns.Add(inReferenciaID);

            GridColumn inReferenciaIDDesc = new GridColumn();
            inReferenciaIDDesc.FieldName = this.unboundPrefix + "inReferenciaIDDesc";
            inReferenciaIDDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_inReferenciaIDDesc");
            inReferenciaIDDesc.UnboundType = UnboundColumnType.String;
            inReferenciaIDDesc.VisibleIndex = 0;
            inReferenciaIDDesc.Width = 200;
            inReferenciaIDDesc.Visible = true;
            inReferenciaIDDesc.OptionsColumn.AllowEdit = false;
            this.gvAlquiler.Columns.Add(inReferenciaIDDesc);

            //CodigoByS
            GridColumn codigoBSIDAlq = new GridColumn();
            codigoBSIDAlq.FieldName = this.unboundPrefix + "CodigoBSID";
            codigoBSIDAlq.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CodigoBSID");
            codigoBSIDAlq.UnboundType = UnboundColumnType.String;
            codigoBSIDAlq.VisibleIndex = 0;
            codigoBSIDAlq.Width = 70;
            codigoBSIDAlq.Visible = true;
            codigoBSIDAlq.OptionsColumn.AllowEdit = false;
            this.gvAlquiler.Columns.Add(codigoBSIDAlq);

            GridColumn codigoBSIDDescAlq = new GridColumn();
            codigoBSIDDescAlq.FieldName = this.unboundPrefix + "CodigoBSIDDesc";
            codigoBSIDDescAlq.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CodigoBSIDDesc");
            codigoBSIDDescAlq.UnboundType = UnboundColumnType.String;
            codigoBSIDDescAlq.VisibleIndex = 0;
            codigoBSIDDescAlq.Width = 200;
            codigoBSIDDescAlq.Visible = true;
            codigoBSIDDescAlq.OptionsColumn.AllowEdit = false;
            this.gvAlquiler.Columns.Add(codigoBSIDDescAlq);         

            //Cantidad
            GridColumn cantidadAlq = new GridColumn();
            cantidadAlq.FieldName = this.unboundPrefix + "Cantidad";
            cantidadAlq.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Cantidad");
            cantidadAlq.UnboundType = UnboundColumnType.Integer;
            cantidadAlq.VisibleIndex = 0;
            cantidadAlq.Width = 100;
            cantidadAlq.Visible = true;
            cantidadAlq.OptionsColumn.AllowEdit = true;
            this.gvAlquiler.Columns.Add(cantidadAlq);

            #endregion

            #region Inventarios

            this.tabPage4.Text = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_lblInventarios");
            GridColumn trabajoIDInv = new GridColumn();
            trabajoIDInv.FieldName = this.unboundPrefix + "TrabajoID";
            trabajoIDInv.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TrabajoID");
            trabajoIDInv.UnboundType = UnboundColumnType.String;
            trabajoIDInv.VisibleIndex = 0;
            trabajoIDInv.Width = 70;
            trabajoIDInv.Visible = true;
            trabajoIDInv.OptionsColumn.AllowEdit = false;
            this.gvInventarios.Columns.Add(trabajoIDInv);

            GridColumn trabajoInvDesc = new GridColumn();
            trabajoInvDesc.FieldName = this.unboundPrefix + "TrabajoIDDesc";
            trabajoInvDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TrabajoIDDesc");
            trabajoInvDesc.UnboundType = UnboundColumnType.String;
            trabajoInvDesc.VisibleIndex = 0;
            trabajoInvDesc.Width = 200;
            trabajoInvDesc.Visible = true;
            trabajoInvDesc.OptionsColumn.AllowEdit = true;
            this.gvInventarios.Columns.Add(trabajoInvDesc);       


            //Inventarios
            GridColumn inReferenciaIDInv = new GridColumn();
            inReferenciaIDInv.FieldName = this.unboundPrefix + "inReferenciaID";
            inReferenciaIDInv.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_inReferenciaID");
            inReferenciaIDInv.UnboundType = UnboundColumnType.String;
            inReferenciaIDInv.VisibleIndex = 0;
            inReferenciaIDInv.Width = 70;
            inReferenciaIDInv.Visible = true;
            inReferenciaIDInv.OptionsColumn.AllowEdit = false;
            this.gvInventarios.Columns.Add(inReferenciaIDInv);

            GridColumn inReferenciaIDDescInv = new GridColumn();
            inReferenciaIDDescInv.FieldName = this.unboundPrefix + "inReferenciaIDDesc";
            inReferenciaIDDescInv.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_inReferenciaIDDesc");
            inReferenciaIDDescInv.UnboundType = UnboundColumnType.String;
            inReferenciaIDDescInv.VisibleIndex = 0;
            inReferenciaIDDescInv.Width = 200;
            inReferenciaIDDescInv.Visible = true;
            inReferenciaIDDescInv.OptionsColumn.AllowEdit = false;
            this.gvInventarios.Columns.Add(inReferenciaIDDescInv);

            GridColumn unidadInvIDInv = new GridColumn();
            unidadInvIDInv.FieldName = this.unboundPrefix + "UnidadInvID";
            unidadInvIDInv.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_UnidadInvID");
            unidadInvIDInv.UnboundType = UnboundColumnType.String;
            unidadInvIDInv.VisibleIndex = 0;
            unidadInvIDInv.Width = 100;
            unidadInvIDInv.Visible = true;
            unidadInvIDInv.OptionsColumn.AllowEdit = true;
            this.gvInventarios.Columns.Add(unidadInvIDInv);   
            
            //Cantidad
            GridColumn cantidadInv= new GridColumn();
            cantidadInv.FieldName = this.unboundPrefix + "Cantidad";
            cantidadInv.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Cantidad");
            cantidadInv.UnboundType = UnboundColumnType.Integer;
            cantidadInv.VisibleIndex = 0;
            cantidadInv.Width = 100;
            cantidadInv.Visible = true;
            cantidadInv.OptionsColumn.AllowEdit = true;
            this.gvInventarios.Columns.Add(cantidadInv);

            #endregion
        }

        #endregion

        #region Funciones Privadas

        private void InitControls()
        {
            //this.tlSeparatorPanel.RowStyles[0].Height = 20;
            //Deshabilita los botones +- de la grilla
            this.gcDocument.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.gcDocument.EmbeddedNavigator.Buttons.CustomButtons[0].Enabled = false;
        }

        /// <summary>
        /// Recarga los datos en las grillas
        /// </summary>
        private void CargarDatos()
        { 
            //Filtra por linea Flujo
            List<DTO_pySolServicio> data = this._lista.Where(x => x.LineaFlujoID.Value == this._lineaFlujoID).ToList();
            if(!string.IsNullOrEmpty(this._EtapaID))
                data = data.Where(x => x.ActividadEtapaID.Value == this._EtapaID).ToList();

            if(!string.IsNullOrEmpty(this._TareaID))
                data = data.Where(x => x.TareaID.Value == this._TareaID).ToList();

            if (!string.IsNullOrEmpty(this._TrabajoID))
                data = data.Where(x => x.TrabajoID.Value == this._TrabajoID).ToList();

            this.lPersonal.Clear();
            this.lServicios.Clear();
            this.lInventarios.Clear();
            this.lEquipos.Clear();

            foreach (var serv in data)
            {                
                foreach (var item in serv.Detalle)
                {
                    item.TrabajoID.Value = serv.TrabajoID.Value;
                    item.TrabajoDesc.Value = serv.TrabajoIDDesc.Value;

                    DTO_pyRecurso recurso = (DTO_pyRecurso)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyRecurso, false, item.RecursoID.Value, false);
                    if (recurso != null)
                    {
                        #region Clase de Bien y Servicio

                        switch (recurso.TipoRecurso.Value)
                        {
                            case 1:
                                {
                                    this.lPersonal.Add(item);
                                    break;
                                }
                            case 2:
                                {
                                    this.lServicios.Add(item);
                                    break;
                                }
                            case 3:
                                {
                                    this.lInventarios.Add(item);
                                    break;
                                }
                            case 4:
                                {
                                    this.lEquipos.Add(item);
                                    break;
                                }
                           
                        }

                        #endregion
                    }                
                }               

            }

            this.gcPersonal.DataSource = this.lPersonal;
            this.gcPersonal.RefreshDataSource();
            this.gcServicios.DataSource = this.lServicios;
            this.gcServicios.RefreshDataSource();
            this.gcInvenitarios.DataSource = this.lInventarios;
            this.gcInvenitarios.RefreshDataSource();
            this.gcAlquiler.DataSource = this.lEquipos;
            this.gcAlquiler.RefreshDataSource();
        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Evento boton Guardar barra de herramientas
        /// </summary>
        public override void TBSave()
        {
            base.TBSave();
            //lUpdates = this._lista.Where(x => x.IndUpdate).ToList();
            if (lUpdates != null && lUpdates.Count > 0)
            {
                Thread process = new Thread(this.SaveThread);
                process.Start();
            }
            else
            {
                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Py_NotChangesSS));
            }

        }


        /// <summary>
        /// Enviar a aprobación
        /// </summary>
        public override void TBSendtoAppr()
        {
            Thread process = new Thread(this.SendToApproveThread);
            process.Start();
        }

        #endregion

        #region Eventos MDI

        /// <summary>
        /// Eventos del Formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Form_Enter(object sender, System.EventArgs e)
        {
            base.Form_Enter(sender, e);
            if (FormProvider.Master.LoadFormTB)
            {
                FormProvider.Master.itemDelete.Visible = false;
                FormProvider.Master.itemPrint.Visible = false;
                FormProvider.Master.itemCopy.Visible = false;
                FormProvider.Master.itemPaste.Visible = false;
                FormProvider.Master.itemImport.Visible = false;
                FormProvider.Master.itemExport.Visible = false;
                FormProvider.Master.itemRevert.Visible = false;
                FormProvider.Master.itemGenerateTemplate.Visible = false;
                FormProvider.Master.itemFilter.Visible = false;
                FormProvider.Master.itemFilterDef.Visible = false;

                FormProvider.Master.itemSendtoAppr.Enabled = false;
                FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);
            }
        }

        #endregion

        #region Eventos Header
       
        /// <summary>
        /// Evento de busquea de documento interno
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void txtNro_Leave(object sender, EventArgs e)
        {
            base.txtNro_Leave(sender, e);
            if (!string.IsNullOrEmpty(this.txtNro.Text))
            {
                this.LoadData(true);
                FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr);
            }
        }

        #endregion

        #region Eventos  Grillas Base

        /// <summary>
        /// Evento de seleccion grilla de Flujos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocument_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            base.gvDocument_FocusedRowChanged(sender, e);
            DTO_pyTareas _TareaFlujo = null;
            this._lTareas.Clear();

            var tareas = (from l in this._lista.AsQueryable()
                          where l.LineaFlujoID.Value == this._lineaFlujoID
                          select new
                          {
                              TareaID = l.TareaID.Value,
                              TareaIDDesc = l.TareaIDDesc.Value
                          }
                         ).Distinct().ToList();

            foreach (var item in tareas)
            {
                _TareaFlujo = new DTO_pyTareas();
                _TareaFlujo.TareaID = item.TareaID;
                _TareaFlujo.TareaIDDesc = item.TareaIDDesc;

                var trabajos = (from et in this._lista.AsEnumerable()
                              where et.LineaFlujoID.Value == this._lineaFlujoID
                              && et.TareaID.Value == item.TareaID
                              select new
                              {
                                  TrabajoID = et.TrabajoID.Value,
                                  TrabajoIDDesc = et.TrabajoIDDesc.Value
                              }
                               ).Distinct().ToList();

                _TareaFlujo.LTrabajos = (from et in trabajos.AsQueryable()
                                       select new DTO_pyTrabajos()
                                       {
                                           TrabajoID = et.TrabajoID,
                                           TrabajoIDDesc = et.TrabajoIDDesc
                                       }
                                      ).ToList();

                this._lTareas.Add(_TareaFlujo);
            }

            this.gcTareas.DataSource = this._lTareas;
            this.gcTareas.RefreshDataSource();

            this.CargarDatos();
        }

        /// <summary>
        /// Evento de seleccion grilla detalle Flujos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDetalle_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            base.gvDetalle_FocusedRowChanged(sender, e);
            this.CargarDatos();
        }

        /// <summary>
        /// Evento de seleccion grilla de Tareas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvTareas_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            base.gvTareas_FocusedRowChanged(sender, e);
            this.CargarDatos();
        }

        /// <summary>
        /// Evento de seleccion grilla Detalle Tareas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvEtapasTareas_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            base.gvEtapasTareas_FocusedRowChanged(sender, e);
            this.CargarDatos();
        }

        #endregion

        #region Eventos Grilla 

        /// <summary>
        /// Formato columnas Grilla Alquiler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAlquiler_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                if (fieldName == "Marca" && e.Value == null)
                    e.Value = this.select.Contains(e.ListSourceRowIndex);
                else
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
            }
            if (e.IsSetData)
            {
                if (fieldName == "Marca")
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
        }

        /// <summary>
        /// Formato columnas Grilla Personal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvPersonal_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                if (fieldName == "Marca" && e.Value == null)
                    e.Value = this.select.Contains(e.ListSourceRowIndex);
                else
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
            }
            if (e.IsSetData)
            {
                if (fieldName == "Marca")
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
        }

        /// <summary>
        /// Formato columnas Grilla Servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServicios_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            try
            {

                Object dto = (Object)e.Row;
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

                if (e.IsGetData)
                {
                    if (fieldName == "Marca" && e.Value == null)
                        e.Value = this.select.Contains(e.ListSourceRowIndex);
                    else
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
                }
                if (e.IsSetData)
                {
                    if (fieldName == "Marca")
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Formato columnas Grilla Inventarios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvInventarios_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            try
            {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                if (fieldName == "Marca" && e.Value == null)
                    e.Value = this.select.Contains(e.ListSourceRowIndex);
                else
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
            }
            if (e.IsSetData)
            {
                if (fieldName == "Marca")
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Edita los valores de las celdas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvPersonal_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            GridColumn col = this.gvPersonal.Columns[this.unboundPrefix + fieldName];

            if (fieldName == "Cantidad")
            {
                decimal val = (decimal)this.gvPersonal.GetRowCellValue(e.RowHandle, col);
                //this._lista[this.gvPersonal.FocusedRowHandle].Cantidad.Value = val;
                this._lista[this.gvPersonal.FocusedRowHandle].IndUpdate = true;
            }
        }

        /// <summary>
        /// Edita los valores de las celdas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAlquiler_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            GridColumn col = this.gvAlquiler.Columns[this.unboundPrefix + fieldName];

            if (fieldName == "Cantidad")
            {
                decimal val = (decimal)this.gvAlquiler.GetRowCellValue(e.RowHandle, col);
                //this._lista[this.gvAlquiler.FocusedRowHandle].Cantidad.Value = val;
                this._lista[this.gvAlquiler.FocusedRowHandle].IndUpdate = true;
            }
        }

        /// <summary>
        /// Edita los valores de las celdas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServicios_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            GridColumn col = this.gvServicios.Columns[this.unboundPrefix + fieldName];

            if (fieldName == "Cantidad")
            {
                decimal val = (decimal)this.gvServicios.GetRowCellValue(e.RowHandle, col);
                //this._lista[this.gvServicios.FocusedRowHandle].Cantidad.Value = val;
                this._lista[this.gvServicios.FocusedRowHandle].IndUpdate = true;
            }
        }

        /// <summary>
        /// Edita los valores de las celdas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvInventarios_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            GridColumn col = this.gvInventarios.Columns[this.unboundPrefix + fieldName];

            if (fieldName == "Cantidad")
            {
                decimal val = (decimal)this.gvInventarios.GetRowCellValue(e.RowHandle, col);
                //this._lista[this.gvInventarios.FocusedRowHandle].Cantidad.Value = val;
                this._lista[this.gvInventarios.FocusedRowHandle].IndUpdate = true;
            }
        }


       
        #endregion  

        #region Hilos

        /// <summary>
        /// Guarda los datos actuales
        /// </summary>
        public override void SaveThread()
        {
            try
            {
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoOpConjuntas(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);
                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.NOK;

                //result = this._bc.AdministrationModel.pyServicioDeta_Upd(this.lUpdates);

                MessageForm frm = new MessageForm(result);
                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                FormProvider.Master.StopProgressBarThread(this.documentID);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Costos.cs", "SaveThread"));
            }
            finally
            {
                this.Invoke(this.saveDelegate);
            }
        }

        /// <summary>
        /// Enviar Aprobación
        /// </summary>
        public override void SendToApproveThread()
        {
            try
            {
                DTO_TxResult resultNOK = new DTO_TxResult();
                resultNOK.Result = ResultValue.NOK;

                this.gvDocument.ActiveFilterString = string.Empty;

                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoOpConjuntas(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);
                DTO_TxResult result;

                result = this._bc.AdministrationModel.pySolServicioDocu_AsignaActividad(this.documentID, this._ctrl.NumeroDoc.Value.Value, this._actFlujo.ID.Value);

                FormProvider.Master.StopProgressBarThread(this.documentID);
                bool isOK = _bc.SendDocumentMail(MailType.SendToApprove, this.documentID, this._actFlujo.seUsuarioID.Value, result, false, true);
                if (isOK)
                {
                    _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);

                    this.newDoc = true;
                    this.deleteOP = true;
                    this.Invoke(this.sendToApproveDelegate);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "Tiempo.cs-SendToApproveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }

        }

        #endregion

        private void gcPersonal_Click(object sender, EventArgs e)
        {

        }

      }
}
