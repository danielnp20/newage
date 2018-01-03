using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraRichEdit.API.Word;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using System.Reflection;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using System.Threading;
using System.Text.RegularExpressions;
using DevExpress.XtraEditors;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class LiquidacionImportacion : DocumentForm
    {
        #region Variables Formulario

        //Obtiene la instancia del controlador
        private BaseController _bc = BaseController.GetInstance();
        private DTO_glDocumentoControl _ctrl = null;
        private DTO_inCostosExistencias _costos;
        private DTO_LiquidacionImportacion dataImportacion = null;
        private List<DTO_inImportacionDeta> footerImportacion = null;
        private List<DTO_inDistribucionCosto> dataFactura = null;
        //private DTO_MvtoInventarios data = null;
        private string monedaLocal;
        private string monedaExtranjera;
        //Indica si el header es valido
        private bool validHeader;
        //variables para funciones particulares
        private string _terceroID = string.Empty;
        private bool cleanDoc = true;

        private decimal _tasaCambio = 0;
        private string param1xDef = string.Empty;
        private string param2xDef = string.Empty;

        #endregion
        
        #region Delegados
        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected override void SaveMethod()
        {
            this.gcDetail.DataSource = this.dataFactura;
            this.gcDocument.DataSource = this.dataImportacion.Footer;
            this.newDoc = true;
            this.CleanData();
        } 
        #endregion

        #region Propiedades

        ///// <summary>
        ///// Comprobante sobre el cual se esta trabajando
        ///// </summary>
        //private DTO_MvtoInventarios _data = null;


        //Numero de una fila segun el indice
        protected int NumFila
        {
            get
            {
                return this.dataImportacion.Footer.FindIndex(det => det.Movimiento.Index == this.indexFila);
            }
        }

        #endregion

        public LiquidacionImportacion()
        {
           //this.InitializeComponent();
        }

        #region Funciones Privadas

        /// <summary>
        /// Limpia el formulario
        /// </summary>
        private void CleanData()
        {
            this._ctrl = new DTO_glDocumentoControl();
            this._costos = new DTO_inCostosExistencias();
            this.dataFactura = new List<DTO_inDistribucionCosto>();
            this._terceroID = string.Empty;
            this.cleanDoc = true;
            this.validHeader = false;

            this.txtDocTransporte.Text = string.Empty;
            this.txtNro.Text = string.Empty;           
            this.txtDeclarImportacion.Text = string.Empty;
            this.txtDocImportadora.Text = string.Empty;
            this.txtDocMvtoZonaFranca.Text = string.Empty;
            this.masterAgenteAduanaProv.Value = string.Empty;
            this.txtObservacion.Text = string.Empty;

            this.txtDocTransporte.Enabled = true;
            this.txtNro.Enabled = true;
            this.txtDeclarImportacion.Enabled = true;
            this.txtDocImportadora.Enabled = true;
            this.txtDocMvtoZonaFranca.Enabled = true;
            this.masterAgenteAduanaProv.Enabled = true;
            this.masterAgenteAduanaProv.Enabled = true;
            this.txtObservacion.Enabled = true;  
            this.txtTasaImport.EditValue = 0;
            this.btnQueryDoc.Enabled = true;

            this.gcDocument.DataSource = null;
            this.gcDetail.DataSource = null;

            this.txtDocTransporte.Focus();
        }

        /// <summary>
        /// Revisa si una grilla es valida o no
        /// </summary>
        /// <returns></returns>
        private bool ValidGrid()
        {            
            if (this.dataFactura != null && this.dataFactura.Count == 0)
            {
                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoData));
                return false;
            }

            if (!this.ValidateRow(this.gvDocument.FocusedRowHandle))
                return false;

            return true;
        }

        /// <summary>
        /// Carga el valor de la tasa de cambio de acuerdo a los items seleccionados
        /// </summary>
        /// <param name="monOr">Moneda Origen</param>
        /// <returns>Retorna el valor de la tasa de cambio</returns>
        private decimal LoadTasaCambio(DateTime fecha)
        {
            try
            {
                this._tasaCambio = this._bc.AdministrationModel.TasaDeCambio_Get(this.monedaExtranjera, fecha);
                return _tasaCambio;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-LiquidacionImportacion.cs", "LoadTasaCambio"));
                return _tasaCambio;
            }
        }

        /// <summary>
        /// valida la informacion del header
        /// </summary>
        private bool ValidateHeader()
        {
            bool result = true;

            if (!this.masterAgenteAduanaProv.ValidID)
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterAgenteAduanaProv.CodeRsx);

                MessageBox.Show(msg);
                this.masterAgenteAduanaProv.Focus();

                result = false;
            }
            //if (string.IsNullOrEmpty(this.txtNro.Text))
            //{
            //    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.lblNroDocInv.Text);

            //    MessageBox.Show(msg);
            //    this.txtNro.Focus();

            //    result = false;
            //}
            return result;
        }

        /// <summary>
        /// Carga informacion de un temporal a partir del cabezote
        /// </summary>
        /// <returns>Retorna el header de un temporal</returns>
        private DTO_LiquidacionImportacion LoadHeader()
        {
            try
            {
                DTO_LiquidacionImportacion liquidacionLoad = new DTO_LiquidacionImportacion();
                DTO_inImportacionDocu header = new DTO_inImportacionDocu();
                this._ctrl.DocumentoID.Value = this.documentID;
                this._ctrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                this._ctrl.EmpresaID.Value = this.empresaID;
                this._ctrl.TerceroID.Value = string.Empty;
                this._ctrl.PrefijoID.Value = base.prefijoID;
                this._ctrl.NumeroDoc.Value = Convert.ToInt32(this.txtNumeroDoc.Text);
                this._ctrl.ComprobanteID.Value = string.Empty; 
                this._ctrl.ComprobanteIDNro.Value = 0;
                this._ctrl.MonedaID.Value = string.Empty; 
                this._ctrl.CuentaID.Value = string.Empty; 
                this._ctrl.ProyectoID.Value = string.Empty; 
                this._ctrl.CentroCostoID.Value = string.Empty; 
                this._ctrl.LugarGeograficoID.Value = string.Empty;
                this._ctrl.LineaPresupuestoID.Value = string.Empty; 
                this._ctrl.Fecha.Value = DateTime.Now;
                this._ctrl.PeriodoDoc.Value = this.dtPeriod.DateTime;
                this._ctrl.TasaCambioCONT.Value = Convert.ToDecimal(this.txtTasaImport.EditValue, CultureInfo.InvariantCulture);
                this._ctrl.TasaCambioDOCU.Value = Convert.ToDecimal(this.txtTasaImport.EditValue, CultureInfo.InvariantCulture);
                this._ctrl.DocumentoNro.Value = 0;           
                this._ctrl.PeriodoUltMov.Value = this.dtPeriod.DateTime;
                this._ctrl.seUsuarioID.Value = this.userID;
                this._ctrl.AreaFuncionalID.Value = this.areaFuncionalID;
                this._ctrl.ConsSaldo.Value = 0;
                this._ctrl.Estado.Value = (byte)EstadoDocControl.SinAprobar;
                this._ctrl.FechaDoc.Value = this.dtFecha.DateTime;
                this._ctrl.Descripcion.Value = this.txtDocDesc.Text;
                this._ctrl.Valor.Value = 0;
                this._ctrl.Iva.Value = 0;
                this._ctrl.seUsuarioID.Value = this.userID;

                header.EmpresaID.Value = this.empresaID;
                header.TasaImport.Value = Convert.ToDecimal(this.txtTasaImport.EditValue, CultureInfo.InvariantCulture);
                header.TipoTransporte.Value = Convert.ToByte(this.cmbTipoTransporte.EditValue);           
                header.DocTransporte.Value = this.txtDocTransporte.Text;
                header.DeclaracionImp.Value = this.txtDeclarImportacion.Text;
                header.DocImportadora.Value = this.txtDocImportadora.Text;
                header.DocMvtoZonaFranca.Value = this.txtDocMvtoZonaFranca.Text;
                header.AgenteAduanaID.Value = this.masterAgenteAduanaProv.Value;
                header.ModalidadImp.Value = Convert.ToByte(this.cmbModalidad.EditValue);

                liquidacionLoad.DocCtrl = this._ctrl;
                liquidacionLoad.Header = header;
                liquidacionLoad.Footer = this.footerImportacion;

                return liquidacionLoad;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-TransaccionManual.cs", "LoadTempHeader: " + ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Carga la información de las grilla de facturas distribuidas
        /// </summary>
        /// <param name="firstTime">Si es la primera vez que carga la grilla</param>
        /// <param name="refresh">Si debe volver a cargar la data del servidor</param>
        private void LoadDataFactura()
        {
            this.gcDetail.DataSource = this.dataFactura;
            this.gcDetail.RefreshDataSource();
            bool hasItems = this.dataFactura.GetEnumerator().MoveNext() ? true : false;
            if (hasItems)
                this.gvDetail.MoveFirst();
        }

        /// <summary>
        /// Agrega las columnas a la grilla principal del documento
        /// </summary>
        private void AddFacturaCols()
        {
            try
            {
                //FacturaNro
                GridColumn FacturaNro = new GridColumn();
                FacturaNro.FieldName = this.unboundPrefix + "FacturaNro";
                FacturaNro.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FacturaNro");
                FacturaNro.UnboundType = UnboundColumnType.String;
                FacturaNro.VisibleIndex = 0;
                FacturaNro.Width = 80;
                FacturaNro.Visible = true;
                FacturaNro.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                FacturaNro.OptionsColumn.AllowEdit = false;
                this.gvDetail.Columns.Add(FacturaNro);

                //ProveedorID
                GridColumn ProveedorID = new GridColumn();
                ProveedorID.FieldName = this.unboundPrefix + "ProveedorID";
                ProveedorID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProveedorID");
                ProveedorID.UnboundType = UnboundColumnType.String;
                ProveedorID.VisibleIndex = 1;
                ProveedorID.Width = 80;
                ProveedorID.Visible = true;
                ProveedorID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                FacturaNro.OptionsColumn.AllowEdit = false;
                this.gvDetail.Columns.Add(ProveedorID);

                //ProveedorDesc
                GridColumn ProveedorDesc = new GridColumn();
                ProveedorDesc.FieldName = this.unboundPrefix + "ProveedorDesc";
                ProveedorDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProveedorDesc");
                ProveedorDesc.UnboundType = UnboundColumnType.String;
                ProveedorDesc.VisibleIndex = 2;
                ProveedorDesc.Width = 80;
                ProveedorDesc.Visible = true;
                ProveedorDesc.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                ProveedorDesc.OptionsColumn.AllowEdit = false;
                this.gvDetail.Columns.Add(ProveedorDesc);

                //Observacion
                GridColumn Observacion = new GridColumn();
                Observacion.FieldName = this.unboundPrefix + "Observacion";
                Observacion.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Observacion");
                Observacion.UnboundType = UnboundColumnType.String;
                Observacion.VisibleIndex = 3;
                Observacion.Width = 80;
                Observacion.Visible = true;
                Observacion.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                FacturaNro.OptionsColumn.AllowEdit = false;
                this.gvDetail.Columns.Add(Observacion);

                //FechaFactura
                GridColumn FechaFactura = new GridColumn();
                FechaFactura.FieldName = this.unboundPrefix + "FechaFactura";
                FechaFactura.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaFactura");
                FechaFactura.UnboundType = UnboundColumnType.Integer;
                FechaFactura.VisibleIndex = 4;
                FechaFactura.Width = 100;
                FechaFactura.Visible = true;
                FechaFactura.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                FechaFactura.OptionsColumn.AllowEdit = false;
                FechaFactura.ColumnEdit = this.editDate;
                this.gvDetail.Columns.Add(FechaFactura);

                //ValorME
                GridColumn ValorME = new GridColumn();
                ValorME.FieldName = this.unboundPrefix + "ValorME";
                ValorME.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorME");
                ValorME.UnboundType = UnboundColumnType.Decimal;
                ValorME.VisibleIndex = 5;
                ValorME.Width = 200;
                ValorME.Visible = true;
                ValorME.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                ValorME.OptionsColumn.AllowEdit = false;
                ValorME.ColumnEdit = this.editSpin;
                this.gvDetail.Columns.Add(ValorME);

                //ValorML
                GridColumn ValorML = new GridColumn();
                ValorML.FieldName = this.unboundPrefix + "ValorML";
                ValorML.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorML");
                ValorML.UnboundType = UnboundColumnType.Decimal;
                ValorML.VisibleIndex = 6;
                ValorML.Width = 200;
                ValorML.Visible = true;
                ValorML.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                ValorML.OptionsColumn.AllowEdit = false;
                ValorML.ColumnEdit = this.editSpin;
                this.gvDetail.Columns.Add(ValorML);

                this.gvDetail.OptionsView.ColumnAutoWidth = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "LiquidacionImportacion.cs-AddDocumentCols"));
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla principal del documento
        /// </summary>
        private void AddDocumentCols()
        {
            try
            {
                #region Columnas visibles

                //FacturaNro
                GridColumn FacturaNro = new GridColumn();
                FacturaNro.FieldName = this.unboundPrefix + "FacturaNro";
                FacturaNro.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_FacturaProveedorNro");
                FacturaNro.UnboundType = UnboundColumnType.String;
                FacturaNro.VisibleIndex = 1;
                FacturaNro.Width = 80;
                FacturaNro.Visible = true;
                this.gvDocument.Columns.Add(FacturaNro);

                //CodigoReferencia+Param1+Param2
                GridColumn codRefP1P2 = new GridColumn();
                codRefP1P2.FieldName = this.unboundPrefix + "ReferenciaIDP1P2";
                codRefP1P2.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_inReferenciaID");
                codRefP1P2.UnboundType = UnboundColumnType.String;
                codRefP1P2.VisibleIndex = 2;
                codRefP1P2.Width = 80;
                codRefP1P2.Visible = true;
                this.gvDocument.Columns.Add(codRefP1P2);

                //Descripcion
                GridColumn desc = new GridColumn();
                desc.FieldName = this.unboundPrefix + "ReferenciaIDP1P2Desc";
                desc.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_DescripTExt");
                desc.UnboundType = UnboundColumnType.String;
                desc.VisibleIndex = 3;
                desc.Width = 120;
                desc.Visible = true;
                this.gvDocument.Columns.Add(desc);

                //Serial
                GridColumn serial = new GridColumn();
                serial.FieldName = this.unboundPrefix + "SerialID";
                serial.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_SerialID");
                serial.UnboundType = UnboundColumnType.String;
                serial.VisibleIndex = 4;
                serial.Width = 80;
                serial.Visible = true;
                this.gvDocument.Columns.Add(serial);

                //EmpaqueInvID
                GridColumn empaqueID = new GridColumn();
                empaqueID.FieldName = this.unboundPrefix + "EmpaqueInvID";
                empaqueID.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_EmpaqueInvID");
                empaqueID.UnboundType = UnboundColumnType.String;
                empaqueID.VisibleIndex = 5;
                empaqueID.Width = 70;
                empaqueID.Visible = true;
                this.gvDocument.Columns.Add(empaqueID);

                //CantidadEmpaques
                GridColumn cantidadEmpaques = new GridColumn();
                cantidadEmpaques.FieldName = this.unboundPrefix + "Cantidad";
                cantidadEmpaques.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_CantidadEMP");
                cantidadEmpaques.UnboundType = UnboundColumnType.Integer;
                cantidadEmpaques.VisibleIndex = 6;
                cantidadEmpaques.Width = 60;
                cantidadEmpaques.Visible = true;
                this.gvDocument.Columns.Add(cantidadEmpaques);

                //ValorUnidadUS
                GridColumn ValorUnidadUS = new GridColumn();
                ValorUnidadUS.FieldName = this.unboundPrefix + "ValorUNI";
                ValorUnidadUS.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorUNI");
                ValorUnidadUS.UnboundType = UnboundColumnType.Decimal;
                ValorUnidadUS.VisibleIndex = 7;
                ValorUnidadUS.Width = 80;
                ValorUnidadUS.ColumnEdit = this.editSpin;
                ValorUnidadUS.Visible = true;
                this.gvDocument.Columns.Add(ValorUnidadUS);

                //ValorCostoUS(valorFOB)
                GridColumn ValorCostoUS = new GridColumn();
                ValorCostoUS.FieldName = this.unboundPrefix + "ValorCostoUS";
                ValorCostoUS.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorCostoUS");
                ValorCostoUS.UnboundType = UnboundColumnType.Decimal;
                ValorCostoUS.VisibleIndex = 8;
                ValorCostoUS.Width = 120;
                ValorCostoUS.ColumnEdit = this.editSpin;
                ValorCostoUS.Visible = true;
                this.gvDocument.Columns.Add(ValorCostoUS);

                //ValorOtros(ValorFacturas)
                GridColumn ValorOtros = new GridColumn();
                ValorOtros.FieldName = this.unboundPrefix + "ValorOtrosUS";
                ValorOtros.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorOtrosUS");
                ValorOtros.UnboundType = UnboundColumnType.Decimal;
                ValorOtros.VisibleIndex = 9;
                ValorOtros.Width = 100;
                ValorOtros.ColumnEdit = this.editSpin;
                ValorOtros.Visible = true;
                this.gvDocument.Columns.Add(ValorOtros);

                //ValorTotalUS (ValorCostoUS + ValorOtros)
                GridColumn ValorTotalUS = new GridColumn();
                ValorTotalUS.FieldName = this.unboundPrefix + "ValorTotalUS";
                ValorTotalUS.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorTotalUS");
                ValorTotalUS.UnboundType = UnboundColumnType.Decimal;
                ValorTotalUS.VisibleIndex = 10;
                ValorTotalUS.Width = 120;
                ValorTotalUS.ColumnEdit = this.editSpin;
                ValorTotalUS.Visible = true;
                this.gvDocument.Columns.Add(ValorTotalUS);

                //ValorTotalPS
                GridColumn ValorTotalPS = new GridColumn();
                ValorTotalPS.FieldName = this.unboundPrefix + "ValorTotalPS";
                ValorTotalPS.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorTotalPS");
                ValorTotalPS.UnboundType = UnboundColumnType.Decimal;
                ValorTotalPS.VisibleIndex = 11;
                ValorTotalPS.Width = 120;
                ValorTotalPS.ColumnEdit = this.editSpin;
                ValorTotalPS.Visible = true;
                this.gvDocument.Columns.Add(ValorTotalPS);

                //PorArancel
                GridColumn PorArancel = new GridColumn();
                PorArancel.FieldName = this.unboundPrefix + "PorArancel";
                PorArancel.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_PorArancel");
                PorArancel.UnboundType = UnboundColumnType.Decimal;
                PorArancel.VisibleIndex = 12;
                PorArancel.Width = 50;
                PorArancel.Visible = true;
                PorArancel.ColumnEdit = this.editSpinPorcen;
                this.gvDocument.Columns.Add(PorArancel);

                //PorIVA
                GridColumn PorIVA = new GridColumn();
                PorIVA.FieldName = this.unboundPrefix + "PorIVA";
                PorIVA.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_PorIVA");
                PorIVA.UnboundType = UnboundColumnType.Decimal;
                PorIVA.VisibleIndex = 13;
                PorIVA.Width = 50;
                PorIVA.Visible = true;
                PorIVA.ColumnEdit = this.editSpinPorcen;
                this.gvDocument.Columns.Add(PorIVA);

                //ValorArancel
                GridColumn ValorArancel = new GridColumn();
                ValorArancel.FieldName = this.unboundPrefix + "ValorArancel";
                ValorArancel.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorArancel");
                ValorArancel.UnboundType = UnboundColumnType.Decimal;
                ValorArancel.VisibleIndex = 14;
                ValorArancel.Width = 90;
                ValorArancel.ColumnEdit = this.editSpin;
                ValorArancel.Visible = true;
                this.gvDocument.Columns.Add(ValorArancel);

                //ValorIVA
                GridColumn ValorIVA = new GridColumn();
                ValorIVA.FieldName = this.unboundPrefix + "ValorIVA";
                ValorIVA.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorIVA");
                ValorIVA.UnboundType = UnboundColumnType.Decimal;
                ValorIVA.VisibleIndex = 15;
                ValorIVA.Width = 90;
                ValorIVA.ColumnEdit = this.editSpin;
                ValorIVA.Visible = true;
                this.gvDocument.Columns.Add(ValorIVA);

                //ValorOtrosPS (ValorArancel + ValorIVA)
                GridColumn ValorOtrosPS = new GridColumn();
                ValorOtrosPS.FieldName = this.unboundPrefix + "ValorOtrosPS";
                ValorOtrosPS.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorOtrosPS");
                ValorOtrosPS.UnboundType = UnboundColumnType.Decimal;
                ValorOtrosPS.VisibleIndex = 16;
                ValorOtrosPS.Width = 100;
                ValorOtrosPS.ColumnEdit = this.editSpin;
                ValorOtrosPS.Visible = true;
                this.gvDocument.Columns.Add(ValorOtrosPS);

                //CostoTotalUS(ValorTotalUS+ValorOtrosPS)
                GridColumn CostoTotalUS = new GridColumn();
                CostoTotalUS.FieldName = this.unboundPrefix + "CostoTotalUS";
                CostoTotalUS.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_CostoTotalUS");
                CostoTotalUS.UnboundType = UnboundColumnType.Decimal;
                CostoTotalUS.VisibleIndex = 17;
                CostoTotalUS.Width = 120;
                CostoTotalUS.ColumnEdit = this.editSpin;
                CostoTotalUS.Visible = true;
                this.gvDocument.Columns.Add(CostoTotalUS);

                //CostoTotalPS
                GridColumn CostoTotalPS = new GridColumn();
                CostoTotalPS.FieldName = this.unboundPrefix + "CostoTotalPS";
                CostoTotalPS.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_CostoTotalPS");
                CostoTotalPS.UnboundType = UnboundColumnType.Decimal;
                CostoTotalPS.VisibleIndex = 18;
                CostoTotalPS.Width = 120;
                CostoTotalPS.ColumnEdit = this.editSpin;
                CostoTotalPS.Visible = true;
                this.gvDocument.Columns.Add(CostoTotalPS);

                //PosArancelaria
                GridColumn PosArancelaria = new GridColumn();
                PosArancelaria.FieldName = this.unboundPrefix + "PosArancelaria";
                PosArancelaria.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_PosArancelaria");
                PosArancelaria.UnboundType = UnboundColumnType.String;
                PosArancelaria.VisibleIndex = 19;
                PosArancelaria.Width = 90;
                PosArancelaria.Visible = true;
                this.gvDocument.Columns.Add(PosArancelaria);

                #endregion
                #region Columnas No Visibles

                //Estado
                GridColumn estado = new GridColumn();
                estado.FieldName = this.unboundPrefix + "EstadoInv";
                estado.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_EstadoInv");
                estado.UnboundType = UnboundColumnType.Integer;
                estado.Visible = false;
                this.gvDocument.Columns.Add(estado);

                //Valor1LOC
                GridColumn valorML = new GridColumn();
                valorML.FieldName = this.unboundPrefix + "Valor1LOC";
                valorML.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorML");
                valorML.UnboundType = UnboundColumnType.Decimal;
                valorML.Visible = false;
                this.gvDocument.Columns.Add(valorML);

                //Valor1EXT
                GridColumn Valor1EXT = new GridColumn();
                Valor1EXT.FieldName = this.unboundPrefix + "Valor1EXT";
                Valor1EXT.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_Valor1EXT");
                Valor1EXT.UnboundType = UnboundColumnType.Decimal;
                Valor1EXT.Visible = false;
                this.gvDocument.Columns.Add(Valor1EXT);

                //ValorAgente
                GridColumn ValorAgente = new GridColumn();
                ValorAgente.FieldName = this.unboundPrefix + "ValorAgente";
                ValorAgente.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorAgente");
                ValorAgente.UnboundType = UnboundColumnType.Decimal;
                ValorAgente.Visible = false;
                this.gvDocument.Columns.Add(ValorAgente);

                //CodigoReferencia
                GridColumn codRef = new GridColumn();
                codRef.FieldName = this.unboundPrefix + "inReferenciaID";
                //codRef.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_inReferenciaID");
                codRef.UnboundType = UnboundColumnType.String;
                codRef.Visible = false;
                this.gvDocument.Columns.Add(codRef);

                //Parameter1
                GridColumn param1 = new GridColumn();
                param1.FieldName = this.unboundPrefix + "Parametro1";
                param1.UnboundType = UnboundColumnType.String;
                param1.Visible = false;
                this.gvDocument.Columns.Add(param1);

                //Parameter2
                GridColumn param2 = new GridColumn();
                param2.FieldName = this.unboundPrefix + "Parametro2";
                param2.UnboundType = UnboundColumnType.String;
                param2.Visible = false;
                this.gvDocument.Columns.Add(param2);

                //Unidad
                GridColumn unidadRef = new GridColumn();
                unidadRef.FieldName = this.unboundPrefix + "UnidadRef";
                unidadRef.UnboundType = UnboundColumnType.String;
                unidadRef.Visible = false;
                this.gvDocument.Columns.Add(unidadRef);

                //IdentificadorTr
                GridColumn param3 = new GridColumn();
                param3.FieldName = this.unboundPrefix + "IdentificadorTr";
                param3.UnboundType = UnboundColumnType.Integer;
                param3.Visible = false;
                this.gvDocument.Columns.Add(param3);

                //ValorUnitario
                GridColumn vlrUnitario = new GridColumn();
                vlrUnitario.FieldName = this.unboundPrefix + "ValorUNI";
                vlrUnitario.UnboundType = UnboundColumnType.Decimal;
                vlrUnitario.Visible = false;
                this.gvDocument.Columns.Add(vlrUnitario);

                //Cantidad
                GridColumn cant = new GridColumn();
                cant.FieldName = this.unboundPrefix + "CantidadUNI";
                cant.UnboundType = UnboundColumnType.Decimal;
                cant.Visible = false;
                this.gvDocument.Columns.Add(cant);

                //Indice de la fila
                GridColumn colIndex = new GridColumn();
                colIndex.FieldName = this.unboundPrefix + "Index";
                colIndex.UnboundType = UnboundColumnType.Integer;
                colIndex.Visible = false;
                this.gvDocument.Columns.Add(colIndex);

                //valorFOBML
                GridColumn valorFOBML = new GridColumn();
                valorFOBML.FieldName = this.unboundPrefix + "Valor2LOC";
                valorFOBML.Visible = false;
                valorFOBML.UnboundType = UnboundColumnType.Decimal;
                this.gvDocument.Columns.Add(valorFOBML);

                //valorFOBME
                GridColumn valorFOBME = new GridColumn();
                valorFOBME.FieldName = this.unboundPrefix + "Valor2EXT";
                valorFOBME.Visible = false;
                valorFOBME.UnboundType = UnboundColumnType.Decimal;
                this.gvDocument.Columns.Add(valorFOBME);

                #endregion
                #region Columnas Detalle

                //DocSoporteTER
                GridColumn DocSoporteTER = new GridColumn();
                DocSoporteTER.FieldName = this.unboundPrefix + "DocSoporteTER";
                DocSoporteTER.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_DocSoporteTER");
                DocSoporteTER.UnboundType = UnboundColumnType.String;
                DocSoporteTER.VisibleIndex = 0;
                DocSoporteTER.Width = 100;
                DocSoporteTER.Visible = true;
                this.gvDetalle.Columns.Add(DocSoporteTER);

                //valorMEDeta
                GridColumn valorMEDeta = new GridColumn();
                valorMEDeta.FieldName = this.unboundPrefix + "Valor1EXT";
                valorMEDeta.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_Valor1EXT");
                valorMEDeta.UnboundType = UnboundColumnType.Decimal;
                valorMEDeta.VisibleIndex = 1;
                valorMEDeta.Width = 120;
                valorMEDeta.Visible = true;
                valorMEDeta.ColumnEdit = this.editSpin;
                this.gvDetalle.Columns.Add(valorMEDeta);

                //valorMLDeta
                GridColumn valorMLDeta = new GridColumn();
                valorMLDeta.FieldName = this.unboundPrefix + "Valor1LOC";
                valorMLDeta.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_Valor1LOC");
                valorMLDeta.UnboundType = UnboundColumnType.Decimal;
                valorMLDeta.VisibleIndex = 2;
                valorMLDeta.Width = 120;
                valorMLDeta.Visible = true;
                valorMLDeta.ColumnEdit = this.editSpin;
                this.gvDetalle.Columns.Add(valorMLDeta);


                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "LiquidacionImportacion.cs-AddDetailCols"));
            }
        }

        /// <summary>
        /// Obtiene el saldo y costo de cada item de la importacion
        /// </summary>
        /// <param name="consSaldos">consecutivo de inSaldoExistencias</param>
        /// <param name="detalle">Dto con el detalle a filtrar</param>
        /// <returns>cantidad del item en Existencias</returns>
        private decimal GetSaldoCostos(ref int consSaldos, DTO_glMovimientoDeta detalle)
        {
            try
            {
                decimal cantidadDisp = 0;
                DTO_inControlSaldosCostos ctrlSaldoCostos;
                #region Consulta los saldos de cada item
                this._costos = new DTO_inCostosExistencias();
                ctrlSaldoCostos = new DTO_inControlSaldosCostos();
                ctrlSaldoCostos.BodegaID.Value = detalle.BodegaID.Value;
                ctrlSaldoCostos.inReferenciaID.Value = detalle.inReferenciaID.Value;
                ctrlSaldoCostos.ActivoID.Value = detalle.ActivoID.Value;
                ctrlSaldoCostos.EstadoInv.Value = detalle.EstadoInv.Value;
                ctrlSaldoCostos.Parametro1.Value = detalle.Parametro1.Value;
                ctrlSaldoCostos.Parametro2.Value = detalle.Parametro2.Value;
                ctrlSaldoCostos.IdentificadorTr.Value = detalle.IdentificadorTr.Value;
                cantidadDisp = this._bc.AdministrationModel.Transaccion_SaldoExistByReferencia(documentID, ctrlSaldoCostos, ref this._costos);
                if (cantidadDisp == 0)
                {
                    MessageBox.Show(detalle.inReferenciaID + ": " + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_NotExistSaldoInventory));
                    return cantidadDisp;
                }
                List<DTO_inControlSaldosCostos> listSaldoCosto = this._bc.AdministrationModel.inControlSaldosCostos_GetByParameter(this.documentID, ctrlSaldoCostos);
                if (listSaldoCosto.Count > 0)
                    consSaldos = listSaldoCosto[0].RegistroSaldo.Value.Value;
                #endregion
                return cantidadDisp;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Obtiene un movimiento de Inventarios
        /// </summary>
        private void GetMvtoInventario(DTO_glDocumentoControl ctrl)
        {
            try
            {
                if (ctrl != null)
                {
                    if (ctrl.Estado.Value.Value == (byte)EstadoDocControl.Aprobado)
                    {
                        DTO_MvtoInventarios saldoCostos = this._bc.AdministrationModel.Transaccion_Get(this.documentID, ctrl.NumeroDoc.Value.Value);
                        if (!string.IsNullOrEmpty(saldoCostos.Header.DatoAdd1.Value))
                        {
                            #region Valida que el mvto de Importacion no haya sido liquidado
                            //Traer la consulta de importacionDocu con el doc transporte para verificar que no exista
                            //List<DTO_inDistribucionCosto> listDistrib = this._bc.AdministrationModel.inDistribucionCosto_GetByNumeroDoc(this.documentID, saldoCostos.DocCtrl.NumeroDoc.Value.Value, false);
                            //if (listDistrib.Count > 0)
                            //{
                            //    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_DistribucionExist));
                            //    this.validHeader = false;
                            //    return;
                            //} 
                            #endregion
                            #region Variables
                            int numeroDocFactura = 0;
                            string facturaProveedor = string.Empty;
                            int consExistencias = 0;
                            #endregion
                            #region Obtiene la factura de Compra del proveedor
                            DTO_glDocumentoControl docCtrlRecibido = this._bc.AdministrationModel.glDocumentoControl_GetByID(saldoCostos.DocCtrl.DocumentoPadre.Value.Value);
                            if (docCtrlRecibido != null)
                            {
                                List<DTO_prDetalleDocu> detalleDocu = this._bc.AdministrationModel.prDetalleDocu_GetByNumeroDoc(docCtrlRecibido.NumeroDoc.Value.Value, false);
                                DTO_glDocumentoControl docCtrlFactura = detalleDocu.Count > 0 ? this._bc.AdministrationModel.glDocumentoControl_GetByID(detalleDocu[0].FacturaDocuID.Value.Value) : null;
                                if (docCtrlFactura != null)
                                {
                                    numeroDocFactura = docCtrlFactura.NumeroDoc.Value.Value;
                                    facturaProveedor = docCtrlFactura.DocumentoTercero.Value;
                                }
                                else
                                {
                                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_In_FacturaProvNotExist));
                                    this.txtDocTransporte.Focus();
                                    return;
                                }
                            }
                            #endregion
                            #region Obtiene el detalle completo de la Importacion
                            foreach (DTO_inMovimientoFooter movFooter in saldoCostos.Footer)
                            {
                                DTO_inImportacionDeta impDeta = new DTO_inImportacionDeta();
                                DTO_glMovimientoDeta detaDocSoporte = new DTO_glMovimientoDeta();
                                decimal vlrFacturasByItem = 0;
                                impDeta.Movimiento = movFooter.Movimiento;
                                #region Obtiene el detalle de la facturas distribuidas
                                detaDocSoporte.DocSoporte.Value = movFooter.Movimiento.DocSoporte.Value;
                                List<DTO_glMovimientoDeta> listDetaFacturas = this._bc.AdministrationModel.glMovimientoDeta_GetByParameter(detaDocSoporte, false);
                                foreach (DTO_glMovimientoDeta detaFact in listDetaFacturas)
                                {
                                    if (detaFact.CantidadUNI.Value == 0)
                                    {
                                        impDeta.Detalle.Add(detaFact);
                                        vlrFacturasByItem += detaFact.Valor1EXT.Value.Value;
                                    }
                                }
                                impDeta.ValorOtrosUS.Value = vlrFacturasByItem;
                                #endregion
                                impDeta.FacturaNro.Value = facturaProveedor;
                                impDeta.NumeroDocFactura.Value = numeroDocFactura;
                                impDeta.NumeroDocNotaEnv.Value = saldoCostos.DocCtrl.NumeroDoc.Value;
                                impDeta.ReferenciaIDP1P2.Value = movFooter.ReferenciaIDP1P2.Value;
                                impDeta.ReferenciaIDP1P2Desc.Value = movFooter.ReferenciaIDP1P2Desc.Value;
                                impDeta.Cantidad.Value = this.GetSaldoCostos(ref consExistencias, movFooter.Movimiento);
                                impDeta.ConsSaldoExistencia.Value = consExistencias;
                                impDeta.ValorCostoUS.Value = movFooter.Movimiento.Valor1EXT.Value;
                                impDeta.ValorUnidadUS.Value = movFooter.Movimiento.ValorUNI.Value;
                                impDeta.PorArancel.Value = 0;
                                impDeta.PorIVA.Value = 0;
                                impDeta.ValorArancel.Value = 0;
                                impDeta.ValorIVA.Value = 0;
                                impDeta.ValorAgente.Value = 0;
                                impDeta.ValorTotalUS.Value = impDeta.ValorCostoUS.Value + impDeta.ValorOtrosUS.Value;
                                impDeta.ValorTotalPS.Value = impDeta.ValorTotalUS.Value * saldoCostos.DocCtrl.TasaCambioDOCU.Value;
                                impDeta.ValorOtrosPS.Value = impDeta.ValorArancel.Value + impDeta.ValorIVA.Value + impDeta.ValorAgente.Value;
                                impDeta.CostoTotalUS.Value = impDeta.ValorTotalUS.Value + (saldoCostos.DocCtrl.TasaCambioDOCU.Value != 0 ? impDeta.ValorOtrosPS.Value / saldoCostos.DocCtrl.TasaCambioDOCU.Value : 0);
                                impDeta.CostoTotalPS.Value = impDeta.CostoTotalUS.Value * saldoCostos.DocCtrl.TasaCambioDOCU.Value;
                                this.footerImportacion.Add(impDeta);
                            }
                            #endregion
                            #region Consulta las facturas distribuidas en la Importacion
                            List<DTO_inDistribucionCosto> listDistrib = this._bc.AdministrationModel.inDistribucionCosto_GetByNumeroDoc(this.documentID, saldoCostos.DocCtrl.NumeroDoc.Value.Value, false);
                            foreach (DTO_inDistribucionCosto fact in listDistrib)
                            {
                                ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(fact.NumeroDocCto.Value.Value);
                                DTO_coTercero tercero = (DTO_coTercero)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, ctrl.TerceroID.Value, true);
                                fact.FechaFactura.Value = ctrl.FechaDoc.Value.Value.Date;
                                fact.FacturaNro.Value = ctrl.DocumentoTercero.Value;
                                fact.ProveedorID.Value = tercero.ID.Value;
                                fact.ProveedorDesc.Value = tercero.Descriptivo.Value;
                                fact.Observacion.Value = ctrl.Observacion.Value;
                                fact.MonedaOrigen.Value = ctrl.MonedaID.Value;
                                fact.ValorML.Value = fact.MonedaOrigen.Value == this.monedaLocal ? fact.Valor.Value : (fact.Valor.Value * ctrl.TasaCambioDOCU.Value);
                                fact.ValorME.Value = fact.MonedaOrigen.Value != this.monedaLocal ? fact.Valor.Value : (fact.Valor.Value / ctrl.TasaCambioDOCU.Value);
                            }
                            this.dataFactura = listDistrib;
                            #endregion
                            this.masterAgenteAduanaProv.Value = saldoCostos.Header.AgenteAduanaID.Value;
                            this.cmbModalidad.EditValue = saldoCostos.Header.ModalidadImp.Value.ToString();
                            this.dataImportacion = this.LoadHeader();
                            this.LoadData(true);
                            this.LoadDataFactura();
                            this.validHeader = true;
                            this.txtDocTransporte.Enabled = false;
                            this.newDoc = false; 
                        }
                        else
                            MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_DocTransporteNotExist));
                    }
                    else
                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EstateInvalid));
                }
                else
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_InvalidNumberTransaccion));
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-LiquidacionImportacion.cs", "GetMvtoInventario: " + ex.Message));
            }
        }

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.LiquidacionImportacion;
            this.frmModule = ModulesPrefix.@in;

            InitializeComponent();
            base.SetInitParameters();

            this.AddDocumentCols();
            this.AddFacturaCols();
            
            //Trae valores por defecto
            this.param1xDef = _bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Parametro1xDefecto);
            this.param2xDef = _bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Parametro2xDefecto);
            this.monedaLocal = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
            this.monedaExtranjera = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize()
        {
            base.AfterInitialize();
            //Modifica el tamaño de las Grillas
            this.tlSeparatorPanel.RowStyles[0].Height = 120;
            this.tlSeparatorPanel.RowStyles[1].Height = 250;
            this.tlSeparatorPanel.RowStyles[2].Height = 180;

            Dictionary<string, string> dicTipoTransp = new Dictionary<string, string>();
            dicTipoTransp.Add("1", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_TransporteAereo));         
            dicTipoTransp.Add("2", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_TransporteMaritimo));
            dicTipoTransp.Add("3", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_TransporteTerrestre));
            dicTipoTransp.Add("4", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_TransporteTraficoPostal));
            this.cmbTipoTransporte.EditValue = 1;
            this.cmbTipoTransporte.Properties.DataSource = dicTipoTransp;
            this.dtFechaImportacion.DateTime = base.dtFecha.DateTime;

            Dictionary<string, string> dicModalidad = new Dictionary<string, string>();
            dicModalidad.Add("1", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_ModalidadImportacion1));
            dicModalidad.Add("2", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_ModalidadImportacion2));
            dicModalidad.Add("3", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_ModalidadImportacion3));
            dicModalidad.Add("4", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_ModalidadImportacion4));
            dicModalidad.Add("5", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_ModalidadImportacion5));
            dicModalidad.Add("6", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_ModalidadImportacion6));
            dicModalidad.Add("7", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_ModalidadImportacion7));
            dicModalidad.Add("8", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_ModalidadImportacion8));
            dicModalidad.Add("9", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_ModalidadImportacion9));
            dicModalidad.Add("10", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_ModalidadImportacion10));
            dicModalidad.Add("11", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_ModalidadImportacion11));
            this.cmbModalidad.EditValue = 1;
            this.cmbModalidad.Properties.DataSource = dicModalidad;

            this.gvDocument.OptionsBehavior.AutoPopulateColumns = true;
            this.gvDetail.OptionsBehavior.AutoPopulateColumns = true;
            this.txtDocTransporte.Focus();

            this.dataImportacion = new DTO_LiquidacionImportacion();
            this.footerImportacion = new List<DTO_inImportacionDeta>();
            this._ctrl = new DTO_glDocumentoControl();
            this.dataFactura = new List<DTO_inDistribucionCosto>();

            //Carga controles del Header
            this._bc.InitMasterUC(this.masterAgenteAduanaProv, AppMasters.prProveedor, true, true, true, false);

            if (this.multiMoneda)
            {
            }
        }

        /// <summary>
        /// Carga la información de las grilla
        /// </summary>
        /// <param name="firstTime">Si es la primera vez que carga la grilla</param>
        /// <param name="refresh">Si debe volver a cargar la data del servidor</param>
        protected override void LoadData(bool firstTime)
        {
            this.gcDocument.DataSource = this.dataImportacion.Footer;
            this.gcDocument.RefreshDataSource();
            bool hasItems = this.dataImportacion.Footer.GetEnumerator().MoveNext() ? true : false;
            if (hasItems)
                this.gvDocument.MoveFirst();
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
                this.newReg = false;
                int cFila = fila;
                GridColumn col = this.gvDocument.Columns[this.unboundPrefix + "Index"];
                this.indexFila = Convert.ToInt16(this.gvDocument.GetRowCellValue(cFila, col));
                this.isValid = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-DocumentMvtoForm.cs", "RowIndexChanged"));
            }
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
                #region Validacion de nulls y Fks
                //#region ProveedorID
                //validField = this._bc.ValidGridCell(this.gvDocument, string.Empty, fila, "ProveedorID", false, true, false, AppMasters.prProveedor);
                //if (!validField)
                //    validRow = false;
                //#endregion
                //#region FacturaNro
                //validField = this._bc.ValidGridCell(this.gvDocument, string.Empty, fila, "FacturaNro", false, false, false, null);
                //if (validField)                    
                //{
                //    GridColumn col = this.gvDocument.Columns[this.unboundPrefix + "FacturaNro"];
                //    string factura = this.gvDocument.GetRowCellValue(fila, this.unboundPrefix + "FacturaNro").ToString();
                //    DTO_CuentaXPagar cxpFactura = this._bc.AdministrationModel.CuentasXPagar_GetForCausacion(AppDocuments.CausarFacturas, this._terceroID, factura, false);
                //    if (cxpFactura == null)
                //    {
                //        string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_FacturaInvalid), factura);
                //        this.gvDocument.SetColumnError(col, msg);
                //        validField = false;
                //    }
                //}
                //if (!validField)
                //    validRow = false;
                //#endregion
                #endregion
                #region Validaciones de valores
                //#region ValorML
                //validField = this._bc.ValidGridCellValue(this.gvDocument, string.Empty, fila, "ValorML", false, true, true, false);
                //if (!validField)
                //    validRow = false;
                //#endregion
                //#region ValorME
                //validField = this._bc.ValidGridCellValue(this.gvDocument, string.Empty, fila, "ValorME", false, true, true, false);
                //if (!validField)
                //    validRow = false;
                //#endregion
                #endregion
                if (validRow)
                {
                    this.isValid = true;
                }
                else
                    this.isValid = false;
            }
            catch (Exception ex)
            {
                validRow = false;
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-DocumentMvtoForm.cs", "ValidateRow"));
            }

            this.hasChanges = true;
            return validRow;
        }

        #endregion

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void Form_Enter(object sender, EventArgs e)
        {
            base.Form_Enter(sender, e);
            FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
            FormProvider.Master.itemGenerateTemplate.Visible = false;
            FormProvider.Master.itemDelete.Visible = false;
            FormProvider.Master.itemPrint.Visible = false;
            FormProvider.Master.itemCopy.Visible = false;
            FormProvider.Master.itemPaste.Visible = false;
            FormProvider.Master.itemImport.Visible = false;
            FormProvider.Master.itemExport.Visible = false;
            FormProvider.Master.itemRevert.Visible = false;
            FormProvider.Master.itemSendtoAppr.Visible = false;
            FormProvider.Master.itemFilter.Visible = false;
            FormProvider.Master.itemFilterDef.Visible = false;

            FormProvider.Master.tbBreak.Visible = false;
            FormProvider.Master.tbBreak0.Visible = false;
            FormProvider.Master.tbBreak1.Visible = false;
        }

        #endregion

        #region Eventos Header

        /// <summary>
        /// Valida que solo numeros se pueden escribir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNro_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), "\\d+") && e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back)
                e.Handled = true;
            if (e.KeyChar == 46)
                e.Handled = true;
        }

        /// <summary>
        /// Valida que el numero ingresado exista
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtControlHeader_Leave(object sender, EventArgs e)
        { 
            try
            {
                if (sender.GetType() == typeof(TextBox))
                {
                    TextBox control = (TextBox)sender;
                    if (control.Name == "txtDocTransporte")
                        this.dataImportacion.Header.DocTransporte.Value = control.Text;
                    if (control.Name == "txtDeclarImportacion")
                        this.dataImportacion.Header.DeclaracionImp.Value = control.Text;
                    if (control.Name == "txtDocImportadora")
                        this.dataImportacion.Header.DocImportadora.Value = control.Text;
                    if (control.Name == "txtDocMvtoZonaFranca")
                        this.dataImportacion.Header.DocMvtoZonaFranca.Value = control.Text;
                    if (control.Name == "txtObservacion")
                    {
                        this.dataImportacion.Header.Observacion.Value = control.Text;
                        this.dataImportacion.DocCtrl.Observacion.Value = control.Text;
                    }
                }
                if (sender.GetType() == typeof(TextEdit))
                {
                    TextEdit control = (TextEdit)sender;
                    this.dataImportacion.Header.TasaImport.Value = Convert.ToDecimal(control.EditValue, CultureInfo.InvariantCulture);
                    this.dataImportacion.DocCtrl.TasaCambioDOCU.Value = Convert.ToDecimal(control.EditValue, CultureInfo.InvariantCulture);
                    this.dataImportacion.DocCtrl.TasaCambioCONT.Value = Convert.ToDecimal(control.EditValue, CultureInfo.InvariantCulture);
                }
                if (sender.GetType() == typeof(ControlsUC.uc_MasterFind))
                {
                    ControlsUC.uc_MasterFind control = (ControlsUC.uc_MasterFind)sender;
                    this.dataImportacion.Header.AgenteAduanaID.Value = control.Value;  
                }
                if (sender.GetType() == typeof(DateEdit))
                {
                    DateEdit control = (DateEdit)sender;
                    this.dataImportacion.DocCtrl.FechaDoc.Value = control.DateTime;
                }
                if (sender.GetType() == typeof(LookUpEdit))
                {
                    LookUpEdit control = (LookUpEdit)sender;
                    if (control.Name == "cmbModalidad")
                        this.dataImportacion.Header.ModalidadImp.Value = Convert.ToByte(control.EditValue);
                    if (control.Name == "cmbTipoTransporte")
                        this.dataImportacion.Header.TipoTransporte.Value = Convert.ToByte(control.EditValue);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-LiquidacionImportacion.cs", "txtNroDocInv_Leave: " + ex.Message));
            }
        }

        /// <summary>
        /// Valida que el documento de transporte
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtDocTransporte_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(this.txtDocTransporte.Text) && this.txtDocTransporte.Text != "0")
                {
                    #region Obtiene y valida el Doc de Transporte
                    List<DTO_inMovimientoDocu> listMovDocuExist = new List<DTO_inMovimientoDocu>();
                    DTO_inMovimientoDocu mvtoDocTransporte = new DTO_inMovimientoDocu();
                    mvtoDocTransporte.EmpresaID.Value = base.empresaID;
                    mvtoDocTransporte.DatoAdd1.Value = this.txtDocTransporte.Text;
                    if (!string.IsNullOrEmpty(this.txtDocTransporte.Text))
                        listMovDocuExist = this._bc.AdministrationModel.inMovimientoDocu_GetByParameter(documentID, mvtoDocTransporte);

                    if (listMovDocuExist.Count == 0)
                    {
                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_DocTransporteNotExist));
                        this.txtDocTransporte.Focus();
                        return;
                    } 
                    #endregion
                    mvtoDocTransporte = listMovDocuExist[0];
                    //Valida que exista el documento
                    DTO_glDocumentoControl docCtrlExist = null;
                    docCtrlExist = this._bc.AdministrationModel.glDocumentoControl_GetByID(mvtoDocTransporte.NumeroDoc.Value.Value);
                    this.GetMvtoInventario(docCtrlExist);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-LiquidacionImportacion.cs", "txtNroDocInv_Leave: " + ex.Message));
                this.txtDocTransporte.Focus();
            }
        }

        /// <summary>
        /// Consulta de documentos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Envento</param>
        private void btnQueryDoc_Click(object sender, EventArgs e)
        {
            List<int> docs = new List<int>();
            docs.Add(AppDocuments.NotaEnvio);
            docs.Add(AppDocuments.TransaccionAutomatica);
            ModalQueryDocument getDocControl = new ModalQueryDocument(docs,false,false);
            getDocControl.ShowDialog();
            if (getDocControl.DocumentoControl != null)
            {
                this.GetMvtoInventario(getDocControl.DocumentoControl);
                this.txtNro.Text = this.dataImportacion.DocCtrl.DocumentoNro.Value.ToString();
            }
        }

        #endregion

        #region Eventos Grilla Importacion(Detalle Mvto)

        /// <summary>
        /// Evento que se ejecuta al entrar a la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gcDocument_Enter(object sender, EventArgs e)
        {
            try
            {
                if (this.ValidateHeader() && this.validHeader)
                    this.validHeader = true;
                else
                    this.validHeader = false;
                //Si el diseño esta cargado y el header es valido
                if (this.validHeader)
                { 

                }
                else
                    this.masterAgenteAduanaProv.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp", "TransaccionManual.cs-gcDocument_Enter: " + ex.Message));
            }
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            object dto = (object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                if (fieldName == "Marca" && e.Value == null)
                    e.Value = this.select.Contains(e.ListSourceRowIndex);
                else
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
                            DTO_inImportacionDeta dtoM = (DTO_inImportacionDeta)e.Row;
                            pi = dtoM.Movimiento.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                            if (pi != null)
                            {
                                if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                                    e.Value = pi.GetValue(dtoM.Movimiento, null);
                                else
                                    e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dtoM.Movimiento, null), null);
                            }
                            else
                            {
                                fi = dtoM.Movimiento.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                                if (fi != null)
                                {
                                    if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double")
                                        e.Value = fi.GetValue(dtoM.Movimiento);
                                    else
                                        e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dtoM.Movimiento), null);
                                }
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
                        else
                        {
                            DTO_inImportacionDeta dtoM = (DTO_inImportacionDeta)e.Row;
                            pi = dtoM.Movimiento.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                            if (pi != null)
                            {
                                if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                                    e.Value = pi.GetValue(dtoM.Movimiento, null);
                                else
                                {
                                    UDT udtProp = (UDT)pi.GetValue(dtoM.Movimiento, null);
                                    udtProp.SetValueFromString(e.Value.ToString());
                                }
                            }
                            else
                            {
                                fi = dtoM.Movimiento.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                                if (fi != null)
                                {
                                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                                    {
                                        //e.Value = pi.GetValue(dto, null);
                                    }
                                    else
                                    {
                                        UDT udtProp = (UDT)fi.GetValue(dtoM.Movimiento);
                                        udtProp.SetValueFromString(e.Value.ToString());
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Eventos Grilla Factura

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetail_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            Type dataType = dto.GetType();
            int unboundPrefixLen = this.unboundPrefix.Length;

            string fieldName = e.Column.FieldName.Substring(unboundPrefixLen);

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

        #endregion

        #region Eventos Barra Herramientas

        /// <summary>
        /// Boton para iniciar un nuevo documento
        /// </summary>
        public override void TBNew()
        {
            try
            {
                base.TBNew();
                if (this.cleanDoc)
                {
                    this.CleanData();
                    //this.CleanHeader(true);
                    //this.EnableHeader(0, true);
                    //this.masterMvtoTipoInv.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-TransaccionManual.cs", "TBNew: " + ex.Message));

            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            try
            {
                base.TBSave();
                this.gvDocument.PostEditor();
                this.gvDocument.ActiveFilterString = string.Empty;
                if (this.ValidGrid())
                {
                    Thread process = new Thread(this.SaveThread);
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-TransaccionManual.cs", "TBSave: " + ex.Message));

            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo de salvar
        /// </summary>
        public override void SaveThread()
        {
            try
            {
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (this._bc.AdministrationModel.ConsultarProgresoInventarios(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                int numeroDoc = 0;
                bool update = false;
                if (this.dataImportacion.DocCtrl.NumeroDoc.Value.Value != 0)
                {
                    update = true;
                    numeroDoc = this.dataImportacion.DocCtrl.NumeroDoc.Value.Value;
                }
                DTO_SerializedObject obj = this._bc.AdministrationModel.LiquidacionImportacion_Add(this.documentID, this.dataImportacion, update, out numeroDoc);
                FormProvider.Master.StopProgressBarThread(this.documentID);

                bool isOK = this._bc.SendDocumentMail(MailType.NewDoc, this.documentID, this._actFlujo.seUsuarioID.Value, obj, true);
                if (isOK)
                {
                    this._bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), this._bc.AdministrationModel.User);

                    this.newDoc = true;
                    this.deleteOP = true;
                    this.footerImportacion = new List<DTO_inImportacionDeta>();
                    this.dataImportacion = new DTO_LiquidacionImportacion();
                    this.Invoke(this.saveDelegate);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-TransaccionManual.cs", "SaveThread: " + ex.Message));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion 
    }
}
