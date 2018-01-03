using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using NewAge.DTO.Negocio;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using System.Text.RegularExpressions;
using SentenceTransformer;
using DevExpress.XtraGrid.Columns;
using System.Threading;
using DevExpress.XtraEditors;
using DevExpress.Data;
using System.Globalization;
using NewAge.DTO.Attributes;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// SolicitudDirecta
    /// </summary>
    public partial class SolicitudDirecta : DocumentProvForm
    {
        public SolicitudDirecta()
        {
          //InitializeComponent();
        }

        #region Delegados

        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected override void SaveMethod()
        {
            this.disableValidate = true;
            this.gcDocument.DataSource = this.data.Footer;
            this.disableValidate = false;
            this.CleanFooter();

            this.CleanHeader(true);
            this.EnableHeader(false);
            this.masterPrefijo.EnableControl(true);
            this.masterPrefijo.Focus();
            this.txtSolicitudNro.Enabled = true;
            this.EnableFooter(false);
            this.gcCargos.Enabled = false;
            this.btnQueryDoc.Enabled = true;
            FormProvider.Master.itemSendtoAppr.Enabled = false;
        }

        /// <summary>
        /// Delegado que actualiza el formulario despues de enviar un documento para aprobacion
        /// </summary>
        protected override void SendToApproveMethod()
        {
            this.disableValidate = true;
            this.gcDocument.DataSource = this.data.Footer;
            this.disableValidate = false;
            this.CleanFooter();

            this.CleanHeader(true);
            this.EnableHeader(false);
            this.masterPrefijo.EnableControl(true);
            this.masterPrefijo.Focus();
            this.txtSolicitudNro.Enabled = true;
            this.btnQueryDoc.Enabled = true;
            this.EnableFooter(false);
            this.gcCargos.Enabled = false;

            FormProvider.Master.itemSendtoAppr.Enabled = false;
        }

        #endregion

        #region Variables formulario

        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        private DTO_glDocumentoControl _ctrl = null;
        private DTO_prSolicitudDirectaDocu _solHeader = null;
        private List<DTO_prSolicitudFooter> _solFooter = null;       
        private DTO_seUsuario _usuario = null;

        private bool _headerLoaded = false;
        private bool _prefijoFocus = false;
        private bool _txtSolicitudNroFocus = false;
        private bool _copyData = false;
        private bool moduleProyectoActive = false;

        private decimal valorTotalDoc = 0;
        private decimal valorIVATotalDoc = 0;
        private Dictionary<string, DTO_prBienServicio> cacheBienServ = new Dictionary<string, DTO_prBienServicio>();
        private Dictionary<string, DTO_glBienServicioClase> cacheBienServClase = new Dictionary<string, DTO_glBienServicioClase>();
        private Dictionary<string, DTO_coPlanCuenta> cacheCuentas = new Dictionary<string, DTO_coPlanCuenta>();
        private string _regimenFiscalEmp = string.Empty;
        private string _regimenFiscalTercero = string.Empty;
        #endregion

        #region Propiedades

        /// <summary>
        /// Variable que maneja la informacion de los temporales
        /// </summary>
        private DTO_prSolicitud TempData
        {
            get
            {
                return (DTO_prSolicitud)this.data;
            }
            set
            {
                this.data = value;
            }
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Funcion para validacion de fechas
        /// </summary>
        private void ValidateDates()
        {
            int currentMonth = this.dtPeriod.DateTime.Month;
            int currentYear = this.dtPeriod.DateTime.Year;
            int minDay = 1;
            int lastDay = DateTime.DaysInMonth(currentYear, currentMonth);

            this.dtFechaSol.Properties.MinValue = new DateTime(currentYear, currentMonth, minDay);
            //this.dtFechaSol.Properties.MaxValue = new DateTime(currentYear, currentMonth, lastDay);
            //this.dtFechaSol.DateTime = new DateTime(currentYear, currentMonth, minDay);
        }

        /// <summary>
        /// Valida un DTO de comprobante footer en la importacion
        /// </summary>
        /// <param name="ctrl">glDocumentoControl a validar</param>
        /// <param name="suppl">DTO Sumplementario a validar</param>
        /// <param name="rd">Variable que va guardando los mensajes de error</param>
        /// <param name="msgInvalidDate">Fecha en periodo invalido</param>
        /// <param name="msgFkNotFound">FK inexistente</param>
        /// <param name="msgFkHierarchyFather">No es una hoja de la jerarquia</param>
        /// <param name="msgCero">Mensaje que no permite ceros en un campo</param>
        /// <param name="msgPositive">Solo permite valores positivos</param>
        private void ValidateDataImport(object dto, DTO_glBienServicioClase bsClase, DTO_TxResultDetail rd, string msgFkNotFound, string msgInvalidField)
        {
            string colRsx;
            DTO_TxResultDetailFields rdF;
            Type dataType = dto.GetType();

            bool createDTO = true;

            if (dataType == typeof(DTO_prDetalleDocu))
            {
                DTO_prDetalleDocu dtoDet = (DTO_prDetalleDocu)dto;
                #region Variables y diccionarios
                TipoCodigo tipoCodigo = (TipoCodigo)Enum.Parse(typeof(TipoCodigo), bsClase.TipoCodigo.Value.Value.ToString());

                #endregion
                #region Valida las FKs
                #region CodigoBSID
                colRsx = base._codigoRsx;
                rdF = _bc.ValidGridCell(colRsx, dtoDet.CodigoBSID.Value, false, true, false, AppMasters.prBienServicio);
                if (rdF != null)
                {
                    rdF.Message = string.Format(msgFkNotFound, colRsx);
                    rd.DetailsFields.Add(rdF);
                    createDTO = false;
                }
                #endregion
                #region inReferenciaID
                colRsx = base._referenciaRsx;
                if (base._tipoCodigo == TipoCodigo.Servicio || base._tipoCodigo == TipoCodigo.Suministros || base._tipoCodigo == TipoCodigo.SuministroPersonal)
                {
                    if (!string.IsNullOrEmpty(dtoDet.inReferenciaID.Value.Trim()))
                    {
                        rdF = new DTO_TxResultDetailFields();
                        rdF.Field = colRsx;
                        rdF.Message = string.Format(msgInvalidField, colRsx);
                        rd.DetailsFields.Add(rdF);
                        createDTO = false;
                    }
                }
                else
                {
                    rdF = new DTO_TxResultDetailFields();
                    rdF.Field = colRsx;
                    rdF.Message = string.Format("El tipo de Codigo solo puede ser de Servicio o Suministros", colRsx);
                    rd.DetailsFields.Add(rdF);
                    createDTO = false;
                }
                #endregion
                #region UnidadInvID
                colRsx = base._unidadRsx;
                rdF = _bc.ValidGridCell(colRsx, dtoDet.UnidadInvID.Value, false, true, false, AppMasters.inUnidad);
                if (rdF != null)
                {
                    rdF.Message = string.Format(msgFkNotFound, colRsx);
                    rd.DetailsFields.Add(rdF);
                    createDTO = false;
                }
                #endregion
                #endregion
                #region Cantidad
                colRsx = base._cantidadRsx;
                rdF = _bc.ValidGridCellValue(colRsx, dtoDet.CantidadDoc5.Value.ToString(), false, false, true, false);
                if (rdF != null)
                {
                    rdF.Message = string.Format(msgInvalidField, colRsx);
                    rd.DetailsFields.Add(rdF);
                    createDTO = false;
                } 
                #endregion
                #region ValorUni
                colRsx = base._valorUniRsx;
                rdF = _bc.ValidGridCellValue(colRsx, dtoDet.ValorUni.Value.ToString(), false, false, true, false);
                if (rdF != null)
                {
                    rdF.Message = string.Format(msgInvalidField, colRsx);
                    rd.DetailsFields.Add(rdF);
                    createDTO = false;
                }
                else
                {
                    if (dtoDet.CantidadDoc5.Value != null)
                    {
                        decimal tc = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);
                        dtoDet.ValorTotML.Value = Math.Round(dtoDet.ValorUni.Value.Value * dtoDet.CantidadDoc5.Value.Value,0);                      
                        if (base.multiMoneda && tc != 0)
                            dtoDet.ValorTotME.Value = dtoDet.ValorTotML.Value / tc;
                        #region Asigna el valor del IVA si existe el impuesto
                        #region Consulta para la tabla de los impuestos
                        DTO_glConsulta consulta = new DTO_glConsulta();
                        List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();
                        DTO_glConsultaFiltro filtro;

                        filtro = new DTO_glConsultaFiltro();
                        filtro.CampoFisico = "RegimenFiscalEmpresaID";
                        filtro.ValorFiltro = this._regimenFiscalEmp;
                        filtro.OperadorFiltro = OperadorFiltro.Igual;
                        filtro.OperadorSentencia = "and";
                        filtros.Add(filtro);

                        filtro = new DTO_glConsultaFiltro();
                        filtro.CampoFisico = "RegimenFiscalTerceroID";
                        filtro.ValorFiltro = this._regimenFiscalTercero;
                        filtro.OperadorFiltro = OperadorFiltro.Igual;
                        filtro.OperadorSentencia = "and";
                        filtros.Add(filtro);

                        filtro = new DTO_glConsultaFiltro();
                        filtro.CampoFisico = "ImpuestoTipoID";
                        filtro.ValorFiltro = this._bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoIVA);
                        filtro.OperadorFiltro = OperadorFiltro.Igual;
                        filtro.OperadorSentencia = "and";
                        filtros.Add(filtro);

                        filtro = new DTO_glConsultaFiltro();
                        filtro.CampoFisico = "ConceptoCargoID";
                        filtro.ValorFiltro = bsClase.ConceptoCargoID.Value;
                        filtro.OperadorFiltro = OperadorFiltro.Igual;
                        filtro.OperadorSentencia = "and";
                        filtros.Add(filtro);

                        consulta.Filtros = filtros;
                        long count = this._bc.AdministrationModel.MasterComplex_Count(AppMasters.coImpuesto, consulta, null);
                        #endregion
                        if (count > 0)
                        {
                            #region Consulta la Cuenta y asigna el Impuesto
                            var listCoImp = _bc.AdministrationModel.MasterComplex_GetPaged(AppMasters.coImpuesto, count, 1, consulta, null);
                            DTO_coImpuesto mastercoImp = listCoImp.Cast<DTO_coImpuesto>().ToList().First();
                            DTO_coPlanCuenta cta = (DTO_coPlanCuenta)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, false, mastercoImp.CuentaID.Value, true);
                            dtoDet.PorcentajeIVA.Value = cta.ImpuestoPorc.Value != null ? cta.ImpuestoPorc.Value.Value : 0;
                            dtoDet.IVAUni.Value = (dtoDet.ValorUni.Value * dtoDet.PorcentajeIVA.Value) / 100;
                            dtoDet.IvaTotML.Value = Math.Round(dtoDet.IVAUni.Value.Value * dtoDet.CantidadDoc5.Value.Value,0);
                            if (base.multiMoneda && tc != 0)
                                dtoDet.IvaTotME.Value = dtoDet.IvaTotML.Value / tc;
                            #endregion
                        }
                        else
                        {
                            dtoDet.IVAUni.Value = 0;
                            dtoDet.IvaTotML.Value = 0; 
                        }
                        #endregion
                    }

                }
                #endregion                
            }

            if (dataType == typeof(DTO_prSolicitudCargos))
            {
                DTO_prSolicitudCargos dtoCarg = (DTO_prSolicitudCargos)dto;
                #region Valida las FKs
                #region ProyectoID
                colRsx = base._proyectoRsx;
                rdF = _bc.ValidGridCell(colRsx, dtoCarg.ProyectoID.Value, false, true, false, AppMasters.coProyecto);
                if (rdF != null)
                {
                    rdF.Message = string.Format(msgFkNotFound, colRsx);
                    rd.DetailsFields.Add(rdF);
                    createDTO = false;
                }
                #endregion
                #region CentroCostoID
                colRsx = base._centroCostoRsx;
                rdF = _bc.ValidGridCell(colRsx, dtoCarg.CentroCostoID.Value, false, true, true, AppMasters.coCentroCosto);
                if (rdF != null)
                {
                    rdF.Message = string.Format(msgFkNotFound, colRsx);
                    rd.DetailsFields.Add(rdF);
                    createDTO = false;
                }
                #endregion
                #endregion
                colRsx = base._porcentajeRsx;
                rdF = _bc.ValidGridCellValue(colRsx, dtoCarg.PorcentajeID.Value.Value.ToString(), false, false, true, false);
                if (rdF != null)
                {
                    rdF.Message = string.Format(msgInvalidField, colRsx);
                    rd.DetailsFields.Add(rdF);
                    createDTO = false;
                }
            }
        }

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Valida una fila
        /// </summary>
        /// <param name="fila">Indice de la fila a validar</param>
        /// <returns>Retorna si la info de la fila es valida o no</returns>
        protected override bool ValidateRow(int fila)
        {
            bool validRow = true;
            bool validField = true;

            try
            {
                GridColumn col = new GridColumn();

                #region Validacion de nulls y Fks
                string rsxEmpty = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                #region CodigoBSID
                validField = _bc.ValidGridCell(this.gvDocument, string.Empty, fila, "CodigoBSID", false, true, true, AppMasters.prBienServicio);
                if (!validField)
                    validRow = false;
                #endregion
                #region Descriptivo
                validField = _bc.ValidGridCell(this.gvDocument, string.Empty, fila, "Descriptivo", false, false, false, null);
                if (!validField)
                    validRow = false;
                #endregion
                #region UnidadInvID
                validField = _bc.ValidGridCell(this.gvDocument, string.Empty, fila, "UnidadInvID", false, true, false, AppMasters.inUnidad);
                if (!validField)
                    validRow = false;
                #endregion
                #region Proyecto
                validField = _bc.ValidGridCell(this.gvDocument, string.Empty, fila, "ProyectoID", false, true, true, AppMasters.coProyecto);
                if (!validField)
                    validRow = false;
                #endregion
                #region Centro Costo
                validField = _bc.ValidGridCell(this.gvDocument, string.Empty, fila, "CentroCostoID", false, true, true, AppMasters.coCentroCosto);
                if (!validField)
                    validRow = false;
                #endregion
                #endregion
                #region Validaciones de valores
                validField = _bc.ValidGridCellValue(this.gvDocument, string.Empty, fila, "CantidadDoc5", false, false, true, false);
                if (!validField)
                    validRow = false;
                validField = _bc.ValidGridCellValue(this.gvDocument, string.Empty, fila, "ValorUni", false, true, true, false);
                if (!validField)
                    validRow = false;            
                validField = _bc.ValidGridCellValue(this.gvDocument, string.Empty, fila, "IVAUni", false, true, true, false);
                if (!validField)
                    validRow = false;               
                #endregion

                if (validRow)
                {
                    this.isValid = true;
                    //this.CalcularTotal();

                    if (!this.newReg)
                        this.UpdateTemp(this.data);
                }
                else
                    this.isValid = false;

                this.hasChanges = true;
                return validRow;
            }
            catch (Exception ex)
            {
                validRow = false;
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Solicitud.cs", "ValidateRow"));
            }

            return validRow;
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.SolicitudDirecta;
            this.frmModule = ModulesPrefix.pr;
            InitializeComponent();

            base.SetInitParameters();

            this.data = new DTO_prSolicitud();
            this._ctrl = new DTO_glDocumentoControl();
            this._solHeader = new DTO_prSolicitudDirectaDocu();
            this._solFooter = new List<DTO_prSolicitudFooter>();

            List<DTO_glConsultaFiltro> filtrosLugarRecibido = new List<DTO_glConsultaFiltro>();
            filtrosLugarRecibido.Add(new DTO_glConsultaFiltro()
            {
                CampoFisico = "ServicioDirectoInd",
                OperadorFiltro = OperadorFiltro.Igual,
                ValorFiltro = "1"
            });
          
            this._bc.InitMasterUC(this.masterPrefijo, AppMasters.glPrefijo, true, true, true, false);
            this._bc.InitMasterUC(this.masterProveedor, AppMasters.prProveedor, true, true, true, false, filtrosLugarRecibido);
            this._bc.InitMasterUC(this.masterPrefijoProyecto, AppMasters.glPrefijo, true, true, true, false);

            this._valorUniRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorUni");
            this._ivaUniRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_IVAUni");
          
        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize()
        {
            base.AfterInitialize();
            string tercero =  this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
            DTO_coTercero dtoTerceroEmp = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, tercero, true);
            this._regimenFiscalEmp = dtoTerceroEmp.ReferenciaID.Value;          
                   
            var modules = this._bc.AdministrationModel.aplModulo_GetByVisible(1, false);
            bool controlSolicitudProyInd = this._bc.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_IndControlSolicitudesBS).Equals("1") ? true : false;
            if (modules.Any(x => x.ModuloID.Value == ModulesPrefix.py.ToString()) && controlSolicitudProyInd)
            {
                this.masterPrefijoProyecto.Visible = true;
                this.lblNroProyecto.Visible = true;
                this.lblPrefijoProy.Visible = true;
                this.txtProyectoNro.Visible = true;
                this.btnProyectoNro.Visible = true;
                this.moduleProyectoActive = true;
            }

            this.EnableFooter(false);
            this.EnableHeader(false);

            if (!this._headerLoaded)
            {
                this.txtNumeroDoc.Text = "0";
                this.masterPrefijo.EnableControl(true);
                this.txtSolicitudNro.Enabled = true;
                if (string.IsNullOrEmpty(this.txtSolicitudNro.Text)) this.txtSolicitudNro.Text = "0";           
                this.ValidateDates();
            }
            this.tlSeparatorPanel.RowStyles[0].Height = 32; 
            this.tlSeparatorPanel.RowStyles[1].Height = 57;

            #region Import Format
            this.format = (_bc.GetResource(LanguageTypes.Forms, "71_Index") + this.formatSeparator);
            foreach (GridColumn col in this.gvDocument.Columns)
            {
                if (!col.FieldName.Contains("NumeroDoc") && !col.FieldName.Contains("inReferenciaID") && !col.FieldName.Contains("Parametro1") && !col.FieldName.Contains("Parametro2")
                     && !col.FieldName.Contains("Documento5ID") && !col.FieldName.Contains("ConsecutivoDetaID") && !col.FieldName.Contains("Detalle5ID") && !string.IsNullOrEmpty(col.FieldName.Trim())
                     && !col.FieldName.Contains("ValorTotML") && !col.FieldName.Contains("IvaTotML") && !col.FieldName.Contains("ValorTotME") && !col.FieldName.Contains("IvaTotME") && !col.FieldName.Contains("IVAUni"))
                {
                    this.format += (_bc.GetResource(LanguageTypes.Forms, col.Caption) + this.formatSeparator);
                }
            }
            #endregion
        }

        /// <summary>
        /// Limpia y deja vacio los controles del header
        /// </summary>
        /// <param name="basic">Indica si tambien se limpian los controles basicos</param>
        protected override void CleanHeader(bool basic)
        {
            if (basic)
            {
                string periodo = _bc.GetControlValueByCompany(this.frmModule, AppControl.pr_Periodo);
                this.dtPeriod.Text = periodo;
                this.prefijoID = string.Empty;
                this.masterPrefijo.Value = this.txtPrefix.Text;
                this.txtNumeroDoc.Text = "0";
                this.txtSolicitudNro.Text = "0";
            }
            
            this._usuario = new DTO_seUsuario(); 
            this.masterProveedor.Value = string.Empty;
            this.ValidateDates();
            this.txtDescDoc.Text = string.Empty;
            base.CleanHeader(basic);
        }

        /// <summary>
        /// Oculta los controles del formulario
        /// </summary>
        protected override void EnableHeader(bool enable)
        {
            this.dtFecha.Enabled = false;
            this.dtPeriod.Enabled = false;
            this.txtPrefix.Enabled = false;
            this.txtNumeroDoc.Enabled = false;

            this.masterPrefijo.EnableControl(enable);
            this.masterProveedor.EnableControl(enable);
            this.masterPrefijoProyecto.EnableControl(enable);

            this.txtSolicitudNro.Enabled = enable;
            this.txtProyectoNro.Enabled = enable;
            this.dtFechaSol.Enabled = enable;
            this.txtDescDoc.Enabled = enable;  
            this.btnProyectoNro.Enabled = enable;       
        }

        /// <summary>
        /// Carga informacion de un temporal a partir del cabezote
        /// </summary>
        /// <returns>Retorna el header de un temporal</returns>
        protected override DTO_prSolicitud LoadTempHeader()
        {
            #region Load DocumentoControl
            this._ctrl.EmpresaID.Value = this.empresaID;
            this._ctrl.TerceroID.Value = this.defTercero; ////
            this._ctrl.NumeroDoc.Value = Convert.ToInt32(this.txtNumeroDoc.Text);
            this._ctrl.ComprobanteID.Value = string.Empty; ////this.comprobanteID;
            this._ctrl.ComprobanteIDNro.Value = 0; 
            this._ctrl.MonedaID.Value = this.monedaLocal; ////
            this._ctrl.CuentaID.Value = string.Empty; ////
            this._ctrl.ProyectoID.Value = this.defProyecto; ////
            this._ctrl.CentroCostoID.Value = this.defCentroCosto; ////
            this._ctrl.LugarGeograficoID.Value = this.defLugarGeo; ////
            this._ctrl.LineaPresupuestoID.Value = this.defLineaPresupuesto;////
            this._ctrl.Fecha.Value = DateTime.Now;
            this._ctrl.PeriodoDoc.Value = this.dtPeriod.DateTime;
            this._ctrl.PrefijoID.Value = this.masterPrefijo.Value;
            this._ctrl.TasaCambioCONT.Value = 0;////
            this._ctrl.TasaCambioDOCU.Value = 0;////
            this._ctrl.DocumentoNro.Value = Convert.ToInt32(txtSolicitudNro.Text);
            this._ctrl.DocumentoID.Value = this.documentID;
            this._ctrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;////
            this._ctrl.PeriodoUltMov.Value = this.dtPeriod.DateTime;
            this._ctrl.seUsuarioID.Value = this.userID;
            this._ctrl.AreaFuncionalID.Value = this.areaFuncionalID;
            this._ctrl.ConsSaldo.Value = 0;
            this._ctrl.Estado.Value = (byte)EstadoDocControl.SinAprobar;
            this._ctrl.Observacion.Value = this.txtDescDoc.Text;
            this._ctrl.FechaDoc.Value = this.dtFechaSol.DateTime;
            this._ctrl.Descripcion.Value = this.txtDocDesc.Text;
            this._ctrl.Valor.Value = 0;
            this._ctrl.Iva.Value = 0;
            #endregion
            #region Load SolicitudHeader
            this._solHeader.EmpresaID.Value = this.empresaID;
            this._solHeader.NumeroDoc.Value = Convert.ToInt32(this.txtNumeroDoc.Text);
            this._solHeader.ProveedorID.Value = this.masterProveedor.Value;
            #endregion

            //this._tipoMonedaOr = this._ctrl.MonedaID.Value == this.monedaLocal ? TipoMoneda_LocExt.Local : TipoMoneda_LocExt.Foreign;
            this.monedaId = this._ctrl.MonedaID.Value;

            DTO_prSolicitud sol = new DTO_prSolicitud();
            sol.HeaderSolDirecta = this._solHeader;
            sol.DocCtrl = this._ctrl;
            sol.Footer = new List<DTO_prSolicitudFooter>();
            this._solFooter = sol.Footer;

            return sol;
        }
        
        /// <summary>
        /// valida la informacion del header
        /// </summary>
        protected override bool ValidateHeader()
        {
            #region Valida los datos obligatorios
            
            #region Valida datos en la maestra de Prefijo
            if (!this.masterPrefijo.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterPrefijo.CodeRsx);

                MessageBox.Show(msg);
                this.masterPrefijo.Focus();

                return false;
            }
            #endregion

            #region Valida datos en la maestra de Proveedor
            if (!this.masterProveedor.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterProveedor.CodeRsx);

                MessageBox.Show(msg);
                this.masterProveedor.Focus();

                return false;
            }
            #endregion            

            #region Valida datos en la fecha de la solicitud
            if (string.IsNullOrEmpty(this.dtFechaSol.Text))
            {
                string txtRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_lblFechaSol");
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), txtRsx);
                MessageBox.Show(msg);

                this.dtFechaSol.Focus();
                return false;
            }
            #endregion
            
            #region Valida datos en el descriptivo
            if (string.IsNullOrEmpty(this.txtDescDoc.Text))
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.lblDescDoc.Text);
                MessageBox.Show(msg);
                this.txtDescDoc.Focus();
                return false;
            }
            #endregion
            
            #region Valida datos del Modulo Proyectos
            if (this.moduleProyectoActive)
            {
                if (!this.masterPrefijoProyecto.ValidID)
                {
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.lblPrefijoProy.Text);
                    MessageBox.Show(msg);

                    this.masterPrefijoProyecto.Focus();
                    return false;
                }
                if (string.IsNullOrEmpty(this.txtProyectoNro.Text))
                {
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.lblNroProyecto.Text);
                    MessageBox.Show(msg);

                    this.txtProyectoNro.Focus();
                    return false;
                } 
            }
            #endregion
            #endregion
            return true;
        }

        /// <summary>
        /// Carga la informacion temporal del documento
        /// Si no tiene informacion del comprobante el temporal se guardo mal
        /// </summary>
        /// <param name="leg">Informacion del temporal</param>
        protected override void LoadTempData(DTO_prSolicitud sol)
        {
            DTO_glDocumentoControl ctrl = sol.DocCtrl;
            DTO_prSolicitudDirectaDocu solHeader = sol.HeaderSolDirecta;

            if (sol.Footer == null)
                sol.Footer = new List<DTO_prSolicitudFooter>();
            this._solFooter = sol.Footer;
            
            this.masterPrefijo.Value = ctrl.PrefijoID.Value;           
            this.txtSolicitudNro.Text = ctrl.DocumentoNro.Value.Value.ToString();
            this.dtFechaSol.DateTime = ctrl.FechaDoc.Value.Value;     
            this.txtDescDoc.Text = ctrl.Descripcion.Value;

            this.monedaId = ctrl.MonedaID.Value;

            this.dtPeriod.DateTime = ctrl.PeriodoDoc.Value.Value;
            this.txtNumeroDoc.Text = ctrl.NumeroDoc.Value.Value.ToString();
            this.dtFecha.DateTime = ctrl.FechaDoc.Value.Value;

            //Si se presenta un problema asignando la tasa de cambio lo bloquea
            if (this.ValidateHeader())
            {
                this.EnableHeader(false);
                this.data = sol;
                this._ctrl = sol.DocCtrl;
                this._solHeader = sol.HeaderSolDirecta;
                this._solFooter = sol.Footer;

                this.validHeader = true;
                this._headerLoaded = true;

                this.LoadData(true);
                this.gcDocument.Focus();
            }
            else
                this.CleanHeader(true);
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected override void AddGridCols()
        {
            try
            {
                #region Columnas basicas
                //CodigoServicios
                GridColumn codBS = new GridColumn();
                codBS.FieldName = this.unboundPrefix + "CodigoBSID";
                codBS.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CodigoBSID"); 
                codBS.UnboundType = UnboundColumnType.String;
                codBS.VisibleIndex = 1;
                codBS.Width = 70;
                codBS.Visible = true;
                codBS.Fixed = FixedStyle.Left;
                codBS.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(codBS);

                //Descriptivo
                GridColumn desc = new GridColumn();
                desc.FieldName = this.unboundPrefix + "Descriptivo";
                desc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descriptivo");
                desc.UnboundType = UnboundColumnType.String;
                desc.VisibleIndex = 2;
                desc.Width = 150;
                desc.Visible = true;
                desc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(desc);

                //UnidadInvID
                GridColumn unidad = new GridColumn();
                unidad.FieldName = this.unboundPrefix + "UnidadInvID";
                unidad.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_UnidadInvID");
                unidad.UnboundType = UnboundColumnType.String;
                unidad.VisibleIndex = 3;
                unidad.Width = 70;
                unidad.Visible = true;
                unidad.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(unidad);

                //Cantidad Solicitud
                GridColumn CantidadDoc5 = new GridColumn();
                CantidadDoc5.FieldName = this.unboundPrefix + "CantidadDoc5";
                CantidadDoc5.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CantidadDoc5");
                CantidadDoc5.UnboundType = UnboundColumnType.Integer;
                CantidadDoc5.VisibleIndex = 4;
                CantidadDoc5.Width = 60;
                CantidadDoc5.Visible = true;
                CantidadDoc5.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(CantidadDoc5);

                //Proyecto
                GridColumn proyecto = new GridColumn();
                proyecto.FieldName = this.unboundPrefix + "ProyectoID";
                proyecto.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProyectoID");
                proyecto.UnboundType = UnboundColumnType.String;
                proyecto.VisibleIndex = 5;
                proyecto.Width = 100;
                proyecto.Visible = true;
                proyecto.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(proyecto);

                //Centro de costo
                GridColumn ctoCosto = new GridColumn();
                ctoCosto.FieldName = this.unboundPrefix + "CentroCostoID";
                ctoCosto.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CentroCostoID");
                ctoCosto.UnboundType = UnboundColumnType.String;
                ctoCosto.VisibleIndex = 6;
                ctoCosto.Width = 100;
                ctoCosto.Visible = true;
                ctoCosto.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(ctoCosto);

                //ValorUni
                GridColumn valorUni = new GridColumn();
                valorUni.FieldName = this.unboundPrefix + "ValorUni";
                valorUni.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorUni");
                valorUni.UnboundType = UnboundColumnType.Decimal;
                valorUni.VisibleIndex = 7;
                valorUni.Width = 100;
                valorUni.Visible = true;
                valorUni.ColumnEdit = base.editSpin;
                valorUni.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(valorUni);

                //IvaUni
                GridColumn ivaUni = new GridColumn();
                ivaUni.FieldName = this.unboundPrefix + "IVAUni";
                ivaUni.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_IVAUni");
                ivaUni.UnboundType = UnboundColumnType.Decimal;
                ivaUni.VisibleIndex = 8;
                ivaUni.Width = 100;
                ivaUni.Visible = true;
                ivaUni.ColumnEdit = base.editSpin;
                ivaUni.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ivaUni);

                //ValorTotML
                GridColumn valorTotML = new GridColumn();
                valorTotML.FieldName = this.unboundPrefix + "ValorTotML";
                valorTotML.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorTotML");
                valorTotML.UnboundType = UnboundColumnType.Decimal;
                valorTotML.VisibleIndex = 9;
                valorTotML.Width = 140;
                valorTotML.Visible = true;
                valorTotML.ColumnEdit = base.editSpin;
                valorTotML.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(valorTotML);

                //IvaTotML
                GridColumn IvaTotML = new GridColumn();
                IvaTotML.FieldName = this.unboundPrefix + "IvaTotML";
                IvaTotML.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_IvaTotML");
                IvaTotML.UnboundType = UnboundColumnType.Decimal;
                IvaTotML.VisibleIndex = 10;
                IvaTotML.Width = 100;
                IvaTotML.Visible = true;
                IvaTotML.ColumnEdit = base.editSpin;
                IvaTotML.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(IvaTotML);

                #endregion
                #region Columnas No Visibles

                //CodigoReferencia
                GridColumn codRef = new GridColumn();
                codRef.FieldName = this.unboundPrefix + "inReferenciaID";
                codRef.UnboundType = UnboundColumnType.String;
                codRef.Visible = false;
                this.gvDocument.Columns.Add(codRef);

                //Parameter1
                GridColumn param1 = new GridColumn();
                param1.FieldName = this.unboundPrefix + "Parametro1";
                param1.UnboundType = UnboundColumnType.String;           
                param1.Visible = false;
                this.gvDocument.Columns.Add(param1);

                //Parameter2
                GridColumn param2 = new GridColumn();
                param2.FieldName = this.unboundPrefix + "Parametro2";           
                param2.UnboundType = UnboundColumnType.String;
                param2.Visible = false;
                this.gvDocument.Columns.Add(param2);

                //NumeroDoc
                GridColumn numDoc = new GridColumn();
                numDoc.FieldName = this.unboundPrefix + "NumeroDoc";
                numDoc.UnboundType = UnboundColumnType.Integer;
                numDoc.Visible = false;
                this.gvDocument.Columns.Add(numDoc);

                //Documento5ID
                GridColumn Documento5ID = new GridColumn();
                Documento5ID.FieldName = this.unboundPrefix + "Documento5ID"; //SolicitudDirectaDocu
                Documento5ID.UnboundType = UnboundColumnType.Integer;
                Documento5ID.Visible = false;
                this.gvDocument.Columns.Add(Documento5ID);

                //ConsecutivoDetaID
                GridColumn consDeta = new GridColumn();
                consDeta.FieldName = this.unboundPrefix + "ConsecutivoDetaID";
                consDeta.UnboundType = UnboundColumnType.Integer;
                consDeta.Visible = false;
                this.gvDocument.Columns.Add(consDeta);

                //SolicitudDetaID
                GridColumn solDeta = new GridColumn();
                solDeta.FieldName = this.unboundPrefix + "Detalle5ID";//Detalle Docu
                solDeta.UnboundType = UnboundColumnType.Integer;
                solDeta.Visible = false;
                this.gvDocument.Columns.Add(solDeta);

                //valorTotME
                GridColumn valorTotME = new GridColumn();
                valorTotME.FieldName = this.unboundPrefix + "ValorTotME";
                valorTotME.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorTotME");
                valorTotME.UnboundType = UnboundColumnType.Decimal;
                valorTotME.VisibleIndex = 11;
                valorTotME.Width = 120;
                valorTotME.Visible = false;
                valorTotME.ColumnEdit = base.editSpin;
                valorTotME.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(valorTotME);

                //IvaTotME
                GridColumn IvaTotME = new GridColumn();
                IvaTotME.FieldName = this.unboundPrefix + "IvaTotME";
                IvaTotME.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_IvaTotME");
                IvaTotME.UnboundType = UnboundColumnType.Decimal;
                IvaTotME.VisibleIndex = 12;
                IvaTotME.Width = 100;
                IvaTotME.Visible = false;
                IvaTotME.ColumnEdit = base.editSpin;
                IvaTotME.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(IvaTotME);

                //Indice de la fila
                GridColumn colIndex = new GridColumn();
                colIndex.FieldName = this.unboundPrefix + "Index";
                colIndex.UnboundType = UnboundColumnType.Integer;
                colIndex.Visible = false;
                this.gvDocument.Columns.Add(colIndex);
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentoProvForm.cs", "AddGridCols"));
            }
        }
   
        /// <summary>
        /// Calcula valores 
        /// </summary>
        protected override void GetValuesDocument(int index)
        {
            #region Variables
            DTO_prBienServicio _bienServicio;
            DTO_glBienServicioClase _bienServClase;
            DTO_coPlanCuenta _cuenta;
            this.valorTotalDoc = 0;
            this.valorIVATotalDoc = 0;
            #endregion

            try
            {
                #region Consulta el BienServicio y la Clase Bien Servicio
                if (cacheBienServ.ContainsKey(this.data.Footer[index].DetalleDocu.CodigoBSID.Value))
                    _bienServicio = cacheBienServ[this.data.Footer[index].DetalleDocu.CodigoBSID.Value];
                else
                {
                    _bienServicio = (DTO_prBienServicio)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.prBienServicio, false, this.data.Footer[index].DetalleDocu.CodigoBSID.Value, true);
                    cacheBienServ.Add(this.data.Footer[index].DetalleDocu.CodigoBSID.Value, _bienServicio);
                }

                if (cacheBienServClase.ContainsKey(_bienServicio.ClaseBSID.Value))
                    _bienServClase = cacheBienServClase[_bienServicio.ClaseBSID.Value];
                else
                {
                    _bienServClase = (DTO_glBienServicioClase)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, false, _bienServicio.ClaseBSID.Value, true);
                    cacheBienServClase.Add(_bienServicio.ClaseBSID.Value, _bienServClase);
                } 
                #endregion

                #region Consulta para la tabla de los impuestos
                DTO_glConsulta consulta = new DTO_glConsulta();
                List<DTO_glConsultaFiltro>  filtros = new List<DTO_glConsultaFiltro>();
                DTO_glConsultaFiltro filtro;

                filtro = new DTO_glConsultaFiltro();
                filtro.CampoFisico = "RegimenFiscalEmpresaID";
                filtro.ValorFiltro = this._regimenFiscalEmp;
                filtro.OperadorFiltro = OperadorFiltro.Igual;
                filtro.OperadorSentencia = "and";
                filtros.Add(filtro);

                filtro = new DTO_glConsultaFiltro();
                filtro.CampoFisico = "RegimenFiscalTerceroID";
                filtro.ValorFiltro = this._regimenFiscalTercero;
                filtro.OperadorFiltro = OperadorFiltro.Igual;
                filtro.OperadorSentencia = "and";
                filtros.Add(filtro);

                filtro = new DTO_glConsultaFiltro();
                filtro.CampoFisico = "ImpuestoTipoID";
                filtro.ValorFiltro = this._bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoIVA);
                filtro.OperadorFiltro = OperadorFiltro.Igual;
                filtro.OperadorSentencia = "and";
                filtros.Add(filtro);

                filtro = new DTO_glConsultaFiltro();
                filtro.CampoFisico = "ConceptoCargoID";
                filtro.ValorFiltro = _bienServClase.ConceptoCargoID.Value;
                filtro.OperadorFiltro = OperadorFiltro.Igual;
                filtro.OperadorSentencia = "and";
                filtros.Add(filtro);

                //filtro = new DTO_glConsultaFiltro();
                //filtro.CampoFisico = "LugarGeograficoID";
                //filtro.ValorFiltro = this.data.DocCtrl.LugarGeograficoID.Value;
                //filtro.OperadorFiltro = OperadorFiltro.Igual;
                //filtro.OperadorSentencia = "and";
                //filtros.Add(filtro);

                consulta.Filtros = filtros;
                #endregion
                long count = this._bc.AdministrationModel.MasterComplex_Count(AppMasters.coImpuesto, consulta, null);
                if (count > 0)
                {
                    #region Consulta la Cuenta y el Impuesto
                    var listCoImp = _bc.AdministrationModel.MasterComplex_GetPaged(AppMasters.coImpuesto, count, 1, consulta, null);
                    DTO_coImpuesto mastercoImp = listCoImp.Cast<DTO_coImpuesto>().ToList().First();

                    if (cacheCuentas.ContainsKey(mastercoImp.CuentaID.Value))
                        _cuenta = cacheCuentas[mastercoImp.CuentaID.Value];
                    else
                    {
                        _cuenta = (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, false, mastercoImp.CuentaID.Value, true);
                        cacheCuentas.Add(mastercoImp.CuentaID.Value, _cuenta);
                    }
                    this.data.Footer[index].DetalleDocu.PorcentajeIVA.Value = _cuenta.ImpuestoPorc.Value != null ? _cuenta.ImpuestoPorc.Value.Value : 0;
                    this.data.Footer[index].DetalleDocu.IVAUni.Value = (this.data.Footer[index].DetalleDocu.ValorUni.Value * this.data.Footer[index].DetalleDocu.PorcentajeIVA.Value) / 100;
                    this.data.Footer[index].DetalleDocu.IvaTotML.Value = Math.Round(this.data.Footer[index].DetalleDocu.IVAUni.Value.Value * this.data.Footer[index].DetalleDocu.CantidadDoc5.Value.Value,0);

                    #endregion
                }

                decimal tasaCambio = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);
                if (this.multiMoneda && tasaCambio != 0 && this.data.Footer[index].DetalleDocu.PorcentajeIVA.Value != null)
                {
                    decimal porcIVA = (this.data.Footer[index].DetalleDocu.PorcentajeIVA.Value.Value / 100);
                    this.data.Footer[index].DetalleDocu.IvaTotME.Value = ((this.data.Footer[index].DetalleDocu.ValorUni.Value.Value * this.data.Footer[index].DetalleDocu.CantidadDoc5.Value.Value) * porcIVA) / tasaCambio;
                    this.data.Footer[index].DetalleDocu.ValorTotME.Value = ((this.data.Footer[index].DetalleDocu.ValorUni.Value.Value * this.data.Footer[index].DetalleDocu.CantidadDoc5.Value.Value) / tasaCambio);
                }

                foreach (var footer in data.Footer)
                {
                    if (data.DocCtrl.MonedaID.Value == this.monedaLocal)
                    {
                        this.valorTotalDoc += footer.DetalleDocu.ValorTotML.Value.Value;
                        this.valorIVATotalDoc += footer.DetalleDocu.IvaTotML.Value.Value;
                    }
                    else
                    {
                        this.valorTotalDoc += footer.DetalleDocu.ValorTotML.Value.Value;
                        this.valorIVATotalDoc += footer.DetalleDocu.IvaTotML.Value.Value;
                    }
                }
                base.data.DocCtrl.Valor.Value = this.valorTotalDoc;
                base.data.DocCtrl.Iva.Value = this.valorIVATotalDoc;
                this.txtValorTotalML.EditValue = this.valorTotalDoc;
                this.txtIvaTotalML.EditValue = this.valorIVATotalDoc;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_CtaIVATarifa));
            }
        }

        #endregion

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void Form_Enter(object sender, EventArgs e)
        {
            base.Form_Enter(sender, e);
            if (FormProvider.Master.LoadFormTB)
                FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
        }
        
        #endregion

        #region Eventos Header

        /// <summary>
        /// Evento que se ejecuta al entrar el prefijo control 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterPrefijo_Enter(object sender, EventArgs e)
        {
            this._prefijoFocus = true;
        }

        /// <summary>
        /// Evento que se ejecuta al salir del prefijo control 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterPrefijo_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_prefijoFocus)
                {
                    _prefijoFocus = false;
                    if (this.masterPrefijo.ValidID)
                    {
                        this.prefijoID = this.masterPrefijo.Value;
                        //this.txtPrefix.Text = this.prefijoID;
                        this.txtSolicitudNro.Focus();
                    }
                    else
                        CleanHeader(true);  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Solicitud.cs", "masterPrefijo_Leave"));
            }
        }

        /// <summary>
        /// Valida que solo numeros se pueden escribir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSolicitudNro_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), "\\d+") && e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back)
                e.Handled = true;
            if (e.KeyChar == 46)
                e.Handled = true;
        }

        /// <summary>
        /// Valida que el usuario haya ingresado prefijo
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtSolicitudNro_Enter(object sender, EventArgs e)
        {           
            this._txtSolicitudNroFocus = true;
            if (!this.masterPrefijo.ValidID)
            {
                this._txtSolicitudNroFocus = false;
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterPrefijo.CodeRsx);

                MessageBox.Show(msg);
                this.masterPrefijo.Focus();
            }
        }

        /// <summary>
        /// Valida que el numero del recibo ingresado exista
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtSolicitudNro_Leave(object sender, EventArgs e)
        {
            if (this._txtSolicitudNroFocus)
            {
                _txtSolicitudNroFocus = false;
                if (this.txtSolicitudNro.Text == string.Empty)
                    this.txtSolicitudNro.Text = "0";

                if (this.txtSolicitudNro.Text == "0")
                {
                    #region Nueva solicitud
                    this.gcDocument.DataSource = null;
                    this.data = null;
                    this.newDoc = true;
                    this.EnableHeader(true);
                    this.masterPrefijo.EnableControl(false);
                    this.txtSolicitudNro.Enabled = false;
                    this.masterProveedor.Value = string.Empty;
                    this.txtDescDoc.Text = string.Empty;
                    #endregion
                }
                else
                {
                    try
                    {
                        DTO_prSolicitud Sol = _bc.AdministrationModel.Solicitud_Load(this.documentID, this.masterPrefijo.Value, Convert.ToInt32(this.txtSolicitudNro.Text));
                        //Valida si existe
                        if (Sol == null)
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Pr_NoSolicitudes)); ////
                            this.txtSolicitudNro.Focus();
                            this.validHeader = false;
                            return;
                        }
                        if (this._copyData)
                        {
                            Sol.DocCtrl.NumeroDoc.Value = 0;
                            Sol.DocCtrl.DocumentoNro.Value = 0;
                            Sol.HeaderSolDirecta.NumeroDoc.Value = 0;                           
                            Sol.DocCtrl.Estado.Value = (byte)EstadoDocControl.SinAprobar;
                            this._copyData = false;  
                        }

                        this.newDoc = false;

                        //Carga los datos
                        this._ctrl = Sol.DocCtrl;
                        this._solHeader = Sol.HeaderSolDirecta;

                        #region Asigna los valores
                        this.txtNumeroDoc.Text = this._ctrl.NumeroDoc.Value.Value.ToString();
                        this.masterPrefijo.Value = this._ctrl.PrefijoID.Value;
                        this.masterProveedor.Value = this._solHeader.ProveedorID.Value;
                        this.txtSolicitudNro.Text = this._ctrl.DocumentoNro.Value.Value.ToString();
                        this.dtFechaSol.DateTime = this._ctrl.FechaDoc.Value.Value;                         
                        this.txtDescDoc.Text = this._ctrl.Observacion.Value;                   

                        //this._tipoMonedaOr = this._ctrl.MonedaID.Value == this.monedaLocal ? TipoMoneda_LocExt.Local : TipoMoneda_LocExt.Foreign;
                        this.monedaId = this._ctrl.MonedaID.Value;
                        this._headerLoaded = true;
                            
                        if (Sol.Footer != null)
                        {
                            this._solFooter = Sol.Footer;
                            this.gcDocument.Focus();
                        }
                        else
                            this._solFooter = new List<DTO_prSolicitudFooter>();

                        this.data = Sol;
                        this.LoadData(true);                           
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Solicitud.cs", "txtSolicitudNro_Leave"));
                    }
                }
            }
        }

        /// <summary>
        /// valida la edición de las fechas
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void dtFechas_DateTimeChanged(object sender, EventArgs e)
        {
            try
            {
                this.ValidateDates();
                this.txtTasaCambio.EditValue =  this.LoadTasaCambio(this.dtFechaSol.DateTime);
            }
            catch (Exception)
            { ; }

        }

        /// <summary>
        /// Consulta de documentos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Envento</param>
        private void btnQueryDoc_Click(object sender, EventArgs e)
        {
            if (this.btnQueryDoc.Focused)
            {
                List<int> docs = new List<int>();
                docs.Add(AppDocuments.SolicitudDirecta);
                ModalQuerySolicitud getDocControl = new ModalQuerySolicitud(docs);
                getDocControl.ShowDialog();
                if (getDocControl.DocumentoControl != null)
                {
                    if (getDocControl.CopiadoInd)
                        this._copyData = true;
                    this.txtSolicitudNro.Enabled = true;
                    this.txtSolicitudNro.Text = getDocControl.DocumentoControl.DocumentoNro.Value.ToString();
                    this.masterPrefijo.Value = getDocControl.DocumentoControl.PrefijoID.Value;
                    this.txtSolicitudNro.Focus();
                    this.btnQueryDoc.Focus();
                    this.btnQueryDoc.Enabled = false;
                }
            }
            else
            {
                ModalQueryDocument getDocControl = new ModalQueryDocument(null);
                getDocControl.ShowDialog();
                if (getDocControl.DocumentoControl != null)
                {
                    if (getDocControl.CopiadoInd)
                        this._copyData = true;
                    this.txtProyectoNro.Enabled = true;
                    this.txtProyectoNro.Text = getDocControl.DocumentoControl.DocumentoNro.Value.ToString();
                    this.masterPrefijoProyecto.Value = getDocControl.DocumentoControl.PrefijoID.Value;
                    this.txtProyectoNro.Focus();
                    this.btnProyectoNro.Focus();
                }
            }
        }

        /// <summary>
        /// Valida que el numero  exista
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtProyectoNro_Leave(object sender, EventArgs e)
        {
            if (this.masterPrefijoProyecto.ValidID && !string.IsNullOrEmpty(this.txtProyectoNro.Text))
            {
                DTO_glDocumentoControl doc = this._bc.AdministrationModel.glDocumentoControl_GetInternalDoc(AppDocuments.PreProyecto, this.masterPrefijoProyecto.Value, Convert.ToInt32(this.txtProyectoNro.Text));
                if (doc != null)
                {
                    base._listSolicitudProyectos = this._bc.AdministrationModel.prDetalleDocu_GetByNumeroDoc(doc.NumeroDoc.Value.Value, false);
                    if (base._listSolicitudProyectos == null)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Pr_DetailProvNotExist));
                        this.txtProyectoNro.Focus();
                        this.validHeader = false;
                        return;
                    }
                    else
                    {
                        DTO_prDetalleDocu detaFilter = new DTO_prDetalleDocu();
                        detaFilter.Documento2ID.Value = doc.NumeroDoc.Value;
                        List<DTO_prDetalleDocu> listSolicitudExist = this._bc.AdministrationModel.prDetalleDocu_GetParameter(detaFilter);
                        if (listSolicitudExist.Count > 0)
                        {
                           foreach (var solExist in listSolicitudExist)
                                 base._listSolicitudProyectos.Where(x => x.CodigoBSID.Value == solExist.CodigoBSID.Value && 
                                                                    x.inReferenciaID.Value == solExist.inReferenciaID.Value).ToList().ForEach(z => z.CantidadDoc2.Value -= solExist.CantidadDoc5.Value);  
                        }
                    }
                }
                else
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_NoDocument));
                    this.txtProyectoNro.Focus();
                    this.validHeader = false;
                    return;
                }
            }
            
        }

        /// <summary>
        /// Evento que se ejecuta al salir del prefijo control 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterProveedor_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.masterProveedor.ValidID)
                {
                    DTO_prProveedor prov = (DTO_prProveedor)this._bc.GetMasterDTO(AppMasters.MasterType.Simple,AppMasters.prProveedor,false,this.masterProveedor.Value,true);
                    DTO_coTercero tercero = (DTO_coTercero)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, prov.TerceroID.Value, true);
                    this._regimenFiscalTercero = tercero.ReferenciaID.Value;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Solicitud.cs", "masterProveedor_Leave"));
            }
        }
        #endregion

        #region Eventos Grilla

        /// <summary>
        /// Evento que se ejecuta al entrar a la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gcDocument_Enter(object sender, EventArgs e)
        {
            if (this.ValidateHeader())
                this.validHeader = true;
            else
                this.validHeader = false;

            //Si el diseño esta cargado y el header es valido
            if (this.validHeader)
            {
                this.ValidHeaderTB();
                if (this.txtSolicitudNro.Text == "0")
                {
                   // FormProvider.Master.itemSendtoAppr.Enabled = false;
                    FormProvider.Master.itemExport.Enabled = false;
                    FormProvider.Master.itemPrint.Enabled = false;
                }

                #region Si entra al detalle y no tiene datos
                this.EnableHeader(false);
                try
                {
                    if (!this._headerLoaded)
                    {
                        DTO_prSolicitud sol = this.LoadTempHeader();
                        this._solHeader = sol.HeaderSolDirecta;
                        this._solFooter = sol.Footer;
                        this.TempData = sol;
                                               
                        this.LoadData(true);

                        this.UpdateTemp(this.data);
                        this._headerLoaded = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Solicitud.cs", "gcDocument_Enter" + ex.Message));
                }
                #endregion                
            }
        }

        /// <summary>
        /// Evento que se ejecuta al presionar un boton por defecto de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gcDocument_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            base.gcDocument_EmbeddedNavigator_ButtonClick(sender, e);

            if (!this.validHeader)
                this.masterPrefijo.Focus();

            if (this.txtNumeroDoc.Text != "0")
            {
                //FormProvider.Master.itemSendtoAppr.Enabled = false;
                FormProvider.Master.itemPrint.Enabled = false;
            }
        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para iniciar un nuevo documento
        /// </summary>
        public override void TBNew()
        {
            try
            {
                base.TBNew();
                if (true)
                {
                    this.data = new DTO_prSolicitud();
                    this._ctrl = new DTO_glDocumentoControl();
                    this._solHeader = new DTO_prSolicitudDirectaDocu();
                    this._solFooter = new List<DTO_prSolicitudFooter>();

                    this.gvDocument.ActiveFilterString = string.Empty;
                    this.disableValidate = true;
                    this.gcDocument.DataSource = this.data.Footer;
                    this.disableValidate = false;
                    this.gvCargos.ActiveFilterString = string.Empty;
                    this.gcCargos.DataSource = null;
                    this.gvCargos.RefreshData();

                    this._prefijoFocus = false;
                    this._txtSolicitudNroFocus = false;
                    this.CleanHeader(true);
                    this.EnableHeader(false);
                    this.masterPrefijo.EnableControl(true);
                    this.masterPrefijoProyecto.EnableControl(true);
                    this.txtSolicitudNro.Enabled = true;
                    this.txtProyectoNro.Enabled = true;
                    this.btnQueryDoc.Enabled = true;
                    this.btnProyectoNro.Enabled = true;
                    this.masterPrefijo.Focus();                 
                    this._headerLoaded = false;
                    this.cleanDoc = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Solicitud.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            try
            {
                base.TBSave();
                this.gvDocument.PostEditor();

                this.gvDocument.ActiveFilterString = string.Empty;
                if (!this.isValidCargo)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Pr_PorcentajeNoCien));
                    return;
                }
                if (this.ValidGrid())
                {
                    Thread process = new Thread(this.SaveThread);
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Solicitud.cs", "TBSave"));
            }
        }

        /// <summary>
        /// Boton para enviar a aprobacion un comprobante
        /// </summary>
        public override void TBSendtoAppr()
        {
                this.gvDocument.PostEditor();

                this.gvDocument.ActiveFilterString = string.Empty;
                if (this.ValidGrid())
                {
                    Thread process = new Thread(this.SendToApproveThread);
                    process.Start();
                }    
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo de salvar
        /// </summary>
        public override void SaveThread()
        {
            try
            {
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoProveedores(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                int numeroDoc = 0;
                bool update = false;
                if (this._ctrl.NumeroDoc.Value.Value != 0)
                {
                    numeroDoc = this._ctrl.NumeroDoc.Value.Value;
                    update = true;
                };

                #region Valida si el Modulo de Proyectos esta activo
                if (this.moduleProyectoActive)
                {
                    foreach (var item in this.TempData.Footer)
                    {
                        item.DetalleDocu.Documento2ID.Value = this._listSolicitudProyectos.First().Documento2ID.Value;
                        item.DetalleDocu.Detalle2ID.Value = this._listSolicitudProyectos.First().Detalle2ID.Value;
                    }
                }
                #endregion
                DTO_SerializedObject result = _bc.AdministrationModel.Solicitud_Guardar(this.documentID, this.TempData.DocCtrl, null,this.TempData.HeaderSolDirecta, this.TempData.Footer, update, out numeroDoc);
                FormProvider.Master.StopProgressBarThread(this.documentID);

                bool isOK = _bc.SendDocumentMail(MailType.NewDoc, this.documentID, this._actFlujo.seUsuarioID.Value, result, true,true);
                if (isOK)
                {
                    _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);
                    this.newDoc = true;
                    this.deleteOP = true;
                    this.data = new DTO_prSolicitud();
                    this._ctrl = new DTO_glDocumentoControl();
                    this._solHeader = new DTO_prSolicitudDirectaDocu();
                    this._solFooter = new List<DTO_prSolicitudFooter>();
                    this._headerLoaded = false;
                    this.Invoke(this.saveDelegate);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Solicitud.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        /// <summary>
        /// Hilo de Enviar para aprobación
        /// </summary>
        public override void SendToApproveThread()
        {
            try
            {
                DTO_TxResult resultNOK = new DTO_TxResult();
                resultNOK.Result = ResultValue.NOK;

                this.gvDocument.ActiveFilterString = string.Empty;

                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (this._bc.AdministrationModel.ConsultarProgresoProveedores(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);
                DTO_SerializedObject result = new DTO_SerializedObject() ;

                if (this._ctrl == null)
                {
                    result = resultNOK;
                    return;
                }
                else if (this.data.DocCtrl.NumeroDoc.Value.Value == 0)
                {
                    int numeroDoc = 0;
                    bool update = false;
                    if (this._ctrl.NumeroDoc.Value.Value != 0)
                    {
                        numeroDoc = this._ctrl.NumeroDoc.Value.Value;
                        update = true;
                    }
                    #region Valida si el Modulo de Proyectos esta activo
                    if (this.moduleProyectoActive)
                    {
                        foreach (var item in this.TempData.Footer)
                        {
                            item.DetalleDocu.Documento2ID.Value = this._listSolicitudProyectos.First().Documento2ID.Value;
                            item.DetalleDocu.Detalle2ID.Value = this._listSolicitudProyectos.First().Detalle2ID.Value;
                            item.DetalleDocu.CantidadDoc2.Value = item.DetalleDocu.CantidadDoc5.Value;
                        }
                    } 
                    #endregion
                    result = this._bc.AdministrationModel.Solicitud_Guardar(this.documentID, this.TempData.DocCtrl, null, this.TempData.HeaderSolDirecta, this.TempData.Footer, update, out numeroDoc);
                    this._ctrl.NumeroDoc.Value = numeroDoc;
                }
                if (result.GetType() != typeof(DTO_TxResult))               
                    result = this._bc.AdministrationModel.Solicitud_SendToAprob(this.documentID, this._ctrl.NumeroDoc.Value.Value, true);
                FormProvider.Master.StopProgressBarThread(this.documentID);
                bool isOK = this._bc.SendDocumentMail(MailType.SendToApprove, this.documentID, this._actFlujo.seUsuarioID.Value, result, true,true);
                if (isOK)
                {
                    this._bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), this._bc.AdministrationModel.User);

                    this.newDoc = true;
                    this.deleteOP = true;
                    this.data = new DTO_prSolicitud();
                    this._ctrl = new DTO_glDocumentoControl();
                    this._solHeader = new DTO_prSolicitudDirectaDocu();
                    this._solFooter = new List<DTO_prSolicitudFooter>();
                    this._headerLoaded = false;
                    this.Invoke(this.sendToApproveDelegate);
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-Solicitud.cs", "SendToApproveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        /// <summary>
        /// Hilo que se ejecuta cualdo el usuario va a imprtar datos al detalle
        /// </summary>
        public override void ImportThread()
        {
            try
            {
                if (this.pasteRet.Success)
                {
                    var text = pasteRet.MsgResult;
                    bool sendToServer = true;
                    string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    #region Variables de función y mensajes de error
                    DTO_TxResult result = new DTO_TxResult();
                    result.Result = ResultValue.OK;
                    result.Details = new List<DTO_TxResultDetail>();
                    //Lista con los dtos a subir y Fks a validas
                    List<DTO_prSolicitudFooter> listFooter = new List<DTO_prSolicitudFooter>();
                    Dictionary<string, List<Tuple<string, bool>>> fks = new Dictionary<string, List<Tuple<string, bool>>>();
                    Dictionary<string, DTO_glBienServicioClase> bienServ = new Dictionary<string, DTO_glBienServicioClase>();
                    Dictionary<string, DTO_inRefTipo> refer = new Dictionary<string, DTO_inRefTipo>();
                    List<string> fkNames = new List<string>();
                    Dictionary<string, string> colNames = new Dictionary<string, string>();
                    Dictionary<string, Object> colVals = new Dictionary<string, Object>();
                    #region Mensajes de error
                    string msgEmptyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField); // "Vacio"
                    string msgInvalidFormat = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidFormat); // "Formato incorrecto"
                    string msgImportInvalidLength = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Import_InvalidLength); // "Los datos ingresados tienen longitud superior a la permitida"
                    string msgFkNotFound = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FkNotFound); // "El código {0} no existe'
                    string msgNoCopyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoCopyField); // "Ningún registro copiado"
                    string msgIncompleteLine = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.IncompleteLine); // "Linea copiada incompleta"
                    string msgFkHierarchyFather = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Import_NotHierarchyFather); // "No puede importar ningún código jerárquico sin movimiento"
                    string msgCero = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ZeroField); // "{0} debe tener un valor diferente de cero'
                    string msgPositive = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue); // "El valor de {0} debe ser un número positivo'


                    string msgPorcentajeNoCien = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Pr_PorcentajeNoCien); // "Porcentaje debe ser 100%"
                    string msgCantidadDeCargos = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Pr_CantidadDeCargos); // "El numero de cargos no puede ser superior a 1 para esta clase de bienes y servicios"
                    string msgInvalidField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidField); // "Inválido"

                    #endregion
                    //Popiedades de un comprobante
                    DTO_prSolicitudFooter det = new DTO_prSolicitudFooter();
                    bool createDTO = true;
                    bool validList = true;
                    #endregion
                    #region Llena las listas de las columnas y FKs
                    List<string> cols = this.format.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    PropertyInfo[] propDetDocu = typeof(DTO_prDetalleDocu).GetProperties();
                    PropertyInfo[] propSolCargos = typeof(DTO_prSolicitudCargos).GetProperties();
                    //Recorre el objeto y revisa el nombre real de la columna
                    colVals.Add(cols[0], string.Empty);
                    colNames.Add(cols[0], "Index");

                    #region Columnas que corresponden a prDetalleDocu
                    foreach (PropertyInfo pi in propDetDocu)
                    {
                        if (!Attribute.IsDefined(pi, typeof(NotImportable)))
                        {
                            string colRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + pi.Name);

                            for (int colIndex = 0; colIndex < cols.Count; colIndex++)
                            {
                                if (cols[colIndex] == colRsx)
                                {
                                    if (!pi.Name.Equals("CantidadSol"))
                                    {
                                        colVals.Add(colRsx, string.Empty);
                                        colNames.Add(colRsx, pi.Name);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    #region Columnas que corresponden a prSolicitudCargos
                    foreach (PropertyInfo pi in propSolCargos)
                    {
                        if (!Attribute.IsDefined(pi, typeof(NotImportable)))
                        {
                            string colRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + pi.Name);

                            for (int colIndex = 0; colIndex < cols.Count; colIndex++)
                            {
                                if (cols[colIndex] == colRsx)
                                {
                                    if (!colVals.ContainsKey(colRsx))
                                    {
                                        colVals.Add(colRsx, string.Empty);
                                        colNames.Add(colRsx, pi.Name);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region Fks
                    fks.Add(this._codigoRsx, new List<Tuple<string, bool>>());
                    fks.Add(this._referenciaRsx, new List<Tuple<string, bool>>());
                    fks.Add(this._parametro1Rsx, new List<Tuple<string, bool>>());
                    fks.Add(this._parametro2Rsx, new List<Tuple<string, bool>>());
                    fks.Add(this._unidadRsx, new List<Tuple<string, bool>>());
                    fks.Add(this._proyectoRsx, new List<Tuple<string, bool>>());
                    fks.Add(this._centroCostoRsx, new List<Tuple<string, bool>>());
                    #endregion
                    #endregion
                    #region Llena información para enviar a la grilla (lee filas)
                    FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ReadRows) });
                    int percent = 0;
                    int indexLine = -1;
                    for (int i = 0; i < lines.Length; ++i)
                    {
                        #region Actualiza barra de progreso
                        FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                        percent = ((i + 1) * 100) / (lines.Length);

                        if (FormProvider.Master.ProcessCanceled(this.documentID))
                        {
                            result.Details = new List<DTO_TxResultDetail>();
                            result.ResultMessage = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser);
                            result.Result = ResultValue.NOK;
                            break;
                        }
                        #endregion
                        #region Valida si existen datos en la lista importada
                        if (lines.Length == 1)
                        {
                            result.ResultMessage = msgNoCopyField;
                            result.Result = ResultValue.NOK;
                        }
                        #endregion
                        #region Divide cada registro importado en columnas
                        //Recorre todas las columnas y verifica que tengan datos validos
                        string[] line = lines[i].Split(new string[] { CopyPasteExtension.tabChar }, StringSplitOptions.None);
                        #endregion

                        //Works with lines with data only
                        bool nuevo = true;
                        DTO_prDetalleDocu detDocu = null;
                        DTO_prSolicitudCargos detCargos = null;

                        if (i > 0 && line.Length > 0)
                        {
                            createDTO = true;

                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                            rd.line = i;
                            rd.Message = "OK";

                            #region Info básica: Llena los valores de las columnas (se revienta si el numero de columnas al importar es menor al requerido(plantilla))
                            if (line.Length < colNames.Count)
                            {
                                result.Result = ResultValue.NOK;
                                DTO_TxResultDetail rdL = new DTO_TxResultDetail();
                                rdL.line = i;
                                rdL.Message = msgIncompleteLine;
                                result.Details.Add(rdL);

                                createDTO = false;
                                validList = false;
                                continue;
                            }
                            else
                            {
                                //Recorre Columnas
                                for (int colIndex = 0; colIndex < cols.Count; colIndex++)
                                {
                                    string colRsx = cols[colIndex];  // Obtiene Nombre Columna
                                    colVals[colRsx] = line[colIndex]; // Obtiene valor Columna

                                    if (!string.IsNullOrWhiteSpace(line[colIndex]))
                                    {
                                        #region Recorre las FKs solamente
                                        if (colRsx == this._codigoRsx || colRsx == this._referenciaRsx || colRsx == this._parametro1Rsx ||
                                            colRsx == this._parametro2Rsx || colRsx == this._unidadRsx || colRsx == this._proyectoRsx ||
                                            colRsx == this._centroCostoRsx)
                                        {
                                            colVals[colRsx] = line[colIndex].ToUpper();

                                            #region Revisa si la columna ya existe
                                            Tuple<string, bool> tupValid = new Tuple<string, bool>(line[colIndex].Trim(), true);
                                            Tuple<string, bool> tupInvalid = new Tuple<string, bool>(line[colIndex].Trim(), false);

                                            if (fks[colRsx].Contains(tupValid))
                                                continue;
                                            if (fks[colRsx].Contains(tupInvalid))
                                            {
                                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                rdF.Field = colRsx;
                                                rdF.Message = string.Format(msgFkNotFound, line[colIndex].Trim());
                                                rd.DetailsFields.Add(rdF);

                                                createDTO = false;
                                            }
                                            #endregion
                                            else
                                            {
                                                int docId = this.GetMasterDocumentID(colRsx);

                                                bool isInt = docId == AppMasters.glDocumento ? true : false;
                                                object dto = _bc.GetMasterDTO(AppMasters.MasterType.Simple, docId, isInt, line[colIndex], true);

                                                bool hierarchyFather = false;
                                                if (dto is DTO_MasterHierarchyBasic)
                                                {
                                                    if ((dto as DTO_MasterHierarchyBasic).MovInd.Value == false)
                                                        hierarchyFather = true;
                                                }
                                                if (dto != null && !hierarchyFather)
                                                {
                                                    fks[colRsx].Add(new Tuple<string, bool>(line[colIndex].Trim(), true));
                                                    #region Asigna los valores de las referencias y bien servicio

                                                    if (colRsx == _codigoRsx)
                                                    {
                                                        if (!bienServ.Keys.Contains(line[colIndex].Trim()))
                                                        {
                                                            DTO_prBienServicio bs = (DTO_prBienServicio)dto;
                                                            DTO_glBienServicioClase bsClase = (DTO_glBienServicioClase)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glBienServicioClase, false, bs.ClaseBSID.Value, true);
                                                            bienServ.Add(line[colIndex].Trim(), bsClase);
                                                        }
                                                    }

                                                    if (colRsx == _referenciaRsx && !string.IsNullOrEmpty(line[colIndex].Trim()))
                                                    {
                                                        if (!refer.Keys.Contains(line[colIndex].Trim()))
                                                        {
                                                            DTO_inReferencia rf = (DTO_inReferencia)dto;
                                                            UDT_BasicID udt = new UDT_BasicID() { Value = rf.TipoInvID.Value };
                                                            DTO_inRefTipo rfTipo = (DTO_inRefTipo)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.inRefTipo, udt, true);
                                                            refer.Add(line[colIndex].Trim(), rfTipo);
                                                        }
                                                    }
                                                    #endregion
                                                }
                                                else
                                                {
                                                    fks[colRsx].Add(new Tuple<string, bool>(line[colIndex].Trim(), false));

                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    if (hierarchyFather)
                                                        rdF.Message = string.Format(msgFkHierarchyFather, line[colIndex]);
                                                    else
                                                        rdF.Message = string.Format(msgFkNotFound, line[colIndex]);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                }
                            }
                            #endregion

                            #region Creacion de DTO y validacion Formatos
                            if (createDTO)
                            {
                                #region Revisa  si el index ha cambiado
                                if (indexLine != Convert.ToInt32(colVals[cols[0]]))
                                {
                                    det = new DTO_prSolicitudFooter();
                                    det.SolicitudCargos = new List<DTO_prSolicitudCargos>();
                                    detDocu = new DTO_prDetalleDocu();
                                    detCargos = new DTO_prSolicitudCargos();
                                    detCargos.PorcentajeID.Value = 100;
                                    nuevo = true;
                                }
                                else
                                {
                                    detCargos = new DTO_prSolicitudCargos();
                                    detCargos.PorcentajeID.Value = 100;
                                    nuevo = false;
                                }
                                indexLine = Convert.ToInt32(colVals[cols[0]]);
                                #endregion

                                for (int colIndex = 1; colIndex < colNames.Count(); colIndex++)
                                {
                                    string colRsx = cols[colIndex];
                                    try
                                    {
                                        string colName = colNames[colRsx]; //Nombre Columna
                                        string colValue = colVals[colRsx].ToString().Trim(); // Valor Columna

                                        #region Validacion de Nulls (Campos basicos)
                                        if (string.IsNullOrEmpty(colValue) && (colRsx == base._codigoRsx ||
                                                colRsx == base._unidadRsx || colRsx == base._descRsx ||
                                                colRsx == base._cantidadRsx || colRsx == base._proyectoRsx ||
                                                colRsx == base._centroCostoRsx || colRsx == base._porcentajeRsx))
                                        {
                                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = colRsx;
                                            rdF.Message = msgEmptyField;
                                            rd.DetailsFields.Add(rdF);
                                            createDTO = false;
                                        }

                                        #endregion
                                        #region Validacion Formatos
                                        #region Define UDT type
                                        PropertyInfo pi = null;
                                        UDT udt = null;
                                        PropertyInfo piUDT = null;
                                        if (nuevo)
                                        {
                                            try
                                            {
                                                pi = detDocu.GetType().GetProperty(colName);
                                                if (pi != null)
                                                {
                                                    udt = (UDT)pi.GetValue(detDocu, null);
                                                    piUDT = udt.GetType().GetProperty("Value");
                                                }
                                                else
                                                {
                                                    pi = detCargos.GetType().GetProperty(colName);
                                                    udt = (UDT)pi.GetValue(detCargos, null);
                                                    piUDT = udt.GetType().GetProperty("Value");
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                throw ex;
                                            }
                                        }
                                        else
                                        {
                                            if (colRsx == _proyectoRsx || colRsx == _centroCostoRsx || colRsx == _porcentajeRsx)
                                            {
                                                try
                                                {
                                                    pi = detCargos.GetType().GetProperty(colName);
                                                    udt = (UDT)pi.GetValue(detCargos, null);
                                                    piUDT = udt.GetType().GetProperty("Value");
                                                }
                                                catch (Exception ex)
                                                {
                                                    throw ex;
                                                }
                                            }
                                            else
                                                continue;
                                        }
                                        #endregion
                                        #region Validaciones basicas
                                        //Comprueba los valores solo Bool
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
                                            #region Fechas
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
                                            #endregion
                                            #region Numericas
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
                                            #endregion
                                        }
                                        #endregion
                                        #endregion

                                        // Si paso las validaciones Asigna el valor al DTO
                                        if (createDTO && !string.IsNullOrWhiteSpace(colValue))
                                            udt.SetValueFromString(colValue); // Llena el campo con el valor correspondiente
                                    }
                                    #region Exception
                                    catch (Exception ex1)
                                    {
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = colRsx;
                                        rdF.Message = _bc.GetResourceForException(ex1, "WinApp", "DocumentProvFrom.cs - Creacion de DTO y validacion Formatos");
                                        rd.DetailsFields.Add(rdF);

                                        createDTO = false;
                                    }
                                    #endregion
                                }
                            }
                            #endregion

                            #region Revisa si hay algun campo invalido
                            result.Details.Add(rd);
                            if (rd.DetailsFields.Count > 0)
                            {
                                rd.Message = "NOK";
                                result.Result = ResultValue.NOK;
                            }
                            #endregion

                            if (createDTO && validList)
                            {
                                if (nuevo)
                                {
                                    det.DetalleDocu = detDocu;
                                    det.SolicitudCargos.Add(detCargos);
                                    det.ProyectoID = detCargos.ProyectoID.Value;
                                    det.CentroCostoID = detCargos.CentroCostoID.Value;
                                    listFooter.Add(det); // Agrega un registro validado a la lista
                                }
                                else
                                    det.SolicitudCargos.Add(detCargos);
                            }
                            else
                                validList = false;
                        }
                    }
                    #endregion
                    #region Valida las restricciones particulares de la solicitud
                    if (validList)
                    {
                        result.Details = new List<DTO_TxResultDetail>();

                        int index = this.NumFila;
                        int i = 0;

                        FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ValidatingData) });
                        percent = 0;

                        foreach (DTO_prSolicitudFooter dto in listFooter)
                        {
                            #region Barra de Progreso
                            createDTO = true;
                            FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                            percent = ((i + 1) * 100) / (listFooter.Count);

                            if (FormProvider.Master.ProcessCanceled(this.documentID))
                            {
                                result.Details = new List<DTO_TxResultDetail>();
                                result.ResultMessage = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser);
                                result.Result = ResultValue.NOK;
                                break;
                            }
                            #endregion

                            #region Indexes
                            dto.DetalleDocu.Index = i;
                            int indexCargo = 0;
                            foreach (DTO_prSolicitudCargos cargo in dto.SolicitudCargos)
                            {
                                cargo.IndexDet = index;
                                cargo.Index = indexCargo;
                                indexCargo++;
                            }
                            i++;
                            #endregion

                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                            rd.line = i;
                            rd.Message = "OK";

                            DTO_glBienServicioClase bsClase = bienServ[dto.DetalleDocu.CodigoBSID.Value];
                            DTO_inRefTipo refTipo = null;
                            if (!string.IsNullOrEmpty(dto.DetalleDocu.inReferenciaID.Value))
                                refTipo = refer[dto.DetalleDocu.inReferenciaID.Value];


                            #region Validar cantidad de los registros en prSolicitudCargos
                            if (((TipoCodigo)Enum.Parse(typeof(TipoCodigo), bsClase.TipoCodigo.Value.Value.ToString()) != TipoCodigo.Servicio &&
                                (TipoCodigo)Enum.Parse(typeof(TipoCodigo), bsClase.TipoCodigo.Value.Value.ToString()) != TipoCodigo.Suministros &&
                                (TipoCodigo)Enum.Parse(typeof(TipoCodigo), bsClase.TipoCodigo.Value.Value.ToString()) != TipoCodigo.SuministroPersonal) && dto.SolicitudCargos.Count > 1)
                            {
                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                rdF.Field = this._solCargosRsx;
                                rdF.Message = msgCantidadDeCargos;
                                rd.DetailsFields.Add(rdF);
                                createDTO = false;
                            }
                            #endregion
                            #region Validaciones particulares de la solicitud
                            this.ValidateDataImport(dto.DetalleDocu, bsClase, rd, msgFkNotFound, msgInvalidField);
                            foreach (DTO_prSolicitudCargos cargo in dto.SolicitudCargos)
                                this.ValidateDataImport(cargo, bsClase, rd, msgFkNotFound, msgInvalidField);
                            #endregion

                            result.Details.Add(rd);
                            if (rd.DetailsFields.Count > 0)
                            {
                                rd.Message = "NOK";
                                result.Result = ResultValue.NOK;
                                result.ResultMessage = ResultValue.NOK.ToString();
                            }
                        }
                    }
                    #endregion
                    #region Actualiza la información de la grilla
                    if (sendToServer && result.Result == ResultValue.OK)
                    {
                        MessageForm frm = new MessageForm(result);
                        if (result.Result.Equals(ResultValue.OK))
                        {
                            this.data.Footer = listFooter;
                            this.UpdateTemp(this.data);
                            this.Invoke(this.refreshGridDelegate);
                            this.Invoke(this.afterImportDelegate);
                        }
                        this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                    }
                    else
                    {
                        MessageForm frm = new MessageForm(result);
                        this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                    }
                    #endregion
                }
                else
                {
                    MessageForm frm = new MessageForm(pasteRet.MsgResult, MessageType.Error);
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                this.importando = false;
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }
        #endregion
    }
}
