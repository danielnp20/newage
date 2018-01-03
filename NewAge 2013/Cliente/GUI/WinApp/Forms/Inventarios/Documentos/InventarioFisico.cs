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
using System.Text.RegularExpressions;
using System.Threading;
using DevExpress.XtraGrid;
using System.Linq.Expressions;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class InventarioFisico : FormWithToolbar
    {
        #region Delegados

        private delegate void SendToApprove();
        private SendToApprove sendToApproveDelegate;
        /// <summary>
        /// Delegado que actualiza el formulario despues de enviar un documento para aprobacion
        /// </summary>
        private void SendToApproveMethod()
        {
            this.gcFisicoInventario.DataSource = null;
            this.gcSerial.DataSource = null;
            FormProvider.Master.itemSendtoAppr.Enabled = false;
            this.masterBodega.Value = string.Empty;
            this.masterBodega.EnableControl(true);
            this.masterReferencia.EnableControl(false);
            this.masterReferencia.Value = string.Empty;
            this.masterProyecto.Value = string.Empty;
            this.txtObservacion.Text = string.Empty;
            this.masterCentroCto.Value = string.Empty;
            this.cmbEstado.Enabled = false;
            this.cmbEstado.Text = string.Empty;
            this.txtSerial.Text = string.Empty;
            this.txtSerial.BackColor = Color.White;
            this.txtCantKardex.Text = string.Empty;
            this.masterBodega.Focus();
            this.CleanDetailNew();
        }

        private delegate void Save();
        private Save saveDelegate;
        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        private void SaveMethod()
        {
            this.gcFisicoInventario.DataSource = null;
            this.gcSerial.DataSource = null;
            FormProvider.Master.itemSave.Enabled = false;
            this.masterBodega.Value = string.Empty;
            this.masterBodega.EnableControl(true);
            this.masterReferencia.EnableControl(false);
            this.masterReferencia.Value = string.Empty;
            this.masterProyecto.Value = string.Empty;
            this.masterCentroCto.Value = string.Empty;
            this.txtObservacion.Text = string.Empty;
            this.cmbEstado.Enabled = false;
            this.txtSerial.Text = string.Empty;
            this.txtSerial.BackColor = Color.White;
            this.txtCantKardex.Text = string.Empty;
            this.masterBodega.Focus();
            this.CleanDetailNew();
        }
        #endregion

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
        private int numeroDoc = 0;
        private bool _filterActive = false;
        //Variables de data
        private List<DTO_inControlSaldosCostos> listSaldosCostos = null;
        //private List<DTO_inFisicoInventario> _listFisicoInventario = null;
        private List<DTO_inFisicoInventario> _listFisicoInventarioGrid = null;
        //private List<DTO_inFisicoInventario> _listFisicoInventarioFiltered = null;
        private List<DTO_glConsultaFiltro> filtrosParam1 = new List<DTO_glConsultaFiltro>();
        private List<DTO_glConsultaFiltro> filtrosParam2 = new List<DTO_glConsultaFiltro>();
        private List<DTO_inReferencia> _listReferenciaAll = null;
        private DTO_inFisicoInventario _rowCurrent = null;
        private DTO_seUsuarioBodega _userBodega = null;
        private DTO_inBodega bodegaUpdate = null;
        private string referenciaCurrent = string.Empty;
        private string param1xDef = string.Empty;
        private string param2xDef = string.Empty;

        protected DTO_glActividadFlujo _actFlujo = new DTO_glActividadFlujo();
        #endregion

        ///<summary>
        /// Constructor 
        /// </summary>
        public InventarioFisico()
        {
            InitializeComponent();
            try
            {
                this.SetInitParameters();
                this._frmName = this._bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());
                FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
                this._bc.Pagging_Init(this.pgGrid, _pageSize);
                this._bc.Pagging_SetEvent(this.pgGrid, this.pagging_Click);
                this.pgGrid.UpdatePageNumber(0, true, false, false);
                this.AddGridColsSerial();
                this.AddGridColsInvFisico();
                this.saveDelegate = new Save(this.SaveMethod);
                this.sendToApproveDelegate = new SendToApprove(this.SendToApproveMethod);

                #region Carga la info de las actividades
                List<string> actividades = this._bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(this._documentID);

                if (actividades.Count != 1)
                {
                    string msg = this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Gl_DocMultActivities);
                    MessageBox.Show(string.Format(msg, this._documentID.ToString()));
                }
                else
                {
                    string actividadFlujoID = actividades[0];
                    this._actFlujo = (DTO_glActividadFlujo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, actividadFlujoID, true);
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-InventarioFisico.cs", "InventarioFisico"));
            }
        }

        #region Funciones privadas

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddGridColsSerial()
        {
            GridColumn key = new GridColumn();
            key.FieldName = this._unboundPrefix + "Key";
            key.UnboundType = UnboundColumnType.String;
            key.VisibleIndex = 1;
            key.Width = 170;
            key.Visible = true;
            key.OptionsColumn.AllowEdit = false;
            key.OptionsColumn.ShowCaption = false;
            this.gvSerial.Columns.Add(key);
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddGridColsInvFisico()
        {
            GridColumn referenciaP1P2 = new GridColumn();
            referenciaP1P2.FieldName = this._unboundPrefix + "ReferenciaP1P2ID";
            referenciaP1P2.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_ReferenciaP1P2ID");
            referenciaP1P2.UnboundType = UnboundColumnType.String;
            referenciaP1P2.VisibleIndex = 0;
            referenciaP1P2.Width = 90;
            referenciaP1P2.OptionsColumn.AllowFocus = false;
            referenciaP1P2.Visible = true;
            referenciaP1P2.OptionsColumn.AllowEdit = false;
            referenciaP1P2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvFisicoInventario.Columns.Add(referenciaP1P2);

            GridColumn descriptivo = new GridColumn();
            descriptivo.FieldName = this._unboundPrefix + "ReferenciaP1P2Desc";
            descriptivo.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_ReferenciaP1P2Desc");
            descriptivo.UnboundType = UnboundColumnType.String;
            descriptivo.VisibleIndex = 1;
            descriptivo.Width = 250;
            descriptivo.Visible = true;
            descriptivo.OptionsColumn.AllowFocus = false;
            descriptivo.OptionsColumn.AllowEdit = false;
            descriptivo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvFisicoInventario.Columns.Add(descriptivo);

            GridColumn RefProveedor = new GridColumn();
            RefProveedor.FieldName = this._unboundPrefix + "RefProveedor";
            RefProveedor.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_RefProveedor");
            RefProveedor.UnboundType = UnboundColumnType.String;
            RefProveedor.VisibleIndex = 2;
            RefProveedor.Width = 100;
            RefProveedor.Visible = true;
            RefProveedor.OptionsColumn.AllowEdit = false;
            this.gvFisicoInventario.Columns.Add(RefProveedor);

            GridColumn MarcaDesc = new GridColumn();
            MarcaDesc.FieldName = this._unboundPrefix + "MarcaDesc";
            MarcaDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_MarcaDesc");
            MarcaDesc.UnboundType = UnboundColumnType.String;
            MarcaDesc.VisibleIndex = 3;
            MarcaDesc.Width = 80;
            MarcaDesc.Visible = true;
            MarcaDesc.OptionsColumn.AllowEdit = false;
            this.gvFisicoInventario.Columns.Add(MarcaDesc);

            GridColumn ubicacion = new GridColumn();
            ubicacion.FieldName = this._unboundPrefix + "UbicacionID";
            ubicacion.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_UbicacionID");
            ubicacion.UnboundType = UnboundColumnType.String;
            ubicacion.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            ubicacion.AppearanceCell.Options.UseTextOptions = true;
            ubicacion.VisibleIndex = 4;
            ubicacion.Width = 60;
            ubicacion.Visible = true;
            ubicacion.OptionsColumn.AllowEdit = false;
            this.gvFisicoInventario.Columns.Add(ubicacion);

            GridColumn saldo = new GridColumn();
            saldo.FieldName = this._unboundPrefix + "CantKardex";
            saldo.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoExistencia");
            saldo.UnboundType = UnboundColumnType.Decimal;
            saldo.VisibleIndex = 5;
            saldo.Width = 85;
            saldo.Visible = true;
            saldo.OptionsColumn.AllowEdit = false;
            saldo.OptionsColumn.AllowFocus = false;
            saldo.ColumnEdit = this.editValue;
            saldo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.gvFisicoInventario.Columns.Add(saldo);

            GridColumn saldoFisico = new GridColumn();
            saldoFisico.FieldName = this._unboundPrefix + "CantFisico";
            saldoFisico.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoFisico");
            saldoFisico.UnboundType = UnboundColumnType.Decimal;
            saldoFisico.VisibleIndex = 6;
            saldoFisico.Width = 90;
            saldoFisico.Visible = true;
            saldoFisico.ColumnEdit = this.editValue;
            saldoFisico.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            saldoFisico.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            saldoFisico.AppearanceCell.Options.UseTextOptions = true;
            saldoFisico.AppearanceCell.Options.UseFont = true;
            this.gvFisicoInventario.Columns.Add(saldoFisico);

            GridColumn cantAjuste = new GridColumn();
            cantAjuste.FieldName = this._unboundPrefix + "CantAjuste";
            cantAjuste.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_CantAjuste");
            cantAjuste.UnboundType = UnboundColumnType.Decimal;
            cantAjuste.VisibleIndex = 7;
            cantAjuste.Width = 75;
            cantAjuste.Visible = true;
            cantAjuste.OptionsColumn.AllowFocus = false;
            cantAjuste.ColumnEdit = this.editValue;
            cantAjuste.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            cantAjuste.OptionsColumn.AllowEdit = false;
            this.gvFisicoInventario.Columns.Add(cantAjuste);

            GridColumn referencia = new GridColumn();
            referencia.FieldName = this._unboundPrefix + "inReferenciaID";
            referencia.UnboundType = UnboundColumnType.String;
            this.gvFisicoInventario.Columns.Add(referencia);

            GridColumn estadoInv = new GridColumn();
            estadoInv.FieldName = this._unboundPrefix + "EstadoInv";
            estadoInv.UnboundType = UnboundColumnType.String;
            this.gvFisicoInventario.Columns.Add(estadoInv);

            GridColumn activoID = new GridColumn();
            activoID.FieldName = this._unboundPrefix + "ActivoID";
            activoID.UnboundType = UnboundColumnType.String;
            this.gvFisicoInventario.Columns.Add(activoID);

            GridColumn serialID = new GridColumn();
            serialID.FieldName = this._unboundPrefix + "SerialID";
            serialID.UnboundType = UnboundColumnType.String;
            this.gvFisicoInventario.Columns.Add(serialID);

            GridColumn parametro1 = new GridColumn();
            parametro1.FieldName = this._unboundPrefix + "Parametro1";
            parametro1.UnboundType = UnboundColumnType.String;
            this.gvFisicoInventario.Columns.Add(parametro1);

            GridColumn parametro2 = new GridColumn();
            parametro2.FieldName = this._unboundPrefix + "Parametro2";
            parametro2.UnboundType = UnboundColumnType.String;
            this.gvFisicoInventario.Columns.Add(parametro2);
        }

        /// <summary>
        /// Carga las grillas de datos
        /// </summary>
        /// <param name="newRef">indica si es nuevo dato</param>
        private void LoadGrids(bool newRef = false)
        {
            try
            {
                if (newRef)
                {
                    #region Agrega item Nuevo de Inventario Fisico
                    DTO_inReferencia _dtoRefer = (DTO_inReferencia)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, false, this.masterReferenciaNew.Value, true);
                    DTO_MasterBasic _dtoParam1 = (DTO_MasterBasic)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inRefParametro1, false, this.masterParametro1New.Value, true);
                    DTO_MasterBasic _dtoParam2 = (DTO_MasterBasic)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inRefParametro2, false, this.masterParametro2New.Value, true);
                    DTO_inFisicoInventario fisico = new DTO_inFisicoInventario();
                    fisico.EmpresaID.Value = this._bc.AdministrationModel.Empresa.ID.Value;
                    fisico.BodegaID.Value = this.masterBodega.Value;
                    fisico.inReferenciaID.Value = this.masterReferenciaNew.Value;
                    fisico.EstadoInv.Value = Convert.ToByte((this.cmbEstadoNew.SelectedItem as ComboBoxItem).Value);
                    fisico.Parametro1.Value = this.masterParametro1New.Value;
                    fisico.Parametro2.Value = this.masterParametro2New.Value;
                    fisico.ActivoID.Value = 0;
                    fisico.Periodo.Value = this.dtPeriodo.DateTime.Date;
                    fisico.CantKardex.Value = 0;
                    fisico.CantEntradaDoc.Value = 0;
                    fisico.CantFisico.Value = Convert.ToDecimal(this.txtCantidadNew.EditValue, CultureInfo.InvariantCulture);
                    fisico.CantAjuste.Value = Convert.ToDecimal(this.txtCantidadNew.EditValue, CultureInfo.InvariantCulture);
                    fisico.CantSalidaDoc.Value = 0;
                    fisico.CostoExtra.Value = 0;
                    fisico.CostoLocal.Value = Convert.ToDecimal(this.txtCostoReferencia.EditValue,CultureInfo.InvariantCulture);
                    fisico.FobLocal.Value = 0;
                    fisico.FobExtra.Value = 0;
                    fisico.NumeroDoc.Value = this.numeroDoc;
                    fisico.ReferenciaP1P2ID.Value = fisico.inReferenciaID.Value;
                    fisico.ReferenciaP1P2Desc.Value = _dtoRefer.Descriptivo.Value;
                    if (this.masterParametro1New.Value != this.param1xDef.ToUpper() && !string.IsNullOrEmpty(this.masterParametro1New.Value))
                    {
                        fisico.ReferenciaP1P2ID.Value += "-" + fisico.Parametro1.Value;
                        fisico.ReferenciaP1P2Desc.Value = _dtoParam1.Descriptivo.Value;
                    }
                    if (this.masterParametro2New.Value != this.param2xDef.ToUpper() && !string.IsNullOrEmpty(this.masterParametro2New.Value))
                    {
                        fisico.ReferenciaP1P2ID.Value += "-" + fisico.Parametro2.Value;
                        fisico.ReferenciaP1P2Desc.Value = _dtoParam2.Descriptivo.Value;
                    }
                    fisico.SerialID.Value = string.Empty;
                    //fisico.Index = this._listFisicoInventario.Last().Index + 1;
                    //this._listFisicoInventario.Add(fisico);
                    this._listFisicoInventarioGrid.Add(fisico);
                    this.CleanDetailNew();
                    FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.SendtoAppr);
                    FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Edit);
                    #endregion
                }
                this._listFisicoInventarioGrid = this._listFisicoInventarioGrid.OrderBy(x => x.ReferenciaP1P2Desc.Value).ToList();
                var tmp = this._listFisicoInventarioGrid.Skip((this.pgGrid.PageNumber - 1) * _pageSize).Take(_pageSize).ToList<DTO_inFisicoInventario>();
                this.gcFisicoInventario.DataSource = tmp.OrderBy(x => x.ReferenciaP1P2Desc.Value).ToList(); 
                this.pgGrid.UpdatePageNumber(this._listFisicoInventarioGrid.Count, false, true, false);
                if (gvFisicoInventario.DataRowCount > 0)
                {
                    this.masterReferencia.EnableControl(true);
                    this.masterMarcaInv.EnableControl(true);
                    this.txtRefProveed.ReadOnly = false;
                    this.btnNewRef.Checked = true;
                }
                else
                {
                    this.masterReferencia.EnableControl(false);
                    this.masterMarcaInv.EnableControl(false);
                    this.txtRefProveed.ReadOnly = true;
                    this.btnNewRef.Checked = false;
                }
                this._filterActive = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-InventarioFisico.cs", "LoadGrids"));
            }
        }

        /// <summary>
        /// Carga los datos de la grilla
        /// </summary>
        /// <param name="firstTime">Si es la primera vez que carga la grilla</param>
        /// <param name="firstPage">Si debe ir a la primera página</param>
        /// <param name="lastPage">Si debe ir a la ultima página</param>
        private void LoadData(bool cargaSaldosInd)
        {
            this.listSaldosCostos = new List<DTO_inControlSaldosCostos>();
            DTO_inControlSaldosCostos saldosCostos = new DTO_inControlSaldosCostos();
            DTO_inCostosExistencias costos;
            DTO_inFisicoInventario fisico = new DTO_inFisicoInventario();            
            try
            {
                this.btnConteo.Enabled = true;
                this.btnFisico.Enabled = true;
                this.btnDiferencias.Enabled = true;

                if (cargaSaldosInd)
                {
                    this._listFisicoInventarioGrid = new List<DTO_inFisicoInventario>();
                    saldosCostos.BodegaID.Value = this.masterBodega.Value;
                    this.listSaldosCostos = _bc.AdministrationModel.inControlSaldosCostos_GetByParameter(this._documentID, saldosCostos);
                    if (this.listSaldosCostos.Count > 0)
                    {
                        foreach (var item in this.listSaldosCostos)
                        {
                            #region Trae saldo y costos disponibles de inventarios(kardex)
                            costos = new DTO_inCostosExistencias();
                            saldosCostos.inReferenciaID.Value = item.inReferenciaID.Value;
                            saldosCostos.Parametro1.Value = item.Parametro1.Value;
                            saldosCostos.Parametro2.Value = item.Parametro2.Value;
                            saldosCostos.EstadoInv.Value = item.EstadoInv.Value;
                            saldosCostos.ActivoID.Value = item.ActivoID.Value;
                            item.CantidadDisp.Value = this._bc.AdministrationModel.Transaccion_SaldoExistByReferencia(_documentID, saldosCostos, ref costos);
                            item.ValorLocalDisp.Value = costos.CtoLocSaldoIni.Value + costos.CtoLocEntrada.Value - costos.CtoLocSalida.Value;
                            item.ValorExtranjeroDisp.Value = costos.CtoExtSaldoIni.Value + costos.CtoExtEntrada.Value - costos.CtoExtSalida.Value;
                            item.ValorFobLocalDisp.Value = costos.FobLocSaldoIni.Value + costos.FobLocEntrada.Value - costos.FobLocSalida.Value;
                            item.ValorFobExtDisp.Value = costos.FobExtSaldoIni.Value + costos.FobExtEntrada.Value - costos.FobExtSalida.Value;
                            #endregion
                            #region Agrega items de Inventario Fisico
                            if (item.CantidadDisp.Value != 0)
                            {
                                fisico = new DTO_inFisicoInventario();
                                fisico.Index = 0;
                                fisico.NumeroDoc.Value = numeroDoc != 0 ? numeroDoc : 0;
                                fisico.EmpresaID.Value = item.EmpresaID.Value;
                                fisico.BodegaID.Value = item.BodegaID.Value;
                                fisico.inReferenciaID.Value = item.inReferenciaID.Value;
                                fisico.EstadoInv.Value = item.EstadoInv.Value;
                                fisico.Parametro1.Value = item.Parametro1.Value;
                                fisico.Parametro2.Value = item.Parametro2.Value;
                                fisico.ActivoID.Value = item.ActivoID.Value;
                                fisico.SerialID.Value = item.SerialID.Value;
                                fisico.Periodo.Value = this.dtPeriodo.DateTime.Date;
                                fisico.ReferenciaP1P2ID.Value = item.ReferenciaP1P2ID.Value;
                                fisico.ReferenciaP1P2Desc.Value = item.ReferenciaP1P2Desc.Value;
                                fisico.CantKardex.Value = item.CantidadDisp.Value;
                                fisico.CantEntradaDoc.Value = 0;
                                fisico.CantFisico.Value = item.CantidadDisp.Value;
                                fisico.CantAjuste.Value = 0;
                                fisico.CantSalidaDoc.Value = 0;
                                fisico.UbicacionID = item.UbicacionID;
                                fisico.RefProveedor.Value = this._listReferenciaAll.Find(x => x.ID.Value == item.inReferenciaID.Value).RefProveedor.Value;
                                fisico.MarcaInvID.Value = this._listReferenciaAll.Find(x => x.ID.Value == item.inReferenciaID.Value).MarcaInvID.Value;
                                fisico.MarcaDesc.Value = this._listReferenciaAll.Find(x => x.ID.Value == item.inReferenciaID.Value).MarcaInvDesc.Value;
                                fisico.CostoExtra.Value = item.ValorExtranjeroDisp.Value;
                                fisico.CostoLocal.Value = item.ValorLocalDisp.Value;
                                fisico.FobLocal.Value = item.ValorFobLocalDisp.Value;
                                fisico.FobExtra.Value = item.ValorFobExtDisp.Value;
                                this._listFisicoInventarioGrid.Add(fisico);
                            }
                            #endregion
                        }
                        //DTO_SerializedObject resultado = this._bc.AdministrationModel.InventarioFisico_Add(this._documentID, masterBodega.Value, dtFecha.DateTime, out numeroDoc, this._listFisicoInventario);

                        #region Agrega items filtrados de inventario fisico
                        foreach (var item in this._listFisicoInventarioGrid)
                        {
                            DTO_inFisicoInventario fisicoFilter = new DTO_inFisicoInventario();
                            fisicoFilter.Index = item.Index;
                            fisicoFilter.EmpresaID.Value = item.EmpresaID.Value;
                            fisicoFilter.NumeroDoc.Value = numeroDoc != 0 ? numeroDoc : 0;
                            fisicoFilter.BodegaID.Value = item.BodegaID.Value;
                            fisicoFilter.inReferenciaID.Value = item.inReferenciaID.Value;
                            fisicoFilter.EstadoInv.Value = item.EstadoInv.Value;
                            fisicoFilter.Parametro1.Value = item.Parametro1.Value;
                            fisicoFilter.Parametro2.Value = item.Parametro2.Value;
                            fisicoFilter.ActivoID.Value = item.ActivoID.Value;
                            fisicoFilter.SerialID.Value = item.SerialID.Value;
                            fisicoFilter.Periodo.Value = item.Periodo.Value;
                            fisicoFilter.ReferenciaP1P2ID.Value = item.ReferenciaP1P2ID.Value;
                            fisicoFilter.ReferenciaP1P2Desc.Value = item.ReferenciaP1P2Desc.Value;
                            fisicoFilter.CantKardex.Value = item.CantKardex.Value;
                            fisicoFilter.CantEntradaDoc.Value = item.CantEntradaDoc.Value;
                            fisicoFilter.CantFisico.Value = item.CantFisico.Value;
                            fisicoFilter.CantAjuste.Value = item.CantAjuste.Value;
                            fisicoFilter.CantSalidaDoc.Value = item.CantSalidaDoc.Value;
                            fisicoFilter.CostoExtra.Value = item.CostoExtra.Value;
                            fisicoFilter.CostoLocal.Value = item.CostoLocal.Value;
                            fisicoFilter.FobLocal.Value = item.FobLocal.Value;
                            fisicoFilter.FobExtra.Value = item.FobExtra.Value;
                            fisicoFilter.UbicacionID = item.UbicacionID;
                            fisicoFilter.RefProveedor.Value = this._listReferenciaAll.Find(x => x.ID.Value == item.inReferenciaID.Value).RefProveedor.Value;
                            fisicoFilter.MarcaInvID.Value = this._listReferenciaAll.Find(x => x.ID.Value == item.inReferenciaID.Value).MarcaInvID.Value;
                            fisicoFilter.MarcaDesc.Value = this._listReferenciaAll.Find(x => x.ID.Value == item.inReferenciaID.Value).MarcaInvDesc.Value;
                            fisicoFilter.Observacion.Value = item.Observacion.Value;
                            //Agrega a la lista que llena la grilla general
                            int indexRef = this._listFisicoInventarioGrid.FindIndex(x => x.inReferenciaID.Value.Equals(fisicoFilter.inReferenciaID.Value) && x.Parametro1.Value.Equals(fisicoFilter.Parametro1.Value)
                                                                                && x.Parametro2.Value.Equals(fisicoFilter.Parametro2.Value) && x.EstadoInv.Value.Equals(fisicoFilter.EstadoInv.Value));
                            if (indexRef < 0)
                            {
                                #region Trae la cantidad de referencias serialiazadas para filtrar los seriales a mostrar
                                var seriales = this._listFisicoInventarioGrid.Where(x => x.inReferenciaID.Value.Equals(fisicoFilter.inReferenciaID.Value)
                                                                          && x.Parametro1.Value.Equals(fisicoFilter.Parametro1.Value) && x.Parametro2.Value.Equals(fisicoFilter.Parametro2.Value)
                                                                          && x.EstadoInv.Value.Equals(fisicoFilter.EstadoInv.Value) && x.CantAjuste.Value >= 0);

                                List<DTO_inFisicoInventario> serialFilter = new List<DTO_inFisicoInventario>();
                                serialFilter.AddRange(seriales);

                                if (serialFilter.Count > 0 && !string.IsNullOrEmpty(fisicoFilter.SerialID.Value))
                                {
                                    fisicoFilter.ListSeriales = new Dictionary<string, bool>();
                                    fisicoFilter.CantKardex.Value = serialFilter.Count();
                                    fisicoFilter.CantFisico.Value = serialFilter.Count();
                                    foreach (var ser in serialFilter)
                                        fisicoFilter.ListSeriales.Add(ser.SerialID.Value, false);
                                }
                                #endregion
                                this._listFisicoInventarioGrid.Add(fisicoFilter);
                            }
                        }
                        #endregion
                        DTO_SerializedObject result = _bc.AdministrationModel.InventarioFisico_Add(this._documentID, masterBodega.Value, dtFecha.DateTime, out numeroDoc, this._listFisicoInventarioGrid);
                        if (result.GetType() == typeof(DTO_TxResult))
                        {
                            DTO_TxResult res = (DTO_TxResult)result;
                            if (res.Result == ResultValue.NOK)
                                MessageBox.Show(res.ResultMessage);
                        }
                    }
                    else
                        MessageBox.Show("No hay saldos en la bodega seleccionada");
                }
                else
                {
                    for (int i = 0; i < this._listFisicoInventarioGrid.Count; i++)
                    {
                        if (this.numeroDoc == 0)
                            this.numeroDoc = this._listFisicoInventarioGrid[i].NumeroDoc.Value.Value;
                        this._listFisicoInventarioGrid[i].Index = i;
                        #region Agrega items filtrados de inventario fisico
                        DTO_inFisicoInventario fisicoFilter = new DTO_inFisicoInventario();
                        fisicoFilter.Index = this._listFisicoInventarioGrid[i].Index;
                        fisicoFilter.EmpresaID.Value = this._listFisicoInventarioGrid[i].EmpresaID.Value;
                        fisicoFilter.BodegaID.Value = this._listFisicoInventarioGrid[i].BodegaID.Value;
                        fisicoFilter.inReferenciaID.Value = this._listFisicoInventarioGrid[i].inReferenciaID.Value;
                        fisicoFilter.EstadoInv.Value = this._listFisicoInventarioGrid[i].EstadoInv.Value;
                        fisicoFilter.Parametro1.Value = this._listFisicoInventarioGrid[i].Parametro1.Value;
                        fisicoFilter.Parametro2.Value = this._listFisicoInventarioGrid[i].Parametro2.Value;
                        fisicoFilter.ActivoID.Value = this._listFisicoInventarioGrid[i].ActivoID.Value;
                        fisicoFilter.SerialID.Value = this._listFisicoInventarioGrid[i].SerialID.Value;
                        fisicoFilter.Periodo.Value = this._listFisicoInventarioGrid[i].Periodo.Value;
                        fisicoFilter.ReferenciaP1P2ID.Value = this._listFisicoInventarioGrid[i].ReferenciaP1P2ID.Value;
                        fisicoFilter.ReferenciaP1P2Desc.Value = this._listFisicoInventarioGrid[i].ReferenciaP1P2Desc.Value;
                        fisicoFilter.CantKardex.Value = this._listFisicoInventarioGrid[i].CantKardex.Value;
                        fisicoFilter.CantEntradaDoc.Value = this._listFisicoInventarioGrid[i].CantEntradaDoc.Value;
                        fisicoFilter.CantFisico.Value = this._listFisicoInventarioGrid[i].CantFisico.Value;
                        fisicoFilter.UbicacionID = this._listFisicoInventarioGrid[i].UbicacionID;
                        fisicoFilter.RefProveedor.Value = this._listFisicoInventarioGrid[i].RefProveedor.Value;
                        fisicoFilter.MarcaInvID.Value = this._listFisicoInventarioGrid[i].MarcaInvID.Value;
                        fisicoFilter.MarcaDesc.Value = this._listFisicoInventarioGrid[i].MarcaDesc.Value;
                        fisicoFilter.CantAjuste.Value = this._listFisicoInventarioGrid[i].CantAjuste.Value;
                        fisicoFilter.CantSalidaDoc.Value = this._listFisicoInventarioGrid[i].CantSalidaDoc.Value;
                        fisicoFilter.CostoExtra.Value = this._listFisicoInventarioGrid[i].CostoExtra.Value;
                        fisicoFilter.CostoLocal.Value = this._listFisicoInventarioGrid[i].CostoLocal.Value;
                        fisicoFilter.FobLocal.Value = this._listFisicoInventarioGrid[i].FobLocal.Value;
                        fisicoFilter.FobExtra.Value = this._listFisicoInventarioGrid[i].FobExtra.Value;
                        fisicoFilter.Observacion.Value = this._listFisicoInventarioGrid[i].Observacion.Value;
                        //Agrega a la lista que llena la grilla general resumiendo seriales
                        int indexRef = this._listFisicoInventarioGrid.FindIndex(x => x.inReferenciaID.Value.Equals(fisicoFilter.inReferenciaID.Value) && x.Parametro1.Value.Equals(fisicoFilter.Parametro1.Value)
                                                                                && x.Parametro2.Value.Equals(fisicoFilter.Parametro2.Value) && x.EstadoInv.Value.Equals(fisicoFilter.EstadoInv.Value));
                        if (indexRef < 0)
                        {
                            #region Trae la cantidad de referencias serialiazadas para filtrar los seriales a mostrar
                            var seriales = this._listFisicoInventarioGrid.Where(x => x.inReferenciaID.Value.Equals(fisicoFilter.inReferenciaID.Value)
                                                                      && x.Parametro1.Value.Equals(fisicoFilter.Parametro1.Value) && x.Parametro2.Value.Equals(fisicoFilter.Parametro2.Value)
                                                                      && x.EstadoInv.Value.Equals(fisicoFilter.EstadoInv.Value) && x.CantAjuste.Value >= 0);

                            List<DTO_inFisicoInventario> serialFilter = new List<DTO_inFisicoInventario>();
                            serialFilter.AddRange(seriales);

                            if (serialFilter.Count > 0 && !string.IsNullOrEmpty(fisicoFilter.SerialID.Value))
                            {
                                fisicoFilter.ListSeriales = new Dictionary<string, bool>();
                                fisicoFilter.CantKardex.Value = serialFilter.Count();
                                fisicoFilter.CantFisico.Value = serialFilter.Count();
                                foreach (var item in serialFilter)
                                    fisicoFilter.ListSeriales.Add(item.SerialID.Value, false);
                            }
                            #endregion
                            this._listFisicoInventarioGrid.Add(fisicoFilter);
                        }
                        #endregion
                    }
                    saldosCostos.BodegaID.Value = this.masterBodega.Value;
                    this.listSaldosCostos = _bc.AdministrationModel.inControlSaldosCostos_GetByParameter(this._documentID, saldosCostos);
                }
                this._listFisicoInventarioGrid = this._listFisicoInventarioGrid.OrderBy(x => x.ReferenciaP1P2Desc.Value).ToList();
                this.LoadGrids();
                this.masterProyecto.Value = this.bodegaUpdate != null ? this.bodegaUpdate.ProyectoID.Value : string.Empty;
                this.masterCentroCto.Value = this.bodegaUpdate != null ? this.bodegaUpdate.CentroCostoID.Value : string.Empty; 
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-InventarioFisico.cs", "LoadData"));
            }
        }

        private List<DTO_inFisicoInventario> filterData()
        {
            List<DTO_inFisicoInventario> list = new List<DTO_inFisicoInventario>();

            if (string.IsNullOrEmpty(this.cmbEstado.Text) && string.IsNullOrEmpty(this.txtSerial.Text))
                list = this._listFisicoInventarioGrid.Where(x => x.inReferenciaID.Value.Equals(this.masterReferencia.Value)).ToList();
            else if (string.IsNullOrEmpty(this.cmbEstado.Text) && string.IsNullOrEmpty(this.txtSerial.Text))
                list = this._listFisicoInventarioGrid.Where(x => x.inReferenciaID.Value.Equals(this.masterReferencia.Value)).ToList();
            else if (string.IsNullOrEmpty(this.cmbEstado.Text) && string.IsNullOrEmpty(this.txtSerial.Text))
                list = this._listFisicoInventarioGrid.Where(x => x.inReferenciaID.Value.Equals(this.masterReferencia.Value)).ToList();
            else if (string.IsNullOrEmpty(this.cmbEstado.Text) && string.IsNullOrEmpty(this.txtSerial.Text))
                list = this._listFisicoInventarioGrid.Where(x => x.inReferenciaID.Value.Equals(this.masterReferencia.Value)).ToList();
            else if (!string.IsNullOrEmpty(this.cmbEstado.Text) && string.IsNullOrEmpty(this.txtSerial.Text))
                list = this._listFisicoInventarioGrid.Where(x => x.inReferenciaID.Value.Equals(this.masterReferencia.Value) &&
                                                            x.EstadoInv.Value == Convert.ToByte((this.cmbEstado.SelectedItem as ComboBoxItem).Value)).ToList();
            else if (!string.IsNullOrEmpty(this.cmbEstado.Text) && !string.IsNullOrEmpty(this.txtSerial.Text))
                list = this._listFisicoInventarioGrid.Where(x => x.inReferenciaID.Value.Equals(this.masterReferencia.Value) &&
                                                            x.EstadoInv.Value == Convert.ToByte((this.cmbEstado.SelectedItem as ComboBoxItem).Value) &&
                                                            x.SerialID.Value == this.txtSerial.Text).ToList();
            else if (string.IsNullOrEmpty(this.cmbEstado.Text) && !string.IsNullOrEmpty(this.txtSerial.Text))
                list = this._listFisicoInventarioGrid.Where(x => x.inReferenciaID.Value.Equals(this.masterReferencia.Value) &&
                                                            x.SerialID.Value.Equals(this.txtSerial.Text)).ToList();
            else if (!string.IsNullOrEmpty(this.cmbEstado.Text) && !string.IsNullOrEmpty(this.txtSerial.Text))
                list = this._listFisicoInventarioGrid.Where(x => x.inReferenciaID.Value.Equals(this.masterReferencia.Value) &&
                                                            x.EstadoInv.Value == Convert.ToByte((this.cmbEstado.SelectedItem as ComboBoxItem).Value) &&
                                                            x.SerialID.Value.Equals(this.txtSerial.Text)).ToList();
            else if (!string.IsNullOrEmpty(this.cmbEstado.Text) && !string.IsNullOrEmpty(this.txtSerial.Text))
                list = this._listFisicoInventarioGrid.Where(x => x.inReferenciaID.Value.Equals(this.masterReferencia.Value) &&
                                                            x.EstadoInv.Value == Convert.ToByte((this.cmbEstado.SelectedItem as ComboBoxItem).Value) &&
                                                            x.SerialID.Value.Equals(this.txtSerial.Text)).ToList();
            else if (string.IsNullOrEmpty(this.cmbEstado.Text) && !string.IsNullOrEmpty(this.txtSerial.Text))
                list = this._listFisicoInventarioGrid.Where(x => x.inReferenciaID.Value.Equals(this.masterReferencia.Value)  &&
                                                            x.SerialID.Value.Equals(this.txtSerial.Text)).ToList();
            else if (!string.IsNullOrEmpty(this.cmbEstado.Text) && string.IsNullOrEmpty(this.txtSerial.Text))
                list = this._listFisicoInventarioGrid.Where(x => x.inReferenciaID.Value.Equals(this.masterReferencia.Value) &&
                                                            x.EstadoInv.Value == Convert.ToByte((this.cmbEstado.SelectedItem as ComboBoxItem).Value)).ToList();
            else if (string.IsNullOrEmpty(this.cmbEstado.Text) && !string.IsNullOrEmpty(this.txtSerial.Text))
                list = this._listFisicoInventarioGrid.Where(x => x.inReferenciaID.Value.Equals(this.masterReferencia.Value) &&
                                                            x.SerialID.Value.Equals(this.txtSerial.Text)).ToList();
            else if ( !string.IsNullOrEmpty(this.cmbEstado.Text) && string.IsNullOrEmpty(this.txtSerial.Text))
                list = this._listFisicoInventarioGrid.Where(x => x.inReferenciaID.Value.Equals(this.masterReferencia.Value)  &&
                                                            x.EstadoInv.Value == Convert.ToByte((this.cmbEstado.SelectedItem as ComboBoxItem).Value)).ToList();
            else if ( !string.IsNullOrEmpty(this.cmbEstado.Text) && string.IsNullOrEmpty(this.txtSerial.Text))
                list = this._listFisicoInventarioGrid.Where(x => x.inReferenciaID.Value.Equals(this.masterReferencia.Value) &&
                                                            x.EstadoInv.Value == Convert.ToByte((this.cmbEstado.SelectedItem as ComboBoxItem).Value)).ToList();

            if(this.masterMarcaInv.ValidID)
                list = this._listFisicoInventarioGrid.Where(x => x.MarcaInvID.Value.Equals(this.masterMarcaInv.Value)).ToList();
            if (!string.IsNullOrEmpty(this.txtRefProveed.Text))
                list = this._listFisicoInventarioGrid.Where(x => x.RefProveedor.Value.Equals(this.txtRefProveed.Text)).ToList();
            return list;
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            this._documentID = AppDocuments.InventarioFisico;
            this._frmModule = ModulesPrefix.@in;

            //Inicializa Controles
            _bc.InitMasterUC(this.masterBodega, AppMasters.inBodega, true, true, false, false);
            _bc.InitMasterUC(this.masterReferencia, AppMasters.inReferencia, true, true, false, false);
            _bc.InitMasterUC(this.masterTipoMovEntrada, AppMasters.inMovimientoTipo, true, true, false, false);
            _bc.InitMasterUC(this.masterTipoMovSalida, AppMasters.inMovimientoTipo, true, true, false, false);
            _bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true, true, false, false);
            _bc.InitMasterUC(this.masterCentroCto, AppMasters.coCentroCosto, true, true, false, false);
            _bc.InitMasterUC(this.masterReferenciaNew, AppMasters.inReferencia, true, true, false, false);
            _bc.InitMasterUC(this.masterParametro1New, AppMasters.inRefParametro1, true, true, false, false);
            _bc.InitMasterUC(this.masterParametro2New, AppMasters.inRefParametro2, true, true, false, false);
            _bc.InitMasterUC(this.masterMarcaInv, AppMasters.inMarca, true, true, false, false);

            this.masterReferencia.EnableControl(false);
            this.masterTipoMovEntrada.EnableControl(false);
            this.masterTipoMovSalida.EnableControl(false);
            this.masterProyecto.EnableControl(false);
            this.masterCentroCto.EnableControl(false);
            this.masterParametro1New.EnableControl(false);
            this.masterParametro2New.EnableControl(false);
            this.masterMarcaInv.EnableControl(false);
            this.txtRefProveed.ReadOnly = true;
            this.pnNewRef.Enabled = false;
            this.cmbEstadoNew.Enabled = false;
            this.dtPeriodo.Enabled = false;
            //Trae informacion por defecto
            this.masterTipoMovEntrada.Value = _bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_TipoMovEntradaInvFisico);
            this.masterTipoMovSalida.Value = _bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_TipoMovSalidaInvFisico);
            this.param1xDef = _bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Parametro1xDefecto);
            this.param2xDef = _bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Parametro2xDefecto);
            this.masterParametro1New.Value = this.param1xDef;
            this.masterParametro2New.Value = this.param2xDef;

            //Control de fecha
            this.dtPeriodo.DateTime = Convert.ToDateTime(this._bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Periodo));
            if (this.dtPeriodo.DateTime.Month == DateTime.Now.Month)
                this.dtFecha.DateTime = DateTime.Now;
            else
                this.dtFecha.DateTime = new DateTime(this.dtPeriodo.DateTime.Year, this.dtPeriodo.DateTime.Month, DateTime.DaysInMonth(this.dtPeriodo.DateTime.Year, this.dtPeriodo.DateTime.Month));

            //Llena los combos
            TablesResources.GetTableResources(this.cmbEstado, typeof(EstadoInv));
            TablesResources.GetTableResources(this.cmbEstadoNew, typeof(EstadoInv));
            this.cmbEstadoNew.SelectedItem = this.cmbEstadoNew.GetItem(((byte)EstadoInv.Activo).ToString());
        }

        /// <summary>
        /// Limpia los controles del detalle de nueva referencia
        /// </summary>
        private void CleanDetailNew()
        {
            try
            {
                _bc.InitMasterUC(this.masterParametro1New, AppMasters.inRefParametro1, true, true, false, false);
                _bc.InitMasterUC(this.masterParametro2New, AppMasters.inRefParametro2, true, true, false, false);
                this.txtSerialNew.BackColor = Color.White;
                this.cmbEstadoNew.BackColor = Color.White;
                this.masterReferenciaNew.Value = string.Empty;
                this.masterParametro1New.Value = this.param1xDef;
                this.masterParametro2New.Value = this.param2xDef;
                this.cmbEstadoNew.SelectedItem = this.cmbEstadoNew.GetItem(((byte)EstadoInv.Activo).ToString());
                this.txtCantidadNew.Text = "0";
                this.txtCostoReferencia.EditValue = "0";
                this.btnNewRef.Checked = false;
                #region Limpia Costos
                this.txtValorTotalME.EditValue = 0;
                this.txtValorTotalML.EditValue = 0;
                this.txtValorUnitME.EditValue = 0;
                this.txtValorUnitML.EditValue = 0;
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-InventarioFisico.cs", "CleanDetailNew"));
            }
        }
        
        #endregion

        #region Eventos Controles Header

        /// <summary>
        /// Evento que se ejecuta al darle click al paginador
        /// </summary>
        /// <param name="sender">Objeto que inicia el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void pagging_Click(object sender, System.EventArgs e)
        {
            try
            {
                var tmp = this._listFisicoInventarioGrid.Skip((this.pgGrid.PageNumber - 1) * this._pageSize).Take(this._pageSize).ToList<DTO_inFisicoInventario>();
                this.pgGrid.UpdatePageNumber(this._listFisicoInventarioGrid.Count, false, false, false);
                this.gvFisicoInventario.MoveFirst();
                this.gcFisicoInventario.DataSource = null;
                this.gcFisicoInventario.DataSource = tmp.OrderBy(x=>x.ReferenciaP1P2Desc.Value).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-InventarioFisico.cs", "pagging_Click"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al salir del control de maestra
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>+
        private void masterHeader_Leave(object sender, EventArgs e)
        {
            ControlsUC.uc_MasterFind master = (ControlsUC.uc_MasterFind)sender;
            object result;
            decimal totalFiltered = 0;
            try
            {
                this._listReferenciaAll = new List<DTO_inReferencia>();
                long count = this._bc.AdministrationModel.MasterSimple_Count(AppMasters.inReferencia, null, null, true);
                this._listReferenciaAll = this._bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.inReferencia, count, 1, null, null, true).Cast<DTO_inReferencia>().ToList();

                if (master.ColId != "BodegaID" && !this._filterActive)
                    {
                        //this._listFisicoInventarioFiltered = new List<DTO_inFisicoInventario>();
                        //this._listFisicoInventarioFiltered = this._listFisicoInventarioGrid;
                    }
                    switch (master.ColId)
                    {
                        case "BodegaID":
                        if (master.ValidID && !this._filterActive)
                        {
                            #region Variables
                            this.bodegaUpdate = (DTO_inBodega)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, false, this.masterBodega.Value, true);
                            this._listFisicoInventarioGrid = new List<DTO_inFisicoInventario>();
                            DTO_inFisicoInventario fisico = new DTO_inFisicoInventario();
                            #endregion
                            #region Carga los persmisos del user para Consultar Costos
                            Dictionary<string, string> keysUserBodega = new Dictionary<string, string>();
                            keysUserBodega.Add("seUsuarioID", this._bc.AdministrationModel.User.ReplicaID.Value.ToString());
                            keysUserBodega.Add("BodegaID", master.Value);
                            this._userBodega = (DTO_seUsuarioBodega)this._bc.GetMasterComplexDTO(AppMasters.seUsuarioBodega, keysUserBodega, true);
                            if (this._userBodega != null && this._userBodega.ConsultaCostosInd.Value.Value)
                                this.pnlValor.Visible = true;
                            else
                                this.pnlValor.Visible = false;
                            #endregion
                            #region Trae los saldos de referencias por bodega digitada
                            fisico.BodegaID.Value = this.masterBodega.Value;
                            fisico.Periodo.Value = this.dtPeriodo.DateTime;
                            this._listFisicoInventarioGrid = this._bc.AdministrationModel.InventarioFisico_Get(this._documentID, fisico);
                            if (this._listFisicoInventarioGrid.Count == 0)
                            {
                                this.btnCargaSaldos.Enabled = true;
                                this.btnUpdateSaldos.Enabled = false;
                            }                                  
                            else
                            {
                                DTO_glDocumentoControl _docControl = this._bc.AdministrationModel.glDocumentoControl_GetByID(this._listFisicoInventarioGrid[0].NumeroDoc.Value.Value);
                                if (_docControl != null && _docControl.Estado.Value == (byte)EstadoDocControl.Aprobado)
                                {
                                    this._listFisicoInventarioGrid.Clear();
                                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, "Esta bodega ya tiene aprobado un inventario físico para este periodo"));
                                }
                                else
                                {
                                    this.LoadData(false);
                                    FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Edit);
                                }
                                this.btnUpdateSaldos.Enabled = true;
                            }
                            #endregion
                            #region Trae datos por defecto

                            foreach (var item in this._listFisicoInventarioGrid)
                                totalFiltered += item.CantKardex.Value.Value;
                            this.txtCantKardex.EditValue = totalFiltered;
                            #endregion
                            this.masterBodega.EnableControl(false);
                            this.cmbEstado.Text = string.Empty;
                            }
                            break;
                        case "inReferenciaID":
                            if (master.ValidID && this.referenciaCurrent != master.Value)
                            {
                                if (this.referenciaCurrent != master.Value)
                                {
                                    this.cmbEstado.Text = string.Empty;
                                    this.txtSerial.Text = string.Empty;
                                }
                                this.referenciaCurrent = master.Value;
                                DTO_inReferencia referencia = (DTO_inReferencia)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, false, this.referenciaCurrent, true);
                                DTO_inRefTipo _tipoInv = (DTO_inRefTipo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inRefTipo, false, referencia.TipoInvID.Value, true);
                                #region Referencia serializada
                                if (_tipoInv.SerializadoInd.Value.Value)
                                    this.txtSerial.BackColor = Color.LightBlue;
                                else
                                    this.txtSerial.BackColor = Color.White;
                                this.txtSerial.Enabled = _tipoInv.SerializadoInd.Value.Value;
                                #endregion
                                this.cmbEstado.Enabled = true;
                                this.cmbEstado.BackColor = Color.LightBlue;
                                //result = this.filterData();
                                //this._listFisicoInventarioFiltered = (List<DTO_inFisicoInventario>)result;
                                //foreach (var item in this._listFisicoInventarioFiltered)
                                //    totalFiltered += item.CantKardex.Value.Value;
                                this.txtCantKardex.EditValue = totalFiltered;
                                this._filterActive = true;
                                this.btnAddRef.Enabled = !this._filterActive;
                            }
                            break;

                        case "Parametro1ID":
                            result = this.filterData();
                            //this._listFisicoInventarioFiltered = (List<DTO_inFisicoInventario>)result;
                            //foreach (var item in this._listFisicoInventarioFiltered)
                            //    totalFiltered += item.CantKardex.Value.Value;
                            this.txtCantKardex.EditValue = totalFiltered;
                            this._filterActive = true;
                            break;
                        case "Parametro2ID":
                            result = this.filterData();
                            //this._listFisicoInventarioFiltered = (List<DTO_inFisicoInventario>)result;
                            //foreach (var item in this._listFisicoInventarioFiltered)
                            //    totalFiltered += item.CantKardex.Value.Value;
                            this.txtCantKardex.EditValue = totalFiltered;
                            this._filterActive = true;
                            break;
                    case "MarcaInvID":
                        result = this.filterData();
                        //this._listFisicoInventarioFiltered = (List<DTO_inFisicoInventario>)result;
                        //foreach (var item in this._listFisicoInventarioFiltered)
                        //    totalFiltered += item.CantKardex.Value.Value;
                        this.txtCantKardex.EditValue = totalFiltered;
                        this._filterActive = true;
                        break;
                }
                    if (master.ValidID && !master.ColId.Equals("BodegaID"))
                    {
                        //this.gvFisicoInventario.MoveFirst();
                        //this.gcFisicoInventario.DataSource = null;
                        //this.gcFisicoInventario.DataSource = this._listFisicoInventarioFiltered.OrderBy(x => x.ReferenciaP1P2Desc.Value).ToList(); 
                        //this.pgGrid.UpdatePageNumber(this._listFisicoInventarioFiltered.Count, false, true, false);
                    }
                    if (string.IsNullOrEmpty(this.masterReferencia.Value) && string.IsNullOrEmpty(this.cmbEstado.Text) && string.IsNullOrEmpty(this.txtSerial.Text) && this._filterActive)
                    {
                        this.LoadData(false);
                        this._filterActive = false;
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-InventarioFisico.cs", "masterHeader_Leave")); ;
            }
        }

        /// <summary>
        /// Boton para cargar los saldos de referencias de la bodega en la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento </param>
        private void btnCargaSaldos_Click(object sender, EventArgs e)
        {
            this.LoadData(true);
            #region Actualiza la maestra de Bodega
            this.bodegaUpdate.InvFisicoInd.Value = true;
            this.bodegaUpdate.NumeroDocINI.Value = this.numeroDoc;
            this._bc.AdministrationModel.MasterSimple_Update(AppMasters.inBodega, this.bodegaUpdate);
            FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Edit);
            #endregion
        }

        /// <summary>
        /// Se ejecuta cuando deja el control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtHeader_Leave(object sender, EventArgs e)
        {
            TextEdit txtControl = (TextEdit)sender;
            object result;
            decimal totalFiltered = 0;
            if (!this._filterActive)
            {
                //this._listFisicoInventarioFiltered = new List<DTO_inFisicoInventario>();
                //this._listFisicoInventarioFiltered = this._listFisicoInventarioGrid;
            }
            try
            {
                switch (txtControl.Name)
                {
                    case "txtSerial":
                        result = this.filterData();
                        //this._listFisicoInventarioFiltered = (List<DTO_inFisicoInventario>)result;
                        //foreach (var item in this._listFisicoInventarioFiltered)
                        //    totalFiltered += item.CantKardex.Value.Value;
                        this.txtCantKardex.EditValue = totalFiltered;
                        this._filterActive = true;
                        break;
                    case "txtRefProveedor":
                        result = this.filterData();
                        //this._listFisicoInventarioFiltered = (List<DTO_inFisicoInventario>)result;
                        //foreach (var item in this._listFisicoInventarioFiltered)
                        //    totalFiltered += item.CantKardex.Value.Value;
                        this.txtCantKardex.EditValue = totalFiltered;
                        this._filterActive = true;
                        break;
                }
                //this.gvFisicoInventario.MoveFirst();
                //this.gcFisicoInventario.DataSource = null;
                //this.gcFisicoInventario.DataSource = this._listFisicoInventarioFiltered.OrderBy(x => x.ReferenciaP1P2Desc.Value).ToList(); ;
                //this.pgGrid.UpdatePageNumber(this._listFisicoInventarioFiltered.Count, false, true, false);
                if (string.IsNullOrEmpty(this.masterReferencia.Value) && string.IsNullOrEmpty(this.cmbEstado.Text) && string.IsNullOrEmpty(this.txtSerial.Text) && this._filterActive)
                {
                    this.LoadData(false);
                    this._filterActive = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-InventarioFisico.cs", "txtHeader_Leave"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando deja el control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void cmbEstadoHeader_Leave(object sender, EventArgs e)
        {
            ComboBoxEx cmbControl = (ComboBoxEx)sender;
            object result;
            decimal totalFiltered = 0;
            //if (!this._filterActive)
            //{
            //    this._listFisicoInventarioFiltered = new List<DTO_inFisicoInventario>();
            //    this._listFisicoInventarioFiltered = this._listFisicoInventarioGrid;
            //}
            try
            {
                switch (cmbControl.Name)
                {
                    case "cmbEstado":
                        result = this.filterData();
                        //this._listFisicoInventarioFiltered = (List<DTO_inFisicoInventario>)result;
                        //foreach (var item in this._listFisicoInventarioFiltered)
                        //    totalFiltered += item.CantKardex.Value.Value;
                        this.txtCantKardex.EditValue = totalFiltered;
                        this._filterActive = true;
                        break;
                }
                //this.gvFisicoInventario.MoveFirst();
                //this.gcFisicoInventario.DataSource = null;
                //this.gcFisicoInventario.DataSource = this._listFisicoInventarioFiltered.OrderBy(x => x.ReferenciaP1P2Desc.Value).ToList(); 
                //this.pgGrid.UpdatePageNumber(this._listFisicoInventarioFiltered.Count, false, true, false);
                if (string.IsNullOrEmpty(this.masterReferencia.Value) && string.IsNullOrEmpty(this.cmbEstado.Text) && string.IsNullOrEmpty(this.txtSerial.Text) && this._filterActive)
                {
                    this.LoadData(false);
                    this._filterActive = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-InventarioFisico.cs", "cmbEstadoHeader_Leave"));
            }
        }

        /// <summary>
        /// Borra los saldos de la bodega actual ya guardados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateSaldos_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Desea realmente eliminar los saldos de inventario fisico actuales y recargar los saldos de al bodega?", "Advertencia", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    DTO_inFisicoInventario filterFisico = new DTO_inFisicoInventario();
                    filterFisico.BodegaID.Value = this.masterBodega.Value;
                    filterFisico.Periodo.Value = this.dtPeriodo.DateTime;
                    this._listFisicoInventarioGrid = this._bc.AdministrationModel.InventarioFisico_Get(this._documentID, filterFisico);
                    DTO_glDocumentoControl _docControl = this._bc.AdministrationModel.glDocumentoControl_GetByID(this._listFisicoInventarioGrid.Count > 0?this._listFisicoInventarioGrid[0].NumeroDoc.Value.Value : 0);
                    if (_docControl != null && _docControl.Estado.Value == (byte)EstadoDocControl.Aprobado)
                    {
                        this._listFisicoInventarioGrid.Clear();
                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, "Esta bodega ya tiene aprobado un inventario físico para este periodo"));
                        return;
                    }

                    decimal totalFiltered = 0;
                    this._listFisicoInventarioGrid = new List<DTO_inFisicoInventario>();
                    DTO_inFisicoInventario fisico = new DTO_inFisicoInventario();
                    filterFisico.BodegaID.Value = this.masterBodega.Value;
                    filterFisico.Periodo.Value = this.dtPeriodo.DateTime;
                    this.LoadData(true);
                    FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Edit);
                    FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.SendtoAppr);
                    foreach (var item in this._listFisicoInventarioGrid)
                    {
                        totalFiltered += item.CantKardex.Value.Value;
                        item.EmpresaID.Value = this._bc.AdministrationModel.Empresa.ID.Value;
                    }

                    this.txtCantKardex.EditValue = totalFiltered;

                    this.gvFisicoInventario.MoveFirst();
                    this.gcFisicoInventario.DataSource = null;

                    this.LoadData(false);
                    this._filterActive = false;
                    FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Edit);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-InventarioFisico.cs", "btnDeleteSaldos_Click"));
                }
            }
        }



        #endregion

        #region Eventos para los reportes
        /// <summary>
        /// Evento del boton ipnConteo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConteo_Click(object sender, EventArgs e)
        {
            this._bc.AdministrationModel.ReporteInvFisico(this._documentID,this.masterBodega.Value,this.dtPeriodo.DateTime, _listFisicoInventarioGrid, InventarioFisicoReportType.Conteo);
        }

        /// <summary>
        /// Evento del boton ipnFisico
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pnFisico_Click(object sender, EventArgs e)
        {
            this._bc.AdministrationModel.ReporteInvFisico(this._documentID, this.masterBodega.Value, this.dtPeriodo.DateTime, _listFisicoInventarioGrid, InventarioFisicoReportType.Fisico);
        }

        /// <summary>
        /// Evento del boton ipnDiferencias
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pnDiferencias_Click(object sender, EventArgs e)
        {
            this._bc.AdministrationModel.ReporteInvFisico(this._documentID, this.masterBodega.Value, this.dtPeriodo.DateTime, _listFisicoInventarioGrid, InventarioFisicoReportType.Diferencia);
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
                FormProvider.Master.itemUpdate.Visible = false;
                FormProvider.Master.itemSave.Visible = true;
                FormProvider.Master.itemSendtoAppr.Visible = true;
                FormProvider.Master.itemNew.Visible = true;
                FormProvider.Master.itemNew.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Add);
                FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Edit);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-InventarioFisico.cs", "Form_Enter"));
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
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-InventarioFisico.cs", "Form_Leave"));
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
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-InventarioFisico.cs", "Form_FormClosing"));
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
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-InventarioFisico.cs", "Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos Grillas

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvFisicoInventario_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

            if (e.IsGetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (pi != null)
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double" || pi.PropertyType.Name == "Decimal")
                        e.Value = pi.GetValue(dto, null);
                    else
                        e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                else
                {
                    FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (fi != null)
                    {
                        if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double" || pi.PropertyType.Name == "Decimal")
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
                    {
                        e.Value = pi.GetValue(dto, null);
                    }
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
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void gvFisicoInventario_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (e.RowHandle >= 0)
                {
                    //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
                    GridColumn col = this.gvFisicoInventario.Columns[e.Column.FieldName];
                    string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);
                    this._rowCurrent = (DTO_inFisicoInventario)this.gvFisicoInventario.GetRow(this.gvFisicoInventario.FocusedRowHandle);

                    if (fieldName == "CantFisico")
                    {
                        decimal val = Convert.ToDecimal(e.Value);                       
                        decimal result = val - Convert.ToDecimal(this.gvFisicoInventario.GetRowCellValue(e.RowHandle, this._unboundPrefix + "CantKardex"));
                        this._listFisicoInventarioGrid.Find(x => x.inReferenciaID.Value == this._rowCurrent.inReferenciaID.Value).CantFisico.Value = val;
                        this._listFisicoInventarioGrid.Find(x => x.inReferenciaID.Value == this._rowCurrent.inReferenciaID.Value).CantAjuste.Value = result;
                        this._rowCurrent.CantFisico.Value = val;
                        this._rowCurrent.CantAjuste.Value = result;
                        FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Edit);

                        FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.SendtoAppr);
                        this.gvFisicoInventario.RefreshData();
                    } 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-InventarioFisico.cs", "gvFisicoInventario_CellValueChanged")); ;
            }
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void gvFisicoInventario_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            try
            {
                if (this.gvFisicoInventario.DataRowCount > 0 && e.RowHandle >= 0)
                {
                    //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
                    GridColumn col = this.gvFisicoInventario.Columns[this._unboundPrefix + "CantFisico"];
                    int val = Convert.ToInt32(this.gvFisicoInventario.GetRowCellValue(e.RowHandle, col.FieldName));
                    if (val < 0)
                    {
                        string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue);
                        this.gvFisicoInventario.SetColumnError(col, string.Format(msg, col.Caption));
                        e.Allow = false;
                    }
                    else
                        e.Allow = true;
                    if (this._listFisicoInventarioGrid.Find(x => x.inReferenciaID.Value == this._rowCurrent.inReferenciaID.Value).ActivoID.Value != 0)
                    {
                        decimal result = val - Convert.ToDecimal(this.gvFisicoInventario.GetRowCellValue(e.RowHandle, this._unboundPrefix + "CantKardex"));
                        if (result > 0)
                        {
                            this.gvFisicoInventario.SetColumnError(col, this._bc.GetResource(LanguageTypes.Messages, "No puede hacer el ingreso de una referencia serializada en este proceso"));
                            e.Allow = false;
                        }
                        else
                            this.gvSerial.Focus();
                    }
                    else
                        e.Allow = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-InventarioFisico.cs", "gvFisicoInventario_BeforeLeaveRow"));
            }

        }

        /// <summary>
        /// Maneja el cambio de registros en la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFisicoInventario_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (this.gvFisicoInventario.DataRowCount > 0 && e.FocusedRowHandle >= 0)
                {
                    this._rowCurrent = (DTO_inFisicoInventario)this.gvFisicoInventario.GetRow(this.gvFisicoInventario.FocusedRowHandle);
                    if (this._rowCurrent.ListSeriales != null && this._rowCurrent.ListSeriales.Count > 0)
                    {
                        #region Agrega una columnna a la data para borrar el serial actual
                        GridColumn del = new GridColumn();
                        del.FieldName = "x";
                        if (this.gvSerial.Columns.ColumnByFieldName("x") == null)
                            this.gvSerial.Columns.AddVisible("x");
                        this.gvSerial.Columns["x"].Width = 25;
                        this.gvSerial.Columns["x"].Caption = " ";
                        this.gvSerial.Columns["x"].OptionsColumn.AllowSize = false;
                        this.gvSerial.Columns["x"].ColumnEdit = this.editBtnDelete;
                        #endregion
                        #region Llena los costos de la referencia
                        decimal serialTotalLocal = 0;
                        decimal serialTotalExtra = 0;
                        foreach (var item in this._rowCurrent.ListSeriales)
                        {
                            var serialCosto = this._listFisicoInventarioGrid.Where(x => x.SerialID.Value.Equals(item.Key)).FirstOrDefault();
                            DTO_inFisicoInventario dto = serialCosto;
                            serialTotalLocal += dto.CostoLocal.Value.Value;
                            serialTotalExtra += dto.CostoExtra.Value.Value;
                        }
                        this.txtValorTotalML.EditValue = serialTotalLocal;
                        this.txtValorTotalME.EditValue = serialTotalExtra;
                        this.txtValorUnitML.EditValue = this._rowCurrent.ListSeriales.Count > 1 ? 0 : (this._rowCurrent.CostoLocal.Value / this._rowCurrent.CantKardex.Value);
                        this.txtValorUnitME.EditValue = this._rowCurrent.ListSeriales.Count > 1 ? 0 : (this._rowCurrent.CostoExtra.Value / this._rowCurrent.CantKardex.Value);

                        #endregion
                        this.gcSerial.DataSource = this._rowCurrent.ListSeriales;
                    }
                    else
                    {
                        this.txtValorTotalML.EditValue = this._rowCurrent.CostoLocal.Value.Value;
                        this.txtValorTotalME.EditValue = this._rowCurrent.CostoExtra.Value.Value;
                        this.txtValorUnitML.EditValue = this._rowCurrent.CantKardex.Value != 0? this._rowCurrent.CostoLocal.Value / this._rowCurrent.CantKardex.Value : 0;
                        this.txtValorUnitME.EditValue = this._rowCurrent.CantKardex.Value != 0 ? this._rowCurrent.CostoExtra.Value / this._rowCurrent.CantKardex.Value : 0;
                        this.gcSerial.DataSource = null;
                    }
                    if (string.IsNullOrEmpty(this._rowCurrent.SerialID.Value))
                        this.gvFisicoInventario.Columns[this._unboundPrefix + "CantFisico"].OptionsColumn.AllowEdit = true;
                    else
                        this.gvFisicoInventario.Columns[this._unboundPrefix + "CantFisico"].OptionsColumn.AllowEdit = false;
                    this.txtObservacion.Text = this._rowCurrent.Observacion.Value;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-InventarioFisico.cs", "gvFisicoInventario_FocusedRowChanged")); ;
            }
        }

        /// <summary>
        /// Borra los seriales que se se seleccionan al hacer clic
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void editBtnDelete_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                string serial = this.gvSerial.GetRowCellValue(this.gvSerial.FocusedRowHandle, "Key").ToString();
                #region Actualiza valores en la lista general
                int focus = this.gvFisicoInventario.FocusedRowHandle;
                int indexRef = this._listFisicoInventarioGrid.FindIndex(x => x.SerialID.Value.Equals(serial));
                if (indexRef >= 0)
                {
                    this._listFisicoInventarioGrid[indexRef].CantFisico.Value -= this._listFisicoInventarioGrid[indexRef].CantFisico.Value;
                    this._listFisicoInventarioGrid[indexRef].CantAjuste.Value = this._listFisicoInventarioGrid[indexRef].CantFisico.Value - this._listFisicoInventarioGrid[indexRef].CantKardex.Value;
                }
                #endregion
                #region Actualiza valores en lista de la grilla general
                this._listFisicoInventarioGrid[this.gvFisicoInventario.FocusedRowHandle].CantFisico.Value -= 1;
                this._listFisicoInventarioGrid[this.gvFisicoInventario.FocusedRowHandle].CantAjuste.Value -= 1;
                this._listFisicoInventarioGrid[this.gvFisicoInventario.FocusedRowHandle].ListSeriales.Remove(serial);
                this.gcSerial.DataSource = null;
                this.gcSerial.DataSource = this._listFisicoInventarioGrid[this.gvFisicoInventario.FocusedRowHandle].ListSeriales;
                this.gvFisicoInventario.UpdateCurrentRow();
                FormProvider.Master.itemSendtoAppr.Enabled = false;
                FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Edit);
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-InventarioFisico.cs", "editBtnDelete_ButtonClick")); ;
            }
        }

        #endregion

        #region Eventos Detalle(Nueva Ref)

        /// <summary>
        /// Se ejecuta cuando deja el control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterDetailNew_Leave(object sender, EventArgs e)
        {
            ControlsUC.uc_MasterFind master = (ControlsUC.uc_MasterFind)sender;
            try
            {
                switch (master.ColId)
                {
                    case "inReferenciaID":
                        if (master.ValidID)
                        {
                            DTO_inReferencia referencia = (DTO_inReferencia)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, false, master.Value, true);
                            DTO_inRefTipo _tipoInv = (DTO_inRefTipo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inRefTipo, false, referencia.TipoInvID.Value, true);
                            #region Referencia serializada
                            if (_tipoInv.SerializadoInd.Value.Value)
                                this.txtSerialNew.BackColor = Color.LightBlue;
                            else
                                this.txtSerialNew.BackColor = Color.White;
                            this.txtSerialNew.Enabled = _tipoInv.SerializadoInd.Value.Value;
                            #endregion
                            #region Trae los Parametros 1 y 2 si están habilitados
                            //if (_tipoInv.Parametro1Ind.Value.Value)
                            //{
                            //    filtrosParam1 = InventoryParameters.GetQueryParameters(_tipoInv.ID.Value, true);
                            //    this._bc.InitMasterUC(this.masterParametro1New, AppMasters.inRefParametro1, true, true, true, true, filtrosParam1);
                            //    this.masterParametro1New.EnableControl(true);
                            //}
                            //else
                            //{
                            //    this._bc.InitMasterUC(this.masterParametro1New, AppMasters.inRefParametro1, true, true, true, false);
                            //    this.masterParametro1New.EnableControl(false);
                            //    this.masterParametro1New.Value = this._bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Parametro1xDefecto);
                            //}
                            //if (_tipoInv.Parametro2Ind.Value.Value)
                            //{
                            //    filtrosParam2 = InventoryParameters.GetQueryParameters(_tipoInv.ID.Value, false);
                            //    this._bc.InitMasterUC(this.masterParametro2New, AppMasters.inRefParametro2, true, true, true, true, filtrosParam2);
                            //    this.masterParametro2New.EnableControl(true);
                            //}
                            //else
                            //{
                            //    this._bc.InitMasterUC(this.masterParametro2New, AppMasters.inRefParametro2, true, true, true, false);
                            //    this.masterParametro2New.EnableControl(false);
                            //    this.masterParametro2New.Value = this._bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Parametro2xDefecto);
                            //}
                            #endregion
                            //this.cmbEstadoNew.Enabled = true;
                            //this.cmbEstadoNew.BackColor = Color.LightBlue;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-InventarioFisico.cs", "masterDetailNew_Leave"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando deja el control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void cmbEstadoNew_Leave(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Se ejecuta cuando deja el control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtNew_Leave(object sender, EventArgs e)
        {
            TextEdit txtControl = (TextEdit)sender;
            try
            {
                switch (txtControl.Name)
                { 
                    case "txtObservacion":
                        this._rowCurrent.Observacion.Value = txtControl.Text;
                        FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Edit);
                        FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.SendtoAppr);
                        this.gcFisicoInventario.RefreshDataSource();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-InventarioFisico.cs", "txtNew_Leave"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando se clickea el boton
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnAddRef_Click(object sender, EventArgs e)
        {
            if (this.masterReferenciaNew.ValidID && this.masterParametro1New.ValidID && this.masterParametro2New.ValidID
                && this.txtCantidadNew.Text != "0" && !string.IsNullOrEmpty(this.txtCantidadNew.Text))
                this.LoadGrids(true);
            else
                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_DataIncompleteNewReferencia));
        }

        /// <summary>
        /// Boton para agregar una nueva referencia a los saldos de la bodega
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnNewRef_CheckedChanged(object sender, EventArgs e)
        {
            this.pnNewRef.Enabled = this.btnNewRef.Checked;
            this.btnAddRef.Enabled = this.btnNewRef.Checked;
        }
        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            try
            {
                base.TBSave();
                this.gvFisicoInventario.PostEditor();
                this.gvFisicoInventario.ActiveFilterString = string.Empty;
                Thread process = new Thread(this.SaveThread);
                process.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-InventarioFisico.cs", "TBSave"));
            }
        }

        /// <summary>
        /// Boton para enviar a aprobacion
        /// </summary>
        public override void TBSendtoAppr()
        {
            Thread process = new Thread(this.SendToApproveThread);
            process.Start();
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Boton para iniciar un nuevo documento
        /// </summary>
        public override void TBNew()
        {
            try
            {
                this._listFisicoInventarioGrid = new List<DTO_inFisicoInventario>();
                this.listSaldosCostos = new List<DTO_inControlSaldosCostos>();
                this._rowCurrent = new DTO_inFisicoInventario();
                this.Invoke(this.sendToApproveDelegate);
                this.btnCargaSaldos.Enabled = false;
                this.btnUpdateSaldos.Enabled = false;
                FormProvider.Master.itemSave.Enabled = false;
                FormProvider.Master.itemSendtoAppr.Enabled = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Hilo de salvar
        /// </summary>
        public virtual void SaveThread()
        {
            try
            {
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this._documentID, this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (this._bc.AdministrationModel.ConsultarProgresoInventarios(this._documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this._documentID);

                DTO_SerializedObject result = this._bc.AdministrationModel.InventarioFisico_Add(this._documentID, masterBodega.Value, this.dtFecha.DateTime, out this.numeroDoc, this._listFisicoInventarioGrid);
                FormProvider.Master.StopProgressBarThread(this._documentID);

                bool isOK = this._bc.SendDocumentMail(MailType.NotSend, this._documentID, this._actFlujo.seUsuarioID.Value, result, true);
                if (isOK)
                {
                    //this._listFisicoInventario = new List<DTO_inFisicoInventario>();
                    //this._listFisicoInventarioFiltered = new List<DTO_inFisicoInventario>();
                    //this.listSaldosCostos = new List<DTO_inControlSaldosCostos>();
                    //this.filtrosParam1.Clear();
                    //this.filtrosParam2.Clear();
                    //this.Invoke(this.saveDelegate);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-InventarioFisico.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this._documentID);
            }
        }

        /// <summary>
        /// Hilo de Enviar para aprobación
        /// </summary>
        public virtual void SendToApproveThread()
        {
            try
            {
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this._documentID, this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (this._bc.AdministrationModel.ConsultarProgresoInventarios(this._documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this._documentID);

                DTO_SerializedObject obj = this._bc.AdministrationModel.InventarioFisico_SendToAprob(this._documentID, this._actFlujo.ID.Value, this.numeroDoc, true);
                FormProvider.Master.StopProgressBarThread(this._documentID);

                bool isOK = this._bc.SendDocumentMail(MailType.SendToApprove, this._documentID, this._actFlujo.seUsuarioID.Value, obj, true);
                if (isOK)
                {
                    this._bc.AdministrationModel.aplTemporales_Clean(this._documentID.ToString(), this._bc.AdministrationModel.User);
                    this._listFisicoInventarioGrid = new List<DTO_inFisicoInventario>();
                    this.Invoke(this.sendToApproveDelegate);
                }              
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-InventarioFisico.cs", "SendToApproveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this._documentID);
            }
        }
        #endregion

    }
}
