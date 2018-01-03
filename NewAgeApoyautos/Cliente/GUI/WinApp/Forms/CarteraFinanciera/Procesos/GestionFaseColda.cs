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
    public partial class GestionFaseColda : ProcessForm
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
                if (this.result != null)
                    this.btnInconsistencias.Enabled = this.result.Details.Count > 0 ? true : false;
                this._revisaInconsistenciaInd = !this.btnInconsistencias.Enabled;
            }
            else
            {
                if(this.result != null)
                    this.btnInconsistencias.Enabled = this.result.Details.Count > 0 ? true : false;
                this._revisaInconsistenciaInd = !this.btnInconsistencias.Enabled;
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
        public EndPagar endGuardarDelegate;
        public void EndGuardarMethod()
        {
            this.Enabled = true;
            this.btnImportar.Enabled = true;
            if (this._isOK)
            {
                MessageForm frm = new MessageForm(this.result);
                this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });            
                
                this._fasecoldas = new  List<DTO_ccFasecolda>();
                this.modelos = new List<DTO_ccFasecoldaModelo>();
                this.btnImportar.Enabled = true;
                this.btnInconsistencias.Enabled = false;

                this._isOK = false;
                this.results = null;
                this.result = null;
                this.lblLeyenda.Text = "";
            }
            else
            {
                MessageForm frm = new MessageForm(this.result);
                this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });

                this.btnImportar.Enabled = false;
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

        /// <summary>
        /// Delegado para poder invocar StateChanged desde otro hilo
        /// </summary>
        /// <param name="text">Texto del SplashScreen</param>
        delegate void TextChangedDeleg(string text);

        /// <summary>
        /// Función que permite desde el metodo que hace el cargue
        /// poner mensajes de estado . por ejemplo "Cargando idiomas"
        /// Utiliza el delegado para procesar los cambios desde otros hilos
        /// </summary>
        /// <param name="text">Cadena a mostrar en la pantalla de cargue</param>
        public void TextChanged(string text)
        {
            if (this.lblLeyenda.InvokeRequired)
            {
                TextChangedDeleg d = new TextChangedDeleg(TextChanged);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.lblLeyenda.Text = text;
            }

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
        List<DTO_glDocMigracionCampo> columnasImportacion = new List<DTO_glDocMigracionCampo>();
        List<string> codigos = new List<string>();
        //Variables con los recursos de las Fks
        private string reportName;
        private string fileURl;
        private bool _revisaInconsistenciaInd = true;

        private List<DTO_ccFasecolda> _fasecoldas = null;
        private List<DTO_ccFasecolda> _fasecoldasExist = null;
        private List<DTO_ccFasecoldaModelo> modelos = new List<DTO_ccFasecoldaModelo>();
        private Microsoft.Office.Interop.Excel.Range _range = null;
        #endregion

        public GestionFaseColda()
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
                this.documentID = AppProcess.GestionFasecolda;
                this.InitializeComponent();

                //Inicializa los delegados
                this.endImportarDelegate = new EndImportar(this.EndImportarMethod);
                this.endGuardarDelegate = new EndPagar(this.EndGuardarMethod);
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

                //Carga los recuros
                this.format = "Novedad\tMarca\tClase\tCodigo\tHomologoCodigo\tReferencia1\tReferencia2\tReferencia3\tPeso\tIdServicio\tServicio\t1970\t1971\t1972\t1973\t1974\t1975\t1976\t1977\t1978\t1979\t1980\t	1981\t1982\t1983\t1984\t1985\t1986\t1987\t1988\t1989\t1990\t1991\t1992\t1993\t1994\t1995\t1996\t1997\t1998\t1999\t2000\t2001\t2002\t2003\t2004\t2005\t2006\t2007\t2008\t2009\t2010\t2011\t2012\t2013\t2014\t2015\t2016\t2017\t2018\tBcpp\tImportado\tPotencia\tTipoCaja\tCilindraje\tNacionalidad\tCapacidadPasajeros\t	CapacidadCarga\tPuertas\tAireAcondicionado\tEjes\tEstado\tCombustible\tTransmision\tUm\tPesoCategoria";
                
                FormProvider.LoadResources(this, AppProcess.GestionFasecolda);
                this.btnInconsistencias.Enabled = false;
            }
            catch (Exception ex)
            {                
               MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GestionFaseColda.cs", "InitForm"));
            }
        }

        #endregion

        #region Funciones Privadas     

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xlsFilePath"></param>
        public void GetDataXLS( Microsoft.Office.Interop.Excel.Range range)
        {
            this._fasecoldas = new List<DTO_ccFasecolda>();
            this.modelos = new List<DTO_ccFasecoldaModelo>();
            try
            {
                int cols = range.Columns.Count;
                int rows = range.Rows.Count;
                Dictionary<int,int> años = new Dictionary<int,int>();

                //Agrega la cantidad de años segun archivo

                this.lblLeyenda.Text = "Leyendo Años...";
                for (int i = 1; i < cols; i++)//Recorre las columnas
                {
                    //Convierte el año para agregarlo con el index, si hay error no es año
                    int año = 0;
                    try { año = Convert.ToInt32((range.Cells[1, i] as Excel.Range).Text); } catch (Exception) { año = 0; };
                    if (año != 0)
                        años.Add(i, año);
                }

                //Recorre las filas del rango
                for (int row = 1; row <= rows; row++)
                {
                    int percent = (row * 100) / rows;
                    this.pbProcess.Position = percent;
                    this.pbProcess.Update();

                    //int columna = 34;
                    //var a = ((range.Cells[row, columna] as Excel.Range).Text);
                    //var b = ((range.Cells[row, columna] as Excel.Range).Value);
                    //var c = ((range.Cells[row, columna] as Excel.Range).Value2);
                    //var d = ((range.Cells[row, columna] as Excel.Range).FormulaLocal);
                  
                    //Valida que tipo de datos se asigna y desde la fila de datos de interes
                    if (row >= 2)
                    {
                        this.lblLeyenda.Text = "Leyendo Fasecolda...";                       
                        DTO_ccFasecolda fasecolda = new DTO_ccFasecolda();
                        #region FaseColda 
                        fasecolda.Marca.Value = (range.Cells[row, 2] as Excel.Range).Text;
                        fasecolda.Clase.Value = (range.Cells[row, 3] as Excel.Range).Text;
                        fasecolda.ID.Value = (range.Cells[row, 4] as Excel.Range).Text;
                        fasecolda.Tipo1.Value = (range.Cells[row, 6] as Excel.Range).Text;
                        fasecolda.Tipo2.Value = (range.Cells[row, 7] as Excel.Range).Text;
                        fasecolda.Tipo3.Value = (range.Cells[row, 8] as Excel.Range).Text;
                        //datos.Peso.Value = (range.Cells[row, 9] as Excel.Range).Text;
                        fasecolda.Servicio.Value = !string.IsNullOrEmpty((range.Cells[row, 10] as Excel.Range).Text) ? Convert.ToByte((range.Cells[row, 10] as Excel.Range).Text) : null;
                        //datos.NombreServicio.Value = (range.Cells[row, 11] as Excel.Range).Text;                       

                        this.lblLeyenda.Text = "Leyendo Modelos...";
                        foreach (KeyValuePair<int,int> año in años)
                        {
                            decimal valor = !string.IsNullOrEmpty((range.Cells[row, año.Key] as Excel.Range).FormulaLocal) ? Convert.ToDecimal((range.Cells[row, año.Key] as Excel.Range).FormulaLocal) : null;
                            if (valor > 0)
                            {
                                DTO_ccFasecoldaModelo mod = new DTO_ccFasecoldaModelo();
                                mod.FasecoldaID.Value = fasecolda.ID.Value;
                                mod.Modelo.Value = año.Value.ToString();
                                mod.Valor.Value = valor;
                                mod.CtrlVersion.Value = 1;
                                mod.ActivoInd.Value = true;
                                this.modelos.Add(mod); 
                            }
                        }

                        this.lblLeyenda.Text = "Leyendo Fasecolda...";
                        int columnTipoCaja = años.Last().Key + 4;
                        //string tipoCaja = (range.Cells[row, columnTipoCaja] as Excel.Range).Text;
                        //    fasecolda.TipoCaja.Value = tipoCaja.TrimEnd().Equals("MT") ? (byte)1 : (tipoCaja.TrimEnd().Equals("AT") ? (byte)2 : (byte)3);///PREGUNTAR
                        fasecolda.Cilindraje.Value = !string.IsNullOrEmpty((range.Cells[row, columnTipoCaja + 1] as Excel.Range).FormulaLocal) ? Convert.ToInt32((range.Cells[row, columnTipoCaja + 1] as Excel.Range).FormulaLocal) : null;
                        fasecolda.Pasajeros.Value = !string.IsNullOrEmpty((range.Cells[row, columnTipoCaja + 3] as Excel.Range).Text) ? Convert.ToByte((range.Cells[row, columnTipoCaja + 3] as Excel.Range).Text) : null;
                        fasecolda.Carga.Value = !string.IsNullOrEmpty((range.Cells[row, columnTipoCaja + 4] as Excel.Range).Text) ? Convert.ToInt32((range.Cells[row, columnTipoCaja + 4] as Excel.Range).Text) : null;
                        fasecolda.Puertas.Value = !string.IsNullOrEmpty((range.Cells[row, columnTipoCaja + 5] as Excel.Range).Text)? Convert.ToByte((range.Cells[row, columnTipoCaja + 5] as Excel.Range).Text) : null;
                        fasecolda.AireAcondicionadoInd.Value = (range.Cells[row, columnTipoCaja + 6] as Excel.Range).Text == "1" ? true : false;
                        fasecolda.Descriptivo.Value = fasecolda.Clase.Value.TrimEnd() + "-" + fasecolda.Marca.Value.TrimEnd() + "-" + fasecolda.Tipo1.Value.TrimEnd() + "-" + fasecolda.Tipo2.Value.TrimEnd();
                        #endregion
                        this._fasecoldas.Add(fasecolda);
                    }                   
                }
                this.lblLeyenda.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GestionFaseColda.cs", "GetDataXLS"));
            }
        }

        #endregion

        #region Eventos Formulario
    
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

                if (loadData)
                {
                    this.Enabled = false;

                    this.pasteRet = CopyPasteExtension.PasteFromClipBoard(this.format);
                    this.btnImportar.Enabled = false;
                    this.btnInconsistencias.Enabled = false;

                    this.lblLeyenda.Text = "Validando datos....";

                    long count = this._bc.AdministrationModel.MasterSimple_Count(AppMasters.ccFasecolda, null, null, true);
                    this._fasecoldasExist = this._bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.ccFasecolda, count, 1, null, null, null).Cast<DTO_ccFasecolda>().ToList();

                    this.results = null;
                    this.result = null;
                    Thread process = new Thread(this.ImportThread);
                    process.Start();
                }



                //OpenFileDialog fDialog = new OpenFileDialog();
                //fDialog.Filter = "Excel (*.xls, *.xlsx)|*.xls;*.xlsx";
                //if (fDialog.ShowDialog() == DialogResult.OK)
                //{
                //    this.lblLeyenda.Text = "Validando datos....";

                //    long count = this._bc.AdministrationModel.MasterSimple_Count(AppMasters.ccFasecolda, null, null, true);
                //    this._fasecoldasExist = this._bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.ccFasecolda, count, 1, null, null, null).Cast<DTO_ccFasecolda>().ToList();

                //    this.lblLeyenda.Text = "Abriendo archivo....";

                //    var miss = Type.Missing;
                //    string path = Path.GetDirectoryName(fDialog.FileName);
                //    string filename = Path.GetFileNameWithoutExtension(fDialog.FileName);
                //    string ext = Path.GetExtension(fDialog.FileName);

                //    string filePath = path + "\\" + filename + ext;

                //    if (!File.Exists(filePath))
                //        return;

                //    Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                //    Microsoft.Office.Interop.Excel.Workbook xlWorkBook = xlApp.Workbooks.Open(filePath, miss, miss, miss, miss, miss, miss, miss, miss, miss, miss, miss, miss, miss, miss);

                //    // Seleccion de la hoja de calculo (get_item() devuelve object y numera las hojas a partir de 1)
                //    Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(2);
                //    // seleccion rango activo de la hoja
                //    this._range = xlWorkSheet.UsedRange;

                //    this.btnImportar.Enabled = false;
                //    this.btnInconsistencias.Enabled = false;

                //    Thread processRead = new Thread(this.ReadFileThread);
                //    processRead.Start();

                //    //this.GetDataXLS(this._range);                  

                //    //this.btnImportar.Enabled = false;
                //    //this.results = null;
                //    //this.result = null;

                //    //this.lblLeyenda.Text = "Guardando información....";
                //    //Thread process = new Thread(this.GuardarThread);
                //    //process.Start();
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GestionFaseColda.cs", "btnImportar_Click"));
                this.btnInconsistencias.Enabled = true;
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
                    var text = pasteRet.MsgResult;
                    string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    #region Variables de función y mensajes de error
                    this.result = new DTO_TxResult();
                    this.result.Result = ResultValue.OK;
                    this.result.Details = new List<DTO_TxResultDetail>();
                    //Lista con los dtos a subir y Fks a validas
                    this._fasecoldas = new List<DTO_ccFasecolda>();
                    this.modelos = new List<DTO_ccFasecoldaModelo>();
                    //Mensajes de error
                    //Popiedades de la incorporacion
                    DTO_ccFasecolda fasecolda = new DTO_ccFasecolda();
                    bool createDTO = true;
                    bool validList = true;
                    int lastYear = 0;
                    Dictionary<int, int> años = new Dictionary<int, int>();
                    #endregion                    
                    int percent = 0;
                    #region Llena información de las lineas
                    for (int i = 0; i < lines.Length; ++i)
                    {
                        string[] line = lines[i].Split(new string[] { CopyPasteExtension.tabChar }, StringSplitOptions.None);
                        #region Aumenta el porcentaje y revisa que tenga lineas para leer
                        this.Invoke(this.UpdateProgressDelegate, new object[] { percent });
                        percent = ((i + 1) * 100) / (lines.Length);

                        if (lines.Length == 1)
                        {
                            this.result.ResultMessage = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoCopyField); 
                            this.result.Result = ResultValue.NOK;
                            validList = false;
                        }
                        if (!validList)
                            break; 
                        #endregion
                        #region Valida la cantidad de modelos
                        if (i == 0)
                        {
                            TextChanged("Validando datos...");
                            for (int j = 10; j < line.Length; j++)//Recorre las columnas
                            {
                                //Convierte el año para agregarlo con el index, si hay error no es año
                                int año = 0;
                                try { año = Convert.ToInt32(line[j]); } catch (Exception) { año = 0; };
                                if (año != 0)
                                {
                                    lastYear = año;
                                    años.Add(j, año);
                                }
                            }
                        } 
                        #endregion
                        if (i > 0 && line.Length > 0)
                        {
                            createDTO = true;
                            #region Creacion de DTOs
                            if (createDTO)
                            {
                                string fasecoldaID = line[3];
                                TextChanged("Leyendo Fasecoldas...");
                                fasecolda = new DTO_ccFasecolda();
                                fasecolda.Marca.Value = line[1];
                                fasecolda.Clase.Value = line[2];
                                fasecolda.ID.Value = fasecoldaID;
                                fasecolda.Tipo1.Value = line[5];
                                fasecolda.Tipo2.Value = line[6];
                                fasecolda.Tipo3.Value = line[7];
                                //datos.Peso.Value = (range.Cells[row, 8] as Excel.Range;
                                fasecolda.Servicio.Value = Convert.ToByte(line[9]) ;
                                //datos.NombreServicio.Value = (range.Cells[row, 10] as Excel.Range;    

                                int columnTipoCaja = años.Last().Key + 4;
                                string tipoCaja = line[columnTipoCaja];
                                fasecolda.TipoCaja.Value = tipoCaja.TrimEnd().Equals("MT") ? (byte)1 : (tipoCaja.TrimEnd().Equals("AT") ? (byte)2 : (byte)3);
                                fasecolda.Cilindraje.Value = !string.IsNullOrEmpty(line[columnTipoCaja + 1]) ? Convert.ToInt32(line[columnTipoCaja + 1]) : 0;
                                fasecolda.Pasajeros.Value = !string.IsNullOrEmpty(line[columnTipoCaja + 3]) ? Convert.ToInt32(line[columnTipoCaja + 3]) : 0;
                                fasecolda.Carga.Value = !string.IsNullOrEmpty(line[columnTipoCaja + 4]) ? Convert.ToInt32(line[columnTipoCaja + 4]) : 0;
                                fasecolda.Puertas.Value = !string.IsNullOrEmpty(line[columnTipoCaja + 5]) ? Convert.ToByte(line[columnTipoCaja + 5]) : (byte)0;
                                fasecolda.AireAcondicionadoInd.Value = line[columnTipoCaja + 6] == "1" ? true : false;
                                fasecolda.Descriptivo.Value = fasecolda.Clase.Value.TrimEnd() + "-" + fasecolda.Marca.Value.TrimEnd() + "-" + fasecolda.Tipo1.Value.TrimEnd() + "-" + fasecolda.Tipo2.Value.TrimEnd();
                                fasecolda.EmpresaGrupoID.Value = this._fasecoldasExist.Count > 0 ? this._fasecoldasExist.First().EmpresaGrupoID.Value : this._bc.AdministrationModel.Empresa.ID.Value;
                                if (!this._fasecoldasExist.Exists(x=>x.ID.Value == fasecoldaID))
                                {
                                    this._fasecoldas.Add(fasecolda);
                                    this._fasecoldasExist.Add(fasecolda);
                                }
                                foreach (var año in años)
                                {
                                    decimal valor = Convert.ToDecimal(line[año.Key]); 
                                    if (valor > 0)
                                    {
                                        DTO_ccFasecoldaModelo mod = new DTO_ccFasecoldaModelo();
                                        mod.FasecoldaID.Value = fasecoldaID;
                                        mod.Modelo.Value = año.Value.ToString();
                                        mod.Valor.Value = valor;
                                        mod.EmpresaGrupoID.Value = this._fasecoldasExist.Find(x => x.ID.Value == fasecoldaID).EmpresaGrupoID.Value;
                                        this.modelos.Add(mod);
                                    }
                                }                               
                            }
                            #endregion                            
                        }
                    }
                    this.Invoke(this.UpdateProgressDelegate, new object[] { percent });
                    #endregion
                    if (validList)
                    {
                        #region Guarda datos
                        this.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCartera(this.documentID));
                        this.ProgressBarThread = new Thread(this.CheckServerProcessStatus);
                        this.ProgressBarThread.Start();
                       
                        byte[] faseColdaitems = CompressedSerializer.Compress<IEnumerable<DTO_ccFasecolda>>(this._fasecoldas);
                        byte[] faseColdaModitems = CompressedSerializer.Compress<IEnumerable<DTO_ccFasecoldaModelo>>(this.modelos);
                        TextChanged("Guardando información....");
                        this.result = _bc.AdministrationModel.ccFasecolda_Migracion(faseColdaitems, faseColdaModitems);

                        TextChanged("");

                        this.StopProgressBarThread();

                        if (this.result.Result == ResultValue.OK)
                        {
                            this._isOK = true;
                        }
                        else
                        {
                            this._isOK = false;
                            this._fasecoldas = new List<DTO_ccFasecolda>();
                        }
                        #endregion
                    }
                    else
                    {
                        #region Muestra mensajes de error
                        this._isOK = false;
                        this._fasecoldas = new List<DTO_ccFasecolda>();
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                this._fasecoldas = new List<DTO_ccFasecolda>();
                this._isOK = false;
                this.result.Result = ResultValue.NOK;
                this.result.ResultMessage = _bc.GetResourceForException(ex, "WinApp-GestionFaseColda.cs", "ImportThread");
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
        /// Hilo de Pago de nomina
        /// </summary>
        public void ReadFileThread()
        {
            try
            {
                this.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCartera(this.documentID));
                this.ProgressBarThread = new Thread(this.CheckServerProcessStatus);
                this.ProgressBarThread.Start();

                this._fasecoldas = new List<DTO_ccFasecolda>();
                this.modelos = new List<DTO_ccFasecoldaModelo>();

                int cols = this._range.Columns.Count;
                int rows = 500;// this._range.Rows.Count;
                Dictionary<int, int> años = new Dictionary<int, int>();

                //Agrega la cantidad de años segun archivo

                TextChanged("Validando datos...");
                for (int i = 60; i < cols; i++)//Recorre las columnas
                {
                    //Convierte el año para agregarlo con el index, si hay error no es año
                    int año = 0;
                    try { año = Convert.ToInt32((this._range.Cells[1, i] as Excel.Range).Text); } catch (Exception) { año = 0; break; };
                    if (año != 0)
                        años.Add(i, año);
                }

                //Recorre las filas del rango
                for (int row = 1; row <= rows; row++)
                {
                    //int percent = (row * 100) / rows;
                    //this.pbProcess.Position = percent;
                    //this.pbProcess.Update();                  

                    Tuple<int, int> tupProgress = new Tuple<int, int>(this._bc.AdministrationModel.User.ReplicaID.Value.Value, this.documentID);
                    DictionaryProgress.BatchProgress[tupProgress] = 1;
                    int countTotal = rows == 0 ? 1 : rows;
                    int percent = (row * 100) / countTotal;
                    DictionaryProgress.BatchProgress[tupProgress] = percent;

                    //int columna = 34;
                    //var a = ((range.Cells[row, columna] as Excel.Range).Text);
                    //var b = ((range.Cells[row, columna] as Excel.Range).Value);
                    //var c = ((range.Cells[row, columna] as Excel.Range).Value2);

                    //Valida que tipo de datos se asigna y desde la fila de datos de interes
                    if (row >= 2)
                    {
                        string fasecoldaID = (this._range.Cells[row, 4]).Text;
                        if(true) //(!this._fasecoldasExist.Exists(x => x.ID.Value == fasecoldaID))
                        {
                            TextChanged("Leyendo Fasecolda " + fasecoldaID + "...");
                            DTO_ccFasecolda fasecolda = new DTO_ccFasecolda();
                            #region FaseColda 
                            fasecolda.Marca.Value = (this._range.Cells[row, 2]).Text;
                            fasecolda.Clase.Value = (this._range.Cells[row, 3] ).Text;
                            fasecolda.ID.Value = fasecoldaID;
                            fasecolda.Tipo1.Value = (this._range.Cells[row, 6] ).Text;
                            fasecolda.Tipo2.Value = (this._range.Cells[row, 7]).Text;
                            fasecolda.Tipo3.Value = (this._range.Cells[row, 8] ).Text;
                            //datos.Peso.Value = (range.Cells[row, 9] as Excel.Range).Text;
                            fasecolda.Servicio.Value = !string.IsNullOrEmpty((this._range.Cells[row, 10]).Text) ? Convert.ToByte((this._range.Cells[row, 10]).Text) : null;
                            //datos.NombreServicio.Value = (range.Cells[row, 11] as Excel.Range).Text;    

                            int columnTipoCaja = años.Last().Key + 4;
                            string tipoCaja = (this._range.Cells[row, columnTipoCaja]).Text;
                            fasecolda.TipoCaja.Value = tipoCaja.TrimEnd().Equals("MT") ? (byte)1 : (tipoCaja.TrimEnd().Equals("AT") ? (byte)2 : (byte)3);///PREGUNTAR
                            fasecolda.Cilindraje.Value = !string.IsNullOrEmpty((this._range.Cells[row, columnTipoCaja + 1]).Text) ? Convert.ToInt32((this._range.Cells[row, columnTipoCaja + 1]).Text) : null;
                            fasecolda.Pasajeros.Value = !string.IsNullOrEmpty((this._range.Cells[row, columnTipoCaja + 3] ).Text) ? Convert.ToInt32((this._range.Cells[row, columnTipoCaja + 3] ).Text) : null;
                            fasecolda.Carga.Value = !string.IsNullOrEmpty((this._range.Cells[row, columnTipoCaja + 4] ).Text) ? Convert.ToInt32((this._range.Cells[row, columnTipoCaja + 4]).Text) : null;
                            fasecolda.Puertas.Value = !string.IsNullOrEmpty((this._range.Cells[row, columnTipoCaja + 5] ).Text) ? Convert.ToByte((this._range.Cells[row, columnTipoCaja + 5]).Text) : null;
                            fasecolda.AireAcondicionadoInd.Value = (this._range.Cells[row, columnTipoCaja + 6] ).Text == "1" ? true : false;
                            fasecolda.Descriptivo.Value = fasecolda.Clase.Value.TrimEnd() + "-" + fasecolda.Marca.Value.TrimEnd() + "-" + fasecolda.Tipo1.Value.TrimEnd() + "-" + fasecolda.Tipo2.Value.TrimEnd();
                            fasecolda.EmpresaGrupoID.Value = this._fasecoldasExist.Count > 0 ? this._fasecoldasExist.First().EmpresaGrupoID.Value : this._bc.AdministrationModel.Empresa.ID.Value;

                            TextChanged("Leyendo Modelos Fasecolda " + fasecoldaID + "...");
                            //fasecolda.Mod1970.Value = Convert.ToDecimal((this._range.Cells[row, 12]).Text);
                            //fasecolda.Mod1971.Value = Convert.ToDecimal((this._range.Cells[row, 13]).Text);
                            //fasecolda.Mod1972.Value = Convert.ToDecimal((this._range.Cells[row, 14]).Text);
                            //fasecolda.Mod1973.Value = Convert.ToDecimal((this._range.Cells[row, 15]).Text);
                            //fasecolda.Mod1974.Value = Convert.ToDecimal((this._range.Cells[row, 16]).Text);
                            //fasecolda.Mod1975.Value = Convert.ToDecimal((this._range.Cells[row, 17]).Text);
                            //fasecolda.Mod1976.Value = Convert.ToDecimal((this._range.Cells[row, 18]).Text);
                            //fasecolda.Mod1977.Value = Convert.ToDecimal((this._range.Cells[row, 19]).Text);
                            //fasecolda.Mod1978.Value = Convert.ToDecimal((this._range.Cells[row, 20]).Text);
                            //fasecolda.Mod1979.Value = Convert.ToDecimal((this._range.Cells[row, 21]).Text);
                            //fasecolda.Mod1980.Value = Convert.ToDecimal((this._range.Cells[row, 22]).Text);
                            //fasecolda.Mod1981.Value = Convert.ToDecimal((this._range.Cells[row, 23]).Text);
                            //fasecolda.Mod1982.Value = Convert.ToDecimal((this._range.Cells[row, 24]).Text);
                            //fasecolda.Mod1983.Value = Convert.ToDecimal((this._range.Cells[row, 25]).Text);
                            //fasecolda.Mod1984.Value = Convert.ToDecimal((this._range.Cells[row, 26]).Text);
                            //fasecolda.Mod1985.Value = Convert.ToDecimal((this._range.Cells[row, 27]).Text);
                            //fasecolda.Mod1986.Value = Convert.ToDecimal((this._range.Cells[row, 28]).Text);
                            //fasecolda.Mod1987.Value = Convert.ToDecimal((this._range.Cells[row, 29]).Text);
                            //fasecolda.Mod1988.Value = Convert.ToDecimal((this._range.Cells[row, 30]).Text);
                            //fasecolda.Mod1989.Value = Convert.ToDecimal((this._range.Cells[row, 31]).Text);
                            //fasecolda.Mod1990.Value = Convert.ToDecimal((this._range.Cells[row, 32]).Text);
                            //fasecolda.Mod1991.Value = Convert.ToDecimal((this._range.Cells[row, 33]).Text);
                            //fasecolda.Mod1992.Value = Convert.ToDecimal((this._range.Cells[row, 34]).Text);
                            //fasecolda.Mod1993.Value = Convert.ToDecimal((this._range.Cells[row, 35]).Text);
                            //fasecolda.Mod1994.Value = Convert.ToDecimal((this._range.Cells[row, 36]).Text);
                            //fasecolda.Mod1995.Value = Convert.ToDecimal((this._range.Cells[row, 37]).Text);
                            //fasecolda.Mod1996.Value = Convert.ToDecimal((this._range.Cells[row, 38]).Text);
                            //fasecolda.Mod1997.Value = Convert.ToDecimal((this._range.Cells[row, 39]).Text);
                            //fasecolda.Mod1998.Value = Convert.ToDecimal((this._range.Cells[row, 40]).Text);
                            //fasecolda.Mod1999.Value = Convert.ToDecimal((this._range.Cells[row, 41]).Text);
                            //fasecolda.Mod2000.Value = Convert.ToDecimal((this._range.Cells[row, 42]).Text);
                            //fasecolda.Mod2001.Value = Convert.ToDecimal((this._range.Cells[row, 43]).Text);
                            //fasecolda.Mod2002.Value = Convert.ToDecimal((this._range.Cells[row, 44]).Text);
                            //fasecolda.Mod2003.Value = Convert.ToDecimal((this._range.Cells[row, 45]).Text);
                            //fasecolda.Mod2004.Value = Convert.ToDecimal((this._range.Cells[row, 46]).Text);
                            //fasecolda.Mod2005.Value = Convert.ToDecimal((this._range.Cells[row, 47]).Text);
                            //fasecolda.Mod2006.Value = Convert.ToDecimal((this._range.Cells[row, 48]).Text);
                            //fasecolda.Mod2007.Value = Convert.ToDecimal((this._range.Cells[row, 49]).Text);
                            //fasecolda.Mod2008.Value = Convert.ToDecimal((this._range.Cells[row, 50]).Text);
                            //fasecolda.Mod2009.Value = Convert.ToDecimal((this._range.Cells[row, 51]).Text);
                            //fasecolda.Mod2010.Value = Convert.ToDecimal((this._range.Cells[row, 52]).Text);
                            //fasecolda.Mod2011.Value = Convert.ToDecimal((this._range.Cells[row, 53]).Text);
                            //fasecolda.Mod2012.Value = Convert.ToDecimal((this._range.Cells[row, 54]).Text);
                            //fasecolda.Mod2013.Value = Convert.ToDecimal((this._range.Cells[row, 55]).Text);
                            //fasecolda.Mod2014.Value = Convert.ToDecimal((this._range.Cells[row, 56]).Text);
                            //fasecolda.Mod2015.Value = Convert.ToDecimal((this._range.Cells[row, 57]).Text);
                            //fasecolda.Mod2016.Value = Convert.ToDecimal((this._range.Cells[row, 58]).Text);
                            //fasecolda.Mod2017.Value = Convert.ToDecimal((this._range.Cells[row, 59]).Text);
                            //fasecolda.Mod2018.Value = Convert.ToDecimal((this._range.Cells[row, 60]).Text);
                            this._fasecoldas.Add(fasecolda);
                            //this._fasecoldasExist.Add(fasecolda);
                            #endregion
                        }
                        //TextChanged("Leyendo Modelos Fasecolda " + fasecoldaID + "...");
                        //foreach (var año in años)
                        //{
                        //    decimal valor = !string.IsNullOrEmpty((this._range.Cells[row, año.Key]).Text) ? Convert.ToDecimal((this._range.Cells[row, año.Key]).Text) : 0;
                        //    if (valor > 0)
                        //    {
                        //        DTO_ccFasecoldaModelo mod = new DTO_ccFasecoldaModelo();
                        //        mod.FasecoldaID.Value = fasecoldaID;
                        //        mod.Modelo.Value = año.Value.ToString();
                        //        mod.Valor.Value = valor;
                        //        mod.EmpresaGrupoID.Value = this._fasecoldasExist.Find(x => x.ID.Value == fasecoldaID).EmpresaGrupoID.Value;
                        //        this.modelos.Add(mod);
                        //    }
                        //}
                    }
                }
                this.results = null;
                this.result = null;

                TextChanged("Guardando información....");
                byte[] faseColdaitems = CompressedSerializer.Compress<IEnumerable<DTO_ccFasecolda>>(this._fasecoldas);
                byte[] faseColdaModitems = CompressedSerializer.Compress<IEnumerable<DTO_ccFasecoldaModelo>>(this.modelos);
                this.result = _bc.AdministrationModel.ccFasecolda_Migracion(faseColdaitems, faseColdaModitems); 

                this.StopProgressBarThread();
                this._isOK = this.result.Result == ResultValue.OK ? true : false;

                this.StopProgressBarThread();
            }
            catch (Exception ex)
            {
                this.StopProgressBarThread();
                this.result.ResultMessage = _bc.GetResourceForException(ex, "WinApp-GestionFaseColda.cs", "ReadFileThread");
            }
            finally
            {
                this.Invoke(this.endGuardarDelegate);
            }
        }

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

                //this.result = _bc.AdministrationModel.DatacreditoGestion_Add(this.documentID,this._fasecoldas,this.score,this.ubica,this.quanto,true);

                this.StopProgressBarThread();

                this._isOK = this.result.Result == ResultValue.OK ? true : false;
            }
            catch (Exception ex)
            {
                this.StopProgressBarThread();

                this._isOK = false;
                this.result = new DTO_TxResult();
                this.result.Result = ResultValue.NOK;
                this.result.ResultMessage = _bc.GetResourceForException(ex, "WinApp-GestionFaseColda.cs", "GuardarThread");
            }
            finally
            {
                this.Invoke(this.endGuardarDelegate);
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
                _bc.GetResourceForException(ex, "WinApp-GestionFaseColda.cs", "InconsistenciasThread");
            }
            finally
            {
                this.Invoke(this.endInconsistenciasDelegate);
            }
        }

        #endregion
   
    }
}
