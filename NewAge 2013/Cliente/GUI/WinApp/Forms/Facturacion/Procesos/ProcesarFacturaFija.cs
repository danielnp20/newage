using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Librerias.Project;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using System.Threading;
using NewAge.DTO.Resultados;
using System.Reflection;
using NewAge.DTO.Attributes;
using NewAge.DTO.UDT;
using SentenceTransformer;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ProcesarFacturaFija : ProcessForm
    {
        #region Delegados

        /// <summary>
        /// Delegado que finaliza el proceso de generacion de preliminares
        /// </summary>
        public delegate void EndImportar();
        public EndImportar endImportarDelegate;
        public void EndImportarMethod()
        {            
            if (this._isOK)
                this.btnGenerar.Enabled = true;
            else
                this.btnGenerar.Enabled = false;
        }

        /// <summary>
        /// Delegado que finaliza el proceso de aprobar los comprobantes del cierre
        /// </summary>
        public delegate void EndProcesar();
        public EndProcesar endProcesarDelegate;
        public void EndProcesarMethod()
        {
            if (this._isOK)
                this.btnGenerar.Enabled = false;
            else
                this.btnGenerar.Enabled = true;
        }

        #endregion

        #region Variables

        BaseController _bc = BaseController.GetInstance();
        //Variables para proceso
        private int documentMigracionID;
        private string areaFuncionalID;
        private PasteOpDTO pasteRet;
        //Variables para importar
        private int colsCount;
        private string firstColSuppl;
        private string format;
        private string formatSeparator = "\t";
        //Variables de monedas
        private bool isMultiMoneda;
        private string monedaLocal;
        private string monedaExtranjera;
        private decimal tasaCierre;
        private bool hasTasaCierre;
        private decimal valorTotal = 0;
        private string _unboundPrefix = "Unbound_";
        private DTO_QueryDetailFactura _detalle = null;
        private List<DTO_QueryHeadFactura> _data = null; 
        //Variables del formulario
        private bool _isOK;
        private List<DTO_glDocumentoControl> _ctrlList;
        private List<DTO_faFacturaDocu> _facturaDocuList;
        //Variables con valores x defecto (glControl)
        protected string defTercero = string.Empty;
        protected string defPrefijo = string.Empty;
        protected string defProyecto = string.Empty;
        protected string defCentroCosto = string.Empty;
        protected string defLineaPresupuesto = string.Empty;
        protected string defConceptoCargo = string.Empty;
        protected string defLugarGeo = string.Empty;
        protected string defConceptoCxP = string.Empty;

        #endregion

        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        public ProcesarFacturaFija() 
        {
            //this.InitializeComponent();
        }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables del formulario
        /// </summary>
        protected override void InitForm()
        {
            try
            {
                this.documentID = AppProcess.ProcesaFacturaFija;              

                InitializeComponent();
              
                //Inicializa los delegados
                this.endImportarDelegate = new EndImportar(EndImportarMethod);
                this.endProcesarDelegate = new EndProcesar(EndProcesarMethod);

                //Inicializa controles
                this._bc.InitPeriodUC(this.dtPeriod, 0);
                this._bc.InitMasterUC(this.masterDocumento, AppMasters.glDocumento, true, true, true, false);

                this._isOK = false;
                this.masterDocumento.EnableControl(false);
                this.documentMigracionID = AppDocuments.FacturaVenta;
                this.masterDocumento.Value = this.documentMigracionID.ToString();
                this.AddGridCols();
                FormProvider.LoadResources(this, this.documentID);

                //Inicia las variables
                string periodo = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_Periodo);
                this.dtPeriod.DateTime = !string.IsNullOrEmpty(periodo) ? Convert.ToDateTime(periodo) : DateTime.Now;
                //this.LoadGridData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-ProcesarFacturaFija.cs", "InitForm"));
            }
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Inicia las columnas de las grillas
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                #region Columnas de grilla principal

                // PrefDoc
                GridColumn PrefDoc = new GridColumn();
                PrefDoc.FieldName = this._unboundPrefix + "PrefDoc";
                PrefDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PrefDoc");
                PrefDoc.UnboundType = UnboundColumnType.String;
                PrefDoc.VisibleIndex = 0;
                PrefDoc.Width = 90;
                PrefDoc.Visible = true;
                PrefDoc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(PrefDoc);

                //TerceroID
                GridColumn TerceroID = new GridColumn();
                TerceroID.FieldName = this._unboundPrefix + "TerceroID";
                TerceroID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TerceroID");
                TerceroID.UnboundType = UnboundColumnType.String;
                TerceroID.VisibleIndex = 1;
                TerceroID.Width = 90;
                TerceroID.Visible = true;
                TerceroID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(TerceroID);

                //Nombre
                GridColumn Nombre = new GridColumn();
                Nombre.FieldName = this._unboundPrefix + "Nombre";
                Nombre.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Nombre");
                Nombre.UnboundType = UnboundColumnType.String;
                Nombre.VisibleIndex = 2;
                Nombre.Width = 100;
                Nombre.Visible = true;
                Nombre.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Nombre);

                // Valor
                GridColumn Valor = new GridColumn();
                Valor.FieldName = this._unboundPrefix + "Valor";
                Valor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Valor");
                Valor.UnboundType = UnboundColumnType.Decimal;
                Valor.VisibleIndex = 8;
                Valor.Width = 100;
                Valor.Visible = true;
                Valor.ColumnEdit = this.TextEdit;
                Valor.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Valor);

                //Observacion
                GridColumn Observacion = new GridColumn();
                Observacion.FieldName = this._unboundPrefix + "Observacion";
                Observacion.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Observacion");
                Observacion.UnboundType = UnboundColumnType.String;
                Observacion.VisibleIndex = 9;
                Observacion.Width = 100;
                Observacion.Visible = true;
                Observacion.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Observacion);

                #endregion                
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-", "ConsultaDocumentosFact.cs-AddGridCols"));
            }
        }

        /// <summary>
        /// Inicializa las variables
        /// </summary>
        private void LoadGridData()
        {
            try
            {
                this._data = this._bc.AdministrationModel.ConsultarFacturasXNota(this.dtPeriod.DateTime, string.Empty, 1, string.Empty, string.Empty, string.Empty, 3, string.Empty, string.Empty, true);
                foreach (var d in this._data)
                    d.Detalle = new List<DTO_QueryDetailFactura>();

                this.gcDocument.DataSource = this._data;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-ProcesarFacturaFija.cs", "LoadGridData"));
            }
        }
        #endregion

        #region Eventos
        /// <summary>
        /// Boton de cierre
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnGenerarDocumentos_Click(object sender, EventArgs e)
        {
            try
            {               
                    this.btnGenerar.Enabled = false;

                    Thread process = new Thread(this.ProcesarThread);
                    process.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-ProcesarFacturaFija.cs", "btnGenerarDocumentos_Click"));
            }
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
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
            if (e.IsSetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (e.Value == null)
                    e.Value = String.Empty;
                if (pi != null)
                {
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                    {
                        e.Value = pi.GetValue(dto, null);
                    }
                    else if (pi.PropertyType.Name == "UDTSQL_smalldatetime")
                    {
                        UDT udtProp = (UDT)pi.GetValue(dto, null);
                        udtProp.SetValueFromString(Convert.ToDateTime(e.Value).ToShortDateString());
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

        /// <summary>
        /// Al cambiar el valor del periodo
        /// </summary>
        private void dtPeriod_ValueChanged()
        {
            this.LoadGridData();
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo de Procesar Cierre
        /// </summary>
        private void ProcesarThread()
        {
            try
            {
                this.FuncProgressBarThread = () => (this._bc.AdministrationModel.ConsultarProgresoCont(this.documentID));
                this.ProgressBarThread = new Thread(this.CheckServerProcessStatus);
                this.ProgressBarThread.Start();

                DTO_TxResult result = null;// this._bc.AdministrationModel.CuentasXCobrar_Migracion(Convert.ToInt32(this.documentMigracionID), this._actFlujo.ID.Value, this.masterConcSaldo.Value, this._ctrlList, this._facturaDocuList);
                this.StopProgressBarThread();

                this._isOK = true;
                if (result.Result == ResultValue.NOK)
                    this._isOK = false;

                MessageForm frm = new MessageForm(result);
                this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });
                this.Invoke(this.endProcesarDelegate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-ProcesarFacturaFija.cs", "btnProcesar_Click"));
                this.StopProgressBarThread();
            }
        }

        #endregion



    }
}
