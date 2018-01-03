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
using DevExpress.XtraEditors;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class SolicitudGestion : FormWithToolbar
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

        private List<DTO_SolicitudGestion> data = new List<DTO_SolicitudGestion>();
        private DTO_SolicitudGestion rowCurrent = new DTO_SolicitudGestion();


        //Variables formulario
        private bool validate = true;
        private string clienteID = string.Empty;


        #endregion

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public SolicitudGestion()
        {
            this.Constructor();
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public SolicitudGestion(string mod)
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
                this.AddGridCols();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());
                if (!string.IsNullOrWhiteSpace(mod))
                    this._frmModule = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), mod);

                FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);        
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudGestion.cs", "SolicitudGestion.cs-SolicitudGestion"));
            }
        }     
     
        #region Funciones Privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            this._documentID = AppQueries.QuerySolicitudGestion;
            this._frmModule = ModulesPrefix.cc;

            //Carga los combos de la fecha
            //this.dtFechaInicial.EditValue = DateTime.Now;
            //this.dtFechaCorte.EditValue = DateTime.Now;
            //this._bc.InitMasterUC(this.masterEtapa, AppMasters.glIncumplimientoEtapa, true, true, true, false);

        }

        /// <summary>
        /// Agrega las columnas a las grillas
        /// </summary>
        /// <summary>
        /// Agrega las columnas a las grillas
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                #region Grilla Principal
                // Solicitud
                GridColumn Solicitud = new GridColumn();
                Solicitud.FieldName = this._unboundPrefix + "Libranza";
                Solicitud.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Solicitud");
                Solicitud.UnboundType = UnboundColumnType.Integer;
                Solicitud.VisibleIndex = 1;
                Solicitud.Width = 80;
                Solicitud.Visible = true;
                Solicitud.OptionsColumn.AllowEdit = false;
                this.gvPendientes.Columns.Add(Solicitud);
                /// Solicitud

                // Fecha
                GridColumn Fecha = new GridColumn();
                Fecha.FieldName = this._unboundPrefix + "Fecha";
                Fecha.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Fecha");
                Fecha.UnboundType = UnboundColumnType.DateTime;
                Fecha.VisibleIndex = 2;
                Fecha.Width = 100;
                Fecha.Visible = true;
                Fecha.OptionsColumn.AllowEdit = false;
                this.gvPendientes.Columns.Add(Fecha);
                /// Fecha
                //ClienteID
                GridColumn ClienteID = new GridColumn();
                ClienteID.FieldName = this._unboundPrefix + "ClienteID";
                ClienteID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ClienteID");
                ClienteID.UnboundType = UnboundColumnType.String;
                ClienteID.VisibleIndex = 3;
                ClienteID.Width = 110;
                ClienteID.Visible = true;
                ClienteID.OptionsColumn.AllowEdit = false;
                this.gvPendientes.Columns.Add(ClienteID);

                //Nombre
                GridColumn Nombre = new GridColumn();
                Nombre.FieldName = this._unboundPrefix + "Nombre";
                Nombre.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Nombre");
                Nombre.UnboundType = UnboundColumnType.String;
                Nombre.VisibleIndex = 4;
                Nombre.Width = 200;
                Nombre.Visible = true;
                Nombre.OptionsColumn.AllowEdit = false;
                this.gvPendientes.Columns.Add(Nombre);

                //ZonaID
                GridColumn ZonaID = new GridColumn();
                ZonaID.FieldName = this._unboundPrefix + "Zona";
                ZonaID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ZonaID");
                ZonaID.UnboundType = UnboundColumnType.String;
                ZonaID.VisibleIndex = 5;
                ZonaID.Width = 60;
                ZonaID.Visible = true;
                ZonaID.OptionsColumn.AllowEdit = false;
                this.gvPendientes.Columns.Add(ZonaID);

                //Pagaduria
                GridColumn Pagaduria = new GridColumn();
                Pagaduria.FieldName = this._unboundPrefix + "Pagaduria";
                Pagaduria.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Pagaduria");
                Pagaduria.UnboundType = UnboundColumnType.String;
                Pagaduria.VisibleIndex = 6;
                Pagaduria.Width = 60;
                Pagaduria.Visible = true;
                Pagaduria.OptionsColumn.AllowEdit = false;
                this.gvPendientes.Columns.Add(Pagaduria);

                //LineaCredito
                GridColumn LineaCredito = new GridColumn();
                LineaCredito.FieldName = this._unboundPrefix + "LineaCredito";
                LineaCredito.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_LineaCredito");
                LineaCredito.UnboundType = UnboundColumnType.String;
                LineaCredito.VisibleIndex = 7;
                LineaCredito.Width = 60;
                LineaCredito.Visible = true;
                LineaCredito.OptionsColumn.AllowEdit = false;
                this.gvPendientes.Columns.Add(LineaCredito);

                //TipoOperacion
                GridColumn TipoOperacion = new GridColumn();
                TipoOperacion.FieldName = this._unboundPrefix + "TipoOperacion";
                TipoOperacion.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Operacion");
                TipoOperacion.UnboundType = UnboundColumnType.String;
                TipoOperacion.VisibleIndex = 8;
                TipoOperacion.Width = 60;
                TipoOperacion.Visible = true;
                TipoOperacion.OptionsColumn.AllowEdit = false;
                this.gvPendientes.Columns.Add(TipoOperacion);

                //Plazo
                GridColumn Plazo = new GridColumn();
                Plazo.FieldName = this._unboundPrefix + "Plazo";
                Plazo.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Plazo");
                Plazo.UnboundType = UnboundColumnType.String;
                Plazo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                Plazo.AppearanceCell.Options.UseTextOptions = true;
                Plazo.VisibleIndex = 9;
                Plazo.Width = 50;
                Plazo.Visible = true;
                Plazo.OptionsColumn.AllowEdit = false;
                this.gvPendientes.Columns.Add(Plazo);

                //Plazo
                GridColumn VlrSolicitado = new GridColumn();
                VlrSolicitado.FieldName = this._unboundPrefix + "VlrSolicitado";
                VlrSolicitado.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrSolicitado");
                VlrSolicitado.UnboundType = UnboundColumnType.String;
                VlrSolicitado.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrSolicitado.AppearanceCell.Options.UseTextOptions = true;
                VlrSolicitado.VisibleIndex = 10;
                VlrSolicitado.Width = 100;
                VlrSolicitado.Visible = true;
                VlrSolicitado.ColumnEdit = this.editValue;
                VlrSolicitado.OptionsColumn.AllowEdit = false;
                this.gvPendientes.Columns.Add(VlrSolicitado);

                //EtapaID
                GridColumn EtapaID = new GridColumn();
                EtapaID.FieldName = this._unboundPrefix + "Etapa";
                EtapaID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_EtapaID");
                EtapaID.UnboundType = UnboundColumnType.String;
                EtapaID.VisibleIndex = 11;
                EtapaID.Width = 120;
                EtapaID.Visible = true;
                EtapaID.ColumnEdit = this.linkEditViewFile;                
                EtapaID.OptionsColumn.AllowEdit = true;
                this.gvPendientes.Columns.Add(EtapaID);

                //Documento
                GridColumn ViewDoc = new GridColumn();
                ViewDoc.FieldName = this._unboundPrefix + "ViewDoc";
                ViewDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ViewDoc");
                ViewDoc.UnboundType = UnboundColumnType.String;
                ViewDoc.OptionsColumn.ShowCaption = false;
                ViewDoc.VisibleIndex = 12;
                ViewDoc.Width = 80;
                ViewDoc.ColumnEdit = this.linkEditViewFile;
                ViewDoc.Visible = true;
                this.gvPendientes.Columns.Add(ViewDoc);
                this.gvPendientes.OptionsView.ColumnAutoWidth = true;

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudGestions.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Refresca el contenido del formulario
        /// </summary>
        private void CleanData()
        {
            this.validate = false;
            //this.dtFechaInicial.EditValue = DateTime.Now;
            //this.dtFechaCorte.EditValue = DateTime.Now;
            this.data = new List<DTO_SolicitudGestion>();
            this.gcPendientes.DataSource = this.data;
            this.clienteID = string.Empty;
            this.validate = true;
        }        
        
        /// <summary>
        /// Funcion que realiza la opreacion de busqueda
        /// </summary>
        private void LoadData()
        {
            try
            {
                this.data = this._bc.AdministrationModel.SolicitudLibranza_GetGestionSolicitud();
                if (this.data!= null && this.data.Count > 0)
                {
                    #region Carga la informacion de la grilla
                    this.gcPendientes.DataSource = null;
                    this.gcPendientes.DataSource = this.data;
                    this.gcPendientes.RefreshDataSource();
                    //this.gvPendientes.BestFitColumns();
                    this.gvPendientes.MoveFirst();
                    #endregion
                }
                else
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
                    this.CleanData();
                }
            }                      
            catch (Exception ex)
            {                
               MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudGestion.cs", "GetSearch"));
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
                FormProvider.Master.itemSearch.Visible = false;
                FormProvider.Master.itemSearch.Enabled = true;
                FormProvider.Master.itemSave.Visible = false;
                FormProvider.Master.itemExport.Visible = true;
                FormProvider.Master.itemExport.Enabled = true;
                this.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudGestion.cs", "Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudGestion.cs", "Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudGestion.cs", "Form_FormClosed"));
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
                HyperLinkEdit link = (HyperLinkEdit)sender;
                if (string.IsNullOrEmpty(link.Text))
                {
                    DTO_glDocumentoControl ctrl = new DTO_glDocumentoControl();
                    DTO_Comprobante comprobante = new DTO_Comprobante();
                    {
                        ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(this.rowCurrent.NumeroDoc.Value.Value);
                        ctrl.ComprobanteIDNro.Value = ctrl.ComprobanteIDNro.Value ?? 0;
                        comprobante = this._bc.AdministrationModel.Comprobante_Get(true, false, ctrl.PeriodoDoc.Value.Value, ctrl.ComprobanteID.Value, ctrl.ComprobanteIDNro.Value.Value, null, null, null);
                    }

                    ShowDocumentForm documentForm = new ShowDocumentForm(ctrl, comprobante);
                    documentForm.Show();
                }
                else
                {
                    if (this.rowCurrent != null)
                    {
                        Type frm = null;
                        switch (this.rowCurrent.DocumentoID.Value.Value)
                        {
                            case AppDocuments.EnvioSolicitudLibranza:
                                frm = typeof(EnvioSolicitudLibranza);
                                FormProvider.GetInstance(frm, new object[] { this.rowCurrent.Libranza.Value.Value,false });
                                break;
                            case AppDocuments.AnalisisRiesgo:
                                frm = typeof(AnalisisRiesgo);
                                FormProvider.GetInstance(frm, new object[] { this.rowCurrent.Libranza.Value.Value, false });
                                break;
                            case AppDocuments.RegistroSolicitud:
                                frm = typeof(SolicitudRegistro);
                                FormProvider.GetInstance(frm, new object[] { this.rowCurrent.Libranza.Value.Value, false });
                                break;
                            case AppDocuments.Referenciacion:
                                frm = typeof(Referenciacion);
                                FormProvider.GetInstance(frm, new object[] { this.rowCurrent.Libranza.Value, this._frmModule.ToString() });
                                break;
                            case AppDocuments.VerificacionPreliminar:
                                frm = typeof(VerificacionPreliminar);
                                FormProvider.GetInstance(frm, new object[] { this.rowCurrent.Libranza.Value,false });
                                break;
                            case AppDocuments.DigitacionCreditoFinanciera:
                                frm = typeof(DigitacionCreditoFinanciera);
                                FormProvider.GetInstance(frm, new object[] { this.rowCurrent.Libranza.Value, false });
                                break;                           
                            case AppDocuments.AprobacionSolicitudFin:
                                frm = typeof(AprobacionSolicitudFin);
                                FormProvider.GetInstance(frm, new object[] { this.rowCurrent.Libranza.Value,false });
                                break;
                            case AppDocuments.EnvioLiquidacionCartera:
                                frm = typeof(EnvioSolicitudLibranza);
                                FormProvider.GetInstance(frm, new object[] { this.rowCurrent.Libranza.Value.Value, false });
                                break;
                            case AppDocuments.SolicitudAnticipo:
                                frm = typeof(SolicitudAnticipos);
                                FormProvider.GetInstance(frm, new object[] { this.rowCurrent.Libranza.Value });
                                break;
                        }
                    }
                }

            }
            catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ShowDocumentForm.cs", "SolicitudGestion"));            
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
        private void gvPendientes_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle >= 0)
                {
                    int row = e.FocusedRowHandle;
                    this.rowCurrent = (DTO_SolicitudGestion)this.gvPendientes.GetRow(row);
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
        private void gvPendientes_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);
            if (fieldName == "Etapa" && e.Value!=null)
                e.DisplayText = e.Value.ToString();
            if (fieldName == "ViewDoc")
                e.DisplayText = "Ver Documento";
        }

        #endregion

        #region barraherramientas
        public override void TBUpdate()
        {
            this.LoadData();
        }
        #endregion
    }
}