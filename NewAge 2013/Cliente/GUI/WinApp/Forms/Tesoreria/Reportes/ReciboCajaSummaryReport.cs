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
using NewAge.Librerias.Project;
using NewAge.Cliente.GUI.WinApp.Reports;
using NewAge.DTO.UDT;

namespace NewAge.Cliente.GUI.WinApp.Reports
{
    class ReciboCajaSummaryReport : BaseReport //XtraReport//
    {
        #region Variables

        BaseController _bc = BaseController.GetInstance(); 
        #endregion

        #region Funciones Publicas
        /// <summary>
        /// Relacion Documentos Report Constructor
        /// </summary>
        /// <param name="reportData"> data for the report </param>
        /// <param name="fieldList"> list of fields for report detail table</param>
        /// <param name="minDate">initial date of the period</param>
        /// <param name="maxDate">final date of the period</param>
        /// <param name="romp">indocates rompimiento</param>
        /// <param name="selectedFiltersList">list of filters assigned by the user</param>
        public ReciboCajaSummaryReport(List<DTO_ReportReciboCaja> reportData, ArrayList fieldList, DateTime minDate, DateTime maxDate, string romp, List<string> selectedFiltersList)
        {
            this.lblReportName.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.coReciboCaja).ToString());

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
            reportPeriodBand.HeightF = (selectedFiltersList.Count > 0) ? 85 : 55;
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
            reportGroupHeaderBand.GroupFields.Add(reportGroupField);

            GroupHeaderBand reportSubGroupHeaderBand;
            reportSubGroupHeaderBand = new GroupHeaderBand();
            reportSubGroupHeaderBand.Level = 0;
            reportSubGroupHeaderBand.HeightF = 30;

            GroupField reportSubGroupField = new GroupField("ReportRompimiento2.GroupFieldValue");

            DetailBand reportTableDetailBand;
            reportTableDetailBand = new DetailBand();
            reportTableDetailBand.HeightF = 20;
            reportTableBand.Bands.Add(reportTableDetailBand);

            GroupFooterBand reportSubGroupFooterBand;
            reportSubGroupFooterBand = new GroupFooterBand();
            reportSubGroupFooterBand.Level = 0;
            reportSubGroupFooterBand.HeightF = 40;

            GroupFooterBand reportGroupFooterBand;
            reportGroupFooterBand = new GroupFooterBand();
            reportGroupFooterBand.Level = 1;
            reportGroupFooterBand.HeightF = 60;
            reportTableBand.Bands.Add(reportGroupFooterBand);

            GroupFooterBand reportTotalFooterBand;
            reportTotalFooterBand = new GroupFooterBand();
            reportTotalFooterBand.Level = 2;
            reportTotalFooterBand.HeightF = 40;
            reportTableBand.Bands.Add(reportTotalFooterBand);

            if (romp.Contains("Consecutivo"))
            {
                reportTableBand.Bands.Add(reportSubGroupHeaderBand);
                reportSubGroupHeaderBand.GroupFields.Add(reportSubGroupField);
                reportTableBand.Bands.Add(reportSubGroupFooterBand);
            }

            this.Bands.Add(reportTableBand);
            #endregion

            #region Report field width
            float tableWidth = 0;
            float columnWidth = 0;

            tableWidth = this.PageWidth - (this.Margins.Right + this.Margins.Left);

            columnWidth = tableWidth / fieldList.Count;
            #endregion

            #region Report elements
            #region Report period header
                                  
            XRShape periodFrame = new XRShape();
            periodFrame.LocationF = new PointF(tableWidth / 4, 10);
            periodFrame.SizeF = new SizeF(tableWidth/2, 40);
            periodFrame.LineWidth = 2;
            periodFrame.Shape = new ShapeRectangle()
            {
                Fillet = 50,
            };
            periodFrame.SendToBack();
            reportPeriodBand.Controls.Add(periodFrame);

            XRTable periodTable;
            XRTableRow periodTableRow;
            XRTableCell periodTableCell_FromMonth;
            XRTableCell periodTableCell_TillMonth;
            periodTable = new XRTable();
            periodTable.LocationF = periodFrame.LocationF;
            periodTable.SizeF = periodFrame.SizeF;
            periodTable.Font = new Font("Times New Roman", 10, FontStyle.Bold);
            periodTable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            periodTableRow = new XRTableRow();

            float periodTableCellWidth = tableWidth / 4;
            
            periodTableCell_FromMonth = new XRTableCell();
            periodTableCell_FromMonth.WidthF = periodTableCellWidth;
            periodTableCell_FromMonth.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_lblFrom") + ":   " + minDate.ToString("MMMM/yyyy");
            periodTableRow.Cells.Add(periodTableCell_FromMonth);

            periodTableCell_TillMonth = new XRTableCell();
            periodTableCell_TillMonth.WidthF = periodTableCellWidth;
            periodTableCell_TillMonth.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_lblUntil") + ":   " + maxDate.ToString("MMMM/yyyy");
            periodTableRow.Cells.Add(periodTableCell_TillMonth);
            
            periodTable.Rows.Add(periodTableRow);
            reportPeriodBand.Controls.Add(periodTable);

            XRLabel selectedFiltersLable = new XRLabel();
            selectedFiltersLable.LocationF = new PointF(0, periodFrame.LocationF.Y + periodFrame.HeightF + 10);
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
            groupHeaderFrame.LineWidth = 1;
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
            tableHeader.HeightF = reportTableHeaderBand.HeightF;
            tableHeader.StyleName = "tableHeaderStyle";
            tableHeaderRow = new XRTableRow();

            XRTable tableDetail;
            XRTableRow tableDetailRow;
            XRTableCell tableDetailCell;
            tableDetail = new XRTable();
            tableDetail.WidthF = tableWidth;
            tableDetail.HeightF = reportTableDetailBand.HeightF;
            tableDetail.EvenStyleName = "tableDetailEvenStyle";
            tableDetail.OddStyleName = "tableDetailOddStyle";
            tableDetailRow = new XRTableRow();
            tableDetailRow.Name = "tableDetailRow";
            tableDetailRow.HeightF = tableDetail.HeightF;
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

            #region Report total footer
            XRTable totalFooter;
            XRTableRow totalFooterRow;
            XRTableCell totalFooterCell_Name;
            XRTableCell totalFooterCell_Value;
            totalFooter = new XRTable();
            totalFooter.LocationF = new PointF(0, 10);
            totalFooter.SizeF = new SizeF(tableWidth, 40);
            totalFooter.StyleName = "totalFooterStyle";
            totalFooterRow = new XRTableRow();
            
            XRLine totalFooterLine_1 = new XRLine()
            {
                LineWidth = 2,
                SizeF = new SizeF(tableWidth, 2),
                LocationF = new PointF(0, totalFooter.LocationF.Y+totalFooter.HeightF + 20)
            };
            reportTotalFooterBand.Controls.Add(totalFooterLine_1);

            XRLine totalFooterLine_2 = new XRLine()
            {
                LineWidth = 2,
                SizeF = new SizeF(tableWidth, 2),
                LocationF = new PointF(0, totalFooter.LocationF.Y + totalFooter.HeightF + 24)
            };
            reportTotalFooterBand.Controls.Add(totalFooterLine_2);
            #endregion
            #endregion

            #region Report Table

            #region Calculated fields
            CalculatedField DocumentoNombre = new CalculatedField();
            DocumentoNombre.DataSource = reportData;
            DocumentoNombre.FieldType = FieldType.String;
            DocumentoNombre.DisplayName = "CajaID_calc";
            DocumentoNombre.Name = "CajaID_calc";
            DocumentoNombre.Expression = "Trim([CajaID]) + ' - ' + Trim([ReciboNro])";
            this.CalculatedFields.Add(DocumentoNombre);
            #endregion
            
            int TotalsInd = 0;
            float totalFieldLocation = 0;

            foreach (string fieldName in fieldList)
            {
                #region Report table header
                tableHeaderCell = new XRTableCell();
                tableHeaderCell.WidthF = columnWidth;

                if (fieldName.Contains("Desc"))
                    tableHeaderCell.WidthF = columnWidth + 150;
                if (fieldName.Contains("TerceroID") || fieldName.Contains("Fecha") || fieldName.Contains("Moneda"))
                    tableHeaderCell.WidthF = columnWidth - 50;

                string resourceFieldID = (AppReports.coReciboCaja).ToString() + "_" + fieldName;
                string tableColumnName = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, resourceFieldID);
                tableHeaderCell.Text = tableColumnName;

                tableHeaderRow.Controls.Add(tableHeaderCell);
                #endregion

                #region Report table detail
                tableDetailCell = new XRTableCell();
                tableDetailCell.WidthF = tableHeaderCell.WidthF;
                if (fieldName.Contains("Fecha")) tableDetailCell.DataBindings.Add("Text", this.DataSource, fieldName, "{0:dd/MM/yyyy}");
                else tableDetailCell.DataBindings.Add("Text", this.DataSource, fieldName);
                if (fieldName.Contains("Moneda"))
                    tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                else
                    if (fieldName.Contains("TerceroID"))
                    {
                        tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                        tableDetailCell.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 5, 0, 0);
                    }
                    else
                    {
                        tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                        tableDetailCell.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0);
                    };
                tableDetailRow.Controls.Add(tableDetailCell);
                #endregion

                #region Report table footer
                if (fieldName.Contains("Valor"))
                {
                    if (TotalsInd == 0)
                    {
                        totalFooterCell_Name = new XRTableCell();
                        totalFooterCell_Name.WidthF = totalFieldLocation;
                        totalFooterCell_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 20, 0, 0);
                        totalFooterCell_Name.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_Totals") + ": ";
                        totalFooterRow.Controls.Add(totalFooterCell_Name);

                        groupFooterCell_Name = new XRTableCell();
                        groupFooterCell_Name.WidthF = totalFieldLocation;
                        groupFooterCell_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 20, 0, 0);
                        groupFooterCell_Name.Name = "groupFooterCell_Name";
                        groupFooterRow.Controls.Add(groupFooterCell_Name);

                        if (romp.Contains("Consecutivo"))
                        {
                            subGroupFooterCell_Name = new XRTableCell();
                            subGroupFooterCell_Name.WidthF = totalFieldLocation;
                            subGroupFooterCell_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 20, 0, 0);
                            subGroupFooterCell_Name.Name = "subGroupFooterCell_Name";
                            subGroupFooterRow.Controls.Add(subGroupFooterCell_Name);
                        };

                        TotalsInd = 1;
                    };

                    totalFooterCell_Value = new XRTableCell();
                    totalFooterCell_Value.Name = fieldName + "_total";
                    totalFooterCell_Value.WidthF = tableHeaderCell.WidthF;
                    totalFooterCell_Value.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                    totalFooterCell_Value.BorderWidth = 2;
                    totalFooterCell_Value.Summary.Func = SummaryFunc.Sum;
                    totalFooterCell_Value.Summary.Running = SummaryRunning.Report;
                    totalFooterCell_Value.Summary.FormatString = "{0:#,0.00}";
                    totalFooterCell_Value.DataBindings.Add("Text", this.DataSource, fieldName);
                    totalFooterRow.Controls.Add(totalFooterCell_Value);

                    groupFooterCell_Value = new XRTableCell();
                    groupFooterCell_Value.Name = fieldName + "_group";
                    groupFooterCell_Value.WidthF = tableHeaderCell.WidthF;
                    groupFooterCell_Value.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                    groupFooterCell_Value.Summary.Func = SummaryFunc.Sum;
                    groupFooterCell_Value.Summary.Running = SummaryRunning.Group;
                    groupFooterCell_Value.Summary.FormatString = "{0:#,0.00}";
                    groupFooterCell_Value.DataBindings.Add("Text", this.DataSource, fieldName);
                    groupFooterRow.Controls.Add(groupFooterCell_Value);

                        if (romp.Contains("Consecutivo"))
                    {
                        subGroupFooterCell_Value = new XRTableCell();
                        subGroupFooterCell_Value.Name = fieldName + "_subgroup";
                        subGroupFooterCell_Value.WidthF = tableHeaderCell.WidthF;
                        subGroupFooterCell_Value.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                        subGroupFooterCell_Value.Summary.Func = SummaryFunc.Sum;
                        subGroupFooterCell_Value.Summary.Running = SummaryRunning.Group;
                        subGroupFooterCell_Value.Summary.FormatString = "{0:#,0.00}";
                        subGroupFooterCell_Value.DataBindings.Add("Text", this.DataSource, fieldName);
                        subGroupFooterRow.Controls.Add(subGroupFooterCell_Value);
                    };
                };

                if (TotalsInd == 0)
                    totalFieldLocation += tableHeaderCell.WidthF;
                #endregion
            };

            tableHeader.Controls.Add(tableHeaderRow);
            tableDetail.Controls.Add(tableDetailRow);
            groupFooter.Controls.Add(groupFooterRow);
            totalFooter.Controls.Add(totalFooterRow);
            
            reportTableHeaderBand.Controls.Add(tableHeader);
            reportTableDetailBand.Controls.Add(tableDetail);
            reportGroupFooterBand.Controls.Add(groupFooter);
            reportTotalFooterBand.Controls.Add(totalFooter);

            if (romp.Contains("Consecutivo"))
            {
                subGroupFooter.Controls.Add(subGroupFooterRow);
                reportSubGroupFooterBand.Controls.Add(subGroupFooter);
            };

            #endregion

            #region Report group header
            groupHeaderCell_Name = new XRTableCell();
            groupHeaderCell_Name.Name = "groupHeaderCell_Name";
            groupHeaderCell_Name.WidthF = 100;               
            groupHeaderCell_Name.DataBindings.Add("Text", this.DataSource, "ReportRompimiento1.GroupFieldName");
            groupHeaderRow.Controls.Add(groupHeaderCell_Name);
            groupHeaderCell_Name.BeforePrint += new System.Drawing.Printing.PrintEventHandler(groupHeaderCell_Name_BeforePrint);

            groupHeaderCell_Value = new XRTableCell();
            groupHeaderCell_Value.WidthF = 100;
            groupHeaderCell_Value.DataBindings.Add("Text", this.DataSource, "ReportRompimiento1.GroupFieldValue");
             groupHeaderRow.Controls.Add(groupHeaderCell_Value);

            groupHeaderCell_Desc = new XRTableCell();
            groupHeaderCell_Desc.WidthF = tableWidth - 200;
            groupHeaderCell_Desc.DataBindings.Add("Text", this.DataSource, "ReportRompimiento1.GroupFieldDesc");
            groupHeaderRow.Controls.Add(groupHeaderCell_Desc);

            groupHeader.Controls.Add(groupHeaderRow);
            reportGroupHeaderBand.Controls.Add(groupHeader);
            #endregion

            #region Report sub group header
            if (romp.Contains("Consecutivo"))
            {
                subGroupHeaderCell_Name = new XRTableCell();
                subGroupHeaderCell_Name.Name = "subGroupHeaderCell_Name";
                subGroupHeaderCell_Name.WidthF = 100;
                subGroupHeaderCell_Name.DataBindings.Add("Text", this.DataSource, "ReportRompimiento2.GroupFieldName");
                subGroupHeaderCell_Name.BeforePrint += new System.Drawing.Printing.PrintEventHandler(groupHeaderCell_Name_BeforePrint);
                subGroupHeaderRow.Controls.Add(subGroupHeaderCell_Name);

                subGroupHeaderCell_Value = new XRTableCell();
                subGroupHeaderCell_Value.WidthF = tableWidth - 130;
                subGroupHeaderCell_Value.DataBindings.Add("Text", this.DataSource, "ReportRompimiento2.GroupFieldValue");
                subGroupHeaderRow.Controls.Add(subGroupHeaderCell_Value);         
                subGroupHeaderCell_Value.BeforePrint +=new System.Drawing.Printing.PrintEventHandler(subGroupHeaderCell_Value_BeforePrint);

                subGroupHeader.Controls.Add(subGroupHeaderRow);
                reportSubGroupHeaderBand.Controls.Add(subGroupHeader);
            };
            #endregion
        } 
        #endregion

        #region Eventos
        /// <summary>
        /// Checks if the group header should be printed and puts proper field captions depending on group field name
        /// </summary>
        private void subGroupHeaderCell_Value_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell headerCell = (XRTableCell)sender;
            if (!string.IsNullOrEmpty(headerCell.Text) && !string.IsNullOrWhiteSpace(headerCell.Text))
                headerCell.Text = (DateTime.Parse(headerCell.Text)).ToString("dd/ MMMM / yyyy");
            else
                e.Cancel = true;
        }

        /// <summary>
        /// Puts proper field captions depending on group field name
        /// </summary>
        private void groupHeaderCell_Name_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell headerCell = (XRTableCell)sender;
            if (!string.IsNullOrEmpty(headerCell.Text) && !string.IsNullOrWhiteSpace(headerCell.Text))
                headerCell.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.coReciboCaja).ToString() + "_" + headerCell.Text);

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
