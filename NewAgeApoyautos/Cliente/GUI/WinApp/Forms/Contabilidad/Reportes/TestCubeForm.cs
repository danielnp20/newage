using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraPivotGrid;
using DevExpress.XtraEditors;
using DevExpress.XtraCharts;
using DevExpress.Utils;
using System.Reflection;
using DevExpress.XtraBars;
using System.IO;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class TestCubeForm : Form
    {
        #region Variables
        private BaseController _bc = BaseController.GetInstance();
        private bool _canOpen;
        private PivotGridControl _pivotTable; 
        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Report based on data from the Cube (grid and chart)
        /// </summary>
        public TestCubeForm()
        {
            string openCube = _bc.GetControlValue(AppControl.IndicadorCubo);
            if (openCube == "0")
            {
                this._canOpen = false;
                this.VisibleChanged += new EventHandler(this.TestCubeForm_VisibleChanged);
            }
            else
            {
                this._canOpen = true;

                InitializeComponent();

                this.WindowState = FormWindowState.Maximized;

                #region ToolBar
                BarManager barManager = new BarManager();
                barManager.Form = this;
                barManager.AllowCustomization = false;

                barManager.BeginUpdate();

                Bar bar = new Bar(barManager, "Main menu");
                bar.DockStyle = BarDockStyle.Top;
                bar.OptionsBar.UseWholeRow = true;

                barManager.Images = iconsList;

                BarButtonItem btnExportXls = new BarButtonItem(barManager, "Export to Excel");
                btnExportXls.Hint = "Export to Excel";
                btnExportXls.Id = 0;
                btnExportXls.ImageIndex = 0;
                btnExportXls.ImageIndexDisabled = 0;
                btnExportXls.Name = "btnExportXls";
                bar.AddItem(btnExportXls);

                btnExportXls.ItemClick += new ItemClickEventHandler(btnExportXls_ItemClick);

                barManager.EndUpdate();
                #endregion

                #region Split Container
                SplitContainerControl splitCont = new SplitContainerControl();
                splitCont.Location = new Point(0, 0);
                splitCont.Size = new System.Drawing.Size(1000, 500);
                splitCont.SplitterPosition = 400;
                splitCont.Dock = DockStyle.Fill;
                splitCont.Horizontal = false;
                splitCont.CollapsePanel = SplitCollapsePanel.Panel2;
                splitCont.FixedPanel = SplitFixedPanel.None;
                splitCont.Panel1.Name = "tablePanel";
                splitCont.Panel2.Name = "chartPanel";
                this.Controls.Add(splitCont);
                #endregion

                _pivotTable = new PivotGridControl();
                _pivotTable.Dock = DockStyle.Fill;
                _pivotTable.OptionsView.ShowTotalsForSingleValues = true;
                splitCont.Panel1.Controls.Add(_pivotTable);

                _pivotTable.OLAPConnectionString = "Provider=MSOLAP;Data Source=192.168.0.17;Initial Catalog=ASProject;Cube Name=dsvASProject;User=Administrator;Password=newage!2012;";
                //pivotTable.OLAPConnectionString = "Provider=MSOLAP;Data Source=http://192.168.0.17:8895/OLAP/msmdpump.dll; Initial Catalog=ASProject;Cube Name=dsvASProject;";
                //pivotTable.OLAPConnectionString = "Provider=MSOLAP;Data Source=http://190.147.102.24:8895/OLAP/msmdpump.dll; Initial Catalog=ASProject;Cube Name=dsvASProject;";

                _pivotTable.FieldValueDisplayText += new PivotFieldDisplayTextEventHandler(pivotTable_FieldValueDisplayText);

                _pivotTable.ToolTipController = new ToolTipController();
                _pivotTable.ToolTipController.GetActiveObjectInfo += new DevExpress.Utils.ToolTipControllerGetActiveObjectInfoEventHandler(ToolTipController_GetActiveObjectInfo);

                int levelsQty = 10;

                #region Pivot table Data field
                PivotGridField pivotDataField_MovML = new PivotGridField("[Measures].[MovimientoML]", PivotArea.DataArea);
                pivotDataField_MovML.AllowedAreas = PivotGridAllowedAreas.DataArea;
                pivotDataField_MovML.AreaIndex = 0;
                pivotDataField_MovML.Caption = "Movimiento Mda Loc";
                pivotDataField_MovML.Name = "fieldMovML";
                pivotDataField_MovML.Options.AllowDrag = DevExpress.Utils.DefaultBoolean.False;
                pivotDataField_MovML.Options.AllowDragInCustomizationForm = DevExpress.Utils.DefaultBoolean.False;
                pivotDataField_MovML.Options.AllowEdit = false;
                pivotDataField_MovML.CellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                pivotDataField_MovML.CellFormat.FormatString = "{0:#,0.00}";
                _pivotTable.Fields.Add(pivotDataField_MovML);
                #endregion

                #region Pivot table Filter fields
                PivotGridField pivotFilterField_Year = new PivotGridField("[dim_Date].[Year].[Year]", PivotArea.FilterArea);
                pivotFilterField_Year.AllowedAreas = PivotGridAllowedAreas.FilterArea;
                pivotFilterField_Year.AreaIndex = 0;
                pivotFilterField_Year.FilterValues.FilterType = PivotFilterType.Included;
                pivotFilterField_Year.Caption = "Year";
                pivotFilterField_Year.Name = "fieldYear";
                pivotFilterField_Year.Options.AllowDrag = DevExpress.Utils.DefaultBoolean.False;
                pivotFilterField_Year.Options.AllowDragInCustomizationForm = DevExpress.Utils.DefaultBoolean.False;
                pivotFilterField_Year.Options.AllowEdit = false;
                pivotFilterField_Year.Options.AllowExpand = DevExpress.Utils.DefaultBoolean.False;
                pivotFilterField_Year.ValueFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                pivotFilterField_Year.ValueFormat.FormatString = "yyyy";
                pivotFilterField_Year.FilterValues.ValuesIncluded = new object[] { new DateTime(2011, 1, 1) };
                _pivotTable.Fields.Add(pivotFilterField_Year);

                PivotGridField pivotFilterField_BalanceTipo = new PivotGridField("[dim_coBalanceTipo].[BalanceTipoID].[BalanceTipoID]", PivotArea.FilterArea);
                pivotFilterField_BalanceTipo.AllowedAreas = PivotGridAllowedAreas.FilterArea;
                pivotFilterField_BalanceTipo.AreaIndex = 1;
                pivotFilterField_BalanceTipo.FilterValues.FilterType = PivotFilterType.Excluded;
                pivotFilterField_BalanceTipo.Caption = "Balance Tipo";
                pivotFilterField_BalanceTipo.Name = "fieldBalanceTipo";
                pivotFilterField_BalanceTipo.Options.AllowDrag = DevExpress.Utils.DefaultBoolean.False;
                pivotFilterField_BalanceTipo.Options.AllowDragInCustomizationForm = DevExpress.Utils.DefaultBoolean.False;
                pivotFilterField_BalanceTipo.Options.AllowEdit = false;
                pivotFilterField_BalanceTipo.Options.AllowExpand = DevExpress.Utils.DefaultBoolean.False;
                _pivotTable.Fields.Add(pivotFilterField_BalanceTipo);
                #endregion

                #region Pivot table Column fields
                PivotGridField pivotColumnField_Month = new PivotGridField("[dim_Date].[Month].[Month]", PivotArea.ColumnArea);
                pivotColumnField_Month.AllowedAreas = PivotGridAllowedAreas.ColumnArea;
                pivotColumnField_Month.AreaIndex = 0;
                pivotColumnField_Month.Caption = "Month";
                pivotColumnField_Month.Name = "fieldMonth";
                pivotColumnField_Month.Options.AllowDrag = DevExpress.Utils.DefaultBoolean.False;
                pivotColumnField_Month.Options.AllowDragInCustomizationForm = DevExpress.Utils.DefaultBoolean.False;
                pivotColumnField_Month.Options.AllowEdit = false;
                pivotColumnField_Month.ValueFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                pivotColumnField_Month.ValueFormat.FormatString = "MMM, yyyy";
                pivotColumnField_Month.Width = 90;
                _pivotTable.Fields.Add(pivotColumnField_Month);
                #endregion

                #region Pivot Table Row field
                #region Pivot table Cuenta fields
                PivotGridField pivotRowField_Cuenta = new PivotGridField("[Dim Cuenta].[Hierarchy].[Level1]", PivotArea.RowArea);
                pivotRowField_Cuenta.AllowedAreas = PivotGridAllowedAreas.RowArea | PivotGridAllowedAreas.FilterArea;
                pivotRowField_Cuenta.AreaIndex = 0;
                pivotRowField_Cuenta.Caption = "Cuenta Level1";
                pivotRowField_Cuenta.Name = "fieldCuentaLevel1";
                pivotRowField_Cuenta.Options.AllowEdit = false;
                pivotRowField_Cuenta.ExpandedInFieldsGroup = false;
                _pivotTable.Fields.Add(pivotRowField_Cuenta);

                for (int i = 2; i <= levelsQty; i++)
                {
                    pivotRowField_Cuenta = new PivotGridField("[Dim Cuenta].[Hierarchy].[Level" + i.ToString() + "]", PivotArea.RowArea);
                    pivotRowField_Cuenta.AllowedAreas = PivotGridAllowedAreas.RowArea | PivotGridAllowedAreas.FilterArea;
                    pivotRowField_Cuenta.AreaIndex = i - 1;
                    pivotRowField_Cuenta.Caption = "Level" + i.ToString();
                    pivotRowField_Cuenta.Name = "fieldCuentaLevel" + i.ToString();
                    pivotRowField_Cuenta.Options.AllowEdit = false;
                    pivotRowField_Cuenta.ExpandedInFieldsGroup = false;
                    pivotRowField_Cuenta.Visible = false;
                    _pivotTable.Fields.Add(pivotRowField_Cuenta);
                };
                #endregion

                #region Pivot table CentroCosto fields
                PivotGridField pivotRowField_CentroCosto = new PivotGridField("[Dim Co Centro Costo].[Hierarchy].[Level1]", PivotArea.FilterArea);
                pivotRowField_CentroCosto.AllowedAreas = PivotGridAllowedAreas.RowArea | PivotGridAllowedAreas.FilterArea;
                pivotRowField_CentroCosto.AreaIndex = 2;
                pivotRowField_CentroCosto.Caption = "CentroCosto Level1";
                pivotRowField_CentroCosto.Name = "fieldCentroCostoLevel1";
                pivotRowField_CentroCosto.Options.AllowEdit = false;
                pivotRowField_CentroCosto.ExpandedInFieldsGroup = false;
                _pivotTable.Fields.Add(pivotRowField_CentroCosto);

                for (int i = 2; i <= levelsQty; i++)
                {
                    pivotRowField_CentroCosto = new PivotGridField("[Dim Co Centro Costo].[Hierarchy].[Level" + i.ToString() + "]", PivotArea.FilterArea);
                    pivotRowField_CentroCosto.AllowedAreas = PivotGridAllowedAreas.RowArea | PivotGridAllowedAreas.FilterArea;
                    pivotRowField_CentroCosto.AreaIndex = i + 1;
                    pivotRowField_CentroCosto.Caption = "Level" + i.ToString();
                    pivotRowField_CentroCosto.Name = "fieldCentroCostoLevel" + i.ToString();
                    pivotRowField_CentroCosto.Options.AllowEdit = false;
                    pivotRowField_CentroCosto.ExpandedInFieldsGroup = false;
                    pivotRowField_CentroCosto.Visible = false;
                    _pivotTable.Fields.Add(pivotRowField_CentroCosto);
                };
                #endregion

                #region Pivot table Proyecto fields
                PivotGridField pivotRowField_Proyecto;
                pivotRowField_Proyecto = new PivotGridField("[Dim Co Proyecto].[Hierarchy].[Level1]", PivotArea.FilterArea);
                pivotRowField_Proyecto.AllowedAreas = PivotGridAllowedAreas.RowArea | PivotGridAllowedAreas.FilterArea;
                pivotRowField_Proyecto.AreaIndex = 12;
                pivotRowField_Proyecto.Caption = "Proyecto Level1";
                pivotRowField_Proyecto.Name = "fieldProyectoLevel1";
                pivotRowField_Proyecto.Options.AllowEdit = false;
                pivotRowField_Proyecto.ExpandedInFieldsGroup = false;
                _pivotTable.Fields.Add(pivotRowField_Proyecto);

                for (int i = 2; i <= levelsQty; i++)
                {
                    pivotRowField_Proyecto = new PivotGridField("[Dim Co Proyecto].[Hierarchy].[Level" + i.ToString() + "]", PivotArea.FilterArea);
                    pivotRowField_Proyecto.AllowedAreas = PivotGridAllowedAreas.RowArea | PivotGridAllowedAreas.FilterArea;
                    pivotRowField_Proyecto.AreaIndex = 11 + i;
                    pivotRowField_Proyecto.Caption = "Level" + i.ToString();
                    pivotRowField_Proyecto.Name = "fieldProyectoLevel" + i.ToString();
                    pivotRowField_Proyecto.Options.AllowEdit = false;
                    pivotRowField_Proyecto.ExpandedInFieldsGroup = false;
                    pivotRowField_Proyecto.Visible = false;
                    _pivotTable.Fields.Add(pivotRowField_Proyecto);
                }
                #endregion

                #region Pivot table LineaPre fields
                PivotGridField pivotRowField_LineaPre;
                pivotRowField_LineaPre = new PivotGridField("[Dim Pl Linea Presupuesto].[Hierarchy].[Level1]", PivotArea.FilterArea);
                pivotRowField_LineaPre.AllowedAreas = PivotGridAllowedAreas.RowArea | PivotGridAllowedAreas.FilterArea;
                pivotRowField_LineaPre.AreaIndex = 22;
                pivotRowField_LineaPre.Caption = "Linea Pre Level1";
                pivotRowField_LineaPre.Name = "fieldLineaPreLevel1";
                pivotRowField_LineaPre.Options.AllowEdit = false;
                pivotRowField_LineaPre.ExpandedInFieldsGroup = false;
                _pivotTable.Fields.Add(pivotRowField_LineaPre);

                for (int i = 2; i <= levelsQty; i++)
                {
                    pivotRowField_LineaPre = new PivotGridField("[Dim Pl Linea Presupuesto].[Hierarchy].[Level" + i.ToString() + "]", PivotArea.FilterArea);
                    pivotRowField_LineaPre.AllowedAreas = PivotGridAllowedAreas.RowArea | PivotGridAllowedAreas.FilterArea;
                    pivotRowField_LineaPre.AreaIndex = 21 + i;
                    pivotRowField_LineaPre.Caption = "Level" + i.ToString();
                    pivotRowField_LineaPre.Name = "fieldLineaPreLevel" + i.ToString();
                    pivotRowField_LineaPre.Options.AllowEdit = false;
                    pivotRowField_LineaPre.ExpandedInFieldsGroup = false;
                    pivotRowField_LineaPre.Visible = false;
                    _pivotTable.Fields.Add(pivotRowField_LineaPre);
                }
                #endregion
                #endregion

                #region Chart
                ChartControl chart = new ChartControl();
                chart.Dock = DockStyle.Fill;
                chart.DataSource = _pivotTable;
                //pivotTable.CustomChartDataSourceData +=new PivotCustomChartDataSourceDataEventHandler(pivotTable_CustomChartDataSourceData);
                _pivotTable.OptionsChartDataSource.SelectionOnly = true;
                _pivotTable.OptionsChartDataSource.FieldValuesProvideMode = PivotChartFieldValuesProvideMode.DisplayText;
                ((XYDiagram)chart.Diagram).AxisY.NumericOptions.Format = NumericFormat.Currency;
                ((XYDiagram)chart.Diagram).AxisY.NumericOptions.Precision = 0;
                ((XYDiagram)chart.Diagram).AxisY.Label.Antialiasing = true;
                ((XYDiagram)chart.Diagram).AxisX.Label.Antialiasing = true;
                ((XYDiagram)chart.Diagram).AxisX.Label.Angle = -30;
                ((XYDiagram)chart.Diagram).AxisX.GridLines.Visible = true;
                ((XYDiagram)chart.Diagram).AxisX.GridLines.LineStyle.DashStyle = DashStyle.Dash;
                ((XYDiagram)chart.Diagram).DefaultPane.Shadow.Color = Color.LightGray;
                ((XYDiagram)chart.Diagram).DefaultPane.Shadow.Visible = true;
                chart.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Right;
                chart.Legend.AlignmentVertical = LegendAlignmentVertical.BottomOutside;
                chart.Legend.Direction = LegendDirection.LeftToRight;
                chart.Legend.Shadow.Color = Color.LightGray;
                chart.Legend.Shadow.Visible = true;
                chart.SeriesDataMember = "Series";
                chart.SeriesTemplate.ArgumentDataMember = "Arguments";
                chart.PivotGridDataSourceOptions.RetrieveDataByColumns = false;
                chart.SeriesTemplate.Label.PointOptions.ValueNumericOptions.Precision = 2;
                chart.SeriesTemplate.Label.PointOptions.ValueNumericOptions.Format = NumericFormat.Currency;

                splitCont.Panel2.Controls.Add(chart);
                #endregion
            }
        } 

        #endregion

        #region Eventos

        /// <summary>
        /// Format data for the chart
        /// </summary>
        private void pivotTable_CustomChartDataSourceData(object sender, PivotCustomChartDataSourceDataEventArgs e)
        {
            if (e.ItemType == PivotChartItemType.CellItem)
            {
                e.Value = Math.Round(Convert.ToDecimal(e.CellInfo.Value), 2);
            }
        }

        /// <summary>
        /// Provieds field caption depending on hierarchy level (lowest level among already opened fields -> detailed caption)
        /// </summary>
        private void pivotTable_FieldValueDisplayText(object sender, PivotFieldDisplayTextEventArgs e)
        {
            try
            {
                if (e.Field != null && e.Field.Area == PivotArea.RowArea && e.FieldIndex >= 0)
                {
                    string pivotFieldUniqueName = _pivotTable.GetFieldValueOLAPMember(e.Field, e.FieldIndex - e.Field.InnerGroupIndex).UniqueName.ToString();
                    if (pivotFieldUniqueName.Contains("&"))
                    {
                        string pivotFieldKey = pivotFieldUniqueName.Substring(pivotFieldUniqueName.LastIndexOf('&') + 2);
                        pivotFieldKey = pivotFieldKey.Remove(pivotFieldKey.Length - 1, 1);
                        e.DisplayText = ((((DevExpress.XtraPivotGrid.PivotFieldValueEventArgs)(e)).Item).IsLastFieldLevel) ? pivotFieldKey + " - " + e.Value.ToString() : pivotFieldKey;
                    };
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TestCubeForm.cs", "pivotTable_FieldValueDisplayText"));
            }
        }

        /// <summary>
        /// Prompt appearing - Mouse hover event handler
        /// </summary>
        private void ToolTipController_GetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
        {
            try
            {
                PivotGridHitInfo pivotInfo = _pivotTable.CalcHitInfo(e.ControlMousePosition);
                if (pivotInfo.HitTest == PivotGridHitTest.Value)
                {
                    if (pivotInfo.ValueInfo.Field != null && pivotInfo.ValueInfo.Field.Area == PivotArea.RowArea)
                    {
                        _pivotTable.ToolTipController.Active = true;
                        object o = pivotInfo.HitTest.ToString() + "_" + pivotInfo.ValueInfo.Value.ToString();
                        string text = pivotInfo.ValueInfo.Value.ToString();
                        e.Info = new ToolTipControlInfo(o, text);
                    }
                    else
                    {
                        _pivotTable.ToolTipController.Active = false;
                    }
                }
                else
                {
                    _pivotTable.ToolTipController.Active = false;
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TestCubeForm.cs", "ToolTioController_GetActiveObjectInfo"));
            }
        }

        /// <summary>
        /// Export data to Excel file
        /// </summary>
        private void btnExportXls_ItemClick(object sender, ItemClickEventArgs e)
        {
            string exportPath = "c:\\Test.xls";

            _pivotTable.ExportToXls(exportPath);
        }

        #endregion

        private void TestCubeForm_VisibleChanged(object sender, EventArgs e)
        {
            if (!this._canOpen && this.Visible)
            {
                this.VisibleChanged -= new EventHandler(this.TestCubeForm_VisibleChanged);
                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoCube));
                this.Close();
            }
        }

    }
}
