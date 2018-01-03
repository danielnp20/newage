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
    public partial class AnticipoViajeReport : BaseCommonReport
    {
        #region Variables
        protected CommonReportDataSupplier _dataSupplier; 
        #endregion

        #region Funciones Publicos
        /// <summary>
        /// Anticipo del Viaje Report Constructor
        /// </summary>
        /// <param name="docId">Report ID (from AppReport)</param>
        /// <param name="documentoData">data for the report</param>
        /// <param name="supplier"> Interface que provee de informacion a un reporte comun</param>
        public AnticipoViajeReport(int docID, List<DTO_ReportAnticipoViaje> documentoData, bool estadoInd, CommonReportDataSupplier supplier)            
        :base(supplier)
        {
            this._dataSupplier = supplier;

            InitializeComponent();

            #region Documento styles

            XRControlStyle headerStyle = new XRControlStyle()
            {
                Name = "headerStyle",
                BackColor = Color.Transparent,
                ForeColor = Color.Black,
                Borders = DevExpress.XtraPrinting.BorderSide.All,
                Font = new Font("Arial", 10, FontStyle.Bold),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
            };
            this.StyleSheet.Add(headerStyle);

            XRControlStyle tableStyle = new XRControlStyle()
            {
                Name = "tableStyle",
                BackColor = Color.Transparent,
                ForeColor = Color.Black,
                Borders = DevExpress.XtraPrinting.BorderSide.All,
                Font = new Font("Arial", 9, FontStyle.Bold),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight,
                Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0)
            };
            this.StyleSheet.Add(tableStyle);

            //XRControlStyles tableStyles = new XRControlStyles(this)
            //{
            //    EvenStyle = new XRControlStyle()
            //    {
            //        Name = "tableDetailEvenStyle",
            //        BackColor = Color.WhiteSmoke,
            //        ForeColor = Color.Black,
            //        Font = new Font("Arial", 8),
            //        TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
            //        //Borders = DevExpress.XtraPrinting.BorderSide.Bottom,
            //        Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0)
            //    },
            //    OddStyle = new XRControlStyle()
            //    {
            //        Name = "tableDetailOddStyle",
            //        BackColor = Color.White,
            //        ForeColor = Color.Black,
            //        Font = new Font("Arial", 8),
            //        TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
            //        //Borders = DevExpress.XtraPrinting.BorderSide.Bottom,
            //        Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0)
            //    },
            //    Style = new XRControlStyle()
            //    {
            //        Name = "tableHeaderStyle",
            //        BackColor = Color.DimGray,
            //        ForeColor = Color.White,
            //        Font = new Font("Arial", 9, FontStyle.Bold),
            //        TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
            //    }
            //};

            //this.StyleSheet.Add(tableStyles.EvenStyle);
            //this.StyleSheet.Add(tableStyles.OddStyle);
            //this.StyleSheet.Add(tableStyles.Style);

            #endregion

            #region Documento bands
            this.Margins = new System.Drawing.Printing.Margins(80, 80, 80, 80);

            DetailReportBand documentoBand;
            documentoBand = new DetailReportBand();

            GroupHeaderBand documentoTitle = new GroupHeaderBand();
            documentoTitle.HeightF = 60;
            documentoTitle.Level = 2;
            documentoBand.Bands.Add(documentoTitle);

            GroupHeaderBand documentoHeader = new GroupHeaderBand();
            documentoHeader.HeightF = 155;
            documentoHeader.Level = 1;
            documentoBand.Bands.Add(documentoHeader);

            GroupHeaderBand documentoTableHeader = new GroupHeaderBand();
            documentoTableHeader.HeightF = 60;
            documentoTableHeader.Level = 0;
            documentoBand.Bands.Add(documentoTableHeader);

            DetailBand documentoTableDetail;
            documentoTableDetail = new DetailBand();
            documentoTableDetail.HeightF = 100;
            documentoBand.Bands.Add(documentoTableDetail);

            GroupFooterBand documentoTableFooter = new GroupFooterBand();
            documentoTableFooter.HeightF = 40;
            documentoTableFooter.Level = 0;
            documentoBand.Bands.Add(documentoTableFooter);

            GroupFooterBand documentoFooter = new GroupFooterBand();
            documentoFooter.HeightF = 260;
            documentoFooter.Level = 1;
            documentoBand.Bands.Add(documentoFooter);

            this.Bands.Add(documentoBand);
            #endregion

            #region Documento field width
            float tableWidth = this.PageWidth - (this.Margins.Right + this.Margins.Left);

            #endregion

            #region Documento elements
            this.ReportHeader.Controls.Remove(this.lblNombreEmpresa);
            this.ReportHeader.Controls.Remove(this.lblReportName);
            this.ReportHeader.Controls.Remove(this.imgLogoEmpresa);

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

            XRTable documentoTitleTable;
            XRTableRow documentoTitleTableRow;
            XRTableCell documentoTitleTableCell;

            documentoTitleTable = new XRTable();
            documentoTitleTable.BeginInit();
            documentoTitleTable.LocationF = new PointF(0, 0);
            documentoTitleTable.SizeF = new SizeF(tableWidth, 40);
            documentoTitleTable.Borders = DevExpress.XtraPrinting.BorderSide.All;

            documentoTitleTableRow = new XRTableRow();
            documentoTitleTableRow.HeightF = documentoTitleTable.HeightF;

            documentoTitleTableCell = new XRTableCell();
            documentoTitleTableCell.WidthF = tableWidth / 6;
            documentoTitleTableRow.Cells.Add(documentoTitleTableCell);

            documentoTitleTableCell = new XRTableCell();
            documentoTitleTableCell.WidthF = 4 * tableWidth / 6;
            documentoTitleTableCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            documentoTitleTableCell.Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
            documentoTitleTableCell.Text = "SOLICITUD DE ANTICIPO DE GASTOS DE VIAJE";
            documentoTitleTableRow.Cells.Add(documentoTitleTableCell);

            documentoTitleTableCell = new XRTableCell();
            documentoTitleTableCell.WidthF = tableWidth / 6;
            documentoTitleTableCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoTitleTableCell.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            documentoTitleTableCell.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
            documentoTitleTableCell.Text = "No.";
            documentoTitleTableRow.Cells.Add(documentoTitleTableCell);

            documentoTitleTable.Rows.Add(documentoTitleTableRow);
            documentoTitleTable.EndInit();
            documentoTitle.Controls.Add(documentoTitleTable);

            XRPanel titlePanel = new XRPanel();
            titlePanel.LocationF = documentoTitleTable.LocationF;
            titlePanel.SizeF = documentoTitleTable.SizeF;
            titlePanel.BackColor = Color.Transparent;
            titlePanel.Borders = DevExpress.XtraPrinting.BorderSide.All;
            titlePanel.BorderWidth = 2;
            documentoTitle.Controls.Add(titlePanel);
            #endregion

            #region Documento header
            XRTable documentoHeaderTable;
            XRTableRow documentoHeaderTableRow;
            XRTableCell documentoHeaderTableCell_Name;
            XRTableCell documentoHeaderTableCell_Value;

            documentoHeaderTable = new XRTable();
            documentoHeaderTable.BeginInit();
            documentoHeaderTable.LocationF = new PointF(0, 0);
            documentoHeaderTable.SizeF = new SizeF(tableWidth, 125);
            documentoHeaderTable.StyleName = "headerStyle";

            #region Row 1
            float smallCellWidth = tableWidth / 25;

            documentoHeaderTableRow = new XRTableRow();
            documentoHeaderTableRow.HeightF = documentoHeaderTable.HeightF / 5;

            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = 2 * smallCellWidth;
            documentoHeaderTableCell_Name.Text = "Fecha:";
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = smallCellWidth;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 10);
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "Fecha","{0:dd}"); ////////////// day
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = smallCellWidth;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 10);
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "Fecha","{0:MM}"); ////////////// month
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = 2 * smallCellWidth;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 10);
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "Fecha","{0:yyyy}"); ////////////// year
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = 3 * smallCellWidth;
            documentoHeaderTableCell_Name.Text = "Empresa:";
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = 10 * smallCellWidth;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 10);
            documentoHeaderTableCell_Value.Text = this._dataSupplier.GetNombreEmpresa();
            //documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, ""); ////////////// Empresa
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = 2 * smallCellWidth;
            documentoHeaderTableCell_Name.Text = "Área:";
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = 4 * smallCellWidth;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 10);
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "Area"); ////////////// Area
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTable.Rows.Add(documentoHeaderTableRow);
            #endregion

            #region Row 2
            documentoHeaderTableRow = new XRTableRow();
            documentoHeaderTableRow.HeightF = documentoHeaderTable.HeightF / 5;

            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = 9 * smallCellWidth;
            documentoHeaderTableCell_Name.Text = "Nombre y Apellidos de Funcionario:";
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = 16 * smallCellWidth;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 10);
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "Nombres"); ////////////// Nombre y Apellidos de Funcionario
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTable.Rows.Add(documentoHeaderTableRow);
            #endregion

            #region Row 3

            documentoHeaderTableRow = new XRTableRow();
            documentoHeaderTableRow.HeightF = documentoHeaderTable.HeightF / 5;

            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = 7 * smallCellWidth;
            documentoHeaderTableCell_Name.Text = "Documento de Identidad:";
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = 7 * smallCellWidth;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 10);
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "DocumentoIdent"); ////////////// Documento Identidad
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = 11 * smallCellWidth;
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTable.Rows.Add(documentoHeaderTableRow);
            #endregion

            #region Row 4

            documentoHeaderTableRow = new XRTableRow();
            documentoHeaderTableRow.HeightF = documentoHeaderTable.HeightF / 5;

            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = 7 * smallCellWidth;
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = 2 * smallCellWidth;
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = 5 * smallCellWidth;
            documentoHeaderTableCell_Name.Text = "Motivo del Viaje:";
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = 11 * smallCellWidth;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 10);
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "MotivoViaje"); ////////////// Motivo del Viaje
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTable.Rows.Add(documentoHeaderTableRow);
            #endregion

            #region Row 5

            documentoHeaderTableRow = new XRTableRow();
            documentoHeaderTableRow.HeightF = documentoHeaderTable.HeightF / 5;

            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = 4 * smallCellWidth;
            documentoHeaderTableCell_Name.Text = "Destino(s):";
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = 21 * smallCellWidth;
            documentoHeaderTableCell_Value.Font = new Font("Arial", 10);
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "Destino"); ////////////// Destino
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTable.Rows.Add(documentoHeaderTableRow);
            #endregion

            documentoHeaderTable.EndInit();
            documentoHeader.Controls.Add(documentoHeaderTable);

            XRPanel headerPanel = new XRPanel();
            headerPanel.LocationF = documentoHeaderTable.LocationF;
            headerPanel.SizeF = documentoHeaderTable.SizeF;
            headerPanel.BackColor = Color.Transparent;
            headerPanel.Borders = DevExpress.XtraPrinting.BorderSide.All;
            headerPanel.BorderWidth = 2;
            documentoHeader.Controls.Add(headerPanel);
            #endregion

            #region Documento table header
            XRTable tableHeader;
            XRTableRow tableHeaderRow;
            XRTableCell tableHeaderCell;

            tableHeader = new XRTable();
            tableHeader.BeginInit();
            tableHeader.LocationF = new PointF(0, 0);
            tableHeader.SizeF = new SizeF(tableWidth, 60);
            tableHeader.StyleName = "headerStyle";

            #region Row1
            tableHeaderRow = new XRTableRow();
            tableHeaderRow.HeightF = documentoTitleTable.HeightF / 2;
            tableHeaderRow.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            tableHeaderRow.BorderWidth = 2;

            tableHeaderCell = new XRTableCell();
            tableHeaderCell.WidthF = tableWidth;
            tableHeaderCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            tableHeaderCell.Text = "ANTICIPOS";
            tableHeaderRow.Cells.Add(tableHeaderCell);

            tableHeader.Rows.Add(tableHeaderRow);
            #endregion

            #region Row2
            tableHeaderRow = new XRTableRow();
            tableHeaderRow.HeightF = documentoTitleTable.HeightF / 2;

            tableHeaderCell = new XRTableCell();
            tableHeaderCell.WidthF = 3 * tableWidth / 8;
            tableHeaderCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            tableHeaderCell.Text = "Viajo Nacional";
            tableHeaderRow.Cells.Add(tableHeaderCell);

            tableHeaderCell = new XRTableCell();
            tableHeaderCell.WidthF = tableWidth / 8;
            tableHeaderCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            tableHeaderCell.Text = "No. de Dias";
            tableHeaderRow.Cells.Add(tableHeaderCell);

            tableHeaderCell = new XRTableCell();
            tableHeaderCell.WidthF = 30;
            tableHeaderCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            tableHeaderCell.Text = "";
            tableHeaderRow.Cells.Add(tableHeaderCell);

            tableHeaderCell = new XRTableCell();
            tableHeaderCell.WidthF = tableWidth / 4 - 30;
            tableHeaderCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            tableHeaderCell.Text = "Valor";
            tableHeaderRow.Cells.Add(tableHeaderCell);

            tableHeaderCell = new XRTableCell();
            tableHeaderCell.WidthF = 30;
            tableHeaderCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            tableHeaderCell.Text = "";
            tableHeaderRow.Cells.Add(tableHeaderCell);

            tableHeaderCell = new XRTableCell();
            tableHeaderCell.WidthF = tableWidth / 4 - 30;
            tableHeaderCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            tableHeaderCell.Text = "TOTAL";
            tableHeaderRow.Cells.Add(tableHeaderCell);

            tableHeader.Rows.Add(tableHeaderRow);
            #endregion

            tableHeader.EndInit();
            documentoTableHeader.Controls.Add(tableHeader);

            XRPanel tableHeaderPanel = new XRPanel();
            tableHeaderPanel.LocationF = tableHeader.LocationF;
            tableHeaderPanel.SizeF = tableHeader.SizeF;
            tableHeaderPanel.BackColor = Color.Transparent;
            tableHeaderPanel.Borders = DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right;
            tableHeaderPanel.BorderWidth = 2;
            documentoTableHeader.Controls.Add(tableHeaderPanel);
            #endregion

            #region Documento table detail
            XRTable tableDetail;
            XRTableRow tableDetailRow;
            XRTableCell tableDetailCell_Name;
            XRTableCell tableDetailCell_Value;

            tableDetail = new XRTable();
            tableDetail.BeginInit();
            tableDetail.LocationF = new PointF(0, 0);
            tableDetail.SizeF = new SizeF(tableWidth, 100);
            tableDetail.StyleName = "tableStyle";

            #region Row1
            tableDetailRow = new XRTableRow();
            tableDetailRow.HeightF = documentoTitleTable.HeightF / 4;

            tableDetailCell_Name = new XRTableCell();
            tableDetailCell_Name.WidthF = 3 * tableWidth / 8;
            tableDetailCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            tableDetailCell_Name.Text = "Gastos de Alojamiento";
            tableDetailRow.Cells.Add(tableDetailCell_Name);

            tableDetailCell_Value = new XRTableCell();
            tableDetailCell_Value.WidthF = tableWidth / 8;
            tableDetailCell_Value.Font = new System.Drawing.Font("Arial", 9);
            tableDetailCell_Value.DataBindings.Add("Text", documentoData, "DiasAlojamiento"); ////////////// Gastos de Alojamiento - Dias
            tableDetailRow.Cells.Add(tableDetailCell_Value);

            tableDetailCell_Name = new XRTableCell();
            tableDetailCell_Name.WidthF = 30;
            tableDetailCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            tableDetailCell_Name.Text = "";
            tableDetailRow.Cells.Add(tableDetailCell_Name);

            tableDetailCell_Value = new XRTableCell();
            tableDetailCell_Value.WidthF = tableWidth / 4 - 30;
            tableDetailCell_Value.Font = new System.Drawing.Font("Arial", 9);
            tableDetailCell_Value.DataBindings.Add("Text", documentoData, "ValorAlojamiento", "{0:#,0.00}"); ////////////// Gastos de Alojamiento - Valor
            tableDetailRow.Cells.Add(tableDetailCell_Value);

            tableDetailCell_Name = new XRTableCell();
            tableDetailCell_Name.WidthF = 30;
            tableDetailCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            tableDetailCell_Name.Text = "";
            tableDetailRow.Cells.Add(tableDetailCell_Name);

            tableDetailCell_Value = new XRTableCell();
            tableDetailCell_Value.WidthF = tableWidth / 4 - 30;
            tableDetailCell_Value.Font = new System.Drawing.Font("Arial", 9);
            tableDetailCell_Value.DataBindings.Add("Text", documentoData, "TotalAlojamiento", "{0:#,0.00}"); ////////////// Gastos de Alojamiento - Total
            tableDetailRow.Cells.Add(tableDetailCell_Value);

            tableDetail.Rows.Add(tableDetailRow);
            #endregion

            #region Row2
            tableDetailRow = new XRTableRow();
            tableDetailRow.HeightF = documentoTitleTable.HeightF / 4;

            tableDetailCell_Name = new XRTableCell();
            tableDetailCell_Name.WidthF = 3 * tableWidth / 8;
            tableDetailCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            tableDetailCell_Name.Text = "Gastos de Alimentación";
            tableDetailRow.Cells.Add(tableDetailCell_Name);

            tableDetailCell_Value = new XRTableCell();
            tableDetailCell_Value.WidthF = tableWidth / 8;
            tableDetailCell_Value.Font = new System.Drawing.Font("Arial", 9);
            tableDetailCell_Value.DataBindings.Add("Text", documentoData, "DiasAlimentacion"); ////////////// Gastos de Alimentación - Dias
            tableDetailRow.Cells.Add(tableDetailCell_Value);

            tableDetailCell_Name = new XRTableCell();
            tableDetailCell_Name.WidthF = 30;
            tableDetailCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            tableDetailCell_Name.Text = "";
            tableDetailRow.Cells.Add(tableDetailCell_Name);

            tableDetailCell_Value = new XRTableCell();
            tableDetailCell_Value.WidthF = tableWidth / 4 - 30;
            tableDetailCell_Value.Font = new System.Drawing.Font("Arial", 9);
            tableDetailCell_Value.DataBindings.Add("Text", documentoData, "ValorAlimentacion", "{0:#,0.00}"); ////////////// Gastos de Alimentación - Valor
            tableDetailRow.Cells.Add(tableDetailCell_Value);

            tableDetailCell_Name = new XRTableCell();
            tableDetailCell_Name.WidthF = 30;
            tableDetailCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            tableDetailCell_Name.Text = "";
            tableDetailRow.Cells.Add(tableDetailCell_Name);

            tableDetailCell_Value = new XRTableCell();
            tableDetailCell_Value.WidthF = tableWidth / 4 - 30;
            tableDetailCell_Value.Font = new System.Drawing.Font("Arial", 9);
            tableDetailCell_Value.DataBindings.Add("Text", documentoData, "TotalAlimentacion", "{0:#,0.00}"); ////////////// Gastos de Alimentación - Total
            tableDetailRow.Cells.Add(tableDetailCell_Value);

            tableDetail.Rows.Add(tableDetailRow);
            #endregion

            #region Row3
            tableDetailRow = new XRTableRow();
            tableDetailRow.HeightF = documentoTitleTable.HeightF / 4;

            tableDetailCell_Name = new XRTableCell();
            tableDetailCell_Name.WidthF = 3 * tableWidth / 8;
            tableDetailCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            tableDetailCell_Name.Text = "Relación Gastos de Transporte";
            tableDetailRow.Cells.Add(tableDetailCell_Name);

            tableDetailCell_Value = new XRTableCell();
            tableDetailCell_Value.WidthF = tableWidth / 8;
            tableDetailCell_Value.Font = new System.Drawing.Font("Arial", 9);
            tableDetailCell_Value.DataBindings.Add("Text", documentoData, "DiasTransporte"); ////////////// Relación Gastos de Transporte - Dias
            tableDetailRow.Cells.Add(tableDetailCell_Value);

            tableDetailCell_Name = new XRTableCell();
            tableDetailCell_Name.WidthF = 30;
            tableDetailCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            tableDetailCell_Name.Text = "";
            tableDetailRow.Cells.Add(tableDetailCell_Name);

            tableDetailCell_Value = new XRTableCell();
            tableDetailCell_Value.WidthF = tableWidth / 4 - 30;
            tableDetailCell_Value.Font = new System.Drawing.Font("Arial", 9);
            tableDetailCell_Value.DataBindings.Add("Text", documentoData, "ValorTransporte", "{0:#,0.00}"); ////////////// Relación Gastos de Transporte - Valor
            tableDetailRow.Cells.Add(tableDetailCell_Value);

            tableDetailCell_Name = new XRTableCell();
            tableDetailCell_Name.WidthF = 30;
            tableDetailCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            tableDetailCell_Name.Text = "";
            tableDetailRow.Cells.Add(tableDetailCell_Name);

            tableDetailCell_Value = new XRTableCell();
            tableDetailCell_Value.WidthF = tableWidth / 4 - 30;
            tableDetailCell_Value.Font = new System.Drawing.Font("Arial", 9);
            tableDetailCell_Value.DataBindings.Add("Text", documentoData, "TotalTransporte", "{0:#,0.00}"); ////////////// Relación Gastos de Transporte - Total
            tableDetailRow.Cells.Add(tableDetailCell_Value);

            tableDetail.Rows.Add(tableDetailRow);
            #endregion

            #region Row4
            tableDetailRow = new XRTableRow();
            tableDetailRow.HeightF = documentoTitleTable.HeightF / 4;

            tableDetailCell_Name = new XRTableCell();
            tableDetailCell_Name.WidthF = 3 * tableWidth / 8;
            tableDetailCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            tableDetailCell_Name.Text = "Otros Gastos";
            tableDetailRow.Cells.Add(tableDetailCell_Name);

            tableDetailCell_Value = new XRTableCell();
            tableDetailCell_Value.WidthF = tableWidth / 8;
            tableDetailCell_Value.Font = new System.Drawing.Font("Arial", 9);
            tableDetailCell_Value.DataBindings.Add("Text", documentoData, "DiasOtrosGastos"); ////////////// Otros Gastos - Dias
            tableDetailRow.Cells.Add(tableDetailCell_Value);

            tableDetailCell_Name = new XRTableCell();
            tableDetailCell_Name.WidthF = 30;
            tableDetailCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            tableDetailCell_Name.Text = "";
            tableDetailRow.Cells.Add(tableDetailCell_Name);

            tableDetailCell_Value = new XRTableCell();
            tableDetailCell_Value.WidthF = tableWidth / 4 - 30;
            tableDetailCell_Value.Font = new System.Drawing.Font("Arial", 9);
            tableDetailCell_Value.DataBindings.Add("Text", documentoData, "ValorOtrosGastos", "{0:#,0.00}"); ////////////// Otros Gastos - Valor
            tableDetailRow.Cells.Add(tableDetailCell_Value);

            tableDetailCell_Name = new XRTableCell();
            tableDetailCell_Name.WidthF = 30;
            tableDetailCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            tableDetailCell_Name.Text = "";
            tableDetailRow.Cells.Add(tableDetailCell_Name);

            tableDetailCell_Value = new XRTableCell();
            tableDetailCell_Value.WidthF = tableWidth / 4 - 30;
            tableDetailCell_Value.Font = new System.Drawing.Font("Arial", 9);
            tableDetailCell_Value.DataBindings.Add("Text", documentoData, "TotalOtrosGastos", "{0:#,0.00}"); ////////////// Otros Gastos - Total
            tableDetailRow.Cells.Add(tableDetailCell_Value);

            tableDetail.Rows.Add(tableDetailRow);
            #endregion

            tableDetail.EndInit();
            documentoTableDetail.Controls.Add(tableDetail);

            XRPanel tableDetailPanel = new XRPanel();
            tableDetailPanel.LocationF = tableDetail.LocationF;
            tableDetailPanel.SizeF = tableDetail.SizeF;
            tableDetailPanel.BackColor = Color.Transparent;
            tableDetailPanel.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right;
            tableDetailPanel.BorderWidth = 2;
            documentoTableDetail.Controls.Add(tableDetailPanel);
            #endregion

            #region Documento table footer
            XRTable tableFooter;
            XRTableRow tableFooterRow;
            XRTableCell tableFooterCell_Name;
            XRTableCell tableFooterCell_Value;

            tableFooter = new XRTable();
            tableFooter.BeginInit();
            tableFooter.LocationF = new PointF(0, 0);
            tableFooter.SizeF = new SizeF(tableWidth, 40);
            tableFooter.StyleName = "headerStyle";

            #region Row1
            tableFooterRow = new XRTableRow();
            tableFooterRow.HeightF = documentoTitleTable.HeightF;

            tableFooterCell_Name = new XRTableCell();
            tableFooterCell_Name.WidthF = 3 * tableWidth / 8;
            tableFooterRow.Cells.Add(tableFooterCell_Name);

            tableFooterCell_Name = new XRTableCell();
            tableFooterCell_Name.WidthF = 3 * tableWidth / 8 + 30;
            tableFooterCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            tableFooterCell_Name.Text = "TOTAL ANTICIPO EN PESOS";
            tableFooterRow.Cells.Add(tableFooterCell_Name);

            tableFooterCell_Value = new XRTableCell();
            tableFooterCell_Value.WidthF = tableWidth / 4 - 30;
            tableFooterCell_Value.Font = new System.Drawing.Font("Arial", 10);
            tableFooterCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            tableFooterCell_Value.DataBindings.Add("Text", documentoData, "TotalAnticipo", "{0:#,0.00}"); ////////////// TOTAL ANTICIPO EN PESOS
            tableFooterRow.Cells.Add(tableFooterCell_Value);

            tableFooter.Rows.Add(tableFooterRow);
            #endregion

            tableFooter.EndInit();
            documentoTableFooter.Controls.Add(tableFooter);

            XRPanel tableFooterPanel = new XRPanel();
            tableFooterPanel.LocationF = tableFooter.LocationF;
            tableFooterPanel.SizeF = tableFooter.SizeF;
            tableFooterPanel.BackColor = Color.Transparent;
            tableFooterPanel.Borders = DevExpress.XtraPrinting.BorderSide.Bottom | DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right;
            tableFooterPanel.BorderWidth = 2;
            documentoTableFooter.Controls.Add(tableFooterPanel);
            #endregion

            #region Documento footer
            XRLabel footerLable_Autorizo = new XRLabel();
            footerLable_Autorizo.LocationF = new PointF(0, 40);
            footerLable_Autorizo.SizeF = new SizeF(tableWidth, 60);
            footerLable_Autorizo.Borders = DevExpress.XtraPrinting.BorderSide.All;
            footerLable_Autorizo.BorderWidth = 2;
            footerLable_Autorizo.Font = new Font("Arial", 10);
            footerLable_Autorizo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            footerLable_Autorizo.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0);
            footerLable_Autorizo.Multiline = true;
            footerLable_Autorizo.Text = "Autorizo a la empresa a la cual pertenezco para que descuente de mi salario, el valor entregado por\r\nconcepto de anticipo de gastos de viaje, si pasados cinco (5) días de realizado el gasto no he legalizado\r\ndicho anticipo.";
            documentoFooter.Controls.Add(footerLable_Autorizo);

            XRTable documentoFooterTable;
            XRTableRow documentoFooterTableRow;
            XRTableCell documentoFooterTableCell;

            documentoFooterTable = new XRTable();
            documentoFooterTable.BeginInit();
            documentoFooterTable.LocationF = new PointF(0, footerLable_Autorizo.LocationF.Y + footerLable_Autorizo.HeightF + 50);
            documentoFooterTable.SizeF = new SizeF(tableWidth, 100);
            documentoFooterTable.StyleName = "headerStyle";
            documentoFooterTable.BorderWidth = 2;

            #region Row1
            documentoFooterTableRow = new XRTableRow();
            documentoFooterTableRow.HeightF = 25;

            documentoFooterTableCell = new XRTableCell();
            documentoFooterTableCell.WidthF = tableWidth / 2 - 45;
            documentoFooterTableCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            documentoFooterTableCell.Text = "Funcionario que solicita el anticipo";
            documentoFooterTableRow.Cells.Add(documentoFooterTableCell);

            documentoFooterTableCell = new XRTableCell();
            documentoFooterTableCell.WidthF = 30;
            documentoFooterTableRow.Cells.Add(documentoFooterTableCell);

            documentoFooterTableCell = new XRTableCell();
            documentoFooterTableCell.WidthF = 30;
            documentoFooterTableRow.Cells.Add(documentoFooterTableCell);

            documentoFooterTableCell = new XRTableCell();
            documentoFooterTableCell.WidthF = tableWidth / 2 - 15;
            documentoFooterTableCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            documentoFooterTableCell.Text = "Autorizado por:";
            documentoFooterTableRow.Cells.Add(documentoFooterTableCell);

            documentoFooterTable.Rows.Add(documentoFooterTableRow);
            #endregion

            #region Row2
            documentoFooterTableRow = new XRTableRow();
            documentoFooterTableRow.HeightF = 75;

            documentoFooterTableCell = new XRTableCell();
            documentoFooterTableCell.WidthF = tableWidth / 2 - 15;
            documentoFooterTableCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            //documentoFooterTableCell.DataBindings.Add("Text", documentoData, ""); ////////////// Funcionario que solicita el anticipo
            documentoFooterTableRow.Cells.Add(documentoFooterTableCell);

            documentoFooterTableCell = new XRTableCell();
            documentoFooterTableCell.WidthF = 30;
            documentoFooterTableCell.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right;
            documentoFooterTableRow.Cells.Add(documentoFooterTableCell);

            documentoFooterTableCell = new XRTableCell();
            documentoFooterTableCell.WidthF = tableWidth / 2 - 15;
            documentoFooterTableCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            //documentoFooterTableCell.DataBindings.Add("Text", documentoData, ""); ////////////// Autorizado por
            documentoFooterTableRow.Cells.Add(documentoFooterTableCell);

            documentoFooterTable.Rows.Add(documentoFooterTableRow);
            #endregion

            documentoFooterTable.EndInit();
            documentoFooter.Controls.Add(documentoFooterTable);
            #endregion
            #endregion

        }       
        #endregion
    }
}
