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
    public partial class ConsultaReferencia : FormWithToolbar
    {
        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        //Variables basicas
        private FormTypes _frmType = FormTypes.Query;
        private string _frmName;
        private int _documentID;
        private ModulesPrefix _frmModule;
        private string _unboundPrefix = "Unbound_";
        private int _pageSize = 50;
        private bool _filterActive = false;
        //Variables de data
        private List<DTO_inBodega> _listBodega = null;
        private List<DTO_inReferencia> _listReferenciaAll = null;
        private List<DTO_inReferencia> _listRefSelected = null;
        private List<DTO_inReferencia> _listRefFiltered = null;
        private List<DTO_inControlSaldosCostos> listSaldosCostos = null;
        private DTO_seUsuarioBodega _userBodega = null;
        private DTO_inBodega _bodegaCurrent = null;
        #endregion

        ///<summary>
        /// Constructor 
        /// </summary>
        public ConsultaReferencia()
        {
            InitializeComponent();
            try
            {
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());
                FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter,this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
                _bc.Pagging_Init(this.pgGrid, _pageSize);
                _bc.Pagging_SetEvent(this.pgGrid, this.pagging_Click);
                this.AddGridColsBodega();
                this.AddGridColsReferencia();
                this.LoadGridData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaReferencia.cs", "ConsultaReferencia: " + ex.Message));
            }
        }

        #region Funciones privadas

        /// <summary>
        /// Carga los datos de la grilla
        /// </summary>
        /// <param name="firstTime">Si es la primera vez que carga la grilla</param>
        /// <param name="firstPage">Si debe ir a la primera página</param>
        /// <param name="lastPage">Si debe ir a la ultima página</param>
        private void LoadGridData()
        {
            try
            {
                #region Carga las bodega existentes
                this._listBodega = new List<DTO_inBodega>();
                long count = this._bc.AdministrationModel.MasterSimple_Count(AppMasters.inBodega, null, null, true);
                bool hasItems = count > 0 ? true : false;
                this._listBodega = this._bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.inBodega, count, 1, null, null, true).Cast<DTO_inBodega>().ToList();
                #endregion

                if ( this._listBodega.Count > 0)
                {
                    #region Trae todas las referencias
                    this._listReferenciaAll = new List<DTO_inReferencia>();
                    count = this._bc.AdministrationModel.MasterSimple_Count(AppMasters.inReferencia, null, null, true);
                    //Aca va la nueva consulta para traer las referencias en glMovimientoDeta
                    this._listReferenciaAll = this._bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.inReferencia, count, 1, null, null, true).Cast<DTO_inReferencia>().ToList();
                    this._listReferenciaAll = this._listReferenciaAll.OrderBy(x => x.Descriptivo.Value).ToList();
                    this.pgGrid.UpdatePageNumber(count, true, false, false);
                    #endregion
                }
                this.gcBodega.DataSource = this._listBodega;

                if (hasItems)
                    this.gvBodega.MoveFirst();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaReferencia.cs", "ConsultaReferencia(" + this._documentID + ")-LoadGridData: " + ex.Message));
            }
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            this._documentID = AppQueries.QueryReferencia;
            this._frmModule = ModulesPrefix.@in;
            
            #region Inicializa Controles
            _bc.InitMasterUC(this.masterGrupoInv, AppMasters.inRefGrupo, true, false, false, false);
            _bc.InitMasterUC(this.masterClaseInv, AppMasters.inRefClase, true, false, false, false);
            _bc.InitMasterUC(this.masterTipoInv, AppMasters.inRefTipo, true, false, false, false);
            _bc.InitMasterUC(this.masterSerieInv, AppMasters.inSerie, true, false, false, false);
            _bc.InitMasterUC(this.masterMaterialInv, AppMasters.inMaterial, true, false, false, false);
            _bc.InitMasterUC(this.masterMarcaInv, AppMasters.inMarca, true, false, false, false);
            _bc.InitMasterUC(this.masterReferencia, AppMasters.inReferencia, true, false, false, false);
            _bc.InitMasterUC(this.masterUnidadInv, AppMasters.inUnidad, true, false, false, false);
            _bc.InitMasterUC(this.masterEmpaqueInv, AppMasters.inEmpaque, true, false, false, false);
            _bc.InitMasterUC(this.masterParam1, AppMasters.inRefParametro1, true, false, false, false);
            _bc.InitMasterUC(this.masterParam2, AppMasters.inRefParametro2, true, false, false, false); 
            #endregion
            #region Deshabilita controles
            this.masterGrupoInv.EnableControl(false);
            this.masterClaseInv.EnableControl(false);
            this.masterTipoInv.EnableControl(false);
            this.masterSerieInv.EnableControl(false);
            this.masterMaterialInv.EnableControl(false);
            this.masterMarcaInv.EnableControl(false);
            this.masterReferencia.EnableControl(false);
            this.masterUnidadInv.EnableControl(false);
            this.masterEmpaqueInv.EnableControl(false);
            this.masterParam1.EnableControl(false);
            this.masterParam2.EnableControl(false); 
            #endregion
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddGridColsBodega()
        {
            GridColumn bodega = new GridColumn();
            bodega.FieldName = this._unboundPrefix + "ID";
            bodega.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_BodegaID");
            bodega.UnboundType = UnboundColumnType.String;
            bodega.VisibleIndex = 0;
            bodega.Width = 120;
            bodega.Visible = true;
            this.gvBodega.Columns.Add(bodega);

            GridColumn descriptivo = new GridColumn();
            descriptivo.FieldName = this._unboundPrefix + "Descriptivo";
            descriptivo.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Descriptivo");
            descriptivo.UnboundType = UnboundColumnType.String;
            descriptivo.VisibleIndex = 1;
            descriptivo.Width = 250;
            descriptivo.Visible = true;
            this.gvBodega.Columns.Add(descriptivo);

            GridColumn ProyectoID = new GridColumn();
            ProyectoID.FieldName = this._unboundPrefix + "ProyectoID";
            ProyectoID.Caption = _bc.GetResource(LanguageTypes.Forms, "Proyecto");
            ProyectoID.UnboundType = UnboundColumnType.String;
            ProyectoID.VisibleIndex = 2;
            ProyectoID.Width = 80;
            ProyectoID.Visible = true;
            this.gvBodega.Columns.Add(ProyectoID);

            GridColumn ProyectoDesc = new GridColumn();
            ProyectoDesc.FieldName = this._unboundPrefix + "ProyectoDesc";
            ProyectoDesc.Caption = _bc.GetResource(LanguageTypes.Forms,"Proyecto Desc");
            ProyectoDesc.UnboundType = UnboundColumnType.String;
            ProyectoDesc.VisibleIndex = 3;
            ProyectoDesc.Width = 150;
            ProyectoDesc.Visible = true;
            this.gvBodega.Columns.Add(ProyectoDesc);
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private  void AddGridColsReferencia()
        {
            GridColumn referencia = new GridColumn();
            referencia.FieldName = this._unboundPrefix + "ID";
            referencia.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_inReferenciaID");
            referencia.UnboundType = UnboundColumnType.String;
            referencia.VisibleIndex = 0;
            referencia.Width = 100;
            referencia.Visible = true;
            referencia.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvDetail.Columns.Add(referencia);

            GridColumn descriptivo = new GridColumn();
            descriptivo.FieldName = this._unboundPrefix + "Descriptivo";
            descriptivo.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Descriptivo");
            descriptivo.UnboundType = UnboundColumnType.String;
            descriptivo.VisibleIndex = 1;
            descriptivo.Width = 250;
            descriptivo.Visible = true;
            descriptivo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            descriptivo.AppearanceCell.Options.UseTextOptions = true;
            this.gvDetail.Columns.Add(descriptivo);

            GridColumn saldo = new GridColumn();
            saldo.FieldName = this._unboundPrefix + "SaldoExistencia";
            saldo.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoExistencia");
            saldo.UnboundType = UnboundColumnType.Decimal;
            saldo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            saldo.AppearanceCell.Options.UseTextOptions = true;
            saldo.VisibleIndex = 2;
            saldo.Width = 120;
            saldo.Visible = true;
            saldo.ColumnEdit = this.editValue;
            saldo.OptionsColumn.AllowEdit = false;
            this.gvDetail.Columns.Add(saldo);

            GridColumn ubicacion = new GridColumn();
            ubicacion.FieldName = this._unboundPrefix + "UbicacionID";
            ubicacion.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_UbicacionID");
            ubicacion.UnboundType = UnboundColumnType.String;
            ubicacion.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            ubicacion.AppearanceCell.Options.UseTextOptions = true;
            ubicacion.VisibleIndex = 2;
            ubicacion.Width = 65;
            ubicacion.Visible = true;
            ubicacion.OptionsColumn.AllowEdit = false;
            this.gvDetail.Columns.Add(ubicacion);

            GridColumn viewDetail = new GridColumn();
            viewDetail.FieldName = this._unboundPrefix + "VerDetalle";
            viewDetail.VisibleIndex = 3;
            viewDetail.Width = 120;
            viewDetail.Visible = true;
            viewDetail.OptionsColumn.ShowCaption = false;
            viewDetail.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvDetail.Columns.Add(viewDetail);

            GridColumn viewMov = new GridColumn();
            viewMov.FieldName = this._unboundPrefix + "VerMvto";
            viewMov.UnboundType = UnboundColumnType.String;
            viewMov.VisibleIndex = 4;
            viewMov.Width = 120;
            viewMov.Visible = true;
            viewMov.OptionsColumn.ShowCaption = false;
            viewMov.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvDetail.Columns.Add(viewMov);

            #region Columnas no visibles al Inicio

            GridColumn GrupoInvID = new GridColumn();
            GrupoInvID.FieldName = this._unboundPrefix + "GrupoInvID";
            GrupoInvID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_GrupoInvID");
            GrupoInvID.UnboundType = UnboundColumnType.String;
            GrupoInvID.Width = 100;
            GrupoInvID.Visible = false;
            this.gvDetail.Columns.Add(GrupoInvID);

            GridColumn ClaseInvID = new GridColumn();
            ClaseInvID.FieldName = this._unboundPrefix + "ClaseInvID";
            ClaseInvID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ClaseInvID");
            ClaseInvID.UnboundType = UnboundColumnType.String;
            ClaseInvID.Width = 100;
            ClaseInvID.Visible = false;
            this.gvDetail.Columns.Add(ClaseInvID);

            GridColumn TipoInvID = new GridColumn();
            TipoInvID.FieldName = this._unboundPrefix + "TipoInvID";
            TipoInvID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_TipoInvID");
            TipoInvID.UnboundType = UnboundColumnType.String;
            TipoInvID.Width = 100;
            TipoInvID.Visible = false;
            this.gvDetail.Columns.Add(TipoInvID);

            GridColumn SerieID = new GridColumn();
            SerieID.FieldName = this._unboundPrefix + "SerieID";
            SerieID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_SerieID");
            SerieID.UnboundType = UnboundColumnType.String;
            SerieID.Width = 100;
            SerieID.Visible = false;
            this.gvDetail.Columns.Add(SerieID);

            GridColumn MaterialInvID = new GridColumn();
            MaterialInvID.FieldName = this._unboundPrefix + "MaterialInvID";
            MaterialInvID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_MaterialInvID");
            MaterialInvID.UnboundType = UnboundColumnType.String;
            MaterialInvID.Width = 100;
            MaterialInvID.Visible = false;
            this.gvDetail.Columns.Add(MaterialInvID);

            GridColumn MarcaInvID = new GridColumn();
            MarcaInvID.FieldName = this._unboundPrefix + "MarcaInvID";
            MarcaInvID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_MarcaInvID");
            MarcaInvID.UnboundType = UnboundColumnType.String;
            MarcaInvID.Width = 100;
            MarcaInvID.Visible = false;
            this.gvDetail.Columns.Add(MarcaInvID);

            GridColumn PosicionArancelID = new GridColumn();
            PosicionArancelID.FieldName = this._unboundPrefix + "PosicionArancelID";
            PosicionArancelID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_PosicionArancelID");
            PosicionArancelID.UnboundType = UnboundColumnType.String;
            PosicionArancelID.Width = 100;
            PosicionArancelID.Visible = false;
            this.gvDetail.Columns.Add(PosicionArancelID);

            GridColumn UnidadInvID = new GridColumn();
            UnidadInvID.FieldName = this._unboundPrefix + "UnidadInvID";
            UnidadInvID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_UnidadInvID");
            UnidadInvID.UnboundType = UnboundColumnType.String;
            UnidadInvID.Width = 100;
            UnidadInvID.Visible = false;
            this.gvDetail.Columns.Add(UnidadInvID);

            GridColumn EmpaqueInvID = new GridColumn();
            EmpaqueInvID.FieldName = this._unboundPrefix + "EmpaqueInvID";
            EmpaqueInvID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_EmpaqueInvID");
            EmpaqueInvID.UnboundType = UnboundColumnType.String;
            EmpaqueInvID.Width = 100;
            EmpaqueInvID.Visible = false;
            this.gvDetail.Columns.Add(EmpaqueInvID);

            GridColumn RefProveedor = new GridColumn();
            RefProveedor.FieldName = this._unboundPrefix + "RefProveedor";
            RefProveedor.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_RefProveedor");
            RefProveedor.UnboundType = UnboundColumnType.String;
            RefProveedor.Width = 100;
            RefProveedor.Visible = false;
            this.gvDetail.Columns.Add(RefProveedor); 
            #endregion

        }
        
        /// <summary>
        /// Asigna los valores de costos de las referencias validando permisos
        /// </summary>
        /// <param name="index">Identificador de la fila</param>
        private void GetCostos(int index)
        {
            try
            {
                if (this._userBodega != null && this._userBodega.ConsultaCostosInd.Value.Value && this.gvDetail.DataRowCount > 0)
                {
                    string refSelected = this.gvDetail.GetRowCellValue(index, this._unboundPrefix + "ID").ToString();
                    if (!this._filterActive && this._listRefSelected.Count > 0)
                    {
                        var data = this._listRefSelected.Where(x => x.ID.Value.Equals(refSelected)).FirstOrDefault();
                        if (data != null)
                        {
                            DTO_inReferencia refer = (DTO_inReferencia)data;
                            //Trae costos segun la cantidad de la referencia
                            decimal? valorUnitGralLoc = refer.SaldoExistencia.Value != null && refer.SaldoExistencia.Value != 0 ? (refer.CostosExistencia.CtoLocSaldoIni.Value + refer.CostosExistencia.CtoLocEntrada.Value - refer.CostosExistencia.CtoLocSalida.Value) / refer.SaldoExistencia.Value : 0;
                            decimal? valorUnitGralExt = refer.SaldoExistencia.Value != null && refer.SaldoExistencia.Value != 0 ? (refer.CostosExistencia.CtoExtSaldoIni.Value + refer.CostosExistencia.CtoExtEntrada.Value - refer.CostosExistencia.CtoExtSalida.Value) / refer.SaldoExistencia.Value : 0;
                            this.txtValorUnitML.EditValue = refer.CostosExistencia.CtoUnitarioLoc.Value;// valorUnitGralLoc;
                            this.txtValorUnitME.EditValue = refer.CostosExistencia.CtoUnitarioExt.Value; //valorUnitGralExt;
                            this.txtValorTotalML.EditValue = refer.CostosExistencia.CtoUnitarioLoc.Value * refer.SaldoExistencia.Value;
                            this.txtValorTotalME.EditValue = refer.CostosExistencia.CtoUnitarioExt.Value * refer.SaldoExistencia.Value;
                        }
                        else
                        {
                            this.txtValorUnitML.EditValue = 0;
                            this.txtValorUnitME.EditValue = 0;
                            this.txtValorTotalML.EditValue = 0;
                            this.txtValorTotalME.EditValue = 0 ;
                        }
                    }
                    else if (this._listRefFiltered != null && this._listRefFiltered.Count > 0)
                    {
                        var data = this._listRefFiltered.Where(x => x.ID.Value.Equals(refSelected)).FirstOrDefault();
                        if (data != null)
                        {
                            DTO_inReferencia refer = (DTO_inReferencia)data;
                            decimal? valorUnitGralLoc = refer.SaldoExistencia.Value != null && refer.SaldoExistencia.Value != 0 ? (refer.CostosExistencia.CtoLocSaldoIni.Value + refer.CostosExistencia.CtoLocEntrada.Value - refer.CostosExistencia.CtoLocSalida.Value) / refer.SaldoExistencia.Value : 0;
                            decimal? valorUnitGralExt = refer.SaldoExistencia.Value != null && refer.SaldoExistencia.Value != 0 ? (refer.CostosExistencia.CtoExtSaldoIni.Value + refer.CostosExistencia.CtoExtEntrada.Value - refer.CostosExistencia.CtoExtSalida.Value) / refer.SaldoExistencia.Value : 0;
                            this.txtValorUnitML.EditValue = valorUnitGralLoc;
                            this.txtValorUnitME.EditValue = valorUnitGralExt;
                            this.txtValorTotalML.EditValue = valorUnitGralLoc * refer.SaldoExistencia.Value;
                            this.txtValorTotalME.EditValue = valorUnitGralExt * refer.SaldoExistencia.Value;
                        }
                        else
                        {
                            this.txtValorUnitML.EditValue = 0;
                            this.txtValorUnitME.EditValue = 0;
                            this.txtValorTotalML.EditValue = 0;
                            this.txtValorTotalME.EditValue = 0;
                        }
                    } 
                }
                else
                {
                    this.txtValorTotalML.EditValue = 0;
                    this.txtValorTotalME.EditValue = 0;
                    this.txtValorUnitML.EditValue = 0;
                    this.txtValorUnitME.EditValue = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaReferencia.cs", "GetCostos: " + ex.Message));
            }
        }

        /// <summary>
        /// Limpia los controles de filtros
        /// </summary>
        private void CleanFilter()
        {
            this.chkClaseInv.Checked = false;
            this.chkGrupoInv.Checked = false;
            this.chkSerieInv.Checked = false;
            this.chkMarcaInv.Checked = false;
            this.chkMaterialInv.Checked = false;
            this.chkReferencia.Checked = false;
            this.chkTipoInv.Checked = false;
            this.chkUnidadInv.Checked = false;
            this.chkEmpaqueInv.Checked = false;
            this.masterClaseInv.Value = string.Empty;
            this.masterGrupoInv.Value = string.Empty;
            this.masterEmpaqueInv.Value = string.Empty;
            this.masterMarcaInv.Value = string.Empty;
            this.masterMaterialInv.Value = string.Empty;
            this.masterReferencia.Value = string.Empty;
            this.masterSerieInv.Value = string.Empty;
            this.masterTipoInv.Value = string.Empty;
            this.masterUnidadInv.Value = string.Empty;
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
                var tmp = this._filterActive?   this._listRefFiltered.Skip((this.pgGrid.PageNumber - 1) * this._pageSize).Take(this._pageSize).ToList<DTO_inReferencia>() :
                                                this._listRefSelected.Skip((this.pgGrid.PageNumber - 1) * this._pageSize).Take(this._pageSize).ToList<DTO_inReferencia>();
                this.pgGrid.UpdatePageNumber(this._filterActive? this._listRefFiltered.Count : this._listRefSelected.Count, false, false, false);
                this.gvDetail.MoveFirst();
                this.gcDetail.DataSource = null;
                this.gcDetail.DataSource = tmp;
                this.GetCostos(this.gvDetail.FocusedRowHandle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaReferencia.cs", "pagging_Click: " + ex.Message));
            }
        }

        /// <summary>
        /// Muestra u oculta los campos de filtro de referencias
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnFilterChk_CheckedChanged(object sender, EventArgs e)
        {
            if (this.btnFilterChk.Checked)
                this.pnlFilter.Visible = true;
            else
                this.pnlFilter.Visible = false;
        }

        /// <summary>
        /// Habilita los controles de filtro de la referencia
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void chkFilter_CheckedChanged(object sender, EventArgs e)
        {
            CheckEdit ctrl = (CheckEdit)sender;
            try
            {
                switch (ctrl.Name)
                {
                    case "chkSerial":
                        if (ctrl.Checked)
                        {
                            this.txtSerial.Enabled = true;
                        }                          
                        else
                        {
                            this.txtSerial.Enabled = false;
                        }
                        break;
                    case "chkRefProvee":
                        if (ctrl.Checked)
                        {
                            this.txtRefProveedor.Enabled = true;
                            this.gvDetail.Columns[this._unboundPrefix + "RefProveedor"].VisibleIndex = 2;
                            this.gvDetail.Columns[this._unboundPrefix + "RefProveedor"].Visible = true;
                        }                           
                        else
                        {
                            this.txtRefProveedor.Enabled = false;
                            this.gvDetail.Columns[this._unboundPrefix + "RefProveedor"].VisibleIndex = 2;
                            this.gvDetail.Columns[this._unboundPrefix + "RefProveedor"].Visible = false;
                        }                           
                        break;
                    case "chkEstado":
                        if (ctrl.Checked)
                        {
                            this.cmbEstado.Enabled = true;
                        }                            
                        else
                        {
                            this.cmbEstado.Enabled = false;
                        }                            
                        break;
                    case "chkParam1":
                        if (ctrl.Checked)
                        {
                            this.masterParam1.EnableControl(true);
                        }                            
                        else
                        {
                            this.masterParam1.EnableControl(false);
                        }
                           
                        break;
                    case "chkParam2":
                        if (ctrl.Checked)
                        {
                            this.masterParam2.EnableControl(true);
                        }                            
                        else
                        {
                            this.masterParam2.EnableControl(false);
                        }                           
                        break;
                    case "chkGrupoInv":
                        if (ctrl.Checked)
                        {
                            this.masterGrupoInv.EnableControl(true);
                            this.gvDetail.Columns[this._unboundPrefix + "GrupoInvID"].VisibleIndex = 2;
                            this.gvDetail.Columns[this._unboundPrefix + "GrupoInvID"].Visible = true;
                        }                          
                        else
                        {
                            this.masterGrupoInv.EnableControl(false);
                            this.gvDetail.Columns[this._unboundPrefix + "GrupoInvID"].VisibleIndex = 2;
                            this.gvDetail.Columns[this._unboundPrefix + "GrupoInvID"].Visible = false;
                        }                           
                        break;
                    case "chkClaseInv":
                        if (ctrl.Checked)
                        {
                            this.masterClaseInv.EnableControl(true);
                            this.gvDetail.Columns[this._unboundPrefix + "ClaseInvID"].VisibleIndex = 2;
                            this.gvDetail.Columns[this._unboundPrefix + "ClaseInvID"].Visible = true;
                        }                           
                        else
                        {
                            this.masterClaseInv.EnableControl(false);
                            this.gvDetail.Columns[this._unboundPrefix + "ClaseInvID"].VisibleIndex = 2;
                            this.gvDetail.Columns[this._unboundPrefix + "ClaseInvID"].Visible = false;
                        }                           
                        break;
                    case "chkTipoInv":
                        if (ctrl.Checked)
                        {
                            this.masterTipoInv.EnableControl(true);
                            this.gvDetail.Columns[this._unboundPrefix + "TipoInvID"].VisibleIndex = 2;
                            this.gvDetail.Columns[this._unboundPrefix + "TipoInvID"].Visible = true;
                        }
                            
                        else
                        {
                             this.masterTipoInv.EnableControl(false);
                             this.gvDetail.Columns[this._unboundPrefix + "TipoInvID"].VisibleIndex = 2;
                             this.gvDetail.Columns[this._unboundPrefix + "TipoInvID"].Visible = false;
                        }                           
                        break;
                    case "chkSerieInv":
                        if (ctrl.Checked)
                        {
                            this.masterSerieInv.EnableControl(true);
                            this.gvDetail.Columns[this._unboundPrefix + "SerieID"].VisibleIndex = 2;
                            this.gvDetail.Columns[this._unboundPrefix + "SerieID"].Visible = true;
                        }                           
                        else
                        {
                            this.masterSerieInv.EnableControl(false);
                            this.gvDetail.Columns[this._unboundPrefix + "SerieID"].VisibleIndex = 2;
                            this.gvDetail.Columns[this._unboundPrefix + "SerieID"].Visible = false;
                        }                            
                        break;
                    case "chkUnidadInv":
                        if (ctrl.Checked)
                        {
                            this.masterUnidadInv.EnableControl(true);
                            this.gvDetail.Columns[this._unboundPrefix + "UnidadInvID"].VisibleIndex = 2;
                            this.gvDetail.Columns[this._unboundPrefix + "UnidadInvID"].Visible = true;
                        }                           
                        else
                        {
                            this.masterUnidadInv.EnableControl(false);
                            this.gvDetail.Columns[this._unboundPrefix + "UnidadInvID"].VisibleIndex = 2;
                            this.gvDetail.Columns[this._unboundPrefix + "UnidadInvID"].Visible = false;
                        }                         
                        break;
                    case "chkMaterialInv":
                        if (ctrl.Checked)
                        {
                            this.masterMaterialInv.EnableControl(true);
                            this.gvDetail.Columns[this._unboundPrefix + "MaterialInvID"].VisibleIndex = 2;
                            this.gvDetail.Columns[this._unboundPrefix + "MaterialInvID"].Visible = true;
                        }                          
                        else
                        {
                            this.masterMaterialInv.EnableControl(false);
                            this.gvDetail.Columns[this._unboundPrefix + "MaterialInvID"].VisibleIndex = 2;
                            this.gvDetail.Columns[this._unboundPrefix + "MaterialInvID"].Visible = false;
                        }                            
                        break;
                    case "chkMarcaInv":
                        if (ctrl.Checked)
                        {
                            this.masterMarcaInv.EnableControl(true);
                            this.gvDetail.Columns[this._unboundPrefix + "MarcaInvID"].VisibleIndex = 2;
                            this.gvDetail.Columns[this._unboundPrefix + "MarcaInvID"].Visible = true;
                        }                         
                        else
                        {
                            this.masterMarcaInv.EnableControl(false);
                            this.gvDetail.Columns[this._unboundPrefix + "MarcaInvID"].VisibleIndex = 2;
                            this.gvDetail.Columns[this._unboundPrefix + "MarcaInvID"].Visible = false;
                        }                           
                        break;
                    case "chkEmpaqueInv":
                        if (ctrl.Checked)
                        {
                            this.masterEmpaqueInv.EnableControl(true);
                            this.gvDetail.Columns[this._unboundPrefix + "EmpaqueInvID"].VisibleIndex = 2;
                            this.gvDetail.Columns[this._unboundPrefix + "EmpaqueInvID"].Visible = true;
                        }                         
                        else
                        {
                            this.masterEmpaqueInv.EnableControl(false);
                            this.gvDetail.Columns[this._unboundPrefix + "EmpaqueInvID"].VisibleIndex = 2;
                            this.gvDetail.Columns[this._unboundPrefix + "EmpaqueInvID"].Visible = false;
                        }                            
                        break;
                    case "chkReferencia":
                        if (ctrl.Checked)
                            this.masterReferencia.EnableControl(true);
                        else
                            this.masterReferencia.EnableControl(false);
                        break;                    
                }
                if (!this.chkGrupoInv.Checked && !this.chkClaseInv.Checked && !this.chkTipoInv.Checked && !this.chkSerieInv.Checked && !this.chkUnidadInv.Checked &&
                    !this.chkMaterialInv.Checked && !this.chkMarcaInv.Checked && !this.chkReferencia.Checked && !this.chkEmpaqueInv.Checked && !this.chkEstado.Checked &&
                    !this.chkSerial.Checked && !this.chkParam1.Checked && !this.chkParam2.Checked)
                        this._filterActive = false;
                this.masterFilter_Leave(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaReferencia.cs", "chkFilter_CheckedChanged: " + ex.Message));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al salir del control de maestra
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterFilter_Leave(object sender, EventArgs e)
        {
            if (this._listRefSelected.Count > 0)
            {
                Object listFilter;
                List<DTO_inReferencia> referencia;
                this._listRefFiltered = new List<DTO_inReferencia>();
                this._listRefFiltered = this._listRefSelected;
                try
                {
                    if (this.chkParam1.Checked && this.masterParam1.ValidID)
                    {
                        #region Parametro1
                        //listFilter = this._dtoListRefFiltered.Where(x => x.TipoInvID.Value.Equals(this.masterGrupoInv.Value)).ToList();
                        //referencia = (List<DTO_inReferencia>)listFilter;
                        //this._dtoListRefFiltered = referencia;
                        //this._filterActive = true;
                        #endregion
                    }
                    if (this.chkParam2.Checked && this.masterParam2.ValidID)
                    {
                        #region Parametro2
                        //listFilter = this._dtoListRefFiltered.Where(x => x.TipoInvID.Value.Equals(this.masterGrupoInv.Value)).ToList();
                        //referencia = (List<DTO_inReferencia>)listFilter;
                        //this._dtoListRefFiltered = referencia;
                        //this._filterActive = true;
                        #endregion
                    }
                    if (this.chkGrupoInv.Checked && this.masterGrupoInv.ValidID)
                    {
                        #region GrupoInvID
                        listFilter = this._listRefFiltered.Where(x => x.GrupoInvID.Value.Equals(this.masterGrupoInv.Value)).ToList();
                        referencia = (List<DTO_inReferencia>)listFilter;
                        this._listRefFiltered = referencia;
                        this._filterActive = true;
                        #endregion
                    }
                    if (this.chkClaseInv.Checked && this.masterClaseInv.ValidID)
                    {
                        #region ClaseInvID
                        listFilter = this._listRefFiltered.Where(x => x.ClaseInvID.Value.Equals(this.masterClaseInv.Value)).ToList();
                        referencia = (List<DTO_inReferencia>)listFilter;
                        this._listRefFiltered = referencia;
                        this._filterActive = true;
                        #endregion
                    }
                    if (this.chkTipoInv.Checked && this.masterTipoInv.ValidID)
                    {
                        #region TipoInvID
                        listFilter = this._listRefFiltered.Where(x => x.TipoInvID.Value.Equals(this.masterTipoInv.Value)).ToList();
                        referencia = (List<DTO_inReferencia>)listFilter;
                        this._listRefFiltered = referencia;
                        this._filterActive = true;
                        #endregion
                    }
                    if (this.chkSerieInv.Checked && this.masterSerieInv.ValidID)
                    {
                        #region TipoInvID
                        listFilter = this._listRefFiltered.Where(x => x.SerieID.Value.Equals(this.masterSerieInv.Value)).ToList();
                        referencia = (List<DTO_inReferencia>)listFilter;
                        this._listRefFiltered = referencia;
                        this._filterActive = true;
                        #endregion
                    }
                    if (this.chkMaterialInv.Checked && this.masterMaterialInv.ValidID)
                    {
                        #region MaterialInvID
                        listFilter = this._listRefFiltered.Where(x => x.MaterialInvID.Value.Equals(this.masterMaterialInv.Value)).ToList();
                        referencia = (List<DTO_inReferencia>)listFilter;
                        this._listRefFiltered = referencia;
                        this._filterActive = true;
                        #endregion
                    }
                    if (this.chkMarcaInv.Checked && this.masterMarcaInv.ValidID)
                    {
                        #region MarcaInvID
                        listFilter = this._listRefFiltered.Where(x => x.MarcaInvID.Value.Equals(this.masterMarcaInv.Value)).ToList();
                        referencia = (List<DTO_inReferencia>)listFilter;
                        this._listRefFiltered = referencia;
                        this._filterActive = true;
                        #endregion
                    }
                    if (this.chkReferencia.Checked && this.masterReferencia.ValidID)
                    {
                        #region Referencia
                        listFilter = this._listRefFiltered.Where(x => x.ID.Value.Contains(this.masterReferencia.Value)).ToList();
                        referencia = (List<DTO_inReferencia>)listFilter;
                        this._listRefFiltered = referencia;
                        this._filterActive = true;
                        #endregion
                    }
                    if (this.chkUnidadInv.Checked && this.masterUnidadInv.ValidID)
                    {
                        #region UnidadInvID
                        listFilter = this._listRefFiltered.Where(x => x.UnidadInvID.Value.Equals(this.masterUnidadInv.Value)).ToList();
                        referencia = (List<DTO_inReferencia>)listFilter;
                        this._listRefFiltered = referencia;
                        this._filterActive = true;
                        #endregion
                    }
                    if (this.chkEmpaqueInv.Checked && this.masterEmpaqueInv.ValidID)
                    {
                        #region EmpaqueInvID
                        listFilter = this._listRefFiltered.Where(x => x.EmpaqueInvID.Value.Equals(this.masterEmpaqueInv.Value)).ToList();
                        referencia = (List<DTO_inReferencia>)listFilter;
                        this._listRefFiltered = referencia;
                        this._filterActive = true;
                        #endregion
                    }
                    if (this.chkRefProvee.Checked && !string.IsNullOrEmpty(this.txtRefProveedor.Text))
                    {
                        #region Ref Proveedor(Modelo)
                        listFilter = this._listRefSelected.Where(x => x.RefProveedor.Value.Contains(this.txtRefProveedor.Text.ToUpper())).ToList();
                        referencia = (List<DTO_inReferencia>)listFilter;
                        this._listRefFiltered = referencia;
                        this._filterActive = true;
                        #endregion
                    }
                    if (this._filterActive)
                    {
                        this.pgGrid.UpdatePageNumber(this._listRefFiltered.Count, false, true, false);
                        this.gcDetail.DataSource = null;
                        this.gcDetail.DataSource = this._listRefFiltered;
                        this.gvDetail.RefreshData();
                        this.GetCostos(this.gvDetail.FocusedRowHandle);
                    }
                    else
                    {
                        this.gcDetail.DataSource = null;
                        this.gcDetail.DataSource = this._listRefSelected;
                        this.gvDetail.RefreshData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaReferencia.cs", "masterFilter_Leave: " + ex.Message));
                }
            }
        }

        /// <summary>
        /// Se ejecuta al salir del control y filtra los datos de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //if (sender.GetType() == typeof(ControlsUC.uc_MasterFind))
                    this.masterFilter_Leave(sender, e);
                //else
                //    this.txtrefProveedor_Leave(sender, e);
            }
               
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
                FormProvider.Master.Form_Enter(this, this._documentID, this._frmType, this._frmModule);
                FormProvider.Master.itemSearch.Visible = false;
                FormProvider.Master.itemExport.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaReferencia.cs", "Form_Enter: " + ex.Message));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Leave(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Leave(this, this._documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaReferencia.cs", "Form_Leave: " + ex.Message));
            }
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
                FormProvider.Master.Form_Closing(this, this._documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaReferencia.cs", "Form_FormClosing: " + ex.Message));
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
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this._frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaReferencia.cs", "Form_FormClosed: " + ex.Message));
            }
        }

        #endregion

        #region Eventos Grillas

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvHeader_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
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
        private void gvHeader_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (this._listReferenciaAll != null && e.FocusedRowHandle >= 0)
            {
                this._bodegaCurrent = (DTO_inBodega)this.gvBodega.GetRow(e.FocusedRowHandle);
                #region Variables
                this.listSaldosCostos = new List<DTO_inControlSaldosCostos>();
                DTO_inControlSaldosCostos saldos = new DTO_inControlSaldosCostos();
                this._listRefSelected = new List<DTO_inReferencia>();
                //this._filterActive = false;
                //this.CleanFilter();
                #endregion
                try
                {
                    #region Trae los saldos de referencias por bodega
                    saldos.BodegaID.Value = this._bodegaCurrent.ID.Value;
                    this.listSaldosCostos = _bc.AdministrationModel.inControlSaldosCostos_GetByParameter(this._documentID, saldos);
                    //this.listSaldosCostos = this.listSaldosCostos.FindAll(x => x.inReferenciaID.Value == "0305002" || x.inReferenciaID.Value == "0305006").ToList();
                    foreach (DTO_inControlSaldosCostos saldo in this.listSaldosCostos)
                    {
                        DTO_inReferencia refer = (DTO_inReferencia)this._listReferenciaAll.Where(x => x.ID.Value.Equals(saldo.inReferenciaID.Value)).FirstOrDefault();
                        //costos = new DTO_inCostosExistencias();
                        saldos.inReferenciaID.Value = saldo.inReferenciaID.Value;
                        //decimal cantidadDisp = this._bc.AdministrationModel.Transaccion_SaldoExistByReferencia(_documentID, saldos, ref costos);
                        refer.SaldoExistencia.Value = saldo.CantidadDisp.Value;// cantidadDisp;
                        refer.CostosExistencia =saldo.CostosExistencia;
                        refer.UbicacionID = saldo.UbicacionID;
                        bool refExist = this._listRefSelected.Exists(x => x.ID.Value.Equals(saldo.inReferenciaID.Value));
                        if (!refExist && refer.SaldoExistencia.Value != 0)
                            this._listRefSelected.Add(refer);
                    }
                    this._listRefSelected = this._listRefSelected.OrderBy(x => x.Descriptivo.Value).ToList();
                    this._bodegaCurrent.DetalleReferencias = this._listRefSelected;
                    #endregion
                    #region Carga los permisos del user para Consultar Costos
                    Dictionary<string, string> keysUserBodega = new Dictionary<string, string>();
                    keysUserBodega.Add("seUsuarioID", _bc.AdministrationModel.User.ReplicaID.Value.ToString());
                    keysUserBodega.Add("BodegaID", this._bodegaCurrent.ID.Value);
                    this._userBodega = (DTO_seUsuarioBodega)_bc.GetMasterComplexDTO(AppMasters.seUsuarioBodega, keysUserBodega, true);
                    this._bodegaCurrent.ConsultaCostoInd.Value = this._userBodega != null ? this._userBodega.ConsultaCostosInd.Value : false; 
                    #endregion
                    var tmp = this._listRefSelected.Skip((this.pgGrid.PageNumber - 1) * _pageSize).Take(_pageSize).ToList<DTO_inReferencia>();
                    this.gvDetail.MoveFirst();
                    this.gcDetail.DataSource = null;
                    this.gcDetail.RefreshDataSource();
                    this.gcDetail.DataSource = tmp.OrderBy(x=>x.Descriptivo.Value).ToList();
                    this.pgGrid.UpdatePageNumber(this._listRefSelected.Count, false, true, false);
                    this.GetCostos(0);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaReferencia.cs", "gvHeader_FocusedRowChanged: " + ex.Message));
                }
            }
        }

        /// <summary>
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvBodega_RowClick(object sender, RowClickEventArgs e)
        {
            if (this._listReferenciaAll != null && e.RowHandle >= 0)
            {
                this._bodegaCurrent = (DTO_inBodega)this.gvBodega.GetRow(e.RowHandle);
                #region Variables
                this.listSaldosCostos = new List<DTO_inControlSaldosCostos>();
                DTO_inControlSaldosCostos saldos = new DTO_inControlSaldosCostos();
                this._listRefSelected = new List<DTO_inReferencia>();
                #endregion
                try
                {
                    #region Trae los saldos de referencias por bodega
                    saldos.BodegaID.Value = this._bodegaCurrent.ID.Value;
                    this.listSaldosCostos = _bc.AdministrationModel.inControlSaldosCostos_GetByParameter(this._documentID, saldos);
                    //this.listSaldosCostos = this.listSaldosCostos.FindAll(x => x.inReferenciaID.Value == "0305002" || x.inReferenciaID.Value == "0305006").ToList();
                    foreach (DTO_inControlSaldosCostos saldo in this.listSaldosCostos)
                    {
                        DTO_inReferencia refer = (DTO_inReferencia)this._listReferenciaAll.Where(x => x.ID.Value.Equals(saldo.inReferenciaID.Value)).FirstOrDefault();
                        //costos = new DTO_inCostosExistencias();
                        saldos.inReferenciaID.Value = saldo.inReferenciaID.Value;
                        //decimal cantidadDisp = this._bc.AdministrationModel.Transaccion_SaldoExistByReferencia(_documentID, saldos, ref costos);
                        refer.SaldoExistencia.Value = saldo.CantidadDisp.Value;// cantidadDisp;
                        refer.CostosExistencia = saldo.CostosExistencia;
                        bool refExist = this._listRefSelected.Exists(x => x.ID.Value.Equals(saldo.inReferenciaID.Value));
                        if (!refExist && refer.SaldoExistencia.Value != 0)
                            this._listRefSelected.Add(refer);
                    }
                    this._listRefSelected = this._listRefSelected.OrderBy(x => x.Descriptivo.Value).ToList();
                    this._bodegaCurrent.DetalleReferencias = this._listRefSelected;
                    #endregion
                    #region Carga los permisos del user para Consultar Costos
                        Dictionary<string, string> keysUserBodega = new Dictionary<string, string>();
                        keysUserBodega.Add("seUsuarioID", _bc.AdministrationModel.User.ReplicaID.Value.ToString());
                        keysUserBodega.Add("BodegaID", this._bodegaCurrent.ID.Value);
                        this._userBodega = (DTO_seUsuarioBodega)_bc.GetMasterComplexDTO(AppMasters.seUsuarioBodega, keysUserBodega, true);
                        this._bodegaCurrent.ConsultaCostoInd.Value = this._userBodega != null ? this._userBodega.ConsultaCostosInd.Value : false;
                    #endregion
                    var tmp = this._listRefSelected.Skip((this.pgGrid.PageNumber - 1) * _pageSize).Take(_pageSize).ToList<DTO_inReferencia>();
                    this.gvDetail.MoveFirst();
                    this.gcDetail.DataSource = null;
                    this.gcDetail.RefreshDataSource();
                    this.gcDetail.DataSource = tmp;
                    this.pgGrid.UpdatePageNumber(this._listRefSelected.Count, false, true, false);
                    this.GetCostos(0);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaReferencia.cs", "gvBodega_RowClick: " + ex.Message));
                }
            }
        }

        /// <summary>
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetail_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if(!this.gvBodega.IsFocusedView)
                this.GetCostos(e.FocusedRowHandle);
        }

        /// <summary>
        /// Asigna controles editor a las columnas de la grilla seleccionadas
        /// </summary>
        /// <param name="sender">objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetail_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

            if (fieldName == "VerDetalle")
                e.RepositoryItem = this.editBtnDetail;
            else if (fieldName == "VerMvto")
                e.RepositoryItem = this.editBtnMvto;
        }

        /// <summary>
        /// Abre la modal para detalle
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void editBtnDetail_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                string refSelected = this.gvDetail.GetRowCellValue(this.gvDetail.FocusedRowHandle, this._unboundPrefix + "ID").ToString();
                ModalConsultaInventario fact = new ModalConsultaInventario(refSelected, this._bodegaCurrent.ID.Value, true, this._userBodega != null ? this._userBodega.ConsultaCostosInd.Value.Value : false);
                fact.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaReferencia.cs", "editBtnDetail_ButtonClick: " + ex.Message));
            }
        }

        /// <summary>
        /// Abre la modal para movimientos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void editBtnMvto_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                string refSelected = this.gvDetail.GetRowCellValue(this.gvDetail.FocusedRowHandle, this._unboundPrefix + "ID").ToString();
                ModalConsultaInventario fact = new ModalConsultaInventario(refSelected, this._bodegaCurrent.ID.Value, false, this._userBodega != null ? this._userBodega.ConsultaCostosInd.Value.Value : false);
                fact.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaReferencia.cs", "editBtnMvto_ButtonClick: " + ex.Message));
            }
        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para actualizar las busquedas
        /// </summary>
        public override void TBUpdate()
        {
            this.LoadGridData();
            this._listRefFiltered = null;
            this._filterActive = false;
            this.CleanFilter();
        }

        /// <summary>
        /// Boton para importar datos la data actual
        /// </summary>
        public override void TBExport()
        {
            try
            {
                if (this._bodegaCurrent != null)
                {
                    DateTime periodoInv = Convert.ToDateTime(this._bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Periodo));
                    DataTable query = this._bc.AdministrationModel.Reportes_In_InventarioToExcel(AppReports.inSaldos, periodoInv, new DateTime(periodoInv.Year,periodoInv.Month,DateTime.DaysInMonth(periodoInv.Year,periodoInv.Month)), this._bodegaCurrent.ID.Value,
                                      string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false, string.Empty,string.Empty,string.Empty, null, null, null);

                    if (query.Rows.Count != 0)
                    {
                        ReportExcelBase frm = new ReportExcelBase(query, AppReports.inSaldos);
                        frm.Show();
                    }
                    else
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostos.cs", "TBExport"));
            }
        }
        #endregion

    }
}
