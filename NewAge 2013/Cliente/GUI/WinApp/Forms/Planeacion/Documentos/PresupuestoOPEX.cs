using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class PresupuestoOPEX : DocumentForm
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

        /// <summary>
        /// Variable de Carga de servicio
        /// </summary>
        private DTO_pySolServicio _Servicio = new DTO_pySolServicio();
        private DTO_pyPreProyectoDocu _Docu = new DTO_pyPreProyectoDocu();
        private DTO_glDocumentoControl _Control = new DTO_glDocumentoControl();

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
            this.documentID = AppDocuments.PresupuestoOPEX;
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

            GridColumn monedaID = new GridColumn();
            monedaID.FieldName = this.unboundPrefix + "MonedaID";
            monedaID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_MonedaID");
            monedaID.UnboundType = UnboundColumnType.String;
            monedaID.VisibleIndex = 0;
            monedaID.Width = 100;
            monedaID.Visible = true;
            monedaID.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(monedaID);

            GridColumn costoLocal = new GridColumn();
            costoLocal.FieldName = this.unboundPrefix + "CostoLocal";
            costoLocal.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CostoLocal");
            costoLocal.UnboundType = UnboundColumnType.String;
            costoLocal.VisibleIndex = 0;
            costoLocal.Width = 100;
            costoLocal.Visible = true;
            costoLocal.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(costoLocal);

            GridColumn unidad = new GridColumn();
            unidad.FieldName = this.unboundPrefix + "Unidad";
            unidad.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Unidad");
            unidad.UnboundType = UnboundColumnType.String;
            unidad.VisibleIndex = 0;
            unidad.Width = 100;
            unidad.Visible = true;
            unidad.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(unidad);

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
            this._bc.InitMasterUC(this.uc_Proyecto, AppMasters.coProyecto, true, true, true, false);
            this._bc.InitMasterUC(this.uc_Contrato, AppMasters.pyContrato, true, true, true, false);
            this._bc.InitMasterUC(this.uc_Actividad, AppMasters.pyClaseProyecto, true, true, true, false);
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

            this.tlSeparatorPanel.RowStyles[0].Height = 90;
            this.tlSeparatorPanel.RowStyles[1].Height = 270;
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

            this._Servicio.ClaseServicioID.Value = this.claseServicioDef;
            this._Servicio.LineaFlujoID.Value = this.lineaFlujoDef;
            this._Servicio.ActividadEtapaID.Value = this.etapaDef;
            this._Servicio.TareaID.Value = this.TareaDef;
            this._Servicio.TrabajoID.Value = this.trabajoDef;

        }


        #endregion

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
    }
}
