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
    class SaldosDocumentoReport : BaseReport //XtraReport//
    {
        #region Variables

        private int _rowInd;
        BaseController _bc = BaseController.GetInstance(); 
        #endregion

        #region Funciones Publicas
        /// <summary>
        /// Saldos Documentos Report Constructor
        /// </summary>
        /// <param name="reportData"> data for the report </param>
        /// <param name="fieldList"> list of fields for report detail table</param>
        /// <param name="MM"> Moneda type of the report - local, foreign, both</param>
        /// <param name="Date">periodo del reporte</param>
        /// <param name="RompIndList">enables rompimiento (true - enabled)</param>
        /// <param name="selectedFiltersList">list of filters assigned by the user</param>
        /// <param name="docID">ID of the reported Document</param>
        public SaldosDocumentoReport(List<DTO_ReportSaldosDocumentos> reportData, ArrayList fieldList, TipoMoneda MM, DateTime Date, List<bool> RompIndList, List<string> selectedFiltersList, string docID)
        {
            this.lblReportName.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.coSaldosDocumentos).ToString());

            #region Report styles

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
            bool detailInd = (RompIndList[0] && RompIndList[1]) || (RompIndList[0] && RompIndList[2]);

            DetailReportBand reportTableBand;
            reportTableBand = new DetailReportBand();
            reportTableBand.Name = "reportTableBand";
            reportTableBand.DataSource = reportData;

            GroupHeaderBand reportPeriodBand;
            reportPeriodBand = new GroupHeaderBand();
            reportPeriodBand.Level = 4;
            reportPeriodBand.HeightF = (selectedFiltersList.Count > 0) ? 130 : 100;
            reportPeriodBand.RepeatEveryPage = true;
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

            GroupHeaderBand reportSubGroupHeaderBand;
            reportSubGroupHeaderBand = new GroupHeaderBand();
            reportSubGroupHeaderBand.Level = 1;
            reportSubGroupHeaderBand.HeightF = 30;

            GroupField reportSubGroupField = new GroupField("ReportRompimiento2.GroupFieldValue");
            reportSubGroupField.SortOrder = XRColumnSortOrder.Ascending;

            GroupHeaderBand reportDetailGroupHeaderBand;
            reportDetailGroupHeaderBand = new GroupHeaderBand();
            reportDetailGroupHeaderBand.Level = 0;
            reportDetailGroupHeaderBand.HeightF = 0;

            GroupField reportDetailGroupField = (RompIndList[0]) ? new GroupField("ReportRompimiento2.GroupFieldValue") : new GroupField("ReportRompimiento1.GroupFieldValue");
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
            reportTotalFooterBand.HeightF = 70;
            reportTableBand.Bands.Add(reportTotalFooterBand);

            if (!detailInd)
            {
                reportTableBand.Bands.Add(reportDetailGroupHeaderBand);
                reportDetailGroupHeaderBand.GroupFields.Add(reportDetailGroupField);
                reportTableBand.Bands.Add(reportDetailGroupFooterBand);
            };

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

            columnWidth = (tableWidth - 50) / fieldList.Count;
            #endregion

            #region Report elements
            #region Report period header

            XRShape estadoFrame = new XRShape()
            {
                LocationF = new PointF(0, 0),
                SizeF = new System.Drawing.SizeF(tableWidth / 3, 35),
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
            estadoLabel.Text = docID + " - " + ((NewAge.DTO.Negocio.DTO_MasterBasic)((DTO_glDocumento)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.glDocumento, new UDT_BasicID() { Value = docID }, true))).Descriptivo.ToString();
            reportPeriodBand.Controls.Add(estadoLabel);

            XRShape periodFrame = new XRShape();
            periodFrame.LocationF = new PointF(0, estadoFrame.LocationF.Y + estadoFrame.HeightF + 10);
            //periodFrame.LocationF = new PointF(0, 0);
            periodFrame.SizeF = new SizeF(tableWidth, 40);
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
            if (MM == TipoMoneda.Both)
            {
                XRTableRow tableHeaderUpperRow = new XRTableRow()
                {
                    Borders = DevExpress.XtraPrinting.BorderSide.All,
                    BorderColor = Color.White
                };
                XRTableCell emptyCell = new XRTableCell()
                {
                    WidthF = tableWidth - 2 * columnWidth,
                    BackColor = Color.White,
                };
                XRTableCell MLCell = new XRTableCell()
                {
                    WidthF = 1 * columnWidth,
                    Text = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyLocal)
                };
                XRTableCell MECell = new XRTableCell()
                {
                    WidthF = 1 * columnWidth,
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

            CalculatedField DocumentoNombre = new CalculatedField();
            DocumentoNombre.DataSource = reportData;
            DocumentoNombre.FieldType = FieldType.String;
            DocumentoNombre.DisplayName = "DocumentoNombre";
            DocumentoNombre.Name = "DocumentoNombre";
            DocumentoNombre.Expression = "Iif([DocSaldoControl] == " + (int)SaldoControl.Doc_Interno + ",Trim([DocumentoPrefijo]) + ' - ' + Trim([DocumentoNumero]),[DocumentoTercero])";
            this.CalculatedFields.Add(DocumentoNombre);

            foreach (string fieldName in fieldList)
            {
                #region Report table header
                tableHeaderCell = new XRTableCell();
                if (fieldName.Contains("Desc"))
                    tableHeaderCell.WidthF = columnWidth + 50;
                else
                    tableHeaderCell.WidthF = columnWidth;

                string resourceFieldID = (AppReports.coSaldosDocumentos).ToString() + "_" + fieldName;
                string tableColumnName = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, resourceFieldID);
                tableHeaderCell.Text = tableColumnName;

                tableHeaderRow.Controls.Add(tableHeaderCell);
                #endregion

                #region Report table detail
                tableDetailCell = new XRTableCell();
                tableDetailCell.WidthF = tableHeaderCell.WidthF;
                if (!fieldName.Contains("ML") && !fieldName.Contains("ME"))
                {
                    tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                    tableDetailCell.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0);
                    if (fieldName.Contains("Fecha")) tableDetailCell.DataBindings.Add("Text", this.DataSource, fieldName, "{0:dd/MM/yyyy}");
                    else tableDetailCell.DataBindings.Add("Text", this.DataSource, fieldName);
                }
                else
                {
                    if (detailInd)
                    {
                        tableDetailCell.DataBindings.Add("Text", this.DataSource, fieldName, "{0:#,0.00}");
                    }
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
                        totalFooterCell_Name.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_Totals") + ": ";
                        totalFooterRow.Controls.Add(totalFooterCell_Name);

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

                    if (RompIndList[0])
                    {
                        groupFooterCell_Value = new XRTableCell();
                        groupFooterCell_Value.Name = fieldName + "_group";
                        groupFooterCell_Value.WidthF = tableHeaderCell.WidthF;
                        groupFooterCell_Value.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                        groupFooterCell_Value.Summary.Func = SummaryFunc.Sum;
                        groupFooterCell_Value.Summary.Running = SummaryRunning.Group;
                        groupFooterCell_Value.Summary.FormatString = "{0:#,0.00}";
                        groupFooterCell_Value.DataBindings.Add("Text", this.DataSource, fieldName);
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
            totalFooter.Controls.Add(totalFooterRow);

            reportTableHeaderBand.Controls.Add(tableHeader);
            if (detailInd) reportTableDetailBand.Controls.Add(tableDetail);
            else reportDetailGroupFooterBand.Controls.Add(tableDetail);
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
        /// Puts proper field captions depending on group field name
        /// </summary>
        private void groupHeaderCell_Name_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell headerCell = (XRTableCell)sender;
            if (!string.IsNullOrEmpty(headerCell.Text) && !string.IsNullOrWhiteSpace(headerCell.Text))
                headerCell.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.coSaldosDocumentos).ToString() + "_" + headerCell.Text);

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
