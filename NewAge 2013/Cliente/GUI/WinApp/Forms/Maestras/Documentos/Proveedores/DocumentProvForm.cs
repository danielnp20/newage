using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using NewAge.DTO.Attributes;
using NewAge.DTO.Reportes;
using NewAge.Librerias.Project;
using SentenceTransformer;
using System.Text.RegularExpressions;
using NewAge.Cliente.GUI.WinApp.Reports;
using NewAge.ReportesComunes;
using DevExpress.XtraGrid;
using DevExpress.XtraEditors;
using System.Runtime.Serialization;
using System.Threading;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Maestra de comprobante auxiliar
    /// </summary>
    public partial class DocumentProvForm : DocumentForm
    {
        //public DocumentProvForm()
        //{
        //   InitializeComponent();
        //}

        #region Delegados

        /// <summary>
        /// Delegado que finaliza el proceso de importación
        /// </summary>
        protected override void RefreshGridMethod()
        {
            this.LoadData(true);
            FormProvider.Master.itemSendtoAppr.Enabled = false;
        }

        protected delegate void AfterImport();
        protected AfterImport afterImportDelegate;
        /// <summary>
        /// Delegado que actualiza el formulario despues de importat un documento
        /// </summary>
        protected void AfterImportMethod() 
        {
            this.isImporting = true;
            this.gcDocument.Focus();
            if (this.isImporting)
                this.isImporting = false;
            else 
            {
                this.LoadEditGridData(false, this.indexFila);
            }
            this.EnableFooter(false);
            this.gcCargos.Enabled = false;
        }
        #endregion

        #region Variables privadas
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();

        //Variables para gvCargos      
        private bool newRegCargo = false;
        private bool isImporting = false;
        private int indexFilaCargos = 0;
        private string lastColNameCargos = string.Empty;
        private string unboundPrefixCargo = "UnboundPref_";

        //Variables para MasterComplex
        private List<DTO_glConsultaFiltro> _filtrosRef;
        private Dictionary<string, string> _pksPar1 = new Dictionary<string,string>();
        private Dictionary<string, string> _pksPar2 = new Dictionary<string,string>();

        #endregion

        #region Variables Protected
        //Variables formulario
        protected DTO_prSolicitud data = null;
        protected List<DTO_prDetalleDocu> _listSolicitudProyectos = null;
        //Variables Moneda
        protected string monedaLocal;
        protected string monedaExtranjera;
        protected string monedaId;
        protected bool biMoneda = false;

        //Indica si el header es valido
        protected bool validHeader;

        //Variables con valores x defecto (glControl)
        protected string defTercero = string.Empty;
        protected string defPrefijo = string.Empty;
        protected string defProyecto = string.Empty;
        protected string defCentroCosto = string.Empty;
        protected string defLineaPresupuesto = string.Empty;
        protected string defConceptoCargo = string.Empty;
        protected string defLugarGeo = string.Empty;
        protected string defLocFisica = string.Empty;
        protected string defAreaFunc = string.Empty;
        protected string codigoBSInventarios = string.Empty;


        //Variables con los recursos 
        protected string _codigoRsx = string.Empty;
        protected string _referenciaRsx = string.Empty;
        protected string _parametro1Rsx = string.Empty;
        protected string _parametro2Rsx = string.Empty;
        protected string _unidadRsx = string.Empty;
        protected string _proyectoRsx = string.Empty;
        protected string _centroCostoRsx = string.Empty;
        protected string _cantidadRsx = string.Empty;
        protected string _porcentajeRsx = string.Empty;
        protected string _descRsx = string.Empty;
        protected string _solCargosRsx = string.Empty;
        protected string _valorUniRsx = string.Empty;
        protected string _ivaUniRsx = string.Empty;

        //variables para funciones particulares
        protected bool cleanDoc = false;
        protected bool isValidCargo = true;
        protected DTO_prSolicitudFooter _rowCurrent = new DTO_prSolicitudFooter();

        #endregion

        #region Propiedades

        //Numero de una fila segun el indice
        protected int NumFila
        {
            get     { return this.data.Footer.FindIndex(det => det.DetalleDocu.Index == this.indexFila); }
        }

        private int NumFilaCargos
        {
            get     {return this.data.Footer[this.indexFila].SolicitudCargos.FindIndex(det => det.Index == this.indexFilaCargos);  }
        }
        
        //BienServicio
        private DTO_prBienServicio _bienServicio = null;
        private DTO_prBienServicio BienServicio
        {
            get      {  return this._bienServicio;   }
            set
            {
                this._bienServicio = value;
                int index = this.cleanDoc ? 0 : this.NumFila;

                if (this.data != null && this.data.Footer.Count > 0)
                {
                    if (value == null)
                    {
                        #region Si servicio no existe
                        this.BienServicioClase = null;

                        this.data.Footer[index].DetalleDocu.CodigoBSID.Value = string.Empty;
                        this.data.Footer[index].DetalleDocu.inReferenciaID.Value = string.Empty;
                        this.data.Footer[index].DetalleDocu.Parametro1.Value = string.Empty;
                        this.data.Footer[index].DetalleDocu.Parametro2.Value = string.Empty;
                        this.data.Footer[index].DetalleDocu.Descriptivo.Value = string.Empty;
                        this.data.Footer[index].DetalleDocu.UnidadInvID.Value = string.Empty;
                        this.data.Footer[index].DetalleDocu.CantidadSol.Value = 0;
                        this.data.Footer[index].ProyectoID = string.Empty;
                        this.data.Footer[index].CentroCostoID = string.Empty;

                        this.masterReferencia.Value = string.Empty;
                        this.txtDesc.Text = string.Empty;

                        this.EnableFooter(false);
                        #endregion
                    }
                    else
                    {
                        this.EnableFooter(true);
                        //if (this.BienServicioClase == null || this.BienServicio.ClaseBSID.Value != this.BienServicioClase.ID.Value)
                        //{
                        this.BienServicioClase = (DTO_glBienServicioClase)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, false, _bienServicio.ClaseBSID.Value, true);
                        //}
                    } 
                }
                this.gvDocument.RefreshData();
            }
        }

        //BienServicioClase
        protected TipoCodigo _tipoCodigo;
        private DTO_glBienServicioClase _bienServicioClase = null;
        private DTO_glBienServicioClase BienServicioClase
        {
            get     {  return this._bienServicioClase; }
            set
            {
                this._bienServicioClase = value;
                if (value != null )//&& this._tipoCodigo != (TipoCodigo)Enum.Parse(typeof(TipoCodigo), this._bienServicioClase.TipoCodigo.Value.Value.ToString()))
                {
                    if (string.IsNullOrEmpty(this._bienServicioClase.TipoCodigo.Value.ToString()))
                    {
                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Pr_TipoCodigoNotExist));
                        return;
                    }
                    this._tipoCodigo = (TipoCodigo)Enum.Parse(typeof(TipoCodigo), this._bienServicioClase.TipoCodigo.Value.Value.ToString());

                    #region Habilita los controles
                    #region Clase Servicios
                    if (this._tipoCodigo == TipoCodigo.Servicio || this._tipoCodigo == TipoCodigo.Suministros || this._tipoCodigo == TipoCodigo.SuministroPersonal)
                    {
                        this.Referencia = null;
                        this.masterReferencia.EnableControl(false);
                        this.btnReferencia.Enabled = false;
                        this.gvDocument.Columns[this.unboundPrefix + "Parametro1"].OptionsColumn.AllowEdit = false;
                        this.gvDocument.Columns[this.unboundPrefix + "Parametro2"].OptionsColumn.AllowEdit = false;
                    }
                    #endregion
                    #region Clase Inventarios
                    else if (this._tipoCodigo == TipoCodigo.Inventario)
                    {
                        DTO_glConsulta consultaRefClase = new DTO_glConsulta();
                        List<DTO_glConsultaFiltro> filtrosRefClase = new List<DTO_glConsultaFiltro>();
                        filtrosRefClase.Add(new DTO_glConsultaFiltro()
                        {
                            CampoFisico = "InventarioInd",
                            OperadorFiltro = OperadorFiltro.Igual,
                            ValorFiltro = "1"
                        });
                        consultaRefClase.Filtros = filtrosRefClase;
                        long count = this._bc.AdministrationModel.MasterSimple_Count(AppMasters.inRefClase, consultaRefClase, null, true);
                        List<DTO_inRefClase> masterRefClase = this._bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.inRefClase, count, 1, consultaRefClase, null, true).Cast<DTO_inRefClase>().ToList();

                        DTO_glConsulta consultaRef = new DTO_glConsulta();
                        List<DTO_glConsultaFiltro> filtrosRef = new List<DTO_glConsultaFiltro>();
                        foreach (DTO_inRefClase refClase in masterRefClase)
                        {
                            filtrosRef.Add(new DTO_glConsultaFiltro()
                            {
                                CampoFisico = "ClaseInvID",
                                OperadorFiltro = OperadorFiltro.Igual,
                                OperadorSentencia = "OR",
                                ValorFiltro = refClase.ID.Value
                            });
                        }
                        consultaRef.Filtros = filtrosRef;

                        if (this.Referencia != null)
                        {
                            List<DTO_glConsultaFiltro> filtrosExtra = new List<DTO_glConsultaFiltro>();
                            filtrosExtra.Add(new DTO_glConsultaFiltro()
                            {
                                CampoFisico = "inReferenciaID",
                                OperadorFiltro = OperadorFiltro.Igual,
                                ValorFiltro = this.Referencia.ID.Value
                            });

                            count = this._bc.AdministrationModel.MasterSimple_Count(AppMasters.inReferencia, consultaRef, filtrosExtra, true);
                         //if (count == 0)
                         //    this.Referencia = null;
                        }

                        this.masterReferencia.EnableControl(true);
                        this.btnReferencia.Enabled = true;
                        _bc.InitMasterUC(this.masterReferencia, AppMasters.inReferencia, true, true, true, false, filtrosRef);
                    }
                    #endregion
                    #region Clase Activos
                    else if (this._tipoCodigo == TipoCodigo.Activo)
                    {
                        this.masterReferencia.EnableControl(true);
                        this.btnReferencia.Enabled = true;
                        DTO_glConsulta consultaRefClase = new DTO_glConsulta();
                        List<DTO_glConsultaFiltro> filtrosRefClase = new List<DTO_glConsultaFiltro>();
                        filtrosRefClase.Add(new DTO_glConsultaFiltro()
                        {
                            CampoFisico = "ActivoFijoInd",
                            OperadorFiltro = OperadorFiltro.Igual,
                            ValorFiltro = "1"
                        });
                        consultaRefClase.Filtros = filtrosRefClase;
                        long count = this._bc.AdministrationModel.MasterSimple_Count(AppMasters.inRefClase, consultaRefClase, null, true);
                        List<DTO_inRefClase> masterRefClase = this._bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.inRefClase, count, 1, consultaRefClase, null, true).Cast<DTO_inRefClase>().ToList();

                        DTO_glConsulta consultaRef = new DTO_glConsulta();
                        List<DTO_glConsultaFiltro> filtrosRef = new List<DTO_glConsultaFiltro>();
                        foreach (DTO_inRefClase refClase in masterRefClase)
                        {
                            filtrosRef.Add(new DTO_glConsultaFiltro()
                            {
                                CampoFisico = "ClaseInvID",
                                OperadorFiltro = OperadorFiltro.Igual,
                                OperadorSentencia = "OR",
                                ValorFiltro = refClase.ID.Value
                            });
                        }
                        consultaRef.Filtros = filtrosRef;

                        if (this.Referencia != null)
                        {
                            List<DTO_glConsultaFiltro> filtrosExtra = new List<DTO_glConsultaFiltro>();
                            filtrosExtra.Add(new DTO_glConsultaFiltro()
                            {
                                CampoFisico = "inReferenciaID",
                                OperadorFiltro = OperadorFiltro.Igual,
                                ValorFiltro = this.Referencia.ID.Value
                            });

                            count = this._bc.AdministrationModel.MasterSimple_Count(AppMasters.inReferencia, consultaRef, filtrosExtra, true);
                            //if (count == 0)
                            //    this.Referencia = null;
                        }

                        this.masterReferencia.EnableControl(true);
                        this.btnReferencia.Enabled = true;
                        _bc.InitMasterUC(this.masterReferencia, AppMasters.inReferencia, true, true, true, false, filtrosRef);

                        this.gvDocument.Columns[this.unboundPrefix + "Parametro1"].OptionsColumn.AllowEdit = false;
                        this.gvDocument.Columns[this.unboundPrefix + "Parametro2"].OptionsColumn.AllowEdit = false;
                    }
                    #endregion
                    else
                    {
                        this.Referencia = null;
                        this.masterReferencia.EnableControl(false);
                        this.btnReferencia.Enabled = false;
                        this.gvDocument.Columns[this.unboundPrefix + "Parametro1"].OptionsColumn.AllowEdit = false;
                        this.gvDocument.Columns[this.unboundPrefix + "Parametro2"].OptionsColumn.AllowEdit = false;
                    }
                    #endregion
                }
            }
        }
        
        //Referencia
        private DTO_inReferencia _referencia = null;
        private DTO_inReferencia Referencia
        {
            get   {  return this._referencia; }
            set
            {
                this._referencia = value;
                int index = this.cleanDoc? 0 : this.NumFila;
                this.EnableFooter(true);

                if (this.data != null && this.data.Footer.Count > 0)
                {
                    if (value == null)
                    {
                        #region Si servicio no existe
                        this.TipoRef = null;

                        this.masterReferencia.Value = string.Empty;
                        this.masterReferencia.EnableControl(false);
                        this.btnReferencia.Enabled = false;
                        this.data.Footer[index].DetalleDocu.inReferenciaID.Value = string.Empty;
                        #endregion
                    }
                    else
                    {
                        this.TipoRef = (DTO_inRefTipo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inRefTipo, false, _referencia.TipoInvID.Value, true);
                        this.masterReferencia.EnableControl(true);
                        this.btnReferencia.Enabled = true;
                    } 
                }
                this.gvDocument.RefreshData();
            }
        }
        
        //Referencia Tipo
        private DTO_inRefTipo _tipoRef = null;
        private DTO_inRefTipo TipoRef
        {
            get    {  return this._tipoRef;  }
            set
            {
                this._tipoRef = value;
                int index = this.cleanDoc ? 0 : this.NumFila;

                if (this.data != null && this.data.Footer.Count > 0)
                {
                    if (value == null)
                    {
                        this._pksPar1 = new Dictionary<string, string>();
                        this._pksPar2 = new Dictionary<string, string>();
                        this.data.Footer[index].DetalleDocu.Parametro1.Value = string.Empty;
                        this.data.Footer[index].DetalleDocu.Parametro2.Value = string.Empty;
                    }
                    else
                    {
                        if (this._tipoCodigo == TipoCodigo.Inventario)
                        {
                            this.gvDocument.Columns[this.unboundPrefix + "Parametro1"].OptionsColumn.AllowEdit = this._tipoRef.Parametro1Ind.Value.Value;
                            this.gvDocument.Columns[this.unboundPrefix + "Parametro2"].OptionsColumn.AllowEdit = this._tipoRef.Parametro2Ind.Value.Value;

                            #region Consulta segun Referencia tipo
                            this._filtrosRef = new List<DTO_glConsultaFiltro>();
                            DTO_glConsultaFiltro filtroRef = new DTO_glConsultaFiltro();
                            filtroRef.CampoFisico = "TipoInvID";
                            filtroRef.ValorFiltro = this._tipoRef.ID.Value;
                            filtroRef.OperadorFiltro = OperadorFiltro.Igual;
                            filtroRef.OperadorSentencia = "and";
                            _filtrosRef.Add(filtroRef);
                            #endregion



                            _pksPar1 = new Dictionary<string, string>();
                            _pksPar1.Add("TipoInvID", this._tipoRef.ID.Value);
                            if (this._tipoRef.Parametro1Ind.Value.Value)
                            {
                                //_pksPar1.Add("TipoInvID", this._tipoRef.ID.Value);
                                _pksPar1.Add("Parametro1ID", this.data.Footer[index].DetalleDocu.Parametro1.Value);
                            }
                            else
                            {
                                this.data.Footer[index].DetalleDocu.Parametro1.Value = _bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Parametro1xDefecto);
                                _pksPar1.Add("Parametro1ID", this.data.Footer[index].DetalleDocu.Parametro1.Value);
                            }

                            _pksPar2 = new Dictionary<string, string>();
                            _pksPar2.Add("TipoInvID", this._tipoRef.ID.Value);
                            if (this._tipoRef.Parametro2Ind.Value.Value)
                            {
                                //_pksPar2.Add("TipoInvID", this._tipoRef.ID.Value);
                                _pksPar2.Add("Parametro2ID", this.data.Footer[index].DetalleDocu.Parametro2.Value);
                            }
                            else
                            {
                                this.data.Footer[index].DetalleDocu.Parametro2.Value = _bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Parametro2xDefecto);
                                _pksPar2.Add("Parametro2ID", this.data.Footer[index].DetalleDocu.Parametro2.Value);
                            }
                        }
                        else
                        {
                            this.data.Footer[index].DetalleDocu.Parametro1.Value = _bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Parametro1xDefecto);
                            this.data.Footer[index].DetalleDocu.Parametro2.Value = _bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Parametro2xDefecto);
                            _pksPar1 = new Dictionary<string, string>();
                            _pksPar1.Add("TipoInvID", this._tipoRef.ID.Value);
                            _pksPar1.Add("Parametro1ID", this.data.Footer[index].DetalleDocu.Parametro1.Value);
                            _pksPar2 = new Dictionary<string, string>();
                            _pksPar2.Add("TipoInvID", this._tipoRef.ID.Value);
                            _pksPar2.Add("Parametro2ID", this.data.Footer[index].DetalleDocu.Parametro2.Value);
                        }
                    } 
                }
            }
        }
        #endregion

        #region Funciones Privadas y protected

        /// <summary>
        /// Genera el reporte del comprobante actual
        /// </summary>
        /// <param name="show">Indica si se debe mostrar o solo generar</param>
        /// <returns>Reporte</returns>
        private void GenerateReport(bool show, bool allFields = false)
        {            
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddCargosGridCols()
        {
            try
            {
                #region Columnas Visibles
                //Proyecto
                GridColumn proyecto = new GridColumn();
                proyecto.FieldName = this.unboundPrefixCargo + "ProyectoID";
                proyecto.Caption = this._proyectoRsx;
                proyecto.UnboundType = UnboundColumnType.String;
                proyecto.VisibleIndex = 1;
                proyecto.Width = 100;
                proyecto.Visible = true;
                proyecto.OptionsColumn.AllowEdit = true;
                this.gvCargos.Columns.Add(proyecto);

                //Centro de costo
                GridColumn ctoCosto = new GridColumn();
                ctoCosto.FieldName = this.unboundPrefixCargo + "CentroCostoID";
                ctoCosto.Caption = this._centroCostoRsx;
                ctoCosto.UnboundType = UnboundColumnType.String;
                ctoCosto.VisibleIndex = 2;
                ctoCosto.Width = 100;
                ctoCosto.Visible = true;
                ctoCosto.OptionsColumn.AllowEdit = true;
                this.gvCargos.Columns.Add(ctoCosto);

                //Centro de costo
                GridColumn percent = new GridColumn();
                percent.FieldName = this.unboundPrefixCargo + "PorcentajeID";
                percent.Caption = this._porcentajeRsx;
                percent.UnboundType = UnboundColumnType.Decimal;
                percent.VisibleIndex = 3;
                percent.Width = 100;
                percent.Visible = true;
                percent.OptionsColumn.AllowEdit = true;
                this.gvCargos.Columns.Add(percent);
                #endregion
                #region Columnas No Visibles

                //NumeroDoc
                GridColumn numDoc = new GridColumn();
                numDoc.FieldName = this.unboundPrefixCargo + "NumeroDoc";
                numDoc.UnboundType = UnboundColumnType.Integer;
                numDoc.Visible = false;
                this.gvCargos.Columns.Add(numDoc);

                //ConsecutivoDetaID
                GridColumn consDeta = new GridColumn();
                consDeta.FieldName = this.unboundPrefixCargo + "ConsecutivoDetaID";
                consDeta.UnboundType = UnboundColumnType.Integer;
                consDeta.Visible = false;
                this.gvCargos.Columns.Add(consDeta);

                //Indice de la fila de la grilla de los cargos
                GridColumn cargoColIndex = new GridColumn();
                cargoColIndex.FieldName = this.unboundPrefixCargo + "Index";
                cargoColIndex.UnboundType = UnboundColumnType.Integer;
                cargoColIndex.Visible = false;
                this.gvCargos.Columns.Add(cargoColIndex);

                //Indice de la fila la grilla principal
                GridColumn detColIndex = new GridColumn();
                detColIndex.FieldName = this.unboundPrefixCargo + "IndexDet";
                detColIndex.UnboundType = UnboundColumnType.Integer;
                detColIndex.Visible = false;
                this.gvCargos.Columns.Add(detColIndex);
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentoProvForm.cs", "AddCargosGridCols"));
            }
        }

        /// <summary>
        /// Valida una fila
        /// </summary>
        /// <param name="fila">Indice de la fila a validar</param>
        /// <returns>Retorna si la info de la fila es valida o no</returns>
        private bool ValidateCargoRow(int fila)
        {
            bool validRow = true;
            bool validField = true;

            try
            {
                string msg;
                string colVal;
                GridColumn col = new GridColumn();

                #region Validacion de nulls y Fks
                string rsxEmpty = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                #region Proyecto
                validField = _bc.ValidGridCell(this.gvCargos, this.unboundPrefixCargo, fila, "ProyectoID", false, true, true, AppMasters.coProyecto);
                if (!validField)
                    validRow = false;
                #endregion
                #region Centro Costo
                validField = _bc.ValidGridCell(this.gvCargos, this.unboundPrefixCargo, fila, "CentroCostoID", false, true, true, AppMasters.coCentroCosto);
                if (!validField)
                    validRow = false;
                #endregion
                #endregion
                #region Validaciones de cantidad
                #region PorcentajeID
                validField = _bc.ValidGridCellValue(this.gvCargos, this.unboundPrefixCargo, fila, "PorcentajeID", false, false, true, false);
                if (!validField)
                    validRow = false;
                #endregion
                #endregion

                if (validRow)
                {
                    this.isValidCargo = true;

                    if (!this.newRegCargo)
                        this.UpdateTemp(this.data);
                }
                else
                    this.isValidCargo = false;
            }
            catch (Exception ex)
            {
                validRow = false;
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentoProvForm.cs", "ValidateCargoRow"));
            }

            //this.hasChanges = true;
            return validRow;
        }        
            
        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        private void AddNewCargoRow() 
        {
            DTO_prSolicitudCargos footerCargos = new DTO_prSolicitudCargos();

            #region Asigna datos a la fila
            try
            {
                footerCargos.Index = this.data.Footer[this.indexFila].SolicitudCargos.Last().Index + 1;
                footerCargos.IndexDet = this.indexFila;
                footerCargos.ProyectoID.Value = this.data.Footer[this.indexFila].SolicitudCargos.Last().ProyectoID.Value;
                footerCargos.CentroCostoID.Value = this.data.Footer[this.indexFila].SolicitudCargos.Last().CentroCostoID.Value;
                footerCargos.PorcentajeID.Value = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            #endregion

            this.data.Footer[this.indexFila].SolicitudCargos.Add(footerCargos);
            this.gvCargos.RefreshData();
            this.gvCargos.FocusedRowHandle = this.gvCargos.DataRowCount - 1;

            this.isValidCargo = false;
        }

        /// <summary>
        /// Evento que obliga que se ejecute una funcion al cambiar de fila
        /// <param name="fila">Fila que se debe actualizar</param>
        /// <param name="oper">Indica si se debe ejecutar una segunda operacion</param>
        /// </summary>
        private void RowIndexCargosChanged(int fila, bool oper)
        {
            try
            {
                this.newRegCargo = false;
                int cFila = fila;
                GridColumn col = this.gvCargos.Columns[this.unboundPrefixCargo + "Index"];
                this.indexFilaCargos = Convert.ToInt16(this.gvCargos.GetRowCellValue(cFila, col));

                //this.LoadEditGridData(false, cFila);
                //this.LoadCargosGrid(this.indexFila);
                this.isValidCargo = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentoProvForm.cs", "RowIndexCargosChanged"));
            }
        }

        /// <summary>
        /// Muestra el formulario modal para MAsterComplex
        /// </summary>
        /// <param name="row">Fila donde esta la FK</param>
        /// <param name="col">Columna seleccionada</param>
        /// <param name="be">Boton que ejecuta la accion</param>
        private void ShowFKComplex(int row, string col, ButtonEdit be)
        {
            this.IsModalFormOpened = true;
            try
            {
                #region Tabla que corresponde a FK
                DTO_aplMaestraPropiedades propsFK = _bc.GetMasterPropertyByColId(col);
                int docFK = propsFK.DocumentoID;
                string modEmpGrupoFK = _bc.GetMaestraEmpresaGrupoByDocumentID(Convert.ToInt32(docFK));
                Tuple<int, string> tupFK = new Tuple<int, string>(Convert.ToInt32(docFK), modEmpGrupoFK);
                DTO_glTabla fktableFK = _bc.AdministrationModel.Tables[tupFK];
                bool jerarquicaFKInd = fktableFK.Jerarquica.Value.Value;
                #endregion

                string countMethod = "MasterComplex_Count";
                string dataMethod = "MasterComplex_GetPaged";

                string modFrmCode = (col.Contains("1")) ? AppMasters.inTipoParameter1.ToString() : AppMasters.inTipoParameter2.ToString();
                string modEmpGrupo = _bc.GetMaestraEmpresaGrupoByDocumentID(Convert.ToInt32(modFrmCode));

                //Tuple<int, string> tup = new Tuple<int, string>(Convert.ToInt32(modFrmCode), modEmpGrupo);
                //DTO_glTabla fktable = _bc.AdministrationModel.Tables[tup];

                ModalComplex modal = new ModalComplex(be, modFrmCode, countMethod, dataMethod, null, col, string.Empty, docFK, jerarquicaFKInd, this._filtrosRef);
                modal.ShowDialog();
            }
            finally
            {
                this.IsModalFormOpened = false;
            }
        }

        /// <summary>
        /// Valida un DTO de comprobante footer en la importacion
        /// </summary>
        /// <param name="ctrl">glDocumentoControl a validar</param>
        /// <param name="suppl">DTO Sumplementario a validar</param>
        /// <param name="rd">Variable que va guardando los mensajes de error</param>
        /// <param name="msgInvalidDate">Fecha en periodo invalido</param>
        /// <param name="msgFkNotFound">FK inexistente</param>
        /// <param name="msgFkHierarchyFather">No es una hoja de la jerarquia</param>
        /// <param name="msgCero">Mensaje que no permite ceros en un campo</param>
        /// <param name="msgPositive">Solo permite valores positivos</param>
        private void ValidateDataImport(object dto, DTO_glBienServicioClase bsClase, DTO_inRefTipo refTipo, DTO_TxResultDetail rd, string msgFkNotFound, string msgInvalidField)
        {
            string colRsx;
            DTO_TxResultDetailFields rdF;
            Type dataType = dto.GetType();

            bool createDTO = true;

            if (dataType == typeof(DTO_prDetalleDocu))
            {
                DTO_prDetalleDocu dtoDet = (DTO_prDetalleDocu)dto;
                #region Variables y diccionarios
                TipoCodigo tipoCodigo = (TipoCodigo)Enum.Parse(typeof(TipoCodigo), bsClase.TipoCodigo.Value.Value.ToString());

                Dictionary<string, string> pksPar1 = new Dictionary<string, string>();
                Dictionary<string, string> pksPar2 = new Dictionary<string, string>();

                if (refTipo!= null &&refTipo.Parametro1Ind.Value!=null && refTipo.Parametro1Ind.Value.Value)
                {
                    pksPar1.Add("TipoInvID", refTipo.ID.Value);
                    pksPar1.Add("Parametro1ID", dtoDet.Parametro1.Value);
                }

                if (refTipo != null && refTipo.Parametro2Ind.Value != null && refTipo.Parametro2Ind.Value.Value)
                {
                    pksPar2.Add("TipoInvID", refTipo.ID.Value);
                    pksPar2.Add("Parametro2ID", dtoDet.Parametro2.Value);
                }
                #endregion
                #region Valida las FKs
                #region CodigoBSID
                colRsx = this._codigoRsx;
                rdF = _bc.ValidGridCell(colRsx, dtoDet.CodigoBSID.Value, false, true, false, AppMasters.prBienServicio);
                if (rdF != null)
                {
                    rdF.Message = string.Format(msgFkNotFound, colRsx);
                    rd.DetailsFields.Add(rdF);

                    createDTO = false;
                }
                #endregion
                #region inReferenciaID
                colRsx = this._referenciaRsx;
                if (this._tipoCodigo == TipoCodigo.Servicio || this._tipoCodigo == TipoCodigo.Suministros || this._tipoCodigo == TipoCodigo.SuministroPersonal)
                {
                    if (!string.IsNullOrEmpty(dtoDet.inReferenciaID.Value.Trim()))
                    {
                        rdF = new DTO_TxResultDetailFields();
                        rdF.Field = colRsx;
                        rdF.Message = string.Format(msgInvalidField, colRsx);
                        rd.DetailsFields.Add(rdF);

                        createDTO = false;
                    }
                }
                else
                {
                    rdF = _bc.ValidGridCell(colRsx, dtoDet.inReferenciaID.Value, false, true, false, AppMasters.inReferencia);
                    if (rdF != null)
                    {
                        rdF.Message = string.Format(msgFkNotFound, colRsx);
                        rd.DetailsFields.Add(rdF);

                        createDTO = false;
                    }
                }
                #endregion
                #region Parametro1
                colRsx = this._parametro1Rsx;
                if (this._tipoCodigo == TipoCodigo.Servicio || this._tipoCodigo == TipoCodigo.Suministros || this._tipoCodigo == TipoCodigo.SuministroPersonal)
                {
                    if (!string.IsNullOrEmpty(dtoDet.Parametro1.Value.Trim()))
                    {
                        rdF = new DTO_TxResultDetailFields();
                        rdF.Field = colRsx;
                        rdF.Message = string.Format(msgInvalidField, colRsx);
                        rd.DetailsFields.Add(rdF);

                        createDTO = false;
                    }
                }
                //else if (refTipo != null && refTipo.Parametro1Ind.Value != null && !refTipo.Parametro1Ind.Value.Value)
                //{
                //    if (!string.IsNullOrEmpty(dtoDet.Parametro1.Value.Trim()))
                //    {
                //        {
                //            rdF = new DTO_TxResultDetailFields();
                //            rdF.Field = colRsx;
                //            rdF.Message = string.Format(msgInvalidField, colRsx);
                //            rd.DetailsFields.Add(rdF);

                //            createDTO = false;
                //        }
                //    }
                //}
                else if (this._tipoCodigo == TipoCodigo.Servicio || this._tipoCodigo == TipoCodigo.Suministros || this._tipoCodigo == TipoCodigo.SuministroPersonal)
                {
                    if (refTipo == null || refTipo.Parametro1Ind.Value == null || refTipo.Parametro1Ind.Value.Value)
                    {
                        rdF = _bc.ValidGridCellComplex(colRsx, dtoDet.Parametro1.Value, pksPar1, false, AppMasters.inTipoParameter1);
                        if (rdF != null)
                        {
                            rdF.Message = string.Format(msgFkNotFound, colRsx);
                            rd.DetailsFields.Add(rdF);

                            createDTO = false;
                        }
                    }
                }
                #endregion
                #region Parametro2
                colRsx = this._parametro2Rsx;
                if (this._tipoCodigo == TipoCodigo.Servicio || this._tipoCodigo == TipoCodigo.Suministros || this._tipoCodigo == TipoCodigo.SuministroPersonal)
                {
                    if (!string.IsNullOrEmpty(dtoDet.Parametro2.Value.Trim()))
                    {
                        rdF = new DTO_TxResultDetailFields();
                        rdF.Field = colRsx;
                        rdF.Message = string.Format(msgInvalidField, colRsx);
                        rd.DetailsFields.Add(rdF);

                        createDTO = false;
                    }
                }
                //else if (refTipo != null && refTipo.Parametro2Ind.Value != null && !refTipo.Parametro2Ind.Value.Value)
                //{
                //    if (!string.IsNullOrEmpty(dtoDet.Parametro2.Value.Trim()))
                //    {
                //        {
                //            rdF = new DTO_TxResultDetailFields();
                //            rdF.Field = colRsx;
                //            rdF.Message = string.Format(msgInvalidField, colRsx);
                //            rd.DetailsFields.Add(rdF);

                //            createDTO = false;
                //        }
                //    }
                //}
                else if (this._tipoCodigo == TipoCodigo.Servicio || this._tipoCodigo == TipoCodigo.Suministros || this._tipoCodigo == TipoCodigo.SuministroPersonal)
                {
                    if (refTipo == null || refTipo.Parametro2Ind.Value == null || refTipo.Parametro2Ind.Value.Value)
                    {
                        rdF = _bc.ValidGridCellComplex(colRsx, dtoDet.Parametro2.Value, pksPar2, false, AppMasters.inTipoParameter2);
                        if (rdF != null)
                        {
                            rdF.Message = string.Format(msgFkNotFound, colRsx);
                            rd.DetailsFields.Add(rdF);

                            createDTO = false;
                        }
                    }
                }
                #endregion
                #region UnidadInvID
                colRsx = this._unidadRsx;
                rdF = _bc.ValidGridCell(colRsx, dtoDet.UnidadInvID.Value, false, true, false, AppMasters.inUnidad);
                if (rdF != null)
                {
                    rdF.Message = string.Format(msgFkNotFound, colRsx);
                    rd.DetailsFields.Add(rdF);

                    createDTO = false;
                }
                #endregion
                #endregion
                #region Validacion de cantidad
                colRsx = this._cantidadRsx;
                rdF = _bc.ValidGridCellValue(colRsx, dtoDet.CantidadSol.Value.Value.ToString(), false, false, true, false);
                if (rdF != null)
                {
                    rdF.Message = string.Format(msgInvalidField, colRsx);
                    rd.DetailsFields.Add(rdF);

                    createDTO = false;
                }
                #endregion
            }

            if (dataType == typeof(DTO_prSolicitudCargos))
            {
                DTO_prSolicitudCargos dtoCarg = (DTO_prSolicitudCargos)dto;
                #region Valida las FKs
                #region ProyectoID
                colRsx = this._proyectoRsx;
                rdF = _bc.ValidGridCell(colRsx, dtoCarg.ProyectoID.Value, false, true, false, AppMasters.coProyecto);
                if (rdF != null)
                {
                    rdF.Message = string.Format(msgFkNotFound, colRsx);
                    rd.DetailsFields.Add(rdF);

                    createDTO = false;
                }
                #endregion
                #region CentroCostoID
                colRsx = this._centroCostoRsx;
                rdF = _bc.ValidGridCell(colRsx, dtoCarg.CentroCostoID.Value, false, true, true, AppMasters.coCentroCosto);
                if (rdF != null)
                {
                    rdF.Message = string.Format(msgFkNotFound, colRsx);
                    rd.DetailsFields.Add(rdF);

                    createDTO = false;
                }
                #endregion
                #endregion
                #region Validaciones de porcentaje
                colRsx = this._porcentajeRsx;
                rdF = _bc.ValidGridCellValue(colRsx, dtoCarg.PorcentajeID.Value.Value.ToString(), false, false, true, false);
                if (rdF != null)
                {
                    rdF.Message = string.Format(msgInvalidField, colRsx);
                    rd.DetailsFields.Add(rdF);

                    createDTO = false;
                }
                #endregion
            }
        }
        #endregion

        #region Funciones Abstractas (Implementacion DocumentForm)

        /// <summary>
        /// Trae el documento id de una maestra de acuerdo al nombre de un campo
        /// </summary>
        /// <param name="colName">Nombre del campo</param>
        /// <returns>Retorna el documento id de una maestra</returns>
        protected override int GetMasterDocumentID(string colName)
        {
            //Cuenta
            if (colName == this._codigoRsx)
                return AppMasters.prBienServicio;
            //Tercero
            if (colName == this._referenciaRsx)
                return AppMasters.inReferencia;
            //Prefijo
            if (colName == this._parametro1Rsx)
                return AppMasters.inRefParametro1;
            //Prefijo
            if (colName == this._parametro2Rsx)
                return AppMasters.inRefParametro2;
            //Linea presupuestal
            if (colName == this._unidadRsx)
                return AppMasters.inUnidad;
            //Proyecto
            if (colName == this._proyectoRsx)
                return AppMasters.coProyecto;
            //Cwentro Costo
            if (colName == this._centroCostoRsx)
                return AppMasters.coCentroCosto;

            return 0;   
        }

        /// <summary>
        /// Carga la información de las grilla
        /// </summary>
        /// <param name="firstTime">Si es la primera vez que carga la grilla</param>
        /// <param name="refresh">Si debe volver a cargar la data del servidor</param>
        protected override void LoadData(bool firstTime)
        {
            this.gcDocument.DataSource = this.data.Footer;
            this.gcDocument.RefreshDataSource();
            bool hasItems = this.data.Footer.GetEnumerator().MoveNext() ? true : false;
            if (hasItems)
            {
                this.gvDocument.MoveFirst();
                this.gcCargos.DataSource = this.data.Footer[this.gvDocument.FocusedRowHandle].SolicitudCargos;
                this.gcCargos.RefreshDataSource();
                bool hasItemsCargos = this.data.Footer[this.gvDocument.FocusedRowHandle].SolicitudCargos.GetEnumerator().MoveNext() ? true : false;
                    if (hasItems)
                        this.gvCargos.MoveFirst();
            }

            this.dataLoaded = true;
            //this.CalcularTotal();
        }

        /// <summary>
        /// Evento que obliga que se ejecute una funcion al cambiar de fila
        /// <param name="fila">Fila que se debe actualizar</param>
        /// <param name="oper">Indica si se debe ejecutar una segunda operacion</param>
        /// </summary>
        protected override void RowIndexChanged(int fila, bool oper)
        {
            try
            {
                if (fila >= 0)
                {
                    this.newReg = false;
                    int cFila = fila;
                    GridColumn col = this.gvDocument.Columns[this.unboundPrefix + "Index"];
                    this.indexFila = Convert.ToInt16(this.gvDocument.GetRowCellValue(cFila, col));
                    this.indexFila = this.gvDocument.FocusedRowHandle;// Debe ser igual que el anterior index

                    this.LoadEditGridData(false, cFila);

                    this.indexFilaCargos = 0;

                    if (this.data.Footer.Count > 0)
                    {
                        this.gcCargos.DataSource = this.data.Footer[this.indexFila].SolicitudCargos;
                        this.gcCargos.RefreshDataSource();
                        this.gvCargos.RefreshData();
                        this.gvCargos.FocusedRowHandle = 0;
                    }
                    else
                    {
                        this.gcCargos.DataSource = null;
                        this.gcCargos.RefreshDataSource();
                    }

                    this.isValid = true; 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentoProvForm.cs", "gvDocument_FocusedRowChanged"));
            }
        }

        #endregion 

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            base.SetInitParameters();
            this.afterImportDelegate = new AfterImport(this.AfterImportMethod);

            //Carga info de las monedas
            this.monedaLocal = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
            this.monedaExtranjera = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
            //Carga los valores por defecto
            this.defPrefijo = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
            this.defProyecto = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
            this.defCentroCosto = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
            this.defAreaFunc = _bc.AdministrationModel.User.AreaFuncionalID.Value;
            this.codigoBSInventarios = _bc.GetControlValueByCompanyAllowEmpty(ModulesPrefix.pr, AppControl.pr_CodigoBSCompraInv);

            //Carga los recursos de las Fks
            this._codigoRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CodigoBSID");
            this._referenciaRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_inReferenciaID");
            this._parametro1Rsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Parametro1");
            this._parametro2Rsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Parametro2");
            this._unidadRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_UnidadInvID");
            this._proyectoRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProyectoID");
            this._centroCostoRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CentroCostoID");

            //Carga los recursos de los valores            
            this._cantidadRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CantidadSol");
            this._porcentajeRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PorcentajeID");
            this._descRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descriptivo");
            this._solCargosRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_SolicitudCargos");

            this.gcDocument.ShowOnlyPredefinedDetails = true;         

            FormProvider.Master.itemPaste.Enabled = false;
            FormProvider.Master.itemImport.Enabled = false;

            //Controles del detalle
            _bc.InitMasterUC(this.masterCodigoBS, AppMasters.prBienServicio, true, true, true, false);
            _bc.InitMasterUC(this.masterReferencia, AppMasters.inReferencia, true, true, true, false);

            this.format = string.Empty;
        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize()
        {
            base.AfterInitialize();

            this.AddGridCols();
            this.AddCargosGridCols();
            this.lastColName = this.unboundPrefix + "CentroCostoID";
            this.lastColNameCargos = this.unboundPrefixCargo + "PorcentajeID";
            this.EnableFooter(false);
            this.gcCargos.Enabled = false;

            #region Carga temporales
            if (this.HasTemporales())
            {
                string msgTitleLoadTemp = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_TempLoad);
                string msgLoadTemp = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Temp_LoadData);
                try
                {
                    //Revisa si desea cargar los temporales
                    if (MessageBox.Show(msgLoadTemp, msgTitleLoadTemp, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        var det = _bc.AdministrationModel.aplTemporales_GetByOrigen(this.documentID.ToString(), _bc.AdministrationModel.User);
                        if (det != null)
                        {
                            try
                            {
                                this.LoadTempData((DTO_prSolicitud)det);
                            }
                            catch (Exception ex1)
                            {
                                this.validHeader = false;
                                MessageBox.Show(_bc.GetResourceError(DictionaryMessages.Err_TempLoad));
                                _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);
                            }
                        }
                        else
                        {
                            this.validHeader = false;
                            MessageBox.Show(_bc.GetResourceError(DictionaryMessages.Err_TempLoad));
                            _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);
                        }
                    }
                    else
                    {
                        this.validHeader = false;
                        _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentoProvForm.cs", "AfterInitialize: " + ex.Message));
                }
            }
            #endregion

            #region Import Format
            this.format += (_bc.GetResource(LanguageTypes.Forms, "71_Index") + this.formatSeparator);
            foreach (GridColumn col in this.gvDocument.Columns)
            {
                if (!col.Caption.Contains("NumeroDoc") && !col.Caption.Contains("SolicitudDocuID")
                    && !col.Caption.Contains("ConsecutivoDetaID") && !col.Caption.Contains("SolicitudDetaID") && !string.IsNullOrEmpty(col.Caption.Trim()))
                {
                    this.format += (_bc.GetResource(LanguageTypes.Forms, col.Caption) + this.formatSeparator);
                }
            }
            //this.format += _bc.GetResource(LanguageTypes.Forms, "71_PorcentajeID");
            #endregion
        }

        /// <summary>
        /// Habilita o deshabilita la barra de herramientas segun donde el usuario este
        /// </summary>
        protected override void ValidHeaderTB()
        {
            if (this.validHeader)
            {
                FormProvider.Master.itemFilterDef.Enabled = true;
                FormProvider.Master.itemFilter.Enabled = true;

                //Habilita el boton de salvar
                if (SecurityManager.HasAccess(this.documentID, FormsActions.Add) || SecurityManager.HasAccess(this.documentID, FormsActions.Edit))
                    FormProvider.Master.itemSave.Enabled = true;
                else
                    FormProvider.Master.itemSave.Enabled = false;

                //FormProvider.Master.itemRevert.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Revert);
                FormProvider.Master.itemDelete.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Delete);
                FormProvider.Master.itemPrint.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Print);
                FormProvider.Master.itemExport.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Export);
                FormProvider.Master.itemCopy.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Copy);
                FormProvider.Master.itemPaste.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Paste);
                FormProvider.Master.itemImport.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Import);
                FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr);
            }
            else
            {
                FormProvider.Master.itemSave.Enabled = false;
                FormProvider.Master.itemRevert.Enabled = false;
                FormProvider.Master.itemDelete.Enabled = false;
                FormProvider.Master.itemFilterDef.Enabled = false;
                FormProvider.Master.itemFilter.Enabled = false;
                FormProvider.Master.itemPrint.Enabled = false;
                FormProvider.Master.itemExport.Enabled = false;
                FormProvider.Master.itemCopy.Enabled = false;
                FormProvider.Master.itemPaste.Enabled = false;
                FormProvider.Master.itemImport.Enabled = false;
                FormProvider.Master.itemSendtoAppr.Enabled = false;
            }

        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected override void AddGridCols()
        {
            try
            {
                this.editSpin.Mask.EditMask = "n1";
                #region Columnas visibles
                //CodigoServicios
                GridColumn codBS= new GridColumn();
                codBS.FieldName = this.unboundPrefix + "CodigoBSID";
                codBS.Caption = this._codigoRsx;
                codBS.UnboundType = UnboundColumnType.String;
                codBS.VisibleIndex = 1;
                codBS.Width = 70;
                codBS.Visible = true;
                codBS.Fixed = FixedStyle.Left;
                codBS.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(codBS);

                //CodigoReferencia
                GridColumn codRef = new GridColumn();
                codRef.FieldName = this.unboundPrefix + "inReferenciaID";
                codRef.Caption = this._referenciaRsx;
                codRef.UnboundType = UnboundColumnType.String;
                codRef.VisibleIndex = 2;
                codRef.Width = 70;
                codRef.Visible = true;
                codRef.Fixed = FixedStyle.Left;
                codRef.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(codRef);

                //Parameter1
                GridColumn param1 = new GridColumn();
                param1.FieldName = this.unboundPrefix + "Parametro1";
                param1.Caption = this._parametro1Rsx;
                param1.UnboundType = UnboundColumnType.String;
                param1.VisibleIndex = 3;
                param1.Width = 60;
                param1.Visible = false;
                param1.Fixed = FixedStyle.Left;
                param1.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(param1);

                //Parameter2
                GridColumn param2 = new GridColumn();
                param2.FieldName = this.unboundPrefix + "Parametro2";
                param2.Caption = this._parametro2Rsx;
                param2.UnboundType = UnboundColumnType.String;
                param2.VisibleIndex = 4;
                param2.Width = 60;
                param2.Visible = false;
                param2.Fixed = FixedStyle.Left;
                param2.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(param2);

                //Descriptivo
                GridColumn desc = new GridColumn();
                desc.FieldName = this.unboundPrefix + "Descriptivo";
                desc.Caption = this._descRsx;
                desc.UnboundType = UnboundColumnType.String;
                desc.VisibleIndex = 5;
                desc.Width = 150;
                desc.Visible = true;
                desc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(desc);

                //UnidadInvID
                GridColumn unidad = new GridColumn();
                unidad.FieldName = this.unboundPrefix + "UnidadInvID";
                unidad.Caption = this._unidadRsx;
                unidad.UnboundType = UnboundColumnType.String;
                unidad.VisibleIndex = 6;
                unidad.Width = 50;
                unidad.Visible = true;
                unidad.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(unidad);

                //Cantidad Solicitud
                GridColumn cant = new GridColumn();
                cant.FieldName = this.unboundPrefix + "CantidadSol";
                cant.Caption = this._cantidadRsx;
                cant.UnboundType = UnboundColumnType.Integer;
                cant.VisibleIndex = 7;
                cant.Width = 70;
                cant.Visible = true;
                cant.ColumnEdit = this.editSpin;
                cant.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(cant);
        
                //Proyecto
                GridColumn proyecto = new GridColumn();
                proyecto.FieldName = this.unboundPrefix + "ProyectoID";
                proyecto.Caption = this._proyectoRsx;
                proyecto.UnboundType = UnboundColumnType.String;
                proyecto.VisibleIndex = 8;
                proyecto.Width = 100;
                proyecto.Visible = true;
                proyecto.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(proyecto);

                //Centro de costo
                GridColumn ctoCosto = new GridColumn();
                ctoCosto.FieldName = this.unboundPrefix + "CentroCostoID";
                ctoCosto.Caption = this._centroCostoRsx;
                ctoCosto.UnboundType = UnboundColumnType.String;
                ctoCosto.VisibleIndex = 9;
                ctoCosto.Width = 100;
                ctoCosto.Visible = true;
                ctoCosto.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(ctoCosto);

                //MarcaInvID
                GridColumn MarcaInvID = new GridColumn();
                MarcaInvID.FieldName = this.unboundPrefix + "MarcaInvID";
                MarcaInvID.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_MarcaInvID"); ;
                MarcaInvID.UnboundType = UnboundColumnType.String;
                MarcaInvID.VisibleIndex = 10;
                MarcaInvID.Width = 90;
                MarcaInvID.Visible = true;
                MarcaInvID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(MarcaInvID);

                //RefProveedor
                GridColumn RefProveedor = new GridColumn();
                RefProveedor.FieldName = this.unboundPrefix + "RefProveedor";
                RefProveedor.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_RefProveedor");
                RefProveedor.UnboundType = UnboundColumnType.String;
                RefProveedor.VisibleIndex = 11;
                RefProveedor.Width = 100;
                RefProveedor.Visible = true;
                RefProveedor.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(RefProveedor);

                //TareaID
                GridColumn TareaID = new GridColumn();
                TareaID.FieldName = this.unboundPrefix + "TareaID";
                TareaID.Caption = "Tarea";
                TareaID.UnboundType = UnboundColumnType.String;
                TareaID.VisibleIndex = 12;
                TareaID.Width = 100;
                TareaID.Visible = true;
                TareaID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(TareaID);

                //CantidadTarea
                GridColumn cantidadTarea = new GridColumn();
                cantidadTarea.FieldName = this.unboundPrefix + "CantidadTarea";
                cantidadTarea.Caption = "Cantidad Tarea";
                cantidadTarea.AppearanceCell.BackColor = Color.Gray;
                cantidadTarea.AppearanceCell.Options.UseBackColor = true;
                cantidadTarea.UnboundType = UnboundColumnType.String;
                cantidadTarea.VisibleIndex = 13;
                cantidadTarea.Width = 70;
                cantidadTarea.Visible = false;
                cantidadTarea.ColumnEdit = this.editSpin;
                cantidadTarea.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(cantidadTarea);

                #endregion
                #region Columnas No Visibles

                //NumeroDoc
                GridColumn numDoc = new GridColumn();
                numDoc.FieldName = this.unboundPrefix + "NumeroDoc";
                numDoc.UnboundType = UnboundColumnType.Integer;
                numDoc.Visible = false;
                this.gvDocument.Columns.Add(numDoc);

                //SolicitudDocuID
                GridColumn solDocu = new GridColumn();
                solDocu.FieldName = this.unboundPrefix + "SolicitudDocuID";
                solDocu.UnboundType = UnboundColumnType.Integer;
                solDocu.Visible = false;
                this.gvDocument.Columns.Add(solDocu);

                //ConsecutivoDetaID
                GridColumn consDeta = new GridColumn();
                consDeta.FieldName = this.unboundPrefix + "ConsecutivoDetaID";
                consDeta.UnboundType = UnboundColumnType.Integer;
                consDeta.Visible = false;
                this.gvDocument.Columns.Add(consDeta);

                //SolicitudDetaID
                GridColumn solDeta = new GridColumn();
                solDeta.FieldName = this.unboundPrefix + "SolicitudDetaID";
                solDeta.UnboundType = UnboundColumnType.Integer;
                solDeta.Visible = false;
                this.gvDocument.Columns.Add(solDeta);


                //Indice de la fila
                GridColumn colIndex = new GridColumn();
                colIndex.FieldName = this.unboundPrefix + "Index";
                colIndex.UnboundType = UnboundColumnType.Integer;
                colIndex.Visible = false;
                this.gvDocument.Columns.Add(colIndex);
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentoProvForm.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Extrae datos de la grilla para las cajas de texto
        /// </summary>
        /// <param name="isNew">Identifica si es un nuevo registro</param>
        /// <param name="rowIndex">Numero de la fila</param>
        protected override void LoadEditGridData(bool isNew, int rowIndex) 
        {
            if (!this.disableValidate)
            {
                try
                {
                    string val_CodigoBS = (isNew || gvDocument.Columns[this.unboundPrefix + "CodigoBSID"] == null || gvDocument.RowCount == 0) ? string.Empty : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "CodigoBSID"]).ToString();
                    string val_Referencia = (isNew || gvDocument.Columns[this.unboundPrefix + "inReferenciaID"] == null || gvDocument.RowCount == 0) ? string.Empty : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "inReferenciaID"]).ToString();
                    string val_Desc = (isNew || gvDocument.Columns[this.unboundPrefix + "Descriptivo"] == null || gvDocument.RowCount == 0) ? string.Empty : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "Descriptivo"]).ToString();

                    this.masterCodigoBS.Value = val_CodigoBS;
                    this.masterReferencia.Value = val_Referencia;
                    this.txtDesc.Text = val_Desc;

                    if (this.newDoc)
                    {
                        this.EnableFooter(false);
                        this.newDoc = false;
                    }
                    else
                    {
                        if (this.BienServicio != null && !this.BienServicio.ID.Value.Equals(val_CodigoBS))
                            this.BienServicio = (DTO_prBienServicio)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.prBienServicio, false, val_CodigoBS, true);
                        if (this.Referencia != null && !this.Referencia.ID.Value.Equals(val_Referencia))
                            this.Referencia = (DTO_inReferencia)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, false, val_Referencia, true);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentoProvForm.cs", "LoadEditGridData"));
                }
            }
        }

        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        protected override void AddNewRow()
        {
            DTO_prSolicitudFooter footerDet = new DTO_prSolicitudFooter();
            DTO_prSolicitudCargos footerCargos = new DTO_prSolicitudCargos();
            this.indexFilaCargos = 0;

            #region Asigna datos a la fila
            try
            {
                if (this.data.Footer.Count > 0)
                {
                    footerDet.DetalleDocu.Index = this.data.Footer.Last().DetalleDocu.Index + 1;
                    footerDet.DetalleDocu.CodigoBSID.Value = this.data.Footer.Last().DetalleDocu.CodigoBSID.Value;
                    //footerDet.DetalleDocu.inReferenciaID.Value = this.data.Footer.Last().DetalleDocu.inReferenciaID.Value;
                    footerDet.DetalleDocu.Parametro1.Value = this.data.Footer.Last().DetalleDocu.Parametro1.Value;
                    footerDet.DetalleDocu.Parametro2.Value = this.data.Footer.Last().DetalleDocu.Parametro2.Value;
                    footerDet.DetalleDocu.Descriptivo.Value = this.data.Footer.Last().DetalleDocu.Descriptivo.Value;
                    footerDet.DetalleDocu.UnidadInvID.Value = this.data.Footer.Last().DetalleDocu.UnidadInvID.Value;
                    footerDet.DetalleDocu.ValorTotML.Value = 0;
                    footerDet.DetalleDocu.ValorTotME.Value =0;
                    footerDet.DetalleDocu.IvaTotML.Value = 0;
                    footerDet.DetalleDocu.IvaTotME.Value =0;
                    footerDet.DetalleDocu.ValorUni.Value = 0;
                    footerDet.DetalleDocu.IVAUni.Value = 0;
                    footerDet.ProyectoID = this.data.Footer.Last().ProyectoID;
                    footerDet.CentroCostoID = this.data.Footer.Last().CentroCostoID;
                    footerCargos.IndexDet = footerDet.DetalleDocu.Index;
                    footerCargos.Index = 0;
                    footerCargos.ProyectoID.Value = footerDet.ProyectoID;
                    footerCargos.CentroCostoID.Value = footerDet.CentroCostoID;
                    footerCargos.PorcentajeID.Value = 100;
                    footerDet.SolicitudCargos.Add(footerCargos);
                }
                else
                {
                    footerDet.DetalleDocu.Index = 0;
                    footerDet.DetalleDocu.CodigoBSID.Value = string.Empty;
                    footerDet.DetalleDocu.inReferenciaID.Value = string.Empty;
                    footerDet.DetalleDocu.Parametro1.Value = string.Empty;
                    footerDet.DetalleDocu.Parametro2.Value = string.Empty;
                    footerDet.DetalleDocu.Descriptivo.Value = string.Empty;
                    footerDet.DetalleDocu.UnidadInvID.Value = string.Empty;
                    footerDet.DetalleDocu.ValorTotML.Value = 0;
                    footerDet.DetalleDocu.ValorTotME.Value = 0;
                    footerDet.DetalleDocu.IvaTotML.Value = 0;
                    footerDet.DetalleDocu.IvaTotME.Value = 0;
                    footerDet.DetalleDocu.ValorUni.Value = 0;
                    footerDet.DetalleDocu.IVAUni.Value = 0;
                    footerDet.ProyectoID = string.Empty;
                    footerDet.CentroCostoID = string.Empty;
                    footerCargos.IndexDet = footerDet.DetalleDocu.Index;
                    footerCargos.Index = 0;
                    footerCargos.ProyectoID.Value = string.Empty;
                    footerCargos.CentroCostoID.Value = string.Empty;
                    footerCargos.PorcentajeID.Value = 0;
                    footerDet.SolicitudCargos.Add(footerCargos);
                }
                footerDet.DetalleDocu.CantidadSol.Value = 0;
                footerDet.DetalleDocu.CantidadDoc5.Value = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            #endregion

            this.data.Footer.Add(footerDet);
            this.gvDocument.RefreshData();
            this.gvDocument.FocusedRowHandle = this.gvDocument.DataRowCount - 1;

            this.gcCargos.DataSource = this.data.Footer[this.gvDocument.FocusedRowHandle].SolicitudCargos;
            this.gcCargos.RefreshDataSource();
            this.gvCargos.RefreshData();
            this.gvCargos.FocusedRowHandle = 0;

            this.gcCargos.Enabled = false;     

            this.isValid = false;
            this.EnableFooter(false);
            this.masterCodigoBS.EnableControl(true);
            this.btnCodigoBS.Enabled = true;
            if (this.masterCodigoBS.ValidID)
                this.BienServicio = (DTO_prBienServicio)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.prBienServicio, false, this.masterCodigoBS.Value, true);
            else
                this._bienServicio = new DTO_prBienServicio();
            if (this.masterReferencia.ValidID)
                this.Referencia = (DTO_inReferencia)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, false, this.masterReferencia.Value, true);
            else
                this._referencia = new DTO_inReferencia();
            this.masterCodigoBS.Focus();
        }

        /// <summary>
        /// Valida una fila
        /// </summary>
        /// <param name="fila">Indice de la fila a validar</param>
        /// <returns>Retorna si la info de la fila es valida o no</returns>
        protected override bool ValidateRow(int fila)
        {
            bool validRow = true;
            bool validField = true;

            try
            {
                GridColumn col = new GridColumn();

                #region Validacion de nulls y Fks
                string rsxEmpty = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                #region CodigoBSID
                validField = _bc.ValidGridCell(this.gvDocument, string.Empty, fila, "CodigoBSID", false, true,true, AppMasters.prBienServicio);
                if (!validField)
                    validRow = false;
                #endregion
                #region inReferenciaID
                validField = _bc.ValidGridCell(this.gvDocument, string.Empty, fila, "inReferenciaID", (this._tipoCodigo == TipoCodigo.Servicio || this._tipoCodigo == TipoCodigo.Suministros || this._tipoCodigo == TipoCodigo.SuministroPersonal) ? true : false, true, false, AppMasters.inReferencia); // prBienServicioClase.TipoCodigo
                if (!validField)
                    validRow = false;
                #endregion
                #region Parametro1
                if (this._tipoCodigo == TipoCodigo.Inventario)
                {
                    if (this.TipoRef == null || this.TipoRef.Parametro1Ind.Value == null || this.TipoRef.Parametro1Ind.Value.Value)
                    {
                        if (this._pksPar1.Count > 0)
                            validField = _bc.ValidGridCellComplex(this.gvDocument, string.Empty, fila, "Parametro1", this._pksPar1, false, AppMasters.inTipoParameter1);
                        if (!validField)
                            validRow = false;
                    }
                }
                #endregion
                #region Parametro2
                if (this._tipoCodigo == TipoCodigo.Inventario)
                {
                    if (this.TipoRef == null || this.TipoRef.Parametro2Ind.Value == null || this.TipoRef.Parametro2Ind.Value.Value)
                    {
                        if (this._pksPar2.Count > 0)
                        validField = _bc.ValidGridCellComplex(this.gvDocument, string.Empty, fila, "Parametro2", this._pksPar2, false, AppMasters.inTipoParameter2);
                        if (!validField)
                            validRow = false;
                    }
                }
                #endregion
                #region Descriptivo
                validField = _bc.ValidGridCell(this.gvDocument, string.Empty, fila, "Descriptivo", false, false, false, null);
                if (!validField)
                    validRow = false;
                #endregion
                #region UnidadInvID
                validField = _bc.ValidGridCell(this.gvDocument, string.Empty, fila, "UnidadInvID", false, true, false, AppMasters.inUnidad);
                if (!validField)
                    validRow = false;
                #endregion
                #region Proyecto
                validField = _bc.ValidGridCell(this.gvDocument, string.Empty, fila, "ProyectoID", false, true, true, AppMasters.coProyecto);
                if (!validField)
                    validRow = false;
                #endregion
                #region Centro Costo
                validField = _bc.ValidGridCell(this.gvDocument, string.Empty, fila, "CentroCostoID", false, true, true, AppMasters.coCentroCosto);
                if (!validField)
                    validRow = false;
                #endregion
                #endregion
                #region Validaciones de Cantidad
                #region CantidadSol
                validField = _bc.ValidGridCellValue(this.gvDocument, string.Empty, fila, "CantidadSol", false, false, true, false);
                #region Valida si existen items de solicitudes de Proyectos
                if (validField && this._listSolicitudProyectos != null && this._listSolicitudProyectos.Count > 0)
                {
                    //Revisa si existe el item digitado
                    bool exist = this._listSolicitudProyectos.Exists(x => x.CodigoBSID.Value == this.data.Footer[fila].DetalleDocu.CodigoBSID.Value &&
                                                                        x.inReferenciaID.Value == this.data.Footer[fila].DetalleDocu.inReferenciaID.Value);
                    if (!exist)
                    {
                        col = this.gvDocument.Columns[this.unboundPrefix + "CodigoBSID"];
                        this.gvDocument.SetColumnError(col,this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Pr_CodigoBSProyectoNotExist));
                        validField = false;
                    }
                    else
                    {
                        //Valida la cantidad disponible y la solicitada
                        DTO_prDetalleDocu detProyecto = this._listSolicitudProyectos.Where(x => x.CodigoBSID.Value == this.data.Footer[fila].DetalleDocu.CodigoBSID.Value &&
                                                                                            x.inReferenciaID.Value == this.data.Footer[fila].DetalleDocu.inReferenciaID.Value).First();
                        decimal cantidadSolicita = this.data.Footer.Where(x => x.DetalleDocu.CodigoBSID.Value == this.data.Footer[fila].DetalleDocu.CodigoBSID.Value &&
                                                                            x.DetalleDocu.inReferenciaID.Value == this.data.Footer[fila].DetalleDocu.inReferenciaID.Value).First().DetalleDocu.CantidadSol.Value.Value;

                        if (detProyecto.CantidadDoc2.Value < cantidadSolicita)
                        {
                            col = this.gvDocument.Columns[this.unboundPrefix + "CantidadSol"];
                            this.gvDocument.SetColumnError(col, this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Pr_CantidadSolNotAvailable));
                            validField = false;
                        }
                        else
                            this.data.Footer[fila].DetalleDocu.LineaPresupuestoID.Value = detProyecto.LineaPresupuestoID.Value;
                    }
                }
                #endregion
                if (!validField)
                    validRow = false;                
                #endregion
                #endregion

                if (validRow)
                {
                    this.isValid = true;
                    //this.CalcularTotal();

                    if (!this.newReg)
                        this.UpdateTemp(this.data);
                }
                else
                    this.isValid = false;
            }
            catch (Exception ex)
            {
                validRow = false;
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentoProvForm.cs", "ValidateRow"));
            }

            this.hasChanges = true;
            return validRow;
        }

        /// <summary>
        /// Revisa si una grilla es valida o no
        /// </summary>
        /// <returns></returns>
        protected virtual bool ValidGrid()
        {
            if (this.data != null && this.data.Footer != null && this.data.Footer.Count == 0)
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoData));
                return false;
            }
            else if (this.data.DocCtrl.Estado.Value == (byte)EstadoDocControl.Aprobado)
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_DocumentEstateAprob));
                return false;
            }

            if (this.data == null || !this.ValidateRow(this.gvDocument.FocusedRowHandle))
                return false;

            return true;
        }

        /// <summary>
        /// Habilita o deshabilita los controles del footer
        /// </summary>
        protected virtual void EnableFooter(bool enable)
        {
            if (this.gvDocument.DataRowCount == 0)
                enable = false;
            this.masterCodigoBS.EnableControl(enable);
            this.btnCodigoBS.Enabled = enable;
            this.masterReferencia.EnableControl(enable);
            this.btnReferencia.Enabled = enable;
            this.txtDesc.Enabled = enable;
        }

        /// <summary>
        /// Limpia y deja vacio los controles del header
        /// </summary>
        /// <param name="basic">Indica si tambien se limpian los controles basicos</param>
        protected virtual void CleanHeader(bool basic)
        {
            this.dtFecha.DateTime = this.dtPeriod.DateTime;

            this.validHeader = false;
            this.ValidHeaderTB();

            //this.CalcularTotal();
        }

        /// <summary>
        /// Limpia y deja vacio los controles del footer
        /// </summary>
        protected void CleanFooter()
        {
            this.masterCodigoBS.Value = string.Empty;
            this.masterReferencia.Value = string.Empty;
            this.txtDesc.Text = string.Empty;
        }

        /// <summary>
        /// Oculta los controles del formulario
        /// </summary>
        protected virtual void EnableHeader(bool enable) { }

        /// <summary>
        /// Carga informacion de un temporal a partir del cabezote
        /// </summary>
        /// <returns>Retorna el header de un temporal</returns>
        protected virtual DTO_prSolicitud LoadTempHeader() { return null; }

        /// <summary>
        /// valida la informacion del header
        /// </summary>
        protected virtual bool ValidateHeader() { return true; }

        /// <summary>
        /// Carga la informacion temporal del documento
        /// </summary>
        /// <param name="aux">Informacion del temporal</param>
        protected virtual void LoadTempData(DTO_prSolicitud sol) { }

        /// <summary>
        /// Calcula valores globales del documento
        /// </summary>
        /// <param name="index">identificador de fila actual de la grilla</param>
        protected virtual void GetValuesDocument(int index) { }

        /// <summary>
        /// Carga el valor de la tasa de cambio de acuerdo a los items seleccionados
        /// </summary>
        /// <param name="monOr">Moneda Origen</param>
        /// <returns>Retorna el valor de la tasa de cambio</returns>
        protected virtual decimal LoadTasaCambio(DateTime fechaSolititud)
        {
            try
            {
                decimal valor = _bc.AdministrationModel.TasaDeCambio_Get(this.monedaExtranjera, fechaSolititud);
                return valor;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentoOrdenCompForm.cs", "LoadTasaCambio"));
                return 0;
            }
        }

        #endregion

        #region Eventos del MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void Form_Enter(object sender, EventArgs e)
        {
            base.Form_Enter(sender, e);

            FormProvider.Master.tbBreak0.Visible = false;
            FormProvider.Master.tbBreak1.Visible = false;
            FormProvider.Master.itemRevert.Visible = false;
            FormProvider.Master.itemDelete.Visible = false;
        }

        /// <summary>
        /// Se ejecuta cuando la forma se cierra
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.HasTemporales())
            {
                string msgTitleWarning = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
                string msgLostInfo = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.LostInfo);

                if (MessageBox.Show(msgLostInfo, msgTitleWarning, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);
                    base.Form_FormClosing(sender, e);
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        #endregion

        #region Eventos Grilla

        /// <summary>
        /// Revisa botones al digitar algo sobre la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.lastColName) && this.gvDocument.IsLastRow && this.gvDocument.FocusedColumn.FieldName == this.lastColName && e.KeyCode == Keys.Tab)
            {
                bool isV = this.ValidateRow(this.gvDocument.FocusedRowHandle);
                if (isV)
                {
                    this.newReg = true;
                    //this.AddNewRow();
                }
            }
        }

        /// <summary>
        /// Evento que se ejecuta al presionar un boton por defecto de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gcDocument_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            if (this.validHeader)
            {
                if (this.data == null)
                {
                    this.gcDocument.Focus();
                    e.Handled = true;
                    return;
                }

                this.gvDocument.PostEditor();
                if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Custom)
                {
                    if (this.gvDocument.ActiveFilterString != string.Empty)
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoAddInFilter));
                    else
                    {
                        this.deleteOP = false;
                        if (this.isValid)
                        {
                            if (this.isValidCargo)
                            {
                                this.newReg = true;
                                this.AddNewRow();
                            }
                            else 
                                e.Handled = true;
                        }
                        else
                        {
                            bool isV = this.ValidateRow(this.gvDocument.FocusedRowHandle);
                            if (isV)
                            {
                                if (this.isValidCargo)
                                {
                                    this.newReg = true;
                                    this.AddNewRow();
                                }
                                else
                                    e.Handled = true;
                            }
                        }
                    }
                }
                if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Remove)
                {
                    string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Delete);
                    string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Register);

                    //Revisa si desea cargar los temporales
                    if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        this.deleteOP = true;
                        int rowHandle = this.gvDocument.FocusedRowHandle;

                        if (this.data.Footer.Count == 1)
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.RowsNeeded));
                            e.Handled = true;
                        }
                        else
                        {
                            this.data.Footer.RemoveAll(x => x.DetalleDocu.Index == this.indexFila);
                            //Si borra el primer registro
                            if (rowHandle == 0)
                                this.gvDocument.FocusedRowHandle = 0;
                            //Si selecciona el ultimo
                            else
                                this.gvDocument.FocusedRowHandle = rowHandle - 1;

                            this.UpdateTemp(this.data);

                            this.gvDocument.RefreshData();
                            this.RowIndexChanged(this.gvDocument.FocusedRowHandle, true);
                        }
                    }
                    e.Handled = true;
                }
            }            
        }

        /// <summary>
        /// asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (fieldName == "CodigoBSID" || fieldName == "inReferenciaID" || fieldName == "Parametro1" || fieldName == "Parametro2"
                || fieldName == "UnidadInvID" || fieldName == "ProyectoID" || fieldName == "CentroCostoID" || fieldName == "TareaID")
                    e.RepositoryItem = this.editBtnGrid;
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            if (!this.disableValidate)
            {
                bool validRow = this.deleteOP ? true : this.ValidateRow(e.RowHandle);
                this.deleteOP = false;                

                if (validRow)
                {
                    this.isValid = true;
                    if (!this.isValidCargo)
                        e.Allow = false;
                }
                else
                {
                    e.Allow = false;
                    this.isValid = false;
                }
            }
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            if (this.documentID != AppDocuments.Solicitud && this.documentID != AppDocuments.SolicitudDirecta)
                base.gvDocument_CustomUnboundColumnData(sender, e);
            else
            {
                object dto = (object)e.Row;
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

                if (e.IsGetData)
                {
                    PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (pi != null)
                    {
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                            e.Value = pi.GetValue(dto, null);
                        else
                            e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                    }
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
                        else
                        {
                            DTO_prSolicitudFooter dtoDet = (DTO_prSolicitudFooter)e.Row;
                            pi = dtoDet.DetalleDocu.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                            if (pi != null)
                            {
                                if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                                    e.Value = pi.GetValue(dtoDet.DetalleDocu, null);
                                else
                                    e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dtoDet.DetalleDocu, null), null);
                            }
                            else
                            {
                                fi = dtoDet.DetalleDocu.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                                if (fi != null)
                                {
                                    if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double")
                                        e.Value = fi.GetValue(dtoDet.DetalleDocu);
                                    else
                                        e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dtoDet.DetalleDocu), null);
                                }
                            }
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
                            pi.SetValue(dto, e.Value ,null);
                            //e.Value = pi.GetValue(dto, null);
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
                                pi.SetValue(dto, e.Value, null);
                                //e.Value = pi.GetValue(dto, null);
                            }
                            else
                            {
                                UDT udtProp = (UDT)fi.GetValue(dto);
                                udtProp.SetValueFromString(e.Value.ToString());
                            }
                        }
                        else
                        {
                            DTO_prSolicitudFooter dtoDet = (DTO_prSolicitudFooter)e.Row;
                            pi = dtoDet.DetalleDocu.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                            if (pi != null)
                            {
                                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                                {
                                    e.Value = pi.GetValue(dtoDet.DetalleDocu, null);
                                }
                                else
                                {
                                    UDT udtProp = (UDT)pi.GetValue(dtoDet.DetalleDocu, null);
                                    udtProp.SetValueFromString(e.Value.ToString());
                                }
                            }
                            else
                            {
                                fi = dtoDet.DetalleDocu.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                                if (fi != null)
                                {
                                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                                    {
                                        pi.SetValue(dto, e.Value, null);
                                        //e.Value = pi.GetValue(dto, null);
                                    }
                                    else
                                    {
                                        UDT udtProp = (UDT)fi.GetValue(dtoDet.DetalleDocu);
                                        udtProp.SetValueFromString(e.Value.ToString());
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Evento que se ejecuta al cambiar el foco de una columna a otra
        /// Cambia el estilo de una celda segun las condiciones del formulario
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param> 
        protected override void gvDocument_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {            
            bool editableCell = true;
            string fieldName = this.gvDocument.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);

            #region Validacion de otros campos
            if ((fieldName == "inReferenciaID" || fieldName == "Parametro1" || fieldName == "Parametro2") && this._tipoCodigo == TipoCodigo.Servicio || this._tipoCodigo == TipoCodigo.Suministros || this._tipoCodigo == TipoCodigo.SuministroPersonal)
                editableCell = false;
            if (fieldName == "Parametro1" && (this._tipoCodigo == TipoCodigo.Servicio || this._tipoCodigo == TipoCodigo.Suministros || this._tipoCodigo == TipoCodigo.SuministroPersonal || this.TipoRef == null || this.TipoRef.Parametro1Ind.Value == null || !this.TipoRef.Parametro1Ind.Value.Value))
                editableCell = false;
            if (fieldName == "Parametro2" && (this._tipoCodigo == TipoCodigo.Servicio || this._tipoCodigo == TipoCodigo.Suministros || this._tipoCodigo == TipoCodigo.SuministroPersonal || this.TipoRef == null || this.TipoRef.Parametro2Ind.Value == null || !this.TipoRef.Parametro2Ind.Value.Value))
                editableCell = false;
            #endregion

            if (editableCell)
                this.gvDocument.Appearance.FocusedCell.BackColor = Color.White;
            else
                this.gvDocument.Appearance.FocusedCell.BackColor = Color.Lavender;
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);            
            int index = this.NumFila;
            bool validField = true;

            #region Se modifican FKs
            if (fieldName == "CodigoBSID")
                validField = _bc.ValidGridCell(this.gvDocument, string.Empty, e.RowHandle, fieldName, false, true,true, AppMasters.prBienServicio);
            if (fieldName == "inReferenciaID")
            {
                validField = _bc.ValidGridCell(this.gvDocument, string.Empty, e.RowHandle, fieldName, false, true, false, AppMasters.inReferencia);
                if (validField)
                {
                   // DTO_inReferencia refer = (DTO_inReferencia)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, false, e.Value.ToString(), true);
                    //this.data.Footer[index].DetalleDocu.EmpaqueInvID.Value = refer != null ? refer.EmpaqueInvID.Value : string.Empty;
                }
            }
            if (fieldName == "Parametro1")
            {
                _pksPar1["Parametro1ID"] = e.Value.ToString().Trim();
                validField = _bc.ValidGridCellComplex(this.gvDocument, string.Empty, e.RowHandle, fieldName, _pksPar1, false, AppMasters.inTipoParameter1);
            }
            if (fieldName == "Parametro2")
            {
                _pksPar2["Parametro2ID"] = e.Value.ToString().Trim();
                validField = _bc.ValidGridCellComplex(this.gvDocument, string.Empty, e.RowHandle, fieldName, _pksPar2, false, AppMasters.inTipoParameter2);
            }
            if (fieldName == "ProyectoID")
            {
                validField = _bc.ValidGridCell(this.gvDocument, string.Empty, e.RowHandle, fieldName, false, true, true, AppMasters.coProyecto);
                if (validField)
                {
                    this.data.Footer[e.RowHandle].SolicitudCargos[0].ProyectoID.Value = this.data.Footer[e.RowHandle].ProyectoID;
                    if (this.data.Footer[e.RowHandle].SolicitudCargos[0].PorcentajeID.Value.Value == 0)
                        this.data.Footer[e.RowHandle].SolicitudCargos[0].PorcentajeID.Value = 100;
                    this.gcCargos.RefreshDataSource();
                    //Trae el proyecto del Modulo Proyectos
                    this.data.Footer[index].DetalleDocu.TareaID.Value = null;
                    this.data.Footer[index].DetalleDocu.ConsecTarea = null;
                    this.data.Footer[index].DetalleDocu.CantidadTarea.Value = null;
                    DTO_glDocumentoControl filter = new DTO_glDocumentoControl();
                    filter.ProyectoID.Value = this._rowCurrent.ProyectoID;
                    filter.DocumentoID.Value = AppDocuments.Proyecto;
                    List<DTO_glDocumentoControl> proy = this._bc.AdministrationModel.glDocumentoControl_GetByParameter(filter);
                    if (proy.Count > 0)
                        this.gvDocument.Columns[this.unboundPrefix + "TareaID"].OptionsColumn.AllowEdit = true;
                    else
                        this.gvDocument.Columns[this.unboundPrefix + "TareaID"].OptionsColumn.AllowEdit = false;
                }
            }
            if (fieldName == "CentroCostoID")
            {
                validField = _bc.ValidGridCell(this.gvDocument, string.Empty, e.RowHandle, fieldName, false, true, true, AppMasters.coCentroCosto);
                if (validField)
                {
                    this.data.Footer[e.RowHandle].SolicitudCargos[0].CentroCostoID.Value = this.data.Footer[e.RowHandle].CentroCostoID;
                    if (this.data.Footer[e.RowHandle].SolicitudCargos[0].PorcentajeID.Value.Value == 0)
                        this.data.Footer[e.RowHandle].SolicitudCargos[0].PorcentajeID.Value = 100;
                    this.gcCargos.RefreshDataSource();
                }
            }
            if (fieldName == "UnidadInvID")
                validField = _bc.ValidGridCell(this.gvDocument, string.Empty, e.RowHandle, fieldName, false, true, false, AppMasters.inUnidad);            
            #endregion
            if (fieldName == "TareaID")
            {
                this.data.Footer[index].DetalleDocu.TareaID.Value = e.Value.ToString();
                if (string.IsNullOrEmpty(this.data.Footer[index].DetalleDocu.TareaID.Value))
                {
                    this.data.Footer[index].DetalleDocu.ConsecTarea = null;
                    this.data.Footer[index].DetalleDocu.CantidadTarea.Value = null;
                }              
            }
            #region Se modifican CantidadSol
            if (fieldName == "CantidadSol")
            {
                if (this.data.Footer[index].DetalleDocu.CantidadSol.Value == null)
                    this.data.Footer[index].DetalleDocu.CantidadSol.Value = 0;

                validField = _bc.ValidGridCellValue(this.gvDocument, string.Empty, e.RowHandle, fieldName, false, false, true, false);                   
            }           
            if (fieldName == "CantidadDoc5")
            {
                if (this.data.Footer[index].DetalleDocu.CantidadDoc5.Value == null)
                    this.data.Footer[index].DetalleDocu.CantidadDoc5.Value = 0;

                validField = _bc.ValidGridCellValue(this.gvDocument, string.Empty, e.RowHandle, fieldName, false, false, true, false);
                if (validField)
                {
                    this.data.Footer[e.RowHandle].DetalleDocu.ValorUni.Value = this.data.Footer[e.RowHandle].DetalleDocu.ValorUni.Value ?? 0;
                    this.data.Footer[e.RowHandle].DetalleDocu.ValorTotML.Value = Math.Round(this.data.Footer[e.RowHandle].DetalleDocu.ValorUni.Value.Value * Convert.ToDecimal(e.Value, CultureInfo.InvariantCulture),0);
                    this.GetValuesDocument(e.RowHandle);
                }
            }
            #endregion
            #region Se modifican Otros
            if (fieldName == "Descriptivo")
                validField = _bc.ValidGridCell(this.gvDocument, string.Empty, this.gvDocument.FocusedRowHandle, fieldName, false, false, false, null);
            if (fieldName == "ValorUni")
            {
                if (this.data.Footer[index].DetalleDocu.ValorUni.Value == null)
                    this.data.Footer[index].DetalleDocu.ValorUni.Value = 0;

                validField = _bc.ValidGridCellValue(this.gvDocument, string.Empty, e.RowHandle, fieldName, false, true, true, false);
                if (validField)
                {
                    this.data.Footer[e.RowHandle].DetalleDocu.CantidadDoc5.Value = this.data.Footer[e.RowHandle].DetalleDocu.CantidadDoc5.Value ?? 0;
                    this.data.Footer[e.RowHandle].DetalleDocu.ValorTotML.Value = Math.Round(this.data.Footer[e.RowHandle].DetalleDocu.CantidadDoc5.Value.Value * Convert.ToDecimal(e.Value, CultureInfo.InvariantCulture),0);
                    this.GetValuesDocument(e.RowHandle);
                }
            }            
            #endregion

            //this.RowEdited = true;
            if (!validField)
                this.isValid = false;
            //FormProvider.Master.itemSendtoAppr.Enabled = false;
            FormProvider.Master.itemPrint.Enabled = false;

            if (this.ValidateRow(this.indexFila))
                this.gcCargos.Enabled = true;  
        }

        /// <summary>
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0)
                this._rowCurrent = (DTO_prSolicitudFooter)this.gvDocument.GetRow(e.FocusedRowHandle);
            if (this.isValidCargo)
                base.gvDocument_FocusedRowChanged(sender, e);
        }

        /// <summary>
        /// Ocurre al presionar un botos para traer una FK
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void editBtnGrid_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                string colName = this.gvDocument.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);

                if (!colName.Contains("Parametro"))
                    if (colName.Equals("TareaID"))
                    {
                        ModalProyectoMvto dialog = new ModalProyectoMvto(this._rowCurrent.ProyectoID);
                        dialog.ShowDialog();
                        if (dialog.TareaSelected != null)
                        {
                            this._rowCurrent.DetalleDocu.TareaID.Value = dialog.TareaSelected.TareaID.Value;
                            this._rowCurrent.DetalleDocu.ConsecTarea = dialog.TareaSelected.Consecutivo.Value;
                            this._rowCurrent.DetalleDocu.CantidadTarea.Value = dialog.TareaSelected.Cantidad.Value;
                            this.gvDocument.RefreshData();
                        }
                    }                       
                    else
                        base.editBtnGrid_ButtonClick(sender, e);
                else
                {
                    colName += "ID";
                    ButtonEdit origin = (ButtonEdit)sender;
                    this.ShowFKComplex(this.gvDocument.FocusedRowHandle, colName, origin);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Eventos Grilla Cargos
        /// <summary>
        /// Acciona los botones de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gcCargos_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            if (this._tipoCodigo == TipoCodigo.Servicio || this._tipoCodigo == TipoCodigo.Suministros || this._tipoCodigo == TipoCodigo.SuministroPersonal)
            {
                this.gvCargos.PostEditor();
                if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Custom)
                {
                    if (this.gvCargos.ActiveFilterString != string.Empty)
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoAddInFilter));
                    else
                    {
                        this.deleteOP = false;
                        if (this.isValidCargo)
                        {
                            this.newRegCargo = true;
                            this.AddNewCargoRow();
                        }
                        else
                        {
                            bool isV = this.ValidateCargoRow(this.gvCargos.FocusedRowHandle);
                            if (isV)
                            {
                                this.newRegCargo = true;
                                this.AddNewCargoRow();
                            }
                        }
                    }
                }
                if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Remove)
                {
                    string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Delete);
                    string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Register);

                    //Revisa si desea cargar los temporales
                    if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        this.deleteOP = true;
                        int rowHandle = this.gvCargos.FocusedRowHandle;

                        if (this.data.Footer[this.indexFila].SolicitudCargos.Count == 1)
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.RowsNeeded));
                            e.Handled = true;
                        }
                        else
                        {
                            this.data.Footer[this.indexFila].SolicitudCargos.RemoveAll(x => x.Index == this.indexFilaCargos);
                            //Si borra el primer registro
                            if (rowHandle == 0)
                                this.gvCargos.FocusedRowHandle = 0;
                            //Si selecciona el ultimo
                            else
                                this.gvCargos.FocusedRowHandle = rowHandle - 1;

                            this.UpdateTemp(this.data);

                            this.gvCargos.RefreshData();
                            this.RowIndexCargosChanged(this.gvCargos.FocusedRowHandle, true);
                        }
                    }
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvCargos_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefixCargo.Length);

            if (fieldName == "ProyectoID" || fieldName == "CentroCostoID")
                e.RepositoryItem = this.editBtnGridCargos;

            if (fieldName == "PorcentajeID")
                e.RepositoryItem = this.editSpinPorc;
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvCargos_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefixCargo.Length);
            int index = this.NumFilaCargos;
            bool validField = true;

            #region Se modifican FKs
            if (fieldName == "ProyectoID")
            {
                validField = _bc.ValidGridCell(this.gvCargos, this.unboundPrefixCargo, e.RowHandle, fieldName, false, true, true, AppMasters.coProyecto);
                if (validField && e.RowHandle == 0)
                {
                    this.data.Footer[this.indexFila].ProyectoID = this.data.Footer[this.indexFila].SolicitudCargos[0].ProyectoID.Value;
                    this.gcDocument.RefreshDataSource();
                }
            }
            if (fieldName == "CentroCostoID")
            {
                validField = _bc.ValidGridCell(this.gvCargos, this.unboundPrefixCargo, e.RowHandle, fieldName, false, true, true, AppMasters.coCentroCosto);
                if (validField && e.RowHandle == 0)
                {
                    this.data.Footer[this.indexFila].CentroCostoID = this.data.Footer[this.indexFila].SolicitudCargos[0].CentroCostoID.Value;
                    this.gcDocument.RefreshDataSource();
                }
            }
            #endregion
            #region Se modifican PorcentajeID
            if (fieldName == "PorcentajeID")
            {
                if (this.data.Footer[this.indexFila].SolicitudCargos[index].PorcentajeID.Value == null)
                    this.data.Footer[this.indexFila].SolicitudCargos[index].PorcentajeID.Value = 0;

                validField = _bc.ValidGridCellValue(this.gvCargos, this.unboundPrefixCargo, e.RowHandle, fieldName, false, false, false, false);
            }
            #endregion

            //this.RowEdited = true;
            if (!validField)
                this.isValidCargo = false;
            //FormProvider.Master.itemSendtoAppr.Enabled = false;
            FormProvider.Master.itemPrint.Enabled = false;
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvCargos_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            try
            {
                if (this.data.Footer.Count > 0)
                {
                    bool validRow = this.deleteOP ? true : this.ValidateCargoRow(e.RowHandle);
                    this.deleteOP = false;
                    if (validRow)
                        this.isValidCargo = true;
                    else
                    {
                        e.Allow = false;
                        this.isValidCargo = false;
                    }
                    if (this.data.Footer[this.indexFila].SolicitudCargos.Sum(x => x.PorcentajeID.Value.Value) != 100)
                    {
                        this.isValidCargo = false;
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Pr_PorcentajeNoCien));
                    }
                }
            }
            catch (Exception ex)
            {                
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp--DocumentoProvForm.cs", "gvCargos_BeforeLeaveRow"));
            }
        }

        /// <summary>
        /// Valida los datos antes de salir de la grilla de Cergos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gcCargos_Leave(object sender, EventArgs e)
        {
            this.isValidCargo = true;
            this.gvCargos.PostEditor();
            if (this.gvCargos.DataRowCount > 0)
            {
                if (!this.ValidateCargoRow(this.indexFilaCargos))
                {
                    this.gcCargos.Focus();
                    this.isValidCargo = false;
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Pr_InvalidCargos));
                }
                else if (this.data.Footer[this.indexFila].SolicitudCargos.Sum(x => x.PorcentajeID.Value.Value) != 100)
                {
                    //this.gcCargos.Focus();
                    this.isValidCargo = false;
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Pr_PorcentajeNoCien));
                } 
            }
        }

        /// <summary>
        /// Evento que se ejecuta al cambiar el foco de una columna a otra
        /// Cambia el estilo de una celda segun las condiciones del formulario
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvCargos_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {
            bool editableCell = true;
            string fieldName = this.gvCargos.FocusedColumn.FieldName.Substring(this.unboundPrefixCargo.Length);

            #region Validacion de otros campos
           
            #endregion

            if (editableCell)
                this.gvDocument.Appearance.FocusedCell.BackColor = Color.White;
            else
                this.gvDocument.Appearance.FocusedCell.BackColor = Color.Lavender;
        }

        /// <summary>
        /// Revisa botones al digitar algo sobre la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvCargos_KeyUp(object sender, KeyEventArgs e)
        {
            //if (!string.IsNullOrWhiteSpace(this.lastColNameCargos) && this.gvCargos.IsLastRow && this.gvCargos.FocusedColumn.FieldName == this.lastColNameCargos && e.KeyCode == Keys.Tab)
            //{
            //    bool isV = this.ValidateCargoRow(this.gvCargos.FocusedRowHandle);
            //    if (isV)
            //    {
            //        this.newRegCargo = true;
            //        this.AddNewCargoRow();
            //    }
            //}
        }

        /// <summary>
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvCargos_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            this.RowIndexCargosChanged(e.FocusedRowHandle, false);
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvCargos_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            DTO_prSolicitudCargos dto = (DTO_prSolicitudCargos)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefixCargo.Length);

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
        /// Ocurre al presionar un botos para traer una FK
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void editBtnGridCargos_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            //this.cargosFocus = true;
            this.IsModalFormOpened = true;
            try
            {
                string colName = this.gvCargos.FocusedColumn.FieldName.Substring(this.unboundPrefixCargo.Length);

                ButtonEdit origin = (ButtonEdit)sender;
                
                DTO_aplMaestraPropiedades props = _bc.GetMasterPropertyByColId(colName);
                string countMethod = "MasterSimple_Count";
                string dataMethod = "MasterSimple_GetPaged";
                string dataRowMethod = "MasterSimple_GetByID";

                string modFrmCode = props.DocumentoID.ToString();
                string modEmpGrupo = _bc.GetMaestraEmpresaGrupoByDocumentID(Convert.ToInt32(modFrmCode));
                Tuple<int, string> tup = new Tuple<int, string>(Convert.ToInt32(modFrmCode), modEmpGrupo);

                DTO_glTabla fktable = _bc.AdministrationModel.Tables[tup];
                if (fktable.Jerarquica.Value.Value)
                {
                    ModalMaster modal = new ModalMaster(origin, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, true);
                    modal.ShowDialog();
                }
                else
                {
                    ModalMaster modal = new ModalMaster(origin, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, false);
                    modal.ShowDialog();
                }

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                this.IsModalFormOpened = false;
            }
        }
        #endregion

        #region Eventos Detalle (footer)

        /// <summary>
        /// Evento que se ejecuta al salir del detalle de la cuenta
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterDetails_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.gvDocument.RowCount > 0 && this.NumFila >= 0)
                {
                    ControlsUC.uc_MasterFind master = (ControlsUC.uc_MasterFind)sender;
                    GridColumn col = new GridColumn();

                    switch (master.ColId)
                    {
                        case "CodigoBSID":
                            #region CodigoBS
                            if (master.ValidID)
                            {
                                if (this.BienServicio == null || master.Value != this.BienServicio.ID.Value)
                                {
                                    this.data.Footer[this.NumFila].DetalleDocu.CodigoBSID.Value = master.Value;
                                    this.BienServicio = (DTO_prBienServicio)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.prBienServicio, false, master.Value, true);

                                    this.txtDesc.Enabled = true;                                    
                                    this.ValidateRow(this.gvDocument.FocusedRowHandle);
                                    if (this._tipoCodigo == TipoCodigo.Servicio || this._tipoCodigo == TipoCodigo.Suministros || this._tipoCodigo == TipoCodigo.SuministroPersonal)
                                    {
                                        this.txtDesc.Focus();
                                        this.txtDesc.Text = this.BienServicio.Descriptivo.Value;
                                        this.data.Footer[this.NumFila].DetalleDocu.Descriptivo.Value = this.txtDesc.Text;
                                    }
                                    else if (this._tipoCodigo == TipoCodigo.Inventario)
                                    {
                                        if (this.BienServicio.ID.Value != this.codigoBSInventarios)
                                        {
                                            this.masterReferencia.Focus();
                                            this.masterReferencia.Value = this.BienServicio.ID.Value;
                                            this.txtDesc.Text = this.BienServicio.Descriptivo.Value;
                                            this.data.Footer[this.NumFila].DetalleDocu.inReferenciaID.Value = this.masterReferencia.Value;
                                            this.data.Footer[this.NumFila].DetalleDocu.Descriptivo.Value = this.txtDesc.Text; 
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (isImporting)
                                    isImporting = false;
                                else
                                {
                                    this.Referencia = null;
                                    this.BienServicio = null;
                                    this.masterCodigoBS.EnableControl(true);
                                    this.btnCodigoBS.Enabled = true;
                                }
                            }
                            #endregion
                            break;
                        case "inReferenciaID":
                            #region inReferenciaID
                            if (master.ValidID)
                            {
                                if (this.Referencia == null || master.Value != this.Referencia.ID.Value)
                                {
                                    this.data.Footer[this.NumFila].DetalleDocu.inReferenciaID.Value = master.Value;
                                    this.Referencia = (DTO_inReferencia)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, false, master.Value, true);

                                    this.txtDesc.Enabled = true;
                                    this.txtDesc.Text = this.Referencia.Descriptivo.Value;
                                    this.data.Footer[this.NumFila].DetalleDocu.Descriptivo.Value = this.txtDesc.Text;
                                    this.data.Footer[this.NumFila].DetalleDocu.UnidadInvID.Value = this.Referencia.UnidadInvID.Value;
                                    this.data.Footer[this.NumFila].DetalleDocu.EmpaqueInvID.Value = this.Referencia.EmpaqueInvID.Value;
                                    //col = this.gvDocument.Columns[this.unboundPrefix + "inReferenciaID"];
                                    //this.gvDocument.SetColumnError(col, string.Empty);
                                }
                            }
                            else if ((this._tipoCodigo != TipoCodigo.Servicio && this._tipoCodigo != TipoCodigo.Suministros && this._tipoCodigo != TipoCodigo.SuministroPersonal) && this.Referencia != null && master.Value != this.Referencia.ID.Value)
                            {
                                this.Referencia = null;
                                //this.masterReferencia.Focus();
                                this.masterReferencia.EnableControl(true);
                                this.btnReferencia.Enabled = true;
                            }

                            this.ValidateRow(this.gvDocument.FocusedRowHandle);
                            //_bc.ValidGridCell(this.gvDocument, this.gvDocument.FocusedRowHandle, "inReferenciaID", (this.tipoCodigo == TipoCodigo.Servicio) ? true : false, true, false, AppMasters.inReferencia);
                            #endregion
                            break;
                    }

                    #region Valida si trae Info de Inventarios
                    var referenciaInv = (DTO_inReferencia)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, false, master.Value, true);
                    if (referenciaInv != null)
                    {
                        this.data.Footer[this.NumFila].DetalleDocu.MarcaInvID.Value = referenciaInv.MarcaInvID.Value;
                        this.data.Footer[this.NumFila].DetalleDocu.RefProveedor.Value = referenciaInv.RefProveedor.Value;
                    }
                    else
                    {
                        if (this.data.Footer.Count > 0)
                        {
                            this.data.Footer[this.NumFila].DetalleDocu.MarcaInvID.Value = string.Empty;
                            this.data.Footer[this.NumFila].DetalleDocu.RefProveedor.Value = string.Empty;
                        }                       
                    } 
                    #endregion                                      

                    this.gvDocument.RefreshData();
                    //this.RowEdited = true;
                    //FormProvider.Master.itemSendtoAppr.Enabled = false;
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp--DocumentoProvForm.cs", "masterDetails_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al salir del campo de descripcion
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void textControl_Leave(object sender, EventArgs e)
        {
            TextBox ctrl = (TextBox)sender;
            GridColumn col = new GridColumn();

            if (this.NumFila >= 0)
            {
                switch (ctrl.Name)
                {
                    case "txtDesc":
                        #region Descriptivo
                        this.data.Footer[this.NumFila].DetalleDocu.Descriptivo.Value = ctrl.Text;
                        _bc.ValidGridCell(this.gvDocument, string.Empty, this.gvDocument.FocusedRowHandle, "Descriptivo", false, false, false, null);
                        #endregion
                        break;
                }

                this.gvDocument.RefreshData();
            }
        }

        /// <summary>
        /// Al dar clic en la referencia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCodigoBS_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                ModalCodigoBSFilter mod = new ModalCodigoBSFilter(this.documentID);
                mod.ShowDialog();
                this.gvDocument.SetRowCellValue(this.gvDocument.FocusedRowHandle, this.unboundPrefix + "CodigoBSID", mod.IDSelected);
                this.masterCodigoBS.Value = mod.IDSelected;
                this.masterDetails_Leave(this.masterCodigoBS,null);
                this.masterCodigoBS.Focus();
            }
            catch (Exception ex)
            {
                 MessageBox.Show(_bc.GetResourceForException(ex, "WinApp--DocumentoProvForm.cs", "btnReferencia_ButtonClick"));
            }
        }

        /// <summary>
        /// Al dar clic en la referencia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReferencia_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                ModalReferenciasFilter mod = new ModalReferenciasFilter(string.Empty, AppDocuments.TransaccionManual);
                mod.ShowDialog();
                this.gvDocument.SetRowCellValue(this.gvDocument.FocusedRowHandle, this.unboundPrefix + "inReferenciaID", mod.IDSelected);
                this.masterReferencia.Value = mod.IDSelected;
                this.masterDetails_Leave(this.masterReferencia, null);
                this.masterReferencia.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-DocumentMvtoForm.cs", "TBPaste: " + ex.Message));
            }
        }
        
        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para iniciar un nuevo documento
        /// </summary>
        public override void TBNew()
        {
            try
            {
                if (this.validHeader)
                {
                    this.cleanDoc = false;
                    if (this.ReplaceDocument())
                    {
                        _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);
                        this.cleanDoc = true;

                        this.validHeader = false;
                        this.ValidHeaderTB();
                    }
                }

                if (this.cleanDoc)
                {
                    //this.newDoc = true;
                    this.deleteOP = true;

                    this.EnableFooter(false);
                    //this.newDoc = false;
                    this.CleanFooter();

                    this.CleanHeader(true);
                    this.EnableHeader(true);

                    this._bienServicio = new DTO_prBienServicio();
                    this._bienServicioClase = new DTO_glBienServicioClase();
                    this._referencia = new DTO_inReferencia();
                    this._tipoRef = new DTO_inRefTipo();
                    this._tipoCodigo = 0;
                    this.Referencia = null;
                    this.BienServicio = null;
                    this.BienServicioClase = null;

                    FormProvider.Master.itemSendtoAppr.Enabled = false;
                }
                this.isValid = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ///// <summary>
        ///// Boton para iniciar un nuevo documento
        ///// </summary>
        //public override void TBSave()
        //{
        //    this.gcDocument.Focus();
        //}

        /// <summary>
        /// Boton para generar reportes
        /// </summary>
        public override void TBPrint()
        {
            this.GenerateReport(true);
        }

        /// <summary>
        /// Filtra información de la grilla
        /// </summary>
        public override void TBFilterDef()
        {
            bool validRow = true;
            if (this.gvDocument.RowCount > 0)
                validRow = this.ValidateRow(this.gvDocument.FocusedRowHandle);

            if (validRow)
            {
                this.gvDocument.ActiveFilterString = string.Empty;

                if (this.gvDocument.RowCount > 0)
                    this.gvDocument.MoveFirst();
                //this.CalcularTotal();
            }
        }

        /// <summary>
        /// Filtra información de la grilla
        /// </summary>
        public override void TBFilter()
        {
            try
            {
                if (this.data.Footer.Count() > 0 && this.ValidateRow(this.gvDocument.FocusedRowHandle))
                {
                    MasterQuery mq = new MasterQuery(this, this.documentID, _bc.AdministrationModel.User.ReplicaID.Value.Value, false, typeof(DTO_prDetalleDocu), typeof(Filtrable));
                    #region definir Fks
                    mq.SetFK("CodigoBSID", AppMasters.prBienServicio, _bc.CreateFKConfig(AppMasters.prBienServicio));
                    //mq.SetFK("inReferenciaID", AppMasters.inReferencia, _bc.CreateFKConfig(AppMasters.inReferencia));                    
                    #endregion
                    mq.ShowDialog();
                }
            }
            catch (Exception)
            { }
        }

        /// <summary>
        /// Boton para reiniciar un comprobante
        /// </summary>
        public override void TBCopy()
        {
            try
            {
                if (this.ValidGrid())
                {
                    _bc.AdministrationModel.DataCopied = this.data.Footer;
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ResultOK));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Boton para reiniciar un comprobante
        /// </summary>
        public override void TBPaste()
        {
            try
            {
                //Carga la info del comprobante
                List<DTO_prSolicitudFooter> solDet = new List<DTO_prSolicitudFooter>();
                try
                {
                    object o = _bc.AdministrationModel.DataCopied;
                    solDet = (List<DTO_prSolicitudFooter>)o;

                    if (solDet == null)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_InvalidCompPaste));
                        return;
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_InvalidCompPaste));
                    return;
                }

                //Revisa que cumple las condiciones
                if (!this.ReplaceDocument())
                    return;

                _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);
                this.data.Footer = solDet;
                this.LoadData(true);
                this.UpdateTemp(this.data);
                //FormProvider.Master.itemSendtoAppr.Enabled = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Boton para importar datos la data actual
        /// </summary>
        public override void TBImport()
        {
            this.gcDocument.Focus();
            if (!this.ReplaceDocument())
                return;

            this.gvDocument.ActiveFilterString = string.Empty;

            bool hasItems = this.data.Footer.GetEnumerator().MoveNext() ? true : false;
            if (hasItems)
                this.gvDocument.MoveFirst();

            this.pasteRet = CopyPasteExtension.PasteFromClipBoard(this.format);
            Thread process = new Thread(this.ImportThread);
            process.Start();
        }
        #endregion

        #region Hilos

        /// <summary>
        /// Hilo que se ejecuta cualdo el usuario va a imprtar datos al detalle
        /// </summary>
        public override void ImportThread()
        {
            try
            {
                if (this.pasteRet.Success)
                {
                    var text = pasteRet.MsgResult;
                    bool sendToServer = true;
                    string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    #region Variables de función y mensajes de error
                    DTO_TxResult result = new DTO_TxResult();
                    result.Result = ResultValue.OK;
                    result.Details = new List<DTO_TxResultDetail>();
                    //Lista con los dtos a subir y Fks a validas
                    List<DTO_prSolicitudFooter> listFooter = new List<DTO_prSolicitudFooter>();
                    Dictionary<string, List<Tuple<string, bool>>> fks = new Dictionary<string, List<Tuple<string, bool>>>();
                    Dictionary<string, DTO_glBienServicioClase> bienServ = new Dictionary<string,DTO_glBienServicioClase>();
                    Dictionary<string, DTO_inRefTipo> refer = new Dictionary<string,DTO_inRefTipo>();
                    List<string> fkNames = new List<string>();
                    Dictionary<string, string> colNames = new Dictionary<string, string>();
                    Dictionary<string, Object> colVals = new Dictionary<string, Object>();
                    #region Mensajes de error
                    string msgEmptyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField); // "Vacio"
                    string msgInvalidFormat = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidFormat); // "Formato incorrecto"
                    string msgImportInvalidLength = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Import_InvalidLength); // "Los datos ingresados tienen longitud superior a la permitida"
                    string msgFkNotFound = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FkNotFound); // "El código {0} no existe'
                    string msgNoCopyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoCopyField); // "Ningún registro copiado"
                    string msgIncompleteLine = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.IncompleteLine); // "Linea copiada incompleta"
                    string msgFkHierarchyFather = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Import_NotHierarchyFather); // "No puede importar ningún código jerárquico sin movimiento"
                    string msgCero = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ZeroField); // "{0} debe tener un valor diferente de cero'
                    string msgPositive = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue); // "El valor de {0} debe ser un número positivo'


                    string msgPorcentajeNoCien = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Pr_PorcentajeNoCien); // "Porcentaje debe ser 100%"
                    string msgCantidadDeCargos = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Pr_CantidadDeCargos); // "El numero de cargos no puede ser superior a 1 para esta clase de bienes y servicios"
                    string msgInvalidField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidField); // "Inválido"
                        
                    #endregion
                    //Popiedades de un comprobante
                    DTO_prSolicitudFooter det = new DTO_prSolicitudFooter();
                    bool createDTO = true;
                    bool validList = true;
                    #endregion
                    #region Llena las listas de las columnas y FKs
                    List<string> cols = this.format.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    PropertyInfo[] propDetDocu = typeof(DTO_prDetalleDocu).GetProperties();
                    PropertyInfo[] propSolCargos = typeof(DTO_prSolicitudCargos).GetProperties();
                    //Recorre el objeto y revisa el nombre real de la columna
                    colVals.Add(cols[0], string.Empty);
                    colNames.Add(cols[0], "Index");

                    #region Columnas que corresponden a prDetalleDocu
                    foreach (PropertyInfo pi in propDetDocu)
                    {
                        if (!Attribute.IsDefined(pi, typeof(NotImportable)))
                        {
                            string colRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + pi.Name);

                            for (int colIndex = 0; colIndex < cols.Count; colIndex++)
                            {
                                if (cols[colIndex] == colRsx)
                                {
                                    colVals.Add(colRsx, string.Empty);
                                    colNames.Add(colRsx, pi.Name);
                                    break;
                                }
                            }
                        }
                    }
                    #endregion
                    #region Columnas que corresponden a prSolicitudCargos
                    foreach (PropertyInfo pi in propSolCargos)
                    {
                        if (!Attribute.IsDefined(pi, typeof(NotImportable)))
                        {
                            string colRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + pi.Name);

                            for (int colIndex = 0; colIndex < cols.Count; colIndex++)
                            {
                                if (cols[colIndex] == colRsx)
                                {
                                    colVals.Add(colRsx, string.Empty);
                                    colNames.Add(colRsx, pi.Name);
                                    break;
                                }
                            }
                        }
                    }
                    #endregion

                    #region Fks
                    fks.Add(this._codigoRsx, new List<Tuple<string, bool>>());
                    fks.Add(this._referenciaRsx, new List<Tuple<string, bool>>());
                    fks.Add(this._parametro1Rsx, new List<Tuple<string, bool>>());
                    fks.Add(this._parametro2Rsx, new List<Tuple<string, bool>>());
                    fks.Add(this._unidadRsx, new List<Tuple<string, bool>>());
                    fks.Add(this._proyectoRsx, new List<Tuple<string, bool>>());
                    fks.Add(this._centroCostoRsx, new List<Tuple<string, bool>>());
                    #endregion
                    #endregion
                    #region Llena información para enviar a la grilla (lee filas)
                    FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ReadRows) });
                    int percent = 0;
                    int indexLine = -1;
                    for (int i = 0; i < lines.Length; ++i)
                    {
                        #region Actualiza barra de progreso
                        FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                        percent = ((i + 1) * 100) / (lines.Length);

                        if (FormProvider.Master.ProcessCanceled(this.documentID))
                        {
                            result.Details = new List<DTO_TxResultDetail>();
                            result.ResultMessage = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser);
                            result.Result = ResultValue.NOK;
                            break;
                        }
                        #endregion
                        #region Valida si existen datos en la lista importada
                        if (lines.Length == 1)
                        {
                            result.ResultMessage = msgNoCopyField;
                            result.Result = ResultValue.NOK;
                        }
                        #endregion
                        #region Divide cada registro importado en columnas
                        //Recorre todas las columnas y verifica que tengan datos validos
                        string[] line = lines[i].Split(new string[] { CopyPasteExtension.tabChar }, StringSplitOptions.None);
                        #endregion

                        //Works with lines with data only
                        bool nuevo = true;
                        DTO_prDetalleDocu detDocu = null;
                        DTO_prSolicitudCargos detCargos = null;

                        if (i > 0 && line.Length > 0)
                        {
                            createDTO = true;

                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                            rd.line = i;
                            rd.Message = "OK";

                            #region Info básica: Llena los valores de las columnas (se revienta si el numero de columnas al importar es menor al requerido(plantilla))
                            if (line.Length < colNames.Count)
                            {
                                result.Result = ResultValue.NOK;
                                DTO_TxResultDetail rdL = new DTO_TxResultDetail();
                                rdL.line = i;
                                rdL.Message = msgIncompleteLine;
                                result.Details.Add(rdL);

                                createDTO = false;
                                validList = false;
                                continue;
                            }                          
                            else
                            {                           
                                //Recorre Columnas
                                for (int colIndex = 0; colIndex < cols.Count; colIndex++)
                                {
                                    string colRsx = cols[colIndex];  // Obtiene Nombre Columna
                                    colVals[colRsx] = line[colIndex]; // Obtiene valor Columna

                                    if (!string.IsNullOrWhiteSpace(line[colIndex]))
                                    {
                                        #region Recorre las FKs solamente
                                        if (colRsx == this._codigoRsx || colRsx == this._referenciaRsx || colRsx == this._parametro1Rsx ||
                                            colRsx == this._parametro2Rsx || colRsx == this._unidadRsx || colRsx == this._proyectoRsx ||
                                            colRsx == this._centroCostoRsx)
                                        {
                                            colVals[colRsx] = line[colIndex].ToUpper();

                                            #region Revisa si la columna ya existe
                                            Tuple<string, bool> tupValid = new Tuple<string, bool>(line[colIndex].Trim(), true);
                                            Tuple<string, bool> tupInvalid = new Tuple<string, bool>(line[colIndex].Trim(), false);

                                            if (fks[colRsx].Contains(tupValid))
                                                continue;
                                            if (fks[colRsx].Contains(tupInvalid))
                                            {
                                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                rdF.Field = colRsx;
                                                rdF.Message = string.Format(msgFkNotFound, line[colIndex].Trim());
                                                rd.DetailsFields.Add(rdF);

                                                createDTO = false;
                                            }
                                            #endregion
                                            else
                                            { 
                                                int docId = this.GetMasterDocumentID(colRsx);

                                                bool isInt = docId == AppMasters.glDocumento ? true : false;
                                                object dto = _bc.GetMasterDTO(AppMasters.MasterType.Simple, docId, isInt, line[colIndex], true);

                                                bool hierarchyFather = false;
                                                if (dto is DTO_MasterHierarchyBasic)
                                                {
                                                    if ((dto as DTO_MasterHierarchyBasic).MovInd.Value == false)
                                                        hierarchyFather = true;
                                                }
                                                if (dto != null && !hierarchyFather)
                                                {
                                                    fks[colRsx].Add(new Tuple<string, bool>(line[colIndex].Trim(), true));
                                                    #region Asigna los valores de las referencias y bien servicio
                                                   
                                                    if (colRsx == _codigoRsx)
                                                    {
                                                        if (!bienServ.Keys.Contains(line[colIndex].Trim()))
                                                        {
                                                            DTO_prBienServicio bs = (DTO_prBienServicio)dto;
                                                            DTO_glBienServicioClase bsClase = (DTO_glBienServicioClase)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, false, bs.ClaseBSID.Value, true);
                                                            bienServ.Add(line[colIndex].Trim(), bsClase);
                                                        }
                                                    }

                                                    if (colRsx == _referenciaRsx && !string.IsNullOrEmpty(line[colIndex].Trim()))
                                                    {
                                                        if (!refer.Keys.Contains(line[colIndex].Trim()))
                                                        {
                                                            DTO_inReferencia rf = (DTO_inReferencia)dto;
                                                            UDT_BasicID udt = new UDT_BasicID() { Value = rf.TipoInvID.Value };
                                                            DTO_inRefTipo rfTipo = (DTO_inRefTipo)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.inRefTipo, udt, true);
                                                            refer.Add(line[colIndex].Trim(), rfTipo);
                                                        }
                                                    }
                                                    #endregion
                                                }
                                                else
                                                {
                                                    fks[colRsx].Add(new Tuple<string, bool>(line[colIndex].Trim(), false));

                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    if (hierarchyFather)
                                                        rdF.Message = string.Format(msgFkHierarchyFather, line[colIndex]);
                                                    else
                                                        rdF.Message = string.Format(msgFkNotFound, line[colIndex]);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                              }
                                            }
                                        #endregion
                                    }
                                }
                            }
                            #endregion

                            #region Creacion de DTO y validacion Formatos
                            if (createDTO)
                            {
                                #region Revisa  si el index ha cambiado
                                if (indexLine != Convert.ToInt32(colVals[cols[0]]))
                                {
                                    det = new DTO_prSolicitudFooter(); 
                                    det.SolicitudCargos = new List<DTO_prSolicitudCargos>();
                                    detDocu = new DTO_prDetalleDocu();
                                    detCargos = new DTO_prSolicitudCargos();
                                    detCargos.PorcentajeID.Value = 100;
                                    nuevo = true;
                                }
                                else
                                {
                                    detCargos = new DTO_prSolicitudCargos();
                                    detCargos.PorcentajeID.Value = 100;
                                    nuevo = false;
                                }
                                indexLine = Convert.ToInt32(colVals[cols[0]]);
                                #endregion

                                for (int colIndex = 1; colIndex < colNames.Count(); colIndex++)
                                {
                                    string colRsx = cols[colIndex];
                                    try
                                    {
                                        string colName = colNames[colRsx]; //Nombre Columna
                                        string colValue = colVals[colRsx].ToString().Trim(); // Valor Columna

                                        #region Validacion de Nulls (Campos basicos)
                                        if (string.IsNullOrEmpty(colValue) &&  (colRsx == _codigoRsx ||
                                                colRsx == _unidadRsx ||  colRsx == _descRsx ||
                                                colRsx == _cantidadRsx ||  colRsx == _proyectoRsx ||
                                                colRsx == _centroCostoRsx ||  colRsx == _porcentajeRsx))
                                        {
                                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = colRsx;
                                            rdF.Message = msgEmptyField;
                                            rd.DetailsFields.Add(rdF);
                                            createDTO = false;
                                        }

                                        #endregion
                                        #region Validacion Formatos
                                        #region Define UDT type
                                        PropertyInfo pi = null;
                                        UDT udt = null;
                                        PropertyInfo piUDT = null;
                                        if (nuevo)
                                        {
                                            try
                                            {
                                                pi = detDocu.GetType().GetProperty(colName);
                                                if (pi != null)
                                                {
                                                    udt = (UDT)pi.GetValue(detDocu, null);
                                                    piUDT = udt.GetType().GetProperty("Value");
                                                }
                                                else
                                                {
                                                    pi = detCargos.GetType().GetProperty(colName);
                                                    udt = (UDT)pi.GetValue(detCargos, null);
                                                    piUDT = udt.GetType().GetProperty("Value");                                                
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                throw ex; 
                                            }
                                        }
                                        else
                                        {
                                            if (colRsx == _proyectoRsx || colRsx == _centroCostoRsx || colRsx == _porcentajeRsx)
                                            {
                                                try
                                                {
                                                    pi = detCargos.GetType().GetProperty(colName);
                                                    udt = (UDT)pi.GetValue(detCargos, null);
                                                    piUDT = udt.GetType().GetProperty("Value");
                                                }
                                                catch (Exception ex)
                                                {
                                                    throw ex;
                                                }
                                            }
                                            else
                                                continue;
                                        }
                                        #endregion
                                        #region Validaciones basicas
                                        //Comprueba los valores solo Bool
                                        if (piUDT.PropertyType.Equals(typeof(bool)) || piUDT.PropertyType.Equals(typeof(Nullable<bool>)))
                                        {
                                            string colVal = "false";
                                            if (colValue.Trim() != string.Empty)
                                            {
                                                colVal = "true";
                                                if (colValue.ToLower() != "x")
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + " (x)";
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            colVals[colRsx] = colVal;
                                        }

                                        //Valida formatos para las otras columnas
                                        if (colValue != string.Empty)
                                        {
                                            #region Fechas
                                            if (piUDT.PropertyType.Equals(typeof(DateTime)) || piUDT.PropertyType.Equals(typeof(Nullable<DateTime>)))
                                            {
                                                try
                                                {
                                                    DateTime val = DateTime.ParseExact(colValue, FormatString.Date, System.Globalization.CultureInfo.InvariantCulture);
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDate);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            } 
                                            #endregion
                                            #region Numericas
                                            if (piUDT.PropertyType.Equals(typeof(int)) || piUDT.PropertyType.Equals(typeof(Nullable<int>)))
                                            {
                                                try
                                                {
                                                    int val = Convert.ToInt32(colValue);
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatInvalidNumber);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            } 
                                            if (piUDT.PropertyType.Equals(typeof(long)) || piUDT.PropertyType.Equals(typeof(Nullable<long>)))
                                            {
                                                try
                                                {
                                                    long val = Convert.ToInt64(colValue);
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatInteger);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            if (piUDT.PropertyType.Equals(typeof(short)) || piUDT.PropertyType.Equals(typeof(Nullable<short>)))
                                            {
                                                try
                                                {
                                                    short val = Convert.ToInt16(colValue);
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatLimitNumber);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            if (piUDT.PropertyType.Equals(typeof(byte)) || piUDT.PropertyType.Equals(typeof(Nullable<byte>)))
                                            {
                                                try
                                                {
                                                    byte val = Convert.ToByte(colValue);
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatNumberRange);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            if (piUDT.PropertyType.Equals(typeof(decimal)) || piUDT.PropertyType.Equals(typeof(Nullable<decimal>)))
                                            {
                                                try
                                                {
                                                    decimal val = Convert.ToDecimal(colValue, CultureInfo.InvariantCulture);
                                                    if (colValue.Trim().Contains(','))
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDecimal);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDecimal);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            #endregion
                                        }
                                        #endregion
                                        #endregion

                                        // Si paso las validaciones Asigna el valor al DTO
                                        if (createDTO && !string.IsNullOrWhiteSpace(colValue))
                                            udt.SetValueFromString(colValue); // Llena el campo con el valor correspondiente
                                    }
                                    #region Exception
                                    catch (Exception ex1)
                                    {
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = colRsx;
                                        rdF.Message = _bc.GetResourceForException(ex1, "WinApp", "DocumentProvFrom.cs - Creacion de DTO y validacion Formatos");
                                        rd.DetailsFields.Add(rdF);

                                        createDTO = false;
                                    }
                                    #endregion
                                }
                            }
                            #endregion

                            #region Revisa si hay algun campo invalido 
                            result.Details.Add(rd);
                            if (rd.DetailsFields.Count > 0)
                            {
                                rd.Message = "NOK";
                                result.Result = ResultValue.NOK;
                            }
                            #endregion

                            if (createDTO && validList)
                            {
                                if (nuevo)
                                {
                                    det.DetalleDocu = detDocu;
                                    det.SolicitudCargos.Add(detCargos);
                                    det.ProyectoID = detCargos.ProyectoID.Value;
                                    det.CentroCostoID = detCargos.CentroCostoID.Value;
                                    listFooter.Add(det); // Agrega un registro validado a la lista
                                }
                                else
                                    det.SolicitudCargos.Add(detCargos);
                            }
                            else
                                validList = false;
                        }
                    }
                    #endregion
                    #region Valida las restricciones particulares de la solicitud
                    if (validList)
                    {
                        result.Details = new List<DTO_TxResultDetail>();

                        int index = this.NumFila;
                        int i = 0;

                        FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ValidatingData) });
                        percent = 0;

                        foreach (DTO_prSolicitudFooter dto in listFooter)
                        {
                            #region Barra de Progreso
                            createDTO = true;
                            FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                            percent = ((i + 1) * 100) / (listFooter.Count);

                            if (FormProvider.Master.ProcessCanceled(this.documentID))
                            {
                                result.Details = new List<DTO_TxResultDetail>();
                                result.ResultMessage = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser);
                                result.Result = ResultValue.NOK;
                                break;
                            }
                            #endregion

                            #region Indexes
                            dto.DetalleDocu.Index = i;
                            int indexCargo = 0;
                            foreach (DTO_prSolicitudCargos cargo in dto.SolicitudCargos)
                            {
                                cargo.IndexDet = index;
                                cargo.Index = indexCargo;
                                indexCargo++;
                            }
                            i++;
                            #endregion

                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                            rd.line = i;
                            rd.Message = "OK";

                            DTO_glBienServicioClase bsClase = bienServ[dto.DetalleDocu.CodigoBSID.Value];
                            DTO_inRefTipo refTipo = null;
                            if (!string.IsNullOrEmpty(dto.DetalleDocu.inReferenciaID.Value))
                                refTipo = refer[dto.DetalleDocu.inReferenciaID.Value];

                       
                            #region Validar cantidad de los registros en prSolicitudCargos
                            if (((TipoCodigo)Enum.Parse(typeof(TipoCodigo), bsClase.TipoCodigo.Value.Value.ToString()) != TipoCodigo.Servicio &&
                                (TipoCodigo)Enum.Parse(typeof(TipoCodigo), bsClase.TipoCodigo.Value.Value.ToString()) != TipoCodigo.Suministros &&
                                (TipoCodigo)Enum.Parse(typeof(TipoCodigo), bsClase.TipoCodigo.Value.Value.ToString()) != TipoCodigo.SuministroPersonal) && dto.SolicitudCargos.Count > 1)
                            {
                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                rdF.Field = this._solCargosRsx;
                                rdF.Message = msgCantidadDeCargos;
                                rd.DetailsFields.Add(rdF);
                                createDTO = false;
                            }
                            #endregion
                            #region Validaciones particulares de la solicitud
                            this.ValidateDataImport(dto.DetalleDocu, bsClase, refTipo, rd, msgFkNotFound, msgInvalidField);
                            foreach (DTO_prSolicitudCargos cargo in dto.SolicitudCargos)
                                this.ValidateDataImport(cargo, bsClase, refTipo, rd, msgFkNotFound, msgInvalidField);
                            #endregion

                            result.Details.Add(rd);
                            if (rd.DetailsFields.Count > 0)
                            {
                                rd.Message = "NOK";
                                result.Result = ResultValue.NOK;
                                result.ResultMessage = ResultValue.NOK.ToString();
                            }
                        }
                    }
                    #endregion
                    #region Actualiza la información de la grilla
                    if (sendToServer && result.Result == ResultValue.OK)
                    {
                        MessageForm frm = new MessageForm(result);
                        if (result.Result.Equals(ResultValue.OK))
                        {
                            this.data.Footer = listFooter;
                            this.UpdateTemp(this.data);
                            this.Invoke(this.refreshGridDelegate);
                            this.Invoke(this.afterImportDelegate);
                        }
                        this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                    }
                    else
                    {
                        MessageForm frm = new MessageForm(result);
                        this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                    }
                    #endregion
                }
                else
                {
                    MessageForm frm = new MessageForm(pasteRet.MsgResult, MessageType.Error);
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                this.importando = false;
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion

        #region Filtrado de la grilla

        /// <summary>
        /// Asigna una consulta desde MasterQuery para hacer el filtrado de datos
        /// </summary>
        /// <param name="consulta"></param>
        /// <param name="fields"></param>
        public override void SetConsulta(DTO_glConsulta consulta, List<ConsultasFields> fields)
        {
            try
            {
                string filtros = Transformer.FiltrosGrilla(consulta.Filtros, fields, typeof(DTO_prDetalleDocu));
                //this.disableValidate = true;
                this.deleteOP = true;
                this.newDoc = true;
                filtros = filtros.Replace("[", "[DetalleDocu.");
                this.gvDocument.ActiveFilterString = filtros;
                if (this.gvDocument.RowCount > 0)
                    this.RowIndexChanged(0, true);
            }
            catch (Exception e)
            {
                ;
            }
            finally
            {
                this.disableValidate = false;
            }
        }

        #endregion
    }
}
