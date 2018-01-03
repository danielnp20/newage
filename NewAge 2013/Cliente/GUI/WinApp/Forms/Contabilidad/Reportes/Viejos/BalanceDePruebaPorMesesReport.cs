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
    class BalanceDePruebaPorMesesReport : BaseReport
    {
        #region Variables
        BaseController _bc = BaseController.GetInstance();
        #endregion

        #region Funciones Publicas
        /// <summary>
        ///  Balance De Prueba por meses Report Constructor
        /// </summary>
        /// <param name="reportData">data for the report</param>
        /// <param name="fieldList">list of fields for report detail table</param>
        /// <param name="MM"> Moneda type of the report - local, foreign, both</param>
        /// <param name="balanceTipo">tipo de balance</param>
        /// <param name="Date">report period</param>
        /// <param name="selectedFiltersList">list of filters assigned by the user</param>
        /// <param name="cuentaFuncInd">indicador del tipo de la cuenta (true - cuenta Funcional;false - cuenta Alterna)</param>
        public BalanceDePruebaPorMesesReport(List<DTO_ReportBalanceDePruebaPorMeses> reportData, ArrayList fieldList, TipoMoneda MM, string balanceTipo, DateTime Date, List<string> selectedFiltersList)
        {
            this.lblReportName.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.coBalanceDePruebaPorMeses).ToString()) + " (.000)";

            #region Report styles
            this.Landscape = true;
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
            //reportTableHeaderBand.HeightF = (MM == TipoMoneda.Both)? 80 : 55;
            reportTableHeaderBand.HeightF = 55;
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
            if (fieldList.Contains("CuentaDesc"))
                columnWidth = (tableWidth - 50) / fieldList.Count;
            else
                columnWidth = tableWidth / fieldList.Count;
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
            XRTableCell periodTableCell_Moneda;
            XRTableCell periodTableCell_BalanceTipo;

            periodTable = new XRTable();
            periodTable.LocationF = periodFrame.LocationF;
            periodTable.SizeF = periodFrame.SizeF;
            periodTable.Font = new Font("Times New Roman", 11, FontStyle.Bold);
            periodTable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            periodTableRow = new XRTableRow();

            float periodTableCellWidth = (tableWidth) / 3;

            periodTableCell_Year = new XRTableCell();
            periodTableCell_Year.WidthF = periodTableCellWidth;
            periodTableCell_Year.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_lblYear") + ":   " + Date.ToString("yyyy");
            periodTableRow.Cells.Add(periodTableCell_Year);

            periodTableCell_Moneda = new XRTableCell();
            periodTableCell_Moneda.WidthF = periodTableCellWidth;
            //periodTableCell_Moneda.Text = "Moneda:   " + MM.ToString();
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
            periodTableCell_BalanceTipo.WidthF = periodTableCellWidth;
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
            XRTable upperTableHeader;
            XRTableRow upperTableHeaderRow;
            XRTableCell upperTableHeaderCell;
            XRTableCell upperTableHeaderCell_RoundInd;
            upperTableHeader = new XRTable();
            upperTableHeader.HeightF = 25;
            upperTableHeader.WidthF = (MM == TipoMoneda.Both) ? tableWidth - 2 * columnWidth : tableWidth - columnWidth;
            upperTableHeader.StyleName = "tableHeaderStyle";
            upperTableHeaderRow = new XRTableRow();
            upperTableHeaderRow.Borders = DevExpress.XtraPrinting.BorderSide.All;
            upperTableHeaderRow.BorderColor = Color.White;
            upperTableHeaderRow.BorderWidth = 1;
            upperTableHeaderCell_RoundInd = new XRTableCell();

            if (fieldList.Contains("CuentaDesc"))
                upperTableHeaderCell_RoundInd.WidthF = 3 * columnWidth + 50;
            else
                upperTableHeaderCell_RoundInd.WidthF = 2 * columnWidth;
            if (MM == TipoMoneda.Both) upperTableHeaderCell_RoundInd.WidthF += columnWidth;
            upperTableHeaderCell_RoundInd.Text = string.Empty;//".000";
            upperTableHeaderCell_RoundInd.BackColor = Color.White;
            upperTableHeaderCell_RoundInd.ForeColor = Color.Black;
            upperTableHeaderCell_RoundInd.Font = new Font("Arial Narrow", 8, FontStyle.Underline);
            upperTableHeaderCell_RoundInd.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            upperTableHeaderCell_RoundInd.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 2, 0, 0);
            upperTableHeaderRow.Cells.Add(upperTableHeaderCell_RoundInd);

            for (int i = 1; i < 5; i++)
            {
                upperTableHeaderCell = new XRTableCell();
                upperTableHeaderCell.WidthF = (MM == TipoMoneda.Both) ? 6 * columnWidth : 3 * columnWidth;
                upperTableHeaderCell.Text = "Q" + i.ToString();
                upperTableHeaderRow.Cells.Add(upperTableHeaderCell);
            };
            upperTableHeader.Controls.Add(upperTableHeaderRow);
            reportTableHeaderBand.Controls.Add(upperTableHeader);

            //XRTable tableHeaderMdaInd;
            //XRTableRow tableHeaderMdaIndRow;
            //XRTableCell tableHeaderMdaIndCell_Inicial;
            //XRTableCell tableHeaderMdaIndCell_Quarter;
            //XRTableCell tableHeaderMdaIndCell_Final;
            //tableHeaderMdaInd = new XRTable();
            //tableHeaderMdaInd.WidthF = tableWidth - (2 * columnWidth + 50);
            //tableHeaderMdaInd.HeightF = 25;
            //tableHeaderMdaInd.LocationF = new PointF(2*columnWidth+50, upperTableHeader.HeightF);
            //tableHeaderMdaInd.StyleName = "tableHeaderStyle";
            //tableHeaderMdaIndRow = new XRTableRow();
            //tableHeaderMdaIndRow.Borders = DevExpress.XtraPrinting.BorderSide.All;
            //tableHeaderMdaIndRow.BorderColor = Color.White;
            //tableHeaderMdaIndRow.BorderWidth = 1;


            //for (int i = 1; i < 3; i++)
            //{
            //    tableHeaderMdaIndCell_Inicial = new XRTableCell();
            //    tableHeaderMdaIndCell_Inicial.WidthF = columnWidth;
            //    tableHeaderMdaIndCell_Inicial.Text = (i % 2 == 0) ? "ME" : "ML";
            //    tableHeaderMdaIndRow.Cells.Add(tableHeaderMdaIndCell_Inicial);
            //};           

            //for (int i = 1; i < 9; i++)
            //{
            //    tableHeaderMdaIndCell_Quarter = new XRTableCell();
            //    tableHeaderMdaIndCell_Quarter.WidthF = 3*columnWidth;
            //    tableHeaderMdaIndCell_Quarter.Text = (i % 2 == 0)? "ME" : "ML";
            //    tableHeaderMdaIndRow.Cells.Add(tableHeaderMdaIndCell_Quarter);
            //};

            //for (int i = 1; i < 3; i++)
            //{
            //    tableHeaderMdaIndCell_Final = new XRTableCell();
            //    tableHeaderMdaIndCell_Final.WidthF = columnWidth;
            //    tableHeaderMdaIndCell_Final.Text = (i % 2 == 0) ? "ME" : "ML";
            //    tableHeaderMdaIndRow.Cells.Add(tableHeaderMdaIndCell_Final);
            //};    

            //tableHeaderMdaInd.Rows.Add(tableHeaderMdaIndRow);

            //if (MM == TipoMoneda.Both) reportTableHeaderBand.Controls.Add(tableHeaderMdaInd);

            XRTable tableHeader;
            XRTableRow tableHeaderRow;
            XRTableCell tableHeaderCell;
            tableHeader = new XRTable();
            tableHeader.WidthF = tableWidth;
            tableHeader.HeightF = 25;
            //tableHeader.LocationF = (MM == TipoMoneda.Both) ? new PointF(0, upperTableHeader.HeightF + tableHeaderMdaInd.HeightF):new PointF(0, upperTableHeader.HeightF);
            tableHeader.LocationF = new PointF(0, upperTableHeader.HeightF);
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

            XRCrossBandBox totalFrameY;
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

            float totalFrameLocation = 0;

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
                if (fieldName.Contains("3") || fieldName.Contains("6") || fieldName.Contains("9"))
                {
                    tableHeaderCell.Borders = DevExpress.XtraPrinting.BorderSide.Right;
                    tableHeaderCell.BorderColor = Color.White;
                };
                string resourceFieldID = (AppReports.coBalanceDePruebaPorMeses).ToString() + "_" + fieldName;
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
                    if (fieldName.Contains("Year") || fieldName.Contains("Inicial"))
                    {
                        totalFrameY = new XRCrossBandBox();
                        totalFrameY.StartBand = reportTableHeaderBand;
                        totalFrameY.EndBand = reportGroupFooterBand;
                        totalFrameY.Borders = DevExpress.XtraPrinting.BorderSide.All;
                        totalFrameY.BorderWidth = 1;
                        totalFrameY.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
                        totalFrameY.StartPointF = new PointF(totalFrameLocation, tableHeader.HeightF);
                        totalFrameY.EndPointF = new PointF(totalFrameLocation, 1);
                        totalFrameY.WidthF = columnWidth - 1;
                        this.CrossBandControls.Add(totalFrameY);
                    };

                    tableDetailCell.DataBindings.Add("Text", this.DataSource, fieldName, "{0:#,0.}");
                }

                if (fieldName.Contains("3") || fieldName.Contains("6") || fieldName.Contains("9"))
                {
                    tableDetailCell.Borders = DevExpress.XtraPrinting.BorderSide.Right;
                    tableDetailCell.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
                };

                totalFrameLocation += tableDetailCell.WidthF;

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

            //XRTableCell cell = (XRTableCell)sender;
            //XRTableRow row = FindControl("tableDetailRow", true) as XRTableRow;

            //string empGrupo = _bc.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta);
            //Tuple<int, string> tup = new Tuple<int, string>(AppMasters.coPlanCuenta, empGrupo);
            //DTO_glTabla table = _bc.AdministrationModel.Tables[tup];

            //string cellLength = (cell.Text.Trim()).Length.ToString();


            if (!string.IsNullOrEmpty(cell_MaxLengthInd.Text.Trim()))
                if (Convert.ToInt16(cell_MaxLengthInd.Text) == 1)
                    row.Font = new Font("Times New Roman", 8, FontStyle.Regular);
                else
                    row.Font = new Font("Times New Roman", 8, FontStyle.Bold);
        }
        
        #endregion
    }
}