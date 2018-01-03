using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using System.Collections.Generic;
using NewAge.DTO.Reportes;
using DevExpress.XtraPrinting;
using NewAge.Cliente.GUI.WinApp.Clases;

namespace NewAge.Cliente.GUI.WinApp.Reports
{
    public partial class SeguridadReport : BaseReport//XtraReport//
    {

        BaseController _bc = BaseController.GetInstance();

        public enum reportPart                  
        {
            Master_Report = 0,
            Process_Report = 1,
            Bitacora_Report = 2,
            Report_Report = 3,
            Query_Report = 4,
            Document_Report = 5,
            DocumentAprob_Report = 6
        }

        /// <summary>
        /// Seguridad Report Constructor
        /// </summary>
        /// <param name="reportDataList">data for the report</param>
        /// <param name="reportFielsList">list of fields for report detail table</param>
        /// <param name="documentId">ID of the current document allowing to get information about it</param>
        public SeguridadReport(List<DTO_ReportSeguridad> reportDataList, List<ArrayList> reportFielsList,int documentId)
        {
            InitializeComponent();

            DataSource = reportDataList;

            #region Report styles
            XRControlStyles tableStyles = new XRControlStyles(this)
            {
                EvenStyle = new XRControlStyle()
                {
                    Name = "tableDetailEvenStyle",
                    BackColor = Color.White,
                    ForeColor = Color.Black,
                    Font = new Font("Times New Roman", 9),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                    Borders = DevExpress.XtraPrinting.BorderSide.Bottom,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0)
                },
                OddStyle = new XRControlStyle()
                {
                    Name = "tableDetailOddStyle",
                    BackColor = Color.LightGray,
                    ForeColor = Color.Black,
                    Font = new Font("Times New Roman", 9),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                    Borders = DevExpress.XtraPrinting.BorderSide.Bottom,
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
                    BorderWidth = 2
                }
            };

            this.StyleSheet.Add(tableStyles.EvenStyle);
            this.StyleSheet.Add(tableStyles.OddStyle);
            this.StyleSheet.Add(tableStyles.Style);
            #endregion

            #region Report bands collection
            DetailReportBandCollection reportBandCollection = new DetailReportBandCollection(this);
            DetailReportBand reportTableBand;
            GroupHeaderBand reportPartNameBand;
            GroupHeaderBand reportTableHeaderBand;
            DetailBand reportTableDetailBand;
            GroupFooterBand reportTableFooterBand;
#endregion

            #region Reprot elements collection
            XRLabel reportPartName;
            
            XRTable tableHeader;
            XRTableRow tableHeaderRow;
            XRTableCell tableHeaderCell;


            XRTable tableDetail;
            XRTableRow tableDetailRow;
            XRTableCell tableDetailCell;

            float tableWidth = 0;
            tableWidth = this.PageWidth - (this.Margins.Right + this.Margins.Left);

            XRLine tableFooterLine;
            #endregion

            foreach (ArrayList fieldList in reportFielsList)
            {
                #region Report bands               
                reportTableBand = new DetailReportBand();
                reportTableBand.DataSource = this.DataSource;
                reportTableBand.DataMember = ((reportPart)reportFielsList.IndexOf(fieldList)).ToString();

                reportPartNameBand = new GroupHeaderBand();
                reportPartNameBand.Level = 1;
                reportPartNameBand.HeightF = 30;
                reportTableBand.Bands.Add(reportPartNameBand);

                reportTableHeaderBand = new GroupHeaderBand();
                reportTableHeaderBand.Level = 0;
                reportTableHeaderBand.HeightF = 20;  
                reportTableHeaderBand.RepeatEveryPage = true;
                reportTableBand.Bands.Add(reportTableHeaderBand);

                reportTableDetailBand = new DetailBand();
                reportTableDetailBand.HeightF = 15;
                reportTableBand.Bands.Add(reportTableDetailBand);

                reportTableFooterBand = new GroupFooterBand();
                reportTableFooterBand.Level = 0;
                reportTableFooterBand.HeightF = 30;
                reportTableBand.Bands.Add(reportTableFooterBand);

                reportBandCollection.Add(reportTableBand);
                this.Bands.Add(reportBandCollection[reportFielsList.IndexOf(fieldList)]);
                #endregion
                
                #region Report elements
                reportPartName = new XRLabel();
                string fieldListName = ((reportPart)reportFielsList.IndexOf(fieldList)).ToString();
                string resourceListID = documentId.ToString() + "_tp" + fieldListName.Remove(fieldListName.Length - 7, 7);
                string partName = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, resourceListID);
                reportPartName.Text = partName;
                reportPartName.WidthF = tableWidth;
                reportPartName.HeightF = reportPartNameBand.HeightF;
                reportPartName.Font = new Font("Times New Roman", 12, FontStyle.Bold | FontStyle.Italic | FontStyle.Underline);
                reportPartName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                reportPartNameBand.Controls.Add(reportPartName);

                tableHeader = new XRTable();
                tableHeader.WidthF = tableWidth;
                tableHeader.HeightF = reportTableHeaderBand.HeightF;
                tableHeader.StyleName = "tableHeaderStyle";
                tableHeaderRow = new XRTableRow();

                tableDetail = new XRTable();
                tableDetail.WidthF = tableWidth;
                tableDetail.HeightF = reportTableDetailBand.HeightF;
                tableDetail.EvenStyleName = "tableDetailEvenStyle";
                tableDetail.OddStyleName = "tableDetailOddStyle";
                tableDetailRow = new XRTableRow();

                tableFooterLine = new XRLine()
                {
                    SizeF = new System.Drawing.SizeF(tableWidth, 3),
                    LineWidth = 2,
                    LocationF = new PointF(0, 10)
                };
                reportTableFooterBand.Controls.Add(tableFooterLine);
                #endregion
                
                #region Report table column width
                float columnWidth = 0;
                if (fieldList.Count > 12)
                {
                    columnWidth = (tableWidth - 70) / fieldList.Count;
                }
                else
                {
                    columnWidth = (tableWidth - 100) / fieldList.Count;
                };
                #endregion

                #region Report table
                foreach (string fieldName in fieldList)
                {
                    #region Report table header
                    tableHeaderCell = new XRTableCell();
                    if (fieldName.Contains("Docum"))
                    {
                        switch (fieldName)
                        {
                            case "DocumentoDesc":
                                tableHeaderCell.WidthF = columnWidth + (tableWidth - columnWidth * fieldList.Count) - 10;
                                break;
                            case "DocumentoID":
                                tableHeaderCell.WidthF = columnWidth + 10;
                                break;
                        }   
                    }
                    else
                    {
                        tableHeaderCell.WidthF = columnWidth;
                    };
                    string resourceFieldID = documentId.ToString() + "_" + fieldName;
                    if ((fieldList.Count > 10) && (fieldName.Contains("DocumentoID") || fieldName.Contains("Generate") || fieldName.Contains("Import") || fieldName.Contains("Export") || fieldName.Contains("Cancel") || fieldName.Contains("Sendto")))
                    {
                        resourceFieldID += "-Sh";
                    };
                    string tableColumnName = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, resourceFieldID);
                    tableHeaderCell.Text = tableColumnName;
                    tableHeaderRow.Controls.Add(tableHeaderCell);
                    #endregion

                    #region Report table delail
                    tableDetailCell = new XRTableCell();
                    tableDetailCell.WidthF = tableHeaderCell.WidthF;
                    tableDetailCell.DataBindings.Add("Text", this.DataSource, fieldListName + "." + fieldName);
                    tableDetailRow.Controls.Add(tableDetailCell);
                    #endregion               
                };

                tableHeader.Controls.Add(tableHeaderRow);
                tableDetail.Controls.Add(tableDetailRow);

                reportTableHeaderBand.Controls.Add(tableHeader);
                reportTableDetailBand.Controls.Add(tableDetail);
                #endregion
            };
        }  
    }
}
            