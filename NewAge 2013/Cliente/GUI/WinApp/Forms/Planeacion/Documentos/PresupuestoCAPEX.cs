using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.DTO.Resultados;
using NewAge.Librerias.Project;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class PresupuestoCAPEX : DocumentForm
    {
        #region Variables

        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        private string claseServicioDef;
        private string trabajoDef;
        private string etapaDef;
        private string lineaFlujoDef;
        private string listaPreciosDef;
        private string recursoDef;
        private string actividadEtapaDef;
        private string TareaDef;
        private string centroCostoDef;

        /// <summary>
        /// Variable de Carga de servicio
        /// </summary>
        private List<DTO_pySolServicio> _lista = new List<DTO_pySolServicio>();
        private DTO_pySolServicio _Servicio = new DTO_pySolServicio();
        private DTO_pyPreProyectoDocu _doc = new DTO_pyPreProyectoDocu();
        public DTO_glDocumentoControl _ctrl = null;
        private string _Actividad;
        
        // Variables Formulario
        private bool _prefijoFocus = false;
        private int _numeroDoc = 0;
        private bool _existe = false;

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Carga los parametros iniciales
        /// </summary>
        protected override void SetInitParameters()
        {
            base.SetInitParameters();
            InitializeComponent();
            this.frmModule = ModulesPrefix.pl;
            this.documentID = AppDocuments.PresupuestoCAPEX;
            base.SetInitParameters();
            this.AddGridCols();
            this.SetVariables();
            this.InitControls();
        }

        /// <summary>
        /// Adiciona las grillas a las columnas
        /// </summary>
        protected override void AddGridCols()
        {
            GridColumn recursoID = new GridColumn();
            recursoID.FieldName = this.unboundPrefix + "RecursoID";
            recursoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_RecursoID");
            recursoID.UnboundType = UnboundColumnType.String;
            recursoID.VisibleIndex = 0;
            recursoID.Width = 100;
            recursoID.Visible = true;
            recursoID.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(recursoID);

            GridColumn lineaPresupuestoID = new GridColumn();
            lineaPresupuestoID.FieldName = this.unboundPrefix + "LineaPresupuestoID";
            lineaPresupuestoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_LineaPresupuestoID");
            lineaPresupuestoID.UnboundType = UnboundColumnType.String;
            lineaPresupuestoID.VisibleIndex = 0;
            lineaPresupuestoID.Width = 100;
            lineaPresupuestoID.Visible = true;
            lineaPresupuestoID.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(lineaPresupuestoID);

            GridColumn codigoBSID = new GridColumn();
            codigoBSID.FieldName = this.unboundPrefix + "CodigoBSID";
            codigoBSID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CodigoBSID");
            codigoBSID.UnboundType = UnboundColumnType.String;
            codigoBSID.VisibleIndex = 0;
            codigoBSID.Width = 100;
            codigoBSID.Visible = true;
            codigoBSID.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(codigoBSID);

            GridColumn inReferenciaID = new GridColumn();
            inReferenciaID.FieldName = this.unboundPrefix + "inReferenciaID";
            inReferenciaID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_inReferenciaID");
            inReferenciaID.UnboundType = UnboundColumnType.String;
            inReferenciaID.VisibleIndex = 0;
            inReferenciaID.Width = 100;
            inReferenciaID.Visible = true;
            inReferenciaID.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(inReferenciaID);

            GridColumn proveedorID = new GridColumn();
            proveedorID.FieldName = this.unboundPrefix + "ProveedorID";
            proveedorID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProveedorID");
            proveedorID.UnboundType = UnboundColumnType.String;
            proveedorID.VisibleIndex = 0;
            proveedorID.Width = 100;
            proveedorID.Visible = true;
            proveedorID.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(proveedorID);

            GridColumn monedaID = new GridColumn();
            monedaID.FieldName = this.unboundPrefix + "MonedaID";
            monedaID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_MonedaID");
            monedaID.UnboundType = UnboundColumnType.String;
            monedaID.VisibleIndex = 0;
            monedaID.Width = 100;
            monedaID.Visible = true;
            monedaID.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(monedaID);

            GridColumn costoLocalEmp = new GridColumn();
            costoLocalEmp.FieldName = this.unboundPrefix + "CostoLocalEmp";
            costoLocalEmp.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CostoLocalEmp");
            costoLocalEmp.UnboundType = UnboundColumnType.String;
            costoLocalEmp.VisibleIndex = 0;
            costoLocalEmp.Width = 100;
            costoLocalEmp.Visible = true;
            costoLocalEmp.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(costoLocalEmp);

            GridColumn unidad = new GridColumn();
            unidad.FieldName = this.unboundPrefix + "Unidad";
            unidad.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Unidad");
            unidad.UnboundType = UnboundColumnType.String;
            unidad.VisibleIndex = 0;
            unidad.Width = 100;
            unidad.Visible = true;
            unidad.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(unidad);

            GridColumn porcentajeVariacion = new GridColumn();
            porcentajeVariacion.FieldName = this.unboundPrefix + "PorcentajeVariacion";
            porcentajeVariacion.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PorcentajeVariacion");
            porcentajeVariacion.UnboundType = UnboundColumnType.String;
            porcentajeVariacion.VisibleIndex = 0;
            porcentajeVariacion.Width = 100;
            porcentajeVariacion.Visible = true;
            porcentajeVariacion.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(porcentajeVariacion);

            GridColumn costoLocal = new GridColumn();
            costoLocal.FieldName = this.unboundPrefix + "CostoLocal";
            costoLocal.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CostoLocal");
            costoLocal.UnboundType = UnboundColumnType.String;
            costoLocal.VisibleIndex = 0;
            costoLocal.Width = 100;
            costoLocal.Visible = true;
            costoLocal.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(costoLocal);

            GridColumn cantidad = new GridColumn();
            cantidad.FieldName = this.unboundPrefix + "Cantidad";
            cantidad.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Cantidad");
            cantidad.UnboundType = UnboundColumnType.String;
            cantidad.VisibleIndex = 0;
            cantidad.Width = 100;
            cantidad.Visible = true;
            cantidad.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(cantidad);

            GridColumn costoTotLocal = new GridColumn();
            costoTotLocal.FieldName = this.unboundPrefix + "CostoTotLocal";
            costoTotLocal.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CostoTotLocal");
            costoTotLocal.UnboundType = UnboundColumnType.String;
            costoTotLocal.VisibleIndex = 0;
            costoTotLocal.Width = 100;
            costoTotLocal.Visible = true;
            costoTotLocal.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(costoTotLocal);

            GridColumn costoTotDolares = new GridColumn();
            costoTotDolares.FieldName = this.unboundPrefix + "CostoTotDolares";
            costoTotDolares.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CostoTotDolares");
            costoTotDolares.UnboundType = UnboundColumnType.String;
            costoTotDolares.VisibleIndex = 0;
            costoTotDolares.Width = 100;
            costoTotDolares.Visible = true;
            costoTotDolares.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(costoTotDolares);
        }

        #region Funciones Privadas

        //Inicializa los controles
        private void InitControls()
        {
            this._bc.InitMasterUC(this.uc_Proyecto, AppMasters.coProyecto, true, true, true, true);
            this._bc.InitMasterUC(this.uc_Contrato, AppMasters.pyContrato, true, true, true, false);
            this._bc.InitMasterUC(this.uc_Actividad, AppMasters.coActividad,true, true, true, true);
            this._bc.InitMasterUC(this.uc_Recurso, AppMasters.pyRecurso, true, true, true, false);
            this._bc.InitMasterUC(this.uc_LineaPresupuestal, AppMasters.plLineaPresupuesto, true, true, false, false);
            this._bc.InitMasterUC(this.uc_ProveedorFind, AppMasters.prProveedor, true, true, true, false);
            this._bc.InitMasterUC(this.uc_LineaPresupuestalFind, AppMasters.plLineaPresupuesto, true, true, false, false);
            this._bc.InitMasterUC(this.uc_CodigoBySFind, AppMasters.prBienServicio, true, true, true, false);
            this._bc.InitMasterUC(this.uc_ReferenciaFind, AppMasters.inReferencia, true, true, true, false);
            this._bc.InitMasterUC(this.uc_RecursoFind, AppMasters.pyRecurso, true, true, true, false);

            //Inhabilita los controles del Footer
            this.uc_LineaPresupuestalFind.EnableControl(false);
            this.uc_ProveedorFind.EnableControl(false);
            this.uc_RecursoFind.EnableControl(false);
            this.uc_ReferenciaFind.EnableControl(false);
            this.uc_CodigoBySFind.EnableControl(false);

            this.tlSeparatorPanel.RowStyles[0].Height = 120;
            this.tlSeparatorPanel.RowStyles[1].Height = 240;
            this.tlSeparatorPanel.RowStyles[2].Height = 120;
            
        }

        /// <summary>
        /// Setea kas variables por defecto
        /// </summary>
        private void SetVariables()
        {
            this.claseServicioDef = this._bc.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_ClaseActividadDefecto);
            this.trabajoDef = this._bc.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_TrabajoDefecto);
            this.etapaDef = this._bc.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_EtapaDefecto);
            this.lineaFlujoDef = this._bc.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_LineaFlujoDefecto);
            this.listaPreciosDef = this._bc.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_ListaPrecioDefecto);
            this.recursoDef = this._bc.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_RecursoDefecto);
            this.actividadEtapaDef = this._bc.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_ActividadEtapaDefecto);
            this.TareaDef = this._bc.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_TareaDefecto);
            this.centroCostoDef = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);

        }

        /// <summary>
        /// Setear Valores Documento 
        /// </summary>
        private void SetDocument()
        {
            this._doc = new DTO_pyPreProyectoDocu();
            this._doc.ClaseServicioID.Value = this.claseServicioDef;
            this._doc.ClienteID.Value = string.Empty; //TODO Validar
            this._doc.ContratoID.Value = this.uc_Contrato.Value;
            this._doc.EmpresaID.Value = this._bc.AdministrationModel.User.EmpresaID.Value;
            this._doc.EmpresaNombre.Value = this._bc.AdministrationModel.User.EmpresaDesc.Value;
            this._doc.NumeroDoc.Value = this._numeroDoc;
            //this._doc.ProyectoID.Value = this.uc_Proyecto.Value;
            this._doc.ResponsableCLI.Value = string.Empty; 
            this._doc.ResponsableCorreo.Value = string.Empty; 
            this._doc.ResponsableEMP.Value = this._bc.AdministrationModel.User.EmpresaID.Value; 
            this._doc.ResponsableTelefono.Value = string.Empty; 
            this._doc.TipoSolicitud.Value = 4; //Interna
        }

        
        /// <summary>
        /// Setea los datos en la lista 
        /// </summary>
        private void SetLista()
        {
            //Borra la lista 
            this._lista.Clear();
            this._Servicio.ClaseServicioID.Value = this.claseServicioDef;
            this._Servicio.LineaFlujoID.Value = this.lineaFlujoDef;
            this._Servicio.ActividadEtapaID.Value = this.etapaDef;
            this._Servicio.TareaID.Value = this.TareaDef;
            this._Servicio.TrabajoID.Value = this.trabajoDef;
            this._Servicio.SemanaPrograma.Value = 1; //TODO Validar
            this._Servicio.SemanaProgramaFin.Value = 1; //TODO Validar
            this._Servicio.CentroCostoID.Value = this.centroCostoDef; //TODO Validar
            this._Servicio.Observaciones.Value = string.Empty; //TODO Validar
        }
        

        #endregion

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Validar Campos Obligatorios
        /// </summary>
        /// <returns></returns>
        private string ValidaFields()
        {
            string camposObligatorios = string.Empty;
                       
            if (string.IsNullOrEmpty(this.uc_Proyecto.Value))
            {
                camposObligatorios = camposObligatorios + this.uc_Proyecto.LabelRsx + "\n";
            }         
            return camposObligatorios;
        }

        #endregion

        #region Eventos MDI

        /// <summary>
        /// Eventos de Inicio del Formulario
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
                FormProvider.Master.itemRevert.Visible = false;
                FormProvider.Master.itemGenerateTemplate.Visible = false;
                FormProvider.Master.itemFilter.Visible = false;
                FormProvider.Master.itemFilterDef.Visible = false;
                FormProvider.Master.itemSendtoAppr.Enabled = false;
                FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);
            }
        }

        #endregion

        #region Eventos Barra de Herramientas 

        /// <summary>
        /// Guarda  la información
        /// </summary>
        public override void TBSave()
        {
            this.SetDocument();
            string camposObligatorios = this.ValidaFields();
            if (string.IsNullOrEmpty(camposObligatorios))
            {                
                Thread process = new Thread(this.SaveThread);
                process.Start();               
            }
            else
                MessageBox.Show(string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Py_CamposObligatorios), camposObligatorios)); 
        }

        #endregion

        #region Eventos Header

        /// <summary>
        /// Evento que se desecadena al escoger al actividad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void uc_Actividad_Leave(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.uc_Actividad.Value))
            {
                this._Actividad = this.uc_Actividad.Value;
                var p = (DTO_plActividadLineaPresupuestal)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plActividadLineaPresupuestal, false, this._Actividad, true);
                if (p != null)
                {
                    var lineaPres = (DTO_plLineaPresupuesto)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plLineaPresupuesto, false, p.LineaPresupuestoID.Value, true);
                    if (lineaPres != null)
                    {
                        this.uc_LineaPresupuestal.Value = lineaPres.ID.Value;
                    }
                }
                else
                {
                    this._Actividad = string.Empty;
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Pl_ActividadLinea));
                }
            }
        }

        /// <summary>
        /// Carga el proyecto desde planeación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCalcular_Click(object sender, System.EventArgs e)
        {
            this.SetLista();

        }
       

        #endregion     

        #region Hilos 

        /// <summary>
        /// Guarda la información del proceso
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

                //result = this._bc.AdministrationModel.SolicitudTrabajo_Add(this.documentID,
                //    this._numeroDoc,
                //    this.claseServicioDef,
                //    this.areaFuncionalID,
                //    this.prefijoID,
                //    this.uc_Proyecto.Value,
                //    string.Empty,
                //    this.txtDescriptivo.Text,
                //    this._doc,
                //    null,null);


                if (result.Result == ResultValue.NOK)
                {
                    MessageForm frm = new MessageForm(result);
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                }
                else
                {
                    MessageBox.Show(string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Py_SuccessSol), this.prefijoID, result.ExtraField));

                }
                FormProvider.Master.StopProgressBarThread(this.documentID);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudProye.cs", "SaveThread"));
            }
            finally
            {
                this.Invoke(this.saveDelegate);
            }
        }

        #endregion 

     

    }
}
