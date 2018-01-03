using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using System.Threading;
using NewAge.DTO.Resultados;
using NewAge.Forms;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de documentos
    /// </summary>
    public partial class AdicionInvActivos : DocumentForm
    {
        //public AltaActivos()
        //{
        //    this.InitializeComponent();
        //}

        #region Delegados
        /// <summary>
        /// Delegado que finaliza el proceso de generacion de preliminares
        /// </summary>
        public delegate void RefreshGrid();
        public RefreshGrid refresGrd;
        public void RefreshGridMethod()
        {
            this._noLoaded = true;
            FormProvider.Master.itemSave.Enabled = false;
            this.LoadData(false);
            this.EnableFooter(false);
            this.EnableHeader(false);
            this.CleanHeader();
            this.CleanFooter();
            this.select = new List<int>();
        }
        #endregion

        #region Variables Formulario

        private BaseController _bc = BaseController.GetInstance();

        private DTO_glDocumentoControl _glDocumentoControl = null;
        private string _documento = string.Empty;
        /// <summary>
        /// MOneda q viene del codocumneto
        /// </summary>
        private string _monedaOrigen;
        private string _monedaLocal;
        private string _monedaExtranjera;
        private List<DTO_acActivoControl > _lActivos;
        private bool selected = false;
        private bool validMove;
        private bool _validRow = false;
        private bool _noLoaded = false;

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Carga la información de las grilla
        /// </summary>
        /// <param name="firstTime">Si es la primera vez que carga la grilla</param>
        /// <param name="refresh">Si debe volver a cargar la data del servidor</param>
        protected override void LoadData(bool firstTime)
        {
            this.gcDocument.DataSource = this._lActivos;
            this.gcDocument.RefreshDataSource();
        }

        /// <summary>
        /// Inicializa los parametros de la pantalla
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.AdicionInvActivos;
            InitializeComponent();

            base.SetInitParameters();

            this.InitControls();
            this.AddGridCols();

            //Inicializa los delegados
            this.refresGrd = new RefreshGrid(RefreshGridMethod);

            //Carga info del formulario
            this.userID = this._bc.AdministrationModel.User.ReplicaID.Value.Value;
            this.areaFuncionalID = this._bc.AdministrationModel.User.AreaFuncionalID.Value;
            this.frmModule = ModulesPrefix.ac;

            //Carga info de las monedas
            this._monedaLocal = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
            this._monedaExtranjera = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);

            //Deshabilita los botones +- de la grilla
            this.gcDocument.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.gcDocument.EmbeddedNavigator.Buttons.CustomButtons[0].Enabled = false;

        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected override void AddGridCols()
        {
            try
            {
                base.AddGridCols();

                //Plaqueta
                GridColumn plaquetaID = new GridColumn();
                plaquetaID.FieldName = this.unboundPrefix + "PlaquetaID";
                plaquetaID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PlaquetaID");
                plaquetaID.UnboundType = UnboundColumnType.String;
                plaquetaID.VisibleIndex = 1;
                plaquetaID.Width = 110;
                plaquetaID.Visible = true;
                plaquetaID.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(plaquetaID);

                //Serial
                GridColumn serialID = new GridColumn();
                serialID.FieldName = this.unboundPrefix + "SerialID";
                serialID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_SerialID");
                serialID.UnboundType = UnboundColumnType.Integer;
                serialID.VisibleIndex = 2;
                serialID.Width = 80;
                serialID.Visible = true;
                serialID.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(serialID);

                //Referencia
                GridColumn inReferenciaID = new GridColumn();
                inReferenciaID.FieldName = this.unboundPrefix + "inReferenciaID";
                inReferenciaID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_inReferenciaID");
                inReferenciaID.UnboundType = UnboundColumnType.String;
                inReferenciaID.VisibleIndex = 3;
                inReferenciaID.Width = 100;
                inReferenciaID.Visible = true;
                inReferenciaID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(inReferenciaID);

                //Descripcion
                GridColumn descripcion = new GridColumn();
                descripcion.FieldName = this.unboundPrefix + "Descriptivo";
                descripcion.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descripcion");
                descripcion.UnboundType = UnboundColumnType.String;
                descripcion.VisibleIndex = 4;
                descripcion.Width = 200;
                descripcion.Visible = true;
                descripcion.OptionsColumn.AllowEdit = false;
                descripcion.Fixed = FixedStyle.Left;
                this.gvDocument.Columns.Add(descripcion);
                

                //Clase
                GridColumn clase = new GridColumn();
                clase.FieldName = this.unboundPrefix + "ActivoClaseID";
                clase.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ActivoClaseID");
                clase.UnboundType = UnboundColumnType.String;
                clase.VisibleIndex = 5;
                clase.Width = 90;
                clase.Visible = true;
                clase.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(clase);

                //Tipo
                GridColumn ActivoTipoID = new GridColumn();
                ActivoTipoID.FieldName = this.unboundPrefix + "ActivoTipoID";
                ActivoTipoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ActivoTipoID");
                ActivoTipoID.UnboundType = UnboundColumnType.String;
                ActivoTipoID.VisibleIndex = 6;
                ActivoTipoID.Width = 90;
                ActivoTipoID.Visible = true;
                ActivoTipoID.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(ActivoTipoID);

                //Grupo
                GridColumn grupo = new GridColumn();
                grupo.FieldName = this.unboundPrefix + "ActivoGrupoID";
                grupo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ActivoGrupoID");
                grupo.UnboundType = UnboundColumnType.String;
                grupo.VisibleIndex = 7;
                grupo.Width = 90;
                grupo.Visible = true;
                grupo.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(grupo);

                //Modelo
                GridColumn modelo = new GridColumn();
                modelo.FieldName = this.unboundPrefix + "Modelo";
                modelo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Modelo");
                modelo.UnboundType = UnboundColumnType.String;
                modelo.VisibleIndex = 8;
                modelo.Width = 80;
                modelo.Visible = true;
                modelo.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(modelo);

                //Localizacion Fisica
                GridColumn locFisica = new GridColumn();
                locFisica.FieldName = this.unboundPrefix + "LocFisicaID";
                locFisica.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_LocFisicaID");
                locFisica.UnboundType = UnboundColumnType.String;
                locFisica.VisibleIndex = 9;
                locFisica.Width = 90;
                locFisica.Visible = true;
                locFisica.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(locFisica);

                //Proyecto
                GridColumn proyecto = new GridColumn();
                proyecto.FieldName = this.unboundPrefix + "ProyectoID";
                proyecto.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProyectoID");
                proyecto.UnboundType = UnboundColumnType.String;
                proyecto.VisibleIndex = 10;
                proyecto.Width = 90;
                proyecto.Visible = true;
                proyecto.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(proyecto);

                //Centro de Costo
                GridColumn centroCosto = new GridColumn();
                centroCosto.FieldName = this.unboundPrefix + "CentroCostoID";
                centroCosto.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CentroCostoID");
                centroCosto.UnboundType = UnboundColumnType.String;
                centroCosto.VisibleIndex = 11;
                centroCosto.Width = 90;
                centroCosto.Visible = true;
                centroCosto.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(centroCosto);

                //Turnos
                GridColumn turnos = new GridColumn();
                turnos.FieldName = this.unboundPrefix + "Turnos";
                turnos.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Turnos");
                turnos.UnboundType = UnboundColumnType.Integer;
                turnos.VisibleIndex = 11;
                turnos.Width = 90;
                turnos.Visible = true;
                turnos.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(turnos);

                //Valor Pesos
                GridColumn valorP = new GridColumn();
                valorP.FieldName = this.unboundPrefix + "CostoLOC";
                valorP.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CostoLOC");
                valorP.UnboundType = UnboundColumnType.Decimal;
                valorP.VisibleIndex = 11;
                valorP.Width = 90;
                valorP.Visible = true;
                valorP.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(valorP);

                //Valor Dolares
                GridColumn valorD = new GridColumn();
                valorD.FieldName = this.unboundPrefix + "CostoEXT";
                valorD.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CostoEXT");
                valorD.UnboundType = UnboundColumnType.Decimal;
                valorD.VisibleIndex = 11;
                valorD.Width = 90;
                valorD.Visible = true;
                valorD.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(valorD);

                //Indice de la fila
                GridColumn colIndex = new GridColumn();
                colIndex.FieldName = this.unboundPrefix + "Index";
                colIndex.UnboundType = UnboundColumnType.Integer;
                colIndex.Visible = false;
                this.gvDocument.Columns.Add(colIndex);


            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AltaActivos.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize()
        {
            base.AfterInitialize();
            this._lActivos = new List<DTO_acActivoControl>();
            #region Carga temporales
            if (this.HasTemporales())
            {
                string msgTitleLoadTemp = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_TempLoad);
                string msgLoadTemp = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Temp_LoadData);
                try
                {
                    //Revisa si desea cargar los temporales
                    if (MessageBox.Show(msgLoadTemp, msgTitleLoadTemp, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        var aux = _bc.AdministrationModel.aplTemporales_GetByOrigen(this.documentID.ToString(), _bc.AdministrationModel.User);
                        if (aux != null)
                        {
                            try
                            {
                                this.LoadTempData(aux);
                            }
                            catch (Exception ex1)
                            {
                                //this.validHeader = false;
                                MessageBox.Show(_bc.GetResourceError(DictionaryMessages.Err_TempLoad));
                                _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);
                            }
                        }
                        else
                        {
                            //this.validHeader = false;
                            MessageBox.Show(_bc.GetResourceError(DictionaryMessages.Err_TempLoad));
                            _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);
                        }
                    }
                    else
                    {
                        //this.validHeader = false;
                        _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ComprobanteManual.cs", "AfterInitialize: " + ex.Message));
                }
            }
            #endregion
        }

        /// <summary>
        /// Carga la informacion temporal del documento
        /// </summary>
        /// <param name="aux">Informacion del temporal</param>
        protected override void LoadTempData(object aux)
        {
            this._lActivos = (List<DTO_acActivoControl>)aux;
            this.LoadData(true);
            DTO_acActivoControl actrl = this._lActivos.First();
            this._glDocumentoControl = _bc.AdministrationModel.glDocumentoControl_GetByID(actrl.NumeroDoc.Value.Value);

            if (this._glDocumentoControl.Fecha.Value.HasValue &&
                this._glDocumentoControl.Fecha.Value.Value.Year == this.dtPeriod.DateTime.Year &&
                this._glDocumentoControl.Fecha.Value.Value.Month == this.dtPeriod.DateTime.Month)
            {
                List<DTO_acActivoControl> acTemp = this._bc.AdministrationModel.AltaActivos_GetActivosByNumDoc(this._glDocumentoControl.NumeroDoc.Value.Value);
                if (acTemp.Count == this._lActivos.Count)
                {                    
                    this.dtFecha.DateTime = this._glDocumentoControl.Fecha.Value.Value;
                    this.EnableHeader(false);
                    this.EnableFooter(true);
                }
                else
                {
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.DocumentChanged));
                    _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);
                }
            }
            else
            {
                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ac_FacturaFueraPeriodo));
                _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);
            }
        }

        protected override void Form_Enter(object sender, EventArgs e)
        {
            base.Form_Enter(sender, e);
            FormProvider.Master.itemGenerateTemplate.Visible = false;
        }
        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Inicializa los controles de la aplicacion
        /// </summary>
        public void InitControls()
        {
            //Habilitar o Deshabilitar Controles
            this.dtFecha.Enabled = false;

            //Controles del header
            #region Carga le filtro para el tipo de movimiento
            ////Define las fks
            //Dictionary<string, string> fks = new Dictionary<string, string>();
            //fks.Add("coDocumentoID", "coDocumentoID");
            //fks.Add("eg_coDocumento", "EmpresaGrupoID");

            ////Empresa Grupo
            //DTO_glConsultaFiltro eg = new DTO_glConsultaFiltro();
            //eg.CampoFisico = "EmpresaGrupoID";
            //eg.OperadorFiltro = OperadoresFiltro.Igual;
            //eg.ValorFiltro = _bc.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coDocumento);
            //eg.OperadorSentencia = "AND";

            ////coDocumento
            //DTO_glConsultaFiltro coDoc = new DTO_glConsultaFiltro();
            //coDoc.CampoFisico = "DocumentoID";
            //coDoc.OperadorFiltro = OperadoresFiltro.Igual;
            //coDoc.ValorFiltro = AppDocuments.AltaActivos.ToString();
            //coDoc.OperadorSentencia = "AND";

            //List<DTO_glConsultaFiltro> filtrosDeta = new List<DTO_glConsultaFiltro>();
            //filtrosDeta.Add(eg);
            //filtrosDeta.Add(coDoc);

            List<DTO_glConsultaFiltro> filtrosComplejos = new List<DTO_glConsultaFiltro>();
            //DTO_glConsultaFiltroComplejo f = new DTO_glConsultaFiltroComplejo(AppMasters.coDocumento, fks, filtrosDeta);
            //filtrosComplejos.Add(f);
            #endregion
            //Llenar Combos
            TablesResources.GetTableResources(this.cmbTipoDecpreciacionML, typeof(TipoDepreciacion));
            if (this.cmbTipoDecpreciacionML.SelectedIndex == Convert.ToInt32(TipoDepreciacion.SaldosDecrecientes))
                this.txtVidaUtilLocal.Enabled = true;
            if (this.cmbTipoDecpreciacionME.SelectedIndex == Convert.ToInt32(TipoDepreciacion.SaldosDecrecientes))
                this.txtVidaUtilIFRS.Enabled = true;

            TablesResources.GetTableResources(this.cmbTipoDecpreciacionME, typeof(TipoDepreciacion));
            //Botones Barra.
            FormProvider.Master.itemGenerateTemplate.Visible = false;
            FormProvider.Master.itemGenerateTemplate.Enabled = false;
            
            this.EnableFooter(false);
        }

        /// <summary>
        /// Verifica la informacion del DTO_acActivoControl
        /// </summary>
        /// <returns>DTO_TxResult</returns>
        private DTO_TxResult ValidarData()
        {
            try
            {
                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.OK;

                result.Details = new List<DTO_TxResultDetail>();
                DTO_TxResultDetail rd = new DTO_TxResultDetail();
                DTO_TxResultDetailFields drF = new DTO_TxResultDetailFields();

                //Nombre de las columnas.
                string colCtroCosto = string.Empty;
                string colPlaquetaID = string.Empty;
                string colAcClase = string.Empty;
                string colAcGrupo = string.Empty;
                string colTipo = string.Empty;
                string colProyecto = string.Empty;
                string colLocFisica = string.Empty;
                string colInRefID = string.Empty;

                drF = new DTO_TxResultDetailFields();
                drF.Message = string.Empty;
                foreach (int index in this.select)
                {
                    DTO_acActivoControl acCtrl = this._lActivos[index];
                    rd = new DTO_TxResultDetail();
                    rd.Message = string.Empty;
                    rd.line = index;

                    if (this.gcDocument.DataSource != null)
                    {
                        #region Validacion de nulls y FKs
                        #region Cto Costo
                        colCtroCosto = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CentroCostoID");
                        drF = _bc.ValidGridCell(colCtroCosto, acCtrl.CentroCostoID.Value, false, true, true, AppMasters.coCentroCosto);
                        if (drF != null)
                            rd.DetailsFields.Add(drF);
                        #endregion
                        #region PlaquetaID
                        colPlaquetaID = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_plaquetaid");
                        drF = _bc.ValidGridCell(colPlaquetaID, acCtrl.PlaquetaID.Value, true, false, false, null);

                        if (drF != null)
                            rd.DetailsFields.Add(drF);
                        #endregion
                        #region Ac ClaseID
                        colAcClase = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ActivoClaseID");
                        drF = _bc.ValidGridCell(colAcClase, acCtrl.ActivoClaseID.Value, true, true, false, AppMasters.acClase);

                        if (drF != null)
                            rd.DetailsFields.Add(drF);
                        #endregion
                        #region Ac GrupoID
                        colAcGrupo = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ActivoGrupoID");
                        drF = _bc.ValidGridCell(colAcGrupo, acCtrl.ActivoGrupoID.Value, true, true, false, AppMasters.acGrupo);

                        if (drF != null)
                            rd.DetailsFields.Add(drF);
                        #endregion
                        #region Tipo
                        colTipo = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Tipo");
                        drF = _bc.ValidGridCell(colTipo, acCtrl.ActivoTipoID.Value, true, true, false, AppMasters.acTipo);

                        if (drF != null)
                            rd.DetailsFields.Add(drF);
                        #endregion
                        #region ProyectoID
                        colProyecto = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProyectoID");
                        drF = _bc.ValidGridCell(colProyecto, acCtrl.ProyectoID.Value, false, true, true, AppMasters.coProyecto);

                        if (drF != null)
                            rd.DetailsFields.Add(drF);
                        #endregion
                        #region LocFisica
                        colLocFisica = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_LocFisicaID");
                        drF = _bc.ValidGridCell(colLocFisica, acCtrl.LocFisicaID.Value, false, true, true, AppMasters.glLocFisica);

                        if (drF != null)
                            rd.DetailsFields.Add(drF);
                        #endregion
                        #region ReferenciaID
                        colInRefID = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_inReferenciaID");
                        drF = _bc.ValidGridCell(colInRefID, acCtrl.inReferenciaID.Value, true, true, false, AppMasters.inReferencia);


                        if (drF != null)
                            rd.DetailsFields.Add(drF);
                        #endregion

                        #endregion
                       
                    }
                    if (rd.DetailsFields.Count > 0)
                    {
                        result.Result = ResultValue.NOK;
                        result.Details.Add(rd);
                    }
                }

                if (result.Result == ResultValue.NOK)
                {
                    MessageForm frm = new MessageForm(result);
                    frm.ShowDialog();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Verifica que no se duplique la plaqueta.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>isValid</returns>
        private bool ValidarPlaqueta(int index)
        {
            this.isValid = true;
            GridColumn col = this.gvDocument.Columns[this.unboundPrefix + "PlaquetaID"];
            string plaqueta = Convert.ToString(gvDocument.GetRowCellValue(index, col));
            DTO_acActivoControl dtoPLaqueta = new DTO_acActivoControl();

            dtoPLaqueta = _bc.AdministrationModel.acActivoControl_GetByPlaqueta(plaqueta);
            if (dtoPLaqueta != null)
            {
                this.isValid = false;
            }

            return this.isValid;
        }

        /// <summary>
        /// Función que habilita o deshabilita controles del Footer
        /// </summary>
        /// <param name="enable">enable</param>
        private void EnableFooter(bool enable)
        {
            if (!enable)
            {
                //Combos
                this.cmbTipoDecpreciacionML.SelectedIndex = 0;
                this.cmbTipoDecpreciacionME.SelectedIndex = 0;

                //Campos del DTO
                this.txtVidaUtilLocal.Text = "1";
                this.txtValorResidualML.Text = "0";
                this.txtValorResidualML.Enabled = false;

                this.txtVidaUtilIFRS.Text = "1";
                this.txtValorResidualME.Text = "0";
                this.txtValorResidualME.Enabled = false;

                //Totales
                this.txtDebLocal.Text = string.Empty;
                this.txtDebForeign.Text = string.Empty;
                this.txtCredLocal.Text = string.Empty;
                this.txtDebForeign.Text = string.Empty;
                this.txtTotalLocal.Text = string.Empty;
                this.txtTotalForeign.Text = string.Empty;
            }

           
            this.txtVidaUtilLocal.Enabled = enable;            
            this.txtVidaUtilIFRS.Enabled = enable;
            this.cmbTipoDecpreciacionML.Enabled = enable;
            this.cmbTipoDecpreciacionME.Enabled = enable;
        }

        /// <summary>
        /// Función que habilita o deshabilita controles del Header
        /// </summary>
        /// <param name="enable">enable</param>
        private void EnableHeader(bool enable)
        {
            if (enable)
            {
                FormProvider.Master.itemSave.Enabled = true;

            }            else
                FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);
        }

        /// <summary>
        /// Extrae datos de la grilla para las cajas de texto calculando los valores correspondientes
        /// </summary>
        private void CalcularTotal()
        {
            try
            {
                decimal sumLocal = 0;
                decimal sumExtran = 0;
                decimal credlocal = 0;
                decimal credextran = 0;

                sumLocal = (decimal)this._lActivos.Sum(x => x.CostoLOC.Value);
                sumExtran = (decimal)this._lActivos.Sum(x => x.CostoEXT.Value);

                this.txtDebLocal.Text = (sumLocal - credlocal).ToString();
                this.txtDebForeign.Text = (sumExtran - credextran).ToString();
                this.txtCredLocal.Text = credlocal.ToString();
                this.txtCredForeign.Text = credextran.ToString();
                this.txtTotalLocal.Text = sumLocal.ToString();
                this.txtTotalForeign.Text = sumExtran.ToString();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// Funcion que completa la info de los datos antes de enviarlos al servidor
        /// </summary>
        private void CompleteData()
        {
            foreach (int index in this.select)
            {
                DTO_acActivoControl acCtrl = this._lActivos[index];

                acCtrl.Tipo.Value = (byte)TipoActivo.Activo;
                acCtrl.DocumentoID.Value = this.documentID;
                acCtrl.Fecha.Value = this.dtFecha.DateTime;
                acCtrl.Periodo.Value = this.dtPeriod.DateTime;
                acCtrl.EstadoInv.Value = (byte)EstadoInv.Activo;
                acCtrl.Propietario.Value = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
                acCtrl.EstadoActID.Value = _bc.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_EstadoXDefecto);
                acCtrl.MonedaID.Value = this._glDocumentoControl.MonedaID.Value;
                acCtrl.TerceroID.Value = this.terceroID;
                acCtrl.NumeroDocCompra.Value = Convert.ToInt16(this.txtDocumentoID.Text);
                acCtrl.DocumentoTercero.Value = ""; //TODO revisar
                acCtrl.VidaUtilLOC.Value = Convert.ToInt32(this.txtVidaUtilLocal.Text);
                acCtrl.VidaUtilIFRS.Value = Convert.ToInt32(this.txtVidaUtilIFRS.Text);
                acCtrl.TipoDepreLOC.Value = Convert.ToByte((this.cmbTipoDecpreciacionML.SelectedItem as ComboBoxItem).Value);
                acCtrl.TipoDepreIFRS.Value = Convert.ToByte((this.cmbTipoDecpreciacionME.SelectedItem as ComboBoxItem).Value);
            }
        }

        /// <summary>
        /// Limia los Controls del Header 
        /// </summary>
        private void CleanHeader()
        {
            this.txtNumeroDoc.Text = string.Empty;
        }

        /// <summary>
        /// Funcion que limpia los coontroles del footer
        /// </summary>
        private void CleanFooter()
        {
            this.cmbTipoDecpreciacionME.SelectedItem = 0;
            this.cmbTipoDecpreciacionML.SelectedItem = 0;
            this.txtVidaUtilLocal.Text = string.Empty;
            this.txtVidaUtilIFRS.Text = string.Empty;           
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

        }

        /// <summary>
        /// Maneja campos en las grillas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocument_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (this._noLoaded)
            {
                if (fieldName == "Marca")
                    e.Value = null;
            }

            if (e.IsGetData)
            {
                if (fieldName == "Marca" && e.Value == null)
                {
                    e.Value = this.select.Contains(e.ListSourceRowIndex);
                }
                else
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
                        {
                            e.Value = pi.GetValue(dto, null);
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
        }

        /// <summary>
        /// Maneja campos de controles en la grilla 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocument_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "Marca")
            {
                e.RepositoryItem = this.editChkBox;
            }
            if (fieldName == "ActivoGrupoID" || fieldName == "ActivoClaseID" || fieldName == "LocFisicaID" ||
                fieldName == "ProyectoID" || fieldName == "CentroCostoID" || fieldName == "ActivoTipoID")
            {
                e.RepositoryItem = this.editBtnGrid;
            }
        }

        /// <summary>
        /// Evento que obliga que se ejecute una funcion al cambiar de fila
        /// <param name="fila">Fila que se debe actualizar</param>
        /// <param name="oper">Indica si se debe ejecutar una segunda operacion</param>
        /// </summary>
        protected override void RowIndexChanged(int fila, bool oper)
        {
            try
            {
                if (this._lActivos.Count > 0)
                {
                    #region Valores Moneda Local

                    this.cmbTipoDecpreciacionML.SelectedIndex = Convert.ToInt32(this._lActivos[fila].TipoDepreLOC.Value);
                    //this.txtValorResidualML.EditValue = this._activo[fila].ValorSalvamentoLOC.Value;

                    #endregion

                    #region Valores Moneda Extranjera

                    //this.cmbTipoDecpreciacionME.SelectedIndex = Convert.ToInt32(this._activo[fila].TipoDepreEXT.Value);
                    //this.txtValorResidualME.EditValue = this._activo[fila].ValorSalvamentoEXT.Value;

                    #endregion

                    //Asigna los valores del costo al control de valores para verificar que no sea menor a la del valor residual.
                    //this.txtValorML.Text = this._activo[fila].CostoLOC.Value.Value.ToString();
                    //this.txtValorME.Text = this._activo[fila].CostoEXT.Value.Value.ToString();

                    this.gvDocument.RefreshData();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "Marca")
            {
                this.CalcularTotal();
                this.UpdateTemp(this._lActivos);

                if ((bool)e.Value)
                {
                    this.selected = true;
                    this._validRow = true;
                }
                else
                {
                    this.selected = false;
                    this._validRow = false;
                }
            }
        }

        /// <summary>
        /// Evento que envia el evento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocument_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            if (this._noLoaded)
                return;

            bool selectMark;
            int fila = this.gvDocument.FocusedRowHandle;

            if (this._validRow)
            {
                #region Serial
                if (!this.disableValidate)
                {
                    bool validRow = _bc.ValidGridCell(gvDocument, string.Empty, fila, "SerialID", false, false, false, null);
                    this.deleteOP = false;

                    if (validRow)
                    {
                        selectMark = true;
                        this._validRow = false;
                    }
                    else
                    {
                        e.Allow = false;
                        selectMark = false;
                    }
                }
                #endregion
                #region ActivoGrupoID
                if (!this.disableValidate)
                {
                    bool validRow = _bc.ValidGridCell(gvDocument, string.Empty, fila, "ActivoGrupoID", false, false, false, null);
                    this.deleteOP = false;
                    if (validRow)
                    {
                        selectMark = true;
                        this._validRow = false;
                    }
                    else
                    {
                        e.Allow = false;
                        selectMark = false;
                    }
                }
                #endregion
                #region ActivoClaseID
                if (!this.disableValidate)
                {
                    bool validRow = _bc.ValidGridCell(gvDocument, string.Empty, fila, "ActivoClaseID", false, false, false, null);
                    this.deleteOP = false;

                    if (validRow)
                    {
                        selectMark = true;
                        this._validRow = false;
                    }
                    else
                    {
                        e.Allow = false;
                        selectMark = false;
                    }
                }
                #endregion
                #region LocFisicaID
                if (!this.disableValidate)
                {
                    bool validRow = _bc.ValidGridCell(gvDocument, string.Empty, fila, "LocFisicaID", false, false, false, null);
                    this.deleteOP = false;

                    if (validRow)
                    {
                        selectMark = true;
                        this._validRow = false;
                    }
                    else
                    {
                        e.Allow = false;
                        selectMark = false;
                    }
                }
                #endregion
                #region ProyectoID
                if (!this.disableValidate)
                {
                    bool validRow = _bc.ValidGridCell(gvDocument, string.Empty, fila, "ProyectoID", false, false, false, null);
                    this.deleteOP = false;

                    if (validRow)
                    {
                        selectMark = true;
                        this._validRow = false;
                    }
                    else
                    {
                        e.Allow = false;
                        selectMark = false;
                    }
                }
                #endregion
                #region CentroCostoID
                if (!this.disableValidate)
                {
                    bool validRow = _bc.ValidGridCell(gvDocument, string.Empty, fila, "CentroCostoID", false, false, false, null);
                    this.deleteOP = false;

                    if (validRow)
                    {
                        selectMark = true;
                        this._validRow = false;
                    }
                    else
                    {
                        e.Allow = false;
                        selectMark = false;
                    }
                }
                #endregion
                #region ActivoTipoID
                if (!this.disableValidate)
                {
                    bool validRow = _bc.ValidGridCell(gvDocument, string.Empty, fila, "ActivoTipoID", false, false, false, null);
                    this.deleteOP = false;

                    if (validRow)
                    {
                        selectMark = true;
                        this._validRow = false;
                    }
                    else
                    {
                        e.Allow = false;
                        selectMark = false;
                    }
                }
                #endregion
            }
        }

        #endregion

        #region Eventos Footer

        /// <summary>
        /// Evento que controlar la digitacion del campo este es solo numerico
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNumero_TextChanged(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), "\\d+") && e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back)
                e.Handled = true;
            if (e.KeyChar == 46)
                e.Handled = true;    
            else if (e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back)
            {
                try
                {
                    TextBox txt = (TextBox)sender;
                    string str = txt.Text + e.KeyChar.ToString();
                    Convert.ToByte(str);
                }
                catch (Exception ex)
                {
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Evento que valida la informacion digitada en el TextBox
        /// </summary>
        /// <param name="sender">evento</param>
        /// <param name="e"></param>
        private void txtTurno_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), "\\d+") && e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back)
                e.Handled = true;
            if (e.KeyChar == 46)
                e.Handled = true;
            else if (e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back)
            {
                try
                {
                    TextBox txt = (TextBox)sender;
                    string str = txt.Text + e.KeyChar.ToString();
                    Convert.ToByte(str);

                    if (str == "0" || str == "1" || str == "2")
                        e.Handled = false;
                    else
                        e.Handled = true;

                }
                catch (Exception ex)
                {
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Evento que se ejecuta al salir de un textbox 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void textControl_Leave(object sender, EventArgs e)
        {
            if (this._noLoaded)
                return;

            TextBox ctrl = (TextBox)sender;

            int index = this.gvDocument.FocusedRowHandle;

            switch (ctrl.Name)
            {
                // Vida Util Moneda Local 
                case "txtVidaUtilML":
                    if (string.IsNullOrWhiteSpace(this.txtVidaUtilLocal.Text))
                        this.txtVidaUtilLocal.Text = "1";
                    //this._activo[index].VidaUtilML.Value = Convert.ToInt16(ctrl.Text);
                    break;

                // Vida Util Moneda Exranjera 
                case "txtVidaUtilME":
                    if (string.IsNullOrWhiteSpace(this.txtVidaUtilIFRS.Text))
                        this.txtVidaUtilIFRS.Text = "1";
                    //this._activo[index].VidaUtilME.Value = Convert.ToInt16(ctrl.Text);
                    break;                                  
                case "txtValorResidualML":
                    this.gvDocument.RefreshData();
                    break;
                case "txtValorResidualME":
                    this.gvDocument.RefreshData();
                    break;
            }

            this.gvDocument.RefreshData();
        }

        /// <summary>
        /// Evento que se ejecuta al salir un combo
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void cmbControl_Leave(object sender, EventArgs e)
        {
            ComboBoxEx ctrl = (ComboBoxEx)sender;
            int index = this.gvDocument.FocusedRowHandle;

            switch (ctrl.Name)
            {
                // Combo Tipo depreciacion Moneda Local
                case "cmbTipoDecpreciacionML":
                    this._lActivos[index].TipoDepreLOC.Value = Convert.ToByte((this.cmbTipoDecpreciacionML.SelectedItem as ComboBoxItem).Value);
                    if (this.cmbTipoDecpreciacionML.SelectedIndex == Convert.ToInt32(TipoDepreciacion.SaldosDecrecientes))
                        this.txtValorResidualML.Enabled = true;
                    else
                        this.txtValorResidualML.Enabled = false;
                    break;

                // Combo Tipo depreciacion Moneda extranjera 
                case "cmbTipoDecpreciacionME":
                    //this._activo[index].TipoDepreEXT.Value = Convert.ToByte((this.cmbTipoDecpreciacionME.SelectedItem as ComboBoxItem).Value);
                    if (this.cmbTipoDecpreciacionME.SelectedIndex == Convert.ToInt32(TipoDepreciacion.SaldosDecrecientes))
                        this.txtValorResidualME.Enabled = true;
                    else
                        this.txtValorResidualME.Enabled = false;
                    break;
            }
            this.gvDocument.RefreshData();
        }

        /// <summary>
        /// Evento q habilita el valor resifdual
        /// </summary>
        /// <param name="sender">e</param>
        /// <param name="e">e</param>
        private void cmbTipoDecpreciacionML_SelectedIndexChanged(object sender, EventArgs e)
        {

            int fila = this.gvDocument.FocusedRowHandle;
            if (fila < 0)
                return;
            if (this.cmbTipoDecpreciacionML.SelectedIndex == Convert.ToInt32(TipoDepreciacion.SaldosDecrecientes))
            {
                this.txtValorResidualML.Enabled = true;
                this._lActivos[fila].TipoDepreLOC.Value = 1;
            }
            else
                this.txtValorResidualML.Enabled = false;

            if (this.cmbTipoDecpreciacionML.SelectedIndex == Convert.ToInt32(TipoDepreciacion.LineaRecta))
                this._lActivos[fila].TipoDepreLOC.Value = 0;

            if (this.cmbTipoDecpreciacionML.SelectedIndex == Convert.ToInt32(TipoDepreciacion.UnidadesDeProduccion))
                this._lActivos[fila].TipoDepreLOC.Value = 2;
        }

        /// <summary>
        /// Evento q habilita el valor resifdual
        /// </summary>
        /// <param name="sender">e</param>
        /// <param name="e">e</param>
        private void cmbTipoDecpreciacionME_SelectedIndexChanged(object sender, EventArgs e)
        {
            int fila = this.gvDocument.FocusedRowHandle;
            if (fila < 0)
                return;
            if (this.cmbTipoDecpreciacionME.SelectedIndex == Convert.ToInt32(TipoDepreciacion.SaldosDecrecientes))
            {
                this.txtValorResidualME.Enabled = true;
                //this._activo[fila].TipoDepreEXT.Value = 1;
            }
            else
                this.txtValorResidualME.Enabled = false;

            //if (this.cmbTipoDecpreciacionME.SelectedIndex == Convert.ToInt32(TipoDepreciacion.LineaRecta))
                //this._activo[fila].TipoDepreEXT.Value = 0;

            //if (this.cmbTipoDecpreciacionME.SelectedIndex == Convert.ToInt32(TipoDepreciacion.UnidadesDeProduccion))
                //this._activo[fila].TipoDepreEXT.Value = 2;
        }

        /// <summary>
        /// Evento q asigna el valor del control al dto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtValorResidualML_EditValueChanged(object sender, EventArgs e)
        {
            int fila = this.gvDocument.FocusedRowHandle;
            if (fila < 0)
                return;
            //this._activo[fila].ValorSalvamentoLOC.Value = Convert.ToInt32(this.txtValorResidualML.EditValue);
        }

        /// <summary>
        /// Evento q asigna el valor del control al dto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtValorResidualME_EditValueChanged(object sender, EventArgs e)
        {
            int fila = this.gvDocument.FocusedRowHandle;
            if (fila < 0)
                return;
           // this._activo[fila].ValorSalvamentoEXT.Value = Convert.ToInt32(this.txtValorResidualME.EditValue);
        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Restablece los valores iniciales en el formulario
        /// </summary>
        public override void TBNew()
        {
            try
            {
                if (this._lActivos.Count > 0)
                {
                    string msgTitleWarning = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
                    string msgNewDoc = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewDocument);

                    if (MessageBox.Show(msgNewDoc, msgTitleWarning, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);
                        this._lActivos = new List<DTO_acActivoControl>();
                        this.Invoke(this.refresGrd);
                        this.EnableFooter(false);
                        this.EnableHeader(true);
                                              
                    }
                }

                if (this._lActivos.Count == 0)
                {
                    _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);
                    this._lActivos = new List<DTO_acActivoControl>();
                    this.Invoke(this.refresGrd);
                    this.EnableFooter(false);
                    this.EnableHeader(true);
                }
                this._noLoaded = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            base.TBSave();
            this.gvDocument.PostEditor();
            try
            {
                if (base.select.Count == 0)
                    return;

                //validarChek();
                if (this.selected == false)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ac_anyRowSelected));
                    return;
                }
                if (!this.validMove)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ac_invalidMove));
                    return;
                }
                DTO_TxResult res = this.ValidarData();

                if (res.Result == ResultValue.OK)
                {
                    _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);
                    this.CompleteData();
                    Thread process = new Thread(this.SaveThread);
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
                this.gvDocument.PostEditor();
                List<DTO_acActivoControl> listAppr = new List<DTO_acActivoControl>();

                foreach (int index in this.select)
                {
                    listAppr.Add(this._lActivos[index]);
                    //Valida que no haya una plaqueta repetida en la bd
                    this.ValidarPlaqueta(index);
                }
                //Valida que la info que viene no repita la plaqueta
                List<string> indices = listAppr.Select(x => x.PlaquetaID.Value).Distinct().ToList();

                if (!this.isValid || indices.Count != listAppr.Count)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ac_DuplicatePlate));
                    return;
                }
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCont(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                //TODO revisar
                List<DTO_TxResult> results = _bc.AdministrationModel.acActivoControl_AddList(this.documentID, this._actFlujo.ID.Value, this.txtPrefix.Text, listAppr, "", 0);
                FormProvider.Master.StopProgressBarThread(this.documentID);
                List<DTO_TxResult> resultsNOK = new List<DTO_TxResult>();
                foreach (DTO_TxResult result in results)
                {
                    if ((result.Details != null && result.Details.Count > 0) || !string.IsNullOrWhiteSpace(result.ResultMessage))
                        resultsNOK.Add(result);
                }

                MessageForm frm = new MessageForm(resultsNOK);
                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });

                if (resultsNOK.Count == 0)
                {
                    _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);

                    this.newDoc = true;
                    this.deleteOP = true;
                    this._lActivos = new List<DTO_acActivoControl>();
                    this.Invoke(this.saveDelegate);
                    this.Invoke(this.refresGrd);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion

        private void btnBuscarActivo_Click(object sender, EventArgs e)
        {
            FindActivosModal findModal = new FindActivosModal();
            findModal.ShowDialog();
        }

       
    }
}