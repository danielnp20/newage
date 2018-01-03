using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Reportes;
using System.Collections;
using DevExpress.XtraReports.UI;
using System.Drawing;
using DevExpress.XtraPrinting.Shape;
using NewAge.DTO.Negocio;
using System.Windows.Forms;
using NewAge.Librerias.Project;
using NewAge.Cliente.GUI.WinApp.Reports;

namespace NewAge.Cliente.GUI.WinApp.Reports
{
    class BalanceGeneralReport : BaseReport
    {
        #region Variables
        BaseController _bc = BaseController.GetInstance();
        #endregion

        #region Funciones Publicas
        /// <summary>
        ///  Balance General Report Constructor
        /// </summary>
        /// <param name="reportData">data for the report</param>
        /// <param name="fieldList">list of fields for report detail table</param>
        /// <param name="MM"> Moneda type of the report - local, foreign, both</param>
        /// <param name="balanceTipo">tipo de balance</param>
        /// <param name="Date">report period</param>
        /// <param name="selectedFiltersList">list of filters assigned by the user</param>
        /// <param name="cuentaFuncInd">indicador del tipo de la cuenta (true - cuenta Funcional;false - cuenta Alterna)</param>
        public BalanceGeneralReport(List<DTO_ReportBalanceGeneral> reportData, ArrayList fieldList, TipoMoneda MM, string balanceTipo, DateTime Date, List<string> selectedFiltersList)
        {
            this.lblReportName.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.coBalanceGeneral).ToString());

            #region Report styles
            //this.Landscape = true;
            XRControlStyles tableStyles = new XRControlStyles(this)
            {
                EvenStyle = new XRControlStyle()
                {
                    Name = "tableDetailEvenStyle",
                    BackColor = Color.White,
                    ForeColor = Color.Black,
                    Font = new Font("Times New Roman", 8, FontStyle.Bold),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0)
                },
                OddStyle = new XRControlStyle()
                {
                    Name = "tableDetailOddStyle",
                    BackColor = Color.WhiteSmoke,
                    ForeColor = Color.Black,
                    Font = new Font("Times New Roman", 8, FontStyle.Bold),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0)
                },
                Style = new XRControlStyle()
                {
                    Name = "tableHeaderStyle",
                    BackColor = Color.DimGray,
                    ForeColor = Color.White,
                    Font = new Font("Arial Narrow", 9, FontStyle.Bold),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                    Borders = DevExpress.XtraPrinting.BorderSide.Bottom,
                    BorderColor = Color.DimGray,
                    BorderWidth = 2
                }
            };

            this.StyleSheet.Add(tableStyles.EvenStyle);
            this.StyleSheet.Add(tableStyles.OddStyle);
            this.StyleSheet.Add(tableStyles.Style);
            #endregion

            #region Report bands
            DetailReportBand reportTableBand;
            reportTableBand = new DetailReportBand();
            reportTableBand.Name = "reportTableBand";
            reportTableBand.DataSource = reportData;

            GroupHeaderBand reportPeriodBand;
            reportPeriodBand = new GroupHeaderBand();
            reportPeriodBand.Level = 1;
            reportPeriodBand.HeightF = (selectedFiltersList.Count > 0) ? 85 : 55;
            reportPeriodBand.RepeatEveryPage = true;
            reportTableBand.Bands.Add(reportPeriodBand);

            GroupHeaderBand reportTableHeaderBand;
            reportTableHeaderBand = new GroupHeaderBand();
            reportTableHeaderBand.Level = 0;
            reportTableHeaderBand.HeightF = 30;
            reportTableHeaderBand.RepeatEveryPage = true;
            reportTableBand.Bands.Add(reportTableHeaderBand);

            DetailBand reportTableDetailBand;
            reportTableDetailBand = new DetailBand();
            reportTableDetailBand.HeightF = 20;
            reportTableBand.Bands.Add(reportTableDetailBand);

            GroupFooterBand reportGroupFooterBand;
            reportGroupFooterBand = new GroupFooterBand();
            reportGroupFooterBand.Level = 0;
            reportGroupFooterBand.HeightF = 30;
            reportTableBand.Bands.Add(reportGroupFooterBand);

            this.Bands.Add(reportTableBand);
            #endregion

            #region Report field width
            float tableWidth = 0;
            float columnWidth = 0;

            tableWidth = this.PageWidth - (this.Margins.Right + this.Margins.Left);
            columnWidth = (tableWidth - 70) / fieldList.Count;
            #endregion

            #region Report elements
            #region Report period band
 
            XRShape periodFrame = new XRShape();
            periodFrame.LocationF = new PointF(0, 0);
            periodFrame.SizeF = new SizeF(tableWidth, 35);
            periodFrame.LineWidth = 2;
            periodFrame.SendToBack();
            periodFrame.Shape = new ShapeRectangle()
            {
                Fillet = 50,
            };
            reportPeriodBand.Controls.Add(periodFrame);

            XRTable periodTable;
            XRTableRow periodTableRow;
            XRTableCell periodTableCell_Year;
            XRTableCell periodTableCell_Month;
            XRTableCell periodTableCell_Moneda;
            XRTableCell periodTableCell_BalanceTipo;

            periodTable = new XRTable();
            periodTable.LocationF = periodFrame.LocationF;
            periodTable.SizeF = periodFrame.SizeF;
            periodTable.Font = new Font("Times New Roman", 11, FontStyle.Bold);
            periodTable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            periodTableRow = new XRTableRow();

            float periodTableCellWidth = (tableWidth) / 4;

            periodTableCell_Year = new XRTableCell();
            periodTableCell_Year.WidthF = periodTableCellWidth - 30;
            periodTableCell_Year.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_lblYear") + ":   " + Date.ToString("yyyy");
            periodTableRow.Cells.Add(periodTableCell_Year);

            periodTableCell_Month = new XRTableCell();
            periodTableCell_Month.WidthF = periodTableCellWidth -30;
            periodTableCell_Month.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_lblMonth") + ":   " + Date.ToString("MMMM");
            periodTableRow.Cells.Add(periodTableCell_Month);

            periodTableCell_Moneda = new XRTableCell();
            periodTableCell_Moneda.WidthF = periodTableCellWidth + 30;
            switch (MM)
            {
                case TipoMoneda.Local:
                    periodTableCell_Moneda.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_Moneda") + ":   " + _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyLocal);
                    break;
                case TipoMoneda.Foreign:
                    periodTableCell_Moneda.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_Moneda") + ":   " + _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyForeign);
                    break;
                //case TipoMoneda.Both:
                //    periodTableCell_Moneda.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_Moneda") + ":   " + _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyBoth);
                //    break; ;
            };
            periodTableRow.Cells.Add(periodTableCell_Moneda);

            periodTableCell_BalanceTipo = new XRTableCell();
            periodTableCell_BalanceTipo.WidthF = periodTableCellWidth + 30;
            periodTableCell_BalanceTipo.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_TipoBalance") + ":   " + balanceTipo;
            periodTableRow.Cells.Add(periodTableCell_BalanceTipo);

            periodTable.Rows.Add(periodTableRow);
            reportPeriodBand.Controls.Add(periodTable);

            XRLabel selectedFiltersLable = new XRLabel();
            selectedFiltersLable.LocationF = new PointF(0, periodFrame.HeightF + 10);
            selectedFiltersLable.SizeF = new SizeF(tableWidth, 20);
            selectedFiltersLable.Font = new Font("Times New Roman", 9, FontStyle.Italic);
            selectedFiltersLable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            selectedFiltersLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(20, 0, 0, 0);
            selectedFiltersLable.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_FiltradoPor") + "   ";
            if (selectedFiltersList.Count > 0)
            {
                int filterCount = 0;
                foreach (string filter in selectedFiltersList)
                {
                    selectedFiltersLable.Text += filter;
                    filterCount++;
                    if (!(filterCount == (selectedFiltersList.Count))) selectedFiltersLable.Text += ",  ";
                };
                reportPeriodBand.Controls.Add(selectedFiltersLable);
            };
            #endregion

            #region Report Table detail
            XRTable tableHeader;
            XRTableRow tableHeaderRow;
            XRTableCell tableHeaderCell;
            tableHeader = new XRTable();
            tableHeader.WidthF = tableWidth;
            tableHeader.HeightF = reportTableHeaderBand.HeightF - 5;
            tableHeader.LocationF = new PointF(0, 0);
            tableHeader.StyleName = "tableHeaderStyle";
            tableHeaderRow = new XRTableRow();
            tableHeaderRow.HeightF = tableHeader.HeightF;

            XRTable tableDetail;
            XRTableRow tableDetailRow;
            XRTableCell tableDetailCell;
            tableDetail = new XRTable();
            tableDetail.Name = "tableDetail";
            tableDetail.WidthF = tableWidth;
            tableDetail.HeightF = reportTableDetailBand.HeightF;
            tableDetail.EvenStyleName = "tableDetailEvenStyle";
            tableDetail.OddStyleName = "tableDetailOddStyle";
            tableDetailRow = new XRTableRow();
            tableDetailRow.Name = "tableDetailRow";
            #endregion

            #region Report footer band
            XRLine footerLowerLine_1 = new XRLine()
            {
                LineWidth = 1,
                SizeF = new SizeF(tableWidth, 2),
                LocationF = new PointF(0, 12)
            };
            reportGroupFooterBand.Controls.Add(footerLowerLine_1);

            XRLine footerLowerLine_2 = new XRLine()
            {
                LineWidth = 1,
                SizeF = new SizeF(tableWidth, 2),
                LocationF = new PointF(0, 15)
            };
            reportGroupFooterBand.Controls.Add(footerLowerLine_2);
            #endregion
            #endregion

            #region Report Table
            foreach (string fieldName in fieldList)
            {
                #region Report table header
                tableHeaderCell = new XRTableCell();
                tableHeaderCell.WidthF = columnWidth;
                if (fieldName.Contains("Cuenta"))
                {
                    if (fieldName.Contains("Desc"))
                        tableHeaderCell.WidthF = columnWidth + 100;
                    else
                        tableHeaderCell.WidthF = columnWidth - 30;
                };
                if (fieldName.Contains("_L"))
                    tableHeaderCell.Text = "";
                else
                {
                    string resourceFieldID = (AppReports.coBalanceGeneral).ToString() + "_" + fieldName;
                    string tableColumnName = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, resourceFieldID);
                    tableHeaderCell.Text = tableColumnName;
                };
                tableHeaderRow.Controls.Add(tableHeaderCell);
                #endregion

                #region Report table detail
                tableDetailCell = new XRTableCell();
                tableDetailCell.WidthF = tableHeaderCell.WidthF;

                if (fieldName.Contains("Cuenta"))
                {
                    if (fieldName.Contains("ID"))
                        tableDetailCell.BeforePrint += new System.Drawing.Printing.PrintEventHandler(tableDetailCell_BeforePrint);
                    else
                    {
                        tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                        tableDetailCell.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0);
                    }
                    tableDetailCell.DataBindings.Add("Text", this.DataSource, fieldName);
                }
                else               
                    tableDetailCell.DataBindings.Add("Text", this.DataSource, fieldName, "{0:#,0.00}");

                tableDetailRow.Controls.Add(tableDetailCell);
                #endregion
            };
            tableHeader.Controls.Add(tableHeaderRow);
            tableDetailCell = new XRTableCell() { WidthF = 0, Visible = false, Name = "tableDetailCell_MaxLengthInd" };
            tableDetailCell.DataBindings.Add("Text", this.DataSource, "MaxLengthInd");
            tableDetailRow.Controls.Add(tableDetailCell);
            tableDetail.Controls.Add(tableDetailRow);

            reportTableHeaderBand.Controls.Add(tableHeader);
            reportTableDetailBand.Controls.Add(tableDetail);
            #endregion
        }
        
        #endregion

        #region Eventos

        /// <summary>
        /// Changes font in the table detail row depnding on the data
        /// </summary>
        private void tableDetailCell_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell cell = (XRTableCell)sender;
            XRTableRow row = FindControl("tableDetailRow", true) as XRTableRow;
            XRTableCell cell_MaxLengthInd = FindControl("tableDetailCell_MaxLengthInd", true) as XRTableCell;

            if (!string.IsNullOrEmpty(cell_MaxLengthInd.Text.Trim()))
                if (Convert.ToInt16(cell_MaxLengthInd.Text) == 1)
                    row.Font = new Font("Times New Roman", 8, FontStyle.Regular);
                else
                    row.Font = new Font("Times New Roman", 8, FontStyle.Bold);
        }
        
        #endregion
    }
}