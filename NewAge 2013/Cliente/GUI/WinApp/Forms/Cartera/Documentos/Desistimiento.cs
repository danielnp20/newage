using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraRichEdit.API.Word;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using System.Reflection;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using System.Globalization;
using SentenceTransformer;
using DevExpress.XtraEditors;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class Desistimiento : DocumentForm
    {
        #region Variables Formulario

        //Obtiene la instancia del controlador
        private BaseController _bc = BaseController.GetInstance();
        private string libranzaID = string.Empty;
        private int proposito;
        private DTO_ccCliente cliente;
        private List<DTO_ccCreditoDocu> creditos = null;
        private DTO_ccCreditoDocu credito = null;
        private List<DTO_ccEstadoCuentaComponentes> componentes = new List<DTO_ccEstadoCuentaComponentes>();

        private DateTime fechaPerido;

        #endregion

        public Desistimiento()
            : base()
        {
            //InitializeComponent();
        }

        public Desistimiento(string mod)
            : base(mod)
        {
        }

        #region Funciones Privadas

        /// <summary>
        /// Limpia el formulario
        /// </summary>
        private void CleanData()
        {
            this.cliente = null;
            this.masterCliente.Value = String.Empty;
            this.libranzaID = string.Empty;
            this.lkp_Libranzas.Properties.DataSource = string.Empty;

            this.credito = null;
            this.creditos = new List<DTO_ccCreditoDocu>(); 
            this.componentes = null;

            this.gvDocument.Columns[this.unboundPrefix + "ComponenteCarteraID"].OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns[this.unboundPrefix + "AbonoValor"].OptionsColumn.AllowEdit = false;

            this.gcDocument.DataSource = this.componentes;
        }

        /// <summary>
        /// Valida la info del cabezote
        /// </summary>
        private bool ValidateDoc()
        {
            if (String.IsNullOrWhiteSpace(this.masterCliente.Value))
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCliente.LabelRsx);
                MessageBox.Show(msg);
                this.masterCliente.Focus();
                return false;
            }

            if (this.componentes.Count == 0)
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidField), this.lkp_Libranzas.Text);
                MessageBox.Show(msg);
                this.lkp_Libranzas.Focus();
                return false;
            }

            return true;
        }

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.Desistimiento;
            this.frmModule = ModulesPrefix.cc;

            InitializeComponent();
            base.SetInitParameters();

            this.AddComponentesCols();

            //Modifica el tamaño de las Grillas
            this.tlSeparatorPanel.RowStyles[2].Height = 200;

            this.gvDocument.OptionsBehavior.AutoPopulateColumns = true;
            this.grpboxDetail.Dock = DockStyle.Fill;

            base.dtFecha.Enabled = false;

            //Carga la Informacion del Hedear
            _bc.InitMasterUC(this.masterCliente, AppMasters.ccCliente, true, true, true, false);
            this.fechaPerido = Convert.ToDateTime(this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_Periodo));

            //Pone la fecha de aplica con base a la del periodo
            bool cierreValido = true;
            int diaCierre = 1;
            string indCierreDiaStr = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_CierreDiarioInd);
            if(indCierreDiaStr == "1")
            {
                string diaCierreStr = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_DiaUltimoCierre);
                if (string.IsNullOrWhiteSpace(diaCierreStr) || diaCierreStr == "0")
                    diaCierreStr = "1";

                diaCierre = Convert.ToInt16(diaCierreStr);
                if(diaCierre > DateTime.DaysInMonth(this.fechaPerido.Year, this.fechaPerido.Month))
                {
                    cierreValido = false;
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_DiaCerrado));

                    this.masterCliente.EnableControl(false);
                    this.dtFechaAplica.Enabled = false;

                    FormProvider.Master.itemNew.Enabled = false;
                    FormProvider.Master.itemSave.Enabled = false;
                }
            }

            if (cierreValido)
            {
                this.dtFechaAplica.Properties.MaxValue = new DateTime(this.fechaPerido.Year, this.fechaPerido.Month, DateTime.DaysInMonth(this.fechaPerido.Year, this.fechaPerido.Month));
                this.dtFechaAplica.Properties.MinValue = new DateTime(this.fechaPerido.Year, this.fechaPerido.Month, diaCierre);
                this.dtFechaAplica.DateTime = new DateTime(this.fechaPerido.Year, this.fechaPerido.Month, diaCierre);
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla de detalles
        /// </summary>
        protected virtual void AddComponentesCols()
        {
            try
            {
                //Codigo Componente
                GridColumn comptCarteraID = new GridColumn();
                comptCarteraID.FieldName = this.unboundPrefix + "ComponenteCarteraID";
                comptCarteraID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PagosTotales.ToString() + "_ComponenteCarteraID");
                comptCarteraID.UnboundType = UnboundColumnType.String;
                comptCarteraID.VisibleIndex = 0;
                comptCarteraID.Width = 100;
                comptCarteraID.Visible = true;
                comptCarteraID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                comptCarteraID.OptionsColumn.AllowEdit = false;
                comptCarteraID.ColumnEdit = this.editBtnGrid;
                this.gvDocument.Columns.Add(comptCarteraID);

                //Descripcion
                GridColumn descripcion = new GridColumn();
                descripcion.FieldName = this.unboundPrefix + "Descriptivo";
                descripcion.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PagosTotales.ToString() + "_Descripcion");
                descripcion.UnboundType = UnboundColumnType.String;
                descripcion.VisibleIndex = 1;
                descripcion.Width = 200;
                descripcion.Visible = true;
                descripcion.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                descripcion.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(descripcion);                

                //SaldoValor
                GridColumn saldoValor = new GridColumn();
                saldoValor.FieldName = this.unboundPrefix + "SaldoValor";
                saldoValor.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PagosTotales.ToString() + "_SaldoValor");
                saldoValor.UnboundType = UnboundColumnType.Integer;
                saldoValor.VisibleIndex = 2;
                saldoValor.Width = 200;
                saldoValor.Visible = true;
                saldoValor.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                saldoValor.OptionsColumn.AllowEdit = false;
                saldoValor.ColumnEdit = this.editSpin;
                this.gvDocument.Columns.Add(saldoValor);

                //PagoValor
                GridColumn pagoValor = new GridColumn();
                pagoValor.FieldName = this.unboundPrefix + "PagoValor";
                pagoValor.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PagosTotales.ToString() + "_PagoValor");
                pagoValor.UnboundType = UnboundColumnType.Integer;
                pagoValor.VisibleIndex = 3;
                pagoValor.Width = 200;
                pagoValor.Visible = true;
                pagoValor.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                pagoValor.OptionsColumn.AllowEdit = false;
                pagoValor.ColumnEdit = this.editSpin;
                this.gvDocument.Columns.Add(pagoValor);

                //Abono Valor
                GridColumn abonoValor = new GridColumn();
                abonoValor.FieldName = this.unboundPrefix + "AbonoValor";
                abonoValor.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PagosTotales.ToString() + "_AbonoValor");
                abonoValor.UnboundType = UnboundColumnType.Integer;
                abonoValor.VisibleIndex = 4;
                abonoValor.Width = 200;
                abonoValor.Visible = true;
                abonoValor.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                abonoValor.OptionsColumn.AllowEdit = false;
                abonoValor.ColumnEdit = this.editSpin;
                this.gvDocument.Columns.Add(abonoValor);

                this.gvDocument.OptionsView.ColumnAutoWidth = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Desistimiento.cs", "AddDocumentCols"));
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
            {
                FormProvider.Master.itemSave.Enabled = FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
                FormProvider.Master.itemGenerateTemplate.Visible = false;
                FormProvider.Master.itemDelete.Visible = false;
                FormProvider.Master.itemPrint.Visible = false;
                FormProvider.Master.itemCopy.Visible = false;
                FormProvider.Master.itemPaste.Visible = false;
                FormProvider.Master.itemImport.Visible = false;
                FormProvider.Master.itemExport.Visible = false;
                FormProvider.Master.itemRevert.Visible = false;
                FormProvider.Master.itemSendtoAppr.Visible = false;
                FormProvider.Master.itemFilter.Visible = false;
                FormProvider.Master.itemFilterDef.Visible = false;
                FormProvider.Master.tbBreak.Visible = false;
                FormProvider.Master.tbBreak0.Visible = false;
                FormProvider.Master.tbBreak1.Visible = false;
            }
        }

        #endregion

        #region Eventos Header

        /// <summary>
        /// Evento que filtra una lista de DTO_ccCreditoDocu de acuerdo al cliente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterCliente_Leave(object sender, EventArgs e)
        {
            try
            {
                this.creditos = new List<DTO_ccCreditoDocu>();
                if (this.cliente == null || this.cliente.ID.Value != this.masterCliente.Value)
                {
                    if (this.masterCliente.ValidID)
                    {
                        this.proposito = Convert.ToByte(PropositoEstadoCuenta.Desistimiento);
                        this.creditos = _bc.AdministrationModel.GetCreditosPendientesByProposito(this.masterCliente.Value, this.proposito);

                        if (this.creditos.Count == 0)
                        {
                            DateTime fechaAplicaTemp = this.dtFechaAplica.DateTime;
                            string clienteTemp = this.masterCliente.Value;

                            string msg = String.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ClienteForDesistimiento), this.masterCliente.Value);
                            MessageBox.Show(msg);
                            this.CleanData();

                            this.dtFechaAplica.DateTime = fechaAplicaTemp;
                            this.masterCliente.Value = clienteTemp;
                        }
                        else
                        {
                            this.lkp_Libranzas.Properties.DataSource = this.creditos;
                        }
                    }
                    else
                    {
                        this.cliente = null;
                        this.lkp_Libranzas.Properties.DataSource = string.Empty;
                        this.componentes = null;
                        this.gcDocument.DataSource = this.componentes;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Desistimiento.cs", "masterCliente_Leave"));
            }
        }

        /// <summary>
        /// Evento que carga la grilla del cliente de acuerdo a la libranza seleccionada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lkp_Libranzas_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(this.lkp_Libranzas.Text) && this.libranzaID != this.lkp_Libranzas.Text)
                {
                    List<DTO_ccCreditoDocu> temp = (from c in this.creditos where c.Libranza.Value.Value.ToString() == this.lkp_Libranzas.Text && (c.EC_FijadoInd.Value == true || c.EC_FijadoInd.Value == null) select c).ToList();
                    if (temp.Count > 0 )
                    {
                        this.libranzaID = this.lkp_Libranzas.Text;

                        this.credito = creditos.FirstOrDefault(c => c.Libranza.Value.ToString() == this.libranzaID);
                        this.componentes = this._bc.AdministrationModel.EstadoCuenta_GetComponentesByNumeroDoc(credito.DocEstadoCuenta.Value.Value, false);
                        this.gcDocument.DataSource = this.componentes;
                        this.gvDocument.PostEditor();
                    }
                    else
                    {
                        this.credito = null;
                        this.libranzaID = string.Empty;
                        this.gcDocument.DataSource = null;
                        string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_EstadoCuentaNoFijado);
                        MessageBox.Show(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Desistimiento.cs", "lkp_Libranzas_Leave"));
            }
        }

        /// <summary>
        /// Evento que recalcula el plan de pagos con base a la fecha seleccionada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtFechaConsigna_DateTimeChanged(object sender, EventArgs e)
        {
            this.lkp_Libranzas_Leave(sender, e);
        }

        #endregion

        #region Eventos Barra Herramientas

        /// <summary>
        /// Boton para crear nuevo registro
        /// </summary>
        public override void TBNew()
        {
            try
            {
                this.CleanData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Desistimiento.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            try
            {
                this.gvDocument.PostEditor();

                bool isValid = this.ValidateDoc();

                if (isValid)
                {
                    #region Carga el DTO de SaldoComponentes
                    List<DTO_ccSaldosComponentes> saldoComponentes = new List<DTO_ccSaldosComponentes>();
                    foreach (DTO_ccEstadoCuentaComponentes item in this.componentes)
                    {
                        DTO_ccSaldosComponentes saldoComp = new DTO_ccSaldosComponentes();
                        saldoComp.ComponenteCarteraID.Value = item.ComponenteCarteraID.Value;
                        saldoComp.Descriptivo.Value = item.Descriptivo.Value;

                        saldoComp.TotalSaldo.Value = item.SaldoValor.Value;
                        saldoComp.CuotaSaldo.Value = item.AbonoValor.Value;
                        saldoComp.AbonoValor.Value = item.AbonoValor.Value;

                        saldoComponentes.Add(saldoComp);
                    }
                    #endregion
                    #region Guarda la info

                    DTO_ccCreditoDocu creditoDocuTemp = ObjectCopier.Clone(this.credito);
                    creditoDocuTemp.FechaPagoParcial.Value = this.dtFechaAplica.DateTime;

                    DTO_TxResult result = _bc.AdministrationModel.Credito_Desistimiento(this.documentID, this._actFlujo.ID.Value, creditoDocuTemp, this.dtFechaAplica.DateTime);// saldoComponentes);
                    if (result.Result == ResultValue.OK)
                    {
                        MessageForm frm = new MessageForm(result);
                        frm.ShowDialog();

                        this.CleanData();
                    }
                    else
                    {
                        MessageForm msg = new MessageForm(result);
                        msg.ShowDialog();
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Desistimiento.cs", "TBSave"));
            }
        }

        #endregion

    }
}
