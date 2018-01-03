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
    public partial class AutorizacionDeGiroReport : BaseCommonReport
    {
        #region Funciones Publicas
        /// <summary>
        /// Autorizacion De Giro Report Constructor
        /// </summary>
        /// <param name="docId">Report ID (from AppReport)</param>
        /// <param name="documentoData">data for the report</param>
        /// <param name="fieldList">list of fields for report detail table</param>
        /// <param name="supplier"> Interface que provee de informacion a un reporte comun</param>
        public AutorizacionDeGiroReport(int docID, List<DTO_ReportAutorizacionDeGiro> documentoData, ArrayList fieldList, bool multiMoneda, bool estadoInd, CommonReportDataSupplier supplier)
            : base(supplier)
        {
            this.lblReportName.Text = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString());

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

            XRControlStyle sumFieldStyle = new XRControlStyle()
            {
                Name = "groupFooterStyle",
                BackColor = Color.Transparent,
                ForeColor = Color.Black,
                Font = new Font("Arial", 9, FontStyle.Bold),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight,
                Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0)
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
                    BackColor = Color.White,
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
            this.Margins = new System.Drawing.Printing.Margins(50, 50, 50, 50);

            DetailReportBand documentoBand;
            documentoBand = new DetailReportBand();
            documentoBand.DataSource = documentoData;
            documentoBand.DataMember = "AutorizacionDetail";

            //GroupHeaderBand documentoTitle = new GroupHeaderBand();
            //documentoTitle.HeightF = 25;
            //documentoTitle.Level = 2;
            //documentoBand.Bands.Add(documentoTitle);

            GroupHeaderBand documentoHeader = new GroupHeaderBand();
            documentoHeader.HeightF = 150;
            documentoHeader.Level = 1;
            documentoBand.Bands.Add(documentoHeader);

            GroupHeaderBand documentoTableHeader = new GroupHeaderBand();
            documentoTableHeader.HeightF = 55;
            documentoTableHeader.Level = 0;
            documentoBand.Bands.Add(documentoTableHeader);

            DetailBand documentoTableDetail;
            documentoTableDetail = new DetailBand();
            documentoTableDetail.HeightF = 20;
            documentoBand.Bands.Add(documentoTableDetail);

            GroupFooterBand documentoTableFooter = new GroupFooterBand();
            documentoTableFooter.HeightF = 275;
            documentoTableFooter.Level = 0;
            documentoBand.Bands.Add(documentoTableFooter);

            GroupFooterBand documentoFooter = new GroupFooterBand();
            documentoFooter.HeightF = 45;
            documentoFooter.Level = 1;
            documentoBand.Bands.Add(documentoFooter);

            this.Bands.Add(documentoBand);
            #endregion

            #region Documento field width
            float tableWidth = 0;
            float columnWidth = 0;

            tableWidth = this.PageWidth - (this.Margins.Right + this.Margins.Left);

            columnWidth = (tableWidth - 50) / 8; //fieldList.Count;
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

            float documentoHeaderTableCellWidth_Caption = (tableWidth - 2) / 8;
            float documentoHeaderTableCellWidth_Narrow = documentoHeaderTableCellWidth_Caption;
            float documentoHeaderTableCellWidth_Wide = 3 * (tableWidth - 2) / 8;


            XRTable documentoHeaderTable;
            XRTableRow documentoHeaderTableRow;
            XRTableCell documentoHeaderTableCell_Name;
            XRTableCell documentoHeaderTableCell_Value;

            documentoHeaderTable = new XRTable();
            documentoHeaderTable.BeginInit();
            documentoHeaderTable.LocationF = new PointF(headerFrame.LocationF.X + 1, headerFrame.LocationF.Y + 1);
            documentoHeaderTable.SizeF = new SizeF(headerFrame.WidthF - 2, headerFrame.Height - 2);
            documentoHeaderTable.StyleName = "groupHeaderStyle";

            #region Row 1
            documentoHeaderTableRow = new XRTableRow();
            documentoHeaderTableRow.HeightF = documentoHeaderTable.HeightF / 4;

            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = documentoHeaderTableCellWidth_Caption;
            //documentoHeaderTableCell_Name.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            //documentoHeaderTableCell_Name.BorderColor = Color.Black;
            //documentoHeaderTableCell_Name.BorderWidth = 1;
            documentoHeaderTableCell_Name.Text = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_AFavorDe");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = documentoHeaderTableCellWidth_Wide;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 9);
            documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "TerceroDesc");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = documentoHeaderTableCellWidth_Caption;
            //documentoHeaderTableCell_Name.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            //documentoHeaderTableCell_Name.BorderColor = Color.Black;
            //documentoHeaderTableCell_Name.BorderWidth = 1;
            documentoHeaderTableCell_Name.Text = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Nit");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = 2 * documentoHeaderTableCellWidth_Narrow + documentoHeaderTableCellWidth_Caption;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 9);
            documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "TerceroID");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            //documentoHeaderTableCell_Name = new XRTableCell();
            //documentoHeaderTableCell_Name.WidthF = documentoHeaderTableCellWidth_Caption;
            //documentoHeaderTableCell_Name.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            //documentoHeaderTableCell_Name.BorderColor = Color.Black;
            //documentoHeaderTableCell_Name.BorderWidth = 1;
            //documentoHeaderTableCell_Name.Text = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_FechaDigt");
            //documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            //documentoHeaderTableCell_Value = new XRTableCell();
            //documentoHeaderTableCell_Value.WidthF = documentoHeaderTableCellWidth_Narrow;
            //documentoHeaderTableCell_Value.Font = new Font("Arial", 9);
            //documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            ////documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "");
            //documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTable.Rows.Add(documentoHeaderTableRow);
            #endregion

            #region Row 2

            documentoHeaderTableRow = new XRTableRow();
            documentoHeaderTableRow.HeightF = documentoHeaderTable.HeightF / 4;

            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = documentoHeaderTableCellWidth_Caption;
            //documentoHeaderTableCell_Name.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            //documentoHeaderTableCell_Name.BorderColor = Color.Black;
            //documentoHeaderTableCell_Name.BorderWidth = 1;
            documentoHeaderTableCell_Name.Text = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_FacturaNo");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = documentoHeaderTableCellWidth_Narrow;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 9);
            documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "DocumentoTercero");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = documentoHeaderTableCellWidth_Caption;
            //documentoHeaderTableCell_Name.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            //documentoHeaderTableCell_Name.BorderColor = Color.Black;
            //documentoHeaderTableCell_Name.BorderWidth = 1;
            documentoHeaderTableCell_Name.Text = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Descripcion");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = tableWidth - 2 - 2 * documentoHeaderTableCellWidth_Caption - documentoHeaderTableCellWidth_Narrow;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 9);
            documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "Descripcion");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTable.Rows.Add(documentoHeaderTableRow);
            #endregion

            #region Row 3

            documentoHeaderTableRow = new XRTableRow();
            documentoHeaderTableRow.HeightF = documentoHeaderTable.HeightF / 4;

            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = documentoHeaderTableCellWidth_Caption;
            //documentoHeaderTableCell_Name.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            //documentoHeaderTableCell_Name.BorderColor = Color.Black;
            //documentoHeaderTableCell_Name.BorderWidth = 1;
            documentoHeaderTableCell_Name.Text = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_FechaFact");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = documentoHeaderTableCellWidth_Narrow;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 9);
            documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "FechaFact", "{0:dd/MM/yyyy}");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = documentoHeaderTableCellWidth_Caption;
            //documentoHeaderTableCell_Name.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            //documentoHeaderTableCell_Name.BorderColor = Color.Black;
            //documentoHeaderTableCell_Name.BorderWidth = 1;
            documentoHeaderTableCell_Name.Text = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_FechaDePago");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = documentoHeaderTable.WidthF - documentoHeaderTableCellWidth_Narrow - 2 * documentoHeaderTableCellWidth_Caption;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 9);
            documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "FechaVto", "{0:dd/MM/yyyy}");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTable.Rows.Add(documentoHeaderTableRow);
            #endregion

            #region Row 4
            documentoHeaderTableRow = new XRTableRow();
            documentoHeaderTableRow.HeightF = documentoHeaderTable.HeightF / 4;

            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = documentoHeaderTableCellWidth_Caption;
            //documentoHeaderTableCell_Name.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            //documentoHeaderTableCell_Name.BorderColor = Color.Black;
            //documentoHeaderTableCell_Name.BorderWidth = 1;
            documentoHeaderTableCell_Name.Text = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = documentoHeaderTableCellWidth_Narrow;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 9);
            documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "Fecha", "{0:dd/MM/yyyy}");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = documentoHeaderTableCellWidth_Caption;
            //documentoHeaderTableCell_Name.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            //documentoHeaderTableCell_Name.BorderColor = Color.Black;
            //documentoHeaderTableCell_Name.BorderWidth = 1;
            documentoHeaderTableCell_Name.Text = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Comprobante");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = documentoHeaderTableCellWidth_Narrow / 2;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 9);
            documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "ComprobanteID");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = documentoHeaderTableCellWidth_Narrow / 2;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 9);
            documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            documentoHeaderTableCell_Value.Text = "-";
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = documentoHeaderTableCellWidth_Wide - documentoHeaderTableCellWidth_Narrow;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 9);
            documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "ComprobanteNro");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = documentoHeaderTableCellWidth_Caption;
            //documentoHeaderTableCell_Name.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            //documentoHeaderTableCell_Name.BorderColor = Color.Black;
            //documentoHeaderTableCell_Name.BorderWidth = 1;
            documentoHeaderTableCell_Name.Text = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_TRM");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = documentoHeaderTableCellWidth_Narrow;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 9);
            documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "TasaCambio", (multiMoneda)?"{0:#,0.00}":"{0:*}");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTable.Rows.Add(documentoHeaderTableRow);
            #endregion

            documentoHeaderTable.EndInit();
            documentoHeader.Controls.Add(documentoHeaderTable);
            #endregion

            #region Documento Table header
            XRTable tableHeader;
            XRTableRow tableHeaderRow;
            XRTableCell tableHeaderCell;
            tableHeader = new XRTable();
            tableHeader.LocationF = new PointF(0, 20);
            tableHeader.WidthF = tableWidth;
            tableHeader.HeightF = documentoTableHeader.HeightF - 25;
            tableHeader.StyleName = "tableHeaderStyle";
            tableHeaderRow = new XRTableRow();
            #endregion

            #region Documento Table detail

            XRTable tableDetail;
            XRTableRow tableDetailRow;
            XRTableCell tableDetailCell;
            tableDetail = new XRTable();
            tableDetail.LocationF = new PointF(0, 0);
            tableDetail.WidthF = tableWidth;
            tableDetail.HeightF = 20; // documentoTableDetail.HeightF;
            tableDetail.OddStyleName = "tableDetailOddStyle";
            tableDetail.EvenStyleName = "tableDetailEvenStyle";
            tableDetailRow = new XRTableRow();
            tableDetailRow.Name = "tableDetailRow";
            tableDetailRow.HeightF = 20;
            #endregion

            #region Documento Table footer
            XRLine footerLine_1 = new XRLine()
            {
                LineWidth = 1,
                SizeF = new SizeF(tableWidth, 1),
                LocationF = new PointF(0, 2)
            };
            documentoTableFooter.Controls.Add(footerLine_1);

            XRTable tableFooter;
            XRTableRow tableFooterRow;
            XRTableCell tableFooterCell_Name;
            XRTableCell tableFooterCell_Value;
            tableFooter = new XRTable();
            tableFooter.LocationF = new PointF(0, footerLine_1.LocationF.Y + footerLine_1.HeightF);
            tableFooter.SizeF = new SizeF(tableWidth, 30);
            tableFooter.StyleName = "groupFooterStyle";
            tableFooterRow = new XRTableRow();

            XRLine footerLine_2 = new XRLine()
            {
                LineWidth = 1,
                SizeF = new SizeF(tableWidth, 1),
                LocationF = new PointF(0, tableFooter.LocationF.Y + tableFooter.HeightF)
            };
            documentoTableFooter.Controls.Add(footerLine_2);

            XRTable totalFooter;
            XRTableRow totalFooterRow;
            XRTableCell totalFooterCell_Name;
            XRTableCell totalFooterCell_ValueML;
            XRTableCell totalFooterCell_ValueME;
            totalFooter = new XRTable();
            totalFooter.BeginInit();
            totalFooter.LocationF = new PointF((multiMoneda) ? tableWidth - 3 * columnWidth : tableWidth - 2 * columnWidth, footerLine_2.LocationF.Y + 10);
            totalFooter.SizeF = new SizeF((multiMoneda) ? 3 * columnWidth : 2 * columnWidth, 8 * 25);
            totalFooter.StyleName = "groupFooterStyle";

            XRTable totalFooterNeto;
            XRTableRow totalFooterNetoRow;
            XRTableCell totalFooterNetoCell_Name;
            XRTableCell totalFooterNetoCell_ValueML;
            XRTableCell totalFooterNetoCell_ValueME;
            totalFooterNeto = new XRTable();
            totalFooterNeto.BeginInit();
            totalFooterNeto.LocationF = new PointF(totalFooter.LocationF.X, totalFooter.LocationF.Y + totalFooter.HeightF + 2);
            totalFooterNeto.SizeF = new SizeF(totalFooter.SizeF.Width, 35);
            totalFooterNeto.StyleName = "groupFooterStyle";
            totalFooterNeto.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            totalFooterNeto.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            totalFooterNetoRow = new XRTableRow();
            totalFooterNetoCell_Name = new XRTableCell();
            totalFooterNetoCell_Name.WidthF = columnWidth;
            totalFooterNetoCell_ValueML = new XRTableCell();
            totalFooterNetoCell_ValueML.WidthF = columnWidth;
            totalFooterNetoCell_ValueML.Font = new System.Drawing.Font("Arial", 9);
            totalFooterNetoCell_ValueME = new XRTableCell();
            totalFooterNetoCell_ValueME.WidthF = columnWidth;
            totalFooterNetoCell_ValueME.Font = new System.Drawing.Font("Arial", 9);
            #endregion

            #region Documento footer
            XRLabel lblSolicita_blank = new XRLabel();
            lblSolicita_blank.LocationF = new PointF(30, 5);
            lblSolicita_blank.SizeF = new System.Drawing.SizeF(columnWidth, 10);
            lblSolicita_blank.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            documentoFooter.Controls.Add(lblSolicita_blank);

            XRLabel lblSolicita = new XRLabel();
            lblSolicita.LocationF = new PointF(lblSolicita_blank.LocationF.X, lblSolicita_blank.LocationF.Y + lblSolicita_blank.HeightF);
            lblSolicita.SizeF = new System.Drawing.SizeF(columnWidth, 25);
            lblSolicita.Font = new System.Drawing.Font("Arial", 10);
            lblSolicita.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            lblSolicita.Text = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Solicita");
            documentoFooter.Controls.Add(lblSolicita);

            XRLabel lblAprovado_blank = new XRLabel();
            lblAprovado_blank.LocationF = new PointF(lblSolicita_blank.LocationF.X + lblSolicita_blank.WidthF + 30, lblSolicita_blank.LocationF.Y);
            lblAprovado_blank.SizeF = new System.Drawing.SizeF(columnWidth, 10);
            lblAprovado_blank.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            documentoFooter.Controls.Add(lblAprovado_blank);

            XRLabel lblAprovado = new XRLabel();
            lblAprovado.LocationF = new PointF(lblAprovado_blank.LocationF.X, lblAprovado_blank.LocationF.Y + lblAprovado_blank.HeightF);
            lblAprovado.SizeF = new System.Drawing.SizeF(columnWidth, 25);
            lblAprovado.Font = new System.Drawing.Font("Arial", 10);
            lblAprovado.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            lblAprovado.Text = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Aprovado");
            documentoFooter.Controls.Add(lblAprovado);
            #endregion
            #endregion

            #region Documento Table
            int TotlasInd = 0;
            float totalsFieldLocation = 0;

            foreach (string fieldName in fieldList)
            {
                #region Documento table header
                tableHeaderCell = new XRTableCell();
                tableHeaderCell.WidthF = (fieldName.Contains("Desc")) ? columnWidth + 50 : columnWidth;
                tableHeaderCell.BorderColor = Color.White;
                if (fieldName.Contains("ValorML")) tableHeaderCell.Borders = DevExpress.XtraPrinting.BorderSide.Left;
                string resourceId = docID.ToString() + "_" + fieldName;
                string columnname = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, resourceId);
                tableHeaderCell.Text = columnname;

                tableHeaderRow.Controls.Add(tableHeaderCell);
                #endregion

                #region Documento table detail
                tableDetailCell = new XRTableCell();
                tableDetailCell.WidthF = tableHeaderCell.WidthF;
                if (fieldName.Contains("ValorML")) tableDetailCell.Borders = DevExpress.XtraPrinting.BorderSide.Left;

                if (fieldName.Contains("ML"))
                {
                    tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                    tableDetailCell.DataBindings.Add("Text", documentoData, "AutorizacionDetail." + fieldName , "{0:#,0.00}");
                }
                else
                {
                    if (!fieldName.Contains("CuentaID") && !fieldName.Contains("Percent")) tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                    tableDetailCell.DataBindings.Add("Text", documentoData, "AutorizacionDetail." + fieldName);
                };
                tableDetailRow.Controls.Add(tableDetailCell);
                #endregion

                #region Documento table footer
                if (fieldName.Contains("Valor"))
                {
                    if (TotlasInd == 0)
                    {
                        tableFooterCell_Name = new XRTableCell()
                        {
                            Text = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_SumasIguales"),
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
                    tableFooterCell_Value.DataBindings.Add("Text", documentoData, "AutorizacionDetail." + fieldName);
                    tableFooterRow.Cells.Add(tableFooterCell_Value);
                };

                if (TotlasInd == 0)
                {
                    totalsFieldLocation += tableHeaderCell.WidthF;
                };
                #endregion

            };
            tableHeader.Controls.Add(tableHeaderRow);
            tableDetail.Controls.Add(tableDetailRow);
            tableFooter.Controls.Add(tableFooterRow);

            documentoTableHeader.Controls.Add(tableHeader);
            documentoTableDetail.Controls.Add(tableDetail);
            documentoTableFooter.Controls.Add(tableFooter);
            #endregion

            #region Documento table footer
            #region Valores
            for (int i = 0; i < 8; i++)
            {
                totalFooterRow = new XRTableRow();
                totalFooterRow.HeightF = 20;

                totalFooterCell_Name = new XRTableCell();
                totalFooterCell_Name.WidthF = columnWidth;
                totalFooterCell_ValueML = new XRTableCell();
                totalFooterCell_ValueML.WidthF = columnWidth;
                totalFooterCell_ValueML.Font = new System.Drawing.Font("Arial", 9);
                totalFooterCell_ValueME = new XRTableCell();
                totalFooterCell_ValueME.WidthF = columnWidth;
                totalFooterCell_ValueME.Font = new System.Drawing.Font("Arial", 9);
                if (i==0) {
                    totalFooterCell_Name.Text = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_VrBruto");
                    totalFooterCell_ValueML.DataBindings.Add("Text", documentoData, "VrBrutoML", "{0:#,0.00}");
                    totalFooterCell_ValueME.DataBindings.Add("Text", documentoData, "VrBrutoME", "{0:#,0.00}"); }
                if (i==1) {
                    totalFooterCell_Name.Text = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_IVA");
                    totalFooterCell_ValueML.DataBindings.Add("Text", documentoData, "IvaML", "{0:#,0.00}");
                    totalFooterCell_ValueME.DataBindings.Add("Text", documentoData, "IvaME", "{0:#,0.00}"); }
                if (i==2) {
                    totalFooterCell_Name.Text = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_RetIVA");
                    totalFooterCell_ValueML.DataBindings.Add("Text", documentoData, "ReteIvaML", "{0:#,0.00}");
                    totalFooterCell_ValueME.DataBindings.Add("Text", documentoData, "ReteIvaME", "{0:#,0.00}"); }
                if (i==3) {
                    totalFooterCell_Name.Text = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_ReteFuente");
                    totalFooterCell_ValueML.DataBindings.Add("Text", documentoData, "ReteFuenteML", "{0:#,0.00}");
                    totalFooterCell_ValueME.DataBindings.Add("Text", documentoData, "ReteFuenteME", "{0:#,0.00}"); }
                if (i==4) {
                    totalFooterCell_Name.Text = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_ReteICA");
                    totalFooterCell_ValueML.DataBindings.Add("Text", documentoData, "ReteIcaML", "{0:#,0.00}");
                    totalFooterCell_ValueME.DataBindings.Add("Text", documentoData, "ReteIcaME", "{0:#,0.00}");  }
                if (i==5) {
                    totalFooterCell_Name.Text = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Timbre");
                    totalFooterCell_ValueML.DataBindings.Add("Text", documentoData, "TimbreML", "{0:#,0.00}"); 
                    totalFooterCell_ValueME.DataBindings.Add("Text", documentoData, "TimbreME", "{0:#,0.00}"); }
                if (i==6) {
                    totalFooterCell_Name.Text = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Anticipos");
                    totalFooterCell_ValueML.DataBindings.Add("Text", documentoData, "AnticiposML", "{0:#,0.00}");
                    totalFooterCell_ValueME.DataBindings.Add("Text", documentoData, "AnticiposME", "{0:#,0.00}"); }
                if (i == 7) {
                    totalFooterCell_Name.Text = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_OtrosDts");
                    totalFooterCell_Name.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                    totalFooterCell_Name.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
                    totalFooterCell_ValueML.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                    totalFooterCell_ValueML.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
                    totalFooterCell_ValueML.DataBindings.Add("Text", documentoData, "OtrosDtosML", "{0:#,0.00}");
                    totalFooterCell_ValueME.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                    totalFooterCell_ValueME.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
                    totalFooterCell_ValueME.DataBindings.Add("Text", documentoData, "OtrosDtosME", "{0:#,0.00}"); }
                totalFooterRow.Cells.Add(totalFooterCell_Name);
                totalFooterRow.Cells.Add(totalFooterCell_ValueML);
                if (multiMoneda) totalFooterRow.Cells.Add(totalFooterCell_ValueME);
                totalFooter.Rows.Add(totalFooterRow);
            };
            totalFooter.EndInit();
            documentoTableFooter.Controls.Add(totalFooter);
            #endregion
            #region Neto
            totalFooterNetoCell_Name.Text = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_NetoAPagar");
            totalFooterNetoCell_ValueML.DataBindings.Add("Text", documentoData, "VrNetoML", "{0:#,0.00}");
            totalFooterNetoCell_ValueME.DataBindings.Add("Text", documentoData, "VrNetoME", "{0:#,0.00}"); 

            totalFooterNetoRow.Cells.Add(totalFooterNetoCell_Name);
            totalFooterNetoRow.Cells.Add(totalFooterNetoCell_ValueML);
            if (multiMoneda) totalFooterNetoRow.Cells.Add(totalFooterNetoCell_ValueME);
            totalFooterNeto.Rows.Add(totalFooterNetoRow);

            totalFooterNeto.EndInit();
            documentoTableFooter.Controls.Add(totalFooterNeto);
            #endregion
            #endregion
        } 
        #endregion
    }
}

