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
using System.Diagnostics;
using System.Net;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using System.Reflection;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using NewAge.DTO.Resultados;
using System.Threading;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class QueryGestionCobranza : FormWithToolbar
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();

        //Para manejo de propiedades
        private FormTypes _frmType = FormTypes.DocumentAprob;

        private string _frmName;
        private int _documentID;
        private ModulesPrefix _frmModule;
        private string _unboundPrefix = "Unbound_";

        //DTOs        
        private List<DTO_QueryGestionCobranza> cobranzas = new List<DTO_QueryGestionCobranza>();
        private DTO_QueryGestionCobranza cobranza = new DTO_QueryGestionCobranza();

        //Variables formulario
        private bool validate = true;
        private string clienteID = string.Empty;


        #endregion

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public QueryGestionCobranza()
        {
            this.Constructor();
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public QueryGestionCobranza(string mod)
        {
            this.Constructor(mod);
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public void Constructor(string mod = null)
        {
            this.InitializeComponent();
            try
            {
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());
                if (!string.IsNullOrWhiteSpace(mod))
                    this._frmModule = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), mod);

                FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
                this.AddGridCols();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryGestionCobranza.cs", "QueryGestionCobranza.cs-QueryGestionCobranza"));
            }
        }     
     
        #region Funciones Privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            this._documentID = AppQueries.QueryGestionCobranza;
            this._frmModule = ModulesPrefix.cc;

            //Carga los combos de la fecha
            this.dtFechaInicial.EditValue = DateTime.Now;
            this.dtFechaCorte.EditValue = DateTime.Now;
            this._bc.InitMasterUC(this.masterEtapa, AppMasters.glIncumplimientoEtapa, true, true, true, false);

        }

        /// <summary>
        /// Agrega las columnas a las grillas
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                #region Grilla Principal
                //Libranza
                GridColumn libranza = new GridColumn();
                libranza.FieldName = this._unboundPrefix + "Libranza";
                libranza.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Libranza");
                libranza.UnboundType = UnboundColumnType.String;
                libranza.VisibleIndex = 1;
                libranza.Width = 80;
                libranza.Visible = true;
                libranza.OptionsColumn.AllowEdit = false;
                this.gvCobranzas.Columns.Add(libranza);

                //ClienteID
                GridColumn ClienteID = new GridColumn();
                ClienteID.FieldName = this._unboundPrefix + "ClienteID";
                ClienteID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ClienteID");
                ClienteID.UnboundType = UnboundColumnType.String;
                ClienteID.VisibleIndex = 2;
                ClienteID.Width = 80;
                ClienteID.Visible = true;
                ClienteID.OptionsColumn.AllowEdit = false;
                this.gvCobranzas.Columns.Add(ClienteID);

                //Nombre
                GridColumn Nombre = new GridColumn();
                Nombre.FieldName = this._unboundPrefix + "Nombre";
                Nombre.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Nombre");
                Nombre.UnboundType = UnboundColumnType.String;
                Nombre.VisibleIndex = 3;
                Nombre.Width = 180;
                Nombre.Visible = true;
                Nombre.OptionsColumn.AllowEdit = false;
                this.gvCobranzas.Columns.Add(Nombre);

                //TipoEstado
                GridColumn TipoEstado = new GridColumn();
                TipoEstado.FieldName = this._unboundPrefix + "TipoEstado";
                TipoEstado.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_TipoEstado");
                TipoEstado.UnboundType = UnboundColumnType.String;
                TipoEstado.VisibleIndex = 4;
                TipoEstado.Width = 60;
                TipoEstado.Visible = true;
                TipoEstado.OptionsColumn.AllowEdit = false;
                this.gvCobranzas.Columns.Add(TipoEstado);

                //CobranzaCierre
                GridColumn CobranzaCierre = new GridColumn();
                CobranzaCierre.FieldName = this._unboundPrefix + "CobranzaGestionCierre";
                CobranzaCierre.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CobranzaGestionCierre");
                CobranzaCierre.UnboundType = UnboundColumnType.String;
                CobranzaCierre.VisibleIndex = 5;
                CobranzaCierre.Width = 60;
                CobranzaCierre.Visible = true;
                CobranzaCierre.OptionsColumn.AllowEdit = false;
                this.gvCobranzas.Columns.Add(CobranzaCierre);

                //CuotaID
                GridColumn cuotaID = new GridColumn();
                cuotaID.FieldName = this._unboundPrefix + "CuotaID";
                cuotaID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CuotaID");
                cuotaID.UnboundType = UnboundColumnType.Integer;
                cuotaID.VisibleIndex = 6;
                cuotaID.Width = 60;
                cuotaID.Visible = true;
                cuotaID.OptionsColumn.AllowEdit = false;
                this.gvCobranzas.Columns.Add(cuotaID);

                //Fecha Cuota
                GridColumn fechaCuota = new GridColumn();
                fechaCuota.FieldName = this._unboundPrefix + "FechaCuota";
                fechaCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaCuota");
                fechaCuota.UnboundType = UnboundColumnType.DateTime;
                fechaCuota.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                fechaCuota.AppearanceCell.Options.UseTextOptions = true;
                fechaCuota.VisibleIndex = 7;
                fechaCuota.Width = 100;
                fechaCuota.Visible = true;
                fechaCuota.OptionsColumn.AllowEdit = false;
                this.gvCobranzas.Columns.Add(fechaCuota);

                //FechaAct
                GridColumn FechaAct = new GridColumn();
                FechaAct.FieldName = this._unboundPrefix + "FechaAct";
                FechaAct.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaAct");
                FechaAct.UnboundType = UnboundColumnType.DateTime;
                FechaAct.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                FechaAct.AppearanceCell.Options.UseTextOptions = true;
                FechaAct.VisibleIndex = 8;
                FechaAct.Width = 100;
                FechaAct.Visible = true;
                FechaAct.OptionsColumn.AllowEdit = false;
                this.gvCobranzas.Columns.Add(FechaAct);

                //VlrSaldoCuota
                GridColumn vlrSaldo = new GridColumn();
                vlrSaldo.FieldName = this._unboundPrefix + "VlrSaldoCuota";
                vlrSaldo.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrSaldoCuota");
                vlrSaldo.UnboundType = UnboundColumnType.Decimal;
                vlrSaldo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrSaldo.AppearanceCell.Options.UseTextOptions = true;
                vlrSaldo.VisibleIndex = 9;
                vlrSaldo.Width = 150;
                vlrSaldo.Visible = true;
                vlrSaldo.ColumnEdit = this.editValue;
                vlrSaldo.OptionsColumn.AllowEdit = false;
                this.gvCobranzas.Columns.Add(vlrSaldo);

                //EtapaID
                GridColumn EtapaID = new GridColumn();
                EtapaID.FieldName = this._unboundPrefix + "EtapaID";
                EtapaID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_EtapaID");
                EtapaID.UnboundType = UnboundColumnType.String;
                EtapaID.VisibleIndex = 10;
                EtapaID.Width = 100;
                EtapaID.Visible = true;
                EtapaID.OptionsColumn.AllowEdit = false;
                this.gvCobranzas.Columns.Add(EtapaID);

                //EtapaDesc
                GridColumn EtapaDesc = new GridColumn();
                EtapaDesc.FieldName = this._unboundPrefix + "EtapaDesc";
                EtapaDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_EtapaDesc");
                EtapaDesc.UnboundType = UnboundColumnType.String;
                EtapaDesc.VisibleIndex = 11;
                EtapaDesc.Width = 180;
                EtapaDesc.Visible = true;
                EtapaDesc.OptionsColumn.AllowEdit = false;
                this.gvCobranzas.Columns.Add(EtapaDesc);

                //Ver
                GridColumn ver = new GridColumn();
                ver.FieldName = this._unboundPrefix + "FileUrl";
                ver.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FileUrl");
                ver.UnboundType = UnboundColumnType.String;
                ver.OptionsColumn.ShowCaption = false;
                ver.VisibleIndex = 12;
                ver.Width = 80;
                ver.ColumnEdit = this.linkEditViewFile;
                this.gvCobranzas.Columns.Add(ver);
                this.gvCobranzas.OptionsView.ColumnAutoWidth = true;
   
                #endregion                
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryGestionCobranzas.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Refresca el contenido del formulario
        /// </summary>
        private void CleanData()
        {
            this.validate = false;
            this.dtFechaInicial.EditValue = DateTime.Now;
            this.dtFechaCorte.EditValue = DateTime.Now;
            this.cobranza = new DTO_QueryGestionCobranza();
            this.cobranzas = new List<DTO_QueryGestionCobranza>();
            this.gcCobranzas.DataSource = this.cobranzas;
            this.clienteID = string.Empty;
            this.validate = true;
        }

        /// <summary>
        /// Funcion que realiza la opreacion de busqueda
        /// </summary>
        private void GetSearch()
        {
            try
            {
                if (this.masterEtapa.ValidID)
                {
                    this.cobranzas = this._bc.AdministrationModel.GestionCobranza_GetActividades(this.dtFechaCorte.DateTime, this.dtFechaCorte.DateTime, this.masterEtapa.Value, string.Empty);

                    if (this.cobranzas != null && this.cobranzas.Count > 0)
                    {
                        #region Carga la informacion de la grilla
                        this.cobranzas = this.cobranzas.OrderBy(x => x.Nombre.Value).ToList();
                        this.gcCobranzas.DataSource = this.cobranzas;
                        this.gcCobranzas.RefreshDataSource();
                        this.gvCobranzas.BestFitColumns();
                        this.gvCuotas.BestFitColumns();
                        this.gvCobranzas.MoveFirst();
                        #endregion
                    }
                    else
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
                        this.CleanData();
                    }     
                }          
            }
            catch (Exception ex)
            {                
               MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryGestionCobranza.cs", "GetSearch"));
            }
        }

        #endregion Funciones Privadas

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Enter(this, this._documentID, this._frmType, this._frmModule);
                FormProvider.Master.itemSearch.Visible = true;
                FormProvider.Master.itemSearch.Enabled = true;
                FormProvider.Master.itemSave.Visible = false;
                FormProvider.Master.itemExport.Visible = true;
                FormProvider.Master.itemExport.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryGestionCobranza.cs", "Form_Enter"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Leave(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Closing(this, this._documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryGestionCobranza.cs", "Form_Closing"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma se cierra
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this._frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryGestionCobranza.cs", "Form_FormClosed"));
            }
        }

        #endregion Eventos MDI

        #region Eventos Formulario

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkEditViewFile_Click(object sender, EventArgs e)
        {
            try
            {
                Type frm = typeof(GestionCobranza);
                if (this.cobranza != null)
                    FormProvider.GetInstance(frm, new object[] { this.cobranza.ClienteID.Value, this._frmModule.ToString() });
                else
                    FormProvider.GetInstance(frm);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ShowDocumentForm.cs", "QueryGestionCobranza"));
            }
        }
        #endregion Eventos Formulario

        #region Eventos Grilla

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvLibranzas_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
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
                    e.Value = true;
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
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvCobranzas_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle >= 0)
                {
                    int row = e.FocusedRowHandle;
                    this.cobranza = (DTO_QueryGestionCobranza)this.gvCobranzas.GetRow(row);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Asigna texto por defecto para la columna de archivos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvCobranzas_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);
            if (fieldName == "FileUrl")
                e.DisplayText = this._bc.GetResource(LanguageTypes.Messages,"Gestión");
            else if (fieldName == "TipoEstado" && e.Value != null)
            {
                if (e.Value.ToString().Equals("1"))
                    e.DisplayText = "Propia";
                else if (e.Value.ToString().Equals("2"))
                    e.DisplayText = "Cedida";
                else if (e.Value.ToString().Equals("3"))
                    e.DisplayText = "Arrendada";
                else if (e.Value.ToString().Equals("4"))
                    e.DisplayText = "Cobro Jurídico";
                else if (e.Value.ToString().Equals("5"))
                    e.DisplayText = " Acuerdo de Pago";
                else if (e.Value.ToString().Equals("6"))
                    e.DisplayText = "Acuerdo Incumplido";
            }
        }

        #endregion

        #region Eventos Barra Herramientas

        /// <summary>
        /// Boton para crear buscar
        /// </summary>
        public override void TBSearch()
        {
            try
            {
               this.GetSearch();             
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryGestionCobranza.cs", "TBSearch"));
            }
        }

        /// <summary>
        /// Boton para crear buscar
        /// </summary>
        public override void TBExport()
        {
            try
            {
                if (this.gvCobranzas.DataRowCount > 0)
                {
                    DataTableOperations tableOp = new DataTableOperations();                   
                    System.Data.DataTable tableAll = tableOp.Convert_GenericListToDataTable(typeof(DTO_QueryGestionCobranza), this.cobranzas);
                    tableAll.Columns.Remove("NumeroDoc");
                    System.Data.DataTable tableExport = new System.Data.DataTable();

                    ReportExcelBase frm = new ReportExcelBase(tableAll,this._documentID);
                    frm.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryGestionCobranza.cs", "TBExport"));
            }
        }

        #endregion

    }
}
