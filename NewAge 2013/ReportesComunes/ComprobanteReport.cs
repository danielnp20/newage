using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.IO;
using NewAge.DTO.Negocio;
using NewAge.DTO.Reportes;
using System.Collections.Generic;
using SentenceTransformer;
using DevExpress.XtraPrinting.Shape;

namespace NewAge.CommonReports
{
    public partial class ComprobanteReport : BaseCommonReport//XtraReport//
    {
        /// <summary>
        /// Comprobante Report Constructor
        /// </summary>
        /// <param name="documentId">ID of the current document allowing to get information about it</param>
        /// <param name="reportList">data for the report</param>
        /// <param name="multiMonedaInd">MultiMoneda property of the document (true - MultiMoneda; false - not MultiMoneda) </param>
        /// <param name="fieldList">list of fields for report detail table</param>
        /// <param name="supplier"> Interface que provee de informacion a un reporte comun</param>
        /// <param name="selectedFiltersList">list of filters applied by user</param>
        public ComprobanteReport(int documentId, List<DTO_ReportComprobante> reportList, bool multiMonedaInd, ArrayList fieldList, CommonReportDataSupplier supplier, List<string> selectedFiltersList)
            : base(supplier)
        {
            this.lblReportName.Text = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, documentId.ToString());

            InitializeComponent();

            this.DataSource = reportList;            

            #region Report styles
            this.Landscape = true;

            XRControlStyle headerStyle = new XRControlStyle()
            {
                Name = "groupHeaderStyle",
                BackColor = Color.White,
                ForeColor = Color.Black,
                Font = new Font("Times New Roman", 9),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
            };
            this.StyleSheet.Add(headerStyle);

            XRControlStyle sumFieldStyle = new XRControlStyle()
            {
                Name = "groupFooterStyle",
                BackColor = Color.White,
                ForeColor = Color.Black,
                Font = new Font("Times New Roman", 9,FontStyle.Bold),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight,
                Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0)
            };
            this.StyleSheet.Add(sumFieldStyle);
            
            XRControlStyles tableStyles = new XRControlStyles(this)
            {
                EvenStyle = new XRControlStyle()
                {
                    Name = "tableDetailEvenStyle",
                    BackColor = Color.WhiteSmoke,
                    ForeColor = Color.Black,
                    Font = new Font("Times New Roman", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                    //Borders = DevExpress.XtraPrinting.BorderSide.Bottom,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0,2,0,0)
                },
                OddStyle = new XRControlStyle()
                {
                    Name = "tableDetailOddStyle",
                    BackColor = Color.White,
                    ForeColor = Color.Black,
                    Font = new Font("Times New Roman", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                    //Borders = DevExpress.XtraPrinting.BorderSide.Bottom,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0)
                },
                Style = new XRControlStyle()
                {
                    Name = "tableHeaderStyle",
                    BackColor = Color.DimGray,
                    ForeColor = Color.White,
                    Font = new Font("Times New Roman", 9, FontStyle.Bold),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                }
            };

            this.StyleSheet.Add(tableStyles.EvenStyle);
            this.StyleSheet.Add(tableStyles.OddStyle);
            this.StyleSheet.Add(tableStyles.Style);
            #endregion

            #region Report Bands
            this.PageHeader.HeightF = 55;          

            DetailReportBand reportTableBand = new DetailReportBand();
            reportTableBand.DataSource = this.DataSource;
            reportTableBand.DataMember = "footerReport";

            //GroupHeaderBand reportTableHeaderBand;
            //reportTableHeaderBand = new GroupHeaderBand();
            //reportTableHeaderBand.Level = 1;
            //reportTableHeaderBand.HeightF = 30;
            //reportTableHeaderBand.RepeatEveryPage = true;
            //reportTableBand_forHeader.Bands.Add(reportTableHeaderBand);

            GroupHeaderBand reportGroupHeader = new GroupHeaderBand();
            reportGroupHeader.Level = 0;
            reportGroupHeader.HeightF = (selectedFiltersList.Count > 0) ? 75 : 45;
            reportTableBand.Bands.Add(reportGroupHeader);

            GroupField groupField = new GroupField("Header.ComprobanteID");
            groupField.SortOrder = XRColumnSortOrder.Ascending;

            DetailBand reportDetail = new DetailBand();
            //reportDetail.SortFields.Add(new GroupField("Header.CouentaID"));
            reportDetail.HeightF = 20;
            reportTableBand.Bands.Add(reportDetail);
            this.Bands.Add(reportTableBand);

            GroupFooterBand reportGroupFooter = new GroupFooterBand();
            reportGroupFooter.HeightF = 60;
            reportGroupFooter.Level = 0;
            reportTableBand.Bands.Add(reportGroupFooter);
            #endregion

            #region Report field width
            float tableWidth = this.PageWidth - (this.Margins.Left + this.Margins.Right);
            float columnWidth = 0;

            if (fieldList.Contains("Descriptivo"))
                columnWidth = (tableWidth - 70) / fieldList.Count;
            else
                columnWidth = tableWidth / fieldList.Count;
            #endregion

            #region Report elements
            #region Report header
            XRShape headerFrame = new XRShape()
            {
                SizeF = new System.Drawing.SizeF(tableWidth, 25),
                LocationF = new PointF(0, 1),
                BackColor = Color.Transparent
            };
            headerFrame.Shape = new ShapeRectangle()
            {
                Fillet = 50
            };
            reportGroupHeader.Controls.Add(headerFrame);

            XRTable groupHeader;
            XRTableRow groupHeaderRow;
            XRTableCell groupHeaderCell_ComprobanteName;
            XRTableCell groupHeaderCell_ComprobanteValue;
            XRTableCell groupHeaderCell_ComprobanteNroName = new XRTableCell();
            XRTableCell groupHeaderCell_ComprobanteNroValue = new XRTableCell();
            XRTableCell groupHeaderCell_FechaName = new XRTableCell();
            XRTableCell groupHeaderCell_FechaValue = new XRTableCell();

            float groupHeaderCellWidth = tableWidth / 6;

            groupHeader = new XRTable();
            groupHeader.LocationF = headerFrame.LocationF;
            groupHeader.SizeF = headerFrame.SizeF;
            groupHeader.StyleName = "groupHeaderStyle";
            groupHeaderRow = new XRTableRow();

            groupHeaderCell_ComprobanteName = new XRTableCell();
            groupHeaderCell_ComprobanteName.WidthF = groupHeaderCellWidth;
            groupHeaderCell_ComprobanteName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            groupHeaderCell_ComprobanteName.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 10, 0, 0);
            groupHeaderCell_ComprobanteName.Text = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, documentId.ToString() + "_ComprobanteID") + ":  ";
            groupHeaderRow.Cells.Add(groupHeaderCell_ComprobanteName);

            groupHeaderCell_ComprobanteValue = new XRTableCell();
            groupHeaderCell_ComprobanteValue.WidthF = groupHeaderCellWidth;
            groupHeaderCell_ComprobanteValue.DataBindings.Add("Text", this.DataSource, "Header.ComprobanteID");
            groupHeaderRow.Cells.Add(groupHeaderCell_ComprobanteValue);

            groupHeaderCell_ComprobanteNroName = new XRTableCell();
            groupHeaderCell_ComprobanteNroName.WidthF = groupHeaderCellWidth;
            groupHeaderCell_ComprobanteNroName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            groupHeaderCell_ComprobanteNroName.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 10, 0, 0);
            groupHeaderCell_ComprobanteNroName.Text = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, documentId.ToString() + "_Nro") + ":  ";
            groupHeaderRow.Cells.Add(groupHeaderCell_ComprobanteNroName);

            groupHeaderCell_ComprobanteNroValue = new XRTableCell();
            groupHeaderCell_ComprobanteNroValue.WidthF = groupHeaderCellWidth;
            groupHeaderCell_ComprobanteNroValue.DataBindings.Add("Text", this.DataSource, "Header.ComprobanteNro");
            groupHeaderRow.Cells.Add(groupHeaderCell_ComprobanteNroValue);

            groupHeaderCell_FechaName = new XRTableCell();
            groupHeaderCell_FechaName.WidthF = groupHeaderCellWidth;
            groupHeaderCell_FechaName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            groupHeaderCell_FechaName.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 10, 0, 0);
            groupHeaderCell_FechaName.Text = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, documentId.ToString() +"_Fecha") + ":  ";
            groupHeaderRow.Cells.Add(groupHeaderCell_FechaName);

            groupHeaderCell_FechaValue = new XRTableCell();
            groupHeaderCell_FechaValue.WidthF = groupHeaderCellWidth;
            groupHeaderCell_FechaValue.DataBindings.Add("Text", this.DataSource, "Header.Fecha");
            groupHeaderRow.Cells.Add(groupHeaderCell_FechaValue);

            groupHeader.Rows.Add(groupHeaderRow);
            reportGroupHeader.Controls.Add(groupHeader);

            XRLabel selectedFiltersLable = new XRLabel();
            selectedFiltersLable.LocationF = new PointF(0, headerFrame.LocationF.Y + headerFrame.HeightF + 10);
            selectedFiltersLable.SizeF = new SizeF(tableWidth, 20);
            selectedFiltersLable.Font = new Font("Times New Roman", 9, FontStyle.Italic);
            selectedFiltersLable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            selectedFiltersLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(20, 0, 0, 0);
            selectedFiltersLable.Text = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, documentId.ToString() + "_FiltradoPor") + ":   ";
            if (selectedFiltersList.Count > 0)
            {
                int filterCount = 0;
                foreach (string filter in selectedFiltersList)
                {
                    selectedFiltersLable.Text += filter;
                    filterCount++;
                    if (!(filterCount == (selectedFiltersList.Count))) selectedFiltersLable.Text += ",  ";
                };
                reportGroupHeader.Controls.Add(selectedFiltersLable);
            };

            XRLine groupHeaderLine = new XRLine()
            {
                SizeF = new System.Drawing.SizeF(tableWidth, 3),
                LineWidth = 2,
                LocationF = (selectedFiltersList.Count > 0) ? new PointF(0, selectedFiltersLable.LocationF.Y + selectedFiltersLable.HeightF + 12) : new PointF(0, headerFrame.LocationF.Y + headerFrame.HeightF + 12),
            };
            reportGroupHeader.Controls.Add(groupHeaderLine);
            #endregion

            #region Report detail
            XRTable tableHeader = new XRTable();
            tableHeader.WidthF = tableWidth;
            tableHeader.HeightF = 50;
            tableHeader.StyleName = "tableHeaderStyle";
            //if (MM == TipoMoneda.Both)
            //{
                XRTableRow tableHeaderUpperRow = new XRTableRow()
                {
                    HeightF = 25,
                    Borders = DevExpress.XtraPrinting.BorderSide.All,
                    BorderColor = Color.White
                };
                XRTableCell emptyCell = new XRTableCell()
                {
                    WidthF = tableWidth - 5 * columnWidth,
                    BackColor = Color.White,
                };
                XRTableCell MLCell = new XRTableCell()
                {
                    WidthF = 3 * columnWidth,
                    Text = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, documentId.ToString() + "_MdaLoc")
                };
                XRTableCell MECell = new XRTableCell()
                {
                    WidthF = 2 * columnWidth,
                    Text = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, documentId.ToString() + "_MdaExt")
                };
                tableHeaderUpperRow.Cells.AddRange(new XRTableCell[] { emptyCell, MLCell, MECell });
                tableHeader.Rows.Add(tableHeaderUpperRow);

                XRTableRow tableHeaderRow = new XRTableRow();
            tableHeaderRow.HeightF = 50;
            //};

            XRTable tableDetail = new XRTable();
            tableDetail.WidthF = tableWidth;
            tableDetail.HeightF = reportDetail.HeightF;
            XRTableRow tableDetailRow = new XRTableRow();
            tableDetailRow.WidthF = tableWidth;
            tableDetail.EvenStyleName = "tableDetailEvenStyle";
            tableDetail.OddStyleName = "tableDetailOddStyle";

            XRTableCell tableHeaderCell;
            XRTableCell tableDetailCell;
            #endregion
            
            #region Report footer
            XRTable tableFooter;
            XRTableRow tableFooterRow;
            XRTableCell tableFooterCell_Name;
            XRTableCell tableFooterCell_Value;
            tableFooter = new XRTable();
            tableFooter.LocationF = new PointF(0, 0);
            tableFooter.SizeF = new SizeF(tableWidth, 40);
            tableFooter.StyleName = "groupFooterStyle";
            tableFooterRow = new XRTableRow();

            CalculatedField calcField;

            XRLine reportFooterLine = new XRLine()
            {
                SizeF = new System.Drawing.SizeF(tableWidth, 2),
                LineWidth = 1,
                LocationF = new PointF(0, 1)
            };
            this.ReportFooter.Controls.Add(reportFooterLine);

            reportFooterLine = new XRLine()
            {
                SizeF = new System.Drawing.SizeF(tableWidth, 2),
                LineWidth = 1,
                LocationF = new PointF(0, 4)
            };
            this.ReportFooter.Controls.Add(reportFooterLine);
            #endregion
            #endregion

            float totalsFieldLocation = 0;
            int MLFieldInd = 0;
            int MEFieldInd = 0;
            int TotlasInd = 0;

            foreach (string fieldName in fieldList)
            {
                #region Report table header
                tableHeaderCell = new XRTableCell();
                tableHeaderCell.WidthF = (fieldName.Contains("Descriptivo")) ? columnWidth + 70 : columnWidth;
                string resourceId = documentId.ToString() + "_" + fieldName;
                string columnname = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, resourceId);
                tableHeaderCell.Text = columnname;

                tableHeaderRow.Controls.Add(tableHeaderCell);
                #endregion

                #region Report table detail
                tableDetailCell = new XRTableCell();
                tableDetailCell.WidthF = tableHeaderCell.WidthF;

                #region Table detail binding sourse
                if (fieldName.Contains("Debito") || fieldName.Contains("Credito"))
                {
                    calcField = new CalculatedField();
                    this.CalculatedFields.Add(calcField);

                    calcField.DataSource = this.DataSource;
                    calcField.DataMember = "footerReport";
                    calcField.FieldType = FieldType.Double;
                    calcField.DisplayName = fieldName + "_calculated";
                    calcField.Name = fieldName;
                    switch (fieldName)
                    {
                        case "DebitoML":
                            calcField.Expression = "Iif([Debito],[vlrMdaLoc.Value],0)";
                            tableHeaderCell.Text = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, documentId.ToString() + "_DebitoML_rep"); ////////////// Changing field header name
                            break;
                        case "CreditoML":
                            calcField.Expression = "Iif([Debito],0,[vlrMdaLoc.Value])";
                            tableHeaderCell.Text = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms,documentId.ToString() + "_CreditoML_rep"); ////////////// Changing field header name
                            break;
                        case "DebitoME":
                            calcField.Expression = "Iif([Debito],[vlrMdaExt.Value],0)";
                            tableHeaderCell.Text = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, documentId.ToString() + "_DebitoME_rep"); ////////////// Changing field header name
                            break;
                        case "CreditoME":
                            calcField.Expression = "Iif([Debito],0,[vlrMdaExt.Value])";
                            tableHeaderCell.Text = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms,documentId.ToString() + "_CreditoME_rep"); ////////////// Changing field header name
                            break;
                    };
                };
                if (fieldName.Contains("ME") || fieldName.Contains("ML"))
                {
                    if (fieldName.Contains("Base"))
                    {
                        tableDetailCell.DataBindings.Add("Text", this.DataSource, "footerReport." + fieldName + ".Value").FormatString = "{0:#,0.00}";
                        tableHeaderCell.Text = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms,documentId.ToString() + "_BaseML_rep"); ////////////// Changing field header name
                    }
                    else
                    {
                        tableDetailCell.DataBindings.Add("Text", this.DataSource, "footerReport." + fieldName).FormatString = "{0:#,0.00}";
                    }
                }
                else
                {
                    tableDetailCell.DataBindings.Add("Text", this.DataSource, "footerReport." + fieldName);
                };
                #endregion

                tableDetailRow.Controls.Add(tableDetailCell);
                #endregion

                #region Report table footer
                if (fieldName.Contains("ME") || fieldName.Contains("ML"))
                {
                    if (TotlasInd == 0)
                    {
                        tableFooterCell_Name = new XRTableCell()
                        {
                            Text = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, documentId.ToString() + "_Totals") + ": ",
                            WidthF = totalsFieldLocation,
                        };
                        tableFooterRow.Cells.Add(tableFooterCell_Name);
                        TotlasInd = 1;
                    };
                    tableFooterCell_Value = new XRTableCell();
                    tableFooterCell_Value.WidthF = tableHeaderCell.WidthF;
                    tableFooterCell_Value.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                    tableFooterCell_Value.BorderColor = Color.Black;
                    tableFooterCell_Value.Summary.Func = SummaryFunc.Sum;
                    tableFooterCell_Value.Summary.Running = SummaryRunning.Group;
                    tableFooterCell_Value.Summary.FormatString = "{0:#,0.00}";
                    if (fieldName.Contains("Base"))
                    {
                        tableFooterCell_Value.DataBindings.Add("Text", this.DataSource, "footerReport." + fieldName + ".Value");
                    }
                    else
                    {
                        tableFooterCell_Value.DataBindings.Add("Text", this.DataSource, "footerReport." + fieldName);
                    };
                    tableFooterRow.Cells.Add(tableFooterCell_Value);

                    #region Highlight ML and ME
                    tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;

                    if (fieldName.Contains("ML") && MLFieldInd == 0)
                    {
                        tableHeaderCell.Borders = DevExpress.XtraPrinting.BorderSide.Left;
                        tableHeaderCell.BorderColor = Color.White;
                        tableDetailCell.Borders = DevExpress.XtraPrinting.BorderSide.Left;
                        tableDetailCell.BorderColor = Color.Black;
                        tableFooterCell_Value.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top;
                        tableFooterCell_Value.BorderColor = Color.Black;
                        MLFieldInd = 1;
                    };
                    if (fieldName.Contains("ME") && MEFieldInd == 0)
                    {
                        tableHeaderCell.Borders = DevExpress.XtraPrinting.BorderSide.Left;
                        tableHeaderCell.BorderColor = Color.White;
                        tableDetailCell.Borders = DevExpress.XtraPrinting.BorderSide.Left;
                        tableDetailCell.BorderColor = Color.Black;
                        tableFooterCell_Value.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top;
                        tableFooterCell_Value.BorderColor = Color.Black;
                        MEFieldInd = 1;
                    };
                    #endregion
                };

                #endregion
                                
                if (TotlasInd == 0)
                {
                    totalsFieldLocation += tableHeaderCell.WidthF;
                };
            };

            tableHeader.Controls.Add(tableHeaderRow);
            tableDetail.Controls.Add(tableDetailRow);
            tableFooter.Controls.Add(tableFooterRow);

            this.PageHeader.Controls.Add(tableHeader);
            reportDetail.Controls.Add(tableDetail);
            reportGroupFooter.Controls.Add(tableFooter);           
        }             
    }
}



