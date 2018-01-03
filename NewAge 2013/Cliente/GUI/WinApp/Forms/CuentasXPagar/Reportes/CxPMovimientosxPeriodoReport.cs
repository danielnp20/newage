using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraReports.Native;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Reportes;
using System.Collections;
using DevExpress.XtraReports.UI;
using System.Drawing;
using DevExpress.XtraPrinting.Shape;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Cliente.GUI.WinApp.Reports;

namespace NewAge.Cliente.GUI.WinApp.Reports
{
    class CxPMovimientosxPeriodoReport : BaseReport //XtraReport//
    {
        #region Variables
        BaseController _bc = BaseController.GetInstance();
        #endregion

        #region Funciones Publicas
        /// <summary>
        /// Cuentas por pagar por edades (Flujo semanal) Report Constructor
        /// </summary>
        /// <param name="reportData"> data for the report </param>
        /// <param name="fieldList"> list of fields for report detail table</param>
        /// <param name="weekList"> list dynamic names of the fields (start and finel dates of weeks in a month)</param>
        /// <param name="MM"> Moneda type of the report - local, foreign</param>
        /// <param name="Date">month of the report</param>
        public CxPMovimientosxPeriodoReport(List<DTO_ReportCxPxMovimientosxPeriodo> reportData, DateTime minDate, DateTime maxDate, ArrayList fieldList, TipoMoneda MM)
        {
            this.lblReportName.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.cpMovimientosPeriodo).ToString());

            #region Report styles

            this.Landscape = true;
            XRControlStyles tableStyles = new XRControlStyles(this)
            {
                EvenStyle = new XRControlStyle()
                {
                    Name = "tableDetailEvenStyle",
                    BackColor = Color.Transparent,
                    ForeColor = Color.Black,
                    Font = new Font("Times New Roman", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0)
                },
                OddStyle = new XRControlStyle()
                {
                    Name = "tableDetailOddStyle",
                    BackColor = Color.WhiteSmoke,
                    ForeColor = Color.Black,
                    Font = new Font("Times New Roman", 8),
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
                    Font = new Font("Times New Roman", 8, FontStyle.Bold),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0)
                };
            this.StyleSheet.Add(groupFooterStyle);

            XRControlStyle totalFooterStyle = new XRControlStyle()
                {
                    Name = "totalFooterStyle",
                    BackColor = Color.White,
                    ForeColor = Color.Black,
                    Font = new Font("Times New Roman", 9, FontStyle.Bold),
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
            reportPeriodBand.Level = 3;
            reportPeriodBand.HeightF = 95;
            reportPeriodBand.RepeatEveryPage = true;
            reportTableBand.Bands.Add(reportPeriodBand);

            GroupHeaderBand reportTableHeaderBand;
            reportTableHeaderBand = new GroupHeaderBand();
            reportTableHeaderBand.Level = 2;
            reportTableHeaderBand.HeightF = 30;
            reportTableHeaderBand.RepeatEveryPage = true;
            reportTableBand.Bands.Add(reportTableHeaderBand);

            GroupHeaderBand reportGroupHeaderBand;
            reportGroupHeaderBand = new GroupHeaderBand();
            reportGroupHeaderBand.Level = 1;
            reportGroupHeaderBand.HeightF = 40;
            reportTableBand.Bands.Add(reportGroupHeaderBand);

            GroupField reportGroupField = new GroupField("ReportRompimiento1.GroupFieldValue");
            reportGroupField.SortOrder = XRColumnSortOrder.Ascending;
            reportGroupHeaderBand.GroupFields.Add(reportGroupField);

            GroupHeaderBand reportSubGroupHeaderBand;
            reportSubGroupHeaderBand = new GroupHeaderBand();
            reportSubGroupHeaderBand.Level = 0;
            reportSubGroupHeaderBand.HeightF = 30;
            reportTableBand.Bands.Add(reportSubGroupHeaderBand);

            GroupField reportSubGroupField = new GroupField("ReportRompimiento2.GroupFieldValue");
            reportSubGroupField.SortOrder = XRColumnSortOrder.Ascending;
            reportSubGroupHeaderBand.GroupFields.Add(reportSubGroupField);

            DetailBand reportTableDetailBand;
            reportTableDetailBand = new DetailBand();
            reportTableDetailBand.HeightF = 20;
            reportTableBand.Bands.Add(reportTableDetailBand);

            GroupFooterBand reportSubGroupFooterBand;
            reportSubGroupFooterBand = new GroupFooterBand();
            reportSubGroupFooterBand.Level = 0;
            reportSubGroupFooterBand.HeightF = 40;
            reportTableBand.Bands.Add(reportSubGroupFooterBand);

            GroupFooterBand reportGroupFooterBand;
            reportGroupFooterBand = new GroupFooterBand();
            reportGroupFooterBand.Level = 1;
            reportGroupFooterBand.HeightF = 65;
            reportTableBand.Bands.Add(reportGroupFooterBand);

            GroupFooterBand reportTotalFooterBand;
            reportTotalFooterBand = new GroupFooterBand();
            reportTotalFooterBand.Level = 2;
            reportTotalFooterBand.HeightF = 110;
            reportTableBand.Bands.Add(reportTotalFooterBand);

            this.Bands.Add(reportTableBand);

            #endregion

            #region Report field width

            float tableWidth = 0;
            float columnWidth = 0;

            tableWidth = this.PageWidth - (this.Margins.Right + this.Margins.Left);
            //if (fieldList.Contains("CuentaID"))
            columnWidth = (tableWidth - 150) / fieldList.Count;

            #endregion

            #region Report elements

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

            #region Report period header

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
            XRTableCell periodTableCell_FirsMonth;
            XRTableCell periodTableCell_LastMonth;
            XRTableCell periodTableCell_Moneda;
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

            periodTable = new XRTable();
            periodTable.BeginInit();
            periodTable.LocationF = periodFrame.LocationF;
            periodTable.SizeF = new SizeF(tableWidth, 80);
            periodTable.Font = new Font("Times New Roman", 10, FontStyle.Bold);
            periodTable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            periodTableRow = new XRTableRow();
            periodTableRow.HeightF = periodFrame.HeightF;

            float periodTableCellWidth = (tableWidth - 50) / 3;

            periodTableCell_Year = new XRTableCell();
            periodTableCell_Year.WidthF = periodTableCellWidth;
            periodTableCell_Year.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_lblYear") + ":   " + minDate.ToString("yyyy");
            periodTableCell_Year.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            periodTableCell_Year.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 10, 0, 0);
            periodTableRow.Cells.Add(periodTableCell_Year);

            periodTableCell_FirsMonth = new XRTableCell();
            periodTableCell_FirsMonth.WidthF = periodTableCellWidth;
            periodTableCell_FirsMonth.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_lblFrom") + ":   " + minDate.ToString("MMMM");
            periodTableCell_FirsMonth.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            periodTableCell_FirsMonth.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            periodTableRow.Cells.Add(periodTableCell_FirsMonth);

            periodTableCell_LastMonth = new XRTableCell();
            periodTableCell_LastMonth.WidthF = periodTableCellWidth;
            periodTableCell_LastMonth.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_lblUntil") + ":   " + maxDate.ToString("MMMM");
            periodTableCell_LastMonth.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            periodTableCell_LastMonth.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            periodTableRow.Cells.Add(periodTableCell_LastMonth);

            periodTableCell_Moneda = new XRTableCell();
            periodTableCell_Moneda.WidthF = periodTableCellWidth + 50;
            periodTableCell_Moneda.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            switch (MM)
            {
                case TipoMoneda.Local:
                    periodTableCell_Moneda.Text =
                        _bc.GetResource(Librerias.Project.LanguageTypes.Forms,
                                        (AppForms.ReportForm).ToString() + "_Moneda") + ":   " +
                        _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyLocal);
                    break;
                case TipoMoneda.Foreign:
                    periodTableCell_Moneda.Text =
                        _bc.GetResource(Librerias.Project.LanguageTypes.Forms,
                                        (AppForms.ReportForm).ToString() + "_Moneda") + ":   " +
                        _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyForeign);
                    break;
            }
            ;
            periodTableRow.Cells.Add(periodTableCell_Moneda);

            periodTable.Rows.Add(periodTableRow);

            periodTableRow = new XRTableRow();
            periodTableRow.HeightF = periodFrame.HeightF;

            periodTable.Rows.Add(periodTableRow);
            periodTable.EndInit();
            reportPeriodBand.Controls.Add(periodTable);

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
            if (MM == TipoMoneda.Both)
            {
                XRTableRow tableHeaderUpperRow = new XRTableRow()
                {
                    Borders = DevExpress.XtraPrinting.BorderSide.All,
                    BorderColor = Color.White
                };
                XRTableCell emptyCell = new XRTableCell()
                {
                    WidthF = tableWidth - 8 * columnWidth,
                    BackColor = Color.White,
                };
                XRTableCell MLCell = new XRTableCell()
                {
                    WidthF = 4 * columnWidth,
                    Text = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyLocal)
                };
                XRTableCell MECell = new XRTableCell()
                {
                    WidthF = 4 * columnWidth,
                    Text = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyForeign)
                };
                tableHeaderUpperRow.Cells.AddRange(new XRTableCell[] { emptyCell, MLCell, MECell });
                tableHeader.Rows.Add(tableHeaderUpperRow);
            }
            ;

            XRTable tableDetail;
            XRTableRow tableDetailRow;
            XRTableCell tableDetailCell;
            XRTableCell tableDetailCell_MaxLengthInd;
            tableDetail = new XRTable();
            tableDetail.WidthF = tableWidth;
            tableDetail.HeightF = reportTableDetailBand.HeightF;
            tableDetail.StyleName = "tableDetailEvenStyle";
            tableDetailRow = new XRTableRow();
            tableDetailRow.Name = "tableDetailRow";
            //tableDetailRow.HeightF = tableDetail.HeightF;

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
                LocationF =
                    new PointF(30, subGroupFooter.LocationF.Y + subGroupFooter.HeightF)
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
                LocationF =
                    new PointF(0, groupFooter.LocationF.Y + groupFooter.HeightF + 5)
            };
            reportGroupFooterBand.Controls.Add(footerLowerLine_1);

            XRLine footerLowerLine_2 = new XRLine()
            {
                LineWidth = 1,
                SizeF = new SizeF(tableWidth, 2),
                LocationF =
                    new PointF(0, groupFooter.LocationF.Y + groupFooter.HeightF + 8)
            };
            reportGroupFooterBand.Controls.Add(footerLowerLine_2);

            #endregion

            #region Report footer band

            XRTable totalFooter;
            XRTableRow totalFooterRow;
            XRTableCell totalFooterCell_Caption;
            XRTableCell totalFooterCell_Value;
            totalFooter = new XRTable();
            totalFooter.LocationF = new PointF(0, 10);
            totalFooter.SizeF = new SizeF(tableWidth, 30);
            totalFooter.StyleName = "totalFooterStyle";
            totalFooterRow = new XRTableRow();

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
                tableHeaderCell.WidthF = columnWidth;

                //if (fieldName.Contains("DocumentoTercero"))
                //    tableHeaderCell.WidthF = columnWidth + 60;
                if (fieldName.Contains("Descriptivo"))
                    tableHeaderCell.WidthF = columnWidth + 150;
                //if (fieldName.Contains("Cuenta") || fieldName.Contains("vlrMdaLoc") || fieldName.Contains("vlrMdaExt"))
                //    tableHeaderCell.WidthF = columnWidth - 50;


                string resourceFieldID = (AppReports.cpMovimientosPeriodo).ToString() + "_" + fieldName;
                string tableColumnName = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, resourceFieldID);
                tableHeaderCell.Text = tableColumnName;

                tableHeaderRow.Controls.Add(tableHeaderCell);

                #endregion

                #region Report table detail

                tableDetailCell = new XRTableCell();
                tableDetailCell.WidthF = tableHeaderCell.WidthF;
                if (fieldName.Contains("Fecha"))
                {
                    tableDetailCell.DataBindings.Add("Text", this.DataSource, fieldName, "{0:dd/MM/yyyy}");
                    tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                }
                else tableDetailCell.DataBindings.Add("Text", this.DataSource, fieldName);

                if (fieldName.Contains("vlrMdaLoc") || fieldName.Contains("vlrMdaExt"))
                    tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                else if (fieldName.Contains("Cuenta"))
                    tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;

                //if (fieldName.Contains("DocumentoTercero"))
                //    tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                if (fieldName.Contains("Descriptivo"))
                    tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify;

                tableDetailRow.Controls.Add(tableDetailCell);

                #endregion

                #region Report table footer

                if (fieldName.Contains("vlrMdaLoc") || fieldName.Contains("vlrMdaExt"))
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

                        groupFooterCell_Name = new XRTableCell();
                        groupFooterCell_Name.WidthF = totalFieldLocation;
                        groupFooterCell_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 20, 0, 0);
                        groupFooterCell_Name.Name = "groupFooterCell_Name";
                        groupFooterRow.Controls.Add(groupFooterCell_Name);

                        subGroupFooterCell_Name = new XRTableCell();
                        subGroupFooterCell_Name.WidthF = totalFieldLocation;
                        subGroupFooterCell_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 20, 0, 0);
                        subGroupFooterCell_Name.Name = "subGroupFooterCell_Name";
                        subGroupFooterRow.Controls.Add(subGroupFooterCell_Name);
                    }

                    totalsInd = 1;

                    totalFooterCell_Value = new XRTableCell();
                    totalFooterCell_Value.WidthF = tableHeaderCell.WidthF;
                    totalFooterCell_Value.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                    totalFooterCell_Value.BorderWidth = 2;
                    totalFooterCell_Value.Summary.Func = SummaryFunc.Sum;
                    totalFooterCell_Value.Summary.Running = SummaryRunning.Report;
                    totalFooterCell_Value.Summary.FormatString = "{0:#,0.00}";
                    totalFooterCell_Value.DataBindings.Add("Text", this.DataSource, fieldName + ".Value");
                    totalFooterRow.Controls.Add(totalFooterCell_Value);

                    groupFooterCell_Value = new XRTableCell();
                    groupFooterCell_Value.Name = fieldName + "_group";
                    groupFooterCell_Value.WidthF = tableHeaderCell.WidthF;
                    groupFooterCell_Value.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                    groupFooterCell_Value.Summary.Func = SummaryFunc.Sum;
                    groupFooterCell_Value.Summary.Running = SummaryRunning.Group;
                    groupFooterCell_Value.Summary.FormatString = "{0:#,0.00}";
                    groupFooterCell_Value.DataBindings.Add("Text", this.DataSource, fieldName + ".Value");
                    groupFooterRow.Controls.Add(groupFooterCell_Value);

                    subGroupFooterCell_Value = new XRTableCell();
                    subGroupFooterCell_Value.Name = fieldName + "_subgroup";
                    subGroupFooterCell_Value.WidthF = tableHeaderCell.WidthF;
                    subGroupFooterCell_Value.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                    subGroupFooterCell_Value.Summary.Func = SummaryFunc.Sum;
                    subGroupFooterCell_Value.Summary.Running = SummaryRunning.Group;
                    subGroupFooterCell_Value.Summary.FormatString = "{0:#,0.00}";
                    subGroupFooterCell_Value.DataBindings.Add("Text", this.DataSource, fieldName + ".Value");
                    subGroupFooterRow.Controls.Add(subGroupFooterCell_Value);
                }

                if (totalsInd == 0)
                    totalFieldLocation += tableHeaderCell.WidthF;

                #endregion
            }
            tableHeader.Controls.Add(tableHeaderRow);
            tableDetail.Controls.Add(tableDetailRow);
            subGroupFooter.Controls.Add(subGroupFooterRow);
            groupFooter.Controls.Add(groupFooterRow);
            totalFooter.Controls.Add(totalFooterRow);

            reportTableHeaderBand.Controls.Add(tableHeader);
            reportTableDetailBand.Controls.Add(tableDetail);
            reportSubGroupFooterBand.Controls.Add(subGroupFooter);
            reportGroupFooterBand.Controls.Add(groupFooter);
            reportTotalFooterBand.Controls.Add(totalFooter);

            #endregion

            #region Report group header

            groupHeaderCell_Name = new XRTableCell();
            groupHeaderCell_Name.Name = "groupHeaderCell_Name";
            groupHeaderCell_Name.WidthF = 100;
            groupHeaderCell_Name.DataBindings.Add("Text", this.DataSource, "ReportRompimiento1.GroupFieldName");
            groupHeaderCell_Name.BeforePrint +=
                new System.Drawing.Printing.PrintEventHandler(groupHeaderCell_Name_BeforePrint);

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

            #endregion

            #region Report sub group header

            subGroupHeaderCell_Name = new XRTableCell();
            subGroupHeaderCell_Name.Name = "subGroupHeaderCell_Name";
            subGroupHeaderCell_Name.WidthF = 100;
            subGroupHeaderCell_Name.DataBindings.Add("Text", this.DataSource, "ReportRompimiento2.GroupFieldName");
            subGroupHeaderCell_Name.BeforePrint +=
                new System.Drawing.Printing.PrintEventHandler(groupHeaderCell_Name_BeforePrint);
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

            #endregion
        }

        #endregion

        #region Eventos

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

