using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting.Shape;
using NewAge.Librerias.Project;
using NewAge.DTO.Reportes;
using System.Collections.Generic;
using DevExpress.XtraPrinting.Drawing;

namespace NewAge.ReportesComunes
{
    public partial class LegalizacionGastosReport : BaseCommonReport
    {
        #region Variables
        CommonReportDataSupplier _supplier;
        int _docId;
        #endregion

        #region Funciones Publicas
        /// <summary>
        /// LegalizacionGastosReport Report Constructor
        /// </summary>
        /// <param name="docId">Report ID (from AppReport)</param>
        /// <param name="documentoData">data for the report</param>
        /// <param name="fieldList">list of fields for report detail table</param>
        /// <param name="estadoInd">indicador de aprobacion del documneto (aprobado - false)</param> 
        /// <param name="supplier"> Interface que provee de informacion a un reporte comun</param>
        public LegalizacionGastosReport(int docId, List<DTO_ReportLegalizacionGastos> documentoData, ArrayList footerFieldList, ArrayList detailFieldList, bool estadoInd, CommonReportDataSupplier supplier)
            : base(supplier)
        {
            this._supplier = supplier;
            this._docId = docId;
            this.lblReportName.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString());
            
            #region Documento styles
            this.Landscape = true;

            XRControlStyle headerStyle = new XRControlStyle()
            {
                Name = "groupHeaderStyle",
                BackColor = Color.Transparent,
                ForeColor = Color.Black,
                Font = new Font("Arial", 9, FontStyle.Bold),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight,
                Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 5, 0, 0)
            };
            this.StyleSheet.Add(headerStyle);

            XRControlStyle tableHeaderStyle2 = new XRControlStyle()
            {
                Name = "tableHeaderStyle2",
                BackColor = Color.Transparent,
                ForeColor = Color.Black,
                Borders = DevExpress.XtraPrinting.BorderSide.All,
                BorderColor = Color.Black,
                Font = new Font("Arial", 9, FontStyle.Bold),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
            };
            this.StyleSheet.Add(tableHeaderStyle2);

            XRControlStyle sumFieldStyle = new XRControlStyle()
            {
                Name = "groupFooterStyle",
                BackColor = Color.Transparent,
                ForeColor = Color.Black,
                Font = new Font("Arial", 9, FontStyle.Bold),
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
                    Font = new Font("Arial", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                    //Borders = DevExpress.XtraPrinting.BorderSide.Bottom,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0)
                },
                OddStyle = new XRControlStyle()
                {
                    Name = "tableDetailOddStyle",
                    BackColor = Color.Transparent,
                    ForeColor = Color.Black,
                    Font = new Font("Arial", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                    //Borders = DevExpress.XtraPrinting.BorderSide.Bottom,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0)
                },
                Style = new XRControlStyle()
                {
                    Name = "tableHeaderStyle",
                    BackColor = Color.DimGray,
                    ForeColor = Color.White,
                    Font = new Font("Arial", 9, FontStyle.Bold),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                }
            };

            this.StyleSheet.Add(tableStyles.EvenStyle);
            this.StyleSheet.Add(tableStyles.OddStyle);
            this.StyleSheet.Add(tableStyles.Style);

            #endregion

            #region Documento bands
            DetailReportBand documentoBand;
            documentoBand = new DetailReportBand();
            documentoBand.DataSource = documentoData;

            GroupHeaderBand documentoHeader = new GroupHeaderBand();
            documentoHeader.HeightF = 100;
            documentoHeader.Level = 1;
            documentoBand.Bands.Add(documentoHeader);

            DetailBand documentoDetail;
            documentoDetail = new DetailBand();
            documentoDetail.HeightF = 0;
            documentoBand.Bands.Add(documentoDetail);

            GroupFooterBand documentoFooter = new GroupFooterBand();
            documentoFooter.HeightF = 45;
            documentoFooter.Level = 1;
            documentoBand.Bands.Add(documentoFooter);

            #region Table 1

            DetailReportBand documentoBand1;
            documentoBand1 = new DetailReportBand();
            documentoBand1.DataSource = documentoData;
            documentoBand1.DataMember = "Footer";
            documentoBand1.Level = 0;

            GroupHeaderBand documentoTable1Header = new GroupHeaderBand();
            documentoTable1Header.HeightF = 30;
            documentoTable1Header.Level = 0;
            documentoBand1.Bands.Add(documentoTable1Header);

            DetailBand documentoTable1Detail;
            documentoTable1Detail = new DetailBand();
            documentoTable1Detail.HeightF = 20;
            documentoBand1.Bands.Add(documentoTable1Detail);

            GroupFooterBand documentoTable1Footer = new GroupFooterBand();
            documentoTable1Footer.HeightF = 70;
            documentoTable1Footer.Level = 0;
            documentoTable1Footer.PageBreak = DevExpress.XtraReports.UI.PageBreak.AfterBand;
            documentoBand1.Bands.Add(documentoTable1Footer);

            documentoBand.Bands.Add(documentoBand1);
            #endregion

            #region Table 2
            DetailReportBand documentoBand2;
            documentoBand2 = new DetailReportBand();
            documentoBand2.Level = 1;
            documentoBand2.DataSource = documentoData;
            documentoBand2.DataMember = "Detail";

            GroupHeaderBand documentoSubHeader = new GroupHeaderBand();
            documentoSubHeader.HeightF = 125;
            documentoSubHeader.Level = 1;
            documentoBand2.Bands.Add(documentoSubHeader);

            GroupHeaderBand documentoTable2Header = new GroupHeaderBand();
            documentoTable2Header.HeightF = 45;
            documentoTable2Header.Level = 0;
            documentoBand2.Bands.Add(documentoTable2Header);

            DetailBand documentoTable2Detail;
            documentoTable2Detail = new DetailBand();
            documentoTable2Detail.HeightF = 20;
            documentoBand2.Bands.Add(documentoTable2Detail);

            GroupFooterBand documentoTable2Footer = new GroupFooterBand();
            documentoTable2Footer.HeightF = 10;
            documentoTable2Footer.Level = 0;
            documentoBand2.Bands.Add(documentoTable2Footer);

            documentoBand.Controls.Add(documentoBand2);
            #endregion

            this.Bands.Add(documentoBand);
            #endregion

            #region Documento field width
            float tableWidth = 0;
            float tableWidth2 = 0;
            float columnWidth1 = 0;
            float columnWidth2 = 0;

            tableWidth = this.PageWidth - (this.Margins.Right + this.Margins.Left);            
            tableWidth2 = 3 * tableWidth/5;

            columnWidth1 = (tableWidth - 50) / footerFieldList.Count;
            columnWidth2 = (tableWidth2 - 50) / detailFieldList.Count;
            #endregion

            #region Documento elements

            #region Watermark
            if (estadoInd)
            {
                this.Watermark.Text = "Preliminar";
                this.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
                this.Watermark.Font = new Font("Arial", 100);
                this.Watermark.ForeColor = Color.LightGray;
                this.Watermark.TextTransparency = 150;
                this.Watermark.ShowBehind = true;
            };
            #endregion

            #region Documento title
            XRLabel lblTitleNit_Name = new XRLabel();
            lblTitleNit_Name.LocationF = new PointF(0, this.lblNombreEmpresa.LocationF.Y + this.lblNombreEmpresa.HeightF + 20);
            lblTitleNit_Name.SizeF = new SizeF(140, 20);
            lblTitleNit_Name.Font = new Font("Arial", 10, FontStyle.Bold);
            lblTitleNit_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            lblTitleNit_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(100, 0, 0, 0);
            lblTitleNit_Name.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (this._docId).ToString() + "_Nit");
            this.ReportHeader.Controls.Add(lblTitleNit_Name);

            XRLabel lblTitleNit_Value = new XRLabel();
            lblTitleNit_Value.LocationF = new PointF(lblTitleNit_Name.LocationF.X + lblTitleNit_Name.WidthF, lblTitleNit_Name.LocationF.Y);
            lblTitleNit_Value.SizeF = new SizeF(tableWidth - lblTitleNit_Name.WidthF, 20);
            lblTitleNit_Value.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0);
            lblTitleNit_Value.Font = new Font("Arial", 10);
            lblTitleNit_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            lblTitleNit_Value.Text = this._supplier.GetNitEmpresa();
            this.ReportHeader.Controls.Add(lblTitleNit_Value);
            #endregion

            #region Documento header

            XRShape headerFrame = new XRShape()
            {
                SizeF = new System.Drawing.SizeF(tableWidth, documentoHeader.HeightF),
                LocationF = new PointF(0, 1),
                BackColor = Color.Transparent,
                AnchorVertical = VerticalAnchorStyles.Both,
                LineWidth = 2,
            };
            headerFrame.Shape = new ShapeRectangle() { Fillet = 8 };
            documentoHeader.Controls.Add(headerFrame);

            XRTable documentoHeaderTable1;
            XRTableRow documentoHeaderTableRow1;
            XRTableCell documentoHeaderTableCell_Name1;
            XRTableCell documentoHeaderTableCell_Value1;

            documentoHeaderTable1 = new XRTable();
            documentoHeaderTable1.BeginInit();
            documentoHeaderTable1.LocationF = new PointF(headerFrame.LocationF.X + 1, headerFrame.LocationF.Y + 1);
            documentoHeaderTable1.HeightF = headerFrame.Height - 2;
            documentoHeaderTable1.WidthF = 5 * headerFrame.WidthF / 6 - 1;
            documentoHeaderTable1.StyleName = "groupHeaderStyle";
            float documentoHeaderTableCellWidth1 = documentoHeaderTable1.WidthF / 8;
            #region Row 1
            documentoHeaderTableRow1 = new XRTableRow();
            documentoHeaderTableRow1.HeightF = documentoHeaderTable1.HeightF / 3;

            documentoHeaderTableCell_Name1 = new XRTableCell();
            documentoHeaderTableCell_Name1.WidthF = documentoHeaderTableCellWidth1;
            documentoHeaderTableCell_Name1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoHeaderTableCell_Name1.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString() + "_Fecha") + ":";
            documentoHeaderTableRow1.Cells.Add(documentoHeaderTableCell_Name1);

            documentoHeaderTableCell_Value1 = new XRTableCell();
            documentoHeaderTableCell_Value1.WidthF = documentoHeaderTableCellWidth1;
            documentoHeaderTableCell_Value1.Font = new Font("Arial", 9);
            documentoHeaderTableCell_Value1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoHeaderTableCell_Value1.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            documentoHeaderTableCell_Value1.DataBindings.Add("Text", documentoData, "Header.Fecha", "{0:dd/MM/yyyy}");
            documentoHeaderTableRow1.Cells.Add(documentoHeaderTableCell_Value1);

            documentoHeaderTableCell_Name1 = new XRTableCell();
            documentoHeaderTableCell_Name1.WidthF = documentoHeaderTableCellWidth1;
            documentoHeaderTableCell_Name1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            documentoHeaderTableCell_Name1.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString() + "_Empoyee") + ":";
            documentoHeaderTableRow1.Cells.Add(documentoHeaderTableCell_Name1);

            documentoHeaderTableCell_Value1 = new XRTableCell();
            documentoHeaderTableCell_Value1.WidthF = 3 * documentoHeaderTableCellWidth1;
            documentoHeaderTableCell_Value1.Font = new Font("Arial", 9);
            documentoHeaderTableCell_Value1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoHeaderTableCell_Value1.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            documentoHeaderTableCell_Value1.DataBindings.Add("Text", documentoData, "Header.TerceroDesc");
            documentoHeaderTableRow1.Cells.Add(documentoHeaderTableCell_Value1);

            documentoHeaderTableCell_Name1 = new XRTableCell();
            documentoHeaderTableCell_Name1.WidthF = documentoHeaderTableCellWidth1;
            documentoHeaderTableCell_Name1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoHeaderTableCell_Name1.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString() + "_Documento") + ":";
            documentoHeaderTableRow1.Cells.Add(documentoHeaderTableCell_Name1);

            documentoHeaderTableCell_Value1 = new XRTableCell();
            documentoHeaderTableCell_Value1.WidthF = documentoHeaderTableCellWidth1 / 2 - 10;
            documentoHeaderTableCell_Value1.Font = new Font("Arial", 9);
            documentoHeaderTableCell_Value1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            documentoHeaderTableCell_Value1.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 3, 0, 0);
            documentoHeaderTableCell_Value1.DataBindings.Add("Text", documentoData, "Header.Prefijo");
            documentoHeaderTableRow1.Cells.Add(documentoHeaderTableCell_Value1);

            documentoHeaderTableCell_Value1 = new XRTableCell();
            documentoHeaderTableCell_Value1.WidthF = 20;
            documentoHeaderTableCell_Value1.Font = new Font("Arial", 9);
            documentoHeaderTableCell_Value1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            documentoHeaderTableCell_Value1.Text = "-";
            documentoHeaderTableRow1.Cells.Add(documentoHeaderTableCell_Value1);

            documentoHeaderTableCell_Value1 = new XRTableCell();
            documentoHeaderTableCell_Value1.WidthF = documentoHeaderTableCellWidth1 / 2 - 10;
            documentoHeaderTableCell_Value1.Font = new Font("Arial", 9);
            documentoHeaderTableCell_Value1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoHeaderTableCell_Value1.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 0, 0);
            documentoHeaderTableCell_Value1.DataBindings.Add("Text", documentoData, "Header.DocumentoNro");
            documentoHeaderTableRow1.Cells.Add(documentoHeaderTableCell_Value1);

            documentoHeaderTable1.Rows.Add(documentoHeaderTableRow1);
            #endregion
            #region Row 2
            documentoHeaderTableRow1 = new XRTableRow();
            documentoHeaderTableRow1.HeightF = 2 * documentoHeaderTable1.HeightF / 3;
            
            documentoHeaderTableCell_Name1 = new XRTableCell();
            documentoHeaderTableCell_Name1.WidthF = documentoHeaderTableCellWidth1;
            documentoHeaderTableCell_Name1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoHeaderTableCell_Name1.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString() + "_DocumentoDesc");
            documentoHeaderTableRow1.Cells.Add(documentoHeaderTableCell_Name1);

            documentoHeaderTableCell_Value1 = new XRTableCell();
            documentoHeaderTableCell_Value1.WidthF = 5 * documentoHeaderTableCellWidth1;
            documentoHeaderTableCell_Value1.Font = new Font("Arial", 9);
            documentoHeaderTableCell_Value1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoHeaderTableCell_Value1.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            documentoHeaderTableCell_Value1.DataBindings.Add("Text", documentoData, "Header.DocumentoDesc");
            documentoHeaderTableRow1.Cells.Add(documentoHeaderTableCell_Value1);
           
            documentoHeaderTable1.Rows.Add(documentoHeaderTableRow1);
            #endregion
            documentoHeaderTable1.EndInit();
            documentoHeader.Controls.Add(documentoHeaderTable1);

            XRTable documentoHeaderTable2;
            XRTableRow documentoHeaderTableRow2;
            XRTableCell documentoHeaderTableCell_Name2;
            XRTableCell documentoHeaderTableCell_Value2;

            documentoHeaderTable2 = new XRTable();
            documentoHeaderTable2.BeginInit();
            documentoHeaderTable2.LocationF = new PointF(headerFrame.LocationF.X + documentoHeaderTable1.LocationF.X + documentoHeaderTable1.WidthF, headerFrame.LocationF.Y + 1);
            documentoHeaderTable2.HeightF = headerFrame.Height - 2;
            documentoHeaderTable2.WidthF = headerFrame.WidthF / 6 - 1;
            documentoHeaderTable2.StyleName = "groupHeaderStyle";
            float documentoHeaderTableCellWidth2 = documentoHeaderTable2.WidthF / 2;
            #region Row 1
            documentoHeaderTableRow2 = new XRTableRow();
            documentoHeaderTableRow2.HeightF = documentoHeaderTable1.HeightF / 4;

            documentoHeaderTableCell_Name2 = new XRTableCell();
            documentoHeaderTableCell_Name2.WidthF = documentoHeaderTableCellWidth2;
            documentoHeaderTableCell_Name2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoHeaderTableCell_Name2.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString() + "_MonedaID") + ":";
            documentoHeaderTableRow2.Cells.Add(documentoHeaderTableCell_Name2);

            documentoHeaderTableCell_Value2 = new XRTableCell();
            documentoHeaderTableCell_Value2.WidthF = documentoHeaderTableCellWidth2;
            documentoHeaderTableCell_Value2.Font = new Font("Arial", 9);
            documentoHeaderTableCell_Value2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoHeaderTableCell_Value2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0);
            documentoHeaderTableCell_Value2.DataBindings.Add("Text", documentoData, "Header.MonedaID");
            documentoHeaderTableRow2.Cells.Add(documentoHeaderTableCell_Value2);

            documentoHeaderTable2.Rows.Add(documentoHeaderTableRow2);
            #endregion
            #region Row 2
            documentoHeaderTableRow2 = new XRTableRow();
            documentoHeaderTableRow2.HeightF = documentoHeaderTable1.HeightF / 4;

            documentoHeaderTableCell_Name2 = new XRTableCell();
            documentoHeaderTableCell_Name2.WidthF = documentoHeaderTableCellWidth2;
            documentoHeaderTableCell_Name2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoHeaderTableCell_Name2.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString() + "_LugarGeografico") + ":";
            documentoHeaderTableRow2.Cells.Add(documentoHeaderTableCell_Name2);

            documentoHeaderTableCell_Value2 = new XRTableCell();
            documentoHeaderTableCell_Value2.WidthF = documentoHeaderTableCellWidth2;
            documentoHeaderTableCell_Value2.Font = new Font("Arial", 9);
            documentoHeaderTableCell_Value2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoHeaderTableCell_Value2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0);
            documentoHeaderTableCell_Value2.DataBindings.Add("Text", documentoData, "Header.LugarGeograficoDesc");
            documentoHeaderTableRow2.Cells.Add(documentoHeaderTableCell_Value2);

            documentoHeaderTable2.Rows.Add(documentoHeaderTableRow2);
            #endregion
            #region Row 3
            documentoHeaderTableRow2 = new XRTableRow();
            documentoHeaderTableRow2.HeightF = documentoHeaderTable1.HeightF / 4;

            documentoHeaderTableCell_Name2 = new XRTableCell();
            documentoHeaderTableCell_Name2.WidthF = documentoHeaderTableCellWidth2;
            documentoHeaderTableCell_Name2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            //documentoHeaderTableCell_Name2.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString() + "_MonedaID") + ":";
            documentoHeaderTableRow2.Cells.Add(documentoHeaderTableCell_Name2);

            documentoHeaderTableCell_Value2 = new XRTableCell();
            documentoHeaderTableCell_Value2.WidthF = documentoHeaderTableCellWidth2;
            documentoHeaderTableCell_Value2.Font = new Font("Arial", 9);
            documentoHeaderTableCell_Value2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoHeaderTableCell_Value2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0);
            //documentoHeaderTableCell_Value2.DataBindings.Add("Text", documentoData, "Header.MonedaID");
            documentoHeaderTableRow2.Cells.Add(documentoHeaderTableCell_Value2);

            documentoHeaderTable2.Rows.Add(documentoHeaderTableRow2);
            #endregion
            #region Row 4
            documentoHeaderTableRow2 = new XRTableRow();
            documentoHeaderTableRow2.HeightF = documentoHeaderTable1.HeightF / 4;

            documentoHeaderTableCell_Name2 = new XRTableCell();
            documentoHeaderTableCell_Name2.WidthF = documentoHeaderTableCellWidth2;
            documentoHeaderTableCell_Name2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            //documentoHeaderTableCell_Name2.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString() + "_LugarGeografico") + ":";
            documentoHeaderTableRow2.Cells.Add(documentoHeaderTableCell_Name2);

            documentoHeaderTableCell_Value2 = new XRTableCell();
            documentoHeaderTableCell_Value2.WidthF = documentoHeaderTableCellWidth2;
            documentoHeaderTableCell_Value2.Font = new Font("Arial", 9);
            documentoHeaderTableCell_Value2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoHeaderTableCell_Value2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0);
            //documentoHeaderTableCell_Value2.DataBindings.Add("Text", documentoData, "Header.LugarGeograficoDesc");
            documentoHeaderTableRow2.Cells.Add(documentoHeaderTableCell_Value2);

            documentoHeaderTable2.Rows.Add(documentoHeaderTableRow2);
            #endregion
            documentoHeaderTable2.EndInit();
            documentoHeader.Controls.Add(documentoHeaderTable2);
            #endregion

            #region Decumento Table 1

            #region Documento Table header
            XRTable table1Header;
            XRTableRow table1HeaderRow;
            XRTableCell table1HeaderCell;
            table1Header = new XRTable();
            table1Header.LocationF = new PointF(0, 20);
            table1Header.SizeF = new System.Drawing.SizeF(tableWidth, 25);
            table1Header.StyleName = "tableHeaderStyle";
            table1HeaderRow = new XRTableRow();
            #endregion

            #region Documento Table detail

            XRTable table1Detail;
            XRTableRow table1DetailRow;
            XRTableCell table1DetailCell;
            table1Detail = new XRTable();
            table1Detail.LocationF = new PointF(0, 0);
            table1Detail.SizeF = new System.Drawing.SizeF(tableWidth, 20);
            table1Detail.OddStyleName = "tableDetailOddStyle";
            table1Detail.EvenStyleName = "tableDetailEvenStyle";
            table1DetailRow = new XRTableRow();
            table1DetailRow.Name = "tableDetailRow";
            table1DetailRow.HeightF = 20;
            #endregion

            #region Documento Table footer
            XRTable table1Footer;
            XRTableRow table1FooterRow;
            XRTableCell table1FooterCell;
            table1Footer = new XRTable();
            table1Footer.LocationF = new PointF(0, 2);
            table1Footer.SizeF = new SizeF(tableWidth, 30);
            table1Footer.StyleName = "groupFooterStyle";
            table1FooterRow = new XRTableRow();
            table1FooterRow.Name = "table1FooterRow";
            table1FooterRow.HeightF = 30;
            #endregion

            #endregion

            #region Documento Table 2
            
            #region Documento Subheader
            XRShape subHeaderFrame = new XRShape()
            {
                SizeF = new System.Drawing.SizeF(tableWidth, documentoSubHeader.HeightF),
                LocationF = new PointF(0, 0),
                BackColor = Color.Transparent,
                ForeColor = Color.Black,
                AnchorVertical = VerticalAnchorStyles.Both,
                LineWidth = 2,
            };

            subHeaderFrame.Shape = new ShapeRectangle() { Fillet = 2 };
            documentoSubHeader.Controls.Add(subHeaderFrame);

            XRTable documentoSubHeaderTable1;
            XRTableRow documentoSubHeaderTableRow1;
            XRTableCell documentoSubHeaderTableCell;

            documentoSubHeaderTable1 = new XRTable();
            documentoSubHeaderTable1.BeginInit();
            documentoSubHeaderTable1.LocationF = new PointF(subHeaderFrame.LocationF.X + 1, subHeaderFrame.LocationF.Y + 1);
            documentoSubHeaderTable1.HeightF = subHeaderFrame.HeightF -2;
            documentoSubHeaderTable1.WidthF = 4 * (tableWidth-2) / 5;
            documentoSubHeaderTable1.StyleName = "groupHeaderStyle";
            documentoSubHeaderTable1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            #region Row 1
            documentoSubHeaderTableRow1 = new XRTableRow();
            documentoSubHeaderTableRow1.HeightF = (subHeaderFrame.HeightF - 2) / 5;
            documentoSubHeaderTableCell = new XRTableCell();
            documentoSubHeaderTableCell.WidthF = tableWidth;
            documentoSubHeaderTableCell.Padding = new DevExpress.XtraPrinting.PaddingInfo(10,0,0,0);  
            documentoSubHeaderTableCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoSubHeaderTableCell.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString() + "_Empoyee") + ":";
            documentoSubHeaderTableRow1.Cells.Add(documentoSubHeaderTableCell);
            documentoSubHeaderTable1.Rows.Add(documentoSubHeaderTableRow1);

            documentoSubHeaderTableRow1 = new XRTableRow();
            documentoSubHeaderTableRow1.HeightF = (subHeaderFrame.HeightF - 2) / 5;
            documentoSubHeaderTableCell = new XRTableCell();
            documentoSubHeaderTableCell.WidthF = tableWidth;
            documentoSubHeaderTableCell.Font = new Font("Arial", 9);
            documentoSubHeaderTableCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoSubHeaderTableCell.Padding = new DevExpress.XtraPrinting.PaddingInfo(100, 0, 0, 0);
            documentoSubHeaderTableCell.Borders = DevExpress.XtraPrinting.BorderSide.Bottom; 
            documentoSubHeaderTableCell.DataBindings.Add("Text", documentoData, "Header.TerceroDesc");
            documentoSubHeaderTableRow1.Cells.Add(documentoSubHeaderTableCell);
            documentoSubHeaderTable1.Rows.Add(documentoSubHeaderTableRow1);
            #endregion
            #region Row 2
            documentoSubHeaderTableRow1 = new XRTableRow();
            documentoSubHeaderTableRow1.HeightF = (subHeaderFrame.HeightF - 2) / 5;
            documentoSubHeaderTableCell = new XRTableCell();
            documentoSubHeaderTableCell.WidthF = tableWidth;
            documentoSubHeaderTableCell.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            documentoSubHeaderTableCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoSubHeaderTableCell.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString() + "_DocumentoDesc") + ":";
            documentoSubHeaderTableRow1.Cells.Add(documentoSubHeaderTableCell);
            documentoSubHeaderTable1.Rows.Add(documentoSubHeaderTableRow1);

            documentoSubHeaderTableRow1 = new XRTableRow();
            documentoSubHeaderTableRow1.HeightF = 2 * (subHeaderFrame.HeightF - 2) / 5;
            documentoSubHeaderTableCell = new XRTableCell();
            documentoSubHeaderTableCell.WidthF = tableWidth;
            documentoSubHeaderTableCell.Font = new Font("Arial", 9);
            documentoSubHeaderTableCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoSubHeaderTableCell.Padding = new DevExpress.XtraPrinting.PaddingInfo(100, 0, 0, 0);
            documentoSubHeaderTableCell.DataBindings.Add("Text", documentoData, "Header.DocumentoDesc");
            documentoSubHeaderTableRow1.Cells.Add(documentoSubHeaderTableCell);
            documentoSubHeaderTable1.Rows.Add(documentoSubHeaderTableRow1);
            #endregion
            documentoSubHeaderTable1.EndInit();
            documentoSubHeader.Controls.Add(documentoSubHeaderTable1);

            XRTable documentoSubHeaderTable2;
            XRTableRow documentoSubHeaderTableRow2;
            XRTableCell documentoSubHeaderTableCell_Name2;
            XRTableCell documentoSubHeaderTableCell_Value2;

            documentoSubHeaderTable2 = new XRTable();
            documentoSubHeaderTable2.BeginInit();
            documentoSubHeaderTable2.LocationF = new PointF(documentoSubHeaderTable1.LocationF.X + documentoSubHeaderTable1.WidthF, documentoSubHeaderTable1.LocationF.Y);
            documentoSubHeaderTable2.HeightF = subHeaderFrame.HeightF - 2;
            documentoSubHeaderTable2.AnchorVertical = VerticalAnchorStyles.Both;
            documentoSubHeaderTable2.WidthF =  (tableWidth - 2) / 5;
            documentoSubHeaderTable2.StyleName = "groupHeaderStyle";
            documentoSubHeaderTable2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            float documentoSubHeaderTableCellWidth2 = documentoSubHeaderTable2.WidthF / 2;
            #region Row 1
            documentoSubHeaderTableRow2 = new XRTableRow();
            documentoSubHeaderTableRow2.HeightF = 2 * (subHeaderFrame.HeightF - 2) / 5;

            documentoSubHeaderTableCell_Name2 = new XRTableCell();
            documentoSubHeaderTableCell_Name2.WidthF = documentoSubHeaderTableCellWidth2;
            documentoSubHeaderTableCell_Name2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoSubHeaderTableCell_Name2.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom;
            documentoSubHeaderTableCell_Name2.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString() + "_MonedaID") + ":";
            documentoSubHeaderTableRow2.Cells.Add(documentoSubHeaderTableCell_Name2);

            documentoSubHeaderTableCell_Value2 = new XRTableCell();
            documentoSubHeaderTableCell_Value2.WidthF = documentoSubHeaderTableCellWidth2;
            documentoSubHeaderTableCell_Value2.Font = new Font("Arial", 9);
            documentoSubHeaderTableCell_Value2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoSubHeaderTableCell_Value2.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            documentoSubHeaderTableCell_Value2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0);
            documentoSubHeaderTableCell_Value2.DataBindings.Add("Text", documentoData, "Header.MonedaID");
            documentoSubHeaderTableRow2.Cells.Add(documentoSubHeaderTableCell_Value2);

            documentoSubHeaderTable2.Rows.Add(documentoSubHeaderTableRow2);
            #endregion
            #region Row 2
            documentoSubHeaderTableRow2 = new XRTableRow();
            documentoSubHeaderTableRow2.HeightF = (subHeaderFrame.HeightF - 2) / 5;

            documentoSubHeaderTableCell_Name2 = new XRTableCell();
            documentoSubHeaderTableCell_Name2.WidthF = documentoSubHeaderTableCellWidth2;
            documentoSubHeaderTableCell_Name2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoSubHeaderTableCell_Name2.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right;
            documentoSubHeaderTableCell_Name2.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString() + "_LugarGeografico") + ":";
            documentoSubHeaderTableRow2.Cells.Add(documentoSubHeaderTableCell_Name2);

            documentoSubHeaderTableCell_Value2 = new XRTableCell();
            documentoSubHeaderTableCell_Value2.WidthF = documentoSubHeaderTableCellWidth2;
            documentoSubHeaderTableCell_Value2.Font = new Font("Arial", 9);
            documentoSubHeaderTableCell_Value2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoSubHeaderTableCell_Value2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0);
            documentoSubHeaderTableCell_Value2.DataBindings.Add("Text", documentoData, "Header.LugarGeograficoDesc");
            documentoSubHeaderTableRow2.Cells.Add(documentoSubHeaderTableCell_Value2);

            documentoSubHeaderTable2.Rows.Add(documentoSubHeaderTableRow2);
            #endregion
            #region Row 3
            documentoSubHeaderTableRow2 = new XRTableRow();
            documentoSubHeaderTableRow2.HeightF = (subHeaderFrame.HeightF - 2) / 5;

            documentoSubHeaderTableCell_Name2 = new XRTableCell();
            documentoSubHeaderTableCell_Name2.WidthF = documentoSubHeaderTableCellWidth2;
            documentoSubHeaderTableCell_Name2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoSubHeaderTableCell_Name2.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Top;
            //documentoSubHeaderTableCell_Name2.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString() + "_MonedaID") + ":";
            documentoSubHeaderTableRow2.Cells.Add(documentoSubHeaderTableCell_Name2);

            documentoSubHeaderTableCell_Value2 = new XRTableCell();
            documentoSubHeaderTableCell_Value2.WidthF = documentoSubHeaderTableCellWidth2;
            documentoSubHeaderTableCell_Value2.Font = new Font("Arial", 9);
            documentoSubHeaderTableCell_Value2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoSubHeaderTableCell_Value2.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            documentoSubHeaderTableCell_Value2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0);
            //documentoSubHeaderTableCell_Value2.DataBindings.Add("Text", documentoData, "Header.MonedaID");
            documentoSubHeaderTableRow2.Cells.Add(documentoSubHeaderTableCell_Value2);

            documentoSubHeaderTable2.Rows.Add(documentoSubHeaderTableRow2);
            #endregion
            #region Row 4
            documentoSubHeaderTableRow2 = new XRTableRow();
            documentoSubHeaderTableRow2.CanGrow = false;
            documentoSubHeaderTableRow2.HeightF = (subHeaderFrame.HeightF - 2) / 5;

            documentoSubHeaderTableCell_Name2 = new XRTableCell();
            documentoSubHeaderTableCell_Name2.WidthF = documentoSubHeaderTableCellWidth2;
            documentoSubHeaderTableCell_Name2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoSubHeaderTableCell_Name2.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Top;
            //documentoSubHeaderTableCell_Name2.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString() + "_LugarGeografico") + ":";
            documentoSubHeaderTableRow2.Cells.Add(documentoSubHeaderTableCell_Name2);

            documentoSubHeaderTableCell_Value2 = new XRTableCell();
            documentoSubHeaderTableCell_Value2.WidthF = documentoSubHeaderTableCellWidth2;
            documentoSubHeaderTableCell_Value2.Font = new Font("Arial", 9);
            documentoSubHeaderTableCell_Value2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoSubHeaderTableCell_Value2.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            documentoSubHeaderTableCell_Value2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0);
            //documentoSubHeaderTableCell_Value2.DataBindings.Add("Text", documentoData, "Header.LugarGeograficoDesc");
            documentoSubHeaderTableRow2.Cells.Add(documentoSubHeaderTableCell_Value2);

            documentoSubHeaderTable2.Rows.Add(documentoSubHeaderTableRow2);
            #endregion
            documentoSubHeaderTable2.EndInit();
            documentoSubHeader.Controls.Add(documentoSubHeaderTable2);
            #endregion

            #region Documento Table header
            XRTable table2Header;
            XRTableRow table2HeaderRow;
            XRTableCell table2HeaderCell;
            table2Header = new XRTable();
            table2Header.LocationF = new PointF(tableWidth / 5, 25);
            table2Header.SizeF = new System.Drawing.SizeF(tableWidth2, 25);
            table2Header.StyleName = "tableHeaderStyle2";
            table2HeaderRow = new XRTableRow();
            #endregion

            #region Documento Table detail

            XRTable table2Detail;
            XRTableRow table2DetailRow;
            XRTableCell table2DetailCell;
            table2Detail = new XRTable();
            table2Detail.LocationF = new PointF(tableWidth / 5, 0);
            table2Detail.SizeF = new System.Drawing.SizeF(tableWidth2,20);
            table2Detail.StyleName= "tableDetailOddStyle";
            table2Detail.Borders = DevExpress.XtraPrinting.BorderSide.All;
            table2DetailRow = new XRTableRow();
            table2DetailRow.Name = "tableDetailRow";
            table2DetailRow.HeightF = 20;
            #endregion

            #region Documento Table footer
            XRTable table2Footer;
            XRTableRow table2FooterRow;
            XRTableCell table2FooterCell;
            table2Footer = new XRTable();
            table2Footer.LocationF = new PointF(tableWidth / 5, 0);
            table2Footer.SizeF = new SizeF(tableWidth2, 30);
            table2Footer.StyleName = "groupFooterStyle";
            table2FooterRow = new XRTableRow();
            table2FooterRow.Name = "table1FooterRow";
            table2FooterRow.HeightF = 30;
            #endregion
            #endregion
            #endregion
            
            #region Table 1
            foreach (string fieldName in footerFieldList)
            {
                #region Documento table header
                table1HeaderCell = new XRTableCell();
                table1HeaderCell.WidthF = (fieldName.Contains("Observacion")) ? columnWidth1 + 50 : columnWidth1;
                table1HeaderCell.BorderColor = Color.White;
                string resourceId = this._docId.ToString() + "_" + fieldName;
                string columnname = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, resourceId);
                table1HeaderCell.Text = columnname;
               
                table1HeaderRow.Controls.Add(table1HeaderCell);
                #endregion

                #region Documento table detail
                table1DetailCell = new XRTableCell();
                table1DetailCell.WidthF = table1HeaderCell.WidthF;
                table1DetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                if (fieldName.Contains("Observacion"))
                    table1DetailCell.DataBindings.Add("Text", this.DataSource, "Footer." + fieldName);
                else if (fieldName.Contains("Fecha"))
                    table1DetailCell.DataBindings.Add("Text", this.DataSource, "Footer." + fieldName, "{0:dd/MM/yyyy}");
                else
                {
                    table1DetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                    table1DetailCell.DataBindings.Add("Text", this.DataSource, "Footer." + fieldName, "{0:#,0.00}");
                }
              
                table1DetailRow.Controls.Add(table1DetailCell);
                #endregion

                #region Documento table footer
                table1FooterCell = new XRTableCell();
                table1FooterCell.WidthF = table1HeaderCell.WidthF;

                if (fieldName.Contains("Observacion"))
                {
                    table1FooterCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                    table1FooterCell.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 10, 0, 0);
                    table1FooterCell.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_Totals");
                }
                else if (fieldName.Contains("Valor"))
                {
                    table1FooterCell.Summary.Func = SummaryFunc.Sum;
                    table1FooterCell.Summary.Running = SummaryRunning.Report;
                    table1FooterCell.Summary.FormatString = "{0:#,0.00}";
                    table1FooterCell.DataBindings.Add("Text", documentoBand.DataSource, "Footer." + fieldName);
                }
                table1FooterRow.Controls.Add(table1FooterCell);
                #endregion
            };
            table1Header.Controls.Add(table1HeaderRow);
            table1Detail.Controls.Add(table1DetailRow);
            table1Footer.Controls.Add(table1FooterRow);

            documentoTable1Header.Controls.Add(table1Header);
            documentoTable1Detail.Controls.Add(table1Detail);
            documentoTable1Footer.Controls.Add(table1Footer);
            #endregion

            #region Table 2
            foreach (string fieldName in detailFieldList)
            {
                #region Documento table header
                table2HeaderCell = new XRTableCell();
                table2HeaderCell.WidthF = (fieldName.Contains("Desc")) ? columnWidth2 + 50 : columnWidth2;
                table2HeaderCell.BorderColor = Color.Black;
                string resourceId = this._docId.ToString() + "_" + fieldName;
                string columnname = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, resourceId);
                table2HeaderCell.Text = columnname;
                table2HeaderRow.Controls.Add(table2HeaderCell);
                #endregion

                #region Documento table detail
                table2DetailCell = new XRTableCell();
                table2DetailCell.WidthF = table2HeaderCell.WidthF;
                table2DetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;

                if (fieldName.Contains("Valor"))
                {
                    table2DetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                    table2DetailCell.DataBindings.Add("Text", this.DataSource, "Detail." + fieldName, "{0:#,0.00}");
                }
                else
                {
                    table2DetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    table2DetailCell.DataBindings.Add("Text", this.DataSource, "Detail." + fieldName);
                }

                table2DetailRow.Controls.Add(table2DetailCell);
                #endregion

                #region Documento table footer
                table2FooterCell = new XRTableCell();
                table2FooterCell.WidthF = table2HeaderCell.WidthF;

                if (fieldName.Contains("Lugar"))
                {
                    table2FooterCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                    table2FooterCell.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 10, 0, 0);
                    table2FooterCell.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_Totals");
                    table2FooterCell.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom;
                    table2FooterCell.BorderWidth = 2;
                }
                else if (fieldName.Contains("Valor"))
                {
                    table2FooterCell.Summary.Func = SummaryFunc.Sum;
                    table2FooterCell.Summary.Running = SummaryRunning.Report;
                    table2FooterCell.Summary.FormatString = "{0:#,0.00}";
                    table2FooterCell.DataBindings.Add("Text", documentoBand.DataSource, "Detail." + fieldName);
                    table2FooterCell.Borders = DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom;
                    table2FooterCell.BorderWidth = 2;
                }
                table2FooterRow.Controls.Add(table2FooterCell);
                #endregion
            };
            table2Header.Controls.Add(table2HeaderRow);
            table2Detail.Controls.Add(table2DetailRow);
            table2Footer.Controls.Add(table2FooterRow);

            documentoTable2Header.Controls.Add(table2Header);
            documentoTable2Detail.Controls.Add(table2Detail);
            documentoTable2Footer.Controls.Add(table2Footer);
            #endregion
            
        } 
        #endregion

    }
}

