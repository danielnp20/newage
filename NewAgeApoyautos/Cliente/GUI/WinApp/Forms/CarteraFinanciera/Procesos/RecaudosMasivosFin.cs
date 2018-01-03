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
using System.IO;
using SentenceTransformer;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class RecaudosMasivosFin : ProcessForm
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
            this.btnImportar.Enabled = true;

            if (this._isOK)
            {
                this.txtValorIni.EditValue = this.data.Select(x => x.ValorCuota.Value.Value).Sum(); //Valor Total inicial
                this.txtValorInconsistencia.EditValue = this.data.FindAll(x => x.IndInconsistencia.Value.Value).Sum(x => x.ValorCuota.Value.Value); //Valor de Inconsistencias
                //Retira los que tengan inconsistencias
                this.data = this.data.FindAll(x => !x.IndInconsistencia.Value.Value);

                //Agrupa los registros con cliente y libranza duplicados
                List<DTO_ccIncorporacionDeta> tmp = new List<DTO_ccIncorporacionDeta>();
                List<string> clientes = this.data.Select(x => x.ClienteID.Value).Distinct().ToList();
                foreach (string cli in clientes)
                {
                    List<DTO_ccIncorporacionDeta> libranzasxCli = this.data.FindAll(x => x.ClienteID.Value == cli).ToList();
                    List<int?> libranzasDist = libranzasxCli.Select(x => x.Libranza.Value).Distinct().ToList();
                    foreach (int? lib in libranzasDist)
                    {
                        DTO_ccIncorporacionDeta d = this.data.Find(x => x.ClienteID.Value == cli && x.Libranza.Value == lib);
                        d.ValorCuota.Value = this.data.FindAll(x => x.ClienteID.Value == cli && x.Libranza.Value == lib).Sum(y => y.ValorCuota.Value);
                        d.ValorNomina.Value = this.data.FindAll(x => x.ClienteID.Value == cli && x.Libranza.Value == lib).Sum(y => y.ValorNomina.Value);
                        tmp.Add(d);
                    }                       
                }
                this.data = tmp;

                this.txtValorNeto.EditValue = this.data.Select(x => x.ValorCuota.Value.Value).Sum();
                this.btnInconsistencias.Enabled = this.result.Details.Count > 0 ? true : false;
                this._revisaInconsistenciaInd = !this.btnInconsistencias.Enabled;
                this.btnPremilinar.Enabled = true;
            }
            else
            {
                this.btnInconsistencias.Enabled = this.result.Details.Count > 0 ? true : false;
                this._revisaInconsistenciaInd = !this.btnInconsistencias.Enabled;

                this.txtValorNeto.EditValue = 0;
                this.txtValorIni.EditValue = 0;
                this.txtValorInconsistencia.EditValue = 0;
                this.masterBanco.EnableControl(true);
                this.dtFecha.Enabled = true;
            }

            MessageForm frm = new MessageForm(this.result);
            this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });
        }

        /// <summary>
        /// Delegado que finaliza el proceso de validaciones del servidor
        /// </summary>
        public delegate void EndPreliminar();
        public EndPreliminar endPreliminarDelegate;
        public void EndPreliminarMethod()
        {
            this.Enabled = true;

            this.btnImportar.Enabled = true;
            this.btnPremilinar.Enabled = true;

            if (this.result.Result == ResultValue.NOK)
            {
                this.btnInconsistencias.Enabled = true;
                this.validarInconsistencias = true;
                this._isOK = false;
            }
            else if (this.result.Details.Count == 0)
            {
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
                this.btnRelPagos.Enabled = true;
                this.btnReporte.Enabled = true;
            }
        }

        /// <summary>
        /// Delegado que finaliza el proceso de relación de pagos por componentes 
        /// </summary>
        public delegate void ShowRelPagos();
        public ShowRelPagos showRelPagosDelegate;
        public void ShowRelPagosMethod()
        {
            if (this.tableRelacionPagos != null)
            {
                ReportExcelBase frm = new ReportExcelBase(this.tableRelacionPagos);
                frm.Show();
            }

            this.StopProgressBarThread();
            this.Enabled = true;

            this.btnImportar.Enabled = true;
            this.btnPremilinar.Enabled = true;
            this.btnRelPagos.Enabled = true;
            this.btnReporte.Enabled = true; 
            this.btnPagar.Enabled = true;
        }

        /// <summary>
        /// Delegado que finaliza el proceso de pago de creditos
        /// </summary>
        public delegate void EndPagar();
        public EndPagar endPagarDelegate;
        public void EndPagarMethod()
        {
            this.Enabled = true;

            this.btnImportar.Enabled = true;
            if (this._isOK)
            {
                MessageForm frm = new MessageForm(this.result);
                this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });

                this.data = new List<DTO_ccIncorporacionDeta>();
                this.btnPremilinar.Enabled = false;
                this.btnRelPagos.Enabled = false;
                this.btnReporte.Enabled = false; 
                this.btnPagar.Enabled = false;
                this.btnInconsistencias.Enabled = false;

                this._isOK = false;
                this.centroPagoID = string.Empty;
                this.masterBanco.Value = string.Empty;
                this.dtFecha.DateTime = this.dtPeriod.DateTime;
                this.lkp_periodo.EditValue = "1";
                this.txtValorNeto.EditValue = 0;
                this.txtValorIni.EditValue = 0;
                this.txtValorInconsistencia.EditValue = 0;

                this.masterBanco.EnableControl(true);
                this.dtFecha.Enabled = true;

                this.results = null;
                this.result = null;
            }
            else
            {
                MessageForm frm = new MessageForm(this.result);
                this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });

                this.btnRelPagos.Enabled = true;
                this.btnReporte.Enabled = true; 
                this.btnPremilinar.Enabled = true;
                this.btnPagar.Enabled = true;
                this.btnInconsistencias.Enabled = true;
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
        //Variables del formulario
        private bool _isOK;
        private List<DTO_ccIncorporacionDeta> data;
        private string centroPagoID = string.Empty;
        private string bancoID = string.Empty;
        private DTO_tsBancosCuenta bancosCta;
        private DateTime periodo = DateTime.Now;
        private bool validarInconsistencias;
        private decimal valorPagaduria;
        //Variables para la importacion
        DTO_TxResult result;
        List<DTO_TxResult> results;
        DataTableOperations tableOp;
        DataTable tableImportacion = new DataTable();
        List<DTO_glDocMigracionCampo> columnasImportacion = new List<DTO_glDocMigracionCampo>();
        List<string> codigos = new List<string>();
        Dictionary<string, string> clientes = new Dictionary<string, string>();
        DataTable tableRelacionPagos;
        //Variables con los recursos de las Fks
        private string _clienteRsx = "ClienteID";
        private string reportName;
        private string fileURl;
        private bool _revisaInconsistenciaInd = true;
        #endregion

        public RecaudosMasivosFin()
        {
           // InitializeComponent();
        }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            try
            {
                this.documentID = AppDocuments.RecaudosMasivos;
                this.InitializeComponent();

                //Inicializa los delegados
                this.endImportarDelegate = new EndImportar(this.EndImportarMethod);
                this.endPreliminarDelegate = new EndPreliminar(this.EndPreliminarMethod);
                this.showRelPagosDelegate = new ShowRelPagos(this.ShowRelPagosMethod);
                this.endPagarDelegate = new EndPagar(this.EndPagarMethod);
                this.endInconsistenciasDelegate = new EndInconsistencias(this.EndInconsistenciasMethod);

                //Carga la configuracion inicial
                this._isOK = false;

                this.data = new List<DTO_ccIncorporacionDeta>();
                this.btnImportar.Enabled = false;
                this.btnPremilinar.Enabled = false;
                this.btnRelPagos.Enabled = false;
                this.btnReporte.Enabled = false;
                this.btnPagar.Enabled = false;
                this.btnInconsistencias.Enabled = false;

                //Carga la info inicial de los controles 
                _bc.InitMasterUC(this.masterBanco, AppMasters.tsBancosCuenta, true, true, true, false);

                //Centro de pago y pagaduria
                this.centroPagoID = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_CentroPagoPorDefecto);

                //Periodo
                _bc.InitPeriodUC(this.dtPeriod, 0);
                string periodoStr = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo);
                this.periodo = Convert.ToDateTime(periodoStr);
                this.dtPeriod.DateTime = this.periodo;
                this.dtFecha.DateTime = DateTime.Now.Date.AddDays(-1);

                //Fecha de cierre
                string diaCierreStr = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_DiaUltimoCierre);
                int diaCierre = Convert.ToInt32(diaCierreStr);
                if (diaCierre != 0)
                    try {this.dtFechaCierre.DateTime = new DateTime(this.periodo.Year, this.periodo.Month, diaCierre); }catch (Exception){;}
                else
                    this.dtFechaCierre.DateTime = new DateTime(this.periodo.Year, this.periodo.Month, 1);

                //Fecha del pago
                this.dtFecha.Properties.MinValue = this.dtFechaCierre.DateTime;
                DateTime fechaFin = new DateTime(this.periodo.Year, this.periodo.Month, DateTime.DaysInMonth(this.periodo.Year, this.periodo.Month));
                this.dtFecha.Properties.MaxValue = fechaFin;
                if (this.periodo.Month != this.dtFecha.DateTime.Month || this.periodo.Year != this.dtFecha.DateTime.Year)
                    this.dtFecha.DateTime = fechaFin;

                //Info del combo de seleccion
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("1", this._bc.GetResource(LanguageTypes.Tables, AppProcess.RecaudosMasivos.ToString() + "_tbl_Mensual"));
                dic.Add("2", this._bc.GetResource(LanguageTypes.Tables, AppProcess.RecaudosMasivos.ToString() + "_tbl_Quincenal"));
                this.lkp_periodo.Properties.DataSource = dic;
                this.lkp_periodo.EditValue = "1";
                this.lkp_periodo.Enabled = false;

                //Variable para valida inconsistencias
                string validar = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_AnalisisInconsistenciaNomina);
                if (validar == "0")
                    this.validarInconsistencias = false;
                else
                    this.validarInconsistencias = true;

                //Carga los recuros
                FormProvider.LoadResources(this, AppProcess.RecaudosMasivosFin);
            }
            catch (Exception ex)
            {                
               MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RecaudosMasivosFin.cs", "InitForm"));
            }
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
            if (colName == this._clienteRsx)
                return AppMasters.ccCliente;

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
        private void ValidateDataImport(DTO_ccIncorporacionDeta dto, DTO_TxResultDetail rd, string msgFecha, string msgNoRel, string msgPositive, string msgEmptyField,
            string msgClienteRepetido, string msgCodAndCliente)
        {
            try
            {
                #region Valida que la fecha sea la misma del formulario
                if (dto.FechaNomina.Value.Value != this.dtFecha.DateTime.Date)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppProcess.RecaudosMasivos + "_FechaNomina"); ;
                    rdF.Message = msgFecha;
                    rd.DetailsFields.Add(rdF);
                }
                #endregion
                #region Validacion cliente / codigo / libranza
                if (string.IsNullOrWhiteSpace(dto.ClienteID.Value) && string.IsNullOrWhiteSpace(dto.CodEmpleado.Value) && (dto.Libranza.Value == null))
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppProcess.RecaudosMasivos + "_ClienteID") +
                            _bc.GetResource(LanguageTypes.Forms, AppProcess.RecaudosMasivos + "_CodEmpleado") +
                            _bc.GetResource(LanguageTypes.Forms, AppProcess.RecaudosMasivos + "_Libranza");

                    rdF.Message = msgNoRel;
                    rd.DetailsFields.Add(rdF);
                }
                #endregion
                #region Validacion de Valores

                if (dto.ValorCuota.Value.Value <= 0)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppProcess.RecaudosMasivos + "_ValorCuota");
                    rdF.Message = msgPositive;
                    rd.DetailsFields.Add(rdF);
                }

                #endregion
                #region Validacion de exclusion entre cliente y codigo de empleado
                if (!string.IsNullOrWhiteSpace(dto.CodEmpleado.Value) && !string.IsNullOrWhiteSpace(dto.ClienteID.Value))
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppProcess.RecaudosMasivos + "_ClienteID") + _bc.GetResource(LanguageTypes.Forms, AppProcess.RecaudosMasivos + "_CodEmpleado");
                    rdF.Message = msgCodAndCliente;
                    rd.DetailsFields.Add(rdF);
                }
                #endregion
                #region Validacion para que codigo de empleado que se repita debe tener la libranza

                if (!string.IsNullOrWhiteSpace(dto.CodEmpleado.Value))
                {
                    if (this.codigos.Contains(dto.CodEmpleado.Value))
                    {
                        if (string.IsNullOrWhiteSpace(this.clientes[dto.CodEmpleado.Value]) || dto.Libranza.Value == null)
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppProcess.RecaudosMasivos + "_CodEmpleado");
                            rdF.Message = msgClienteRepetido;
                            rd.DetailsFields.Add(rdF);
                        }
                    }
                    else
                    {
                        string val = dto.Libranza.Value == null ? string.Empty : dto.Libranza.Value.Value.ToString();
                        this.codigos.Add(dto.CodEmpleado.Value);
                        this.clientes.Add(dto.CodEmpleado.Value, val);
                    }
                }

                #endregion
                #region Validacion para que cliente que se repita debe tener la libranza (En comentarios)

                //if (!string.IsNullOrWhiteSpace(dto.ClienteID.Value))
                //{
                //    if (clientes.ContainsKey(dto.ClienteID.Value))
                //    {
                //        if (string.IsNullOrWhiteSpace(clientes[dto.ClienteID.Value]) || dto.Libranza.Value == null)
                //        {
                //            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                //            rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppProcess.RecaudosMasivos + "_ClienteID");
                //            rdF.Message = msgClienteRepetido;
                //            rd.DetailsFields.Add(rdF);
                //        }
                //    }
                //    else
                //    {
                //        string val = dto.Libranza.Value == null ? string.Empty : dto.Libranza.Value.Value.ToString();
                //        clientes.Add(dto.ClienteID.Value, val);
                //    }
                //}

                #endregion

                dto.FechaNomina.Value = dto.FechaNomina.Value.Value.Date;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RecaudosMasivosFin.cs", "ValidateDataImport"));
            }
        }

        #endregion

        #region Eventos Formulario

        /// <summary>
        /// Evento que se ejecuta al salir del centro de pago
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterBanco_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.bancoID != this.masterBanco.Value)
                {
                    this.bancoID = this.masterBanco.Value;

                    //Carga los controles
                    this.data = new List<DTO_ccIncorporacionDeta>();
                    this.btnImportar.Enabled = false;
                    this.btnRelPagos.Enabled = false;
                    this.btnReporte.Enabled = false; 
                    this.btnPremilinar.Enabled = false;
                    this.btnPagar.Enabled = false;
                    this.btnInconsistencias.Enabled = false;

                    if (this.masterBanco.ValidID)
                    {
                        this.bancosCta = (DTO_tsBancosCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.tsBancosCuenta, false, this.masterBanco.Value, true);
                        if (string.IsNullOrWhiteSpace(bancosCta.CodMigraRecaudos.Value))
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ts_BancoSinEstructuras));
                        }
                        else
                        {
                            #region CARGA DE ESTRUCTURAS

                            string codDoc = bancosCta.CodMigraRecaudos.Value;
                            DTO_glDocMigracionEstructura estructura = (DTO_glDocMigracionEstructura)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocMigracionEstructura, false, codDoc, true);
                            if (estructura != null)
                            {
                                DTO_glConsulta query = new DTO_glConsulta();
                                List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();

                                filtros.Add(new DTO_glConsultaFiltro()
                                {
                                    CampoFisico = "CodigoDoc",
                                    OperadorFiltro = OperadorFiltro.Igual,
                                    OperadorSentencia = OperadorSentencia.And,
                                    ValorFiltro = codDoc
                                });
                                query.Filtros = filtros;

                                long count = _bc.AdministrationModel.MasterComplex_Count(AppMasters.glDocMigracionCampo, query, true);
                                if (count == 0)
                                {
                                    this.bancoID = string.Empty;
                                    this.bancosCta = null;
                                    this.masterBanco.Focus();

                                    string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoMigracionDoc);
                                    MessageBox.Show(string.Format(msg, codDoc));
                                }
                                else
                                {
                                    List<DTO_MasterComplex> fieldsObj = _bc.AdministrationModel.MasterComplex_GetPaged(AppMasters.glDocMigracionCampo, count, 1, query, true).ToList();

                                    this.columnasImportacion = fieldsObj.Cast<DTO_glDocMigracionCampo>().ToList();
                                    List<object> objList = fieldsObj.Cast<object>().ToList();
                                    this.tableOp = new DataTableOperations(estructura, objList);
                                }
                            }
                            else
                            {
                                this.bancoID = string.Empty;
                                this.bancosCta = null;
                                this.masterBanco.Focus();

                                string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoMigracionDoc);
                                MessageBox.Show(string.Format(msg, codDoc));
                            }

                            #endregion

                            this.btnImportar.Enabled = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.bancoID = string.Empty;
                this.bancosCta = null;
                this.masterBanco.Focus();

                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RecaudosMasivosFin.cs", "masterBanco_Leave"));
            }
        }

        /// <summary>
        /// Boton para limpiar el formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClean_Click(object sender, EventArgs e)
        {
            string msgTitleSearch = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning));
            string msgNewSearch = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewData);

            if (MessageBox.Show(msgNewSearch, msgTitleSearch, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.data = new List<DTO_ccIncorporacionDeta>();
                this.btnPremilinar.Enabled = false;
                this.btnRelPagos.Enabled = false;
                this.btnReporte.Enabled = false; 
                this.btnPagar.Enabled = false;
                this.btnInconsistencias.Enabled = false;

                this._isOK = false;
                this.bancoID = string.Empty;
                this.bancosCta = null;
                this.masterBanco.Value = string.Empty;
                this.dtFecha.DateTime = this.dtPeriod.DateTime;
                this.txtValorNeto.EditValue = 0;
                this.txtValorIni.EditValue = 0;
                this.txtValorInconsistencia.EditValue = 0;
                this.dtFecha.Enabled = true;
                this.results = null;
                this.result = null;
            }
            this.masterBanco.EnableControl(true);
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
                bool loadData = true;
                if (this.data.Count > 0)
                {
                    string msgTitleSearch = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning));
                    string msgNewSearch = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewData);

                    if (MessageBox.Show(msgNewSearch, msgTitleSearch, MessageBoxButtons.YesNo) == DialogResult.No)
                        loadData = false;                
                }

                if (loadData)
                {
                    bool hasData = false;
                    this.codigos = new List<string>();
                    this.clientes = new Dictionary<string, string>();
                    this.tableOp.Import_FromTextFile();
                    this.tableImportacion = this.tableOp.Table_Details.Copy();

                    if (this.tableImportacion != null)
                    {
                        DataRow[] filasValidas = this.tableImportacion.Select("ValidRow = 1");
                        if (filasValidas.Count() > 0)
                            hasData = true;
                    }

                    if (hasData)
                    {
                        #region Valida los datos importados

                        //Limpia la tabla
                        List<string> removableCols = new List<string>();
                        foreach (DataColumn col in this.tableImportacion.Columns)
                        {
                            if (col.ColumnName != "ValidRow"
                                && col.ColumnName != "FechaNomina"
                                && col.ColumnName != "ClienteID"
                                && col.ColumnName != "CodEmpleado"
                                && col.ColumnName != "Libranza"
                                && col.ColumnName != "ValorCuota")
                            {
                                removableCols.Add(col.ColumnName);
                            }
                        }

                        foreach (string col in removableCols)
                            this.tableImportacion.Columns.Remove(col);

                        //Carga los controles
                        this.Enabled = false;

                        this.btnImportar.Enabled = false;
                        this.btnRelPagos.Enabled = false;
                        this.btnReporte.Enabled = false; 
                        this.btnPremilinar.Enabled = false;
                        this.btnPagar.Enabled = false;
                        this.btnInconsistencias.Enabled = false;

                        this.masterBanco.EnableControl(false);
                        this.dtFecha.Enabled = false;

                        this.results = null;
                        this.result = null;

                        Thread process = new Thread(this.ImportThread);
                        process.Start();

                        #endregion
                    }
                    else
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoCopyField));
                        this.btnRelPagos.Enabled = false;
                        this.btnReporte.Enabled = false; 
                        this.btnPremilinar.Enabled = false;
                        this.btnPagar.Enabled = false;
                        this.btnInconsistencias.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RecaudosMasivosFin.cs", "btnImportar_Click"));
                this.btnRelPagos.Enabled = false;
                this.btnReporte.Enabled = false; 
                this.btnPremilinar.Enabled = false;
                this.btnPagar.Enabled = false;
                this.btnInconsistencias.Enabled = false;
            }
        }

        /// <summary>
        /// Evento que se encarga de mirar una relación de pagos por componente y crédito
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRelPagos_Click(object sender, EventArgs e)
        {
            this.Enabled = false;

            Thread process = new Thread(this.RelPagosThread);
            process.Start();
        }

        /// <summary>
        /// Evento que se encarga de verificar las inconsistencias
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPremilinar_Click(object sender, EventArgs e)
        {
             if (this._revisaInconsistenciaInd)
             {    
               
                bool loadData = true;
                if (this.data.Count > 0 && this.btnPagar.Enabled)
                {
                    string msgTitleSearch = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning));
                    string msgNewSearch = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewData);

                    if (MessageBox.Show(msgNewSearch, msgTitleSearch, MessageBoxButtons.YesNo) == DialogResult.No)
                        loadData = false;
                }

                if (loadData)
                {
                    this.Enabled = false;

                    this.btnImportar.Enabled = false;
                    this.btnRelPagos.Enabled = false;
                    this.btnReporte.Enabled = false;
                    this.btnPremilinar.Enabled = false;
                    this.btnPagar.Enabled = false;
                    this.btnInconsistencias.Enabled = false;

                    this.results = new List<DTO_TxResult>();
                    this.result = new DTO_TxResult();

                    Thread process = new Thread(this.PreliminarThread);
                    process.Start();
                } 
            }
             else
                 MessageBox.Show("Debe revisar antes las Inconsistencias presentes en el proceso");
        }

        /// <summary>
        /// Evento que se encarga de pagar las migraciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPagar_Click(object sender, EventArgs e)
        {
            this.Enabled = false;

            this.btnImportar.Enabled = false;
            this.btnRelPagos.Enabled = false;
            this.btnReporte.Enabled = false; 
            this.btnPremilinar.Enabled = false;
            this.btnPagar.Enabled = false;
            this.btnInconsistencias.Enabled = false;

            this.results = null;
            this.result = null;

            Thread process = new Thread(this.PagarThread);
            process.Start();
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

        /// <summary>
        /// Evento que se encarga de pagar las migraciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReporte_Click(object sender, EventArgs e)
        {
            Thread process = new Thread(this.LoadReportThread);
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
                clientes = new Dictionary<string, string>();

                #region Variables de función y mensajes de error
                this.result = new DTO_TxResult();
                this.result.Result = ResultValue.OK;
                this.result.Details = new List<DTO_TxResultDetail>();
                //Lista con los dtos a subir y Fks a validas
                this.data = new List<DTO_ccIncorporacionDeta>();
                Dictionary<string, List<Tuple<string, bool>>> fks = new Dictionary<string, List<Tuple<string, bool>>>();
                List<string> fkNames = new List<string>();
                //Mensajes de error
                string msgEmptyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField);
                string msgInvalidFormat = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidFormat);
                string msgFecha = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidDate);
                string msgNoRel = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ImpNoRel);
                string msgPositive = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue);
                string msgClinteRepetido = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ClienteSinLibranza);
                string msgCodAndCliente = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ClienteYCodigo);
                string msgFkNotFound = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FkNotFound);
                string msgNoRowAdded = _bc.GetResourceError(DictionaryMessages.Err_NoRowAdded); 
                //Popiedades de la incorporacion
                DTO_ccIncorporacionDeta incorp = new DTO_ccIncorporacionDeta();
                bool createDTO = true;
                bool validList = true;
                #endregion
                #region Llena las listas de las columnas

                List<string> cols = new List<string>();
                foreach(DataColumn col in this.tableImportacion.Columns)
                    cols.Add(col.ColumnName);

                fks.Add(this._clienteRsx, new List<Tuple<string, bool>>());

                #endregion
                #region Llena información para enviar a la grilla (lee filas)
                int percent = 0;
                for (int i = 0; i < this.tableImportacion.Rows.Count; ++i)
                {
                    #region Aumenta el porcentaje y revisa que tenga lineas para leer

                    this.Invoke(this.UpdateProgressDelegate, new object[] { percent });
                    percent = ((i + 1) * 100) / (this.tableImportacion.Rows.Count);

                    if (FormProvider.Master.ProcessCanceled(this.documentID))
                    {
                        this.result.Details = new List<DTO_TxResultDetail>();
                        this.result.ResultMessage = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser);
                        this.result.Result = ResultValue.NOK;
                        break;
                    }

                    #endregion

                    //Revisa si la fila es valida
                    DTO_TxResultDetail rd = new DTO_TxResultDetail();
                    rd.line = i + 1;
                    if (this.tableImportacion.Rows[i]["ValidRow"] == "1")
                    {
                        #region Recorre todas las columnas y verifica que tengan datos validos

                        createDTO = true;

                        rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                        rd.Message = "OK";

                        #region Revisa la info de las FKs

                        foreach (DataColumn col in this.tableImportacion.Columns)
                        {
                            string colVal = this.tableImportacion.Rows[i][col.ColumnName].ToString().Trim();
                            if (col.ColumnName == this._clienteRsx && !string.IsNullOrWhiteSpace(colVal))
                            {
                                this.tableImportacion.Rows[i][col.ColumnName] = colVal.ToUpper();

                                Tuple<string, bool> tupValid = new Tuple<string, bool>(colVal, true);
                                Tuple<string, bool> tupInvalid = new Tuple<string, bool>(colVal, false);

                                if (fks[col.ColumnName].Contains(tupValid))
                                    continue;
                                else if (fks[col.ColumnName].Contains(tupInvalid))
                                {
                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                    rdF.Field = col.ColumnName;
                                    rdF.Message = string.Format(msgFkNotFound, colVal);
                                    rd.DetailsFields.Add(rdF);

                                    createDTO = false;
                                }
                                else
                                {
                                    int docId = this.GetMasterDocumentID(col.ColumnName);
                                    object dto = _bc.GetMasterDTO(AppMasters.MasterType.Simple, docId, false, colVal, true);

                                    if (dto == null)
                                    {
                                        fks[col.ColumnName].Add(new Tuple<string, bool>(colVal, false));

                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = col.ColumnName;
                                        rdF.Message = string.Format(msgFkNotFound, colVal);
                                        rd.DetailsFields.Add(rdF);

                                        createDTO = false;
                                    }
                                    else
                                        fks[col.ColumnName].Add(new Tuple<string, bool>(colVal, true));
                                }
                            }
                        }

                        #endregion
                        #region Creacion de DTO y validacion Formatos
                        if (createDTO)
                        {
                            incorp = new DTO_ccIncorporacionDeta();
                            foreach (DataColumn col in this.tableImportacion.Columns)
                            {
                                if (col.ColumnName != "ValidRow")
                                {
                                    string colVal = this.tableImportacion.Rows[i][col.ColumnName].ToString().Trim();
                                    try
                                    {
                                        #region Validacion de Nulls (Campos basicos)

                                        if (string.IsNullOrEmpty(colVal) && (col.ColumnName == "FechaNomina" || col.ColumnName == "ValorCuota"))
                                        {
                                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = col.ColumnName;
                                            rdF.Message = msgEmptyField;
                                            rd.DetailsFields.Add(rdF);

                                            createDTO = false;
                                        }

                                        #endregion
                                        #region Validacion Formatos

                                        PropertyInfo pi = incorp.GetType().GetProperty(col.ColumnName);
                                        UDT udt = (UDT)pi.GetValue(incorp, null);
                                        PropertyInfo piUDT = udt.GetType().GetProperty("Value");

                                        //Comprueba los valores solo para los booleanos
                                        if (piUDT.PropertyType.Equals(typeof(bool)) || piUDT.PropertyType.Equals(typeof(Nullable<bool>)))
                                        {
                                            string colValTemp = "false";
                                            if (colVal.Trim() != string.Empty)
                                            {
                                                colValTemp = "true";
                                                if (colVal.ToLower() != "x")
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = col.ColumnName;
                                                    rdF.Message = msgInvalidFormat + " (x)";
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            this.tableImportacion.Rows[i][col.ColumnName] = colValTemp;
                                        }

                                        //Valida formatos para las otras columnas
                                        if (colVal != string.Empty)
                                        {
                                            if (piUDT.PropertyType.Equals(typeof(DateTime)) || piUDT.PropertyType.Equals(typeof(Nullable<DateTime>)))
                                            {
                                                try
                                                {
                                                    DateTime val = DateTime.ParseExact(colVal, FormatString.Date, System.Globalization.CultureInfo.InvariantCulture);
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = col.ColumnName;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDate);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            if (piUDT.PropertyType.Equals(typeof(int)) || piUDT.PropertyType.Equals(typeof(Nullable<int>)))
                                            {
                                                try
                                                {
                                                    int val = Convert.ToInt32(colVal);
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = col.ColumnName;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatInvalidNumber);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            if (piUDT.PropertyType.Equals(typeof(long)) || piUDT.PropertyType.Equals(typeof(Nullable<long>)))
                                            {
                                                try
                                                {
                                                    long val = Convert.ToInt64(colVal);
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = col.ColumnName;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatInteger);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            if (piUDT.PropertyType.Equals(typeof(short)) || piUDT.PropertyType.Equals(typeof(Nullable<short>)))
                                            {
                                                try
                                                {
                                                    short val = Convert.ToInt16(colVal);
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = col.ColumnName;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatLimitNumber);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            if (piUDT.PropertyType.Equals(typeof(byte)) || piUDT.PropertyType.Equals(typeof(Nullable<byte>)))
                                            {
                                                try
                                                {
                                                    byte val = Convert.ToByte(colVal);
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = col.ColumnName;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatNumberRange);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            if (piUDT.PropertyType.Equals(typeof(decimal)) || piUDT.PropertyType.Equals(typeof(Nullable<decimal>)))
                                            {
                                                try
                                                {
                                                    decimal val = Convert.ToDecimal(colVal, CultureInfo.InvariantCulture);
                                                    if (colVal.Trim().Contains(','))
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = col.ColumnName;
                                                        rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDecimal);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = col.ColumnName;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDecimal);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }

                                        } //validacion si no es null
                                        #endregion

                                        //Si paso las validaciones asigne el valor al DTO
                                        if (createDTO && !string.IsNullOrWhiteSpace(colVal))
                                            udt.SetValueFromString(colVal);
                                    }
                                    catch (Exception ex1)
                                    {
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = col.ColumnName;
                                        rdF.Message = _bc.GetResourceForException(ex1, "WinApp", "RecaudosMasivosFin.cs - Creacion de DTO y validacion Formatos");
                                        rd.DetailsFields.Add(rdF);

                                        createDTO = false;
                                    }
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
                            incorp.PosicionImportacion.Value = rd.line;
                            this.data.Add(incorp);
                        }
                        else
                            validList = false;

                        #endregion

                        #endregion
                    }
                    else
                    {
                        rd.Message = msgNoRowAdded;
                        this.result.Details.Add(rd);
                    }
                }
                this.Invoke(this.UpdateProgressDelegate, new object[] { percent });
                #endregion
                #region Valida las restricciones particulares de los recaudos masivos
                if (validList)
                {
                    #region Variables generales

                    this.result = new DTO_TxResult();
                    this.result.Result = ResultValue.OK;
                    this.result.Details = new List<DTO_TxResultDetail>();

                    int i = 0;
                    percent = 0;

                    #endregion
                    foreach (DTO_ccIncorporacionDeta dto in this.data)
                    {
                        #region Aumenta el porcentaje y revisa que tenga lineas para leer
                        this.Invoke(this.UpdateProgressDelegate, new object[] { percent });
                        percent = ((i + 1) * 100) / (this.data.Count);
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
                        rd.line = dto.PosicionImportacion.Value.Value;
                        rd.Message = "OK";
                        createDTO = true;
                        #endregion
                        #region Validaciones particulares del DTO
                        this.ValidateDataImport(dto, rd, msgFecha, msgNoRel, msgPositive, msgEmptyField, msgClinteRepetido, msgCodAndCliente);
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
                if (validList)
                {
                    #region Hace validaciones del servidor
                    this.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCartera(this.documentID));
                    this.ProgressBarThread = new Thread(this.CheckServerProcessStatus);
                    this.ProgressBarThread.Start();

                    this.result = _bc.AdministrationModel.RecaudosMasivosFin_Validar(this.documentID, this.dtFecha.DateTime.Date, ref this.data);
                    this.StopProgressBarThread();

                    if (this.result.Result == ResultValue.OK)
                    {
                        this._isOK = true;
                    }
                    else
                    {
                        this._isOK = false;
                        this.data = new List<DTO_ccIncorporacionDeta>();
                    }
                    #endregion
                }
                else
                {
                    #region Muestra mensajes de error
                    this._isOK = false;
                    this.data = new List<DTO_ccIncorporacionDeta>();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                this.data = new List<DTO_ccIncorporacionDeta>();
                this._isOK = false;
                this.result.Result = ResultValue.NOK;
                this.result.ResultMessage = _bc.GetResourceForException(ex, "WinApp-RecaudosMasivosFin.cs", "ImportThread");
            }
            finally
            {
                this.Invoke(this.endImportarDelegate);
            }
        }

        /// <summary>
        /// Hilo de Procesar las inconsistencias
        /// </summary>
        private void PreliminarThread()
        {
            try
            {               
                this.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCartera(this.documentID));
                this.ProgressBarThread = new Thread(this.CheckServerProcessStatus);
                this.ProgressBarThread.Start();

                this.result = _bc.AdministrationModel.RecaudosMasivosFin_Procesar(this.documentID, this.dtPeriod.DateTime.Date, this.data);
                this.StopProgressBarThread();                   
            }
            catch (Exception ex)
            {
                this.data = new List<DTO_ccIncorporacionDeta>();
                this._isOK = false;
                this.result.Result = ResultValue.NOK;
                this.result.ResultMessage = _bc.GetResourceForException(ex, "WinApp-RecaudosMasivosFin.cs", "ProcesarThread");
            }
            finally
            {
                this.Invoke(this.endPreliminarDelegate);
            }
        }

        /// <summary>
        /// Carga el reporte con las inconsistencias
        /// </summary>
        private void RelPagosThread()
        {
            try
            {
                this.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCartera(this.documentID));
                this.ProgressBarThread = new Thread(this.CheckServerProcessStatus);
                this.ProgressBarThread.Start();

                List<DTO_ccIncorporacionDeta> tmp = ObjectCopier.Clone(this.data);
                this.tableRelacionPagos = _bc.AdministrationModel.RecaudosMasivos_GetRelacionPagos(this.documentID, tmp);
                this.Invoke(this.showRelPagosDelegate);
            }
            catch (Exception ex)
            {
                _bc.GetResourceForException(ex, "WinApp-RecaudosMasivosFin.cs", "RelPagosThread");
            }
        }

        /// <summary>
        /// Hilo de Pago de nomina
        /// </summary>
        private void PagarThread()
        {
            try
            {
                this.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCartera(this.documentID));
                this.ProgressBarThread = new Thread(this.CheckServerProcessStatus);
                this.ProgressBarThread.Start();

                decimal valNomina = Convert.ToDecimal(this.txtValorNeto.EditValue);
                decimal valPagos = this.data.Sum(x => x.ValorCuota.Value.Value);
                List<DTO_ccIncorporacionDeta> temp = ObjectCopier.Clone(this.data);
                this.result = _bc.AdministrationModel.RecaudosMasivos_Pagar(this.documentID, this._actFlujo.ID.Value, this.periodo,
                    this.dtFecha.DateTime, this.dtFecha.DateTime, this.valorPagaduria, this.centroPagoID, this.bancosCta, this.data);

                this.data = temp;
                this.StopProgressBarThread();

                this._isOK = this.result.Result == ResultValue.OK ? true : false;
            }
            catch (Exception ex)
            {
                this.StopProgressBarThread();

                this._isOK = false;
                this.result = new DTO_TxResult();
                this.result.Result = ResultValue.NOK;
                this.result.ResultMessage = _bc.GetResourceForException(ex, "WinApp-RecaudosMasivosFin.cs", "PagarThread");
            }
            finally
            {
                this.Invoke(this.endPagarDelegate);
            }
        }

        /// <summary>
        /// Carga el reporte con las inconsistencias
        /// </summary>
        private void InconsistenciasThread()
        {
            try
            {
                this._revisaInconsistenciaInd = true;
                if (this.result != null)
                {
                    _bc.AssignResultResources(null, this.result);

                    reportName = this._bc.AdministrationModel.Rep_TxResult(this.result);
                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                    Process.Start(fileURl);
                }
                else if (this.results != null)
                {

                    _bc.AssignResultResources(null, this.results);

                    reportName = this._bc.AdministrationModel.Rep_TxResultDetails(this.results);
                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                    Process.Start(fileURl);
                }
            }
            catch (Exception ex)
            {
                _bc.GetResourceForException(ex, "WinApp-RecaudosMasivosFin.cs", "InconsistenciasThread");
            }
            finally
            {
                this.Invoke(this.endInconsistenciasDelegate);
            }
        }

        /// <summary>
        /// FUncion para generar el reporte
        /// </summary>
        private void LoadReportThread()
        {
            try
            {
                string reportName;
                string fileURl;

                List<DTO_ccIncorporacionDeta> tmp = ObjectCopier.Clone(this.data);
                reportName = this._bc.AdministrationModel.Report_Cc_RecaudosMasivosGetRelacionPagos(this.documentID, tmp);
                fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                Process.Start(fileURl);

                this.StopProgressBarThread();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "RecaudosMasivos.cs-LoadReportThread"));
                throw;
            }
        }

        #endregion
    }
}
