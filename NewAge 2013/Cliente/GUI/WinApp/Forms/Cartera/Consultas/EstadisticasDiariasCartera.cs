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
    public partial class EstadisticasDiariasCartera : FormWithToolbar
    {
        #region Variables

        BaseController _bc = BaseController.GetInstance();
        
        private List<DTO_ccCierreDia> cierres;
        private List<DTO_ccCierreDia> cierresFiltro;
        private FormTypes _frmType = FormTypes.DocumentAprob;
        private string _frmName;
        private ModulesPrefix _frmModule;
        private int _documentID;

        private DateTime periodo;
        private string tipoDato;

        #endregion

        public EstadisticasDiariasCartera()
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
                this.chartCierre.SeriesTemplate.View = view;// SideBySideBarSeriesView();
                //this.pivot2.SeriesNameTemplate.BeginText = "Filtro: ";

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadisticasDiarias.cs", "DashBoard"));
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
                this._documentID = AppQueries.EstadisticasDiariasCartera;
                this._frmModule = ModulesPrefix.cc;

                //Carga la informacion de la maestras
                _bc.InitMasterUC(this.masterLineaCredito, AppMasters.ccLineaCredito, true, true, true, false);
                _bc.InitMasterUC(this.masterAsesor, AppMasters.ccAsesor, true, true, true, false);
                _bc.InitMasterUC(this.masterCentroPago, AppMasters.ccCentroPagoPAG, true, true, true, false);
                _bc.InitMasterUC(this.masterZona, AppMasters.glZona, true, true, true, false);
                _bc.InitMasterUC(this.masterComprador, AppMasters.ccCompradorCartera, true, true, true, false);

                // Carga el tipo de cierre
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("1", this._bc.GetResource(LanguageTypes.Tables, "35551_tbl_Cantidad"));
                dic.Add("2", this._bc.GetResource(LanguageTypes.Tables, "35551_tbl_VlrNominal"));
                dic.Add("3", this._bc.GetResource(LanguageTypes.Tables, "35551_tbl_VlrCapital"));
                dic.Add("4", this._bc.GetResource(LanguageTypes.Tables, "35551_tbl_VlrGiro"));
                this.cmbTipo.Properties.DataSource = dic;
                this.cmbTipo.EditValue = "1";  

                // Periodo 
                string periodoStr = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo);
                this.dtPeriod.DateTime = Convert.ToDateTime(periodoStr);
                this.periodo = this.dtPeriod.DateTime;

                // Plazo
                this.comboPlazo.SelectedValue = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadisticasDiarias.cs", "SetInitParameters"));
            }
        }

        /// <summary>
        /// Funcion temporal para traer los datos 
        /// </summary>
        private void GetData()
        {
            try
            {
               this.cierres = _bc.AdministrationModel.ccCierreDia_GetAll(this.periodo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadisticasDiarias.cs", "GetData"));
            }
        }

        /// <summary>
        /// Carga la información de la grilla superior derecha
        /// </summary>
        /// <param name="dia">Día de consulta</param>
        private void AddDetailChart(int dia)
        {
            try
            {
                this.chartDetail.Series.Clear();
                DataTable tableDetail = this.GetDetailTable(dia);

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
                titleBar.Text = this.lblDia.Text + dia.ToString() + ": ";
                switch (this.rbDetails.SelectedIndex)
                {
                    #region Línea de crédito
                    case 0:
                        titleBar.Text += this.masterLineaCredito.LabelRsx;
                        break;
                    #endregion
                    #region Asesor
                    case 1:
                        titleBar.Text += this.masterAsesor.LabelRsx;
                        break;
                    #endregion
                    #region Centro de pago
                    case 2:
                        titleBar.Text += this.masterCentroPago.LabelRsx;
                        break;
                    #endregion
                    #region Zona
                    case 3:
                        titleBar.Text += this.masterZona.LabelRsx;
                        break;
                    #endregion
                    #region Comprador
                    case 4:
                        titleBar.Text += this.masterComprador.LabelRsx;
                        break;
                    #endregion
                    #region Plazo
                    case 5:
                        titleBar.Text += this.lblPlazo.Text;
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadisticasDiarias.cs", "CreateDetailsChart"));
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
                if (this.cierresFiltro.Count > 0)
                {
                    #region Info de totales
                    for (int i = 1; i <= 31; i++)
                    {
                        DataRow row = row = table.NewRow();
                        row["Member"] = "Total";
                        row["Argument"] = i.ToString();

                        #region carga el valor
                        switch (i)
                        {
                            case 1:
                                row["Value"] = this.cierresFiltro.Sum(x => x.ValorDia01.Value);
                                break;
                            case 2:
                                row["Value"] = this.cierresFiltro.Sum(x => x.ValorDia02.Value);
                                break;
                            case 3:
                                row["Value"] = this.cierresFiltro.Sum(x => x.ValorDia03.Value);
                                break;
                            case 4:
                                row["Value"] = this.cierresFiltro.Sum(x => x.ValorDia04.Value);
                                break;
                            case 5:
                                row["Value"] = this.cierresFiltro.Sum(x => x.ValorDia05.Value);
                                break;
                            case 6:
                                row["Value"] = this.cierresFiltro.Sum(x => x.ValorDia06.Value);
                                break;
                            case 7:
                                row["Value"] = this.cierresFiltro.Sum(x => x.ValorDia07.Value);
                                break;
                            case 8:
                                row["Value"] = this.cierresFiltro.Sum(x => x.ValorDia08.Value);
                                break;
                            case 9:
                                row["Value"] = this.cierresFiltro.Sum(x => x.ValorDia09.Value);
                                break;
                            case 10:
                                row["Value"] = this.cierresFiltro.Sum(x => x.ValorDia10.Value);
                                break;
                            case 11:
                                row["Value"] = this.cierresFiltro.Sum(x => x.ValorDia11.Value);
                                break;
                            case 12:
                                row["Value"] = this.cierresFiltro.Sum(x => x.ValorDia12.Value);
                                break;
                            case 13:
                                row["Value"] = this.cierresFiltro.Sum(x => x.ValorDia13.Value);
                                break;
                            case 14:
                                row["Value"] = this.cierresFiltro.Sum(x => x.ValorDia14.Value);
                                break;
                            case 15:
                                row["Value"] = this.cierresFiltro.Sum(x => x.ValorDia15.Value);
                                break;
                            case 16:
                                row["Value"] = this.cierresFiltro.Sum(x => x.ValorDia16.Value);
                                break;
                            case 17:
                                row["Value"] = this.cierresFiltro.Sum(x => x.ValorDia17.Value);
                                break;
                            case 18:
                                row["Value"] = this.cierresFiltro.Sum(x => x.ValorDia18.Value);
                                break;
                            case 19:
                                row["Value"] = this.cierresFiltro.Sum(x => x.ValorDia19.Value);
                                break;
                            case 20:
                                row["Value"] = this.cierresFiltro.Sum(x => x.ValorDia20.Value);
                                break;
                            case 21:
                                row["Value"] = this.cierresFiltro.Sum(x => x.ValorDia21.Value);
                                break;
                            case 22:
                                row["Value"] = this.cierresFiltro.Sum(x => x.ValorDia22.Value);
                                break;
                            case 23:
                                row["Value"] = this.cierresFiltro.Sum(x => x.ValorDia23.Value);
                                break;
                            case 24:
                                row["Value"] = this.cierresFiltro.Sum(x => x.ValorDia24.Value);
                                break;
                            case 25:
                                row["Value"] = this.cierresFiltro.Sum(x => x.ValorDia25.Value);
                                break;
                            case 26:
                                row["Value"] = this.cierresFiltro.Sum(x => x.ValorDia26.Value);
                                break;
                            case 27:
                                row["Value"] = this.cierresFiltro.Sum(x => x.ValorDia27.Value);
                                break;
                            case 28:
                                row["Value"] = this.cierresFiltro.Sum(x => x.ValorDia28.Value);
                                break;
                            case 29:
                                row["Value"] = this.cierresFiltro.Sum(x => x.ValorDia29.Value);
                                break;
                            case 30:
                                row["Value"] = this.cierresFiltro.Sum(x => x.ValorDia30.Value);
                                break;
                            case 31:
                                row["Value"] = this.cierresFiltro.Sum(x => x.ValorDia31.Value);
                                break;
                        }

                        #endregion

                        table.Rows.Add(row);
                    }
                    #endregion
                    #region Filtro para el plazo
                    if (this.comboPlazo.SelectedItem != null && !string.IsNullOrWhiteSpace(this.comboPlazo.SelectedItem.ToString()))
                    {
                        Int16 plazo = Convert.ToInt16(this.comboPlazo.SelectedValue);
                        for (int i = 1; i <= 31; i++)
                        {
                            DataRow row = row = table.NewRow();
                            row["Member"] = this.lblPlazo.Text + ": " + this.comboPlazo.SelectedItem;
                            row["Argument"] = i.ToString();

                            #region carga el valor
                            switch (i)
                            {
                                case 1:
                                    row["Value"] = this.cierresFiltro.Where(c => c.Plazo.Value == plazo).Sum(x => x.ValorDia01.Value);
                                    break;
                                case 2:
                                    row["Value"] = this.cierresFiltro.Where(c => c.Plazo.Value == plazo).Sum(x => x.ValorDia02.Value);
                                    break;
                                case 3:
                                    row["Value"] = this.cierresFiltro.Where(c => c.Plazo.Value == plazo).Sum(x => x.ValorDia03.Value);
                                    break;
                                case 4:
                                    row["Value"] = this.cierresFiltro.Where(c => c.Plazo.Value == plazo).Sum(x => x.ValorDia04.Value);
                                    break;
                                case 5:
                                    row["Value"] = this.cierresFiltro.Where(c => c.Plazo.Value == plazo).Sum(x => x.ValorDia05.Value);
                                    break;
                                case 6:
                                    row["Value"] = this.cierresFiltro.Where(c => c.Plazo.Value == plazo).Sum(x => x.ValorDia06.Value);
                                    break;
                                case 7:
                                    row["Value"] = this.cierresFiltro.Where(c => c.Plazo.Value == plazo).Sum(x => x.ValorDia07.Value);
                                    break;
                                case 8:
                                    row["Value"] = this.cierresFiltro.Where(c => c.Plazo.Value == plazo).Sum(x => x.ValorDia08.Value);
                                    break;
                                case 9:
                                    row["Value"] = this.cierresFiltro.Where(c => c.Plazo.Value == plazo).Sum(x => x.ValorDia09.Value);
                                    break;
                                case 10:
                                    row["Value"] = this.cierresFiltro.Where(c => c.Plazo.Value == plazo).Sum(x => x.ValorDia10.Value);
                                    break;
                                case 11:
                                    row["Value"] = this.cierresFiltro.Where(c => c.Plazo.Value == plazo).Sum(x => x.ValorDia11.Value);
                                    break;
                                case 12:
                                    row["Value"] = this.cierresFiltro.Where(c => c.Plazo.Value == plazo).Sum(x => x.ValorDia12.Value);
                                    break;
                                case 13:
                                    row["Value"] = this.cierresFiltro.Where(c => c.Plazo.Value == plazo).Sum(x => x.ValorDia13.Value);
                                    break;
                                case 14:
                                    row["Value"] = this.cierresFiltro.Where(c => c.Plazo.Value == plazo).Sum(x => x.ValorDia14.Value);
                                    break;
                                case 15:
                                    row["Value"] = this.cierresFiltro.Where(c => c.Plazo.Value == plazo).Sum(x => x.ValorDia15.Value);
                                    break;
                                case 16:
                                    row["Value"] = this.cierresFiltro.Where(c => c.Plazo.Value == plazo).Sum(x => x.ValorDia16.Value);
                                    break;
                                case 17:
                                    row["Value"] = this.cierresFiltro.Where(c => c.Plazo.Value == plazo).Sum(x => x.ValorDia17.Value);
                                    break;
                                case 18:
                                    row["Value"] = this.cierresFiltro.Where(c => c.Plazo.Value == plazo).Sum(x => x.ValorDia18.Value);
                                    break;
                                case 19:
                                    row["Value"] = this.cierresFiltro.Where(c => c.Plazo.Value == plazo).Sum(x => x.ValorDia19.Value);
                                    break;
                                case 20:
                                    row["Value"] = this.cierresFiltro.Where(c => c.Plazo.Value == plazo).Sum(x => x.ValorDia20.Value);
                                    break;
                                case 21:
                                    row["Value"] = this.cierresFiltro.Where(c => c.Plazo.Value == plazo).Sum(x => x.ValorDia21.Value);
                                    break;
                                case 22:
                                    row["Value"] = this.cierresFiltro.Where(c => c.Plazo.Value == plazo).Sum(x => x.ValorDia22.Value);
                                    break;
                                case 23:
                                    row["Value"] = this.cierresFiltro.Where(c => c.Plazo.Value == plazo).Sum(x => x.ValorDia23.Value);
                                    break;
                                case 24:
                                    row["Value"] = this.cierresFiltro.Where(c => c.Plazo.Value == plazo).Sum(x => x.ValorDia24.Value);
                                    break;
                                case 25:
                                    row["Value"] = this.cierresFiltro.Where(c => c.Plazo.Value == plazo).Sum(x => x.ValorDia25.Value);
                                    break;
                                case 26:
                                    row["Value"] = this.cierresFiltro.Where(c => c.Plazo.Value == plazo).Sum(x => x.ValorDia26.Value);
                                    break;
                                case 27:
                                    row["Value"] = this.cierresFiltro.Where(c => c.Plazo.Value == plazo).Sum(x => x.ValorDia27.Value);
                                    break;
                                case 28:
                                    row["Value"] = this.cierresFiltro.Where(c => c.Plazo.Value == plazo).Sum(x => x.ValorDia28.Value);
                                    break;
                                case 29:
                                    row["Value"] = this.cierresFiltro.Where(c => c.Plazo.Value == plazo).Sum(x => x.ValorDia29.Value);
                                    break;
                                case 30:
                                    row["Value"] = this.cierresFiltro.Where(c => c.Plazo.Value == plazo).Sum(x => x.ValorDia30.Value);
                                    break;
                                case 31:
                                    row["Value"] = this.cierresFiltro.Where(c => c.Plazo.Value == plazo).Sum(x => x.ValorDia31.Value);
                                    break;
                            }

                            #endregion

                            table.Rows.Add(row);
                        }
                    }
                    #endregion
                    #region Filtro para linea de crédito
                    if (this.masterLineaCredito.ValidID)
                    {
                        for (int i = 1; i <= 31; i++)
                        {
                            DataRow row = row = table.NewRow();
                            row["Member"] = this.masterLineaCredito.LabelRsx + ": " + this.masterLineaCredito.Value;
                            row["Argument"] = i.ToString();

                            #region carga el valor
                            switch (i)
                            {
                                case 1:
                                    row["Value"] = this.cierresFiltro.Where(c => c.LineaCreditoID.Value == this.masterLineaCredito.Value).Sum(x => x.ValorDia01.Value);
                                    break;
                                case 2:
                                    row["Value"] = this.cierresFiltro.Where(c => c.LineaCreditoID.Value == this.masterLineaCredito.Value).Sum(x => x.ValorDia02.Value);
                                    break;
                                case 3:
                                    row["Value"] = this.cierresFiltro.Where(c => c.LineaCreditoID.Value == this.masterLineaCredito.Value).Sum(x => x.ValorDia03.Value);
                                    break;
                                case 4:
                                    row["Value"] = this.cierresFiltro.Where(c => c.LineaCreditoID.Value == this.masterLineaCredito.Value).Sum(x => x.ValorDia04.Value);
                                    break;
                                case 5:
                                    row["Value"] = this.cierresFiltro.Where(c => c.LineaCreditoID.Value == this.masterLineaCredito.Value).Sum(x => x.ValorDia05.Value);
                                    break;
                                case 6:
                                    row["Value"] = this.cierresFiltro.Where(c => c.LineaCreditoID.Value == this.masterLineaCredito.Value).Sum(x => x.ValorDia06.Value);
                                    break;
                                case 7:
                                    row["Value"] = this.cierresFiltro.Where(c => c.LineaCreditoID.Value == this.masterLineaCredito.Value).Sum(x => x.ValorDia07.Value);
                                    break;
                                case 8:
                                    row["Value"] = this.cierresFiltro.Where(c => c.LineaCreditoID.Value == this.masterLineaCredito.Value).Sum(x => x.ValorDia08.Value);
                                    break;
                                case 9:
                                    row["Value"] = this.cierresFiltro.Where(c => c.LineaCreditoID.Value == this.masterLineaCredito.Value).Sum(x => x.ValorDia09.Value);
                                    break;
                                case 10:
                                    row["Value"] = this.cierresFiltro.Where(c => c.LineaCreditoID.Value == this.masterLineaCredito.Value).Sum(x => x.ValorDia10.Value);
                                    break;
                                case 11:
                                    row["Value"] = this.cierresFiltro.Where(c => c.LineaCreditoID.Value == this.masterLineaCredito.Value).Sum(x => x.ValorDia11.Value);
                                    break;
                                case 12:
                                    row["Value"] = this.cierresFiltro.Where(c => c.LineaCreditoID.Value == this.masterLineaCredito.Value).Sum(x => x.ValorDia12.Value);
                                    break;
                                case 13:
                                    row["Value"] = this.cierresFiltro.Where(c => c.LineaCreditoID.Value == this.masterLineaCredito.Value).Sum(x => x.ValorDia13.Value);
                                    break;
                                case 14:
                                    row["Value"] = this.cierresFiltro.Where(c => c.LineaCreditoID.Value == this.masterLineaCredito.Value).Sum(x => x.ValorDia14.Value);
                                    break;
                                case 15:
                                    row["Value"] = this.cierresFiltro.Where(c => c.LineaCreditoID.Value == this.masterLineaCredito.Value).Sum(x => x.ValorDia15.Value);
                                    break;
                                case 16:
                                    row["Value"] = this.cierresFiltro.Where(c => c.LineaCreditoID.Value == this.masterLineaCredito.Value).Sum(x => x.ValorDia16.Value);
                                    break;
                                case 17:
                                    row["Value"] = this.cierresFiltro.Where(c => c.LineaCreditoID.Value == this.masterLineaCredito.Value).Sum(x => x.ValorDia17.Value);
                                    break;
                                case 18:
                                    row["Value"] = this.cierresFiltro.Where(c => c.LineaCreditoID.Value == this.masterLineaCredito.Value).Sum(x => x.ValorDia18.Value);
                                    break;
                                case 19:
                                    row["Value"] = this.cierresFiltro.Where(c => c.LineaCreditoID.Value == this.masterLineaCredito.Value).Sum(x => x.ValorDia19.Value);
                                    break;
                                case 20:
                                    row["Value"] = this.cierresFiltro.Where(c => c.LineaCreditoID.Value == this.masterLineaCredito.Value).Sum(x => x.ValorDia20.Value);
                                    break;
                                case 21:
                                    row["Value"] = this.cierresFiltro.Where(c => c.LineaCreditoID.Value == this.masterLineaCredito.Value).Sum(x => x.ValorDia21.Value);
                                    break;
                                case 22:
                                    row["Value"] = this.cierresFiltro.Where(c => c.LineaCreditoID.Value == this.masterLineaCredito.Value).Sum(x => x.ValorDia22.Value);
                                    break;
                                case 23:
                                    row["Value"] = this.cierresFiltro.Where(c => c.LineaCreditoID.Value == this.masterLineaCredito.Value).Sum(x => x.ValorDia23.Value);
                                    break;
                                case 24:
                                    row["Value"] = this.cierresFiltro.Where(c => c.LineaCreditoID.Value == this.masterLineaCredito.Value).Sum(x => x.ValorDia24.Value);
                                    break;
                                case 25:
                                    row["Value"] = this.cierresFiltro.Where(c => c.LineaCreditoID.Value == this.masterLineaCredito.Value).Sum(x => x.ValorDia25.Value);
                                    break;
                                case 26:
                                    row["Value"] = this.cierresFiltro.Where(c => c.LineaCreditoID.Value == this.masterLineaCredito.Value).Sum(x => x.ValorDia26.Value);
                                    break;
                                case 27:
                                    row["Value"] = this.cierresFiltro.Where(c => c.LineaCreditoID.Value == this.masterLineaCredito.Value).Sum(x => x.ValorDia27.Value);
                                    break;
                                case 28:
                                    row["Value"] = this.cierresFiltro.Where(c => c.LineaCreditoID.Value == this.masterLineaCredito.Value).Sum(x => x.ValorDia28.Value);
                                    break;
                                case 29:
                                    row["Value"] = this.cierresFiltro.Where(c => c.LineaCreditoID.Value == this.masterLineaCredito.Value).Sum(x => x.ValorDia29.Value);
                                    break;
                                case 30:
                                    row["Value"] = this.cierresFiltro.Where(c => c.LineaCreditoID.Value == this.masterLineaCredito.Value).Sum(x => x.ValorDia30.Value);
                                    break;
                                case 31:
                                    row["Value"] = this.cierresFiltro.Where(c => c.LineaCreditoID.Value == this.masterLineaCredito.Value).Sum(x => x.ValorDia31.Value);
                                    break;
                            }

                            #endregion

                            table.Rows.Add(row);
                        }
                    }
                    #endregion
                    #region Filtro para asesor
                    if (this.masterAsesor.ValidID)
                    {
                        for (int i = 1; i <= 31; i++)
                        {
                            DataRow row = row = table.NewRow();
                            row["Member"] = this.masterAsesor.LabelRsx + ": " + this.masterAsesor.Value;
                            row["Argument"] = i.ToString();

                            #region carga el valor
                            switch (i)
                            {
                                case 1:
                                    row["Value"] = this.cierresFiltro.Where(c => c.AsesorID.Value == this.masterAsesor.Value).Sum(x => x.ValorDia01.Value);
                                    break;
                                case 2:
                                    row["Value"] = this.cierresFiltro.Where(c => c.AsesorID.Value == this.masterAsesor.Value).Sum(x => x.ValorDia02.Value);
                                    break;
                                case 3:
                                    row["Value"] = this.cierresFiltro.Where(c => c.AsesorID.Value == this.masterAsesor.Value).Sum(x => x.ValorDia03.Value);
                                    break;
                                case 4:
                                    row["Value"] = this.cierresFiltro.Where(c => c.AsesorID.Value == this.masterAsesor.Value).Sum(x => x.ValorDia04.Value);
                                    break;
                                case 5:
                                    row["Value"] = this.cierresFiltro.Where(c => c.AsesorID.Value == this.masterAsesor.Value).Sum(x => x.ValorDia05.Value);
                                    break;
                                case 6:
                                    row["Value"] = this.cierresFiltro.Where(c => c.AsesorID.Value == this.masterAsesor.Value).Sum(x => x.ValorDia06.Value);
                                    break;
                                case 7:
                                    row["Value"] = this.cierresFiltro.Where(c => c.AsesorID.Value == this.masterAsesor.Value).Sum(x => x.ValorDia07.Value);
                                    break;
                                case 8:
                                    row["Value"] = this.cierresFiltro.Where(c => c.AsesorID.Value == this.masterAsesor.Value).Sum(x => x.ValorDia08.Value);
                                    break;
                                case 9:
                                    row["Value"] = this.cierresFiltro.Where(c => c.AsesorID.Value == this.masterAsesor.Value).Sum(x => x.ValorDia09.Value);
                                    break;
                                case 10:
                                    row["Value"] = this.cierresFiltro.Where(c => c.AsesorID.Value == this.masterAsesor.Value).Sum(x => x.ValorDia10.Value);
                                    break;
                                case 11:
                                    row["Value"] = this.cierresFiltro.Where(c => c.AsesorID.Value == this.masterAsesor.Value).Sum(x => x.ValorDia11.Value);
                                    break;
                                case 12:
                                    row["Value"] = this.cierresFiltro.Where(c => c.AsesorID.Value == this.masterAsesor.Value).Sum(x => x.ValorDia12.Value);
                                    break;
                                case 13:
                                    row["Value"] = this.cierresFiltro.Where(c => c.AsesorID.Value == this.masterAsesor.Value).Sum(x => x.ValorDia13.Value);
                                    break;
                                case 14:
                                    row["Value"] = this.cierresFiltro.Where(c => c.AsesorID.Value == this.masterAsesor.Value).Sum(x => x.ValorDia14.Value);
                                    break;
                                case 15:
                                    row["Value"] = this.cierresFiltro.Where(c => c.AsesorID.Value == this.masterAsesor.Value).Sum(x => x.ValorDia15.Value);
                                    break;
                                case 16:
                                    row["Value"] = this.cierresFiltro.Where(c => c.AsesorID.Value == this.masterAsesor.Value).Sum(x => x.ValorDia16.Value);
                                    break;
                                case 17:
                                    row["Value"] = this.cierresFiltro.Where(c => c.AsesorID.Value == this.masterAsesor.Value).Sum(x => x.ValorDia17.Value);
                                    break;
                                case 18:
                                    row["Value"] = this.cierresFiltro.Where(c => c.AsesorID.Value == this.masterAsesor.Value).Sum(x => x.ValorDia18.Value);
                                    break;
                                case 19:
                                    row["Value"] = this.cierresFiltro.Where(c => c.AsesorID.Value == this.masterAsesor.Value).Sum(x => x.ValorDia19.Value);
                                    break;
                                case 20:
                                    row["Value"] = this.cierresFiltro.Where(c => c.AsesorID.Value == this.masterAsesor.Value).Sum(x => x.ValorDia20.Value);
                                    break;
                                case 21:
                                    row["Value"] = this.cierresFiltro.Where(c => c.AsesorID.Value == this.masterAsesor.Value).Sum(x => x.ValorDia21.Value);
                                    break;
                                case 22:
                                    row["Value"] = this.cierresFiltro.Where(c => c.AsesorID.Value == this.masterAsesor.Value).Sum(x => x.ValorDia22.Value);
                                    break;
                                case 23:
                                    row["Value"] = this.cierresFiltro.Where(c => c.AsesorID.Value == this.masterAsesor.Value).Sum(x => x.ValorDia23.Value);
                                    break;
                                case 24:
                                    row["Value"] = this.cierresFiltro.Where(c => c.AsesorID.Value == this.masterAsesor.Value).Sum(x => x.ValorDia24.Value);
                                    break;
                                case 25:
                                    row["Value"] = this.cierresFiltro.Where(c => c.AsesorID.Value == this.masterAsesor.Value).Sum(x => x.ValorDia25.Value);
                                    break;
                                case 26:
                                    row["Value"] = this.cierresFiltro.Where(c => c.AsesorID.Value == this.masterAsesor.Value).Sum(x => x.ValorDia26.Value);
                                    break;
                                case 27:
                                    row["Value"] = this.cierresFiltro.Where(c => c.AsesorID.Value == this.masterAsesor.Value).Sum(x => x.ValorDia27.Value);
                                    break;
                                case 28:
                                    row["Value"] = this.cierresFiltro.Where(c => c.AsesorID.Value == this.masterAsesor.Value).Sum(x => x.ValorDia28.Value);
                                    break;
                                case 29:
                                    row["Value"] = this.cierresFiltro.Where(c => c.AsesorID.Value == this.masterAsesor.Value).Sum(x => x.ValorDia29.Value);
                                    break;
                                case 30:
                                    row["Value"] = this.cierresFiltro.Where(c => c.AsesorID.Value == this.masterAsesor.Value).Sum(x => x.ValorDia30.Value);
                                    break;
                                case 31:
                                    row["Value"] = this.cierresFiltro.Where(c => c.AsesorID.Value == this.masterAsesor.Value).Sum(x => x.ValorDia31.Value);
                                    break;
                            }

                            #endregion

                            table.Rows.Add(row);
                        }
                    }
                    #endregion
                    #region Filtro para centro de pago
                    if (this.masterCentroPago.ValidID)
                    {
                        for (int i = 1; i <= 31; i++)
                        {
                            DataRow row = row = table.NewRow();
                            row["Member"] = this.masterCentroPago.LabelRsx + ": " + this.masterCentroPago.Value;
                            row["Argument"] = i.ToString();

                            #region carga el valor
                            switch (i)
                            {
                                case 1:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CentroPagoID.Value == this.masterCentroPago.Value).Sum(x => x.ValorDia01.Value);
                                    break;
                                case 2:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CentroPagoID.Value == this.masterCentroPago.Value).Sum(x => x.ValorDia02.Value);
                                    break;
                                case 3:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CentroPagoID.Value == this.masterCentroPago.Value).Sum(x => x.ValorDia03.Value);
                                    break;
                                case 4:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CentroPagoID.Value == this.masterCentroPago.Value).Sum(x => x.ValorDia04.Value);
                                    break;
                                case 5:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CentroPagoID.Value == this.masterCentroPago.Value).Sum(x => x.ValorDia05.Value);
                                    break;
                                case 6:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CentroPagoID.Value == this.masterCentroPago.Value).Sum(x => x.ValorDia06.Value);
                                    break;
                                case 7:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CentroPagoID.Value == this.masterCentroPago.Value).Sum(x => x.ValorDia07.Value);
                                    break;
                                case 8:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CentroPagoID.Value == this.masterCentroPago.Value).Sum(x => x.ValorDia08.Value);
                                    break;
                                case 9:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CentroPagoID.Value == this.masterCentroPago.Value).Sum(x => x.ValorDia09.Value);
                                    break;
                                case 10:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CentroPagoID.Value == this.masterCentroPago.Value).Sum(x => x.ValorDia10.Value);
                                    break;
                                case 11:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CentroPagoID.Value == this.masterCentroPago.Value).Sum(x => x.ValorDia11.Value);
                                    break;
                                case 12:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CentroPagoID.Value == this.masterCentroPago.Value).Sum(x => x.ValorDia12.Value);
                                    break;
                                case 13:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CentroPagoID.Value == this.masterCentroPago.Value).Sum(x => x.ValorDia13.Value);
                                    break;
                                case 14:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CentroPagoID.Value == this.masterCentroPago.Value).Sum(x => x.ValorDia14.Value);
                                    break;
                                case 15:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CentroPagoID.Value == this.masterCentroPago.Value).Sum(x => x.ValorDia15.Value);
                                    break;
                                case 16:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CentroPagoID.Value == this.masterCentroPago.Value).Sum(x => x.ValorDia16.Value);
                                    break;
                                case 17:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CentroPagoID.Value == this.masterCentroPago.Value).Sum(x => x.ValorDia17.Value);
                                    break;
                                case 18:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CentroPagoID.Value == this.masterCentroPago.Value).Sum(x => x.ValorDia18.Value);
                                    break;
                                case 19:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CentroPagoID.Value == this.masterCentroPago.Value).Sum(x => x.ValorDia19.Value);
                                    break;
                                case 20:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CentroPagoID.Value == this.masterCentroPago.Value).Sum(x => x.ValorDia20.Value);
                                    break;
                                case 21:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CentroPagoID.Value == this.masterCentroPago.Value).Sum(x => x.ValorDia21.Value);
                                    break;
                                case 22:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CentroPagoID.Value == this.masterCentroPago.Value).Sum(x => x.ValorDia22.Value);
                                    break;
                                case 23:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CentroPagoID.Value == this.masterCentroPago.Value).Sum(x => x.ValorDia23.Value);
                                    break;
                                case 24:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CentroPagoID.Value == this.masterCentroPago.Value).Sum(x => x.ValorDia24.Value);
                                    break;
                                case 25:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CentroPagoID.Value == this.masterCentroPago.Value).Sum(x => x.ValorDia25.Value);
                                    break;
                                case 26:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CentroPagoID.Value == this.masterCentroPago.Value).Sum(x => x.ValorDia26.Value);
                                    break;
                                case 27:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CentroPagoID.Value == this.masterCentroPago.Value).Sum(x => x.ValorDia27.Value);
                                    break;
                                case 28:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CentroPagoID.Value == this.masterCentroPago.Value).Sum(x => x.ValorDia28.Value);
                                    break;
                                case 29:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CentroPagoID.Value == this.masterCentroPago.Value).Sum(x => x.ValorDia29.Value);
                                    break;
                                case 30:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CentroPagoID.Value == this.masterCentroPago.Value).Sum(x => x.ValorDia30.Value);
                                    break;
                                case 31:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CentroPagoID.Value == this.masterCentroPago.Value).Sum(x => x.ValorDia31.Value);
                                    break;
                            }

                            #endregion

                            table.Rows.Add(row);
                        }
                    }
                    #endregion
                    #region Filtro para zona
                    if (this.masterZona.ValidID)
                    {
                        for (int i = 1; i <= 31; i++)
                        {
                            DataRow row = row = table.NewRow();
                            row["Member"] = this.masterZona.LabelRsx + ": " + this.masterZona.Value;
                            row["Argument"] = i.ToString();

                            #region carga el valor
                            switch (i)
                            {
                                case 1:
                                    row["Value"] = this.cierresFiltro.Where(c => c.ZonaID.Value == this.masterZona.Value).Sum(x => x.ValorDia01.Value);
                                    break;
                                case 2:
                                    row["Value"] = this.cierresFiltro.Where(c => c.ZonaID.Value == this.masterZona.Value).Sum(x => x.ValorDia02.Value);
                                    break;
                                case 3:
                                    row["Value"] = this.cierresFiltro.Where(c => c.ZonaID.Value == this.masterZona.Value).Sum(x => x.ValorDia03.Value);
                                    break;
                                case 4:
                                    row["Value"] = this.cierresFiltro.Where(c => c.ZonaID.Value == this.masterZona.Value).Sum(x => x.ValorDia04.Value);
                                    break;
                                case 5:
                                    row["Value"] = this.cierresFiltro.Where(c => c.ZonaID.Value == this.masterZona.Value).Sum(x => x.ValorDia05.Value);
                                    break;
                                case 6:
                                    row["Value"] = this.cierresFiltro.Where(c => c.ZonaID.Value == this.masterZona.Value).Sum(x => x.ValorDia06.Value);
                                    break;
                                case 7:
                                    row["Value"] = this.cierresFiltro.Where(c => c.ZonaID.Value == this.masterZona.Value).Sum(x => x.ValorDia07.Value);
                                    break;
                                case 8:
                                    row["Value"] = this.cierresFiltro.Where(c => c.ZonaID.Value == this.masterZona.Value).Sum(x => x.ValorDia08.Value);
                                    break;
                                case 9:
                                    row["Value"] = this.cierresFiltro.Where(c => c.ZonaID.Value == this.masterZona.Value).Sum(x => x.ValorDia09.Value);
                                    break;
                                case 10:
                                    row["Value"] = this.cierresFiltro.Where(c => c.ZonaID.Value == this.masterZona.Value).Sum(x => x.ValorDia10.Value);
                                    break;
                                case 11:
                                    row["Value"] = this.cierresFiltro.Where(c => c.ZonaID.Value == this.masterZona.Value).Sum(x => x.ValorDia11.Value);
                                    break;
                                case 12:
                                    row["Value"] = this.cierresFiltro.Where(c => c.ZonaID.Value == this.masterZona.Value).Sum(x => x.ValorDia12.Value);
                                    break;
                                case 13:
                                    row["Value"] = this.cierresFiltro.Where(c => c.ZonaID.Value == this.masterZona.Value).Sum(x => x.ValorDia13.Value);
                                    break;
                                case 14:
                                    row["Value"] = this.cierresFiltro.Where(c => c.ZonaID.Value == this.masterZona.Value).Sum(x => x.ValorDia14.Value);
                                    break;
                                case 15:
                                    row["Value"] = this.cierresFiltro.Where(c => c.ZonaID.Value == this.masterZona.Value).Sum(x => x.ValorDia15.Value);
                                    break;
                                case 16:
                                    row["Value"] = this.cierresFiltro.Where(c => c.ZonaID.Value == this.masterZona.Value).Sum(x => x.ValorDia16.Value);
                                    break;
                                case 17:
                                    row["Value"] = this.cierresFiltro.Where(c => c.ZonaID.Value == this.masterZona.Value).Sum(x => x.ValorDia17.Value);
                                    break;
                                case 18:
                                    row["Value"] = this.cierresFiltro.Where(c => c.ZonaID.Value == this.masterZona.Value).Sum(x => x.ValorDia18.Value);
                                    break;
                                case 19:
                                    row["Value"] = this.cierresFiltro.Where(c => c.ZonaID.Value == this.masterZona.Value).Sum(x => x.ValorDia19.Value);
                                    break;
                                case 20:
                                    row["Value"] = this.cierresFiltro.Where(c => c.ZonaID.Value == this.masterZona.Value).Sum(x => x.ValorDia20.Value);
                                    break;
                                case 21:
                                    row["Value"] = this.cierresFiltro.Where(c => c.ZonaID.Value == this.masterZona.Value).Sum(x => x.ValorDia21.Value);
                                    break;
                                case 22:
                                    row["Value"] = this.cierresFiltro.Where(c => c.ZonaID.Value == this.masterZona.Value).Sum(x => x.ValorDia22.Value);
                                    break;
                                case 23:
                                    row["Value"] = this.cierresFiltro.Where(c => c.ZonaID.Value == this.masterZona.Value).Sum(x => x.ValorDia23.Value);
                                    break;
                                case 24:
                                    row["Value"] = this.cierresFiltro.Where(c => c.ZonaID.Value == this.masterZona.Value).Sum(x => x.ValorDia24.Value);
                                    break;
                                case 25:
                                    row["Value"] = this.cierresFiltro.Where(c => c.ZonaID.Value == this.masterZona.Value).Sum(x => x.ValorDia25.Value);
                                    break;
                                case 26:
                                    row["Value"] = this.cierresFiltro.Where(c => c.ZonaID.Value == this.masterZona.Value).Sum(x => x.ValorDia26.Value);
                                    break;
                                case 27:
                                    row["Value"] = this.cierresFiltro.Where(c => c.ZonaID.Value == this.masterZona.Value).Sum(x => x.ValorDia27.Value);
                                    break;
                                case 28:
                                    row["Value"] = this.cierresFiltro.Where(c => c.ZonaID.Value == this.masterZona.Value).Sum(x => x.ValorDia28.Value);
                                    break;
                                case 29:
                                    row["Value"] = this.cierresFiltro.Where(c => c.ZonaID.Value == this.masterZona.Value).Sum(x => x.ValorDia29.Value);
                                    break;
                                case 30:
                                    row["Value"] = this.cierresFiltro.Where(c => c.ZonaID.Value == this.masterZona.Value).Sum(x => x.ValorDia30.Value);
                                    break;
                                case 31:
                                    row["Value"] = this.cierresFiltro.Where(c => c.ZonaID.Value == this.masterZona.Value).Sum(x => x.ValorDia31.Value);
                                    break;
                            }

                            #endregion

                            table.Rows.Add(row);
                        }
                    }
                    #endregion
                    #region Filtro para comprador de cartera
                    if (this.masterComprador.ValidID)
                    {
                        for (int i = 1; i <= 31; i++)
                        {
                            DataRow row = row = table.NewRow();
                            row["Member"] = this.masterComprador.LabelRsx + ": " + this.masterComprador.Value;
                            row["Argument"] = i.ToString();

                            #region carga el valor
                            switch (i)
                            {
                                case 1:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CompradorCarteraID.Value == this.masterComprador.Value).Sum(x => x.ValorDia01.Value);
                                    break;
                                case 2:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CompradorCarteraID.Value == this.masterComprador.Value).Sum(x => x.ValorDia02.Value);
                                    break;
                                case 3:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CompradorCarteraID.Value == this.masterComprador.Value).Sum(x => x.ValorDia03.Value);
                                    break;
                                case 4:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CompradorCarteraID.Value == this.masterComprador.Value).Sum(x => x.ValorDia04.Value);
                                    break;
                                case 5:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CompradorCarteraID.Value == this.masterComprador.Value).Sum(x => x.ValorDia05.Value);
                                    break;
                                case 6:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CompradorCarteraID.Value == this.masterComprador.Value).Sum(x => x.ValorDia06.Value);
                                    break;
                                case 7:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CompradorCarteraID.Value == this.masterComprador.Value).Sum(x => x.ValorDia07.Value);
                                    break;
                                case 8:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CompradorCarteraID.Value == this.masterComprador.Value).Sum(x => x.ValorDia08.Value);
                                    break;
                                case 9:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CompradorCarteraID.Value == this.masterComprador.Value).Sum(x => x.ValorDia09.Value);
                                    break;
                                case 10:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CompradorCarteraID.Value == this.masterComprador.Value).Sum(x => x.ValorDia10.Value);
                                    break;
                                case 11:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CompradorCarteraID.Value == this.masterComprador.Value).Sum(x => x.ValorDia11.Value);
                                    break;
                                case 12:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CompradorCarteraID.Value == this.masterComprador.Value).Sum(x => x.ValorDia12.Value);
                                    break;
                                case 13:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CompradorCarteraID.Value == this.masterComprador.Value).Sum(x => x.ValorDia13.Value);
                                    break;
                                case 14:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CompradorCarteraID.Value == this.masterComprador.Value).Sum(x => x.ValorDia14.Value);
                                    break;
                                case 15:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CompradorCarteraID.Value == this.masterComprador.Value).Sum(x => x.ValorDia15.Value);
                                    break;
                                case 16:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CompradorCarteraID.Value == this.masterComprador.Value).Sum(x => x.ValorDia16.Value);
                                    break;
                                case 17:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CompradorCarteraID.Value == this.masterComprador.Value).Sum(x => x.ValorDia17.Value);
                                    break;
                                case 18:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CompradorCarteraID.Value == this.masterComprador.Value).Sum(x => x.ValorDia18.Value);
                                    break;
                                case 19:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CompradorCarteraID.Value == this.masterComprador.Value).Sum(x => x.ValorDia19.Value);
                                    break;
                                case 20:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CompradorCarteraID.Value == this.masterComprador.Value).Sum(x => x.ValorDia20.Value);
                                    break;
                                case 21:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CompradorCarteraID.Value == this.masterComprador.Value).Sum(x => x.ValorDia21.Value);
                                    break;
                                case 22:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CompradorCarteraID.Value == this.masterComprador.Value).Sum(x => x.ValorDia22.Value);
                                    break;
                                case 23:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CompradorCarteraID.Value == this.masterComprador.Value).Sum(x => x.ValorDia23.Value);
                                    break;
                                case 24:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CompradorCarteraID.Value == this.masterComprador.Value).Sum(x => x.ValorDia24.Value);
                                    break;
                                case 25:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CompradorCarteraID.Value == this.masterComprador.Value).Sum(x => x.ValorDia25.Value);
                                    break;
                                case 26:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CompradorCarteraID.Value == this.masterComprador.Value).Sum(x => x.ValorDia26.Value);
                                    break;
                                case 27:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CompradorCarteraID.Value == this.masterComprador.Value).Sum(x => x.ValorDia27.Value);
                                    break;
                                case 28:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CompradorCarteraID.Value == this.masterComprador.Value).Sum(x => x.ValorDia28.Value);
                                    break;
                                case 29:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CompradorCarteraID.Value == this.masterComprador.Value).Sum(x => x.ValorDia29.Value);
                                    break;
                                case 30:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CompradorCarteraID.Value == this.masterComprador.Value).Sum(x => x.ValorDia30.Value);
                                    break;
                                case 31:
                                    row["Value"] = this.cierresFiltro.Where(c => c.CompradorCarteraID.Value == this.masterComprador.Value).Sum(x => x.ValorDia31.Value);
                                    break;
                            }

                            #endregion

                            table.Rows.Add(row);
                        }
                    }
                    #endregion
                }
                return table;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadisticasDiarias.cs", "CreateChartData"));
                return table;
            }
        }

        /// <summary>
        /// Carga la tabla para mostrar el detalle del filtro
        /// </summary>
        /// <param name="dia">Dia de consulta</param>
        /// <returns></returns>
        private DataTable GetDetailTable(int dia)
        {
            // Create an empty table.
            DataTable table = new DataTable("TableDetail");
            table.Columns.Add("Argument");
            table.Columns.Add("Value", typeof(Decimal));

            try
            {
                decimal value = 0;
                List<string> filterKeys = new List<string>();
                List<DTO_ccCierreDia> filtroAux = ObjectCopier.Clone(this.cierresFiltro);

                foreach (DTO_ccCierreDia cTemp in filtroAux)
                {
                    switch (dia)
                    #region Carga el valor del dia correspondiente en el dia 1
                    {
                        case 2:
                            cTemp.ValorDia01.Value = cTemp.ValorDia02.Value.HasValue ? cTemp.ValorDia02.Value : 0;
                            break;
                        case 3:
                            cTemp.ValorDia01.Value = cTemp.ValorDia03.Value.HasValue ? cTemp.ValorDia03.Value : 0;
                            break;
                        case 4:
                            cTemp.ValorDia01.Value = cTemp.ValorDia04.Value.HasValue ? cTemp.ValorDia04.Value : 0;
                            break;
                        case 5:
                            cTemp.ValorDia01.Value = cTemp.ValorDia05.Value.HasValue ? cTemp.ValorDia05.Value : 0;
                            break;
                        case 6:
                            cTemp.ValorDia01.Value = cTemp.ValorDia06.Value.HasValue ? cTemp.ValorDia06.Value : 0;
                            break;
                        case 7:
                            cTemp.ValorDia01.Value = cTemp.ValorDia07.Value.HasValue ? cTemp.ValorDia07.Value : 0;
                            break;
                        case 8:
                            cTemp.ValorDia01.Value = cTemp.ValorDia08.Value.HasValue ? cTemp.ValorDia08.Value : 0;
                            break;
                        case 9:
                            cTemp.ValorDia01.Value = cTemp.ValorDia09.Value.HasValue ? cTemp.ValorDia09.Value : 0;
                            break;
                        case 10:
                            cTemp.ValorDia01.Value = cTemp.ValorDia10.Value.HasValue ? cTemp.ValorDia10.Value : 0;
                            break;
                        case 11:
                            cTemp.ValorDia01.Value = cTemp.ValorDia11.Value.HasValue ? cTemp.ValorDia11.Value : 0;
                            break;
                        case 12:
                            cTemp.ValorDia01.Value = cTemp.ValorDia12.Value.HasValue ? cTemp.ValorDia12.Value : 0;
                            break;
                        case 13:
                            cTemp.ValorDia01.Value = cTemp.ValorDia13.Value.HasValue ? cTemp.ValorDia13.Value : 0;
                            break;
                        case 14:
                            cTemp.ValorDia01.Value = cTemp.ValorDia14.Value.HasValue ? cTemp.ValorDia14.Value : 0;
                            break;
                        case 15:
                            cTemp.ValorDia01.Value = cTemp.ValorDia15.Value.HasValue ? cTemp.ValorDia15.Value : 0;
                            break;
                        case 16:
                            cTemp.ValorDia01.Value = cTemp.ValorDia16.Value.HasValue ? cTemp.ValorDia16.Value : 0;
                            break;
                        case 17:
                            cTemp.ValorDia01.Value = cTemp.ValorDia17.Value.HasValue ? cTemp.ValorDia17.Value : 0;
                            break;
                        case 18:
                            cTemp.ValorDia01.Value = cTemp.ValorDia18.Value.HasValue ? cTemp.ValorDia18.Value : 0;
                            break;
                        case 19:
                            cTemp.ValorDia01.Value = cTemp.ValorDia19.Value.HasValue ? cTemp.ValorDia19.Value : 0;
                            break;
                        case 20:
                            cTemp.ValorDia01.Value = cTemp.ValorDia20.Value.HasValue ? cTemp.ValorDia20.Value : 0;
                            break;
                        case 21:
                            cTemp.ValorDia01.Value = cTemp.ValorDia21.Value.HasValue ? cTemp.ValorDia21.Value : 0;
                            break;
                        case 22:
                            cTemp.ValorDia01.Value = cTemp.ValorDia22.Value.HasValue ? cTemp.ValorDia22.Value : 0;
                            break;
                        case 23:
                            cTemp.ValorDia01.Value = cTemp.ValorDia23.Value.HasValue ? cTemp.ValorDia23.Value : 0;
                            break;
                        case 24:
                            cTemp.ValorDia01.Value = cTemp.ValorDia24.Value.HasValue ? cTemp.ValorDia24.Value : 0;
                            break;
                        case 25:
                            cTemp.ValorDia01.Value = cTemp.ValorDia25.Value.HasValue ? cTemp.ValorDia25.Value : 0;
                            break;
                        case 26:
                            cTemp.ValorDia01.Value = cTemp.ValorDia26.Value.HasValue ? cTemp.ValorDia26.Value : 0;
                            break;
                        case 27:
                            cTemp.ValorDia01.Value = cTemp.ValorDia27.Value.HasValue ? cTemp.ValorDia27.Value : 0;
                            break;
                        case 28:
                            cTemp.ValorDia01.Value = cTemp.ValorDia28.Value.HasValue ? cTemp.ValorDia28.Value : 0;
                            break;
                        case 29:
                            cTemp.ValorDia01.Value = cTemp.ValorDia29.Value.HasValue ? cTemp.ValorDia29.Value : 0;
                            break;
                        case 30:
                            cTemp.ValorDia01.Value = cTemp.ValorDia30.Value.HasValue ? cTemp.ValorDia30.Value : 0;
                            break;
                        case 31:
                            cTemp.ValorDia01.Value = cTemp.ValorDia31.Value.HasValue ? cTemp.ValorDia31.Value : 0;
                            break;
                    }
                    #endregion
                }

                #region Carga los filtros
                switch (this.rbDetails.SelectedIndex)
                {
                    #region Línea de crédito
                    case 0:
                        filterKeys = (from f in filtroAux select f.LineaCreditoID.Value).Distinct().ToList();
                        foreach (string key in filterKeys)
                        {
                            value = (from f in filtroAux where f.LineaCreditoID.Value == key select f.ValorDia01.Value.Value).Sum();

                            DataRow row = row = table.NewRow();
                            row["Argument"] = key;
                            row["Value"] = value;
                            
                            table.Rows.Add(row);
                        }

                        break;
                    #endregion
                    #region Asesor
                    case 1:
                        filterKeys = (from f in filtroAux select f.AsesorID.Value).Distinct().ToList();
                        foreach (string key in filterKeys)
                        {
                            value = (from f in filtroAux where f.AsesorID.Value == key select f.ValorDia01.Value.Value).Sum();

                            DataRow row = row = table.NewRow();
                            row["Argument"] = key;
                            row["Value"] = value;

                            table.Rows.Add(row);
                        }
                        break;
                    #endregion
                    #region Centro de pago
                    case 2:
                        filterKeys = (from f in filtroAux select f.CentroPagoID.Value).Distinct().ToList();
                        foreach (string key in filterKeys)
                        {
                            value = (from f in filtroAux where f.CentroPagoID.Value == key select f.ValorDia01.Value.Value).Sum();

                            DataRow row = row = table.NewRow();
                            row["Argument"] = key;
                            row["Value"] = value;

                            table.Rows.Add(row);
                        }
                        break;
                    #endregion
                    #region Zona
                    case 3:
                        filterKeys = (from f in filtroAux select f.ZonaID.Value).Distinct().ToList();
                        foreach (string key in filterKeys)
                        {
                            value = (from f in filtroAux where f.ZonaID.Value == key select f.ValorDia01.Value.Value).Sum();

                            DataRow row = row = table.NewRow();
                            row["Argument"] = key;
                            row["Value"] = value;

                            table.Rows.Add(row);
                        }
                        break;
                    #endregion
                    #region Comprador
                    case 4:
                        filterKeys = (from f in filtroAux select f.CompradorCarteraID.Value).Distinct().ToList();
                        foreach (string key in filterKeys)
                        {
                            if (!string.IsNullOrWhiteSpace(key))
                            {
                                value = (from f in filtroAux where f.CompradorCarteraID.Value == key select f.ValorDia01.Value.Value).Sum();

                                DataRow row = row = table.NewRow();
                                row["Argument"] = key;
                                row["Value"] = value;

                                table.Rows.Add(row);
                            }
                        }
                        break;
                    #endregion
                    #region Plazo
                    case 5:
                        filterKeys = (from f in filtroAux select f.Plazo.Value.Value.ToString()).Distinct().ToList();
                        foreach (string key in filterKeys)
                        {
                            value = (from f in filtroAux where f.Plazo.Value.Value.ToString() == key select f.ValorDia01.Value.Value).Sum();

                            DataRow row = row = table.NewRow();
                            row["Argument"] = key;
                            row["Value"] = value;

                            table.Rows.Add(row);
                        }
                        break;
                    #endregion
                }
                #endregion
                
                return table;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadisticasDiarias.cs", "CreateChartDetail"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadisticasDiarias.cs", "Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadisticasDiarias.cs", "Form_FormClosing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadisticasDiarias.cs", "Form_FormClosed"));
            }
        }

        #endregion Eventos MDI

        #region Eventos formulario y charts

        /// <summary>
        /// Evento que vuelve a traer información de base de datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtPeriod_Leave(object sender, EventArgs e)
        {
            try
            {
                bool newDate = true;

                if (this.cierres != null && this.cierres.Count > 0 && this.dtPeriod.DateTime != this.periodo)
                {
                    string msgTitleSearch = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_GetData));
                    string msgNewSearch = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewSearch);

                    if (MessageBox.Show(msgNewSearch, msgTitleSearch, MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        newDate = false;
                        this.dtPeriod.DateTime = this.periodo;
                    }
                }

                if (newDate)
                {
                    #region Carga los nuevos datos
                    this.periodo = this.dtPeriod.DateTime;

                    this.cmbTipo.EditValue = 1;
                    this.masterLineaCredito.Value = string.Empty;
                    this.masterAsesor.Value = string.Empty;
                    this.masterCentroPago.Value = string.Empty;
                    this.masterComprador.Value = string.Empty;
                    this.masterZona.Value = string.Empty;
                    this.comboPlazo.SelectedIndex = 0;

                    this.rbDetails.SelectedIndex = 0;
                    this.chartDetail.Series.Clear();

                    this.GetData();

                    this.cierresFiltro = this.cierres.Where(c => c.TipoDato.Value.Value == 1).ToList();
                    DataTable table = this.GetChartTable();

                    this.chartCierre.DataSource = table;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadisticasDiarias.cs", "dtPeriod_Leave"));
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

                #region Plazo
                if (this.comboPlazo.SelectedItem != null && !string.IsNullOrWhiteSpace(this.comboPlazo.SelectedItem.ToString()))
                {
                    Series sPlazo = chart.GetSeriesByName(this.lblPlazo.Text + ": " + this.comboPlazo.SelectedItem);
                    if (sPlazo != null)
                    {
                        sPlazo.View.Color = Color.Blue;
                        sPlazo.ChangeView(ViewType.Line);
                    }
                }
                #endregion
                #region Linea de credito
                if (this.masterLineaCredito.ValidID)
                {
                    Series linea = chart.GetSeriesByName(this.masterLineaCredito.LabelRsx + ": " + this.masterLineaCredito.Value);
                    if (linea != null)
                    {
                        linea.View.Color = Color.GreenYellow;
                        linea.ChangeView(ViewType.Line);
                    }
                }
                #endregion
                #region Asesor
                if (this.masterAsesor.ValidID)
                {
                    Series asesor = chart.GetSeriesByName(this.masterAsesor.LabelRsx + ": " + this.masterAsesor.Value);
                    if (asesor != null)
                    {
                        asesor.View.Color = Color.Red;
                        asesor.ChangeView(ViewType.Line);
                    }
                }
                #endregion
                #region Centro de pago
                if (this.masterCentroPago.ValidID)
                {
                    Series centroPago = chart.GetSeriesByName(this.masterCentroPago.LabelRsx + ": " + this.masterCentroPago.Value);
                    if (centroPago != null)
                    {
                        centroPago.View.Color = Color.Pink;
                        centroPago.ChangeView(ViewType.Line);
                    }
                }
                #endregion
                #region Zona
                if (this.masterZona.ValidID)
                {
                    Series zona = chart.GetSeriesByName(this.masterZona.LabelRsx + ": " + this.masterZona.Value);
                    if (zona != null)
                    {
                        zona.View.Color = Color.Green;
                        zona.ChangeView(ViewType.Line);
                    }
                }
                #endregion
                #region Comprador de cartera
                if (this.masterComprador.ValidID)
                {
                    Series comprador = chart.GetSeriesByName(this.masterComprador.LabelRsx + ": " + this.masterComprador.Value);
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
                    if (this.cmbTipo.EditValue == "1")
                        s.LegendPointOptions.ValueNumericOptions.Format = NumericFormat.Number;
                    else
                    {
                        s.LegendPointOptions.ValueNumericOptions.Format = NumericFormat.Currency;
                        s.CrosshairLabelPattern = "{A} : {V:C}";
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadisticasDiarias.cs", "chart_BoundDataChanged"));
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
                    int dia = Convert.ToInt32(point.Argument);
                    this.AddDetailChart(dia);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EstadisticasDiarias.cs", "chartCierre_Click"));
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
                byte tipo = Convert.ToByte(this.cmbTipo.EditValue);

                this.cierresFiltro = this.cierres.Where(c => c.TipoDato.Value.Value == tipo).ToList();
                DataTable table = this.GetChartTable();

                this.chartCierre.DataSource = table;

                //Limpia el control de detalles
                if (this.cmbTipo.EditValue.ToString() != this.tipoDato)
                {
                    this.tipoDato = this.cmbTipo.EditValue.ToString();
                    this.chartDetail.Titles.Clear();
                    this.chartDetail.Series.Clear();

                    //Asigna formatos
                    XYDiagram diagram = (XYDiagram)this.chartCierre.Diagram;

                    if (this.cmbTipo.EditValue == "1")
                    {
                        this.chartCierre.SeriesTemplate.LegendPointOptions.ValueNumericOptions.Format = NumericFormat.Number;

                        diagram.AxisY.NumericOptions.Format = NumericFormat.Number;
                        diagram.AxisY.GridSpacingAuto = false;
                        diagram.AxisY.GridSpacing = 1;
                    }
                    else
                    {
                        this.chartCierre.SeriesTemplate.LegendPointOptions.ValueNumericOptions.Format = NumericFormat.Currency;

                        diagram.AxisY.NumericOptions.Format = NumericFormat.Currency;
                        diagram.AxisY.GridSpacingAuto = true;
                    }
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
