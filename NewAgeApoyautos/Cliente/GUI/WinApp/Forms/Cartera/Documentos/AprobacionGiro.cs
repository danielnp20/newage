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
    public partial class AprobacionGiro : DocumentAprobBasicForm
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
        private List<DTO_ccCreditoDocu> creditosTemp = new List<DTO_ccCreditoDocu>();
        private List<DTO_ccCreditoDocu> creditosToSend = new List<DTO_ccCreditoDocu>();
        private DTO_coTercero clienteTercero = new DTO_coTercero();

        private bool isPagoMasivo;
        private bool firstTime = true;
        #endregion

        public AprobacionGiro()
            : base()
        {
        }

        public AprobacionGiro(string mod)
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
                    this.creditosTemp = new List<DTO_ccCreditoDocu>();
                    this.creditos = this._bc.AdministrationModel.LiquidacionCredito_GetAll(this.actividadFlujoID, true, false);
                    this.creditos = this.creditos.FindAll(x=>x.VlrGiro.Value != 0);
                    if (this.lkp_TipoPago.EditValue == "1")
                    {
                        this.creditosTemp = this.creditos.Where(x => !x.PagoVentanillaInd.Value.Value).ToList();
                        this.PagoDirecto();
                    }
                    else if (this.lkp_TipoPago.EditValue == "2")
                    {
                        this.creditosTemp = this.creditos.Where(x => x.PagoVentanillaInd.Value.Value).ToList();
                        this.PagoMasivo();
                    }
                    else
                    {
                        this.gcDocuments.DataSource = null;
                    }
                }
                else
                    this.firstTime = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionGiro.cs", "LoadDocuments"));
            }
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.AprobacionGiros;
            this.frmModule = ModulesPrefix.cc;

            #region Carga el combo de forma de pago
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("1", this._bc.GetResource(LanguageTypes.Tables, "tbl_32561_PagosDirectos"));
            dic.Add("2", this._bc.GetResource(LanguageTypes.Tables, "tbl_32561_PagosMasivos"));
            this.lkp_TipoPago.Properties.DataSource = dic;
            this.lkp_TipoPago.EditValue = "1";
            #endregion

            //Carga la informacion de la maestras
            _bc.InitMasterUC(this.masterBancos, AppMasters.tsBancosCuenta, true, true, true, false);
            this.masterBancos.EnableControl(false);

            base.SetInitParameters();
            this.gvDocuments.OptionsView.ShowAutoFilterRow = true;
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
                noAprob.Visible = true;
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

                #region Agrego las columnas de la sub-grilla
                //TerceroID
                GridColumn terceroID = new GridColumn();
                terceroID.FieldName = this.unboundPrefix + "TerceroID";
                terceroID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TerceroID");
                terceroID.UnboundType = UnboundColumnType.String;
                terceroID.VisibleIndex = 0;
                terceroID.Width = 100;
                terceroID.Visible = true;
                terceroID.AppearanceHeader.Font = new Font("Arial", 6.50F, FontStyle.Bold, GraphicsUnit.Pixel);
                terceroID.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(terceroID);

                //Nombre
                GridColumn nombreBeneficiario = new GridColumn();
                nombreBeneficiario.FieldName = this.unboundPrefix + "Nombre";
                nombreBeneficiario.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Nombre");
                nombreBeneficiario.UnboundType = UnboundColumnType.String;
                nombreBeneficiario.VisibleIndex = 1;
                nombreBeneficiario.Width = 100;
                nombreBeneficiario.Visible = true;
                nombreBeneficiario.AppearanceHeader.Font = new Font("Arial", 6.50F, FontStyle.Bold, GraphicsUnit.Pixel);
                nombreBeneficiario.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(nombreBeneficiario);

                //Valor
                GridColumn valor = new GridColumn();
                valor.FieldName = this.unboundPrefix + "Valor";
                valor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Valor");
                valor.UnboundType = UnboundColumnType.Integer;
                valor.VisibleIndex = 2;
                valor.Width = 100;
                valor.Visible = true;
                valor.ColumnEdit = this.editSpin;
                valor.AppearanceHeader.Font = new Font("Arial", 6.50F, FontStyle.Bold, GraphicsUnit.Pixel);
                valor.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(valor);
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionGiro.cs", "AddDocumentCols"));
            }
        }

        #endregion

        #region Funciones privadas

        /// <summary>
        /// Funcion para limpiar el documento
        /// </summary>
        private void CleanData()
        {
            this.creditosTemp = new List<DTO_ccCreditoDocu>();
            this.creditos = new List<DTO_ccCreditoDocu>();
            this.crediDocu = new DTO_ccCreditoDocu();

            this.gcDocuments.DataSource = null;

            if (this.lkp_TipoPago.EditValue == "1")
                this.masterBancos.EnableControl(false);
            else
                this.masterBancos.EnableControl(true);

            this.masterBancos.Value = String.Empty;
        }

        /// <summary>
        /// Funcion que valida la grilla antes de aprobar cada registro
        /// </summary>
        /// <returns></returns>
        private bool ValidateHeader()
        {
            try
            {
                #region Valida si es pago masivo
                if (this.lkp_TipoPago.EditValue == "2" && !this.masterBancos.ValidID)
                {
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_SelectBanco));
                    return false;
                }
                #endregion
                #region Valida pago directo
                DTO_TxResult result = new DTO_TxResult();
                result.Details = new List<DTO_TxResultDetail>();
                int count = 1;

                string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ClienteSinBanco);
                foreach (DTO_ccCreditoDocu item in this.creditosTemp)
                {
                    if (item.Aprobado.Value == true)
                    {
                        //Se comentario porque se mando quitar la restriccion de que tiene que exisitir un banco, esto se debe validar es en el modulo de tesoreria
                        //if (String.IsNullOrWhiteSpace(item.BancoID.Value))
                        //{
                        //    DTO_TxResultDetail rd = new DTO_TxResultDetail();
                        //    rd.line = count;
                        //    rd.Message = String.Format(msg, item.ClienteID.Value);;
                        //    result.Details.Add(rd);
                        //}
                    }
                    ++count;
                }

                if (result.Details.Count > 0)
                {
                    result.Result = ResultValue.NOK;
                    MessageForm form = new MessageForm(result);
                    form.Show();

                    return false;
                }

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionGiro.cs", "validateHeader"));
                return false;
            }
        }

        /// <summary>
        /// Funcion para establecer los giros para pagos directos
        /// </summary>
        private void PagoDirecto()
        {
            #region Carga la info de los bancos
            foreach (DTO_ccCreditoDocu item in this.creditosTemp)
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

            this.txtVlrGiro.EditValue = this.creditosTemp.Count > 0 ? this.creditosTemp[0].VlrGiro.Value.Value : 0;
            #endregion
            #region Asigna la info a la grilla

            if (this.creditosTemp.Count > 0)
            {
                this.allowValidate = false;
                this.currentRow = 0;
                this.gcDocuments.DataSource = this.creditosTemp;
                this.allowValidate = true;
                this.gvDocuments.MoveFirst();
            }
            else
            {
                this.gcDocuments.DataSource = null;
            }

            #endregion
        }

        /// <summary>
        /// Funcion para establecer los giros para pagos directos
        /// </summary>
        private void PagoMasivo()
        {
            this.txtVlrGiro.EditValue = 0;
            if (this.masterBancos.ValidID)
            {
                #region Asigna la informacion del banco
                DTO_tsBancosCuenta banco = (DTO_tsBancosCuenta)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.tsBancosCuenta, false, masterBancos.Value, false);
                foreach (DTO_ccCreditoDocu item in this.creditosTemp)
                {
                    this.clienteTercero = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, item.ClienteID.Value, true);

                    this.crediDocu = item;
                    this.crediDocu.Nombre.Value = this.clienteTercero.ApellidoPri.Value + this.clienteTercero.ApellidoSdo.Value + this.clienteTercero.NombrePri.Value;
                    this.crediDocu.BancoID.Value = banco.BancoID.Value;
                    this.crediDocu.DescBanco.Value = banco.BancoDesc.Value;
                    this.crediDocu.TipoCuenta.Value = banco.CuentaTipo.Value;
                    this.crediDocu.NumCuenta.Value = banco.CuentaNumero.Value;
                    this.crediDocu.VlrPagar.Value = item.VlrCuota.Value;

                    this.gcDocuments.RefreshDataSource();
                }
                #endregion
            }
            else
            {
                #region Deja la informacion del banco limpia
                foreach (DTO_ccCreditoDocu item in this.creditosTemp)
                {
                    this.clienteTercero = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, item.ClienteID.Value, true);

                    this.crediDocu = item;
                    this.crediDocu.Nombre.Value = string.Empty;
                    this.crediDocu.BancoID.Value = string.Empty;
                    this.crediDocu.DescBanco.Value = string.Empty;
                    this.crediDocu.TipoCuenta.Value = 0;
                    this.crediDocu.NumCuenta.Value = string.Empty;
                    this.crediDocu.VlrPagar.Value = item.VlrCuota.Value;

                    this.gcDocuments.RefreshDataSource();
                }
                #endregion
            }

            #region Asigna la info a la grilla
            if (this.creditosTemp.Count > 0)
            {
                this.allowValidate = false;
                this.currentRow = 0;
                this.gcDocuments.DataSource = this.creditosTemp;
                this.allowValidate = true;
                this.gvDocuments.MoveFirst();
            }
            else
            {
                this.gcDocuments.DataSource = null;
            }
            #endregion
        }

        #endregion

        #region Evento Header

        /// <summary>
        /// Evento que identifica si se realiza un pago masivo o directo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lkp_TipoPago_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.creditosTemp = new List<DTO_ccCreditoDocu>();
                this.masterBancos.EnableControl(false);

                this.creditosTemp = new List<DTO_ccCreditoDocu>();
                this.creditos = this._bc.AdministrationModel.LiquidacionCredito_GetAll(this.actividadFlujoID, true, false);
                this.creditos = this.creditos.FindAll(x => x.VlrGiro.Value != 0);
                if (this.lkp_TipoPago.EditValue == "1")
                {
                    this.masterBancos.Value = string.Empty;
                    this.creditosTemp = this.creditos.Where(x => !x.PagoVentanillaInd.Value.Value).ToList();
                    this.PagoDirecto();
                }
                else if (this.lkp_TipoPago.EditValue == "2")
                {
                    this.masterBancos.EnableControl(true);
                    this.creditosTemp = this.creditos.Where(x => x.PagoVentanillaInd.Value.Value).ToList();
                    this.PagoMasivo();
                }
                else
                {
                    this.txtVlrGiro.EditValue = "0";
                    this.masterBancos.Value = string.Empty;
                    this.gcDocuments.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionGiro.cs", "lkp_TipoPago_EditValueChanged"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterBancos_Leave(object sender, EventArgs e)
        {
            if (this.lkp_TipoPago.EditValue == "2")
                this.PagoMasivo();
        }

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
                    this.creditosTemp[i].Aprobado.Value = true;
                    this.creditosTemp[i].Rechazado.Value = false;
                    valorTotal += this.creditosTemp[i].VlrGiro.Value.Value;
                }
                this.txtVlrGiro.EditValue = valorTotal;
            }
            else
            {
                for (int i = 0; i < this.gvDocuments.DataRowCount; i++)
                {
                    this.creditosTemp[i].Aprobado.Value = false;
                    this.creditosTemp[i].Rechazado.Value = false;
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
                if (e.FocusedRowHandle >= 0 && this.lkp_TipoPago.EditValue == "1" && e.FocusedRowHandle <= this.gvDocuments.RowCount - 1)
                {
                    this.currentRow = e.FocusedRowHandle;
                    this.txtVlrGiro.EditValue = this.creditosTemp[this.currentRow].VlrGiro.Value;
                }
            }
            else
            {
                this.txtVlrGiro.EditValue = "0";
            }
        }

        /// <summary>
        /// Asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocuments_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            base.gvDocuments_CustomRowCellEdit(sender, e);
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "VlrGiro")
                e.RepositoryItem = this.editSpin;
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
                bool add = false;
                bool delete = false;

                #region Generales
                if (fieldName == "Aprobado")
                {
                    if ((bool)e.Value)
                    {
                        add = true;
                        this.creditosTemp[e.RowHandle].Aprobado.Value = true;
                        this.creditosTemp[e.RowHandle].Rechazado.Value = false;
                        if(String.IsNullOrWhiteSpace(this.creditosTemp[e.RowHandle].Observacion.Value))
                            this.gvDocuments.Columns[11].Visible = false;
                    }
                    else
                    {
                        delete = true;
                        this.creditosTemp[e.RowHandle].Aprobado.Value = false;
                    }
                }
                if (fieldName == "Rechazado")
                {
                    if ((bool)e.Value)
                    {
                        this.gvDocuments.Columns[11].Visible = true;
                        if (this.creditosTemp[e.RowHandle].Aprobado.Value.Value)
                            delete = true;
                        this.creditosTemp[e.RowHandle].Aprobado.Value = false;
                        this.creditosTemp[e.RowHandle].Rechazado.Value = true;
                    }
                    else
                    {
                        this.creditosTemp[e.RowHandle].Rechazado.Value = false;
                    }
                }
                if (fieldName == "Observacion")
                {
                    this.creditosTemp[this.currentRow].Observacion.Value = e.Value.ToString();
                }
                #endregion
                #region Actualiza el valos de giro para los pagos masivos
                if (this.lkp_TipoPago.EditValue == "2")
                {
                    decimal vlrTotal = Convert.ToDecimal(this.txtVlrGiro.EditValue, CultureInfo.InvariantCulture);

                    if (add)
                        vlrTotal += this.creditosTemp[e.RowHandle].VlrGiro.Value.Value;
                    else if (delete)
                        vlrTotal -= this.creditosTemp[e.RowHandle].VlrGiro.Value.Value;

                    this.txtVlrGiro.EditValue = vlrTotal;
                }
                #endregion
                this.ValidateDocRow(e.RowHandle);
                this.gcDocuments.RefreshDataSource();
                this.gvDocuments.BestFitColumns();
                this.gvDocuments.Columns[11].Width = 100; 
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionGiro.cs", "gvDocuments_CellValueChanging"));
            }
        }

        #endregion

        #region Eventos editor de texto

        /// <summary>
        /// Toma los valores de la grilla y los envia al popup al momento de abrirlo
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void riPopup_QueryPopUp(object sender, CancelEventArgs e)
        {
            string fieldName = this.gvDocuments.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "Observacion")
                this.richEditControl.Document.Text = this.gvDocuments.GetFocusedRowCellValue(fieldName).ToString();
        }

        /// <summary>
        /// Toma los valores ingresados en el popup al momento de cerrarlo y los envia a la celda de la grilla 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void riPopup_QueryResultValue(object sender, QueryResultValueEventArgs e)
        {
            e.Value = this.richEditControl.Document.Text;
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
                if (this.creditosTemp != null && this.creditosTemp.Count > 0)
                {
                    if (this.ValidateHeader())
                    {
                        if (this.ValidateDocRow(this.gvDocuments.FocusedRowHandle))
                        {
                            this.isPagoMasivo = this.lkp_TipoPago.EditValue == "1" ? false : true;
                            this.creditosToSend = this.creditos.Where(x => x.Aprobado.Value.Value || x.Rechazado.Value.Value).ToList();
                            Thread process = new Thread(this.ApproveThread);
                            process.Start();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionGiro.cs", "TBSave"));
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

                List<DTO_TxResult> results;

                results = _bc.AdministrationModel.AprobarGiro_Credito_AprobarRechazar(this.documentID, this.actividadFlujoID, this.creditosToSend, this.isPagoMasivo);

                FormProvider.Master.StopProgressBarThread(this.documentID);
                List<DTO_TxResult> resultsNOK = new List<DTO_TxResult>();

                int i = 0;
                int percent = 0;
                #region Variables para el mail

                DTO_seUsuario user = _bc.AdministrationModel.seUsuario_GetUserbyID(this.actividadDTO.seUsuarioID.Value);

                string body = string.Empty;
                string subject = string.Empty;
                string email = user.CorreoElectronico.Value;

                string subjectApr = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_Approved_Subject);
                string subjectRech = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_Rejected_Subject);
                string bodyApr = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_ApprovedCartera_Body);
                string bodyRech = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_RejectedCartera_Body);
                string formName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());

                #endregion
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SendingMails) });
                foreach (DTO_TxResult result in results)
                {
                    #region Manejo de progreso
                    FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                    percent = ((i + 1) * 100) / (results.Count);

                    if (FormProvider.Master.ProcessCanceled(this.documentID))
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser));
                        break;
                    }
                    #endregion
                    DTO_ccCreditoDocu credito = this.creditosToSend[i];
                    if (credito.Aprobado.Value.Value || credito.Rechazado.Value.Value)
                    {
                        if (result.Result == ResultValue.OK)
                        {
                            #region Envia el correo
                            if (credito.Aprobado.Value.Value)
                            {
                                subject = string.Format(subjectApr, formName);
                                body = string.Format(bodyApr, formName, credito.Libranza.Value, credito.Nombre.Value,
                                  credito.Observacion.Value);
                            }
                            else if (credito.Rechazado.Value.Value)
                            {
                                subject = string.Format(subjectRech, formName);
                                body = string.Format(bodyRech, formName, credito.Observacion.Value, credito.Libranza.Value,
                                    credito.Nombre.Value);
                            }

                            _bc.SendMail(this.documentID, subject, body, email);
                            #endregion
                        }
                        resultsNOK.Add(result);
                    }
                    i++;
                }
                MessageForm frm = new MessageForm(resultsNOK);
                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                this.Invoke(this.refreshData);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionGiro.cs", "ApproveThread"));
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
