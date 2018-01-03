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
    public partial class PagoFacturasReport : BaseCommonReport
    {
        #region Variables
        CommonReportDataSupplier _supplier;
        int _docId;
        #endregion

        #region Funciones Publicas
        /// <summary>
        /// Pago Facturas Report Constructor
        /// </summary>
        /// <param name="docId">Report ID (from AppReport)</param>
        /// <param name="documentoData">data for the report</param>
        /// <param name="fieldList">list of fields for report detail table</param>
        /// <param name="supplier"> Interface que provee de informacion a un reporte comun</param>
        public PagoFacturasReport(int docId, List<DTO_ReportPagoFacturas> documentoData, ArrayList fieldList, CommonReportDataSupplier supplier)
            : base(supplier)
        {
            this._supplier = supplier;
            this._docId = docId;
           // this.lblReportName.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString());
            
            #region Documento styles
            //this.Landscape = true;

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
                    BackColor = Color.White,
                    ForeColor = Color.Black,
                    Font = new Font("Arial", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                    //Borders = DevExpress.XtraPrinting.BorderSide.Bottom,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0)
                },
                OddStyle = new XRControlStyle()
                {
                    Name = "tableDetailOddStyle",
                    BackColor = Color.WhiteSmoke,
                    ForeColor = Color.Black,
                    Font = new Font("Arial", 8),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                    //Borders = DevExpress.XtraPrinting.BorderSide.Bottom | DevExpress.XtraPrinting.BorderSide.Top,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0)
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
            documentoBand.DataMember = "PagoFacturasDetail";

            GroupHeaderBand documentoHeader = new GroupHeaderBand();
            documentoHeader.HeightF = 200;
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
            documentoTableFooter.HeightF = 55;
            documentoTableFooter.Level = 0;
            documentoBand.Bands.Add(documentoTableFooter);

            this.Bands.Add(documentoBand);
            #endregion

            #region Documento field width
            float tableWidth = 0;
            float columnWidth = 0;

            tableWidth = this.PageWidth - (this.Margins.Right + this.Margins.Left);

            columnWidth = (tableWidth - 200) /fieldList.Count;
            #endregion

            #region Documento elements

            #region Watermark
            //if (estadoInd)
            //{
            //    this.Watermark.Text = "Preliminar";
            //    this.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
            //    this.Watermark.Font = new Font("Arial", 100);
            //    this.Watermark.ForeColor = Color.LightGray;
            //    this.Watermark.TextTransparency = 150;
            //    this.Watermark.ShowBehind = true;
            //};
            #endregion

            #region Documento title
            this.ReportHeader.Controls.Remove(this.lblNombreEmpresa);
            this.ReportHeader.Controls.Remove(this.lblReportName);
            this.ReportHeader.Controls.Remove(this.imgLogoEmpresa);

            XRTable documentoTitleTable;
            XRTableRow documentoTitleTableRow;
            XRTableCell documentoTitleTableCell;

            documentoTitleTable = new XRTable();
            documentoTitleTable.BeginInit();
            documentoTitleTable.LocationF = new PointF(0, 20);
            documentoTitleTable.SizeF = new SizeF(tableWidth, 3 * 30);
            documentoTitleTable.StyleName = "groupHeaderStyle";

            #region Row 1
            documentoTitleTableRow = new XRTableRow();
            documentoTitleTableRow.HeightF = documentoTitleTable.HeightF / 3;

            documentoTitleTableCell = new XRTableCell();
            documentoTitleTableCell.WidthF = tableWidth / 3;
            documentoTitleTableCell.Text = string.Empty;
            documentoTitleTableRow.Cells.Add(documentoTitleTableCell);

            documentoTitleTableCell = new XRTableCell();
            documentoTitleTableCell.WidthF = tableWidth / 3;
            documentoTitleTableCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            documentoTitleTableCell.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0);
            documentoTitleTableCell.DataBindings.Add("Text", documentoData, "Fecha","{0:yyyy     MM     dd}");
            documentoTitleTableRow.Cells.Add(documentoTitleTableCell);

            documentoTitleTableCell = new XRTableCell();
            documentoTitleTableCell.WidthF = tableWidth / 3;
            documentoTitleTableCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            documentoTitleTableCell.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 30, 0, 0);
            documentoTitleTableCell.DataBindings.Add("Text", documentoData, "ValorCheque", "{0:#,0.00}");
            documentoTitleTableRow.Cells.Add(documentoTitleTableCell);

            documentoTitleTable.Rows.Add(documentoTitleTableRow);
            #endregion

            #region Row 2
            
            documentoTitleTableRow = new XRTableRow();
            documentoTitleTableRow.HeightF = documentoTitleTable.HeightF / 3;
            
            documentoTitleTableCell = new XRTableCell();
            documentoTitleTableCell.WidthF = tableWidth;
            documentoTitleTableCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoTitleTableCell.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 30, 0, 0);
            documentoTitleTableCell.DataBindings.Add("Text", documentoData, "TerceroDesc");
            documentoTitleTableRow.Cells.Add(documentoTitleTableCell);

            documentoTitleTable.Rows.Add(documentoTitleTableRow);
            #endregion

            #region Row 3

            documentoTitleTableRow = new XRTableRow();
            documentoTitleTableRow.HeightF = documentoTitleTable.HeightF / 3;

            documentoTitleTableCell = new XRTableCell();
            documentoTitleTableCell.WidthF = tableWidth;
            documentoTitleTableCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoTitleTableCell.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 30, 0, 0);
            documentoTitleTableCell.DataBindings.Add("Text", documentoData, "ValorCheque_letters");
            documentoTitleTableRow.Cells.Add(documentoTitleTableCell);

            documentoTitleTable.Rows.Add(documentoTitleTableRow);
            #endregion

            documentoTitleTable.EndInit();
            this.ReportHeader.Controls.Add(documentoTitleTable);

            #endregion

            #region Documento header

            XRTable documentoHeaderTable;
            XRTableRow documentoHeaderTableRow;
            XRTableCell documentoHeaderTableCell_Name;
            XRTableCell documentoHeaderTableCell_Value;

            documentoHeaderTable = new XRTable();
            documentoHeaderTable.BeginInit();
            documentoHeaderTable.LocationF = new PointF(0, 20);
            documentoHeaderTable.SizeF = new SizeF(tableWidth, 5 * 25 + 10);
            documentoHeaderTable.StyleName = "groupHeaderStyle";

            #region Row 1
            documentoHeaderTableRow = new XRTableRow();
            documentoHeaderTableRow.HeightF = documentoHeaderTable.HeightF / 5 + 10;

            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = tableWidth/6;
            documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomRight;
            documentoHeaderTableCell_Name.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString() + "_Documento") + ":";
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = tableWidth / 3;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 9);
            documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            documentoHeaderTableCell_Value.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0);
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "Documento");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = tableWidth / 6;
            documentoHeaderTableCell_Name.Font = new Font("Arial", 12, FontStyle.Bold);
            documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomRight;
            documentoHeaderTableCell_Name.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString() + "_Nro") + ":";
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = 20;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 12);
            documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomRight;
            documentoHeaderTableCell_Value.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0);
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "ComprobanteID");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = tableWidth / 3 - 20;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 12);
            documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            documentoHeaderTableCell_Value.Padding = new DevExpress.XtraPrinting.PaddingInfo(1, 0, 0, 0);
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "ComprobanteNro");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTable.Rows.Add(documentoHeaderTableRow);
            #endregion

            #region Row 2
            documentoHeaderTableRow = new XRTableRow();
            documentoHeaderTableRow.HeightF = documentoHeaderTable.HeightF / 5;

            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = tableWidth / 6;
            documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomRight;
            documentoHeaderTableCell_Name.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString() + "_FechaYCiudad") + ":";
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = tableWidth / 6;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 9);
            documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            documentoHeaderTableCell_Value.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0);
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "Fecha", "{0:dd MM yyyy ,}");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = 2 * tableWidth / 3;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 9);
            documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            documentoHeaderTableCell_Value.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0);
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "Ciudad");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTable.Rows.Add(documentoHeaderTableRow);
            #endregion

            #region Row 3
            documentoHeaderTableRow = new XRTableRow();
            documentoHeaderTableRow.HeightF = documentoHeaderTable.HeightF / 5;

            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = tableWidth / 6;
            documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomRight;
            documentoHeaderTableCell_Name.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString() + "_Beneficiario") + ":";
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = tableWidth / 3;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 9);
            documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            documentoHeaderTableCell_Value.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0);
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "TerceroDesc");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = tableWidth / 6;
            documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomRight;
            documentoHeaderTableCell_Name.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString() + "_Ident") + ":";
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = tableWidth / 3;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 9);
            documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            documentoHeaderTableCell_Value.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0);
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "TercerID");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTable.Rows.Add(documentoHeaderTableRow);
            #endregion

            #region Row 4
            documentoHeaderTableRow = new XRTableRow();
            documentoHeaderTableRow.HeightF = documentoHeaderTable.HeightF / 5;

            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = tableWidth / 6;
            documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomRight;
            documentoHeaderTableCell_Name.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString() + "_ChequeNro") + ":";
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = 5 * tableWidth / 6;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 9);
            documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            documentoHeaderTableCell_Value.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0);
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "ChequeNro");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTable.Rows.Add(documentoHeaderTableRow);
            #endregion

            #region Row 5
            documentoHeaderTableRow = new XRTableRow();
            documentoHeaderTableRow.HeightF = documentoHeaderTable.HeightF / 5;

            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = tableWidth / 6;
            documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomRight;
            documentoHeaderTableCell_Name.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString() + "_Banco") + ":";
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = tableWidth / 3;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 9);
            documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            documentoHeaderTableCell_Value.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0);
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "BancoDesc");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = tableWidth / 6;
            documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomRight;
            documentoHeaderTableCell_Name.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString() + "_CtaBanco") + ":";
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = tableWidth / 3;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 9);
            documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            documentoHeaderTableCell_Value.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0);
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "BancoCuentaID");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTable.Rows.Add(documentoHeaderTableRow);
            #endregion

            documentoHeaderTable.EndInit();
            documentoHeader.Controls.Add(documentoHeaderTable);
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
                        
            #region Documento total footer
            XRLabel lblTableFooter_Name = new XRLabel();
            lblTableFooter_Name.LocationF = new PointF(0, 0);
            lblTableFooter_Name.SizeF = new SizeF(tableWidth/3, 30);
            lblTableFooter_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 2, 0);
            lblTableFooter_Name.Borders = DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom | DevExpress.XtraPrinting.BorderSide.Left;
            lblTableFooter_Name.BorderWidth = 2;
            lblTableFooter_Name.Font = new Font("Arial", 10, FontStyle.Bold);
            lblTableFooter_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            lblTableFooter_Name.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString() + "_TotalDocumento");
            documentoTableFooter.Controls.Add(lblTableFooter_Name);

            XRLabel lblTableFooter_Value = new XRLabel();
            lblTableFooter_Value.LocationF = new PointF(lblTableFooter_Name.WidthF, 0);
            lblTableFooter_Value.SizeF = new SizeF(tableWidth / 3, 30);
            lblTableFooter_Value.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 10, 2, 0);
            lblTableFooter_Value.Borders = DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom;
            lblTableFooter_Value.BorderWidth = 2;
            lblTableFooter_Value.Font = new Font("Arial", 10);
            lblTableFooter_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            lblTableFooter_Value.DataBindings.Add("Text", this.DataSource, "ValorCheque","{0:#,0.00}");
            documentoTableFooter.Controls.Add(lblTableFooter_Value);

            XRLabel lblFirmaYSella = new XRLabel();
            lblFirmaYSella.LocationF = new PointF(lblTableFooter_Value.LocationF.X + lblTableFooter_Value.WidthF, 0);
            lblFirmaYSella.SizeF = new SizeF(tableWidth / 3, 90);
            lblFirmaYSella.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 2, 0);
            lblFirmaYSella.Borders = DevExpress.XtraPrinting.BorderSide.All;
            lblFirmaYSella.BorderWidth = 2;
            lblFirmaYSella.Font = new Font("Arial", 10);
            lblFirmaYSella.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            lblFirmaYSella.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString() + "_FirmaYSella");
            documentoTableFooter.Controls.Add(lblFirmaYSella);

            XRLabel lblElaboro = new XRLabel();
            lblElaboro.LocationF = new PointF(lblTableFooter_Name.LocationF.X, lblTableFooter_Name.LocationF.Y + lblTableFooter_Name.HeightF);
            lblElaboro.SizeF = new SizeF(lblTableFooter_Name.WidthF, lblFirmaYSella.HeightF - lblTableFooter_Name.HeightF);
            lblElaboro.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 2, 0);
            lblElaboro.Borders = DevExpress.XtraPrinting.BorderSide.Bottom | DevExpress.XtraPrinting.BorderSide.Left;
            lblElaboro.BorderWidth = 2;
            lblElaboro.Font = new Font("Arial", 10, FontStyle.Bold);
            lblElaboro.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            lblElaboro.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString() + "_Elaboro");
            documentoTableFooter.Controls.Add(lblElaboro);

            XRLabel lblAprobo = new XRLabel();
            lblAprobo.LocationF = new PointF(lblTableFooter_Value.LocationF.X, lblTableFooter_Value.LocationF.Y + lblTableFooter_Value.HeightF);
            lblAprobo.SizeF = new SizeF(lblTableFooter_Value.WidthF, lblElaboro.HeightF);
            lblAprobo.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 10, 2, 0);
            lblAprobo.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom;
            lblAprobo.BorderWidth = 2;
            lblAprobo.Font = new Font("Arial", 10, FontStyle.Bold);
            lblAprobo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            lblAprobo.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString() + "_Aprobo");
            documentoTableFooter.Controls.Add(lblAprobo);

            #endregion
            
            #endregion

            foreach (string fieldName in fieldList)
            {
                #region Documento table header
                tableHeaderCell = new XRTableCell();
                tableHeaderCell.WidthF = (fieldName.Contains("Desc")) ? columnWidth + 200 : columnWidth;
                tableHeaderCell.BorderColor = Color.White;
                string resourceId = this._docId.ToString() + "_" + fieldName;
                string columnname = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, resourceId);
                tableHeaderCell.Text = columnname;

                tableHeaderRow.Controls.Add(tableHeaderCell);
                #endregion

                #region Documento table detail
                tableDetailCell = new XRTableCell();
                tableDetailCell.WidthF = tableHeaderCell.WidthF;
                tableDetailCell.BorderWidth = 2;

                if (fieldName.Contains("Cuenta"))
                    tableDetailCell.Borders = DevExpress.XtraPrinting.BorderSide.Left;

                if (fieldName.Contains("Valor"))
                {
                    tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                    tableDetailCell.Borders = DevExpress.XtraPrinting.BorderSide.Right;
                    tableDetailCell.DataBindings.Add("Text", documentoData, "PagoFacturasDetail." + fieldName , "{0:#,0.00}");
                }                
                else
                {
                    tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                    tableDetailCell.DataBindings.Add("Text", documentoData, "PagoFacturasDetail." + fieldName);
                };
                tableDetailRow.Controls.Add(tableDetailCell);
                #endregion                
            };
            tableHeader.Controls.Add(tableHeaderRow);
            tableDetail.Controls.Add(tableDetailRow);
            
            documentoTableHeader.Controls.Add(tableHeader);
            documentoTableDetail.Controls.Add(tableDetail);
        } 
        #endregion

    }
}

