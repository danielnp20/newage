using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Printing;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting.Shape;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.DTO.Reportes;
using System.Collections.Generic;
using DevExpress.XtraPrinting.Drawing;
using System.Linq;

namespace NewAge.ReportesComunes
{
    public partial class VacacionesReport : BaseCommonReport
    {
        #region Variables
        CommonReportDataSupplier _supplier;
        int _docId;
        private decimal netoPago = 0;
        #endregion

        #region Funciones Publicas
        /// <summary>
        /// LegalizacionGastosReport Report Constructor
        /// </summary>
        /// <param name="docId">Report ID (from AppReport)</param>
        /// <param name="documentoData">data for the report</param>
        /// <param name="supplier"> Interface que provee de informacion a un reporte comun</param>
        public VacacionesReport(int docId, List<DTO_ReportNoVacaciones> documentoData, ArrayList detailFieldList1, ArrayList detailFieldList2, CommonReportDataSupplier supplier)
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
                Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0)
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

            GroupHeaderBand documentoTitle1 = new GroupHeaderBand();
            documentoTitle1.HeightF = 100;
            documentoTitle1.Level = 2;
            documentoBand.Bands.Add(documentoTitle1);

            GroupField documentoGroupField = new GroupField("ReportRompimiento1.GroupFieldValue");
            //documentoGroupField.SortOrder = XRColumnSortOrder.Ascending;
            documentoTitle1.GroupFields.Add(documentoGroupField);

            GroupHeaderBand documentoTitle = new GroupHeaderBand();
            documentoTitle.HeightF = 80;
            documentoTitle.Level = 1;
            documentoBand.Bands.Add(documentoTitle);
            //documentoTitle.GroupFields.Add(documentoGroupField);            

            GroupHeaderBand documentoHeader = new GroupHeaderBand();
            documentoHeader.HeightF = 110;
            documentoHeader.Level = 0;
            documentoBand.Bands.Add(documentoHeader);
            //documentoHeader.GroupFields.Add(documentoGroupField);

            DetailBand documentoDetail;
            documentoDetail = new DetailBand();
            documentoDetail.HeightF = 0;
            documentoBand.Bands.Add(documentoDetail);

            GroupFooterBand documentoFooter = new GroupFooterBand();
            documentoFooter.HeightF = 10;
            documentoFooter.Level = 0;
            documentoFooter.PageBreak = DevExpress.XtraReports.UI.PageBreak.AfterBand;
            documentoBand.Bands.Add(documentoFooter);

            GroupFooterBand documentFooterBand;
            documentFooterBand = new GroupFooterBand();
            documentFooterBand.Level = 3;
            documentFooterBand.HeightF = 100;
            documentoBand.Bands.Add(documentFooterBand);
            
            #region Table 1

            DetailReportBand documentoBand1;
            documentoBand1 = new DetailReportBand();
            documentoBand1.DataSource = documentoData;
            documentoBand1.DataMember = "Detail";
            documentoBand1.Level = 0;

            GroupHeaderBand documentoTable1Header = new GroupHeaderBand();
            documentoTable1Header.HeightF = 60;
            documentoTable1Header.Level = 0;
            documentoBand1.Bands.Add(documentoTable1Header);

            DetailBand documentoTable1Detail;
            documentoTable1Detail = new DetailBand();
            documentoTable1Detail.HeightF = 20;
            documentoBand1.Bands.Add(documentoTable1Detail);

            GroupFooterBand documentoTable1Footer = new GroupFooterBand();
            documentoTable1Footer.HeightF = 30;
            documentoTable1Footer.Level = 0;
            documentoBand1.Bands.Add(documentoTable1Footer);

            documentoBand.Bands.Add(documentoBand1);
            #endregion

            #region Table 2
            DetailReportBand documentoBand2;
            documentoBand2 = new DetailReportBand();
            documentoBand2.Level = 1;
            documentoBand2.DataSource = documentoData;
            documentoBand2.DataMember = "Detail";

            GroupHeaderBand documentoHeader2 = new GroupHeaderBand();
            documentoHeader2.HeightF = 60;
            documentoHeader2.Level = 1;
            documentoBand2.Bands.Add(documentoHeader2);

            GroupHeaderBand documentoTable2Header = new GroupHeaderBand();
            documentoTable2Header.HeightF = 60;
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
            #region Table 1
            float tableWidth1 = 0;
            float columnWidth1 = 0;

            tableWidth1 = this.PageWidth - (this.Margins.Right + this.Margins.Left);

            columnWidth1 = (tableWidth1 - 200) / 5; //fieldList.Count;
            #endregion

            #region Table 2
            float tableWidth2 = 0;
            float columnWidth2 = 0;

            tableWidth2 = this.PageWidth - (this.Margins.Right + this.Margins.Left);

            columnWidth2 = (tableWidth2) / 4; //fieldList.Count;
            #endregion
            #endregion

            #region Documento elements

            int TotalsInd = 0;
            float totalsFieldLocation = 0;
            bool isApro = false;

            #region Watermark

            foreach (var item in documentoData)
            {
                isApro = item.isApro;
            }
            if (!isApro)
            {
                this.Watermark.Text = "Preliminar";
                this.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
                this.Watermark.Font = new Font("Arial", 100);
                this.Watermark.ForeColor = Color.LightGray;
                this.Watermark.TextTransparency = 150;
                this.Watermark.ShowBehind = true;
            }

            #endregion

            #region Table 1
            #region Documento title
            this.ReportHeader.HeightF = 0;

            documentoTitle1.Controls.Add(this.imgLogoEmpresa);
            documentoTitle1.Controls.Add(this.lblNombreEmpresa);
            documentoTitle1.Controls.Add(this.lblReportName);

            this.lblNombreEmpresa.LocationF = new PointF(100, 40);
            this.lblReportName.LocationF = new PointF(0, this.lblNombreEmpresa.LocationF.Y + this.lblNombreEmpresa.HeightF + 20);
            this.lblReportName.SizeF = new SizeF(130, 20);
            this.lblReportName.Font = new Font("Arial", 10, FontStyle.Bold);
            this.lblReportName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.lblReportName.Padding = new DevExpress.XtraPrinting.PaddingInfo(100, 0, 0, 0);
            this.lblReportName.Text = "Nit";//_bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.FacturaEquivalente).ToString() + "_Nit");

            XRLabel lblTitleNit_Value = new XRLabel();
            lblTitleNit_Value.LocationF = new PointF(this.lblReportName.LocationF.X + this.lblReportName.WidthF, this.lblReportName.LocationF.Y);
            lblTitleNit_Value.SizeF = new SizeF(tableWidth1 - this.lblReportName.WidthF, 20);
            lblTitleNit_Value.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0);
            lblTitleNit_Value.Font = new Font("Arial", 10);
            lblTitleNit_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            lblTitleNit_Value.DataBindings.Add("Text", this.DataSource, "EmpresaNit");
            documentoTitle1.Controls.Add(lblTitleNit_Value);
            #endregion

            #region Documento header

            XRTable documentoHeaderPeriodTable;
            XRTableRow documentoHeaderTableRow;
            XRTableCell documentoHeaderTableCell_Name;
            XRTableCell documentoHeaderTableCell_Value;

            XRTable documentoHeaderPrefixTable;
            XRTableRow documentoHeaderPrefixTableRow;
            XRTableCell documentoHeaderPrefixTableCell_Name;
            XRTableCell documentoHeaderPrefixTableCell_Value;

            documentoHeaderPeriodTable = new XRTable();
            documentoHeaderPeriodTable.BeginInit();
            documentoHeaderPeriodTable.LocationF = new PointF(0, 0);
            documentoHeaderPeriodTable.SizeF = new SizeF(140, 0);
            documentoHeaderPeriodTable.WidthF = tableWidth1;
            documentoHeaderPeriodTable.StyleName = "groupHeaderStyle";

            documentoHeaderPrefixTable = new XRTable();
            documentoHeaderPrefixTable.BeginInit();
            documentoHeaderPrefixTable.LocationF = new PointF(0, 0);
            documentoHeaderPrefixTable.SizeF = new SizeF(140, 0);
            documentoHeaderPrefixTable.WidthF = tableWidth1;
            documentoHeaderPrefixTable.StyleName = "groupHeaderStyle";
            for (int i = 0; i < documentoData.Count; i++)
            {
                #region Row 1

                documentoHeaderTableRow = new XRTableRow();
                documentoHeaderTableRow.HeightF = documentoHeaderPeriodTable.HeightF;

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = tableWidth1 / 5;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 9);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Cedula:";
                //base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Para");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = tableWidth1 / 5;
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "CedulaEmpleado");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = tableWidth1 / 5;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 10);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Empleado:";
                //base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Para");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = tableWidth1 / 5;
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "Empleado");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

                documentoHeaderPeriodTable.Rows.Add(documentoHeaderTableRow);

                #endregion

                #region Row 2

                documentoHeaderTableRow = new XRTableRow();
                documentoHeaderTableRow.HeightF = documentoHeaderPeriodTable.HeightF + 10;

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = tableWidth1 / 3;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 9);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Periodo:";
                //base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = tableWidth1 / 3;
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "Periodo");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = tableWidth1 / 3;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 9);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Periodo De Pago:";
                //base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = tableWidth1 / 3;
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "PeriodoPago");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

                documentoHeaderPeriodTable.Rows.Add(documentoHeaderTableRow);
                #endregion

                #region Row 3

                documentoHeaderTableRow = new XRTableRow();
                documentoHeaderTableRow.HeightF = documentoHeaderPeriodTable.HeightF + 10;

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = tableWidth1 / 4;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 9);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Fecha De Ingreso:";
                //base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = tableWidth1 / 4;
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "FechaIngreso");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = tableWidth1 / 4;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 9);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Periodo Descanso:";
                //base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = tableWidth1 / 4;
                documentoHeaderTableCell_Value.Font = new Font("Arial", 9);
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "PeriodoDescanso");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

                documentoHeaderPeriodTable.Rows.Add(documentoHeaderTableRow);
                #endregion

                #region Row 4

                documentoHeaderTableRow = new XRTableRow();
                documentoHeaderTableRow.HeightF = documentoHeaderPeriodTable.HeightF;

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = tableWidth1 / 4;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 9);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Fecha De ReIntegro:";
                //base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = tableWidth1 / 6;
                documentoHeaderTableCell_Value.Font = new Font("Arial", 8);
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "FechaReIntegro");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);
                
                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = tableWidth1 / 3;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 9);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Días Tomados:";
                //base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = tableWidth1 / 6;
                documentoHeaderTableCell_Value.Font = new Font("Arial", 9);
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "DiasTomados");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = tableWidth1 / 3;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 9);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Días Pagados:";
                //base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = tableWidth1 / 3;
                documentoHeaderTableCell_Value.Font = new Font("Arial", 9);
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "DiasPagados");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = tableWidth1 / 3;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 9);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Resolución:";
                //base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = tableWidth1 / 3;
                documentoHeaderTableCell_Value.Font = new Font("Arial", 9);
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "Resolucion");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

                documentoHeaderPeriodTable.Rows.Add(documentoHeaderTableRow);

                #endregion

                #region Row 5

                documentoHeaderTableRow = new XRTableRow();
                documentoHeaderTableRow.HeightF = documentoHeaderPeriodTable.HeightF + 10;

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = tableWidth1 / 6;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 9);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Salario :";
                //base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = tableWidth1;
                documentoHeaderTableCell_Value.Font = new Font("Arial", 8);
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "Salario", "{0:C0}");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

                documentoHeaderPeriodTable.Rows.Add(documentoHeaderTableRow);

                #endregion

                #region Row 6

                documentoHeaderTableRow = new XRTableRow();
                documentoHeaderTableRow.HeightF = documentoHeaderPeriodTable.HeightF + 10;

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = tableWidth1;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 10, FontStyle.Bold);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                documentoHeaderTableCell_Name.Text = "DEVENGOS DE LIQUIDACIÓN";
                //base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderPeriodTable.Rows.Add(documentoHeaderTableRow);
                #endregion
            }

            documentoHeaderPeriodTable.EndInit();
            documentoTable1Header.Controls.Add(documentoHeaderPeriodTable);

            #endregion

            #region Documento Table header
            XRTable table1Header;
            XRTableRow table1HeaderRow;
            XRTableCell table1HeaderCell;
            table1Header = new XRTable();
            table1Header.LocationF = new PointF(0, 20);
            table1Header.WidthF = tableWidth1;
            table1Header.HeightF = documentoTable1Header.HeightF - 5;
            table1Header.StyleName = "tableHeaderStyle";
            table1HeaderRow = new XRTableRow();
            #endregion

            #region Documento Table detail

            XRTable table1Detail;
            XRTableRow table1DetailRow;
            XRTableCell table1DetailCell;
            table1Detail = new XRTable();
            table1Detail.WidthF = tableWidth1;
            table1Detail.HeightF = 20; // documentoTableDetail.HeightF;
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
            XRTableCell table1TotalFooterCell_Name;
            XRTableCell table1TotalFooterCell_Value;
            table1Footer = new XRTable();
            table1Footer.LocationF = new PointF(0, 2);
            table1Footer.SizeF = new SizeF(tableWidth1, 30);
            table1Footer.StyleName = "groupFooterStyle";
            table1FooterRow = new XRTableRow();
            table1FooterRow.Name = "table1FooterRow";
            table1FooterRow.HeightF = 30;
            #endregion

            #region Documento group footer

            XRTable groupFooter;
            XRTableRow groupFooterRow;
            XRTableCell groupFooterCell_Name;
            XRTableCell groupFooterCell_Value;
            groupFooter = new XRTable();
            groupFooter.LocationF = new PointF(0, 5);
            groupFooter.SizeF = new SizeF(tableWidth1, 40);
            groupFooter.StyleName = "groupFooterStyle";
            groupFooterRow = new XRTableRow();

            #endregion
            #endregion

            #region Table 2

            #region Report footer band

            XRTable totalFooter;
            XRTableRow tableTotalFooterRow;
            XRTableRow tableTotalFooterRow2;
            XRTableCell tableTotalFooterCell_Name;
            XRTableCell tableTotalFooterCell_Value;
            totalFooter = new XRTable();
            totalFooter.LocationF = new PointF(0, 10);
            totalFooter.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            totalFooter.SizeF = new SizeF(tableWidth2, 30);
            totalFooter.StyleName = "totalFooterStyle";
            tableTotalFooterRow = new XRTableRow();

            totalFooter = new XRTable();
            totalFooter.LocationF = new PointF(0, 10);
            totalFooter.SizeF = new SizeF(tableWidth2, 30);
            totalFooter.WidthF = tableWidth2;
            totalFooter.StyleName = "totalFooterStyle";
            tableTotalFooterRow = new XRTableRow();
            #endregion

            #region Documento header
            XRLabel lblHeaderContab = new XRLabel();
            lblHeaderContab.LocationF = new PointF(0, 20);
            lblHeaderContab.SizeF = new SizeF(tableWidth2, 30);
            lblHeaderContab.Font = new Font("Arial", 10, FontStyle.Bold);
            lblHeaderContab.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            lblHeaderContab.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0);
            lblHeaderContab.Multiline = true;
            lblHeaderContab.Text = "DEDUCCIONES DE LIQUIDACIÓN";
            documentoHeader2.Controls.Add(lblHeaderContab);
            
            #region Documento footer
            List<DTO_ReportNoVacacionesDetail> detalle = new List<DTO_ReportNoVacacionesDetail>();
            foreach (DTO_ReportNoVacaciones item in documentoData)
            {
                detalle = item.Detail;
            }
            decimal devengos = 0;
            decimal deduccions = 0;
            for (int i = 0; i < detalle.Count; i++)
            {
                devengos += detalle[i].ValorDevengos;
                deduccions += detalle[i].ValorDeducciones;
            }
            if (deduccions < 0)
                deduccions = deduccions * -1;

            if (devengos >= deduccions)
                netoPago += devengos - deduccions;
            else
                netoPago += deduccions - devengos;
           
            XRLine footerLine2 = new XRLine()
            {
                LineWidth = 1,
                SizeF = new SizeF(tableWidth1, 2),
                LocationF = new PointF(0, 0),
                LineStyle = System.Drawing.Drawing2D.DashStyle.Dash
            };

            groupFooterRow = new XRTableRow();
            groupFooterRow.HeightF = documentoHeaderPeriodTable.HeightF;

            XRLabel lblSolicita = new XRLabel();
            lblSolicita.LocationF = new PointF(0, footerLine2.LocationF.Y + 50);
            lblSolicita.SizeF = new SizeF(tableWidth1 / 3, 25);
            lblSolicita.Font = new Font("Arial", 10);
            lblSolicita.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            lblSolicita.Text = "$" + netoPago;

            documentFooterBand.Controls.Add(lblSolicita);

            XRLabel lblNetoPago = new XRLabel();
            lblNetoPago.LocationF = new PointF(0, footerLine2.LocationF.Y + 50);
            lblNetoPago.SizeF = new SizeF(tableWidth1 / 2, 5);
            lblNetoPago.Font = new Font("Arial", 10);
            lblNetoPago.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            lblNetoPago.Text = "Neto a Pagar:";

            documentFooterBand.Controls.Add(lblNetoPago);

            XRLabel lblRecibido = new XRLabel();
            lblRecibido.LocationF = new PointF(300, footerLine2.LocationF.Y + 50);
            lblRecibido.SizeF = new SizeF(tableWidth1 / 2, 5);
            lblRecibido.Font = new Font("Arial", 10);
            lblRecibido.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            lblRecibido.Text = "Recibi ______________  CC________________";

            documentFooterBand.Controls.Add(lblRecibido);

            XRLabel lblTexto = new XRLabel();
            lblTexto.LocationF = new PointF(10, footerLine2.LocationF.Y + 60);
            lblTexto.SizeF = new SizeF(tableWidth1 - 10, 5);
            lblTexto.Font = new Font("Arial", 10);
            lblTexto.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            lblTexto.Text = "El suscrito trabajador hace constar que en esta fecha ha recibido el valor correspondiente a las vacaciones de las cuales se ha hecho acreedor, y estas serán por el trabajador de acuerdo a los datos suministrados en este comprobante, además declara que acepta toda la liquidación, así como la fecha de retorno a las labores.";

            documentFooterBand.Controls.Add(lblTexto);

            tableTotalFooterCell_Value = new XRTableCell();
            tableTotalFooterCell_Value.WidthF = tableWidth1;
            tableTotalFooterCell_Value.Borders = DevExpress.XtraPrinting.BorderSide.Bottom | DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Top;
            tableTotalFooterCell_Value.Text = string.Empty;
            tableTotalFooterCell_Value.LocationF = new PointF(300, footerLine2.LocationF.Y + 90);
            tableTotalFooterCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            tableTotalFooterCell_Value.BeforePrint += new PrintEventHandler(certFooterCell_Value_BeforePrint);

            documentFooterBand.Controls.Add(tableTotalFooterCell_Value);

            #endregion
            #endregion

            #region Documento Table header
            XRTable table2Header;
            XRTableRow table2HeaderRow;
            XRTableCell table2HeaderCell;
            table2Header = new XRTable();
            table2Header.LocationF = new PointF(0, 0);
            table2Header.WidthF = tableWidth2;
            table2Header.HeightF = documentoTable2Header.HeightF - 5;
            table2Header.StyleName = "tableHeaderStyle";
            table2HeaderRow = new XRTableRow();
            #endregion

            #region Documento Table detail

            XRTable table2Detail;
            XRTableRow table2DetailRow;
            XRTableCell table2DetailCell;
            table2Detail = new XRTable();
            table2Detail.WidthF = tableWidth2;
            table2Detail.HeightF = 20; // documentoTableDetail.HeightF;
            table2Detail.OddStyleName = "tableDetailOddStyle";
            table2Detail.EvenStyleName = "tableDetailEvenStyle";
            table2DetailRow = new XRTableRow();
            table2DetailRow.Name = "tableDetailRow";
            table2DetailRow.HeightF = 20;
            #endregion

            #region Documento Table footer

            XRTable table2Footer;
            XRTableRow table2FooterRow;
            XRTableCell table2FooterCell;
            XRTableCell table2TotalFooterCell_Name;
            XRTableCell table2TotalFooterCell_Value;
            table2Footer = new XRTable();
            table2Footer.LocationF = new PointF(0, 2);
            table2Footer.SizeF = new SizeF(tableWidth1, 30);
            table2Footer.StyleName = "groupFooterStyle";
            table2FooterRow = new XRTableRow();
            table2FooterRow.Name = "table1FooterRow";
            table2FooterRow.HeightF = 30;
            XRLine footerLine = new XRLine()
            {
                LineWidth = 1,
                ForeColor = Color.Gray,
                SizeF = new SizeF(tableWidth2, 1),
                LocationF = new PointF(0, 0)
            };
            documentoTable2Footer.Controls.Add(footerLine);
            #endregion
            #endregion
            #endregion

            #region Table 1
            foreach (string fieldName in detailFieldList1)
            {
                #region Documento table header
                table1HeaderCell = new XRTableCell();
                table1HeaderCell.WidthF = tableWidth1 / detailFieldList1.Count;

                string resourceFieldID = (AppReports.noPreNomina).ToString() + "_" + fieldName;
                string tableColumnName = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, resourceFieldID);
                table1HeaderCell.Text = tableColumnName;
                table1HeaderRow.Controls.Add(table1HeaderCell);

                #endregion

                #region Report table detail

                table1DetailCell = new XRTableCell();
                table1DetailCell.WidthF = table1HeaderCell.WidthF;
                if (fieldName.Contains("CodigoDevengos"))
                {
                    table1DetailCell.WidthF = table1HeaderCell.WidthF / 5;
                    table1DetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    table1DetailCell.DataBindings.Add("Text", documentoData, "Detail." + fieldName);
                }
                if (fieldName.Contains("DescripcionDevengos"))
                {
                    table1DetailCell.WidthF = table1HeaderCell.WidthF / 2;
                    table1DetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    table1DetailCell.DataBindings.Add("Text", documentoData, "Detail." + fieldName);
                }
                if (fieldName.Contains("BaseDevengos"))
                {
                    table1DetailCell.WidthF = table1HeaderCell.WidthF / 5;
                    table1DetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    table1DetailCell.DataBindings.Add("Text", documentoData, "Detail." + fieldName, "{0:C0}");
                }
                if (fieldName.Contains("ValorDevengos"))
                {
                    table1DetailCell.WidthF = table1HeaderCell.WidthF / 4;
                    table1DetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    table1DetailCell.DataBindings.Add("Text", documentoData, "Detail." + fieldName, "{0:C0}");
                }

                table1DetailRow.Controls.Add(table1DetailCell);

                #endregion

                #region Documento table footer
                table1FooterCell = new XRTableCell();
                table1FooterCell.WidthF = table1HeaderCell.WidthF;

                if (fieldName.Contains("Valor")) // para trabajar con ambos Valores
                {
                    if (TotalsInd == 0)
                    {
                        table1TotalFooterCell_Name = new XRTableCell();
                        table1TotalFooterCell_Name.WidthF = totalsFieldLocation;
                        table1TotalFooterCell_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 20, 0, 0);
                        table1TotalFooterCell_Name.Text = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_" + fieldName.Replace("Valor", "Total")) + " "; // el nomre secambia automaticamente. no es necesario. pero me parece mas corto y mas bonito)))
                        table1FooterRow.Controls.Add(table1TotalFooterCell_Name);

                        TotalsInd = 1;
                    }

                    table1TotalFooterCell_Value = new XRTableCell();
                    table1TotalFooterCell_Value.Name = fieldName + "_total";
                    table1TotalFooterCell_Value.WidthF = table1HeaderCell.WidthF;
                    table1TotalFooterCell_Value.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                    table1TotalFooterCell_Value.BorderWidth = 2;
                    table1TotalFooterCell_Value.Summary.Func = SummaryFunc.Sum;
                    table1TotalFooterCell_Value.Summary.Running = SummaryRunning.Report;
                    table1TotalFooterCell_Value.Summary.FormatString = "{0:C2}";
                    table1TotalFooterCell_Value.DataBindings.Add("Text", this.DataSource, "Detail." + fieldName);
                    table1FooterRow.Controls.Add(table1TotalFooterCell_Value);

                }

                if (TotalsInd == 0)
                    totalsFieldLocation += table1HeaderCell.WidthF;
                else // para calcular el ancho de tableTotalFooterCell_Name para TotalDeducido
                {
                    totalsFieldLocation = 0;
                    TotalsInd = 0;
                }
                #endregion
            }
            table1Header.Controls.Add(table1HeaderRow);
            table1Detail.Controls.Add(table1DetailRow);
            table1Footer.Controls.Add(table1FooterRow);

            documentoTable1Header.Controls.Add(table1Header);
            documentoTable1Detail.Controls.Add(table1Detail);
            documentoTable1Footer.Controls.Add(table1Footer);
            #endregion

            #region Table 2
            foreach (string fieldName in detailFieldList2)
            {
                #region Documento table header
                table2HeaderCell = new XRTableCell();
                table2HeaderCell.WidthF = tableWidth2 / detailFieldList2.Count;

                string resourceFieldID = (AppReports.noVacaciones).ToString() + "_" + fieldName;
                string tableColumnName = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, resourceFieldID);
                table2HeaderCell.Text = tableColumnName;
                table2HeaderRow.Controls.Add(table2HeaderCell);

                #endregion

                #region Documento table detail
                table2DetailCell = new XRTableCell();
                table2DetailCell.WidthF = table2HeaderCell.WidthF;

                if (fieldName.Contains("CodigoDeducciones"))
                {
                    table2DetailCell.WidthF = table2HeaderCell.WidthF / 5;
                    table2DetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    table2DetailCell.DataBindings.Add("Text", documentoData, "Detail." + fieldName);
                }
                if (fieldName.Contains("DescripcionDeducciones"))
                {
                    table2DetailCell.WidthF = table2HeaderCell.WidthF / 2;
                    table2DetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    table2DetailCell.DataBindings.Add("Text", documentoData, "Detail." + fieldName);
                }
                if (fieldName.Contains("BaseDeducciones"))
                {
                    table2DetailCell.WidthF = table2HeaderCell.WidthF / 5;
                    table2DetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    table2DetailCell.DataBindings.Add("Text", documentoData, "Detail." + fieldName, "{0:C0}");
                }
                if (fieldName.Contains("ValorDeducciones"))
                {
                    table2DetailCell.WidthF = table2HeaderCell.WidthF / 4;
                    table2DetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    table2DetailCell.DataBindings.Add("Text", documentoData, "Detail." + fieldName, "{0:C0}");
                }
                table2DetailRow.Controls.Add(table2DetailCell);
                #endregion

                #region Documento table footer
                table2FooterCell = new XRTableCell();
                table2FooterCell.WidthF = table2HeaderCell.WidthF;

                if (fieldName.Contains("Valor")) // para trabajar con ambos Valores
                {
                    if (TotalsInd == 0)
                    {
                        table2TotalFooterCell_Name = new XRTableCell();
                        table2TotalFooterCell_Name.WidthF = totalsFieldLocation;
                        table2TotalFooterCell_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 20, 0, 0);
                        table2TotalFooterCell_Name.Text = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_" + fieldName.Replace("Valor", "Total")) + " "; // el nomre secambia automaticamente. no es necesario. pero me parece mas corto y mas bonito)))
                        table2FooterRow.Controls.Add(table2TotalFooterCell_Name);

                        TotalsInd = 1;
                    }

                    table2TotalFooterCell_Value = new XRTableCell();
                    table2TotalFooterCell_Value.Name = fieldName + "_total";
                    table2TotalFooterCell_Value.WidthF = table2HeaderCell.WidthF;
                    table2TotalFooterCell_Value.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                    table2TotalFooterCell_Value.BorderWidth = 2;
                    table2TotalFooterCell_Value.Summary.Func = SummaryFunc.Sum;
                    table2TotalFooterCell_Value.Summary.Running = SummaryRunning.Report;
                    table2TotalFooterCell_Value.Summary.FormatString = "{0:C2}";
                    table2TotalFooterCell_Value.DataBindings.Add("Text", this.DataSource, "Detail." + fieldName);
                    table2FooterRow.Controls.Add(table2TotalFooterCell_Value);

                }

                if (TotalsInd == 0)
                    totalsFieldLocation += table2HeaderCell.WidthF;
                else // para calcular el ancho de tableTotalFooterCell_Name para TotalDeducido
                {
                    totalsFieldLocation = 0;
                    TotalsInd = 0;
                }
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

        #region Eventos
        /// <summary>
        /// Total value in letters
        /// </summary>
        private void certFooterCell_Value_BeforePrint(object sender, PrintEventArgs e)
        {
            XRTableCell cell = (XRTableCell)sender;

            XRTableCell totalCell = FindControl("Total", true) as XRTableCell;
            if (!string.IsNullOrEmpty(netoPago.ToString()))
            {
                decimal total = Convert.ToDecimal(netoPago);
                cell.Text = CurrencyFormater.GetCurrencyString("ES1", "COP", total);
            }
        }
        #endregion
    }
}

