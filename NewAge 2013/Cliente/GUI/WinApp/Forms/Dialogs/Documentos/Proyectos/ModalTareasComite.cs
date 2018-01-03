using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Resultados;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using DevExpress.XtraGrid.Views.Grid;
using System.Threading;
using System.Globalization;
using NewAge.DTO.Attributes;
using System.Drawing;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de documentos
    /// </summary>
    public partial class ModalTareasComite : Form
    {

         #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        //Variables Privadas
        private FormTypes _frmType = FormTypes.Document;
        private string _frmName;
        private int userID = 0;
        //Para manejo de propiedades
        private string empresaID = string.Empty;
        private int documentID;
        private ModulesPrefix frmModule;
        // Variables Formulario
        private int _numeroDoc = 0;
        private bool modRegistro = false;
        private bool indAprueba = false;
        //Variables para importar
        private string unboundPrefix = "Unbound_";
        //Variables de datos
        private DTO_pyProyectoModificaFechas _rowExistente = null;
        private List<DTO_pyProyectoModificaFechas> _listMoficaciones = new List<DTO_pyProyectoModificaFechas>();
        
        ////Variables de datos
        //private DTO_pyProyectoDocu _proyectoDocu = new DTO_pyProyectoDocu();
        //private DTO_glDocumentoControl _ctrlProyecto = null;
        //private DTO_pyProyectoTarea _rowTarea = new DTO_pyProyectoTarea();
        //private DTO_pyProyectoDeta _rowDetalle = new DTO_pyProyectoDeta();
        //private List<DTO_pyProyectoTarea> _listTareasAll = new List<DTO_pyProyectoTarea>();
        //private List<DTO_pyProyectoDeta> _listRecursosXTareaAll = new List<DTO_pyProyectoDeta>();
        //private List<DTO_pyProyectoMvto> _listMvtos = new List<DTO_pyProyectoMvto>();
        #endregion        

        ///<summary>
        /// Constructor 
        /// </summary>
        public ModalTareasComite()
        {
            try
            {
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());

               // this.LoadDocumentInfo(true);
                this.frmModule = ModulesPrefix.py;

                //FormProvider.Master.Form_Load(this, this.frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalTareasComite.cs", "ModalTareasComite"));
            }
        }

        ///<summary>
        /// Constructor 
        /// </summary>
        public ModalTareasComite(DTO_QueryComiteTecnico comite, DTO_QueryComiteTecnicoTareas tarea)
        {
            try
            {
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());
                FormProvider.LoadResources(this, this.documentID);
                if (comite != null)
                {
                    this.masterProyecto.Value = comite.ProyectoID.Value;
                    this.txtNroVersion.EditValue = comite.Version.Value;
                    this.txtNroDoc.EditValue = comite.NumeroDoc.Value;
                    this._numeroDoc = comite.NumeroDoc.Value.Value;
                }
                if(tarea != null)
                {
                    this.masterTarea.Value = tarea.TareaID.Value;
                    this.dtSolFechaActual.EditValue = tarea.FechaSolicitud.Value;
                    this.dtTrabFechaActual.EditValue = tarea.FechaTrabajo.Value;
                    this.dtEntrFechaActual.EditValue = tarea.FechaFin.Value;
                    this.dtFechaNueva.EditValue = System.DateTime.Now;

                    this.txtNroDoc.Enabled = false;
                    this.masterJustifica.Enabled = true;
                    this.dtSolFechaActual.Enabled = false;
                    this.dtTrabFechaActual.Enabled = false;
                    this.dtEntrFechaActual.Enabled = false;
                    this.btnAprobar.Enabled = SecurityManager.HasAccess(AppQueries.QueryComiteTecPermiso, FormsActions.Get);
                    this.LoadData(comite.NumeroDoc.Value.Value,tarea.TareaID.Value);
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalTareasComite.cs", "ModalTareasComite"));
            }
        }

        #region Funciones Privadas

        /// <summary>
        /// Add Columns in Grid
        /// </summary>
        private void AddGridCols()
        {
            #region Grilla Tareas


            GridColumn Codigo = new GridColumn();
            Codigo.FieldName = this.unboundPrefix + "Codigo";
            Codigo.Caption = _bc.GetResource(LanguageTypes.Forms, "Justificación");
            Codigo.UnboundType = UnboundColumnType.String;
            Codigo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Codigo.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Codigo.AppearanceCell.Options.UseTextOptions = true;
            Codigo.AppearanceCell.Options.UseFont = true;
            Codigo.VisibleIndex = 1;
            Codigo.Width = 60;
            Codigo.Visible = true;
            Codigo.OptionsColumn.AllowEdit = false;
            this.gvModificaciones.Columns.Add(Codigo);

            //GridColumn DescMod = new GridColumn();
            //DescMod.FieldName = this.unboundPrefix + "DescMod";
            //DescMod.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DescMod");
            //DescMod.UnboundType = UnboundColumnType.String;
            //DescMod.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            //DescMod.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //DescMod.AppearanceCell.Options.UseTextOptions = true;
            //DescMod.AppearanceCell.Options.UseFont = true;
            //DescMod.VisibleIndex = 2;
            //DescMod.Width = 80;
            //DescMod.Visible = true;
            //DescMod.OptionsColumn.AllowEdit = false;
            //this.gvModificaciones.Columns.Add(DescMod);

            GridColumn Observaciones = new GridColumn();
            Observaciones.FieldName = this.unboundPrefix + "Observaciones";
            Observaciones.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryComiteCompras + "_Observacion");
            Observaciones.UnboundType = UnboundColumnType.String;
            Observaciones.VisibleIndex = 3;
            Observaciones.Width = 210;
            Observaciones.Visible = true;
            Observaciones.OptionsColumn.AllowEdit = false;
            this.gvModificaciones.Columns.Add(Observaciones);

            GridColumn FechaFin = new GridColumn();
            FechaFin.FieldName = this.unboundPrefix + "FechaActual";
            FechaFin.Caption = _bc.GetResource(LanguageTypes.Forms, "Fecha Actual");
            FechaFin.UnboundType = UnboundColumnType.DateTime;
            FechaFin.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            FechaFin.AppearanceCell.Options.UseTextOptions = true;
            FechaFin.VisibleIndex = 4;
            FechaFin.Width = 70;
            FechaFin.Visible = true;
            FechaFin.OptionsColumn.AllowEdit = false;
            this.gvModificaciones.Columns.Add(FechaFin);

            GridColumn FechaTermina = new GridColumn();
            FechaTermina.FieldName = this.unboundPrefix + "FechaNueva";
            FechaTermina.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryComiteCompras + "_FechaNueva");
            FechaTermina.UnboundType = UnboundColumnType.DateTime;
            FechaTermina.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            FechaTermina.AppearanceCell.Options.UseTextOptions = true;
            FechaTermina.VisibleIndex = 5;
            FechaTermina.Width = 70;
            FechaTermina.Visible = true;
            FechaTermina.OptionsColumn.AllowEdit = false;
            this.gvModificaciones.Columns.Add(FechaTermina);

            GridColumn UsuarioDigita = new GridColumn();
            UsuarioDigita.FieldName = this.unboundPrefix + "UsuarioDigita";
            UsuarioDigita.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID +"_UsuarioDigita");
            UsuarioDigita.UnboundType = UnboundColumnType.DateTime;
            UsuarioDigita.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            UsuarioDigita.AppearanceCell.Options.UseTextOptions = true;
            UsuarioDigita.VisibleIndex = 6;
            UsuarioDigita.Width = 60;
            UsuarioDigita.Visible = true;
            UsuarioDigita.OptionsColumn.AllowEdit = false;
            this.gvModificaciones.Columns.Add(UsuarioDigita);

            GridColumn FechaDigita = new GridColumn();
            FechaDigita.FieldName = this.unboundPrefix + "FechaDigita";
            FechaDigita.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID +"_FechaDigita");
            FechaDigita.UnboundType = UnboundColumnType.DateTime;
            FechaDigita.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            FechaDigita.AppearanceCell.Options.UseTextOptions = true;
            FechaDigita.VisibleIndex = 7;
            FechaDigita.Width = 60;
            FechaDigita.Visible = true;
            FechaDigita.OptionsColumn.AllowEdit = false;
            this.gvModificaciones.Columns.Add(FechaDigita);


            GridColumn UsuarioAprueba = new GridColumn();
            UsuarioAprueba.FieldName = this.unboundPrefix + "UsuarioAprueba";
            UsuarioAprueba.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID +"_UsuarioAprueba");
            UsuarioAprueba.UnboundType = UnboundColumnType.DateTime;
            UsuarioAprueba.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            UsuarioAprueba.AppearanceCell.Options.UseTextOptions = true;
            UsuarioAprueba.VisibleIndex = 8;
            UsuarioAprueba.Width = 60;
            UsuarioAprueba.Visible = true;
            UsuarioAprueba.OptionsColumn.AllowEdit = false;
            this.gvModificaciones.Columns.Add(UsuarioAprueba);

            GridColumn FechaAprueba = new GridColumn();
            FechaAprueba.FieldName = this.unboundPrefix + "FechaAprueba";
            FechaAprueba.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID +"_FechaAprueba");
            FechaAprueba.UnboundType = UnboundColumnType.DateTime;
            FechaAprueba.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            FechaAprueba.AppearanceCell.Options.UseTextOptions = true;
            FechaAprueba.VisibleIndex = 10;
            FechaAprueba.Width = 60;
            FechaAprueba.Visible = true;
            FechaAprueba.OptionsColumn.AllowEdit = false;
            this.gvModificaciones.Columns.Add(FechaAprueba);


            this.gvModificaciones.OptionsView.ColumnAutoWidth = true;

            #endregion

            
        }

        /// <summary>
        /// Verifiva que el header sea valido
        /// </summary>
        /// <returns></returns>
        private bool ValidateHeader()
        {
            #region Hace las Validaciones
            //Valida que haya seleccionado la justificacion
            if (String.IsNullOrEmpty(this.masterJustifica.Value))
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.masterJustifica.LabelRsx);
                MessageBox.Show(msg);
                this.masterJustifica.Focus();
                return false;
            }

            //Valida que este escrito la justificacion
            if (String.IsNullOrEmpty(this.txtDescripcion.Text))
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.txtDescripcion.Text);
                MessageBox.Show(msg);
                this.txtDescripcion.Focus();
                return false;
            }

            //Valida que este la nueva fecha    
            if (String.IsNullOrEmpty(this.dtFechaNueva.EditValue.ToString()))
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.dtFechaNueva.EditValue);
                MessageBox.Show(msg);
                this.dtFechaNueva.Focus();
                return false;
            }

            #endregion
            return true;
        }
        /// <summary>
        /// Inicializar controles
        /// </summary>
        private void InitControls()
        {
            try
            {
                this._bc.InitMasterUC(this.masterTarea, AppMasters.pyTarea, true, true, true, true);
                this._bc.InitMasterUC(this.masterJustifica, AppMasters.pyJustificaModificacion, true, true, true, true);
                this._bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true, true, true, true);
                this.masterProyecto.EnableControl(false);
                this.masterTarea.EnableControl(false);
                this.masterJustifica.EnableControl(true);
                this.txtNroVersion.ReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalTareasComite.cs", "InitControls"));
            }
        }

        /// <summary>
        /// Loads the document main info
        /// </summary>
        private void LoadDocumentInfo(bool firstTime)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalTareasComite", "LoadDocumentInfo"));
            }
        }



        /// <summary>
        /// Carga la información
        /// </summary>
        /// <param name="firstTime"></param>
        private void LoadData(int numeroDoc,string tarea)
        {
            try
            {
                List<DTO_pyProyectoModificaFechas> transacciones = this._bc.AdministrationModel.pyProyectoModificaFechas_GetByNumeroDoc(numeroDoc,tarea);
                //DTO_prComprasModificaFechas transaccion = this._bc.AdministrationModel.prComprasModificaFechas_Load(numeroDoc);

                this.modRegistro = false;
                if (transacciones.Count > 0)
                {
                    this._listMoficaciones = transacciones;
                    this.LoadGrids();
                    if (transacciones.Any(x => x.ApruebaInd.Value == false))
                    {
                        this.modRegistro = true;
                        this._rowExistente = transacciones.Find(x => x.ApruebaInd.Value == false);
                        this.txtDescripcion.Text = this._rowExistente.Observaciones.Value;
                        this.dtFechaNueva.DateTime = this._rowExistente.FechaNueva.Value.Value;
                        this.masterJustifica.Value = this._rowExistente.Codigo.Value;
                        this.radioGroup1.SelectedIndex = Convert.ToByte(this._rowExistente.TipoAjuste.Value) - 1;



                    }
                    else
                    {
                        this.dtFechaNueva.DateTime = System.DateTime.Now;
                        this.txtDescripcion.Text = string.Empty;
                        this.masterJustifica.Value = string.Empty;
                        this.modRegistro = false;
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalModificaFechas", "LoadData"));
            }
        }

        /// <summary>
        /// Carga la información del detalle
        /// </summary>
        /// <param name="firstTime"></param>
        private void LoadGrids()
        {
            try
            {
                this.gcModificaciones.DataSource = this._listMoficaciones;
                this.gcModificaciones.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalModificaFechas", "LoadGrids"));
            }
        }
        /// <summary>
        /// Refrescar Formulario
        /// </summary>
        private void RefreshForm()
        {

            this.txtNroVersion.Text = string.Empty;
            this.masterJustifica.Value = string.Empty;
            this.masterProyecto.Value = string.Empty;
            this.txtDescripcion.Text = string.Empty;
            this._listMoficaciones = new List<DTO_pyProyectoModificaFechas>();
            this.gcModificaciones.DataSource = this._listMoficaciones;
            this.gcModificaciones.RefreshDataSource();
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            InitializeComponent();
            this.frmModule = ModulesPrefix.py;
            this.documentID = AppQueries.QueryComiteTecnico;
            this.AddGridCols();
            this.InitControls();

            this.empresaID = _bc.AdministrationModel.Empresa.ID.Value;
            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;
            this.txtDescripcion.ReadOnly = false;
            this.txtDescripcion.Enabled = true;
        
        }
        /// <summary>
        /// Valida una fila
        /// </summary>
        /// <param name="fila">Indice de la fila a validar</param>
        /// <returns>Retorna si la info de la fila es valida o no</returns>
        private bool ValidateRow(int fila)
        {
            return true;
        }

        #endregion

        #region Eventos Header Superior

        /// <summary>
        /// Evento que se ejecuta al salir del numero de documento (glDocumentoControl - NumeroDoc)
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtNumeroDoc_Leave(object sender, EventArgs e) { }

        /// <summary>
        /// Evento que se ejecuta al pararse sobre el control de fecha
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void dtFecha_Enter(object sender, EventArgs e) { }
       
        /// <summary>
        /// Valida que solo ingrese numeros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNumPrefix_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        #endregion

        #region Eventos Header


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdGroupVer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                 MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalTareasComite", "rdGroupVer_SelectedIndexChanged"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Verifica 
        /// </summary>
        /// <returns>DTO_TxResult</returns>
        private DTO_TxResult ValidarData()
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;

            result.Details = new List<DTO_TxResultDetail>();
            DTO_TxResultDetail rd = new DTO_TxResultDetail();
            DTO_TxResultDetailFields drF = new DTO_TxResultDetailFields();
            try
            {
                if (indAprueba)
                {

                    if (this._rowExistente != null)
                    {
                        this._rowExistente.ApruebaInd.Value = true;
                        this._rowExistente.UsuarioAprueba.Value = this._bc.AdministrationModel.User.ID.Value;
                        this._rowExistente.FechaAprueba.Value = System.DateTime.Now;
                        result = this._bc.AdministrationModel.pyProyectoModificaFechas_Upd(this._rowExistente);
                    }
                }
                else
                {

                    if (this.modRegistro)
                    {
                        if (this._rowExistente != null)
                        {
                            this._rowExistente.Codigo.Value = this.masterJustifica.Value;
                            this._rowExistente.FechaNueva.Value = Convert.ToDateTime(this.dtFechaNueva.EditValue);
                            this._rowExistente.UsuarioDigita.Value = this._bc.AdministrationModel.User.ID.Value;
                            this._rowExistente.FechaDigita.Value = System.DateTime.Now;
                            this._rowExistente.Observaciones.Value = this.txtDescripcion.Text;
                            this._rowExistente.TipoAjuste.Value = Convert.ToByte(this.radioGroup1.SelectedIndex + 1);
                            if (Convert.ToByte(this.radioGroup1.SelectedIndex) == 0)
                                this._rowExistente.FechaActual.Value = Convert.ToDateTime(this.dtSolFechaActual.EditValue);
                            if (Convert.ToByte(this.radioGroup1.SelectedIndex) == 1)
                                this._rowExistente.FechaActual.Value = Convert.ToDateTime(this.dtTrabFechaActual.EditValue);
                            if (Convert.ToByte(this.radioGroup1.SelectedIndex) == 2)
                                this._rowExistente.FechaActual.Value = Convert.ToDateTime(this.dtEntrFechaActual.EditValue);

                            result = this._bc.AdministrationModel.pyProyectoModificaFechas_Upd(this._rowExistente);
                        }
                    }
                    else
                    {
                        DTO_pyProyectoModificaFechas Modifica = new DTO_pyProyectoModificaFechas();

                        Modifica.NumeroDoc.Value = this._numeroDoc;
                        Modifica.Codigo.Value = this.masterJustifica.Value;
                        Modifica.ProyectoID.Value = this.masterProyecto.Value;
                        Modifica.TareaID.Value = this.masterTarea.Value;
                        Modifica.FechaNueva.Value = Convert.ToDateTime(this.dtFechaNueva.EditValue);
                        Modifica.UsuarioDigita.Value = this._bc.AdministrationModel.User.ID.Value;
                        Modifica.FechaDigita.Value = System.DateTime.Now;
                        Modifica.ApruebaInd.Value = false;
                        Modifica.TipoAjuste.Value = Convert.ToByte(this.radioGroup1.SelectedIndex+1);
                        if (Convert.ToByte(this.radioGroup1.SelectedIndex)== 0)
                            Modifica.FechaActual.Value = Convert.ToDateTime(this.dtSolFechaActual.EditValue);
                        if (Convert.ToByte(this.radioGroup1.SelectedIndex) == 1)
                            Modifica.FechaActual.Value = Convert.ToDateTime(this.dtTrabFechaActual.EditValue);
                        if (Convert.ToByte(this.radioGroup1.SelectedIndex) == 2)
                            Modifica.FechaActual.Value = Convert.ToDateTime(this.dtEntrFechaActual.EditValue);

                        
                        Modifica.Observaciones.Value = this.txtDescripcion.Text;
                        result = this._bc.AdministrationModel.pyProyectoModificaFechas_Add(Modifica);
                        this.LoadData(Convert.ToInt32(Modifica.NumeroDoc.Value),Convert.ToString(Modifica.TareaID.Value));
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalModificaFechas.cs", "ValidarData"));
                result.Result = ResultValue.NOK;
                return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Eventos Grilla

        #region Tareas

        /// <summary>
        /// Se ejecutar cuando se selecciona un registro de la Grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDocument_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle >= 0)
                {
                    this._rowExistente = (DTO_pyProyectoModificaFechas)this.gvModificaciones.GetRow(e.FocusedRowHandle);
                    this.memoEdit1.EditValue = this._rowExistente.Observaciones;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalModificaFechas.cs", "gvDocument_FocusedRowChanged"));
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
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (pi != null)
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" ||
                        pi.PropertyType.Name == "Double")
                        e.Value = pi.GetValue(dto, null);
                    else
                        e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                else
                {
                    FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (fi != null)
                    {
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" ||
                            pi.PropertyType.Name == "Double")
                            e.Value = fi.GetValue(dto);
                        else
                            e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                    }
                }
                if (e.Value == null && pi != null && pi.PropertyType.Name == "UDT_Cantidad")
                    e.Value = 0;
            }
            if (e.IsSetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (e.Value == null)
                    e.Value = string.Empty;
                if (pi != null)
                {
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" ||
                        pi.PropertyType.Name == "Double")
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
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" ||
                            pi.PropertyType.Name == "Double")
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


        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAprobar_Click(object sender, EventArgs e)
        {
            indAprueba = false;
            try
            {
                this.gvDetalle.PostEditor();
                indAprueba = true;
                DTO_TxResult res = this.ValidarData();
                MessageForm frm = new MessageForm(res);
                frm.ShowDialog();

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalTareasComite.cs", "ModalTareasComite"));
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            indAprueba = false;
            try
            {
                if (this.ValidateHeader())
                {
                    this.gvDetalle.PostEditor();
                    DTO_TxResult res = this.ValidarData();
                    MessageForm frm = new MessageForm(res);
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalTareasComite.cs", "ModalTareasComite"));
            }
        }


        #endregion

    }
}
