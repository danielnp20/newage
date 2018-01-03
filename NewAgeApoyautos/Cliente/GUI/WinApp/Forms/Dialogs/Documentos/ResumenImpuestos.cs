using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using NewAge.DTO.UDT;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Collections;
using DevExpress.XtraGrid.Views.Grid;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de Resumen de los Impuestos de causacion factura
    /// </summary>
    public partial class ResumenImpuestos : Form
    {
        #region Variables
        private BaseController _bc = BaseController.GetInstance();
        private int _documentID = AppForms.ResumenImpuestosForm;
        private TipoMoneda _tipoMoneda;
        private string _tercero;
        protected string unboundPrefix = "Unbound_";
        private List<DTO_ComprobanteFooter> _footer = new List<DTO_ComprobanteFooter>();
        private List<DTO_ResumenImpustos> _resumenImp = new List<DTO_ResumenImpustos>();
        public bool _aprobado = false;
        #endregion

        /// <summary>
        /// Constructor de la grilla de los impustos
        /// </summary>
        /// <param name="footer">Lista DTO_ComprobanteFooter</param>
        /// 
        /// <param name="antResumen">Lista de anticipos que ya fueron cargados</param>
        /// <param name="periodo">Periodo sobre el cual se van a consultar los anticipos</param>
        /// <param name="tm">Tipo de moneda sobre la cual se esta trabajando en el documento</param>
        /// <param name="tasaCambio">Tasa de cambio actual del documento</param>
        public ResumenImpuestos(List<DTO_ComprobanteFooter> footer, TipoMoneda tm, string terceroID)
        {
            //Inicializa el formulario
            InitializeComponent();
            FormProvider.LoadResources(this, this._documentID);

            //variables
            this._tipoMoneda = tm;
            this._footer = footer;
            this._aprobado = false;
            this._tercero = terceroID;
            
            //Carga de datos
            this.InitControls();
            this.LoadGridStructure();
            this.LoadGridData();
        }

        #region Funciones privadas

        /// <summary>
        /// Inicia los controles
        /// </summary>
        private void InitControls()
        {
            _bc.InitMasterUC(this.masterTercero, AppMasters.coTercero, true, true, true, false);
            _bc.InitMasterUC(this.masterDocTipo, AppMasters.coTerceroDocTipo, true, true, true, false);
            _bc.InitMasterUC(this.masterCiudad, AppMasters.glLugarGeografico, false, true, true, false);
            _bc.InitMasterUC(this.masterDepto, AppMasters.glLugarGeografico, false, true, false, false);
            _bc.InitMasterUC(this.masterPais, AppMasters.glPais, true, true, true, false);

            this.masterTercero.EnableControl(false);
            this.masterDocTipo.EnableControl(false);
            this.masterCiudad.EnableControl(false);
            this.masterDepto.EnableControl(false);
            this.masterPais.EnableControl(false);

            DTO_coTercero terceroDTO = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, this._tercero, true);
            if (terceroDTO != null)
            {
                this.masterTercero.Value = this._tercero;
                this.masterDocTipo.Value = terceroDTO.TerceroDocTipoID.Value;
                this.masterCiudad.Value = terceroDTO.LugarGeograficoID.Value;
                this.masterDepto.Value = this.masterCiudad.Value.Substring(0, 2);

                //Pais
                DTO_glLugarGeografico lg = (DTO_glLugarGeografico)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glLugarGeografico, false, this.masterCiudad.Value, true);
                this.masterPais.Value = lg.PaisID.Value;
                
                //Regimen
                DTO_coRegimenFiscal rf = (DTO_coRegimenFiscal)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coRegimenFiscal, false, terceroDTO.ReferenciaID.Value, true);
                switch (rf.TipoTercero.Value.Value)
                {
                    case 1:
                        this.txtRegimen.Text = _bc.GetResource(LanguageTypes.Tables, "tbl_restr_TipoTercero_v1");
                        break;
                    case 2:
                        this.txtRegimen.Text = _bc.GetResource(LanguageTypes.Tables, "tbl_restr_TipoTercero_v2");
                        break;
                    case 3:
                        this.txtRegimen.Text = _bc.GetResource(LanguageTypes.Tables, "tbl_restr_TipoTercero_v3");
                        break;
                }
                
            }
        }

        /// <summary>
        /// Genera la estructura de la grilla
        /// </summary>
        private void LoadGridStructure()
        {
            try
            {
                //Index
                GridColumn index = new GridColumn();
                index.FieldName = this.unboundPrefix + "Descriptivo";
                index.Visible = false;
                this.gvData.Columns.Add(index);

                //Descriptivo
                GridColumn desc = new GridColumn();
                desc.FieldName = this.unboundPrefix + "Descriptivo";
                desc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_Descriptivo");
                desc.UnboundType = UnboundColumnType.String;
                desc.VisibleIndex = 1;
                desc.Width = 200;
                desc.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(desc);

                //Cuenta
                GridColumn cuenta = new GridColumn();
                cuenta.FieldName = this.unboundPrefix + "CuentaID";
                cuenta.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_CuentaID");
                cuenta.UnboundType = UnboundColumnType.String;
                cuenta.VisibleIndex = 2;
                cuenta.Width = 100;
                cuenta.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(cuenta);

                //Base
                GridColumn vlrBase = new GridColumn();
                vlrBase.FieldName = this.unboundPrefix + "Base";
                vlrBase.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_Base");
                vlrBase.UnboundType = UnboundColumnType.Decimal;
                vlrBase.VisibleIndex = 4;
                vlrBase.Width = 120;
                vlrBase.OptionsColumn.AllowEdit = false;
                vlrBase.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                this.gvData.Columns.Add(vlrBase);

                ////Base (Moneda Extranjera)
                //if (this._tipoMoneda == TipoMoneda.Both || this._tipoMoneda == TipoMoneda.Foreign)
                //{
                //    GridColumn baseME = new GridColumn();
                //    baseME.FieldName = this.unboundPrefix + "BaseME";
                //    baseME.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_BaseME");
                //    baseME.UnboundType = UnboundColumnType.Decimal;
                //    baseME.VisibleIndex = 4;
                //    baseME.Width = 110;
                //    baseME.OptionsColumn.AllowEdit = false;
                //    this.gvData.Columns.Add(baseME);
                //}

                //Tarifa
                GridColumn tarifa = new GridColumn();
                tarifa.FieldName = this.unboundPrefix + "Tarifa";
                tarifa.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_Tarifa");
                tarifa.UnboundType = UnboundColumnType.Decimal;
                tarifa.VisibleIndex = 4;
                tarifa.Width = 70;
                tarifa.OptionsColumn.AllowEdit = false;
                tarifa.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                this.gvData.Columns.Add(tarifa);

                //Valor
                GridColumn valor = new GridColumn();
                valor.FieldName = this.unboundPrefix + "Valor";
                valor.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_Valor");
                valor.UnboundType = UnboundColumnType.Decimal;
                valor.VisibleIndex = 4;
                valor.Width = 120;
                valor.OptionsColumn.AllowEdit = false;
                valor.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                this.gvData.Columns.Add(valor);

                ////Valor (Moneda Extranjera)
                //if (this._tipoMoneda == TipoMoneda.Both || this._tipoMoneda == TipoMoneda.Foreign)
                //{
                //    GridColumn valorME = new GridColumn();
                //    valorME.FieldName = this.unboundPrefix + "ValorME";
                //    valorME.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_ValorME");
                //    valorME.UnboundType = UnboundColumnType.Decimal;
                //    valorME.VisibleIndex = 4;
                //    valorME.Width = 110;
                //    valorME.OptionsColumn.AllowEdit = false;
                //    this.gvData.Columns.Add(valorME);
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ResumenImpuesto.cs", "LoadGridStructure"));
            }
        }

        /// <summary>
        /// Carga los datos de la grilla
        /// </summary>
        private void LoadGridData()
        {            
            #region Variables
            DTO_coPlanCuenta _cuenta = new DTO_coPlanCuenta();
            DTO_coCuentaGrupo _cuentaGrupo = new DTO_coCuentaGrupo();
            Dictionary<string, DTO_coPlanCuenta> cacheCuenta = new Dictionary<string, DTO_coPlanCuenta>();
            Dictionary<string, DTO_coCuentaGrupo> cacheCuentaGrupo = new Dictionary<string, DTO_coCuentaGrupo>();
            
            List<DTO_ResumenImpustos> impuestos = new List<DTO_ResumenImpustos>();
            DTO_ResumenImpustos imp;
            #endregion

            try
            {
                foreach (DTO_ComprobanteFooter item in this._footer)
                {
                    if (string.IsNullOrWhiteSpace(item.DatoAdd2.Value))
                        item.DatoAdd2.Value = "0";

                    #region Trae Cuenta y CuentaGrupo
                    if (cacheCuenta.ContainsKey(item.CuentaID.Value))
                        _cuenta = cacheCuenta[item.CuentaID.Value];
                    else
                    {
                        _cuenta = (DTO_coPlanCuenta)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.coPlanCuenta, new UDT_BasicID() { Value = item.CuentaID.Value }, true);
                        cacheCuenta.Add(item.CuentaID.Value, _cuenta);
                    }

                    if (cacheCuentaGrupo.ContainsKey(_cuenta.CuentaGrupoID.Value))
                        _cuentaGrupo = cacheCuentaGrupo[_cuenta.CuentaGrupoID.Value];
                    else
                    {
                        _cuentaGrupo = (DTO_coCuentaGrupo)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.coCuentaGrupo, _cuenta.CuentaGrupoID, true);
                        cacheCuentaGrupo.Add(_cuenta.CuentaGrupoID.Value, _cuentaGrupo);
                    }
                    #endregion

                    imp = new DTO_ResumenImpustos();
                    #region Valores Bruto
                    if (string.IsNullOrEmpty(_cuenta.ImpuestoTipoID.Value) && item.DatoAdd4.Value != AuxiliarDatoAdd4.Anticipo.ToString()
                            && item.DatoAdd1.Value != AuxiliarDatoAdd1.IVA.ToString() && _cuentaGrupo.CostoInd.Value.Value)
                    {
                        imp.Index = 1;
                        imp.Descriptivo = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_DescCostos");
                        imp.CuentaID = string.Empty;
                        imp.Tarifa = Convert.ToDecimal(item.DatoAdd2.Value.Trim());
                        if (string.IsNullOrEmpty(item.DatoAdd2.Value.Trim()))
                        {
                            imp.Index = 7;
                            imp.Descriptivo = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_DescOtrosDb");
                        }
                    }
                    #endregion
                    #region Anticipos
                    if (string.IsNullOrEmpty(_cuenta.ImpuestoTipoID.Value) && item.DatoAdd4.Value == AuxiliarDatoAdd4.Anticipo.ToString())
                    {
                        imp.Index = 8;
                        imp.Descriptivo = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_DescAnticipos");
                        imp.CuentaID = string.Empty;
                        imp.Tarifa = 0;
                    }
                    #endregion
                    #region IVAs
                    if (!string.IsNullOrEmpty(_cuenta.ImpuestoTipoID.Value) && _cuenta.ImpuestoTipoID.Value == this._bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoIVA)
                            || _cuentaGrupo.CostoInd.Value.Value && !string.IsNullOrEmpty(item.DatoAdd1.Value) && item.DatoAdd1.Value == AuxiliarDatoAdd1.IVA.ToString())
                    {
                        imp.Index = 2;
                        imp.Descriptivo = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_DescIVAs");
                        imp.CuentaID = item.CuentaID.Value.Trim();
                        imp.Tarifa = (_cuentaGrupo.CostoInd.Value.Value && !string.IsNullOrEmpty(item.DatoAdd1.Value) && item.DatoAdd1.Value == AuxiliarDatoAdd1.IVA.ToString())?
                            Convert.ToDecimal(item.DatoAdd2.Value.Trim(), CultureInfo.InvariantCulture) : _cuenta.ImpuestoPorc.Value.Value;
                        if (this._tipoMoneda == TipoMoneda.Both || this._tipoMoneda == TipoMoneda.Local)
                            imp.BaseML = item.vlrBaseML.Value.Value;
                        if (this._tipoMoneda == TipoMoneda.Both || this._tipoMoneda == TipoMoneda.Foreign)
                            imp.BaseME = item.vlrBaseME.Value.Value;
                    }
                    #endregion
                    #region ReteIVAs
                    if (!string.IsNullOrEmpty(_cuenta.ImpuestoTipoID.Value) && _cuenta.ImpuestoTipoID.Value == this._bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteIVA))
                    {
                        imp.Index = 3;
                        imp.Descriptivo = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_DescReteIVAs");
                        imp.CuentaID = item.CuentaID.Value.Trim();
                        imp.Tarifa = _cuenta.ImpuestoPorc.Value.Value;
                        if (this._tipoMoneda == TipoMoneda.Both || this._tipoMoneda == TipoMoneda.Local)
                            imp.BaseML = item.vlrBaseML.Value.Value;
                        if (this._tipoMoneda == TipoMoneda.Both || this._tipoMoneda == TipoMoneda.Foreign)
                            imp.BaseME = item.vlrBaseME.Value.Value;
                    }
                    #endregion
                    #region ReteFuentes
                    if (!string.IsNullOrEmpty(_cuenta.ImpuestoTipoID.Value) && _cuenta.ImpuestoTipoID.Value == this._bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteFuente))
                    {
                        imp.Index = 4;
                        imp.Descriptivo = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_DescReteFTs");
                        imp.CuentaID = item.CuentaID.Value.Trim();
                        imp.Tarifa = _cuenta.ImpuestoPorc.Value.Value;
                        if (this._tipoMoneda == TipoMoneda.Both || this._tipoMoneda == TipoMoneda.Local)
                            imp.BaseML = item.vlrBaseML.Value.Value;
                        if (this._tipoMoneda == TipoMoneda.Both || this._tipoMoneda == TipoMoneda.Foreign)
                            imp.BaseME = item.vlrBaseME.Value.Value;
                    }
                    #endregion
                    #region ReteICAs
                    if (!string.IsNullOrEmpty(_cuenta.ImpuestoTipoID.Value) && _cuenta.ImpuestoTipoID.Value == this._bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteICA))
                    {
                        imp.Index = 5;
                        imp.Descriptivo = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_DescReteICAs");
                        imp.CuentaID = item.CuentaID.Value.Trim();
                        imp.Tarifa = _cuenta.ImpuestoPorc.Value.Value;
                        if (this._tipoMoneda == TipoMoneda.Both || this._tipoMoneda == TipoMoneda.Local)
                            imp.BaseML = item.vlrBaseML.Value.Value;
                        if (this._tipoMoneda == TipoMoneda.Both || this._tipoMoneda == TipoMoneda.Foreign)
                            imp.BaseME = item.vlrBaseME.Value.Value;
                    }
                    #endregion
                    #region Otros
                    if (string.IsNullOrEmpty(imp.Descriptivo))
                    {
                        if (!string.IsNullOrEmpty(_cuenta.ImpuestoTipoID.Value))
                        {
                            imp.Index = 6;
                            imp.Descriptivo = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_DescOtrosCr") + " (" + _cuenta.ImpuestoTipoID.Value + ")";
                            imp.CuentaID = item.CuentaID.Value.Trim();
                            imp.Tarifa = _cuenta.ImpuestoPorc.Value.Value;
                            if (this._tipoMoneda == TipoMoneda.Both || this._tipoMoneda == TipoMoneda.Local)
                                imp.BaseML = item.vlrBaseML.Value.Value;
                            if (this._tipoMoneda == TipoMoneda.Both || this._tipoMoneda == TipoMoneda.Foreign)
                                imp.BaseME = item.vlrBaseME.Value.Value;
                        }
                        else
                        {
                            imp.Index = 7;
                            imp.Descriptivo = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_DescOtrosDb");
                            imp.CuentaID = string.Empty;
                            imp.Tarifa = 0;
                        }
                    }
                    #endregion

                    if (this._tipoMoneda == TipoMoneda.Both || this._tipoMoneda == TipoMoneda.Local)
                    {
                        imp.ValorML = item.vlrMdaLoc.Value.Value;
                        imp.ValorME = 0;
                    }
                    if (this._tipoMoneda == TipoMoneda.Both || this._tipoMoneda == TipoMoneda.Foreign)
                    {
                        imp.ValorME = item.vlrMdaExt.Value.Value;
                        imp.ValorML = 0;
                    }
                    impuestos.Add(imp);
                }

                this._resumenImp = impuestos.GroupBy(g => new {g.Index, g.Descriptivo, g.Tarifa, g.CuentaID }).Select(group => new DTO_ResumenImpustos
                {
                    Index = group.Key.Index,
                    Descriptivo = group.Key.Descriptivo,
                    CuentaID = group.Key.CuentaID,
                    BaseML = group.Sum(x => x.BaseML),
                    BaseME = group.Sum(x => x.BaseME),
                    Tarifa = group.Key.Tarifa,
                    ValorML = group.Sum(x => x.ValorML),
                    ValorME = group.Sum(x => x.ValorME)
                }).ToList();

                #region Valor Neto
                imp = new DTO_ResumenImpustos();
                imp.Index = 9;
                imp.Descriptivo = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString() + "_DescNetos");
                imp.CuentaID = string.Empty;
                imp.Tarifa = 0;
                imp.BaseML = this._resumenImp.Sum(x=> x.BaseML);
                imp.BaseME = this._resumenImp.Sum(x=> x.BaseME);
                imp.ValorML = this._resumenImp.Sum(x=> x.ValorML);
                imp.ValorME = this._resumenImp.Sum(x=> x.ValorME);
                this._resumenImp.Add(imp);
                #endregion

                this._resumenImp = this._resumenImp.OrderBy(x => x.Tarifa).ToList();
                this._resumenImp = this._resumenImp.OrderBy(x => x.Index).ToList();
                this.gcData.DataSource = this._resumenImp;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ResumenImpuestos.cs", "LoadGridData"));
            }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvData_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (fieldName == "Valor" || fieldName == "Base")
                e.RepositoryItem = this.editSpin;

            if (fieldName == "Tarifa")
                e.RepositoryItem = this.editSpinPorc;
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvData_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            DTO_ResumenImpustos dto = this._resumenImp.ElementAt(e.ListSourceRowIndex);
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                if (fieldName == "Descriptivo")
                    e.Value = dto.Descriptivo;
                if (fieldName == "CuentaID")
                    e.Value = dto.CuentaID;
                if (fieldName == "Base")
                    e.Value = this._tipoMoneda == TipoMoneda.Local ? dto.BaseML : dto.BaseME;
                if (fieldName == "Tarifa")
                    e.Value = dto.Tarifa;
                if (fieldName == "Valor")
                    e.Value = this._tipoMoneda == TipoMoneda.Local ? dto.ValorML : dto.ValorME;
            }
            if (e.IsSetData)
            {}
        }

        /// <summary>
        /// Devuelve indicador que los impuestos estan aprobados
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnAprobar_Click(object sender, EventArgs e)
        {
            this._aprobado = true;
            this.Close();            
        }

        /// <summary>
        /// Devuelve indicador que los impuestos no estan aprobados
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this._aprobado = false;
            this.Close();    
        }

        /// <summary>
        /// Cambia style de la ultima fila
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvData_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle != -1 && this._resumenImp[e.RowHandle].Index == 9)
            {
                e.Appearance.BackColor = Color.Silver;
                e.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }
        }
        
        #endregion

    }
}
