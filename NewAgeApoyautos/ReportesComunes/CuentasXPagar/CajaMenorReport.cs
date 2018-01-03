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
    public partial class CajaMenorReport : BaseCommonReport
    {
        #region Variables
        CommonReportDataSupplier _supplier;
        int _docId;
        #endregion

        #region Funciones Publicas
        /// <summary>
        /// Caja Menor Report Constructor
        /// </summary>
        /// <param name="docId">Report ID (from AppReport)</param>
        /// <param name="documentoData">data for the report</param>
        /// <param name="fieldList">list of fields for report detail table</param>
        /// <param name="estadoInd">indicador de aprobacion del documneto (aprobado - false)</param> 
        /// <param name="supplier"> Interface que provee de informacion a un reporte comun</param>
        public CajaMenorReport(int docId, List<DTO_ReportCajaMenor> documentoData, ArrayList fieldList, bool estadoInd, CommonReportDataSupplier supplier)
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
            DetailReportBand documentoBand;
            documentoBand = new DetailReportBand();
            documentoBand.DataSource = documentoData;
            documentoBand.DataMember = "CajaMenorDetail";

            GroupHeaderBand documentoHeader = new GroupHeaderBand();
            documentoHeader.HeightF = 100;
            documentoHeader.Level = 2;
            documentoBand.Bands.Add(documentoHeader);

            GroupHeaderBand documentoTableHeader = new GroupHeaderBand();
            documentoTableHeader.HeightF = 55;
            documentoTableHeader.Level = 1;
            documentoBand.Bands.Add(documentoTableHeader);

            GroupHeaderBand documentoGroupHeader;
            documentoGroupHeader = new GroupHeaderBand();
            documentoGroupHeader.Level = 0;
            documentoGroupHeader.HeightF = 40;
            documentoBand.Bands.Add(documentoGroupHeader);

            GroupField documentoGroupField = new GroupField("CajaMenorDetail.CargoEspID");
            documentoGroupField.SortOrder = XRColumnSortOrder.Ascending;
            documentoGroupHeader.GroupFields.Add(documentoGroupField);

            DetailBand documentoTableDetail;
            documentoTableDetail = new DetailBand();
            documentoTableDetail.HeightF = 20;
            documentoBand.Bands.Add(documentoTableDetail);

            GroupFooterBand documentoGroupFooter;
            documentoGroupFooter = new GroupFooterBand();
            documentoGroupFooter.Level = 0;
            documentoGroupFooter.HeightF = 65;
            documentoBand.Bands.Add(documentoGroupFooter);

            GroupFooterBand documentoTableFooter = new GroupFooterBand();
            documentoTableFooter.HeightF = 275;
            documentoTableFooter.Level = 1;
            documentoBand.Bands.Add(documentoTableFooter);

            GroupFooterBand documentoFooter = new GroupFooterBand();
            documentoFooter.HeightF = 45;
            documentoFooter.Level = 2;
            documentoBand.Bands.Add(documentoFooter);

            this.Bands.Add(documentoBand);
            #endregion

            #region Documento field width
            float tableWidth = 0;
            float columnWidth = 0;

            tableWidth = this.PageWidth - (this.Margins.Right + this.Margins.Left);

            columnWidth = (tableWidth - 50) /fieldList.Count;
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

            float documentoHeaderTableCellWidth = (tableWidth - 2) / 4;

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
            documentoHeaderTableRow.HeightF = documentoHeaderTable.HeightF / 3;

            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = documentoHeaderTableCellWidth;
            documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoHeaderTableCell_Name.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString() + "_Responsable") + ":";
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = 3* documentoHeaderTableCellWidth;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 9);
            documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoHeaderTableCell_Value.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "Responsable");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTable.Rows.Add(documentoHeaderTableRow);
            #endregion

            #region Row 2
            documentoHeaderTableRow = new XRTableRow();
            documentoHeaderTableRow.HeightF = documentoHeaderTable.HeightF / 3;
            
            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = documentoHeaderTableCellWidth;
            documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoHeaderTableCell_Name.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString() + "_Periodo");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = 100;
            documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            documentoHeaderTableCell_Name.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_lblFrom") + ":";
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = documentoHeaderTableCellWidth - 100;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 9);
            documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoHeaderTableCell_Value.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "FechaIni", "{0:dd/MM/yyyy}");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = 100;
            documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            documentoHeaderTableCell_Name.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_lblUntil") + ":";
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = documentoHeaderTableCellWidth - 100;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 9);
            documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoHeaderTableCell_Value.Padding = new DevExpress.XtraPrinting.PaddingInfo(10,0,0,0);
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "FechaFin", "{0:dd/MM/yyyy}");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = 150;
            documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            documentoHeaderTableCell_Name.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (this._docId).ToString() + "_Numero") + ":";
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = documentoHeaderTableCellWidth - 150;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 9);
            documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoHeaderTableCell_Value.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "Factura");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTable.Rows.Add(documentoHeaderTableRow);
            #endregion

            #region Row 3
            documentoHeaderTableRow = new XRTableRow();
            documentoHeaderTableRow.HeightF = documentoHeaderTable.HeightF / 3;

            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = documentoHeaderTableCellWidth;
            documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoHeaderTableCell_Name.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString() + "_FechaCont") + ":";
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = 2*documentoHeaderTableCellWidth;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 9);
            documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoHeaderTableCell_Value.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "FechaCont","{0:dd/MM/yyyy}");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = 150;
            documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            documentoHeaderTableCell_Name.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString() + "_Registros") + ":";
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = documentoHeaderTableCellWidth - 150;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 9);
            documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoHeaderTableCell_Value.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "RegNro");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTable.Rows.Add(documentoHeaderTableRow);
            #endregion

            documentoHeaderTable.EndInit();
            documentoHeader.Controls.Add(documentoHeaderTable);
            #endregion

            #region Documento group header
            XRShape groupHeaderFrame = new XRShape();
            groupHeaderFrame.LocationF = new PointF(0, 10);
            groupHeaderFrame.SizeF = new SizeF(tableWidth, 25);
            groupHeaderFrame.BorderWidth = 1;
            groupHeaderFrame.SendToBack();
            groupHeaderFrame.Shape = new ShapeRectangle()
            {
                Fillet = 50
            };
            documentoGroupHeader.Controls.Add(groupHeaderFrame);

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

            #region Decumento Table
            XRTable tableHeader;
            XRTableRow tableHeaderRow;
            XRTableCell tableHeaderCell;
            tableHeader = new XRTable();
            tableHeader.LocationF = new PointF(0, 20);
            tableHeader.WidthF = tableWidth;
            tableHeader.HeightF = documentoTableHeader.HeightF - 25;
            tableHeader.StyleName = "tableHeaderStyle";
            tableHeaderRow = new XRTableRow();

            XRTable tableDetail;
            XRTableRow tableDetailRow;
            XRTableCell tableDetailCell;
            tableDetail = new XRTable();
            tableDetail.WidthF = tableWidth;
            tableDetail.HeightF = 20; // documentoTableDetail.HeightF;
            tableDetail.OddStyleName = "tableDetailOddStyle";
            tableDetail.EvenStyleName = "tableDetailEvenStyle";
            tableDetailRow = new XRTableRow();
            tableDetailRow.Name = "tableDetailRow";
            tableDetailRow.HeightF = 20;

            #endregion

            #region Documento group footer
            XRTable groupFooter;
            XRTableRow groupFooterRow;
            XRTableCell groupFooterCell_Name;
            XRTableCell groupFooterCell_Value;
            groupFooter = new XRTable();
            groupFooter.LocationF = new PointF(0, 5);
            groupFooter.SizeF = new SizeF(tableWidth, 40);
            groupFooter.StyleName = "groupFooterStyle";
            groupFooterRow = new XRTableRow();

            XRLine footerLowerLine_1 = new XRLine()
            {
                LineWidth = 1,
                SizeF = new SizeF(tableWidth, 2),
                LocationF = new PointF(0, groupFooter.LocationF.Y + groupFooter.HeightF + 5)
            };
            documentoGroupFooter.Controls.Add(footerLowerLine_1);

            XRLine footerLowerLine_2 = new XRLine()
            {
                LineWidth = 1,
                SizeF = new SizeF(tableWidth, 2),
                LocationF = new PointF(0, groupFooter.LocationF.Y + groupFooter.HeightF + 8)
            };
            documentoGroupFooter.Controls.Add(footerLowerLine_2);
            #endregion
            
            #region Documento total footer
            XRTable tableTotalFooter;
            XRTableRow tableTotalFooterRow;
            XRTableCell tableTotalFooterCell_Name;
            XRTableCell tableTotalFooterCell_Value;
            tableTotalFooter = new XRTable();
            tableTotalFooter.LocationF = new PointF(0, 10);
            tableTotalFooter.SizeF = new SizeF(tableWidth, 40);
            tableTotalFooter.StyleName = "groupFooterStyle";
            tableTotalFooterRow = new XRTableRow();

            XRLine totalFooterLowerLine_1 = new XRLine()
            {
                LineWidth = 2,
                SizeF = new SizeF(tableWidth, 3),
                LocationF = new PointF(0, tableTotalFooter.HeightF + 5)
            };
            documentoTableFooter.Controls.Add(totalFooterLowerLine_1);

            XRLine totalFooterLowerLine_2 = new XRLine()
            {
                LineWidth = 2,
                SizeF = new SizeF(tableWidth, 3),
                LocationF = new PointF(0, tableTotalFooter.HeightF + 9)
            };
            documentoTableFooter.Controls.Add(totalFooterLowerLine_2);


            XRTable documentoTotalFooter;
            XRTableRow documentoTotalFooterRow;
            XRTableCell documentoTotalFooterCell_Name;
            XRTableCell documentoTotalFooterCell_Value;
            documentoTotalFooter = new XRTable();
            documentoTotalFooter.BeginInit();
            documentoTotalFooter.LocationF = new PointF(tableWidth - 4 * columnWidth - 100, totalFooterLowerLine_2.LocationF.Y + 20);
            documentoTotalFooter.SizeF = new SizeF(4 * columnWidth, 3 * 30);
            documentoTotalFooter.StyleName = "groupFooterStyle";
            #endregion

            #region Documento footer
            float labelWidth = tableWidth / 7;
            float gapWidth = tableWidth / 49;

            XRLabel lblElaborado_Name = new XRLabel();
            lblElaborado_Name.LocationF = new PointF(gapWidth, 5);
            lblElaborado_Name.SizeF = new System.Drawing.SizeF(labelWidth, 50);
            lblElaborado_Name.Font = new System.Drawing.Font("Arial", 10);
            lblElaborado_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            lblElaborado_Name.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString() + "_Elaborado");
            documentoFooter.Controls.Add(lblElaborado_Name);

            XRLabel lblElaborado_Value = new XRLabel();
            lblElaborado_Value.LocationF = new PointF(lblElaborado_Name.LocationF.X, lblElaborado_Name.LocationF.Y + lblElaborado_Name.HeightF);
            lblElaborado_Value.SizeF = new System.Drawing.SizeF(labelWidth, 30);
            lblElaborado_Value.Font = new System.Drawing.Font("Arial", 10);
            lblElaborado_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            lblElaborado_Value.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            lblElaborado_Value.DataBindings.Add("Text", this.DataSource, "UsuarioElab");
            documentoFooter.Controls.Add(lblElaborado_Value);

            XRLabel lblSolicita_Name = new XRLabel();
            lblSolicita_Name.LocationF = new PointF(lblElaborado_Name.LocationF.X + lblElaborado_Name.WidthF + gapWidth, lblElaborado_Name.LocationF.Y);
            lblSolicita_Name.SizeF = new System.Drawing.SizeF(labelWidth, 50);
            lblSolicita_Name.Font = new System.Drawing.Font("Arial", 10);
            lblSolicita_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            lblSolicita_Name.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString() + "_Solicita");
            documentoFooter.Controls.Add(lblSolicita_Name);

            XRLabel lblSolicita_Value = new XRLabel();
            lblSolicita_Value.LocationF = new PointF(lblSolicita_Name.LocationF.X, lblSolicita_Name.LocationF.Y + lblSolicita_Name.HeightF);
            lblSolicita_Value.SizeF = new System.Drawing.SizeF(labelWidth, 30);
            lblSolicita_Value.Font = new System.Drawing.Font("Arial", 10);
            lblSolicita_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            lblSolicita_Value.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            lblSolicita_Value.DataBindings.Add("Text", this.DataSource, "UsuarioSol");
            documentoFooter.Controls.Add(lblSolicita_Value);

            XRLabel lblRevisado_Name = new XRLabel();
            lblRevisado_Name.LocationF = new PointF(lblSolicita_Name.LocationF.X + lblSolicita_Name.WidthF + gapWidth, lblSolicita_Name.LocationF.Y);
            lblRevisado_Name.SizeF = new System.Drawing.SizeF(labelWidth, 50);
            lblRevisado_Name.Font = new System.Drawing.Font("Arial", 10);
            lblRevisado_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            lblRevisado_Name.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString() + "_Revisado");
            documentoFooter.Controls.Add(lblRevisado_Name);

            XRLabel lblRevisado_Value = new XRLabel();
            lblRevisado_Value.LocationF = new PointF(lblRevisado_Name.LocationF.X, lblRevisado_Name.LocationF.Y + lblRevisado_Name.HeightF);
            lblRevisado_Value.SizeF = new System.Drawing.SizeF(labelWidth, 30);
            lblRevisado_Value.Font = new System.Drawing.Font("Arial", 10);
            lblRevisado_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            lblRevisado_Value.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            lblRevisado_Value.DataBindings.Add("Text", this.DataSource, "UsuarioRev");
            documentoFooter.Controls.Add(lblRevisado_Value);

            XRLabel lblSuperVisado_Name = new XRLabel();
            lblSuperVisado_Name.LocationF = new PointF(lblRevisado_Name.LocationF.X + lblRevisado_Name.WidthF + gapWidth, lblRevisado_Name.LocationF.Y);
            lblSuperVisado_Name.SizeF = new System.Drawing.SizeF(labelWidth, 50);
            lblSuperVisado_Name.Font = new System.Drawing.Font("Arial", 10);
            lblSuperVisado_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            lblSuperVisado_Name.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString() + "_SuperVisado");
            documentoFooter.Controls.Add(lblSuperVisado_Name);

            XRLabel lblSuperVisado_Value = new XRLabel();
            lblSuperVisado_Value.LocationF = new PointF(lblSuperVisado_Name.LocationF.X, lblSuperVisado_Name.LocationF.Y + lblSuperVisado_Name.HeightF);
            lblSuperVisado_Value.SizeF = new System.Drawing.SizeF(labelWidth, 30);
            lblSuperVisado_Value.Font = new System.Drawing.Font("Arial", 10);
            lblSuperVisado_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            lblSuperVisado_Value.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            lblSuperVisado_Value.DataBindings.Add("Text", this.DataSource, "UsuarioSV");
            documentoFooter.Controls.Add(lblSuperVisado_Value);

            XRLabel lblAprovado_Name = new XRLabel();
            lblAprovado_Name.LocationF = new PointF(lblSuperVisado_Name.LocationF.X + lblSuperVisado_Name.WidthF + gapWidth, lblSuperVisado_Name.LocationF.Y);
            lblAprovado_Name.SizeF = new System.Drawing.SizeF(labelWidth, 50);
            lblAprovado_Name.Font = new System.Drawing.Font("Arial", 10);
            lblAprovado_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            lblAprovado_Name.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString() + "_Aprobado");
            documentoFooter.Controls.Add(lblAprovado_Name);

            XRLabel lblAprovado_Value = new XRLabel();
            lblAprovado_Value.LocationF = new PointF(lblAprovado_Name.LocationF.X, lblAprovado_Name.LocationF.Y + lblAprovado_Name.HeightF);
            lblAprovado_Value.SizeF = new System.Drawing.SizeF(labelWidth, 30);
            lblAprovado_Value.Font = new System.Drawing.Font("Arial", 10);
            lblAprovado_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            lblAprovado_Value.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            lblAprovado_Value.DataBindings.Add("Text", this.DataSource, "UsuarioApr");
            documentoFooter.Controls.Add(lblAprovado_Value);

            XRLabel lblContabolozado_Name = new XRLabel();
            lblContabolozado_Name.LocationF = new PointF(lblAprovado_Name.LocationF.X + lblAprovado_Name.WidthF + gapWidth, lblAprovado_Name.LocationF.Y);
            lblContabolozado_Name.SizeF = new System.Drawing.SizeF(labelWidth, 50);
            lblContabolozado_Name.Font = new System.Drawing.Font("Arial", 10);
            lblContabolozado_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            lblContabolozado_Name.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString() + "_Contabilizado");
            documentoFooter.Controls.Add(lblContabolozado_Name);

            XRLabel lblContabolozado_Value = new XRLabel();
            lblContabolozado_Value.LocationF = new PointF(lblContabolozado_Name.LocationF.X, lblContabolozado_Name.LocationF.Y + lblContabolozado_Name.HeightF);
            lblContabolozado_Value.SizeF = new System.Drawing.SizeF(labelWidth, 30);
            lblContabolozado_Value.Font = new System.Drawing.Font("Arial", 10);
            lblContabolozado_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            lblContabolozado_Value.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            lblContabolozado_Value.DataBindings.Add("Text", this.DataSource, "UsuarioCont");
            documentoFooter.Controls.Add(lblContabolozado_Value);
            #endregion
            #endregion

            int TotalsInd = 0;
            float totalsFieldLocation = 0;

            foreach (string fieldName in fieldList)
            {
                #region Documento table header
                tableHeaderCell = new XRTableCell();
                tableHeaderCell.WidthF = (fieldName.Contains("Desc")) ? columnWidth + 50 : columnWidth;
                tableHeaderCell.BorderColor = Color.White;
                string resourceId = this._docId.ToString() + "_" + fieldName;
                string columnname = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, resourceId);
                tableHeaderCell.Text = columnname;

                tableHeaderRow.Controls.Add(tableHeaderCell);
                #endregion

                #region Documento table detail
                tableDetailCell = new XRTableCell();
                tableDetailCell.WidthF = tableHeaderCell.WidthF;

                if (fieldName.Contains("Valor"))
                {
                    tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                    tableDetailCell.DataBindings.Add("Text", documentoData, "CajaMenorDetail." + fieldName , "{0:#,0.00}");
                }
                else if (fieldName.Contains("Fecha"))
                {
                    tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    tableDetailCell.DataBindings.Add("Text", documentoData, "CajaMenorDetail." + fieldName,"{0:dd/MM/yyyy}");
                }
                else
                {
                    tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                    tableDetailCell.DataBindings.Add("Text", documentoData, "CajaMenorDetail." + fieldName);
                };
                tableDetailRow.Controls.Add(tableDetailCell);
                #endregion

                #region Documento table footer
                if (fieldName.Contains("Valor"))
                {
                    if (TotalsInd == 0)
                    {
                        tableTotalFooterCell_Name = new XRTableCell();
                        tableTotalFooterCell_Name.WidthF = totalsFieldLocation;
                        tableTotalFooterCell_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 20, 0, 0);
                        tableTotalFooterCell_Name.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_TotalGeneral") + ": ";
                        tableTotalFooterRow.Controls.Add(tableTotalFooterCell_Name);

                        groupFooterCell_Name = new XRTableCell();
                        groupFooterCell_Name.WidthF = totalsFieldLocation;
                        groupFooterCell_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 20, 0, 0);
                        groupFooterCell_Name.Name = "groupFooterCell_Name";
                        groupFooterRow.Controls.Add(groupFooterCell_Name);

                        TotalsInd = 1;
                    };

                    tableTotalFooterCell_Value = new XRTableCell();
                    tableTotalFooterCell_Value.Name = fieldName + "_total";
                    tableTotalFooterCell_Value.WidthF = tableHeaderCell.WidthF;
                    tableTotalFooterCell_Value.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                    tableTotalFooterCell_Value.BorderWidth = 2;
                    tableTotalFooterCell_Value.Summary.Func = SummaryFunc.Sum;
                    tableTotalFooterCell_Value.Summary.Running = SummaryRunning.Report;
                    tableTotalFooterCell_Value.Summary.FormatString = "{0:#,0.00}";
                    tableTotalFooterCell_Value.DataBindings.Add("Text", this.DataSource, "CajaMenorDetail." + fieldName);
                    tableTotalFooterRow.Controls.Add(tableTotalFooterCell_Value);

                    groupFooterCell_Value = new XRTableCell();
                    groupFooterCell_Value.Name = fieldName + "_group";
                    groupFooterCell_Value.WidthF = tableHeaderCell.WidthF;
                    groupFooterCell_Value.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                    groupFooterCell_Value.Summary.Func = SummaryFunc.Sum;
                    groupFooterCell_Value.Summary.Running = SummaryRunning.Group;
                    groupFooterCell_Value.Summary.FormatString = "{0:#,0.00}";
                    groupFooterCell_Value.DataBindings.Add("Text", this.DataSource, "CajaMenorDetail." + fieldName);
                    groupFooterRow.Controls.Add(groupFooterCell_Value);
                };

                if (TotalsInd == 0)
                    totalsFieldLocation += tableHeaderCell.WidthF;
                #endregion

            };
            tableHeader.Controls.Add(tableHeaderRow);
            tableDetail.Controls.Add(tableDetailRow);
            groupFooter.Controls.Add(groupFooterRow);
            tableTotalFooter.Controls.Add(tableTotalFooterRow);
            
            documentoTableHeader.Controls.Add(tableHeader);
            documentoTableDetail.Controls.Add(tableDetail);
            documentoGroupFooter.Controls.Add(groupFooter);
            documentoTableFooter.Controls.Add(tableTotalFooter);
            
            #region Documento table footer

            for (int i = 0; i < 3; i++)
            {
                documentoTotalFooterRow = new XRTableRow();
                documentoTotalFooterRow.HeightF = 30;

                documentoTotalFooterCell_Name = new XRTableCell();
                documentoTotalFooterCell_Name.WidthF = 2 *columnWidth;
                documentoTotalFooterCell_Value = new XRTableCell();
                documentoTotalFooterCell_Value.WidthF = 2*columnWidth;
                documentoTotalFooterCell_Value.Font = new System.Drawing.Font("Arial", 9);
                switch (i)
                {
                    case 0:
                        documentoTotalFooterCell_Name.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString() + "_ValorCajaMenor");
                        documentoTotalFooterCell_Value.Name = "totalFooterCell_ValorCajaMenor";
                        documentoTotalFooterCell_Value.DataBindings.Add("Text", documentoData, "ValorCajaMenor", "{0:#,0.00}");
                        break;
                    case 1:

                        documentoTotalFooterCell_Name.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString() + "_ValorSoportes");
                        documentoTotalFooterCell_Value.Name = "totalFooterCell_ValorSoportes";
                        documentoTotalFooterCell_Value.DataBindings.Add("Text", documentoData, "ValorSoportes", "{0:#,0.00}");
                        break;
                    case 2:
                        documentoTotalFooterCell_Name.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString() + "_ValorDisponible");
                        documentoTotalFooterCell_Value.Name = "totalFooterCell_ValorDisponible";
                        documentoTotalFooterCell_Value.DataBindings.Add("Text", documentoData, "ValorDisponible", "{0:#,0.00}");
                        break;                   
                };
                documentoTotalFooterRow.Cells.Add(documentoTotalFooterCell_Name);
                documentoTotalFooterRow.Cells.Add(documentoTotalFooterCell_Value);
                documentoTotalFooter.Rows.Add(documentoTotalFooterRow);
            };
            documentoTotalFooter.EndInit();
            documentoTableFooter.Controls.Add(documentoTotalFooter);
            #endregion

            #region Report group header (En comentarios)
            //groupHeaderCell_Name = new XRTableCell();
            //groupHeaderCell_Name.Name = "groupHeaderCell_Name";
            //groupHeaderCell_Name.WidthF = 100;
            //groupHeaderCell_Name.Text = "CargoEspID";
            //groupHeaderCell_Name.BeforePrint += new System.Drawing.Printing.PrintEventHandler(groupHeaderCell_Name_BeforePrint);

            //groupHeaderCell_Value = new XRTableCell();
            //groupHeaderCell_Value.WidthF = 100;
            //groupHeaderCell_Value.DataBindings.Add("Text", this.DataSource, "CajaMenorDetail.CargoEspID");

            //groupHeaderCell_Desc = new XRTableCell();
            //groupHeaderCell_Desc.WidthF = tableWidth - 200;
            //groupHeaderCell_Desc.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            //groupHeaderCell_Desc.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            //groupHeaderCell_Desc.DataBindings.Add("Text", this.DataSource, "CajaMenorDetail.CargoEspDesc");

            //groupHeaderRow.Controls.Add(groupHeaderCell_Name);
            //groupHeaderRow.Controls.Add(groupHeaderCell_Value);
            //groupHeaderRow.Controls.Add(groupHeaderCell_Desc);

            //groupHeader.Controls.Add(groupHeaderRow);
            //documentoGroupHeader.Controls.Add(groupHeader);
            #endregion
        } 
        #endregion

        #region Eventos
        /// <summary>
        /// Puts proper field captions depending on group field name
        /// </summary>
        private void groupHeaderCell_Name_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell headerCell = (XRTableCell)sender;
            if (!string.IsNullOrEmpty(headerCell.Text) && !string.IsNullOrWhiteSpace(headerCell.Text))
                headerCell.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (this._docId).ToString() + "_" + headerCell.Text);

            XRTableCell footerCell = FindControl("groupFooterCell_Name", true) as XRTableCell;
            footerCell.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_Totals") + "  x  " + headerCell.Text;
        }
        #endregion
    }
}

