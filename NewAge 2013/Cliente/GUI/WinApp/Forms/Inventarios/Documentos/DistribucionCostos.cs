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

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class DistribucionCostos : DocumentForm
    {
        #region Variables Formulario

        //Obtiene la instancia del controlador
        private BaseController _bc = BaseController.GetInstance();
        private DTO_glDocumentoControl _ctrl = null;
        private DTO_inCostosExistencias _costos;
        private DTO_MvtoInventarios dataDetail = null;
        private List<DTO_inDistribucionCosto> dataFactura = null;
        //private DTO_MvtoInventarios data = null;
        private string monedaLocal;
        private string monedaExtranjera;
        private string monedaId;
        //Indica si el header es valido
        private bool validHeader;
        //variables para funciones particulares
        private string _terceroID = string.Empty;
        private string _facturaNro = string.Empty;
        private bool cleanDoc = true;

        private decimal _valorMvtoTotalML = 0;
        private decimal _valorMvtoTotalME = 0;
        private decimal _valorTotalDistribucionML = 0;
        private decimal _valorTotalDistribucionME = 0;
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
            this.gcDetail.DataSource = this.dataDetail.Footer;
            this.gcDocument.DataSource = this.dataFactura;
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
                return this.dataDetail.Footer.FindIndex(det => det.Movimiento.Index == this.indexFila);
            }
        }

        #endregion

        public DistribucionCostos()
        {
            //this.InitializeComponent();
        }

        #region Funciones Privadas

        /// <summary>
        /// Funcion que calcular los valores de distribucion
        /// </summary>
        /// <param name="recompra">Credito al que se le calculan los valores</param>
        /// <param name="sustituye">Indica si el credito se esta sustituyendo o recomprando</param>
        private void CalcularDistribucionCto(bool porcentajeInd)
        {
            try
            {
                decimal porcValid = 0;
                #region Obtiene el total de las Facturas para Distribucion
                this._valorTotalDistribucionML = 0;
                this._valorTotalDistribucionME = 0;
                foreach (var factura in this.dataFactura)
                {
                    this._valorTotalDistribucionML += factura.ValorML.Value.Value;
                    this._valorTotalDistribucionME += factura.ValorME.Value.Value;
                    this.txtValorDistribucionML.EditValue = this._valorTotalDistribucionML;
                    this.txtValorDistribucionME.EditValue = this._valorTotalDistribucionME;
                } 
                #endregion  

                foreach (var mvto in this.dataDetail.Footer)
                {
                    if (porcentajeInd)
                    {
                        #region Asigna los porcentajes a cada registro
                        decimal valorML = mvto.ValorItemTotalML.Value.Value;
                        decimal valorME = mvto.ValorItemTotalME.Value.Value;

                        if (valorML != 0 && this._valorMvtoTotalML != 0)
                            mvto.PorcDistribucion.Value = (valorML * 100) / this._valorMvtoTotalML;
                        else if (valorME != 0 && this._valorMvtoTotalME != 0)
                            mvto.PorcDistribucion.Value =(valorME * 100) / this._valorMvtoTotalME;
                        porcValid += mvto.PorcDistribucion.Value.Value; 
                        #endregion
                    }
                    else
                    {                        
                        #region Obtiene el valor de distribucion a cada registro
                        decimal porcDistr = mvto.PorcDistribucion.Value.Value;

                        mvto.ValorDistribucionML.Value = Math.Round(((porcDistr * this._valorTotalDistribucionML) / 100), 2); 
                        mvto.ValorDistribucionME.Value = Math.Round(((porcDistr * this._valorTotalDistribucionME) / 100), 2); 
                        #endregion
                    }
                }
                 if (porcentajeInd && porcValid != 100)
                     MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_PercentInvalid));
                 this.gcDetail.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DistribucionCostos.cs", "CalcularDistribucionCto"));
            }
        }

        /// <summary>
        /// Calcula el saldo de existencias en las referencias
        /// </summary>
        private void GetSaldoAvailable()
        {
            try
            {
                int index = this.NumFila;
                decimal cantidadDisp = 0;
                DTO_inControlSaldosCostos saldos;
                foreach (var item in this.dataDetail.Footer)
                {
                    #region Consulta los saldos de cada referencia
                    //this._costos = new DTO_inCostosExistencias();
                    //saldos = new DTO_inControlSaldosCostos();
                    //saldos.BodegaID.Value = item.Movimiento.BodegaID.Value;
                    //saldos.inReferenciaID.Value = item.Movimiento.inReferenciaID.Value;
                    //saldos.ActivoID.Value = item.Movimiento.ActivoID.Value;
                    //saldos.EstadoInv.Value = item.Movimiento.EstadoInv.Value;
                    //saldos.Parametro1.Value = item.Movimiento.Parametro1.Value;
                    //saldos.Parametro2.Value = item.Movimiento.Parametro2.Value;
                    ////saldos.IdentificadorTr.Value = this.costeoGrupoOri.CosteoTipo.Value == (byte)TipoCosteoInv.Transaccional ? this.data.DocCtrl.NumeroDoc.Value : 0;
                    //cantidadDisp = this._bc.AdministrationModel.Transaccion_SaldoExistByReferencia(documentID, saldos, ref this._costos);
                    //if (cantidadDisp == 0)
                    //{
                    //    MessageBox.Show(item.ReferenciaIDP1P2 + ": " + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_NotExistSaldoInventory));
                    //    return;
                    //}
                    #endregion
                    #region Asigna Costos(Bruto + Fob)
                    item.ValorItemTotalML.Value = item.Movimiento.Valor1LOC.Value.Value + item.Movimiento.Valor2LOC.Value.Value;
                    item.ValorItemTotalME.Value = item.Movimiento.Valor1EXT.Value.Value + item.Movimiento.Valor2EXT.Value.Value;
                    #endregion
                    this._valorMvtoTotalML += item.Movimiento.Valor1LOC.Value.Value + item.Movimiento.Valor2LOC.Value.Value;
                    this._valorMvtoTotalME += item.Movimiento.Valor1EXT.Value.Value + item.Movimiento.Valor2EXT.Value.Value;
                    this.txtValorMvtoML.EditValue = this._valorMvtoTotalML;
                    this.txtValorMvtoME.EditValue = this._valorMvtoTotalME;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Obtiene el valor de la factura verificando las Cuentas de Tipo Inventario
        /// </summary>
        /// <param name="fila">Identificador de la Fila</param>
        private void GetValorFactura(int fila, DTO_CuentaXPagar cxpFactura)
        {
            #region Variables
            Dictionary<string, DTO_coPlanCuenta> cacheCuenta = new Dictionary<string, DTO_coPlanCuenta>();
            DTO_coPlanCuenta cta = null;
            Dictionary<string, DTO_coCuentaGrupo> cacheCuentaGrupo = new Dictionary<string, DTO_coCuentaGrupo>();
            DTO_coCuentaGrupo ctaGrupo = null;
            decimal valorFacturaML = 0;
            decimal valorFacturaME = 0;
            #endregion
            #region Valida que la factura no haya sido usada antes en Distribucion
            List<DTO_inDistribucionCosto> listDistrib = this._bc.AdministrationModel.inDistribucionCosto_GetByNumeroDoc(this.documentID, cxpFactura.DocControl.NumeroDoc.Value.Value, true);
            if (listDistrib.Count > 0)
            {
                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_FacturaAlreadyDistribucion));
                return;
            }
            #endregion
            #region Obtiene el comprobante de la factura(validar Cuentas)
            //Trae items relacionados a la factura
            DTO_Comprobante saldos = this._bc.AdministrationModel.Comprobante_GetAll(cxpFactura.DocControl.NumeroDoc.Value.Value, false, cxpFactura.DocControl.PeriodoDoc.Value.Value,
                                                                                     cxpFactura.DocControl.ComprobanteID.Value, cxpFactura.DocControl.ComprobanteIDNro.Value);
            foreach (var saldo in saldos.Footer)
            {
                #region Carga la Cuenta
                if (cacheCuenta.ContainsKey(saldo.CuentaID.Value))
                    cta = cacheCuenta[saldo.CuentaID.Value];
                else
                {
                    cta = (DTO_coPlanCuenta)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, false, saldo.CuentaID.Value, true);
                    cacheCuenta.Add(saldo.CuentaID.Value, cta);
                }
                #endregion
                #region Carga la Cuenta Grupo
                if (cacheCuentaGrupo.ContainsKey(cta.CuentaGrupoID.Value))
                    ctaGrupo = cacheCuentaGrupo[cta.CuentaGrupoID.Value];
                else
                {
                    ctaGrupo = (DTO_coCuentaGrupo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coCuentaGrupo, false, cta.CuentaGrupoID.Value, true);
                    cacheCuentaGrupo.Add(cta.CuentaGrupoID.Value, ctaGrupo);
                }
                #endregion
                #region Si es de Inventario Transito trae valores
                if (ctaGrupo != null && ctaGrupo.TipoCuenta.Value == (byte)TipoCuentaGrupo.InvTransito)
                {
                    valorFacturaML += saldo.vlrMdaLoc.Value.Value;
                    valorFacturaME += saldo.vlrMdaExt.Value.Value;
                }
                #endregion
            }
            #endregion
            #region Asigna el valor de la factura
            if (valorFacturaML > 0)
            {
                this.dataFactura[fila].MonedaOrigen.Value = cxpFactura.DocControl.MonedaID.Value;
                this.dataFactura[fila].NumeroDocCto.Value = cxpFactura.DocControl.NumeroDoc.Value;
                this.dataFactura[fila].Valor.Value = cxpFactura.DocControl.MonedaID.Value == this.monedaLocal ? valorFacturaML : valorFacturaME;
                #region Asigna valores Local y Extranjero
                if (this.multiMoneda)
                {
                    this.dataFactura[fila].ValorML.Value = valorFacturaML;
                    this.dataFactura[fila].ValorME.Value = valorFacturaME;
                }
                else
                {
                    this.dataFactura[fila].ValorML.Value = valorFacturaML;
                    this.dataFactura[fila].ValorME.Value = 0;
                }
                #endregion
            }
            else
                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Pr_ServicioNotExistFact));
            
            #endregion           
            #region Obtiene el valor de los Servicios de cada factura
            //List<DTO_prDetalleDocu> detalleDocu = this._bc.AdministrationModel.prDetalleDocu_GetByNumeroDoc(cxpFactura.DocControl.NumeroDoc.Value.Value, true);
            //foreach (var det in detalleDocu)
            //{
            //    DTO_prBienServicio servicio = (DTO_prBienServicio)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.prBienServicio, false, det.CodigoBSID.Value, true);
            //    DTO_glBienServicioClase claseBS = (DTO_glBienServicioClase)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, false, servicio.ClaseBSID.Value, true);
            //    //Obtiene el valor si el item es del Tipo Servicio
            //    if (claseBS.TipoCodigo.Value == (byte)TipoCodigo.Servicio)
            //        valorFacturaML += det.ValorTotML.Value.Value;
            //}
            ////Asigna los valores al doc actual
            //if (valorFacturaML > 0)
            //{
            //    valorFacturaML = cxpFactura.CxP.Valor.Value.Value;
            //    this.dataFactura[fila].MonedaOrigen.Value = cxpFactura.DocControl.MonedaID.Value;
            //    this.dataFactura[fila].NumeroDocCto.Value = cxpFactura.DocControl.NumeroDoc.Value;
            //    this.dataFactura[fila].Valor.Value = valorFacturaML;
            //    #region Asigna valores Local y Extranjero
            //    if (this.multiMoneda)
            //    {
            //        if (cxpFactura.DocControl.MonedaID.Value == this.monedaLocal)
            //        {
            //            this.dataFactura[fila].ValorML.Value = valorFacturaML;
            //            this.dataFactura[fila].ValorME.Value = Math.Round((valorFacturaML / cxpFactura.DocControl.TasaCambioCONT.Value.Value), 2);
            //        }
            //        else
            //        {
            //            this.dataFactura[fila].ValorML.Value = Math.Round((valorFacturaML * cxpFactura.DocControl.TasaCambioCONT.Value.Value), 2);
            //            this.dataFactura[fila].ValorME.Value = valorFacturaML;
            //        }
            //    }
            //    else
            //    {
            //        this.dataFactura[fila].ValorML.Value = valorFacturaML;
            //        this.dataFactura[fila].ValorME.Value = 0;
            //    }

            //    #endregion
            //}
            //else
            //    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Pr_ServicioNotExistFact));
            #endregion
        }

        private void GetMvtoInventario(DTO_glDocumentoControl ctrl)
        {
            DTO_MvtoInventarios saldoCostos = this._bc.AdministrationModel.Transaccion_Get(this.documentID, ctrl.NumeroDoc.Value.Value);
            if (this._copyData)
            {
                saldoCostos.DocCtrl.NumeroDoc.Value = 0;
                saldoCostos.DocCtrl.Estado.Value = (byte)EstadoDocControl.SinAprobar;
                saldoCostos.Header.NumeroDoc.Value = 0;
            }
            this.dataDetail = saldoCostos;
            this.masterPrefijo.Text = ctrl.PrefijoID.Value;
            this.txtNroDocInv.Text = ctrl.DocumentoNro.Value.ToString();
            this.GetSaldoAvailable();
            this.CalcularDistribucionCto(true);
            this.LoadData(true);
            this.validHeader = true;
            this.masterPrefijo.EnableControl(false);
            this.txtNroDocInv.Enabled = false;
            this.newDoc = false;
            this.AddNewRow();

        }

        /// <summary>
        /// Limpia el formulario
        /// </summary>
        private void CleanData()
        {
            this._ctrl = new DTO_glDocumentoControl();
            this._costos = new DTO_inCostosExistencias();
            this.dataDetail = new DTO_MvtoInventarios();
            this.dataFactura = new List<DTO_inDistribucionCosto>();
            this._terceroID = string.Empty;
            this.cleanDoc = true;

            this._valorMvtoTotalML = 0;
            this._valorMvtoTotalME = 0;
            this._valorTotalDistribucionML = 0;
            this._valorTotalDistribucionME = 0;
            this.masterPrefijo.EnableControl(true);
            this.txtNroDocInv.Enabled = true;
            this.masterPrefijo.Value = string.Empty;
            this.txtNroDocInv.Text = string.Empty;
            this.txtValorMvtoML.EditValue = 0;
            this.txtValorMvtoME.EditValue = 0;
            this.txtValorDistribucionML.EditValue = 0;
            this.txtValorDistribucionME.EditValue = 0;
            this.txtObservacion.Text = string.Empty;

            this.gcDocument.DataSource = null;
            this.gcDetail.DataSource = null;
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
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-DistribucionCostos.cs", "LoadTasaCambio"));
                return _tasaCambio;
            }
        }

        /// <summary>
        /// valida la informacion del header
        /// </summary>
        private bool ValidateHeader()
        {
            bool result = true;
            this.validHeader = true;
            if (!this.masterPrefijo.ValidID)
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterPrefijo.CodeRsx);

                MessageBox.Show(msg);
                this.masterPrefijo.Focus();
                this.validHeader = false;
                result = false;
            }
            if (string.IsNullOrEmpty(this.txtNroDocInv.Text))
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.lblNroDocInv.Text);

                MessageBox.Show(msg);
                this.txtNroDocInv.Focus();
                this.validHeader = false;
                result = false;
            }
            if (string.IsNullOrEmpty(this.txtObservacion.Text))
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.lblObservacion.Text);

                MessageBox.Show(msg);
                this.txtObservacion.Focus();
                this.validHeader = false;
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Carga informacion de un temporal a partir del cabezote
        /// </summary>
        /// <returns>Retorna el header de un temporal</returns>
        private DTO_glDocumentoControl LoadDocCtrl()
        {
            try
            {
                this._ctrl = ObjectCopier.Clone(this.dataDetail.DocCtrl);
                this._ctrl.DocumentoID.Value = AppDocuments.DistribucionCostos;
                this._ctrl.NumeroDoc.Value = 0;//this.data.DocCtrl.NumeroDoc.Value != null ? this.data.DocCtrl.NumeroDoc.Value : 0;
                this._ctrl.Fecha.Value = DateTime.Now;
                this._ctrl.FechaDoc.Value = this.dtFecha.DateTime;
                this._ctrl.PeriodoDoc.Value = this.dtPeriod.DateTime;
                this._ctrl.DocumentoID.Value = this.documentID;
                this._ctrl.Estado.Value = (byte)EstadoDocControl.Aprobado;
                this._ctrl.DocumentoPadre.Value = 0;
                this._ctrl.DocumentoTercero.Value = string.Empty;
                this._ctrl.Descripcion.Value = base.txtDocDesc.Text;
                this._ctrl.Observacion.Value = this.txtObservacion.Text;
                this._ctrl.Valor.Value = 0;
                this._ctrl.Iva.Value = 0;
                //this._ctrl.UsuarioRESP.Value = this._bc.AdministrationModel.User.ID.Value;
                return this._ctrl;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-TransaccionManual.cs", "LoadTempHeader: " + ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla principal del documento
        /// </summary>
        private void AddDocumentCols()
        {
            try
            {
                //ProveedorID
                GridColumn ProveedorID = new GridColumn();
                ProveedorID.FieldName = this.unboundPrefix + "ProveedorID";
                ProveedorID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProveedorID");
                ProveedorID.UnboundType = UnboundColumnType.String;
                ProveedorID.VisibleIndex = 0;
                ProveedorID.Width = 80;
                ProveedorID.Visible = true;
                ProveedorID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                ProveedorID.OptionsColumn.AllowEdit = true;
                ProveedorID.ColumnEdit = this.editBtnGrid;
                this.gvDocument.Columns.Add(ProveedorID);

                //ProveedorDesc
                GridColumn ProveedorDesc = new GridColumn();
                ProveedorDesc.FieldName = this.unboundPrefix + "ProveedorDesc";
                ProveedorDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProveedorDesc");
                ProveedorDesc.UnboundType = UnboundColumnType.String;
                ProveedorDesc.VisibleIndex = 1;
                ProveedorDesc.Width = 80;
                ProveedorDesc.Visible = true;
                ProveedorDesc.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                ProveedorDesc.OptionsColumn.AllowEdit = false;
                ProveedorDesc.OptionsColumn.AllowFocus = false;
                this.gvDocument.Columns.Add(ProveedorDesc);

                //FacturaNro
                GridColumn FacturaNro = new GridColumn();
                FacturaNro.FieldName = this.unboundPrefix + "FacturaNro";
                FacturaNro.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FacturaNro");
                FacturaNro.UnboundType = UnboundColumnType.String;
                FacturaNro.VisibleIndex = 2;
                FacturaNro.Width = 80;
                FacturaNro.Visible = true;
                FacturaNro.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                FacturaNro.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(FacturaNro);

                //MonedaOrigen ID
                GridColumn MonedaOrigen = new GridColumn();
                MonedaOrigen.FieldName = this.unboundPrefix + "MonedaOrigen";
                MonedaOrigen.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_MonedaOrigen");
                MonedaOrigen.UnboundType = UnboundColumnType.Integer;
                MonedaOrigen.VisibleIndex = 3;
                MonedaOrigen.Width = 100;
                MonedaOrigen.Visible = true;
                MonedaOrigen.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                MonedaOrigen.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(MonedaOrigen);

                //ValorML
                GridColumn ValorML = new GridColumn();
                ValorML.FieldName = this.unboundPrefix + "ValorML";
                ValorML.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorML");
                ValorML.UnboundType = UnboundColumnType.Decimal;
                ValorML.VisibleIndex = 9;
                ValorML.Width = 200;
                ValorML.Visible = true;
                ValorML.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                ValorML.OptionsColumn.AllowEdit = false;
                ValorML.ColumnEdit = this.editSpin;
                this.gvDocument.Columns.Add(ValorML);

                //ValorME
                GridColumn ValorME = new GridColumn();
                ValorME.FieldName = this.unboundPrefix + "ValorME";
                ValorME.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorME");
                ValorME.UnboundType = UnboundColumnType.Decimal;
                ValorME.VisibleIndex = 10;
                ValorME.Width = 200;
                ValorME.Visible = true;
                ValorME.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                ValorME.OptionsColumn.AllowEdit = false;
                ValorME.ColumnEdit = this.editSpin;
                this.gvDocument.Columns.Add(ValorME);

                this.gvDocument.OptionsView.ColumnAutoWidth = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "DistribucionCostos.cs-AddDocumentCols"));
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla principal del documento
        /// </summary>
        private void AddDetailCols()
        {
            try
            {
                #region Columnas basicas

                //CodigoReferencia+Param1+Param2
                GridColumn codRefP1P2 = new GridColumn();
                codRefP1P2.FieldName = this.unboundPrefix + "ReferenciaIDP1P2";
                codRefP1P2.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_inReferenciaID");
                codRefP1P2.UnboundType = UnboundColumnType.String;
                codRefP1P2.VisibleIndex = 2;
                codRefP1P2.Width = 90;
                codRefP1P2.Visible = true;
                this.gvDetail.Columns.Add(codRefP1P2);

                //Descripcion
                GridColumn desc = new GridColumn();
                desc.FieldName = this.unboundPrefix + "ReferenciaIDP1P2Desc";
                desc.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_DescripTExt");
                desc.UnboundType = UnboundColumnType.String;
                desc.VisibleIndex = 3;
                desc.Width = 120;
                desc.Visible = true;
                this.gvDetail.Columns.Add(desc);

                //Estado
                GridColumn estado = new GridColumn();
                estado.FieldName = this.unboundPrefix + "EstadoInv";
                estado.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_EstadoInv");
                estado.UnboundType = UnboundColumnType.Integer;
                estado.VisibleIndex = 4;
                estado.Width = 60;
                estado.Visible = true;
                this.gvDetail.Columns.Add(estado);

                //Serial
                GridColumn serial = new GridColumn();
                serial.FieldName = this.unboundPrefix + "SerialID";
                serial.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_SerialID");
                serial.UnboundType = UnboundColumnType.String;
                serial.VisibleIndex = 5;
                serial.Width = 90;
                serial.Visible = true;
                this.gvDetail.Columns.Add(serial);

                //EmpaqueInvID
                GridColumn empaqueID = new GridColumn();
                empaqueID.FieldName = this.unboundPrefix + "EmpaqueInvID";
                empaqueID.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_EmpaqueInvID");
                empaqueID.UnboundType = UnboundColumnType.String;
                empaqueID.VisibleIndex = 7;
                empaqueID.Width = 70;
                empaqueID.Visible = true;
                this.gvDetail.Columns.Add(empaqueID);

                //CantidadEmpaques
                GridColumn cantidadEmpaques = new GridColumn();
                cantidadEmpaques.FieldName = this.unboundPrefix + "CantidadEMP";
                cantidadEmpaques.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_CantidadEMP");
                cantidadEmpaques.UnboundType = UnboundColumnType.Integer;
                cantidadEmpaques.VisibleIndex = 8;
                cantidadEmpaques.Width = 60;
                cantidadEmpaques.Visible = true;
                this.gvDetail.Columns.Add(cantidadEmpaques);

                //Valor1LOC
                GridColumn valorML = new GridColumn();
                valorML.FieldName = this.unboundPrefix + "Valor1LOC";
                valorML.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorML");
                valorML.UnboundType = UnboundColumnType.Decimal;
                valorML.VisibleIndex = 9;
                valorML.Width = 120;
                valorML.ColumnEdit = this.editSpin;
                valorML.Visible = true;
                this.gvDetail.Columns.Add(valorML);

                //Valor1EXT
                GridColumn valorME = new GridColumn();
                valorME.FieldName = this.unboundPrefix + "Valor1EXT";
                valorME.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorME");
                valorME.UnboundType = UnboundColumnType.Decimal;
                valorME.VisibleIndex = 10;
                valorME.Width = 120;
                valorME.ColumnEdit = this.editSpin;
                valorME.Visible = true;
                this.gvDetail.Columns.Add(valorME);

                //PorcDistribucion
                GridColumn PorcDistribucion = new GridColumn();
                PorcDistribucion.FieldName = this.unboundPrefix + "PorcDistribucion";
                PorcDistribucion.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_PorcDistribucion");
                PorcDistribucion.UnboundType = UnboundColumnType.Decimal;
                PorcDistribucion.VisibleIndex = 10;
                PorcDistribucion.Width = 70;
                PorcDistribucion.Visible = true;
                PorcDistribucion.ColumnEdit = this.editSpinPorcen;
                this.gvDetail.Columns.Add(PorcDistribucion);

                //ValorDistribucionML
                GridColumn ValorDistribuyeML = new GridColumn();
                ValorDistribuyeML.FieldName = this.unboundPrefix + "ValorDistribucionML";
                ValorDistribuyeML.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorDistribuyeML");
                ValorDistribuyeML.UnboundType = UnboundColumnType.Decimal;
                ValorDistribuyeML.VisibleIndex = 10;
                ValorDistribuyeML.Width = 120;
                ValorDistribuyeML.ColumnEdit = this.editSpin;
                ValorDistribuyeML.Visible = true;
                this.gvDetail.Columns.Add(ValorDistribuyeML);

                //ValorDistribucionME
                GridColumn ValorDistribuyeME = new GridColumn();
                ValorDistribuyeME.FieldName = this.unboundPrefix + "ValorDistribucionME";
                ValorDistribuyeME.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorDistribuyeME");
                ValorDistribuyeME.UnboundType = UnboundColumnType.Decimal;
                ValorDistribuyeME.VisibleIndex = 10;
                ValorDistribuyeME.Width = 120;
                ValorDistribuyeME.ColumnEdit = this.editSpin;
                ValorDistribuyeME.Visible = true;
                this.gvDetail.Columns.Add(ValorDistribuyeME);

                #endregion
                #region Columnas No Visibles

                //CodigoReferencia
                GridColumn codRef = new GridColumn();
                codRef.FieldName = this.unboundPrefix + "inReferenciaID";
                //codRef.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_inReferenciaID");
                codRef.UnboundType = UnboundColumnType.String;
                codRef.VisibleIndex = 2;
                codRef.Width = 70;
                codRef.Visible = false;
                this.gvDetail.Columns.Add(codRef);

                //Parameter1
                GridColumn param1 = new GridColumn();
                param1.FieldName = this.unboundPrefix + "Parametro1";
                param1.UnboundType = UnboundColumnType.String;
                param1.Visible = false;
                this.gvDetail.Columns.Add(param1);

                //Parameter2
                GridColumn param2 = new GridColumn();
                param2.FieldName = this.unboundPrefix + "Parametro2";
                param2.UnboundType = UnboundColumnType.String;
                param2.Visible = false;
                this.gvDetail.Columns.Add(param2);

                //Unidad
                GridColumn unidadRef = new GridColumn();
                unidadRef.FieldName = this.unboundPrefix + "UnidadRef";
                unidadRef.UnboundType = UnboundColumnType.String;
                unidadRef.Visible = false;
                this.gvDetail.Columns.Add(unidadRef);

                //IdentificadorTr
                GridColumn param3 = new GridColumn();
                param3.FieldName = this.unboundPrefix + "IdentificadorTr";
                param3.UnboundType = UnboundColumnType.Integer;
                param3.Visible = false;
                this.gvDetail.Columns.Add(param3);

                //ValorUnitario
                GridColumn vlrUnitario = new GridColumn();
                vlrUnitario.FieldName = this.unboundPrefix + "ValorUNI";
                vlrUnitario.UnboundType = UnboundColumnType.Decimal;
                vlrUnitario.Visible = false;
                this.gvDetail.Columns.Add(vlrUnitario);

                //Cantidad
                GridColumn cant = new GridColumn();
                cant.FieldName = this.unboundPrefix + "CantidadUNI";
                cant.UnboundType = UnboundColumnType.Decimal;
                cant.Visible = false;
                this.gvDetail.Columns.Add(cant);

                //Indice de la fila
                GridColumn colIndex = new GridColumn();
                colIndex.FieldName = this.unboundPrefix + "Index";
                colIndex.UnboundType = UnboundColumnType.Integer;
                colIndex.Visible = false;
                this.gvDetail.Columns.Add(colIndex);

                //valorFOBML
                GridColumn valorFOBML = new GridColumn();
                valorFOBML.FieldName = this.unboundPrefix + "Valor2LOC";
                valorFOBML.Visible = false;
                valorFOBML.UnboundType = UnboundColumnType.Decimal;

                this.gvDetail.Columns.Add(valorFOBML);

                //valorFOBME
                GridColumn valorFOBME = new GridColumn();
                valorFOBME.FieldName = this.unboundPrefix + "Valor2EXT";
                valorFOBME.Visible = false;
                valorFOBME.UnboundType = UnboundColumnType.Decimal;
                this.gvDetail.Columns.Add(valorFOBME);

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "DistribucionCostos.cs-AddDetailCols"));
            }
        }

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.DistribucionCostos;
            this.frmModule = ModulesPrefix.@in;
            this.dataFactura = new List<DTO_inDistribucionCosto>();
            this._ctrl = new DTO_glDocumentoControl();

            InitializeComponent();
            base.SetInitParameters();

            this.AddDocumentCols();
            this.AddDetailCols();
            
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
            this.tlSeparatorPanel.RowStyles[0].Height = 80;
            this.tlSeparatorPanel.RowStyles[1].Height = 40;
            this.tlSeparatorPanel.RowStyles[2].Height = 390;

            this.gvDocument.OptionsBehavior.AutoPopulateColumns = true;
            this.gvDetail.OptionsBehavior.AutoPopulateColumns = true;
            this.gcDocument.UseEmbeddedNavigator = false;

            //Carga controles del Header
            this._bc.InitMasterUC(this.masterPrefijo, AppMasters.glPrefijo, true, true, true, false);

            if (this.multiMoneda)
            {
                this.lblValorDistribucionME.Visible = true;
                this.lblValorMvtoME.Visible = true;
                this.txtValorDistribucionME.Visible = true;
                this.txtValorMvtoME.Visible = true;
            }
        }

        /// <summary>
        /// Carga la información de las grilla
        /// </summary>
        /// <param name="firstTime">Si es la primera vez que carga la grilla</param>
        /// <param name="refresh">Si debe volver a cargar la data del servidor</param>
        protected override void LoadData(bool firstTime)
        {
            this.gcDetail.DataSource = this.dataDetail.Footer;
            this.gcDetail.RefreshDataSource();
            bool hasItems = this.dataDetail.Footer.GetEnumerator().MoveNext() ? true : false;
            if (hasItems)
                this.gvDetail.MoveFirst();
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
        /// Agrega una nueva fila
        /// </summary>
        protected override void AddNewRow()
        {
            DTO_inDistribucionCosto footerFactura = new DTO_inDistribucionCosto();
            if (this.dataFactura == null)
                this.dataFactura = new List<DTO_inDistribucionCosto>();
            try
            {
                #region Asigna datos a la fila

                footerFactura.ProveedorID.Value = string.Empty;
                footerFactura.ProveedorDesc.Value = string.Empty;
                footerFactura.FacturaNro.Value = string.Empty;
                footerFactura.MonedaOrigen.Value = string.Empty;
                footerFactura.NumeroDocCto.Value =  0;
                footerFactura.NumeroDocINV.Value = 0;
                footerFactura.ValorML.Value = 0;
                footerFactura.ValorME.Value = 0;
                footerFactura.Valor.Value = 0;

                #endregion            
    
                this.dataFactura.Add(footerFactura);
                this.gcDocument.DataSource = this.dataFactura;
                this.gvDocument.RefreshData();
                this.gcDocument.RefreshDataSource();
                try { this.gvDocument.FocusedRowHandle = this.gvDocument.DataRowCount - 1; } catch (Exception) { ;}
                this.isValid = false;
                base.gcDocument.EmbeddedNavigator.Buttons.CustomButtons[1].Enabled = this.gvDocument.DataRowCount > 1 ? true : false;                
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-DistribucionCostos.cs", "AddNewRow: " + ex.Message));
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
                #region ProveedorID
                validField = this._bc.ValidGridCell(this.gvDocument, string.Empty, fila, "ProveedorID", false, true, false, AppMasters.prProveedor);
                if (!validField)
                    validRow = false;
                #endregion
                #region FacturaNro
                validField = this._bc.ValidGridCell(this.gvDocument, string.Empty, fila, "FacturaNro", false, false, false, null);
                if (validField)                    
                {
                    GridColumn col = this.gvDocument.Columns[this.unboundPrefix + "FacturaNro"];
                    string factura = this.gvDocument.GetRowCellValue(fila, this.unboundPrefix + "FacturaNro").ToString();
                    DTO_CuentaXPagar cxpFactura = this._bc.AdministrationModel.CuentasXPagar_GetForCausacion(AppDocuments.CausarFacturas, this._terceroID, factura, false);
                    if (cxpFactura == null)
                    {
                        string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_FacturaInvalid), factura);
                        this.gvDocument.SetColumnError(col, msg);
                        validField = false;
                    }
                }
                if (!validField)
                    validRow = false;
                #endregion
                #endregion
                #region Validaciones de valores
                #region ValorML
                validField = this._bc.ValidGridCellValue(this.gvDocument, string.Empty, fila, "ValorML", false, true, true, false);
                if (!validField)
                    validRow = false;
                #endregion
                #region ValorME
                validField = this._bc.ValidGridCellValue(this.gvDocument, string.Empty, fila, "ValorME", false, true, true, false);
                if (!validField)
                    validRow = false;
                #endregion
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
            FormProvider.Master.itemSave.Enabled = FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
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
        /// Valida que el usuario haya ingresado un comprobante existente
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtNroDocInv_Enter(object sender, EventArgs e)
        {
            if (!this.masterPrefijo.ValidID)
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterPrefijo.CodeRsx);
                MessageBox.Show(msg);
                this.masterPrefijo.Focus();
            }
        }

        /// <summary>
        /// Valida que solo numeros se pueden escribir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNroDocInv_KeyPress(object sender, KeyPressEventArgs e)
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
        private void txtNroDocInv_Leave(object sender, EventArgs e)
        { 
            try
            {
                if (!string.IsNullOrEmpty(this.txtNroDocInv.Text) && this.txtNroDocInv.Text != "0")
                {
                    //Valida que exista el documento
                    DTO_glDocumentoControl docCtrlExist = null;
                    docCtrlExist = this._bc.AdministrationModel.glDocumentoControl_GetInternalDoc(AppDocuments.NotaEnvio, this.masterPrefijo.Value, Convert.ToInt32(this.txtNroDocInv.Text));
                    if (docCtrlExist == null)
                        docCtrlExist = this._bc.AdministrationModel.glDocumentoControl_GetInternalDoc(AppDocuments.TransaccionAutomatica, this.masterPrefijo.Value, Convert.ToInt32(this.txtNroDocInv.Text));

                    if (docCtrlExist != null && docCtrlExist.Estado.Value.Value == (byte)EstadoDocControl.Aprobado)
                        this.GetMvtoInventario(docCtrlExist);
                    else
                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_InvalidNumberTransaccion));                 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-DistribucionCostos.cs", "txtNroDocInv_Leave: " + ex.Message));
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
            docs.Add(AppDocuments.DistribucionCostos);
            ModalQueryDocument getDocControl = new ModalQueryDocument(docs);
            getDocControl.ShowDialog();
            if (getDocControl.DocumentoControl != null)
            {
                if (getDocControl.CopiadoInd)
                    this._copyData = true;
                this.GetMvtoInventario(getDocControl.DocumentoControl);
            }
        }

        #endregion

        #region Eventos Grilla Facturas

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>  
        protected override void gvDocument_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            try
            {
                if (fieldName == "ProveedorID")
                {
                    DTO_prProveedor proveedor = (DTO_prProveedor)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.prProveedor, false, e.Value.ToString(), true);
                    if (proveedor != null)
                    {
                        this.dataFactura[e.RowHandle].ProveedorDesc.Value = proveedor.Descriptivo.Value;
                        this._terceroID = proveedor.TerceroID.Value;
                    }
                }
                if (fieldName == "FacturaNro")
                {
                    DTO_CuentaXPagar cxpFactura = this._bc.AdministrationModel.CuentasXPagar_GetForCausacion(AppDocuments.CausarFacturas, this._terceroID, e.Value.ToString(), false);
                    if (cxpFactura != null && cxpFactura.DocControl.Estado.Value == (byte)EstadoDocControl.Aprobado)
                    {
                        #region Trae valores de la factura
                        this.GetValorFactura(e.RowHandle, cxpFactura);
                        this._facturaNro = e.Value.ToString();
                        #endregion
                    }
                    else
                    {
                        #region Valida la existencia y estado de la factura
                        string msg = string.Empty;
                        if (cxpFactura == null)
                            msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_FacturaInvalid), e.Value.ToString());
                        else
                            msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_NoRadicado), e.Value.ToString());
                        this.dataFactura[e.RowHandle].MonedaOrigen.Value = string.Empty;
                        this.dataFactura[e.RowHandle].NumeroDocCto.Value = 0;
                        this.dataFactura[e.RowHandle].Valor.Value = 0;
                        this.gvDocument.SetColumnError(e.Column, msg);
                        this.gcDocument.RefreshDataSource();
                        return;
                        #endregion
                    }
                    //Realiza la distribucion
                    this.CalcularDistribucionCto(false);
                    this.gvDocument.SetColumnError(e.Column, string.Empty);
                 }
                 this.gcDocument.RefreshDataSource();
            }
            catch (Exception ex)
            {
               MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-DistribucionCostos.cs", "gvDocument_CellValueChanged"));
            }
        }

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
                    //Carga el docControl para el documentoActual
                    this.dataDetail.DocCtrl = this.LoadDocCtrl();
                    this.LoadData(true);
                }
                else
                    this.masterPrefijo.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp", "TransaccionManual.cs-gcDocument_Enter: " + ex.Message));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al presionar un boton por defecto de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gcDocument_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            if (this.dataFactura != null && this.dataDetail.Footer.Count > 0)
            {
                if (this.validHeader)
                {
                    if (this.dataFactura == null)
                    {
                        this.gcDocument.Focus();
                        e.Handled = true;
                    }

                    #region Agrega Item
                    if (e.Button.ImageIndex == 6)
                    {
                        if (this.gvDocument.ActiveFilterString != string.Empty)
                            MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoAddInFilter));
                        else
                        {
                            this.deleteOP = false;
                            if (this.isValid)
                            {
                                this.newReg = true;
                                this.AddNewRow();
                            }
                            else
                            {
                                bool isV = this.ValidateRow(this.gvDocument.FocusedRowHandle);
                                if (isV)
                                {
                                    this.newReg = true;
                                    this.AddNewRow();
                                }
                            }
                        }
                    } 
                    #endregion
                    #region Borra Item
                    if (e.Button.ImageIndex == 7)
                    {
                        string msgTitleDelete = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Delete);
                        string msgDelete = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Register);
                        //Revisa si desea cargar los temporales
                        if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            this.deleteOP = true;
                            int rowHandle = this.gvDocument.FocusedRowHandle;

                            if (this.dataFactura.Count == 1)
                            {
                                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.RowsNeeded));
                                e.Handled = true;
                            }
                            else
                            {
                                //Si borra el primer registro
                                if (rowHandle == 0)
                                    this.gvDocument.FocusedRowHandle = 0;
                                //Si selecciona el ultimo
                                else
                                    this.gvDocument.FocusedRowHandle = rowHandle - 1;

                                this.gvDocument.RefreshData();
                                this.RowIndexChanged(this.gvDocument.FocusedRowHandle, true);

                            }
                        } 
                    #endregion
                    e.Handled = true;
                    }
                } 
            }
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
                base.gcDocument.EmbeddedNavigator.Buttons.CustomButtons[1].Enabled = this.dataDetail.Footer.Count > 1 ? true : false;
                this.deleteOP = false;

                if (validRow)
                {
                    this.isValid = true;
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

        #region Eventos Grilla Detalles

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetail_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
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
                            DTO_inMovimientoFooter dtoM = (DTO_inMovimientoFooter)e.Row;
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
                            DTO_inMovimientoFooter dtoM = (DTO_inMovimientoFooter)e.Row;
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
                if (this.ValidGrid() && this._valorTotalDistribucionML != 0)
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
                if (this.dataDetail.DocCtrl.NumeroDoc.Value.Value != 0)
                {
                    update = true;
                    numeroDoc = this.dataDetail.DocCtrl.NumeroDoc.Value.Value;
                }

                #region Prepara el detalle de la Distribucion
                foreach (var deta in this.dataDetail.Footer)
                {
                    deta.Movimiento.EntradaSalida.Value = (byte)EntradaSalida.Entrada;
                    deta.Movimiento.NumeroDoc.Value = 0;
                    deta.Movimiento.CantidadUNI.Value = 0;
                    deta.Movimiento.CantidadEMP.Value = 0;
                    deta.Movimiento.CantidadDoc.Value = 0;
                    deta.Movimiento.Valor1LOC.Value = deta.ValorDistribucionML.Value;
                    deta.Movimiento.Valor1EXT.Value = deta.ValorDistribucionME.Value;
                    deta.Movimiento.ValorUNI.Value = 0;
                    deta.Movimiento.Fecha.Value = this.dtFecha.DateTime;
                    deta.Movimiento.DocSoporte.Value = deta.Movimiento.Consecutivo.Value;
                    deta.Movimiento.DocSoporteTER.Value = this._facturaNro;
                    deta.Movimiento.Consecutivo.Value = 0;                   
                } 
                #endregion

                DTO_SerializedObject obj = this._bc.AdministrationModel.inDistribucionCostos_Add(this.documentID, this.dataDetail,this.dataFactura, update, out numeroDoc);
                FormProvider.Master.StopProgressBarThread(this.documentID);

                bool isOK = this._bc.SendDocumentMail(MailType.NewDoc, this.documentID, this._actFlujo.seUsuarioID.Value, obj, true);
                if (isOK)
                {
                    this._bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), this._bc.AdministrationModel.User);

                    this.newDoc = true;
                    this.deleteOP = true;
                    this.dataDetail = new DTO_MvtoInventarios();
                    this.dataFactura = new List<DTO_inDistribucionCosto>();
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
