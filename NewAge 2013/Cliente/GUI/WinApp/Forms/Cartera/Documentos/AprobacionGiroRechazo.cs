using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.DTO.Resultados;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using System.ComponentModel;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using SentenceTransformer;
using System.Linq;
using System.Threading;
using System.Drawing;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Documento de prueba
    /// </summary>
    public partial class AprobacionGiroRechazo : DocumentAprobBasicForm
    {
        #region Delegados

        /// <summary>
        /// Delegado que finaliza el proceso de aprobacion
        /// </summary>
        public override void RefreshDataMethod()
        {
            this.CleanData();
            this.LoadDocuments();
        }

        #endregion

        #region Variables formulario

        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
     
        //Listas de datos
        private DTO_ccCreditoDocu crediDocu = new DTO_ccCreditoDocu();
        private List<DTO_ccCreditoDocu> creditos = new List<DTO_ccCreditoDocu>();
        private List<DTO_ccCreditoDocu> creditosToSend = new List<DTO_ccCreditoDocu>();
        private DTO_coTercero clienteTercero = new DTO_coTercero();

        private bool firstTime = true;
        #endregion

        public AprobacionGiroRechazo()
            : base()
        {
            this.InitializeComponent();
        }

        public AprobacionGiroRechazo(string mod)
            : base(mod)
        {
        }

        #region Funciones Virtuales del DocumentAprobBasicForm

        /// <summary>
        /// Carga la información de la grilla de documentos
        /// </summary>
        protected override void LoadDocuments()
        {
            try
            {
                if (!this.firstTime)
                {
                    this.creditos = this._bc.AdministrationModel.AprobarGiroRechazo_GetAll(this.actividadFlujoID);
                    this.PagoDirecto();
                }
                else
                    this.firstTime = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionGiroRechazo.cs", "LoadDocuments"));
            }
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.AprobacionGiros;
            this.frmModule = ModulesPrefix.cc;

            base.SetInitParameters();
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void AfterInitialize()
        {
            this.LoadDocuments();
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected override void AddDocumentCols()
        {
            try
            {
                //Aprobar
                GridColumn aprob = new GridColumn();
                aprob.FieldName = this.unboundPrefix + "Aprobado";
                aprob.Caption = "√";
                aprob.UnboundType = UnboundColumnType.Boolean;
                aprob.VisibleIndex = 0;
                aprob.Width = 15;
                aprob.Visible = true;
                aprob.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Aprobado");
                aprob.AppearanceHeader.ForeColor = Color.Lime;
                aprob.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                aprob.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                aprob.AppearanceHeader.Options.UseTextOptions = true;
                aprob.AppearanceHeader.Options.UseFont = true;
                aprob.AppearanceHeader.Options.UseForeColor = true;
                this.gvDocuments.Columns.Add(aprob);

                //Rechazar
                GridColumn noAprob = new GridColumn();
                noAprob.FieldName = this.unboundPrefix + "Rechazado";
                noAprob.Caption = "X";
                noAprob.UnboundType = UnboundColumnType.Boolean;
                noAprob.VisibleIndex = 1;
                noAprob.Width = 15;
                noAprob.Visible = false;
                noAprob.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Rechazado");
                noAprob.AppearanceHeader.ForeColor = Color.Red;
                noAprob.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                noAprob.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                noAprob.AppearanceHeader.Options.UseTextOptions = true;
                noAprob.AppearanceHeader.Options.UseFont = true;
                noAprob.AppearanceHeader.Options.UseForeColor = true;
                this.gvDocuments.Columns.Add(noAprob);

                //ClienteID
                GridColumn clienteID = new GridColumn();
                clienteID.FieldName = this.unboundPrefix + "ClienteID";
                clienteID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ClienteID");
                clienteID.UnboundType = UnboundColumnType.String;
                clienteID.VisibleIndex = 3;
                clienteID.Width = 25;
                clienteID.Visible = true;
                clienteID.AppearanceHeader.Font = new Font("Arial", 6.50F, FontStyle.Bold, GraphicsUnit.Pixel);
                clienteID.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(clienteID);

                //Nombre
                GridColumn nombre = new GridColumn();
                nombre.FieldName = this.unboundPrefix + "Nombre";
                nombre.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Nombre");
                nombre.UnboundType = UnboundColumnType.String;
                nombre.VisibleIndex = 4;
                nombre.Width = 60;
                nombre.Visible = true;
                nombre.AppearanceHeader.Font = new Font("Arial", 1.11F, FontStyle.Bold, GraphicsUnit.Pixel);
                nombre.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(nombre);

                //Libranza
                GridColumn libranza = new GridColumn();
                libranza.FieldName = this.unboundPrefix + "Libranza";
                libranza.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Libranza");
                libranza.UnboundType = UnboundColumnType.String;
                libranza.VisibleIndex = 4;
                libranza.Width = 30;
                libranza.Visible = true;
                libranza.AppearanceHeader.Font = new Font("Arial", 6.50F, FontStyle.Bold, GraphicsUnit.Pixel);
                libranza.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(libranza);

                //BancoID
                GridColumn bancoID = new GridColumn();
                bancoID.FieldName = this.unboundPrefix + "BancoID";
                bancoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_BancoID");
                bancoID.UnboundType = UnboundColumnType.String;
                bancoID.VisibleIndex = 5;
                bancoID.Width = 40;
                bancoID.Visible = true;
                bancoID.AppearanceHeader.Font = new Font("Arial", 6.50F, FontStyle.Bold, GraphicsUnit.Pixel);
                bancoID.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(bancoID);

                //DescBanco
                GridColumn descBanco = new GridColumn();
                descBanco.FieldName = this.unboundPrefix + "DescBanco";
                descBanco.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DescBanco");
                descBanco.UnboundType = UnboundColumnType.String;
                descBanco.VisibleIndex = 6;
                descBanco.Width = 30;
                descBanco.Visible = true;
                descBanco.AppearanceHeader.Font = new Font("Arial", 6.50F, FontStyle.Bold, GraphicsUnit.Pixel);
                descBanco.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(descBanco);

                //TipoCuenta
                GridColumn tipoCuenta = new GridColumn();
                tipoCuenta.FieldName = this.unboundPrefix + "TipoCuenta";
                tipoCuenta.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TipoCuenta");
                tipoCuenta.UnboundType = UnboundColumnType.String;
                tipoCuenta.VisibleIndex = 7;
                tipoCuenta.Width = 40;
                tipoCuenta.Visible = true;
                tipoCuenta.AppearanceHeader.Font = new Font("Arial", 6.50F, FontStyle.Bold, GraphicsUnit.Pixel);
                tipoCuenta.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(tipoCuenta);

                //Numero Cuenta
                GridColumn numCuenta = new GridColumn();
                numCuenta.FieldName = this.unboundPrefix + "NumCuenta";
                numCuenta.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_NumCuenta");
                numCuenta.UnboundType = UnboundColumnType.String;
                numCuenta.VisibleIndex = 8;
                numCuenta.Width = 40;
                numCuenta.Visible = true;
                numCuenta.AppearanceHeader.Font = new Font("Arial", 6.50F, FontStyle.Bold, GraphicsUnit.Pixel);
                numCuenta.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(numCuenta);

                //Valor Giro
                GridColumn vlrGiro = new GridColumn();
                vlrGiro.FieldName = this.unboundPrefix + "VlrGiro";
                vlrGiro.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrGiro");
                vlrGiro.UnboundType = UnboundColumnType.Decimal;
                vlrGiro.VisibleIndex = 9;
                vlrGiro.Width = 40;
                vlrGiro.Visible = true;
                vlrGiro.AppearanceHeader.Font = new Font("Arial", 6.50F, FontStyle.Bold, GraphicsUnit.Pixel);
                vlrGiro.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(vlrGiro);

                //Archivo
                GridColumn file = new GridColumn();
                file.FieldName = this.unboundPrefix + "FileUrl";
                file.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FileUrl");
                file.UnboundType = UnboundColumnType.String;
                file.Width = 30;
                file.VisibleIndex = 10;
                file.AppearanceHeader.Font = new Font("Arial", 6.50F, FontStyle.Bold, GraphicsUnit.Pixel);
                file.Visible = true;
                this.gvDocuments.Columns.Add(file);

                //Observacion
                GridColumn observacion = new GridColumn();
                observacion.FieldName = this.unboundPrefix + "Observacion";
                observacion.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Observacion");
                observacion.UnboundType = UnboundColumnType.String;
                observacion.Width = 80;
                observacion.VisibleIndex = 11;
                observacion.AppearanceHeader.Font = new Font("Arial", 6.50F, FontStyle.Bold, GraphicsUnit.Pixel);
                observacion.Visible = false;
                observacion.OptionsColumn.AllowEdit = true;
                this.gvDocuments.Columns.Add(observacion);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionGiroRechazo.cs", "AddDocumentCols"));
            }
        }

        #endregion

        #region Funciones privadas

        /// <summary>
        /// Funcion para limpiar el documento
        /// </summary>
        private void CleanData()
        {
            this.creditos = new List<DTO_ccCreditoDocu>();
            this.crediDocu = new DTO_ccCreditoDocu();

            this.gcDocuments.DataSource = null;
        }

        /// <summary>
        /// Funcion para establecer los giros para pagos directos
        /// </summary>
        private void PagoDirecto()
        {
            #region Carga la info de los bancos
            foreach (DTO_ccCreditoDocu item in this.creditos)
            {
                DTO_ccCliente cliente = (DTO_ccCliente)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCliente, false, item.ClienteID.Value, true);
                this.clienteTercero = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, cliente.TerceroID.Value, true);

                this.crediDocu = item;
                this.crediDocu.Nombre.Value = this.clienteTercero.ApellidoPri.Value + this.clienteTercero.ApellidoSdo.Value + this.clienteTercero.NombrePri.Value;
                this.crediDocu.BancoID.Value = this.clienteTercero.BancoID_1.Value;
                this.crediDocu.DescBanco.Value = this.clienteTercero.Banco1Desc.Value;
                this.crediDocu.TipoCuenta.Value = this.clienteTercero.CuentaTipo_1.Value;
                this.crediDocu.NumCuenta.Value = this.clienteTercero.BcoCtaNro_1.Value;
                this.crediDocu.VlrPagar.Value = item.VlrCuota.Value;
            }

            this.txtVlrGiro.EditValue = this.creditos.Count > 0 ? this.creditos[0].VlrGiro.Value.Value : 0;
            #endregion
            #region Asigna la info a la grilla

            if (this.creditos.Count > 0)
            {
                this.allowValidate = false;
                this.currentRow = 0;
                this.gcDocuments.DataSource = creditos;
                this.allowValidate = true;
                this.gvDocuments.MoveFirst();
            }
            else
            {
                this.gcDocuments.DataSource = null;
            }

            this.gcDocuments.RefreshDataSource();
            #endregion
        }

        #endregion

        #region Evento Header

        /// <summary>
        /// Evento para validar el check de la columna aprobado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void chkSeleccionar_CheckedChanged(object sender, EventArgs e)
        {
            decimal valorTotal = 0;
            if (this.chkSeleccionar.Checked)
            {
                for (int i = 0; i < this.gvDocuments.DataRowCount; i++)
                {
                    creditos[i].Aprobado.Value = true;
                    valorTotal += creditos[i].VlrGiro.Value.Value;
                }
                this.txtVlrGiro.EditValue = valorTotal;
            }
            else
            {
                for (int i = 0; i < this.gvDocuments.DataRowCount; i++)
                {
                    creditos[i].Aprobado.Value = false;
                }
                this.txtVlrGiro.EditValue = 0;
            }
            this.gcDocuments.RefreshDataSource();
        }

        #endregion

        #region Eventos grilla de Documentos

        /// <summary>
        /// Se ejecuta cuando se cambia la fila de los documentos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocuments_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            base.gvDocuments_FocusedRowChanged(sender, e);
            if (this.currentRow != -1)
            {
                if (e.FocusedRowHandle >= 0 && e.FocusedRowHandle <= this.gvDocuments.RowCount - 1)
                {
                    this.currentRow = e.FocusedRowHandle;
                    this.txtVlrGiro.EditValue = creditos[this.currentRow].VlrGiro.Value;
                }
            }
            else
            {
                this.txtVlrGiro.EditValue = 0;
            }
        }

        /// <summary>
        /// Asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocuments_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            try
            {
                base.gvDocuments_CustomRowCellEdit(sender, e);
                string fieldName = e.Column.FieldName.Length >= this.unboundPrefix.Length ? e.Column.FieldName.Substring(this.unboundPrefix.Length) : string.Empty;
                if (fieldName == "VlrGiro")
                    e.RepositoryItem = this.editSpin;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionGiroRechazo.cs", "gvDocuments_CustomRowCellEdit"));
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocuments_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

                #region Generales
                if (fieldName == "Aprobado")
                {
                    if ((bool)e.Value)
                    {
                        this.creditos[e.RowHandle].Aprobado.Value = true;
                    }
                    else
                    {
                        this.creditos[e.RowHandle].Aprobado.Value = false;
                    }
                }

                #endregion

                this.ValidateDocRow(e.RowHandle);
                this.gcDocuments.RefreshDataSource();
                this.gvDocuments.BestFitColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionGiro.cs", "gvDocuments_CellValueChanging"));
            }
        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            this.gvDocuments.PostEditor();
            try
            {
                if (creditos != null && creditos.Count > 0)
                {
                    this.creditosToSend = this.creditos.Where(x => x.Aprobado.Value.Value).ToList();
                    Thread process = new Thread(this.ApproveThread);
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionGiroRechazo.cs", "TBSave"));
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo que se ejecuta al enviar a aprobacion
        /// </summary>
        protected override void ApproveThread()
        {
            try
            {
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCartera(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                List<DTO_TxResult> results = _bc.AdministrationModel.AprobarGiro_CreditoRechazo(this.documentID, this.actividadFlujoID, this.creditosToSend);

                FormProvider.Master.StopProgressBarThread(this.documentID);
                List<DTO_TxResult> resultsNOK = new List<DTO_TxResult>();

                MessageForm frm = new MessageForm(results);
                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                this.Invoke(this.refreshData);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionGiroRechazo.cs", "ApproveThread"));
                this.Invoke(this.refreshData);
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion

    }
}
