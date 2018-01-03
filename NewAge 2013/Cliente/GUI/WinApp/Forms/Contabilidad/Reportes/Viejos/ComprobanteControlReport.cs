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
using NewAge.Cliente.GUI.WinApp.Reports;
using NewAge.Librerias.Project;

namespace NewAge.Cliente.GUI.WinApp.Reports
{
    class ComprobanteControlReport : BaseReport
    {
        #region Variables
        BaseController _bc = BaseController.GetInstance(); 
        #endregion

        #region Funciones Publicas
        /// <summary>
        /// Comprobante Control Report Constructor
        /// </summary>
        /// <param name="reportData">data for the report </param>
        /// <param name="fieldList"> list of fields for report detail table</param>
        /// <param name="Date">report period</param>
        public ComprobanteControlReport(List<DTO_ReportComprobanteControl> reportData, ArrayList fieldList, DateTime Date)
        {
            this.lblReportName.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.ComprobanteControl).ToString());

            this.DataSource = reportData;

            #region Report styles
            XRControlStyles tableStyles = new XRControlStyles(this)
            {
                EvenStyle = new XRControlStyle()
                {
                    Name = "tableDetailEvenStyle",
                    BackColor = Color.White,
                    ForeColor = Color.Black,
                    Font = new Font("Times New Roman", 9),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0)
                },
                OddStyle = new XRControlStyle()
                {
                    Name = "tableDetailOddStyle",
                    BackColor = Color.WhiteSmoke,
                    ForeColor = Color.Black,
                    Font = new Font("Times New Roman", 9),
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
                Font = new Font("Times New Roman", 9, FontStyle.Bold),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight,
                Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0)
            };
            this.StyleSheet.Add(groupFooterStyle);

            XRControlStyle totalFooterStyle = new XRControlStyle()
            {
                Name = "totalFooterStyle",
                BackColor = Color.White,
                ForeColor = Color.Black,
                Font = new Font("Times New Roman", 11, FontStyle.Bold),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight,
                Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0)
            };
            this.StyleSheet.Add(totalFooterStyle);
            #endregion

            #region Report bands
            DetailReportBand reportTableBand;
            reportTableBand = new DetailReportBand();
            reportTableBand.Name = "reportTableBand";
            reportTableBand.DataSource = this.DataSource;
            reportTableBand.DataMember = "CompControlResume";

            GroupHeaderBand reportGroupHeaderBand;
            reportGroupHeaderBand = new GroupHeaderBand();
            reportGroupHeaderBand.Level = 0;
            reportGroupHeaderBand.HeightF = 70;
            reportGroupHeaderBand.RepeatEveryPage = true;
            reportTableBand.Bands.Add(reportGroupHeaderBand);

            GroupField reportGroupField = new GroupField("ComprobanteID");
            reportGroupHeaderBand.GroupFields.Add(reportGroupField);                       

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
            float pageWidth = (this.PageWidth - (this.Margins.Right + this.Margins.Left));
            float tableWidth = (this.PageWidth - (this.Margins.Right + this.Margins.Left))/3;
            float columnWidth = (tableWidth-30) / fieldList.Count;
            #endregion

            #region Report elements
            #region Report period band 
            XRShape periodFrame = new XRShape();
            periodFrame.LocationF = new PointF(tableWidth, this.lblReportName.LocationF.Y + this.lblReportName.HeightF + 20);
            periodFrame.SizeF = new SizeF(tableWidth, 25);
            periodFrame.SendToBack();
            periodFrame.LineWidth = 2;
            periodFrame.Shape = new ShapeRectangle()
            {
                Fillet = 50,
            };
            this.ReportHeader.Controls.Add(periodFrame);

            XRTable periodTable;
            XRTableRow periodTableRow;
            XRTableCell periodTableCell_Year;
            XRTableCell periodTableCell_Month;
            periodTable = new XRTable();
            periodTable.LocationF = periodFrame.LocationF;
            periodTable.SizeF = periodFrame.SizeF;
            periodTable.Font = new Font("Times New Roman", 10, FontStyle.Bold);
            periodTable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            periodTableRow = new XRTableRow();

            float periodTableCellWidth = tableWidth / 2;

            periodTableCell_Year = new XRTableCell();
            periodTableCell_Year.WidthF = periodTableCellWidth;
            periodTableCell_Year.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_lblYear") + ":   " + Date.ToString("yyyy");
            periodTableRow.Cells.Add(periodTableCell_Year);

            periodTableCell_Month = new XRTableCell();
            periodTableCell_Month.WidthF = periodTableCellWidth;
            periodTableCell_Month.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_lblMonth") + ":   " + Date.ToString("MMMM");
            periodTableRow.Cells.Add(periodTableCell_Month);

            periodTable.Rows.Add(periodTableRow);

            this.ReportHeader.Controls.Add(periodTable);
            #endregion

            #region Report group header
            XRShape groupHeaderFrame = new XRShape();
            groupHeaderFrame.LocationF = new PointF(0, 30);
            groupHeaderFrame.SizeF = new SizeF(pageWidth, 25);
            groupHeaderFrame.BorderWidth = 1;
            groupHeaderFrame.SendToBack();
            groupHeaderFrame.Shape = new ShapeRectangle() { Fillet = 50 };
            reportGroupHeaderBand.Controls.Add(groupHeaderFrame);

            XRTable groupHeader;
            XRTableRow groupHeaderRow;
            XRTableCell groupHeaderCell_ID;
            XRTableCell groupHeaderCell_Desc;
            groupHeader = new XRTable();
            groupHeader.LocationF = groupHeaderFrame.LocationF;
            groupHeader.SizeF = groupHeaderFrame.SizeF;
            groupHeader.StyleName = "groupHeaderStyle";
            groupHeaderRow = new XRTableRow();
            #endregion

            #region Report Table detail
            XRTable tableDetail;
            XRTableRow tableDetailRow;
            XRTableCell tableDetailCell;
            tableDetail = new XRTable();
            tableDetail.Name = "tableDetail";
            tableDetail.WidthF = tableWidth;
            tableDetail.HeightF = reportTableDetailBand.HeightF;
            tableDetail.StyleName = "tableDetailEvenStyle";
            //tableDetail.EvenStyleName = "tableDetailEvenStyle";
            //tableDetail.OddStyleName = "tableDetailOddStyle";
            tableDetailRow = new XRTableRow();
            tableDetailRow.Name = "tableDetailRow";
            #endregion

            #region Report group footer
            XRTable groupFooter;
            XRTableRow groupFooterRow;
            XRTableCell groupFooterCell_Name;
            XRTableCell groupFooterCell_Value;
            groupFooter = new XRTable();
            groupFooter.LocationF = new PointF(tableWidth, 10);
            groupFooter.SizeF = new SizeF(tableWidth*2, 25);
            groupFooter.StyleName = "groupFooterStyle";
            groupFooterRow = new XRTableRow();           
            #endregion
            #endregion

            #region Report Table
            foreach (string fieldName in fieldList)
            {                
                #region Report table detail
                tableDetailCell = new XRTableCell();
                tableDetailCell.WidthF = columnWidth;
                tableDetailCell.DataBindings.Add("Text", this.DataSource, "CompControlResume." + fieldName);
                if (fieldName.Contains("From"))
                {
                    tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                    tableDetailRow.Controls.Add(tableDetailCell);
                    tableDetailCell = new XRTableCell();
                    tableDetailCell.WidthF = 30;
                    tableDetailCell.Text =  _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.ComprobanteControl).ToString() +"_Al");
                    tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    tableDetailCell.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0);
                };
                tableDetailRow.Controls.Add(tableDetailCell);
                #endregion
            };
            tableDetail.Controls.Add(tableDetailRow);
            reportTableDetailBand.Controls.Add(tableDetail);
            #endregion

            #region Report group header
            groupHeaderCell_ID = new XRTableCell();
            groupHeaderCell_ID.Name = "groupHeaderCell_Name";
            groupHeaderCell_ID.WidthF = columnWidth-2;
            groupHeaderCell_ID.DataBindings.Add("Text", this.DataSource, "ComprobanteID");

            groupHeaderCell_Desc = new XRTableCell();
            groupHeaderCell_Desc.WidthF = pageWidth - columnWidth/2;
            groupHeaderCell_Desc.DataBindings.Add("Text", this.DataSource, "ComprobanteDesc");

            groupHeaderRow.Controls.Add(groupHeaderCell_ID);
            groupHeaderRow.Controls.Add(groupHeaderCell_Desc);

            groupHeader.Controls.Add(groupHeaderRow);
            reportGroupHeaderBand.Controls.Add(groupHeader);
            #endregion

            #region Report group footer
            groupFooterRow.Borders = DevExpress.XtraPrinting.BorderSide.Top;

            groupFooterCell_Name = new XRTableCell();
            groupFooterCell_Name.Name = "groupFooterCell_Name";
            groupFooterCell_Name.WidthF = tableWidth / 2;
            groupFooterCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            groupFooterCell_Name.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.ComprobanteControl).ToString() + "_TotalComp");
            groupFooterRow.Controls.Add(groupFooterCell_Name);

            groupFooterCell_Value = new XRTableCell();
            groupFooterCell_Value.WidthF = tableWidth / 2;
            groupFooterCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            groupFooterCell_Value.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            groupFooterCell_Value.Summary.Func = SummaryFunc.Sum;
            groupFooterCell_Value.Summary.Running = SummaryRunning.Group;
            groupFooterCell_Value.DataBindings.Add("Text", this.DataSource, "CompControlResume.ComprobanteQty");
            groupFooterRow.Controls.Add(groupFooterCell_Value);

            groupFooterCell_Name = new XRTableCell();
            groupFooterCell_Name.Name = "groupFooterCell_Name";
            groupFooterCell_Name.WidthF = tableWidth / 2;
            groupFooterCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            groupFooterCell_Name.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.ComprobanteControl).ToString() + "_TotalReg");
            groupFooterRow.Controls.Add(groupFooterCell_Name);

            groupFooterCell_Value = new XRTableCell();
            groupFooterCell_Value.WidthF = tableWidth / 2;
            groupFooterCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            groupFooterCell_Value.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            groupFooterCell_Value.Summary.Func = SummaryFunc.Sum;
            groupFooterCell_Value.Summary.Running = SummaryRunning.Group;
            groupFooterCell_Value.DataBindings.Add("Text", this.DataSource, "CompControlResume.RecordQty");
            groupFooterRow.Controls.Add(groupFooterCell_Value);

            groupFooter.Controls.Add(groupFooterRow);
            reportGroupFooterBand.Controls.Add(groupFooter);
            #endregion
        } 
        #endregion
    }
}
