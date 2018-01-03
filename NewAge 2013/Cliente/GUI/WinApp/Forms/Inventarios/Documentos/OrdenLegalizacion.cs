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

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class OrdenLegalizacion : DocumentForm
    {
        #region Variables Formulario

        //Obtiene la instancia del controlador
        private BaseController _bc = BaseController.GetInstance();
        private DTO_inCostosExistencias _costos;
        private DTO_LiquidacionImportacion _dataImportacion = null;
        private DTO_MvtoInventarios _data = null;
        private DTO_glDocumentoControl _docCtrlMvto = null;
        private DTO_inMovimientoDocu _headerMvto = null;
        private List<DTO_inMovimientoFooter> _footerMvto = null;
        private List<DTO_inImportacionDeta> footerImportacion = null;
        private List<DTO_inDistribucionCosto> _dataFactura = null;
      
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
        private bool _copyData = false;

        #endregion
        
        #region Delegados
        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected override void SaveMethod()
        {
            this.gcDocument.DataSource = this._dataImportacion.Footer;
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
                return this._dataImportacion.Footer.FindIndex(det => det.Movimiento.Index == this.indexFila);
            }
        }

        #endregion

        public OrdenLegalizacion()
        {
           //this.InitializeComponent();
        }

        #region Funciones Privadas

        /// <summary>
        /// Limpia el formulario
        /// </summary>
        private void CleanData()
        {
            this._costos = new DTO_inCostosExistencias();
            this._dataFactura = new List<DTO_inDistribucionCosto>();
            this._headerMvto = new DTO_inMovimientoDocu();
            this._data = new DTO_MvtoInventarios();
            this._terceroID = string.Empty;
            this.cleanDoc = true;
            this.validHeader = false;

            this.masterPrefijo.Value = string.Empty;
            this.txtNro.Text = string.Empty;      
            this.txtNroOrdenLegal.Text = string.Empty;            
            this.masterAgenteAduanaProv.Value = string.Empty;
            this.cmbModalidad.EditValue = 1;

            this.masterPrefijo.EnableControl(true);
            this.txtNro.Enabled = true;
            this.txtNroOrdenLegal.Enabled = true;
            this.masterAgenteAduanaProv.EnableControl(true);
            this.cmbModalidad.Enabled = true;
            this.btnQueryDoc.Enabled = true;

            this.gcDocument.DataSource = null;
            this.masterPrefijo.Focus();
        }

        /// <summary>
        /// valida la informacion del header
        /// </summary>
        private bool ValidateHeader()
        {
            bool result = true;
            if (!this.masterPrefijo.ValidID)
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterPrefijo.CodeRsx);

                MessageBox.Show(msg);
                this.masterPrefijo.Focus();

                result = false;
            }
            if (string.IsNullOrEmpty(this.txtNro.Text))
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.lblNro.Text);

                MessageBox.Show(msg);
                this.txtNro.Focus();

                result = false;
            }

            if (!this.masterAgenteAduanaProv.ValidID)
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.lblAgenteAduana.Text);

                MessageBox.Show(msg);
                this.masterAgenteAduanaProv.Focus();

                result = false;
            }
            if (string.IsNullOrEmpty(this.txtNroOrdenLegal.Text))
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.lblNroOrdenLegal.Text);

                MessageBox.Show(msg);
                this.txtNroOrdenLegal.Focus();

                result = false;
            }

            return result;
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "OrdenLegalizacion.cs-AddDocumentCols"));
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
            decimal cantidadDisp = 0;
            try
            {             
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
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "OrdenLegalizacion.cs-GetSaldoCostos"));
                return cantidadDisp;
            }
        }

        /// <summary>
        /// Obtiene un movimiento de Inventarios
        /// </summary>
        private void GetMvtoInventario()
        {
            try
            {
                if (this._docCtrlMvto != null)
                {
                    if (this._docCtrlMvto.Estado.Value.Value == (byte)EstadoDocControl.Aprobado)
                    {
                        DTO_MvtoInventarios saldoCostos = this._bc.AdministrationModel.Transaccion_Get(this.documentID, this._docCtrlMvto.NumeroDoc.Value.Value);
                        this._headerMvto = new DTO_inMovimientoDocu();
                        this._footerMvto = new List<DTO_inMovimientoFooter>();
                        if (this._copyData)
                        {
                            saldoCostos.DocCtrl.NumeroDoc.Value = 0;
                            saldoCostos.DocCtrl.Estado.Value = (byte)EstadoDocControl.SinAprobar;
                            saldoCostos.Header.NumeroDoc.Value = 0;
                            this._copyData = false;  
                        }
                        this._headerMvto = saldoCostos.Header;
                        this._footerMvto = saldoCostos.Footer;
                        #region Variables
                        int numeroDocFactura = 0;
                        string facturaProveedor = string.Empty;
                        int consExistencias = 0;
                        #endregion
                        #region Obtiene la factura de Compra del proveedor
                        if (saldoCostos.DocCtrl.DocumentoPadre != null)
                        {
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
                                    this.txtNro.Focus();
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
                            DTO_glDocumentoControl docCtrlFactDistr = this._bc.AdministrationModel.glDocumentoControl_GetByID(fact.NumeroDocCto.Value.Value);
                            DTO_coTercero tercero = (DTO_coTercero)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, docCtrlFactDistr.TerceroID.Value, true);
                            fact.FechaFactura.Value = docCtrlFactDistr.FechaDoc.Value.Value.Date;
                            fact.FacturaNro.Value = docCtrlFactDistr.DocumentoTercero.Value;
                            fact.ProveedorID.Value = tercero.ID.Value;
                            fact.ProveedorDesc.Value = tercero.Descriptivo.Value;
                            fact.Observacion.Value = docCtrlFactDistr.Observacion.Value;
                            fact.MonedaOrigen.Value = docCtrlFactDistr.MonedaID.Value;
                            fact.ValorML.Value = fact.MonedaOrigen.Value == this.monedaLocal ? fact.Valor.Value : (fact.Valor.Value * docCtrlFactDistr.TasaCambioDOCU.Value);
                            fact.ValorME.Value = fact.MonedaOrigen.Value != this.monedaLocal ? fact.Valor.Value : (fact.Valor.Value / docCtrlFactDistr.TasaCambioDOCU.Value);
                        }
                        this._dataFactura = listDistrib;
                        #endregion
                        this.masterAgenteAduanaProv.Value = this._headerMvto.AgenteAduanaID.Value;
                        this.txtNroOrdenLegal.Text = this._headerMvto.DatoAdd3.Value;
                        this.cmbModalidad.EditValue = this._headerMvto.ModalidadImp.Value;
                        this._dataImportacion.Footer = this.footerImportacion;
                        this.LoadData(true);
                        this.validHeader = true;
                        this.txtNro.Enabled = false;
                        this.masterPrefijo.EnableControl(false);
                        this.newDoc = false; 
                        }
                        else
                            MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, "Revisar facturas en Nota de envio"));
                    }
                    else
                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EstateInvalid));
                }
                else
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_InvalidNumberTransaccion));
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "OrdenLegalizacion.cs-GetMvtoInventario"));
            }      
        }

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.OrdenLegalizacion;
            this.frmModule = ModulesPrefix.@in;

            InitializeComponent();
            base.SetInitParameters();

            this.AddDocumentCols();
            
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
            //Modifica el tamaño de las secciones
            this.tlSeparatorPanel.RowStyles[0].Height = 110;
            this.tlSeparatorPanel.RowStyles[1].Height = 280; 
            this.tlSeparatorPanel.RowStyles[2].Height = 100;

            //Carga controles del Header
            this._bc.InitMasterUC(this.masterAgenteAduanaProv, AppMasters.prProveedor, false, true, true, false);
            this._bc.InitMasterUC(this.masterPrefijo, AppMasters.glPrefijo, true, true, true, false);

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
            this.gvDocument.OptionsBehavior.ReadOnly = true;
            this.masterPrefijo.Focus();

            this._dataImportacion = new DTO_LiquidacionImportacion();
            this.footerImportacion = new List<DTO_inImportacionDeta>();
            this._dataFactura = new List<DTO_inDistribucionCosto>();
        }

        /// <summary>
        /// Carga la información de las grilla
        /// </summary>
        /// <param name="firstTime">Si es la primera vez que carga la grilla</param>
        /// <param name="refresh">Si debe volver a cargar la data del servidor</param>
        protected override void LoadData(bool firstTime)
        {
            this.gcDocument.DataSource = this._dataImportacion.Footer;
            this.gcDocument.RefreshDataSource();
            bool hasItems = this._dataImportacion.Footer.GetEnumerator().MoveNext() ? true : false;
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
            FormProvider.Master.itemDelete.Visible = false;
            FormProvider.Master.itemPrint.Visible = false;
            FormProvider.Master.itemCopy.Visible = false;
            FormProvider.Master.itemPaste.Visible = false;
            FormProvider.Master.itemImport.Visible = false;
            FormProvider.Master.itemGenerateTemplate.Visible = false;
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
        /// Valida que el documento de transporte
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtNro_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(this.txtNro.Text) && this.txtNro.Text != "0")
                {                   
                    //Valida que exista el documento
                    this._docCtrlMvto = this._bc.AdministrationModel.glDocumentoControl_GetInternalDoc(AppDocuments.TransaccionAutomatica, this.masterPrefijo.Value, Convert.ToInt32(this.txtNro.Text));
                    if (this._docCtrlMvto == null)
                        this._docCtrlMvto = this._bc.AdministrationModel.glDocumentoControl_GetInternalDoc(AppDocuments.NotaEnvio, this.masterPrefijo.Value, Convert.ToInt32(this.txtNro.Text));
                    this.GetMvtoInventario();  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-OrdenLegalizacion.cs", "txtNro_Leave: " + ex.Message));
                this.txtNro.Focus();
            }
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
                    if (control.Name == "txtNroOrdenLegal")
                        this._headerMvto.DatoAdd3.Value = control.Text;
                }
                if (sender.GetType() == typeof(ControlsUC.uc_MasterFind))
                {
                    ControlsUC.uc_MasterFind control = (ControlsUC.uc_MasterFind)sender;
                    if (control.ValidID)
                        this._headerMvto.AgenteAduanaID.Value = control.Value;  
                }
                if (sender.GetType() == typeof(LookUpEdit))
                {
                    LookUpEdit control = (LookUpEdit)sender;
                    this._headerMvto.ModalidadImp.Value = Convert.ToByte(control.EditValue);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-OrdenLegalizacion.cs", "txtControlHeader_Leave: " + ex.Message));
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
            ModalQueryDocument getDocControl = new ModalQueryDocument(docs);
            getDocControl.ShowDialog();
            if (getDocControl.DocumentoControl != null)
            {
                if (getDocControl.CopiadoInd)
                    this._copyData = true;
                this._docCtrlMvto = getDocControl.DocumentoControl;
                this.GetMvtoInventario();
                this.txtNro.Text = this._docCtrlMvto.DocumentoNro.Value.ToString();
                this.masterPrefijo.Value = this._docCtrlMvto.PrefijoID.Value;
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
                if (!this.validHeader)
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
                this.gvDocument.Focus();
                this.gvDocument.ActiveFilterString = string.Empty;
                if (this.ValidateHeader() && this._dataImportacion.Footer.Count > 0)
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

                int numeroDoc = 1;
                int c = 0;
                this._data = new DTO_MvtoInventarios();
                this._data.DocCtrl = this._docCtrlMvto;
                this._data.Header = this._headerMvto;
                //Asigna un consecutivo ordenado al detalle del mvto para archivo
                foreach (DTO_inMovimientoFooter footer in this._footerMvto)
                {
                    c++;
                    footer.Movimiento.Valor10EXT.Value = c;                   
                }
                this._data.Footer = this._footerMvto;

                DTO_SerializedObject obj = this._bc.AdministrationModel.Transaccion_Add(this.documentID, this._data,true, out numeroDoc);
                FormProvider.Master.StopProgressBarThread(this.documentID);

                bool isOK = this._bc.SendDocumentMail(MailType.NewDoc, this.documentID, this._actFlujo.seUsuarioID.Value, obj, true);
                if (isOK)
                {
                    this._bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), this._bc.AdministrationModel.User);

                    this.newDoc = true;
                    this.deleteOP = true;
                    this.footerImportacion = new List<DTO_inImportacionDeta>();
                    this._dataImportacion = new DTO_LiquidacionImportacion();
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
