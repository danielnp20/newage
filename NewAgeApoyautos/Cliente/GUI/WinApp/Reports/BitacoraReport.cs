using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using NewAge.DTO.Negocio;
using DevExpress.XtraReports.UI;
using System.Drawing;
using System.Windows.Forms;

namespace NewAge.Cliente.GUI.WinApp.Reports
{
    public class BitacoraReport : BaseReport
    {
        /// <summary>
        /// Bitacora Report Constructor
        /// </summary>
        /// <param name="documentId">ID of the current document allowing to get information about it</param>
        /// <param name="reportData">data for the report</param>
        /// <param name="fieldList">list of fields for report detail table</param>
        /// <param name="subFieldList">list of fields for the nested tables</param>
        public BitacoraReport(int documentId, List<DTO_aplBitacora> reportData, ArrayList fieldList, ArrayList subFieldList)
        {
            #region Report styles
            this.Landscape = true;
            
            #region MainTable styles
            XRControlStyles tableStyles = new XRControlStyles(this)
            {
                EvenStyle = new XRControlStyle()
                {
                    Name = "tableDetailEvenStyle",
                    BackColor = Color.White,
                    ForeColor = Color.Black,
                    Font = new Font("Times New Roman", 9),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                    Borders = DevExpress.XtraPrinting.BorderSide.Top,
                    BorderColor = Color.DarkGray,
                    BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0)
                },
                OddStyle = new XRControlStyle()
                {
                    Name = "tableDetailOddStyle",
                    BackColor = Color.WhiteSmoke,
                    ForeColor = Color.Black,
                    Font = new Font("Times New Roman", 9),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                    Borders = DevExpress.XtraPrinting.BorderSide.Top,
                    BorderColor = Color.DarkGray,
                    BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot,
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
            #endregion

            #region SubTable style
            XRControlStyles subTableStyles = new XRControlStyles(this)
            {
                EvenStyle = new XRControlStyle()
                {
                    Name = "subTableDetailEvenStyle",
                    BackColor = Color.White,
                    ForeColor = Color.Black,
                    Font = new Font("Times New Roman", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                    Borders = DevExpress.XtraPrinting.BorderSide.All,
                    BorderColor = Color.Silver,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0)
                },
                OddStyle = new XRControlStyle()
                {
                    Name = "subTableDetailOddStyle",
                    BackColor = Color.Gainsboro,
                    ForeColor = Color.Black,
                    Font = new Font("Times New Roman", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                    Borders = DevExpress.XtraPrinting.BorderSide.All,
                    BorderColor = Color.Silver,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0)                    
                },
                Style = new XRControlStyle()
                {
                    Name = "subTableHeaderStyle",
                    BackColor = Color.DarkGray,
                    ForeColor = Color.White,
                    Font = new Font("Arial Narrow", 8, FontStyle.Bold),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                    Borders = DevExpress.XtraPrinting.BorderSide.All,
                    BorderColor = Color.DarkGray
                }
            };

            this.StyleSheet.Add(subTableStyles.EvenStyle);
            this.StyleSheet.Add(subTableStyles.OddStyle);
            this.StyleSheet.Add(subTableStyles.Style);
            #endregion
            #endregion
 
            #region Report bands
            #region Report MainBands
            DetailReportBand reportTableBand;            
            reportTableBand = new DetailReportBand();
            reportTableBand.Name = "reportTableBand";
            reportTableBand.DataSource = reportData;

            GroupHeaderBand reportTableHeaderBand;
            reportTableHeaderBand = new GroupHeaderBand();
            reportTableHeaderBand.Level = 0;
            reportTableHeaderBand.HeightF = 20;
            reportTableHeaderBand.RepeatEveryPage = true;
            reportTableBand.Bands.Add(reportTableHeaderBand);

            DetailBand reportTableDetailBand;
            reportTableDetailBand = new DetailBand();
            reportTableDetailBand.HeightF = 15;
            reportTableBand.Bands.Add(reportTableDetailBand);
            #endregion

            #region Report SubBands
            DetailReportBand reportSubTableBand;
            reportSubTableBand = new DetailReportBand();
            reportSubTableBand.Name = "reportSubTableBand";
            reportSubTableBand.DataSource = reportTableBand.DataSource;
            reportSubTableBand.DataMember = "Actualizaciones";
            reportSubTableBand.ReportPrintOptions.PrintOnEmptyDataSource = false;
            reportSubTableBand.BeforePrint +=new System.Drawing.Printing.PrintEventHandler(reportSubTableBand_BeforePrint);

            GroupHeaderBand reportSubTableHeaderBand;
            reportSubTableHeaderBand = new GroupHeaderBand();
            reportSubTableHeaderBand.Name = "reportSubTableHeaderBand";
            reportSubTableHeaderBand.Level = 0;
            reportSubTableHeaderBand.HeightF = 18;
            reportSubTableHeaderBand.RepeatEveryPage = true;
            reportSubTableBand.Bands.Add(reportSubTableHeaderBand);

            DetailBand reportSubTableDetailBand;
            reportSubTableDetailBand = new DetailBand();
            reportSubTableDetailBand.Name = "reportSubTableDetailBand";
            reportSubTableDetailBand.HeightF = 17;
            reportSubTableBand.Bands.Add(reportSubTableDetailBand);

            GroupFooterBand reportSubTableFooterBand;
            reportSubTableFooterBand = new GroupFooterBand();
            reportSubTableFooterBand.Name = "reportSubTableFooterBand";
            reportSubTableFooterBand.Level = 0;
            reportSubTableFooterBand.HeightF = 4;
            reportSubTableBand.Bands.Add(reportSubTableFooterBand);

            reportTableBand.Bands.Add(reportSubTableBand);
            #endregion 

            this.Bands.Add(reportTableBand);   
            #endregion

            #region Report field width
            #region Report MainField width
            float tableWidth = 0;
            float columnWidth = 0;

            tableWidth = this.PageWidth - (this.Margins.Right + this.Margins.Left);
            if (fieldList.Contains("DocumentoID"))
            {
                columnWidth = (tableWidth-20) / fieldList.Count;
            }
            else
            {
                columnWidth = tableWidth / fieldList.Count;
            };
            #endregion
            
            #region Report SubField width
            float subTableWidth = 0;
            float subColumnWidth = 0; 

            subTableWidth = this.PageWidth - (this.Margins.Right + this.Margins.Left) - 300;
            subColumnWidth = subTableWidth / subFieldList.Count;
            #endregion
            #endregion

            #region Report elements
            #region Report MainTable
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
            #endregion

            #region Report SubTable
            XRTable subTableHeader;
            XRTableRow subTableHeaderRow;
            XRTableCell subTableHeaderCell;
            subTableHeader = new XRTable();
            subTableHeader.Name = "subTableHeader";
            subTableHeader.LocationF = new PointF(250, 2);
            subTableHeader.WidthF = subTableWidth;
            subTableHeader.HeightF = reportSubTableHeaderBand.HeightF - 2;
            subTableHeader.AnchorVertical = VerticalAnchorStyles.Both;
            subTableHeader.StyleName = "subTableHeaderStyle";
            subTableHeaderRow = new XRTableRow();

            XRTable subTableDetail;
            XRTableRow subTableDetailRow;
            XRTableCell subTableDetailCell;
            subTableDetail = new XRTable();
            subTableDetail.LocationF = new PointF(subTableHeader.LocationF.X, 0);
            subTableDetail.WidthF = subTableWidth;
            subTableDetail.HeightF = reportSubTableDetailBand.HeightF;
            subTableDetail.AnchorVertical = VerticalAnchorStyles.Both;
            subTableDetail.EvenStyleName = "subTableDetailEvenStyle";
            subTableDetail.OddStyleName = "subTableDetailOddStyle";
            subTableDetailRow = new XRTableRow();

            XRPanel headerPanel = new XRPanel();
            headerPanel.Name = "headerPanel";
            headerPanel.SizeF = new SizeF(tableWidth, reportSubTableHeaderBand.HeightF);
            headerPanel.SendToBack();
            reportSubTableHeaderBand.Controls.Add(headerPanel);

            XRPanel detailPanel = new XRPanel();
            detailPanel.Name = "detailPanel";
            detailPanel.SizeF = new SizeF(tableWidth, reportSubTableDetailBand.HeightF);
            detailPanel.SendToBack();
            reportSubTableDetailBand.Controls.Add(detailPanel);

            XRPanel footerPanel = new XRPanel();
            footerPanel.Name = "footerPanel";
            footerPanel.SizeF = new SizeF(tableWidth, reportSubTableFooterBand.HeightF);
            footerPanel.SendToBack();
            reportSubTableFooterBand.Controls.Add(footerPanel);
            #endregion
            #endregion

            #region Report Table
            foreach (string fieldName in fieldList)
                {
                    #region Report table header
                    tableHeaderCell = new XRTableCell();
                    if (fieldName.Contains("Documento"))
                    {
                        tableHeaderCell.WidthF = columnWidth + 10;
                    }
                    else
                    {
                        tableHeaderCell.WidthF = columnWidth;
                    };                    
                    string resourceFieldID = documentId.ToString() + "_" + fieldName;                   
                    string tableColumnName = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, resourceFieldID);
                    tableHeaderCell.Text = tableColumnName;
                    tableHeaderRow.Controls.Add(tableHeaderCell);
                    #endregion

                    #region Report table delail
                    tableDetailCell = new XRTableCell();
                    tableDetailCell.WidthF = tableHeaderCell.WidthF;
                    tableDetailCell.DataBindings.Add("Text", this.DataSource, fieldName);
                    tableDetailRow.Controls.Add(tableDetailCell);
                    #endregion                                 
                };
            tableHeader.Controls.Add(tableHeaderRow);
            tableDetail.Controls.Add(tableDetailRow);

            reportTableHeaderBand.Controls.Add(tableHeader);
            reportTableDetailBand.Controls.Add(tableDetail);
            #endregion

            #region Report subTable
            foreach (string subFieldName in subFieldList)
            {
                #region Report subTable header
                subTableHeaderCell = new XRTableCell();
                subTableHeaderCell.WidthF = subColumnWidth;
                string resourceSubFieldID = documentId.ToString() + "_" + subFieldName;
                string subTableColumnName = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, resourceSubFieldID);
                subTableHeaderCell.Text = subTableColumnName;
                subTableHeaderRow.Controls.Add(subTableHeaderCell);
                #endregion

                #region Report subTable delail
                subTableDetailCell = new XRTableCell();
                subTableDetailCell.WidthF = subTableHeaderCell.WidthF;
                if (subFieldName.Contains("Valor"))
                {
                    subTableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                };
                subTableDetailCell.DataBindings.Add("Text", this.DataSource, "Actualizaciones." + subFieldName);
                subTableDetailRow.Controls.Add(subTableDetailCell);
                #endregion
            };
            subTableHeader.Controls.Add(subTableHeaderRow);
            subTableDetail.Controls.Add(subTableDetailRow);

            reportSubTableHeaderBand.Controls.Add(subTableHeader);
            reportSubTableDetailBand.Controls.Add(subTableDetail);
            #endregion

              
        }

        /// <summary>
        /// Rows color alteration
        /// </summary>
        private void reportSubTableBand_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DetailReportBand drb = FindControl("reportTableBand", true) as DetailReportBand;
            XRPanel headerPanel = FindControl("headerPanel", true) as XRPanel;
            XRPanel detailPanel = FindControl("detailPanel", true) as XRPanel;
            XRPanel footerPanel = FindControl("footerPanel", true) as XRPanel;
            headerPanel.SendToBack();
            detailPanel.SendToBack();
            footerPanel.SendToBack();

            if (drb.CurrentRowIndex != 0 && drb.CurrentRowIndex % 2 != 0)
            {
                headerPanel.BackColor = Color.WhiteSmoke;
                detailPanel.BackColor = Color.WhiteSmoke;
                footerPanel.BackColor = Color.WhiteSmoke;
            }
            else
            {
                headerPanel.BackColor = Color.White;
                detailPanel.BackColor = Color.White;
                footerPanel.BackColor = Color.White;
            };
        }
    }
}
