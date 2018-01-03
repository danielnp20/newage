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
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using NewAge.DTO.Resultados;
using System.Threading;
using NewAge.DTO.Negocio.Documentos.Activos;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class QueryActivos : Form
    {
        #region Variables

        /// <summary>
        /// Base controller
        /// </summary>
        BaseController _bc = BaseController.GetInstance();

        /// <summary>
        /// Variable para asignar los recursos del alta de activo para las columnas de la grilla
        ///  </summary>
        private int documentAltaID = AppDocuments.AltaActivos;

        /// <summary>
        /// DTO_glMovimientoDeta para cargar la info en la pestaña de detalle
        /// </summary>
        private List<DTO_glMovimientoDeta> _glMvtoDeta;

        /// <summary>
        /// Trae la info de los saldos de cada activo por todos sus componentes
        /// </summary>
        private List<DTO_acActivoQuerySaldos> _acActivoSaldoFuncional;
        private List<DTO_acActivoQuerySaldos> _acActivoSaldoIFRS;

        /// <summary>
        /// Variable para almacenar el AcivoID y usarlo en diferentes funciones
        /// </summary>
        private int _activoID = 0;

        /// <summary>
        /// Variable para cargar el activoClase ID y poderlo usar den diferentes funciones
        /// </summary>
        private string _acClaseID = null;

        private DTO_acQueryActivoControl _acControlsel;

        /// <summary>
        /// indica que páginas se han cargado
        /// </summary>
        private bool[] _loadedDataInd = new bool[6];

        /// <summary>
        /// Dto para cargar la data acActivoControl.
        /// </summary>
        private List<DTO_acQueryActivoControl> _activo = null;

        /// <summary>
        /// identificador de recursos del formulario
        /// </summary>
        private int _documentID;
        private string _monedaLocal;
        private string _monedaExtranjera;
        private string _tab = "\t";
        private string _unboundPrefix = "Unbound_";

        private int currentRow;
        private bool _select;
        private List<int> select;
        #endregion

        /// <summary>
        /// Constructor con un documento
        /// </summary>
        /// <param name="doc"></param>
        public QueryActivos()
        {
            this._documentID = AppQueries.QueryActivosControl;
            InitializeComponent();
            FormProvider.LoadResources(this, this._documentID);
            this.InitControls();
            this.InitTabs();
            this.AddGridCols();

            for (int i = 1; i < 6; i++)
                this._loadedDataInd[i] = false;
        }

        #region Funciones Privadas

        /// <summary>
        /// Inicializa los controles
        /// </summary>
        private void InitControls()
        {
            #region BusquedaDocumento
            this._bc.InitMasterUC(this.uc_MF_Clase, AppMasters.acClase, true, true, true, false);
            this._bc.InitMasterUC(this.uc_MF_Grupo, AppMasters.acGrupo, true, true, true, false);
            this._bc.InitMasterUC(this.uc_MF_LocFisica, AppMasters.glLocFisica, true, true, true, false);
            this._bc.InitMasterUC(this.uc_MF_Ref, AppMasters.inReferencia, true, true, true, false);
            this._bc.InitMasterUC(this.uc_MF_Tipo, AppMasters.acTipo, true, true, true, false);
            this._bc.InitMasterUC(this.uc_MF_Responsable, AppMasters.coTercero, true, true, true, false);
            //this.dt_CFecha.Value = Convert.ToDateTime(string.Empty);
            #endregion
            #region Saldos

            string periodo = _bc.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_Periodo);
            this.uc_Saldos_PerEditPeriodo.DateTime = DateTime.Parse(periodo);
            #endregion
        }

        private void LoadFooterControls(DTO_acQueryActivoControl datosCargados)
        {
            #region Compras
            this.txt_CProvedor.Text = datosCargados.Proveedor.Value;
            this.dt_CFecha.Value = datosCargados.FechaCompra.Value.Value;
            this.txt_CDocumento.Text = datosCargados.Factura.Value;
            #endregion
            #region Clasificación

            this.txt_ClClase.Text = datosCargados.ActivoClase.Value;
            this.txt_ClReferencia.Text = datosCargados.inReferenciaID.Value;
            this.txt_ClGrupo.Text = datosCargados.ActivoGrupo.Value;
            this.txt_ClTipo.Text = datosCargados.ActivoTipo.Value;
            this.txt_ClModelo.Text = datosCargados.Modelo.Value;

            #endregion
            #region Ubicación
            this.txt_UProyecto.Text = datosCargados.Proyecto.Value;
            this.txt_ULocFisica.Text = datosCargados.LocFisica.Value;
            this.txt_UCtrCosto.Text = datosCargados.CentroCosto.Value;
            this.txt_UContenedor.Text = datosCargados.PlaquetaID.Value;
            this.txt_UResponsable.Text = datosCargados.Responsable.Value;
            #endregion
            #region Otros Datos
            #region COl

            this.txt_ColTipoDepre.Text = datosCargados.TipoDepreLOC.Value.Value.ToString();
            this.txt_ColVidaUtil.Text = datosCargados.VidaUtilLOC.Value.Value.ToString();
            this.txt_ColRetiro.Text = string.Empty;
            this.txt_ColSalvamento.Text = datosCargados.ValorSalvamentoLOC.Value.Value.ToString();
            #endregion
            #region IFRS
            this.txt_IFTipoDepre.Text = datosCargados.TipoDepreIFRS.Value.Value.ToString();
            this.txt_IFVidaUtil.Text = datosCargados.VidaUtilIFRS.Value.Value.ToString();
            this.txt_IFVlrRetiro.Text = datosCargados.ValorRetiroIFRS.Value.Value.ToString();
            this.txt_IFVlrSalvamento.Text = datosCargados.ValorSalvamentoIFRS.Value.Value.ToString();
            #endregion
            #region USGAP
            this.txt_USTipoDepre.Text = datosCargados.TipoDepreUSG.Value.Value.ToString();
            this.txt_USVidaUtil.Text = datosCargados.VidaUtilUSG.Value.Value.ToString();
            this.txt_UsVlrRetiro.Text = string.Empty;
            this.txt_USVlrSalvamento.Text = datosCargados.ValorSalvamentoUSG.Value.Value.ToString();
            #endregion
            #endregion
        }

        /// <summary>
        /// Inicializa las pestañas
        /// </summary>
        private void InitTabs()
        {
            this.tp_Busqueda.PageVisible = true;
            this.tp_saldos.PageVisible = false;
        }

        /// <summary>
        /// Funcion que carga el data en la grilla
        /// </summary>
        /// <param name="firstTime">valida que si es la primera vez</param>
        private void LoadData(bool firstTime)
        {
            this.gcBusqueda.DataSource = null;
            this.gcBusqueda.DataSource = this._activo;
            this.gcBusqueda.RefreshDataSource();
        }

        /// <summary>
        /// Cargar los datos para un página específica
        /// </summary>
        /// <param name="page"></param>
        private void LoadPageData(int page)
        {
            switch (page)
            {
                case 0:
                    break;
                case 1:
                    #region Datos Activo

                    #endregion
                    break;
                case 2:
                    #region Saldos

                    this.txtPLaquetaSaldos.Text = this._acControlsel.PlaquetaID.Value;
                    this.txtSerialSaldos.Text = this._acControlsel.SerialID.Value;
                    this.txtDescripcionSaldos.Text = this._acControlsel.Observacion.Value;
                    this.txt_SActivoGrupo.Text = this._acControlsel.ActivoGrupo.Value;
                    this.txt_STipo.Text = this._acControlsel.ActivoTipo.Value;
                    this.txt_SClase.Text = this._acControlsel.ActivoClase.Value;

                    //this.gcSaldosDetail.DataSource = this._acActivoSaldo;
                    //this.gcSaldosDetail.RefreshDataSource();
                    #endregion
                    break;

            }
            this._loadedDataInd[page] = true;
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                #region Busqueda Activo
                //Plaqueta
                GridColumn plaquetaID = new GridColumn();
                plaquetaID.FieldName = this._unboundPrefix + "PlaquetaID";
                plaquetaID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_PlaquetaID");
                plaquetaID.UnboundType = UnboundColumnType.String;
                plaquetaID.VisibleIndex = 1;
                plaquetaID.Width = 150;
                plaquetaID.Visible = true;
                plaquetaID.OptionsColumn.AllowEdit = false;
                this.gvBusqueda.Columns.Add(plaquetaID);

                //Serial
                GridColumn serialID = new GridColumn();
                serialID.FieldName = this._unboundPrefix + "SerialID";
                serialID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_SerialID");
                serialID.UnboundType = UnboundColumnType.Integer;
                serialID.VisibleIndex = 2;
                serialID.Width = 150;
                serialID.Visible = true;
                serialID.OptionsColumn.AllowEdit = false;
                this.gvBusqueda.Columns.Add(serialID);

                //onservacion
                GridColumn onservacion = new GridColumn();
                onservacion.FieldName = this._unboundPrefix + "Observacion";
                onservacion.Caption = _bc.GetResource(LanguageTypes.Forms, documentAltaID + "_Descripcion");
                onservacion.UnboundType = UnboundColumnType.String;
                onservacion.VisibleIndex = 3;
                onservacion.Width = 600;
                onservacion.Visible = true;
                onservacion.OptionsColumn.AllowEdit = false;
                this.gvBusqueda.Columns.Add(onservacion);

                #endregion
                #region Saldos Detail
                #region Libro IFRS
                GridColumn componentes = new GridColumn();
                componentes.FieldName = this._unboundPrefix + "Componente";
                componentes.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_Componente");
                componentes.UnboundType = UnboundColumnType.String;
                componentes.VisibleIndex = 1;
                componentes.Width = 100;
                componentes.Visible = true;
                componentes.OptionsColumn.AllowEdit = false;
                this.gvSaldosDetail.Columns.Add(componentes);

                //saldoIniLoc
                GridColumn saldoIniLoc = new GridColumn();
                saldoIniLoc.FieldName = this._unboundPrefix + "SaldoIniML";
                saldoIniLoc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_SaldoIniLoc");
                saldoIniLoc.UnboundType = UnboundColumnType.String;
                saldoIniLoc.VisibleIndex = 2;
                saldoIniLoc.Width = 100;
                saldoIniLoc.Visible = true;
                saldoIniLoc.OptionsColumn.AllowEdit = false;
                this.gvSaldosDetail.Columns.Add(saldoIniLoc);

                //DecriptivoComponente
                GridColumn saldoIniExt = new GridColumn();
                saldoIniExt.FieldName = this._unboundPrefix + "SaldoIniME";
                saldoIniExt.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_SaldoIniExt"); ;
                saldoIniExt.UnboundType = UnboundColumnType.String;
                saldoIniExt.VisibleIndex = 3;
                saldoIniExt.Width = 100;
                saldoIniExt.Visible = true;
                saldoIniExt.OptionsColumn.AllowEdit = false;
                this.gvSaldosDetail.Columns.Add(saldoIniExt);

                //saldoMovLoc
                GridColumn saldoMovLoc = new GridColumn();
                saldoMovLoc.FieldName = this._unboundPrefix + "VlrMtoML";
                saldoMovLoc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_SaldoMovLoc");
                saldoMovLoc.UnboundType = UnboundColumnType.Decimal;
                saldoMovLoc.VisibleIndex = 4;
                saldoMovLoc.Width = 100;
                saldoMovLoc.Visible = true;
                saldoMovLoc.OptionsColumn.AllowEdit = false;
                this.gvSaldosDetail.Columns.Add(saldoMovLoc);

                //saldoMovExt
                GridColumn saldoMovExt = new GridColumn();
                saldoMovExt.FieldName = this._unboundPrefix + "VlrMtoME";
                saldoMovExt.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_SaldoMovExt");
                saldoMovExt.UnboundType = UnboundColumnType.Decimal;
                saldoMovExt.VisibleIndex = 5;
                saldoMovExt.Width = 100;
                saldoMovExt.Visible = _bc.AdministrationModel.MultiMoneda;
                saldoMovExt.OptionsColumn.AllowEdit = false;
                this.gvSaldosDetail.Columns.Add(saldoMovExt);

                //NvoSaldoLoc
                GridColumn nvoSaldoLoc = new GridColumn();
                nvoSaldoLoc.FieldName = this._unboundPrefix + "SaldoActualML";
                nvoSaldoLoc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_SaldoActualML");
                nvoSaldoLoc.UnboundType = UnboundColumnType.Decimal;
                nvoSaldoLoc.VisibleIndex = 6;
                nvoSaldoLoc.Width = 100;
                nvoSaldoLoc.Visible = _bc.AdministrationModel.MultiMoneda;
                nvoSaldoLoc.OptionsColumn.AllowEdit = false;
                this.gvSaldosDetail.Columns.Add(nvoSaldoLoc);

                //nvoSaldoExt
                GridColumn nvoSaldoExt = new GridColumn();
                nvoSaldoExt.FieldName = this._unboundPrefix + "SaldoActualME";
                nvoSaldoExt.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_SaldoActualME");
                nvoSaldoExt.UnboundType = UnboundColumnType.Decimal;
                nvoSaldoExt.VisibleIndex = 7;
                nvoSaldoExt.Width = 100;
                nvoSaldoExt.Visible = _bc.AdministrationModel.MultiMoneda;
                nvoSaldoExt.OptionsColumn.AllowEdit = false;
                this.gvSaldosDetail.Columns.Add(nvoSaldoExt);
                #endregion
                #region Libro Funcional
                GridColumn funComponentes = new GridColumn();
                funComponentes.FieldName = this._unboundPrefix + "Componente";
                funComponentes.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_Componente");
                funComponentes.UnboundType = UnboundColumnType.String;
                funComponentes.VisibleIndex = 1;
                funComponentes.Width = 100;
                funComponentes.Visible = true;
                funComponentes.OptionsColumn.AllowEdit = false;
                this.gvFuncional.Columns.Add(funComponentes);

                //saldoIniLoc
                GridColumn funSaldoIniLoc = new GridColumn();
                funSaldoIniLoc.FieldName = this._unboundPrefix + "SaldoIniML";
                funSaldoIniLoc.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_SaldoIniLoc");
                funSaldoIniLoc.UnboundType = UnboundColumnType.String;
                funSaldoIniLoc.VisibleIndex = 2;
                funSaldoIniLoc.Width = 100;
                funSaldoIniLoc.Visible = true;
                funSaldoIniLoc.OptionsColumn.AllowEdit = false;
                this.gvFuncional.Columns.Add(funSaldoIniLoc);

                //DecriptivoComponente
                GridColumn funSaldoIniExt = new GridColumn();
                funSaldoIniExt.FieldName = this._unboundPrefix + "SaldoIniME";
                funSaldoIniExt.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_SaldoIniExt"); ;
                funSaldoIniExt.UnboundType = UnboundColumnType.String;
                funSaldoIniExt.VisibleIndex = 3;
                funSaldoIniExt.Width = 100;
                funSaldoIniExt.Visible = true;
                funSaldoIniExt.OptionsColumn.AllowEdit = false;
                this.gvFuncional.Columns.Add(funSaldoIniExt);

                //saldoMovLoc
                GridColumn funSaldoMovLoc = new GridColumn();
                funSaldoMovLoc.FieldName = this._unboundPrefix + "VlrMtoML";
                funSaldoMovLoc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_SaldoMovLoc");
                funSaldoMovLoc.UnboundType = UnboundColumnType.Decimal;
                funSaldoMovLoc.VisibleIndex = 4;
                funSaldoMovLoc.Width = 100;
                funSaldoMovLoc.Visible = true;
                funSaldoMovLoc.OptionsColumn.AllowEdit = false;
                this.gvFuncional.Columns.Add(funSaldoMovLoc);

                //saldoMovExt
                GridColumn funSaldoMovExt = new GridColumn();
                funSaldoMovExt.FieldName = this._unboundPrefix + "VlrMtoME";
                funSaldoMovExt.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_SaldoMovExt");
                funSaldoMovExt.UnboundType = UnboundColumnType.Decimal;
                funSaldoMovExt.VisibleIndex = 5;
                funSaldoMovExt.Width = 100;
                funSaldoMovExt.Visible = _bc.AdministrationModel.MultiMoneda;
                funSaldoMovExt.OptionsColumn.AllowEdit = false;
                this.gvFuncional.Columns.Add(funSaldoMovExt);

                //NvoSaldoLoc
                GridColumn funNvoSaldoLoc = new GridColumn();
                funNvoSaldoLoc.FieldName = this._unboundPrefix + "SaldoActualML";
                funNvoSaldoLoc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_SaldoActualML");
                funNvoSaldoLoc.UnboundType = UnboundColumnType.Decimal;
                funNvoSaldoLoc.VisibleIndex = 6;
                funNvoSaldoLoc.Width = 100;
                funNvoSaldoLoc.Visible = _bc.AdministrationModel.MultiMoneda;
                funNvoSaldoLoc.OptionsColumn.AllowEdit = false;
                this.gvFuncional.Columns.Add(funNvoSaldoLoc);

                //nvoSaldoExt
                GridColumn funNvoSaldoExt = new GridColumn();
                funNvoSaldoExt.FieldName = this._unboundPrefix + "SaldoActualME";
                funNvoSaldoExt.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_SaldoActualME");
                funNvoSaldoExt.UnboundType = UnboundColumnType.Decimal;
                funNvoSaldoExt.VisibleIndex = 7;
                funNvoSaldoExt.Width = 100;
                funNvoSaldoExt.Visible = _bc.AdministrationModel.MultiMoneda;
                funNvoSaldoExt.OptionsColumn.AllowEdit = false;
                this.gvFuncional.Columns.Add(funNvoSaldoExt);
                #endregion
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-BusquedaacControl.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Asigna el Componente y los mvtos a los controles de la pantalla
        /// </summary>
        /// <param name="saldoMvto">Lista de Mvtos</param>
        private void CargarTransaccionesXComponente(bool isCount)
        {
            //for (int i = 0; i < this._comprobanteFooter.Count; i++)
            //{
            //    switch (i)
            //    {
            //        case 0:
            //            this.txtComponenteLocal1.EditValue = this._comprobanteFooter[i].vlrMdaLoc.Value;
            //            this.txtComponenteExt1.EditValue = this._comprobanteFooter[i].vlrMdaExt.Value;
            //            break;
            //        case 1:
            //            this.txtComponenteLocal2.EditValue = this._comprobanteFooter[i].vlrMdaLoc.Value;
            //            this.txtComponenteExt2.EditValue = this._comprobanteFooter[i].vlrMdaExt.Value;
            //            break;
            //        case 2:
            //            this.txtComponenteLocal3.EditValue = this._comprobanteFooter[i].vlrMdaLoc.Value;
            //            this.txtComponenteExt3.EditValue = this._comprobanteFooter[i].vlrMdaExt.Value;
            //            break;
            //        case 3:
            //            this.txtComponenteLocal4.EditValue = this._comprobanteFooter[i].vlrMdaLoc.Value;
            //            this.txtComponenteExt4.EditValue = this._comprobanteFooter[i].vlrMdaExt.Value;
            //            break;
            //        case 4:
            //            this.txtComponenteLocal5.EditValue = this._comprobanteFooter[i].vlrMdaLoc.Value;
            //            this.txtComponenteExt5.EditValue = this._comprobanteFooter[i].vlrMdaExt.Value;
            //            break;
            //        case 5:
            //            this.txtComponenteLocal6.EditValue = this._comprobanteFooter[i].vlrMdaLoc.Value;
            //            this.txtComponenteExt6.EditValue = this._comprobanteFooter[i].vlrMdaExt.Value;
            //            break;
            //        case 6:
            //            this.txtComponenteLocal7.EditValue = this._comprobanteFooter[i].vlrMdaLoc.Value;
            //            this.txtComponenteExt7.EditValue = this._comprobanteFooter[i].vlrMdaExt.Value;
            //            break;
            //        case 7:
            //            this.txtComponenteLocal8.EditValue = this._comprobanteFooter[i].vlrMdaLoc.Value;
            //            this.txtComponenteExt8.EditValue = this._comprobanteFooter[i].vlrMdaExt.Value;
            //            break;
            //    }
            //}
            //if (this._comprobanteFooter.Count == 0)
            //{
            //    this.txtComponenteLocal8.EditValue = string.Empty;
            //    this.txtComponenteExt8.EditValue = string.Empty;
            //    this.txtComponenteExt7.EditValue = string.Empty;
            //    this.txtComponenteLocal7.EditValue = string.Empty;
            //    this.txtComponenteExt6.EditValue = string.Empty;
            //    this.txtComponenteLocal6.EditValue = string.Empty;
            //    this.txtComponenteExt5.EditValue = string.Empty;
            //    this.txtComponenteLocal5.EditValue = string.Empty;
            //    this.txtComponenteExt4.EditValue = string.Empty;
            //    this.txtComponenteLocal4.EditValue = string.Empty;
            //    this.txtComponenteExt3.EditValue = string.Empty;
            //    this.txtComponenteLocal3.EditValue = string.Empty;
            //    this.txtComponenteExt2.EditValue = string.Empty;
            //    this.txtComponenteLocal2.EditValue = string.Empty;
            //    this.txtComponenteExt1.EditValue = string.Empty;
            //    this.txtComponenteLocal1.EditValue = string.Empty;
            //}
        }
        #endregion

        #region Eventos

        #region Busqueda

        /// <summary>
        /// Busca el documento de acuerdo al Filtro
        /// </summary>
        /// <param name="sender">OBjeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btn_BuscarActivo_Click(object sender, EventArgs e)
        {
            try
            {
                this._activo = new List<DTO_acQueryActivoControl>();
                this._activo = this._bc.AdministrationModel.ActivoGetByParameter(this.txtPlaquetabuscar.Text, this.txtSerialBuscar.Text, this.uc_MF_Ref.Value, this.uc_MF_Clase.Value, this.uc_MF_Tipo.Value, this.uc_MF_Grupo.Value, this.uc_MF_LocFisica.Value, true, this.uc_MF_Responsable.Value);
                this.LoadData(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Buscar_Activo.cs", "btn_BuscarActivo_Click"));
                throw;
            }
        }
        #endregion

        #region Saldos

        /// <summary>
        /// Evento que envia el evento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uc_Saldos_PerEditPeriodo_Leave(object sender, EventArgs e)
        {
            try
            {
                #region Funcional
                this._acActivoSaldoFuncional = new List<DTO_acActivoQuerySaldos>();
                string libroFuncional = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);
                this._acActivoSaldoFuncional = this._bc.AdministrationModel.ActivoControl_GetSaldosByMesYLibro(this._acControlsel.ActivoID.Value.Value, libroFuncional, this.uc_Saldos_PerEditPeriodo.DateTime);
                this.gcSaldosDetail.DataSource = this._acActivoSaldoFuncional;
                this.gcSaldosDetail.RefreshDataSource();
                #endregion
                #region IFRS
                this._acActivoSaldoIFRS = new List<DTO_acActivoQuerySaldos>();
                string libroIFRS = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceIFRS);
                this._acActivoSaldoIFRS = this._bc.AdministrationModel.ActivoControl_GetSaldosByMesYLibro(this._acControlsel.ActivoID.Value.Value, libroIFRS, this.uc_Saldos_PerEditPeriodo.DateTime);
                this.gcFuncional.DataSource = this._acActivoSaldoIFRS;
                this.gcFuncional.RefreshDataSource();
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-BusquedaacControl.cs", "uc_Saldos_PerEditPeriodo_Leave"));
            }
        }
        #endregion
        #endregion

        #region Eventos Grilla
        #region Busqueda Activos
        /// <summary>
        /// Evento que asigna la informacion del dto a la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvBusqueda_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

            if (e.IsGetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (pi != null)
                {
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" ||
                        pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                    {
                        e.Value = pi.GetValue(dto, null);
                    }
                    else
                    {
                        e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                    }
                }
                else
                {
                    FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (fi != null)
                    {
                        if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" ||
                            fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double")
                        {
                            e.Value = fi.GetValue(dto);
                        }
                        else
                        {
                            e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                        }
                    }
                }

            }
        }

        /// <summary>
        /// carga informacion con respecto al DTO_acControl en otras ventanas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvBusqueda_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                GridHitInfo info = view.CalcHitInfo(this.gcBusqueda.PointToClient(MousePosition));
                int fila = info.RowHandle;
                this._acControlsel = new DTO_acQueryActivoControl();
                this._acControlsel = this._activo[fila];
                this.tp_saldos.PageVisible = true;

                this.LoadPageData(2);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Evento al movesrse entre detalles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvBusqueda_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            int fila = e.FocusedRowHandle;
            try
            {
                DTO_acQueryActivoControl datosCargados = new DTO_acQueryActivoControl();
                if (this._activo.Count == 1)
                    datosCargados = this._activo[0];
                else
                    datosCargados = this._activo[fila];

                this.LoadFooterControls(datosCargados);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Buscar_Activo.cs", "gvBusqueda_FocusedRowChanged"));
                throw;
            }
        }

        #endregion

        #region Detalle
        /// <summary>
        /// Evento que valida la info de cada Dto para asignarle info al cuadro de valores
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">evento</param>
        private void gvDetalle_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            int fila = e.FocusedRowHandle;
            if (this._glMvtoDeta.Count > 0)
            {
                int numeroDoc = Convert.ToInt32(this._glMvtoDeta[fila].NumeroDoc.Value);
                int idTr = Convert.ToInt32(this._glMvtoDeta[fila].IdentificadorTr.Value);
                //this._comprobanteFooter = this._bc.AdministrationModel.acActivoControl_GetByIdentificadorTR(numeroDoc, idTr);

                this.CargarTransaccionesXComponente(true);
            }
        }
        #endregion

        #region Saldos
        /// <summary>
        /// asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void gvSaldosDetail_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

            if (fieldName == "VlrMtoML" || fieldName == "VlrMtoME" || fieldName == "SaldoActualML" ||
                fieldName == "SaldoIniML" || fieldName == "SaldoIniME" || fieldName == "SaldoActualME")
            {
                e.RepositoryItem = this.SpinEdit;
            }
        }


        /// <summary>
        /// Evento que asigna la informacion del dto a la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvFuncional_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

            if (e.IsGetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (pi != null)
                {
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" ||
                        pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                    {
                        e.Value = pi.GetValue(dto, null);
                    }
                    else
                    {
                        e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                    }
                }
                else
                {
                    FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (fi != null)
                    {
                        if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" ||
                            fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double")
                        {
                            e.Value = fi.GetValue(dto);
                        }
                        else
                        {
                            e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                        }
                    }
                }

            }
        }

        #endregion

        #region Salodos Moneda Local
        /// <summary>
        /// Evento para agregar formacto a los campos de moneda.
        /// </summary>
        /// <param name="sender">Evento</param>
        /// <param name="e">Evento</param>
        private void gvMonedaLocalGrid_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

            if (e.IsGetData)
            {
                if (fieldName == "Marca" && e.Value == null)
                {
                    e.Value = this.select.Contains(e.ListSourceRowIndex);
                }
                else
                {
                    PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (pi != null)
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                        {
                            e.Value = pi.GetValue(dto, null);
                        }
                        else
                        {
                            e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                        }
                    else
                    {
                        FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                        if (fi != null)
                        {
                            if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double")
                            {
                                e.Value = fi.GetValue(dto);
                            }
                            else
                            {
                                e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                            }
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
        }
        #endregion
        #endregion
    }
}
