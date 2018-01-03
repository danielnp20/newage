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

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de consulta maestro (modal)
    /// </summary>
    public partial class ModalConsultaInventario : Form
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();
        private int _documentID = 0;// AppQueries.QueryReferencia;
        private string _unboundPrefix = "Unbound_";
        private DTO_inReferencia dtoReferencia;
        private string param1xDef = string.Empty;
        private string param2xDef = string.Empty;
        private bool _isDetail = true;


        #endregion

        /// <summary>
        /// Constructor de la grilla de facturas 
        /// </summary>
        /// <param name="saldoCosto">Lista de facturas que ya fueron cargados</param>
        /// <param name="periodo">Periodo sobre el cual se van a consultar los facturas</param>
        /// <param name="tm">Tipo de moneda sobre la cual se esta trabajando en el documento</param>
        /// <param name="tasaCambio">Tasa de cambio actual del documento</param>
        public ModalConsultaInventario(string referencia, string bodegaID, bool isDetail, bool consultaUserInd)
        {
            //Inicializa el formulario
            InitializeComponent();
            FormProvider.LoadResources(this, this._documentID);

            _bc.InitMasterUC(this.masterReferencia, AppMasters.inReferencia, true, false, false, true);
            this.masterReferencia.EnableControl(false);
            _bc.InitMasterUC(this.masterBodega, AppMasters.inBodega, true, true, false, true);
            this.masterBodega.EnableControl(false);
            this.LoadGridData(isDetail, referencia, bodegaID, consultaUserInd);
            this._isDetail = isDetail;
        }

        #region Funciones privadas

        /// <summary>
        /// Genera la estructura de la grilla
        /// </summary>
        private void AddGridColsDetalle(bool param1, bool param2, bool estadoInv, bool serial, bool consultaUserInd)
        {
            try
            {
                //Parametro1
                GridColumn Parametro1 = new GridColumn();
                Parametro1.FieldName = this._unboundPrefix + "Parametro1";
                Parametro1.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Parametro1");
                Parametro1.UnboundType = UnboundColumnType.String;
                Parametro1.VisibleIndex = 1;
                Parametro1.Width = 80;
                Parametro1.OptionsColumn.AllowEdit = false;
                Parametro1.Visible = param1 ? true :  false;
                this.gvData.Columns.Add(Parametro1);

                //Parametro1Desc
                GridColumn Parametro1Desc = new GridColumn();
                Parametro1Desc.FieldName = this._unboundPrefix + "Parametro1Desc";
                Parametro1Desc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Parametro1Desc");
                Parametro1Desc.UnboundType = UnboundColumnType.String;
                Parametro1Desc.VisibleIndex = 1;
                Parametro1Desc.Width = 100;
                Parametro1Desc.OptionsColumn.AllowEdit = false;
                Parametro1Desc.Visible = param1 ? true : false;
                this.gvData.Columns.Add(Parametro1Desc);

                //Parametro2
                GridColumn Parametro2 = new GridColumn();
                Parametro2.FieldName = this._unboundPrefix + "Parametro2";
                Parametro2.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Parametro2");
                Parametro2.UnboundType = UnboundColumnType.String;
                Parametro2.VisibleIndex = 2;
                Parametro2.Width = 80;
                Parametro2.OptionsColumn.AllowEdit = false;
                Parametro2.Visible = param2 ? true : false; 
                this.gvData.Columns.Add(Parametro2);

                //Parametro2Desc
                GridColumn Parametro2Desc = new GridColumn();
                Parametro2Desc.FieldName = this._unboundPrefix + "Parametro2Desc";
                Parametro2Desc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Parametro2Desc");
                Parametro2Desc.UnboundType = UnboundColumnType.String;
                Parametro2Desc.VisibleIndex = 2;
                Parametro2Desc.Width = 100;
                Parametro2Desc.OptionsColumn.AllowEdit = false;
                Parametro2Desc.Visible = param2 ? true : false;
                this.gvData.Columns.Add(Parametro2Desc);

                //EstadoInv
                GridColumn EstadoInv = new GridColumn();
                EstadoInv.FieldName = this._unboundPrefix + "EstadoInv";
                EstadoInv.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_EstadoInv");
                EstadoInv.UnboundType = UnboundColumnType.String;
                EstadoInv.VisibleIndex = 3;
                EstadoInv.Width = 100;
                EstadoInv.OptionsColumn.AllowEdit = false;
                EstadoInv.Visible = estadoInv ? true : false; 
                this.gvData.Columns.Add(EstadoInv);

                //Serial
                GridColumn Serial = new GridColumn();
                Serial.FieldName = this._unboundPrefix + "SerialID";
                Serial.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_SerialID");
                Serial.UnboundType = UnboundColumnType.String;
                Serial.VisibleIndex = 4;
                Serial.Width = 100;
                Serial.OptionsColumn.AllowEdit = false;
                Serial.Visible = serial ? true : false;
                this.gvData.Columns.Add(Serial);

                //CantidadUNI
                GridColumn CantidadUNI = new GridColumn();
                CantidadUNI.FieldName = this._unboundPrefix + "CantidadDisp";
                CantidadUNI.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CantidadUNI");
                CantidadUNI.UnboundType = UnboundColumnType.Decimal;
                CantidadUNI.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                CantidadUNI.AppearanceCell.Options.UseTextOptions = true;
                CantidadUNI.VisibleIndex = 5;
                CantidadUNI.Width = 80;
                CantidadUNI.Visible = true;
                CantidadUNI.OptionsColumn.AllowEdit = false;
                CantidadUNI.ColumnEdit = this.editCant2;
                this.gvData.Columns.Add(CantidadUNI);

                //ValorLocal
                GridColumn ValorLocal = new GridColumn();
                ValorLocal.FieldName = this._unboundPrefix + "ValorLocalDisp";
                ValorLocal.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ValorLocal");
                ValorLocal.UnboundType = UnboundColumnType.Decimal;
                ValorLocal.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                ValorLocal.AppearanceCell.Options.UseTextOptions = true;
                ValorLocal.VisibleIndex = 6;
                ValorLocal.Width = 100;
                ValorLocal.ColumnEdit = this.editSpin;
                ValorLocal.OptionsColumn.AllowEdit = false;
                ValorLocal.Visible = consultaUserInd ? true: false;
                this.gvData.Columns.Add(ValorLocal);

                //ValorExtranjero
                GridColumn ValorExtranjero = new GridColumn();
                ValorExtranjero.FieldName = this._unboundPrefix + "ValorExtranjeroDisp";
                ValorExtranjero.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ValorExtranjero");
                ValorExtranjero.UnboundType = UnboundColumnType.Decimal;
                ValorExtranjero.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                ValorExtranjero.AppearanceCell.Options.UseTextOptions = true;
                ValorExtranjero.VisibleIndex = 7;
                ValorExtranjero.Width = 100;
                ValorExtranjero.ColumnEdit = this.editSpin;
                ValorExtranjero.OptionsColumn.AllowEdit = false;
                ValorExtranjero.Visible = consultaUserInd ? true : false;
                this.gvData.Columns.Add(ValorExtranjero);


                this.lblTitle.Text = _bc.GetResource(LanguageTypes.Forms, "26311_lblDetail");
                this.gvData.OptionsView.ColumnAutoWidth = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalConsultaInventario.cs", "LoadGridStructure"));
            }
        }

        /// <summary>
        /// Genera la estructura de la grilla
        /// </summary>
        private void AddGridColsMvto(bool consultaUserInd)
        {
            try
            {
                #region Movimiento

                //Prefijo_Documento
                GridColumn Prefijo_Documento = new GridColumn();
                Prefijo_Documento.FieldName = this._unboundPrefix + "Prefijo_Documento";
                Prefijo_Documento.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Prefijo_Documento");
                Prefijo_Documento.UnboundType = UnboundColumnType.String;
                Prefijo_Documento.VisibleIndex = 1;
                Prefijo_Documento.Width = 80;
                Prefijo_Documento.Visible = true;
                Prefijo_Documento.OptionsColumn.AllowEdit = true;
                Prefijo_Documento.ColumnEdit = this.editLink;
                this.gvData.Columns.Add(Prefijo_Documento);

                //Prefijo_Documento
                GridColumn NumeroDoc = new GridColumn();
                NumeroDoc.FieldName = this._unboundPrefix + "NumeroDoc";
                NumeroDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_NumeroDoc");
                NumeroDoc.UnboundType = UnboundColumnType.Integer;
                NumeroDoc.VisibleIndex = 1;
                NumeroDoc.Width = 80;
                NumeroDoc.Visible = true;
                NumeroDoc.OptionsColumn.AllowEdit = true;
                this.gvData.Columns.Add(NumeroDoc);

                //Fecha
                GridColumn Fecha = new GridColumn();
                Fecha.FieldName = this._unboundPrefix + "Fecha";
                Fecha.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Fecha");
                Fecha.UnboundType = UnboundColumnType.String;
                Fecha.VisibleIndex = 2;
                Fecha.Width = 80;
                Fecha.Visible = true;
                Fecha.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(Fecha);

                //MvtoTipoInvID
                GridColumn MvtoTipoInvID = new GridColumn();
                MvtoTipoInvID.FieldName = this._unboundPrefix + "MvtoTipoInvID";
                MvtoTipoInvID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_TipoMovimiento");
                MvtoTipoInvID.UnboundType = UnboundColumnType.String;
                MvtoTipoInvID.VisibleIndex = 3;
                MvtoTipoInvID.Width = 60;
                MvtoTipoInvID.Visible = true;
                MvtoTipoInvID.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(MvtoTipoInvID);

                //EntradaSalidaLetras
                GridColumn EntradaSalidaLetras = new GridColumn();
                EntradaSalidaLetras.FieldName = this._unboundPrefix + "EntradaSalidaLetras";
                EntradaSalidaLetras.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_EntradaSalida");
                EntradaSalidaLetras.UnboundType = UnboundColumnType.String;
                EntradaSalidaLetras.VisibleIndex = 4;
                EntradaSalidaLetras.Width = 30;
                EntradaSalidaLetras.Visible = true;
                EntradaSalidaLetras.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(EntradaSalidaLetras);

                //Parametro1
                GridColumn Parametro1 = new GridColumn();
                Parametro1.FieldName = this._unboundPrefix + "Parametro1";
                Parametro1.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Parametro1");
                Parametro1.UnboundType = UnboundColumnType.String;
                Parametro1.VisibleIndex = 5;
                Parametro1.Width = 70;
                Parametro1.Visible = true;
                Parametro1.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(Parametro1);

                //Parametro2
                GridColumn Parametro2 = new GridColumn();
                Parametro2.FieldName = this._unboundPrefix + "Parametro2";
                Parametro2.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Parametro2");
                Parametro2.UnboundType = UnboundColumnType.String;
                Parametro2.VisibleIndex = 7;
                Parametro2.Width = 80;
                Parametro2.Visible = true;
                Parametro2.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(Parametro2);

                //EstadoInv
                GridColumn EstadoInv = new GridColumn();
                EstadoInv.FieldName = this._unboundPrefix + "EstadoInv";
                EstadoInv.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_EstadoInv");
                EstadoInv.UnboundType = UnboundColumnType.String;
                EstadoInv.VisibleIndex = 9;
                EstadoInv.Width = 70;
                EstadoInv.Visible = true;
                EstadoInv.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(EstadoInv);

                //Serial
                GridColumn identificadorTr = new GridColumn();
                identificadorTr.FieldName = this._unboundPrefix + "IdentificadorTr";
                identificadorTr.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_IdentificadorTr");
                identificadorTr.UnboundType = UnboundColumnType.Integer;
                identificadorTr.VisibleIndex = 10;
                identificadorTr.Width = 80;
                identificadorTr.Visible = true;
                identificadorTr.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(identificadorTr);

                //Serial
                GridColumn Serial = new GridColumn();
                Serial.FieldName = this._unboundPrefix + "SerialID";
                Serial.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_SerialID");
                Serial.UnboundType = UnboundColumnType.String;
                Serial.VisibleIndex = 11;
                Serial.Width = 80;
                Serial.Visible = true;
                Serial.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(Serial);

                //DocSoporte
                GridColumn DocSoporte = new GridColumn();
                DocSoporte.FieldName = this._unboundPrefix + "DocSoporte";
                DocSoporte.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_DocSoporte");
                DocSoporte.UnboundType = UnboundColumnType.Integer;
                DocSoporte.VisibleIndex = 12;
                DocSoporte.Width = 80;
                DocSoporte.Visible = true;
                DocSoporte.OptionsColumn.AllowEdit = false;
                this.gvData.Columns.Add(DocSoporte);

                //CantidadUNI
                GridColumn CantidadUNI = new GridColumn();
                CantidadUNI.FieldName = this._unboundPrefix + "CantidadUNI";
                CantidadUNI.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CantidadUNI");
                CantidadUNI.UnboundType = UnboundColumnType.Integer;
                CantidadUNI.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                CantidadUNI.AppearanceCell.Options.UseTextOptions = true;
                CantidadUNI.VisibleIndex = 13;
                CantidadUNI.Width = 50;
                CantidadUNI.Visible = true;
                CantidadUNI.OptionsColumn.AllowEdit = false;
                CantidadUNI.ColumnEdit = this.editCant2;
                this.gvData.Columns.Add(CantidadUNI);

                //ValorUNI
                GridColumn ValorUNI = new GridColumn();
                ValorUNI.FieldName = this._unboundPrefix + "ValorUNI";
                ValorUNI.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ValorUNI");
                ValorUNI.UnboundType = UnboundColumnType.Decimal;
                ValorUNI.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                ValorUNI.AppearanceCell.Options.UseTextOptions = true;
                ValorUNI.VisibleIndex = 14;
                ValorUNI.Width = 80;
                ValorUNI.ColumnEdit = this.editSpin;
                ValorUNI.OptionsColumn.AllowEdit = false;
                ValorUNI.Visible = consultaUserInd ? true: false;
                this.gvData.Columns.Add(ValorUNI);

                //Valor1LOC+FobLocal
                GridColumn Valor1LOC = new GridColumn();
                Valor1LOC.FieldName = this._unboundPrefix + "Valor1LOC";
                Valor1LOC.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Valor1LOC");
                Valor1LOC.UnboundType = UnboundColumnType.Decimal;
                Valor1LOC.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                Valor1LOC.AppearanceCell.Options.UseTextOptions = true;
                Valor1LOC.VisibleIndex = 15;
                Valor1LOC.Width = 80;
                Valor1LOC.ColumnEdit = this.editSpin;
                Valor1LOC.OptionsColumn.AllowEdit = false;
                Valor1LOC.Visible = consultaUserInd ? true : false;
                this.gvData.Columns.Add(Valor1LOC);

                //Valor1EXT+FobExt
                GridColumn Valor1EXT = new GridColumn();
                Valor1EXT.FieldName = this._unboundPrefix + "Valor1EXT";
                Valor1EXT.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Valor1EXT");
                Valor1EXT.UnboundType = UnboundColumnType.Decimal;
                Valor1EXT.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                Valor1EXT.AppearanceCell.Options.UseTextOptions = true;
                Valor1EXT.VisibleIndex = 16;
                Valor1EXT.Width = 80;
                Valor1EXT.ColumnEdit = this.editSpin;
                Valor1EXT.OptionsColumn.AllowEdit = false;
                Valor1EXT.Visible = consultaUserInd ? true : false;
                this.gvData.Columns.Add(Valor1EXT);

                this.lblTitle.Text = _bc.GetResource(LanguageTypes.Forms, "26311_lblMov");

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalConsultaInventario.cs", "AddGridColsMvto"));
            }
        }

        /// <summary>
        /// Carga los datos de la grilla
        /// </summary>
        private void LoadGridData(bool isDetalle, string referencia, string BodegaID, bool consultaUserInd)
        {
            try
            {
                this.masterReferencia.Value = referencia;
                this.masterBodega.Value = BodegaID;
                this.param1xDef = _bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Parametro1xDefecto);
                this.param2xDef = _bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Parametro2xDefecto);

                this.dtoReferencia = (DTO_inReferencia)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, false, referencia, true);
                this.txtDescripcion.Text = dtoReferencia.Descriptivo.Value;

                if (isDetalle)
                {
                    decimal cantidadDisp = 0;
                    DTO_inControlSaldosCostos listSaldosCostos = new DTO_inControlSaldosCostos();
                    DTO_inCostosExistencias costos = new DTO_inCostosExistencias();
                    List<DTO_inControlSaldosCostos> result = new List<DTO_inControlSaldosCostos>();
                    #region Trae los saldos de la referencia por bodega
                    listSaldosCostos.BodegaID.Value = BodegaID;
                    listSaldosCostos.inReferenciaID.Value = referencia; 
                    foreach (var item in result)
                    {
                        if (item.Parametro1.Value != param1xDef.ToUpper())
                        {
                            DTO_MasterBasic param1 = (DTO_MasterBasic)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inRefParametro1, false, item.Parametro1.Value, true);
                            item.Parametro1Desc.Value = param1.Descriptivo.Value;
                            this.chkParam1.Checked = true;
                            this.chkParam1.Enabled = true;
                        }
                        if (item.Parametro2.Value != param2xDef.ToUpper())
                        {
                            DTO_MasterBasic param2 = (DTO_MasterBasic)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inRefParametro2, false, item.Parametro2.Value, true);
                            item.Parametro2Desc.Value = param2.Descriptivo.Value;
                            this.chkParam2.Checked = true;
                            this.chkParam2.Enabled = true;
                        }
                        if (item.EstadoInv.Value.Value != (byte)EstadoInv.Activo)
                        {
                            this.chkEstadoInv.Checked = true;
                            this.chkEstadoInv.Enabled = true;
                        }
                        if (item.ActivoID.Value.Value != 0)
                        {
                            this.chkSerializado.Checked = true;
                            this.chkSerializado.Enabled = true;
                        }
                    }

                    this.AddGridColsDetalle(this.chkParam1.Checked, this.chkParam2.Checked, this.chkEstadoInv.Checked, this.chkSerializado.Checked, consultaUserInd);
                    this.gcData.DataSource = result;
                    this.pnlParam.Visible = true;
                }
                else
                {
                    DTO_glMovimientoDeta mvtoDeta = new DTO_glMovimientoDeta();
                    List<DTO_glMovimientoDeta> result = new List<DTO_glMovimientoDeta>();

                    mvtoDeta.BodegaID.Value = BodegaID;
                    mvtoDeta.inReferenciaID.Value = referencia;
                    result = _bc.AdministrationModel.glMovimientoDeta_GetByParameter(mvtoDeta, false);
                    result = result.FindAll(x => x.DocumentoID.Value != 90051 && x.DocumentoID.Value != 90056).ToList();
                    this.AddGridColsMvto(consultaUserInd);
                    DTO_inBodega dtoBodega = (DTO_inBodega)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, false, BodegaID, true);
                    DTO_inCosteoGrupo dtoCosteo = dtoBodega != null ? (DTO_inCosteoGrupo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inCosteoGrupo, false, dtoBodega.CosteoGrupoInvID.Value, true) : null;
                    if (dtoCosteo.CosteoTipo.Value != (byte)TipoCosteoInv.Transaccional)
                        this.gvData.Columns[this._unboundPrefix + "IdentificadorTr"].Visible = false;
                    this.gcData.DataSource = result;
                }
                #endregion                    
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalConsultaInventario.cs", "LoadGridData"));
            }
        }
        #endregion

        #region Eventos

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvData_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

            if (e.IsGetData)
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

        /// <summary>
        /// Asigna texto por defecto para la columna de archivos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void gvData_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);
            //if (fieldName == "Prefijo_Documento")
            //    e.DisplayText = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ViewDocument);
        }
        
        /// <summary>
        /// Al entrar al link de la grilla para ver la descripcin de documento
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void editLink_Click(object sender, EventArgs e)
        {
            try
            {
               if (!this._isDetail)
	            {
		            int fila = this.gvData.FocusedRowHandle;

                    DTO_glDocumentoControl ctrl = new DTO_glDocumentoControl();
                    DTO_Comprobante comprobante = new DTO_Comprobante();

                    DTO_glMovimientoDeta det = (DTO_glMovimientoDeta)this.gvData.GetRow(this.gvData.FocusedRowHandle);
                    ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(det.NumeroDoc.Value.Value);
                    comprobante = !string.IsNullOrEmpty(ctrl.ComprobanteID.Value) ? this._bc.AdministrationModel.Comprobante_Get(true, false, ctrl.PeriodoDoc.Value.Value, ctrl.ComprobanteID.Value, ctrl.ComprobanteIDNro.Value.Value, null, null, null) : null;

                    ShowDocumentForm documentForm = new ShowDocumentForm(ctrl, comprobante);
                    documentForm.Show(); 
	            }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MOdalConsutaInventario.cs", "editLink_Click"));
            }
        }

        #endregion

    }
}
