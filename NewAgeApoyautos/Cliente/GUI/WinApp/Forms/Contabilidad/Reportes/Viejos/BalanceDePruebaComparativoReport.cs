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

namespace NewAge.Cliente.GUI.WinApp.Reports
{
    class BalanceDePruebaComparativoReport : BaseReport //XtraReport//
    {
        #region Variables

        private int _rowInd;
        BaseController _bc = BaseController.GetInstance(); 
        #endregion

        #region Funciones Publicos
        /// <summary>
        /// Balance De Prueba Comparativo Report Constructor
        /// </summary>
        /// <param name="reportData">data for the report</param>
        /// <param name="fieldList">list of fields for report detail table</param>
        /// <param name="MM"> Moneda type of the report - local, foreign, both</param>
        /// <param name="balanceTipo">tipo de balance</param>
        /// <param name="minDate">initial date of the period</param>
        /// <param name="maxDate">final date of the period</param>
        /// <param name="selectedFiltersList">list of filters assigned by the user</param>
        /// <param name="cuentaFuncInd">indicador del tipo de la cuenta (true - cuenta Funcional;false - cuenta Alterna)</param>
        public BalanceDePruebaComparativoReport(List<DTO_ReportBalanceDePruebaComparativo> reportData, List<string> fieldList, TipoMoneda MM, string balanceTipo, DateTime minDate, DateTime maxDate, List<string> selectedFiltersList)
        {
            this.lblReportName.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.coBalanceDePrueba).ToString());

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
            reportPeriodBand.Level = 2;
            reportPeriodBand.HeightF = 45;
            reportPeriodBand.RepeatEveryPage = true;
            reportTableBand.Bands.Add(reportPeriodBand);

            GroupHeaderBand reportTableHeaderBand;
            reportTableHeaderBand = new GroupHeaderBand();
            reportTableHeaderBand.Level = 0;
            //reportTableHeaderBand.HeightF = (MM == TipoMoneda.Both) ? 55 : 30;
            reportTableHeaderBand.HeightF = 55;
            reportTableHeaderBand.RepeatEveryPage = true;
            reportTableBand.Bands.Add(reportTableHeaderBand);

            //GroupHeaderBand reportGroupHeaderBand;
            //reportGroupHeaderBand = new GroupHeaderBand();
            //reportGroupHeaderBand.Level = 0;
            //reportGroupHeaderBand.HeightF = 40;
            //reportTableBand.Bands.Add(reportGroupHeaderBand);

            //GroupField reportGroupField = (new GroupField("ReportRompimiento1.GroupFieldValue"));
            //reportGroupField.SortOrder = XRColumnSortOrder.Ascending; ;
            //reportGroupHeaderBand.GroupFields.Add(reportGroupField);

            DetailBand reportTableDetailBand;
            reportTableDetailBand = new DetailBand();
            reportTableDetailBand.HeightF = 20;
            reportTableBand.Bands.Add(reportTableDetailBand);

            //GroupFooterBand reportGroupFooterBand;
            //reportGroupFooterBand = new GroupFooterBand();
            //reportGroupFooterBand.Level = 0;
            //reportGroupFooterBand.HeightF = 85;
            //reportTableBand.Bands.Add(reportGroupFooterBand);

            GroupFooterBand reportTotalFooterBand;
            reportTotalFooterBand = new GroupFooterBand();
            reportTotalFooterBand.Level = 0;
            reportTotalFooterBand.HeightF = 110;
            reportTableBand.Bands.Add(reportTotalFooterBand);

            this.Bands.Add(reportTableBand);
            #endregion

            #region Report field width
            float tableWidth = 0;
            float columnWidth = 0;

            tableWidth = this.PageWidth - (this.Margins.Right + this.Margins.Left);

            columnWidth = (tableWidth - 100) / fieldList.Count;
            #endregion

            #region Report elements
            #region Report period header
            this.lblNombreEmpresa.LocationF = new PointF(100, 40);

            XRLabel lblEmpresaNit_Name = new XRLabel();
            lblEmpresaNit_Name.LocationF = new PointF(0, this.lblNombreEmpresa.LocationF.Y + this.lblNombreEmpresa.HeightF + 20);
            lblEmpresaNit_Name.SizeF = new SizeF(130, 20);
            lblEmpresaNit_Name.Font = new Font("Arial", 10, FontStyle.Bold);
            lblEmpresaNit_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            lblEmpresaNit_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(100, 0, 0, 0);
            lblEmpresaNit_Name.Text = "Nit:";//_bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_Nit"); 
            this.ReportHeader.Controls.Add(lblEmpresaNit_Name);

            XRLabel lblEmpresaNit_Value = new XRLabel();
            lblEmpresaNit_Value.LocationF = new PointF(lblEmpresaNit_Name.LocationF.X + lblEmpresaNit_Name.WidthF, lblEmpresaNit_Name.LocationF.Y);
            lblEmpresaNit_Value.SizeF = new SizeF(tableWidth - lblEmpresaNit_Name.WidthF, 20);
            lblEmpresaNit_Value.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0);
            lblEmpresaNit_Value.Font = new Font("Arial", 10);
            lblEmpresaNit_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            lblEmpresaNit_Value.Text = this._bc.GetControlValueByCompany(ModulesPrefix.co,AppControl.co_TerceroXDefecto);
            this.ReportHeader.Controls.Add(lblEmpresaNit_Value);

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
            periodTableCell_FromMonth.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_lblFrom") + ":   " 
                + ((minDate.Day == 1) ? minDate.ToString("MMMM") : _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_deCierre"));
            periodTableRow.Cells.Add(periodTableCell_FromMonth); 

            periodTableCell_TillMonth = new XRTableCell();
            periodTableCell_TillMonth.WidthF = periodTableCellWidth;
            periodTableCell_TillMonth.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_lblUntil") + ":   " 
                + ((maxDate.Day == 1) ? maxDate.ToString("MMMM") : _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_deCierre"));
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

            #region Report Table detail
            XRTable tableHeader;
            XRTableRow tableHeaderRow;
            XRTableCell tableHeaderCell;
            tableHeader = new XRTable();
            tableHeader.WidthF = tableWidth;
            tableHeader.HeightF = reportTableHeaderBand.HeightF - 5;
            tableHeader.StyleName = "tableHeaderStyle";
            XRTableRow tableHeaderUpperRow = new XRTableRow()
            {
                Borders = DevExpress.XtraPrinting.BorderSide.All,
                BorderColor = Color.White
            };
            XRTableCell emptyCell = new XRTableCell()
            {
                WidthF = 2 * columnWidth + 100,
                BackColor = Color.White,
            };
            XRTableCell Cell_1 = new XRTableCell()
            {
                WidthF = 3 * columnWidth,
                Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.coBalanceDePruebaComparativo).ToString() + "_PeriodoCorriente")
            };
            XRTableCell Cell_2 = new XRTableCell()
            {
                WidthF = 3 * columnWidth,
                Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.coBalanceDePruebaComparativo).ToString() + "_Acumulado")
            };
            tableHeaderUpperRow.Cells.AddRange(new XRTableCell[] { emptyCell, Cell_1, Cell_2 });
            tableHeader.Rows.Add(tableHeaderUpperRow);
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

            #region Report total footer
            XRTable totalFooter;
            XRTableRow totalFooterRow;
            XRTableCell totalFooterCell_Name;
            XRTableCell totalFooterCell_blank;
            XRTableCell totalFooterCell_Value;
            XRTableCell totalFooterCell_TotalValue;
            totalFooter = new XRTable();
            totalFooter.LocationF = new PointF(0, 20);
            totalFooter.SizeF = new SizeF(tableWidth, 60);
            totalFooter.StyleName = "totalFooterStyle";
            totalFooterRow = new XRTableRow();
            totalFooterRow.HeightF = 30;

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

            int totalsInd = 0;
            CalculatedField calcField;

            #region Report Table
            foreach (string fieldName in fieldList)
            {
                #region Report table header
                tableHeaderCell = new XRTableCell();
                tableHeaderCell.WidthF = (fieldName.Contains("Desc"))? columnWidth + 100 : columnWidth;
                
                if (fieldName.Contains("prev"))
                    tableHeaderCell.Text = maxDate.AddYears(-1).ToString("yyyy");
                else if (fieldName.Contains("curr"))
                    tableHeaderCell.Text = maxDate.ToString("yyyy");
                else 
                {
                    string resourceFieldID = (AppReports.coBalanceDePruebaComparativo).ToString() + "_" + fieldName;
                    string tableColumnName = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, resourceFieldID);
                    tableHeaderCell.Text = tableColumnName;
                }
                tableHeaderRow.Controls.Add(tableHeaderCell);
                #endregion

                #region Report table detail
                tableDetailCell = new XRTableCell();
                tableDetailCell.WidthF = tableHeaderCell.WidthF;
                if (fieldName.Contains("Cuenta") || fieldName.Contains("Dif"))
                {
                    tableDetailCell.DataBindings.Add("Text", this.DataSource, fieldName);
                    if (fieldName.Contains("Desc"))
                    {
                        tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                        tableDetailCell.Padding = new DevExpress.XtraPrinting.PaddingInfo(10,2,0,0);
                    };
                    if (fieldName.Contains("ID"))
                    {
                        tableDetailCell.BeforePrint += new System.Drawing.Printing.PrintEventHandler(tableDetailCell_BeforePrint);
                    };
                }
                else
                {
                    tableDetailCell.DataBindings.Add("Text", this.DataSource, fieldName, "{0:#,0.00}");
                };
                tableDetailRow.Controls.Add(tableDetailCell);
                #endregion

                #region Report table footer
                if (fieldName.Contains("ML") || fieldName.Contains("ME"))
                {
                    if (totalsInd == 0)
                    {
                        totalFooterCell_Name = new XRTableCell();
                        totalFooterCell_Name.WidthF = 2 * columnWidth + 100;
                        totalFooterCell_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 20, 0, 0);
                        totalFooterCell_Name.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_Totals") + ": ";
                        totalFooterRow.Controls.Add(totalFooterCell_Name);

                        totalsInd = 1;
                    };
                    
                    totalFooterCell_Value = new XRTableCell();
                    totalFooterCell_Value.Name = (fieldName.Contains("ML")) ? fieldName.Replace("ML", "") + "_total" : fieldName.Replace("ME", "") + "_total";
                    totalFooterCell_Value.WidthF = tableHeaderCell.WidthF;
                    totalFooterCell_Value.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                    totalFooterCell_Value.BorderWidth = 2;
                    if (fieldName.Contains("Dif"))
                    { 
                        totalFooterCell_Value.BeforePrint +=new System.Drawing.Printing.PrintEventHandler(totalFooterCell_Value_BeforePrint);
                    }
                    else
                    {
                        calcField = new CalculatedField();
                        this.CalculatedFields.Add(calcField);
                        calcField.DataSource = reportData;
                        calcField.Expression = "Iif([MaxLengthInd] == 1 ,[" + fieldName + "],0)";
                        calcField.FieldType = FieldType.Double;
                        calcField.Name = fieldName + "_calc";

                        totalFooterCell_Value.Summary.Func = SummaryFunc.Sum;
                        totalFooterCell_Value.Summary.Running = SummaryRunning.Report;
                        totalFooterCell_Value.Summary.FormatString = "{0:#,0.00}";
                        totalFooterCell_Value.DataBindings.Add("Text", this.DataSource, fieldName + "_calc");
                    }
                    totalFooterRow.Controls.Add(totalFooterCell_Value);
                };
                #endregion
            };

            tableHeader.Controls.Add(tableHeaderRow);

            tableDetailCell = new XRTableCell() { WidthF = 0, Visible = false, Name = "tableDetailCell_MaxLengthInd" };
            tableDetailCell.DataBindings.Add("Text", this.DataSource, "MaxLengthInd");
            tableDetailRow.Controls.Add(tableDetailCell);
            tableDetail.Controls.Add(tableDetailRow);

            totalFooter.Controls.Add(totalFooterRow);            

            reportTableHeaderBand.Controls.Add(tableHeader);
            reportTableDetailBand.Controls.Add(tableDetail);
            reportTotalFooterBand.Controls.Add(totalFooter);

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
        
        /// <summary>
        /// Calculates totales
        /// </summary>
        private void totalFooterCell_Value_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) 
        {
            XRTableCell differenceCell = (XRTableCell)sender;
            XRTableCell previosCell;
            XRTableCell currentCell;

            decimal differenceCell_Value;
            decimal previosCell_Value;
            decimal currentCell_Value;

            if (differenceCell.Name.Contains("Mov"))
            {
                previosCell = FindControl("Movimiento_prev_total", true) as XRTableCell;
                previosCell_Value = Convert.ToDecimal(previosCell.Summary.GetResult());
                currentCell = FindControl("Movimiento_curr_total", true) as XRTableCell;
                currentCell_Value = Convert.ToDecimal(currentCell.Summary.GetResult());
                if (previosCell_Value == 0)
                    differenceCell.Text = "*";
                else
                {
                    differenceCell_Value = (currentCell_Value - previosCell_Value) / previosCell_Value * 100;
                    differenceCell.Text = differenceCell_Value.ToString("0.00");
                };
            };

            if (differenceCell.Name.Contains("Final"))
            {
                previosCell = FindControl("Final_prev_total", true) as XRTableCell;
                previosCell_Value = Convert.ToDecimal(previosCell.Summary.GetResult());
                currentCell = FindControl("Final_curr_total", true) as XRTableCell;
                currentCell_Value = Convert.ToDecimal(currentCell.Summary.GetResult());
                if (previosCell_Value == 0)
                    differenceCell.Text = "*";
                else
                {
                    differenceCell_Value = (currentCell_Value - previosCell_Value) / previosCell_Value * 100;
                    differenceCell.Text = differenceCell_Value.ToString("0.00");
                };
            };
        }

        #endregion
    }
}
