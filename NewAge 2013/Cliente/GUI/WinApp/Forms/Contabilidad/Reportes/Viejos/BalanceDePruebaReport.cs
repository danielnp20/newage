using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Reportes;
using System.Collections;
using DevExpress.XtraReports.UI;
using System.Drawing;
using NewAge.DTO.Negocio;
using DevExpress.XtraPrinting.Shape;
using NewAge.Cliente.GUI.WinApp.Componentes.ReportParameters;
using NewAge.Librerias.Project;
using NewAge.Cliente.GUI.WinApp.Forms;

namespace NewAge.Cliente.GUI.WinApp.Reports
{
    class BalanceDePruebaReport : BaseReport //DevExpress.XtraReports.UI.XtraReport
    {
        #region Variables
        BaseController _bc = BaseController.GetInstance();

        private int _rowInd;         
        #endregion

        #region Funciones Publicas
        /// <summary>
        /// Balance De Prueba Report Constructor
        /// </summary>
        /// <param name="reportData">data for the report</param>
        /// <param name="fieldList">list of fields for report detail table</param>
        /// <param name="MM"> Moneda type of the report - local, foreign, both</param>
        /// <param name="balanceTipo">tipo de balance</param>
        /// <param name="minDate">initial date of the period</param>
        /// <param name="maxDate">final date of the period</param>
        /// <param name="selectedFiltersList">list of filters assigned by the user</param>
        /// <param name="RompIndList">list of bool parameters to enable rompimientos (true - enabled)</param>
        /// <param name="cuentaFuncInd">indicador del tipo de la cuenta (true - cuenta Funcional;false - cuenta Alterna)</param>
        public BalanceDePruebaReport(List<DTO_ReportBalanceDePrueba> reportData, List<string> fieldList, TipoMoneda MM, string balanceTipo, DateTime minDate, DateTime maxDate, List<string> selectedFiltersList, List<bool> RompIndList,int signIni, int signFin)
        {
            this.lblReportName.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.coBalanceDePrueba).ToString());

            #region Report styles

            XRControlStyles tableStyles = new XRControlStyles(this)
            {
                EvenStyle = new XRControlStyle()
                {
                    Name = "tableDetailEvenStyle",
                    BackColor = Color.White,
                    ForeColor = Color.Black,
                    Font = new Font("Times New Roman", 8, FontStyle.Bold),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0)
                },
                OddStyle = new XRControlStyle()
                {
                    Name = "tableDetailOddStyle",
                    BackColor = Color.WhiteSmoke,
                    ForeColor = Color.Black,
                    Font = new Font("Times New Roman", 8, FontStyle.Bold),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0)
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

            XRControlStyle groupHeaderStyle = new XRControlStyle()
            {
                Name = "groupHeaderStyle",
                BackColor = Color.White,
                ForeColor = Color.Black,
                Font = new Font("Arial Narrow", 9, FontStyle.Bold),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 2, 0, 0)
            };
            this.StyleSheet.Add(groupHeaderStyle);

            XRControlStyle groupFooterStyle = new XRControlStyle()
            {
                Name = "groupFooterStyle",
                BackColor = Color.White,
                ForeColor = Color.Black,
                Font = new Font("Times New Roman", 8, FontStyle.Bold | FontStyle.Italic),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight,
                Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0)
            };
            this.StyleSheet.Add(groupFooterStyle);

            XRControlStyle totalFooterStyle = new XRControlStyle()
            {
                Name = "totalFooterStyle",
                BackColor = Color.White,
                ForeColor = Color.Black,
                Font = new Font("Times New Roman", 9, FontStyle.Bold | FontStyle.Italic),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight,
                Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0)
            };
            this.StyleSheet.Add(totalFooterStyle);
            #endregion

            #region Report bands
            DetailReportBand reportTableBand;
            reportTableBand = new DetailReportBand();
            reportTableBand.Name = "reportTableBand";
            reportTableBand.DataSource = reportData;

            GroupHeaderBand reportPeriodBand;
            reportPeriodBand = new GroupHeaderBand();
            reportPeriodBand.Level = 4;
            reportPeriodBand.HeightF = (selectedFiltersList.Count > 0) ? 85 : 55;
            reportPeriodBand.RepeatEveryPage = true;
            reportTableBand.Bands.Add(reportPeriodBand);

            GroupHeaderBand reportTableHeaderBand;
            reportTableHeaderBand = new GroupHeaderBand();
            reportTableHeaderBand.Level = 3;
            reportTableHeaderBand.HeightF = 30;
            reportTableHeaderBand.RepeatEveryPage = true;
            reportTableBand.Bands.Add(reportTableHeaderBand);

            GroupHeaderBand reportGroupHeaderBand;
            reportGroupHeaderBand = new GroupHeaderBand();
            reportGroupHeaderBand.Level = 2;
            reportGroupHeaderBand.HeightF = 40;

            GroupField reportGroupField = new GroupField("ReportRompimiento1.GroupFieldValue");
            reportGroupField.SortOrder = XRColumnSortOrder.Ascending; ;

            GroupHeaderBand reportSubGroupHeaderBand;
            reportSubGroupHeaderBand = new GroupHeaderBand();
            reportSubGroupHeaderBand.Level = 1;
            reportSubGroupHeaderBand.HeightF = 30;

            GroupField reportSubGroupField = new GroupField("ReportRompimiento2.GroupFieldValue");
            reportSubGroupField.SortOrder = XRColumnSortOrder.Ascending;

            GroupHeaderBand reportGroupDetailHeaderBand;
            reportGroupDetailHeaderBand = new GroupHeaderBand();
            reportGroupDetailHeaderBand.Level = 0;
            reportGroupDetailHeaderBand.HeightF = 0;
            reportTableBand.Bands.Add(reportGroupDetailHeaderBand);

            GroupField reportGroupDetailField = new GroupField("CuentaID");
            reportGroupDetailField.SortOrder = XRColumnSortOrder.Ascending;
            reportGroupDetailHeaderBand.GroupFields.Add(reportGroupDetailField);

            DetailBand reportTableDetailBand;
            reportTableDetailBand = new DetailBand();
            reportTableDetailBand.HeightF = 0;
            reportTableBand.Bands.Add(reportTableDetailBand);

            GroupFooterBand reportGroupDetailFooterBand;
            reportGroupDetailFooterBand = new GroupFooterBand();
            reportGroupDetailFooterBand.Level = 0;
            reportGroupDetailFooterBand.HeightF = 20;
            reportTableBand.Bands.Add(reportGroupDetailFooterBand);

            GroupFooterBand reportSubGroupFooterBand;
            reportSubGroupFooterBand = new GroupFooterBand();
            reportSubGroupFooterBand.Level = 1;
            reportSubGroupFooterBand.HeightF = 40;

            GroupFooterBand reportGroupFooterBand;
            reportGroupFooterBand = new GroupFooterBand();
            reportGroupFooterBand.Level = 2;
            reportGroupFooterBand.HeightF = 65;

            GroupFooterBand reportTotalFooterBand;
            reportTotalFooterBand = new GroupFooterBand();
            reportTotalFooterBand.Level = 3;
            reportTotalFooterBand.HeightF = 50;
            reportTableBand.Bands.Add(reportTotalFooterBand);

            if (RompIndList[0])
            {
                reportTableBand.Bands.Add(reportGroupHeaderBand);
                reportGroupHeaderBand.GroupFields.Add(reportGroupField);
                reportTableBand.Bands.Add(reportGroupFooterBand);
            };

            if (RompIndList[1])
            {
                reportTableBand.Bands.Add(reportSubGroupHeaderBand);
                reportSubGroupHeaderBand.GroupFields.Add(reportSubGroupField);
                reportTableBand.Bands.Add(reportSubGroupFooterBand);
            };

            this.Bands.Add(reportTableBand);
            #endregion

            #region Report field width
            float tableWidth = 0;
            float columnWidth = 0;

            tableWidth = this.PageWidth - (this.Margins.Right + this.Margins.Left);
            if (fieldList.Contains("CuentaDesc"))
            {
                columnWidth = (tableWidth - 50) / fieldList.Count;
            }
            else
            {
                columnWidth = tableWidth / fieldList.Count;
            };
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
            XRTableCell periodTableCell_FromMonth;
            XRTableCell periodTableCell_TillMonth;
            XRTableCell periodTableCell_Moneda;
            XRTableCell periodTableCell_BalanceTipo;
            periodTable = new XRTable();
            periodTable.LocationF = periodFrame.LocationF;
            periodTable.SizeF = periodFrame.SizeF;
            periodTable.Font = new Font("Times New Roman", 10, FontStyle.Bold);
            periodTable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            periodTableRow = new XRTableRow();

            float periodTableCellWidth = (tableWidth - 50) / 5;

            periodTableCell_Year = new XRTableCell();
            periodTableCell_Year.WidthF = periodTableCellWidth;
            periodTableCell_Year.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_lblYear") + ":   " + minDate.ToString("yyyy");
            periodTableRow.Cells.Add(periodTableCell_Year);

            periodTableCell_FromMonth = new XRTableCell();
            periodTableCell_FromMonth.WidthF = periodTableCellWidth;
            periodTableCell_FromMonth.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_lblFrom") + ":   " + minDate.ToString("MMMM");
            periodTableRow.Cells.Add(periodTableCell_FromMonth);

            periodTableCell_TillMonth = new XRTableCell();
            periodTableCell_TillMonth.WidthF = periodTableCellWidth;
            periodTableCell_TillMonth.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_lblUntil") + ":   " + maxDate.ToString("MMMM");
            periodTableRow.Cells.Add(periodTableCell_TillMonth);

            periodTableCell_Moneda = new XRTableCell();
            periodTableCell_Moneda.WidthF = periodTableCellWidth + 20;
            switch (MM)
            {
                case TipoMoneda.Local:
                    periodTableCell_Moneda.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_Moneda") + ":   " + _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyLocal);
                    break;
                case TipoMoneda.Foreign:
                    periodTableCell_Moneda.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_Moneda") + ":   " + _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyForeign);
                    break;
                case TipoMoneda.Both:
                    periodTableCell_Moneda.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_Moneda") + ":   " + _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyBoth);
                    break; ;
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

            #region Report group header
            XRShape groupHeaderFrame = new XRShape();
            groupHeaderFrame.LocationF = new PointF(0, 10);
            groupHeaderFrame.SizeF = new SizeF(tableWidth, 25);
            groupHeaderFrame.BorderWidth = 1;
            groupHeaderFrame.SendToBack();
            groupHeaderFrame.Shape = new ShapeRectangle()
            {
                Fillet = 50
            };
            reportGroupHeaderBand.Controls.Add(groupHeaderFrame);

            XRTable groupHeader;
            XRTableRow groupHeaderRow;
            XRTableCell groupHeaderCell_Name;
            XRTableCell groupHeaderCell_Value;
            XRTableCell groupHeaderCell_Desc;
            groupHeader = new XRTable();
            groupHeader.LocationF = groupHeaderFrame.LocationF;
            groupHeader.SizeF = groupHeaderFrame.SizeF;
            groupHeader.StyleName = "groupHeaderStyle";
            groupHeaderRow = new XRTableRow();
            #endregion

            #region Report sub group header
            XRTable subGroupHeader;
            XRTableRow subGroupHeaderRow;
            XRTableCell subGroupHeaderCell_Name;
            XRTableCell subGroupHeaderCell_Value;
            XRTableCell subGroupHeaderCell_Desc;
            subGroupHeader = new XRTable();
            subGroupHeader.LocationF = new PointF(30, 0);
            subGroupHeader.SizeF = new SizeF(tableWidth - 30, 25);
            subGroupHeader.StyleName = "groupHeaderStyle";
            subGroupHeaderRow = new XRTableRow();
            #endregion

            #region Report Table detail
            XRTable tableHeader;
            XRTableRow tableHeaderRow;
            XRTableCell tableHeaderCell;
            tableHeader = new XRTable();
            tableHeader.WidthF = tableWidth;
            tableHeader.HeightF = reportTableHeaderBand.HeightF - 5;
            tableHeader.StyleName = "tableHeaderStyle";
            tableHeaderRow = new XRTableRow();
            tableHeaderRow.HeightF = tableHeader.HeightF;

            if (MM == TipoMoneda.Both)
            {
                reportTableHeaderBand.HeightF = reportTableHeaderBand.HeightF + tableHeader.HeightF;
                tableHeader.LocationF = new PointF(0, tableHeader.HeightF);
                XRTable upperTableHeader = new XRTable();
                upperTableHeader.HeightF = tableHeader.HeightF;
                if (fieldList.Contains("CuentaDesc"))
                {
                    upperTableHeader.WidthF = tableWidth - 2 * columnWidth - 50;
                    upperTableHeader.LocationF = new PointF(2 * columnWidth + 50, 0);
                }
                else
                {
                    upperTableHeader.WidthF = tableWidth - 2 * columnWidth;
                    upperTableHeader.LocationF = new PointF(2 * columnWidth, 0);
                };
                upperTableHeader.StyleName = "tableHeaderStyle";
                XRTableRow upperTableHeaderRow = new XRTableRow();
                XRTableCell upperTableHeaderCell_ML = new XRTableCell();
                upperTableHeaderCell_ML.WidthF = 3 * columnWidth;
                upperTableHeaderCell_ML.Text = "Moneda Local";
                upperTableHeaderCell_ML.Borders = DevExpress.XtraPrinting.BorderSide.Bottom | DevExpress.XtraPrinting.BorderSide.Right;
                upperTableHeaderCell_ML.BorderColor = Color.WhiteSmoke;
                upperTableHeaderCell_ML.BorderWidth = 1;
                upperTableHeaderRow.Cells.Add(upperTableHeaderCell_ML);
                XRTableCell upperTableHeaderCell_ME = new XRTableCell();
                upperTableHeaderCell_ME.WidthF = 3 * columnWidth;
                upperTableHeaderCell_ME.Text = "Moneda Extranjera";
                upperTableHeaderCell_ME.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                upperTableHeaderCell_ME.BorderColor = Color.WhiteSmoke;
                upperTableHeaderCell_ME.BorderWidth = 1;
                upperTableHeaderRow.Cells.Add(upperTableHeaderCell_ME);
                upperTableHeader.Rows.Add(upperTableHeaderRow);
                reportTableHeaderBand.Controls.Add(upperTableHeader);
            };

            XRTable tableDetail;
            XRTableRow tableDetailRow;
            XRTableCell tableDetailCell;
            XRTableCell tableDetailCell_MaxLengthInd;
            tableDetail = new XRTable();
            tableDetail.WidthF = tableWidth;
            tableDetail.HeightF = reportGroupDetailFooterBand.HeightF;
            tableDetail.StyleName = "tableDetailEvenStyle";
            tableDetailRow = new XRTableRow();
            tableDetailRow.Name = "tableDetailRow";
            //tableDetailRow.HeightF = tableDetail.HeightF;

            _rowInd = 0;
            #endregion

            #region Report sub group footer
            XRTable subGroupFooter;
            XRTableRow subGroupFooterRow;
            XRTableCell subGroupFooterCell_Name;
            XRTableCell subGroupFooterCell_Value;
            subGroupFooter = new XRTable();
            subGroupFooter.LocationF = new PointF(0, 2);
            subGroupFooter.SizeF = new SizeF(tableWidth, 30);
            subGroupFooter.StyleName = "groupFooterStyle";
            subGroupFooterRow = new XRTableRow();

            XRLine subFooterLowerLine_1 = new XRLine()
            {
                LineWidth = 1,
                SizeF = new SizeF(tableWidth - 30, 2),
                LocationF = new PointF(30, subGroupFooter.LocationF.Y + subGroupFooter.HeightF)
            };
            reportSubGroupFooterBand.Controls.Add(subFooterLowerLine_1);
            #endregion

            #region Report group footer
            XRTable groupFooter;
            XRTableRow groupFooterRow;
            XRTableCell groupFooterCell_Name;
            XRTableCell groupFooterCell_Value;
            groupFooter = new XRTable();
            groupFooter.LocationF = new PointF(0, 5);
            groupFooter.SizeF = new SizeF(tableWidth, 40);
            groupFooter.StyleName = "groupFooterStyle";
            groupFooterRow = new XRTableRow();

            XRLine footerLowerLine_1 = new XRLine()
            {
                LineWidth = 1,
                SizeF = new SizeF(tableWidth, 2),
                LocationF = new PointF(0, groupFooter.LocationF.Y + groupFooter.HeightF + 5)
            };
            reportGroupFooterBand.Controls.Add(footerLowerLine_1);

            XRLine footerLowerLine_2 = new XRLine()
            {
                LineWidth = 1,
                SizeF = new SizeF(tableWidth, 2),
                LocationF = new PointF(0, groupFooter.LocationF.Y + groupFooter.HeightF + 8)
            };
            reportGroupFooterBand.Controls.Add(footerLowerLine_2);
            #endregion

            #region Report footer band
            CalculatedField calcField;
            XRTable totalFooter;
            XRTableRow totalFooterRow;
            XRTableCell totalFooterCell_Caption;
            XRTableCell totalFooterCell_Value;
            totalFooter = new XRTable();
            totalFooter.LocationF = new PointF(0, 10);
            totalFooter.SizeF = new SizeF(tableWidth, 30);
            totalFooter.StyleName = "totalFooterStyle";
            totalFooterRow = new XRTableRow();

            XRLine footerUpperLine = new XRLine()
            {
                LineWidth = 2,
            };

            XRLine totalFooterLowerLine_1 = new XRLine()
            {
                LineWidth = 2,
                SizeF = new SizeF(tableWidth, 2),
                LocationF = new PointF(0, reportTotalFooterBand.HeightF - 5)
            };
            reportTotalFooterBand.Controls.Add(totalFooterLowerLine_1);

            XRLine totalFooterLowerLine_2 = new XRLine()
            {
                LineWidth = 2,
                SizeF = new SizeF(tableWidth, 2),
                LocationF = new PointF(0, reportTotalFooterBand.HeightF - 1)
            };
            reportTotalFooterBand.Controls.Add(totalFooterLowerLine_2);
            #endregion
            #endregion

            int totalsInd = 0;
            float totalFieldLocation = 0;

            #region Report Table
            foreach (string fieldName in fieldList)
            {
                #region Report table header
                tableHeaderCell = new XRTableCell();
                if (fieldName.Contains("Desc"))
                {
                    tableHeaderCell.WidthF = columnWidth + 50;
                }
                else
                {
                    tableHeaderCell.WidthF = columnWidth;
                };
                string resourceFieldID = (AppReports.coBalanceDePrueba).ToString() + "_" + fieldName;
                string tableColumnName = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, resourceFieldID);
                tableHeaderCell.Text = tableColumnName;
                tableHeaderRow.Controls.Add(tableHeaderCell);
                #endregion

                #region Report table detail
                tableDetailCell = new XRTableCell();
                tableDetailCell.WidthF = tableHeaderCell.WidthF;

                if (fieldName.Contains("Cuenta"))
                {
                    tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                    tableDetailCell.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0);
                    tableDetailCell.DataBindings.Add("Text", this.DataSource, fieldName);
                    if (fieldName.Contains("ID"))
                    {
                        tableDetailCell.BeforePrint += new System.Drawing.Printing.PrintEventHandler(tableDetailCell_BeforePrint);
                    };
                }
                else
                {
                    tableDetailCell.Summary.Func = SummaryFunc.Sum;
                    tableDetailCell.Summary.Running = SummaryRunning.Group;
                    tableDetailCell.Summary.FormatString = "{0:#,0.00}";
                    tableDetailCell.DataBindings.Add("Text", this.DataSource, fieldName);
                }
                tableDetailRow.Controls.Add(tableDetailCell);
                #endregion

                #region Report table footer
                if (fieldName.Contains("ML") || fieldName.Contains("ME"))
                {
                    if (totalsInd == 0)
                    {
                        totalFooterCell_Caption = new XRTableCell();
                        totalFooterCell_Caption.WidthF = totalFieldLocation;
                        totalFooterCell_Caption.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 20, 0, 0);
                        //totalFooterCell_Caption.Font = new Font("Times New Roman", 11, FontStyle.Bold);
                        totalFooterCell_Caption.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_Totals") + ": ";
                        totalFooterRow.Controls.Add(totalFooterCell_Caption);

                        //footerUpperLine.LocationF = new PointF(totalFieldLocation, 5);
                        //footerUpperLine.SizeF = new SizeF(tableWidth - totalFieldLocation, 2);
                        //reportTotalFooterBand.Controls.Add(footerUpperLine);

                        if (RompIndList[0])
                        {
                            groupFooterCell_Name = new XRTableCell();
                            groupFooterCell_Name.WidthF = totalFieldLocation;
                            groupFooterCell_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 20, 0, 0);
                            groupFooterCell_Name.Name = "groupFooterCell_Name";
                            groupFooterRow.Controls.Add(groupFooterCell_Name);
                        };

                        if (RompIndList[1])
                        {
                            subGroupFooterCell_Name = new XRTableCell();
                            subGroupFooterCell_Name.WidthF = totalFieldLocation;
                            subGroupFooterCell_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 20, 0, 0);
                            subGroupFooterCell_Name.Name = "subGroupFooterCell_Name";
                            subGroupFooterRow.Controls.Add(subGroupFooterCell_Name);
                        };

                        totalsInd = 1;
                    };

                    calcField = new CalculatedField();
                    this.CalculatedFields.Add(calcField);
                    calcField.DataSource = reportData;
                    //calcField.Expression = (fieldName.Contains("Inicial") || fieldName.Contains("Final")) ? "Iif([MaxLengthInd] == 1 ,[" + fieldName + "]*[Signo],0)" : "Iif([MaxLengthInd] == 1 ,[" + fieldName + "],0)";
                    calcField.Expression = (fieldName.Contains("Inicial")) ? "Iif([MaxLengthInd] == 1 ,[" + fieldName + "]*[Signo]*" + signIni + ",0)" : (fieldName.Contains("Final")) ? "Iif([MaxLengthInd] == 1 ,[" + fieldName + "]*[Signo]*" + signFin + ",0)" : "Iif([MaxLengthInd] == 1 ,[" + fieldName + "],0)";
                    calcField.FieldType = FieldType.Double;
                    calcField.Name = fieldName + "_calc";

                    totalFooterCell_Value = new XRTableCell();
                    totalFooterCell_Value.WidthF = tableHeaderCell.WidthF;
                    totalFooterCell_Value.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                    totalFooterCell_Value.BorderWidth = 2;
                    totalFooterCell_Value.Summary.Func = SummaryFunc.Sum;
                    totalFooterCell_Value.Summary.Running = SummaryRunning.Report;
                    totalFooterCell_Value.Summary.FormatString = "{0:#,0.00}";
                    totalFooterCell_Value.DataBindings.Add("Text", this.DataSource, fieldName + "_calc");
                    totalFooterRow.Controls.Add(totalFooterCell_Value);                                       

                  
                    if (RompIndList[0])
                    {
                        groupFooterCell_Value = new XRTableCell();
                        groupFooterCell_Value.Name = fieldName + "_group";
                        groupFooterCell_Value.WidthF = tableHeaderCell.WidthF;
                        groupFooterCell_Value.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                        groupFooterCell_Value.Summary.Func = SummaryFunc.Sum;
                        groupFooterCell_Value.Summary.Running = SummaryRunning.Group;
                        groupFooterCell_Value.Summary.FormatString = "{0:#,0.00}";
                        groupFooterCell_Value.DataBindings.Add("Text", this.DataSource, fieldName + "_calc");
                        groupFooterRow.Controls.Add(groupFooterCell_Value);
                    };

                    if (RompIndList[1])
                    {
                        subGroupFooterCell_Value = new XRTableCell();
                        subGroupFooterCell_Value.Name = fieldName + "_subgroup";
                        subGroupFooterCell_Value.WidthF = tableHeaderCell.WidthF;
                        subGroupFooterCell_Value.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                        subGroupFooterCell_Value.Summary.Func = SummaryFunc.Sum;
                        subGroupFooterCell_Value.Summary.Running = SummaryRunning.Group;
                        subGroupFooterCell_Value.Summary.FormatString = "{0:#,0.00}";
                        subGroupFooterCell_Value.DataBindings.Add("Text", this.DataSource, fieldName + "_calc");
                        subGroupFooterRow.Controls.Add(subGroupFooterCell_Value);
                    };
                };
                #endregion

                if (totalsInd == 0)
                    totalFieldLocation += tableHeaderCell.WidthF;

            };
            tableHeader.Controls.Add(tableHeaderRow);

            tableDetailCell = new XRTableCell() { WidthF = 0, Visible = false, Name = "tableDetailCell_MaxLengthInd" };
            tableDetailCell.DataBindings.Add("Text", this.DataSource, "MaxLengthInd");
            tableDetailRow.Controls.Add(tableDetailCell);
            tableDetail.Controls.Add(tableDetailRow);

            totalFooter.Controls.Add(totalFooterRow);

            reportTableHeaderBand.Controls.Add(tableHeader);
            reportGroupDetailFooterBand.Controls.Add(tableDetail);
            reportTotalFooterBand.Controls.Add(totalFooter);

            if (RompIndList[0])
            {
                groupFooter.Controls.Add(groupFooterRow);
                reportGroupFooterBand.Controls.Add(groupFooter);
            };

            if (RompIndList[1])
            {
                subGroupFooter.Controls.Add(subGroupFooterRow);
                reportSubGroupFooterBand.Controls.Add(subGroupFooter);
            };
            #endregion

            #region Report group header
            if (RompIndList[0])
            {
                groupHeaderCell_Name = new XRTableCell();
                groupHeaderCell_Name.Name = "groupHeaderCell_Name";
                groupHeaderCell_Name.WidthF = 100;
                groupHeaderCell_Name.DataBindings.Add("Text", this.DataSource, "ReportRompimiento1.GroupFieldName");
                groupHeaderCell_Name.BeforePrint += new System.Drawing.Printing.PrintEventHandler(groupHeaderCell_Name_BeforePrint);

                groupHeaderCell_Value = new XRTableCell();
                groupHeaderCell_Value.WidthF = 100;
                groupHeaderCell_Value.DataBindings.Add("Text", this.DataSource, "ReportRompimiento1.GroupFieldValue");

                groupHeaderCell_Desc = new XRTableCell();
                groupHeaderCell_Desc.WidthF = tableWidth - 200;
                groupHeaderCell_Desc.DataBindings.Add("Text", this.DataSource, "ReportRompimiento1.GroupFieldDesc");

                groupHeaderRow.Controls.Add(groupHeaderCell_Name);
                groupHeaderRow.Controls.Add(groupHeaderCell_Value);
                groupHeaderRow.Controls.Add(groupHeaderCell_Desc);

                groupHeader.Controls.Add(groupHeaderRow);
                reportGroupHeaderBand.Controls.Add(groupHeader);
            };
            #endregion

            #region Report sub group header
            if (RompIndList[1])
            {
                subGroupHeaderCell_Name = new XRTableCell();
                subGroupHeaderCell_Name.Name = "subGroupHeaderCell_Name";
                subGroupHeaderCell_Name.WidthF = 100;
                subGroupHeaderCell_Name.DataBindings.Add("Text", this.DataSource, "ReportRompimiento2.GroupFieldName");
                subGroupHeaderCell_Name.BeforePrint += new System.Drawing.Printing.PrintEventHandler(groupHeaderCell_Name_BeforePrint);
                subGroupHeaderRow.Controls.Add(subGroupHeaderCell_Name);

                subGroupHeaderCell_Value = new XRTableCell();
                subGroupHeaderCell_Value.WidthF = 100;
                subGroupHeaderCell_Value.DataBindings.Add("Text", this.DataSource, "ReportRompimiento2.GroupFieldValue");
                subGroupHeaderRow.Controls.Add(subGroupHeaderCell_Value);

                subGroupHeaderCell_Desc = new XRTableCell();
                subGroupHeaderCell_Desc.WidthF = tableWidth - 230;
                subGroupHeaderCell_Desc.DataBindings.Add("Text", this.DataSource, "ReportRompimiento2.GroupFieldDesc");
                subGroupHeaderRow.Controls.Add(subGroupHeaderCell_Desc);

                subGroupHeader.Controls.Add(subGroupHeaderRow);
                reportSubGroupHeaderBand.Controls.Add(subGroupHeader);
            };
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

            if (_rowInd != 0 && _rowInd % 2 != 0)
                row.BackColor = Color.WhiteSmoke;
            else
                row.BackColor = Color.White;

            _rowInd++;
        }

        /// <summary>
        /// Puts proper field captions depending on group field name
        /// </summary>
        private void groupHeaderCell_Name_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell headerCell = (XRTableCell)sender;
            if (!string.IsNullOrEmpty(headerCell.Text) && !string.IsNullOrWhiteSpace(headerCell.Text))
                headerCell.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.coBalanceDePrueba).ToString() + "_" + headerCell.Text);

            if (headerCell.Name.Contains("sub"))
            {
                XRTableCell footerCell = FindControl("subGroupFooterCell_Name", true) as XRTableCell;
                footerCell.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_Totals") + "  x  " + headerCell.Text;
            }
            else
            {
                XRTableCell footerCell = FindControl("groupFooterCell_Name", true) as XRTableCell;
                footerCell.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_Totals") + "  x  " + headerCell.Text;
            };
        } 


        #endregion
    }
}
