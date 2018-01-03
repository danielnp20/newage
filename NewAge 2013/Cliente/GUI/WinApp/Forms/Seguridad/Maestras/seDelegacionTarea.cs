using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using NewAge.Cliente.GUI.WinApp.Clases;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Base;
using System.Reflection;
using DevExpress.XtraGrid.Views.Grid;
using NewAge.DTO.UDT;
using DevExpress.XtraEditors.Repository;
using System.Threading;
using NewAge.DTO.Resultados;
using SentenceTransformer;
using NewAge.DTO.Attributes;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario para aprobacion de documentos
    /// </summary>
    public partial class seDelegacionTarea : FormWithToolbar, IFiltrable
    {
        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        private int documentID = AppMasters.seDelegacionTareas;
        private ModulesPrefix frmModule = ModulesPrefix.se;
        private string unboundPrefix = "Unbound_";
        private FormTypes _frmType = FormTypes.DocumentAprob;

        //Variables de datos
        private List<DTO_seUsuario> _users = null;
        private List<DTO_seDelegacionHistoria> _details = null;
        private DTO_seUsuario currentUser = null;
        private DTO_seDelegacionHistoria currentDetail = null; 
        private DTO_glConsulta consulta = null;
        private List<DTO_glConsultaFiltro> filtrosConsulta = null;

        //Variables de indices
        private int numDetails = 0;
        private int currentRow = -1;

        //Variables Privadas
        private string _frmName;
        private bool _canCreate;

        #endregion

        public seDelegacionTarea()
        {
            try
            {
                this.InitializeComponent();

                this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());
                FormProvider.Master.Form_Load(this, this.frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);

                this.numDetails = Convert.ToInt32(_bc.GetControlValue(AppControl.PaginadorAprobacionDocumentos));

                //Asigna la lista de columnas
                this.AddUsersCols();
                this.AddDetailCols();

                //Inicia el control de paginacion
                _bc.InitMasterUC(this.master_UserDelegado, AppMasters.seUsuario, false, true, false, false);
                _bc.Pagging_Init(this.pgGrid, 10);
                _bc.Pagging_SetEvent(this.pgGrid, this.pagging_Click);

                this._canCreate = SecurityManager.HasAccess(this.documentID, FormsActions.Add);
                this.LoadUsers(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-seDelegacionTarea.cs", "seDelegacionTarea"));
            }
        }

        #region  Funciones Privadas

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddUsersCols()
        {
            //UsuarioID
            GridColumn user = new GridColumn();
            user.FieldName = this.unboundPrefix + "ID";
            user.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ID");
            user.UnboundType = UnboundColumnType.String;
            user.VisibleIndex = 0;
            user.Width = 60;
            user.OptionsColumn.AllowEdit = false;
            this.gvDocuments.Columns.Add(user);

            //Descriptivo
            GridColumn descr = new GridColumn();
            descr.FieldName = this.unboundPrefix + "Descriptivo";
            descr.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descriptivo");
            descr.UnboundType = UnboundColumnType.String;
            descr.VisibleIndex = 1;
            descr.Width = 100;
            descr.OptionsColumn.AllowEdit = false;
            this.gvDocuments.Columns.Add(descr);

            //AreaFuncionalID
            GridColumn af = new GridColumn();
            af.FieldName = this.unboundPrefix + "AreaFuncionalID";
            af.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_AreaFuncionalID");
            af.UnboundType = UnboundColumnType.String;
            af.VisibleIndex = 2;
            af.Width = 50;
            af.OptionsColumn.AllowEdit = false;
            this.gvDocuments.Columns.Add(af);

            //CorreoElectronico
            GridColumn mail = new GridColumn();
            mail.FieldName = this.unboundPrefix + "CorreoElectronico";
            mail.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CorreoElectronico");
            mail.UnboundType = UnboundColumnType.String;
            mail.VisibleIndex = 3;
            mail.Width = 100;
            mail.OptionsColumn.AllowEdit = false;
            this.gvDocuments.Columns.Add(mail);

            //UsuarioDelegado
            GridColumn delegado = new GridColumn();
            delegado.FieldName = this.unboundPrefix + "UsuarioDelegado";
            delegado.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_UsuarioDelegado");
            delegado.UnboundType = UnboundColumnType.String;
            delegado.VisibleIndex = 4;
            delegado.Width = 60;
            delegado.OptionsColumn.AllowEdit = false;
            this.gvDocuments.Columns.Add(delegado);

            //FechaDelegaINI
            GridColumn fechaIni = new GridColumn();
            fechaIni.FieldName = this.unboundPrefix + "FechaDelegaINI";
            fechaIni.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaDelegaINI");
            fechaIni.UnboundType = UnboundColumnType.DateTime;
            fechaIni.VisibleIndex = 5;
            fechaIni.Width = 50;
            fechaIni.OptionsColumn.AllowEdit = false;
            this.gvDocuments.Columns.Add(fechaIni);

            //FechaDelegaFIN
            GridColumn fechaFin = new GridColumn();
            fechaFin.FieldName = this.unboundPrefix + "FechaDelegaFIN";
            fechaFin.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaDelegaFIN");
            fechaFin.UnboundType = UnboundColumnType.DateTime;
            fechaFin.VisibleIndex = 6;
            fechaFin.Width = 50;
            fechaFin.OptionsColumn.AllowEdit = false;
            this.gvDocuments.Columns.Add(fechaFin);

            //DelegacionActivaInd
            GridColumn delInd = new GridColumn();
            delInd.FieldName = this.unboundPrefix + "DelegacionActivaInd";
            delInd.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DelegacionActivaInd");
            delInd.UnboundType = UnboundColumnType.Boolean;
            delInd.VisibleIndex = 7;
            delInd.Width = 50;
            delInd.OptionsColumn.AllowEdit = false;
            this.gvDocuments.Columns.Add(delInd);
        }

        /// <summary>
        /// Asigna la lista de columnas del detalle
        /// </summary>
        private void AddDetailCols() 
        {
            //FechaInicialAsig
            GridColumn fechaIni = new GridColumn();
            fechaIni.FieldName = this.unboundPrefix + "FechaInicialAsig";
            fechaIni.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaInicialAsig");
            fechaIni.UnboundType = UnboundColumnType.DateTime;
            fechaIni.VisibleIndex = 0;
            fechaIni.Width = 50;
            fechaIni.OptionsColumn.AllowEdit = false;
            this.gvDetails.Columns.Add(fechaIni);

            //FechaFinalAsig
            GridColumn fechaFin = new GridColumn();
            fechaFin.FieldName = this.unboundPrefix + "FechaFinalAsig";
            fechaFin.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaFinalAsig");
            fechaFin.UnboundType = UnboundColumnType.DateTime;
            fechaFin.VisibleIndex = 1;
            fechaFin.Width = 50;
            fechaFin.OptionsColumn.AllowEdit = false;
            this.gvDetails.Columns.Add(fechaFin);

            //Usuario Remplazo
            GridColumn user = new GridColumn();
            user.FieldName = this.unboundPrefix + "UsuarioRemplazo";
            user.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_UsuarioRemplazo");
            user.UnboundType = UnboundColumnType.String;
            user.VisibleIndex = 2;
            user.Width = 60;
            user.OptionsColumn.AllowEdit = false;
            this.gvDetails.Columns.Add(user);

            //DelegacionActivaInd
            GridColumn delega = new GridColumn();
            delega.FieldName = this.unboundPrefix + "DelegacionActivaInd";
            delega.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DelegacionActivaInd");
            delega.UnboundType = UnboundColumnType.Boolean;
            delega.VisibleIndex = 3;
            delega.Width = 60;
            delega.OptionsColumn.AllowEdit = false;
            this.gvDetails.Columns.Add(delega);
        }

        /// <summary>
        /// Carga la información de las grilla de documentos
        /// </summary>
        /// <param name="firstTime">Indica si es al primera vez que carga</param>
        private void LoadUsers(bool firstTime)
        {
            try
            {
                if(firstTime)
                    this.pgGrid.PageNumber = 1;
                else
                    this.gvDocuments.MoveFirst();

                this.currentUser = null;

                long count = _bc.AdministrationModel.MasterSimple_Count(AppMasters.seUsuario, this.consulta, this.filtrosConsulta, true);
                this.pgGrid.UpdatePageNumber(count, (this.pgGrid.PageNumber == 1), (this.pgGrid.PageNumber == 1), false);
                IEnumerable<DTO_MasterBasic> basic = _bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.seUsuario, this.pgGrid.PageSize, this.pgGrid.PageNumber, this.consulta, this.filtrosConsulta, true);
                this._users = basic.Cast<DTO_seUsuario>().ToList();

                this.currentRow = -1;
                this.gcDocuments.DataSource = null;

                if (this._users.Count > 0)
                {
                    this.currentRow = 0;
                    this.gcDocuments.DataSource = this._users;

                    this.currentUser = (DTO_seUsuario)this.gvDocuments.GetRow(this.currentRow);
                    this.LoadDetails();

                    this.gvDocuments.MoveFirst();
                }
                else
                {
                    this._details = null;
                    this.gcDetails.DataSource = null;
                    this.currentDetail = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-seDelegacionTarea.cs", "LoadUsers"));
            }
        }

        /// <summary>
        /// Carga la información de las grilla del detalle
        /// </summary>
        private void LoadDetails()
        {
            try
            {
                this.currentDetail = null;
                this._details = _bc.AdministrationModel.seDelegacionHistoria_Get(this.currentUser.ID.Value);
                this.gcDetails.DataSource = this._details;

                if (this._details.Count > 0)
                    this.LoadDetail(this._details[0]);
                else
                    this.EnableNewUser();

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-seDelegacionTarea.cs", "LoadDetails"));
            }
        }

        /// <summary>
        /// Funcion que se encarga de realizar la busqueda rapida
        /// </summary>
        private void ValidateSearchData()
        {
            if (string.IsNullOrWhiteSpace(this.txtCode.Text) && string.IsNullOrWhiteSpace(this.txtDescrip.Text))
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoSearchCriteria));
                return;
            }

            this.filtrosConsulta = new List<DTO_glConsultaFiltro>();
            //Agrega el codigo al filtro
            if (!string.IsNullOrWhiteSpace(this.txtCode.Text))
            {
                DTO_glConsultaFiltro codFilter = new DTO_glConsultaFiltro()
                {
                    CampoFisico = "UsuarioID",
                    ValorFiltro = this.txtCode.Text.Trim(),
                    OperadorFiltro = OperadorFiltro.Comienza,
                    OperadorSentencia = "AND"
                };
                this.filtrosConsulta.Add(codFilter);
            }

            //Agrega la descripcion al filtro
            if (!string.IsNullOrWhiteSpace(this.txtDescrip.Text))
            {
                DTO_glConsultaFiltro descFilter = new DTO_glConsultaFiltro()
                {
                    CampoFisico = "Descriptivo",
                    ValorFiltro = this.txtDescrip.Text.Trim(),
                    OperadorFiltro = OperadorFiltro.Comienza,
                    OperadorSentencia = "AND"
                };
                this.filtrosConsulta.Add(descFilter);
            }

            this.LoadUsers(true);
        }

        /// <summary>
        /// Habilita o deshabilita los controles para un nuevo usuario
        /// </summary>
        /// <param name="enabled"></param>
        private void EnableNewUser()
        {
            this.master_UserDelegado.EnableControl(true);
            this.dtFechaIni.Enabled = true;
            this.dtFechaFin.Enabled = true;

            //Valores por defecto
            this.master_UserDelegado.Value = string.Empty;
            this.dtFechaIni.Properties.MinValue = DateTime.Now.Date;
            this.dtFechaIni.DateTime = DateTime.Now.Date;
            this.dtFechaFin.Properties.MinValue = DateTime.Now.Date;
            this.dtFechaFin.DateTime = DateTime.Now.Date;

            FormProvider.Master.itemNew.Enabled = false;
            FormProvider.Master.itemDelete.Enabled = false;
            FormProvider.Master.itemSave.Enabled = true;
        }

        /// <summary>
        /// Carga la informacion de un historico
        /// </summary>
        /// <param name="detail">Detalle (delegado)</param>
        private void LoadDetail(DTO_seDelegacionHistoria detail)
        {
            try
            {
                this.currentDetail = detail;
                FormProvider.Master.itemDelete.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);

                this.master_UserDelegado.Value = this.currentDetail.UsuarioRemplazo.Value;
                this.dtFechaIni.DateTime = this.currentDetail.FechaInicialAsig.Value.Value;
                this.dtFechaFin.DateTime = this.currentDetail.FechaFinalAsig.Value.Value;

                //Verifica si se puede modificar con la fecha final
                if (this.currentDetail.FechaFinalAsig.Value.Value > DateTime.Now.Date)
                {
                    FormProvider.Master.itemSave.Enabled = true;
                    this.master_UserDelegado.EnableControl(true);
                    this.dtFechaIni.Enabled = false;
                    this.dtFechaFin.Enabled = true;
                }
                else
                {
                    FormProvider.Master.itemSave.Enabled = false;
                    this.master_UserDelegado.EnableControl(false);
                    this.dtFechaIni.Enabled = false;
                    this.dtFechaFin.Enabled = false;
                }

                FormProvider.Master.itemNew.Enabled = this._canCreate;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-seDelegacionTarea.cs", "LoadDetail"));
            }
        }

        #endregion

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
                FormProvider.Master.Form_Enter(this, this.documentID, this._frmType, this.frmModule);

                FormProvider.Master.tbBreak.Visible = true;
                FormProvider.Master.tbBreak0.Visible = true;

                FormProvider.Master.itemSearch.Visible = true;
                FormProvider.Master.itemFilter.Visible = true;
                FormProvider.Master.itemFilterDef.Visible = true;
                FormProvider.Master.itemNew.Visible = true;
                FormProvider.Master.itemDelete.Visible = true;

                FormProvider.Master.itemSearch.Enabled = true;
                FormProvider.Master.itemFilter.Enabled = true;
                FormProvider.Master.itemFilterDef.Enabled = true;

                if (FormProvider.Master.LoadFormTB)
                {
                    FormProvider.Master.itemNew.Enabled = this._canCreate;
                    if (this._details == null)
                    {
                        FormProvider.Master.itemSave.Enabled = false;
                        FormProvider.Master.itemDelete.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-seDelegacionTarea.cs", "Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-seDelegacionTarea.cs", "Form_Leave"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-seDelegacionTarea.cs", "Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-seDelegacionTarea.cs", "Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos Formulario

        /// <summary>
        /// Evento que se ejecuta al darle click al paginador
        /// </summary>
        /// <param name="sender">Objeto que inicia el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void pagging_Click(object sender, System.EventArgs e)
        {
            this.LoadUsers(false);
        }

        /// <summary>
        /// Busca data en la grilla segun la información filtrada en las cajas de texto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.ValidateSearchData();
        }

        /// <summary>
        /// Busca data en la grilla segun la información filtrada en las cajas de texto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtSearch_Click(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                this.ValidateSearchData();
        }

        /// <summary>
        /// Evento para actualizar el label cuando la fecha cambia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtFechaIni_EditValueChanged(object sender, EventArgs e)
        {
            DateTime dt = this.dtFechaIni.DateTime;
            if (dt.Ticks < new DateTime(1753, 1, 1).Ticks || dt.Ticks > new DateTime(9999, 12, 1).Ticks)
            {
                dt = new DateTime(1753, 1, 1);
                this.dtFechaIni.DateTime = dt;
            }

            this.dtFechaFin.Properties.MinValue = this.dtFechaIni.DateTime;
            this.dtFechaFin.DateTime = this.dtFechaIni.DateTime;
        }

        #endregion

        #region Eventos grilla de Documentos y Detalles

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDocuments_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
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
        /// Se ejecuta cuando se cambia la fila de los documentos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDocuments_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (this.currentRow != -1)
                {
                    if (e.FocusedRowHandle <= this.gvDocuments.RowCount - 1)
                        this.currentRow = e.FocusedRowHandle;

                    this.currentUser = (DTO_seUsuario)this.gvDocuments.GetRow(this.currentRow);
                    this.LoadDetails();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-seDelegacionTarea.cs", "gvDocuments_FocusedRowChanged"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando se cambia la fila de los documentos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDetails_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (this._details.Count > 0 && e.FocusedRowHandle != -1)
                    this.LoadDetail((DTO_seDelegacionHistoria)this.gvDetails.GetRow(e.FocusedRowHandle));
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-seDelegacionTarea.cs", "gvDetails_FocusedRowChanged"));
            }
        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para crear nuevo registro
        /// </summary>
        public override void TBNew()
        {
            this.currentDetail = null;
            this.EnableNewUser();
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            this.gvDocuments.PostEditor();
            try
            {
                if (this.currentUser != null)
                {
                    //Valida el usuario
                    if (!this.master_UserDelegado.ValidID || this.master_UserDelegado.Value == this.currentUser.ID.Value)
                    {
                        string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidUser);
                        MessageBox.Show(string.Format(msg, this.master_UserDelegado.Value));
                        return;
                    }

                    //Valida que no tenga la fecha inicial asignada 
                    List<DTO_seDelegacionHistoria> temp = this._details.Where
                    ( x =>
                        (this.dtFechaIni.DateTime.Date >= x.FechaInicialAsig.Value.Value.Date && this.dtFechaIni.DateTime.Date <= x.FechaFinalAsig.Value.Value.Date) ||
                        (this.dtFechaFin.DateTime.Date >= x.FechaInicialAsig.Value.Value.Date && this.dtFechaFin.DateTime.Date <= x.FechaFinalAsig.Value.Value.Date)
                    ).ToList();

                    if (temp.Count > 0 && currentDetail == null)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PkInUse));
                        return;
                    }

                    string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_AddDelegado);
                    string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.AddDelegado);

                    //Revisa si desea cargar los temporales
                    if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        DTO_seDelegacionHistoria delegado = new DTO_seDelegacionHistoria();
                        delegado.UsuarioID.Value = this.currentUser.ID.Value;
                        delegado.FechaInicialAsig.Value = this.dtFechaIni.DateTime;
                        delegado.FechaFinalAsig.Value = this.dtFechaFin.DateTime;
                        delegado.UsuarioRemplazo.Value = this.master_UserDelegado.Value;
                        delegado.DelegacionActivaInd.Value = false;

                        bool res = _bc.AdministrationModel.seDelegacionHistoria_Add(this.documentID, delegado);
                        if (res)
                        {
                            this.LoadDetails();

                            this.pnSearch.Visible = false;
                            this.txtCode.Text = string.Empty;
                            this.txtDescrip.Text = string.Empty;
                            this.filtrosConsulta = null;
                            this.consulta = null;

                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ResultOK));
                        }
                        else
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_AddData));

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-seDelegacionTarea.cs", "TBSave"));
            }
        }

        /// <summary>
        /// Boton para  desactivar un delegadoun nuevo documento
        /// </summary>
        public override void TBDelete()
        {
            try
            {
                if (this.currentUser != null)
                {
                    string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_DesableRegister);
                    string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.DisableRegister);

                    //Revisa si desea cargar los temporales
                    if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        bool res = _bc.AdministrationModel.seDelegacionHistoria_UpdateStatus(this.documentID, this.currentUser.ID.Value, this.dtFechaFin.DateTime, false);
                        if (res)
                        {
                            this.LoadDetails();

                            this.pnSearch.Visible = false;
                            this.txtCode.Text = string.Empty;
                            this.txtDescrip.Text = string.Empty;
                            this.filtrosConsulta = null;
                            this.consulta = null;

                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ResultOK));
                        }
                        else
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_UpdateData));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-seDelegacionTarea.cs", "TBDelete"));
            }
        }

        /// <summary>
        /// Boton para busquedas
        /// </summary>
        public override void TBSearch()
        {
            if (this.pnSearch.Visible)
            {
                #region Oculta el control de busqueda
                this.pnSearch.Visible = false;
                if (this.filtrosConsulta != null)
                {
                    this.filtrosConsulta = null;
                    this.txtCode.Text = string.Empty;
                    this.txtDescrip.Text = string.Empty;

                    this.LoadUsers(true);
                }
                #endregion
            }
            else
            {
                //Muestra el control de busqueda
                this.pnSearch.Visible = true;
                this.txtCode.Focus();
            }
        }

        /// <summary>
        /// Boton para asignar un filtro de resultados por defecto
        /// </summary>
        public override void TBFilterDef()
        {
            this.consulta = null;

            this.LoadUsers(true);
            this.gvDocuments.ClearSorting();
        }

        /// <summary>
        /// Boton para filtrar la lista de resultados
        /// </summary>
        public override void TBFilter()
        {
            try
            {                
                #region Define campos de filtro
                List<ConsultasFields> consultaFields = new List<ConsultasFields>();

                //UsuarioID
                ConsultasFields fl = new ConsultasFields();
                fl.Field = "ID";
                fl.FieldShown = _bc.GetResource(LanguageTypes.Forms, AppMasters.seUsuario.ToString() + "_ID");
                PropertyInfo pi = typeof(UDT_UsuarioID).GetProperty("Value");
                fl.Tipo = pi.PropertyType;
                fl.A_Seleccion = false;
                consultaFields.Add(fl);

                //AreaFuncionalID
                ConsultasFields fl1 = new ConsultasFields();
                fl1.Field = "AreaFuncionalID";
                fl1.FieldShown = _bc.GetResource(LanguageTypes.Forms, AppMasters.seUsuario.ToString() + "_AreaFuncionalID");
                PropertyInfo pi1 = typeof(UDT_AreaFuncionalID).GetProperty("Value");
                fl1.Tipo = pi1.PropertyType;
                fl1.A_Seleccion = false;
                consultaFields.Add(fl1);

                //UsuarioDelegado
                ConsultasFields fl2 = new ConsultasFields();
                fl2.Field = "UsuarioDelegado";
                fl2.FieldShown = _bc.GetResource(LanguageTypes.Forms, AppMasters.seUsuario.ToString() + "_UsuarioDelegado");
                PropertyInfo pi2 = typeof(UDT_UsuarioID).GetProperty("Value");
                fl2.Tipo = pi2.PropertyType;
                fl2.A_Seleccion = false;
                consultaFields.Add(fl2);

                //DelegacionActivaInd
                ConsultasFields fl3 = new ConsultasFields();
                fl3.Field = "DelegacionActivaInd";
                fl3.FieldShown = _bc.GetResource(LanguageTypes.Forms, AppMasters.seUsuario.ToString() + "_DelegacionActivaInd");
                PropertyInfo pi3 = typeof(UDT_SiNo).GetProperty("Value");
                fl3.Tipo = pi3.PropertyType;
                fl3.A_Seleccion = false;
                consultaFields.Add(fl3);

                #endregion
                MasterQuery mq = new MasterQuery(this, this.documentID, (int)this._bc.AdministrationModel.User.ReplicaID.Value, false, consultaFields);
                #region definir Fks
                mq.SetFK("AreaFuncionalID", AppMasters.glAreaFuncional, _bc.CreateFKConfig(AppMasters.glAreaFuncional));
                mq.SetFK("UsuarioDelegado", AppMasters.seUsuario, _bc.CreateFKConfig(AppMasters.seUsuario));
                #endregion
                mq.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-seDelegacionTarea.cs", "TBFilter"));
            }
        }

        /// <summary>
        /// Boton para actualizar la lista de documentos
        /// </summary>
        public override void TBUpdate()
        {
            try
            {
                this.LoadUsers(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-seDelegacionTarea.cs", "TBUpdate"));
            }
        }

        #endregion

        #region Filtrado de la grilla

        /// <summary>
        /// Asigna una consulta desde MasterQuery para hacer el filtrado de datos
        /// </summary>
        /// <param name="consulta"></param>
        /// <param name="fields"></param>
        public virtual void SetConsulta(DTO_glConsulta consulta, List<ConsultasFields> fields)
        {
            try
            {
                this.consulta = consulta;
                string filtros = Transformer.FiltrosGrilla(consulta.Filtros, fields, typeof(DTO_ComprobanteFooter));

                this.gvDocuments.ActiveFilterString = filtros;
                if (this.gvDocuments.RowCount > 0)
                    this.gvDocuments.MoveFirst();

                this.pnSearch.Visible = false;
                this.txtCode.Text = string.Empty;
                this.txtDescrip.Text = string.Empty;
                this.filtrosConsulta = null;
                this.LoadUsers(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-seDelegacionTarea.cs", "SetConsulta"));
            }
        }

        #endregion
    }
}
