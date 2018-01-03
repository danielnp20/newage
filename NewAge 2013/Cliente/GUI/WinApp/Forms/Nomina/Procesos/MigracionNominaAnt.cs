using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using DevExpress.XtraGrid.Columns;
using NewAge.DTO.Negocio;
using DevExpress.Data;
using NewAge.DTO.Resultados;
using System.Reflection;
using NewAge.DTO.Attributes; 
using NewAge.DTO.UDT;
using System.Threading;
using System.Diagnostics;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class MigracionNominaAnt : ProcessForm
    {
        #region Delegados

        /// <summary>
        /// Delegado que finaliza el proceso de validacion 
        /// </summary>
        public delegate void EndImportar();
        public EndImportar endImportarDelegate;
        public void EndImportarMethod()
        {
            this.Enabled = true;
            
            this.btnTemplate.Enabled = true;
            this.btnImportar.Enabled = true;

            if (this._isOK)
            {
                MessageForm frm = new MessageForm(this.result);
                this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });

                this.txtValor.EditValue = this._data.Sum(x=>x.Valor.Value);

                this.btnInconsistencias.Enabled = this.result.Details.Count > 0 ? true : false;
                this.btnProcesar.Enabled = true;
            }
            else
            {
                this.btnInconsistencias.Enabled = !this.pasteRet.Success ? false : true;
                this.masterConceptoNOM.Enabled = true;
                this.txtValor.Enabled = true;
            }
        }

        /// <summary>
        /// Delegado que finaliza el proceso de validaciones del servidor
        /// </summary>
        public delegate void EndProcesar();
        public EndProcesar endProcesarDelegate;
        public void EndProcesarMethod()
        {
            this.Enabled = true;
            
            this.btnTemplate.Enabled = true;
            this.btnImportar.Enabled = true;
            this.btnInconsistencias.Enabled = false;

            try
            {
                if (this.result != null && this.result.Result == ResultValue.NOK)
                {
                    this.btnInconsistencias.Enabled = true;
                    this.validarInconsistencias = true;
                    this._isOK = false;
                }
                else if (this.result != null && this.result.Details.Count == 0)
                {
                    this.result.ResultMessage = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ResultOK);
                    MessageForm frm = new MessageForm(this.result);
                    this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });
                    this._isOK = true;
                }
                else
                {
                    this.btnInconsistencias.Enabled = true;
                    this._isOK = true;
                }

                if (this._isOK || !this.validarInconsistencias)
                {
                    this.btnProcesar.Enabled = true;
                    this.txtValor.EditValue = 0;
                    this.masterConceptoNOM.Value = string.Empty;
                    this.masterConceptoNOM.EnableControl(true);
                }
                this._consecutivoAnticipo = _bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_ConsecutivoAnticipo);
                this._consecutivoAnticipo = string.IsNullOrEmpty(this._consecutivoAnticipo) ? "1" : this._consecutivoAnticipo;

            }
            catch (Exception ex)
            {                
                 MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MigracionNominaAnt.cs", "EndProcesarMethod"));
            }
        }

        /// <summary>
        /// Delegado que finaliza el proceso imprimir las inconsistencias
        /// </summary>
        public delegate void EndInconsistencias();
        public EndInconsistencias endInconsistenciasDelegate;
        public void EndInconsistenciasMethod()
        {
            this.Enabled = true;
        }

        #endregion

        #region Variables

        private BaseController _bc = BaseController.GetInstance();
        //Variables para proceso
        private PasteOpDTO pasteRet;
        //Variables para importar
        private string format;
        private string formatSeparator = "\t";
        //Variables del formulario
        private bool _isOK;
        private List<DTO_noNovedadesNomina> _data;
        private List<DTO_CuentaXPagar> _listCxPs;
        private DateTime periodo = DateTime.Now;
        bool isMensual = false;
        private bool validarInconsistencias;
        //Variables para la importacion
        private DTO_TxResult result;
        private List<DTO_TxResult> results;
        private List<string> codigos = new List<string>();
        private string reportName;
        private string fileURl;
        private string monedaId;
        private string monedaLocal;
        private string defTercero = string.Empty;
        private string defPrefijo = string.Empty;
        private string defProyecto = string.Empty;
        private string defCentroCosto = string.Empty;
        private string defLineaPresupuesto = string.Empty;
        private string defConceptoCargo = string.Empty;
        private string defLugarGeo = string.Empty;
        private bool _saveCxPInd = false;
        private string _consecutivoAnticipo = string.Empty;
        #endregion

        public MigracionNominaAnt()
        {
            //InitializeComponent();
        }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.documentID = AppProcess.MigracionNominaAnt;

            this.InitializeComponent();
            FormProvider.LoadResources(this, this.documentID);

            //Inicializa los delegados
            this.endImportarDelegate = new EndImportar(this.EndImportarMethod);
            this.endProcesarDelegate = new EndProcesar(this.EndProcesarMethod);
            this.endInconsistenciasDelegate = new EndInconsistencias(this.EndInconsistenciasMethod);

            //Carga la configuracion inicial
            this._isOK = false;

            this._data = new List<DTO_noNovedadesNomina>();
            //this.btnImportar.Enabled = false;
            this.btnProcesar.Enabled = false;
            this.btnInconsistencias.Enabled = false;

            //Carga la info inicial de los controles (centro de pago y periodo)
            _bc.InitMasterUC(this.masterConceptoNOM, AppMasters.noConceptoNOM, true, true, true, true);

            //Periodo
            _bc.InitPeriodUC(this.dtPeriod, 0);
            string periodoStr = _bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_Periodo);
            this.periodo = Convert.ToDateTime(periodoStr);
            this.dtPeriod.DateTime = this.periodo;

            //Carga los valores por defecto
            this.defPrefijo = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
            this.defProyecto = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
            this.defCentroCosto = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
            this.defLineaPresupuesto = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
            this.defConceptoCargo = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
            this.defLugarGeo = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
            //Carga info de las monedas
            this.monedaLocal = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
            string monedaExtranjera = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
            if (!this._bc.AdministrationModel.MultiMoneda)
                this.monedaId = monedaLocal;
            else
                this.monedaId = monedaExtranjera;

            //Variable para valida inconsistencias
            string validar = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_AnalisisInconsistenciaNomina);
            if (validar == "0")
                this.validarInconsistencias = false;
            else
                this.validarInconsistencias = true;

            //Asigna el formato
            this.format = _bc.GetImportExportFormat(typeof(DTO_noNovedadesNomina), AppProcess.MigracionNominaAnt);
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Trae el documento id de una maestra de acuerdo al nombre de un campo
        /// </summary>
        /// <param name="colName">Nombre del campo</param>
        /// <returns>Retorna el documento id de una maestra</returns>
        private int GetMasterDocumentID(string colName)
        {
            //Comprobante
            if (colName == "ConceptoNoID")
                return AppMasters.noConceptoNOM;
            else if (colName == "EmpleadoID")
                return AppMasters.noEmpleado;

            return 0;
        }

        /// <summary>
        /// Valida un DTO de comprobante footer en la importacion
        /// </summary>
        /// <param name="dto">DTO a validar</param>
        /// <param name="rd">Variable que va guardando los mensajes de error</param>
        /// <param name="msgFecha">Mensaje que indica que la fecha esta en un periodo incorrecto</param>
        /// <param name="msgNoRel">Mensaje que indica que toca tener un valor de cliente, codigo de empleado o libranza</param>
        /// <param name="msgPositive">Mensaje de solo acepta valores positivos</param>
        /// <param name="msgEmptyField">Mensaje de campo vacio</param>
        /// <param name="msgClienteRepetido">Mensaje para cliente repetido sin libranza</param>
        /// <param name="msgCodCliente">Mensaje para indicar que no se puede poner el codigo y el cliente</param>
        private void ValidateDataImport(DTO_noNovedadesNomina dto, DTO_TxResultDetail rd, string msgFecha, string msgNoRel, string msgPositive, string msgEmptyField)
        {
            try
            {
                #region Validacion ConceptoNOID
                if (string.IsNullOrWhiteSpace(dto.ConceptoNOID.Value))
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppProcess.MigracionNominaAnt + "_ConceptoNOID");
                    rdF.Message = msgEmptyField;
                    rd.DetailsFields.Add(rdF);
                }
                else
                {
                    if (dto.ConceptoNOID.Value != this.masterConceptoNOM.Value)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppProcess.MigracionNominaAnt + "_ConceptoNOID");
                        rdF.Message = "Diferente del digitado";
                        rd.DetailsFields.Add(rdF);
                    }
                }
                #endregion
                #region Validacion EmpleadoID
                if (string.IsNullOrWhiteSpace(dto.EmpleadoID.Value))
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppProcess.MigracionNominaAnt + "_EmpleadoID");

                    rdF.Message = msgEmptyField;
                    rd.DetailsFields.Add(rdF);
                }
                else
                {
                    DTO_MasterBasic existEmple = (DTO_MasterBasic)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noEmpleado, false, dto.EmpleadoID.Value, true);
                    if (existEmple == null)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppProcess.MigracionNominaAnt + "_EmpleadoID");

                        rdF.Message = dto.EmpleadoID.Value + " No existe";
                        rd.DetailsFields.Add(rdF);
                    }
                    DTO_MasterBasic existTercero = (DTO_MasterBasic)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, dto.EmpleadoID.Value, true);
                    if (existTercero == null)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppProcess.MigracionNominaAnt + "_Tercero");

                        rdF.Message = dto.EmpleadoID.Value + " No existe";
                        rd.DetailsFields.Add(rdF);
                    }

                }
                #endregion
                #region Validacion de Valor

                if (dto.Valor.Value == null)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppProcess.MigracionNominaAnt + "_Valor");
                    rdF.Message = msgEmptyField;
                    rd.DetailsFields.Add(rdF);
                }
                else if (dto.Valor.Value <= 0)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppProcess.MigracionNominaAnt + "_Valor");
                    rdF.Message = msgPositive;
                    rd.DetailsFields.Add(rdF);
                }

                #endregion
                #region Validacion de FijaInd
                //if (dto.FijaInd.Value == null)
                //{
                //    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                //    rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppProcess.MigracionNominaAnt + "_FijaInd");
                //    rdF.Message = msgEmptyField;
                //    rd.DetailsFields.Add(rdF);
                //}

                #endregion
                #region Validacion de PeriodoPago
                if (dto.PeriodoPago.Value == null || dto.PeriodoPago.Value > 3)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppProcess.MigracionNominaAnt + "_PeriodoPago");
                    rdF.Message = msgEmptyField;
                    rd.DetailsFields.Add(rdF);
                }

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MigracionNominaAnt.cs", "ValidateDataImport"));
            }
        }

        /// <summary>
        /// Construye las novedades para guardar
        /// </summary>
        private List<DTO_CuentaXPagar> LoadCxPAnticipo()
        {
            try
            {
                this._listCxPs = new List<DTO_CuentaXPagar>();
                DTO_cpConceptoCXP concepto = new DTO_cpConceptoCXP();
                DTO_coDocumento docContable = new DTO_coDocumento();
                string conceptoCxP = this._bc.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ConceptoCxPAnticiposEmpl);
                DTO_noConceptoNOM conceptoNOM = (DTO_noConceptoNOM)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noConceptoNOM, false, this.masterConceptoNOM.Value, true);
                DTO_coPlanCuenta cuentaID = (DTO_coPlanCuenta)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, false, conceptoNOM.CuentaID.Value, true);
                if (cuentaID == null)
                {
                    MessageBox.Show("Cuenta de Concepto de Nómina vacía");
                    return null;
                }    
                if (string.IsNullOrEmpty(conceptoCxP))
                {
                    MessageBox.Show("Concepto CxP de Anticipos Empleados vacío");
                    return null;
                }
                else
                {
                    concepto = (DTO_cpConceptoCXP)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpConceptoCXP, false, conceptoCxP, true);
                    if (concepto == null)
                    {
                        MessageBox.Show("Concepto CxP de Anticipos Empleados " + conceptoCxP + " No existe");
                        return null;
                    }
                    docContable = (DTO_coDocumento)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, false, concepto.coDocumentoID.Value, true);
                    if (string.IsNullOrEmpty(docContable.CuentaLOC.Value))
                    {
                        MessageBox.Show("Cuenta del documento contable CxP vacía");
                        return null;
                    }
                }
                DTO_coPlanCuenta cuentaCxP = (DTO_coPlanCuenta)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, false, docContable.CuentaLOC.Value, true);
                     

                foreach (var novedad in this._data)
                {
                    DTO_CuentaXPagar CXP = new DTO_CuentaXPagar();
                    #region Doc Ctrl
                    CXP.DocControl = new DTO_glDocumentoControl();
                    CXP.DocControl.NumeroDoc.Value = 0;
                    CXP.DocControl.DocumentoID.Value = AppDocuments.CausarFacturas;
                    CXP.DocControl.DocumentoTipo.Value = (byte)DocumentoTipo.DocExterno;
                    CXP.DocControl.DocumentoNro.Value = 0;
                    CXP.DocControl.PeriodoDoc.Value = this.dtPeriod.DateTime;
                    CXP.DocControl.FechaDoc.Value = DateTime.Today.Month == this.dtPeriod.DateTime.Month? DateTime.Today : new DateTime(this.dtPeriod.DateTime.Year,this.dtPeriod.DateTime.Month,DateTime.DaysInMonth(this.dtPeriod.DateTime.Year,this.dtPeriod.DateTime.Month));
                    CXP.DocControl.PeriodoUltMov.Value = this.dtPeriod.DateTime;
                    CXP.DocControl.AreaFuncionalID.Value = this._bc.AdministrationModel.User.AreaFuncionalID.Value;
                    CXP.DocControl.PrefijoID.Value = this.defPrefijo;
                    CXP.DocControl.Observacion.Value = "CAUSACIÓN FACTURAS NOMINA ANTIC";
                    CXP.DocControl.Descripcion.Value = "Causacion Facturas";
                    CXP.DocControl.MonedaID.Value = this.monedaId;
                    CXP.DocControl.TasaCambioCONT.Value = 0;
                    CXP.DocControl.TasaCambioDOCU.Value = 0;
                    CXP.DocControl.ProyectoID.Value = this.defProyecto;
                    CXP.DocControl.CentroCostoID.Value = this.defCentroCosto;
                    CXP.DocControl.LineaPresupuestoID.Value = this.defLineaPresupuesto;
                    CXP.DocControl.LugarGeograficoID.Value = this.defLugarGeo;
                    CXP.DocControl.TerceroID.Value = novedad.EmpleadoID.Value; //PREGUNTAR SI ES ESE
                    CXP.DocControl.CuentaID.Value = docContable.CuentaLOC.Value;                  
                    CXP.DocControl.DocumentoTercero.Value = "Ant_" + this._consecutivoAnticipo; //PREGUNTAR SI ES ESE
                    CXP.DocControl.Estado.Value = (byte)EstadoDocControl.ParaAprobacion;
                    CXP.DocControl.seUsuarioID.Value = this._bc.AdministrationModel.User.ReplicaID.Value;
                    CXP.DocControl.Valor.Value = novedad.Valor.Value;
                    #endregion
                    #region CxP
                    CXP.CxP = new DTO_cpCuentaXPagar();
                    CXP.CxP.NumeroDoc.Value = CXP.DocControl.NumeroDoc.Value;
                    CXP.CxP.ConceptoCxPID.Value = conceptoCxP;
                    CXP.CxP.Dato1.Value = EstadoResumenImp.SinAprobar.ToString();
                    CXP.CxP.Dato8.Value = novedad.FijaInd.Value.ToString();
                    CXP.CxP.Dato9.Value = novedad.PeriodoPago.Value.ToString();
                    CXP.CxP.Dato10.Value = this.masterConceptoNOM.Value;                   
                    CXP.CxP.MonedaPago.Value = this.monedaId;
                    CXP.CxP.Valor.Value = novedad.Valor.Value;
                    CXP.CxP.IVA.Value = 0;
                    CXP.CxP.ValorLocal.Value = novedad.Valor.Value;
                    CXP.CxP.ValorExtra.Value = 0;
                    CXP.CxP.MonedaPago.Value = this.monedaId;
                    CXP.CxP.FacturaFecha.Value = CXP.DocControl.FechaDoc.Value;
                    CXP.CxP.VtoFecha.Value = CXP.DocControl.FechaDoc.Value;
                    CXP.CxP.DistribuyeImpLocalInd.Value = false;
                    #endregion
                    #region Header Comprobante
                    CXP.Comp.Header = new DTO_ComprobanteHeader();
                    CXP.Comp.Header.ComprobanteID.Value = docContable.ComprobanteID.Value;
                    CXP.Comp.Header.ComprobanteNro.Value = 0;
                    CXP.Comp.Header.EmpresaID.Value = this._bc.AdministrationModel.Empresa.ID.Value;
                    CXP.Comp.Header.Fecha.Value = this.dtPeriod.DateTime;
                    CXP.Comp.Header.NumeroDoc.Value = 0;
                    CXP.Comp.Header.MdaOrigen.Value = 1;
                    CXP.Comp.Header.MdaTransacc.Value = this.monedaId;
                    CXP.Comp.Header.PeriodoID.Value = this.dtPeriod.DateTime;
                    CXP.Comp.Header.TasaCambioBase.Value = 0;
                    CXP.Comp.Header.TasaCambioOtr.Value = 0;
                    CXP.Comp.Footer = new List<DTO_ComprobanteFooter>();                    
                    #endregion
                    #region Partida
                    DTO_ComprobanteFooter footer = new DTO_ComprobanteFooter();
                    footer.Index = 0;
                    footer.CuentaID.Value = conceptoNOM.CuentaID.Value;
                    footer.TerceroID.Value = novedad.EmpleadoID.Value;
                    footer.ProyectoID.Value = this.defProyecto;
                    footer.CentroCostoID.Value = this.defCentroCosto;
                    footer.LineaPresupuestoID.Value = this.defLineaPresupuesto;
                    footer.ConceptoCargoID.Value = !cuentaID.ConceptoCargoInd.Value.Value ? this.defConceptoCargo : cuentaID.ConceptoCargoID.Value;
                    footer.LugarGeograficoID.Value = this.defLugarGeo;
                    footer.PrefijoCOM.Value = this.defPrefijo;
                    footer.DocumentoCOM.Value = "Ant_" + this._consecutivoAnticipo;
                    footer.ConceptoSaldoID.Value = cuentaID.ConceptoSaldoID.Value;
                    footer.IdentificadorTR.Value = 0;
                    footer.Descriptivo.Value = "Anticipo Nomina";
                    footer.TasaCambio.Value = 0;//Validar moneda
                    footer.vlrBaseML.Value = 0;
                    footer.vlrBaseME.Value = 0;
                    footer.vlrMdaLoc.Value = novedad.Valor.Value;
                    footer.vlrMdaExt.Value = 0; //Validar moneda
                    footer.vlrMdaOtr.Value = footer.vlrMdaLoc.Value;
                    CXP.Comp.Footer.Add(footer);
                    #endregion
                    #region Crea contrapartida
                    DTO_ComprobanteFooter contrap = new DTO_ComprobanteFooter();
                    contrap.Index = 1;
                    contrap.CuentaID.Value = docContable.CuentaLOC.Value;
                    contrap.TerceroID.Value = novedad.EmpleadoID.Value;
                    contrap.ProyectoID.Value = this.defProyecto;
                    contrap.CentroCostoID.Value = this.defCentroCosto;
                    contrap.LineaPresupuestoID.Value = this.defLineaPresupuesto;
                    contrap.ConceptoCargoID.Value = !cuentaID.ConceptoCargoInd.Value.Value ? this.defConceptoCargo : cuentaID.ConceptoCargoID.Value;
                    contrap.LugarGeograficoID.Value = this.defLugarGeo;
                    contrap.PrefijoCOM.Value = this.defPrefijo;
                    contrap.DocumentoCOM.Value = footer.DocumentoCOM.Value;
                    contrap.ConceptoSaldoID.Value = cuentaCxP.ConceptoSaldoID.Value;
                    contrap.IdentificadorTR.Value = 0;
                    contrap.Descriptivo.Value = "Contrapartida Anticipo Nomina";
                    contrap.TasaCambio.Value = 0;//Validar moneda
                    contrap.vlrBaseML.Value = 0;
                    contrap.vlrBaseME.Value = 0;
                    contrap.vlrMdaLoc.Value = novedad.Valor.Value * -1;
                    contrap.vlrMdaExt.Value = 0; //Validar moneda
                    contrap.vlrMdaOtr.Value = footer.vlrMdaLoc.Value;
                    contrap.DatoAdd4.Value = AuxiliarDatoAdd4.Contrapartida.ToString();
                    CXP.Comp.Footer.Add(contrap);
                    #endregion 
                    //Incrementa el consecutivo de anticipos
                    this._consecutivoAnticipo = (Convert.ToInt32(this._consecutivoAnticipo) + 1).ToString();
                    this._listCxPs.Add(CXP);
                }
                return this._listCxPs;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MIgracionNomina.cs-", "LoadCxPAnticipo"));
                return null;
            }
        }

        #endregion

        #region Eventos Formulario

        /// <summary>
        /// Boton para limpiar el formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClean_Click(object sender, EventArgs e)
        {
            if (this._data.Count > 0)
            {
                string msgTitleSearch = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning));
                string msgNewSearch = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewData);

                if (MessageBox.Show(msgNewSearch, msgTitleSearch, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this._data = new List<DTO_noNovedadesNomina>();
                    this.btnProcesar.Enabled = false;
                    this.btnInconsistencias.Enabled = false;

                    this._isOK = false;
                    this.masterConceptoNOM.Value = string.Empty;
                    this.txtValor.EditValue = 0;

                    this.masterConceptoNOM.Enabled = true;
                    this.txtValor.Enabled = true;

                    this.results = null;
                    this.result = null;
                }
                this._consecutivoAnticipo = _bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_ConsecutivoAnticipo);
                this._consecutivoAnticipo = string.IsNullOrEmpty(this._consecutivoAnticipo) ? "1" : this._consecutivoAnticipo;
            }
        }

        /// <summary>
        /// Evento para generar laplantilla de importacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                ExcelGenerator excell_app = new ExcelGenerator();

                int row = 1;
                int col = 1;
                string[] cols = this.format.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries);
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MigracionNominaAnt.cs", "btnTemplate_Click"));
            }
        }

        /// <summary>
        /// Evento que importa los registros de la plantilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImportar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.masterConceptoNOM.ValidID)
                    MessageBox.Show("No se ha digitado un Concepto de Nómina");
                else
                {
                    this.Enabled = false;

                    this.pasteRet = CopyPasteExtension.PasteFromClipBoard(this.format);
                    this.btnImportar.Enabled = false;
                    this.btnTemplate.Enabled = false;
                    this.btnProcesar.Enabled = false;
                    this.btnInconsistencias.Enabled = false;
                    this.masterConceptoNOM.Enabled = false;
                    this.txtValor.Enabled = false;

                    this.results = null;
                    this.result = null;

                    DTO_noConceptoNOM conceptoNOM = (DTO_noConceptoNOM)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noConceptoNOM, false, this.masterConceptoNOM.Value, true);
                    this._saveCxPInd = conceptoNOM.Ind_39.Value.Value; //Indica si guarda o no CxP o solo novedades

                    this._consecutivoAnticipo = _bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_ConsecutivoAnticipo);
                    this._consecutivoAnticipo = string.IsNullOrEmpty(this._consecutivoAnticipo) ? "1" : this._consecutivoAnticipo;

                    Thread process = new Thread(this.ImportThread);
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MIgracionNomina.cs-", "btnImportar_Click"));
            }
        }

        /// <summary>
        /// Evento que se encarga de verificar las inconsistencias
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnProcesar_Click(object sender, EventArgs e)
        {
            bool loadData = true;
            //if (this._data.Count > 0)
            //{
            //    string msgTitleSearch = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning));
            //    string msgNewSearch = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewData);

            //    if (MessageBox.Show(msgNewSearch, msgTitleSearch, MessageBoxButtons.YesNo) == DialogResult.No)
            //        loadData = false;
            //}

            try
            {
                if (loadData)
                {
                    this.Enabled = false;

                    this.btnImportar.Enabled = false;
                    this.btnTemplate.Enabled = false;
                    this.btnProcesar.Enabled = false;
                    this.btnInconsistencias.Enabled = false;

                    this.results = null;
                    this.result = null;

                    if (this._saveCxPInd)
                        this.LoadCxPAnticipo();

                    Thread process = new Thread(this.ProcesarThread);
                    process.Start();
                }
            }
            catch (Exception ex)
            {                
              this.result.ResultMessage = _bc.GetResourceForException(ex, "WinApp-MigracionNominaAnt.cs", "btnProcesar_Click");
            }
        }

        /// <summary>
        /// Evento que muestra las inconsistencias
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInconsistencias_Click(object sender, EventArgs e)
        {
            this.Enabled = false;

            Thread process = new Thread(this.InconsistenciasThread);
            process.Start();
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo que se ejecuta cualdo el usuario va a imprtar datos al detalle
        /// </summary>
        public void ImportThread()
        {
            try
            {
                if (this.pasteRet.Success)
                {
                    this.codigos = new List<string>();
                    var text = pasteRet.MsgResult;
                    string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    #region Variables de función y mensajes de error
                    this.result = new DTO_TxResult();
                    this.result.Result = ResultValue.OK;
                    this.result.Details = new List<DTO_TxResultDetail>();
                    //Lista con los dtos a subir y Fks a validas
                    this._data = new List<DTO_noNovedadesNomina>();
                    Dictionary<string, List<Tuple<string, bool>>> fks = new Dictionary<string, List<Tuple<string, bool>>>();
                    List<string> fkNames = new List<string>();
                    Dictionary<string, string> colNames = new Dictionary<string, string>();
                    Dictionary<string, Object> colVals = new Dictionary<string, Object>();
                    //Mensajes de error
                    string msgEmptyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField);
                    string msgInvalidFormat = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidFormat);
                    string msgNoCopyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoCopyField);
                    string msgIncompleteLine = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.IncompleteLine);
                    string msgFecha = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_InvalidFechaAplica);
                    string msgNoRel = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ImpNoRel);
                    string msgPositive = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue);
                    string msgClinteRepetido = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ClienteSinLibranza);
                    string msgCodAndCliente = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ClienteYCodigo);
                    string msgFkNotFound = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FkNotFound);
                    bool liquidaQuincenal = this._bc.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_LiquidaNominaQuincenal).Equals("0") ? false : true;
                    //Popiedades de la incorporacion
                    DTO_noNovedadesNomina mig = new DTO_noNovedadesNomina();
                    bool createDTO = true;
                    bool validList = true;
                    #endregion
                    #region Llena las listas de las columnas
                    List<string> cols = this.format.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    PropertyInfo[] pis = typeof(DTO_noNovedadesNomina).GetProperties();

                    //Recorre el objeto y revisa el nombre real de la columna
                    foreach (PropertyInfo pi in pis)
                    {
                        if (!Attribute.IsDefined(pi, typeof(NotImportable)))
                        {
                            string colRsx = _bc.GetResource(LanguageTypes.Forms, AppProcess.MigracionNominaAnt.ToString() + "_" + pi.Name);
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
                    #region Llena información para enviar a la grilla (lee filas)
                    int percent = 0;
                    for (int i = 0; i < lines.Length; ++i)
                    {
                        #region Aumenta el porcentaje y revisa que tenga lineas para leer
                        this.Invoke(this.UpdateProgressDelegate, new object[] { percent });
                        percent = ((i + 1) * 100) / (lines.Length);

                        if (FormProvider.Master.ProcessCanceled(this.documentID))
                        {
                            this.result.Details = new List<DTO_TxResultDetail>();
                            this.result.ResultMessage = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser);
                            this.result.Result = ResultValue.NOK;
                            break;
                        }

                        if (lines.Length == 1)
                        {
                            this.result.ResultMessage = msgNoCopyField;
                            this.result.Result = ResultValue.NOK;
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
                            //Llena los valores de las columnas (se revienta si el numero de columnas al importar es menor al necesario)
                            if (line.Length < colNames.Count)
                            {
                                this.result.Result = ResultValue.NOK;
                                DTO_TxResultDetail rdL = new DTO_TxResultDetail();
                                rdL.line = i;
                                rdL.Message = msgIncompleteLine;
                                this.result.Details.Add(rdL);

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

                                    #region Revisa la info de las FKs
                                    if (!string.IsNullOrWhiteSpace(line[colIndex]) && colRsx == "EmpleadoID")
                                    {
                                        colVals[colRsx] = line[colIndex].ToUpper();

                                        Tuple<string, bool> tupValid = new Tuple<string, bool>(line[colIndex].Trim(), true);
                                        Tuple<string, bool> tupInvalid = new Tuple<string, bool>(line[colIndex].Trim(), false);

                                        if (fks[colRsx].Contains(tupValid))
                                            continue;
                                        else if (fks[colRsx].Contains(tupInvalid))
                                        {
                                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = colRsx;
                                            rdF.Message = string.Format(msgFkNotFound, line[colIndex].Trim());
                                            rd.DetailsFields.Add(rdF);

                                            createDTO = false;
                                        }
                                        else
                                        {
                                            int docId = this.GetMasterDocumentID(colRsx);
                                            object dto = _bc.GetMasterDTO(AppMasters.MasterType.Simple, docId, false, line[colIndex], true);

                                            if (dto == null)
                                            {
                                                fks[colRsx].Add(new Tuple<string, bool>(line[colIndex].Trim(), false));

                                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                rdF.Field = colRsx;
                                                rdF.Message = string.Format(msgFkNotFound, line[colIndex]);
                                                rd.DetailsFields.Add(rdF);

                                                createDTO = false;
                                            }
                                            else
                                                fks[colRsx].Add(new Tuple<string, bool>(line[colIndex].Trim(), true));
                                        }
                                    }
                                }
                                #endregion
                            }

                            #endregion
                            #region Creacion de DTO y validacion Formatos
                            if (createDTO)
                            {
                                mig = new DTO_noNovedadesNomina();
                                for (int colIndex = 0; colIndex < colNames.Count(); colIndex++)
                                {
                                    string colRsx = cols[colIndex];
                                    try
                                    {
                                        string colName = colNames[colRsx];
                                        string colValue = colVals[colRsx].ToString().Trim();

                                        #region Validacion de Nulls (Campos basicos)
                                        if (string.IsNullOrEmpty(colValue) && (colName == "FechaNomina" || colName == "ValorCuota"))
                                        {
                                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = colRsx;
                                            rdF.Message = msgEmptyField;
                                            rd.DetailsFields.Add(rdF);

                                            createDTO = false;
                                        }

                                        #endregion
                                        #region Validacion Formatos

                                        PropertyInfo pi = mig.GetType().GetProperty(colName);
                                        UDT udt = (UDT)pi.GetValue(mig, null);
                                        PropertyInfo piUDT = udt.GetType().GetProperty("Value");

                                        //Comprueba los valores solo para los booleanos
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

                                        } //validacion si no es null
                                        #endregion

                                        //Si paso las validaciones asigne el valor al DTO
                                        udt.ColRsx = colRsx;
                                        if (createDTO && !string.IsNullOrWhiteSpace(colValue))
                                            udt.SetValueFromString(colValue);
                                    }
                                    catch (Exception ex1)
                                    {
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = colRsx;
                                        rdF.Message = _bc.GetResourceForException(ex1, "WinApp", "MigracionNominaAnt.cs - Creacion de DTO y validacion Formatos");
                                        rd.DetailsFields.Add(rdF);

                                        createDTO = false;
                                    }
                                }
                            }
                            #endregion
                            #region Carga la informacion de los resultados
                            if (rd.DetailsFields.Count > 0)
                            {
                                rd.Message = "NOK";
                                this.result.Details.Add(rd);
                                this.result.Result = ResultValue.NOK;
                            }

                            if (createDTO && validList)
                            {
                                DTO_noNovedadesNomina exist = this._data.Find(x => x.EmpleadoID.Value == mig.EmpleadoID.Value &&
                                                        x.ConceptoNOID.Value == mig.ConceptoNOID.Value &&
                                                        x.PeriodoPago.Value == mig.PeriodoPago.Value);

                                if (exist != null)
                                    exist.Valor.Value += mig.Valor.Value;
                                else
                                    this._data.Add(mig);
                            }
                              
                            else
                                validList = false;
                            #endregion
                        }
                        #endregion
                    }
                    this.Invoke(this.UpdateProgressDelegate, new object[] { percent });
                    #endregion
                    #region Valida las restricciones particulares de la migracion de nomina
                    if (validList)
                    {
                        #region Variables generales
                        this.result = new DTO_TxResult();
                        this.result.Result = ResultValue.OK;
                        this.result.Details = new List<DTO_TxResultDetail>();

                        int i = 0;
                        percent = 0;

                        #endregion
                        foreach (DTO_noNovedadesNomina dto in this._data)
                        {
                            dto.EmpresaID.Value = this._bc.AdministrationModel.Empresa.ID.Value;
                            dto.ActivaInd.Value = true;
                            dto.FijaInd.Value = dto.FijaInd.Value?? false;
                            dto.PeriodoPago.Value = !liquidaQuincenal ? 2 : dto.PeriodoPago.Value;
                            #region Aumenta el porcentaje y revisa que tenga lineas para leer
                            this.Invoke(this.UpdateProgressDelegate, new object[] { percent });
                            percent = ((i + 1) * 100) / (this._data.Count);
                            i++;

                            if (FormProvider.Master.ProcessCanceled(this.documentID))
                            {
                                this.result.Details = new List<DTO_TxResultDetail>();
                                this.result.ResultMessage = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser);
                                this.result.Result = ResultValue.NOK;
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
                            this.ValidateDataImport(dto, rd, msgFecha, msgNoRel, msgPositive, msgEmptyField);
                            #endregion
                            #region Carga la informacion de los resultados
                            if (rd.DetailsFields.Count > 0)
                            {
                                rd.Message = "NOK";
                                this.result.Details.Add(rd);
                                this.result.Result = ResultValue.NOK;

                                validList = false;
                            }
                            #endregion
                        }

                        this.Invoke(this.UpdateProgressDelegate, new object[] { percent });
                    }
                    #endregion
                    #region Valida los valores

                    if (validList)
                    {
                        //decimal valNomina = Convert.ToDecimal(this.txtValor.EditValue, CultureInfo.InvariantCulture);
                        //decimal valPagos = this.data.Sum(x => x.Valor.Value.Value);
                        //this.valorPagaduria = valNomina - valPagos;
                        //if (valPagos != valNomina)
                        //{
                        //    string valNominaStr = valNomina.ToString("c", CultureInfo.CreateSpecificCulture("en-US"));
                        //    string valPagosStr = valPagos.ToString("c", CultureInfo.CreateSpecificCulture("en-US"));
                        //    string valDifStr = this.valorPagaduria.ToString("c", CultureInfo.CreateSpecificCulture("en-US"));

                        //    string msgTitleDif = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning));
                        //    string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_NominaDifVal);

                        //    string msgDif = string.Format(msg, valNominaStr, valPagosStr, valDifStr);
                        //    if (MessageBox.Show(msgDif, msgTitleDif, MessageBoxButtons.YesNo) == DialogResult.No)
                        //        validList = false;
                        //}
                    }

                    #endregion

                    if (validList  && this.result.Result == ResultValue.OK)
                        this._isOK = true;
                    else
                    {
                        this._isOK = false;
                        this._data = new List<DTO_noNovedadesNomina>();
                    }               
                }
            }
            catch (Exception ex)
            {
                this._data = new List<DTO_noNovedadesNomina>();
                this._isOK = false;
                this.result.Result = ResultValue.NOK;
                this.result.ResultMessage = _bc.GetResourceForException(ex, "WinApp-MigracionNominaAnt.cs", "ImportThread");
            }
            finally
            {
                this.Invoke(this.endImportarDelegate);
                if (!this.pasteRet.Success)
                {
                    this._isOK = false;
                    MessageForm frm = new MessageForm(pasteRet.MsgResult, MessageType.Error);
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                }
            }
        }

        /// <summary>
        /// Hilo de Procesar las inconsistencias
        /// </summary>
        private void ProcesarThread()
        {
            try
            {
                this.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCartera(this.documentID));
                this.ProgressBarThread = new Thread(this.CheckServerProcessStatus);
                this.ProgressBarThread.Start();

                if (this._saveCxPInd)
                {                   
                    this.result = _bc.AdministrationModel.CuentasXPagar_CausarMasivo(this.documentID,this._listCxPs);
                    if (this.result.Result == ResultValue.NOK)
                        return;
                    if (this.result != null && this.result.Result == ResultValue.OK)
                    {
                        string actCausacion = this._bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(AppDocuments.CausarFacturas).First();
                        //Manda a aprobacion
                        foreach (var cxp in this._listCxPs)
                        {
                            object obj = _bc.AdministrationModel.ComprobantePre_SendToAprob(AppDocuments.CausarFacturas, actCausacion, ModulesPrefix.cp, cxp.DocControl.PeriodoDoc.Value.Value, cxp.DocControl.ComprobanteID.Value, cxp.DocControl.ComprobanteIDNro.Value.Value, false);
                            if (obj.GetType() == typeof(DTO_Alarma))
                            {
                                string numDoc = ((DTO_Alarma)obj).NumeroDoc;
                                bool finaliza = ((DTO_Alarma)obj).Finaliza;
                                //reportName = this._bc.AdministrationModel.Reportes_Cp_CausacionFacturas(Convert.ToInt32(numDoc), finaliza, ExportFormatType.pdf);

                                //if (reportName == string.Empty)
                                //    MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_NoSeGeneroReporte));
                            }
                        }


                        // Actualiza el  glControl
                        string empNro = this._bc.AdministrationModel.Empresa.NumeroControl.Value;
                        string _modId = ((int)ModulesPrefix.cp).ToString();
                        if (_modId.Length == 1) _modId = "0" + _modId;
                        DTO_glControl ctrlConsec = new DTO_glControl();
                        ctrlConsec.glControlID.Value = Convert.ToInt32((empNro + _modId + AppControl.cp_ConsecutivoAnticipo));
                        ctrlConsec.Descriptivo.Value = "Consecutivo Anticipos";
                        ctrlConsec.Data.Value = this._consecutivoAnticipo.ToString();
                        this._bc.AdministrationModel.glControl_Update(ctrlConsec);
                        this._bc.AdministrationModel.ControlList = _bc.AdministrationModel.glControl_GetByNumeroEmpresa(false, empNro).ToList(); 
                    }
                }
                else
                {
                    this._data.ForEach(x => x.OrigenNovedad.Value = 2);
                    this.result = _bc.AdministrationModel.Nomina_AddNovedadNomina(this._data);
                }
                this.StopProgressBarThread();
            }
            catch (Exception ex)
            {
                this._data = new List<DTO_noNovedadesNomina>(); 
                this._isOK = false;
                this.result.Result = ResultValue.NOK;
                this.result.ResultMessage = _bc.GetResourceForException(ex, "WinApp-MigracionNominaAnt.cs", "ProcesarThread");
            }
            finally
            {
                this.Invoke(this.endProcesarDelegate);
            }
        }

        /// <summary>
        /// Carga el reporte con las inconsistencias
        /// </summary>
        private void InconsistenciasThread()
        {
            try
            {
                if (this.result != null)
                {
                    _bc.AssignResultResources(null, this.result);

                    reportName = this._bc.AdministrationModel.Rep_TxResult(this.result);
                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                    Process.Start(fileURl);
                }
                else if(this.results != null)
                {

                    _bc.AssignResultResources(null, this.results);

                    reportName = this._bc.AdministrationModel.Rep_TxResultDetails(this.results);
                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                    Process.Start(fileURl);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                this.Invoke(this.endInconsistenciasDelegate);
            }
        }

        #endregion

    }
}
