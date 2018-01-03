using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraRichEdit.API.Word;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.DTO.Negocio.Documentos.Activos;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using System.Threading;
using NewAge.DTO.Resultados;
using DevExpress.XtraReports.UI;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de documentos
    /// </summary>
    public partial class TransaccionActivos : DocumentForm
    {
        //public TransaccionActivos()
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
            this.select = new List<int>();
            this._activoSaldos = new List<DTO_acActivoSaldo>();
            this.idTrs = new List<int>();
            this.detalles = null;
            List<DTO_acActivoSaldo> d = null;
            this.gcDocument.RefreshDataSource();
            this.LoadData(false);
            this.LimpiarHeader();
        }
        #endregion

        #region Variables Formulario

        private BaseController _bc = BaseController.GetInstance();

        private FormTypes _frmType = FormTypes.Document;
        private bool isValid = true;

        private List<DTO_acActivoControl> _activo;
        private List<DTO_acActivoSaldo> _activoSaldos;
        private int _idTR = 0;
        private string _acClase;
        private int fila = 0;
        private string _monedaLocal;
        private string _monedaExtranjera;
        private List<DTO_coCuentaSaldo> _coCuentaSaldo;
        List<int> idTrs = new List<int>();
        List<DTO_acActivoSaldo> detalles = new List<DTO_acActivoSaldo>();
        private bool validMove;
        private string mvtoTipo = null;

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
            this.documentID = AppDocuments.TransaccionActivos;
            InitializeComponent();

            base.SetInitParameters();

            // this.InitControls();
            this.AddGridCols();

            //Inicializa los delegados
            this.refresGrd = new RefreshGrid(RefreshGridMethod);

            //Carga info del formulario
            this.userID = this._bc.AdministrationModel.User.ReplicaID.Value.Value;
            this.areaFuncionalID = this._bc.AdministrationModel.User.AreaFuncionalID.Value;
            this.frmModule = ModulesPrefix.ac;

            //Cargar los Controles de Mestras
            _bc.InitMasterUC(this.MasterMvtoTipo, AppMasters.acMovimientoTipo, true, true, true, false);
            _bc.InitMasterUC(this.MasterCentroCosto, AppMasters.coCentroCosto, true, true, true, false);
            _bc.InitMasterUC(this.MasterProyecto, AppMasters.coProyecto, true, true, true, false);

            //Deshabilita los botones +- de la grilla            
            this.gcDocument.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.gcDocument.EmbeddedNavigator.Buttons.CustomButtons[0].Enabled = false;

            //Deshabilita los botones +- de la grilla            
            this.gcValores.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.gcValores.EmbeddedNavigator.Buttons.CustomButtons[0].Enabled = false;

            //Permite modificar los paneles
            this.tlSeparatorPanel.RowStyles[0].Height = 50;
            //this.tlSeparatorPanel.RowStyles[0].Height = 75;
            this.tlSeparatorPanel.RowStyles[2].Height = 250;

            this.detalles = new List<DTO_acActivoSaldo>();
            this._activoSaldos = new List<DTO_acActivoSaldo>();
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected override void AddGridCols()
        {
            try
            {
                base.AddGridCols();
                #region gvDocument
                int documentoAlta = AppDocuments.TransaccionActivos;

                //Plaqueta
                GridColumn plaquetaID = new GridColumn();
                plaquetaID.FieldName = this.unboundPrefix + "PlaquetaID";
                plaquetaID.Caption = _bc.GetResource(LanguageTypes.Forms, documentoAlta + "_PlaquetaID");
                plaquetaID.UnboundType = UnboundColumnType.String;
                plaquetaID.VisibleIndex = 1;
                plaquetaID.Width = 110;
                plaquetaID.Visible = true;
                plaquetaID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(plaquetaID);

                //Serial
                GridColumn serialID = new GridColumn();
                serialID.FieldName = this.unboundPrefix + "SerialID";
                serialID.Caption = _bc.GetResource(LanguageTypes.Forms, documentoAlta + "_SerialID");
                serialID.UnboundType = UnboundColumnType.Integer;
                serialID.VisibleIndex = 2;
                serialID.Width = 80;
                serialID.Visible = true;
                serialID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(serialID);


                //Referencia
                GridColumn inReferenciaID = new GridColumn();
                inReferenciaID.FieldName = this.unboundPrefix + "inReferenciaID";
                inReferenciaID.Caption = _bc.GetResource(LanguageTypes.Forms, documentoAlta + "_inReferenciaID");
                inReferenciaID.UnboundType = UnboundColumnType.String;
                inReferenciaID.VisibleIndex = 3;
                inReferenciaID.Width = 150;
                inReferenciaID.Visible = true;
                inReferenciaID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(inReferenciaID);

                //Descripcion
                GridColumn descripcion = new GridColumn();
                descripcion.FieldName = this.unboundPrefix + "Observacion";
                descripcion.Caption = _bc.GetResource(LanguageTypes.Forms, documentoAlta + "_Observacion");
                descripcion.UnboundType = UnboundColumnType.String;
                descripcion.VisibleIndex = 4;
                descripcion.Width = 300;
                descripcion.Visible = true;
                descripcion.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(descripcion);

                //Localizacion Fisica
                GridColumn locFisica = new GridColumn();
                locFisica.FieldName = this.unboundPrefix + "LocFisicaID";
                locFisica.Caption = _bc.GetResource(LanguageTypes.Forms, documentoAlta + "_LocFisicaID");
                locFisica.UnboundType = UnboundColumnType.String;
                locFisica.VisibleIndex = 5;
                locFisica.Width = 120;
                locFisica.Visible = true;
                locFisica.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(locFisica);

                //Clase
                GridColumn clase = new GridColumn();
                clase.FieldName = this.unboundPrefix + "ActivoClaseID";
                clase.Caption = _bc.GetResource(LanguageTypes.Forms, documentoAlta + "_ActivoClaseID");
                clase.UnboundType = UnboundColumnType.String;
                clase.VisibleIndex = 6;
                clase.Width = 120;
                clase.Visible = true;
                clase.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(clase);

                //Tipo
                GridColumn ActivoTipoID = new GridColumn();
                ActivoTipoID.FieldName = this.unboundPrefix + "ActivoTipoID";
                ActivoTipoID.Caption = _bc.GetResource(LanguageTypes.Forms, documentoAlta + "_ActivoTipoID");
                ActivoTipoID.UnboundType = UnboundColumnType.String;
                ActivoTipoID.VisibleIndex = 7;
                ActivoTipoID.Width = 120;
                ActivoTipoID.Visible = true;
                ActivoTipoID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ActivoTipoID);

                //Grupo
                GridColumn grupo = new GridColumn();
                grupo.FieldName = this.unboundPrefix + "ActivoGrupoID";
                grupo.Caption = _bc.GetResource(LanguageTypes.Forms, documentoAlta + "_ActivoGrupoID");
                grupo.UnboundType = UnboundColumnType.String;
                grupo.VisibleIndex = 8;
                grupo.Width = 120;
                grupo.Visible = true;
                grupo.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(grupo);

                //Grupo
                GridColumn responsable = new GridColumn();
                responsable.FieldName = this.unboundPrefix + "Responsable";
                responsable.Caption = _bc.GetResource(LanguageTypes.Forms, documentoAlta + "_Responsable");
                responsable.UnboundType = UnboundColumnType.String;
                responsable.VisibleIndex = 9;
                responsable.Width = 120;
                responsable.Visible = true;
                responsable.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(responsable);
                #endregion
                #region gvValores

                //descriptivo
                GridColumn descriptivo = new GridColumn();
                descriptivo.FieldName = this.unboundPrefix + "Descriptivo";
                descriptivo.Caption = _bc.GetResource(LanguageTypes.Forms, documentoAlta + "_Descriptivo");
                descriptivo.UnboundType = UnboundColumnType.String;
                descriptivo.VisibleIndex = 2;
                descriptivo.Width = 200;
                descriptivo.Visible = true;
                descriptivo.OptionsColumn.AllowEdit = false;
                this.gvValores.Columns.Add(descriptivo);

                //acComponenteID
                GridColumn acComponenteID = new GridColumn();
                acComponenteID.FieldName = this.unboundPrefix + "acComponenteID";
                acComponenteID.Caption = _bc.GetResource(LanguageTypes.Forms, documentoAlta + "_Componente");
                acComponenteID.UnboundType = UnboundColumnType.Integer;
                acComponenteID.VisibleIndex = 1;
                acComponenteID.Width = 90;
                acComponenteID.Visible = true;
                acComponenteID.OptionsColumn.AllowEdit = false;
                this.gvValores.Columns.Add(acComponenteID);


                //SaldoMLoc
                GridColumn SaldoMLoc = new GridColumn();
                SaldoMLoc.FieldName = this.unboundPrefix + "SaldoMLoc";
                SaldoMLoc.Caption = _bc.GetResource(LanguageTypes.Forms, documentoAlta + "_SaldoMLoc");
                SaldoMLoc.UnboundType = UnboundColumnType.String;
                SaldoMLoc.VisibleIndex = 3;
                SaldoMLoc.Width = 70;
                SaldoMLoc.Visible = true;
                SaldoMLoc.OptionsColumn.AllowEdit = false;
                this.gvValores.Columns.Add(SaldoMLoc);

                //SaldoMExt
                GridColumn SaldoMExt = new GridColumn();
                SaldoMExt.FieldName = this.unboundPrefix + "SaldoMExt";
                SaldoMExt.Caption = _bc.GetResource(LanguageTypes.Forms, documentoAlta + "_SaldoMExt");
                SaldoMExt.UnboundType = UnboundColumnType.String;
                SaldoMExt.VisibleIndex = 4;
                SaldoMExt.Width = 70;
                SaldoMExt.Visible = true;
                SaldoMExt.OptionsColumn.AllowEdit = false;
                this.gvValores.Columns.Add(SaldoMExt);

                //SaldoLoc
                GridColumn SaldoLoc = new GridColumn();
                SaldoLoc.FieldName = this.unboundPrefix + "SaldoLoc";
                SaldoLoc.Caption = _bc.GetResource(LanguageTypes.Forms, documentoAlta + "_SaldoLoc");
                SaldoLoc.UnboundType = UnboundColumnType.Decimal;
                SaldoLoc.VisibleIndex = 5;
                SaldoLoc.Width = 70;
                SaldoLoc.Visible = true;
                SaldoLoc.OptionsColumn.AllowEdit = true;
                this.gvValores.Columns.Add(SaldoLoc);

                //SaldoExt
                GridColumn SaldoExt = new GridColumn();
                SaldoExt.FieldName = this.unboundPrefix + "SaldoExt";
                SaldoExt.Caption = _bc.GetResource(LanguageTypes.Forms, documentoAlta + "_SaldoExt");
                SaldoExt.UnboundType = UnboundColumnType.Decimal;
                SaldoExt.VisibleIndex = 6;
                SaldoExt.Width = 70;
                SaldoExt.Visible = true;
                SaldoExt.OptionsColumn.AllowEdit = true;
                this.gvValores.Columns.Add(SaldoExt);
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TransaccionActivos.cs", "AddGridCols"));
            }
        }

        #endregion

        #region Funciones Privadas

        private void LimpiarHeader()
        {
            this.MasterMvtoTipo.Value = string.Empty;
            this.MasterCentroCosto.Value = string.Empty;
            this.MasterProyecto.Value = string.Empty;
            this.txtSerial.Text = string.Empty;
            this.txtPlaqueta.Text = string.Empty;
            this.gcValores.DataSource = null;
            this.gcValores.RefreshDataSource();
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TransaccionActivos.cs", "Form_Enter"));
            }
        }
        #endregion

        #region Eventos Header
        /// <summary>
        /// Funcionque valida si la informacionel master movimiento escorrecta y es igual al documentID
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterMovimientoTipo_Leave(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(this.MasterMvtoTipo.Value))
                    return;

                this.mvtoTipo = this.MasterMvtoTipo.Value;
                DTO_acMovimientoTipo MvtoTipo = (DTO_acMovimientoTipo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.acMovimientoTipo, false, this.mvtoTipo, true);
                DTO_coDocumento coDoc = (DTO_coDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, false, MvtoTipo.coDocumentoID.Value, true);

                if (coDoc.DocumentoID.Value != this.documentID.ToString())
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ac_invalidMove));
                    this.MasterMvtoTipo.Value = string.Empty;
                    this.validMove = false;
                }
                else
                    this.validMove = true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Restablece los valores iniciales en el formulario
        /// </summary>
        public override void TBNew()
        {
            this.Invoke(this.refresGrd);
        }

        /// <summary>
        /// Realiza la busqueda de acuerdo al criterio
        /// </summary>
        public override void TBSearch()
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;

            if (string.IsNullOrWhiteSpace(this.MasterMvtoTipo.Value))
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ac_invalidMove));
                return;
            }
            if (string.IsNullOrWhiteSpace(this.txtPlaqueta.Text) && string.IsNullOrWhiteSpace(this.txtSerial.Text) && string.IsNullOrWhiteSpace(this.txtPlaqueta.Text)
                && string.IsNullOrWhiteSpace(this.txtSerial.Text) && string.IsNullOrWhiteSpace(this.MasterCentroCosto.Value) && string.IsNullOrWhiteSpace(this.MasterProyecto.Value))
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ac_noFilterSelected));
                return;
            }
            if (!string.IsNullOrWhiteSpace(this.txtPlaqueta.Text) || !string.IsNullOrWhiteSpace(this.txtSerial.Text) || !string.IsNullOrWhiteSpace(this.MasterCentroCosto.Value)
                || !string.IsNullOrWhiteSpace(this.MasterProyecto.Value))
            {
                base.TBSearch();

                DTO_acActivoControl DTOacControl = new DTO_acActivoControl();

                DTOacControl.DocumentoTercero.Value = this.txtSerial.Text;
                DTOacControl.PlaquetaID.Value = this.txtPlaqueta.Text;
                DTOacControl.ProyectoID.Value = this.MasterProyecto.Value;
                DTOacControl.CentroCostoID.Value = this.MasterCentroCosto.Value;

                this._activo = this._bc.AdministrationModel.acActivoControl_GetByParameterForTranfer(DTOacControl);
                if (this._activo.Count != 0)
                {
                    this._idTR = Convert.ToInt32(this._activo.First().ActivoID.Value);
                    this.LoadData(true);
                }

                if (this._activo.Count == 0)
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
                    MessageForm mform = new MessageForm(result);
                    mform.ShowDialog();
                }
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            base.TBSave();
            this.gvDocument.PostEditor();
            this.gvValores.PostEditor();

            try
            {
                if (base.select.Count == 0)
                    return;

                Thread process = new Thread(this.SaveThread);
                process.Start();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Eventos Grilla

        #region gvDocument

        /// <summary>
        /// Evento que carga la grilla de valores.
        /// </summary>
        /// <param name="sender">Evento</param>
        /// <param name="e">Envento</param>
        protected override void gvDocument_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            this.fila = e.FocusedRowHandle;
            List<DTO_acActivoSaldo> componentes = new List<DTO_acActivoSaldo>();

            DateTime periodo = this.dtPeriod.DateTime;
            if (this._activo.Count > 0)
            {
                int idTr = Convert.ToInt32(this._activo[fila].ActivoID.Value);
                this._acClase = this._activo[fila].ActivoClaseID.Value;
                componentes = this._bc.AdministrationModel.acActivoControl_GetComponentesPorClaseActivoID(_acClase);

                if (!this.idTrs.Contains(idTr))
                {
                    this.detalles = this._bc.AdministrationModel.acActivoControl_GetSaldoXComponente(Convert.ToDateTime(periodo), componentes, idTr, _acClase);
                    this.idTrs.Add(idTr);
                    this._activoSaldos.AddRange(this.detalles);
                }

                List<DTO_acActivoSaldo> d = this._activoSaldos.Where(p => p.IdentificadorTR.Value == idTr).ToList();
                this.gcValores.DataSource = d;
                this.gcValores.RefreshDataSource();
            }
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

        #region gvValores
        /// <summary>
        /// Maneja campos en las grillas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvValores_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            int fila = e.ListSourceRowIndex;
            var grilla = this.gvValores.GetRow(fila);
            var grilla2 = this.gvDocument.GetRow(fila);
            Object dto = (Object)grilla;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (pi != null)
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                        e.Value = pi.GetValue(dto, null);
                    else
                        e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
            }
            if (e.IsSetData)
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
            }
        }

        /// <summary>
        /// Evento q se ejecuta al cambiar de celda
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvValores_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            this._monedaLocal = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
            this._monedaExtranjera = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);

            int valor = Convert.ToInt32(e.Value);

            int filaGvValores = e.RowHandle;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            int idTR = Convert.ToInt32(this._activo[fila].ActivoID.Value);
            string conceptosaldo = this._activoSaldos[filaGvValores].acComponenteID.Value;

            this._activoSaldos[filaGvValores].dtoCoCuentaSaldo.IdentificadorTR.Value = idTR;
            this._activoSaldos[filaGvValores].dtoCoCuentaSaldo.ConceptoSaldoID.Value = conceptosaldo;

            switch (fieldName)
            {
                case "SaldoLoc":
                    this._activoSaldos[fila].SaldoLoc.Value = valor;
                    break;
                case "SaldoExt":
                    this._activoSaldos[fila].SaldoExt.Value = valor;
                    break;
            }

            this.gcValores.RefreshDataSource();
            this.gcDocument.RefreshDataSource();
        }
        #endregion

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
                List<DTO_acActivoSaldo> listAppr = new List<DTO_acActivoSaldo>();
                List<DTO_acActivoControl> listActivo = new List<DTO_acActivoControl>();
                List<DTO_acActivoSaldo> saldosCU = new List<DTO_acActivoSaldo>();
                foreach (int index in this.select)
                {
                    listActivo.Add(this._activo[index]);
                }
                for (int i = 0; i < listActivo.Count; i++)
                {
                    saldosCU = this._activoSaldos.Where(x => x.IdentificadorTR.Value == listActivo[i].ActivoID.Value && x.SaldoLoc.Value != null && x.SaldoExt.Value != null && x.SaldoLoc.Value != 0 && x.SaldoExt.Value != 0).ToList();
                    if (saldosCU.Count == 0)
                        saldosCU = this._activoSaldos.Where(x => x.IdentificadorTR.Value == listActivo[i].ActivoID.Value && x.SaldoLoc.Value != null || x.SaldoLoc.Value != null || x.SaldoExt.Value != null).ToList();

                    foreach (var item in saldosCU)
                    {
                        if (item.SaldoLoc.Value == null)
                            item.SaldoLoc.Value = 0;

                        if (item.SaldoExt.Value == null)
                            item.SaldoExt.Value = 0;
                    }

                    listAppr.AddRange(saldosCU);
                }

                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCont(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);
                List<DTO_TxResult> resultsNOK = new List<DTO_TxResult>();
                if (this.validMove)
                {
                    List<DTO_TxResult> results = _bc.AdministrationModel.acActivoControl_UpdateSaldos(this.documentID, this._actFlujo.ID.Value, this.prefijoID, this.MasterMvtoTipo.Value, listAppr, listActivo);
                    foreach (DTO_TxResult result in results)
                    {
                        if ((result.Details != null && result.Details.Count > 0) || !string.IsNullOrWhiteSpace(result.ResultMessage))
                            resultsNOK.Add(result);
                    }
                }

                FormProvider.Master.StopProgressBarThread(this.documentID);
                MessageForm frm = new MessageForm(resultsNOK);
                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });

                if (resultsNOK.Count == 0)
                {
                    _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);

                    this.newDoc = true;
                    this.deleteOP = true;
                    this._activo = new List<DTO_acActivoControl>();
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
    }
}