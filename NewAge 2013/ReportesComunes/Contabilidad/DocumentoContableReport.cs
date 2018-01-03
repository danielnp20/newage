using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.DTO.Reportes;
using System.Collections;
using DevExpress.XtraReports.UI;
using System.Drawing;
using DevExpress.XtraPrinting.Shape;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.ReportesComunes;

namespace NewAge.ReportesComunes
{

    public class DocumentoContableReport : BaseCommonReport
    {
        #region Variables
        private float _columnWidth; 
        #endregion

        #region Funciones Publicas
        
        /// <summary>
        /// Documento Contable Constructor
        /// </summary>
        /// <param name="documentId">ID of the current document allowing to get information about it</param>
        /// <param name="reportList">data for the report</param>
        /// <param name="multiMonedaInd">MultiMoneda property of the document (true - MultiMoneda; false - not MultiMoneda) </param>
        /// <param name="fieldList">list of fields for report detail table</param>
        public DocumentoContableReport(int documentId, List<DTO_ReportDocumentoContable> reportList, bool multiMonedaInd, ArrayList fieldList, CommonReportDataSupplier supplier)
            : base(supplier)
        {
            this.lblReportName.Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (documentId).ToString()); ;
            this.DataSource = reportList;

            #region Report styles
            this.Landscape = true;

            XRControlStyle headerStyle = new XRControlStyle()
            {
                Name = "groupHeaderStyle",
                BackColor = Color.Transparent,
                ForeColor = Color.Black,
                Font = new Font("Times New Roman", 9),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft,
                Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 3, 2, 2),
                Borders = DevExpress.XtraPrinting.BorderSide.Bottom,
                BorderColor = Color.WhiteSmoke
            };
            this.StyleSheet.Add(headerStyle);

            XRControlStyle sumFieldStyle = new XRControlStyle()
            {
                Name = "groupFooterStyle",
                BackColor = Color.Transparent,
                ForeColor = Color.Black,
                Font = new Font("Times New Roman", 9, FontStyle.Bold),
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
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0)
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
            this.PageHeader.HeightF = 0;

            DetailReportBand reportTableBand = new DetailReportBand();
            reportTableBand.DataSource = this.DataSource;
            reportTableBand.DataMember = "footerReport";

            GroupHeaderBand estadoHeaderBand;
            estadoHeaderBand = new GroupHeaderBand();
            estadoHeaderBand.Level = 4;
            estadoHeaderBand.HeightF = 60;
            estadoHeaderBand.RepeatEveryPage = false;
            reportTableBand.Bands.Add(estadoHeaderBand);

            GroupHeaderBand reportHeaderBand;
            reportHeaderBand = new GroupHeaderBand();
            reportHeaderBand.Level = 3;
            reportHeaderBand.HeightF = 120;
            reportHeaderBand.RepeatEveryPage = false;
            reportTableBand.Bands.Add(reportHeaderBand);

            GroupHeaderBand observacionGroupHeader = new GroupHeaderBand();
            observacionGroupHeader.Level = 2;
            observacionGroupHeader.HeightF = 40;//(documentId==AppReports.DocumentoContable)?40:20;
            //reportGroupHeader.HeightF = (selectedFiltersList.Count > 0) ? 75 : 45;
            reportTableBand.Bands.Add(observacionGroupHeader);

            GroupHeaderBand tableHeaderBand;
            tableHeaderBand = new GroupHeaderBand();
            tableHeaderBand.Level = 1;
            tableHeaderBand.HeightF = (multiMonedaInd) ? 53 : 28;
            tableHeaderBand.RepeatEveryPage = true;
            reportTableBand.Bands.Add(tableHeaderBand);

            GroupHeaderBand reportGroupHeader = new GroupHeaderBand();
            reportGroupHeader.Level = 0;
            reportGroupHeader.HeightF = 0;
            //reportGroupHeader.HeightF = (selectedFiltersList.Count > 0) ? 75 : 45;
            reportTableBand.Bands.Add(reportGroupHeader);

            //GroupField groupField = new GroupField("Header.ComprobanteID");
            //groupField.SortOrder = XRColumnSortOrder.Ascending;

            DetailBand reportDetail = new DetailBand();
            //reportDetail.SortFields.Add(new GroupField("Header.CouentaID"));
            reportDetail.HeightF = 20;
            reportTableBand.Bands.Add(reportDetail);
            this.Bands.Add(reportTableBand);

            GroupFooterBand reportGroupFooter = new GroupFooterBand();
            reportGroupFooter.HeightF = 80;
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

            _columnWidth = columnWidth;
            #endregion
            #region Report elements

            #region Report header
            XRShape estadoFrame = new XRShape()
            {
                SizeF = new System.Drawing.SizeF(tableWidth / 3 - 100, 50),
                LocationF = new PointF(2 * tableWidth / 3 + 50, 0),
                BackColor = Color.Transparent,
                AnchorVertical = VerticalAnchorStyles.Both,
                LineWidth = 4,
                ForeColor = Color.LightGray
            };
            estadoFrame.Shape = new ShapeRectangle() { Fillet = 30 };
            estadoHeaderBand.Controls.Add(estadoFrame);

            XRTable estadoTable;
            XRTableRow estadoTableRow;
            XRTableCell estadoTableCell_EstadoName;
            XRTableCell estadoTableCell_EstadoValue;

            estadoTable = new XRTable();
            estadoTable.LocationF = estadoFrame.LocationF;
            estadoTable.SizeF = estadoFrame.SizeF;
            estadoTable.Borders = DevExpress.XtraPrinting.BorderSide.None;
            estadoTableRow = new XRTableRow();

            estadoTableCell_EstadoName = new XRTableCell();
            estadoTableCell_EstadoName.WidthF = estadoTable.WidthF / 3;
            estadoTableCell_EstadoName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomRight;
            estadoTableCell_EstadoName.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 15, 0, 12);
            estadoTableCell_EstadoName.BackColor = Color.Transparent;
            estadoTableCell_EstadoName.ForeColor = Color.Black;
            estadoTableCell_EstadoName.Font = new Font("Times New Roman", 10, FontStyle.Italic);
            estadoTableCell_EstadoName.Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_Estado") + ":  ";
            estadoTableRow.Cells.Add(estadoTableCell_EstadoName);

            estadoTableCell_EstadoValue = new XRTableCell();
            estadoTableCell_EstadoValue.WidthF = 2 * estadoTable.WidthF / 3;
            estadoTableCell_EstadoValue.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            estadoTableCell_EstadoValue.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 10);
            estadoTableCell_EstadoValue.BackColor = Color.Transparent;
            estadoTableCell_EstadoValue.ForeColor = Color.DarkGray;
            estadoTableCell_EstadoValue.Font = new Font("Times New Roman", 16, FontStyle.Bold | FontStyle.Italic);
            estadoTableCell_EstadoValue.DataBindings.Add("Text", this.DataSource, "DocumentoEstado");
            estadoTableRow.Cells.Add(estadoTableCell_EstadoValue);

            estadoTable.Rows.Add(estadoTableRow);
            estadoHeaderBand.Controls.Add(estadoTable);

            XRShape headerFrame = new XRShape()
            {
                SizeF = new System.Drawing.SizeF(tableWidth, reportHeaderBand.HeightF),
                LocationF = new PointF(0, 1),
                BackColor = Color.Transparent,
                AnchorVertical = VerticalAnchorStyles.Both,
                LineWidth = 2,
            };
            headerFrame.Shape = new ShapeRectangle() { Fillet = 8 };
            reportHeaderBand.Controls.Add(headerFrame);

            float reportHeaderTableCellWidth_Caption = (tableWidth - 2) / 8;
            float reportHeaderTableCellWidth_ID = 2 * reportHeaderTableCellWidth_Caption / 3;
            float reportHeaderTableCellWidth_Desc = reportHeaderTableCellWidth_Caption + reportHeaderTableCellWidth_Caption / 2;


            XRTable reportHeaderTable;
            XRTableRow reportHeaderTableRow;
            XRTableCell groupHeaderTableCell_Name;
            XRTableCell groupHeaderTableCell_ID;
            XRTableCell groupHeaderTableCell_Desc;

            #region Table 1

            reportHeaderTable = new XRTable();
            reportHeaderTable.BeginInit();
            reportHeaderTable.LocationF = new PointF(headerFrame.LocationF.X + 2, headerFrame.LocationF.Y);
            reportHeaderTable.SizeF = new SizeF(reportHeaderTableCellWidth_Caption + reportHeaderTableCellWidth_ID + reportHeaderTableCellWidth_Desc, headerFrame.SizeF.Height);
            reportHeaderTable.StyleName = "groupHeaderStyle";

            reportHeaderTableRow = new XRTableRow();
            reportHeaderTableRow.HeightF = reportHeaderTable.HeightF / 5;

            groupHeaderTableCell_Name = new XRTableCell();
            groupHeaderTableCell_Name.WidthF = reportHeaderTableCellWidth_Caption;
            groupHeaderTableCell_Name.Font = new Font("Times New Roman", 9, FontStyle.Bold);
            groupHeaderTableCell_Name.BorderColor = Color.White;
            groupHeaderTableCell_Name.Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (documentId).ToString() + "_DocumentoCont");
            reportHeaderTableRow.Cells.Add(groupHeaderTableCell_Name);

            groupHeaderTableCell_ID = new XRTableCell();
            groupHeaderTableCell_ID.WidthF = reportHeaderTableCellWidth_ID;
            groupHeaderTableCell_ID.Borders = DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom;
            groupHeaderTableCell_ID.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            groupHeaderTableCell_ID.DataBindings.Add("Text", reportList, "coDocumentoID");
            reportHeaderTableRow.Cells.Add(groupHeaderTableCell_ID);

            groupHeaderTableCell_Desc = new XRTableCell();
            groupHeaderTableCell_Desc.WidthF = reportHeaderTableCellWidth_Desc;
            groupHeaderTableCell_Desc.DataBindings.Add("Text", this.DataSource, "DescDocumento");
            reportHeaderTableRow.Cells.Add(groupHeaderTableCell_Desc);

            reportHeaderTable.Rows.Add(reportHeaderTableRow);

            reportHeaderTableRow = new XRTableRow();
            reportHeaderTableRow.HeightF = reportHeaderTable.HeightF / 5;

            groupHeaderTableCell_Name = new XRTableCell();
            groupHeaderTableCell_Name.WidthF = reportHeaderTableCellWidth_Caption;
            groupHeaderTableCell_Name.BorderColor = Color.White;
            groupHeaderTableCell_Name.Font = new Font("Times New Roman", 9, FontStyle.Bold);
            groupHeaderTableCell_Name.Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (documentId).ToString() + "_MdaOrigen");
            reportHeaderTableRow.Cells.Add(groupHeaderTableCell_Name);

            groupHeaderTableCell_ID = new XRTableCell();
            groupHeaderTableCell_ID.WidthF = reportHeaderTableCellWidth_ID;
            groupHeaderTableCell_ID.Borders = DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom;
            groupHeaderTableCell_ID.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            groupHeaderTableCell_ID.DataBindings.Add("Text", this.DataSource, "DescMonedaOrigen");
            reportHeaderTableRow.Cells.Add(groupHeaderTableCell_ID);

            groupHeaderTableCell_Desc = new XRTableCell();
            groupHeaderTableCell_Desc.WidthF = reportHeaderTableCellWidth_Desc;
            groupHeaderTableCell_Desc.Text = "";
            reportHeaderTableRow.Cells.Add(groupHeaderTableCell_Desc);

            reportHeaderTable.Rows.Add(reportHeaderTableRow);

            reportHeaderTableRow = new XRTableRow();
            reportHeaderTableRow.HeightF = reportHeaderTable.HeightF / 5;
            groupHeaderTableCell_Name = new XRTableCell();
            groupHeaderTableCell_Name.WidthF = reportHeaderTableCellWidth_Caption;
            groupHeaderTableCell_Name.Font = new Font("Times New Roman", 9, FontStyle.Bold);
            groupHeaderTableCell_Name.BorderColor = Color.White;
            groupHeaderTableCell_Name.Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (documentId).ToString() + "_CuentaID");
            reportHeaderTableRow.Cells.Add(groupHeaderTableCell_Name);

            groupHeaderTableCell_ID = new XRTableCell();
            groupHeaderTableCell_ID.WidthF = reportHeaderTableCellWidth_ID;
            groupHeaderTableCell_ID.Borders = DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom;
            groupHeaderTableCell_ID.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            groupHeaderTableCell_ID.DataBindings.Add("Text", this.DataSource, "CuentaID");
            reportHeaderTableRow.Cells.Add(groupHeaderTableCell_ID);

            groupHeaderTableCell_Desc = new XRTableCell();
            groupHeaderTableCell_Desc.WidthF = reportHeaderTableCellWidth_Desc;
            groupHeaderTableCell_Desc.DataBindings.Add("Text", this.DataSource, "DescCuenta");
            reportHeaderTableRow.Cells.Add(groupHeaderTableCell_Desc);

            reportHeaderTable.Rows.Add(reportHeaderTableRow);

            reportHeaderTableRow = new XRTableRow();
            reportHeaderTableRow.HeightF = reportHeaderTable.HeightF / 5;

            groupHeaderTableCell_Name = new XRTableCell();
            groupHeaderTableCell_Name.WidthF = reportHeaderTableCellWidth_Caption;
            groupHeaderTableCell_Name.Font = new Font("Times New Roman", 9, FontStyle.Bold);
            groupHeaderTableCell_Name.BorderColor = Color.White;
            groupHeaderTableCell_Name.Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (documentId).ToString() + "_TerceroID");
            reportHeaderTableRow.Cells.Add(groupHeaderTableCell_Name);

            groupHeaderTableCell_ID = new XRTableCell();
            groupHeaderTableCell_ID.WidthF = reportHeaderTableCellWidth_ID;
            groupHeaderTableCell_ID.Borders = DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom;
            groupHeaderTableCell_ID.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            groupHeaderTableCell_ID.DataBindings.Add("Text", this.DataSource, "TerceroID");
            reportHeaderTableRow.Cells.Add(groupHeaderTableCell_ID);

            groupHeaderTableCell_Desc = new XRTableCell();
            groupHeaderTableCell_Desc.WidthF = reportHeaderTableCellWidth_Desc;
            groupHeaderTableCell_Desc.DataBindings.Add("Text", this.DataSource, "DescTercero");
            reportHeaderTableRow.Cells.Add(groupHeaderTableCell_Desc);

            reportHeaderTable.Rows.Add(reportHeaderTableRow);

            reportHeaderTableRow = new XRTableRow();
            reportHeaderTableRow.HeightF = reportHeaderTable.HeightF / 5;

            groupHeaderTableCell_Name = new XRTableCell();
            groupHeaderTableCell_Name.WidthF = reportHeaderTableCellWidth_Caption;
            groupHeaderTableCell_Name.Font = new Font("Times New Roman", 9, FontStyle.Bold);
            groupHeaderTableCell_Name.Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (documentId).ToString() + "_DocumentoCOM");
            reportHeaderTableRow.Cells.Add(groupHeaderTableCell_Name);

            groupHeaderTableCell_ID = new XRTableCell();
            groupHeaderTableCell_ID.WidthF = reportHeaderTableCellWidth_ID;
            groupHeaderTableCell_ID.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            groupHeaderTableCell_ID.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            groupHeaderTableCell_ID.DataBindings.Add("Text", this.DataSource, "DocumentoTercero");
            reportHeaderTableRow.Cells.Add(groupHeaderTableCell_ID);

            groupHeaderTableCell_Desc = new XRTableCell();
            groupHeaderTableCell_Desc.WidthF = reportHeaderTableCellWidth_Desc;
            groupHeaderTableCell_Desc.Borders = DevExpress.XtraPrinting.BorderSide.None;
            groupHeaderTableCell_Desc.Text = "";
            reportHeaderTableRow.Cells.Add(groupHeaderTableCell_Desc);

            reportHeaderTable.Rows.Add(reportHeaderTableRow);

            reportHeaderBand.Controls.Add(reportHeaderTable);

            XRShape table1_HeaderFrame = new XRShape()
            {
                SizeF = new System.Drawing.SizeF(reportHeaderTableCellWidth_Caption, reportHeaderTable.HeightF),
                LocationF = reportHeaderTable.LocationF,
                BackColor = Color.WhiteSmoke,
                AnchorVertical = VerticalAnchorStyles.Both,
                ForeColor = Color.Transparent
            };
            table1_HeaderFrame.SendToBack();
            reportHeaderTable.EndInit();
            reportHeaderBand.Controls.Add(table1_HeaderFrame);
            #endregion

            #region Table 2
            reportHeaderTable = new XRTable();
            reportHeaderTable.BeginInit();
            reportHeaderTable.LocationF = new PointF(headerFrame.LocationF.X + 2 + reportHeaderTableCellWidth_Caption + reportHeaderTableCellWidth_ID + reportHeaderTableCellWidth_Desc, headerFrame.LocationF.Y);
            reportHeaderTable.SizeF = new SizeF(reportHeaderTableCellWidth_Caption + reportHeaderTableCellWidth_ID + reportHeaderTableCellWidth_Desc, headerFrame.SizeF.Height);
            reportHeaderTable.StyleName = "groupHeaderStyle";

            reportHeaderTableRow = new XRTableRow();
            reportHeaderTableRow.HeightF = reportHeaderTable.HeightF / 5;

            groupHeaderTableCell_Name = new XRTableCell();
            groupHeaderTableCell_Name.WidthF = reportHeaderTableCellWidth_Caption;
            groupHeaderTableCell_Name.Font = new Font("Times New Roman", 9, FontStyle.Bold);
            groupHeaderTableCell_Name.BorderColor = Color.White;
            groupHeaderTableCell_Name.Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (documentId == AppReports.coDocumentoContable) ? (documentId).ToString() + "_Nro" : (documentId).ToString() + "_DocAjustado");
            reportHeaderTableRow.Cells.Add(groupHeaderTableCell_Name);

            groupHeaderTableCell_ID = new XRTableCell();
            groupHeaderTableCell_ID.WidthF = reportHeaderTableCellWidth_ID;
            groupHeaderTableCell_ID.Borders = DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom;
            groupHeaderTableCell_ID.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            groupHeaderTableCell_ID.DataBindings.Add("Text", reportList, (documentId == AppReports.coDocumentoContable) ? "DocumentoNro" : "DocAjustadoID");
            reportHeaderTableRow.Cells.Add(groupHeaderTableCell_ID);

            groupHeaderTableCell_Desc = new XRTableCell();
            groupHeaderTableCell_Desc.WidthF = reportHeaderTableCellWidth_Desc;
            if (documentId == AppReports.coDocumentoContable)
                groupHeaderTableCell_Desc.Text = "";
            else
                groupHeaderTableCell_Desc.DataBindings.Add("Text", reportList, "DocAjustadoDesc");
            reportHeaderTableRow.Cells.Add(groupHeaderTableCell_Desc);

            reportHeaderTable.Rows.Add(reportHeaderTableRow);

            reportHeaderTableRow = new XRTableRow();
            reportHeaderTableRow.HeightF = reportHeaderTable.HeightF / 5;

            groupHeaderTableCell_Name = new XRTableCell();
            groupHeaderTableCell_Name.WidthF = reportHeaderTableCellWidth_Caption;
            groupHeaderTableCell_Name.Font = new Font("Times New Roman", 9, FontStyle.Bold);
            groupHeaderTableCell_Name.BorderColor = Color.White;
            groupHeaderTableCell_Name.Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_Moneda");
            reportHeaderTableRow.Cells.Add(groupHeaderTableCell_Name);

            groupHeaderTableCell_ID = new XRTableCell();
            groupHeaderTableCell_ID.WidthF = reportHeaderTableCellWidth_ID;
            groupHeaderTableCell_ID.Borders = DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom;
            groupHeaderTableCell_ID.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            groupHeaderTableCell_ID.DataBindings.Add("Text", this.DataSource, "Header.MdaTransacc");
            reportHeaderTableRow.Cells.Add(groupHeaderTableCell_ID);

            groupHeaderTableCell_Desc = new XRTableCell();
            groupHeaderTableCell_Desc.WidthF = reportHeaderTableCellWidth_Desc;
            groupHeaderTableCell_Desc.DataBindings.Add("Text", reportList, "DescMonedaTransac");
            reportHeaderTableRow.Cells.Add(groupHeaderTableCell_Desc);

            reportHeaderTable.Rows.Add(reportHeaderTableRow);

            reportHeaderTableRow = new XRTableRow();
            reportHeaderTableRow.HeightF = reportHeaderTable.HeightF / 5;

            groupHeaderTableCell_Name = new XRTableCell();
            groupHeaderTableCell_Name.WidthF = reportHeaderTableCellWidth_Caption;
            groupHeaderTableCell_Name.Font = new Font("Times New Roman", 9, FontStyle.Bold);
            groupHeaderTableCell_Name.BorderColor = Color.White;
            groupHeaderTableCell_Name.Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (documentId).ToString() + "_ProyectoID");
            reportHeaderTableRow.Cells.Add(groupHeaderTableCell_Name);

            groupHeaderTableCell_ID = new XRTableCell();
            groupHeaderTableCell_ID.WidthF = reportHeaderTableCellWidth_ID;
            groupHeaderTableCell_ID.Borders = DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom;
            groupHeaderTableCell_ID.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            groupHeaderTableCell_ID.DataBindings.Add("Text", this.DataSource, "ProyectoID");
            reportHeaderTableRow.Cells.Add(groupHeaderTableCell_ID);

            groupHeaderTableCell_Desc = new XRTableCell();
            groupHeaderTableCell_Desc.WidthF = reportHeaderTableCellWidth_Desc;
            groupHeaderTableCell_Desc.DataBindings.Add("Text", this.DataSource, "DescProyecto");
            reportHeaderTableRow.Cells.Add(groupHeaderTableCell_Desc);

            reportHeaderTable.Rows.Add(reportHeaderTableRow);

            reportHeaderTableRow = new XRTableRow();
            reportHeaderTableRow.HeightF = reportHeaderTable.HeightF / 5;

            groupHeaderTableCell_Name = new XRTableCell();
            groupHeaderTableCell_Name.WidthF = reportHeaderTableCellWidth_Caption;
            groupHeaderTableCell_Name.Font = new Font("Times New Roman", 9, FontStyle.Bold);
            groupHeaderTableCell_Name.BorderColor = Color.White;
            groupHeaderTableCell_Name.Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (documentId).ToString() + "_CentroCostoID");
            reportHeaderTableRow.Cells.Add(groupHeaderTableCell_Name);

            groupHeaderTableCell_ID = new XRTableCell();
            groupHeaderTableCell_ID.WidthF = reportHeaderTableCellWidth_ID;
            groupHeaderTableCell_ID.Borders = DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom;
            groupHeaderTableCell_ID.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            groupHeaderTableCell_ID.DataBindings.Add("Text", this.DataSource, "CentroCostoID");
            reportHeaderTableRow.Cells.Add(groupHeaderTableCell_ID);

            groupHeaderTableCell_Desc = new XRTableCell();
            groupHeaderTableCell_Desc.WidthF = reportHeaderTableCellWidth_Desc;
            groupHeaderTableCell_Desc.DataBindings.Add("Text", this.DataSource, "DescCentroCosto");
            reportHeaderTableRow.Cells.Add(groupHeaderTableCell_Desc);

            reportHeaderTable.Rows.Add(reportHeaderTableRow);

            reportHeaderTableRow = new XRTableRow();
            reportHeaderTableRow.HeightF = reportHeaderTable.HeightF / 5;

            groupHeaderTableCell_Name = new XRTableCell();
            groupHeaderTableCell_Name.WidthF = reportHeaderTableCellWidth_Caption;
            groupHeaderTableCell_Name.Font = new Font("Times New Roman", 9, FontStyle.Bold);
            groupHeaderTableCell_Name.Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (documentId).ToString() + "_LugarGeoID");
            reportHeaderTableRow.Cells.Add(groupHeaderTableCell_Name);

            groupHeaderTableCell_ID = new XRTableCell();
            groupHeaderTableCell_ID.WidthF = reportHeaderTableCellWidth_ID;
            groupHeaderTableCell_ID.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            groupHeaderTableCell_ID.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            groupHeaderTableCell_ID.DataBindings.Add("Text", this.DataSource, "LugarGeograficoID");
            reportHeaderTableRow.Cells.Add(groupHeaderTableCell_ID);

            groupHeaderTableCell_Desc = new XRTableCell();
            groupHeaderTableCell_Desc.WidthF = reportHeaderTableCellWidth_Desc;
            groupHeaderTableCell_Desc.Borders = DevExpress.XtraPrinting.BorderSide.None;
            groupHeaderTableCell_Desc.DataBindings.Add("Text", this.DataSource, "DescLugarGeografico");
            reportHeaderTableRow.Cells.Add(groupHeaderTableCell_Desc);

            reportHeaderTable.Rows.Add(reportHeaderTableRow);

            reportHeaderBand.Controls.Add(reportHeaderTable);

            XRShape table2_HeaderFrame = new XRShape()
            {
                SizeF = new System.Drawing.SizeF(reportHeaderTableCellWidth_Caption, reportHeaderTable.HeightF),
                LocationF = reportHeaderTable.LocationF,
                BackColor = Color.WhiteSmoke,
                AnchorVertical = VerticalAnchorStyles.Both,
                ForeColor = Color.Transparent
            };
            table2_HeaderFrame.SendToBack();
            reportHeaderTable.EndInit();
            reportHeaderBand.Controls.Add(table2_HeaderFrame);
            #endregion

            #region Table 3
            reportHeaderTable = new XRTable();
            reportHeaderTable.BeginInit();
            reportHeaderTable.LocationF = new PointF(headerFrame.LocationF.X + 2 + 2 * (reportHeaderTableCellWidth_Caption + reportHeaderTableCellWidth_ID + reportHeaderTableCellWidth_Desc), headerFrame.LocationF.Y);
            reportHeaderTable.SizeF = new SizeF(reportHeaderTableCellWidth_Caption + reportHeaderTableCellWidth_ID, headerFrame.SizeF.Height);
            reportHeaderTable.StyleName = "groupHeaderStyle";

            reportHeaderTableRow = new XRTableRow();
            reportHeaderTableRow.HeightF = 2 * reportHeaderTable.HeightF / 6;

            if (documentId == AppReports.coAjusteSaldos)
            {
                reportHeaderTableRow.HeightF = reportHeaderTable.HeightF / 6;

                groupHeaderTableCell_Name = new XRTableCell();
                groupHeaderTableCell_Name.WidthF = reportHeaderTableCellWidth_Caption;
                groupHeaderTableCell_Name.Font = new Font("Times New Roman", 9, FontStyle.Bold);
                groupHeaderTableCell_Name.BorderColor = Color.White;
                groupHeaderTableCell_Name.Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (documentId).ToString() + "_Nro");
                reportHeaderTableRow.Cells.Add(groupHeaderTableCell_Name);

                groupHeaderTableCell_Desc = new XRTableCell();
                groupHeaderTableCell_Desc.WidthF = reportHeaderTableCellWidth_ID;
                groupHeaderTableCell_Desc.DataBindings.Add("Text", this.DataSource, "DocumentoNro");
                reportHeaderTableRow.Cells.Add(groupHeaderTableCell_Desc);

                reportHeaderTable.Rows.Add(reportHeaderTableRow);

                reportHeaderTableRow = new XRTableRow();
                reportHeaderTableRow.HeightF = reportHeaderTable.HeightF / 6; ;
            }

            groupHeaderTableCell_Name = new XRTableCell();
            groupHeaderTableCell_Name.WidthF = reportHeaderTableCellWidth_Caption;
            groupHeaderTableCell_Name.Font = new Font("Times New Roman", 9, FontStyle.Bold);
            groupHeaderTableCell_Name.BorderColor = Color.White;
            groupHeaderTableCell_Name.Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (documentId).ToString() + "_UsuarioResp");
            reportHeaderTableRow.Cells.Add(groupHeaderTableCell_Name);

            groupHeaderTableCell_Desc = new XRTableCell();
            groupHeaderTableCell_Desc.WidthF = reportHeaderTableCellWidth_ID;
            groupHeaderTableCell_Desc.DataBindings.Add("Text", this.DataSource, "UsuarioResp");
            reportHeaderTableRow.Cells.Add(groupHeaderTableCell_Desc);

            reportHeaderTable.Rows.Add(reportHeaderTableRow);

            reportHeaderTableRow = new XRTableRow();
            reportHeaderTableRow.HeightF = reportHeaderTable.HeightF / 6;

            groupHeaderTableCell_Name = new XRTableCell();
            groupHeaderTableCell_Name.WidthF = reportHeaderTableCellWidth_Caption;
            groupHeaderTableCell_Name.Font = new Font("Times New Roman", 9, FontStyle.Bold);
            groupHeaderTableCell_Name.BorderColor = Color.White;
            groupHeaderTableCell_Name.Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (documentId).ToString() + "_PeriodoID");
            reportHeaderTableRow.Cells.Add(groupHeaderTableCell_Name);

            groupHeaderTableCell_Desc = new XRTableCell();
            groupHeaderTableCell_Desc.WidthF = reportHeaderTableCellWidth_ID;
            groupHeaderTableCell_Desc.DataBindings.Add("Text", this.DataSource, "Header.PeriodoID");
            reportHeaderTableRow.Cells.Add(groupHeaderTableCell_Desc);

            reportHeaderTable.Rows.Add(reportHeaderTableRow);

            reportHeaderTableRow = new XRTableRow();
            reportHeaderTableRow.HeightF = reportHeaderTable.HeightF / 6;

            groupHeaderTableCell_Name = new XRTableCell();
            groupHeaderTableCell_Name.WidthF = reportHeaderTableCellWidth_Caption;
            groupHeaderTableCell_Name.Font = new Font("Times New Roman", 9, FontStyle.Bold);
            groupHeaderTableCell_Name.BorderColor = Color.White;
            groupHeaderTableCell_Name.Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (documentId).ToString() + "_Fecha");
            reportHeaderTableRow.Cells.Add(groupHeaderTableCell_Name);

            groupHeaderTableCell_Desc = new XRTableCell();
            groupHeaderTableCell_Desc.WidthF = reportHeaderTableCellWidth_ID;
            groupHeaderTableCell_Desc.DataBindings.Add("Text", this.DataSource, "Header.Fecha");
            reportHeaderTableRow.Cells.Add(groupHeaderTableCell_Desc);

            reportHeaderTable.Rows.Add(reportHeaderTableRow);

            reportHeaderTableRow = new XRTableRow();
            reportHeaderTableRow.HeightF = reportHeaderTable.HeightF / 6;

            groupHeaderTableCell_Name = new XRTableCell();
            groupHeaderTableCell_Name.WidthF = reportHeaderTableCellWidth_Caption;
            groupHeaderTableCell_Name.Font = new Font("Times New Roman", 9, FontStyle.Bold);
            groupHeaderTableCell_Name.BorderColor = Color.White;
            groupHeaderTableCell_Name.Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (documentId).ToString() + "_TasaDeCambio");
            reportHeaderTableRow.Cells.Add(groupHeaderTableCell_Name);

            groupHeaderTableCell_Desc = new XRTableCell();
            groupHeaderTableCell_Desc.WidthF = reportHeaderTableCellWidth_ID;
            groupHeaderTableCell_Desc.DataBindings.Add("Text", this.DataSource, "Header.TasaCambioBase", "{0:#,0.00}");
            reportHeaderTableRow.Cells.Add(groupHeaderTableCell_Desc);

            reportHeaderTable.Rows.Add(reportHeaderTableRow);

            reportHeaderTableRow = new XRTableRow();
            reportHeaderTableRow.HeightF = reportHeaderTable.HeightF / 6;

            groupHeaderTableCell_Name = new XRTableCell();
            groupHeaderTableCell_Name.WidthF = reportHeaderTableCellWidth_Caption;
            groupHeaderTableCell_Name.Font = new Font("Times New Roman", 9, FontStyle.Bold);
            groupHeaderTableCell_Name.Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (documentId).ToString() + "_ValorDoc");
            reportHeaderTableRow.Cells.Add(groupHeaderTableCell_Name);

            groupHeaderTableCell_Desc = new XRTableCell();
            groupHeaderTableCell_Desc.WidthF = reportHeaderTableCellWidth_ID;
            groupHeaderTableCell_Desc.Borders = DevExpress.XtraPrinting.BorderSide.None;
            groupHeaderTableCell_Desc.DataBindings.Add("Text", this.DataSource, "ValorDoc", "{0:#,0.00}");
            reportHeaderTableRow.Cells.Add(groupHeaderTableCell_Desc);

            reportHeaderTable.Rows.Add(reportHeaderTableRow);

            reportHeaderBand.Controls.Add(reportHeaderTable);

            XRShape table3_HeaderFrame = new XRShape()
            {
                SizeF = new System.Drawing.SizeF(reportHeaderTableCellWidth_Caption, reportHeaderTable.HeightF),
                LocationF = reportHeaderTable.LocationF,
                BackColor = Color.WhiteSmoke,
                AnchorVertical = VerticalAnchorStyles.Both,
                ForeColor = Color.Transparent
            };
            table3_HeaderFrame.SendToBack();
            reportHeaderTable.EndInit();
            reportHeaderBand.Controls.Add(table3_HeaderFrame);
            #endregion


            //XRLabel selectedFiltersLable = new XRLabel();
            //selectedFiltersLable.LocationF = new PointF(0, headerFrame.LocationF.Y + headerFrame.HeightF + 10);
            //selectedFiltersLable.SizeF = new SizeF(tableWidth, 20);
            //selectedFiltersLable.Font = new Font("Times New Roman", 9, FontStyle.Italic);
            //selectedFiltersLable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            //selectedFiltersLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(20, 0, 0, 0);
            //selectedFiltersLable.Text = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, documentId.ToString() + "_FiltradoPor") + ":   ";
            //if (selectedFiltersList.Count > 0)
            //{
            //    int filterCount = 0;
            //    foreach (string filter in selectedFiltersList)
            //    {
            //        selectedFiltersLable.Text += filter;
            //        filterCount++;
            //        if (!(filterCount == (selectedFiltersList.Count))) selectedFiltersLable.Text += ",  ";
            //    };
            //    reportGroupHeader.Controls.Add(selectedFiltersLable);
            //};

            #endregion
            #region Report group header
            XRShape groupFrame = new XRShape()
            {
                SizeF = new System.Drawing.SizeF(tableWidth, observacionGroupHeader.HeightF - 10),
                LocationF = new PointF(0, 5),
                BackColor = Color.Transparent,
                AnchorVertical = VerticalAnchorStyles.Both,
                LineWidth = 2,
                ForeColor = Color.LightGray
            };
            groupFrame.Shape = new ShapeRectangle() { Fillet = 20 };
            observacionGroupHeader.Controls.Add(groupFrame);

            XRTable groupHeader;
            XRTableRow groupHeaderRow;
            XRTableCell groupHeaderCell_ObservacionName;
            XRTableCell groupHeaderCell_ObservacionValue;
            //XRTableCell groupHeaderCell_EstadoName; 
            //XRTableCell groupHeaderCell_EstadoValue; 
            //XRTableCell groupHeaderCell_PeriodoName; 
            //XRTableCell groupHeaderCell_PeriodoValue;
            //XRTableCell groupHeaderCell_FechaName; 
            //XRTableCell groupHeaderCell_FechaValue;

            //float groupHeaderCellWidth = tableWidth / 8;

            groupHeader = new XRTable();
            groupHeader.LocationF = groupFrame.LocationF;//new PointF(tableWidth / 2, 5);
            groupHeader.SizeF = groupFrame.SizeF;//new SizeF(tableWidth / 2,25);
            groupHeader.StyleName = "groupHeaderStyle";
            groupHeader.Borders = DevExpress.XtraPrinting.BorderSide.None;
            groupHeaderRow = new XRTableRow();

            groupHeaderCell_ObservacionName = new XRTableCell();
            groupHeaderCell_ObservacionName.WidthF = tableWidth / 10;
            groupHeaderCell_ObservacionName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            groupHeaderCell_ObservacionName.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 10, 0, 0);
            groupHeaderCell_ObservacionName.Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (documentId).ToString() + "_Observacion") + ":  ";
            groupHeaderRow.Cells.Add(groupHeaderCell_ObservacionName);

            groupHeaderCell_ObservacionValue = new XRTableCell();
            groupHeaderCell_ObservacionValue.WidthF = tableWidth - tableWidth / 10;
            groupHeaderCell_ObservacionValue.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            groupHeaderCell_ObservacionValue.DataBindings.Add("Text", this.DataSource, "Observacion");
            groupHeaderRow.Cells.Add(groupHeaderCell_ObservacionValue);

            //groupHeaderCell_EstadoName = new XRTableCell();
            //groupHeaderCell_EstadoName.WidthF = groupHeaderCellWidth;
            //groupHeaderCell_EstadoName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            //groupHeaderCell_EstadoName.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 10, 0, 0);
            //groupHeaderCell_EstadoName.Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (documentId).ToString() + "_Estado") + ":  ";
            //groupHeaderRow.Cells.Add(groupHeaderCell_EstadoName);

            //groupHeaderCell_EstadoValue = new XRTableCell();
            //groupHeaderCell_EstadoValue.WidthF = 3 * groupHeaderCellWidth;
            //groupHeaderCell_EstadoValue.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            //groupHeaderCell_EstadoValue.DataBindings.Add("Text", this.DataSource, "DocumentoEstado");
            //groupHeaderRow.Cells.Add(groupHeaderCell_EstadoValue);

            //groupHeaderCell_PeriodoName = new XRTableCell();
            //groupHeaderCell_PeriodoName.WidthF = groupHeaderCellWidth;
            //groupHeaderCell_PeriodoName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            //groupHeaderCell_PeriodoName.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 10, 0, 0);
            //groupHeaderCell_PeriodoName.Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (documentId).ToString() + "_PeriodoID") + ":  ";
            //groupHeaderRow.Cells.Add(groupHeaderCell_PeriodoName);

            //groupHeaderCell_PeriodoValue = new XRTableCell();
            //groupHeaderCell_PeriodoValue.WidthF = groupHeaderCellWidth;
            //groupHeaderCell_PeriodoValue.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            //groupHeaderCell_PeriodoValue.DataBindings.Add("Text", this.DataSource, "Header.PeriodoID");
            //groupHeaderRow.Cells.Add(groupHeaderCell_PeriodoValue);

            //groupHeaderCell_FechaName = new XRTableCell();
            //groupHeaderCell_FechaName.WidthF = groupHeaderCellWidth;
            //groupHeaderCell_FechaName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            //groupHeaderCell_FechaName.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 10, 0, 0);
            //groupHeaderCell_FechaName.Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (documentId).ToString() + "_Fecha") + ":  ";
            //groupHeaderRow.Cells.Add(groupHeaderCell_FechaName);

            //groupHeaderCell_FechaValue = new XRTableCell();
            //groupHeaderCell_FechaValue.WidthF = groupHeaderCellWidth;
            //groupHeaderCell_FechaValue.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            //groupHeaderCell_FechaValue.DataBindings.Add("Text", this.DataSource, "Header.Fecha");
            //groupHeaderRow.Cells.Add(groupHeaderCell_FechaValue);

            groupHeader.Rows.Add(groupHeaderRow);
            observacionGroupHeader.Controls.Add(groupHeader);

            //XRLine groupHeaderLine = new XRLine()
            //{
            //    SizeF = new System.Drawing.SizeF(tableWidth, 3),
            //    LineWidth = 2,
            //    //LocationF = (selectedFiltersList.Count > 0) ? new PointF(0, selectedFiltersLable.LocationF.Y + selectedFiltersLable.HeightF + 12) : new PointF(0, headerFrame.LocationF.Y + headerFrame.HeightF + 12),
            //    LocationF = new PointF(0, reportGroupHeader.HeightF),
            //};
            //observacionGroupHeader.Controls.Add(groupHeaderLine);

            #endregion
            #region Report detail
            XRTable tableHeader = new XRTable();
            tableHeader.WidthF = tableWidth;
            tableHeader.HeightF = tableHeaderBand.HeightF - 3;
            tableHeader.LocationF = new PointF(0, 0);
            tableHeader.StyleName = "tableHeaderStyle";
            if (multiMonedaInd == true)
            {
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
                    Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_MdaLoc")
                };
                XRTableCell MECell = new XRTableCell()
                {
                    WidthF = 2 * columnWidth,
                    Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_MdaLoc")
                };
                tableHeaderUpperRow.Cells.AddRange(new XRTableCell[] { emptyCell, MLCell, MECell });
                tableHeader.Rows.Add(tableHeaderUpperRow);
            };
            XRTableRow tableHeaderRow = new XRTableRow();
            tableHeaderRow.HeightF = 25;

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
            //XRTableRow tableFooterRow_Total;
            XRTableCell tableFooterCell_Name;
            XRTableCell tableFooterCell_Value;
            //XRTableCell tableFooterCell_Total;
            tableFooter = new XRTable();
            tableFooter.LocationF = new PointF(0, 5);
            tableFooter.SizeF = new SizeF(tableWidth, 40);
            tableFooter.StyleName = "groupFooterStyle";
            tableFooterRow = new XRTableRow();
            tableFooterRow.HeightF = 40;
            //tableFooterRow_Total = new XRTableRow();
            //tableFooterRow_Total.HeightF = 40;

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
            #region Add controls
            float totalsFieldLocation = 0;
            int MLFieldInd = 0;
            int MEFieldInd = 0;
            int TotlasInd = 0;

            foreach (string fieldName in fieldList)
            {
                #region Report table header
                tableHeaderCell = new XRTableCell();
                tableHeaderCell.WidthF = (fieldName.Contains("Descriptivo")) ? columnWidth + 70 : columnWidth;
                string resourceId = (documentId).ToString() + "_" + fieldName;
                string columnname = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, resourceId);
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
                            calcField.Expression = "Iif([vlrMdaLoc.Value]>0,[vlrMdaLoc.Value],0)";
                            tableHeaderCell.Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (documentId).ToString() + "_DebitoML"); ////////////// Changing field header name
                            break;
                        case "CreditoML":
                            calcField.Expression = "Iif([vlrMdaLoc.Value]>0,0,(-1)*[vlrMdaLoc.Value])";
                            tableHeaderCell.Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (documentId).ToString() + "_CreditoML"); ////////////// Changing field header name
                            break;
                        case "DebitoME":
                            calcField.Expression = "Iif([vlrMdaExt.Value]>0,[vlrMdaExt.Value],0)";
                            tableHeaderCell.Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (documentId).ToString() + "_DebitoME"); ////////////// Changing field header name
                            break;
                        case "CreditoME":
                            calcField.Expression = "Iif([vlrMdaExt.Value]>0,0,(-1)*[vlrMdaExt.Value])";
                            tableHeaderCell.Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (documentId).ToString() + "_CreditoME"); ////////////// Changing field header name
                            break;
                    };
                };
                if (fieldName.Contains("ME") || fieldName.Contains("ML"))
                {
                    if (fieldName.Contains("Base"))
                    {
                        tableDetailCell.DataBindings.Add("Text", this.DataSource, "footerReport." + fieldName + ".Value").FormatString = "{0:#,0.00}";
                        tableHeaderCell.Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (documentId).ToString() + "_BaseML"); ////////////// Changing field header name
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
                            Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_Totals") + ": ",
                            WidthF = totalsFieldLocation,
                        };
                        tableFooterRow.Cells.Add(tableFooterCell_Name);

                        //tableFooterCell_Name = new XRTableCell()
                        //{
                        //    Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (documentId).ToString() + "_TotalMvto") + ": ",
                        //    WidthF = (fieldName.Contains("Base")) ? totalsFieldLocation + columnWidth : totalsFieldLocation,
                        //};
                        //tableFooterRow_Total.Cells.Add(tableFooterCell_Name);
                        TotlasInd = 1;
                    };
                    tableFooterCell_Value = new XRTableCell();
                    //tableFooterCell_Total = new XRTableCell();
                    tableFooterCell_Value.Name = "tableFooterCell_Value_" + fieldName;
                    tableFooterCell_Value.WidthF = tableHeaderCell.WidthF;
                    tableFooterCell_Value.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                    tableFooterCell_Value.BorderWidth = 2;
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
                        //tableFooterCell_Total.Name = "tableFooterCell_Total_" + fieldName;
                        //tableFooterCell_Total.WidthF = 2 * columnWidth;
                        //tableFooterCell_Total.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                        //tableFooterCell_Total.BorderColor = Color.Black;
                        //tableFooterCell_Total.BorderWidth = 2;
                        //if (fieldName.Contains("Debito")) tableFooterRow_Total.Cells.Add(tableFooterCell_Total);
                        //tableFooterCell_Total.BeforePrint +=new System.Drawing.Printing.PrintEventHandler(tableFooterCell_Total_BeforePrint);
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
                        //tableFooterCell_Value.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top;
                        //tableFooterCell_Value.BorderColor = Color.Black;
                        MLFieldInd = 1;
                    };
                    if (fieldName.Contains("ME") && MEFieldInd == 0)
                    {
                        tableHeaderCell.Borders = DevExpress.XtraPrinting.BorderSide.Left;
                        tableHeaderCell.BorderColor = Color.White;
                        tableDetailCell.Borders = DevExpress.XtraPrinting.BorderSide.Left;
                        tableDetailCell.BorderColor = Color.Black;
                        if (multiMonedaInd)
                        {
                            tableFooterCell_Value.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top;
                            tableFooterCell_Value.BorderColor = Color.Black;
                            //tableFooterCell_Total.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top;
                            //tableFooterCell_Total.BorderColor = Color.Black;
                        };
                        MEFieldInd = 1;
                    };
                    #endregion
                };

                #endregion

                if (TotlasInd == 0)
                    totalsFieldLocation += tableHeaderCell.WidthF;
            };

            tableHeader.Controls.Add(tableHeaderRow);
            tableDetail.Controls.Add(tableDetailRow);
            tableFooter.Controls.Add(tableFooterRow);
            tableFooter.Controls.Add(tableFooterRow);
            //tableFooter.Controls.Add(tableFooterRow_Total);

            tableHeaderBand.Controls.Add(tableHeader);
            reportDetail.Controls.Add(tableDetail);
            reportGroupFooter.Controls.Add(tableFooter);
            #endregion
        } 
        
        #endregion

        #region Eventos

        /// <summary>
        /// Calculates Total Movimiento (Debito + Credito)
        /// </summary>
        private void tableFooterCell_Total_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell total_Cell = (XRTableCell)sender;
            if (total_Cell.Name.Contains("ML"))
            {
                XRTableCell totalDebitoML_Cell = FindControl("tableFooterCell_Value_DebitoML", true) as XRTableCell;
                double totalDebitoML = Convert.ToDouble(totalDebitoML_Cell.Summary.GetResult());

                XRTableCell totalCreditoML_Cell = FindControl("tableFooterCell_Value_CreditoML", true) as XRTableCell;
                double totalCreditoML = Convert.ToDouble(totalCreditoML_Cell.Summary.GetResult());

                double total = totalDebitoML - totalCreditoML;
                total_Cell.Text = total.ToString("#,0.00");
                total_Cell.Padding = (total > 0) ? new DevExpress.XtraPrinting.PaddingInfo(0, Convert.ToInt32(_columnWidth) + 1, 0, 0) : new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0);
            }
            else
            {
                XRTableCell totalDebitoME_Cell = FindControl("tableFooterCell_Value_DebitoME", true) as XRTableCell;
                double totalDebitoME = Convert.ToDouble(totalDebitoME_Cell.Summary.GetResult());

                XRTableCell totalCreditoME_Cell = FindControl("tableFooterCell_Value_CreditoME", true) as XRTableCell;
                double totalCreditoME = Convert.ToDouble(totalCreditoME_Cell.Summary.GetResult());

                double total = totalDebitoME - totalCreditoME;
                total_Cell.Text = total.ToString("#,0.00");
                total_Cell.Padding = (total > 0) ? new DevExpress.XtraPrinting.PaddingInfo(0, Convert.ToInt32(_columnWidth) + 1, 0, 0) : new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0);
            };
        } 

        #endregion
    }
}
