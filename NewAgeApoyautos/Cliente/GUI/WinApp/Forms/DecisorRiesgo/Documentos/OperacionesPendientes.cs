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
    public partial class OperacionesPendientes : FormWithToolbar
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

        private List<DTO_OperacionesPendientes> cursor = new List<DTO_OperacionesPendientes>();
        private DTO_OperacionesPendientes rowCurrent = new DTO_OperacionesPendientes();


        //Variables formulario
        private bool validate = true;
        private string clienteID = string.Empty;


        #endregion

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public OperacionesPendientes()
        {
            this.Constructor();
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public OperacionesPendientes(string mod)
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
                //this.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-OperacionesPendientes.cs", "OperacionesPendientes.cs-OperacionesPendientes"));
            }
        }     
     
        #region Funciones Privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            this._documentID = AppQueries.QuerySolicPendientes;
            this._frmModule = ModulesPrefix.dr;

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
                GridColumn Libranza = new GridColumn();
                Libranza.FieldName = this._unboundPrefix + "Libranza";
                Libranza.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Libranza");
                Libranza.UnboundType = UnboundColumnType.Integer;
                Libranza.VisibleIndex = 1;
                Libranza.Width = 50;
                Libranza.Visible = true;
                Libranza.OptionsColumn.AllowEdit = false;
                this.gvPendientes.Columns.Add(Libranza);
                /// Solicitud

                // FechaRadica
                GridColumn FechaRadica = new GridColumn();
                FechaRadica.FieldName = this._unboundPrefix + "FechaRadica";
                FechaRadica.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaRadica");
                FechaRadica.UnboundType = UnboundColumnType.DateTime;
                FechaRadica.VisibleIndex = 2;
                FechaRadica.Width = 80;
                FechaRadica.Visible = true;
                FechaRadica.OptionsColumn.AllowEdit = false;
                this.gvPendientes.Columns.Add(FechaRadica);

                // FechaInicio
                GridColumn FechaInicio = new GridColumn();
                FechaInicio.FieldName = this._unboundPrefix + "FechaInicio";
                FechaInicio.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaInicio");
                FechaInicio.UnboundType = UnboundColumnType.DateTime;
                FechaInicio.VisibleIndex = 2;
                FechaInicio.Width = 80;
                FechaInicio.Visible = true;
                FechaInicio.OptionsColumn.AllowEdit = false;
                this.gvPendientes.Columns.Add(FechaInicio);

                /// Fecha
                //ClienteID
                GridColumn ClienteID = new GridColumn();
                ClienteID.FieldName = this._unboundPrefix + "ClienteID";
                ClienteID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ClienteID");
                ClienteID.UnboundType = UnboundColumnType.String;
                ClienteID.VisibleIndex = 3;
                ClienteID.Width = 80;
                ClienteID.Visible = true;
                ClienteID.OptionsColumn.AllowEdit = false;
                this.gvPendientes.Columns.Add(ClienteID);

                //Nombre
                GridColumn Nombre = new GridColumn();
                Nombre.FieldName = this._unboundPrefix + "Nombre";
                Nombre.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Nombre");
                Nombre.UnboundType = UnboundColumnType.String;
                Nombre.VisibleIndex = 4;
                Nombre.Width = 180;
                Nombre.Visible = true;
                Nombre.OptionsColumn.AllowEdit = false;
                this.gvPendientes.Columns.Add(Nombre);

                //Vitrina
                GridColumn Vitrina = new GridColumn();
                Vitrina.FieldName = this._unboundPrefix + "Vitrina";
                Vitrina.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Vitrina");
                Vitrina.UnboundType = UnboundColumnType.String;
                Vitrina.VisibleIndex = 5;
                Vitrina.Width = 80;
                Vitrina.Visible = true;
                Vitrina.OptionsColumn.AllowEdit = false;
                this.gvPendientes.Columns.Add(Vitrina);

                //ZonaID
                GridColumn ZonaID = new GridColumn();
                ZonaID.FieldName = this._unboundPrefix + "Zona";
                ZonaID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ZonaID");
                ZonaID.UnboundType = UnboundColumnType.String;
                ZonaID.VisibleIndex = 6;
                ZonaID.Width = 60;
                ZonaID.Visible = true;
                ZonaID.OptionsColumn.AllowEdit = false;
                this.gvPendientes.Columns.Add(ZonaID);

                //Operacion
                GridColumn Operacion = new GridColumn();
                Operacion.FieldName = this._unboundPrefix + "TipoOperacion";
                Operacion.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Operacion");
                Operacion.UnboundType = UnboundColumnType.Integer;
                Operacion.VisibleIndex = 7;
                Operacion.Width = 150;
                Operacion.Visible = true;
                Operacion.OptionsColumn.AllowEdit = false;
                this.gvPendientes.Columns.Add(Operacion);

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
                EtapaID.Width = 150;
                EtapaID.Visible = true;
                EtapaID.ColumnEdit = this.linkEditViewFile;                
                EtapaID.OptionsColumn.AllowEdit = true;
                this.gvPendientes.Columns.Add(EtapaID);

                //Estado
                GridColumn Estado = new GridColumn();
                Estado.FieldName = this._unboundPrefix + "Estado";
                Estado.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Estado");
                Estado.UnboundType = UnboundColumnType.DateTime;
                Estado.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                Estado.AppearanceCell.Options.UseTextOptions = true;
                Estado.VisibleIndex = 12;
                Estado.Width = 150;
                Estado.Visible = true;
                Estado.OptionsColumn.AllowEdit = false;
                this.gvPendientes.Columns.Add(Estado);

                //Documento
                GridColumn ViewDoc = new GridColumn();
                ViewDoc.FieldName = this._unboundPrefix + "ViewDoc";
                ViewDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ViewDoc");
                ViewDoc.UnboundType = UnboundColumnType.String;
                ViewDoc.OptionsColumn.ShowCaption = false;
                ViewDoc.VisibleIndex = 13;
                ViewDoc.Width = 80;
                ViewDoc.ColumnEdit = this.linkEditViewFile;
                ViewDoc.Visible = true;
                this.gvPendientes.Columns.Add(ViewDoc);
                this.gvPendientes.OptionsView.ColumnAutoWidth = true;

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-OperacionesPendientess.cs", "AddGridCols"));
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
            this.cursor = new List<DTO_OperacionesPendientes>();
            this.gcPendientes.DataSource = this.cursor;
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
                this.cursor = this._bc.AdministrationModel.OperacionesPendientes();
                if (this.cursor!= null && this.cursor.Count > 0)
                {
                    #region Carga la informacion de la grilla
                    this.gcPendientes.DataSource = null;
                    this.gcPendientes.DataSource = this.cursor;
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
               MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-OperacionesPendientes.cs", "GetSearch"));
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
                FormProvider.Master.itemUpdate.Visible = true;
                FormProvider.Master.itemUpdate.Enabled = true;

                this.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-OperacionesPendientes.cs", "Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-OperacionesPendientes.cs", "Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-OperacionesPendientes.cs", "Form_FormClosed"));
            }
        }

        #endregion Eventos MDI

        #region Eventos Formulario

        /// <summary>
        /// PErmite mostrar u ocultar controles
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
                            case AppDocuments.RegistroSolicitud:
                                frm = typeof(DigitacionSolicitudNuevos);
                                FormProvider.GetInstance(frm, new object[] { this.rowCurrent.ClienteID.Value, this.rowCurrent.Libranza.Value.Value,false,false });
                                break;
                            case AppDocuments.Revision1:
                                frm = typeof(Datacredito);
                                FormProvider.GetInstance(frm, new object[] { this.rowCurrent.Libranza.Value, false });
                                break;
                            case AppDocuments.VerificacionSolicitud:
                                frm = typeof(DigitacionSolicitudNuevos);
                                FormProvider.GetInstance(frm, new object[] { this.rowCurrent.ClienteID.Value, this.rowCurrent.Libranza.Value.Value, true, true });
                                break;
                            case AppDocuments.CartasSolicitud:
                                frm = typeof(CartasSolicitud);
                                FormProvider.GetInstance(frm, new object[] { this.rowCurrent.Libranza.Value, false,1 });
                                break;
                            case AppDocuments.Firma2:
                                frm = typeof(CartasSolicitud);
                                FormProvider.GetInstance(frm, new object[] { this.rowCurrent.Libranza.Value, false,2 });
                                break;
                            case AppDocuments.Firma3:
                                frm = typeof(CartasSolicitud);
                                FormProvider.GetInstance(frm, new object[] { this.rowCurrent.Libranza.Value, false,3 });
                                break;
                            //case AppDocuments.RatificacionSolicitud:
                            //    frm = typeof(Rafiticacion);
                            //    FormProvider.GetInstance(frm, new object[] { this.rowCurrent.Libranza.Value, false });
                            //    break;
                            case AppDocuments.LegalizacionSolicitud:
                                frm = typeof(LiquidacionLegalizacion);
                                FormProvider.GetInstance(frm, new object[] {this.rowCurrent.Libranza.Value,false,false});
                                break;
                            case AppDocuments.RevisionLegalizacionSolicitud:
                                frm = typeof(LiquidacionLegalizacion);
                                FormProvider.GetInstance(frm, new object[] { this.rowCurrent.Libranza.Value, false,true });
                                break;

                            case AppDocuments.DesembolsoSolicitud:
                                frm = typeof(DesembolsoSolicitud);
                                FormProvider.GetInstance(frm, new object[] { this.rowCurrent.Libranza.Value, false });
                                break;
                            case AppDocuments.DigitacionCreditoFinanciera:
                                frm = typeof(DigitacionCreditoFinanciera);
                                FormProvider.GetInstance(frm, new object[] { this.rowCurrent.Libranza.Value, false });
                                break;
                            case AppDocuments.AprobacionSolicitudFin:
                                frm = typeof(AprobacionSolicitudFin);
                                FormProvider.GetInstance(frm, new object[] { this.rowCurrent.Libranza.Value, false });
                                break;
                            case AppDocuments.NegociosGestionar:
                                frm = typeof(AprobacionFlujo);
                                FormProvider.GetInstance(frm, new object[] { this.rowCurrent.Libranza.Value, false, AppDocuments.NegociosGestionar ,this.rowCurrent.Estado.Value});
                                break;
                            //case AppDocuments.AprobacionLegalizacion:
                            //    frm = typeof(AprobacionFlujo);
                            //    FormProvider.GetInstance(frm, new object[] { this.rowCurrent.Libranza.Value, false, AppDocuments.AprobacionLegalizacion });
                            //    break;
                            //case AppDocuments.AprobacionDesembolso:
                            //    frm = typeof(AprobacionFlujo);
                            //    FormProvider.GetInstance(frm, new object[] { this.rowCurrent.Libranza.Value, false, AppDocuments.AprobacionDesembolso });
                            //    break;
                        }
                    }
                }
            }
            catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ShowDocumentForm.cs", "OperacionesPendientes"));            
                }
           
        }

        /// <summary>
        /// Agrega nuevas solicitudes desde el canal preferencial
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCanalPreferncial_Click(object sender, EventArgs e)
        {
            try
            {


                this.btnCanalPreferncial.Enabled = false;
                DTO_TxResult result = this._bc.AdministrationModel.SolicitudCanalPreferencial_Add();
                if (result.Result == ResultValue.OK)
                    this.LoadData();

                MessageForm frm = new MessageForm(result);
                frm.ShowDialog();

                this.btnCanalPreferncial.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-OperacionesPendientes.cs", "btnCanalPreferncial_Click"));
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
                    this.rowCurrent = (DTO_OperacionesPendientes)this.gvPendientes.GetRow(row);
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
            else if (fieldName == "ViewDoc")
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