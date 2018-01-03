using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using NewAge.DTO.UDT;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Collections;
using DevExpress.XtraGrid.Views.Grid;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de consulta maestro (modal)
    /// </summary>
    public partial class ModalSaldosFinanciera : Form
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();
        private int documentID = AppForms.ModalSaldosCartera;
        private string unboundPrefix = "Unbound_";
        private DTO_ccCreditoPlanPagos cuota = new DTO_ccCreditoPlanPagos();
        private List<DTO_ccSaldosComponentes> componentes = new List<DTO_ccSaldosComponentes>();
        private List<DTO_ccSaldosComponentes> componentesTemp = new List<DTO_ccSaldosComponentes>();

        private bool _isSaldo;
        #endregion

        /// <summary>
        /// Constructor de la grilla de plan pagos 
        /// </summary>
        /// <param name="saldosCartera">DTO que tiene el plan de pagos y los componentes</param>
        public ModalSaldosFinanciera(DTO_InfoCredito infoCartera, bool isSaldo)
        {
            //Inicializa el formulario
            InitializeComponent();

            this._isSaldo = isSaldo;
            this.AddGridPlanPagosCols();
            this.AddGridComponentesCols();

            FormProvider.LoadResources(this, this.documentID);

            if (!isSaldo)
                this.Text = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_Pagos");

            this.LoadGridPlanPagos(infoCartera);
        }

        /// <summary>
        /// Constructor de la grilla de plan pagos 
        /// </summary>
        /// <param name="numDoc">Num doc</param>
        /// <param name="libranza">Num de la libranza</param>
        /// <param name="fechaCorte">Fecha corte</param>
        public ModalSaldosFinanciera(int numDoc, int libranza, DateTime fechaCorte, bool isSaldo)
        {
            //Inicializa el formulario
            InitializeComponent();
            
            this._isSaldo = isSaldo;
            this.AddGridPlanPagosCols();
            this.AddGridComponentesCols();
           
            FormProvider.LoadResources(this, this.documentID);
            if (!isSaldo)
                this.Text = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_Pagos");

            DTO_InfoCredito infoCredito = new DTO_InfoCredito();

            if (isSaldo)
                infoCredito = this._bc.AdministrationModel.GetSaldoCreditoFinanciera(numDoc, fechaCorte, true, true);
            else
                infoCredito = this._bc.AdministrationModel.GetPagosCreditoFinanciera(numDoc, fechaCorte);

            this.LoadGridPlanPagos(infoCredito);
        }

        #region Funciones privadas

        /// <summary>
        /// Agrega las Columna a la grilla de plan pagos
        /// </summary>
        private void AddGridPlanPagosCols()
        {
            try
            {
                //Num Cuota
                GridColumn numCuota = new GridColumn();
                numCuota.FieldName = this.unboundPrefix + "CuotaID";
                numCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_NumCuota");
                numCuota.UnboundType = UnboundColumnType.Integer;
                numCuota.VisibleIndex = 1;
                numCuota.Width = 100;
                numCuota.Visible = true;
                numCuota.OptionsColumn.AllowEdit = false;
                this.gvPlanPagos.Columns.Add(numCuota);

                //Fecha
                GridColumn fecha = new GridColumn();
                fecha.FieldName = this.unboundPrefix + "FechaCuota";
                fecha.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Fecha");
                fecha.UnboundType = UnboundColumnType.DateTime;
                fecha.VisibleIndex = 2;
                fecha.Width = 180;
                fecha.Visible = true;
                fecha.OptionsColumn.AllowEdit = false;
                this.gvPlanPagos.Columns.Add(fecha);

                if (this._isSaldo)
                {
                    //Saldo
                    GridColumn saldo = new GridColumn();
                    saldo.FieldName = this.unboundPrefix + "VlrSaldo";
                    saldo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Saldo");
                    saldo.UnboundType = UnboundColumnType.Integer;
                    saldo.VisibleIndex = 3;
                    saldo.Width = 200;
                    saldo.Visible = true;
                    saldo.OptionsColumn.AllowEdit = false;
                    this.gvPlanPagos.Columns.Add(saldo);

                    //Valor Mora Liquida
                    GridColumn vlrMoraLiq = new GridColumn();
                    vlrMoraLiq.FieldName = this.unboundPrefix + "VlrMoraLiquida";
                    vlrMoraLiq.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrMoraLiquida");
                    vlrMoraLiq.UnboundType = UnboundColumnType.Integer;
                    vlrMoraLiq.VisibleIndex = 4;
                    vlrMoraLiq.Width = 200;
                    vlrMoraLiq.Visible = true;
                    vlrMoraLiq.OptionsColumn.AllowEdit = false;
                    this.gvPlanPagos.Columns.Add(vlrMoraLiq);
                }
                else
                {
                    //Valor inicial cuota
                    GridColumn saldo = new GridColumn();
                    saldo.FieldName = this.unboundPrefix + "VlrCuota";
                    saldo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrCuota");
                    saldo.UnboundType = UnboundColumnType.Integer;
                    saldo.VisibleIndex = 3;
                    saldo.Width = 200;
                    saldo.Visible = true;
                    saldo.OptionsColumn.AllowEdit = false;
                    this.gvPlanPagos.Columns.Add(saldo);

                    //Valor Pagado cuota
                    GridColumn pagado = new GridColumn();
                    pagado.FieldName = this.unboundPrefix + "VlrPagadoCuota";
                    pagado.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrPagado");
                    pagado.UnboundType = UnboundColumnType.Integer;
                    pagado.VisibleIndex = 4;
                    pagado.Width = 200;
                    pagado.Visible = true;
                    pagado.OptionsColumn.AllowEdit = false;
                    this.gvPlanPagos.Columns.Add(pagado);

                }

                this.gvPlanPagos.OptionsView.ColumnAutoWidth = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalSaldosCartera.cs", "AddGridPlanPagosCols"));
            }
        }

        /// <summary>
        /// Agrega las Columna a la grilla de los componentes
        /// </summary>
        private void AddGridComponentesCols()
        {
            try
            {
                //Codigo Componente
                GridColumn comptCarteraID = new GridColumn();
                comptCarteraID.FieldName = this.unboundPrefix + "ComponenteCarteraID";
                comptCarteraID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ComponenteCarteraID");
                comptCarteraID.UnboundType = UnboundColumnType.String;
                comptCarteraID.VisibleIndex = 0;
                comptCarteraID.Width = 30;
                comptCarteraID.Visible = true;
                comptCarteraID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                comptCarteraID.OptionsColumn.AllowEdit = false;
                this.gvComponentes.Columns.Add(comptCarteraID);

                //Descripcion
                GridColumn descripcion = new GridColumn();
                descripcion.FieldName = this.unboundPrefix + "Descriptivo";
                descripcion.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descripcion");
                descripcion.UnboundType = UnboundColumnType.String;
                descripcion.VisibleIndex = 1;
                descripcion.Width = 30;
                descripcion.Visible = true;
                descripcion.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                descripcion.OptionsColumn.AllowEdit = false;
                this.gvComponentes.Columns.Add(descripcion);

                if (this._isSaldo)
                {
                    //Saldo Cuota
                    GridColumn cuotaSaldo = new GridColumn();
                    cuotaSaldo.FieldName = this.unboundPrefix + "CuotaSaldo";
                    cuotaSaldo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CuotaSaldo");
                    cuotaSaldo.UnboundType = UnboundColumnType.Integer;
                    cuotaSaldo.VisibleIndex = 3;
                    cuotaSaldo.Width = 40;
                    cuotaSaldo.Visible = true;
                    cuotaSaldo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                    cuotaSaldo.OptionsColumn.AllowEdit = false;
                    this.gvComponentes.Columns.Add(cuotaSaldo);

                    //Valor Cuota
                    GridColumn cuotaValor = new GridColumn();
                    cuotaValor.FieldName = this.unboundPrefix + "AbonoValor";
                    cuotaValor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_AbonoValor");
                    cuotaValor.UnboundType = UnboundColumnType.Integer;
                    cuotaValor.VisibleIndex = 3;
                    cuotaValor.Width = 40;
                    cuotaValor.Visible = true;
                    cuotaValor.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                    cuotaValor.OptionsColumn.AllowEdit = false;
                    this.gvComponentes.Columns.Add(cuotaValor);
                }
                else
                {
                    //Saldo Cuota
                    GridColumn cuotaSaldo = new GridColumn();
                    cuotaSaldo.FieldName = this.unboundPrefix + "CuotaInicial";
                    cuotaSaldo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CuotaInicial");
                    cuotaSaldo.UnboundType = UnboundColumnType.Integer;
                    cuotaSaldo.VisibleIndex = 3;
                    cuotaSaldo.Width = 40;
                    cuotaSaldo.Visible = true;
                    cuotaSaldo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                    cuotaSaldo.OptionsColumn.AllowEdit = false;
                    this.gvComponentes.Columns.Add(cuotaSaldo);

                    //Valor Cuota
                    GridColumn cuotaValor = new GridColumn();
                    cuotaValor.FieldName = this.unboundPrefix + "AbonoValor";
                    cuotaValor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorPago");
                    cuotaValor.UnboundType = UnboundColumnType.Integer;
                    cuotaValor.VisibleIndex = 3;
                    cuotaValor.Width = 40;
                    cuotaValor.Visible = true;
                    cuotaValor.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                    cuotaValor.OptionsColumn.AllowEdit = false;
                    this.gvComponentes.Columns.Add(cuotaValor);
                }


                this.gvComponentes.OptionsView.ColumnAutoWidth = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalSaldosCartera.cs", "AddGridPlanPagosCols"));
            }
        }

        /// <summary>
        /// Carga los datos del plan de pagos
        /// </summary>
        private void LoadGridPlanPagos(DTO_InfoCredito infoCartera)
        {
            try
            {
                this.componentes = infoCartera.SaldosComponentes;
                this.gcPLanPagos.DataSource = infoCartera.PlanPagos;
                this.gvPlanPagos.PostEditor();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalSaldosCartera.cs", "LoadGridPlanPagos"));
            }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "VlrCuota" || fieldName == "VlrPagado" || fieldName == "VlrSaldo" || fieldName == "VlrPagadoCuota" || 
                fieldName == "VlrMoraLiquida" || fieldName == "CuotaSaldo" || fieldName == "CuotaInicial" || fieldName == "AbonoValor")
                e.RepositoryItem = this.editSpin;
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

        /// <summary>
        /// Evento que carga los componentes segun la cuota del plan de pagos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvPlanPagos_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {

            this.cuota = (DTO_ccCreditoPlanPagos)this.gvPlanPagos.GetRow(e.FocusedRowHandle);
            this.componentesTemp = this.componentes.Where(x => x.CuotaID.Value == this.cuota.CuotaID.Value).ToList();
            this.gcComponentes.DataSource = this.componentesTemp;
        }

        #endregion

    }
}
