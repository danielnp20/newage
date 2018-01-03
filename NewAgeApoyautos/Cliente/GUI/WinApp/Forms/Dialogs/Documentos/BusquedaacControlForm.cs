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
    public partial class BusquedaacControlForm : Form
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
        /// Dto del documento
        /// </summary>
        private DTO_glDocumentoControl _docCtrl;

        /// <summary>
        /// DTO_glMovimientoDeta para cargar la info en la pestaña de detalle
        /// </summary>
        private List<DTO_glMovimientoDeta> _glMvtoDeta;

        /// <summary>
        /// Trae la info de los saldos de cada activo por todos sus componentes
        /// </summary>
        private List<DTO_acActivoSaldo> _acActivoSaldo;

        /// <summary>
        /// Lista de DTO para la arga de informacion con saldos en moneda local
        /// </summary>
        private List<DTO_acActivoSaldo> _acActivoSaldoMonedaLocal;

        /// <summary>
        /// Lista de DTO para la arga de informacion con saldos en moneda Extranjera
        /// </summary>
        private List<DTO_acActivoSaldo> _acActivoSaldoMonedaExtranjera;

        /// <summary>
        /// Lista de DTo para cargar el detatlle de transacciones desde el coauxiliar
        /// </summary>
        private List<DTO_ComprobanteFooter> _comprobanteFooter;

        /// <summary>
        /// Variable para almacenar el AcivoID y usarlo en diferentes funciones
        /// </summary>
        private int _activoID = 0;

        /// <summary>
        /// Variable para cargar el activoClase ID y poderlo usar den diferentes funciones
        /// </summary>
        private string _acClaseID = null;

        private DTO_acActivoControl _acControlsel;

        /// <summary>
        /// indica que páginas se han cargado
        /// </summary>
        private bool[] _loadedDataInd = new bool[6];

        /// <summary>
        /// Dto para cargar la data acActivoControl.
        /// </summary>
        private List<DTO_acActivoControl> _activo = null;

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
        public BusquedaacControlForm()
        {
            this._documentID = AppForms.ConsultaActivos;
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
            _bc.InitMasterUC(this.uc_Busqueda_MasterProyectoID, AppMasters.coProyecto, true, true, true, false);
            _bc.InitMasterUC(this.uc_Busqueda_MasterLocFisicaID, AppMasters.glLocFisica, true, true, true, false);
            _bc.InitMasterUC(this.uc_Busqueda_MasterCentroCosto, AppMasters.coCentroCosto, true, true, true, false);
            #endregion
            #region Datos acActivo
            _bc.InitMasterUC(this.uc_DatosAc_MasterRef, AppMasters.inReferencia, true, true, true, false);
            _bc.InitMasterUC(this.uc_DatosAc_MasterAcClase, AppMasters.acClase, true, true, true, false);
            _bc.InitMasterUC(this.uc_DatosAc_MasteProyecto, AppMasters.coProyecto, true, true, true, false);
            _bc.InitMasterUC(this.uc_DatosAc_MasterLocFisica, AppMasters.glLocFisica, true, true, true, false);
            _bc.InitMasterUC(this.uc_DatosAc_MasterCcosto, AppMasters.coCentroCosto, true, true, true, false);
            #endregion
            #region Detalle
            _bc.InitMasterUC(this.uc_Detalle_MasterReferencia, AppMasters.inReferencia, true, true, true, false);
            #endregion
            #region Saldos

            string periodo = _bc.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_Periodo);
            this.uc_Saldos_PerEditPeriodo.DateTime = DateTime.Parse(periodo);
            #endregion
            #region Saldos Moneda Local
            //Combo
            TablesResources.GetTableResources(this.cmbAñoMLocal, typeof(AñosSaldos));
            #endregion
            #region Saldos Moneda Extranjera
            TablesResources.GetTableResources(this.cmbAñoMExtranjera, typeof(AñosSaldos));
            #endregion
        }

        /// <summary>
        /// Inicializa las pestañas
        /// </summary>
        private void InitTabs()
        {
            this.tp_Busqueda.PageVisible = true;
            this.tp_DatosAcActCtrl.PageVisible = false;
            this.tp_Detalle.PageVisible = false;
            this.tp_MonedaLocal.PageVisible = false;
            this.tp_saldos.PageVisible = false;
            this.tp_MonedaExtranjera.PageVisible = false;
        }

        /// <summary>
        /// Funcion que carga el data en la grilla
        /// </summary>
        /// <param name="firstTime">valida que si es la primera vez</param>
        private void LoadData(bool firstTime)
        {
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
                    this.uc_DatosAc_MasterRef.Value = this._acControlsel.inReferenciaID.Value;
                    this.uc_DatosAc_MasterLocFisica.Value = this._acControlsel.LocFisicaID.Value;
                    this.uc_DatosAc_MasterCcosto.Value = this._acControlsel.CentroCostoID.Value;
                    this.uc_DatosAc_MasterAcClase.Value = this._acControlsel.ActivoClaseID.Value;
                    this.txtPlaquetaDatosAc.Text = this._acControlsel.PlaquetaID.Value;
                    this.txtSerialDatosAc.Text = this._acControlsel.SerialID.Value;
                    this.txtFechaCompra.Text = this._acControlsel.Fecha.Value.ToString();
                    this.uc_DatosAc_MasteProyecto.Value = this._acControlsel.ProyectoID.Value;
                    #endregion
                    break;
                case 2:
                    #region Saldos
                    this.txtPLaquetaSaldos.Text = this.txtPlaquetaDetalle.Text;
                    this.txtSerialSaldos.Text = this.txtSerialDetalle.Text;
                    this.txtDescripcionSaldos.Text = this.txtDescripcionDatos.Text;

                    this.gcSaldosDetail.DataSource = this._acActivoSaldo;
                    this.gcSaldosDetail.RefreshDataSource();
                    #endregion
                    break;
                case 3:
                    #region Detalle Activo
                    this.txtPlaquetaDetalle.Text = this.txtPlaquetabuscar.Text;
                    this.txtSerialDetalle.Text = this.txtSerialBuscar.Text;
                    this.uc_Detalle_MasterReferencia.Value = this.uc_DatosAc_MasterRef.Value;
                    this.txtSerialDetalle.Text = this.txtSerialDatosAc.Text;
                    this.txtPlaquetaDetalle.Text = this.txtPlaquetaDatosAc.Text;
                    this.gcDetalle.DataSource = this._glMvtoDeta;
                    this.gcDetalle.RefreshDataSource();
                    #endregion
                    break;
                case 4:
                    #region Saldos Moneda Local
                    this.txtPlaquetaSaldosMLocal.Text = this.txtPlaquetaDetalle.Text;
                    this.txtDescripcionSaldosMLocal.Text = this.txtDescripcionSaldos.Text;
                    this.txtSerialSaldosMLocal.Text = this.txtSerialDetalle.Text;
                    //Carga la grilla
                    this.gcMonedaLocalGrid.DataSource = this._acActivoSaldoMonedaLocal;
                    this.gcMonedaLocalGrid.RefreshDataSource();
                    #endregion
                    break;
                case 5:
                    #region Saldos Moneda Extranjera
                    this.txtPlaquetaSaldosMExtranjera.Text = this.txtPlaquetaDetalle.Text;
                    this.txtDescripcionSaldosMExtranjera.Text = this.txtDescripcionSaldos.Text;
                    this.txtSerialSaldosMExtranjera.Text = this.txtSerialDetalle.Text;
                    //Carga el dataSource
                    this.gcMonedaExtranjera.DataSource = this._acActivoSaldoMonedaExtranjera;
                    this.gcMonedaExtranjera.RefreshDataSource();
                    #endregion
                    break;
                default:
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
                plaquetaID.Width = 70;
                plaquetaID.Visible = true;
                plaquetaID.OptionsColumn.AllowEdit = false;
                this.gvBusqueda.Columns.Add(plaquetaID);

                //Serial
                GridColumn serialID = new GridColumn();
                serialID.FieldName = this._unboundPrefix + "SerialID";
                serialID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_SerialID");
                serialID.UnboundType = UnboundColumnType.Integer;
                serialID.VisibleIndex = 2;
                serialID.Width = 50;
                serialID.Visible = true;
                serialID.OptionsColumn.AllowEdit = false;
                this.gvBusqueda.Columns.Add(serialID);

                //Referencia
                GridColumn inReferenciaID = new GridColumn();
                inReferenciaID.FieldName = this._unboundPrefix + "inReferenciaID";
                inReferenciaID.Caption = _bc.GetResource(LanguageTypes.Forms, documentAltaID + "_inReferenciaID");
                inReferenciaID.UnboundType = UnboundColumnType.String;
                inReferenciaID.VisibleIndex = 3;
                inReferenciaID.Width = 50;
                inReferenciaID.Visible = true;
                inReferenciaID.OptionsColumn.AllowEdit = false;
                this.gvBusqueda.Columns.Add(inReferenciaID);

                //Descripcion
                GridColumn descripcion = new GridColumn();
                descripcion.FieldName = this._unboundPrefix + "Observacion";
                descripcion.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_Observacion");
                descripcion.UnboundType = UnboundColumnType.String;
                descripcion.VisibleIndex = 4;
                descripcion.Width = 200;
                descripcion.Visible = true;
                descripcion.OptionsColumn.AllowEdit = false;
                this.gvBusqueda.Columns.Add(descripcion);

                //Clase
                GridColumn clase = new GridColumn();
                clase.FieldName = this._unboundPrefix + "ActivoClaseID";
                clase.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_ActivoClaseID");
                clase.UnboundType = UnboundColumnType.String;
                clase.VisibleIndex = 5;
                clase.Width = 70;
                clase.Visible = true;
                clase.OptionsColumn.AllowEdit = false;
                this.gvBusqueda.Columns.Add(clase);

                //Tipo
                GridColumn ActivoTipoID = new GridColumn();
                ActivoTipoID.FieldName = this._unboundPrefix + "ActivoTipoID";
                ActivoTipoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_ActivoTipoID");
                ActivoTipoID.UnboundType = UnboundColumnType.String;
                ActivoTipoID.VisibleIndex = 6;
                ActivoTipoID.Width = 70;
                ActivoTipoID.Visible = true;
                ActivoTipoID.OptionsColumn.AllowEdit = false;
                this.gvBusqueda.Columns.Add(ActivoTipoID);

                //Grupo
                GridColumn grupo = new GridColumn();
                grupo.FieldName = this._unboundPrefix + "ActivoGrupoID";
                grupo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_ActivoGrupoID");
                grupo.UnboundType = UnboundColumnType.String;
                grupo.VisibleIndex = 7;
                grupo.Width = 70;
                grupo.Visible = true;
                grupo.OptionsColumn.AllowEdit = false;
                this.gvBusqueda.Columns.Add(grupo);

                //Modelo
                GridColumn modelo = new GridColumn();
                modelo.FieldName = this._unboundPrefix + "Modelo";
                modelo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_Modelo");
                modelo.UnboundType = UnboundColumnType.String;
                modelo.VisibleIndex = 8;
                modelo.Width = 50;
                modelo.Visible = true;
                modelo.OptionsColumn.AllowEdit = false;
                this.gvBusqueda.Columns.Add(modelo);

                //Localizacion Fisica
                GridColumn locFisica = new GridColumn();
                locFisica.FieldName = this._unboundPrefix + "LocFisicaID";
                locFisica.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_LocFisicaID");
                locFisica.UnboundType = UnboundColumnType.String;
                locFisica.VisibleIndex = 9;
                locFisica.Width = 50;
                locFisica.Visible = true;
                locFisica.OptionsColumn.AllowEdit = false;
                this.gvBusqueda.Columns.Add(locFisica);

                //Proyecto
                GridColumn proyecto = new GridColumn();
                proyecto.FieldName = this._unboundPrefix + "ProyectoID";
                proyecto.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_ProyectoID");
                proyecto.UnboundType = UnboundColumnType.String;
                proyecto.VisibleIndex = 10;
                proyecto.Width = 100;
                proyecto.Visible = true;
                proyecto.OptionsColumn.AllowEdit = false;
                this.gvBusqueda.Columns.Add(proyecto);

                //Centro de Costo
                GridColumn centroCosto = new GridColumn();
                centroCosto.FieldName = this._unboundPrefix + "CentroCostoID";
                centroCosto.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_CentroCostoID");
                centroCosto.UnboundType = UnboundColumnType.String;
                centroCosto.VisibleIndex = 11;
                centroCosto.Width = 70;
                centroCosto.Visible = true;
                centroCosto.OptionsColumn.AllowEdit = false;
                this.gvBusqueda.Columns.Add(centroCosto);
                #endregion
                #region Detalle Activo

                //Serial
                GridColumn SerialID = new GridColumn();
                SerialID.FieldName = this._unboundPrefix + "SerialID";
                SerialID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_SerialID");
                SerialID.UnboundType = UnboundColumnType.String;
                SerialID.VisibleIndex = 1;
                SerialID.Width = 50;
                SerialID.Visible = true;
                SerialID.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(SerialID);

                //Plaqueta
                GridColumn DatoAdd2 = new GridColumn();
                DatoAdd2.FieldName = this._unboundPrefix + "DatoAdd2";
                DatoAdd2.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_ModificacionPlaqueta");
                DatoAdd2.UnboundType = UnboundColumnType.String;
                DatoAdd2.VisibleIndex = 2;
                DatoAdd2.Width = 60;
                DatoAdd2.Visible = true;
                DatoAdd2.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(DatoAdd2);

                //PrefijoID
                GridColumn prefijo_Doc = new GridColumn();
                prefijo_Doc.FieldName = this._unboundPrefix + "Prefijo_Documento";
                prefijo_Doc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_PrefijoID/DocumentoNro");
                prefijo_Doc.UnboundType = UnboundColumnType.String;
                prefijo_Doc.VisibleIndex = 3;
                prefijo_Doc.Width = 100;
                prefijo_Doc.Visible = true;
                prefijo_Doc.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(prefijo_Doc);

                //Fecha
                GridColumn Fecha = new GridColumn();
                Fecha.FieldName = this._unboundPrefix + "Fecha";
                Fecha.Caption = _bc.GetResource(LanguageTypes.Forms, documentAltaID + "_Fecha");
                Fecha.UnboundType = UnboundColumnType.DateTime;
                Fecha.VisibleIndex = 5;
                Fecha.Width = 100;
                Fecha.Visible = true;
                Fecha.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(Fecha);

                //MvtoTipoActID
                GridColumn MvtoTipoActID = new GridColumn();
                MvtoTipoActID.FieldName = this._unboundPrefix + "MvtoTipoActID";
                MvtoTipoActID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_TipoMovimieno");
                MvtoTipoActID.UnboundType = UnboundColumnType.String;
                MvtoTipoActID.VisibleIndex = 6;
                MvtoTipoActID.Width = 100;
                MvtoTipoActID.Visible = true;
                MvtoTipoActID.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(MvtoTipoActID);

                //Descriptivo del Moviemiento
                GridColumn Descriptivo = new GridColumn();
                Descriptivo.FieldName = this._unboundPrefix + "Descriptivo";
                Descriptivo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_Descriptivo");
                Descriptivo.UnboundType = UnboundColumnType.String;
                Descriptivo.VisibleIndex = 7;
                Descriptivo.Width = 120;
                Descriptivo.Visible = true;
                Descriptivo.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(Descriptivo);

                //DescripTExt
                GridColumn Descripcion = new GridColumn();
                Descripcion.FieldName = this._unboundPrefix + "DescripTExt";
                Descripcion.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_Descripcion");
                Descripcion.UnboundType = UnboundColumnType.String;
                Descripcion.VisibleIndex = 8;
                Descripcion.Width = 140;
                Descripcion.Visible = true;
                Descripcion.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(Descripcion);

                //ProyectoID
                GridColumn ProyectoID = new GridColumn();
                ProyectoID.FieldName = this._unboundPrefix + "ProyectoID";
                ProyectoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_ProyectoID");
                ProyectoID.UnboundType = UnboundColumnType.String;
                ProyectoID.VisibleIndex = 9;
                ProyectoID.Width = 100;
                ProyectoID.Visible = true;
                ProyectoID.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(ProyectoID);

                //CentroCosto
                GridColumn CentroCosto = new GridColumn();
                CentroCosto.FieldName = this._unboundPrefix + "CentroCostoID";
                CentroCosto.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_CentroCosto");
                CentroCosto.UnboundType = UnboundColumnType.String;
                CentroCosto.VisibleIndex = 10;
                CentroCosto.Width = 100;
                CentroCosto.Visible = true;
                CentroCosto.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(CentroCosto);

                #endregion
                #region Saldos detail

                //Componente
                GridColumn Componente = new GridColumn();
                Componente.FieldName = this._unboundPrefix + "acComponenteID";
                Componente.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_ComponenteID"); ;
                Componente.UnboundType = UnboundColumnType.String;
                Componente.VisibleIndex = 1;
                Componente.Width = 200;
                Componente.Visible = true;
                Componente.OptionsColumn.AllowEdit = false;
                this.gvSaldosDetail.Columns.Add(Componente);

                //DecriptivoComponente
                GridColumn DescriptivoCompnt = new GridColumn();
                DescriptivoCompnt.FieldName = this._unboundPrefix + "Descriptivo";
                DescriptivoCompnt.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_Descriptivo"); ;
                DescriptivoCompnt.UnboundType = UnboundColumnType.String;
                DescriptivoCompnt.VisibleIndex = 2;
                DescriptivoCompnt.Width = 200;
                DescriptivoCompnt.Visible = true;
                DescriptivoCompnt.OptionsColumn.AllowEdit = false;
                this.gvSaldosDetail.Columns.Add(DescriptivoCompnt);

                //Saldo Moneda local
                GridColumn sVlrMdaLoc = new GridColumn();
                sVlrMdaLoc.FieldName = this._unboundPrefix + "SaldoMLoc";
                sVlrMdaLoc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_vlrMdaLoc");
                sVlrMdaLoc.UnboundType = UnboundColumnType.Decimal;
                sVlrMdaLoc.VisibleIndex = 3;
                sVlrMdaLoc.Width = 200;
                sVlrMdaLoc.Visible = true;
                sVlrMdaLoc.OptionsColumn.AllowEdit = false;
                this.gvSaldosDetail.Columns.Add(sVlrMdaLoc);

                //Saldo Moneda Ext
                GridColumn sVlrMdaExt = new GridColumn();
                sVlrMdaExt.FieldName = this._unboundPrefix + "SaldoMExt";
                sVlrMdaExt.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_vlrMdaExt");
                sVlrMdaExt.UnboundType = UnboundColumnType.Decimal;
                sVlrMdaExt.VisibleIndex = 4;
                sVlrMdaExt.Width = 200;
                sVlrMdaExt.Visible = _bc.AdministrationModel.MultiMoneda;
                sVlrMdaExt.OptionsColumn.AllowEdit = false;
                this.gvSaldosDetail.Columns.Add(sVlrMdaExt);

                #endregion
                #region Saldos Moneda Local

                GridColumn cta = new GridColumn();
                cta.FieldName = this._unboundPrefix + "CuentaID";
                cta.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_Cuenta"); ;
                cta.UnboundType = UnboundColumnType.String;
                cta.VisibleIndex = 1;
                cta.Width = 80;
                cta.Visible = true;
                cta.OptionsColumn.AllowEdit = false;
                this.gvMLocalGrid.Columns.Add(cta);

                //Enero
                GridColumn enero = new GridColumn();
                enero.FieldName = this._unboundPrefix + "Enero";
                enero.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_Enero"); ;
                enero.UnboundType = UnboundColumnType.String;
                enero.VisibleIndex = 2;
                enero.Width = 80;
                enero.Visible = true;
                enero.OptionsColumn.AllowEdit = false;
                this.gvMLocalGrid.Columns.Add(enero);

                //Febrero
                GridColumn febrero = new GridColumn();
                febrero.FieldName = this._unboundPrefix + "Febrero";
                febrero.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_Febrero"); ;
                febrero.UnboundType = UnboundColumnType.String;
                febrero.VisibleIndex = 3;
                febrero.Width = 80;
                febrero.Visible = true;
                febrero.OptionsColumn.AllowEdit = false;
                this.gvMLocalGrid.Columns.Add(febrero);

                //Marzo
                GridColumn marzo = new GridColumn();
                marzo.FieldName = this._unboundPrefix + "Marzo";
                marzo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_Marzo");
                marzo.UnboundType = UnboundColumnType.Decimal;
                marzo.VisibleIndex = 4;
                marzo.Width = 80;
                marzo.Visible = true;
                marzo.OptionsColumn.AllowEdit = false;
                this.gvMLocalGrid.Columns.Add(marzo);

                //Abril
                GridColumn abril = new GridColumn();
                abril.FieldName = this._unboundPrefix + "Abril";
                abril.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_Abril");
                abril.UnboundType = UnboundColumnType.Decimal;
                abril.VisibleIndex = 5;
                abril.Width = 80;
                abril.Visible = _bc.AdministrationModel.MultiMoneda;
                abril.OptionsColumn.AllowEdit = false;
                this.gvMLocalGrid.Columns.Add(abril);

                //Mayo
                GridColumn mayo = new GridColumn();
                mayo.FieldName = this._unboundPrefix + "Mayo";
                mayo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_Mayo");
                mayo.UnboundType = UnboundColumnType.Decimal;
                mayo.VisibleIndex = 6;
                mayo.Width = 80;
                mayo.Visible = _bc.AdministrationModel.MultiMoneda;
                mayo.OptionsColumn.AllowEdit = false;
                this.gvMLocalGrid.Columns.Add(mayo);

                //Junio
                GridColumn junio = new GridColumn();
                junio.FieldName = this._unboundPrefix + "Junio";
                junio.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_Junio");
                junio.UnboundType = UnboundColumnType.Decimal;
                junio.VisibleIndex = 7;
                junio.Width = 80;
                junio.Visible = _bc.AdministrationModel.MultiMoneda;
                junio.OptionsColumn.AllowEdit = false;
                this.gvMLocalGrid.Columns.Add(junio);

                //Julio
                GridColumn julio = new GridColumn();
                julio.FieldName = this._unboundPrefix + "Julio";
                julio.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_Julio");
                julio.UnboundType = UnboundColumnType.Decimal;
                julio.VisibleIndex = 8;
                julio.Width = 80;
                julio.Visible = _bc.AdministrationModel.MultiMoneda;
                julio.OptionsColumn.AllowEdit = false;
                this.gvMLocalGrid.Columns.Add(julio);

                //Agosto
                GridColumn agosto = new GridColumn();
                agosto.FieldName = this._unboundPrefix + "Agosto";
                agosto.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_Agosto");
                agosto.UnboundType = UnboundColumnType.Decimal;
                agosto.VisibleIndex = 9;
                agosto.Width = 80;
                agosto.Visible = _bc.AdministrationModel.MultiMoneda;
                agosto.OptionsColumn.AllowEdit = false;
                this.gvMLocalGrid.Columns.Add(agosto);

                //Septiembre
                GridColumn septiembre = new GridColumn();
                septiembre.FieldName = this._unboundPrefix + "Septiembre";
                septiembre.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_Septiembre");
                septiembre.UnboundType = UnboundColumnType.Decimal;
                septiembre.VisibleIndex = 10;
                septiembre.Width = 80;
                septiembre.Visible = _bc.AdministrationModel.MultiMoneda;
                septiembre.OptionsColumn.AllowEdit = false;
                this.gvMLocalGrid.Columns.Add(septiembre);

                //Octubre
                GridColumn octubre = new GridColumn();
                octubre.FieldName = this._unboundPrefix + "Octubre";
                octubre.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_Octubre");
                octubre.UnboundType = UnboundColumnType.Decimal;
                octubre.VisibleIndex = 11;
                octubre.Width = 80;
                octubre.Visible = _bc.AdministrationModel.MultiMoneda;
                octubre.OptionsColumn.AllowEdit = false;
                this.gvMLocalGrid.Columns.Add(octubre);

                //Noviembre
                GridColumn noviembre = new GridColumn();
                noviembre.FieldName = this._unboundPrefix + "Noviembre";
                noviembre.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_Noviembre");
                noviembre.UnboundType = UnboundColumnType.Decimal;
                noviembre.VisibleIndex = 12;
                noviembre.Width = 80;
                noviembre.Visible = _bc.AdministrationModel.MultiMoneda;
                noviembre.OptionsColumn.AllowEdit = false;
                this.gvMLocalGrid.Columns.Add(noviembre);

                //Diciembre
                GridColumn diciembre = new GridColumn();
                diciembre.FieldName = this._unboundPrefix + "Diciembre";
                diciembre.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_Diciembre");
                diciembre.UnboundType = UnboundColumnType.Decimal;
                diciembre.VisibleIndex = 13;
                diciembre.Width = 80;
                diciembre.Visible = _bc.AdministrationModel.MultiMoneda;
                diciembre.OptionsColumn.AllowEdit = false;
                this.gvMLocalGrid.Columns.Add(diciembre);
                #endregion
                #region Saldos Moneda Extrajera

                GridColumn ctaExtranjera = new GridColumn();
                ctaExtranjera.FieldName = this._unboundPrefix + "CuentaID";
                ctaExtranjera.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_Cuenta"); ;
                ctaExtranjera.UnboundType = UnboundColumnType.String;
                ctaExtranjera.VisibleIndex = 1;
                ctaExtranjera.Width = 80;
                ctaExtranjera.Visible = true;
                ctaExtranjera.OptionsColumn.AllowEdit = false;
                this.gvMonedaExtranjerGrid.Columns.Add(ctaExtranjera);

                //Enero
                GridColumn eneroExtranjera = new GridColumn();
                eneroExtranjera.FieldName = this._unboundPrefix + "Enero";
                eneroExtranjera.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_Enero"); ;
                eneroExtranjera.UnboundType = UnboundColumnType.String;
                eneroExtranjera.VisibleIndex = 2;
                eneroExtranjera.Width = 80;
                eneroExtranjera.Visible = true;
                eneroExtranjera.OptionsColumn.AllowEdit = false;
                this.gvMonedaExtranjerGrid.Columns.Add(eneroExtranjera);

                //Febrero
                GridColumn febreroExtran = new GridColumn();
                febreroExtran.FieldName = this._unboundPrefix + "Febrero";
                febreroExtran.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_Febrero"); ;
                febreroExtran.UnboundType = UnboundColumnType.String;
                febreroExtran.VisibleIndex = 3;
                febreroExtran.Width = 80;
                febreroExtran.Visible = true;
                febreroExtran.OptionsColumn.AllowEdit = false;
                this.gvMonedaExtranjerGrid.Columns.Add(febreroExtran);

                //Marzo
                GridColumn marzoExt = new GridColumn();
                marzoExt.FieldName = this._unboundPrefix + "Marzo";
                marzoExt.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_Marzo");
                marzoExt.UnboundType = UnboundColumnType.Decimal;
                marzoExt.VisibleIndex = 4;
                marzoExt.Width = 80;
                marzoExt.Visible = true;
                marzoExt.OptionsColumn.AllowEdit = false;
                this.gvMonedaExtranjerGrid.Columns.Add(marzoExt);

                //Abril
                GridColumn abrilExtra = new GridColumn();
                abrilExtra.FieldName = this._unboundPrefix + "Abril";
                abrilExtra.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_Abril");
                abrilExtra.UnboundType = UnboundColumnType.Decimal;
                abrilExtra.VisibleIndex = 5;
                abrilExtra.Width = 80;
                abrilExtra.Visible = _bc.AdministrationModel.MultiMoneda;
                abrilExtra.OptionsColumn.AllowEdit = false;
                this.gvMonedaExtranjerGrid.Columns.Add(abrilExtra);

                //Mayo
                GridColumn mayoExtra = new GridColumn();
                mayoExtra.FieldName = this._unboundPrefix + "Mayo";
                mayoExtra.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_Mayo");
                mayoExtra.UnboundType = UnboundColumnType.Decimal;
                mayoExtra.VisibleIndex = 6;
                mayoExtra.Width = 80;
                mayoExtra.Visible = _bc.AdministrationModel.MultiMoneda;
                mayoExtra.OptionsColumn.AllowEdit = false;
                this.gvMonedaExtranjerGrid.Columns.Add(mayoExtra);

                //Junio
                GridColumn junioExtra = new GridColumn();
                junioExtra.FieldName = this._unboundPrefix + "Junio";
                junioExtra.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_Junio");
                junioExtra.UnboundType = UnboundColumnType.Decimal;
                junioExtra.VisibleIndex = 7;
                junioExtra.Width = 80;
                junioExtra.Visible = _bc.AdministrationModel.MultiMoneda;
                junioExtra.OptionsColumn.AllowEdit = false;
                this.gvMonedaExtranjerGrid.Columns.Add(junioExtra);

                //Julio
                GridColumn julioExtra = new GridColumn();
                julioExtra.FieldName = this._unboundPrefix + "Julio";
                julioExtra.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_Julio");
                julioExtra.UnboundType = UnboundColumnType.Decimal;
                julioExtra.VisibleIndex = 8;
                julioExtra.Width = 80;
                julioExtra.Visible = _bc.AdministrationModel.MultiMoneda;
                julioExtra.OptionsColumn.AllowEdit = false;
                this.gvMonedaExtranjerGrid.Columns.Add(julioExtra);

                //Agosto
                GridColumn agostoExtra = new GridColumn();
                agostoExtra.FieldName = this._unboundPrefix + "Agosto";
                agostoExtra.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_Agosto");
                agostoExtra.UnboundType = UnboundColumnType.Decimal;
                agostoExtra.VisibleIndex = 9;
                agostoExtra.Width = 80;
                agostoExtra.Visible = _bc.AdministrationModel.MultiMoneda;
                agostoExtra.OptionsColumn.AllowEdit = false;
                this.gvMonedaExtranjerGrid.Columns.Add(agostoExtra);

                //Septiembre
                GridColumn septiembreExtra = new GridColumn();
                septiembreExtra.FieldName = this._unboundPrefix + "Septiembre";
                septiembreExtra.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_Septiembre");
                septiembreExtra.UnboundType = UnboundColumnType.Decimal;
                septiembreExtra.VisibleIndex = 10;
                septiembreExtra.Width = 80;
                septiembreExtra.Visible = _bc.AdministrationModel.MultiMoneda;
                septiembreExtra.OptionsColumn.AllowEdit = false;
                this.gvMonedaExtranjerGrid.Columns.Add(septiembreExtra);

                //Octubre
                GridColumn octubreExtra = new GridColumn();
                octubreExtra.FieldName = this._unboundPrefix + "Octubre";
                octubreExtra.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_Octubre");
                octubreExtra.UnboundType = UnboundColumnType.Decimal;
                octubreExtra.VisibleIndex = 11;
                octubreExtra.Width = 80;
                octubreExtra.Visible = _bc.AdministrationModel.MultiMoneda;
                octubreExtra.OptionsColumn.AllowEdit = false;
                this.gvMonedaExtranjerGrid.Columns.Add(octubreExtra);

                //Noviembre
                GridColumn noviembreExtra = new GridColumn();
                noviembreExtra.FieldName = this._unboundPrefix + "Noviembre";
                noviembreExtra.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_Noviembre");
                noviembreExtra.UnboundType = UnboundColumnType.Decimal;
                noviembreExtra.VisibleIndex = 12;
                noviembreExtra.Width = 80;
                noviembreExtra.Visible = _bc.AdministrationModel.MultiMoneda;
                noviembreExtra.OptionsColumn.AllowEdit = false;
                this.gvMonedaExtranjerGrid.Columns.Add(noviembreExtra);

                //Diciembre
                GridColumn diciembreExtra = new GridColumn();
                diciembreExtra.FieldName = this._unboundPrefix + "Diciembre";
                diciembreExtra.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentAltaID + "_Diciembre");
                diciembreExtra.UnboundType = UnboundColumnType.Decimal;
                diciembreExtra.VisibleIndex = 13;
                diciembreExtra.Width = 80;
                diciembreExtra.Visible = _bc.AdministrationModel.MultiMoneda;
                diciembreExtra.OptionsColumn.AllowEdit = false;
                this.gvMonedaExtranjerGrid.Columns.Add(diciembreExtra);
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
            for (int i = 0; i < this._comprobanteFooter.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        this.txtComponenteLocal1.EditValue = this._comprobanteFooter[i].vlrMdaLoc.Value;
                        this.txtComponenteExt1.EditValue = this._comprobanteFooter[i].vlrMdaExt.Value;
                        break;
                    case 1:
                        this.txtComponenteLocal2.EditValue = this._comprobanteFooter[i].vlrMdaLoc.Value;
                        this.txtComponenteExt2.EditValue = this._comprobanteFooter[i].vlrMdaExt.Value;
                        break;
                    case 2:
                        this.txtComponenteLocal3.EditValue = this._comprobanteFooter[i].vlrMdaLoc.Value;
                        this.txtComponenteExt3.EditValue = this._comprobanteFooter[i].vlrMdaExt.Value;
                        break;
                    case 3:
                        this.txtComponenteLocal4.EditValue = this._comprobanteFooter[i].vlrMdaLoc.Value;
                        this.txtComponenteExt4.EditValue = this._comprobanteFooter[i].vlrMdaExt.Value;
                        break;
                    case 4:
                        this.txtComponenteLocal5.EditValue = this._comprobanteFooter[i].vlrMdaLoc.Value;
                        this.txtComponenteExt5.EditValue = this._comprobanteFooter[i].vlrMdaExt.Value;
                        break;
                    case 5:
                        this.txtComponenteLocal6.EditValue = this._comprobanteFooter[i].vlrMdaLoc.Value;
                        this.txtComponenteExt6.EditValue = this._comprobanteFooter[i].vlrMdaExt.Value;
                        break;
                    case 6:
                        this.txtComponenteLocal7.EditValue = this._comprobanteFooter[i].vlrMdaLoc.Value;
                        this.txtComponenteExt7.EditValue = this._comprobanteFooter[i].vlrMdaExt.Value;
                        break;
                    case 7:
                        this.txtComponenteLocal8.EditValue = this._comprobanteFooter[i].vlrMdaLoc.Value;
                        this.txtComponenteExt8.EditValue = this._comprobanteFooter[i].vlrMdaExt.Value;
                        break;
                }
            }
            if (this._comprobanteFooter.Count == 0)
            {
                this.txtComponenteLocal8.EditValue = string.Empty;
                this.txtComponenteExt8.EditValue = string.Empty;
                this.txtComponenteExt7.EditValue = string.Empty;
                this.txtComponenteLocal7.EditValue = string.Empty;
                this.txtComponenteExt6.EditValue = string.Empty;
                this.txtComponenteLocal6.EditValue = string.Empty;
                this.txtComponenteExt5.EditValue = string.Empty;
                this.txtComponenteLocal5.EditValue = string.Empty;
                this.txtComponenteExt4.EditValue = string.Empty;
                this.txtComponenteLocal4.EditValue = string.Empty;
                this.txtComponenteExt3.EditValue = string.Empty;
                this.txtComponenteLocal3.EditValue = string.Empty;
                this.txtComponenteExt2.EditValue = string.Empty;
                this.txtComponenteLocal2.EditValue = string.Empty;
                this.txtComponenteExt1.EditValue = string.Empty;
                this.txtComponenteLocal1.EditValue = string.Empty;
            }
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
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;

            DTO_acActivoControl dtCtrl = new DTO_acActivoControl();

            dtCtrl.PlaquetaID.Value = this.txtPlaquetabuscar.Text;
            dtCtrl.SerialID.Value = this.txtSerialBuscar.Text;
            dtCtrl.LocFisicaID.Value = this.uc_Busqueda_MasterLocFisicaID.Value;
            dtCtrl.ProyectoID.Value = this.uc_Busqueda_MasterProyectoID.Value;
            dtCtrl.CentroCostoID.Value = this.uc_Busqueda_MasterCentroCosto.Value;

            if (this._activo.Count == 0)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
                MessageForm mform = new MessageForm(result);
                mform.ShowDialog();
            }
            else
                this.LoadData(true);
        }
        #endregion

        #region SaldosHeader
        /// <summary>
        /// Evento q calcula que la iinformacion de los sados sea filtrada por el periodo actual.
        /// </summary>
        /// <param name="sender">Eventot</param>
        /// <param name="e">Evento</param>
        private void uc_Saldos_PerEditPeriodo_Leave(object sender, EventArgs e)
        {
            string periodoSaldos = this.uc_Saldos_PerEditPeriodo.DateTime.ToString();
            this.LoadPageData(2);
        }
        #endregion

        #region SaldosMOnedaLocal
        /// <summary>
        /// Evento que carga la grilla al momento de salir del control
        /// </summary>
        /// <param name="sender">Evento</param>
        /// <param name="e">Evento</param>
        private void cmbAñoMLocal_Leave(object sender, EventArgs e)
        {
            int año = Convert.ToInt16((this.cmbAñoMLocal.SelectedItem as ComboBoxItem).Value);

            this.gcMonedaLocalGrid.DataSource = this._acActivoSaldoMonedaLocal;
            this.gcMonedaLocalGrid.RefreshDataSource();
        }
        #endregion

        #region Saldos moneda extranjera
        /// <summary>
        /// Evento que carga la grilla al momento de salir del control
        /// </summary>
        /// <param name="sender">Evento</param>
        /// <param name="e">Evento</param>
        private void cmbAñoMExtranjera_Leave(object sender, EventArgs e)
        {
            int añoExtra = Convert.ToInt16((this.cmbAñoMExtranjera.SelectedItem as ComboBoxItem).Value);

            this.gcMonedaExtranjera.DataSource = this._acActivoSaldoMonedaExtranjera;
            this.gcMonedaExtranjera.RefreshDataSource();
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

                if (info.HitTest != GridHitTest.Column)
                {
                    string msgTitleData = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_GetData);
                    string msgGetData = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.GetDocument);

                    if (MessageBox.Show(msgGetData, msgTitleData, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (info.InRow || info.InRowCell)
                        {
                            int fila = info.RowHandle;
                            this._acControlsel = this._activo[fila];

                            this._activoID = Convert.ToInt32(this._activo[fila].ActivoID.Value);
                            this._glMvtoDeta = this._bc.AdministrationModel.glMovimientoDeta_GetBy_ActivoFind(_activoID);

                            #region Busca los saldos y carga el DTO
                            this._acClaseID = this._activo[fila].ActivoClaseID.Value;
                            //string perStr = _bc.GetControlValueByCompany(ModulesPrefix.ac, AppControl.co_Periodo);
                            DateTime period = this.uc_Saldos_PerEditPeriodo.DateTime;
                            //this.uc_Saldos_PerEditPeriodo.DateTime = Convert.ToDateTime(perStr);
                            this.tp_saldos.PageVisible = true;
                            #endregion

                            #region Saldos Moneda Local

                            int añoLocal = Convert.ToInt16((this.cmbAñoMLocal.SelectedItem as ComboBoxItem).Value);
                            this.tp_MonedaLocal.PageVisible = true;
                            #endregion

                            #region Saldos Moneda Extranjera
                            int añoExtranjera = Convert.ToInt16((this.cmbAñoMExtranjera.SelectedItem as ComboBoxItem).Value);
                            this.tp_MonedaExtranjera.PageVisible = true;
                            #endregion

                            this.tp_DatosAcActCtrl.PageVisible = true;
                            this.tp_Detalle.PageVisible = true;
                            this.LoadPageData(1);
                            this.LoadPageData(3);
                            this.LoadPageData(2);
                            this.LoadPageData(4);
                            this.LoadPageData(5);
                        }
                    }
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
        /// Evento que valida la info de cada Dto para asignarle info al cuadro de valores
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">evento</param>
        private void gvDetalle_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            int fila = e.FocusedRowHandle;
           
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

            if (fieldName == "SaldoMLoc" || fieldName == "SaldoMExt")
            {
                e.RepositoryItem = this.SpinEdit;
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

        /// <summary>
        /// Evento para agregar formacto a los campos de moneda.
        /// </summary>
        /// <param name="sender">Evento</param>
        /// <param name="e">Evento</param>
        private void gvMLocalGrid_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

            if (fieldName == "Enero" || fieldName == "Febrero" || fieldName == "Marzo" || fieldName == "Abril" || fieldName == "Mayo"
                || fieldName == "Junio" || fieldName == "Julio" || fieldName == "Agosto" || fieldName == "Septiembre" || fieldName == "Octubre"
                || fieldName == "Noviembre" || fieldName == "Diciembre")
            {
                e.RepositoryItem = this.SpinEdit;
            }
        }
        #endregion

        #endregion
    }
}
