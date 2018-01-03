using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using NewAge.Librerias.Project;
using System.Reflection;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using System.Diagnostics;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ModalLiquidacionVacaciones : Form
    {
        #region Variables

        BaseController _bc = BaseController.GetInstance();
        DTO_noEmpleado _empleado = null;
        List<DTO_noLiquidacionPreliminar> _liquidacionesEmpleado = new List<DTO_noLiquidacionPreliminar>();
        DTO_noLiquidacionesDocu liqExist = null;
        int documentID = 0;
        decimal diasTomados = 0;
        string unboundPrefix = "Unbound_";
        bool IsFirtsTime = false;
        DateTime ultimaNominaLiquidada;
        DateTime periodo;
        DateTime fechaDoc;

        #endregion

        public ModalLiquidacionVacaciones(DateTime Periodo, DateTime fechaDoc, DTO_noEmpleado empleado, decimal diasTomados)
        {
            InitializeComponent();
            this._empleado = empleado;
            this.documentID = AppDocuments.Vacaciones;
            this.periodo = Periodo;
            this.fechaDoc = fechaDoc;
            this.diasTomados = diasTomados;
            FormProvider.LoadResources(this, this.documentID);
            this.InitControls();
            this.AddGridCols();
            this.FieldsEnabled(true);
        }

        public ModalLiquidacionVacaciones(DateTime Periodo, DTO_noEmpleado empleado, DTO_noLiquidacionesDocu liqActual, decimal diasTomados,bool onlyConsulta = false)
        {
            InitializeComponent();
            this._empleado = empleado;
            this.liqExist = liqActual;
            this.documentID = AppDocuments.Vacaciones;
            this.periodo = Periodo;
            FormProvider.LoadResources(this, this.documentID);
            this.diasTomados = diasTomados;
            this.InitControls();
            this.AddGridCols();
            this.LoadData(true);
            this.FieldsEnabled(true);
            if(onlyConsulta)
                this.DisableControls();
        }

        #region Funciones Privadas

        /// <summary>
        /// Inicializa las controles
        /// </summary>
        private void InitControls()
        {
            if (this.liqExist == null)
            {
                this.ultimaNominaLiquidada = Convert.ToDateTime(this._bc.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_Periodo));
                this.txtCodigoEmpleado.Text = this._empleado.ID.Value;
                this.txtNombreEmpleado.Text = this._empleado.Descriptivo.Value;
                this.dtFechaIniVD.DateTime = this.periodo;
                this.dtFechaFinVD.DateTime = new DateTime(this.periodo.Year, this.periodo.Month, DateTime.DaysInMonth(this.periodo.Year, this.periodo.Month));
                this.txtTomados.Text = Math.Round(this.diasTomados, 2).ToString();
                var periodoPendiente = this._bc.AdministrationModel.Nomina_GetPeriodoVacaciones(this._empleado.ID.Value, false).FirstOrDefault();
                if (periodoPendiente != null)
                {
                    this.dtFechaIniPPV.DateTime = periodoPendiente.PeriodoInicial.Value.Value;
                    this.dtFechaFinPPV.DateTime = periodoPendiente.PeriodoFinal.Value.Value;
                }
                else
                {
                    this.dtFechaIniPPV.DateTime = this.periodo;
                    this.dtFechaFinPPV.DateTime = new DateTime(this.periodo.Year, this.periodo.Month, DateTime.DaysInMonth(this.periodo.Year, this.periodo.Month));
                }
                this.dtFechaIniPP.DateTime = this.periodo;
                this.dtFechaFinPP.DateTime = new DateTime(this.periodo.Year, this.periodo.Month, DateTime.DaysInMonth(this.periodo.Year, this.periodo.Month));
            }
            else
            {
                //Asigna fechas existentes
                this.ultimaNominaLiquidada = Convert.ToDateTime(this._bc.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_Periodo));
                this.txtCodigoEmpleado.Text = this._empleado.ID.Value;
                this.txtNombreEmpleado.Text = this._empleado.Descriptivo.Value;
                this.dtFechaIniPPV.DateTime = this.liqExist.FechaIni1.Value.Value;
                this.dtFechaFinPPV.DateTime = this.liqExist.FechaFin1.Value.Value;
                this.dtFechaIniVD.DateTime = this.liqExist.FechaIni2.Value.Value;
                this.dtFechaFinVD.DateTime = this.liqExist.FechaFin2.Value.Value;
                //var periodoPendiente = this._bc.AdministrationModel.Nomina_GetPeriodoVacaciones(this._empleado.ID.Value, false).FirstOrDefault();
                //if (periodoPendiente != null)
                //{
                //    this.dtFechaIniPPV.DateTime = periodoPendiente.PeriodoInicial.Value.Value;
                //    this.dtFechaFinPPV.DateTime = periodoPendiente.PeriodoFinal.Value.Value;
                //}
                //else
                //{
                //    this.dtFechaIniPPV.DateTime = this.periodo;
                //    this.dtFechaFinPPV.DateTime = new DateTime(this.periodo.Year, this.periodo.Month, DateTime.DaysInMonth(this.periodo.Year, this.periodo.Month));
                //}
                this.dtFechaIniPP.DateTime = this.liqExist.FechaIni3.Value.Value;
                this.dtFechaFinPP.DateTime = this.liqExist.FechaFin3.Value.Value;
                this.dtFechaIniFRE.DateTime = this.dtFechaFinVD.DateTime.AddDays(1);
                //Campos adicionales
                decimal diasCausados = 0;
                int diasVacaciones = this._bc.AdministrationModel.CalcularDiasVacaciones(
                                                                        this._empleado.ID.Value,
                                                                        this.dtFechaIniVD.DateTime,
                                                                        this.dtFechaFinVD.DateTime,
                                                                        out diasCausados);

                this.txtCausados.Text = Math.Round(diasCausados, 2).ToString();
                this.txtPendientes.Text = Math.Round(diasCausados, 2).ToString();
                this.txtDiasTomados.Text = diasVacaciones.ToString();
                this.txtDiasVacaciones.Text = ((this.dtFechaFinVD.DateTime - this.dtFechaIniVD.DateTime).Days + 1).ToString();
                this.txtTomados.Text = "0";
                this.txtDinero.Text = this.liqExist.Dias2.Value.ToString();
                //this.txtCausados.Text = this.liqExist.Dias3.Value.ToString();             
                //this.txtPendientes.Text = (this.liqExist.Dias3.Value - this.diasTomados).ToString();
                this.txtTotal.Text = (this.diasTomados + this.liqExist.Dias2.Value).ToString();
            }
        }

        /// <summary>
        /// Campos para habilitar o deshabilitar los controles
        /// </summary>
        /// <param name="estado">estado true o false</param>
        private void FieldsEnabled(bool estado)
        {
            this.dtFechaIniVD.Properties.ReadOnly = !estado;
            this.dtFechaFinVD.Properties.ReadOnly = !estado;
            this.txtDiasVacaciones.Properties.ReadOnly = true;
            this.dtFechaIniPPV.Properties.ReadOnly = true;
            this.dtFechaFinPPV.Properties.ReadOnly = true;
            this.dtFechaIniFRE.Properties.ReadOnly = true;
            this.chkInlPrima.Enabled = estado;
            this.chkPagoNomina.Enabled = estado;

            this.txtCausados.Properties.ReadOnly = true;
            this.txtTomados.Properties.ReadOnly = true;
            this.txtPendientes.Properties.ReadOnly = true;
            this.txtDiasTomados.Properties.ReadOnly = true;
            this.txtDinero.Properties.ReadOnly = !estado;
            this.txtTotal.Properties.ReadOnly = true;
            this.txtNominas.Properties.ReadOnly = !estado;
            this.txtResolucion.Properties.ReadOnly = !estado;
            this.txtTotal.Properties.ReadOnly = true;

            this.btnLiquidar.Enabled = estado;
        }

        private void DisableControls()
        {
            this.btnLiquidar.Enabled = false;
            this.dtFechaFinPP.Enabled = false;
            this.dtFechaIniPP.Enabled = false;
            this.dtFechaFinPPV.Enabled = false;
            this.dtFechaIniPPV.Enabled = false;
            this.dtFechaFinVD.Enabled = false;
            this.dtFechaIniVD.Enabled = false;
            this.dtFechaFinPP.Enabled = false;
        }

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Carga la información de las grilla
        /// </summary>
        /// <param name="firstTime">Si es la primera vez que carga la grilla</param>
        /// <param name="refresh">Si debe volver a cargar la data del servidor</param>
        protected void LoadData(bool firstTime)
        {
            try
            {
                if (firstTime)
                {
                    this._liquidacionesEmpleado = this._bc.AdministrationModel.Nomina_LiquidacionPreliminarGetAll(this.documentID, this.periodo, this._empleado);
                    this.gcDetalle.DataSource = this._liquidacionesEmpleado;

                    if (this._liquidacionesEmpleado != null && this._liquidacionesEmpleado.Count > 0)
                    {
                        decimal totalVac = this._liquidacionesEmpleado.Sum(x => x.Valor.Value.Value);
                        this.txtTotalVacaciones.Text = totalVac.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalLiquidacionVacaciones", "LoadData"));
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected void AddGridCols()
        {
            try
            {
                #region Detalle Grilla

                GridColumn conceptoNOID = new GridColumn();
                conceptoNOID.FieldName = this.unboundPrefix + "ConceptoNOID";
                conceptoNOID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ConceptoNOID");
                conceptoNOID.UnboundType = UnboundColumnType.String;
                conceptoNOID.VisibleIndex = 0;
                conceptoNOID.Width = 100;
                conceptoNOID.Visible = true;
                conceptoNOID.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(conceptoNOID);

                GridColumn conceptoNODesc = new GridColumn();
                conceptoNODesc.FieldName = this.unboundPrefix + "ConceptoNODesc";
                conceptoNODesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ConceptoNODesc");
                conceptoNODesc.UnboundType = UnboundColumnType.String;
                conceptoNODesc.VisibleIndex = 0;
                conceptoNODesc.Width = 300;
                conceptoNODesc.Visible = true;
                conceptoNODesc.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(conceptoNODesc);


                GridColumn dias = new GridColumn();
                dias.FieldName = this.unboundPrefix + "Dias";
                dias.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Dias");
                dias.UnboundType = UnboundColumnType.Integer;
                dias.VisibleIndex = 0;
                dias.Width = 50;
                dias.Visible = true;
                dias.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(dias);

                GridColumn Base = new GridColumn();
                Base.FieldName = this.unboundPrefix + "Base";
                Base.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Base");
                Base.UnboundType = UnboundColumnType.Decimal;
                Base.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                Base.AppearanceCell.Options.UseTextOptions = true;
                Base.VisibleIndex = 0;
                Base.Width = 130;
                Base.Visible = true;
                Base.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(Base);

                GridColumn Valor = new GridColumn();
                Valor.FieldName = this.unboundPrefix + "Valor";
                Valor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Valor");
                Valor.UnboundType = UnboundColumnType.Decimal;
                Valor.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                Valor.AppearanceCell.Options.UseTextOptions = true;
                Valor.VisibleIndex = 0;
                Valor.Width = 130;
                Valor.Visible = true;
                Valor.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(Valor);


                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalLiquidacionVacacionesDetalle.cs", "AddGridCols"));
            }
        }

        #endregion

        #region Eventos Grilla Detalle

        /// <summary>
        /// Maneja campos en las grillas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDetalle_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (pi != null)
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32")
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
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32")
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
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32")
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
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32")
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
        /// Evento que define si el valor de la celda se va a mostra en un control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDetalle_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "Valor" || fieldName == "Base")
            {
                e.RepositoryItem = this.editValue;
            }
        }

        #endregion

        #region Eventos Controles

        /// <summary>
        /// Valida Fecha inicial vacaciones disfrutadas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtFechaIniVD_Leave(object sender, EventArgs e)
        {
            try
            {
                if (IsFirtsTime)
                {
                    if (this.dtFechaIniVD.DateTime > this.dtFechaFinVD.DateTime)
                    {
                        this.dtFechaIniVD.DateTime = this.periodo;
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.No_DateStartNotValid));
                    }
                    else
                    {
                        decimal diasCausados = 0;
                        int diasVacaciones = this._bc.AdministrationModel.CalcularDiasVacaciones(
                                                                                this._empleado.ID.Value,
                                                                                this.dtFechaIniVD.DateTime,
                                                                                this.dtFechaFinVD.DateTime,
                                                                                out diasCausados);
                        this.txtCausados.Text = Math.Round(diasCausados, 2).ToString();
                        this.txtPendientes.Text = Math.Round(diasCausados - diasTomados, 2).ToString();
                        this.txtDiasTomados.Text = diasVacaciones.ToString();
                        this.txtDiasVacaciones.Text = ((this.dtFechaFinVD.DateTime - this.dtFechaIniVD.DateTime).Days + 1).ToString();
                        this.dtFechaIniFRE.DateTime = this.dtFechaFinVD.DateTime.AddDays(1);
                        this.dtFechaIniPP.DateTime = this.periodo;
                        this.dtFechaFinPP.DateTime = new DateTime(this.periodo.Year, this.periodo.Month, 30);
                        this.txtTotal.Text = (
                            Convert.ToInt32(this.txtDiasTomados.Text) +
                            (!string.IsNullOrEmpty(this.txtDinero.Text) ? Convert.ToInt32(this.txtDinero.Text) : 0)).ToString();
                    }
                }
                this.IsFirtsTime = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalLiquidacionVacaciones", "dtFechaIniVD_Leave"));
            }
        }

        /// <summary>
        /// Valida Fecha final vacaciones disfrutadas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtFechaFinVD_Leave(object sender, EventArgs e)
        {
            try
            {
                if (IsFirtsTime)
                {
                    if (this.dtFechaFinVD.DateTime < this.dtFechaIniVD.DateTime)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.No_DateStartNotValid));
                    }
                    else
                    {
                        decimal diasCausados = 0;
                        int diasVacaciones = this._bc.AdministrationModel.CalcularDiasVacaciones(
                                                                                this._empleado.ID.Value,
                                                                                this.dtFechaIniVD.DateTime,
                                                                                this.dtFechaFinVD.DateTime,
                                                                                out diasCausados);

                        this.txtCausados.Text = Math.Round(diasCausados, 2).ToString();
                        this.txtPendientes.Text = Math.Round(diasCausados - diasTomados, 2).ToString();
                        this.txtDiasTomados.Text = diasVacaciones.ToString();
                        this.txtDiasVacaciones.Text = ((this.dtFechaFinVD.DateTime - this.dtFechaIniVD.DateTime).Days + 1).ToString();
                        this.dtFechaIniFRE.DateTime = this.dtFechaFinVD.DateTime.AddDays(1);
                        this.dtFechaIniPP.DateTime = this.periodo;
                        this.dtFechaFinPP.DateTime = new DateTime(this.dtFechaFinVD.DateTime.Year, this.dtFechaFinVD.DateTime.Month, this.dtFechaFinVD.DateTime.Day);
                        this.txtTotal.Text = (
                            Convert.ToInt32(this.txtDiasTomados.Text) +
                            (!string.IsNullOrEmpty(this.txtDinero.Text) ? Convert.ToInt32(this.txtDinero.Text) : 0)).ToString();

                    }
                }
                this.IsFirtsTime = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalLiquidacionVacaciones", "dtFechaFinVD_Leave"));
            }
        }

        /// <summary>
        /// Valida Fecha inicial periodo de pago
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtFechaIniPP_Leave(object sender, EventArgs e)
        {
            try
            {
                if (IsFirtsTime)
                {
                    if (this.dtFechaIniPP.DateTime > this.dtFechaFinPP.DateTime)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.No_DateStartNotValid));
                    }
                    if (this.dtFechaIniPP.DateTime > this.dtFechaIniVD.DateTime)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, "La fecha inicial no puede ser superior a la fecha de Vacaciones Disfrutadas"));
                    }
                }
                this.IsFirtsTime = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalLiquidacionVacaciones", "dtFechaIniPP_Leave"));
            }
        }

        /// <summary>
        /// Valida Fecha final periodo de pago
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtFechaFinPP_Leave(object sender, EventArgs e)
        {
            try
            {
                if (IsFirtsTime)
                {
                    if (this.dtFechaFinPP.DateTime < this.dtFechaIniPP.DateTime)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.No_DateStartNotValid));
                    }
                    if (this.dtFechaFinPP.DateTime < this.dtFechaFinVD.DateTime)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, "La fecha final no puede ser inferior a la fecha de Vacaciones Disfrutadas"));
                    }
                }
                this.IsFirtsTime = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalLiquidacionVacaciones", "dtFechaIniPP_Leave"));
            }
        }

        /// <summary>
        /// Ejecuta evento boton liquidar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLiquidar_Click(object sender, EventArgs e)
        {
            try
            {
                List<DTO_noEmpleado> lempleados = new List<DTO_noEmpleado>();
                lempleados.Add(this._empleado);
                var results = this._bc.AdministrationModel.LiquidarVacaciones(this.periodo,
                                                                              DateTime.Now,
                                                                              lempleados,
                                                                              this.dtFechaIniVD.DateTime,
                                                                              this.dtFechaFinVD.DateTime,
                                                                              Convert.ToInt32(this.txtDiasTomados.Text),
                                                                              Convert.ToInt32(this.txtDinero.Text),
                                                                              this.chkPagoNomina.Checked,
                                                                              false,
                                                                              this.txtResolucion.Text,
                                                                              this.dtFechaFinPP.DateTime,
                                                                              this.dtFechaIniPP.DateTime,
                                                                              this.dtFechaIniPPV.DateTime,
                                                                              this.dtFechaFinPPV.DateTime
                                                                              );

                ////Recarga la grilla de novedades
                this.LoadData(true);

                List<DTO_TxResult> resultsNOK = new List<DTO_TxResult>();
                foreach (DTO_TxResult result in results)
                {
                    if (result.Result == ResultValue.NOK)
                    {
                        resultsNOK.Add(result);
                    }
                }

                MessageForm frm = new MessageForm(resultsNOK);
                frm.ShowDialog();

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalLiquidacionVacaciones", "btnLiquidar_Click"));
            }
        }

        /// <summary>
        /// Evento de salida texto vacaciones en dinero
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDinero_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(this.txtDiasTomados.Text) && !string.IsNullOrEmpty(this.txtDinero.Text))
                {
                    this.txtTotal.Text = (Convert.ToInt32(this.txtDiasTomados.Text) + Convert.ToInt32(this.txtDinero.Text)).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalLiquidacionVacaciones", "txtDinero_Leave"));
            }
        }

        /// <summary>
        /// Evento de salida texto vacaciones tomadas en dias 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDiasTomados_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(this.txtDiasTomados.Text) && !string.IsNullOrEmpty(this.txtDinero.Text))
                {
                    this.txtTotal.Text = (Convert.ToInt32(this.txtDiasTomados.Text) + Convert.ToInt32(this.txtDinero.Text)).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalLiquidacionVacaciones", "txtDiasTomados_Leave"));
            }
        }

        /// <summary>
        /// Evento q envia a imprimir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImprimir_Click(object sender, EventArgs e)
        {
            if (this.liqExist != null)
            {
                this._bc.AdministrationModel.noPrintVacaciones(this.documentID, _empleado, this.liqExist, _liquidacionesEmpleado, false);

                string fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, this.liqExist.NumeroDoc.Value, null, string.Empty);
                Process.Start(fileURl);
            }
               
        }
        
        /// <summary>
        /// FInaliza la pantalla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFinish_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

    }
}
