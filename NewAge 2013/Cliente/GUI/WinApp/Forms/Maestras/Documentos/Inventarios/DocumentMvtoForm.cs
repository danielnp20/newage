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
using DevExpress.XtraEditors;
using System.Globalization;
using System.Threading;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Maestra de comprobante auxiliar
    /// </summary>
    public partial class DocumentMvtoForm : DocumentForm
    {
        public DocumentMvtoForm()
        {
          //InitializeComponent();
        }
        
        #region Delegados

        /// <summary>
        /// Delegado que finaliza el proceso de importación
        /// </summary>
        protected override void RefreshGridMethod()
        {
            this.LoadData(true);
        }

        /// <summary>
        /// Delegado que finaliza el proceso de validacion 
        /// </summary>
        public delegate void EndImportar();
        public EndImportar endImportarDelegate;
        public void EndImportarMethod()
        {
            this.LoadData(true);
            this.EnableFooter(true);
            this.txtTotalValorLoc.EditValue = this._valorTotalML;
            this.txtTotalValorExt.EditValue = this._valorTotalME;
            this.txtTotalCantidad.EditValue = this._totalUnidades;
        }
        #endregion

        #region variables privadas
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        private List<DTO_glConsultaFiltro> filtrosParam1 = new List<DTO_glConsultaFiltro>();
        private List<DTO_glConsultaFiltro> filtrosParam2 = new List<DTO_glConsultaFiltro>();
        private DTO_inReferencia _referenciaInv;
        private DTO_inCostosExistencias _costos;
        private decimal _unidadesFinalxRef = 0;
        private bool _serializadoInd = false;
        private bool _editGridInd = false;
        private bool _usoReferenciaCod = false;

        #endregion

        #region Variables Protected
        //Variables Moneda
        protected string monedaLocal;
        protected string monedaExtranjera;
        protected string monedaId;
        //Indica si el header es valido
        protected bool validHeader;
        //variables para funciones particulares
        protected bool cleanDoc = true;
        protected bool indCostosFOB = false;
        protected bool isFacturaVenta = false;
        protected DTO_inMovimientoTipo mvtoTipo = null;
        protected TipoMovimientoInv tipoMovActual;
        protected DTO_inBodega bodegaOrigen = null;
        protected DTO_inBodega bodegaDestino = null;
        protected DTO_inCosteoGrupo costeoGrupoOri = null;
        protected DTO_inCosteoGrupo costeoGrupoDest = null;
        protected DTO_MvtoInventarios data = null;
        protected string proyectoHeader = string.Empty;
        protected string centroCostoHeader = string.Empty;
        protected string terceroHeader = string.Empty;
        protected string _bodegaTransacxDef = string.Empty;
        protected decimal _totalUnidades = 0;
        protected decimal _valorTotalML = 0;
        protected decimal _valorTotalME = 0;
        protected decimal _tasaCambioValue = 0;
        protected int _documentoNro = 0;
        protected string param1xDef = string.Empty;
        protected string param2xDef = string.Empty;
        protected bool _copyData = false;
        protected string _empaqueInvIdDef = string.Empty;
        protected string formatReferencias = string.Empty;
        //Modulo Proyectos
        protected bool _moduloProyectosInd = false;
        protected DTO_SolicitudTrabajo _proyectoInfo = null;
        protected bool moduleProyectosActiveInd = false;
        protected List<DTO_inMovimientoFooter> mvtoStockBodOrigen = new List<DTO_inMovimientoFooter>();
        protected DTO_inMovimientoFooter _rowCurrent = new DTO_inMovimientoFooter();
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
                return this.data.Footer.FindIndex(det => det.Movimiento.Index == this.indexFila);
            }
        }

        #endregion

        #region Funciones Privadas y protected

        /// <summary>
        /// Genera el reporte del comprobante actual
        /// </summary>
        /// <param name="show">Indica si se debe mostrar o solo generar</param>
        /// <returns>Reporte</returns>
        private ComprobanteReport GenerateReport(bool show, bool allFields=false)
        {
            switch (this.documentID)
            {
                case AppDocuments.TransaccionManual:                    
                    return null;
                default:
                    return null;
            };
        }

        /// <summary>
        /// Revisa si el documento actual tiene temporales
        /// </summary>
        /// <returns></returns>
        private bool HasTemporales()
        {
            return _bc.AdministrationModel.aplTemporales_HasTemp(this.documentID.ToString(), _bc.AdministrationModel.User);
        }

        /// <summary>
        /// Calcula el saldo de existencias en las salidas y traslados
        /// </summary>
        private void GetSaldoAvailable(DTO_inMovimientoFooter det, bool isImportData)
        {
            try
            {
                DTO_inControlSaldosCostos saldos;
                if (!isImportData)
                {
                    #region Cuando es manual
                    if (!this.gvDocument.IsFocusedView && !string.IsNullOrEmpty(det.Movimiento.inReferenciaID.Value)
                        && !string.IsNullOrEmpty(det.Movimiento.Parametro1.Value) && !string.IsNullOrEmpty(det.Movimiento.Parametro2.Value) &&
                        !string.IsNullOrEmpty(det.Movimiento.EstadoInv.Value.ToString()) && det.Movimiento.ActivoID.Value != null && !this.isFacturaVenta)
                    {
                        this._costos = new DTO_inCostosExistencias();
                        saldos = new DTO_inControlSaldosCostos();
                        this._referenciaInv = (DTO_inReferencia)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, false, det.Movimiento.inReferenciaID.Value, true);
                        saldos.BodegaID.Value = this.documentID == AppDocuments.NotaEnvio && this.tipoMovActual == TipoMovimientoInv.Traslados && this._documentoNro != 0 ? this._bodegaTransacxDef : this.bodegaOrigen.ID.Value;
                        saldos.inReferenciaID.Value = det.Movimiento.inReferenciaID.Value;
                        saldos.ActivoID.Value = this._serializadoInd && det.Movimiento.ActivoID.Value != 0 ? det.Movimiento.ActivoID.Value : null;
                        saldos.EstadoInv.Value = det.Movimiento.EstadoInv.Value;
                        saldos.Parametro1.Value = det.Movimiento.Parametro1.Value;
                        saldos.Parametro2.Value = det.Movimiento.Parametro2.Value;
                        saldos.IdentificadorTr.Value = this.costeoGrupoOri.CosteoTipo.Value == (byte)TipoCosteoInv.Transaccional ? det.Movimiento.Consecutivo.Value : null;
                        det.CantidadDispon.Value = this._bc.AdministrationModel.Transaccion_SaldoExistByReferencia(documentID, saldos, ref _costos);
                        if (det.CantidadDispon.Value != 0)
                        {
                            DTO_inEmpaque empaqueRef = (DTO_inEmpaque)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inEmpaque, false, _referenciaInv.EmpaqueInvID.Value, true);
                            det.UnidadRef.Value = this._referenciaInv.UnidadInvID.Value;
                            this.masterUnidad.Value = this._referenciaInv.UnidadInvID.Value;
                            this.txtSaldoRef.EditValue = this._rowCurrent.CantidadDispon.Value;
                            this.txtCantUnidad.Enabled = !this._serializadoInd;
                            if (det.Movimiento.CantidadUNI.Value.Value > det.CantidadDispon.Value)
                                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_NotExistSaldoInventory));
                        }
                        else
                        {
                            MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_NotExistSaldoInventory));
                            this.masterUnidad.Value = this._referenciaInv.UnidadInvID.Value;
                            this.txtSaldoRef.EditValue = 0;
                            this.txtCantUnidad.Enabled = false;
                        }
                    }
                    #endregion
                }
                else
                {
                    #region Cuando es importacion data
                    if (!string.IsNullOrEmpty(det.Movimiento.inReferenciaID.Value)
                                && !string.IsNullOrEmpty(det.Movimiento.Parametro1.Value) && !string.IsNullOrEmpty(det.Movimiento.Parametro2.Value) &&
                                !string.IsNullOrEmpty(det.Movimiento.EstadoInv.Value.ToString()) && det.Movimiento.ActivoID.Value != null && !this.isFacturaVenta)
                    {
                        this._costos = new DTO_inCostosExistencias();
                        saldos = new DTO_inControlSaldosCostos();
                        this._referenciaInv = (DTO_inReferencia)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, false, det.Movimiento.inReferenciaID.Value, true);
                        saldos.BodegaID.Value = this.documentID == AppDocuments.NotaEnvio && this.tipoMovActual == TipoMovimientoInv.Traslados && this._documentoNro != 0 ? this._bodegaTransacxDef : this.bodegaOrigen.ID.Value;
                        saldos.inReferenciaID.Value = det.Movimiento.inReferenciaID.Value;
                        saldos.ActivoID.Value = this._serializadoInd && det.Movimiento.ActivoID.Value != 0 ? det.Movimiento.ActivoID.Value : null;
                        saldos.EstadoInv.Value = det.Movimiento.EstadoInv.Value;
                        saldos.Parametro1.Value = det.Movimiento.Parametro1.Value;
                        saldos.Parametro2.Value = det.Movimiento.Parametro2.Value;
                        saldos.IdentificadorTr.Value = this.costeoGrupoOri.CosteoTipo.Value == (byte)TipoCosteoInv.Transaccional ? det.Movimiento.Consecutivo.Value : null;
                        det.CantidadDispon.Value = this._bc.AdministrationModel.Transaccion_SaldoExistByReferencia(documentID, saldos, ref _costos);
                        if (det.CantidadDispon.Value != 0)
                        {
                            DTO_inEmpaque empaqueRef = (DTO_inEmpaque)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inEmpaque, false, _referenciaInv.EmpaqueInvID.Value, true);
                            det.UnidadRef.Value = this._referenciaInv.UnidadInvID.Value;
                            //this.masterUnidad.Value = this._referenciaInv.UnidadInvID.Value;
                            //this.txtSaldoRef.EditValue = this._rowCurrent.CantidadDispon.Value;
                            //this.txtCantUnidad.Enabled = !this._serializadoInd;
                            if (det.Movimiento.CantidadUNI.Value.Value > det.CantidadDispon.Value)
                            {
                                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_NotExistSaldoInventory) + " : " + det.Movimiento.inReferenciaID.Value);
                                det.Movimiento.CantidadEMP.Value = 0;
                                det.Movimiento.CantidadUNI.Value = 0;
                            }
                        }
                        else
                        {
                            MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_NotExistSaldoInventory));
                            this._rowCurrent.CantidadDispon.Value = 0;
                            //this.masterUnidad.Value = this._referenciaInv.UnidadInvID.Value;
                            //this.txtSaldoRef.EditValue = 0;
                            //this.txtCantUnidad.Enabled = false;
                        }
                    }
                    #endregion
                }
            }
            catch (Exception)
            {
                throw;
            }           
        }

        /// <summary>
        /// Actualiza la informacion de los temporales
        /// </summary>
        protected void UpdateTemp(object data)
        {
            try
            {
              this._bc.AdministrationModel.aplTemporales_Save(this.documentID.ToString(), this._bc.AdministrationModel.User, data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Obtiene la info del Modulo de Proyectos
        /// </summary>
        protected void GetInfoProyecto(string proyectoID)
        {
            try
            {
                this._proyectoInfo = this._bc.AdministrationModel.SolicitudProyecto_Load(AppDocuments.Proyecto, prefijoID, null, null, string.Empty, proyectoID, false, true, false, false);

                if (this._proyectoInfo != null)
                {
                    if (this._proyectoInfo.DocCtrl.Estado.Value != (byte)EstadoDocControl.Aprobado)
                    {
                        MessageBox.Show("El Proyecto relacionado a la bodega no se encuentra Aprobado");
                        return;
                    }
                    foreach (var mvto in this.mvtoStockBodOrigen)
                    {
                        DTO_pyProyectoMvto mvtoProy = this._proyectoInfo.Movimientos.Find(x => x.inReferenciaID.Value == mvto.Movimiento.inReferenciaID.Value);
                        //mvto.Movimiento.CantidadEMP.Value = mvtoProy != null ? mvtoProy.CantidadTOT.Value - mvtoProy.CantidadSOL.Value - mvtoProy.CantidadPROV.Value - mvtoProy.CantidadINV.Value : 0;
                        //mvto.Movimiento.CantidadUNI.Value = mvto.Movimiento.CantidadEMP.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentMvtoForm", "GetInfoProyecto"));
            }
        }

        /// <summary>
        /// Obtiene las existencias de la bodega
        /// </summary>
        protected void GetStockByBodega(string bodega)
        {
            try
            {
                DTO_inControlSaldosCostos filterInv = new DTO_inControlSaldosCostos();
                int indexStock = 0;
                filterInv.BodegaID.Value = bodega;
                var mvtosxBodega = this._bc.AdministrationModel.inControlSaldosCostos_GetByParameter(this.documentID, filterInv);
                foreach (DTO_inControlSaldosCostos inv in mvtosxBodega)
                {
                    #region  Trae valores y existencias disponibles
                    DTO_inCostosExistencias costos = new DTO_inCostosExistencias();
                    inv.CantidadDisp.Value = this._bc.AdministrationModel.Transaccion_SaldoExistByReferencia(this.documentID, inv, ref costos);
                    inv.ValorLocalDisp.Value = costos.CtoLocSaldoIni.Value + costos.CtoLocEntrada.Value - costos.CtoLocSalida.Value;
                    inv.ValorExtranjeroDisp.Value = costos.CtoExtSaldoIni.Value + costos.CtoExtEntrada.Value - costos.CtoExtSalida.Value;
                    inv.ValorFobLocalDisp.Value = costos.FobLocSaldoIni.Value + costos.FobLocEntrada.Value - costos.FobLocSalida.Value;
                    inv.ValorFobExtDisp.Value = costos.FobExtSaldoIni.Value + costos.FobExtEntrada.Value - costos.FobExtSalida.Value;
                    #endregion
                    #region Agrega stock
                    DTO_inMovimientoFooter exist = new DTO_inMovimientoFooter();
                    exist.Movimiento.Index = indexStock;
                    exist.Movimiento.EmpresaID.Value = this.empresaID;
                    exist.Movimiento.TerceroID.Value = this.documentID == AppDocuments.TransaccionManual ? this.terceroHeader : string.Empty;
                    exist.Movimiento.ProyectoID.Value = this.proyectoHeader;
                    exist.Movimiento.CentroCostoID.Value = this.centroCostoHeader;
                    exist.Movimiento.BodegaID.Value = inv.BodegaID.Value;
                    exist.Movimiento.inReferenciaID.Value = inv.inReferenciaID.Value;
                    exist.Movimiento.EstadoInv.Value = inv.EstadoInv.Value;
                    exist.Movimiento.Parametro1.Value = inv.Parametro1.Value;
                    exist.Movimiento.Parametro2.Value = inv.Parametro2.Value;
                    exist.ReferenciaIDP1P2.Value = inv.inReferenciaID.Value + (inv.Parametro1.Value == this.param1xDef.ToUpper() || string.IsNullOrEmpty(inv.Parametro1.Value) ? string.Empty : "-" + inv.Parametro1.Value) + (inv.Parametro2.Value == this.param2xDef.ToUpper() || string.IsNullOrEmpty(inv.Parametro2.Value) ? string.Empty : "-" + inv.Parametro2.Value);
                    exist.Movimiento.ActivoID.Value = inv.ActivoID.Value;
                    exist.Movimiento.MvtoTipoInvID.Value = this.mvtoTipo.ID.Value;
                    exist.Movimiento.IdentificadorTr.Value = inv.IdentificadorTr.Value;
                    exist.Movimiento.SerialID.Value = inv.SerialID.Value;
                    exist.Movimiento.EmpaqueInvID.Value = this._empaqueInvIdDef;
                    exist.Movimiento.DescripTExt.Value = inv.ReferenciaP1P2Desc.Value;
                    exist.Movimiento.CantidadDoc.Value = 0;
                    exist.Movimiento.CantidadEMP.Value = inv.CantidadDisp.Value;
                    exist.Movimiento.CantidadUNI.Value = inv.CantidadDisp.Value;
                    exist.Movimiento.EntradaSalida.Value = (byte)EntradaSalida.Salida;
                    exist.Movimiento.ValorUNI.Value = inv.CantidadDisp.Value != 0 ? inv.ValorLocalDisp.Value / inv.CantidadDisp.Value : 0;
                    exist.Movimiento.DocSoporte.Value = 0;
                    exist.Movimiento.DocSoporteTER.Value = string.Empty;
                    exist.Movimiento.Valor1LOC.Value = inv.ValorLocalDisp.Value;
                    exist.Movimiento.Valor2LOC.Value = 0;
                    exist.Movimiento.Valor1EXT.Value = 0;
                    exist.Movimiento.Valor2EXT.Value = 0;
                    exist.Movimiento.DocSoporte.Value = inv.DetalleMvto.Count > 0 ? inv.DetalleMvto.First().DocSoporte.Value : 0;
                    if (inv.CantidadDisp.Value > 0)
                        this.mvtoStockBodOrigen.Add(exist);
                    #endregion
                    indexStock++;
                }  
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentMvtoForm", "GetInfoProyecto"));
            }
        }

        /// <summary>
        /// Valida un DTO de comprobante footer en la importacion
        /// </summary>
        /// <param name="dtoInsumo">DTO a validar</param>
        /// <param name="rd">Variable que va guardando los mensajes de error</param>
        /// <param name="msgFecha">Mensaje que indica que la fecha esta en un periodo incorrecto</param>
        /// <param name="msgNoRel">Mensaje que indica que toca tener un valor de cliente, codigo de empleado o libranza</param>
        /// <param name="msgPositive">Mensaje de solo acepta valores positivos</param>
        /// <param name="msgEmptyField">Mensaje de campo vacio</param>
        /// <param name="msgClienteRepetido">Mensaje para cliente repetido sin libranza</param>
        /// <param name="msgCodCliente">Mensaje para indicar que no se puede poner el codigo y el cliente</param>
        private void ValidateDataImport(DTO_MigracionMvtos dto, DTO_TxResultDetail rd, string msgFkNotFound, string msgEmptyField)
        {
            try
            {                
                if (dto != null)
                {
                    #region Validacion TareaCliente
                    if (string.IsNullOrWhiteSpace(dto.inReferenciaID.Value.ToString()))
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_inReferenciaID");
                        rdF.Message = msgEmptyField;
                        rd.DetailsFields.Add(rdF);
                    }                   
                    #endregion                    
                    #region Validacion Cantidad
                    if (string.IsNullOrWhiteSpace(dto.CantidadEMP.Value.ToString()))
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CantidadEMP");
                        rdF.Message = msgEmptyField;
                        rd.DetailsFields.Add(rdF);
                    }
                    #endregion
                    #region Validacion Valor
                    if (string.IsNullOrWhiteSpace(dto.ValorUNI.Value.ToString()))
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_Valor1LOC");
                        rdF.Message = msgEmptyField;
                        rd.DetailsFields.Add(rdF);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostos.cs", "ValidateDataImport"));
            }
        }

        #endregion

        #region Funciones Virtuales / Abstractas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            base.SetInitParameters();
            
            // Carga info de las monedas
            this.monedaLocal = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
            this.monedaExtranjera = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
            this._bodegaTransacxDef = this._bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_CodigoBodegaTransaccional);
            this.param1xDef = this._bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Parametro1xDefecto);
            this.param2xDef = this._bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Parametro2xDefecto);
            FormProvider.Master.itemPaste.Enabled = false;
            FormProvider.Master.itemImport.Enabled = false;

            //Controles del detalle
            this._bc.InitMasterUC(this.masterReferencia, AppMasters.inReferencia, true, false, true, true);
            this._bc.InitMasterUC(this.masterEmpaque, AppMasters.inEmpaque, true, true, true, false);
            this._bc.InitMasterUC(this.masterParametro1, AppMasters.inRefParametro1, true, true, true, false);
            this._bc.InitMasterUC(this.masterParametro2, AppMasters.inRefParametro2, true, true, true, false);
            this._bc.InitMasterUC(this.masterUnidad, AppMasters.inUnidad, true, true, true, false);
            this._bc.InitMasterUC(this.masterReferenciaCod, AppMasters.inReferenciaCod, true, true, true, false);
            this.masterParametro1.EnableControl(false);
            this.masterParametro2.EnableControl(false);
            this.masterUnidad.EnableControl(false);
            this.endImportarDelegate = new EndImportar(this.EndImportarMethod);

            //Llena los combos
            TablesResources.GetTableResources(this.cmbEstado, typeof(EstadoInv));

        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize()
        {
            base.AfterInitialize();
            bool editFecha = this._bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_IndPermiteModifFechaTransaccion).Equals("0") ? false : true;
            this.indCostosFOB = (this._bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_IndControlCostosFOB)).Equals("0") ? false : true;
            this._usoReferenciaCod = this._bc.GetControlValueByCompany(ModulesPrefix.pr, AppControl.pr_IndUsoCodigoReferenciaRecibidos) == "1" ? true : false;
            this._empaqueInvIdDef = this._bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_EmpaquexDef);

            //Configura controles base
            if (this.dtPeriod.DateTime.Month == DateTime.Now.Month)
                base.dtFecha.DateTime = DateTime.Now;
            else
                base.dtFecha.DateTime =  new DateTime(this.dtPeriod.DateTime.Year, this.dtPeriod.DateTime.Month, DateTime.DaysInMonth(this.dtPeriod.DateTime.Year,this.dtPeriod.DateTime.Month));
            base.dtFecha.Enabled = editFecha;
            base.gvDocument.OptionsBehavior.Editable = false;
            if (this.documentID == AppDocuments.NotaEnvio)
            {
                this.pnlSaldos.Visible = true;
                this.lblKit.Visible = true;
                this.chkKit.Visible = true;
                this.pnlValor.Enabled = false;
            }

            //Habilita el boton de eliminar personalizado
            base.gcDocument.EmbeddedNavigator.Buttons.CustomButtons[1].Visible = true;
            base.gcDocument.EmbeddedNavigator.Buttons.Remove.Visible = false;
            base.gcDocument.EmbeddedNavigator.Buttons.CustomButtons[2].Visible = true;           

            //Valida si esta habilitado el modulo de Proyectos
            List<DTO_aplModulo> modsActive = this._bc.AdministrationModel.aplModulo_GetByVisible(1, true).ToList();
            foreach (DTO_aplModulo m in modsActive)
            {
                if (m.ModuloID.Value.ToLower() == ModulesPrefix.py.ToString())
                {
                    this.moduleProyectosActiveInd = true;
                    break;
                }
            }
            this.editValue.Mask.EditMask = "c2";
            this.editSpin.Mask.EditMask = "n2";
            this.formatReferencias = _bc.GetImportExportFormat(typeof(DTO_MigracionMvtos), this.documentID);

            this.AddGridCols();

            #region Carga Temporales
            if (this.HasTemporales())
            {
                string msgTitleLoadTemp = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_TempLoad);
                string msgLoadTemp = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Temp_LoadData);
                try
                {
                    //Revisa si desea cargar los temporales
                    if (MessageBox.Show(msgLoadTemp, msgTitleLoadTemp, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        DTO_MvtoInventarios factTemp = (DTO_MvtoInventarios)this._bc.AdministrationModel.aplTemporales_GetByOrigen(this.documentID.ToString(), this._bc.AdministrationModel.User);
                        if (factTemp != null)
                        {
                            try
                            {
                                this.LoadTempData(factTemp);
                            }
                            catch (Exception ex1)
                            {
                                this.validHeader = false;
                                MessageBox.Show(this._bc.GetResourceError(DictionaryMessages.Err_TempLoad));
                                this._bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), this._bc.AdministrationModel.User);
                            }
                        }
                        else
                        {
                            this.validHeader = false;
                            MessageBox.Show(this._bc.GetResourceError(DictionaryMessages.Err_TempLoad));
                            this._bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), this._bc.AdministrationModel.User);
                        }
                    }
                    else
                    {
                        this.validHeader = false;
                        this._bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), this._bc.AdministrationModel.User);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-DocumentMvtoForm.cs", "AfterInitialize: " + ex.Message));
                }
            } 
            #endregion
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected override void AddGridCols()
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
                this.gvDocument.Columns.Add(codRefP1P2);

                //CodigoReferencia
                GridColumn codRef = new GridColumn();
                codRef.FieldName = this.unboundPrefix + "inReferenciaID";
                //codRef.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_inReferenciaID");
                codRef.UnboundType = UnboundColumnType.String;
                codRef.VisibleIndex = 2;
                codRef.Width = 70;
                codRef.Visible = false;
                this.gvDocument.Columns.Add(codRef);

                //MarcaDesc
                GridColumn MarcaDesc = new GridColumn();
                MarcaDesc.FieldName = this.unboundPrefix + "MarcaDesc";
                MarcaDesc.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_MarcaDesc");
                MarcaDesc.UnboundType = UnboundColumnType.String;
                MarcaDesc.VisibleIndex = 3;
                MarcaDesc.Width = 110;
                MarcaDesc.Visible = true;
                this.gvDocument.Columns.Add(MarcaDesc);

                //RefProveedor
                GridColumn RefProveedor = new GridColumn();
                RefProveedor.FieldName = this.unboundPrefix + "RefProveedor";
                RefProveedor.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_RefProveedor");
                RefProveedor.UnboundType = UnboundColumnType.String;
                RefProveedor.VisibleIndex = 4;
                RefProveedor.Width = 110;
                RefProveedor.Visible = true;
                this.gvDocument.Columns.Add(RefProveedor);

                //Estado
                GridColumn estado = new GridColumn();
                estado.FieldName = this.unboundPrefix + "EstadoInv";
                estado.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_EstadoInv");
                estado.UnboundType = UnboundColumnType.Integer;
                estado.VisibleIndex = 5;
                estado.Width = 50;
                estado.Visible = true;
                this.gvDocument.Columns.Add(estado);

                //Descripcion
                GridColumn desc = new GridColumn();
                desc.FieldName = this.unboundPrefix + "DescripTExt";
                desc.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_DescripTExt");
                desc.UnboundType = UnboundColumnType.String;
                desc.VisibleIndex = 6;
                desc.Width = 280;
                desc.Visible = true;
                this.gvDocument.Columns.Add(desc);

                //Serial
                GridColumn serial = new GridColumn();
                serial.FieldName = this.unboundPrefix + "SerialID";
                serial.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_SerialID");
                serial.UnboundType = UnboundColumnType.String;
                serial.VisibleIndex = 7;
                serial.Width = 70;
                serial.Visible = true;
                this.gvDocument.Columns.Add(serial);

                //docSoporte
                GridColumn docSoporte = new GridColumn();
                docSoporte.FieldName = this.unboundPrefix + "DocSoporte";
                docSoporte.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_DocSoporte");
                docSoporte.UnboundType = UnboundColumnType.Integer;
                docSoporte.VisibleIndex =8;
                docSoporte.Width = 70;
                docSoporte.Visible = true;
                this.gvDocument.Columns.Add(docSoporte);

                //EmpaqueInvID
                GridColumn empaqueID = new GridColumn();
                empaqueID.FieldName = this.unboundPrefix + "EmpaqueInvID";
                empaqueID.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_EmpaqueInvID");
                empaqueID.UnboundType = UnboundColumnType.String;
                empaqueID.VisibleIndex = 9;
                empaqueID.Width = 70;
                empaqueID.Visible = true;
                this.gvDocument.Columns.Add(empaqueID);

                //CantidadEmpaques
                GridColumn cantidadEmpaques = new GridColumn();
                cantidadEmpaques.FieldName = this.unboundPrefix + "CantidadEMP";
                cantidadEmpaques.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_CantidadEMP");
                cantidadEmpaques.UnboundType = UnboundColumnType.Integer;
                cantidadEmpaques.VisibleIndex = 10;
                cantidadEmpaques.Width = 80;
                cantidadEmpaques.Visible = true;
                cantidadEmpaques.ColumnEdit = this.editSpin;
                this.gvDocument.Columns.Add(cantidadEmpaques);

                //valorML
                GridColumn valorML = new GridColumn();
                valorML.FieldName = this.unboundPrefix + "Valor1LOC";
                valorML.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorML");
                valorML.UnboundType = UnboundColumnType.Decimal;
                valorML.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                valorML.AppearanceCell.Options.UseTextOptions = true;
                valorML.VisibleIndex = 11;
                valorML.Width = 110;
                valorML.Visible = true;
                this.gvDocument.Columns.Add(valorML);

                //valorME
                GridColumn valorME = new GridColumn();
                valorME.FieldName = this.unboundPrefix + "Valor1EXT";
                valorME.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorME");
                valorME.UnboundType = UnboundColumnType.Decimal;
                valorME.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                valorME.AppearanceCell.Options.UseTextOptions = true;
                valorME.VisibleIndex = 12;
                valorME.Width = 110;
                valorME.Visible = this.multiMoneda? true: false;
                this.gvDocument.Columns.Add(valorME);

                //valorFOBML
                GridColumn valorFOBML = new GridColumn();
                valorFOBML.FieldName = this.unboundPrefix + "Valor2LOC";
                valorFOBML.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorFOBML");
                valorFOBML.UnboundType = UnboundColumnType.Decimal;
                valorFOBML.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                valorFOBML.AppearanceCell.Options.UseTextOptions = true;
                valorFOBML.VisibleIndex = 13;
                valorFOBML.Width = 110;
                valorFOBML.Visible = false;
                this.gvDocument.Columns.Add(valorFOBML);

                //valorFOBME
                GridColumn valorFOBME = new GridColumn();
                valorFOBME.FieldName = this.unboundPrefix + "Valor2EXT";
                valorFOBME.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorFOBME");
                valorFOBME.UnboundType = UnboundColumnType.Decimal;
                valorFOBME.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                valorFOBME.AppearanceCell.Options.UseTextOptions = true;
                valorFOBME.VisibleIndex = 14;
                valorFOBME.Width = 110;
                valorFOBME.Visible = false;
                this.gvDocument.Columns.Add(valorFOBME);

                #endregion
                #region Columnas No Visibles

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
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-DocumentMvtoForm.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        protected override void AddNewRow()
        {
            DTO_inMovimientoFooter footerDet = new DTO_inMovimientoFooter();
            try
            {
                #region Asigna datos a la fila

                footerDet.Movimiento.Index = this.data.Footer.Count > 0 ? this.data.Footer.Last().Movimiento.Index + 1 : 0;
                footerDet.Movimiento.EmpresaID.Value = this.empresaID;
                footerDet.Movimiento.TerceroID.Value = this.documentID == AppDocuments.TransaccionManual ? this.terceroHeader : string.Empty;
                footerDet.Movimiento.ProyectoID.Value = this.proyectoHeader;
                footerDet.Movimiento.CentroCostoID.Value = this.centroCostoHeader;
                footerDet.Movimiento.BodegaID.Value = this.bodegaOrigen != null ? this.bodegaOrigen.ID.Value : string.Empty;
                footerDet.Movimiento.inReferenciaID.Value = string.Empty;
                if (this.costeoGrupoOri != null && this.costeoGrupoOri.CosteoTipo.Value.Value == (byte)TipoCosteoInv.SinCosto)
                {
                    footerDet.Movimiento.EstadoInv.Value = (byte)EstadoInv.SinCosto;
                    this.cmbEstado.SelectedItem = this.cmbEstado.GetItem(((byte)EstadoInv.SinCosto).ToString());
                }
                else
                {
                    footerDet.Movimiento.EstadoInv.Value = (byte)EstadoInv.Activo;
                    this.cmbEstado.SelectedItem = this.cmbEstado.GetItem(((byte)EstadoInv.Activo).ToString());
                }
                footerDet.Movimiento.Parametro1.Value = string.Empty;
                footerDet.Movimiento.Parametro2.Value = string.Empty;
                footerDet.Movimiento.ActivoID.Value = 0;
                footerDet.Movimiento.MvtoTipoInvID.Value = this.mvtoTipo.ID.Value;
                footerDet.Movimiento.IdentificadorTr.Value = 0;
                footerDet.Movimiento.SerialID.Value = string.Empty;
                footerDet.Movimiento.EmpaqueInvID.Value = string.Empty;
                footerDet.Movimiento.CantidadDoc.Value = 0;
                footerDet.Movimiento.CantidadEMP.Value = 0;
                footerDet.Movimiento.CantidadUNI.Value = 0;
                if (mvtoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Entradas)
                    footerDet.Movimiento.EntradaSalida.Value = (byte)EntradaSalida.Entrada;
                else if (mvtoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Salidas || mvtoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Traslados)
                    footerDet.Movimiento.EntradaSalida.Value = (byte)EntradaSalida.Salida;
                footerDet.Movimiento.DescripTExt.Value = string.Empty;
                footerDet.Movimiento.ValorUNI.Value = 0;
                footerDet.Movimiento.DocSoporte.Value = 0;
                footerDet.Movimiento.DocSoporteTER.Value = string.Empty;
                footerDet.Movimiento.Valor1LOC.Value = 0;
                footerDet.Movimiento.Valor2LOC.Value = 0;
                footerDet.Movimiento.Valor1EXT.Value = 0;
                footerDet.Movimiento.Valor2EXT.Value = 0;

                #endregion

                this.data.Footer.Add(footerDet);
                this.gvDocument.RefreshData();
                this.gcDocument.RefreshDataSource();
                try { this.gvDocument.FocusedRowHandle = this.gvDocument.DataRowCount - 1; }
                catch (Exception) { ;}
                this.isValid = false;
                base.gcDocument.EmbeddedNavigator.Buttons.CustomButtons[1].Enabled = this.gvDocument.DataRowCount > 1 ? true : false;
                #region Reseteo los controles por defecto
                this._bc.InitMasterUC(this.masterParametro1, AppMasters.inRefParametro1, true, true, true, false);
                this._bc.InitMasterUC(this.masterParametro2, AppMasters.inRefParametro2, true, true, true, false);
                this.cmbEstado.BackColor = Color.White;
                this.txtSerial.BackColor = Color.White;
                this.txtSerial.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                //this.masterReferencia.Focus();
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-DocumentMvtoForm.cs", "AddNewRow: " + ex.Message));
            }
        }

        /// <summary>
        /// Asigna la tasa de cambio
        /// </summary>
        /// <param name="fromTop">Indica si el evento se esta ejecutando desde un control del header superior</param>
        protected virtual bool AsignarTasaCambio() { return false; }      

        /// <summary>
        /// Limpia y deja vacio los controles del footer
        /// </summary>
        /// <param name="basic">Indica si tambien se limpian los controles basicos</param>
        protected virtual void CleanFooter(bool basic)
        {
            this._totalUnidades = 0;
            this._valorTotalML = 0;
            this._valorTotalME = 0;
            this.masterReferencia.Value = string.Empty;
            this.masterReferenciaCod.Value = string.Empty;
            this.masterEmpaque.Value = string.Empty;
            this.masterParametro1.Value = string.Empty;
            this.masterParametro2.Value = string.Empty;
            this.masterUnidad.Value = string.Empty;
            this.txtSerial.Text = string.Empty;
            this.txtDocSoporte.Text = string.Empty;
            this.cmbSerialDisp.Text = string.Empty;
            this.cmbSerialDisp.Items.Clear();
            this.txtCantUnidad.Text = "0";
            this.txtDescr.Text = string.Empty;
            this.txtSaldoRef.EditValue = "0";
            this.txtValorML.EditValue = "0";
            this.txtValorUNI.EditValue = "0";
            this.txtValorFobML.EditValue = "0";
            this.txtValorME.EditValue = "0";
            this.txtValorFobME.EditValue = "0";
            foreach (var footer in this.data.Footer)
            {
                this._totalUnidades += footer.Movimiento.CantidadUNI.Value.Value;
                _valorTotalML += footer.Movimiento.Valor1LOC.Value.Value;
                this._valorTotalME += footer.Movimiento.Valor1EXT.Value.Value;
            }
            this.txtTotalCantidad.EditValue = this._totalUnidades;
            this.txtTotalValorExt.EditValue = this._valorTotalME;
            this.txtTotalValorLoc.EditValue = _valorTotalML;

            this._serializadoInd = false;
            if (this._usoReferenciaCod)
                this.masterReferenciaCod.Focus();
            else
                this.masterReferencia.Focus();
            if (this.newDoc && this.documentID == AppDocuments.TransaccionManual)
                this.pnlSaldos.Visible = false;

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
        }

        /// <summary>
        /// Habilita o deshabilita los controles del footer
        /// </summary>
        protected virtual void EnableFooter(bool enable)
        {
            #region Habilita/Deshabilita los controles
            //Campos de maestras
            this.masterReferencia.EnableControl(enable);
            this.btnReferencia.Enabled = enable;
            this.masterEmpaque.EnableControl(enable);
            this.masterReferenciaCod.EnableControl(enable);
            //Campos de texto
            this.txtDocSoporte.Enabled = enable;
            this.txtCantUnidad.Enabled = enable;
            if (!this.newDoc)
                this.cmbEstado.Enabled = false;// Convert.ToByte((this.cmbEstado.SelectedItem as ComboBoxItem).Value) == (byte)EstadoInv.SinCosto ? false : true;
            else
                this.cmbEstado.Enabled = false;
            this.txtSerial.Enabled = !enable ? false : this._serializadoInd;
            this.cmbSerialDisp.Enabled = false;
            //Campos de valores
            if (!this.multiMoneda)
            {
                this.txtValorME.Enabled = false;
                this.txtValorFobME.Enabled = false;
                this.txtValorUNI.Enabled = enable;
                this.txtValorML.Enabled = enable;
                this.txtValorFobML.Enabled = this.indCostosFOB ? enable : false;
            }
            else
            {
                this.txtValorME.Enabled = this.monedaId == this.monedaLocal ? false : enable;
                this.txtValorFobME.Enabled = !this.txtValorME.Enabled ? false : this.indCostosFOB;
                this.txtValorUNI.Enabled = this.monedaId == this.monedaLocal ? enable : false;
                this.txtValorML.Enabled = this.monedaId == this.monedaLocal ? enable : false;
                this.txtValorFobML.Enabled = !this.txtValorML.Enabled ? false : this.indCostosFOB;
            }
            this.txtTotalCantidad.Enabled = enable;
            this.txtTotalValorExt.Enabled = enable;
            this.txtTotalValorLoc.Enabled = enable;
            if (this._usoReferenciaCod)
            {
                this.masterReferenciaCod.EnableControl(true);
                this.masterReferencia.EnableControl(false);
                this.btnReferencia.Enabled = false;
                this.masterParametro1.EnableControl(false);
                this.masterParametro2.EnableControl(false);
                this.cmbEstado.Enabled = false;
            }
            else
                this.masterReferenciaCod.EnableControl(false);
            #endregion
        }

        /// <summary>
        /// Oculta los controles del formulario
        /// </summary>
        protected virtual void EnableHeader(short TipoMov, bool basic) { }

        /// <summary>
        /// Carga informacion de un temporal a partir del cabezote
        /// </summary>
        /// <returns>Retorna el header de un temporal</returns>
        protected virtual DTO_MvtoInventarios LoadTempHeader() { return null; }

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
                    #region Asignacion de Valores
                    this.masterReferencia.Value = this._rowCurrent.Movimiento.inReferenciaID.Value;
                    this.cmbEstado.SelectedItem = this._rowCurrent.Movimiento.EstadoInv.Value;
                    this.masterParametro1.Value = this._rowCurrent.Movimiento.Parametro1.Value; 
                    this.masterParametro2.Value = this._rowCurrent.Movimiento.Parametro2.Value; 
                    this.masterUnidad.Value = this._rowCurrent.UnidadRef.Value; 
                    this.txtSerial.Text = this._rowCurrent.Movimiento.SerialID.Value; 
                    this.txtDocSoporte.Text = this._rowCurrent.Movimiento.DocSoporte.Value.ToString(); 
                    this.masterEmpaque.Value = this._rowCurrent.Movimiento.EmpaqueInvID.Value; 
                    this.txtCantUnidad.EditValue = this._rowCurrent.Movimiento.CantidadEMP.Value; 
                    this.txtDescr.Text = this._rowCurrent.Movimiento.DescripTExt.Value;
                    this.txtValorUNI.EditValue = this._rowCurrent.Movimiento.ValorUNI.Value; 
                    this.txtValorML.EditValue = this._rowCurrent.Movimiento.Valor1LOC.Value; 
                    this.txtValorME.EditValue = this._rowCurrent.Movimiento.Valor1EXT.Value; 
                    this.txtValorFobML.EditValue = this._rowCurrent.Movimiento.Valor2LOC.Value;
                    this.txtValorFobME.EditValue = this._rowCurrent.Movimiento.Valor2EXT.Value;
                    if (this._rowCurrent.CantidadDispon.Value != null)
                        this.txtSaldoRef.EditValue = this._rowCurrent.CantidadDispon.Value;
                    this.txtTotalCantidad.EditValue = this._totalUnidades;
                    #endregion    
                    if (this.newDoc)
                    {
                        this.EnableFooter(isFacturaVenta? false : true);
                        this.newDoc = false;
                    }
                    if (string.IsNullOrEmpty(this._rowCurrent.Movimiento.SerialID.Value))
                    {
                        this.txtSerial.BackColor = Color.White;
                        this.txtSerial.Enabled = false;
                        this._serializadoInd = false;
                        this.txtCantUnidad.Enabled = true;
                    }
                    else
                    {
                        this.txtSerial.BackColor = Color.LightBlue;
                        this.txtSerial.Enabled = true;
                        this._serializadoInd = true;
                        this.txtCantUnidad.Enabled = false;
                    }
                    if (this.mvtoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Salidas || this.mvtoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Traslados)
                        this.GetSaldoAvailable(this._rowCurrent,false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-DocumentMvtoForm.cs", "LoadEditGridData: " + ex.Message));
                }
            }
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
            }
            this.dataLoaded = true;
        }

        /// <summary>
        /// Carga el valor de la tasa de cambio de acuerdo a los items seleccionados
        /// </summary>
        /// <param name="monOr">Moneda Origen</param>
        /// <returns>Retorna el valor de la tasa de cambio</returns>
        protected virtual decimal LoadTasaCambio(int monOr, DateTime fecha)
        {
            try
            {
                decimal valor = 0;
                //string tasaMon = this.monedaId;
                //if (monOr == (int)TipoMoneda.Local)
                //    tasaMon = this.monedaExtranjera;

                valor = this._bc.AdministrationModel.TasaDeCambio_Get(this.monedaExtranjera, fecha);
                return valor;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-DocumentMvtoForm.cs", "LoadTasaCambio"));
                return 0;
            }
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
                this.indexFila = Convert.ToInt32(this.gvDocument.GetRowCellValue(cFila, col));

                this.LoadEditGridData(false, cFila);
                this.isValid = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-DocumentMvtoForm.cs", "RowIndexChanged"));
            }
        }

        /// <summary>
        /// Habilita o deshabilita la barra de herramientas segun donde el usuario este
        /// </summary>
        protected override void ValidHeaderTB()
        {
            if (this.validHeader)
            {
                //Habilita el boton de salvar
                if (SecurityManager.HasAccess(this.documentID, FormsActions.Add) || SecurityManager.HasAccess(this.documentID, FormsActions.Edit))
                    if(this.data != null && this.data.DocCtrl.Estado.Value == (byte)EstadoDocControl.SinAprobar)
                        FormProvider.Master.itemSave.Enabled = true;
                else
                    FormProvider.Master.itemSave.Enabled = false;

                FormProvider.Master.itemDelete.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Delete);
            }
            else
            {
                FormProvider.Master.itemSave.Enabled = false;
                FormProvider.Master.itemDelete.Enabled = false;

                FormProvider.Master.itemPrint.Enabled = false;

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
                #region Referencia
                validField = this._bc.ValidGridCell(this.gvDocument, string.Empty, fila, "inReferenciaID",false, true, false, AppMasters.inReferencia);
                if (!validField)
                    validRow = false;
                else
                {
                    if (this._rowCurrent.Movimiento.CantidadEMP.Value == 0)
                        validRow = false;
                }                  
                #endregion
                #region Empaque
                validField = this._bc.ValidGridCell(this.gvDocument, string.Empty, fila, "EmpaqueInvID", false, true, false, AppMasters.inEmpaque);
                if (!validField)
                    validRow = false;
                #endregion
                #region Estado
                validField = this._bc.ValidGridCell(this.gvDocument, string.Empty, fila, "EstadoInv", false, false, false, null);
                if (!validField)
                    validRow = false;
                #endregion
                #region Cantidad Emp
                validField = this._bc.ValidGridCellValue(this.gvDocument, string.Empty, fila, "CantidadEMP", false, false, false, false);
                if (!validField)
                    validRow = false;
                #endregion
                #region DescripTExt
                validField = this._bc.ValidGridCell(this.gvDocument, string.Empty, fila, "DescripTExt", false, false, false, null);
                if (!validField)
                    validRow = false;
                #endregion
                #region Serial
                validField = this._bc.ValidGridCell(this.gvDocument, string.Empty, fila, "SerialID", this._serializadoInd ? false : true, false, false, null);
                if (!validField)
                    validRow = false;
                #endregion
                #region DocSoporte 
                //validField = this._bc.ValidGridCellValue(this.gvDocument, string.Empty, fila, "DocSoporte", false, false, true, false);
                //if (!validField)
                //    validRow = false;
                #endregion
                #endregion
                #region Validaciones de valores
                #region ValorML
                validField = this._bc.ValidGridCellValue(this.gvDocument, string.Empty, fila, "Valor1LOC", false, true, false, false);
                if (!validField)
                    validRow = false;
                #endregion
                #region ValorME
                validField = this._bc.ValidGridCellValue(this.gvDocument, string.Empty, fila, "Valor1EXT", false, true, false, false);
                if (!validField)
                    validRow = false;
                #endregion
                #region ValorFOBML
                validField = this._bc.ValidGridCellValue(this.gvDocument, string.Empty, fila, "Valor2LOC", false, true, false, false);
                if (!validField)
                    validRow = false;
                #endregion
                #region ValorFOBME
                validField = this._bc.ValidGridCellValue(this.gvDocument, string.Empty, fila, "Valor2EXT", false, true, false, false);
                if (!validField)
                    validRow = false;
                #endregion
                #endregion
                if (validRow)
                {
                    this.isValid = true;

                    if (!this.newReg)
                        this.UpdateTemp(this.data);
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
    
        /// <summary>
        /// Revisa si una grilla es valida o no
        /// </summary>
        /// <returns></returns>
        protected virtual bool ValidGrid()
        {
            if (this.data.Footer != null && this.data.Footer.Count == 0)
            {
                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoData));
                return false;
            }
            if(this.data.Footer.Any(x =>string.IsNullOrEmpty(x.Movimiento.BodegaID.Value)))
            {
                string itemsBodegavacio = string.Empty;
                foreach (var item in this.data.Footer.FindAll(x=>string.IsNullOrEmpty(x.Movimiento.BodegaID.Value)))
                {
                    itemsBodegavacio += item.Movimiento.Index.ToString() + ": " + item.Movimiento.inReferenciaID.Value;
                }
                MessageBox.Show(itemsBodegavacio);
                return false;
            }

            if (!this.ValidateRow(this.gvDocument.FocusedRowHandle))
                return false;

            return true;
        }

        /// <summary>
        /// valida la informacion del header
        /// </summary>
        protected virtual bool ValidateHeader() { 
            return true;
        }

        /// <summary>
        /// Carga la informacion temporal del documento
        /// </summary>
        /// <param name="aux">Informacion del temporal</param>
        protected virtual void LoadTempData(DTO_MvtoInventarios aux) { }

        /// <summary>
        /// Valida una fila
        /// </summary>
        /// <param name="fila">Indice de la fila a validar</param>
        /// <returns>Retorna si la info de la fila es valida o no</returns>
        protected virtual bool AssignData(object ctrlSender, EventArgs e, DTO_inMovimientoFooter row, bool isImport)
        {
            try
            {
                bool res = true;
                if (!isImport)
                {
                    if (ctrlSender.GetType() == typeof(ControlsUC.uc_MasterFind))
                    {
                        #region Maestras
                        ControlsUC.uc_MasterFind master = (ControlsUC.uc_MasterFind)ctrlSender;
                        switch (master.ColId)
                        {
                            case "inReferenciaCodID":
                                #region Referencia Codigo
                                if (master.ValidID)
                                {
                                    DTO_inReferenciaCod refCod = (DTO_inReferenciaCod)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferenciaCod, false, master.Value, true);
                                    this.masterReferencia.Value = refCod.inReferenciaID.Value;
                                    this.masterParametro1.Value = refCod.Parametro1ID.Value;
                                    this.masterParametro2.Value = refCod.Parametro2ID.Value;
                                    this.cmbEstado.SelectedItem = this.cmbEstado.GetItem((refCod.EstadoInv.Value.ToString()));

                                    row.Movimiento.inReferenciaID.Value = this.masterReferencia.Value;
                                    this._referenciaInv = (DTO_inReferencia)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, false, this.masterReferencia.Value, true);
                                    DTO_inRefTipo _tipoInv = (DTO_inRefTipo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inRefTipo, false, _referenciaInv.TipoInvID.Value, true);
                                    this.masterEmpaque.Value = this._referenciaInv.EmpaqueInvID.Value;
                                    this.txtDescr.Text = this._referenciaInv.Descriptivo.Value;
                                    row.Movimiento.EmpaqueInvID.Value = this.masterEmpaque.Value;
                                    row.Movimiento.DescripTExt.Value = this.txtDescr.Text;
                                    this.masterParametro1.EnableControl(false);
                                    this.masterParametro2.EnableControl(false);
                                    #region Referencia serializada
                                    if (_tipoInv.SerializadoInd.Value.Value)
                                    {
                                        this._serializadoInd = true;
                                        this.txtCantUnidad.EditValue = 1;
                                        this.txtCantUnidad.Enabled = false;
                                        this.txtSerial.BackColor = Color.LightBlue;
                                        this.txtSerial.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                                    }
                                    else
                                    {
                                        this._serializadoInd = false;
                                        this.txtCantUnidad.EditValue = Convert.ToDecimal(this.txtCantUnidad.EditValue, CultureInfo.InvariantCulture) != 0 ? this.txtCantUnidad.EditValue : 0;
                                        this.txtCantUnidad.Enabled = true;
                                        this.txtSerial.BackColor = Color.White;
                                        this.txtSerial.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                                    }
                                    this.txtSerial.Enabled = this._serializadoInd;
                                    this.cmbSerialDisp.Enabled = this.mvtoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Entradas ? false : this._serializadoInd;
                                    this.cmbSerialDisp.Items.Clear();
                                    #endregion
                                    #region  Trae una lista de seriales segun la referencia(Salida-Traslado)
                                    if (this.mvtoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Salidas || this.mvtoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Traslados)
                                    {
                                        if (this._serializadoInd)
                                        {
                                            //Consulta acActivoControl
                                            DTO_acActivoControl activo = new DTO_acActivoControl();
                                            activo.inReferenciaID.Value = this._referenciaInv.ID.Value;
                                            activo.BodegaID.Value = this.bodegaOrigen.ID.Value;
                                            List<DTO_acActivoControl> result = this._bc.AdministrationModel.acActivoControl_GetByParameterForTranfer(activo);
                                            foreach (DTO_acActivoControl serial in result)
                                            {
                                                this.cmbSerialDisp.Items.Add(serial.SerialID.Value);
                                            }
                                            this.cmbSerialDisp.Enabled = this.cmbSerialDisp.Items.Count > 0 ? true : false;
                                        }
                                        this.GetSaldoAvailable(this._rowCurrent,false);
                                    }
                                    this.textEditControl_Leave(this.txtCantUnidad, e);
                                    #endregion
                                    #region Verifica KIT y sus referencias equivalentes
                                    if (_tipoInv.ControlEspecial.Value.Value == (byte)ControlEspecial.Kit)
                                    {
                                        if (this.documentID == AppDocuments.TransaccionManual)
                                        {
                                            MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_ReferenceInvalid));
                                            this.masterReferencia.Focus();
                                        }
                                        else
                                        {
                                            List<DTO_inPartesComponentes> listaKit = new List<DTO_inPartesComponentes>();
                                            #region Filtros
                                            DTO_glConsulta consulta = new DTO_glConsulta();
                                            List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();
                                            //Filtro de la referencia en Kit
                                            filtros.Add(new DTO_glConsultaFiltro()
                                            {
                                                CampoFisico = "ReferenciaGrupo",
                                                ValorFiltro = this.masterReferencia.Value,
                                                OperadorFiltro = OperadorFiltro.Igual
                                            });
                                            consulta.Filtros = filtros;
                                            #endregion
                                            long count = this._bc.AdministrationModel.MasterComplex_Count(AppMasters.inPartesComponentes, consulta, null);
                                            var listPartesComp = this._bc.AdministrationModel.MasterComplex_GetPaged(AppMasters.inPartesComponentes, count, 1, consulta, null).ToList();
                                            foreach (var parte in listPartesComp)
                                            {
                                                DTO_inPartesComponentes kit = (DTO_inPartesComponentes)parte;
                                                listaKit.Add(kit);
                                            }
                                            if (listaKit.Count == 0)
                                                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_KitWithoutReferences));
                                        }
                                        this.chkKit.Checked = true;
                                    }
                                    else
                                        this.chkKit.Checked = false;
                                    #endregion
                                    #region Referencia Especial para salida o traslado de Otros
                                    string refEspecial = this._bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_RefEspecialOtros);
                                    if (this.masterReferencia.Value.Equals(refEspecial))
                                    {
                                        //this.EnableFooter(false);
                                        //this.txtCantUnidad.Enabled = true;
                                        //this.txtDescr.Enabled = true;
                                    }
                                    #endregion
                                    if (row.Movimiento.Valor1LOC.Value.Value != 0)
                                        this.textEditControl_Leave(this.txtValorML, e);
                                    row.ReferenciaIDP1P2.Value = this.masterReferencia.Value + (this.masterParametro1.Value == this.param1xDef.ToUpper() || string.IsNullOrEmpty(this.masterParametro1.Value) ? string.Empty : "-" + this.masterParametro1.Value)
                                                                                   + (this.masterParametro2.Value == this.param2xDef.ToUpper() || string.IsNullOrEmpty(this.masterParametro2.Value) ? string.Empty : "-" + this.masterParametro2.Value);
                                }
                                #endregion
                                break;
                            case "inReferenciaID":
                                #region Codigo Referencia
                                if (master.ValidID)
                                {
                                    row.Movimiento.inReferenciaID.Value = master.Value;
                                    this._referenciaInv = (DTO_inReferencia)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, false, master.Value, true);
                                    DTO_inRefTipo _tipoInv = (DTO_inRefTipo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inRefTipo, false, _referenciaInv.TipoInvID.Value, true);
                                    this.masterEmpaque.Value = this._referenciaInv.EmpaqueInvID.Value;
                                    this.txtDescr.Text = this._referenciaInv.Descriptivo.Value;
                                    row.Movimiento.EmpaqueInvID.Value = this.masterEmpaque.Value;
                                    row.Movimiento.DescripTExt.Value = this.txtDescr.Text;
                                    row.Movimiento.MarcaDesc.Value = this._referenciaInv.MarcaInvDesc.Value;
                                    row.Movimiento.RefProveedor.Value = this._referenciaInv.RefProveedor.Value;
                                    #region Referencia serializada
                                    if (_tipoInv.SerializadoInd.Value.Value)
                                    {
                                        this._serializadoInd = true;
                                        this.txtCantUnidad.EditValue = 1;
                                        this.txtCantUnidad.Enabled = false;
                                        this.txtSerial.BackColor = Color.LightBlue;
                                        this.txtSerial.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                                    }
                                    else
                                    {
                                        this._serializadoInd = false;
                                        this.txtCantUnidad.EditValue = Convert.ToDecimal(this.txtCantUnidad.EditValue, CultureInfo.InvariantCulture) != 0 ? this.txtCantUnidad.EditValue : 1;
                                        this.txtCantUnidad.Enabled = true;
                                        this.txtSerial.BackColor = Color.White;
                                        this.txtSerial.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                                    }
                                    this.txtSerial.Enabled = this._serializadoInd;
                                    this.cmbSerialDisp.Enabled = this.mvtoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Entradas ? false : this._serializadoInd;
                                    this.cmbSerialDisp.Items.Clear();
                                    #endregion
                                    #region Trae los Parametros 1 y 2 si están habilitados
                                    if (_tipoInv.Parametro1Ind.Value.Value)
                                    {
                                        filtrosParam1 = InventoryParameters.GetQueryParameters(_tipoInv.ID.Value, true);
                                        this._bc.InitMasterUC(this.masterParametro1, AppMasters.inRefParametro1, true, true, true, true, filtrosParam1);
                                        this.masterParametro1.EnableControl(true);
                                    }
                                    else
                                    {
                                        this._bc.InitMasterUC(this.masterParametro1, AppMasters.inRefParametro1, true, true, true, false);
                                        this.masterParametro1.EnableControl(false);
                                        this.masterParametro1.Value = this._bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Parametro1xDefecto);
                                        row.Movimiento.Parametro1.Value = this.masterParametro1.Value;
                                    }
                                    if (_tipoInv.Parametro2Ind.Value.Value)
                                    {
                                        filtrosParam2 = InventoryParameters.GetQueryParameters(_tipoInv.ID.Value, false);
                                        this._bc.InitMasterUC(this.masterParametro2, AppMasters.inRefParametro2, true, true, true, true, filtrosParam2);
                                        this.masterParametro2.EnableControl(true);
                                    }
                                    else
                                    {
                                        this._bc.InitMasterUC(this.masterParametro2, AppMasters.inRefParametro2, true, true, true, false);
                                        this.masterParametro2.EnableControl(false);
                                        this.masterParametro2.Value = this._bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Parametro2xDefecto);
                                        row.Movimiento.Parametro2.Value = this.masterParametro2.Value;
                                    }
                                    //Une la referencia con los parametros para mostrarlos
                                    #endregion
                                    #region  Trae una lista de seriales segun la referencia(Salida-Traslado)
                                    if (this.mvtoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Salidas || this.mvtoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Traslados)
                                    {
                                        if (this._serializadoInd)
                                        {
                                            //Consulta acActivoControl
                                            DTO_acActivoControl activo = new DTO_acActivoControl();
                                            activo.inReferenciaID.Value = this._referenciaInv.ID.Value;
                                            activo.BodegaID.Value = this.bodegaOrigen.ID.Value;
                                            List<DTO_acActivoControl> result = this._bc.AdministrationModel.acActivoControl_GetByParameterForTranfer(activo);
                                            foreach (DTO_acActivoControl serial in result)
                                            {
                                                this.cmbSerialDisp.Items.Add(serial.SerialID.Value);
                                            }
                                            this.cmbSerialDisp.Enabled = this.cmbSerialDisp.Items.Count > 0 ? true : false;
                                        }
                                        this.GetSaldoAvailable(this._rowCurrent, false);
                                    }
                                    this.textEditControl_Leave(this.txtCantUnidad, e);
                                    #endregion
                                    #region Verifica KIT y sus referencias equivalentes
                                    if (_tipoInv.ControlEspecial.Value.Value == (byte)ControlEspecial.Kit)
                                    {
                                        if (this.documentID == AppDocuments.TransaccionManual)
                                        {
                                            MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_ReferenceInvalid));
                                            this.masterReferencia.Focus();
                                        }
                                        else
                                        {
                                            List<DTO_inPartesComponentes> listaKit = new List<DTO_inPartesComponentes>();
                                            #region Filtros
                                            DTO_glConsulta consulta = new DTO_glConsulta();
                                            List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();
                                            //Filtro de la referencia en Kit
                                            filtros.Add(new DTO_glConsultaFiltro()
                                            {
                                                CampoFisico = "ReferenciaGrupo",
                                                ValorFiltro = master.Value,
                                                OperadorFiltro = OperadorFiltro.Igual
                                            });
                                            consulta.Filtros = filtros;
                                            #endregion
                                            long count = this._bc.AdministrationModel.MasterComplex_Count(AppMasters.inPartesComponentes, consulta, null);
                                            var listPartesComp = this._bc.AdministrationModel.MasterComplex_GetPaged(AppMasters.inPartesComponentes, count, 1, consulta, null).ToList();
                                            foreach (var parte in listPartesComp)
                                            {
                                                DTO_inPartesComponentes kit = (DTO_inPartesComponentes)parte;
                                                listaKit.Add(kit);
                                            }
                                            if (listaKit.Count == 0)
                                                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_KitWithoutReferences));
                                        }
                                        this.chkKit.Checked = true;
                                    }
                                    else
                                        this.chkKit.Checked = false;
                                    #endregion
                                    #region Referencia Especial para salida o traslado de Otros
                                    string refEspecial = this._bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_RefEspecialOtros);
                                    if (master.Value.Equals(refEspecial))
                                    {
                                        //this.EnableFooter(false);
                                        //this.txtCantUnidad.Enabled = true;
                                        //this.txtDescr.Enabled = true;
                                    }
                                    #endregion
                                    if (row.Movimiento.Valor1LOC.Value.Value != 0)
                                        this.textEditControl_Leave(this.txtValorML, e);
                                    row.ReferenciaIDP1P2.Value = this.masterReferencia.Value + (this.masterParametro1.Value == this.param1xDef.ToUpper() || string.IsNullOrEmpty(this.masterParametro1.Value) ? string.Empty : "-" + this.masterParametro1.Value) + (this.masterParametro2.Value == this.param2xDef.ToUpper() || string.IsNullOrEmpty(this.masterParametro2.Value) ? string.Empty : "-" + this.masterParametro2.Value);
                                }
                                else
                                {
                                    this._referenciaInv = null;
                                    this.masterEmpaque.Value = string.Empty;
                                    this.masterParametro1.Value = string.Empty;
                                    this.masterParametro2.Value = string.Empty;
                                    row.Movimiento.inReferenciaID.Value = string.Empty;
                                    row.ReferenciaIDP1P2.Value = string.Empty;
                                    row.Movimiento.DescripTExt.Value = string.Empty;
                                }
                                #endregion
                                break;
                            case "EmpaqueInvID":
                                #region Codigo Empaque
                                if (master.ValidID && this._referenciaInv != null)
                                {
                                    DTO_inEmpaque empaqueEmp = (DTO_inEmpaque)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inEmpaque, false, master.Value, true);
                                    DTO_inUnidad unidadEmp = (DTO_inUnidad)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inUnidad, false, empaqueEmp.UnidadInvID.Value, true);
                                    DTO_inUnidad unidadRef = (DTO_inUnidad)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inUnidad, false, this._referenciaInv.UnidadInvID.Value, true);

                                    if (unidadEmp.ID.Value != unidadRef.ID.Value)
                                    {
                                        #region Verifica el Tipo de Medida de la Unidad
                                        if (unidadEmp.TipoMedida.Value == unidadRef.TipoMedida.Value)
                                            row.Movimiento.EmpaqueInvID.Value = master.Value;
                                        else
                                        {
                                            MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_UnitInvalidEmp));
                                            master.Value = string.Empty;
                                            master.Focus();
                                        }
                                        #endregion
                                        #region Si requiere conversion de unidad, verifica que exista
                                        decimal cantUnid = Convert.ToDecimal(this.txtCantUnidad.EditValue, CultureInfo.InvariantCulture);
                                        if (!empaqueEmp.UnidadInvID.Value.Equals(this._empaqueInvIdDef))
                                        {
                                            Dictionary<string, string> keysConvert = new Dictionary<string, string>();
                                            keysConvert.Add("UnidadInvID", unidadEmp.ID.Value);
                                            keysConvert.Add("UnidadBase", unidadRef.ID.Value);
                                            DTO_inConversionUnidad conversion = (DTO_inConversionUnidad)this._bc.GetMasterComplexDTO(AppMasters.inConversionUnidad, keysConvert, true);
                                            if (conversion != null)
                                            {
                                                if (cantUnid > 0)
                                                    this._unidadesFinalxRef = (conversion.Factor.Value.Value * Convert.ToDecimal(this.txtCantUnidad.EditValue, CultureInfo.InvariantCulture) * empaqueEmp.Cantidad.Value.Value);
                                                row.Movimiento.CantidadUNI.Value = this._unidadesFinalxRef;
                                                foreach (var footer in this.data.Footer)
                                                {
                                                    this._totalUnidades += footer.Movimiento.CantidadUNI.Value.Value;
                                                }
                                                this.txtTotalCantidad.EditValue = this._totalUnidades;
                                            }
                                            else
                                            {
                                                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_NotExistConvertUnit));
                                                this.txtCantUnidad.EditValue = this._serializadoInd ? 1 : 0;
                                                return false;
                                            }
                                        }
                                        else
                                            this._unidadesFinalxRef = cantUnid;
                                        #endregion
                                    }
                                    else
                                        row.Movimiento.EmpaqueInvID.Value = master.Value;
                                    this.txtCantUnidad.Focus();
                                }
                                #endregion
                                break;
                            case "Parametro1ID":
                                #region Codigo Parametro 1
                                if (master.ValidID)
                                {
                                    row.Movimiento.Parametro1.Value = master.Value;
                                    row.ReferenciaIDP1P2.Value = this.masterReferencia.Value + (this.masterParametro1.Value == this.param1xDef.ToUpper() || string.IsNullOrEmpty(this.masterParametro1.Value) ? string.Empty : "-" + this.masterParametro1.Value)
                                                                                   + (this.masterParametro2.Value == this.param2xDef.ToUpper() || string.IsNullOrEmpty(this.masterParametro2.Value) ? string.Empty : "-" + this.masterParametro2.Value);
                                    if (this.mvtoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Salidas || this.mvtoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Traslados)
                                        this.GetSaldoAvailable(this._rowCurrent, false);
                                }
                                #endregion
                                break;
                            case "Parametro2ID":
                                #region Codigo Parametro 2
                                if (master.ValidID)
                                {
                                    row.Movimiento.Parametro2.Value = master.Value;
                                    row.ReferenciaIDP1P2.Value = this.masterReferencia.Value + (this.masterParametro1.Value == this.param1xDef.ToUpper() || string.IsNullOrEmpty(this.masterParametro1.Value) ? string.Empty : "-" + this.masterParametro1.Value)
                                                                                   + (this.masterParametro2.Value == this.param2xDef.ToUpper() || string.IsNullOrEmpty(this.masterParametro2.Value) ? string.Empty : "-" + this.masterParametro2.Value);
                                    if (this.mvtoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Salidas || this.mvtoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Traslados)
                                        this.GetSaldoAvailable(this._rowCurrent, false);
                                }
                                #endregion
                                break;
                        }
                        #endregion
                    }
                    else if (ctrlSender.GetType() == typeof(TextBox))
                    {
                        #region Textos
                        TextBox ctrl = (TextBox)ctrlSender;
                        bool serialExistCurrent = false;
                        switch (ctrl.Name)
                        {
                            case "txtSerial":
                                #region Serial
                                if (this._serializadoInd && !string.IsNullOrEmpty(this.txtSerial.Text))
                                {
                                    DTO_acActivoControl activo;
                                    List<DTO_acActivoControl> result;
                                    foreach (var item in this.data.Footer)
                                    {
                                        if (item.Movimiento.SerialID.Value == ctrl.Text && item.Movimiento.Index != this.indexFila)
                                        {
                                            MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_AlreadyExistSerialCurrent));
                                            serialExistCurrent = true;
                                            break;
                                        }
                                    }
                                    if (!serialExistCurrent && this.mvtoTipo != null && this.mvtoTipo.TipoMovimiento.Value.Value == (byte)TipoMovimientoInv.Entradas)
                                    {
                                        #region Verifica que el serial ingresado NO exista
                                        activo = new DTO_acActivoControl();
                                        activo.SerialID.Value = ctrl.Text;
                                        result = this._bc.AdministrationModel.acActivoControl_GetByParameterForTranfer(activo);
                                        if (result.Count == 0)
                                            row.Movimiento.SerialID.Value = ctrl.Text;
                                        else
                                            MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_AlreadyExistSerial));
                                        #endregion
                                    }
                                    else if (!serialExistCurrent && this.mvtoTipo != null && (this.mvtoTipo.TipoMovimiento.Value.Value == (byte)TipoMovimientoInv.Salidas || this.mvtoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Traslados))
                                    {
                                        #region Verifica que el serial ingresado SI exista
                                        activo = new DTO_acActivoControl();
                                        activo.SerialID.Value = ctrl.Text;
                                        activo.BodegaID.Value = this.bodegaOrigen.ID.Value;
                                        activo.inReferenciaID.Value = this._referenciaInv.ID.Value;
                                        result = this._bc.AdministrationModel.acActivoControl_GetByParameterForTranfer(activo);
                                        if (result.Count > 0)
                                        {
                                            row.Movimiento.ActivoID.Value = result[0].ActivoID.Value;
                                            row.Movimiento.SerialID.Value = ctrl.Text;
                                            this.GetSaldoAvailable(this._rowCurrent, false);
                                        }
                                        else
                                            MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_NotExistSerial));
                                        #endregion
                                    }

                                }
                                #endregion
                                break;
                            case "txtDocSoporte":
                                if (!string.IsNullOrEmpty(ctrl.Text))
                                    row.Movimiento.DocSoporte.Value = Convert.ToInt32(ctrl.Text);
                                break;
                            case "txtDescr":
                                row.Movimiento.DescripTExt.Value = ctrl.Text;
                                this._bc.ValidGridCell(this.gvDocument, string.Empty, this.gvDocument.FocusedRowHandle, "DescripTExt", false, false, false, null);
                                break;
                        }
                        #endregion
                    }
                    else if (ctrlSender.GetType() == typeof(ComboBoxEx))
                    {
                        #region Combos
                        ComboBoxEx ctrl = (ComboBoxEx)ctrlSender;
                        switch (ctrl.Name)
                        {
                            case "cmbEstado":
                                if (!string.IsNullOrEmpty(ctrl.Text))
                                {
                                    row.Movimiento.EstadoInv.Value = Convert.ToByte((this.cmbEstado.SelectedItem as ComboBoxItem).Value);
                                    if (this.mvtoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Salidas || this.mvtoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Traslados)
                                        this.GetSaldoAvailable(this._rowCurrent, false);
                                }
                                break;
                            case "cmbSerialDisp":
                                if (!string.IsNullOrEmpty(ctrl.Text))
                                {
                                    this.txtSerial.Text = ctrl.Text;
                                    this.textControl_Leave(this.txtSerial, e);
                                }
                                break;
                        }
                        #endregion
                    }
                    else if (ctrlSender.GetType() == typeof(TextEdit))
                    {
                        #region Valores
                        TextEdit ctrl = (TextEdit)ctrlSender;
                        decimal valorDocLoc = 0;
                        decimal valorExtLoc = 0;
                        if (Convert.ToDecimal(ctrl.EditValue, CultureInfo.InvariantCulture) >= 0)
                        {
                            decimal cantidadEmp = Convert.ToDecimal(ctrl.EditValue, CultureInfo.InvariantCulture);
                            switch (ctrl.Name)
                            {
                                case "txtCantUnidad":
                                    #region Cantidad Empaques
                                    if (this._referenciaInv != null && this.masterEmpaque.ValidID)
                                    {
                                        #region Realiza la conversion de unidad si es necesario
                                        DTO_inEmpaque empaqueEmp = (DTO_inEmpaque)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inEmpaque, false, this.masterEmpaque.Value, true);
                                        this._referenciaInv = (DTO_inReferencia)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, false, this.masterReferencia.Value, true);
                                        if (empaqueEmp.UnidadInvID.Value == this._referenciaInv.UnidadInvID.Value)
                                        {
                                            decimal tmp = Convert.ToDecimal(ctrl.EditValue, CultureInfo.InvariantCulture);
                                            this._unidadesFinalxRef = cantidadEmp * empaqueEmp.Cantidad.Value.Value;
                                        }
                                        else
                                        {
                                            if (!empaqueEmp.UnidadInvID.Value.Equals(this._empaqueInvIdDef))
                                            {
                                                Dictionary<string, string> keysConvert = new Dictionary<string, string>();
                                                keysConvert.Add("UnidadInvID", empaqueEmp.UnidadInvID.Value);
                                                keysConvert.Add("UnidadBase", this._referenciaInv.UnidadInvID.Value);
                                                DTO_inConversionUnidad conversion = (DTO_inConversionUnidad)this._bc.GetMasterComplexDTO(AppMasters.inConversionUnidad, keysConvert, true);
                                                if (conversion != null)
                                                    this._unidadesFinalxRef = (conversion.Factor.Value.Value * cantidadEmp * empaqueEmp.Cantidad.Value.Value);
                                                else
                                                {
                                                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_NotExistConvertUnit));
                                                    ctrl.EditValue = this._serializadoInd ? 1 : 0;
                                                    return false;
                                                }
                                            }
                                            else
                                                this._unidadesFinalxRef = cantidadEmp;
                                        }
                                        #endregion
                                        if (this.mvtoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Salidas || this.mvtoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Traslados)
                                        {
                                            #region Verifica la cantidad de unidades a retirar
                                            decimal saldoDisponible = Convert.ToDecimal(this.txtSaldoRef.EditValue, CultureInfo.InvariantCulture) - this._unidadesFinalxRef;
                                            if (saldoDisponible < 0)
                                            {
                                                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_InvalidQuantity));
                                                ctrl.EditValue = this._serializadoInd ? 1 : 0;
                                                return false;
                                            }
                                            #endregion
                                        }
                                        this._totalUnidades = 0;
                                        row.Movimiento.CantidadUNI.Value = this._unidadesFinalxRef;
                                        foreach (var footer in this.data.Footer)
                                            this._totalUnidades += footer.Movimiento.CantidadUNI.Value.Value;
                                        this.txtTotalCantidad.EditValue = this._totalUnidades;
                                    }
                                    row.Movimiento.CantidadEMP.Value = cantidadEmp;
                                    row.Movimiento.Valor1LOC.Value = row.Movimiento.ValorUNI.Value * this._unidadesFinalxRef;
                                    this.txtValorML.EditValue = row.Movimiento.Valor1LOC.Value;
                                    if (row.Movimiento.Valor1LOC.Value.Value != 0)
                                        this.textEditControl_Leave(this.txtValorML, e);
                                    #endregion
                                    break;
                                case "txtValorUNI":
                                    #region Valor Unitario
                                    row.Movimiento.ValorUNI.Value = Convert.ToDecimal(ctrl.EditValue, CultureInfo.InvariantCulture);
                                    row.Movimiento.Valor1LOC.Value = row.Movimiento.ValorUNI.Value * this._unidadesFinalxRef;
                                    this.txtValorML.EditValue = row.Movimiento.Valor1LOC.Value;
                                    #region Realiza la conversion de moneda si es necesario
                                    if (this.multiMoneda)
                                    {
                                        row.Movimiento.Valor1EXT.Value = this._tasaCambioValue != 0 ? row.Movimiento.Valor1LOC.Value / this._tasaCambioValue : 0;
                                        this.txtValorME.EditValue = row.Movimiento.Valor1EXT.Value;
                                    } 
                                    #endregion
                                    foreach (var footer in this.data.Footer)
                                    {
                                        valorDocLoc += footer.Movimiento.Valor1LOC.Value.Value;
                                        valorExtLoc += footer.Movimiento.Valor1EXT.Value.Value;
                                    }
                                    this.txtTotalValorLoc.EditValue = valorDocLoc;
                                    this.txtTotalValorExt.EditValue = valorExtLoc;   
                                    #endregion
                                    break;
                                case "txtValorML":
                                    #region Valor ML
                                    row.Movimiento.Valor1LOC.Value = Convert.ToDecimal(ctrl.EditValue, CultureInfo.InvariantCulture);
                                    #region Realiza la conversion de moneda si es necesario
                                    if (this.multiMoneda)
                                    {
                                        row.Movimiento.Valor1EXT.Value = this._tasaCambioValue != 0 ? row.Movimiento.Valor1LOC.Value / this._tasaCambioValue : 0;
                                        this.txtValorME.EditValue = row.Movimiento.Valor1EXT.Value;
                                    }
                                    if (this._unidadesFinalxRef != 0)
                                    {
                                        row.Movimiento.ValorUNI.Value = this.monedaId == this.monedaLocal ? row.Movimiento.Valor1LOC.Value.Value / this._unidadesFinalxRef :
                                                                                               row.Movimiento.Valor1EXT.Value.Value / this._unidadesFinalxRef; ;
                                        foreach (var footer in this.data.Footer)
                                        {
                                            valorDocLoc += footer.Movimiento.Valor1LOC.Value.Value;
                                            valorExtLoc += footer.Movimiento.Valor1EXT.Value.Value;
                                        }
                                        this.txtTotalValorLoc.EditValue = valorDocLoc;
                                        this.txtTotalValorExt.EditValue = valorExtLoc;
                                    }
                                    if (this.gvDocument.DataRowCount > 0)
                                    {
                                        //GridColumn col = this.gvDocument.Columns[this.unboundPrefix + "Valor1LOC"];
                                        //this.gvDocument.FocusedColumn = col;
                                        //this.gvDocument.Focus();
                                        ////this.gvDocument.FocusedRowHandle = this.gvDocument.DataRowCount - 1;
                                        //this.gvDocument.SelectRow(this.gvDocument.DataRowCount - 1);
                                    }
                                    #endregion
                                    #endregion
                                    break;
                                case "txtValorME":
                                    #region Valor ME
                                    row.Movimiento.Valor1EXT.Value = Convert.ToDecimal(ctrl.EditValue, CultureInfo.InvariantCulture);
                                    #region Realiza la conversion de moneda si es necesario
                                    row.Movimiento.Valor1LOC.Value = this._tasaCambioValue != 0 ? row.Movimiento.Valor1EXT.Value * this._tasaCambioValue : 0;
                                    this.txtValorML.EditValue = row.Movimiento.Valor1LOC.Value;
                                    if (this._unidadesFinalxRef != 0)
                                    {
                                        decimal valorUnid = this.monedaId == this.monedaLocal ? row.Movimiento.Valor1LOC.Value.Value / this._unidadesFinalxRef :
                                                                                               row.Movimiento.Valor1EXT.Value.Value / this._unidadesFinalxRef;
                                        row.Movimiento.ValorUNI.Value = valorUnid;
                                        foreach (var footer in this.data.Footer)
                                        {
                                            valorDocLoc += footer.Movimiento.Valor1LOC.Value.Value;
                                            valorExtLoc += footer.Movimiento.Valor1EXT.Value.Value;
                                        }
                                        this.txtTotalValorLoc.EditValue = valorDocLoc;
                                        this.txtTotalValorExt.EditValue = valorExtLoc;
                                    }
                                    #endregion
                                    #endregion
                                    break;
                            }
                        }
                        else
                        {
                            MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_InvalidNumber));
                            ctrl.EditValue = Convert.ToDecimal(ctrl.EditValue, CultureInfo.InvariantCulture) * -1;
                            ctrl.Focus();
                        }
                        #endregion
                    }
                    this.gcDocument.RefreshDataSource();
                    this.gvDocument.RefreshData();
                }
                else
                {
                    this._valorTotalML = 0;
                    this._valorTotalME = 0;
                    string msg = string.Empty;
                    int count = 1;
                    #region Valida los items importados
                    List<DTO_inReferencia> refer = new List<DTO_inReferencia>();
                    foreach (DTO_inMovimientoFooter det in this.data.Footer)
                    {
                        //Valida que existan las referencias
                        DTO_inReferencia r = (DTO_inReferencia)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, false, det.Movimiento.inReferenciaID.Value, true);
                        if (r == null)
                            msg += "Línea " + count.ToString() + ": " + this._bc.GetResource(LanguageTypes.Messages, "Referencia no existe: ") + det.Movimiento.inReferenciaID.Value + "\n";
                        else
                            refer.Add(r);
                        count++;
                    } 
                    #endregion

                    if (string.IsNullOrEmpty(msg))
                    {
                        foreach (DTO_inMovimientoFooter det in this.data.Footer)
                        {
                            #region Maestras

                            #region Codigo Referencia
                            this._referenciaInv = refer.Find(x=>x.ID.Value == det.Movimiento.inReferenciaID.Value);
                            DTO_inRefTipo _tipoInv = (DTO_inRefTipo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inRefTipo, false, _referenciaInv.TipoInvID.Value, true);
                            det.Movimiento.EmpaqueInvID.Value = this._referenciaInv.EmpaqueInvID.Value;
                            det.Movimiento.DescripTExt.Value = this._referenciaInv.Descriptivo.Value;
                            det.Movimiento.MarcaDesc.Value = this._referenciaInv.MarcaInvDesc.Value;
                            det.Movimiento.RefProveedor.Value = this._referenciaInv.RefProveedor.Value;
                            #region Referencia serializada
                            if (_tipoInv.SerializadoInd.Value.Value)
                                this._serializadoInd = true;
                            else
                                this._serializadoInd = false;
                            #endregion
                            #region Trae los Parametros 1 y 2 si están habilitados
                            if (!_tipoInv.Parametro1Ind.Value.Value)
                                det.Movimiento.Parametro1.Value = this._bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Parametro1xDefecto);
                            if (!_tipoInv.Parametro2Ind.Value.Value)
                                det.Movimiento.Parametro2.Value = this._bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_Parametro2xDefecto);
                            //Une la referencia con los parametros para mostrarlos
                            #endregion
                            #region  Trae una lista de seriales segun la referencia(Salida-Traslado)
                            //this.textEditControl_Leave(this.txtCantUnidad, e);
                            #endregion

                            //if (det.Movimiento.Valor1LOC.Value.Value != 0)
                            //    this.textEditControl_Leave(this.txtValorML, e);
                            det.ReferenciaIDP1P2.Value = det.Movimiento.inReferenciaID.Value + (det.Movimiento.Parametro1.Value == this.param1xDef.ToUpper() || string.IsNullOrEmpty(det.Movimiento.Parametro1.Value) ? string.Empty : "-" + det.Movimiento.Parametro1.Value) +
                                                        (det.Movimiento.Parametro2.Value == this.param2xDef.ToUpper() || string.IsNullOrEmpty(det.Movimiento.Parametro2.Value) ? string.Empty : "-" + det.Movimiento.Parametro2.Value);
                            #endregion
                            #region Codigo Empaque
                            if (this._referenciaInv != null)
                            {
                                DTO_inEmpaque empaqueEmp = (DTO_inEmpaque)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inEmpaque, false, det.Movimiento.EmpaqueInvID.Value, true);
                                DTO_inUnidad unidadEmp = (DTO_inUnidad)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inUnidad, false, empaqueEmp.UnidadInvID.Value, true);
                                DTO_inUnidad unidadRef = (DTO_inUnidad)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inUnidad, false, this._referenciaInv.UnidadInvID.Value, true);

                                if (unidadEmp.ID.Value != unidadRef.ID.Value)
                                {
                                    #region Verifica el Tipo de Medida de la Unidad
                                    if (unidadEmp.TipoMedida.Value == unidadRef.TipoMedida.Value)
                                        det.Movimiento.EmpaqueInvID.Value = det.Movimiento.EmpaqueInvID.Value;
                                    else
                                    {
                                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_UnitInvalidEmp));
                                    }
                                    #endregion
                                    #region Si requiere conversion de unidad, verifica que exista
                                    decimal cantUnid = det.Movimiento.CantidadUNI.Value.Value;
                                    if (!empaqueEmp.UnidadInvID.Value.Equals(this._empaqueInvIdDef))
                                    {
                                        Dictionary<string, string> keysConvert = new Dictionary<string, string>();
                                        keysConvert.Add("UnidadInvID", unidadEmp.ID.Value);
                                        keysConvert.Add("UnidadBase", unidadRef.ID.Value);
                                        DTO_inConversionUnidad conversion = (DTO_inConversionUnidad)this._bc.GetMasterComplexDTO(AppMasters.inConversionUnidad, keysConvert, true);
                                        if (conversion != null)
                                        {
                                            if (cantUnid > 0)
                                                this._unidadesFinalxRef = (conversion.Factor.Value.Value * Convert.ToDecimal(this.txtCantUnidad.EditValue, CultureInfo.InvariantCulture) * empaqueEmp.Cantidad.Value.Value);
                                            det.Movimiento.CantidadUNI.Value = this._unidadesFinalxRef;
                                            foreach (var footer in this.data.Footer)
                                            {
                                                this._totalUnidades += footer.Movimiento.CantidadUNI.Value.Value;
                                            }
                                        }
                                    }
                                    else
                                        this._unidadesFinalxRef = cantUnid;
                                    #endregion
                                }
                                else
                                    det.Movimiento.EmpaqueInvID.Value = det.Movimiento.EmpaqueInvID.Value;
                            }
                            #endregion
                            #region Codigo Parametro 1
                            det.Movimiento.Parametro1.Value = det.Movimiento.Parametro1.Value;
                            det.ReferenciaIDP1P2.Value = det.Movimiento.inReferenciaID.Value + (det.Movimiento.Parametro1.Value == this.param1xDef.ToUpper() || string.IsNullOrEmpty(det.Movimiento.Parametro1.Value) ? string.Empty : "-" + det.Movimiento.Parametro1.Value) +
                                                        (det.Movimiento.Parametro2.Value == this.param2xDef.ToUpper() || string.IsNullOrEmpty(det.Movimiento.Parametro2.Value) ? string.Empty : "-" + det.Movimiento.Parametro2.Value);
                            #endregion
                            #region Codigo Parametro 2
                            det.ReferenciaIDP1P2.Value = det.Movimiento.inReferenciaID.Value + (det.Movimiento.Parametro1.Value == this.param1xDef.ToUpper() || string.IsNullOrEmpty(det.Movimiento.Parametro1.Value) ? string.Empty : "-" + det.Movimiento.Parametro1.Value) +
                                                        (det.Movimiento.Parametro2.Value == this.param2xDef.ToUpper() || string.IsNullOrEmpty(det.Movimiento.Parametro2.Value) ? string.Empty : "-" + det.Movimiento.Parametro2.Value);

                            #endregion
                            #endregion
                            #region Textos
                            #region Serial
                            if (this._serializadoInd)
                            {
                                DTO_acActivoControl activo;
                                bool serialExistCurrent = false;
                                List<DTO_acActivoControl> result;
                                foreach (var item in this.data.Footer)
                                {
                                    if (this.data.Footer.Count(x => x.Movimiento.SerialID.Value == item.Movimiento.SerialID.Value && !string.IsNullOrEmpty(item.Movimiento.SerialID.Value)) > 1)
                                    {
                                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_AlreadyExistSerialCurrent));
                                        serialExistCurrent = true;
                                        break;
                                    }
                                }
                                if (!serialExistCurrent && this.mvtoTipo != null && this.mvtoTipo.TipoMovimiento.Value.Value == (byte)TipoMovimientoInv.Entradas)
                                {
                                    #region Verifica que el serial ingresado NO exista
                                    activo = new DTO_acActivoControl();
                                    result = this._bc.AdministrationModel.acActivoControl_GetByParameterForTranfer(activo);
                                    if (result.Count == 0)
                                        det.Movimiento.SerialID.Value = det.Movimiento.SerialID.Value;
                                    else
                                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_AlreadyExistSerial));
                                    #endregion
                                }
                                else if (!serialExistCurrent && this.mvtoTipo != null && (this.mvtoTipo.TipoMovimiento.Value.Value == (byte)TipoMovimientoInv.Salidas || this.mvtoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Traslados))
                                {
                                    #region Verifica que el serial ingresado SI exista
                                    activo = new DTO_acActivoControl();
                                    activo.SerialID.Value = det.Movimiento.SerialID.Value;
                                    activo.BodegaID.Value = this.bodegaOrigen.ID.Value;
                                    activo.inReferenciaID.Value = this._referenciaInv.ID.Value;
                                    result = this._bc.AdministrationModel.acActivoControl_GetByParameterForTranfer(activo);
                                    if (result.Count > 0)
                                        det.Movimiento.ActivoID.Value = result[0].ActivoID.Value;
                                    else
                                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_NotExistSerial));
                                    #endregion
                                }
                            }
                            #endregion
                            #endregion
                            #region Valores

                            #region Cantidad Empaques
                            if (this._referenciaInv != null)
                            {
                                #region Realiza la conversion de unidad si es necesario
                                DTO_inEmpaque empaqueEmp = (DTO_inEmpaque)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inEmpaque, false, det.Movimiento.EmpaqueInvID.Value, true);
                                this._referenciaInv = (DTO_inReferencia)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, false, det.Movimiento.inReferenciaID.Value, true);
                                if (empaqueEmp.UnidadInvID.Value == this._referenciaInv.UnidadInvID.Value)
                                {
                                    this._unidadesFinalxRef = det.Movimiento.CantidadEMP.Value.Value * empaqueEmp.Cantidad.Value.Value;
                                }
                                else
                                {
                                    if (!empaqueEmp.ID.Value.Equals(this._empaqueInvIdDef))
                                    {
                                        Dictionary<string, string> keysConvert = new Dictionary<string, string>();
                                        keysConvert.Add("UnidadInvID", empaqueEmp.UnidadInvID.Value);
                                        keysConvert.Add("UnidadBase", this._referenciaInv.UnidadInvID.Value);
                                        DTO_inConversionUnidad conversion = (DTO_inConversionUnidad)this._bc.GetMasterComplexDTO(AppMasters.inConversionUnidad, keysConvert, true);
                                        if (conversion != null)
                                            this._unidadesFinalxRef = (conversion.Factor.Value.Value * det.Movimiento.CantidadEMP.Value.Value * empaqueEmp.Cantidad.Value.Value);
                                        else
                                            MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_NotExistConvertUnit));
                                    }
                                    else
                                        this._unidadesFinalxRef = det.Movimiento.CantidadEMP.Value.Value;
                                }
                                #endregion
                                this._totalUnidades = 0;
                                det.Movimiento.CantidadUNI.Value = this._unidadesFinalxRef;
                                foreach (var footer in this.data.Footer)
                                    this._totalUnidades += footer.Movimiento.CantidadUNI.Value.Value;
                            }
                            //if (det.Movimiento.Valor1LOC.Value.Value != 0)
                            //    this.textEditControl_Leave(this.txtValorML, e);
                            #endregion
                            det.Movimiento.Valor1LOC.Value = det.Movimiento.ValorUNI.Value * this._unidadesFinalxRef;
                            #region Realiza la conversion de moneda si es necesario
                            if (this.multiMoneda)
                                det.Movimiento.Valor1EXT.Value = this._tasaCambioValue != 0 ? det.Movimiento.Valor1LOC.Value / this._tasaCambioValue : 0;
                            #endregion
                            #region Valor ML / ME
                            if (this._unidadesFinalxRef != 0)
                            {
                                decimal valorUnid = this.monedaId == this.monedaLocal ? det.Movimiento.Valor1LOC.Value.Value / this._unidadesFinalxRef :
                                                                                        det.Movimiento.Valor1EXT.Value.Value / this._unidadesFinalxRef;
                                det.Movimiento.ValorUNI.Value = valorUnid;
                                this._valorTotalML += det.Movimiento.Valor1LOC.Value.Value;
                                this._valorTotalME += det.Movimiento.Valor1EXT.Value.Value;
                            }
                            #endregion
                            #endregion
                            if (this.mvtoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Salidas || this.mvtoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Traslados)
                            {
                                this.GetSaldoAvailable(det, true);
                                decimal saldoDisponible = Convert.ToDecimal(this.txtSaldoRef.EditValue, CultureInfo.InvariantCulture) - this._unidadesFinalxRef;
                                if (saldoDisponible < 0)
                                {
                                    //MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_InvalidQuantity));
                                    //ctrl.EditValue = this._serializadoInd ? 1 : 0;
                                    //return;
                                }
                            }

                        }
                    }
                    else
                    {
                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, msg),"Error de datos");
                        res = false;
                    }
                      
                }
                return res;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-DocumentMvtoForm.cs", "AssignData"));
                return false;
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
            FormProvider.Master.itemCopy.Visible = false;
            FormProvider.Master.itemPaste.Visible = false;         
            FormProvider.Master.itemExport.Visible = false;
            FormProvider.Master.tbBreak1.Visible = false;
            FormProvider.Master.itemRevert.Visible = false;
            FormProvider.Master.itemDelete.Visible = false;
            FormProvider.Master.itemSendtoAppr.Visible = false;
            FormProvider.Master.itemGenerateTemplate.Visible = true;
            FormProvider.Master.itemImport.Visible = true;
            if (FormProvider.Master.LoadFormTB)
            {
                FormProvider.Master.itemImport.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Import);
               // FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr);
                //FormProvider.Master.itemPrint.Enabled = false;
            }
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
                string msgTitleWarning = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
                string msgLostInfo = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.LostInfo);

                if (MessageBox.Show(msgLostInfo, msgTitleWarning, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this._bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), this._bc.AdministrationModel.User);
                    base.Form_FormClosing(sender, e);
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        #endregion

        #region Eventos header superior

        /// <summary>
        /// Valida que el usuario haya ingresado una fecha
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void dtFecha_Leave(object sender, EventArgs e)
        {
            base.dtFecha_Leave(sender, e);
            bool tc = this.AsignarTasaCambio();
        }

        #endregion

        #region Eventos Grilla

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
                }

                if (e.Button.ImageIndex == 6) //Agrega Nuevo Item
                {
                    #region Agrega Nuevo
                    if (this.gvDocument.ActiveFilterString != string.Empty)
                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoAddInFilter));
                    else
                    {
                        this.deleteOP = false;
                        if (this.isValid)
                        {
                            this.newReg = true;
                            this.AddNewRow();
                            this.EnableFooter(true);
                            this.CleanFooter(true);
                            // this.masterServicio.Focus();
                        }
                        else
                        {
                            bool isV = this.ValidateRow(this.gvDocument.FocusedRowHandle);
                            if (isV)
                            {
                                this.newReg = true;
                                this.AddNewRow();
                                this.EnableFooter(true);
                                this.CleanFooter(true);
                            }
                        }
                    } 
                    #endregion
                }
                else if (e.Button.ImageIndex == 7) //Elimina Nuevo Item
                {
                    #region Elimina un item
                    string msgTitleDelete = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Delete);
                    string msgDelete = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Register);
                    this._valorTotalML = 0;
                    this._valorTotalME = 0;
                    this._totalUnidades = 0;
                    //Revisa si desea cargar los temporales
                    if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        this.deleteOP = true;
                        int rowHandle = this.gvDocument.FocusedRowHandle;

                        if (this.data.Footer.Count == 1)
                        {
                            MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.RowsNeeded));
                            e.Handled = true;
                        }
                        else
                        {
                            this.data.Footer.RemoveAll(x => x.Movimiento.Index == this.indexFila);
                            //Si borra el primer registro
                            if (rowHandle == 0)
                                this.gvDocument.FocusedRowHandle = 0;
                            //Si selecciona el ultimo
                            else
                                this.gvDocument.FocusedRowHandle = rowHandle - 1;

                            this.UpdateTemp(this.data);

                            this.gvDocument.RefreshData();
                            //this.RowIndexChanged(this.gvDocument.FocusedRowHandle, true);
                            foreach (var footer in this.data.Footer)
                            {
                                this._totalUnidades += footer.Movimiento.CantidadUNI.Value.Value;
                                this._valorTotalML += footer.Movimiento.Valor1LOC.Value.Value;
                                this._valorTotalME += footer.Movimiento.Valor1EXT.Value.Value;
                            }
                            this.txtTotalCantidad.EditValue = this._totalUnidades;
                            this.txtTotalValorExt.EditValue = this._valorTotalME;
                            this.txtTotalValorLoc.EditValue = this._valorTotalML;
                        }
                    }
                    e.Handled = true; 
                    #endregion
                }
                else if (e.Button.ImageIndex ==  9 && this.mvtoTipo != null && this.mvtoTipo.TipoMovimiento.Value != (byte)TipoMovimientoInv.Entradas) //Agrega Varios items
                {
                    this._totalUnidades = 0;
                    ModalTrasladoInv mod = new ModalTrasladoInv(this.data.Footer,this.data.Header.BodegaOrigenID.Value, this.data.Header.BodegaDestinoID.Value, this._tasaCambioValue);
                    mod.ShowDialog();
                    foreach (DTO_inMovimientoFooter f in mod.DetalleSelected.FindAll(x => x.Movimiento.CantidadEMP.Value > 0 && x.SelectInd.Value.Value))
                    {
                        f.Movimiento.TerceroID.Value = this.documentID == AppDocuments.TransaccionManual ? this.terceroHeader : string.Empty;
                        f.Movimiento.ProyectoID.Value = string.IsNullOrEmpty(f.Movimiento.ProyectoID.Value) ? this.proyectoHeader : f.Movimiento.ProyectoID.Value;
                        f.Movimiento.CentroCostoID.Value = this.centroCostoHeader;
                        f.Movimiento.MvtoTipoInvID.Value = this.mvtoTipo.ID.Value;
                        f.Movimiento.BodegaID.Value = this.data.Header.BodegaOrigenID.Value;
                    }
                    this.data.Footer = mod.DetalleSelected.FindAll(x => x.Movimiento.CantidadEMP.Value > 0);
                    this.data.Header.TipoTraslado.Value = null;
                    this.data.Header.TipoTraslado.Value = mod.TipoTrasladoSelect != 4 && mod.TipoTrasladoSelect != 5? mod.TipoTrasladoSelect : this.data.Header.TipoTraslado.Value;
                    foreach (DTO_inMovimientoFooter footer in this.data.Footer)
                        this._totalUnidades += footer.Movimiento.CantidadUNI.Value.Value;

                    this.txtTotalCantidad.EditValue = this._totalUnidades;
                    this.UpdateTemp(this.data);
                    this.LoadEditGridData(false, 0);
                    this.LoadData(true);
                    FormProvider.Master.itemSave.Enabled = true;
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

                if (fieldName == "Valor1LOC" || fieldName == "Valor2LOC" || fieldName == "Valor1EXT" || fieldName == "Valor2EXT")
                {
                    e.RepositoryItem = this.editValue;
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
                base.gcDocument.EmbeddedNavigator.Buttons.CustomButtons[1].Enabled = this.data.Footer.Count > 1 ? true : false;
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

        /// <summary>
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle >= 0)
                {
                    this._rowCurrent = (DTO_inMovimientoFooter)this.gvDocument.GetRow(e.FocusedRowHandle);
                    if (this._rowCurrent != null)
                    {
                        this.newReg = false;
                        this.indexFila = this._rowCurrent.Movimiento.Index;
                        this.LoadEditGridData(false, e.FocusedRowHandle);
                        this.isValid = this.ValidateRow(e.FocusedRowHandle);
                        this.gcDocument.EmbeddedNavigator.Buttons.Remove.Enabled = true; 
                    }
                }
                else
                    this.gcDocument.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentMvtoForm.cs", "gvDocument_FocusedRowChanged"));
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

            #region Validacion para llaves foraneas
            if (this.ValidateRow(this.gvDocument.FocusedRowHandle))
            {
               
            }
           
            #endregion            
           
        }


        #endregion

        #region Eventos Detalle (footer)

        /// <summary>
        /// Evento que se ejecuta al salir del control de maestra
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterDetails_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.gvDocument.RowCount > 0)
                {
                    this.AssignData(sender, e, this._rowCurrent, false);                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-DocumentMvtoForm.cs", "masterDetails_Leave: " + ex.Message));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al salir del campo de texto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void textControl_Leave(object sender, EventArgs e)
        {
            try
            {
            TextBox ctrl = (TextBox)sender;
            int index = this.NumFila;
            bool serialExistCurrent = false;
            this.AssignData(sender, e, this._rowCurrent, false);             
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-DocumentMvtoForm.cs", "textControl_Leave: " + ex.Message));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al salir del campo de texto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void cmbEstado_Leave(object sender, EventArgs e)
        {
            ComboBoxEx ctrl = (ComboBoxEx)sender;
            int index = this.NumFila;

            try
            {
                this.AssignData(sender, e, this._rowCurrent, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-DocumentMvtoForm.cs", "cmbEstado_Leave: " + ex.Message)); 
            }
            this.gvDocument.RefreshData();
        }

        /// <summary>
        /// Evento que se ejecuta al salir del campo de texto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void textEditControl_Leave(object sender, EventArgs e)
        {
            TextEdit ctrl = (TextEdit)sender;
            int index = this.NumFila;
            try
            {
              
                if (Convert.ToDecimal(ctrl.EditValue, CultureInfo.InvariantCulture) >= 0)
                {
                    this.AssignData(sender, e, this._rowCurrent, false);
                }
                else
                {
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_InvalidNumber));
                    ctrl.EditValue = Convert.ToDecimal(ctrl.EditValue, CultureInfo.InvariantCulture) * -1;
                    ctrl.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-DocumentMvtoForm.cs", "textEditControl_Leave: " + ex.Message));
            }
        }

        /// <summary>
        /// Valida que solo numeros se pueden escribir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtFooter_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox ctrl = (TextBox)sender;
            int index = this.NumFila;

            if (ctrl.Name == "txtCantEmpaque")
            {
                if (!Regex.IsMatch(e.KeyChar.ToString(), "\\d+") && e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back && e.KeyChar != ',')
                    e.Handled = true;
                if (e.KeyChar == 46)
                    e.Handled = true;
            }
            else
            {
                if (!Regex.IsMatch(e.KeyChar.ToString(), "\\d+") && e.KeyChar != (Char) Keys.Delete &&
                    e.KeyChar != (Char) Keys.Back)
                    e.Handled = true;
                if (e.KeyChar == 46)
                    e.Handled = true;
            }
        }
        
        /// <summary>
        /// Valida las teclas digitadas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtValor_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                TextEdit ctrl = (TextEdit)sender;
                int index = this.NumFila;

                if (e.KeyChar == (Char)Keys.Enter)
                {
                    if (this.ValidateRow(index))
                    {
                        this.AddNewRow();
                        this.masterReferencia.Focus();
                    }
                }
            }
            catch (Exception ex)
            {                
               MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-DocumentMvtoForm.cs", "txtValor_KeyPress: " + ex.Message)); 
            }          
        }

        /// <summary>
        /// Valida las teclas digitadas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                TextEdit ctrl = (TextEdit)sender;
                int index = this.NumFila;

                if (e.KeyChar == (Char)Keys.Enter)
                {
                    if (this.ValidateRow(index))
                    {
                        this.AddNewRow();
                        this.masterReferencia.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                 MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-DocumentMvtoForm.cs", "txtCantidad_KeyPress: " + ex.Message)); 
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
                string bodega = this.bodegaOrigen != null && this.tipoMovActual != TipoMovimientoInv.Entradas ? this.bodegaOrigen.ID.Value : string.Empty;
                ModalReferenciasFilter mod = new ModalReferenciasFilter(bodega, this.documentID);
                mod.ShowDialog();
                this.gvDocument.SetRowCellValue(this.gvDocument.FocusedRowHandle, this.unboundPrefix + "inReferenciaID", mod.IDSelected);
                this.masterReferencia.Value = mod.IDSelected;
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
                        this._bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), this._bc.AdministrationModel.User);
                        this.cleanDoc = true;

                        this.validHeader = false;
                        this.ValidHeaderTB();
                    }
                }

                if (this.cleanDoc)
                {
                    this.newDoc = true;
                    this.deleteOP = true;
                    this.isFacturaVenta = false;
                    this.EnableFooter(false);
                    this.CleanFooter(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-DocumentMvtoForm.cs", "TBNew: " + ex.Message));
            }
        }

        /// <summary>
        /// Boton para iniciar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            this.gcDocument.Focus();
        }

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
            if (this.ValidateRow(this.gvDocument.FocusedRowHandle))
            {
                this.gvDocument.ActiveFilterString = string.Empty;
                if (this.gvDocument.RowCount > 0)
                {
                    this.gvDocument.MoveFirst();
                }
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
                    MasterQuery mq = new MasterQuery(this, this.documentID, this._bc.AdministrationModel.User.ReplicaID.Value.Value, false, typeof(DTO_inMovimientoFooter), typeof(Filtrable));
                    #region definir Fks
                    //mq.SetFK("TerceroID", AppMasters.coTercero, this._bc.CreateFKConfig(AppMasters.coTercero));
                    //mq.SetFK("ProyectoID", AppMasters.coProyecto, this._bc.CreateFKConfig(AppMasters.coProyecto));
                    //mq.SetFK("CentroCostoID", AppMasters.coCentroCosto, this._bc.CreateFKConfig(AppMasters.coCentroCosto));
                    //mq.SetFK("LugarGeograficoID", AppMasters.glLugarGeografico, this._bc.CreateFKConfig(AppMasters.glLugarGeografico));
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
                    this._bc.AdministrationModel.DataCopied = this.data.Footer;
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ResultOK));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-DocumentMvtoForm.cs", "TBCopy: " + ex.Message));
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
                List<DTO_ComprobanteFooter> compDet = new List<DTO_ComprobanteFooter>();
                try
                {
                    object o = this._bc.AdministrationModel.DataCopied;
                    compDet = (List<DTO_ComprobanteFooter>)o;

                    if (compDet == null)
                    {
                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_InvalidCompPaste));
                        return;
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_InvalidCompPaste));
                    return;
                }

                //Revisa que cumple las condiciones
                if (!this.ReplaceDocument())
                    return;

                this._bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), this._bc.AdministrationModel.User);
                //this.data.Footer = compDet;
                this.LoadData(true);
                this.UpdateTemp(this.data);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-DocumentMvtoForm.cs", "TBPaste: " + ex.Message));
            }
        }

        /// <summary>
        /// Nuevo
        /// </summary>
        public override void TBImport()
        {
            if (this.ValidateHeader())
            {
                this.pasteRet = CopyPasteExtension.PasteFromClipBoard(this.formatReferencias);
                Thread process = new Thread(this.ImportThreadTareas);
                process.Start();
            }
            else
            {
                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, "El encabezado no esta completo"));
            }
        }

        /// <summary>
        /// Boton para exportar la data actual
        /// </summary>
        public override void TBGenerateTemplate()
        {
            try
            {
                ExcelGenerator excell_app = new ExcelGenerator();

                int row = 1;
                int col = 1;
                string[] cols = this.formatReferencias.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries);
                for (int colIndex = 0; colIndex < cols.Length; colIndex++)
                {
                    string colName = cols[colIndex];
                    excell_app.AddData(row, col, colName);
                    col++;
                }

                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SuccessTemplate));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo que se ejecuta cualdo el usuario va a imprtar datos al detalle
        /// </summary>
        public void ImportThreadTareas()
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            List<DTO_MigracionMvtos> listImport = new List<DTO_MigracionMvtos>();
            try
            {
                if (this.pasteRet.Success)
                {
                    var text = pasteRet.MsgResult;
                    string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    #region Variables de función y mensajes de error
                    //Lista con los dtos a subir y Fks a validas                    
                    Dictionary<string, List<Tuple<string, bool>>> fks = new Dictionary<string, List<Tuple<string, bool>>>();
                    List<string> fkNames = new List<string>();
                    Dictionary<string, string> colNames = new Dictionary<string, string>();
                    Dictionary<string, Object> colVals = new Dictionary<string, Object>();
                    //Mensajes de error
                    string msgEmptyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField);
                    string msgFkNotFound = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FkNotFound);
                    string msgNoRel = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ImpNoRel);
                    string msgInvalidFormat = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidFormat);
                    string msgNoCopyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoCopyField);
                    string msgIncompleteLine = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.IncompleteLine);
                    string msgFecha = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidDate);


                    //Popiedades
                    DTO_MigracionMvtos referencia = new DTO_MigracionMvtos();
                    bool createDTO = true;
                    bool validList = true;
                    #endregion
                    #region Llena las listas de las columnas
                    List<string> cols = this.formatReferencias.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    PropertyInfo[] pis = typeof(DTO_MigracionMvtos).GetProperties();

                    //Recorre el objeto y revisa el nombre real de la columna
                    foreach (PropertyInfo pi in pis)
                    {
                        if (!Attribute.IsDefined(pi, typeof(NotImportable)))
                        {
                            string colRsx = _bc.GetResource(LanguageTypes.Forms, AppDocuments.TransaccionManual.ToString() + "_" + pi.Name);
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

                    //Fks
                    //fks.Add(this._clienteRsx, new List<Tuple<string, bool>>());

                    #endregion
                    #region Llena información para enviar a la grilla (lee filas)
                    FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ReadRows) });
                    int percent = 0;
                    for (int i = 0; i < lines.Length; ++i)
                    {
                        #region Aumenta el porcentaje y revisa que tenga lineas para leer
                        FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                        percent = ((i + 1) * 100) / (lines.Length);

                        if (FormProvider.Master.ProcessCanceled(this.documentID))
                        {
                            result.Details = new List<DTO_TxResultDetail>();
                            result.ResultMessage = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser);
                            result.Result = ResultValue.NOK;
                            break;
                        }

                        if (lines.Length == 1)
                        {
                            result.ResultMessage = msgNoCopyField;
                            result.Result = ResultValue.NOK;
                            validList = false;
                        }
                        #endregion
                        #region Recorre todas las columnas y verifica que tengan datos validos
                        string[] line = lines[i].Split(new string[] { CopyPasteExtension.tabChar }, StringSplitOptions.None);
                        if (i > 0 && line.Length > 0)
                        {
                            createDTO = true;

                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                            rd.line = i;
                            rd.Message = "OK";

                            #region Info básica
                            //Llena los valores de las columnas (manda error si el numero de columnas al importar es menor al necesario)
                            if (line.Length < cols.Count)
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
                                for (int colIndex = 0; colIndex < cols.Count; colIndex++)
                                {
                                    string colRsx = cols[colIndex];
                                    colVals[colRsx] = line[colIndex];
                                }
                            }

                            #endregion
                            #region Creacion de DTO y validacion Formatos
                            referencia = new DTO_MigracionMvtos();
                            if (createDTO)
                            {
                                for (int colIndex = 0; colIndex < cols.Count; colIndex++)
                                {
                                    string colRsx = cols[colIndex];
                                    try
                                    {
                                        string colName = colNames[colRsx];
                                        string colValue = colVals[colRsx].ToString().Trim();

                                        #region Validacion Formatos
                                        UDT udt;
                                        PropertyInfo pi = referencia.GetType().GetProperty(colName);
                                        udt = pi != null ? (UDT)pi.GetValue(referencia, null) : null;
                                        PropertyInfo piUDT = udt.GetType().GetProperty("Value");

                                        #region Comprueba los valores solo para los booleanos
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
                                            colValue = colVal;
                                            colVals[colRsx] = colVal;
                                        }
                                        #endregion
                                        else
                                        {
                                            if (colValue != string.Empty)
                                            {
                                                #region Valores de Fecha
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
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDate);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                #endregion
                                                #region Valores Numericos
                                                else if (piUDT.PropertyType.Equals(typeof(int)) || piUDT.PropertyType.Equals(typeof(Nullable<int>)))
                                                {
                                                    try
                                                    {
                                                        int val = Convert.ToInt32(colValue);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatInvalidNumber);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                else if (piUDT.PropertyType.Equals(typeof(long)) || piUDT.PropertyType.Equals(typeof(Nullable<long>)))
                                                {
                                                    try
                                                    {
                                                        long val = Convert.ToInt64(colValue);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatInteger);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                else if (piUDT.PropertyType.Equals(typeof(short)) || piUDT.PropertyType.Equals(typeof(Nullable<short>)))
                                                {
                                                    try
                                                    {
                                                        short val = Convert.ToInt16(colValue);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatLimitNumber);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                else if (piUDT.PropertyType.Equals(typeof(byte)) || piUDT.PropertyType.Equals(typeof(Nullable<byte>)))
                                                {
                                                    try
                                                    {
                                                        byte val = Convert.ToByte(colValue);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatNumberRange);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                else if (piUDT.PropertyType.Equals(typeof(decimal)) || piUDT.PropertyType.Equals(typeof(Nullable<decimal>)))
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
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDecimal);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                #endregion
                                            }
                                        }
                                        //Asigna el valor al DTO
                                        udt.ColRsx = colRsx;
                                        if (createDTO && !string.IsNullOrWhiteSpace(colValue))
                                            udt.SetValueFromString(colValue);
                                        #endregion
                                    }
                                    catch (Exception ex1)
                                    {
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = colRsx;
                                        rdF.Message = this._bc.GetResourceForException(ex1, "WinApp", "PlaneacionCostos.cs - Creacion de DTO y validacion Formatos");
                                        rd.DetailsFields.Add(rdF);
                                        createDTO = false;
                                    }
                                }
                            }
                            #endregion
                            #region Carga la informacion de los resultados
                            if (rd.DetailsFields.Count > 0)
                            {
                                result.Details.Add(rd);
                                rd.Message = "NOK";
                                result.Result = ResultValue.NOK;
                                createDTO = false;
                            }

                            if (createDTO)
                                listImport.Add(referencia);
                            else
                                validList = false;
                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                    #region Valida las restricciones particulares de la migracion
                    if (validList)
                    {
                        #region Variables generales
                        result = new DTO_TxResult();
                        result.Result = ResultValue.OK;
                        result.Details = new List<DTO_TxResultDetail>();

                        int i = 0;
                        //percent = 0;

                        #endregion
                        foreach (DTO_MigracionMvtos dto in listImport)
                        {
                            #region Aumenta el porcentaje y revisa que tenga lineas para leer
                            //this.Invoke(this.UpdateProgressDelegate, new object[] { percent });
                            //percent = ((i + 1) * 100) / (listImport.Count);
                            i++;

                            if (FormProvider.Master.ProcessCanceled(this.documentID))
                            {
                                result.Details = new List<DTO_TxResultDetail>();
                                result.ResultMessage = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser);
                                result.Result = ResultValue.NOK;
                                break;
                            }
                            #endregion
                            #region Definicion de variables
                            //Variables de resultados
                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                            rd.line = i;
                            rd.Message = "OK";
                            createDTO = true;
                            #endregion
                            #region Validaciones particulares del DTO
                            this.ValidateDataImport(dto,rd, msgFkNotFound, msgEmptyField);
                            if (rd.DetailsFields.Count > 0)
                            {
                                rd.Message = "NOK";
                                result.Details.Add(rd);
                            }
                            #endregion
                        }
                        if (result.Details.Count > 0)
                        {
                            result.Result = ResultValue.NOK;
                            validList = false;
                        }

                    }
                    #endregion
                    if (validList)
                    {                    
                        this.data.Footer= new List<DTO_inMovimientoFooter>();
                        int indexTarea = 0;
                        foreach (var det in listImport)
                        {
                           
                            DTO_inMovimientoFooter d = new DTO_inMovimientoFooter();
                            d.Movimiento.Index = indexTarea;
                            d.Movimiento.EmpresaID.Value = this.empresaID;
                            d.Movimiento.TerceroID.Value = this.documentID == AppDocuments.TransaccionManual ? this.terceroHeader : string.Empty;
                            d.Movimiento.ProyectoID.Value = this.proyectoHeader;
                            d.Movimiento.CentroCostoID.Value = this.centroCostoHeader;
                            d.Movimiento.BodegaID.Value = this.bodegaOrigen != null ? this.bodegaOrigen.ID.Value : string.Empty;
                            d.Movimiento.inReferenciaID.Value = det.inReferenciaID.Value;
                            if (this.costeoGrupoOri != null && this.costeoGrupoOri.CosteoTipo.Value.Value == (byte)TipoCosteoInv.SinCosto)
                                d.Movimiento.EstadoInv.Value = (byte)EstadoInv.SinCosto;
                            else
                                d.Movimiento.EstadoInv.Value = det.EstadoInv.Value != null? det.EstadoInv.Value: (byte)EstadoInv.Activo;//nuevo
                            d.Movimiento.Parametro1.Value = string.Empty;
                            d.Movimiento.Parametro2.Value = string.Empty;
                            d.Movimiento.ActivoID.Value = 0;
                            d.Movimiento.MvtoTipoInvID.Value = this.mvtoTipo.ID.Value;
                            d.Movimiento.IdentificadorTr.Value = 0;
                            d.Movimiento.SerialID.Value = det.SerialID.Value;
                            d.Movimiento.EmpaqueInvID.Value = det.EmpaqueInvID.Value;                         
                            d.Movimiento.CantidadEMP.Value = det.CantidadEMP.Value;
                            d.Movimiento.CantidadUNI.Value = 0;
                            d.Movimiento.CantidadDoc.Value = 0;
                            if (mvtoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Entradas)
                                d.Movimiento.EntradaSalida.Value = (byte)EntradaSalida.Entrada;
                            else if (mvtoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Salidas || mvtoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Traslados)
                                d.Movimiento.EntradaSalida.Value = (byte)EntradaSalida.Salida;
                            d.Movimiento.DescripTExt.Value = string.Empty;
                            d.Movimiento.ValorUNI.Value = det.ValorUNI.Value;
                            d.Movimiento.DocSoporte.Value = det.DocSoporte.Value == null ? 0 : det.DocSoporte.Value;
                            d.Movimiento.DocSoporteTER.Value = string.Empty;
                            d.Movimiento.Valor1LOC.Value = 0;
                            d.Movimiento.Valor2LOC.Value = 0;
                            d.Movimiento.Valor1EXT.Value = 0;
                            d.Movimiento.Valor2EXT.Value = 0;                         
                            this.data.Footer.Add(d);
                            indexTarea++;
                        }
                        bool res = this.AssignData(null, null, null, true);
                        if (!res)
                        {
                            result.Result = ResultValue.NOK;
                            this.data.Footer = new List<DTO_inMovimientoFooter>();
                        }
                    }
                }
                else
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = this.pasteRet.MsgResult;
                }
                #region Actualiza la información de la grilla
                if (result.Result == ResultValue.OK)
                {                   
                    MessageForm frm = new MessageForm(result);
                    if (result.Result.Equals(ResultValue.OK))
                        this.Invoke(this.endImportarDelegate);
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                }
                else
                {
                    MessageForm frm = new MessageForm(result);
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                    listImport = new List<DTO_MigracionMvtos>();
                }
                FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, 100 });
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentMvtoForm.cs", "ImportThreadTareas"));
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        } 

        #endregion
    }
}