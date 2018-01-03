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
    class RelacionDocumentoReport : BaseReport //XtraReport//
    {
        #region Variables

        private int _rowInd;
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
        public RelacionDocumentoReport(List<DTO_ReportRelacionDocumentos> reportData, ArrayList fieldList, DateTime minDate, DateTime maxDate, string romp, string docId, List<string> selectedFiltersList)
        {
            this.lblReportName.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.coRelacionDocumentos).ToString());

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
            reportPeriodBand.HeightF = (selectedFiltersList.Count > 0) ? 85 : 55;
            reportPeriodBand.RepeatEveryPage = true;
            reportTableBand.Bands.Add(reportPeriodBand);

            GroupHeaderBand reportTableHeaderBand;
            reportTableHeaderBand = new GroupHeaderBand();
            reportTableHeaderBand.Level = 1;
            reportTableHeaderBand.HeightF = 30;
            reportTableHeaderBand.RepeatEveryPage = true;
            reportTableBand.Bands.Add(reportTableHeaderBand);

            GroupHeaderBand reportGroupHeaderBand;
            reportGroupHeaderBand = new GroupHeaderBand();
            reportGroupHeaderBand.Level = 0;
            reportGroupHeaderBand.HeightF = 40;

            GroupField reportGroupField = new GroupField("ReportRompimiento1.GroupFieldValue");
            //reportGroupField.SortOrder = XRColumnSortOrder.Ascending;

            DetailBand reportTableDetailBand;
            reportTableDetailBand = new DetailBand();
            reportTableDetailBand.HeightF = 20;
            reportTableBand.Bands.Add(reportTableDetailBand);
            
            GroupFooterBand reportGroupFooterBand;
            reportGroupFooterBand = new GroupFooterBand();
            reportGroupFooterBand.Level = 0;
            reportGroupFooterBand.HeightF = 15;

            GroupFooterBand reportTotalFooterBand;
            reportTotalFooterBand = new GroupFooterBand();
            reportTotalFooterBand.Level = 1;
            reportTotalFooterBand.HeightF = 40;
            reportTableBand.Bands.Add(reportTotalFooterBand);

            if (!romp.Contains("Consecutivo"))
            {
                reportTableBand.Bands.Add(reportGroupHeaderBand);
                reportGroupHeaderBand.GroupFields.Add(reportGroupField);
                reportTableBand.Bands.Add(reportGroupFooterBand);
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
            periodFrame.LocationF = new PointF(0, 0);
            periodFrame.SizeF = new SizeF(tableWidth, 40);
            periodFrame.LineWidth = 2;
            periodFrame.Shape = new ShapeRectangle()
            {
                Fillet = 50,
            };
            periodFrame.SendToBack();
            reportPeriodBand.Controls.Add(periodFrame);

            XRTable periodTable;
            XRTableRow periodTableRow;
            XRTableCell periodTableCell_DocId;
            XRTableCell periodTableCell_FromMonth;
            XRTableCell periodTableCell_TillMonth;
            periodTable = new XRTable();
            periodTable.LocationF = periodFrame.LocationF;
            periodTable.SizeF = periodFrame.SizeF;
            periodTable.Font = new Font("Times New Roman", 10, FontStyle.Bold);
            periodTable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            periodTableRow = new XRTableRow();

            float periodTableCellWidth = (tableWidth - 100) / 3;

            periodTableCell_DocId = new XRTableCell();
            periodTableCell_DocId.WidthF = periodTableCellWidth + 100;
            periodTableCell_DocId.Text = docId + " - " + ((NewAge.DTO.Negocio.DTO_MasterBasic)((DTO_glDocumento)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.glDocumento, new UDT_BasicID() { Value = docId }, true))).Descriptivo.ToString();
            periodTableRow.Cells.Add(periodTableCell_DocId);

            periodTableCell_FromMonth = new XRTableCell();
            periodTableCell_FromMonth.WidthF = periodTableCellWidth;
            periodTableCell_FromMonth.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_lblFrom") + ":   " + minDate.ToString("MMMM/yyyy");
            periodTableRow.Cells.Add(periodTableCell_FromMonth);

            periodTableCell_TillMonth = new XRTableCell();
            periodTableCell_TillMonth.WidthF = periodTableCellWidth;
            periodTableCell_TillMonth.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_lblUntil") + ":   " + maxDate.ToString("MMMM/yyyy");
            periodTableRow.Cells.Add(periodTableCell_TillMonth);

            //periodTableCell_Moneda = new XRTableCell();
            //periodTableCell_Moneda.WidthF = periodTableCellWidth + 20;
            //switch (MM)
            //{
            //    case TipoMoneda.Local:
            //        periodTableCell_Moneda.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_Moneda") + ":   " + _bc.GetResource(LanguageTypes.Tables, DictionaryTables.CurrencyLocal);
            //        break;
            //    case TipoMoneda.Foreign:
            //        periodTableCell_Moneda.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_Moneda") + ":   " + _bc.GetResource(LanguageTypes.Tables, DictionaryTables.CurrencyForeign);
            //        break;
            //    case TipoMoneda.Both:
            //        periodTableCell_Moneda.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_Moneda") + ":   " + _bc.GetResource(LanguageTypes.Tables, DictionaryTables.CurrencyBoth);
            //        break;
            //};
            //periodTableRow.Cells.Add(periodTableCell_Moneda);

            //periodTableCell_TipoBalance = new XRTableCell();
            //periodTableCell_TipoBalance.WidthF = periodTableCellWidth + 50;
            //periodTableCell_TipoBalance.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_TipoBalance") + ":   " + tipoBalance;
            //periodTableRow.Cells.Add(periodTableCell_TipoBalance);

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
            
            #region Report group footer
            XRLine groupFooterLowerLine = new XRLine()
            {
                LineWidth = 1,
                SizeF = new SizeF(tableWidth, 2),
                LocationF = new PointF(0, 2)
            };
            reportGroupFooterBand.Controls.Add(groupFooterLowerLine);
            #endregion

            #region Report total footer
            XRLine footerLowerLine_1 = new XRLine()
            {
                LineWidth = 2,
                SizeF = new SizeF(tableWidth, 2),
                LocationF = new PointF(0, 30)
            };
            reportTotalFooterBand.Controls.Add(footerLowerLine_1);

            XRLine footerLowerLine_2 = new XRLine()
            {
                LineWidth = 2,
                SizeF = new SizeF(tableWidth, 2),
                LocationF = new PointF(0, 34)
            };
            reportTotalFooterBand.Controls.Add(footerLowerLine_2);
            #endregion
            #endregion

            #region Report Table
            
            #region Calculated fields
            CalculatedField DocumentoNombre = new CalculatedField();
            DocumentoNombre.DataSource = reportData;
            DocumentoNombre.FieldType = FieldType.String;
            DocumentoNombre.DisplayName = "DocumentoNombre_" + docId;
            DocumentoNombre.Name = "DocumentoNombre_" + docId;
            DocumentoNombre.Expression = "Iif([DocSaldoControl] == " + 0 + ",Trim([DocumentoPrefijo]) + ' - ' + Trim([DocumentoNumero]),[DocumentoTercero])";
            this.CalculatedFields.Add(DocumentoNombre);
            #endregion

            foreach (string fieldName in fieldList)
            {
                #region Report table header
                tableHeaderCell = new XRTableCell();
                tableHeaderCell.WidthF = columnWidth;

                if (fieldName.Contains("Desc"))
                    tableHeaderCell.WidthF = columnWidth + 140;
                if (fieldName.Contains("TerceroID") || fieldName.Contains("Fecha") || fieldName.Contains("Moneda"))
                    tableHeaderCell.WidthF = columnWidth - 73;
                if (fieldName.Contains("Estado"))
                    tableHeaderCell.WidthF = columnWidth - 61;

                string resourceFieldID = (AppReports.coRelacionDocumentos).ToString() + "_" + fieldName;
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

            };

            tableHeader.Controls.Add(tableHeaderRow);
            tableDetail.Controls.Add(tableDetailRow);

            //tableDetailCell = new XRTableCell() { WidthF = 0, Visible = false, Name = "PorMeses" };
            //tableDetailCell.DataBindings.Add("Text", this.DataSource, "PorMeses");
            //tableDetailRow.Controls.Add(tableDetailCell);
            //tableDetail.Controls.Add(tableDetailRow);

            reportTableHeaderBand.Controls.Add(tableHeader);
            reportTableDetailBand.Controls.Add(tableDetail);

            #endregion

            #region Report group header
            groupHeaderCell_Name = new XRTableCell();
            groupHeaderCell_Name.Name = "groupHeaderCell_Name";
            groupHeaderCell_Name.WidthF = 100;               
            groupHeaderCell_Name.DataBindings.Add("Text", this.DataSource, "ReportRompimiento1.GroupFieldName");
            groupHeaderRow.Controls.Add(groupHeaderCell_Name);
            groupHeaderCell_Name.BeforePrint += new System.Drawing.Printing.PrintEventHandler(groupHeaderCell_Name_BeforePrint);

            groupHeaderCell_Value = new XRTableCell();
            groupHeaderCell_Value.WidthF = (romp.Contains("Tercero"))? 100 : 200;
            groupHeaderCell_Value.DataBindings.Add("Text", this.DataSource, "ReportRompimiento1.GroupFieldValue");
            if (romp.Contains("PorMeses")) groupHeaderCell_Value.BeforePrint += new System.Drawing.Printing.PrintEventHandler(groupHeaderCell_Value_BeforePrint);
            groupHeaderRow.Controls.Add(groupHeaderCell_Value);

            groupHeaderCell_Desc = new XRTableCell();
            groupHeaderCell_Desc.WidthF = (romp.Contains("Tercero")) ? tableWidth - 200 : tableWidth - 300;
            groupHeaderCell_Desc.DataBindings.Add("Text", this.DataSource, "ReportRompimiento1.GroupFieldDesc");
            groupHeaderRow.Controls.Add(groupHeaderCell_Desc);

            groupHeader.Controls.Add(groupHeaderRow);
            reportGroupHeaderBand.Controls.Add(groupHeader);
            #endregion
        } 
        #endregion

        #region Eventos
        /// <summary>
        /// Checks if the group header should be printed and puts proper field captions depending on group field name
        /// </summary>
        private void groupHeaderCell_Value_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell headerCell = (XRTableCell)sender;
            if (!string.IsNullOrEmpty(headerCell.Text) && !string.IsNullOrWhiteSpace(headerCell.Text))
                headerCell.Text = (DateTime.Parse(headerCell.Text)).ToString("MMMM / yyyy");
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
                headerCell.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.coRelacionDocumentos).ToString() + "_" + headerCell.Text);
        } 
        #endregion
    }
}
