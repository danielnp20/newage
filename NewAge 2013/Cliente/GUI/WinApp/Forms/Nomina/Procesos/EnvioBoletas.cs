using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using System.Reflection;
using NewAge.DTO.UDT;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using System.Threading;
using NewAge.DTO.Resultados;
using DevExpress.XtraEditors;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class EnvioBoletas : DocumentNominaBaseForm
    {
        public EnvioBoletas()
        {
            //this.InitializeComponent();
        }
        #region Variables

        private BaseController _bc = BaseController.GetInstance();
        private List<DTO_NominaEnvioBoleta> _empleados = new List<DTO_NominaEnvioBoleta>();
        private string _ultimaNominaLiq = string.Empty;
        private bool quincenal = false;
        #endregion

        #region Delegados

        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected override void SaveMethod()
        {
            this.RefreshDocument();
        }
       
        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Inicializa los parametros
        /// </summary>
        protected override void SetInitParameters()
        {
            InitializeComponent();
            this.frmModule = ModulesPrefix.no;
            this.documentID = AppProcess.EnvioBoletas;
            base.SetInitParameters();
            this.AddGridCols();
            this.InitControls();
            this.LoadData(false);
        }

        /// <summary>
        /// Columnas Grillas
        /// </summary>
        protected override void AddGridCols()
        {
            try
            {
                #region Documentos

                //Campo de marca
                GridColumn marca = new GridColumn();
                marca.FieldName = this.unboundPrefix + "Seleccionar";
                marca.UnboundType = UnboundColumnType.Boolean;
                marca.VisibleIndex = 0;
                marca.Width = 40;
                marca.Visible = true;
                marca.Fixed = FixedStyle.Left;
                marca.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                marca.OptionsColumn.ShowCaption = false;
                this.gvDocument.Columns.Add(marca);

                GridColumn TerceroID = new GridColumn();
                TerceroID.FieldName = this.unboundPrefix + "TerceroID";
                TerceroID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TerceroID");
                TerceroID.UnboundType = UnboundColumnType.String;
                TerceroID.VisibleIndex = 1;
                TerceroID.Width = 100;
                TerceroID.Visible = true;
                TerceroID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(TerceroID);

                GridColumn Nombre = new GridColumn();
                Nombre.FieldName = this.unboundPrefix + "Nombre";
                Nombre.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Nombre");
                Nombre.UnboundType = UnboundColumnType.String;
                Nombre.VisibleIndex = 2;
                Nombre.Width = 150;
                Nombre.Visible = true;
                Nombre.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Nombre);

                GridColumn Correo = new GridColumn();
                Correo.FieldName = this.unboundPrefix + "CorreoElectronico";
                Correo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CorreoElectronico");
                Correo.UnboundType = UnboundColumnType.String;
                Correo.VisibleIndex = 3;
                Correo.Width = 150;
                Correo.Visible = true;
                Correo.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Correo);

                GridColumn valor = new GridColumn();
                valor.FieldName = this.unboundPrefix + "Valor";
                valor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Valor");
                valor.UnboundType = UnboundColumnType.Decimal;
                valor.VisibleIndex = 4;
                valor.Width = 400;
                valor.Visible = true;
                valor.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(valor);
               
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ContabilizacionNomina.cs", "AddGridCols"));
            }

        }

        /// <summary>
        /// Carga la informacion de las liquidaciones
        /// </summary>
        /// <param name="firstTime">si es en la primera carga</param>
        protected override void LoadData(bool firstTime)
        {
            this._empleados = this._bc.AdministrationModel.Proceso_noLiquidacionesDocu_GetNominaLiquida(documentID, this.dtFecha.DateTime,Convert.ToByte(this.cmbTipoBoleta.EditValue));
            this.gcDocument.DataSource = this._empleados;
            this.gcDocument.RefreshDataSource();
        }

        #endregion

        #region Funciones Privadas

        public void RefreshDocument()
        {
            this.LoadData(true);           
        }

        /// <summary>
        /// Carga la Informacion del Combo de Documentos
        /// </summary>
        protected void InitControls()
        {
            Dictionary<string, string> tipos = new Dictionary<string, string>();
            tipos.Add("1", "Nómina");
            tipos.Add("2", "Prima");
            tipos.Add("3", "Vacaciones");
            tipos.Add("4", "Liquidación");
            tipos.Add("5", "Cesantias");
            this.cmbTipoBoleta.Properties.ValueMember = "Key";
            this.cmbTipoBoleta.Properties.DisplayMember = "Value";
            this.cmbTipoBoleta.Properties.DataSource = tipos;
            this.cmbTipoBoleta.EditValue = "1";
            this.quincenal = this._bc.GetControlValueByCompanyAllowEmpty(ModulesPrefix.no, AppControl.no_LiquidaNominaQuincenal).Equals("1") ? true : false;
            this._ultimaNominaLiq = this._bc.GetControlValueByCompanyAllowEmpty(ModulesPrefix.no, AppControl.no_UltimaNominaLiquidada);

            if (!string.IsNullOrEmpty(this._ultimaNominaLiq))
                this.dtFecha.DateTime = Convert.ToDateTime(this._ultimaNominaLiq);
            else
            {
                if (this.quincenal)
                    this.dtFecha.DateTime = new DateTime(this.dtFecha.DateTime.Year,this.dtFecha.DateTime.Month, 15);
                else
                    this.dtFecha.DateTime = new DateTime(this.dtFecha.DateTime.Year, this.dtFecha.DateTime.Month, DateTime.DaysInMonth(this.dtFecha.DateTime.Year, this.dtFecha.DateTime.Month));

            }
               
        }

        #endregion

        #region Eventos 

        /// <summary>
        /// Selecciona todos los items de la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (var liq in this._empleados)
                    liq.Seleccionar.Value = ((CheckBox)sender).Checked;
                this.gcDocument.RefreshDataSource();
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }
        #endregion

        #region Eventos Grillas

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void gvDocument_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

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
                    else if (pi.PropertyType.Name == "UDTSQL_smalldatetime" || pi.PropertyType.Name == "UDTSQL_datetime")
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
        /// Aplica formato a los campos en la grilla de documentos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDocument_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "Valor")
            {
                e.RepositoryItem = this.editValue;
            }
        }

        #endregion

        #region Eventos MDI

        protected override void Form_Enter(object sender, EventArgs e)
        {
            base.Form_Enter(sender, e);

            FormProvider.Master.tbBreak.Visible = false;
            FormProvider.Master.itemFilter.Visible = false;
            FormProvider.Master.itemFilterDef.Visible = false;
            FormProvider.Master.tbBreak0.Visible = false;
            FormProvider.Master.itemGenerateTemplate.Visible = false;
            FormProvider.Master.itemCopy.Visible = false;
            FormProvider.Master.itemPaste.Visible = false;
            FormProvider.Master.itemExport.Visible = false;
            FormProvider.Master.tbBreak1.Visible = false;
            FormProvider.Master.itemImport.Visible = false;
            FormProvider.Master.itemRevert.Visible = false;
            FormProvider.Master.itemUpdate.Visible = true;
            FormProvider.Master.itemDelete.Visible = false;
            FormProvider.Master.itemPrint.Visible = false;
            FormProvider.Master.itemUpdate.Enabled = true;

            if (FormProvider.Master.LoadFormTB)
                FormProvider.Master.itemSendtoAppr.Enabled = true;
            
        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para actualizar la lista de documentos
        /// </summary>
        public override void TBSendtoAppr()
        {
            this.gvDocument.PostEditor();
            Thread process = new Thread(this.SendToApproveThread);
            process.Start();
        }

        /// <summary>
        /// Boton para actualizar la lista de documentos
        /// </summary>
        public override void TBUpdate()
        {
            this.LoadData(false);
        }


        #endregion

        #region Hilos

        /// <summary>
        /// Hilo de salvar
        /// </summary>
        public override void SendToApproveThread()
        {
            try
            {
               
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { AppDocuments.Nomina, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoNomina(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                DTO_TxResult result = new DTO_TxResult();
                #region Variables para el mail         
               
                string filesPath = this._bc.GetControlValue(AppControl.RutaFisicaArchivos);
                string fileFormat = this._bc.GetControlValue(AppControl.NombreArchivoDocumentos);
                string docsPath = this._bc.GetControlValue(AppControl.RutaDocumentos);
                string subject = "Boleta de Pago " + this.cmbTipoBoleta.Text;
                string quincena = this.quincenal ? (this.dtFecha.DateTime.Day <= 15 ? " Quincena 1 " : " Quincena 2 ") : string.Empty;
                string body = "Se adjunta Boleta de Pago " + this.cmbTipoBoleta.Text + " del Mes " + this.dtFecha.DateTime.ToString("MMMM") + quincena;
                string ext = ".pdf";
                #endregion
                foreach (var emp in this._empleados)
                {
                    if (emp.Seleccionar.Value.Value)
                    {
                        #region Envia el correo con archivos adjuntos
                        List<string> attachedFiles = new List<string>();
                        string repName = string.Format(fileFormat, emp.NumeroDoc.Value.ToString()) + ext;
                        string rutaArchivo = filesPath + docsPath + repName;                     
                        attachedFiles.Add(rutaArchivo);                                             
                        this._bc.SendMail(this.documentID, subject, body, emp.CorreoElectronico.Value,1, attachedFiles);
                        #endregion
                    } 
                }
                MessageForm frm = new MessageForm(result);
                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
              
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EnvioBoletas.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
                this.Invoke(this.saveDelegate);
            }
        }

        #endregion

    }
}
