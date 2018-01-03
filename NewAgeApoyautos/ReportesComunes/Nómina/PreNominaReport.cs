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
    public partial class PreNominaReport : BaseCommonReport
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
        public PreNominaReport(int docId, List<DTO_ReportNoPreNomina> documentoData, ArrayList detailFieldList, ArrayList footerFieldList, CommonReportDataSupplier supplier)
            : base(supplier)
        {
            this._supplier = supplier;
            this._docId = docId;
            this.lblReportName.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms,this._docId.ToString());
            int totalsInd = 0;
            float totalFieldLocation = 0;
            bool isApro = false;

            #region Documento styles

            XRControlStyle headerStyle = new XRControlStyle()
                                             {
                                                 Name = "groupHeaderStyle",
                                                 BackColor = Color.Transparent,
                                                 ForeColor = Color.Black,
                                                 Font = new Font("Arial", 10, FontStyle.Bold),
                                                 TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                                                 Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0)
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
                                                                      TextAlignment =
                                                                          DevExpress.XtraPrinting.TextAlignment.
                                                                          MiddleCenter,
                                                                      //Borders = DevExpress.XtraPrinting.BorderSide.Bottom,
                                                                      Padding =
                                                                          new DevExpress.XtraPrinting.PaddingInfo(0, 2,
                                                                                                                  0, 0)
                                                                  },
                                                  OddStyle = new XRControlStyle()
                                                                 {
                                                                     Name = "tableDetailOddStyle",
                                                                     BackColor = Color.White,
                                                                     ForeColor = Color.Black,
                                                                     Font = new Font("Arial", 8),
                                                                     TextAlignment =
                                                                         DevExpress.XtraPrinting.TextAlignment.
                                                                         MiddleCenter,
                                                                     //Borders = DevExpress.XtraPrinting.BorderSide.Bottom,
                                                                     Padding =
                                                                         new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0,
                                                                                                                 0)
                                                                 },
                                                  Style = new XRControlStyle()
                                                              {
                                                                  Name = "tableHeaderStyle",
                                                                  BackColor = Color.DimGray,
                                                                  ForeColor = Color.White,
                                                                  Font = new Font("Arial", 9, FontStyle.Bold),
                                                                  TextAlignment =
                                                                      DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
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
            documentoBand.DataMember = "Detail";

            GroupHeaderBand documentPeriodBand;
            documentPeriodBand = new GroupHeaderBand();
            documentPeriodBand.Level = 1;
            documentPeriodBand.HeightF = 20;
            documentoBand.Bands.Add(documentPeriodBand);

            GroupHeaderBand documentPrefixBand;
            documentPrefixBand = new GroupHeaderBand();
            documentPrefixBand.Level = 2;
            documentPrefixBand.HeightF = 60;
            documentoBand.Bands.Add(documentPrefixBand);

            GroupHeaderBand documentoHeader = new GroupHeaderBand();
            documentoHeader.HeightF = 15;
            documentoHeader.Level = 0;
            documentoBand.Bands.Add(documentoHeader);

            GroupHeaderBand documentoHeader2 = new GroupHeaderBand();
            documentoHeader2.HeightF = 15;
            documentoHeader2.Level = 0;
            documentoBand.Bands.Add(documentoHeader2);

            DetailBand documentoDetail;
            documentoDetail = new DetailBand();
            documentoDetail.HeightF = 20;
            documentoBand.Bands.Add(documentoDetail);

            GroupFooterBand documentFooterBand;
            documentFooterBand = new GroupFooterBand();
            documentFooterBand.Level = 3;
            documentFooterBand.HeightF = 100;
            documentoBand.Bands.Add(documentFooterBand);

            GroupFooterBand documentFooterBand2;
            documentFooterBand2 = new GroupFooterBand();
            documentFooterBand2.Level = 4;
            documentFooterBand2.HeightF = 100;
            documentoBand.Bands.Add(documentFooterBand2);

            this.Bands.Add(documentoBand);

            #endregion

            #region Documento field width

            float headerTableWidth = (this.PageWidth - (this.Margins.Right + this.Margins.Left));

            #endregion

            #region Documento elements

            int TotalsInd = 0;
            float totalsFieldLocation = 0;

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
            documentoHeaderPeriodTable.WidthF = headerTableWidth;
            documentoHeaderPeriodTable.StyleName = "groupHeaderStyle";

            documentoHeaderPrefixTable = new XRTable();
            documentoHeaderPrefixTable.BeginInit();
            documentoHeaderPrefixTable.LocationF = new PointF(0, 0);
            documentoHeaderPrefixTable.SizeF = new SizeF(140, 0);
            documentoHeaderPrefixTable.WidthF = headerTableWidth;
            documentoHeaderPrefixTable.StyleName = "groupHeaderStyle";
            for (int i = 0; i < documentoData.Count; i++)
            {
                #region Row 1

                documentoHeaderTableRow = new XRTableRow();
                documentoHeaderTableRow.HeightF = documentoHeaderPeriodTable.HeightF;

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 5;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 9);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Cedula:";
                //base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Para");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = headerTableWidth / 5;
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "CedulaEmpleado");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 5;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 10);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Empleado:";
                //base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Para");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = headerTableWidth / 5;
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "Empleado");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

                documentoHeaderPeriodTable.Rows.Add(documentoHeaderTableRow);

                #endregion

                #region Row 2

                documentoHeaderTableRow = new XRTableRow();
                documentoHeaderTableRow.HeightF = documentoHeaderPeriodTable.HeightF + 10;

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 2;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 9);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Proc.ReteFte:";
                //base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = headerTableWidth;
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "ProcReteFte");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 2;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 9);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Porcentaje:";
                //base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = headerTableWidth / 2;
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "Porcentaje");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 2;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 9);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "DDRete:";
                //base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = headerTableWidth / 2;
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "DDRete");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);
                documentoHeaderPeriodTable.Rows.Add(documentoHeaderTableRow);

                #endregion

                #region Row 3

                documentoHeaderTableRow = new XRTableRow();
                documentoHeaderTableRow.HeightF = documentoHeaderPeriodTable.HeightF;

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 10);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Centro Costo:";
                //base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Value.Font = new Font("Arial", 8);
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "CentroCosto");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 10);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "LocFisica:";
                //base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Value.Font = new Font("Arial", 8);
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "LocFisica");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);


                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 10);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Cargo:";
                //base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Value.Font = new Font("Arial", 8);
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "Cargo");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 10);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Brigada:";
                //base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Value.Font = new Font("Arial", 8);
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "Brigada");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 10);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Operación:";
                //base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Value.Font = new Font("Arial", 8);
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "Operacion");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

                documentoHeaderPeriodTable.Rows.Add(documentoHeaderTableRow);

                #endregion

                #region Row 4

                documentoHeaderTableRow = new XRTableRow();
                documentoHeaderTableRow.HeightF = documentoHeaderPeriodTable.HeightF + 10;

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 2;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 9);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = " ";
                //base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 2;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 9);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = " ";
                //base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderPeriodTable.Rows.Add(documentoHeaderTableRow);

                #endregion

                #region Row 5

                documentoHeaderTableRow = new XRTableRow();
                documentoHeaderTableRow.HeightF = documentoHeaderPeriodTable.HeightF + 10;

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 2;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 10);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "DEVENGOS";
                //base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 2;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 10);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "DEDUCCIONES";
                //base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderPeriodTable.Rows.Add(documentoHeaderTableRow); 
                #endregion
            }

            documentoHeaderPeriodTable.EndInit();
            documentPeriodBand.Controls.Add(documentoHeaderPeriodTable);

            #endregion

            #region Report Table detail

            XRTable tablePeriodHeader;
            XRTableRow tablePeriodHeaderRow;
            XRTableCell tablePeriodHeaderCell;
            tablePeriodHeader = new XRTable();
            tablePeriodHeader.WidthF = headerTableWidth / 2;
            tablePeriodHeader.HeightF = documentoHeader.HeightF - 5;
            tablePeriodHeader.StyleName = "tableHeaderStyle";
            tablePeriodHeaderRow = new XRTableRow();

            XRTable tableHeader;
            XRTableRow tableHeaderRow;
            XRTableCell tableHeaderCell;
            tableHeader = new XRTable();
            tableHeader.WidthF = headerTableWidth;
            tableHeader.HeightF = documentoHeader.HeightF - 5;
            tableHeader.StyleName = "tableHeaderStyle";
            tableHeaderRow = new XRTableRow();

            XRTable tableHeader2;
            XRTableRow tableHeaderRow2;
            XRTableCell tableHeaderCell2;
            tableHeader2 = new XRTable();
            tableHeader2.WidthF = headerTableWidth / 2;
            tableHeader2.HeightF = documentoHeader.HeightF - 5;
            tableHeader2.StyleName = "tableHeaderStyle";
            tableHeaderRow2 = new XRTableRow();

            XRTable tableDetail;
            XRTableRow tableDetailRow;
            XRTableCell tableDetailCell;
            XRTableCell tableDetailCell_MaxLengthInd;
            tableDetail = new XRTable();
            tableDetail.WidthF = headerTableWidth;
            tableDetail.HeightF = documentoDetail.HeightF;
            tableDetail.StyleName = "tableDetailEvenStyle";
            tableDetailRow = new XRTableRow();
            tableDetailRow.Name = "tableDetailRow";
            //tableDetailRow.HeightF = tableDetail.HeightF;
            #endregion

            // este reporte no tiene groupFooter. o si tiene??
            #region Documento group footer

            XRTable groupFooter;
            XRTableRow groupFooterRow;
            XRTableCell groupFooterCell_Name;
            XRTableCell groupFooterCell_Value;
            groupFooter = new XRTable();
            groupFooter.LocationF = new PointF(0, 5);
            groupFooter.SizeF = new SizeF(headerTableWidth, 40);
            groupFooter.StyleName = "groupFooterStyle";
            groupFooterRow = new XRTableRow();

            #endregion

            #region Report footer band

            XRTable totalFooter;
            XRTableRow tableTotalFooterRow;
            XRTableRow tableTotalFooterRow2;
            XRTableCell tableTotalFooterCell_Name;
            XRTableCell tableTotalFooterCell_Value;
            totalFooter = new XRTable();
            totalFooter.LocationF = new PointF(0, 10);
            totalFooter.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            totalFooter.SizeF = new SizeF(headerTableWidth, 30);
            totalFooter.StyleName = "totalFooterStyle";
            tableTotalFooterRow = new XRTableRow();

            totalFooter = new XRTable();
            totalFooter.LocationF = new PointF(0, 10);
            totalFooter.SizeF = new SizeF(headerTableWidth, 30);
            totalFooter.WidthF = headerTableWidth;
            totalFooter.StyleName = "totalFooterStyle";
            tableTotalFooterRow = new XRTableRow();
            #endregion

            #region Documento footer
            List<DTO_ReportNoPreNominaDetail> detalle = new List<DTO_ReportNoPreNominaDetail>();
            foreach (DTO_ReportNoPreNomina item in documentoData)
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

            XRLine footerLine = new XRLine()
            {
                LineWidth = 1,
                SizeF = new SizeF(headerTableWidth, 2),
                LocationF = new PointF(0, 0),
                LineStyle = System.Drawing.Drawing2D.DashStyle.Dash
            };
            documentFooterBand.Controls.Add(footerLine);

            groupFooterRow = new XRTableRow();
            groupFooterRow.HeightF = documentoHeaderPeriodTable.HeightF;

            XRLabel lblSolicita = new XRLabel();
            lblSolicita.LocationF = new PointF(0, footerLine.LocationF.Y + 50);
            lblSolicita.SizeF = new SizeF(headerTableWidth / 3, 25);
            lblSolicita.Font = new Font("Arial", 10);
            lblSolicita.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            lblSolicita.Text = "$" + " " + netoPago;
            
            documentFooterBand.Controls.Add(lblSolicita);

            XRLabel lblNetoPago = new XRLabel();
            lblNetoPago.LocationF = new PointF(0, footerLine.LocationF.Y + 50);
            lblNetoPago.SizeF = new SizeF(headerTableWidth / 2, 5);
            lblNetoPago.Font = new Font("Arial", 10);
            lblNetoPago.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            lblNetoPago.Text = "Neto a Pagar:";

            documentFooterBand.Controls.Add(lblNetoPago);

            XRLabel lblRecibido = new XRLabel();
            lblRecibido.LocationF = new PointF(300, footerLine.LocationF.Y + 50);
            lblRecibido.SizeF = new SizeF(headerTableWidth / 2, 5);
            lblRecibido.Font = new Font("Arial", 10);
            lblRecibido.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            lblRecibido.Text = "Recibi ______________  CC________________";

            documentFooterBand.Controls.Add(lblRecibido);

            tableTotalFooterCell_Value = new XRTableCell();
            tableTotalFooterCell_Value.WidthF = headerTableWidth;
            tableTotalFooterCell_Value.Borders = DevExpress.XtraPrinting.BorderSide.Bottom | DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Top;
            tableTotalFooterCell_Value.Text = string.Empty;
            tableTotalFooterCell_Value.LocationF = new PointF(300, footerLine.LocationF.Y + 90);
            tableTotalFooterCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            tableTotalFooterCell_Value.BeforePrint += new PrintEventHandler(certFooterCell_Value_BeforePrint);

            documentFooterBand2.Controls.Add(tableTotalFooterCell_Value);

            #endregion

            #endregion

            #region Report Table
            
            foreach (string fieldName in detailFieldList)
            {
                #region Report table header

                tableHeaderCell = new XRTableCell();
                tableHeaderCell.WidthF = headerTableWidth / detailFieldList.Count;

                string resourceFieldID = (AppReports.noPreNomina).ToString() + "_" + fieldName;
                string tableColumnName = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, resourceFieldID);
                tableHeaderCell.Text = tableColumnName;
                tableHeaderRow.Controls.Add(tableHeaderCell);

                #endregion

                #region Report table detail

                tableDetailCell = new XRTableCell();
                tableDetailCell.WidthF = tableHeaderCell.WidthF;
                if (fieldName.Contains("CodigoDevengos"))
                {
                    tableDetailCell.WidthF = tableHeaderCell.WidthF / 5;
                    tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    tableDetailCell.DataBindings.Add("Text", documentoData, "Detail." + fieldName);
                }
                if (fieldName.Contains("DescripcionDevengos"))
                {
                    tableDetailCell.WidthF = tableHeaderCell.WidthF /2;
                    tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    tableDetailCell.DataBindings.Add("Text", documentoData, "Detail." + fieldName);
                }
                if (fieldName.Contains("BaseDevengos"))
                {
                    tableDetailCell.WidthF = tableHeaderCell.WidthF / 5;
                    tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    tableDetailCell.DataBindings.Add("Text", documentoData, "Detail." + fieldName, "{0:C0}");
                }
                if (fieldName.Contains("ValorDevengos"))
                {
                    tableDetailCell.WidthF = tableHeaderCell.WidthF / 4;
                    tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    tableDetailCell.DataBindings.Add("Text", documentoData, "Detail." + fieldName, "{0:C0}");
                }
                if (fieldName.Contains("CodigoDeducciones"))
                {
                    tableDetailCell.WidthF = tableHeaderCell.WidthF / 5;
                    tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    tableDetailCell.DataBindings.Add("Text", documentoData, "Detail." + fieldName);
                }
                if (fieldName.Contains("DescripcionDeducciones"))
                {
                    tableDetailCell.WidthF = tableHeaderCell.WidthF / 2;
                    tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    tableDetailCell.DataBindings.Add("Text", documentoData, "Detail." + fieldName);
                }
                if (fieldName.Contains("BaseDeducciones"))
                {
                    tableDetailCell.WidthF = tableHeaderCell.WidthF / 5;
                    tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    tableDetailCell.DataBindings.Add("Text", documentoData, "Detail." + fieldName, "{0:C0}");
                }
                if (fieldName.Contains("ValorDeducciones"))
                {
                    tableDetailCell.WidthF = tableHeaderCell.WidthF / 4;
                    tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    tableDetailCell.DataBindings.Add("Text", documentoData, "Detail." + fieldName, "{0:C0}");
                }
                tableDetailRow.Controls.Add(tableDetailCell);

                #endregion

                #region Report table footer

                //if (fieldName.Contains("ValorDevengos"))
                if (fieldName.Contains("Valor")) // para trabajar con ambos Valores
                {
                    if (TotalsInd == 0)
                    {
                        tableTotalFooterCell_Name = new XRTableCell();
                        tableTotalFooterCell_Name.WidthF = totalsFieldLocation;
                        tableTotalFooterCell_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 20, 0, 0);
                        tableTotalFooterCell_Name.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms,(AppForms.ReportForm).ToString() + "_" + fieldName.Replace("Valor", "Total")) + " "; // el nomre secambia automaticamente. no es necesario. pero me parece mas corto y mas bonito)))
                        tableTotalFooterRow.Controls.Add(tableTotalFooterCell_Name);

                        TotalsInd = 1;
                    }

                    tableTotalFooterCell_Value = new XRTableCell();
                    tableTotalFooterCell_Value.Name = fieldName + "_total";
                    tableTotalFooterCell_Value.WidthF = tableHeaderCell.WidthF;
                    tableTotalFooterCell_Value.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                    tableTotalFooterCell_Value.BorderWidth = 2;
                    tableTotalFooterCell_Value.Summary.Func = SummaryFunc.Sum;
                    tableTotalFooterCell_Value.Summary.Running = SummaryRunning.Report;
                    tableTotalFooterCell_Value.Summary.FormatString = "{0:C2}";
                    tableTotalFooterCell_Value.DataBindings.Add("Text", this.DataSource, "Detail." + fieldName);
                    tableTotalFooterRow.Controls.Add(tableTotalFooterCell_Value);

                }

                if (TotalsInd == 0)
                    totalsFieldLocation += tableHeaderCell.WidthF;
                else // para calcular el ancho de tableTotalFooterCell_Name para TotalDeducido
                {
                    totalsFieldLocation = 0;
                    TotalsInd = 0;
                }

                #endregion
            }

            #endregion

            tableHeader.Controls.Add(tableHeaderRow);
            totalFooter.Controls.Add(tableTotalFooterRow);
            tableDetail.Controls.Add(tableDetailRow);

            documentoHeader.Controls.Add(tableHeader);
            documentoDetail.Controls.Add(tableDetail);
            documentFooterBand.Controls.Add(totalFooter);
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

