using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using DevExpress.XtraCharts;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class EstadisticasMensualesContabilidad : FormWithToolbar
    {
        #region Variables

        BaseController _bc = BaseController.GetInstance();

        private List<DTO_coCierreMes> cierres;
        private List<DTO_coCierreMes> cierresFiltro;
        private FormTypes _frmType = FormTypes.DocumentAprob;
        private string _frmName;
        private ModulesPrefix _frmModule;
        private int _documentID;

        private int año;
        private string tipoDato;

        #endregion

        public EstadisticasMensualesContabilidad()
        {
            try
            {
                this.InitializeComponent();

                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());
                FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);

                // Trae la fuente de datos y los filtra
                this.GetData();

                // Especifica las variables para la gráfica.
                this.chartCierre.SeriesDataMember = "Member";
                this.chartCierre.SeriesTemplate.ArgumentDataMember = "Argument";
                this.chartCierre.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "Value" });

                AreaSeriesView view = new AreaSeriesView();
                view.MarkerOptions.Size = 7;
                this.chartCierre.SeriesTemplate.View = view;

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadisticasMensualesContabilidad.cs", "DashBoard"));
            }
        }

        #region Funciones privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            try
            {
                this._documentID = AppQueries.EstadisticasMensualesContabilidad;
                this._frmModule = ModulesPrefix.co;

                //Carga la informacion de la maestras
                this._bc.InitMasterUC(this.masterLibro, AppMasters.coBalanceTipo, true, true, true, false);
                this._bc.InitMasterUC(this.masterCuenta, AppMasters.coPlanCuenta, true, true, true, false);
                this._bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true, true, true, false);
                this._bc.InitMasterUC(this.masterCentroCosto, AppMasters.coCentroCosto, true, true, true, false);
                this._bc.InitMasterUC(this.masterConceptoCargo, AppMasters.coConceptoCargo, true, true, true, false);

                // Carga el tipo de cierre
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("1", this._bc.GetResource(LanguageTypes.Tables,DictionaryTables.Doc_CurrencyLocal));
                dic.Add("2", this._bc.GetResource(LanguageTypes.Tables,DictionaryTables.Doc_CurrencyForeign));

                this.cmbTipo.Properties.DataSource = dic;
                this.cmbTipo.EditValue = "1";

                // Año 
                string periodoStr = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_Periodo);
                DateTime periodo = Convert.ToDateTime(periodoStr);
                this.año = periodo.Year;

                List<int> años = new List<int>();
                for (int i = 0; i < 5; ++i)
                {
                    int año = periodo.Year - i;
                    this.cmbAño.Items.Add(año);
                }
                this.cmbAño.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadisticasMensualesContabilidad.cs", "SetInitParameters"));
            }
        }

        /// <summary>
        /// Funcion temporal para traer los datos 
        /// </summary>
        private void GetData()
        {
            try
            {
                this.cierres = _bc.AdministrationModel.coCierreMes_GetAll(Convert.ToInt16(this.año));
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadisticasMensualesContabilidad.cs", "GetData"));
            }
        }

        /// <summary>
        /// Carga la información de la grilla superior derecha
        /// </summary>
        /// <param name="mes">Día de consulta</param>
        private void AddDetailChart(int mes)
        {
            try
            {
                this.chartDetail.Series.Clear();
                DataTable tableDetail = this.GetDetailTable(mes);

                if (tableDetail.Rows.Count > 10)
                {
                    #region Barra

                    // Crea las series
                    Series serieBarra = new Series("serieBarra", ViewType.Bar);
                    serieBarra.DataSource = tableDetail;

                    //Asigna la fuente de datos
                    this.chartDetail.Series.Add(serieBarra);

                    // Especifica los armuentos y los valores
                    serieBarra.ArgumentDataMember = "Argument";
                    serieBarra.ValueScaleType = ScaleType.Numerical;
                    serieBarra.ValueDataMembers.AddRange(new string[] { "Value" });

                    XYDiagram diagram = (XYDiagram)this.chartDetail.Diagram;
                    if (this.cmbTipo.EditValue == "1")
                    {
                        this.chartDetail.SeriesTemplate.LegendPointOptions.ValueNumericOptions.Format = NumericFormat.Number;

                        diagram.AxisY.NumericOptions.Format = NumericFormat.Number;
                        diagram.AxisY.GridSpacingAuto = false;
                        diagram.AxisY.GridSpacing = 1;
                    }
                    else
                    {
                        this.chartDetail.SeriesTemplate.LegendPointOptions.ValueNumericOptions.Format = NumericFormat.Currency;

                        diagram.AxisY.NumericOptions.Format = NumericFormat.Currency;
                        diagram.AxisY.GridSpacingAuto = true;

                        serieBarra.CrosshairLabelPattern = "{A} : {V:C}";
                    }

                    ((SideBySideBarSeriesView)serieBarra.View).ColorEach = true;
                    #endregion
                }
                else
                {
                    #region Pie

                    Series seriePie = new Series("seriePie", ViewType.Pie);
                    Series seriePieVal = new Series("seriePieVal", ViewType.Pie);
                    foreach (DataRow row in tableDetail.Rows)
                    {
                        seriePie.Points.Add(new SeriesPoint(row[0], row[1]));
                        seriePieVal.Points.Add(new SeriesPoint(row[0], row[1]));
                    }

                    this.chartDetail.Series.Add(seriePie);
                    this.chartDetail.Series.Add(seriePieVal);

                    // Porcentaje
                    seriePie.Label.PointOptions.PointView = PointView.ArgumentAndValues;
                    seriePie.Label.PointOptions.ValueNumericOptions.Format = NumericFormat.Percent;
                    seriePie.Label.PointOptions.ValueNumericOptions.Precision = 0;

                    // Valores
                    seriePieVal.Label.PointOptions.PointView = PointView.ArgumentAndValues;
                    seriePieVal.Label.PointOptions.ValueNumericOptions.Format = this.cmbTipo.EditValue == "1" ? NumericFormat.Number : NumericFormat.Currency;
                    ((PiePointOptions)seriePieVal.Label.PointOptions).PercentOptions.ValueAsPercent = false;
                    seriePieVal.Label.PointOptions.ValueNumericOptions.Precision = 0;

                    // Detect overlapping of series labels.
                    ((PieSeriesLabel)seriePie.Label).ResolveOverlappingMode = ResolveOverlappingMode.Default;
                    ((PieSeriesLabel)seriePieVal.Label).ResolveOverlappingMode = ResolveOverlappingMode.Default;

                    #endregion
                }

                #region Carga el título

                ChartTitle titleBar = new ChartTitle();
                switch (this.rbDetails.SelectedIndex)
                {
                    #region Línea de crédito
                    case 0:
                        titleBar.Text += this.masterLibro.LabelRsx;
                        break;
                    #endregion
                    #region Asesor
                    case 1:
                        titleBar.Text += this.masterCuenta.LabelRsx;
                        break;
                    #endregion
                    #region Centro de pago
                    case 2:
                        titleBar.Text += this.masterProyecto.LabelRsx;
                        break;
                    #endregion
                    #region Zona
                    case 3:
                        titleBar.Text += this.masterCentroCosto.LabelRsx;
                        break;
                    #endregion
                    #region Comprador
                    case 4:
                        titleBar.Text += this.masterConceptoCargo.LabelRsx;
                        break;
                    #endregion
                }

                this.chartDetail.Titles.Clear();
                this.chartDetail.Titles.Add(titleBar);

                #endregion

                this.chartDetail.Legend.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadisticasMensualesContabilidad.cs", "CreateDetailsChart"));
            }
        }

        /// <summary>
        /// Crea la tabla de datos para representar las series
        /// </summary>
        /// <param name="cierres"></param>
        /// <param name="centroPago"></param>
        /// <param name="asesor"></param>
        /// <returns></returns>
        private DataTable GetChartTable()
        {
            // Create an empty table.
            DataTable table = new DataTable("TableChart");
            table.Columns.Add("Member");
            table.Columns.Add("Argument");
            table.Columns.Add("Value", typeof(Decimal));

            try
            {
                if (this.cierres.Count > 0)
                {
                    #region Info de totales

                    for (int i = 1; i <= 12; i++)
                    {
                        DataRow row = row = table.NewRow();
                        row["Member"] = "Total";
                        row["Argument"] = i.ToString();

                        if (Convert.ToInt16(this.cmbTipo.EditValue) == 1)
                        {
                            #region carga el valor en moneda Local
                            switch (i)
                            {
                                case 1:
                                    row["Value"] = this.cierres.Sum(x => x.LocalDB01.Value - x.LocalCR01.Value);
                                    break;
                                case 2:
                                    row["Value"] = this.cierres.Sum(x => x.LocalDB02.Value - x.LocalCR02.Value);
                                    break;
                                case 3:
                                    row["Value"] = this.cierres.Sum(x => x.LocalDB03.Value - x.LocalCR03.Value);
                                    break;
                                case 4:
                                    row["Value"] = this.cierres.Sum(x => x.LocalDB04.Value - x.LocalCR04.Value);
                                    break;
                                case 5:
                                    row["Value"] = this.cierres.Sum(x => x.LocalDB05.Value - x.LocalCR05.Value);
                                    break;
                                case 6:
                                    row["Value"] = this.cierres.Sum(x => x.LocalDB06.Value - x.LocalCR06.Value);
                                    break;
                                case 7:
                                    row["Value"] = this.cierres.Sum(x => x.LocalDB07.Value - x.LocalCR07.Value);
                                    break;
                                case 8:
                                    row["Value"] = this.cierres.Sum(x => x.LocalDB08.Value - x.LocalCR08.Value);
                                    break;
                                case 9:
                                    row["Value"] = this.cierres.Sum(x => x.LocalDB09.Value - x.LocalCR09.Value);
                                    break;
                                case 10:
                                    row["Value"] = this.cierres.Sum(x => x.LocalDB10.Value - x.LocalCR10.Value);
                                    break;
                                case 11:
                                    row["Value"] = this.cierres.Sum(x => x.LocalDB11.Value - x.LocalCR11.Value);
                                    break;
                                case 12:
                                    row["Value"] = this.cierres.Sum(x => x.LocalDB12.Value - x.LocalCR12.Value);
                                    break;
                            }

                            #endregion
                        }
                        else
                        {
                            #region carga el valor en moneda Extranjera
                            switch (i)
                            {
                                case 1:
                                    row["Value"] = this.cierres.Sum(x => x.ExtraDB01.Value - x.ExtraCR01.Value);
                                    break;
                                case 2:
                                    row["Value"] = this.cierres.Sum(x => x.ExtraDB02.Value - x.ExtraCR02.Value);
                                    break;
                                case 3:
                                    row["Value"] = this.cierres.Sum(x => x.ExtraDB03.Value - x.ExtraCR03.Value);
                                    break;
                                case 4:
                                    row["Value"] = this.cierres.Sum(x => x.ExtraDB04.Value - x.ExtraCR04.Value);
                                    break;
                                case 5:
                                    row["Value"] = this.cierres.Sum(x => x.ExtraDB05.Value - x.ExtraCR05.Value);
                                    break;
                                case 6:
                                    row["Value"] = this.cierres.Sum(x => x.ExtraDB06.Value - x.ExtraCR06.Value);
                                    break;
                                case 7:
                                    row["Value"] = this.cierres.Sum(x => x.ExtraDB07.Value - x.ExtraCR07.Value);
                                    break;
                                case 8:
                                    row["Value"] = this.cierres.Sum(x => x.ExtraDB08.Value - x.ExtraCR08.Value);
                                    break;
                                case 9:
                                    row["Value"] = this.cierres.Sum(x => x.ExtraDB09.Value - x.ExtraCR09.Value);
                                    break;
                                case 10:
                                    row["Value"] = this.cierres.Sum(x => x.ExtraDB10.Value - x.ExtraCR10.Value);
                                    break;
                                case 11:
                                    row["Value"] = this.cierres.Sum(x => x.ExtraDB11.Value - x.ExtraCR11.Value);
                                    break;
                                case 12:
                                    row["Value"] = this.cierres.Sum(x => x.ExtraDB12.Value - x.ExtraCR12.Value);
                                    break;
                            }

                            #endregion
                        }

                        table.Rows.Add(row);
                    }
                    #endregion
                    #region Filtro para Libro
                    if (this.masterLibro.ValidID)
                    {
                        for (int i = 1; i <= 12; i++)
                        {
                            DataRow row = row = table.NewRow();
                            row["Member"] = this.masterLibro.LabelRsx + ": " + this.masterLibro.Value;
                            row["Argument"] = i.ToString();

                            if (Convert.ToInt16(this.cmbTipo.EditValue) == 1)
                            {
                                #region carga el valor Moneda Local
                                switch (i)
                                {
                                    case 1:
                                        row["Value"] = this.cierres.Where(c => c.BalanceTipoID.Value == this.masterLibro.Value).Sum(x => x.LocalDB01.Value - x.LocalCR01.Value);
                                        break;
                                    case 2:
                                        row["Value"] = this.cierres.Where(c => c.BalanceTipoID.Value == this.masterLibro.Value).Sum(x => x.LocalDB02.Value - x.LocalCR02.Value);
                                        break;
                                    case 3:
                                        row["Value"] = this.cierres.Where(c => c.BalanceTipoID.Value == this.masterLibro.Value).Sum(x => x.LocalDB03.Value - x.LocalCR03.Value);
                                        break;
                                    case 4:
                                        row["Value"] = this.cierres.Where(c => c.BalanceTipoID.Value == this.masterLibro.Value).Sum(x => x.LocalDB04.Value - x.LocalCR04.Value);
                                        break;
                                    case 5:
                                        row["Value"] = this.cierres.Where(c => c.BalanceTipoID.Value == this.masterLibro.Value).Sum(x => x.LocalDB05.Value - x.LocalCR05.Value);
                                        break;
                                    case 6:
                                        row["Value"] = this.cierres.Where(c => c.BalanceTipoID.Value == this.masterLibro.Value).Sum(x => x.LocalDB06.Value - x.LocalCR06.Value);
                                        break;
                                    case 7:
                                        row["Value"] = this.cierres.Where(c => c.BalanceTipoID.Value == this.masterLibro.Value).Sum(x => x.LocalDB07.Value - x.LocalCR07.Value);
                                        break;
                                    case 8:
                                        row["Value"] = this.cierres.Where(c => c.BalanceTipoID.Value == this.masterLibro.Value).Sum(x => x.LocalDB08.Value - x.LocalCR08.Value);
                                        break;
                                    case 9:
                                        row["Value"] = this.cierres.Where(c => c.BalanceTipoID.Value == this.masterLibro.Value).Sum(x => x.LocalDB09.Value - x.LocalCR09.Value);
                                        break;
                                    case 10:
                                        row["Value"] = this.cierres.Where(c => c.BalanceTipoID.Value == this.masterLibro.Value).Sum(x => x.LocalDB10.Value - x.LocalCR10.Value);
                                        break;
                                    case 11:
                                        row["Value"] = this.cierres.Where(c => c.BalanceTipoID.Value == this.masterLibro.Value).Sum(x => x.LocalDB11.Value - x.LocalCR11.Value);
                                        break;
                                    case 12:
                                        row["Value"] = this.cierres.Where(c => c.BalanceTipoID.Value == this.masterLibro.Value).Sum(x => x.LocalDB12.Value - x.LocalCR12.Value);
                                        break;
                                }

                                #endregion
                            }
                            else
                            {
                                #region carga el valor Moneda Local
                                switch (i)
                                {
                                    case 1:
                                        row["Value"] = this.cierres.Where(c => c.BalanceTipoID.Value == this.masterLibro.Value).Sum(x => x.ExtraDB01.Value - x.ExtraCR01.Value);
                                        break;
                                    case 2:
                                        row["Value"] = this.cierres.Where(c => c.BalanceTipoID.Value == this.masterLibro.Value).Sum(x => x.ExtraDB02.Value - x.ExtraCR02.Value);
                                        break;
                                    case 3:
                                        row["Value"] = this.cierres.Where(c => c.BalanceTipoID.Value == this.masterLibro.Value).Sum(x => x.ExtraDB03.Value - x.ExtraCR03.Value);
                                        break;
                                    case 4:
                                        row["Value"] = this.cierres.Where(c => c.BalanceTipoID.Value == this.masterLibro.Value).Sum(x => x.ExtraDB04.Value - x.ExtraCR04.Value);
                                        break;
                                    case 5:
                                        row["Value"] = this.cierres.Where(c => c.BalanceTipoID.Value == this.masterLibro.Value).Sum(x => x.ExtraDB05.Value - x.ExtraCR05.Value);
                                        break;
                                    case 6:
                                        row["Value"] = this.cierres.Where(c => c.BalanceTipoID.Value == this.masterLibro.Value).Sum(x => x.ExtraDB06.Value - x.ExtraCR06.Value);
                                        break;
                                    case 7:
                                        row["Value"] = this.cierres.Where(c => c.BalanceTipoID.Value == this.masterLibro.Value).Sum(x => x.ExtraDB07.Value - x.ExtraCR07.Value);
                                        break;
                                    case 8:
                                        row["Value"] = this.cierres.Where(c => c.BalanceTipoID.Value == this.masterLibro.Value).Sum(x => x.ExtraDB08.Value - x.ExtraCR08.Value);
                                        break;
                                    case 9:
                                        row["Value"] = this.cierres.Where(c => c.BalanceTipoID.Value == this.masterLibro.Value).Sum(x => x.ExtraDB09.Value - x.ExtraCR09.Value);
                                        break;
                                    case 10:
                                        row["Value"] = this.cierres.Where(c => c.BalanceTipoID.Value == this.masterLibro.Value).Sum(x => x.ExtraDB10.Value - x.ExtraCR10.Value);
                                        break;
                                    case 11:
                                        row["Value"] = this.cierres.Where(c => c.BalanceTipoID.Value == this.masterLibro.Value).Sum(x => x.ExtraDB11.Value - x.ExtraCR11.Value);
                                        break;
                                    case 12:
                                        row["Value"] = this.cierres.Where(c => c.BalanceTipoID.Value == this.masterLibro.Value).Sum(x => x.ExtraDB12.Value - x.ExtraCR12.Value);
                                        break;
                                }

                                #endregion
                            }

                            table.Rows.Add(row);
                        }
                    }
                    #endregion
                    #region Filtro para Cuenta
                    if (this.masterCuenta.ValidID)
                    {
                        for (int i = 1; i <= 12; i++)
                        {
                            DataRow row = row = table.NewRow();
                            row["Member"] = this.masterCuenta.LabelRsx + ": " + this.masterCuenta.Value;
                            row["Argument"] = i.ToString();

                            if (Convert.ToInt16(this.cmbTipo.EditValue) == 1)
                            {
                                #region carga el valor Moneda Local
                                switch (i)
                                {
                                    case 1:
                                        row["Value"] = this.cierres.Where(c => c.CuentaID.Value == this.masterCuenta.Value).Sum(x => x.LocalDB01.Value - x.LocalCR01.Value);
                                        break;
                                    case 2:
                                        row["Value"] = this.cierres.Where(c => c.CuentaID.Value == this.masterCuenta.Value).Sum(x => x.LocalDB02.Value - x.LocalCR02.Value);
                                        break;
                                    case 3:
                                        row["Value"] = this.cierres.Where(c => c.CuentaID.Value == this.masterCuenta.Value).Sum(x => x.LocalDB03.Value - x.LocalCR03.Value);
                                        break;
                                    case 4:
                                        row["Value"] = this.cierres.Where(c => c.CuentaID.Value == this.masterCuenta.Value).Sum(x => x.LocalDB04.Value - x.LocalCR04.Value);
                                        break;
                                    case 5:
                                        row["Value"] = this.cierres.Where(c => c.CuentaID.Value == this.masterCuenta.Value).Sum(x => x.LocalDB05.Value - x.LocalCR05.Value);
                                        break;
                                    case 6:
                                        row["Value"] = this.cierres.Where(c => c.CuentaID.Value == this.masterCuenta.Value).Sum(x => x.LocalDB06.Value - x.LocalCR06.Value);
                                        break;
                                    case 7:
                                        row["Value"] = this.cierres.Where(c => c.CuentaID.Value == this.masterCuenta.Value).Sum(x => x.LocalDB07.Value - x.LocalCR07.Value);
                                        break;
                                    case 8:
                                        row["Value"] = this.cierres.Where(c => c.CuentaID.Value == this.masterCuenta.Value).Sum(x => x.LocalDB08.Value - x.LocalCR08.Value);
                                        break;
                                    case 9:
                                        row["Value"] = this.cierres.Where(c => c.CuentaID.Value == this.masterCuenta.Value).Sum(x => x.LocalDB09.Value - x.LocalCR09.Value);
                                        break;
                                    case 10:
                                        row["Value"] = this.cierres.Where(c => c.CuentaID.Value == this.masterCuenta.Value).Sum(x => x.LocalDB10.Value - x.LocalCR10.Value);
                                        break;
                                    case 11:
                                        row["Value"] = this.cierres.Where(c => c.CuentaID.Value == this.masterCuenta.Value).Sum(x => x.LocalDB11.Value - x.LocalCR11.Value);
                                        break;
                                    case 12:
                                        row["Value"] = this.cierres.Where(c => c.CuentaID.Value == this.masterCuenta.Value).Sum(x => x.LocalDB12.Value - x.LocalCR12.Value);
                                        break;
                                }

                                #endregion
                            }
                            else
                            {
                                #region carga el valor Moneda Local
                                switch (i)
                                {
                                    case 1:
                                        row["Value"] = this.cierres.Where(c => c.CuentaID.Value == this.masterCuenta.Value).Sum(x => x.ExtraDB01.Value - x.ExtraCR01.Value);
                                        break;
                                    case 2:
                                        row["Value"] = this.cierres.Where(c => c.CuentaID.Value == this.masterCuenta.Value).Sum(x => x.ExtraDB02.Value - x.ExtraCR02.Value);
                                        break;
                                    case 3:
                                        row["Value"] = this.cierres.Where(c => c.CuentaID.Value == this.masterCuenta.Value).Sum(x => x.ExtraDB03.Value - x.ExtraCR03.Value);
                                        break;
                                    case 4:
                                        row["Value"] = this.cierres.Where(c => c.CuentaID.Value == this.masterCuenta.Value).Sum(x => x.ExtraDB04.Value - x.ExtraCR04.Value);
                                        break;
                                    case 5:
                                        row["Value"] = this.cierres.Where(c => c.CuentaID.Value == this.masterCuenta.Value).Sum(x => x.ExtraDB05.Value - x.ExtraCR05.Value);
                                        break;
                                    case 6:
                                        row["Value"] = this.cierres.Where(c => c.CuentaID.Value == this.masterCuenta.Value).Sum(x => x.ExtraDB06.Value - x.ExtraCR06.Value);
                                        break;
                                    case 7:
                                        row["Value"] = this.cierres.Where(c => c.CuentaID.Value == this.masterCuenta.Value).Sum(x => x.ExtraDB07.Value - x.ExtraCR07.Value);
                                        break;
                                    case 8:
                                        row["Value"] = this.cierres.Where(c => c.CuentaID.Value == this.masterCuenta.Value).Sum(x => x.ExtraDB08.Value - x.ExtraCR08.Value);
                                        break;
                                    case 9:
                                        row["Value"] = this.cierres.Where(c => c.CuentaID.Value == this.masterCuenta.Value).Sum(x => x.ExtraDB09.Value - x.ExtraCR09.Value);
                                        break;
                                    case 10:
                                        row["Value"] = this.cierres.Where(c => c.CuentaID.Value == this.masterCuenta.Value).Sum(x => x.ExtraDB10.Value - x.ExtraCR10.Value);
                                        break;
                                    case 11:
                                        row["Value"] = this.cierres.Where(c => c.CuentaID.Value == this.masterCuenta.Value).Sum(x => x.ExtraDB11.Value - x.ExtraCR11.Value);
                                        break;
                                    case 12:
                                        row["Value"] = this.cierres.Where(c => c.CuentaID.Value == this.masterCuenta.Value).Sum(x => x.ExtraDB12.Value - x.ExtraCR12.Value);
                                        break;
                                }

                                #endregion
                            }

                            table.Rows.Add(row);
                        }
                    }
                    #endregion
                    #region Filtro para Proyecto
                    if (this.masterProyecto.ValidID)
                    {
                        for (int i = 1; i <= 12; i++)
                        {
                            DataRow row = row = table.NewRow();
                            row["Member"] = this.masterProyecto.LabelRsx + ": " + this.masterProyecto.Value;
                            row["Argument"] = i.ToString();

                            if (Convert.ToInt16(this.cmbTipo.EditValue) == 1)
                            {
                                #region carga el valor Moneda Local
                                switch (i)
                                {
                                    case 1:
                                        row["Value"] = this.cierres.Where(c => c.ProyectoID.Value == this.masterProyecto.Value).Sum(x => x.LocalDB01.Value - x.LocalCR01.Value);
                                        break;
                                    case 2:
                                        row["Value"] = this.cierres.Where(c => c.ProyectoID.Value == this.masterProyecto.Value).Sum(x => x.LocalDB02.Value - x.LocalCR02.Value);
                                        break;
                                    case 3:
                                        row["Value"] = this.cierres.Where(c => c.ProyectoID.Value == this.masterProyecto.Value).Sum(x => x.LocalDB03.Value - x.LocalCR03.Value);
                                        break;
                                    case 4:
                                        row["Value"] = this.cierres.Where(c => c.ProyectoID.Value == this.masterProyecto.Value).Sum(x => x.LocalDB04.Value - x.LocalCR04.Value);
                                        break;
                                    case 5:
                                        row["Value"] = this.cierres.Where(c => c.ProyectoID.Value == this.masterProyecto.Value).Sum(x => x.LocalDB05.Value - x.LocalCR05.Value);
                                        break;
                                    case 6:
                                        row["Value"] = this.cierres.Where(c => c.ProyectoID.Value == this.masterProyecto.Value).Sum(x => x.LocalDB06.Value - x.LocalCR06.Value);
                                        break;
                                    case 7:
                                        row["Value"] = this.cierres.Where(c => c.ProyectoID.Value == this.masterProyecto.Value).Sum(x => x.LocalDB07.Value - x.LocalCR07.Value);
                                        break;
                                    case 8:
                                        row["Value"] = this.cierres.Where(c => c.ProyectoID.Value == this.masterProyecto.Value).Sum(x => x.LocalDB08.Value - x.LocalCR08.Value);
                                        break;
                                    case 9:
                                        row["Value"] = this.cierres.Where(c => c.ProyectoID.Value == this.masterProyecto.Value).Sum(x => x.LocalDB09.Value - x.LocalCR09.Value);
                                        break;
                                    case 10:
                                        row["Value"] = this.cierres.Where(c => c.ProyectoID.Value == this.masterProyecto.Value).Sum(x => x.LocalDB10.Value - x.LocalCR10.Value);
                                        break;
                                    case 11:
                                        row["Value"] = this.cierres.Where(c => c.ProyectoID.Value == this.masterProyecto.Value).Sum(x => x.LocalDB11.Value - x.LocalCR11.Value);
                                        break;
                                    case 12:
                                        row["Value"] = this.cierres.Where(c => c.ProyectoID.Value == this.masterProyecto.Value).Sum(x => x.LocalDB12.Value - x.LocalCR12.Value);
                                        break;
                                }

                                #endregion
                            }
                            else
                            {
                                #region carga el valor Moneda Extranjera
                                switch (i)
                                {
                                    case 1:
                                        row["Value"] = this.cierres.Where(c => c.ProyectoID.Value == this.masterProyecto.Value).Sum(x => x.ExtraDB01.Value - x.ExtraCR01.Value);
                                        break;
                                    case 2:
                                        row["Value"] = this.cierres.Where(c => c.ProyectoID.Value == this.masterProyecto.Value).Sum(x => x.ExtraDB02.Value - x.ExtraCR02.Value);
                                        break;
                                    case 3:
                                        row["Value"] = this.cierres.Where(c => c.ProyectoID.Value == this.masterProyecto.Value).Sum(x => x.ExtraDB03.Value - x.ExtraCR03.Value);
                                        break;
                                    case 4:
                                        row["Value"] = this.cierres.Where(c => c.ProyectoID.Value == this.masterProyecto.Value).Sum(x => x.ExtraDB04.Value - x.ExtraCR04.Value);
                                        break;
                                    case 5:
                                        row["Value"] = this.cierres.Where(c => c.ProyectoID.Value == this.masterProyecto.Value).Sum(x => x.ExtraDB05.Value - x.ExtraCR05.Value);
                                        break;
                                    case 6:
                                        row["Value"] = this.cierres.Where(c => c.ProyectoID.Value == this.masterProyecto.Value).Sum(x => x.ExtraDB06.Value - x.ExtraCR06.Value);
                                        break;
                                    case 7:
                                        row["Value"] = this.cierres.Where(c => c.ProyectoID.Value == this.masterProyecto.Value).Sum(x => x.ExtraDB07.Value - x.ExtraCR07.Value);
                                        break;
                                    case 8:
                                        row["Value"] = this.cierres.Where(c => c.ProyectoID.Value == this.masterProyecto.Value).Sum(x => x.ExtraDB08.Value - x.ExtraCR08.Value);
                                        break;
                                    case 9:
                                        row["Value"] = this.cierres.Where(c => c.ProyectoID.Value == this.masterProyecto.Value).Sum(x => x.ExtraDB09.Value - x.ExtraCR09.Value);
                                        break;
                                    case 10:
                                        row["Value"] = this.cierres.Where(c => c.ProyectoID.Value == this.masterProyecto.Value).Sum(x => x.ExtraDB10.Value - x.ExtraCR10.Value);
                                        break;
                                    case 11:
                                        row["Value"] = this.cierres.Where(c => c.ProyectoID.Value == this.masterProyecto.Value).Sum(x => x.ExtraDB11.Value - x.ExtraCR11.Value);
                                        break;
                                    case 12:
                                        row["Value"] = this.cierres.Where(c => c.ProyectoID.Value == this.masterProyecto.Value).Sum(x => x.ExtraDB12.Value - x.ExtraCR12.Value);
                                        break;
                                }

                                #endregion
                            }


                            table.Rows.Add(row);
                        }
                    }
                    #endregion
                    #region Filtro para Centro Costo
                    if (this.masterCentroCosto.ValidID)
                    {
                        for (int i = 1; i <= 12; i++)
                        {
                            DataRow row = row = table.NewRow();
                            row["Member"] = this.masterCentroCosto.LabelRsx + ": " + this.masterCentroCosto.Value;
                            row["Argument"] = i.ToString();

                            if (Convert.ToInt16(this.cmbTipo.EditValue) == 1)
                            {
                                #region carga el valor Moneda Local
                                switch (i)
                                {
                                    case 1:
                                        row["Value"] = this.cierres.Where(c => c.CentroCostoID.Value == this.masterCentroCosto.Value).Sum(x => x.LocalDB01.Value - x.LocalCR01.Value);
                                        break;
                                    case 2:
                                        row["Value"] = this.cierres.Where(c => c.CentroCostoID.Value == this.masterCentroCosto.Value).Sum(x => x.LocalDB02.Value - x.LocalCR02.Value);
                                        break;
                                    case 3:
                                        row["Value"] = this.cierres.Where(c => c.CentroCostoID.Value == this.masterCentroCosto.Value).Sum(x => x.LocalDB03.Value - x.LocalCR03.Value);
                                        break;
                                    case 4:
                                        row["Value"] = this.cierres.Where(c => c.CentroCostoID.Value == this.masterCentroCosto.Value).Sum(x => x.LocalDB04.Value - x.LocalCR04.Value);
                                        break;
                                    case 5:
                                        row["Value"] = this.cierres.Where(c => c.CentroCostoID.Value == this.masterCentroCosto.Value).Sum(x => x.LocalDB05.Value - x.LocalCR05.Value);
                                        break;
                                    case 6:
                                        row["Value"] = this.cierres.Where(c => c.CentroCostoID.Value == this.masterCentroCosto.Value).Sum(x => x.LocalDB06.Value - x.LocalCR06.Value);
                                        break;
                                    case 7:
                                        row["Value"] = this.cierres.Where(c => c.CentroCostoID.Value == this.masterCentroCosto.Value).Sum(x => x.LocalDB07.Value - x.LocalCR07.Value);
                                        break;
                                    case 8:
                                        row["Value"] = this.cierres.Where(c => c.CentroCostoID.Value == this.masterCentroCosto.Value).Sum(x => x.LocalDB08.Value - x.LocalCR08.Value);
                                        break;
                                    case 9:
                                        row["Value"] = this.cierres.Where(c => c.CentroCostoID.Value == this.masterCentroCosto.Value).Sum(x => x.LocalDB09.Value - x.LocalCR09.Value);
                                        break;
                                    case 10:
                                        row["Value"] = this.cierres.Where(c => c.CentroCostoID.Value == this.masterCentroCosto.Value).Sum(x => x.LocalDB10.Value - x.LocalCR10.Value);
                                        break;
                                    case 11:
                                        row["Value"] = this.cierres.Where(c => c.CentroCostoID.Value == this.masterCentroCosto.Value).Sum(x => x.LocalDB11.Value - x.LocalCR11.Value);
                                        break;
                                    case 12:
                                        row["Value"] = this.cierres.Where(c => c.CentroCostoID.Value == this.masterCentroCosto.Value).Sum(x => x.LocalDB12.Value - x.LocalCR12.Value);
                                        break;
                                }

                                #endregion
                            }
                            else
                            {
                                #region carga el valor Moneda Extranjera
                                switch (i)
                                {
                                    case 1:
                                        row["Value"] = this.cierres.Where(c => c.CentroCostoID.Value == this.masterCentroCosto.Value).Sum(x => x.ExtraDB01.Value - x.ExtraCR01.Value);
                                        break;
                                    case 2:
                                        row["Value"] = this.cierres.Where(c => c.CentroCostoID.Value == this.masterCentroCosto.Value).Sum(x => x.ExtraDB02.Value - x.ExtraCR02.Value);
                                        break;
                                    case 3:
                                        row["Value"] = this.cierres.Where(c => c.CentroCostoID.Value == this.masterCentroCosto.Value).Sum(x => x.ExtraDB03.Value - x.ExtraCR03.Value);
                                        break;
                                    case 4:
                                        row["Value"] = this.cierres.Where(c => c.CentroCostoID.Value == this.masterCentroCosto.Value).Sum(x => x.ExtraDB04.Value - x.ExtraCR04.Value);
                                        break;
                                    case 5:
                                        row["Value"] = this.cierres.Where(c => c.CentroCostoID.Value == this.masterCentroCosto.Value).Sum(x => x.ExtraDB05.Value - x.ExtraCR05.Value);
                                        break;
                                    case 6:
                                        row["Value"] = this.cierres.Where(c => c.CentroCostoID.Value == this.masterCentroCosto.Value).Sum(x => x.ExtraDB06.Value - x.ExtraCR06.Value);
                                        break;
                                    case 7:
                                        row["Value"] = this.cierres.Where(c => c.CentroCostoID.Value == this.masterCentroCosto.Value).Sum(x => x.ExtraDB07.Value - x.ExtraCR07.Value);
                                        break;
                                    case 8:
                                        row["Value"] = this.cierres.Where(c => c.CentroCostoID.Value == this.masterCentroCosto.Value).Sum(x => x.ExtraDB08.Value - x.ExtraCR08.Value);
                                        break;
                                    case 9:
                                        row["Value"] = this.cierres.Where(c => c.CentroCostoID.Value == this.masterCentroCosto.Value).Sum(x => x.ExtraDB09.Value - x.ExtraCR09.Value);
                                        break;
                                    case 10:
                                        row["Value"] = this.cierres.Where(c => c.CentroCostoID.Value == this.masterCentroCosto.Value).Sum(x => x.ExtraDB10.Value - x.ExtraCR10.Value);
                                        break;
                                    case 11:
                                        row["Value"] = this.cierres.Where(c => c.CentroCostoID.Value == this.masterCentroCosto.Value).Sum(x => x.ExtraDB11.Value - x.ExtraCR11.Value);
                                        break;
                                    case 12:
                                        row["Value"] = this.cierres.Where(c => c.CentroCostoID.Value == this.masterCentroCosto.Value).Sum(x => x.ExtraDB12.Value - x.ExtraCR12.Value);
                                        break;
                                }

                                #endregion
                            }

                            table.Rows.Add(row);
                        }
                    }
                    #endregion
                    #region Filtro para Concepto Cargo
                    if (this.masterConceptoCargo.ValidID)
                    {
                        for (int i = 1; i <= 12; i++)
                        {
                            DataRow row = row = table.NewRow();
                            row["Member"] = this.masterConceptoCargo.LabelRsx + ": " + this.masterConceptoCargo.Value;
                            row["Argument"] = i.ToString();

                            if (Convert.ToInt16(this.cmbTipo.EditValue) == 1)
                            {
                                #region carga el valor Moneda Local
                                switch (i)
                                {
                                    case 1:
                                        row["Value"] = this.cierres.Where(c => c.ConceptoCargoID.Value == this.masterConceptoCargo.Value).Sum(x => x.LocalDB01.Value - x.LocalCR01.Value);
                                        break;
                                    case 2:
                                        row["Value"] = this.cierres.Where(c => c.ConceptoCargoID.Value == this.masterConceptoCargo.Value).Sum(x => x.LocalDB02.Value - x.LocalCR02.Value);
                                        break;
                                    case 3:
                                        row["Value"] = this.cierres.Where(c => c.ConceptoCargoID.Value == this.masterConceptoCargo.Value).Sum(x => x.LocalDB03.Value - x.LocalCR03.Value);
                                        break;
                                    case 4:
                                        row["Value"] = this.cierres.Where(c => c.ConceptoCargoID.Value == this.masterConceptoCargo.Value).Sum(x => x.LocalDB04.Value - x.LocalCR04.Value);
                                        break;
                                    case 5:
                                        row["Value"] = this.cierres.Where(c => c.ConceptoCargoID.Value == this.masterConceptoCargo.Value).Sum(x => x.LocalDB05.Value - x.LocalCR05.Value);
                                        break;
                                    case 6:
                                        row["Value"] = this.cierres.Where(c => c.ConceptoCargoID.Value == this.masterConceptoCargo.Value).Sum(x => x.LocalDB06.Value - x.LocalCR06.Value);
                                        break;
                                    case 7:
                                        row["Value"] = this.cierres.Where(c => c.ConceptoCargoID.Value == this.masterConceptoCargo.Value).Sum(x => x.LocalDB07.Value - x.LocalCR07.Value);
                                        break;
                                    case 8:
                                        row["Value"] = this.cierres.Where(c => c.ConceptoCargoID.Value == this.masterConceptoCargo.Value).Sum(x => x.LocalDB08.Value - x.LocalCR08.Value);
                                        break;
                                    case 9:
                                        row["Value"] = this.cierres.Where(c => c.ConceptoCargoID.Value == this.masterConceptoCargo.Value).Sum(x => x.LocalDB09.Value - x.LocalCR09.Value);
                                        break;
                                    case 10:
                                        row["Value"] = this.cierres.Where(c => c.ConceptoCargoID.Value == this.masterConceptoCargo.Value).Sum(x => x.LocalDB10.Value - x.LocalCR10.Value);
                                        break;
                                    case 11:
                                        row["Value"] = this.cierres.Where(c => c.ConceptoCargoID.Value == this.masterConceptoCargo.Value).Sum(x => x.LocalDB11.Value - x.LocalCR11.Value);
                                        break;
                                    case 12:
                                        row["Value"] = this.cierres.Where(c => c.ConceptoCargoID.Value == this.masterConceptoCargo.Value).Sum(x => x.LocalDB12.Value - x.LocalCR12.Value);
                                        break;
                                }

                                #endregion
                            }
                            else
                            {
                                #region carga el valor Moneda Extranjera
                                switch (i)
                                {
                                    case 1:
                                        row["Value"] = this.cierres.Where(c => c.ConceptoCargoID.Value == this.masterConceptoCargo.Value).Sum(x => x.ExtraDB01.Value - x.ExtraCR01.Value);
                                        break;
                                    case 2:
                                        row["Value"] = this.cierres.Where(c => c.ConceptoCargoID.Value == this.masterConceptoCargo.Value).Sum(x => x.ExtraDB02.Value - x.ExtraCR02.Value);
                                        break;
                                    case 3:
                                        row["Value"] = this.cierres.Where(c => c.ConceptoCargoID.Value == this.masterConceptoCargo.Value).Sum(x => x.ExtraDB03.Value - x.ExtraCR03.Value);
                                        break;
                                    case 4:
                                        row["Value"] = this.cierres.Where(c => c.ConceptoCargoID.Value == this.masterConceptoCargo.Value).Sum(x => x.ExtraDB04.Value - x.ExtraCR04.Value);
                                        break;
                                    case 5:
                                        row["Value"] = this.cierres.Where(c => c.ConceptoCargoID.Value == this.masterConceptoCargo.Value).Sum(x => x.ExtraDB05.Value - x.ExtraCR05.Value);
                                        break;
                                    case 6:
                                        row["Value"] = this.cierres.Where(c => c.ConceptoCargoID.Value == this.masterConceptoCargo.Value).Sum(x => x.ExtraDB06.Value - x.ExtraCR06.Value);
                                        break;
                                    case 7:
                                        row["Value"] = this.cierres.Where(c => c.ConceptoCargoID.Value == this.masterConceptoCargo.Value).Sum(x => x.ExtraDB07.Value - x.ExtraCR07.Value);
                                        break;
                                    case 8:
                                        row["Value"] = this.cierres.Where(c => c.ConceptoCargoID.Value == this.masterConceptoCargo.Value).Sum(x => x.ExtraDB08.Value - x.ExtraCR08.Value);
                                        break;
                                    case 9:
                                        row["Value"] = this.cierres.Where(c => c.ConceptoCargoID.Value == this.masterConceptoCargo.Value).Sum(x => x.ExtraDB09.Value - x.ExtraCR09.Value);
                                        break;
                                    case 10:
                                        row["Value"] = this.cierres.Where(c => c.ConceptoCargoID.Value == this.masterConceptoCargo.Value).Sum(x => x.ExtraDB10.Value - x.ExtraCR10.Value);
                                        break;
                                    case 11:
                                        row["Value"] = this.cierres.Where(c => c.ConceptoCargoID.Value == this.masterConceptoCargo.Value).Sum(x => x.ExtraDB11.Value - x.ExtraCR11.Value);
                                        break;
                                    case 12:
                                        row["Value"] = this.cierres.Where(c => c.ConceptoCargoID.Value == this.masterConceptoCargo.Value).Sum(x => x.ExtraDB12.Value - x.ExtraCR12.Value);
                                        break;
                                }

                                #endregion
                            }

                            table.Rows.Add(row);
                        }
                    }
                    #endregion
                }
                return table;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadisticasMensualesContabilidad.cs", "CreateChartData"));
                return table;
            }
        }

        /// <summary>
        /// Carga la tabla para mostrar el detalle del filtro
        /// </summary>
        /// <param name="mes">Mes de consulta</param>
        /// <returns></returns>
        private DataTable GetDetailTable(int mes)
        {
            // Create an empty table.
            DataTable table = new DataTable("TableDetail");
            table.Columns.Add("Argument");
            table.Columns.Add("Value", typeof(Decimal));

            try
            {
                decimal value = 0;
                List<string> filterKeys = new List<string>();
                List<DTO_coCierreMes> filtroAux = ObjectCopier.Clone(this.cierresFiltro);

                foreach (DTO_coCierreMes cTemp in filtroAux)
                {
                    switch (mes)
                    #region Carga el valor del mes correspondiente en el mes 1
                    {
                        //case 2:
                        //    cTemp.ValorMes01.Value = cTemp.ValorMes02.Value.HasValue ? cTemp.ValorMes02.Value : 0;
                        //    break;
                        //case 3:
                        //    cTemp.ValorMes01.Value = cTemp.ValorMes03.Value.HasValue ? cTemp.ValorMes03.Value : 0;
                        //    break;
                        //case 4:
                        //    cTemp.ValorMes01.Value = cTemp.ValorMes04.Value.HasValue ? cTemp.ValorMes04.Value : 0;
                        //    break;
                        //case 5:
                        //    cTemp.ValorMes01.Value = cTemp.ValorMes05.Value.HasValue ? cTemp.ValorMes05.Value : 0;
                        //    break;
                        //case 6:
                        //    cTemp.ValorMes01.Value = cTemp.ValorMes06.Value.HasValue ? cTemp.ValorMes06.Value : 0;
                        //    break;
                        //case 7:
                        //    cTemp.ValorMes01.Value = cTemp.ValorMes07.Value.HasValue ? cTemp.ValorMes07.Value : 0;
                        //    break;
                        //case 8:
                        //    cTemp.ValorMes01.Value = cTemp.ValorMes08.Value.HasValue ? cTemp.ValorMes08.Value : 0;
                        //    break;
                        //case 9:
                        //    cTemp.ValorMes01.Value = cTemp.ValorMes09.Value.HasValue ? cTemp.ValorMes09.Value : 0;
                        //    break;
                        //case 10:
                        //    cTemp.ValorMes01.Value = cTemp.ValorMes10.Value.HasValue ? cTemp.ValorMes10.Value : 0;
                        //    break;
                        //case 11:
                        //    cTemp.ValorMes01.Value = cTemp.ValorMes11.Value.HasValue ? cTemp.ValorMes11.Value : 0;
                        //    break;
                        //case 12:
                        //    cTemp.ValorMes01.Value = cTemp.ValorMes12.Value.HasValue ? cTemp.ValorMes12.Value : 0;
                        //    break;
                    }
                    #endregion
                }

                #region Carga los filtros
                switch (this.rbDetails.SelectedIndex)
                {
                    #region Línea de crédito
                    //case 0:
                    //    filterKeys = (from f in filtroAux select f.LineaCreditoID.Value).Distinct().ToList();
                    //    foreach (string key in filterKeys)
                    //    {
                    //        value = (from f in filtroAux where f.LineaCreditoID.Value == key select f.ValorMes01.Value.Value).Sum();

                    //        DataRow row = row = table.NewRow();
                    //        row["Argument"] = key;
                    //        row["Value"] = value;

                    //        table.Rows.Add(row);
                    //    }

                    //    break;
                    #endregion
                    #region Asesor
                    //case 1:
                    //    filterKeys = (from f in filtroAux select f.AsesorID.Value).Distinct().ToList();
                    //    foreach (string key in filterKeys)
                    //    {
                    //        value = (from f in filtroAux where f.AsesorID.Value == key select f.ValorMes01.Value.Value).Sum();

                    //        DataRow row = row = table.NewRow();
                    //        row["Argument"] = key;
                    //        row["Value"] = value;

                    //        table.Rows.Add(row);
                    //    }
                    //    break;
                    #endregion
                    #region Centro de pago
                    //case 2:
                    //    filterKeys = (from f in filtroAux select f.CentroPagoID.Value).Distinct().ToList();
                    //    foreach (string key in filterKeys)
                    //    {
                    //        value = (from f in filtroAux where f.CentroPagoID.Value == key select f.ValorMes01.Value.Value).Sum();

                    //        DataRow row = row = table.NewRow();
                    //        row["Argument"] = key;
                    //        row["Value"] = value;

                    //        table.Rows.Add(row);
                    //    }
                    //    break;
                    #endregion
                    #region Zona
                    //case 3:
                    //    filterKeys = (from f in filtroAux select f.ZonaID.Value).Distinct().ToList();
                    //    foreach (string key in filterKeys)
                    //    {
                    //        value = (from f in filtroAux where f.ZonaID.Value == key select f.ValorMes01.Value.Value).Sum();

                    //        DataRow row = row = table.NewRow();
                    //        row["Argument"] = key;
                    //        row["Value"] = value;

                    //        table.Rows.Add(row);
                    //    }
                    //    break;
                    #endregion
                    #region Comprador
                    //case 4:
                    //    filterKeys = (from f in filtroAux select f.CompradorCarteraID.Value).Distinct().ToList();
                    //    foreach (string key in filterKeys)
                    //    {
                    //        if (!string.IsNullOrWhiteSpace(key))
                    //        {
                    //            value = (from f in filtroAux where f.CompradorCarteraID.Value == key select f.ValorMes01.Value.Value).Sum();

                    //            DataRow row = row = table.NewRow();
                    //            row["Argument"] = key;
                    //            row["Value"] = value;

                    //            table.Rows.Add(row);
                    //        }
                    //    }
                    //    break;
                    #endregion
                }
                #endregion

                return table;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadisticasMensualesContabilidad.cs", "CreateChartDetail"));
                return table;
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
                FormProvider.Master.Form_Enter(this, this._documentID, this._frmType, this._frmModule);
                FormProvider.Master.itemSave.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadisticasMensualesContabilidad.cs", "Form_Enter"));
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
                FormProvider.Master.Form_Closing(this, this._documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadisticasMensualesContabilidad.cs", "Form_FormClosing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadisticasMensualesContabilidad.cs", "Form_FormClosed"));
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

        #endregion Eventos MDI

        #region Eventos formulario y charts

        /// <summary>
        /// Evento que vuelve a traer información de base de datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbAño_Leave(object sender, EventArgs e)
        {
            try
            {
                bool newDate = true;

                if (this.cierres != null && this.cierres.Count > 0 && this.cmbAño.SelectedItem.ToString() != this.año.ToString())
                {
                    string msgTitleSearch = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_GetData));
                    string msgNewSearch = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewSearch);

                    if (MessageBox.Show(msgNewSearch, msgTitleSearch, MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        newDate = false;
                        this.cmbAño.SelectedItem = this.año;
                    }
                }

                if (newDate)
                {
                    #region Carga los nuevos datos
                    this.año = Convert.ToInt32(this.cmbAño.SelectedItem);

                    this.cmbTipo.EditValue = 1;
                    this.masterLibro.Value = string.Empty;
                    this.masterCuenta.Value = string.Empty;
                    this.masterProyecto.Value = string.Empty;
                    this.masterConceptoCargo.Value = string.Empty;
                    this.masterCentroCosto.Value = string.Empty;

                    this.rbDetails.SelectedIndex = 0;
                    this.chartDetail.Series.Clear();

                    this.GetData();

                    //this.cierresFiltro = this.cierres.Where(c => c.TipoDato.Value.Value == 1).ToList();
                    DataTable table = this.GetChartTable();

                    this.chartCierre.DataSource = table;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadisticasMensualesContabilidad.cs", "dtPeriod_Leave"));
            }
        }

        /// <summary>
        /// Cambio de tipo de serie segun un filtro
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chartCierre_BoundDataChanged(object sender, EventArgs e)
        {
            try
            {
                ChartControl chart = (ChartControl)sender;

                #region Libro
                if (this.masterLibro.ValidID)
                {
                    Series libro = chart.GetSeriesByName(this.masterLibro.LabelRsx + ": " + this.masterLibro.Value);
                    if (libro != null)
                    {
                        libro.View.Color = Color.GreenYellow;
                        libro.ChangeView(ViewType.Line);
                    }
                }
                #endregion
                #region Cuenta
                if (this.masterCuenta.ValidID)
                {
                    Series asesor = chart.GetSeriesByName(this.masterCuenta.LabelRsx + ": " + this.masterCuenta.Value);
                    if (asesor != null)
                    {
                        asesor.View.Color = Color.Red;
                        asesor.ChangeView(ViewType.Line);
                    }
                }
                #endregion
                #region proyecto
                if (this.masterProyecto.ValidID)
                {
                    Series centroPago = chart.GetSeriesByName(this.masterProyecto.LabelRsx + ": " + this.masterProyecto.Value);
                    if (centroPago != null)
                    {
                        centroPago.View.Color = Color.Pink;
                        centroPago.ChangeView(ViewType.Line);
                    }
                }
                #endregion
                #region Centro Costo
                if (this.masterCentroCosto.ValidID)
                {
                    Series zona = chart.GetSeriesByName(this.masterCentroCosto.LabelRsx + ": " + this.masterCentroCosto.Value);
                    if (zona != null)
                    {
                        zona.View.Color = Color.Green;
                        zona.ChangeView(ViewType.Line);
                    }
                }
                #endregion
                #region Concepto Cargo
                if (this.masterConceptoCargo.ValidID)
                {
                    Series comprador = chart.GetSeriesByName(this.masterConceptoCargo.LabelRsx + ": " + this.masterConceptoCargo.Value);
                    if (comprador != null)
                    {
                        comprador.View.Color = Color.MediumPurple;
                        comprador.ChangeView(ViewType.Line);
                    }
                }
                #endregion

                //Asigna el formato
                foreach (Series s in chart.Series)
                {
                    //if (this.cmbTipo.EditValue == "1")
                    //    s.LegendPointOptions.ValueNumericOptions.Format = NumericFormat.Number;
                    //else
                    //{
                        s.LegendPointOptions.ValueNumericOptions.Format = NumericFormat.Currency;
                        s.CrosshairLabelPattern = "{A} : {V:C}";
                    //}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadisticasMensualesContabilidad.cs", "chart_BoundDataChanged"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta para pintar el detalle de un punto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chartCierre_Click(object sender, EventArgs e)
        {
            try
            {
                ChartHitInfo hitInfo = this.chartCierre.CalcHitInfo(((MouseEventArgs)e).Location);
                SeriesPoint point = hitInfo.SeriesPoint;
                if (point != null)
                {
                    int mes = Convert.ToInt32(point.Argument);
                    this.AddDetailChart(mes);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadisticasMensualesContabilidad.cs", "chartCierre_Click"));
            }
        }

        #endregion

        #region Eventos Barra Herramientas

        /// <summary>
        /// Boton para crear buscar
        /// </summary>
        public override void TBUpdate()
        {
            try
            {
                DataTable table = this.GetChartTable();

                this.chartCierre.DataSource = table;

                //Limpia el control de detalles
                if (this.cmbTipo.EditValue.ToString() != this.tipoDato)
                {
                    this.tipoDato = this.cmbTipo.EditValue.ToString();
                    this.chartDetail.Titles.Clear();
                    this.chartDetail.Series.Clear();

                    //Asigna formatos
                    XYDiagram mesgram = (XYDiagram)this.chartCierre.Diagram;
                    this.chartCierre.SeriesTemplate.LegendPointOptions.ValueNumericOptions.Format = NumericFormat.Currency;

                    mesgram.AxisY.NumericOptions.Format = NumericFormat.Currency;
                    mesgram.AxisY.GridSpacingAuto = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DashBoard.cs", "TBSearch"));
            }
        }

        #endregion Eventos Barra Herramientas

    }
}
