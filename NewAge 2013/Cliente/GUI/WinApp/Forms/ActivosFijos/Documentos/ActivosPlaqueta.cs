using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using System.Threading;
using NewAge.DTO.Resultados;


namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de documentos
    /// </summary>
    public partial class ActivosPlaqueta : DocumentForm
    {
        //public ActivosPlaqueta()
        //{
        //    this.InitializeComponent();
        //}

        #region Delegados
        public delegate void RefreshGrid();
        public RefreshGrid refresGrd;
        protected override void RefreshGridMethod()
        {
            this._activo = new List<DTO_acActivoControl>();
            //this.LoadData(false);
            base.select = new List<int>();
            this.CleanData();
        }
        #endregion

        #region Variables Formulario

        private BaseController _bc = BaseController.GetInstance();

        private DTO_coTercero _coTercero = null;
        private FormTypes _frmType = FormTypes.Document;
        private DTO_glDocumentoControl _glDocumentoControl = null;
        private string _documento = string.Empty;
        private bool selected = false;
        private bool isValid = true;
        private bool _Clean = false;
        private List<DTO_acActivoControl> _activo;

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Carga la información de las grilla
        /// </summary>
        /// <param name="firstTime">Si es la primera vez que carga la grilla</param>
        /// <param name="refresh">Si debe volver a cargar la data del servidor</param>
        protected override void LoadData(bool firstTime)
        {
            this.gcDocument.DataSource = this._activo;
            this.gcDocument.RefreshDataSource();
        }

        /// <summary>
        /// Inicializa los parametros de la pantalla
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.ActivosPlaqueta;
            InitializeComponent();

            base.SetInitParameters();

            //Inicia los Delegados
            this.refresGrd = new RefreshGrid(RefreshGridMethod);
            // this.InitControls();
            this.AddGridCols();

            //Carga info del formulario
            this.userID = this._bc.AdministrationModel.User.ReplicaID.Value.Value;
            this.areaFuncionalID = this._bc.AdministrationModel.User.AreaFuncionalID.Value;
            this.frmModule = ModulesPrefix.ac;

            //Cargar los Controles de Mestras
            _bc.InitMasterUC(this.MasterClase, AppMasters.acClase, true, true, true, false);
            _bc.InitMasterUC(this.MasterLocalizacionFisica, AppMasters.glLocFisica, true, true, true, false);
            _bc.InitMasterUC(this.MasterReferencia, AppMasters.inReferencia, true, true, true, false);
            _bc.InitMasterUC(this.MasterTercero, AppMasters.coTercero, true, true, true, false);
            _bc.InitMasterUC(this.MasterTipo, AppMasters.acTipo, true, true, true, false);
            _bc.InitMasterUC(this.MasterGrupo, AppMasters.acGrupo, true, true, true, false);
            _bc.InitMasterUC(this.MasterResponsable, AppMasters.coTercero, true, true, true, false);
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
                plaquetaID.Width = 150;
                plaquetaID.Visible = true;
                plaquetaID.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(plaquetaID);

                //Serial
                GridColumn serialID = new GridColumn();
                serialID.FieldName = this.unboundPrefix + "SerialID";
                serialID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_SerialID");
                serialID.UnboundType = UnboundColumnType.Integer;
                serialID.VisibleIndex = 2;
                serialID.Width = 100;
                serialID.Visible = true;
                serialID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(serialID);

                //Referencia
                GridColumn inReferenciaID = new GridColumn();
                inReferenciaID.FieldName = this.unboundPrefix + "inReferenciaID";
                inReferenciaID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_inReferenciaID");
                inReferenciaID.UnboundType = UnboundColumnType.String;
                inReferenciaID.VisibleIndex = 3;
                inReferenciaID.Width = 150;
                inReferenciaID.Visible = true;
                inReferenciaID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(inReferenciaID);

                //Descripcion
                GridColumn descripcion = new GridColumn();
                descripcion.FieldName = this.unboundPrefix + "Descripcion";
                descripcion.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Observacion");
                descripcion.UnboundType = UnboundColumnType.String;
                descripcion.VisibleIndex = 4;
                descripcion.Width = 200;
                descripcion.Visible = true;
                descripcion.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(descripcion);

                //Localizacion Fisica
                GridColumn locFisica = new GridColumn();
                locFisica.FieldName = this.unboundPrefix + "LocFisicaID";
                locFisica.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_LocFisicaID");
                locFisica.UnboundType = UnboundColumnType.String;
                locFisica.VisibleIndex = 5;
                locFisica.Width = 120;
                locFisica.Visible = true;
                locFisica.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(locFisica);

                //Clase
                GridColumn clase = new GridColumn();
                clase.FieldName = this.unboundPrefix + "ActivoClaseID";
                clase.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ActivoClaseID");
                clase.UnboundType = UnboundColumnType.String;
                clase.VisibleIndex = 6;
                clase.Width = 120;
                clase.Visible = true;
                clase.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(clase);

                //Tipo
                GridColumn ActivoTipoID = new GridColumn();
                ActivoTipoID.FieldName = this.unboundPrefix + "ActivoTipoID";
                ActivoTipoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ActivoTipoID");
                ActivoTipoID.UnboundType = UnboundColumnType.String;
                ActivoTipoID.VisibleIndex = 7;
                ActivoTipoID.Width = 120;
                ActivoTipoID.Visible = true;
                ActivoTipoID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ActivoTipoID);

                //Grupo
                GridColumn grupo = new GridColumn();
                grupo.FieldName = this.unboundPrefix + "ActivoGrupoID";
                grupo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ActivoGrupoID");
                grupo.UnboundType = UnboundColumnType.String;
                grupo.VisibleIndex = 8;
                grupo.Width = 120;
                grupo.Visible = true;
                grupo.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(grupo);

                //Grupo
                GridColumn responsable = new GridColumn();
                responsable.FieldName = this.unboundPrefix + "Responsable";
                responsable.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Responsable");
                responsable.UnboundType = UnboundColumnType.String;
                responsable.VisibleIndex = 9;
                responsable.Width = 120;
                responsable.Visible = true;
                responsable.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(responsable);

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
        }

        #endregion

        #region Funciones MDI

        protected override void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                base.Form_Enter(sender, e);
                //Manejo de Botones de la Barra de Herramientas
                FormProvider.Master.itemSave.Enabled = true;
                FormProvider.Master.itemSearch.Visible = true;
                FormProvider.Master.itemSearch.Enabled = false;
                FormProvider.Master.itemGenerateTemplate.Visible = false;
                FormProvider.Master.itemGenerateTemplate.Enabled = false;
                FormProvider.Master.Form_Enter(this, this.documentID, this._frmType, this.frmModule);
                FormProvider.Master.itemSearch.Visible = true;
                FormProvider.Master.itemUpdate.Visible = true;
                FormProvider.Master.itemSearch.Enabled = true;
                FormProvider.Master.itemUpdate.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DeclaracionImpuestos.cs", "Form_Enter"));
            }
        }
        #endregion

        #region Funciones Privadas
        /// <summary>
        /// Funcion que verifica la informacion que se le envia al DTO
        /// </summary>
        /// <returns>TxResult</returns>
        private DTO_TxResult ValidarData()
        {
            try
            {
                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.OK;

                result.Details = new List<DTO_TxResultDetail>();
                DTO_TxResultDetail rd = new DTO_TxResultDetail();
                DTO_TxResultDetailFields drF;

                //Nombre de las columnas.
                string colPlaquetaID = string.Empty;

                foreach (int index in this.select)
                {
                    DTO_acActivoControl acCtrl = this._activo[index];
                    rd = new DTO_TxResultDetail();
                    rd.Message = string.Empty;

                    drF = new DTO_TxResultDetailFields();
                    rd.line = index;

                    #region PlaquetaID
                    colPlaquetaID = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PlaquetaID");
                    drF = _bc.ValidGridCell(colPlaquetaID, acCtrl.PlaquetaID.Value, false, false, false, null);

                    if (drF != null)
                        rd.DetailsFields.Add(drF);
                    #endregion

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
        /// Limpia los controles del header
        /// </summary>
        private void CleanData()
        {
            //Limpia contrloes del Header
            this.MasterClase.Value = string.Empty;
            this.MasterLocalizacionFisica.Value = string.Empty;
            this.MasterReferencia.Value = string.Empty;
            this.MasterTercero.Value = string.Empty;
            this.MasterTipo.Value = string.Empty;
            this.MasterGrupo.Value = string.Empty;
            this.txtFactura.Text = string.Empty;
            this.txtPlaqueta.Text = string.Empty;

            //Limpia la grilla
            this._activo = new List<DTO_acActivoControl>();
            this.gcDocument.DataSource = this._activo;
        }

        /// <summary>
        /// Verifica que no se duplique la plaqueta.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>isValid</returns>
        private bool ValidarPlaqueta(int index)
        {
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

        #endregion

        #region Eventos Barra de Herramientas

        public override void TBSearch()
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;

            if (string.IsNullOrWhiteSpace(this.MasterTipo.Value) && string.IsNullOrWhiteSpace(this.MasterTercero.Value) &&
                string.IsNullOrWhiteSpace(this.MasterResponsable.Value) && string.IsNullOrWhiteSpace(this.MasterReferencia.Value)
                && string.IsNullOrWhiteSpace(this.MasterLocalizacionFisica.Value) && string.IsNullOrWhiteSpace(this.MasterGrupo.Value)
                && string.IsNullOrWhiteSpace(this.MasterClase.Value) && string.IsNullOrWhiteSpace(this.txtPlaqueta.Text)
                && string.IsNullOrWhiteSpace(this.txtFactura.Text))
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ac_noFilterSelected));
                return;
            }
            if (string.IsNullOrWhiteSpace(this.MasterTipo.Value) && string.IsNullOrWhiteSpace(this.MasterTercero.Value) &&
                string.IsNullOrWhiteSpace(this.MasterResponsable.Value) && string.IsNullOrWhiteSpace(this.MasterReferencia.Value)
                && string.IsNullOrWhiteSpace(this.MasterLocalizacionFisica.Value) && string.IsNullOrWhiteSpace(this.MasterGrupo.Value)
                && string.IsNullOrWhiteSpace(this.MasterClase.Value) && !string.IsNullOrWhiteSpace(this.txtPlaqueta.Text))
            {
                base.TBSearch();
                DTO_acActivoControl plaqutaDto = new DTO_acActivoControl();
                plaqutaDto.PlaquetaID.Value = this.txtPlaqueta.Text;
                this._activo = this._bc.AdministrationModel.acActivoControl_GetByParameterForTranfer(plaqutaDto);
                this.LoadData(true);
                if (this._activo.Count == 0)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
                    MessageForm mform = new MessageForm(result);
                    mform.ShowDialog();
                }
                return;
            }

            base.TBSearch();
            this.gvDocument.PostEditor();
            if (string.IsNullOrWhiteSpace(this.txtFactura.Text) && this.MasterTercero.Value != string.Empty)
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.TerceroIsEmpty));
                return;
            }
            if (!string.IsNullOrWhiteSpace(this.txtFactura.Text) && this.MasterTercero.Value == "")
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FacturaIsEmpty));
                return;
            }
            DTO_acActivoControl DTOacControl = new DTO_acActivoControl();

            DTOacControl.DocumentoTercero.Value = this.txtFactura.Text;
            DTOacControl.PlaquetaID.Value = this.txtPlaqueta.Text;
            DTOacControl.Responsable.Value = this.MasterResponsable.Value;
            DTOacControl.TerceroID.Value = this.MasterTercero.Value;
            DTOacControl.inReferenciaID.Value = this.MasterReferencia.Value;
            DTOacControl.LocFisicaID.Value = this.MasterLocalizacionFisica.Value;
            DTOacControl.ActivoClaseID.Value = this.MasterClase.Value;
            DTOacControl.ActivoTipoID.Value = this.MasterTipo.Value;
            DTOacControl.ActivoGrupoID.Value = this.MasterGrupo.Value;

            this._activo = this._bc.AdministrationModel.acActivoControl_GetBy_Parameter(DTOacControl);
            this.LoadData(true);

            if (this._activo.Count == 0)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
                MessageForm mform = new MessageForm(result);
                mform.ShowDialog();
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
                //validarChek();
                if (base.select.Count == 0)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ac_NoSelected));
                    return;
                }
                DTO_TxResult res = this.ValidarData();

                if (res.Result == ResultValue.OK)
                {
                    Thread process = new Thread(this.SaveThread);
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Restablece los valores iniciales en el formulario
        /// </summary>
        public override void TBNew()
        {
            try
            {
                base.select = new List<int>();
                string msgTitleWarning = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
                string msgNewDoc = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewDocument);

                if (MessageBox.Show(msgNewDoc, msgTitleWarning, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.CleanData();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Eventos Grilla
        
        /// <summary>
        /// Maneja campos en las grillas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocument_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

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

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo de salvar
        /// </summary>
        public override void SaveThread()
        {
            try
            {
                this.isValid = true;
                List<DTO_acActivoControl> listAppr = new List<DTO_acActivoControl>();
                int acID = 0;
                for (int index = 0; index < this.select.Count; index++)
                {
                    listAppr.Add(this._activo[index]);
                    acID = (int)listAppr[index].ActivoID.Value;
                    this.ValidarPlaqueta(index);
                }
                //Valida que la info que viene no repita la plaqueta
                List<string> indices = listAppr.Select(x => x.PlaquetaID.Value).Distinct().ToList();

                if (!this.isValid || indices.Count != listAppr.Count)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ac_DuplicatePlate));
                    return;
                }
                string tipoMov = _bc.GetControlValueByCompany(ModulesPrefix.ac, AppControl.ac_TipoMovimientoCambioDatos);
                if (tipoMov == "")
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ac_NoAssignedMove));
                    return;
                }
                DTO_TxResult result = _bc.AdministrationModel.acActivoControl_PlaquetaUpdate(listAppr, tipoMov, acID, this.prefijoID);
                MessageForm frm = new MessageForm(result);
                if (result.Result == ResultValue.NOK)
                {
                    frm = new MessageForm(result);
                    frm.ShowDialog();
                }
                else
                {
                    frm.ShowDialog();
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
    }
}