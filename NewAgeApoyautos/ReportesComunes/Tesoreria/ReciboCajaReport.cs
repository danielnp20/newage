﻿using System;
using System.Collections.Generic;
using System.Linq;
using NewAge.DTO.Reportes;
using System.Collections;
using DevExpress.XtraReports.UI;
using System.Drawing;
using DevExpress.XtraPrinting.Shape;
using NewAge.Librerias.Project;
using System.Drawing.Printing;

namespace NewAge.ReportesComunes
{
    public class ReciboCajaReport : BaseCommonReport //XtraReport//
    {
        
        #region Funciones Publicas
        /// <summary>
        /// Recibo Caja Constructor
        /// </summary>

        public ReciboCajaReport(int docId ,List<DTO_ReportReciboCaja> documentoData, ArrayList detailFieldList, CommonReportDataSupplier supplier)
            : base(supplier)
        {            

            #region Documento styles

            XRControlStyle headerStyle = new XRControlStyle()
            {
                Name = "groupHeaderStyle",
                BackColor = Color.Transparent,
                ForeColor = Color.Black,
                Font = new Font("Arial", 9, FontStyle.Bold),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0),
            };
            this.StyleSheet.Add(headerStyle);

            XRControlStyle sumFieldStyle = new XRControlStyle()
            {
                Name = "groupFooterStyle",
                BackColor = Color.Transparent,
                ForeColor = Color.Black,
                Font = new Font("Arial", 9, FontStyle.Bold),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                Borders = DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom,
                BorderColor = Color.Gray
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
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight,
                    Borders = DevExpress.XtraPrinting.BorderSide.Left,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0)
                },
                OddStyle = new XRControlStyle()
                {
                    Name = "tableDetailOddStyle",
                    BackColor = Color.White,
                    ForeColor = Color.Black,
                    Font = new Font("Arial", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight,
                    Borders = DevExpress.XtraPrinting.BorderSide.Left,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0)
                },
                Style = new XRControlStyle()
                {
                    Name = "tableHeaderStyle",
                    BackColor = Color.DimGray,
                    ForeColor = Color.White,
                    Font = new Font("Arial", 9, FontStyle.Bold),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                    Borders = DevExpress.XtraPrinting.BorderSide.All,
                    BorderColor = Color.White
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

            GroupHeaderBand documentoTitle = new GroupHeaderBand();
            documentoTitle.HeightF = 200;
            documentoTitle.Level = 2;
            documentoBand.Bands.Add(documentoTitle);

            GroupField documentoGroupField = new GroupField("ReportRompimiento1.GroupFieldValue");
            documentoGroupField.SortOrder = XRColumnSortOrder.Ascending;
            documentoTitle.GroupFields.Add(documentoGroupField);

            GroupHeaderBand documentoHeader = new GroupHeaderBand();
            documentoHeader.HeightF = 150;
            documentoHeader.Level = 1;
            documentoBand.Bands.Add(documentoHeader);

            DetailBand documentDetail;
            documentDetail = new DetailBand();
            documentDetail.HeightF = 0;
            documentoBand.Bands.Add(documentDetail);

            GroupFooterBand documentoFooter = new GroupFooterBand();
            documentoFooter.HeightF = 100;
            documentoFooter.Level = 1;
            documentoFooter.PageBreak = DevExpress.XtraReports.UI.PageBreak.AfterBand;
            documentoBand.Bands.Add(documentoFooter);

            #region Detail Table

            DetailReportBand documentoDetailTableBand;
            documentoDetailTableBand = new DetailReportBand();
            documentoDetailTableBand.DataSource = documentoData;
            documentoDetailTableBand.DataMember = "ReciboDetail";
            documentoDetailTableBand.Level = 0;

            GroupHeaderBand documentoDetailTableHeader = new GroupHeaderBand();
            documentoDetailTableHeader.HeightF = 55;
            documentoDetailTableHeader.Level = 0;
            documentoDetailTableBand.Bands.Add(documentoDetailTableHeader);

            DetailBand documentoDetailTableDetail;
            documentoDetailTableDetail = new DetailBand();
            documentoDetailTableDetail.HeightF = 20;
            documentoDetailTableBand.Bands.Add(documentoDetailTableDetail);

            GroupFooterBand documentoDetailTableFooter = new GroupFooterBand();
            documentoDetailTableFooter.HeightF = 10;
            documentoDetailTableFooter.Level = 0;
            documentoDetailTableBand.Bands.Add(documentoDetailTableFooter);

            documentoBand.Bands.Add(documentoDetailTableBand);
            #endregion

            #region Footer Table
            //DetailReportBand documentoFooterTableBand;
            //documentoFooterTableBand = new DetailReportBand();
            //documentoFooterTableBand.Level = 1;
            ////documentoBand2.DataSource = documentoData;
            ////documentoBand2.DataMember = "FacturaDetail";

            //GroupHeaderBand documentoFooterHeader = new GroupHeaderBand();
            //documentoFooterHeader.HeightF = 30;
            //documentoFooterHeader.Level = 1;
            //documentoFooterTableBand.Bands.Add(documentoFooterHeader);

            //GroupHeaderBand documentoFooterTableHeader = new GroupHeaderBand();
            //documentoFooterTableHeader.HeightF = 30;
            //documentoFooterTableHeader.Level = 0;
            //documentoFooterTableBand.Bands.Add(documentoFooterTableHeader);

            //DetailBand documentoFooterTableDetail;
            //documentoFooterTableDetail = new DetailBand();
            //documentoFooterTableDetail.HeightF = 20;
            //documentoFooterTableBand.Bands.Add(documentoFooterTableDetail);

            //GroupFooterBand documentoFooterTableFooter = new GroupFooterBand();
            //documentoFooterTableFooter.HeightF = 50;
            //documentoFooterTableFooter.Level = 0;
            //documentoFooterTableBand.Bands.Add(documentoFooterTableFooter);

            //documentoBand.Controls.Add(documentoFooterTableBand);
            #endregion

            this.Bands.Add(documentoBand);
            #endregion

            #region Documento field width
            #region Table detail
            float tableDetailWidth = 0;
            float columnDetailWidth = 0;

            tableDetailWidth = this.PageWidth - (this.Margins.Right + this.Margins.Left);

            columnDetailWidth = (tableDetailWidth - 200) / 4; //fieldList.Count;
            #endregion

            #region Table footer
            //float tableFooterWidth = 0;
            //float columnFooterWidth = 0;

            //tableFooterWidth = 3 * tableDetailWidth / 8;

            //columnFooterWidth = (tableFooterWidth - 50) / 3; //fieldList.Count;
            #endregion
            #endregion

            #region Documento elements
            #region Documento title
            this.ReportHeader.HeightF = 0;

            documentoTitle.Controls.Add(this.imgLogoEmpresa);
            documentoTitle.Controls.Add(this.lblNombreEmpresa);
            documentoTitle.Controls.Add(this.lblReportName);

            this.lblNombreEmpresa.LocationF = new PointF(100, 40);
            this.lblReportName.LocationF = new PointF(0, this.lblNombreEmpresa.LocationF.Y + this.lblNombreEmpresa.HeightF + 20);
            this.lblReportName.SizeF = new SizeF(140, 20);
            this.lblReportName.Font = new Font("Arial", 10, FontStyle.Bold);
            this.lblReportName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.lblReportName.Padding = new DevExpress.XtraPrinting.PaddingInfo(100, 0, 0, 0);
            this.lblReportName.Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (docId).ToString() + "_Nit");

            XRLabel lblTitleNit_Value = new XRLabel();
            lblTitleNit_Value.LocationF = new PointF(this.lblReportName.LocationF.X + this.lblReportName.WidthF, this.lblReportName.LocationF.Y);
            lblTitleNit_Value.SizeF = new SizeF(tableDetailWidth - this.lblReportName.WidthF, 20);
            lblTitleNit_Value.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0);
            lblTitleNit_Value.Font = new Font("Arial", 10);
            lblTitleNit_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            lblTitleNit_Value.Text = supplier.GetNitEmpresa();
            documentoTitle.Controls.Add(lblTitleNit_Value);

            XRLabel lblTitleDocumentoName = new XRLabel();
            lblTitleDocumentoName.LocationF = new PointF(0, this.lblReportName.LocationF.Y + this.lblReportName.HeightF + 20);
            lblTitleDocumentoName.SizeF = new SizeF(tableDetailWidth, 35);
            lblTitleDocumentoName.Font = new Font("Arial", 14, FontStyle.Bold);
            lblTitleDocumentoName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            lblTitleDocumentoName.Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (docId).ToString() + "_ReciboDeCaja");
            documentoTitle.Controls.Add(lblTitleDocumentoName);

            #region Table Comprobante/ReciboNro
            XRTable tableTitleNroDoc;
            XRTableRow tableTitleNroDocRow;
            XRTableCell tableTitleNroDocCell;
            tableTitleNroDoc = new XRTable();
            tableTitleNroDoc.BeginInit();
            tableTitleNroDoc.LocationF = new PointF(2 * tableDetailWidth / 3, lblTitleDocumentoName.LocationF.Y + lblTitleDocumentoName.HeightF + 5);
            tableTitleNroDoc.SizeF = new SizeF(tableDetailWidth / 3, 50);

            tableTitleNroDocRow = new XRTableRow();
            tableTitleNroDocRow.HeightF = tableTitleNroDoc.HeightF/2;

            tableTitleNroDocCell = new XRTableCell();
            tableTitleNroDocCell.WidthF = tableTitleNroDoc.WidthF / 2 - 20;
            tableTitleNroDocCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            tableTitleNroDocCell.Font = new Font("Arial", 10, FontStyle.Bold);
            tableTitleNroDocCell.Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (docId).ToString() + "_Nro");
            tableTitleNroDocRow.Cells.Add(tableTitleNroDocCell);

            tableTitleNroDocCell = new XRTableCell();
            tableTitleNroDocCell.WidthF = tableTitleNroDoc.WidthF / 8;
            tableTitleNroDocCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            tableTitleNroDocCell.Font = new Font("Arial", 10, FontStyle.Regular);
            tableTitleNroDocCell.DataBindings.Add("Text", this.DataSource, "CajaID");
            tableTitleNroDocRow.Cells.Add(tableTitleNroDocCell);

            tableTitleNroDocCell = new XRTableCell();
            tableTitleNroDocCell.WidthF = 20;
            tableTitleNroDocCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            tableTitleNroDocCell.Font = new Font("Arial", 10, FontStyle.Regular);
            tableTitleNroDocCell.Text = "-";
            tableTitleNroDocRow.Cells.Add(tableTitleNroDocCell);

            tableTitleNroDocCell = new XRTableCell();
            tableTitleNroDocCell.WidthF = tableTitleNroDoc.WidthF / 8;
            tableTitleNroDocCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            tableTitleNroDocCell.Font = new Font("Arial", 10, FontStyle.Regular);
            tableTitleNroDocCell.DataBindings.Add("Text", this.DataSource, "ReciboNro");
            tableTitleNroDocRow.Cells.Add(tableTitleNroDocCell);

            tableTitleNroDoc.Rows.Add(tableTitleNroDocRow);

            tableTitleNroDocRow = new XRTableRow();
            tableTitleNroDocRow.HeightF = tableTitleNroDoc.HeightF/2;

            tableTitleNroDocCell = new XRTableCell();
            tableTitleNroDocCell.WidthF = tableTitleNroDoc.WidthF / 2 - 20;
            tableTitleNroDocCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            tableTitleNroDocCell.Font = new Font("Arial", 10, FontStyle.Bold);
            tableTitleNroDocCell.Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (docId).ToString() + "_Comprobante");
            tableTitleNroDocRow.Cells.Add(tableTitleNroDocCell);

            tableTitleNroDocCell = new XRTableCell();
            tableTitleNroDocCell.WidthF = tableTitleNroDoc.WidthF / 8;
            tableTitleNroDocCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            tableTitleNroDocCell.Font = new Font("Arial", 10, FontStyle.Regular);
            tableTitleNroDocCell.DataBindings.Add("Text", this.DataSource, "ComprobanteID");
            tableTitleNroDocRow.Cells.Add(tableTitleNroDocCell);

            tableTitleNroDocCell = new XRTableCell();
            tableTitleNroDocCell.WidthF = 20;
            tableTitleNroDocCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            tableTitleNroDocCell.Font = new Font("Arial", 10, FontStyle.Regular);
            tableTitleNroDocCell.Text = "-";
            tableTitleNroDocRow.Cells.Add(tableTitleNroDocCell);

            tableTitleNroDocCell = new XRTableCell();
            tableTitleNroDocCell.WidthF = tableTitleNroDoc.WidthF / 8;
            tableTitleNroDocCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            tableTitleNroDocCell.Font = new Font("Arial", 10, FontStyle.Regular);
            tableTitleNroDocCell.DataBindings.Add("Text", this.DataSource, "ComprobanteNro");
            tableTitleNroDocRow.Cells.Add(tableTitleNroDocCell);

            tableTitleNroDoc.Rows.Add(tableTitleNroDocRow);

            tableTitleNroDoc.EndInit();
            documentoTitle.Controls.Add(tableTitleNroDoc);
            #endregion
            #endregion

            #region Documento header
            XRTable documentoHeaderTable;
            XRTableRow documentoHeaderTableRow;
            XRTableCell documentoHeaderTableCell_Name;
            XRTableCell documentoHeaderTableCell_Value;

            documentoHeaderTable = new XRTable();
            documentoHeaderTable.BeginInit();
            documentoHeaderTable.LocationF = new PointF(0, 20);
            documentoHeaderTable.SizeF = new SizeF(tableDetailWidth, 120);
            documentoHeaderTable.StyleName = "groupHeaderStyle";

            #region Row 1
            documentoHeaderTableRow = new XRTableRow();
            documentoHeaderTableRow.HeightF = documentoHeaderTable.HeightF / 4;

            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = tableDetailWidth / 6;
            documentoHeaderTableCell_Name.Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms,(docId).ToString() + "_Fecha");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = 3 * tableDetailWidth / 6;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 9);
            //documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "Fecha","{0:dd/MM/yyyy}");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = tableDetailWidth / 6;
            documentoHeaderTableCell_Name.Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (docId).ToString() + "_Valor$");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = tableDetailWidth / 6;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 9);
            //documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "Valor", "{0:#,0.00}");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTable.Rows.Add(documentoHeaderTableRow);
            #endregion

            #region Row 2

            documentoHeaderTableRow = new XRTableRow();
            documentoHeaderTableRow.HeightF = documentoHeaderTable.HeightF / 4;

            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = tableDetailWidth / 6;
            documentoHeaderTableCell_Name.Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (docId).ToString() + "_RecibidoDe");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = 3 * tableDetailWidth / 6;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 9);
            //documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "TerceroDesc");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = tableDetailWidth / 6;
            documentoHeaderTableCell_Name.Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (docId).ToString() + "_Nit");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = tableDetailWidth / 6;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 9);
            //documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "TerceroID");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTable.Rows.Add(documentoHeaderTableRow);
            #endregion

            #region Row 3

            documentoHeaderTableRow = new XRTableRow();
            documentoHeaderTableRow.HeightF = documentoHeaderTable.HeightF / 4;

            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = tableDetailWidth / 6;
            documentoHeaderTableCell_Name.Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (docId).ToString() + "_PorConceptoDe");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = 5 * tableDetailWidth / 6;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 9);
            //documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "DocumentoDesc");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTable.Rows.Add(documentoHeaderTableRow);
            #endregion

            #region Row 4

            documentoHeaderTableRow = new XRTableRow();
            documentoHeaderTableRow.HeightF = documentoHeaderTable.HeightF / 4;

            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = tableDetailWidth / 6;
            documentoHeaderTableCell_Name.Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (docId).ToString() + "_LaSumaDe");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = 5 * tableDetailWidth / 6;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 9);
            documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "Valor_letters");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTable.Rows.Add(documentoHeaderTableRow);
            #endregion

            documentoHeaderTable.EndInit();
            documentoHeader.Controls.Add(documentoHeaderTable);
            #endregion

            #region Detail Table
            #region Documento detail Table header
            XRTable detailTableHeader;
            XRTableRow detailTableHeaderRow;
            XRTableCell detailTableHeaderCell;
            detailTableHeader = new XRTable();
            detailTableHeader.LocationF = new PointF(0, 20);
            detailTableHeader.WidthF = tableDetailWidth;
            detailTableHeader.HeightF = 30;
            detailTableHeader.StyleName = "tableHeaderStyle";
            detailTableHeaderRow = new XRTableRow();
            #endregion

            #region Documento detail  Table detail

            XRTable detailTableDetail;
            XRTableRow detailTableDetailRow;
            XRTableCell detailTableDetailCell;
            detailTableDetail = new XRTable();
            detailTableDetail.WidthF = tableDetailWidth;
            detailTableDetail.HeightF = 20; 
            detailTableDetail.OddStyleName = "tableDetailOddStyle";
            detailTableDetail.EvenStyleName = "tableDetailEvenStyle";
            detailTableDetailRow = new XRTableRow();
            detailTableDetailRow.Name = "tableDetailRow";
            detailTableDetailRow.HeightF = 20;
            #endregion

            #region Documento detail Table footer
            
            
            XRLine detailFooterLine = new XRLine()
            {
                LineWidth = 1,
                ForeColor = Color.Gray,
                SizeF = new SizeF(tableDetailWidth, 1),
                LocationF = new PointF(0, 0)
            };
            documentoDetailTableFooter.Controls.Add(detailFooterLine);

            #endregion
            #endregion

            #region Footer Table
            #region Documento footer header
            //XRLabel lblFooterHeader = new XRLabel();
            //lblFooterHeader.LocationF = new PointF(0, 0);
            //lblFooterHeader.SizeF = new SizeF(2 * tableFooterWidth, documentoFooterHeader.HeightF);
            //lblFooterHeader.Font = new Font("Arial", 10, FontStyle.Bold);
            //lblFooterHeader.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            //lblFooterHeader.Text = "FORMA DE PAGO";
            //documentoFooterHeader.Controls.Add(lblFooterHeader);
            #endregion

            #region Documento  footer Table header
            //XRTable footerTable1Header;
            //XRTableRow footerTable1HeaderRow;
            //XRTableCell footerTable1HeaderCell;
            //footerTable1Header = new XRTable();
            //footerTable1Header.LocationF = new PointF(0, 0);
            //footerTable1Header.WidthF = tableFooterWidth;
            //footerTable1Header.HeightF = documentoFooterTableHeader.HeightF - 5;
            //footerTable1Header.StyleName = "tableHeaderStyle";
            //footerTable1HeaderRow = new XRTableRow();

            //XRTable footerTable2Header;
            //XRTableRow footerTable2HeaderRow;
            //XRTableCell footerTable2HeaderCell;
            //footerTable2Header = new XRTable();
            //footerTable2Header.LocationF = new PointF(tableFooterWidth, 0);
            //footerTable2Header.WidthF = tableFooterWidth;
            //footerTable2Header.HeightF = documentoFooterTableHeader.HeightF - 5;
            //footerTable2Header.StyleName = "tableHeaderStyle";
            //footerTable2HeaderRow = new XRTableRow();
            #endregion

            #region Documento footer Table detail
            //XRTable footerTable1Detail;
            //XRTableRow footerTable1DetailRow;
            //XRTableCell footerTable1DetailCell;
            //footerTable1Detail = new XRTable();
            //footerTable1Header.LocationF = new PointF(0, 0);
            //footerTable1Detail.WidthF = tableFooterWidth;
            //footerTable1Detail.HeightF = 20; // documentoTableDetail.HeightF;
            //footerTable1Detail.OddStyleName = "tableDetailOddStyle";
            //footerTable1Detail.EvenStyleName = "tableDetailEvenStyle";
            //footerTable1DetailRow = new XRTableRow();
            //footerTable1DetailRow.Name = "tableDetailRow";
            //footerTable1DetailRow.HeightF = 20;

            //XRTable footerTable2Detail;
            //XRTableRow footerTable2DetailRow;
            //XRTableCell footerTable2DetailCell;
            //footerTable2Detail = new XRTable();
            //footerTable2Detail.LocationF = new PointF(tableFooterWidth, 0);
            //footerTable2Detail.WidthF = tableFooterWidth;
            //footerTable2Detail.HeightF = 20; // documentoTableDetail.HeightF;
            //footerTable2Detail.OddStyleName = "tableDetailOddStyle";
            //footerTable2Detail.EvenStyleName = "tableDetailEvenStyle";
            //footerTable2DetailRow = new XRTableRow();
            //footerTable2DetailRow.Name = "tableDetailRow";
            //footerTable2DetailRow.HeightF = 20;
            #endregion

            #region Documento footer Table footer
            //XRLine footerFooterLine = new XRLine()
            //{
            //    LineWidth = 1,
            //    ForeColor = Color.Gray,
            //    SizeF = new SizeF(2 * tableFooterWidth, 1),
            //    LocationF = new PointF(0, 0)
            //};
            //documentoFooterTableFooter.Controls.Add(footerFooterLine);

            //XRLabel lblTesoreria_blank = new XRLabel();
            //lblTesoreria_blank.LocationF = new PointF(2 * tableFooterWidth + 30, 5);
            //lblTesoreria_blank.SizeF = new SizeF(tableDetailWidth / 5, 30);
            //lblTesoreria_blank.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            //documentoFooter.Controls.Add(lblTesoreria_blank);

            //XRLabel lblTesoreria = new XRLabel();
            //lblTesoreria.LocationF = new PointF(lblTesoreria_blank.LocationF.X, lblTesoreria_blank.LocationF.Y + lblTesoreria_blank.HeightF);
            //lblTesoreria.SizeF = new SizeF(tableDetailWidth / 5, 25);
            //lblTesoreria.Font = new Font("Arial", 9, FontStyle.Bold | FontStyle.Italic);
            //lblTesoreria.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            //lblTesoreria.Text = "TESORERIA";//_bc.GetResource(Librerias.Project.LanguageTypes.Forms, (docId).ToString() + "_Tesoreria");
            //documentoFooter.Controls.Add(lblTesoreria);
            #endregion
            #endregion

            #region Documento Footer
            XRLabel lblTesoreria_blank = new XRLabel();
            lblTesoreria_blank.SizeF = new System.Drawing.SizeF(columnDetailWidth, 10);
            lblTesoreria_blank.LocationF = new PointF(tableDetailWidth - columnDetailWidth - 30 , 50);
            lblTesoreria_blank.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            lblTesoreria_blank.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            lblTesoreria_blank.DataBindings.Add("Text", this.DataSource, "Usuario");
            documentoFooter.Controls.Add(lblTesoreria_blank);

            XRLabel lblTesoreria = new XRLabel();
            lblTesoreria.LocationF = new PointF(lblTesoreria_blank.LocationF.X, lblTesoreria_blank.LocationF.Y + lblTesoreria_blank.HeightF);
            lblTesoreria.SizeF = new System.Drawing.SizeF(columnDetailWidth, 25);
            lblTesoreria.Font = new System.Drawing.Font("Arial", 10,FontStyle.Italic);
            lblTesoreria.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            lblTesoreria.Text = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (docId).ToString() + "_Tesoreria");
            documentoFooter.Controls.Add(lblTesoreria);
            #endregion

            #endregion

            #region Detail Table
            foreach (string fieldName in detailFieldList)
            {
                #region Documento table header
                detailTableHeaderCell = new XRTableCell();
                detailTableHeaderCell.WidthF = (fieldName.Contains("Desc")) ? columnDetailWidth + 200 : columnDetailWidth;
                detailTableHeaderCell.BorderColor = Color.White;
                string resourceId = (docId).ToString() + "_" + fieldName;
                string columnname = supplier.GetResource(Librerias.Project.LanguageTypes.Forms, resourceId);
                detailTableHeaderCell.Text = columnname;  
                detailTableHeaderRow.Controls.Add(detailTableHeaderCell);
                #endregion

                #region Documento table detail
                detailTableDetailCell = new XRTableCell();
                detailTableDetailCell.WidthF = detailTableHeaderCell.WidthF;

                if (fieldName.Contains("Valor"))
                {
                    detailTableDetailCell.DataBindings.Add("Text", this.DataSource, "ReciboDetail." + fieldName, "{0:#,0.00}");
                    detailTableDetailCell.Borders = DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Left;
                }
                else
                {
                    if (fieldName.Contains("Desc")) detailTableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                    detailTableDetailCell.DataBindings.Add("Text", this.DataSource, "ReciboDetail." + fieldName);
                };

                detailTableDetailRow.Controls.Add(detailTableDetailCell);
                #endregion
            };
                        
            detailTableHeader.Controls.Add(detailTableHeaderRow);
            detailTableDetail.Controls.Add(detailTableDetailRow);

            documentoDetailTableHeader.Controls.Add(detailTableHeader);
            documentoDetailTableDetail.Controls.Add(detailTableDetail);
            #endregion

            #region Footer Table
            //foreach (string fieldName in footerFieldList)
            //{
                #region Documento table header
                //footerTable1HeaderCell = new XRTableCell();
                //footerTable1HeaderCell.WidthF = columnFooterWidth;
                //footerTable1HeaderCell.BorderColor = Color.White;
                ////string resourceId = (AppReports.FacturaEquivalente).ToString() + "_" + fieldName;
                ////string columnname = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, resourceId);
                ////footerTable1HeaderCell.Text = columnname;
                //footerTable1HeaderCell.Text = fieldName;
                //footerTable1HeaderRow.Controls.Add(footerTable1HeaderCell);

                //footerTable2HeaderCell = new XRTableCell();
                //footerTable2HeaderCell.WidthF = columnFooterWidth;
                //footerTable2HeaderCell.BorderColor = Color.White;
                //footerTable2HeaderCell.Text = footerTable1HeaderCell.Text;
                //footerTable2HeaderRow.Controls.Add(footerTable2HeaderCell);
                #endregion

                #region Documento table detail
                //footerTable1DetailCell = new XRTableCell();
                //footerTable1DetailCell.WidthF = footerTable1HeaderCell.WidthF;
                //if (fieldName.Contains("Valor"))
                //{
                //    //footerTable1DetailCell.DataBindings.Add("Text", this.DataSource, "ReciboFooter." + fieldName + "1", "{0:#,0.00}");
                //}
                //else
                //{
                //    footerTable1DetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                //    footerTable1DetailCell.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 0, 0, 0);
                //    //footerTable1DetailCell.DataBindings.Add("Text", this.DataSource, "ReciboFooter." + fieldName + "1");
                //};
                //footerTable1DetailRow.Controls.Add(footerTable1DetailCell);

                //footerTable2DetailCell = new XRTableCell();
                //footerTable2DetailCell.WidthF = footerTable1HeaderCell.WidthF;
                //if (fieldName.Contains("Valor"))
                //{
                //    //footerTable2DetailCell.DataBindings.Add("Text", this.DataSource, "ReciboFooter." + fieldName + "2", "{0:#,0.00}");

                //    footerTable2DetailCell.Borders = DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Left;
                //}
                //else
                //{
                //    footerTable2DetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                //    footerTable2DetailCell.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 0, 0, 0);
                //    //footerTable2DetailCell.DataBindings.Add("Text", this.DataSource, "ReciboFooter." + fieldName + "2");
                //};
                //footerTable2DetailRow.Controls.Add(footerTable2DetailCell);
                #endregion
            //};
            //footerTable1Header.Controls.Add(footerTable1HeaderRow);
            //footerTable1Detail.Controls.Add(footerTable1DetailRow);
            //footerTable2Header.Controls.Add(footerTable2HeaderRow);
            //footerTable2Detail.Controls.Add(footerTable2DetailRow);

            //documentoFooterTableHeader.Controls.Add(footerTable1Header);
            //documentoFooterTableDetail.Controls.Add(footerTable1Detail);
            //documentoFooterTableHeader.Controls.Add(footerTable2Header);
            //documentoFooterTableDetail.Controls.Add(footerTable2Detail);
            #endregion
        } 
        #endregion
    }
}