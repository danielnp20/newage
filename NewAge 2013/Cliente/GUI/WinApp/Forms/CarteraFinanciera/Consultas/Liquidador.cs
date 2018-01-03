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
using System.Linq;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class Liquidador: FormWithToolbar
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();

        //Para manejo de propiedades
        private FormTypes _frmType = FormTypes.DocumentAprob;

        private string _frmName;
        private int _documentID;
        private ModulesPrefix _frmModule;
        private string _unboundPrefix = "Unbound_";
        private string _compPrefix = "CompPrefix";
        protected bool isValid = true;
        

        //DTOs        
        private DTO_PlanDePagos _liquidador = null;
        private DTO_Cuota _cuota = new DTO_Cuota();

        //Variables formulario
        private string _lineaCreditoID;
        private string compSeguro;
        private string descSeguro;
        
        #endregion Variables

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public Liquidador()
        {
            InitializeComponent();
            try
            {
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());
                FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
             
                this.AddGridCols();
                this.AddGridColsDetail();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionCredito.cs", "DigitacionCredito"));
            }
        }

        #region Funciones Privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            this._documentID = AppQueries.Liquidador;
            this._frmModule = ModulesPrefix.cc;
            
            //Carga la informacion de la maestras
            _bc.InitMasterUC(this.masterLineaCredito, AppMasters.ccLineaCredito, true, true, true,true);
            _bc.InitMasterUC(this.masterPagaduria, AppMasters.ccPagaduria, true, true, true, false);
           
            //Pone la fecha actual
            this.dtFechaLiquidacion.DateTime = DateTime.Now;
            this.dtFechaCuota.DateTime = DateTime.Now;
            this.dtFechaIncorp.DateTime = DateTime.Now;

            //Carga el codigo del componente de seguro
            this.compSeguro = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteSeguroVida);
        }

        /// <summary>
        /// Agrega las columnas a la grilla 1
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                #region Componentes Cartera
                //Campo de codigo
                GridColumn codigo = new GridColumn();
                codigo.FieldName = this._unboundPrefix + "ComponenteCarteraID";
                codigo.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ComponenteCarteraID");
                codigo.UnboundType = UnboundColumnType.String;
                codigo.VisibleIndex = 0;
                codigo.Width = 50;
                codigo.Visible = true;
                codigo.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(codigo);

                //Descriptivo
                GridColumn descriptivo = new GridColumn();
                descriptivo.FieldName = this._unboundPrefix + "Descripcion";
                descriptivo.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Descripcion");
                descriptivo.UnboundType = UnboundColumnType.String;
                descriptivo.VisibleIndex = 1;
                descriptivo.Width = 100;
                descriptivo.Visible = true;
                descriptivo.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(descriptivo);

                //Valor Cuota
                GridColumn cuotaValor = new GridColumn();
                cuotaValor.FieldName = this._unboundPrefix + "CuotaValor";
                cuotaValor.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ValorCuota");
                cuotaValor.UnboundType = UnboundColumnType.Decimal;
                cuotaValor.VisibleIndex = 2;
                cuotaValor.Width = 100;
                cuotaValor.Visible = true;
                cuotaValor.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(cuotaValor);

                //Valor Total
                GridColumn valorTotal = new GridColumn();
                valorTotal.FieldName = this._unboundPrefix + "TotalValor";
                valorTotal.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ValorTotal");
                valorTotal.UnboundType = UnboundColumnType.Decimal;
                valorTotal.VisibleIndex = 3;
                valorTotal.Width = 150;
                valorTotal.Visible = true;
                valorTotal.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(valorTotal);

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Liquidador.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla 2
        /// </summary>
        private void AddGridColsDetail()
        {
            try
            {
                #region Plan de Pagos
                //Numero Cuota
                GridColumn numCuota = new GridColumn();
                numCuota.FieldName = this._unboundPrefix + "NumCuota";
                numCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_NumeroCuotas");
                numCuota.UnboundType = UnboundColumnType.Integer;
                numCuota.VisibleIndex = 0;
                numCuota.Width = 50;
                numCuota.Visible = true;
                this.gvDetail.Columns.Add(numCuota);

                //Fecha
                GridColumn fecha = new GridColumn();
                fecha.FieldName = this._unboundPrefix + "Fecha";
                fecha.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Fecha");
                fecha.UnboundType = UnboundColumnType.DateTime;
                fecha.VisibleIndex = 1;
                fecha.Width = 100;
                fecha.Visible = true;
                this.gvDetail.Columns.Add(fecha);

                //Capital
                GridColumn capital = new GridColumn();
                capital.FieldName = this._unboundPrefix + "Capital";
                capital.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Capital");
                capital.UnboundType = UnboundColumnType.Integer;
                capital.VisibleIndex = 2;
                capital.Width = 100;
                capital.Visible = true;
                this.gvDetail.Columns.Add(capital);

                //Intereses
                GridColumn intereses = new GridColumn();
                intereses.FieldName = this._unboundPrefix + "Intereses";
                intereses.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Intereses");
                intereses.UnboundType = UnboundColumnType.Decimal;
                intereses.VisibleIndex = 3;
                intereses.Width = 150;
                intereses.Visible = true;
                this.gvDetail.Columns.Add(intereses);

                //Seguro
                GridColumn seguro = new GridColumn();
                seguro.FieldName = this._unboundPrefix + "Seguro";
                seguro.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Seguro");
                seguro.UnboundType = UnboundColumnType.Integer;
                seguro.VisibleIndex = 4;
                seguro.Width = 150;
                seguro.Visible = true;
                this.gvDetail.Columns.Add(seguro);

                int i = 1;
                for (i = 1; i <= this._cuota.Componentes.Count; ++i)
                {
                    string comp = this._cuota.Componentes[i - 1];
                    if (comp != this.descSeguro)
                    {
                        GridColumn componentes = new GridColumn();
                        componentes.FieldName = this._unboundPrefix + this._compPrefix + this._cuota.Componentes[i - 1];
                        componentes.Caption = this._cuota.Componentes[i - 1];
                        componentes.UnboundType = UnboundColumnType.Integer;
                        componentes.VisibleIndex = 4 + i;
                        componentes.Width = 150;
                        componentes.Visible = true;
                        this.gvDetail.Columns.Add(componentes);
                    }
                } 

                //Valor Cuota
                GridColumn valorCuota = new GridColumn();
                valorCuota.FieldName = this._unboundPrefix + "ValorCuota";
                valorCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ValorCuota");
                valorCuota.UnboundType = UnboundColumnType.Integer;
                valorCuota.VisibleIndex = 4 + i;
                valorCuota.Width = 150;
                valorCuota.Visible = true;
                this.gvDetail.Columns.Add(valorCuota);

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Liquidador.cs", "AddGridColsDetail"));
            }
        }

        /// <summary>
        /// Refresca el contenido del formulario
        /// </summary>
        private void CleanData()
        {
            try
            {
                //Header
                this.masterLineaCredito.Value = String.Empty;
                this.masterPagaduria.Value = String.Empty;
                this.comboPlazo.SelectedIndex = -1;
                this.txtVlrSolicitado.Text = "0";
                this.txtEdad.Text = string.Empty;
                this.chkClienteNuevo.Checked = false;

                this.txtVlrAdicional.Text = "0";
                this.txtVlrDto.Text = "0";
                this.txtVlrPrestamo.Text = "0";
                this.txtVlrGiro.Text = "0";
                this.txtVlrCuota.Text = "0";
                this.txtVlrLibranza.Text = "0";
                this.txtIntereses.Text = "0";
                
                //Footer
                this._liquidador = null;
                this._cuota = new DTO_Cuota();

                //Variables
                this._lineaCreditoID = null;

                //Grillas
                this.gcDocument.Enabled = true;
                this.gcDocument.DataSource = this._liquidador;
                this.gcDetail.Enabled = true;
                this.gcDetail.DataSource = this._liquidador;

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Liquidador.cs", "CleanData"));
            }

        }

        #endregion Funciones Privadas

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
                FormProvider.Master.Form_Enter(this, this._documentID, this._frmType, this._frmModule);
                FormProvider.Master.itemNew.Visible = true;
                FormProvider.Master.itemNew.Enabled = true;
                FormProvider.Master.itemSearch.Visible = true;
                FormProvider.Master.itemSearch.Enabled = true;
                FormProvider.Master.itemUpdate.Visible = false;
                FormProvider.Master.itemSave.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Liquidador.cs", "Form_Enter"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Leave(object sender, EventArgs e)
        {
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
                FormProvider.Master.Form_Closing(this, this._documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Liquidador.cs", "Form_Closing"));
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
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this._frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Liquidador.cs", "Form_FormClosed"));
            }
        }

        #endregion Eventos MDI

        #region Eventos Formulario

        /// <summary>
        /// Evento que trae los componentes de cartera con base a la linea de credito
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterLineaCredito_Leave(object sender, EventArgs e)
        {
            if (this._lineaCreditoID != this.masterLineaCredito.Value)
                this._lineaCreditoID = this.masterLineaCredito.Value;
        }

        #endregion Eventos Formulario

        #region Eventos Grilla

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            try
            {
                Object dto = (Object)e.Row;
                string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

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
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetail_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            try
            {
                DTO_Cuota dto = (DTO_Cuota)e.Row;
                string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

                if (e.IsGetData)
                {
                    if (fieldName.StartsWith(this._compPrefix))
                    {
                        fieldName = fieldName.Substring(this._compPrefix.Length);
                        e.Value = dto.ValoresComponentes[e.Column.AbsoluteIndex - 5];
                    }
                    else
                    {
                        PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                        if (pi != null &&
                            (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double" || pi.PropertyType.Name == "DateTime"))
                            e.Value = pi.GetValue(dto, null);
                    }
                }
            }
            catch (Exception)
            {

            }
        }
                
        /// <summary>
        /// asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            try
            {
                //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
                string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);
                if (fieldName == "CuotaValor" || fieldName == "TotalValor" || fieldName == "Capital" || fieldName == "Intereses" || fieldName == "ValorCuota"
                    || fieldName == "Seguro" || fieldName.StartsWith(this._compPrefix))
                {
                    e.RepositoryItem = this.editSpin;
                }
            }
            catch (Exception)
            {

            }
        }

        #endregion Enventos Grilla

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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Liquidador.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Boton para crear buscar
        /// </summary>
        public override void TBSearch()
        {
            try
            {
                if (!this.masterLineaCredito.ValidID || !this.masterPagaduria.ValidID)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidSearchCriteria));
                    return;
                }

                if (String.IsNullOrEmpty(this.txtVlrPrestamo.Text))
                {
                    string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.lblVlrPrestamo.Text);
                    MessageBox.Show(msg);
                    this.txtVlrPrestamo.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(this.txtEdad.Text))
                {
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.lblEdad.Text);
                    MessageBox.Show(msg);
                    this.txtEdad.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(this.comboPlazo.Text))
                {
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.lblPlazo.Text);
                    MessageBox.Show(msg);
                    this.comboPlazo.Focus();
                    return;
                }

                if (this._liquidador != null)
                {
                    #region Actualizar la busqueda
                    string msgTitleSearch = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_GetData));
                    string msgNewSearch = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewSearch);

                    if (MessageBox.Show(msgNewSearch, msgTitleSearch, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        this._liquidador = null;
                        this.gcDocument.DataSource = this._liquidador;
                        this.gcDetail.DataSource = this._liquidador;
                        
                        int vlrSolicitado = Convert.ToInt32(this.txtVlrSolicitado.EditValue);
                        int plazo = Convert.ToInt32(this.comboPlazo.Text);
                        int edad = Convert.ToInt32(this.txtEdad.Text);
                        object res = _bc.AdministrationModel.GenerarLiquidacionCartera(this.masterLineaCredito.Value, this.masterPagaduria.Value, vlrSolicitado, 
                            vlrSolicitado, plazo, edad, this.dtFechaLiquidacion.DateTime, null,this.dtFechaCuota.DateTime);
                        if (res.GetType() == typeof(DTO_TxResult))
                        {
                            DTO_TxResult txRes = (DTO_TxResult)res;
                            MessageForm msg = new MessageForm(txRes);
                            msg.Show();
                        }
                        else
                        {
                            this._liquidador = (DTO_PlanDePagos)res;

                            if (this._liquidador.VlrDescuento != 0)
                                this.txtVlrDto.Text = this._liquidador.VlrDescuento.ToString();

                            this.gcDocument.DataSource = this._liquidador.ComponentesAll;

                            if (this._liquidador.Cuotas != null && this._liquidador.Cuotas.Count != 0)
                            {
                                this.descSeguro = (from c in this._liquidador.ComponentesUsuario where c.ComponenteCarteraID.Value == this.compSeguro select c.Descripcion.Value).FirstOrDefault();
                                this._cuota = this._liquidador.Cuotas[0];
                                this.gvDetail.Columns.Clear();
                                this.AddGridColsDetail();
                                this.gcDetail.DataSource = this._liquidador.Cuotas;
                            }
                            else
                            {
                                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
                            }
                        }

                        this.txtVlrAdicional.Text = this._liquidador.VlrAdicional.ToString();
                        this.txtVlrPrestamo.Text = this._liquidador.VlrPrestamo.ToString();
                        this.txtVlrGiro.Text = this._liquidador.VlrGiro.ToString();
                        this.txtVlrCuota.Text = this._cuota.ValorCuota.ToString();
                        this.txtVlrLibranza.Text = this._liquidador.VlrLibranza.ToString();
                        this.txtIntereses.Text = this._liquidador.TasaTotal.ToString();

                    }
                    #endregion
                }
                else
                {
                    #region Realiza la busqueda
                    int vlrSolicitado = Convert.ToInt32(this.txtVlrSolicitado.EditValue);
                    int plazo = Convert.ToInt32(this.comboPlazo.Text);
                    int edad = Convert.ToInt32(this.txtEdad.Text);
                    object res = _bc.AdministrationModel.GenerarLiquidacionCartera(this.masterLineaCredito.Value, this.masterPagaduria.Value, vlrSolicitado, 
                        vlrSolicitado, plazo, edad, this.dtFechaLiquidacion.DateTime, null, this.dtFechaCuota.DateTime);
                    if (res.GetType() == typeof(DTO_TxResult))
                    {
                        DTO_TxResult txRes = (DTO_TxResult)res;
                        MessageForm msg = new MessageForm(txRes);
                        msg.Show();
                    }
                    else
                    {
                        this._liquidador = (DTO_PlanDePagos)res;

                        if (this._liquidador.VlrDescuento != 0)
                            this.txtVlrDto.Text = this._liquidador.VlrDescuento.ToString();
                        
                        this.gcDocument.DataSource = this._liquidador.ComponentesAll;

                        if (this._liquidador.Cuotas != null && this._liquidador.Cuotas.Count != 0)
                        {
                            this._cuota = this._liquidador.Cuotas[0];
                            this.gvDetail.Columns.Clear();
                            this.AddGridColsDetail();
                            this.gcDetail.DataSource = this._liquidador.Cuotas;
                        }
                        else
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
                        }

                        this.txtVlrAdicional.Text = this._liquidador.VlrAdicional.ToString();
                        this.txtVlrPrestamo.Text = this._liquidador.VlrPrestamo.ToString();
                        this.txtVlrGiro.Text = this._liquidador.VlrGiro.ToString();
                        this.txtVlrCuota.Text = this._cuota.ValorCuota.ToString();
                        this.txtVlrLibranza.Text = this._liquidador.VlrLibranza.ToString();
                        this.txtIntereses.Text = this._liquidador.TasaTotal.ToString();

                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Liquidador.cs", "TBSearch"));
            }
        }

        #endregion Eventos Barra Herramientas

    }
}