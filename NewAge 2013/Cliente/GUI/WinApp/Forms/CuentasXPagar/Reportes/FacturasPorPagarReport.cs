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
    class FacturasPorPagarReport : BaseReport //XtraReport//
    {
        #region Variables

        private int _rowInd;
        private string _docID;
        BaseController _bc = BaseController.GetInstance(); 
        #endregion

        #region Funciones Publicas
        /// <summary>
        /// Facturas Por Pagar Report Constructor
        /// </summary>
        /// <param name="reportData"> data for the report </param>
        /// <param name="fieldList"> list of fields for report detail table</param>
        /// <param name="MM"> Moneda type of the report - local, foreign, both</param>
        /// <param name="Date">periodo del reporte</param>
        /// <param name="selectedFiltersList">list of filters assigned by the user</param>
        /// <param name="detailInd">Indicador del tipo del reporte (true - detallado, false - resumido)</param>
        public FacturasPorPagarReport(List<DTO_ReportFacturasPorPagar> reportData, ArrayList fieldList, TipoMoneda MM, DateTime Date, int signFin, List<string> selectedFiltersList, bool detailInd, string docID)
        {
            _docID = docID;
            this.lblReportName.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.cpFacturasPorPagar).ToString() + "_Report");

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
            reportPeriodBand.Level = 4;
            reportPeriodBand.HeightF = (selectedFiltersList.Count > 0) ? 130 : 100;
            reportTableBand.Bands.Add(reportPeriodBand);

            GroupHeaderBand reportTableHeaderBand;
            reportTableHeaderBand = new GroupHeaderBand();
            reportTableHeaderBand.Level = 3;
            reportTableHeaderBand.HeightF = (MM == TipoMoneda.Both) ? 55 : 30;
            reportTableHeaderBand.RepeatEveryPage = true;
            reportTableBand.Bands.Add(reportTableHeaderBand);

            GroupHeaderBand reportGroupHeaderBand;
            reportGroupHeaderBand = new GroupHeaderBand();
            reportGroupHeaderBand.Level = 2;
            reportGroupHeaderBand.HeightF = 40;

            GroupField reportGroupField = new GroupField("ReportRompimiento1.GroupFieldValue");
            reportGroupField.SortOrder = XRColumnSortOrder.Ascending;

            GroupHeaderBand reportDetailGroupHeaderBand;
            reportDetailGroupHeaderBand = new GroupHeaderBand();
            reportDetailGroupHeaderBand.Level = 0;
            reportDetailGroupHeaderBand.HeightF = 0;

            GroupField reportDetailGroupField = new GroupField("ReportRompimiento1.GroupFieldValue");
            reportDetailGroupField.SortOrder = XRColumnSortOrder.Ascending;

            DetailBand reportTableDetailBand;
            reportTableDetailBand = new DetailBand();
            reportTableDetailBand.HeightF = (detailInd) ? 20 : 0;
            reportTableBand.Bands.Add(reportTableDetailBand);

            GroupFooterBand reportDetailGroupFooterBand;
            reportDetailGroupFooterBand = new GroupFooterBand();
            reportDetailGroupFooterBand.Level = 0;
            reportDetailGroupFooterBand.HeightF = 20;
            reportDetailGroupFooterBand.BeforePrint += new System.Drawing.Printing.PrintEventHandler(reportDetailGroupFooterBand_BeforePrint);

            GroupFooterBand reportGroupFooterBand;
            reportGroupFooterBand = new GroupFooterBand();
            reportGroupFooterBand.Level = 2;
            reportGroupFooterBand.HeightF = 65;

            GroupFooterBand reportTotalFooterBand;
            reportTotalFooterBand = new GroupFooterBand();
            reportTotalFooterBand.Level = 3;
            reportTotalFooterBand.HeightF = 70;
            reportTableBand.Bands.Add(reportTotalFooterBand);

            if (!detailInd)
            {
                reportTableBand.Bands.Add(reportDetailGroupHeaderBand);
                reportDetailGroupHeaderBand.GroupFields.Add(reportDetailGroupField);
                reportTableBand.Bands.Add(reportDetailGroupFooterBand);
            }
            else
            {
                reportTableBand.Bands.Add(reportGroupHeaderBand);
                reportGroupHeaderBand.GroupFields.Add(reportGroupField);
                reportTableBand.Bands.Add(reportGroupFooterBand);
            };

            this.Bands.Add(reportTableBand);
            #endregion

            #region Report field width
            float tableWidth = 0;
            float columnWidth = 0;

            tableWidth = this.PageWidth - (this.Margins.Right + this.Margins.Left);

            columnWidth = (tableWidth - 50) / fieldList.Count;
            #endregion

            #region Report elements
            #region Report period header

            XRShape estadoFrame = new XRShape()
            {
                LocationF = new PointF(5*tableWidth / 6 - 30, 0),
                SizeF = new System.Drawing.SizeF(tableWidth / 6, 35),
                BackColor = Color.Transparent,
                AnchorVertical = VerticalAnchorStyles.Top,
                LineWidth = 4,
                ForeColor = Color.LightGray
            };
            estadoFrame.Shape = new ShapeRectangle() { Fillet = 30 };
            reportPeriodBand.Controls.Add(estadoFrame);

            XRLabel estadoLabel = new XRLabel();
            estadoLabel.LocationF = estadoFrame.LocationF;
            estadoLabel.SizeF = estadoFrame.SizeF;
            estadoLabel.CanGrow = false;
            estadoLabel.Font = new Font("Times New Roman", 12, FontStyle.Italic);
            estadoLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            estadoLabel.BackColor = Color.Transparent;
            estadoLabel.ForeColor = Color.DarkGray;
            estadoLabel.Text = (Date.Year == DateTime.Now.Year && Date.Month == DateTime.Now.Month) ? _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.cpFacturasPorPagar).ToString() + "_Preliminar") : _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.cpFacturasPorPagar).ToString() + "_Definitivo"); 
            reportPeriodBand.Controls.Add(estadoLabel);

            XRShape periodFrame = new XRShape();
            periodFrame.LocationF = new PointF(tableWidth / 4, estadoFrame.LocationF.Y + estadoFrame.HeightF + 10);
            //periodFrame.LocationF = new PointF(0, 0);
            periodFrame.SizeF = new SizeF(tableWidth/2, 40);
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
            periodTable = new XRTable();
            periodTable.LocationF = periodFrame.LocationF;
            periodTable.SizeF = periodFrame.SizeF;
            periodTable.Font = new Font("Times New Roman", 10, FontStyle.Bold);
            periodTable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            periodTableRow = new XRTableRow();

            float periodTableCellWidth = tableWidth / 3;

            periodTableCell_Year = new XRTableCell();
            periodTableCell_Year.WidthF = periodTableCellWidth;
            periodTableCell_Year.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_lblYear") + ":   " + Date.ToString("yyyy");
            periodTableRow.Cells.Add(periodTableCell_Year);

            periodTableCell_Month = new XRTableCell();
            periodTableCell_Month.WidthF = periodTableCellWidth;
            periodTableCell_Month.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_lblMonth") + ":   " + Date.ToString("MMMM");
            periodTableRow.Cells.Add(periodTableCell_Month);

            periodTableCell_Moneda = new XRTableCell();
            periodTableCell_Moneda.WidthF = periodTableCellWidth;
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
                    break;
            };
            periodTableRow.Cells.Add(periodTableCell_Moneda);

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

            #region Report Table detail
            CalculatedField calcField;

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
                    WidthF = tableWidth - 6 * columnWidth,
                    BackColor = Color.White,
                };
                XRTableCell MLCell = new XRTableCell()
                {
                    WidthF = 3 * columnWidth,
                    Text = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyLocal)
                };
                XRTableCell MECell = new XRTableCell()
                {
                    WidthF = 3 * columnWidth,
                    Text = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyForeign)
                };
                tableHeaderUpperRow.Cells.AddRange(new XRTableCell[] { emptyCell, MLCell, MECell });
                tableHeader.Rows.Add(tableHeaderUpperRow);
            };

            XRTable tableDetail;
            XRTableRow tableDetailRow;
            XRTableCell tableDetailCell;
            tableDetail = new XRTable();
            tableDetail.WidthF = tableWidth;
            tableDetail.HeightF = 20;
            if (detailInd)
            {
                tableDetail.EvenStyleName = "tableDetailEvenStyle";
                tableDetail.OddStyleName = "tableDetailOddStyle";
            }
            else
                tableDetail.StyleName = "tableDetailEvenStyle";
            tableDetailRow = new XRTableRow();
            tableDetailRow.Name = "tableDetailRow";
            tableDetailRow.HeightF = tableDetail.HeightF;

            _rowInd = 0;
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

            XRLine totalFooterLowerLine_1 = new XRLine()
            {
                LineWidth = 2,
                SizeF = new SizeF(tableWidth, 3),
                LocationF = new PointF(0, reportTotalFooterBand.HeightF - 5)
            };
            reportTotalFooterBand.Controls.Add(totalFooterLowerLine_1);

            XRLine totalFooterLowerLine_2 = new XRLine()
            {
                LineWidth = 2,
                SizeF = new SizeF(tableWidth, 3),
                LocationF = new PointF(0, reportTotalFooterBand.HeightF - 1)
            };
            reportTotalFooterBand.Controls.Add(totalFooterLowerLine_2);
            #endregion
            #endregion

            int TotalsInd = 0;
            float totalFieldLocation = 0;

            #region Report Table

            foreach (string fieldName in fieldList)
            {
                #region Report table header
                tableHeaderCell = new XRTableCell();
                if (fieldName.Contains("Desc"))
                    tableHeaderCell.WidthF = columnWidth + 50;
                else
                    tableHeaderCell.WidthF = columnWidth;
                string resourceFieldID ="";
                switch (docID)
                {
                    case "21":
                        resourceFieldID = (AppReports.cpFacturasPorPagar).ToString() + "_" + fieldName;
                        break;
                    case "22":
                        resourceFieldID = (AppReports.cpAnticiposPendientes).ToString() + "_" + fieldName;
                        break;
                }
                string tableColumnName = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, resourceFieldID);
                tableHeaderCell.Text = tableColumnName;

                tableHeaderRow.Controls.Add(tableHeaderCell);
                #endregion

                #region Report table detail
                tableDetailCell = new XRTableCell();
                tableDetailCell.WidthF = tableHeaderCell.WidthF;
                if (!fieldName.Contains("ML") && !fieldName.Contains("ME"))
                {
                    if (!fieldName.Contains("CuentaID")) tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                    tableDetailCell.Padding = new DevExpress.XtraPrinting.PaddingInfo(5,2,0,0);

                    if (fieldName.Contains("Fecha"))
                    {
                        tableDetailCell.DataBindings.Add("Text", this.DataSource, fieldName, "{0:dd/MM/yyyy}");
                        tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    }
                    else tableDetailCell.DataBindings.Add("Text", this.DataSource, fieldName);

                    if (fieldName.Contains("Comprobante"))
                    {
                        tableDetailCell.WidthF = tableHeaderCell.WidthF/2 - 10;
                        tableDetailCell.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0);
                        tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                        tableDetailRow.Controls.Add(tableDetailCell);

                        tableDetailCell = new XRTableCell();
                        tableDetailCell.WidthF = tableHeaderCell.WidthF / 2 + 10;
                        tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                        tableDetailCell.DataBindings.Add("Text", this.DataSource, "ComprobanteNro", "-   {0}");
                    }
                }
                else
                {
                    if (detailInd)
                        tableDetailCell.DataBindings.Add("Text", this.DataSource, fieldName, "{0:#,0.00}");
                    else
                    {
                        tableDetailCell.Summary.Func = SummaryFunc.Sum;
                        tableDetailCell.Summary.Running = SummaryRunning.Group;
                        tableDetailCell.Summary.FormatString = "{0:#,0.00}";
                        tableDetailCell.DataBindings.Add("Text", this.DataSource, fieldName);
                    };
                };

                tableDetailRow.Controls.Add(tableDetailCell);
                #endregion

                #region Report table footer
                if (fieldName.Contains("ML") || fieldName.Contains("ME"))
                {
                    if (TotalsInd == 0)
                    {
                        totalFooterCell_Name = new XRTableCell();
                        totalFooterCell_Name.WidthF = totalFieldLocation;
                        totalFooterCell_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 20, 0, 0);
                        totalFooterCell_Name.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_TotalGeneral") + ": ";
                        totalFooterRow.Controls.Add(totalFooterCell_Name);

                        if (detailInd)
                        {
                            groupFooterCell_Name = new XRTableCell();
                            groupFooterCell_Name.WidthF = totalFieldLocation;
                            groupFooterCell_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 20, 0, 0);
                            groupFooterCell_Name.Name = "groupFooterCell_Name";
                            groupFooterRow.Controls.Add(groupFooterCell_Name);
                        };

                        TotalsInd = 1;
                    };

                    calcField = new CalculatedField();
                    this.CalculatedFields.Add(calcField);
                    calcField.DataSource = reportData;
                    calcField.Expression = fieldName.Contains("Saldo") ? "[" + fieldName + "_sinSigno]*" + signFin : fieldName;
                    calcField.FieldType = FieldType.Double;
                    calcField.Name = fieldName + "_calc";

                    totalFooterCell_Value = new XRTableCell();
                    totalFooterCell_Value.Name = fieldName + "_total";
                    totalFooterCell_Value.WidthF = tableHeaderCell.WidthF;
                    totalFooterCell_Value.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                    totalFooterCell_Value.BorderWidth = 2;
                    totalFooterCell_Value.Summary.Func = SummaryFunc.Sum;
                    totalFooterCell_Value.Summary.Running = SummaryRunning.Report;
                    totalFooterCell_Value.Summary.FormatString = "{0:#,0.00}";
                    totalFooterCell_Value.DataBindings.Add("Text", this.DataSource, fieldName+"_calc");
                    totalFooterRow.Controls.Add(totalFooterCell_Value);

                    if (detailInd)
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
                };

                if (TotalsInd == 0)
                    totalFieldLocation += tableHeaderCell.WidthF;
                #endregion
            };

            tableHeader.Controls.Add(tableHeaderRow);
            tableDetail.Controls.Add(tableDetailRow);
            totalFooter.Controls.Add(totalFooterRow);

            reportTableHeaderBand.Controls.Add(tableHeader);
            if (detailInd) reportTableDetailBand.Controls.Add(tableDetail);
            else reportDetailGroupFooterBand.Controls.Add(tableDetail);
            reportTotalFooterBand.Controls.Add(totalFooter);

            if (detailInd)
            {
                groupFooter.Controls.Add(groupFooterRow);
                reportGroupFooterBand.Controls.Add(groupFooter);
            };
            #endregion

            #region Report group header
            if (detailInd)
            {
                groupHeaderCell_Name = new XRTableCell();
                groupHeaderCell_Name.Name = "groupHeaderCell_Name";
                groupHeaderCell_Name.WidthF = 30;
                groupHeaderCell_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
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
                headerCell.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.cpFacturasPorPagar).ToString() + "_" + headerCell.Text);
                    
                XRTableCell footerCell = FindControl("groupFooterCell_Name", true) as XRTableCell;
                footerCell.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_Totals") + "  x  " + headerCell.Text;
        }

        /// <summary>
        /// Rows color alteration
        /// </summary>
        private void reportDetailGroupFooterBand_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableRow row = FindControl("tableDetailRow", true) as XRTableRow;

            if (_rowInd != 0 && _rowInd % 2 != 0)
            {
                row.BackColor = Color.WhiteSmoke;
            }
            else
            {
                row.BackColor = Color.White;
            };

            _rowInd++;
        } 
        #endregion
    }
}
