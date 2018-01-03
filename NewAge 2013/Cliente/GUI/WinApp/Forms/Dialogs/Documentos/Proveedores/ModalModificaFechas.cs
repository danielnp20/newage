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
    public partial class ModalModificaFechas : Form
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
        private bool modRegistro = false;
        private bool indAprueba = false;
        //Variables para importar
        private string unboundPrefix = "Unbound_";
        //Variables de datos
        private DTO_prComprasModificaFechas _rowExistente = null;
        private List<DTO_prComprasModificaFechas> _listMoficaciones = new List<DTO_prComprasModificaFechas>();
        private List<DTO_pyProyectoMvto> _listMvtos = new List<DTO_pyProyectoMvto>();
        private bool _aprobadoDocInd = true;
        private int numDocCompra = 0;

        #endregion        

        ///<summary>
        /// Constructor 
        /// </summary>
        public ModalModificaFechas()
        {
            try
            {
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());

                //FormProvider.Master.Form_Load(this, this.frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalModificaFechas.cs", "ModalModificaFechas"));
            }
        }

        ///<summary>
        /// Constructor 
        /// </summary>
        public ModalModificaFechas(DTO_QueryComiteTecnico comite, DTO_QueryComiteCompras compras)
        {
            try
            {
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());
                FormProvider.LoadResources(this, this.documentID);

                if (comite != null)
                {
                    this.txtNroVersion.EditValue = comite.Version.Value;
                    this.txtNroDoc.EditValue = comite.NumeroDoc.Value;
                    this.masterProyecto.Value = comite.ProyectoID.Value;
                }
                if (compras != null)
                {
                    this.dtFechaEntrega.DateTime = compras.FechaCreacion.Value.Value;
                    this.dtFechaEntrega.Enabled = false;
                    this.dtFechaNueva.DateTime = System.DateTime.Now;
                    this.txtPrefDoc.Text = compras.PrefDocCompra;
                    this.numDocCompra = compras.NumDocCompra.Value.Value;
                    this._aprobadoDocInd = compras.Estado.Value == (byte)EstadoDocControl.Aprobado ? true : false;
                    this.LoadData(compras.NumDocCompra.Value.Value);

                    if (compras.DocIDCompra.Value == AppDocuments.Solicitud)
                    {
                        this.btnAprobar.Enabled = SecurityManager.HasAccess(AppQueries.QueryComiteSolPermiso, FormsActions.Get);
                        this.lblNombreDoc.Text = "Solicitud - Aprobada";
                    }
                    else if (compras.DocIDCompra.Value == AppDocuments.OrdenCompra)
                    {
                        if(compras.Estado.Value == (byte)EstadoDocControl.Aprobado)
                        {
                            this.btnAprobar.Enabled = SecurityManager.HasAccess(AppQueries.QueryComiteOcPermiso, FormsActions.Get);
                            this.lblNombreDoc.Text = "Orden Compra - Aprobada";
                        }
                        else
                        {
                            this.btnAprobar.Enabled = SecurityManager.HasAccess(AppQueries.QueryComiteOcNoAprPermiso, FormsActions.Get);
                            this.lblNombreDoc.Text = "Orden Compra " + (compras.Estado.Value == (byte)EstadoDocControl.SinAprobar ? "- Sin Aprobar" : "- Para Aprobación");
                        }
                    }
                    else if (compras.DocIDCompra.Value == AppDocuments.Recibido)
                    {
                        this.btnAprobar.Enabled = SecurityManager.HasAccess(AppQueries.QueryComiteRecPermiso, FormsActions.Get);
                       this.lblNombreDoc.Text = "Recibido " + (compras.Estado.Value == 3 ? "- Aprobada" : (compras.Estado.Value == 1 ? "- Sin Aprobar" : "- Para Aprobación")); ;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalModificaFechas.cs", "ModalModificaFechas"));
            }
        }

        #region Funciones Privadas

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
        /// Add Columns in Grid
        /// </summary>
        private void AddGridCols()
        {
            #region Grilla 

            GridColumn IndAprob = new GridColumn();
            IndAprob.FieldName = this.unboundPrefix + "ApruebaInd";
            IndAprob.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_IndAprobacion");
            IndAprob.UnboundType = UnboundColumnType.Boolean;
            IndAprob.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            IndAprob.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            IndAprob.AppearanceCell.Options.UseTextOptions = true;
            IndAprob.AppearanceCell.Options.UseFont = true;
            IndAprob.VisibleIndex = 1;
            IndAprob.Width = 60;
            IndAprob.Visible = true;
            IndAprob.OptionsColumn.AllowEdit = false;
            this.gvModificaciones.Columns.Add(IndAprob);

            GridColumn TareaDesc = new GridColumn();
            TareaDesc.FieldName = this.unboundPrefix + "Observaciones";
            TareaDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Observacion");
            TareaDesc.UnboundType = UnboundColumnType.String;
            TareaDesc.VisibleIndex = 4;
            TareaDesc.Width = 190;
            TareaDesc.Visible = true;
            TareaDesc.OptionsColumn.AllowEdit = false;
            this.gvModificaciones.Columns.Add(TareaDesc);

            GridColumn FechaFin = new GridColumn();
            FechaFin.FieldName = this.unboundPrefix + "FechaEntrega";
            FechaFin.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaEntrega");
            FechaFin.UnboundType = UnboundColumnType.DateTime;
            FechaFin.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            FechaFin.AppearanceCell.Options.UseTextOptions = true;
            FechaFin.VisibleIndex = 5;
            FechaFin.Width = 70;
            FechaFin.Visible = true;
            FechaFin.OptionsColumn.AllowEdit = false;
            this.gvModificaciones.Columns.Add(FechaFin);

            GridColumn FechaTermina = new GridColumn();
            FechaTermina.FieldName = this.unboundPrefix + "FechaNueva";
            FechaTermina.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaNueva");
            FechaTermina.UnboundType = UnboundColumnType.DateTime;
            FechaTermina.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            FechaTermina.AppearanceCell.Options.UseTextOptions = true;
            FechaTermina.VisibleIndex = 6;
            FechaTermina.Width = 85;
            FechaTermina.Visible = true;
            FechaTermina.OptionsColumn.AllowEdit = false;
            this.gvModificaciones.Columns.Add(FechaTermina);

            GridColumn UsuarioDigita = new GridColumn();
            UsuarioDigita.FieldName = this.unboundPrefix + "UsuarioDigita";
            UsuarioDigita.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_UsuarioDigita");
            UsuarioDigita.UnboundType = UnboundColumnType.String;
            UsuarioDigita.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            UsuarioDigita.AppearanceCell.Options.UseTextOptions = true;
            UsuarioDigita.VisibleIndex = 7;
            UsuarioDigita.Width = 60;
            UsuarioDigita.Visible = true;
            UsuarioDigita.OptionsColumn.AllowEdit = false;
            this.gvModificaciones.Columns.Add(UsuarioDigita);

            GridColumn FechaDigita = new GridColumn();
            FechaDigita.FieldName = this.unboundPrefix + "FechaDigita";
            FechaDigita.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaDigita");
            FechaDigita.UnboundType = UnboundColumnType.DateTime;
            FechaDigita.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            FechaDigita.AppearanceCell.Options.UseTextOptions = true;
            FechaDigita.VisibleIndex = 8;
            FechaDigita.Width = 60;
            FechaDigita.Visible = true;
            FechaDigita.OptionsColumn.AllowEdit = false;
            this.gvModificaciones.Columns.Add(FechaDigita);


            GridColumn UsuarioAprueba = new GridColumn();
            UsuarioAprueba.FieldName = this.unboundPrefix + "UsuarioAprueba";
            UsuarioAprueba.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_UsuarioAprueba");
            UsuarioAprueba.UnboundType = UnboundColumnType.String;
            UsuarioAprueba.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            UsuarioAprueba.AppearanceCell.Options.UseTextOptions = true;
            UsuarioAprueba.VisibleIndex = 9;
            UsuarioAprueba.Width = 60;
            UsuarioAprueba.Visible = true;
            UsuarioAprueba.OptionsColumn.AllowEdit = false;
            this.gvModificaciones.Columns.Add(UsuarioAprueba);

            GridColumn FechaAprueba = new GridColumn();
            FechaAprueba.FieldName = this.unboundPrefix + "FechaAprueba";
            FechaAprueba.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaAprueba");
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
        /// Inicializar controles
        /// </summary>
        private void InitControls()
        {
            try
            {
                this._bc.InitMasterUC(this.masterJustifica, AppMasters.prJustificaModificacion, true, true, true, true);
                this._bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true, true, true, true);
                this.masterJustifica.EnableControl(true);
                this.masterProyecto.EnableControl(false);


                this.txtNroVersion.ReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalModificaFechas.cs", "InitControls"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalModificaFechas", "LoadDocumentInfo"));
            }
        }

        /// <summary>
        /// Carga la información
        /// </summary>
        /// <param name="firstTime"></param>
        private void LoadData(int numeroDoc)
        {
            try
            {
                List<DTO_prComprasModificaFechas> transacciones = this._bc.AdministrationModel.prComprasModificaFechas_GetByNumeroDoc(numeroDoc, this._aprobadoDocInd);
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
            this._aprobadoDocInd = true;
            this._listMoficaciones = new List<DTO_prComprasModificaFechas>();
            this.gcModificaciones.DataSource = this._listMoficaciones;
            this.gcModificaciones.RefreshDataSource();
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            InitializeComponent();
            this.documentID = AppQueries.QueryComiteCompras;
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
        /// Verifica si hay un documento Existente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNro_Leave(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNroVersion.Text))
            {
                //int docNro = Convert.ToInt32(this.txtNroVersion.Text);
                //DTO_glDocumentoControl docCtrl = this._bc.AdministrationModel.glDocumentoControl_GetInternalDoc(docNro);
                //if (docCtrl != null)
                //    this.LoadData(docNro);
            }
        }

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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalModificaFechas", "rdGroupVer_SelectedIndexChanged"));
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
                        result = this._bc.AdministrationModel.prComprasModificaFechas_Upd(this._rowExistente);
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
                            result = this._bc.AdministrationModel.prComprasModificaFechas_Upd(this._rowExistente);
                        }
                    }
                    else
                    {
                        DTO_prComprasModificaFechas Modifica = new DTO_prComprasModificaFechas();

                        Modifica.NumeroDoc.Value = this.numDocCompra;
                        Modifica.Codigo.Value = this.masterJustifica.Value;
                        Modifica.ProyectoID.Value = this.masterProyecto.Value;
                        Modifica.FechaEntrega.Value = Convert.ToDateTime(this.dtFechaEntrega.EditValue);
                        Modifica.FechaNueva.Value = Convert.ToDateTime(this.dtFechaNueva.EditValue);
                        Modifica.UsuarioDigita.Value = this._bc.AdministrationModel.User.ID.Value;
                        Modifica.FechaDigita.Value = System.DateTime.Now;
                        Modifica.ApruebaInd.Value = false;
                        Modifica.AprobadoDocInd.Value = this._aprobadoDocInd;
                        Modifica.Observaciones.Value = this.txtDescripcion.Text;
                        result = this._bc.AdministrationModel.prComprasModificaFechas_Add(Modifica);
                        this.LoadData(Convert.ToInt32(Modifica.NumeroDoc.Value));
                    }
                }           
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalModificaFechas.cs", "ValidarData"));
                result.Result = ResultValue.NOK;
                return  result;
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalModificaFechas.cs", "ModalModificaFechas"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalModificaFechas.cs", "ModalModificaFechas"));
            }
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
                    this._rowExistente = (DTO_prComprasModificaFechas)this.gvModificaciones.GetRow(e.FocusedRowHandle);
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

        #endregion


    }
}
