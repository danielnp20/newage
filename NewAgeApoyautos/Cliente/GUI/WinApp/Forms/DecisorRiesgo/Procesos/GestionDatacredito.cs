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
    public partial class GestionDatacredito : ProcessForm
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
            this.btnGenerateSol.Enabled = true;
            this.btnImportar.Enabled = true;

            if (this._isOK)
            {            
                if (this.result != null)
                    this.btnInconsistencias.Enabled = this.result.Details.Count > 0 ? true : false;
                this._revisaInconsistenciaInd = !this.btnInconsistencias.Enabled;
            }
            else
            {
                if(this.result != null)
                    this.btnInconsistencias.Enabled = this.result.Details.Count > 0 ? true : false;
                this._revisaInconsistenciaInd = !this.btnInconsistencias.Enabled;
                this.dtFecha.Enabled = true;
            }
            if (this.result != null)
            {
                MessageForm frm = new MessageForm(this.result);
                this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });
            }          
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
                
                this.datos = new  List<DTO_ccSolicitudDataCreditoDatos>();
                this.score = new List<DTO_ccSolicitudDataCreditoScore>();
                this.ubica = new List<DTO_ccSolicitudDataCreditoUbica>();
                this.quanto = new List<DTO_ccSolicitudDataCreditoQuanto>();
                this.btnInconsistencias.Enabled = false;

                this._isOK = false;
                this.dtFecha.Enabled = true;

                this.results = null;
                this.result = null;
                this.lblLeyenda.Text = "";
            }
            else
            {
                MessageForm frm = new MessageForm(this.result);
                this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });

                this.btnInconsistencias.Enabled = true;
                this.lblLeyenda.Text = "Ver Incosistencias";
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
        private DateTime periodo = DateTime.Now;
        private bool validarInconsistencias;
        //Variables para la importacion
        private PasteOpDTO pasteRet;
        private string format;
        private string formatSeparator = "\t";
        DTO_TxResult result;
        List<DTO_TxResult> results;
        DataTableOperations tableOp;
        DataTable tableImportacion = new DataTable();
        List<DTO_glDocMigracionCampo> columnasImportacion = new List<DTO_glDocMigracionCampo>();
        List<string> codigos = new List<string>();
        Dictionary<string, string> clientes = new Dictionary<string, string>();
        //Variables con los recursos de las Fks
        private string _clienteRsx = "ClienteID";
        private string reportName;
        private string fileURl;
        private bool _revisaInconsistenciaInd = true;

        private List<DTO_ccSolicitudDataCreditoDatos> datos = null;
        private List<DTO_ccSolicitudDataCreditoScore> score = null;
        private List<DTO_ccSolicitudDataCreditoUbica> ubica = null;
        private List<DTO_ccSolicitudDataCreditoQuanto> quanto = null;
        #endregion

        public GestionDatacredito()
        {
          //InitializeComponent();
        }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            try
            {
                this.documentID = AppProcess.GestionDatacredito;
                this.InitializeComponent();

                //Inicializa los delegados
                this.endImportarDelegate = new EndImportar(this.EndImportarMethod);
                this.endPagarDelegate = new EndPagar(this.EndPagarMethod);
                this.endInconsistenciasDelegate = new EndInconsistencias(this.EndInconsistenciasMethod);

                string periodoStr = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo);
                string diaCierreStr = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_DiaUltimoCierre);
                string validarIncon = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_AnalisisInconsistenciaNomina);

                //Carga la configuracion inicial de controles
                this._isOK = false;

                //Inconsistencias
                if (validarIncon == "0")
                    this.validarInconsistencias = false;
                else
                    this.validarInconsistencias = true;

                this.dtFecha.DateTime = DateTime.Now.Date.AddDays(-1);
                //Asigna el formato
                this.format = _bc.GetImportExportFormat(typeof(DTO_ccIncorporacionDeta), AppProcess.GestionDatacredito);
                //Carga los recuros
                FormProvider.LoadResources(this, AppProcess.GestionDatacredito);
                this.btnInconsistencias.Enabled = false;
            }
            catch (Exception ex)
            {                
               MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GestionDatacredito.cs", "InitForm"));
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
                {
                    #region Valida que la fecha se encuentre en el periodo
                    //if (dto.FechaNomina.Value.Value != this.dtFechaAplica.DateTime)
                    //{
                    //    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    //    rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppProcess.RecaudosMasivos + "_FechaNomina"); ;
                    //    rdF.Message = msgFecha;
                    //    rd.DetailsFields.Add(rdF);
                    //}
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
                    #region Validacion Codigo del empleado
                  

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
                            if (string.IsNullOrWhiteSpace(clientes[dto.CodEmpleado.Value]) || dto.Libranza.Value == null)
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
                    #region Validacion para que cliente que se repita debe tener la libranza

                    if (!string.IsNullOrWhiteSpace(dto.ClienteID.Value))
                    {
                        if (clientes.ContainsKey(dto.ClienteID.Value))
                        {
                            if (string.IsNullOrWhiteSpace(clientes[dto.ClienteID.Value]) || dto.Libranza.Value == null)
                            {
                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ClienteID");
                                rdF.Message = msgClienteRepetido;
                                rd.DetailsFields.Add(rdF);
                            }
                        }
                        else
                        {
                            string val = dto.Libranza.Value == null ? string.Empty : dto.Libranza.Value.Value.ToString();
                            clientes.Add(dto.ClienteID.Value, val);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GestionDatacredito.cs", "ValidateDataImport"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xlsFilePath"></param>
        public List<object> GetDataXLS(int rows, int cols, Microsoft.Office.Interop.Excel.Range range, Type tipoDTO)
        {
            List<object> result = new List<object>();
            try
            {       
                //Recorre las filas del rango
                for (int row = 1; row <= rows; row++)
                {
                    int percent = (row * 100) / rows;
                    this.pbProcess.Position = percent;
                    this.pbProcess.Update();

                    int columna = 34;
                    var a = ((range.Cells[row, columna] as Excel.Range).Text);
                    var b = ((range.Cells[row, columna] as Excel.Range).Value);
                    var c = ((range.Cells[row, columna] as Excel.Range).Value2);
                    var d = ((range.Cells[row, columna] as Excel.Range).FormulaLocal);

                    //Valida que tipo de datos se asigna y desde la fila de datos de interes
                    if (row >= 4 && tipoDTO == typeof(DTO_ccSolicitudDataCreditoDatos))
                    {
                        this.lblLeyenda.Text = "Leyendo hoja Datos...";
                        DTO_ccSolicitudDataCreditoDatos datos = new DTO_ccSolicitudDataCreditoDatos();                       
                        
                        datos.TipoId.Value = (range.Cells[row, 1] as Excel.Range).Text;
                        datos.NumeroId.Value = (range.Cells[row, 2] as Excel.Range).Text;
                        if (!string.IsNullOrEmpty(datos.NumeroId.Value))
                        {
                            #region DATOS DE IDENTIFICACION
                            datos.EstadoDoc.Value = (range.Cells[row, 3] as Excel.Range).Text;
                            datos.Nombre.Value = (range.Cells[row, 4] as Excel.Range).Text;
                            datos.RangoEdad.Value = (range.Cells[row, 5] as Excel.Range).Text;
                            datos.Genero.Value = (range.Cells[row, 6] as Excel.Range).Text;
                            datos.CiudadExp.Value = (range.Cells[row, 7] as Excel.Range).Text;
                            datos.FechaAct.Value = !string.IsNullOrEmpty((range.Cells[row, 8] as Excel.Range).Text) ? Convert.ToDateTime((range.Cells[row, 8] as Excel.Range).Text) : null;
                            datos.ActEconomica.Value = (range.Cells[row, 9] as Excel.Range).Text;
                            datos.RUT.Value = (range.Cells[row, 10] as Excel.Range).Text;
                            datos.TipoContrato.Value = (range.Cells[row, 11] as Excel.Range).Text;
                            datos.FechaContrato.Value = !string.IsNullOrEmpty((range.Cells[row, 12] as Excel.Range).Text) ? Convert.ToDateTime((range.Cells[row, 12] as Excel.Range).Text) : null;
                            datos.NumeObligACT.Value = !string.IsNullOrEmpty((range.Cells[row, 13] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 13] as Excel.Range).FormulaLocal) : null;
                            #endregion
                            #region CARTERA BANCARIA
                            datos.NumObligaBAN.Value = !string.IsNullOrEmpty((range.Cells[row, 14] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 14] as Excel.Range).FormulaLocal) : null;
                            datos.VlrInicialBAN.Value = !string.IsNullOrEmpty((range.Cells[row, 15] as Excel.Range).FormulaLocal) ? Convert.ToInt32((range.Cells[row, 15] as Excel.Range).FormulaLocal) : null;
                            datos.VlrSaldoBAN.Value = !string.IsNullOrEmpty((range.Cells[row, 16] as Excel.Range).FormulaLocal) ? Convert.ToInt32((range.Cells[row, 16] as Excel.Range).FormulaLocal) : null;
                            datos.VlrCuotasBAN.Value = !string.IsNullOrEmpty((range.Cells[row, 17] as Excel.Range).FormulaLocal) ? Convert.ToInt32((range.Cells[row, 17] as Excel.Range).FormulaLocal) : null;
                            datos.VlrMoraBAN.Value = !string.IsNullOrEmpty((range.Cells[row, 18] as Excel.Range).FormulaLocal) ? Convert.ToInt32((range.Cells[row, 18] as Excel.Range).FormulaLocal) : null;
                            #endregion
                            #region CARTERA VIVIENDA
                            datos.NumObligaVIV.Value = !string.IsNullOrEmpty((range.Cells[row, 19] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 19] as Excel.Range).FormulaLocal) : null;
                            datos.VlrInicialVIV.Value = !string.IsNullOrEmpty((range.Cells[row, 20] as Excel.Range).FormulaLocal) ? Convert.ToInt32((range.Cells[row, 20] as Excel.Range).FormulaLocal) : null;
                            datos.VlrSaldoVIV.Value = !string.IsNullOrEmpty((range.Cells[row, 21] as Excel.Range).FormulaLocal) ? Convert.ToInt32((range.Cells[row, 21] as Excel.Range).FormulaLocal) : null;
                            datos.VlrCuotasVIV.Value = !string.IsNullOrEmpty((range.Cells[row, 22] as Excel.Range).FormulaLocal) ? Convert.ToInt32((range.Cells[row, 22] as Excel.Range).FormulaLocal) : null;
                            datos.VlrMoraVIV.Value = !string.IsNullOrEmpty((range.Cells[row, 23] as Excel.Range).FormulaLocal) ? Convert.ToInt32((range.Cells[row, 23] as Excel.Range).FormulaLocal) : null;

                            #endregion
                            #region OTRA CARTERA FINANCIERA
                            datos.NumObligaFIN.Value = !string.IsNullOrEmpty((range.Cells[row, 24] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 24] as Excel.Range).FormulaLocal) : null;
                            datos.VlrInicialFIN.Value = !string.IsNullOrEmpty((range.Cells[row, 25] as Excel.Range).FormulaLocal) ? Convert.ToInt32((range.Cells[row, 25] as Excel.Range).FormulaLocal) : null;
                            datos.VlrSaldoFIN.Value = !string.IsNullOrEmpty((range.Cells[row, 26] as Excel.Range).FormulaLocal) ? Convert.ToInt32((range.Cells[row, 26] as Excel.Range).FormulaLocal) : null;
                            datos.VlrCuotasFIN.Value = !string.IsNullOrEmpty((range.Cells[row, 27] as Excel.Range).FormulaLocal) ? Convert.ToInt32((range.Cells[row, 27] as Excel.Range).FormulaLocal) : null;
                            datos.VlrMoraFIN.Value = !string.IsNullOrEmpty((range.Cells[row, 28] as Excel.Range).FormulaLocal) ? Convert.ToInt32((range.Cells[row, 28] as Excel.Range).FormulaLocal) : null;
                            #endregion
                            #region INFORMACION TDC 
                            datos.NumeroTDC.Value = !string.IsNullOrEmpty((range.Cells[row, 29] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 29] as Excel.Range).FormulaLocal) : null;
                            datos.VlrCuposTDC.Value = !string.IsNullOrEmpty((range.Cells[row, 30] as Excel.Range).FormulaLocal) ? Convert.ToInt32((range.Cells[row, 30] as Excel.Range).FormulaLocal) : null;
                            datos.VlrUtilizadoTDC.Value = !string.IsNullOrEmpty((range.Cells[row, 31] as Excel.Range).FormulaLocal) ? Convert.ToInt32((range.Cells[row, 31] as Excel.Range).FormulaLocal) : null;
                            string porcUtiliza = "0" + (range.Cells[row, 32] as Excel.Range).FormulaLocal;
                            datos.PorUtilizaTDC.Value = Convert.ToDecimal(porcUtiliza);
                            datos.VlrCuotasTDC.Value = !string.IsNullOrEmpty((range.Cells[row, 33] as Excel.Range).FormulaLocal) ? Convert.ToInt32((range.Cells[row, 33] as Excel.Range).FormulaLocal) : null;
                            datos.VlrMoraTDC.Value = !string.IsNullOrEmpty((range.Cells[row, 34] as Excel.Range).FormulaLocal) ? Convert.ToInt32((range.Cells[row, 34] as Excel.Range).FormulaLocal) : null;
                            datos.Rango0TDC.Value = !string.IsNullOrEmpty((range.Cells[row, 35] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 35] as Excel.Range).FormulaLocal) : null;
                            datos.Rango1TDC.Value = !string.IsNullOrEmpty((range.Cells[row, 36] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 36] as Excel.Range).FormulaLocal) : null;
                            datos.Rango2TDC.Value = !string.IsNullOrEmpty((range.Cells[row, 37] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 37] as Excel.Range).FormulaLocal) : null;
                            datos.Rango3TDC.Value = !string.IsNullOrEmpty((range.Cells[row, 38] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 38] as Excel.Range).FormulaLocal) : null;
                            datos.Rango4TDC.Value = !string.IsNullOrEmpty((range.Cells[row, 39] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 39] as Excel.Range).FormulaLocal) : null;
                            datos.Rango5TDC.Value = !string.IsNullOrEmpty((range.Cells[row, 40] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 40] as Excel.Range).FormulaLocal) : null;
                            datos.Rango6TDC.Value = !string.IsNullOrEmpty((range.Cells[row, 41] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 41] as Excel.Range).FormulaLocal) : null;
                            //datos.FchAntiguaTDC.Value = !string.IsNullOrEmpty((range.Cells[row, 42] as Excel.Range).FormulaLocal) ? Convert.ToDateTime((range.Cells[row, 42] as Excel.Range).FormulaLocal) : null;

                            #endregion
                            #region SECTOR  REAL 
                            datos.NumObligaREA.Value = !string.IsNullOrEmpty((range.Cells[row, 43] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 43] as Excel.Range).FormulaLocal) : null;
                            datos.VlrInicialREA.Value = !string.IsNullOrEmpty((range.Cells[row, 44] as Excel.Range).FormulaLocal) ? Convert.ToInt32((range.Cells[row, 44] as Excel.Range).FormulaLocal) : null;
                            datos.VlrSaldoREA.Value = !string.IsNullOrEmpty((range.Cells[row, 45] as Excel.Range).FormulaLocal) ? Convert.ToInt32((range.Cells[row, 45] as Excel.Range).FormulaLocal) : null;
                            datos.VlrCuotasREA.Value = !string.IsNullOrEmpty((range.Cells[row, 46] as Excel.Range).FormulaLocal) ? Convert.ToInt32((range.Cells[row, 46] as Excel.Range).FormulaLocal) : null;
                            datos.VlrMoraREA.Value = !string.IsNullOrEmpty((range.Cells[row, 47] as Excel.Range).FormulaLocal) ? Convert.ToInt32((range.Cells[row, 47] as Excel.Range).FormulaLocal) : null;
                            #endregion
                            #region SECTOR  TELCOS
                            datos.NumObligaCEL.Value = !string.IsNullOrEmpty((range.Cells[row, 48] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 48] as Excel.Range).FormulaLocal) : null;
                            datos.VlrCuotasCEL.Value = !string.IsNullOrEmpty((range.Cells[row, 49] as Excel.Range).FormulaLocal) ? Convert.ToInt32((range.Cells[row, 49] as Excel.Range).FormulaLocal) : null;
                            datos.VlrMoraCEL.Value = !string.IsNullOrEmpty((range.Cells[row, 50] as Excel.Range).FormulaLocal) ? Convert.ToInt32((range.Cells[row, 50] as Excel.Range).FormulaLocal) : null;
                            #endregion
                            #region SECTOR  COOPERATIVO 
                            datos.NumObligaCOP.Value = !string.IsNullOrEmpty((range.Cells[row, 51] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 51] as Excel.Range).FormulaLocal) : null;
                            datos.VlrInicialCOP.Value = !string.IsNullOrEmpty((range.Cells[row, 52] as Excel.Range).FormulaLocal) ? Convert.ToInt32((range.Cells[row, 52] as Excel.Range).FormulaLocal) : null;
                            datos.VlrSaldoCOP.Value = !string.IsNullOrEmpty((range.Cells[row, 53] as Excel.Range).FormulaLocal) ? Convert.ToInt32((range.Cells[row, 53] as Excel.Range).FormulaLocal) : null;
                            datos.VlrCuotasCOP.Value = !string.IsNullOrEmpty((range.Cells[row, 54] as Excel.Range).FormulaLocal) ? Convert.ToInt32((range.Cells[row, 54] as Excel.Range).FormulaLocal) : null;
                            datos.VlrMoraCOP.Value = !string.IsNullOrEmpty((range.Cells[row, 55] as Excel.Range).FormulaLocal) ? Convert.ToInt32((range.Cells[row, 55] as Excel.Range).FormulaLocal) : null;
                            #endregion
                            #region CODEUDORES 
                            datos.NumObligaCOD.Value = !string.IsNullOrEmpty((range.Cells[row, 56] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 56] as Excel.Range).FormulaLocal) : null;
                            datos.VlrSaldoCOD.Value = !string.IsNullOrEmpty((range.Cells[row, 57] as Excel.Range).FormulaLocal) ? Convert.ToInt32((range.Cells[row, 57] as Excel.Range).FormulaLocal) : null;
                            datos.VlrCuotasCOD.Value = !string.IsNullOrEmpty((range.Cells[row, 58] as Excel.Range).FormulaLocal) ? Convert.ToInt32((range.Cells[row, 58] as Excel.Range).FormulaLocal) : null;
                            datos.VlrMoraCOD.Value = !string.IsNullOrEmpty((range.Cells[row, 59] as Excel.Range).FormulaLocal) ? Convert.ToInt32((range.Cells[row, 59] as Excel.Range).FormulaLocal) : null;
                            #endregion
                            #region ESTADO  DE  CARTERA  ACTUAL
                            datos.EstadoActDia.Value = !string.IsNullOrEmpty((range.Cells[row, 60] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 60] as Excel.Range).FormulaLocal) : null;
                            datos.EstadoAct30.Value = !string.IsNullOrEmpty((range.Cells[row, 61] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 61] as Excel.Range).FormulaLocal) : null;
                            datos.EstadoAct60.Value = !string.IsNullOrEmpty((range.Cells[row, 62] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 62] as Excel.Range).FormulaLocal) : null;
                            datos.EstadoAct90.Value = !string.IsNullOrEmpty((range.Cells[row, 63] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 63] as Excel.Range).FormulaLocal) : null;
                            datos.EstadoAct120.Value = !string.IsNullOrEmpty((range.Cells[row, 64] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 64] as Excel.Range).FormulaLocal) : null;
                            datos.EstadoActCas.Value = !string.IsNullOrEmpty((range.Cells[row, 65] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 65] as Excel.Range).FormulaLocal) : null;
                            datos.EstadoActDud.Value = !string.IsNullOrEmpty((range.Cells[row, 66] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 66] as Excel.Range).FormulaLocal) : null;
                            datos.EstadoActCob.Value = !string.IsNullOrEmpty((range.Cells[row, 67] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 67] as Excel.Range).FormulaLocal) : null;
                            #endregion
                            #region ESTADO CARTERA HISTORICO
                            datos.EstadoHis30.Value = !string.IsNullOrEmpty((range.Cells[row, 68] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 68] as Excel.Range).FormulaLocal) : null;
                            datos.EstadoHis60.Value = !string.IsNullOrEmpty((range.Cells[row, 69] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 69] as Excel.Range).FormulaLocal) : null;
                            datos.EstadoHis90.Value = !string.IsNullOrEmpty((range.Cells[row, 70] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 70] as Excel.Range).FormulaLocal) : null;
                            datos.EstadoHis120.Value = !string.IsNullOrEmpty((range.Cells[row, 71] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 71] as Excel.Range).FormulaLocal) : null;
                            datos.EstadoHisCan.Value = !string.IsNullOrEmpty((range.Cells[row, 72] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 72] as Excel.Range).FormulaLocal) : null;
                            datos.EstadoHisRec.Value = !string.IsNullOrEmpty((range.Cells[row, 73] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 73] as Excel.Range).FormulaLocal) : null;
                            #endregion
                            #region ALTURA MORA MAXIMA
                            datos.AlturaMorTDC.Value = !string.IsNullOrEmpty((range.Cells[row, 74] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 74] as Excel.Range).FormulaLocal) : null;
                            datos.AlturaMorBAN.Value = !string.IsNullOrEmpty((range.Cells[row, 75] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 75] as Excel.Range).FormulaLocal) : null;
                            datos.AlturaMorCOP.Value = !string.IsNullOrEmpty((range.Cells[row, 76] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 76] as Excel.Range).FormulaLocal) : null;
                            datos.AlturaMorHIP.Value = !string.IsNullOrEmpty((range.Cells[row, 77] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 77] as Excel.Range).FormulaLocal) : null;
                            #endregion
                            #region ENDEUDAMIENTO
                            datos.PeorEndeudT1.Value = (range.Cells[row, 78] as Excel.Range).Text;
                            datos.PeorEndeudT2.Value = (range.Cells[row, 79] as Excel.Range).Text;
                            #endregion
                            #region CUENTAS BANCARIAS
                            datos.CtasAhorrosAct.Value = !string.IsNullOrEmpty((range.Cells[row, 80] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 80] as Excel.Range).FormulaLocal) : null;
                            datos.CtasCorrienteAct.Value = !string.IsNullOrEmpty((range.Cells[row, 81] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 81] as Excel.Range).FormulaLocal) : null;
                            datos.CtasEmbargadas.Value = !string.IsNullOrEmpty((range.Cells[row, 82] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 82] as Excel.Range).FormulaLocal) : null;
                            datos.CtasMalManejo.Value = !string.IsNullOrEmpty((range.Cells[row, 83] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 83] as Excel.Range).FormulaLocal) : null;
                            datos.CtasSaldadas.Value = !string.IsNullOrEmpty((range.Cells[row, 84] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 84] as Excel.Range).FormulaLocal) : null;
                            datos.UltConsultas.Value = !string.IsNullOrEmpty((range.Cells[row, 85] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 85] as Excel.Range).FormulaLocal) : null;
                            datos.EstadoConsulta.Value = !string.IsNullOrEmpty((range.Cells[row, 86] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 86] as Excel.Range).FormulaLocal) : null;
                            #endregion
                            result.Add(datos);
                        }
                    }
                    else if (row >= 2 && tipoDTO == typeof(DTO_ccSolicitudDataCreditoScore))
                    {
                        this.lblLeyenda.Text = "Leyendo hoja Score...";
                        DTO_ccSolicitudDataCreditoScore score = new DTO_ccSolicitudDataCreditoScore();
                        #region DATOS GENERALES
                        score.TipoId.Value = (range.Cells[row, 1] as Excel.Range).Text;
                        score.NumeroId.Value = (range.Cells[row, 2] as Excel.Range).Text;
                        if (!string.IsNullOrEmpty(score.NumeroId.Value))
                        {
                            score.Nombre.Value = (range.Cells[row, 3] as Excel.Range).Text;
                            score.Puntaje.Value = (range.Cells[row, 4] as Excel.Range).Text;
                            score.Razon1.Value = (range.Cells[row, 5] as Excel.Range).Text;
                            score.Razon2.Value = (range.Cells[row, 6] as Excel.Range).Text;
                            score.Razon3.Value = (range.Cells[row, 7] as Excel.Range).Text;
                            score.Razon4.Value = (range.Cells[row, 8] as Excel.Range).Text;
                          
                            result.Add(score);
                        }
                        #endregion
                    }
                    else if (row >= 3 && tipoDTO == typeof(DTO_ccSolicitudDataCreditoUbica))
                    {
                        this.lblLeyenda.Text = "Leyendo hoja Ubicación..";
                        DTO_ccSolicitudDataCreditoUbica ubica = new DTO_ccSolicitudDataCreditoUbica();      
                        ubica.TipoId.Value = (range.Cells[row, 1] as Excel.Range).Text;
                        ubica.NumeroId.Value = (range.Cells[row, 2] as Excel.Range).Text;
                        if (!string.IsNullOrEmpty(ubica.NumeroId.Value))
                        {
                            #region DATOS DE IDENTIFICACION
                            ubica.Nombre.Value = (range.Cells[row, 3] as Excel.Range).Text;
                            ubica.FechaExp.Value = !string.IsNullOrEmpty((range.Cells[row, 4] as Excel.Range).Text) ? Convert.ToDateTime((range.Cells[row, 4] as Excel.Range).Text) : null;
                            ubica.CiudadExp.Value = (range.Cells[row, 5] as Excel.Range).Text;
                            ubica.DeptoExp.Value = (range.Cells[row, 6] as Excel.Range).Text;
                            ubica.Genero.Value = (range.Cells[row, 7] as Excel.Range).Text;
                            ubica.RangoEdad.Value = (range.Cells[row, 8] as Excel.Range).Text;
                            #endregion
                            #region TELEFONOS
                            ubica.CiudadTel1.Value = (range.Cells[row, 9] as Excel.Range).Text;
                            ubica.DeptoTel1.Value = (range.Cells[row, 10] as Excel.Range).Text;
                            ubica.CodCiudadTel1.Value = (range.Cells[row, 11] as Excel.Range).Text;
                            ubica.CodDeptoTel1.Value = (range.Cells[row, 12] as Excel.Range).Text;
                            ubica.NumeroTel1.Value = (range.Cells[row, 13] as Excel.Range).Text;
                            ubica.TipoTel1.Value = (range.Cells[row, 14] as Excel.Range).Text;
                            ubica.RepDesdeTel1.Value = !string.IsNullOrEmpty((range.Cells[row, 15] as Excel.Range).Text) ? Convert.ToDateTime((range.Cells[row, 15] as Excel.Range).Text) : null;
                            ubica.FechaActTel1.Value = !string.IsNullOrEmpty((range.Cells[row, 16] as Excel.Range).Text) ? Convert.ToDateTime((range.Cells[row, 16] as Excel.Range).Text) : null;
                            ubica.NumEntidadTel1.Value = !string.IsNullOrEmpty((range.Cells[row, 17] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 17] as Excel.Range).FormulaLocal) : null;
                            ubica.CiudadTel2.Value = (range.Cells[row, 18] as Excel.Range).Text;
                            ubica.DeptoTel2.Value = (range.Cells[row, 19] as Excel.Range).Text;
                            ubica.CodCiudadTel2.Value = (range.Cells[row, 20] as Excel.Range).Text;
                            ubica.CodDeptoTel2.Value = (range.Cells[row, 21] as Excel.Range).Text;
                            ubica.NumeroTel2.Value = (range.Cells[row, 22] as Excel.Range).Text;
                            ubica.TipoTel2.Value = (range.Cells[row, 23] as Excel.Range).Text;
                            ubica.RepDesdeTel2.Value = !string.IsNullOrEmpty((range.Cells[row, 24] as Excel.Range).Text) ? Convert.ToDateTime((range.Cells[row, 24] as Excel.Range).Text) : null;
                            ubica.FechaActTel2.Value = !string.IsNullOrEmpty((range.Cells[row, 25] as Excel.Range).Text) ? Convert.ToDateTime((range.Cells[row, 25] as Excel.Range).Text) : null;
                            ubica.NumEntidadTel2.Value = !string.IsNullOrEmpty((range.Cells[row, 26] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 26] as Excel.Range).FormulaLocal) : null;
                            ubica.CiudadTel3.Value = (range.Cells[row, 27] as Excel.Range).Text;
                            ubica.DeptoTel3.Value = (range.Cells[row, 28] as Excel.Range).Text;
                            ubica.CodCiudadTel3.Value = (range.Cells[row, 29] as Excel.Range).Text;
                            ubica.CodDeptoTel3.Value = (range.Cells[row, 30] as Excel.Range).Text;
                            ubica.NumeroTel3.Value = (range.Cells[row, 31] as Excel.Range).Text;
                            ubica.TipoTel3.Value = (range.Cells[row, 32] as Excel.Range).Text;
                            ubica.RepDesdeTel3.Value = !string.IsNullOrEmpty((range.Cells[row, 33] as Excel.Range).Text) ? Convert.ToDateTime((range.Cells[row, 33] as Excel.Range).Text) : null;
                            ubica.FechaActTel3.Value = !string.IsNullOrEmpty((range.Cells[row, 34] as Excel.Range).Text) ? Convert.ToDateTime((range.Cells[row, 34] as Excel.Range).Text) : null;
                            ubica.NumEntidadTel3.Value = !string.IsNullOrEmpty((range.Cells[row, 35] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 35] as Excel.Range).FormulaLocal) : null;
                            #endregion
                            #region DIRECCIONES
                            ubica.CiudadDir1.Value = (range.Cells[row, 36] as Excel.Range).Text;
                            ubica.DeptoDir1.Value = (range.Cells[row, 37] as Excel.Range).Text;
                            ubica.CodCiudadDir1.Value = (range.Cells[row, 38] as Excel.Range).Text;
                            ubica.CodDeptoDir1.Value = (range.Cells[row, 39] as Excel.Range).Text;
                            ubica.DireccionDir1.Value = (range.Cells[row, 40] as Excel.Range).Text;
                            ubica.TipoDir1.Value = (range.Cells[row, 41] as Excel.Range).Text;
                            ubica.EstratoDir1.Value = (range.Cells[row, 42] as Excel.Range).Text;
                            ubica.RepDesdeDir1.Value = !string.IsNullOrEmpty((range.Cells[row, 43] as Excel.Range).Text) ? Convert.ToDateTime((range.Cells[row, 43] as Excel.Range).Text) : null; ;
                            ubica.FechaActDir1.Value = !string.IsNullOrEmpty((range.Cells[row, 44] as Excel.Range).Text) ? Convert.ToDateTime((range.Cells[row, 44] as Excel.Range).Text) : null; ;
                            ubica.NumEntidadDir1.Value = !string.IsNullOrEmpty((range.Cells[row, 45] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 45] as Excel.Range).FormulaLocal) : null;
                            ubica.CiudadDir2.Value = (range.Cells[row, 46] as Excel.Range).Text;
                            ubica.DeptoDir2.Value = (range.Cells[row, 47] as Excel.Range).Text;
                            ubica.CodCiudadDir2.Value = (range.Cells[row, 48] as Excel.Range).Text;
                            ubica.CodDeptoDir2.Value = (range.Cells[row, 49] as Excel.Range).Text;
                            ubica.DireccionDir2.Value = (range.Cells[row, 50] as Excel.Range).Text;
                            ubica.TipoDir2.Value = (range.Cells[row, 51] as Excel.Range).Text;
                            ubica.EstratoDir2.Value = (range.Cells[row, 52] as Excel.Range).Text;
                            ubica.RepDesdeDir2.Value = !string.IsNullOrEmpty((range.Cells[row, 53] as Excel.Range).Text) ? Convert.ToDateTime((range.Cells[row, 53] as Excel.Range).Text) : null; ;
                            ubica.FechaActDir2.Value = !string.IsNullOrEmpty((range.Cells[row, 54] as Excel.Range).Text) ? Convert.ToDateTime((range.Cells[row, 54] as Excel.Range).Text) : null; ;
                            ubica.NumEntidadDir2.Value = !string.IsNullOrEmpty((range.Cells[row, 55] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 55] as Excel.Range).FormulaLocal) : null;
                            ubica.CiudadDir3.Value = (range.Cells[row, 56] as Excel.Range).Text;
                            ubica.DeptoDir3.Value = (range.Cells[row, 57] as Excel.Range).Text;
                            ubica.CodCiudadDir3.Value = (range.Cells[row, 58] as Excel.Range).Text;
                            ubica.CodDeptoDir3.Value = (range.Cells[row, 59] as Excel.Range).Text;
                            ubica.DireccionDir3.Value = (range.Cells[row, 60] as Excel.Range).Text;
                            ubica.TipoDir3.Value = (range.Cells[row, 61] as Excel.Range).Text;
                            ubica.EstratoDir3.Value = (range.Cells[row, 62] as Excel.Range).Text;
                            ubica.RepDesdeDir3.Value = !string.IsNullOrEmpty((range.Cells[row, 63] as Excel.Range).Text) ? Convert.ToDateTime((range.Cells[row, 63] as Excel.Range).Text) : null; ;
                            ubica.FechaActDir3.Value = !string.IsNullOrEmpty((range.Cells[row, 64] as Excel.Range).Text) ? Convert.ToDateTime((range.Cells[row, 64] as Excel.Range).Text) : null; ;
                            ubica.NumEntidadDir3.Value = !string.IsNullOrEmpty((range.Cells[row, 65] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 65] as Excel.Range).FormulaLocal) : null;
                            #endregion
                            #region  DATOS ADICIONALES
                            ubica.Email1.Value = (range.Cells[row, 66] as Excel.Range).Text;
                            ubica.RepDesdeMail1.Value = !string.IsNullOrEmpty((range.Cells[row, 67] as Excel.Range).Text) ? Convert.ToDateTime((range.Cells[row, 67] as Excel.Range).Text) : null; ;
                            ubica.FechaActMail1.Value = !string.IsNullOrEmpty((range.Cells[row, 68] as Excel.Range).Text) ? Convert.ToDateTime((range.Cells[row, 68] as Excel.Range).Text) : null; ;
                            ubica.NumEntidadMail1.Value = !string.IsNullOrEmpty((range.Cells[row, 69] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 69] as Excel.Range).FormulaLocal) : null;
                            ubica.Email2.Value = (range.Cells[row, 70] as Excel.Range).Text;
                            ubica.RepDesdeMail2.Value = !string.IsNullOrEmpty((range.Cells[row, 71] as Excel.Range).Text) ? Convert.ToDateTime((range.Cells[row, 71] as Excel.Range).Text) : null; ;
                            ubica.FechaActMail2.Value = !string.IsNullOrEmpty((range.Cells[row, 72] as Excel.Range).Text) ? Convert.ToDateTime((range.Cells[row, 72] as Excel.Range).Text) : null; ;
                            ubica.NumEntidadMail2.Value = !string.IsNullOrEmpty((range.Cells[row, 73] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 73] as Excel.Range).FormulaLocal) : null;
                            ubica.Celular1.Value = (range.Cells[row, 74] as Excel.Range).Text;
                            ubica.RepDesdeCel1.Value = !string.IsNullOrEmpty((range.Cells[row, 75] as Excel.Range).Text) ? Convert.ToDateTime((range.Cells[row, 75] as Excel.Range).Text) : null; ;
                            ubica.FechaActCel1.Value = !string.IsNullOrEmpty((range.Cells[row, 76] as Excel.Range).Text) ? Convert.ToDateTime((range.Cells[row, 76] as Excel.Range).Text) : null; ;
                            ubica.NumEntidadCel1.Value = !string.IsNullOrEmpty((range.Cells[row, 77] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 77] as Excel.Range).FormulaLocal) : null;
                            ubica.Celular2.Value = (range.Cells[row, 78] as Excel.Range).Text;
                            ubica.RepDesdeCel2.Value = !string.IsNullOrEmpty((range.Cells[row, 79] as Excel.Range).Text) ? Convert.ToDateTime((range.Cells[row, 79] as Excel.Range).Text) : null; ;
                            ubica.FechaActCel2.Value = !string.IsNullOrEmpty((range.Cells[row, 80] as Excel.Range).Text) ? Convert.ToDateTime((range.Cells[row, 80] as Excel.Range).Text) : null; ;
                            ubica.NumEntidadCel2.Value = !string.IsNullOrEmpty((range.Cells[row, 81] as Excel.Range).FormulaLocal) ? Convert.ToByte((range.Cells[row, 81] as Excel.Range).FormulaLocal) : null;
                            #endregion
                            #region Por defecto
                            ubica.Direccion1IND.Value = false;
                            ubica.Direccion2IND.Value = false;
                            ubica.Direccion3IND.Value = false;
                            ubica.DireccionOtraIND.Value = false;
                            ubica.DireccionOtra.Value = string.Empty;
                            ubica.Telefono1IND.Value = false;
                            ubica.Telefono2IND.Value = false;
                            ubica.Telefono3IND.Value = false;
                            ubica.TelefonoOtroIND.Value = false;
                            ubica.TelefonoOtro.Value = string.Empty;
                            ubica.Celular1IND.Value = false;
                            ubica.Celular2IND.Value = false;
                            ubica.CelularOtraIND.Value = false;
                            ubica.CelularOtro.Value = string.Empty;
                            ubica.EMail1IND.Value = false;
                            ubica.EMail2IND.Value = false;
                            ubica.EMailOtroIND.Value = false;
                            ubica.EMailOtro.Value = string.Empty;
                            
                            #endregion
                            result.Add(ubica);
                        }
                    }
                    else if (row >= 2 && tipoDTO == typeof(DTO_ccSolicitudDataCreditoQuanto))
                    {
                        this.lblLeyenda.Text = "Leyendo hoja Quanto...";
                        DTO_ccSolicitudDataCreditoQuanto quanto = new DTO_ccSolicitudDataCreditoQuanto();
                        #region DATOS GENERALES
                        quanto.TipoId.Value = (range.Cells[row, 1] as Excel.Range).Text;
                        quanto.NumeroId.Value = (range.Cells[row, 2] as Excel.Range).Text;
                        if (!string.IsNullOrEmpty(quanto.NumeroId.Value))
                        {
                            quanto.VlrMinimo.Value = !string.IsNullOrEmpty((range.Cells[row, 3] as Excel.Range).FormulaLocal) ? Convert.ToInt32((range.Cells[row, 3] as Excel.Range).FormulaLocal) : null;
                            quanto.VlrMedio.Value = !string.IsNullOrEmpty((range.Cells[row, 4] as Excel.Range).FormulaLocal) ? Convert.ToInt32((range.Cells[row, 4] as Excel.Range).FormulaLocal) : null;
                            quanto.VlrMaximo.Value = !string.IsNullOrEmpty((range.Cells[row, 5] as Excel.Range).FormulaLocal) ? Convert.ToInt32((range.Cells[row, 5] as Excel.Range).FormulaLocal) : null;

                            string vlrMinimoSMLV = (range.Cells[row, 6] as Excel.Range).FormulaLocal;
                            quanto.VlrMinimoSMLV.Value = vlrMinimoSMLV.Contains("00,") ? 0 : Convert.ToInt32(vlrMinimoSMLV);
                            string VlrMedioSMLV = (range.Cells[row, 7] as Excel.Range).FormulaLocal;
                            quanto.VlrMedioSMLV.Value = VlrMedioSMLV.Contains("00,") ? 0 : Convert.ToInt32(VlrMedioSMLV);
                            string VlrMaximoSMLV = (range.Cells[row, 8] as Excel.Range).FormulaLocal;
                            quanto.VlrMaximoSMLV.Value = VlrMaximoSMLV.Contains("00,") ? 0 : Convert.ToInt32(VlrMaximoSMLV);
                            quanto.Exclusiones.Value = !string.IsNullOrEmpty((range.Cells[row, 9] as Excel.Range).FormulaLocal) ? Convert.ToInt32((range.Cells[row, 9] as Excel.Range).FormulaLocal) : null;
                            #endregion
                            result.Add(quanto);
                        }                    
                    }
                }
                this.lblLeyenda.Text = "";
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GestionDatacredito.cs", "GetDataXLS"));
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
            string msgTitleSearch = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning));
            string msgNewSearch = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewData);

            if (MessageBox.Show(msgNewSearch, msgTitleSearch, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.datos = new List<DTO_ccSolicitudDataCreditoDatos>() ;
                this.score = new List<DTO_ccSolicitudDataCreditoScore>();
                this.ubica = new List<DTO_ccSolicitudDataCreditoUbica>();
                this.quanto = new List<DTO_ccSolicitudDataCreditoQuanto>();
                this.btnInconsistencias.Enabled = false;

                this._isOK = false;        

                this.dtFecha.Enabled = true;
                this.dtFecha.Enabled = true;
                this.results = null;
                this.result = null;
            }
        }

        /// <summary>
        /// Evento para generar laplantilla de importacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerateSol_Click(object sender, EventArgs e)
        {
            try
            {

                DataTable result = _bc.AdministrationModel.drSolicitudDatosPersonales_GetForDatacredito();
                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ResultOK));

                string pathBasic = this._bc.GetControlValue(AppControl.RutaDocumentosEspeciales);
                System.Diagnostics.Process.Start(pathBasic);                             
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GestionDatacredito.cs", "btnGenerateSol_Click"));
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
                OpenFileDialog fDialog = new OpenFileDialog();
                fDialog.Filter = "Excel (*.xls, *.xlsx)|*.xls;*.xlsx";
                if (fDialog.ShowDialog() == DialogResult.OK)
                {
                    var miss = Type.Missing;
                    string path = Path.GetDirectoryName(fDialog.FileName);
                    string filename = Path.GetFileNameWithoutExtension(fDialog.FileName);
                    string ext = Path.GetExtension(fDialog.FileName);

                    string filePath = path + "\\" + filename + ext;

                    if (!File.Exists(filePath))
                        return;

                    Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbook xlWorkBook = xlApp.Workbooks.Open(filePath, miss, miss, miss, miss, miss, miss, miss, miss, miss, miss, miss, miss, miss, miss);

                    // Seleccion de la hoja de calculo (get_item() devuelve object y numera las hojas a partir de 1)
                    Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
                    // seleccion rango activo de la hoja
                    Microsoft.Office.Interop.Excel.Range range;

                    //Trae Datos de Datacredito
                    xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                    range = xlWorkSheet.UsedRange;
                    this.datos = this.GetDataXLS(range.Rows.Count, range.Columns.Count, range, typeof(DTO_ccSolicitudDataCreditoDatos)).Cast<DTO_ccSolicitudDataCreditoDatos>().ToList();

                    //Trae Datos de Datacredito Score
                    xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(2);
                    range = xlWorkSheet.UsedRange;
                    this.score = this.GetDataXLS(range.Rows.Count, range.Columns.Count, range, typeof(DTO_ccSolicitudDataCreditoScore)).Cast<DTO_ccSolicitudDataCreditoScore>().ToList();

                    //Trae Datos de Datacredito Ubicacion
                    xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(3);
                    range = xlWorkSheet.UsedRange;
                    this.ubica = this.GetDataXLS(range.Rows.Count, range.Columns.Count, range, typeof(DTO_ccSolicitudDataCreditoUbica)).Cast<DTO_ccSolicitudDataCreditoUbica>().ToList(); ;

                    //Trae Datos de Datacredito Quanto
                    xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(4);
                    range = xlWorkSheet.UsedRange;
                    this.quanto = this.GetDataXLS(range.Rows.Count, range.Columns.Count, range, typeof(DTO_ccSolicitudDataCreditoQuanto)).Cast<DTO_ccSolicitudDataCreditoQuanto>().ToList();

                    this.Enabled = false;

                    this.btnImportar.Enabled = false;
                    this.results = null;
                    this.result = null;

                    this.lblLeyenda.Text = "Guardando información....";
                    Thread process = new Thread(this.GuardarThread);
                    process.Start();
                    //Microsoft.Office.Interop.Excel.Workbook xlWorkBook = xlApp.Workbooks.Open(filePath, miss, miss, miss, miss, miss, miss, miss, miss, miss, miss, miss, miss, miss, miss);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GestionDatacredito.cs", "btnImportar_Click"));
                this.btnInconsistencias.Enabled = false;
            }
        }

        /// <summary>
        /// Evento que se encarga de pagar las migraciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnProcesar_Click(object sender, EventArgs e)
        {
            //this.Enabled = false;

            //this.btnImportar.Enabled = false;
            //this.btnProcesar.Enabled = false;
            //this.btnInconsistencias.Enabled = false;

            //this.results = null;
            //this.result = null;

            //Thread process = new Thread(this.GuardarThread);
            //process.Start();
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
        /// Hilo de Pago de nomina
        /// </summary>
        private void GuardarThread()
        {
            try
            {
                this.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCartera(this.documentID));
                this.ProgressBarThread = new Thread(this.CheckServerProcessStatus);
                this.ProgressBarThread.Start();

                this.result = _bc.AdministrationModel.DatacreditoGestion_Add(this.documentID,this.datos,this.score,this.ubica,this.quanto,true);

                this.StopProgressBarThread();

                this._isOK = this.result.Result == ResultValue.OK ? true : false;
            }
            catch (Exception ex)
            {
                this.StopProgressBarThread();

                this._isOK = false;
                this.result = new DTO_TxResult();
                this.result.Result = ResultValue.NOK;
                this.result.ResultMessage = _bc.GetResourceForException(ex, "WinApp-GestionDatacredito.cs", "GuardarThread");
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
                _bc.GetResourceForException(ex, "WinApp-GestionDatacredito.cs", "InconsistenciasThread");
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

                //List<DTO_ccIncorporacionDeta> tmp = ObjectCopier.Clone(this.data);
                //reportName = this._bc.AdministrationModel.Report_Cc_RecaudosMasivosGetRelacionPagos(this.documentID, tmp);
                //fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                //Process.Start(fileURl);

                this.StopProgressBarThread();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "GestionDatacredito.cs-LoadReportThread"));
                throw;
            }
        }

        #endregion
    }
}
