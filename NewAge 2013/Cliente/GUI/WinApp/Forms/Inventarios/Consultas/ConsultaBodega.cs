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
    public partial class ConsultaBodega : FormWithToolbar
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
        private List<DTO_inReferencia> _listReferenciaAll = null;
        private List<DTO_inReferencia> _listRefSelected = null;
        private List<DTO_inReferencia> _listRefFiltered = null;
        private List<DTO_inControlSaldosCostos> listSaldosCostosAll = null;
        private List<DTO_inControlSaldosCostos> listSaldosCostosFiltered = null;
        private Dictionary<string, DTO_seUsuarioBodega> cacheUserBodega = new Dictionary<string, DTO_seUsuarioBodega>();
        private DTO_seUsuarioBodega _userBodega = null;
        private DTO_inControlSaldosCostos _bodegaCurrent = null;
        private DTO_inReferencia _referenciaCurrent = null;
        #endregion

        ///<summary>
        /// Constructor 
        /// </summary>
        public ConsultaBodega()
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
                this.LoadReferencias();
                //this.LoadGridData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinAppConsultaBodega.cs", "ConsultaBodega: " + ex.Message));
            }
        }

        #region Funciones privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            this._documentID = AppQueries.QueryBodega;
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
            #region Deshabilita SimCard
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
            bodega.FieldName = this._unboundPrefix + "BodegaID";
            bodega.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_BodegaID");
            bodega.UnboundType = UnboundColumnType.String;
            bodega.VisibleIndex = 0;
            bodega.Width = 170;
            bodega.Visible = true;
            bodega.OptionsColumn.AllowEdit = false;
            this.gvBodega.Columns.Add(bodega);

            GridColumn descriptivo = new GridColumn();
            descriptivo.FieldName = this._unboundPrefix + "BodegaDesc";
            descriptivo.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_BodegaDesc");
            descriptivo.UnboundType = UnboundColumnType.String;
            descriptivo.VisibleIndex = 1;
            descriptivo.Width = 250;
            descriptivo.Visible = true;
            descriptivo.OptionsColumn.AllowEdit = false;
            this.gvBodega.Columns.Add(descriptivo);

            GridColumn saldo = new GridColumn();
            saldo.FieldName = this._unboundPrefix + "CantidadDisp";
            saldo.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CantidadDisp");
            saldo.UnboundType = UnboundColumnType.Decimal;
            saldo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            saldo.AppearanceCell.Options.UseTextOptions = true;
            saldo.VisibleIndex = 2;
            saldo.Width = 150;
            saldo.Visible = true;
            saldo.ColumnEdit = this.editValue;
            saldo.OptionsColumn.AllowEdit = false;
            this.gvBodega.Columns.Add(saldo);

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
            this.gvBodega.Columns.Add(ubicacion);

            GridColumn viewDetail = new GridColumn();
            viewDetail.FieldName = this._unboundPrefix + "VerDetalle";
            viewDetail.VisibleIndex = 3;
            viewDetail.Width = 100;
            viewDetail.Visible = true;
            viewDetail.OptionsColumn.ShowCaption = false;
            viewDetail.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvBodega.Columns.Add(viewDetail);
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddGridColsReferencia()
        {
            GridColumn referencia = new GridColumn();
            referencia.FieldName = this._unboundPrefix + "ID";
            referencia.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_inReferenciaID");
            referencia.UnboundType = UnboundColumnType.String;
            referencia.VisibleIndex = 0;
            referencia.Width = 110;
            referencia.Visible = true;
            this.gvReferencia.Columns.Add(referencia);

            GridColumn descriptivo = new GridColumn();
            descriptivo.FieldName = this._unboundPrefix + "Descriptivo";
            descriptivo.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Descriptivo");
            descriptivo.UnboundType = UnboundColumnType.String;
            descriptivo.VisibleIndex = 1;
            descriptivo.Width = 250;
            descriptivo.Visible = true;
            this.gvReferencia.Columns.Add(descriptivo);

            GridColumn UnidadInvID = new GridColumn();
            UnidadInvID.FieldName = this._unboundPrefix + "UnidadInvID";
            UnidadInvID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_UnidadInvID");
            UnidadInvID.UnboundType = UnboundColumnType.String;
            UnidadInvID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            UnidadInvID.AppearanceCell.Options.UseTextOptions = true;
            UnidadInvID.VisibleIndex = 2;
            UnidadInvID.Width = 100;
            UnidadInvID.Visible = true;
            this.gvReferencia.Columns.Add(UnidadInvID);

            GridColumn RefProveedor = new GridColumn();
            RefProveedor.FieldName = this._unboundPrefix + "RefProveedor";
            RefProveedor.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_RefProveedor");
            RefProveedor.UnboundType = UnboundColumnType.String;
            RefProveedor.VisibleIndex = 3;
            RefProveedor.Width = 100;
            RefProveedor.Visible = true;
            this.gvReferencia.Columns.Add(RefProveedor);

            GridColumn GrupoInvID = new GridColumn();
            GrupoInvID.FieldName = this._unboundPrefix + "GrupoInvID";
            GrupoInvID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_GrupoInvID");
            GrupoInvID.UnboundType = UnboundColumnType.String;
            GrupoInvID.VisibleIndex = 4;
            GrupoInvID.Width = 100;
            GrupoInvID.Visible = false;
            this.gvReferencia.Columns.Add(GrupoInvID);

            GridColumn ClaseInvID = new GridColumn();
            ClaseInvID.FieldName = this._unboundPrefix + "ClaseInvID";
            ClaseInvID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ClaseInvID");
            ClaseInvID.UnboundType = UnboundColumnType.String;
            ClaseInvID.VisibleIndex = 5;
            ClaseInvID.Width = 100;
            ClaseInvID.Visible = false;
            this.gvReferencia.Columns.Add(ClaseInvID);

            GridColumn TipoInvID = new GridColumn();
            TipoInvID.FieldName = this._unboundPrefix + "TipoInvID";
            TipoInvID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_TipoInvID");
            TipoInvID.UnboundType = UnboundColumnType.String;
            TipoInvID.VisibleIndex = 6;
            TipoInvID.Width = 100;
            TipoInvID.Visible = false;
            this.gvReferencia.Columns.Add(TipoInvID);

            GridColumn SerieID = new GridColumn();
            SerieID.FieldName = this._unboundPrefix + "SerieID";
            SerieID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_SerieID");
            SerieID.UnboundType = UnboundColumnType.String;
            SerieID.VisibleIndex = 7;
            SerieID.Width = 100;
            SerieID.Visible = false;
            this.gvReferencia.Columns.Add(SerieID);

            GridColumn MaterialInvID = new GridColumn();
            MaterialInvID.FieldName = this._unboundPrefix + "MaterialInvID";
            MaterialInvID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_MaterialInvID");
            MaterialInvID.UnboundType = UnboundColumnType.String;
            MaterialInvID.VisibleIndex = 8;
            MaterialInvID.Width = 100;
            MaterialInvID.Visible = false;
            this.gvReferencia.Columns.Add(MaterialInvID);

            GridColumn MarcaInvID = new GridColumn();
            MarcaInvID.FieldName = this._unboundPrefix + "MarcaInvID";
            MarcaInvID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_MarcaInvID");
            MarcaInvID.UnboundType = UnboundColumnType.String;
            MarcaInvID.VisibleIndex = 9;
            MarcaInvID.Width = 100;
            MarcaInvID.Visible = true;
            this.gvReferencia.Columns.Add(MarcaInvID);

            GridColumn PosicionArancelID = new GridColumn();
            PosicionArancelID.FieldName = this._unboundPrefix + "PosicionArancelID";
            PosicionArancelID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_PosicionArancelID");
            PosicionArancelID.UnboundType = UnboundColumnType.String;
            PosicionArancelID.VisibleIndex = 10;
            PosicionArancelID.Width = 100;
            PosicionArancelID.Visible = false;
            this.gvReferencia.Columns.Add(PosicionArancelID);       

            GridColumn EmpaqueInvID = new GridColumn();
            EmpaqueInvID.FieldName = this._unboundPrefix + "EmpaqueInvID";
            EmpaqueInvID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_EmpaqueInvID");
            EmpaqueInvID.UnboundType = UnboundColumnType.String;
            EmpaqueInvID.VisibleIndex = 11;
            EmpaqueInvID.Width = 100;
            EmpaqueInvID.Visible = false;
            this.gvReferencia.Columns.Add(EmpaqueInvID);
        }

        /// <summary>
        /// Carga los datos de la grilla
        /// </summary>
        private void LoadReferencias()
        {
            #region Trae todas las referencias
            long count = this._bc.AdministrationModel.MasterSimple_Count(AppMasters.inReferencia, null, null, true);
            this._listReferenciaAll = this._bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.inReferencia, count, 1, null, null, true).Cast<DTO_inReferencia>().ToList();
            this._listReferenciaAll = this._listReferenciaAll.OrderBy(x => x.Descriptivo.Value).ToList();
            this._listRefSelected = this._listReferenciaAll;
            this.pgGrid.UpdatePageNumber(this._listRefSelected.Count, true, true, false);

            List<DTO_inReferencia> pag = this._listRefSelected.Skip((this.pgGrid.PageNumber - 1) * this._pageSize).Take(this._pageSize).ToList<DTO_inReferencia>();
            this.gvReferencia.MoveFirst();
            this.gcReferencia.DataSource = null;
            this.gcReferencia.DataSource = pag;        
            #endregion
        }

        /// <summary>
        /// Carga los datos de la grilla
        /// </summary>
        private void LoadGridData()
        {
            try
            {
                #region Variables
                List<DTO_inControlSaldosCostos> listSaldosCostosTemp = new List<DTO_inControlSaldosCostos>();
                this.listSaldosCostosFiltered = new List<DTO_inControlSaldosCostos>(); 
                #endregion
                #region Carga datos de bodega
                DTO_inControlSaldosCostos saldosCostos = new DTO_inControlSaldosCostos();
                saldosCostos.inReferenciaID.Value = this._referenciaCurrent.ID.Value;
                saldosCostos.BodegaActivaInd.Value = true;
                listSaldosCostosTemp = _bc.AdministrationModel.inControlSaldosCostos_GetByParameter(this._documentID, saldosCostos);
                listSaldosCostosTemp = listSaldosCostosTemp.FindAll(x => x.CantidadDisp.Value > 0);
                listSaldosCostosTemp = listSaldosCostosTemp.OrderBy(x => x.ReferenciaP1P2Desc.Value).ToList();
                //Luego Carga los datos de las bodegas con los costos de cada referencia
                foreach (DTO_inControlSaldosCostos item in listSaldosCostosTemp)
                {
                    saldosCostos = new DTO_inControlSaldosCostos();
                    DTO_inBodega bodega = (DTO_inBodega)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, false, item.BodegaID.Value, true);
                    saldosCostos = item;
                    saldosCostos.BodegaDesc.Value = bodega.Descriptivo.Value;
                    saldosCostos.CantidadDisp.Value = item.CantidadDisp.Value;
                    this._referenciaCurrent.CostosExistencia = item.CostosExistencia;
                    if (item.ActivoID.Value.Value != 0)
                    {
                        DTO_acActivoControl activo = this._bc.AdministrationModel.acActivoControl_GetByID(item.ActivoID.Value.Value);
                        saldosCostos.SerialID.Value = activo != null ? activo.SerialID.Value : null;
                    }
                    if (this.listSaldosCostosFiltered.Exists(x => x.BodegaID.Value.Equals(item.BodegaID.Value) && x.inReferenciaID.Value.Equals(item.inReferenciaID.Value)))
                    {
                        int index = this.listSaldosCostosFiltered.FindIndex(x => x.BodegaID.Value.Equals(item.BodegaID.Value) && x.inReferenciaID.Value.Equals(item.inReferenciaID.Value));
                        this.listSaldosCostosFiltered[index].CantidadDisp.Value += item.CantidadDisp.Value;
                    }
                    else if (saldosCostos.CantidadDisp.Value != 0)
                        this.listSaldosCostosFiltered.Add(saldosCostos);
                } 
                #endregion
                this.gvBodega.MoveFirst();
                this.gcBodega.DataSource = null;
                this.gcBodega.DataSource = listSaldosCostosTemp;            
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaBodega.cs", "ConsultaBodega(" + this._documentID + ")-LoadGridData: " + ex.Message));
            }
        }

        /// <summary>
        /// Asigna los valores de costos de las referencias
        /// </summary>
        /// <param name="index">Identificador de la fila</param>
        private void GetCostos(int index)
        {
            try
            {
                if (this.gvReferencia.DataRowCount != (index - 1))
                    index = 0;
                if (this._userBodega != null && this._userBodega.ConsultaCostosInd.Value.Value && this.gvReferencia.DataRowCount > 0)
                {
                        if (this.listSaldosCostosFiltered.Count > 0)
                        {
                            //Trae la referencia para consultar los costos
                            if (this._bodegaCurrent.CostosExistencia != null)
                            {
                                //Trae costos segun la cantidad de la referencia
                                decimal? valorUnitGralLoc = this._bodegaCurrent.CantidadDisp.Value != null && this._bodegaCurrent.CantidadDisp.Value != 0 ? (this._bodegaCurrent.CostosExistencia.CtoLocSaldoIni.Value + this._bodegaCurrent.CostosExistencia.CtoLocEntrada.Value - this._bodegaCurrent.CostosExistencia.CtoLocSalida.Value) / this._bodegaCurrent.CantidadDisp.Value : 0;
                                decimal? valorUnitGralExt = this._bodegaCurrent.CantidadDisp.Value != null && this._bodegaCurrent.CantidadDisp.Value != 0 ? (this._bodegaCurrent.CostosExistencia.CtoExtSaldoIni.Value + this._bodegaCurrent.CostosExistencia.CtoExtEntrada.Value - this._bodegaCurrent.CostosExistencia.CtoExtSalida.Value) / this._bodegaCurrent.CantidadDisp.Value : 0;
                                this.txtValorUnitML.EditValue = this._bodegaCurrent.CostosExistencia.CtoUnitarioLoc.Value;// valorUnitGralLoc;
                                this.txtValorUnitME.EditValue = this._bodegaCurrent.CostosExistencia.CtoUnitarioExt.Value;// valorUnitGralExt;
                                this.txtValorTotalML.EditValue = this._bodegaCurrent.CostosExistencia.CtoUnitarioLoc.Value * this._bodegaCurrent.CantidadDisp.Value;
                                this.txtValorTotalME.EditValue = this._bodegaCurrent.CostosExistencia.CtoUnitarioExt.Value * this._bodegaCurrent.CantidadDisp.Value; 
                            }
                            else
                            {
                                this.txtValorTotalML.EditValue = 0;
                                this.txtValorTotalME.EditValue = 0;
                                this.txtValorUnitML.EditValue = 0;
                                this.txtValorUnitME.EditValue = 0;
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaBodega.cs", "GetCostos: " + ex.Message));
            }
        }

        /// <summary>
        /// Limpia los controles de filtros
        /// </summary>
        private void CleanFilter()
        {
            this.chkParam1.Checked = false;
            this.chkParam2.Checked = false;
            this.chkSerial.Checked = false;
            this.chkEstado.Checked = false;
            this.chkClaseInv.Checked = false;
            this.chkGrupoInv.Checked = false;
            this.chkSerieInv.Checked = false;
            this.chkMarcaInv.Checked = false;
            this.chkMaterialInv.Checked = false;
            this.chkReferencia.Checked = false;
            this.chkRefProvee.Checked = false;
            this.chkTipoInv.Checked = false;
            this.chkUnidadInv.Checked = false;
            this.chkEmpaqueInv.Checked = false;
            this.masterParam1.Value = string.Empty;
            this.masterParam2.Value = string.Empty;
            this.cmbEstado.Text = string.Empty;
            this.txtSerial.Text = string.Empty;
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

        /// <summary>
        /// Verifica los permisos del usuario en la bodega seleccionada
        /// </summary>
        private void SecurityUser()
        {
            try
            {
                #region Carga los permisos del user para Consultar Costos
                if (this.gvBodega.DataRowCount > 0)
                {
                    //string BodegaSelected = this.gvBodega.GetRowCellValue(this.gvBodega.FocusedRowHandle, this._unboundPrefix + "BodegaID").ToString();
                    #region Carga el usuario de la bodega
                    if (this.cacheUserBodega.ContainsKey(this._bodegaCurrent.BodegaID.Value))
                        this._userBodega = this.cacheUserBodega[this._bodegaCurrent.BodegaID.Value];
                    else
                    {
                        Dictionary<string, string> keysUserBodega = new Dictionary<string, string>();
                        keysUserBodega.Add("seUsuarioID", _bc.AdministrationModel.User.ReplicaID.Value.ToString());
                        keysUserBodega.Add("BodegaID", this._bodegaCurrent.BodegaID.Value);
                        this._userBodega = (DTO_seUsuarioBodega)_bc.GetMasterComplexDTO(AppMasters.seUsuarioBodega, keysUserBodega, true);
                        if (this._userBodega != null)
                            this.cacheUserBodega.Add(this._bodegaCurrent.BodegaID.Value, this._userBodega);
                    }
                    #endregion
                    this.GetCostos(this.gvReferencia.DataRowCount > 0 ? this.gvReferencia.FocusedRowHandle : 0); 
                }
                else
                {
                    this.txtValorTotalML.EditValue = 0;
                    this.txtValorTotalME.EditValue = 0;
                    this.txtValorUnitML.EditValue = 0;
                    this.txtValorUnitME.EditValue = 0;
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaBodega.cs", "SecurityUser: " + ex.Message)); ;
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
                var tmp = this._filterActive ?  this._listRefFiltered.Skip((this.pgGrid.PageNumber - 1) * this._pageSize).Take(this._pageSize).ToList<DTO_inReferencia>() :
                                                this._listRefSelected.Skip((this.pgGrid.PageNumber - 1) * this._pageSize).Take(this._pageSize).ToList<DTO_inReferencia>(); ;
                this.pgGrid.UpdatePageNumber(this._filterActive? this._listRefFiltered.Count : this._listRefSelected.Count, false, false, false);
                this.gvReferencia.MoveFirst();
                this.gcReferencia.DataSource = null;   
                this.gcReferencia.DataSource = tmp;

                //string refSelected = this.gvReferencia.GetRowCellValue(this.gvReferencia.FocusedRowHandle, this._unboundPrefix + "ID").ToString();
                var listSelect = this.listSaldosCostosFiltered.Where(x => x.inReferenciaID.Value.Equals(this._referenciaCurrent.ID.Value));

                this.gcBodega.DataSource = listSelect;
                this.GetCostos(this.gvReferencia.DataRowCount > 0 ? this.gvReferencia.FocusedRowHandle : 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaBodega.cs", "pagging_Click: " + ex.Message)); 
            }
        }

        /// <summary>
        /// Activa los campos de filtro de referencias
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
                            this.txtSerial.Enabled = true;
                        else
                            this.txtSerial.Enabled = false;
                        break;                 
                    case "chkRefProvee":
                        if (ctrl.Checked)
                        {
                            this.txtRefProveedor.Enabled = true;
                            this.gvReferencia.Columns[this._unboundPrefix + "RefProveedor"].VisibleIndex = 2;
                            this.gvReferencia.Columns[this._unboundPrefix + "RefProveedor"].Visible = true;
                        }
                        else
                        {
                            this.txtRefProveedor.Enabled = false;
                            this.gvReferencia.Columns[this._unboundPrefix + "RefProveedor"].VisibleIndex = 2;
                            this.gvReferencia.Columns[this._unboundPrefix + "RefProveedor"].Visible = false;
                        }
                        break;
                    case "chkEstado":
                        if (ctrl.Checked)
                            this.cmbEstado.Enabled = true;
                        else
                            this.cmbEstado.Enabled = false;
                        break;
                    case "chkParam1":
                        if (ctrl.Checked)
                            this.masterParam1.EnableControl(true);
                        else
                            this.masterParam1.EnableControl(false);
                        break;
                    case "chkParam2":
                        if (ctrl.Checked)
                            this.masterParam2.EnableControl(true);
                        else
                            this.masterParam2.EnableControl(false);
                        break;
                    case "chkGrupoInv":
                        if (ctrl.Checked)
                        {
                            this.masterGrupoInv.EnableControl(true);
                            this.gvReferencia.Columns[this._unboundPrefix + "GrupoInvID"].VisibleIndex = 2;
                            this.gvReferencia.Columns[this._unboundPrefix + "GrupoInvID"].Visible = true;
                        }
                        else
                        {
                            this.masterGrupoInv.EnableControl(false);
                            this.gvReferencia.Columns[this._unboundPrefix + "GrupoInvID"].VisibleIndex = 2;
                            this.gvReferencia.Columns[this._unboundPrefix + "GrupoInvID"].Visible = false;
                        }       
                        break;
                    case "chkClaseInv":
                        if (ctrl.Checked)
                        {
                            this.masterClaseInv.EnableControl(true);
                            this.gvReferencia.Columns[this._unboundPrefix + "ClaseInvID"].VisibleIndex = 2;
                            this.gvReferencia.Columns[this._unboundPrefix + "ClaseInvID"].Visible = true;
                        }
                        else
                        {
                            this.masterClaseInv.EnableControl(false);
                            this.gvReferencia.Columns[this._unboundPrefix + "ClaseInvID"].VisibleIndex = 2;
                            this.gvReferencia.Columns[this._unboundPrefix + "ClaseInvID"].Visible = false;
                        }   
                        break;
                    case "chkTipoInv":
                        if (ctrl.Checked)
                        {
                            this.masterTipoInv.EnableControl(true);
                            this.gvReferencia.Columns[this._unboundPrefix + "TipoInvID"].VisibleIndex = 2;
                            this.gvReferencia.Columns[this._unboundPrefix + "TipoInvID"].Visible = true;
                        }
                            
                        else
                        {
                             this.masterTipoInv.EnableControl(false);
                             this.gvReferencia.Columns[this._unboundPrefix + "TipoInvID"].VisibleIndex = 2;
                             this.gvReferencia.Columns[this._unboundPrefix + "TipoInvID"].Visible = false;
                        }
                        break;
                     case "chkSerieInv":
                        if (ctrl.Checked)
                        {
                            this.masterSerieInv.EnableControl(true);
                            this.gvReferencia.Columns[this._unboundPrefix + "SerieID"].VisibleIndex = 2;
                            this.gvReferencia.Columns[this._unboundPrefix + "SerieID"].Visible = true;
                        }                           
                        else
                        {
                            this.masterSerieInv.EnableControl(false);
                            this.gvReferencia.Columns[this._unboundPrefix + "SerieID"].VisibleIndex = 2;
                            this.gvReferencia.Columns[this._unboundPrefix + "SerieID"].Visible = false;
                        }                            
                        break;
                    case "chkUnidadInv":
                        if (ctrl.Checked)
                        {
                            this.masterUnidadInv.EnableControl(true);
                            this.gvReferencia.Columns[this._unboundPrefix + "UnidadInvID"].VisibleIndex = 2;
                            this.gvReferencia.Columns[this._unboundPrefix + "UnidadInvID"].Visible = true;
                        }                           
                        else
                        {
                            this.masterUnidadInv.EnableControl(false);
                            this.gvReferencia.Columns[this._unboundPrefix + "UnidadInvID"].VisibleIndex = 2;
                            this.gvReferencia.Columns[this._unboundPrefix + "UnidadInvID"].Visible = false;
                        }                         
                        break;
                    case "chkMaterialInv":
                        if (ctrl.Checked)
                        {
                            this.masterMaterialInv.EnableControl(true);
                            this.gvReferencia.Columns[this._unboundPrefix + "MaterialInvID"].VisibleIndex = 2;
                            this.gvReferencia.Columns[this._unboundPrefix + "MaterialInvID"].Visible = true;
                        }                          
                        else
                        {
                            this.masterMaterialInv.EnableControl(false);
                            this.gvReferencia.Columns[this._unboundPrefix + "MaterialInvID"].VisibleIndex = 2;
                            this.gvReferencia.Columns[this._unboundPrefix + "MaterialInvID"].Visible = false;
                        }                            
                        break;
                    case "chkMarcaInv":
                        if (ctrl.Checked)
                        {
                            this.masterMarcaInv.EnableControl(true);
                            this.gvReferencia.Columns[this._unboundPrefix + "MarcaInvID"].VisibleIndex = 2;
                            this.gvReferencia.Columns[this._unboundPrefix + "MarcaInvID"].Visible = true;
                        }                         
                        else
                        {
                            this.masterMarcaInv.EnableControl(false);
                            this.gvReferencia.Columns[this._unboundPrefix + "MarcaInvID"].VisibleIndex = 2;
                            this.gvReferencia.Columns[this._unboundPrefix + "MarcaInvID"].Visible = false;
                        }                           
                        break;
                    case "chkEmpaqueInv":
                        if (ctrl.Checked)
                        {
                            this.masterEmpaqueInv.EnableControl(true);
                            this.gvReferencia.Columns[this._unboundPrefix + "EmpaqueInvID"].VisibleIndex = 2;
                            this.gvReferencia.Columns[this._unboundPrefix + "EmpaqueInvID"].Visible = true;
                        }                         
                        else
                        {
                            this.masterEmpaqueInv.EnableControl(false);
                            this.gvReferencia.Columns[this._unboundPrefix + "EmpaqueInvID"].VisibleIndex = 2;
                            this.gvReferencia.Columns[this._unboundPrefix + "EmpaqueInvID"].Visible = false;
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaBodega.cs", "chkFilter_CheckedChanged: " + ex.Message));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al salir del control de maestra Y Filtra la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterFilter_Leave(object sender, EventArgs e)
        {
            if (this._listRefSelected.Count > 0)
            {
                Object listFilter;
                List<DTO_inReferencia> referenciaList;
                this._listRefFiltered = new List<DTO_inReferencia>();
                this._listRefFiltered = this._listRefSelected;
                try
                {
                    if (this.chkParam1.Checked && this.masterParam1.ValidID)
                    {
                        #region Parametro1
                        DTO_inReferencia itemRef;
                        referenciaList = new List<DTO_inReferencia>();
                        var listFilterParam1 = this.listSaldosCostosAll.Where(x => x.Parametro1.Value.Equals(this.masterParam1.Value)).ToList();
                        foreach (var item in listFilterParam1)
                        {
                            itemRef = new DTO_inReferencia();
                            listFilter = this._listRefSelected.Where(x => x.ID.Value.Equals(item.inReferenciaID.Value)).First();
                            itemRef = (DTO_inReferencia)listFilter;
                            if (!referenciaList.Exists(x => x.ID.Value.Equals(itemRef.ID.Value)))
                                referenciaList.Add(itemRef);
                        }
                        this._listRefFiltered = referenciaList;
                        this._filterActive = true;
                        #endregion
                    }
                    if (this.chkParam2.Checked && this.masterParam2.ValidID)
                    {
                        #region Parametro2
                        DTO_inReferencia itemRef;
                        referenciaList = new List<DTO_inReferencia>();
                        var listFilterParam2 = this.listSaldosCostosAll.Where(x => x.Parametro2.Value.Equals(this.masterParam2.Value)).ToList();
                        foreach (var item in listFilterParam2)
                        {
                            itemRef = new DTO_inReferencia();
                            listFilter = this._listRefSelected.Where(x => x.ID.Value.Equals(item.inReferenciaID.Value)).First();
                            itemRef = (DTO_inReferencia)listFilter;
                            if (!referenciaList.Exists(x => x.ID.Value.Equals(itemRef.ID.Value)))
                                referenciaList.Add(itemRef);
                        }
                        this._listRefFiltered = referenciaList;
                        this._filterActive = true;
                        #endregion
                    }
                    if (this.chkGrupoInv.Checked && this.masterGrupoInv.ValidID)
                    {
                        #region GrupoInvID
                        listFilter = this._listRefSelected.Where(x => x.GrupoInvID.Value.Equals(this.masterGrupoInv.Value)).ToList();
                        referenciaList = (List<DTO_inReferencia>)listFilter;
                        this._listRefFiltered = referenciaList;
                        this._filterActive = true;
                        #endregion
                    }
                    if (this.chkClaseInv.Checked && this.masterClaseInv.ValidID)
                    {
                        #region ClaseInvID
                        listFilter = this._listRefSelected.Where(x => x.ClaseInvID.Value.Equals(this.masterClaseInv.Value)).ToList();
                        referenciaList = (List<DTO_inReferencia>)listFilter;
                        this._listRefFiltered = referenciaList;
                        this._filterActive = true;
                        #endregion
                    }
                    if (this.chkTipoInv.Checked && this.masterTipoInv.ValidID)
                    {
                        #region TipoInvID
                        listFilter = this._listRefSelected.Where(x => x.TipoInvID.Value.Equals(this.masterTipoInv.Value)).ToList();
                        referenciaList = (List<DTO_inReferencia>)listFilter;
                        this._listRefFiltered = referenciaList;
                        this._filterActive = true;
                        #endregion
                    }
                    if (this.chkSerieInv.Checked && this.masterSerieInv.ValidID)
                    {
                        #region TipoInvID
                        listFilter = this._listRefSelected.Where(x => x.SerieID.Value.Equals(this.masterSerieInv.Value)).ToList();
                        referenciaList = (List<DTO_inReferencia>)listFilter;
                        this._listRefFiltered = referenciaList;
                        this._filterActive = true;
                        #endregion
                    }
                    if (this.chkMaterialInv.Checked && this.masterMaterialInv.ValidID)
                    {
                        #region MaterialInvID
                        listFilter = this._listRefSelected.Where(x => x.MaterialInvID.Value.Equals(this.masterMaterialInv.Value)).ToList();
                        referenciaList = (List<DTO_inReferencia>)listFilter;
                        this._listRefFiltered = referenciaList;
                        this._filterActive = true;
                        #endregion
                    }
                    if (this.chkMarcaInv.Checked && this.masterMarcaInv.ValidID)
                    {
                        #region MarcaInvID
                        listFilter = this._listRefSelected.Where(x => x.MarcaInvID.Value.Equals(this.masterMarcaInv.Value)).ToList();
                        referenciaList = (List<DTO_inReferencia>)listFilter;
                        this._listRefFiltered = referenciaList;
                        this._filterActive = true;
                        #endregion
                    }
                    if (this.chkReferencia.Checked && this.masterReferencia.ValidID)
                    {
                        #region Referencia
                        listFilter = this._listRefSelected.Where(x => x.ID.Value.Contains(this.masterReferencia.Value)).ToList();
                        referenciaList = (List<DTO_inReferencia>)listFilter;
                        this._listRefFiltered = referenciaList;
                        this._filterActive = true;
                        #endregion
                    }
                    if (this.chkUnidadInv.Checked && this.masterUnidadInv.ValidID)
                    {
                        #region UnidadInvID
                        listFilter = this._listRefSelected.Where(x => x.UnidadInvID.Value.Equals(this.masterUnidadInv.Value)).ToList();
                        referenciaList = (List<DTO_inReferencia>)listFilter;
                        this._listRefFiltered = referenciaList;
                        this._filterActive = true;
                        #endregion
                    }
                    if (this.chkEmpaqueInv.Checked && this.masterEmpaqueInv.ValidID)
                    {
                        #region EmpaqueInvID
                        listFilter = this._listRefSelected.Where(x => x.EmpaqueInvID.Value.Equals(this.masterEmpaqueInv.Value)).ToList();
                        referenciaList = (List<DTO_inReferencia>)listFilter;
                        this._listRefFiltered = referenciaList;
                        this._filterActive = true;
                        #endregion
                    }
                    if (this.chkRefProvee.Checked && !string.IsNullOrEmpty(this.txtRefProveedor.Text))
                    {
                        #region Modelo/Ref
                        listFilter = this._listRefSelected.Where(x => x.RefProveedor.Value.Contains(this.txtRefProveedor.Text.ToUpper())).ToList();
                        referenciaList = (List<DTO_inReferencia>)listFilter;
                        this._listRefFiltered = referenciaList;
                        this._filterActive = true;
                        #endregion
                    }
                    if (this.chkSerial.Checked && !string.IsNullOrEmpty(this.txtSerial.Text))
                    {
                        #region Serial
                        DTO_inReferencia itemRef;
                        referenciaList = new List<DTO_inReferencia>();
                        var listFilterSerial = this.listSaldosCostosAll.Where(x => x.SerialID.Value.Equals(this.txtSerial.Text)).ToList();
                        if (listFilterSerial.Count > 0)
                        {
                            itemRef = new DTO_inReferencia();
                            listFilter = this._listRefSelected.Where(x => x.ID.Value.Equals(listFilterSerial[0].inReferenciaID.Value)).First();
                            itemRef = (DTO_inReferencia)listFilter;
                            referenciaList.Add(itemRef);
                            this._listRefFiltered = referenciaList;
                        }
                        else
                            this._listRefFiltered = referenciaList;
                        this._filterActive = true;
                        #endregion
                    }
                    if (this._filterActive)
                    {
                        this.pgGrid.UpdatePageNumber(this._listRefFiltered.Count, false, true, false);
                        var tmp = this._listRefFiltered.Skip((this.pgGrid.PageNumber - 1) * _pageSize).Take(_pageSize).ToList<DTO_inReferencia>();
                        this.gcReferencia.DataSource = null;   
                        this.gcReferencia.DataSource = tmp;                       
                    }
                    else
                    {
                        this.pgGrid.UpdatePageNumber(this._listRefSelected.Count, false, true, false);
                        var tmp = this._listRefSelected.Skip((this.pgGrid.PageNumber - 1) * _pageSize).Take(_pageSize).ToList<DTO_inReferencia>();
                        this.gcReferencia.DataSource = null;   
                        this.gcReferencia.DataSource = tmp;
                    }
                    this.SecurityUser();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaBodega.cs", "masterFilter_Leave: " + ex.Message));
                }
            }
        }

        /// <summary>
        /// Se ejecuta al salir del control y filtra los datos de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void cmbEstado_Leave(object sender, EventArgs e)
        {
            if (this._listRefSelected.Count > 0)
            {
                Object listFilter;
                List<DTO_inReferencia> referenciaList;
                this._listRefFiltered = new List<DTO_inReferencia>();
                this._listRefFiltered = this._listRefSelected;
                try
                {
                    if (this.chkEstado.Checked && !string.IsNullOrEmpty(this.cmbEstado.Text))
                    {
                        #region Estado
                        DTO_inReferencia itemRef;
                        referenciaList = new List<DTO_inReferencia>();
                        var listFilterEstado = this.listSaldosCostosAll.Where(x => x.EstadoInv.Value.Equals(this.cmbEstado.Text)).ToList();
                        foreach (var item in listFilterEstado)
                        {
                            itemRef = new DTO_inReferencia();
                            listFilter = this._listRefSelected.Where(x => x.ID.Value.Equals(item.inReferenciaID.Value)).First();
                            itemRef = (DTO_inReferencia)listFilter;
                            if (!referenciaList.Exists(x => x.ID.Value.Equals(itemRef.ID.Value)))
                                referenciaList.Add(itemRef);
                        }
                        if (listFilterEstado.Count == 0)
                            this._listRefFiltered = referenciaList;
                        this._filterActive = true;
                        #endregion
                    }
                    if (this._filterActive)
                    {
                        this.pgGrid.UpdatePageNumber(this._listRefFiltered.Count, false, true, false);
                        var tmp = this._listRefFiltered.Skip((this.pgGrid.PageNumber - 1) * _pageSize).Take(_pageSize).ToList<DTO_inReferencia>();
                        this.gcReferencia.DataSource = null;   
                        this.gcReferencia.DataSource = tmp;
                    }
                    else
                    {
                        this.pgGrid.UpdatePageNumber(this._listRefSelected.Count, false, true, false);
                        var tmp = this._listRefSelected.Skip((this.pgGrid.PageNumber - 1) * _pageSize).Take(_pageSize).ToList<DTO_inReferencia>();
                        this.gcReferencia.DataSource = null;   
                        this.gcReferencia.DataSource = tmp;
                    }
                    this.SecurityUser();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaBodega.cs", "cmbEstado_Leave: " + ex.Message));
                }
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaBodega.cs", "Form_Enter: " + ex.Message));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaBodega.cs", "Form_Leave: " + ex.Message));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaBodega.cs", "Form_Closing: " + ex.Message));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaBodega.cs", "Form_FormClosed: " + ex.Message));
            }
        }

        #endregion

        #region Eventos Grillas

        /// <summary>
        /// asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvBodega_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e) 
        {
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

            if (fieldName == "VerDetalle")
                e.RepositoryItem = this.editBtnDetail;
            else if (fieldName == "VerMvto")
                e.RepositoryItem = this.editBtnMvto;
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvBodega_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
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
        private void gvBodega_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle >= 0)
                {  
                    this._bodegaCurrent = (DTO_inControlSaldosCostos)this.gvBodega.GetRow(e.FocusedRowHandle);                   
                }
                   
                this.SecurityUser();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaBodega.cs", "gvBodega_FocusedRowChanged: " + ex.Message));
            }
        }

        /// <summary>
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvBodega_RowClick(object sender, RowClickEventArgs e)
        {
            try
            {
                if (e.RowHandle >= 0)
                {
                    this._bodegaCurrent = (DTO_inControlSaldosCostos)this.gvBodega.GetRow(e.RowHandle);
                }

                this.SecurityUser();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaBodega.cs", "gvBodega_FocusedRowChanged: " + ex.Message));
            }
        }

        /// <summary>
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvReferencia_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            #region Trae los saldos de referencias por bodega
            try
            {
                if (this.gvReferencia.DataRowCount > 0 && e.FocusedRowHandle >= 0)
                {
                    this._referenciaCurrent = (DTO_inReferencia)this.gvReferencia.GetRow(e.FocusedRowHandle);
                    this.gvBodega.MoveFirst();
                    this.LoadGridData();   
                    this.SecurityUser();
                }
                else
                    this.gcBodega.DataSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaBodega.cs", "gvReferencia_FocusedRowChanged: " + ex.Message));
            }

            #endregion
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
                ModalConsultaInventario fact = new ModalConsultaInventario(this._referenciaCurrent.ID.Value, this._bodegaCurrent.BodegaID.Value, true, (this._userBodega != null ? this._userBodega.ConsultaCostosInd.Value.Value : false));
                fact.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaBodega.cs", "editBtnDetail_ButtonClick: " + ex.Message));
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
        #endregion

    }
}
