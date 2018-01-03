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
using NewAge.DTO.Negocio;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using System.Threading;
using NewAge.DTO.Resultados;
using System.Reflection;
using NewAge.DTO.UDT;
using System.Diagnostics;
using DevExpress.XtraEditors;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class SentenciaJudicial : FormWithToolbar
    {
        #region Delegados

        private delegate void RefreshGrid();
        private RefreshGrid refreshGridDelegate;
        /// <summary>
        /// Delegado que finaliza el proceso de importación
        /// </summary>
        private void RefreshGridMethod()
        {
            this.CleanData();
        }

        #endregion

        #region Variables

        private BaseController _bc = BaseController.GetInstance();

        //DTO's
        private List<DTO_ccCreditoDocu> creditosCliente = new List<DTO_ccCreditoDocu>();
        private List<DTO_ccCreditoDocu> cobrosJuridicosTemp = new List<DTO_ccCreditoDocu>();
        private List<DTO_ccCreditoComponentes> _credComponentesGrid = new List<DTO_ccCreditoComponentes>();
        private DTO_ccCreditoDocu _currentCredito = new DTO_ccCreditoDocu();
        //Variables privadas
        private string clienteID = String.Empty;
        private FormTypes _frmType = FormTypes.Document;
        private string _frmName;
        private int userID = 0;
        //Para manejo de propiedades
        private string empresaID = string.Empty;
        private int documentID;
        private bool multiMoneda;
        private ModulesPrefix frmModule;
        private List<int> select = new List<int>();
        //Internas del formulario
        private string areaFuncionalID;
        private string prefijoID;
        private bool isValid = true;
        private byte _claseDeuda = 1;
        private string _compCapital = string.Empty;
        private string _compSeguro = string.Empty;
        private string _compIntSeguro = string.Empty;

        private string tipoMvto = string.Empty;
        private DTO_glActividadFlujo _actFlujo = new DTO_glActividadFlujo();
        private string unboundPrefix = "Unbound_";

        string polizaRsx = string.Empty;
        string polizaIntRsx = string.Empty;
        string polizaSaldoRsx = string.Empty;
        string polizaSaldoIntRsx =string.Empty;
        string capitalRsx = string.Empty;
        string capitalIntRsx = string.Empty;
        string capitalSaldoRsx = string.Empty;
        string capitalSaldoIntRsx = string.Empty;

        #endregion

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public SentenciaJudicial()
        {
            this.Constructor();
            //this.dtFechaMvto
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public SentenciaJudicial(string mod)
        {
            this.Constructor(mod);
        }

        /// <summary>
        /// Funcion que llama el Constructor del Documento
        /// </summary>
        private void Constructor(string mod = null)
        {
            this.InitializeComponent();
            this.SetInitParameters();
            this.multiMoneda = _bc.AdministrationModel.MultiMoneda;
            this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());
            this.frmModule = ModulesPrefix.cf;

            FormProvider.Master.Form_Load(this, this.frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
            this.refreshGridDelegate = new RefreshGrid(RefreshGridMethod);

            #region Carga la info de las actividades
            List<string> actividades = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(this.documentID);

            if (actividades.Count != 1)
            {
                string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Gl_DocMultActivities);
                MessageBox.Show(string.Format(msg, this.documentID.ToString()));
            }
            else
            {
                string actividadFlujoID = actividades[0];
                this._actFlujo = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, actividadFlujoID, true);
            }
            #endregion
            
        }

        #region Funciones Privadas

        /// <summary>
        /// Limpia el formulario
        /// </summary>
        private void CleanData()
        {
            this.masterCliente.Value = String.Empty;

            //Variables
            this.clienteID = string.Empty;
            this.creditosCliente = new List<DTO_ccCreditoDocu>();
            this._credComponentesGrid = new List<DTO_ccCreditoComponentes>();
            this.gcDocument.DataSource = this._credComponentesGrid;
            this.gvDocument.RefreshData();
            this.lkpObligaciones.Properties.DataSource = null;
            this.lkpObligaciones.EditValue = null;
            this.cmbEstadoActual.EditValue = "";
            this.txtValorCuota.EditValue = 0;
            this.cmbPlazoSent.Text = string.Empty;
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            try
            {
                this.documentID = AppDocuments.SentenciaJuzgado;
                this.frmModule = ModulesPrefix.cc;

                #region Info del header superior
                this.areaFuncionalID = _bc.AdministrationModel.User.AreaFuncionalID.Value;
                DTO_MasterBasic basicDTO = (DTO_MasterBasic)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glAreaFuncional, false, this.areaFuncionalID, true);

                this.prefijoID = _bc.GetPrefijo(this.areaFuncionalID, this.documentID);
                DTO_glDocumento dtoDoc = (DTO_glDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, this.documentID.ToString(), true);

                this.txtDocumentoID.Text = this.documentID.ToString();
                this.txtDocDesc.Text = dtoDoc.Descriptivo.Value;
                this.txtNumeroDoc.Text = "0";

                this._compCapital = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteCapital);
                this._compSeguro = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteSeguroVida);
                this._compIntSeguro = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteInteresSeguro);

                string periodoStr = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo);
                this.dtPeriod.DateTime = Convert.ToDateTime(periodoStr);
                this.dtPeriod.Enabled = false;
                if (DateTime.Now.Month != this.dtPeriod.DateTime.Month)
                {
                    this.dtFechaDoc.DateTime = new DateTime(this.dtPeriod.DateTime.Year, this.dtPeriod.DateTime.Month, DateTime.DaysInMonth(this.dtPeriod.DateTime.Year, this.dtPeriod.DateTime.Month));
                }
                else
                {
                    this.dtFechaDoc.Properties.MinValue = new DateTime(this.dtPeriod.DateTime.Year, this.dtPeriod.DateTime.Month, 1);
                    this.dtFechaDoc.Properties.MaxValue = new DateTime(this.dtPeriod.DateTime.Year, this.dtPeriod.DateTime.Month, DateTime.DaysInMonth(this.dtPeriod.DateTime.Year, this.dtPeriod.DateTime.Month));
                    this.dtFechaDoc.DateTime = DateTime.Now;
                }
                this.dtFechaSentencia.DateTime = this.dtFechaDoc.DateTime;
                this.dtFecha1CuotSent.DateTime = this.dtFechaSentencia.DateTime;
                this.txtValorCuota.EditValue = 0;
                #endregion
                #region Inicia los controles y las grillas
                //Carga la grilla con las columnas
                this.AddGridCols();

                //Carga la maestra de comprador de cartera
                this._bc.InitMasterUC(this.masterCliente, AppMasters.ccCliente, true, true, true, false);

                //llena Combos
                this.cmbEstadoActual.Properties.ReadOnly = true;
                #endregion
                #region Carga los diccionarios para los ddl

                //Estado de cartera cliente
                Dictionary<string, string> dicEstado = new Dictionary<string, string>();
                dicEstado.Add(((byte)TipoEstadoCartera.Propia).ToString(), this._bc.GetResource(LanguageTypes.Tables, "tbl_Normal"));
                dicEstado.Add(((byte)TipoEstadoCartera.Cedida).ToString(), this._bc.GetResource(LanguageTypes.Tables, "tbl_carteraCedida"));
                dicEstado.Add(((byte)TipoEstadoCartera.CobroJuridico).ToString(), this._bc.GetResource(LanguageTypes.Tables, "tbl_CobroJuridico"));
                dicEstado.Add(((byte)TipoEstadoCartera.AcuerdoPago).ToString(), this._bc.GetResource(LanguageTypes.Tables, "tbl_AcuerdoPago"));
                dicEstado.Add(((byte)TipoEstadoCartera.AcuerdoPagoIncumplido).ToString(), this._bc.GetResource(LanguageTypes.Tables, "tbl_AcuerdoIncumplido"));
                dicEstado.Add(((byte)TipoEstadoCartera.Castigada).ToString(), this._bc.GetResource(LanguageTypes.Tables, "tbl_Castigada"));
                this.cmbEstadoActual.Properties.DataSource = dicEstado;

                this.masterCliente.Focus();
                #endregion

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SentenciaJudicialGral.cs", "SetInitParameters"));
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla superior
        /// </summary>
        private void AddGridCols()
        {
            try
            {                
                #region Agrega las columnas Grilla

                //ComponenteCarteraID
                GridColumn componenteCarteraID = new GridColumn();
                componenteCarteraID.FieldName = this.unboundPrefix + "ComponenteCarteraID";
                componenteCarteraID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ComponenteCarteraID");
                componenteCarteraID.UnboundType = UnboundColumnType.String;
                componenteCarteraID.VisibleIndex = 0;
                componenteCarteraID.Width = 70;
                componenteCarteraID.Visible = true;
                componenteCarteraID.OptionsColumn.AllowEdit = true;
                componenteCarteraID.OptionsColumn.AllowFocus = false;
                componenteCarteraID.ColumnEdit = this.editBtnGrid;
                this.gvDocument.Columns.Add(componenteCarteraID);

                //Descripcion
                GridColumn descripcion = new GridColumn();
                descripcion.FieldName = this.unboundPrefix + "Descripcion";
                descripcion.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descripcion");
                descripcion.UnboundType = UnboundColumnType.String;
                descripcion.VisibleIndex = 1;
                descripcion.Width = 200;
                descripcion.Visible = true;
                descripcion.OptionsColumn.AllowEdit = false;
                descripcion.OptionsColumn.AllowFocus = false;
                this.gvDocument.Columns.Add(descripcion);

                //TotalSaldo
                GridColumn TotalSaldo = new GridColumn();
                TotalSaldo.FieldName = this.unboundPrefix + "TotalValor";
                TotalSaldo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TotalValor");
                TotalSaldo.UnboundType = UnboundColumnType.Decimal;
                TotalSaldo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                TotalSaldo.AppearanceCell.Options.UseTextOptions = true;
                TotalSaldo.VisibleIndex = 2;
                TotalSaldo.Width = 100;
                TotalSaldo.Visible = true;
                TotalSaldo.OptionsColumn.AllowEdit = false;
                TotalSaldo.ColumnEdit = this.editSpin;
                TotalSaldo.OptionsColumn.AllowFocus = false;
                TotalSaldo.Summary.Add(DevExpress.Data.SummaryItemType.Sum, TotalSaldo.FieldName, "{0:c0}");
                this.gvDocument.Columns.Add(TotalSaldo);

                //TotalSaldo
                GridColumn PasarValor = new GridColumn();
                PasarValor.FieldName = this.unboundPrefix + "PasarValor";
                PasarValor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PasarValor");
                PasarValor.UnboundType = UnboundColumnType.Decimal;
                PasarValor.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                PasarValor.AppearanceCell.Options.UseTextOptions = true;
                PasarValor.VisibleIndex = 2;
                PasarValor.Width = 20;
                PasarValor.Visible = true;
                PasarValor.OptionsColumn.ShowCaption = false;
                PasarValor.OptionsColumn.AllowEdit = true;
                PasarValor.ColumnEdit = this.editbtn;
                this.gvDocument.Columns.Add(PasarValor);

                //VlrCausado
                GridColumn VlrCausado = new GridColumn();
                VlrCausado.FieldName = this.unboundPrefix + "VlrCausado";
                VlrCausado.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrCausado");
                VlrCausado.UnboundType = UnboundColumnType.Decimal;
                VlrCausado.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrCausado.AppearanceCell.Options.UseTextOptions = true;
                VlrCausado.VisibleIndex = 3;
                VlrCausado.Width = 100;
                VlrCausado.Visible = true;
                VlrCausado.OptionsColumn.AllowEdit = true;
                VlrCausado.ColumnEdit = this.editSpin;
                VlrCausado.Summary.Add(DevExpress.Data.SummaryItemType.Sum, VlrCausado.FieldName, "{0:c0}");
                this.gvDocument.Columns.Add(VlrCausado);
                #endregion
                
                this.gvDocument.OptionsView.ColumnAutoWidth = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SentenciaJudicialGral.cs", "AddGridCols"));
            }

        }

        /// <summary>
        /// Trae la información de los créditos deun cliente
        /// </summary>
        private void LoadDocuments()
        {
            try
            {
                this._credComponentesGrid = new List<DTO_ccCreditoComponentes>();
                this.gcDocument.DataSource = null;
                if (this._currentCredito != null)
                {
                    this._credComponentesGrid = new List<DTO_ccCreditoComponentes>();
                    DTO_InfoCredito infoCartera = this._bc.AdministrationModel.GetSaldoCredito(this._currentCredito.NumeroDoc.Value.Value, DateTime.Today.Date, true, true, true, false);
                    infoCartera.SaldosComponentes.RemoveAll(x => x.TipoComponente.Value == (byte)TipoComponente.ComponenteGasto || x.ComponenteCarteraID.Value == this._compSeguro || x.ComponenteCarteraID.Value == this._compIntSeguro);

                    List<string> comps = (from c in infoCartera.SaldosComponentes select c.ComponenteCarteraID.Value).Distinct().ToList();
                    foreach (string c in comps)
                    {
                        DTO_ccCreditoComponentes comp = new DTO_ccCreditoComponentes();
                        DTO_ccCarteraComponente dto = (DTO_ccCarteraComponente)this._bc.GetMasterDTO(AppMasters.MasterType.Simple,AppMasters.ccCarteraComponente,false,c,true);
                        comp.ComponenteCarteraID.Value = c;
                        comp.Descripcion.Value = infoCartera.SaldosComponentes.Find(x => x.ComponenteCarteraID.Value == c).Descriptivo.Value;
                        comp.TotalValor.Value = infoCartera.SaldosComponentes.Where(x => x.ComponenteCarteraID.Value == c).Sum(x => x.CuotaSaldo.Value);
                        comp.VlrCausado.Value = 0;
                        comp.TipoComponente.Value = dto.TipoComponente.Value;
                        if (comp.TotalValor.Value > 0)
                            this._credComponentesGrid.Add(comp);
                    }

                    #region Agrega componentes adicionales de sentencia
                    long count = this._bc.AdministrationModel.MasterSimple_Count(AppMasters.ccCarteraComponente, null, null, true);
                    var masters = this._bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.ccCarteraComponente, count, 1, null, null, true).ToList();
                    List<DTO_ccCarteraComponente> dtos = _bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.ccCarteraComponente, count, 1, null, null, true).Cast<DTO_ccCarteraComponente>().ToList();
                    dtos = dtos.FindAll(x => x.SentenciaInd.Value == true);
                    for (int i = 0; i < dtos.Count; i++)
                    {
                        DTO_ccCreditoComponentes comp = new DTO_ccCreditoComponentes();
                        DTO_ccCarteraComponente dto = dtos[i];
                        comp.ComponenteCarteraID.Value = dto.ID.Value;
                        comp.Descripcion.Value = dto.Descriptivo.Value;
                        comp.TotalValor.Value = 0;
                        comp.VlrCausado.Value = 0;
                        comp.TipoComponente.Value = dto.TipoComponente.Value;
                        this._credComponentesGrid.Add(comp);
                        //}
                    } 
                    #endregion
                    this.gcDocument.DataSource = null;
                    this.gcDocument.DataSource = this._credComponentesGrid;

                    this.cmbEstadoActual.EditValue = this._currentCredito.TipoEstado.Value.ToString();
                    this.txtValorCuota.EditValue = this._currentCredito.VlrCuotaSentencia.Value.HasValue? this._currentCredito.VlrCuotaSentencia.Value : 0;
                    this.dtFecha1CuotSent.EditValue = this._currentCredito.FechaCuota1Sentencia.Value.HasValue? this._currentCredito.FechaCuota1Sentencia.Value : this.dtFechaSentencia.DateTime;
                    this.cmbPlazoSent.Text = this._currentCredito.PlazoSentencia.Value.ToString();

                }
                else
                    this.gcDocument.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SentenciaJudicialGral.cs", "LoadDocuments"));
            }
        }

        /// <summary>
        /// Agrega un nuevo registro a la grilla de componentes
        /// </summary>
        private void AddComponente()
        {
            try
            {
                DTO_ccCreditoComponentes comp = new DTO_ccCreditoComponentes();
                comp.ComponenteCarteraID.Value = string.Empty;
                comp.Descripcion.Value = string.Empty;
                comp.TotalValor.Value = 0;
                comp.VlrCausado.Value = 0;
                this._credComponentesGrid.Add(comp);              

                this.gcDocument.DataSource = this._credComponentesGrid;
                this.gcDocument.RefreshDataSource();

                this.gvDocument.Columns[this.unboundPrefix + "ComponenteCarteraID"].OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns[this.unboundPrefix + "ComponenteCarteraID"].OptionsColumn.AllowFocus = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RecaudosManuales.cs", "AddComponente"));
            }
        }

        /// <summary>
        /// Muestra el formulario modal para una FK
        /// </summary>
        /// <param name="row">Fila donde esta la FK</param>
        /// <param name="col">Columna seleccionada</param>
        /// <param name="be">Boton que ejecuta la accion</param>
        protected void ShowFKModal(int row, string col, ButtonEdit be, List<DTO_glConsultaFiltro> filtros = null)
        {
            try
            {
                DTO_aplMaestraPropiedades props = _bc.GetMasterPropertyByColId(col);
                string countMethod = "MasterSimple_Count";
                string dataMethod = "MasterSimple_GetPaged";

                string modFrmCode = props.DocumentoID.ToString();
                string modEmpGrupo = _bc.GetMaestraEmpresaGrupoByDocumentID(Convert.ToInt32(modFrmCode));
                Tuple<int, string> tup = new Tuple<int, string>(Convert.ToInt32(modFrmCode), modEmpGrupo);

                DTO_glTabla fktable = _bc.AdministrationModel.Tables[tup];
                if (fktable.Jerarquica.Value.Value)
                {
                    ModalMaster modal = new ModalMaster(be, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, true, filtros);
                    modal.ShowDialog();
                }
                else
                {
                    ModalMaster modal = new ModalMaster(be, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, false, filtros);
                    modal.ShowDialog();
                }
            }
            finally
            {
                //this.IsModalFormOpened = false;
            }
        }
        #endregion

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Enter(this, this.documentID, this._frmType, this.frmModule);
              
                FormProvider.Master.itemNew.Visible = true;
                FormProvider.Master.itemSave.Visible = true;
                FormProvider.Master.itemSearch.Visible = true;
                FormProvider.Master.itemUpdate.Visible = true;
                FormProvider.Master.itemDelete.Visible = false;
                FormProvider.Master.itemSendtoAppr.Visible = false;
                FormProvider.Master.itemCopy.Visible = false;
                FormProvider.Master.itemPaste.Visible = false;
                FormProvider.Master.itemImport.Visible = false;
                FormProvider.Master.itemExport.Visible = false;
                FormProvider.Master.itemRevert.Visible = false;
                FormProvider.Master.itemGenerateTemplate.Visible = false;
                FormProvider.Master.itemFilter.Visible = false;
                FormProvider.Master.itemFilterDef.Visible = false;
                FormProvider.Master.tbBreak1.Visible = false;
                FormProvider.Master.tbBreak2.Visible = false;
                FormProvider.Master.itemPrint.Visible = true;
                if (FormProvider.Master.LoadFormTB)
                {                  
                    FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
                    FormProvider.Master.itemNew.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);
                    FormProvider.Master.itemSearch.Enabled = true;
                    FormProvider.Master.itemUpdate.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CobroJuridicoGral.cs", "Form_Enter"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Leave(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Leave(this, this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentForm", "Form_Leave"));
            }
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
                FormProvider.Master.Form_Closing(this, this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAprobBasicForm.cs", "Form_Closing"));
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
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this.frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentForm", "Form_FormClosed"));
            }
        }


        #endregion

        #region Eventos Header

        /// <summary>
        /// Evento que se ejecuta al momento de salir del cliente
        /// </summary>
        private void masterCliente_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.masterCliente.ValidID && this.lkpObligaciones.EditValue == null)
                {
                     this.clienteID = this.masterCliente.Value;

                    DTO_ccCliente cliente = (DTO_ccCliente)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCliente, false, this.masterCliente.Value, true);
                    this.creditosCliente = _bc.AdministrationModel.GetCreditosPendientesByCliente(this.masterCliente.Value);

                    if (this.creditosCliente.Count == 0)
                    {
                      
                    }
                    else
                    {
                        this.gcDocument.DataSource = null;
                        this.gcDocument.RefreshDataSource();
                        this.lkpObligaciones.Properties.DataSource = this.creditosCliente;
                    }
                }
                else
                {
                    if (!this.masterCliente.ValidID)
                    {
                        string msg = String.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCliente.LabelRsx);
                        MessageBox.Show(msg);
                        this.CleanData();
                    }                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CobroJuridicoGral.cs", "masterCliente_Leave"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInteres_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.masterCliente.ValidID && this._currentCredito != null)
                {
                    //this._bc.AdministrationModel.ccCJHistorico_RecalcularInteresCJ(this.dtFechaDoc.DateTime.Date, this._currentCredito.Libranza.Value.Value);
                    //this.LoadDocuments();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CobroJuridico.cs", "btnInteres_Click"));
            }
        }
            
        /// <summary>
        /// Cambio de solicitud
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lkpObligaciones_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.masterCliente.ValidID && !String.IsNullOrWhiteSpace(this.lkpObligaciones.EditValue.ToString()))
                {
                    int numeroDoc = Convert.ToInt32(this.lkpObligaciones.EditValue.ToString());
                    this._currentCredito = this.creditosCliente.Find(x => x.NumeroDoc.Value == numeroDoc);
                    this.LoadDocuments();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RecaudosManuales.cs", "lkpObligaciones_EditValueChanged"));
            }
        }

        /// <summary>
        /// Al seleccionar un item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbPlazoSent_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Al cambiar el item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbPlazoSent_TextChanged(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(this.cmbPlazoSent.Text) && Convert.ToInt32(this.cmbPlazoSent.Text) != 0)
            {
                this.txtValorCuota.EditValue = this._credComponentesGrid.Where(y=>y.TipoComponente.Value == 1 || y.TipoComponente.Value == 4).Sum(x => x.VlrCausado.Value)/Convert.ToInt32(this.cmbPlazoSent.Text);
                this.txtValorCuota.EditValue = Math.Round(Convert.ToDecimal(this.txtValorCuota.EditValue), 0);
            }
        }
        #endregion

        #region Eventos grilla

        /// <summary>
        /// Evento que se ejecuta al presionar un boton por defecto de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gcDocuments_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            try
            {
                if (this.masterCliente.ValidID && this._credComponentesGrid.Count > 0)
                {
                    if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Custom)
                    {
                        this.gvDocument.PostEditor();
                        this.AddComponente();
                    }
                    else if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Remove)
                    {
                        this.gvDocument.PostEditor();
                        DTO_ccCreditoComponentes comp = (DTO_ccCreditoComponentes)this.gvDocument.GetRow(this.gvDocument.FocusedRowHandle);
                        string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Delete);
                        string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Register);
                        if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            this._credComponentesGrid.RemoveAll(x=>x.ComponenteCarteraID.Value == comp.ComponenteCarteraID.Value);
                            //this._currentCredito.Detalle.RemoveAll(x => x.ComponenteCarteraID.Value == comp.ComponenteCarteraID.Value); 
                        }
                        e.Handled = true;
                    }
                }
                else
                    e.Handled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RecaudosManuales.cs", "gcDetails_EmbeddedNavigator_ButtonClick"));
            }
        }
        
        /// <summary>
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                GridView view = (GridView)sender;
                DTO_ccCreditoComponentes comp = (DTO_ccCreditoComponentes)view.GetRow(e.FocusedRowHandle);
                if (comp != null)
                {
                    if (comp.TotalValor.Value == 0)
                    {
                        this.gvDocument.Columns[this.unboundPrefix + "ComponenteCarteraID"].OptionsColumn.AllowEdit = true;
                        this.gvDocument.Columns[this.unboundPrefix + "ComponenteCarteraID"].OptionsColumn.AllowFocus = true;
                    }
                    else
                        this.gvDocument.Columns[this.unboundPrefix + "ComponenteCarteraID"].OptionsColumn.AllowEdit = false;
                }
                this.gvDocument.PostEditor();
                this.gvDocument.RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CobroJuridicoGral.cs", "gvDocument_FocusedRowChanged"));
            }
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                if (fieldName == "Marca" && e.Value == null)
                    e.Value = this.select.Contains(e.ListSourceRowIndex);
                else
                {
                    PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (pi != null)
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                            e.Value = pi.GetValue(dto, null);
                        else
                            e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
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
                    }
                }
            }
        }

        /// <summary>
        /// Asigna texto por defecto para la columna de archivos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_BeforeLeaveRow(object sender, RowAllowEventArgs e)
        {
            try
            {
                GridView view = (GridView)sender;
                DTO_ccCreditoComponentes comp = (DTO_ccCreditoComponentes)view.GetRow(e.RowHandle);

                this.gvDetalle.PostEditor();
                this.gvDocument.RefreshData();
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
        private void gvDocument_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            GridView view = (GridView)sender;
            DTO_ccCreditoComponentes comp = (DTO_ccCreditoComponentes)view.GetRow(e.RowHandle);           
            if (fieldName == "VlrCausado")
            {
              

            }
            if (fieldName == "ComponenteCarteraID")
            {
                DTO_ccCarteraComponente dto = (DTO_ccCarteraComponente)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCarteraComponente, false, e.Value.ToString(), true, null);
                comp.Descripcion.Value = dto != null? dto.Descriptivo.Value : string.Empty;
            }

            this.gvDocument.PostEditor();
            this.gvDocument.RefreshData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gcDocument_Enter(object sender, EventArgs e)
        {
            if (this._currentCredito != null && this._currentCredito.TipoEstado.Value == (byte)TipoEstadoCartera.Cedida)
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, "No puede procesar un crédito cedido"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editbtn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                DTO_ccCreditoComponentes comp = (DTO_ccCreditoComponentes)this.gvDocument.GetRow(this.gvDocument.FocusedRowHandle);
                if (comp != null)
                {
                    comp.VlrCausado.Value = comp.TotalValor.Value;
                }
                this.gvDocument.RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SentenciaJudicialGral.cs", "editbtn_ButtonClick"));
            }

        }

        /// <summary>
        /// Ocurre al presionar un botos para traer una FK
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void editBtnGrid_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                string colName = this.gvDocument.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);

                ButtonEdit origin = (ButtonEdit)sender;
                this.ShowFKModal(this.gvDocument.FocusedRowHandle, colName, origin,null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SentenciaJudicialGral.cs", "editBtnGrid_ButtonClick"));
            }
        }
        #endregion

        #region Eventos Barra Herramientas

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBNew()
        {
            try
            {
                this.CleanData();
                this.masterCliente.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CobroJuridicoGral.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            this.gvDocument.PostEditor();
            try
            {
                if (this.masterCliente.ValidID)
                {
                    if (this._currentCredito == null && !this._currentCredito.Editable.Value.Value)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, "Debe seleccionar un crédito para guardar"));
                        return;
                    }
                    else if(this._currentCredito.TipoEstado.Value == (byte)TipoEstadoCartera.Cedida)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, "No puede procesar un crédito cedido"));
                        return;
                    }
                    else if (this._currentCredito.TipoEstado.Value == (byte)TipoEstadoCartera.Propia)
                    {
                        if (string.IsNullOrEmpty(this.cmbPlazoSent.Text))
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, "Por favor seleccione un plazo para las cuotas pactadas"));
                            return;
                        }
                        else if (Convert.ToDecimal(this.txtValorCuota.EditValue) == 0)
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, "Por favor digite un valor de cuota diferente de $0 para las cuotas pactadas"));
                            return;
                        }
                        this._currentCredito.VlrCuotaSentencia.Value = Convert.ToDecimal(this.txtValorCuota.EditValue);
                        this._currentCredito.FechaCuota1Sentencia.Value = Convert.ToDateTime(this.dtFecha1CuotSent.EditValue);
                        this._currentCredito.PlazoSentencia.Value = Convert.ToInt16(this.cmbPlazoSent.Text);
                    }

                    this._currentCredito.Detalle = this._credComponentesGrid;

                    Thread process = new Thread(this.SaveThread);
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SentenciaJudicial.cs", "TBSave"));
            }
        }

        /// <summary>
        /// Boton para filtrar la información en pantalla
        /// </summary>
        public override void TBSearch()
        {
            if(this.masterCliente.ValidID)
                this.LoadDocuments();
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBUpdate()
        {
            this.LoadDocuments();
        }

        /// <summary>
        /// Boton para imprimir reporte
        /// </summary>
        public override void TBPrint()
        {
            try
            {
                if(this.cobrosJuridicosTemp.Count > 0)
                {
                    //DTO_ccCreditoDocu cred = (DTO_ccCreditoDocu)this.gvDocument.GetRow(this.gvDocument.FocusedRowHandle);
                    //byte claseDeuda = Convert.ToByte(this.cmbClaseDeuda.EditValue);
                    //string reportName = this._bc.AdministrationModel.Report_Cc_CobroJuridicoHistoria(this.documentID,cred.NumeroDoc.Value.Value,claseDeuda);
                    //string fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName);
                    //Process.Start(fileURl);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CobroJUridico.cs", "TBPrint"));
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo que se ejecuta al enviar a aprobacion
        /// </summary>
        public void SaveThread()
        {
            try
            {
                #region Guarda la info
                List<DTO_SerializedObject> results = null;

                    FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                    FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCartera(this.documentID));

                    ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                    FormProvider.Master.ProgressBarThread = new Thread(pth);
                    FormProvider.Master.ProgressBarThread.Start(this.documentID);
                    this._currentCredito.Detalle = this._credComponentesGrid;

                //Guarda la info
                TipoEstadoCartera estActual = (TipoEstadoCartera)Enum.Parse(typeof(TipoEstadoCartera), this.cmbEstadoActual.EditValue.ToString());
                results = _bc.AdministrationModel.SentenciaJudicial_Add(this.documentID, this._actFlujo.ID.Value,this._currentCredito, estActual, this.dtFechaDoc.DateTime,this.dtFechaSentencia.DateTime);
                FormProvider.Master.StopProgressBarThread(this.documentID);
                this._currentCredito.Editable.Value = false;
                #endregion
                if (results != null)
                {
                    #region Carga los resultados
                    int i = 0;
                    int percent = 0;
                    List<DTO_TxResult> resultsNOK = new List<DTO_TxResult>();
                    FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SendingMails) });

                    this.isValid = true;
                    MessageForm frm = null;
                    bool checkResults = true;
                    if (results.Count == 1)
                    {
                        if (results[0].GetType() == typeof(DTO_TxResult))
                        {
                            checkResults = false;
                            frm = new MessageForm((DTO_TxResult)results[0]);
                            this.isValid = false;
                        }
                    }
                    #endregion
                    #region Envia correos y carga los resultados
                    if (checkResults)
                    {
                        foreach (object obj in results)
                        {
                            //#region Funciones de progreso
                            //FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                            //percent = ((i + 1) * 100) / (results.Count);

                            //if (FormProvider.Master.ProcessCanceled(this.documentID))
                            //{
                            //    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser));
                            //    break;
                            //}
                            //#endregion

                            ////if (this.cobrosJuridicos[i].Aprobado.Value.Value)
                            ////{
                            //    bool isOK = this._bc.SendDocumentMail(MailType.SendToApprove, AppDocuments.CobroJuridico, this._actFlujo.seUsuarioID.Value, obj, false);
                            //    if (!isOK)
                            //    {
                            //        DTO_TxResult r = (DTO_TxResult)obj;
                            //        resultsNOK.Add(r);
                            //        this.isValid = false;
                            //    }
                            ////}

                            //i++;
                        }

                        frm = new MessageForm(resultsNOK);
                    }
                    #endregion

                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                    if (this.isValid)
                        this.Invoke(this.refreshGridDelegate);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SentenciaJudicial.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }


        #endregion

    
    }

}
