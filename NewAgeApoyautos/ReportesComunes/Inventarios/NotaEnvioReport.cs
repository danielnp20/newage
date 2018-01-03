using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting.Shape;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.DTO.Reportes;
using System.Collections.Generic;
using DevExpress.XtraPrinting.Drawing;

namespace NewAge.ReportesComunes
{
    public partial class NotaEnvioReport : BaseCommonReport
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
        /// <param name="supplier"> Interface que provee de informacion a un reporte comun</param>
        public NotaEnvioReport(int docId, List<DTO_ReportNotaEnvio> documentoData, ArrayList footerFieldList, ArrayList detailFieldList, CommonReportDataSupplier supplier)
            : base(supplier)
        {
            this._supplier = supplier;
            this._docId = docId;
            this.lblReportName.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString());
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
            documentoBand.DataMember = "Footer";

            GroupHeaderBand documentPeriodBand;
            documentPeriodBand = new GroupHeaderBand();
            documentPeriodBand.Level = 1;
            documentPeriodBand.HeightF = 70;
            documentoBand.Bands.Add(documentPeriodBand);

            GroupHeaderBand documentPrefixBand;
            documentPrefixBand = new GroupHeaderBand();
            documentPrefixBand.Level = 2;
            documentPrefixBand.HeightF = 20;
            documentoBand.Bands.Add(documentPrefixBand);

            GroupHeaderBand documentoHeader = new GroupHeaderBand();
            documentoHeader.HeightF = 20;
            documentoHeader.Level = 0;
            documentoBand.Bands.Add(documentoHeader);

            DetailBand documentoDetail;
            documentoDetail = new DetailBand();
            documentoDetail.HeightF = 20;
            documentoBand.Bands.Add(documentoDetail);

            GroupFooterBand documentFooterBand;
            documentFooterBand = new GroupFooterBand();
            documentFooterBand.Level = 3;
            documentFooterBand.HeightF = 100;
            documentoBand.Bands.Add(documentFooterBand);

            this.Bands.Add(documentoBand);
            #endregion

            #region Documento field width
            float headerTableWidth = (this.PageWidth - (this.Margins.Right + this.Margins.Left));

            #endregion

            #region Documento elements

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
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 9);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Cliente:";//base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Para");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "Cliente");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);
                
                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth;
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 9);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Fecha:";//base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Para");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = headerTableWidth / 4;
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "Fecha", "{0:dd/MMMM/yyyy}");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 10);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Documento:";//base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Para");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "Documento");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

                documentoHeaderPeriodTable.Rows.Add(documentoHeaderTableRow);
                #endregion

                #region Row 2
                documentoHeaderTableRow = new XRTableRow();
                documentoHeaderTableRow.HeightF = documentoHeaderPeriodTable.HeightF;
               
                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 6 ;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 9);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Tipo de Vehiculo:";//base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = headerTableWidth ;
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "TipoVehiculo");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

                documentoHeaderPeriodTable.Rows.Add(documentoHeaderTableRow);
                #endregion

                #region Row 3

                documentoHeaderTableRow = new XRTableRow();
                documentoHeaderTableRow.HeightF = documentoHeaderPeriodTable.HeightF;

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 8;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 9);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Origen:"; //base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Referencia");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = headerTableWidth / 2;
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "BodegaOrigen");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 8;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 9);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Destino:"; //base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Referencia");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = headerTableWidth / 2;
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "BodegaDestino");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

                documentoHeaderPeriodTable.Rows.Add(documentoHeaderTableRow);
                #endregion

                #region Row 4
                documentoHeaderTableRow = new XRTableRow();
                documentoHeaderTableRow.HeightF = documentoHeaderPeriodTable.HeightF;

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 10);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Descripcion:";//base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Value.Font = new Font("Arial", 8);
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "Observacion");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

                documentoHeaderPeriodTable.Rows.Add(documentoHeaderTableRow);
                #endregion
                
                #region Row 5

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 10);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Tipo de Movimiento:"; //base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Referencia");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "MvtoTipoInvID");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);
                documentoHeaderPeriodTable.Rows.Add(documentoHeaderTableRow); 
                #endregion

                #region Row 6

                documentoHeaderTableRow = new XRTableRow();
                documentoHeaderTableRow.HeightF = documentoHeaderPeriodTable.HeightF;

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 10);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Conductor:";//base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "Conductor");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 10);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Cédula:";//base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "Cedula");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 10);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Placa:";//base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "Placa");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

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
            tablePeriodHeader.WidthF = headerTableWidth;
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

            #region Report footer band

            XRTable totalFooter;
            XRTableRow totalFooterRow;
            XRTableCell totalFooterCell_Caption;
            XRTableCell totalFooterCell_Value;
            totalFooter = new XRTable();
            totalFooter.LocationF = new PointF(0, 10);
            totalFooter.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            totalFooter.SizeF = new SizeF(headerTableWidth, 30);
            totalFooter.StyleName = "totalFooterStyle";
            totalFooterRow = new XRTableRow();

            totalFooter = new XRTable();
            totalFooter.LocationF = new PointF(0, 10);
            totalFooter.SizeF = new SizeF(headerTableWidth, 30);
            totalFooter.WidthF = headerTableWidth;
            totalFooter.StyleName = "totalFooterStyle";
            totalFooterRow = new XRTableRow();

            #endregion

            #region Documento footer
            XRLine footerLine = new XRLine()
            {
                LineWidth = 1,
                SizeF = new SizeF(headerTableWidth, 2),
                LocationF = new PointF(0, 0),
                LineStyle = System.Drawing.Drawing2D.DashStyle.Dash
            };
            documentFooterBand.Controls.Add(footerLine);

            XRLabel lblSolicita = new XRLabel();
            lblSolicita.LocationF = new PointF(30, footerLine.LocationF.Y + 100);
            lblSolicita.SizeF = new SizeF(headerTableWidth / 3, 30);
            lblSolicita.Font = new Font("Arial", 10);
            lblSolicita.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            lblSolicita.Text = "DESCPACHADO POR ";
            documentFooterBand.Controls.Add(lblSolicita);

            XRLabel lblSolicita_blank = new XRLabel();
            lblSolicita_blank.LocationF = new PointF(lblSolicita.LocationF.X, lblSolicita.LocationF.Y + lblSolicita.HeightF);
            lblSolicita_blank.SizeF = new SizeF(headerTableWidth / 3, 50);
            lblSolicita_blank.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            documentFooterBand.Controls.Add(lblSolicita_blank);

            XRLabel lblAprobado = new XRLabel();
            lblAprobado.LocationF = new PointF(2 * headerTableWidth / 3, footerLine.LocationF.Y + 100);
            lblAprobado.SizeF = new SizeF(headerTableWidth / 3, 30);
            lblAprobado.Font = new Font("Arial", 10);
            lblAprobado.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            lblAprobado.Text = "RECIBIDO POR";
            documentFooterBand.Controls.Add(lblAprobado);

            XRLabel lblAprobado_blank = new XRLabel();
            lblAprobado_blank.LocationF = new PointF(lblAprobado.LocationF.X, lblAprobado.LocationF.Y + lblAprobado.HeightF);
            lblAprobado_blank.SizeF = new SizeF(headerTableWidth / 3, 50);
            lblAprobado_blank.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            documentFooterBand.Controls.Add(lblAprobado_blank);

            #endregion
            #endregion

            #region Report Table
            //reportLegaDet.Valor = reportLega.Footer.Sum(x => x.ValorAlojamiento);

            foreach (string fieldName in detailFieldList)
            {
                #region Report table header

                tableHeaderCell = new XRTableCell();
                tableHeaderCell.WidthF = headerTableWidth / detailFieldList.Count;

                //if (fieldName.Contains("DocumentoTercero"))
                //    tableHeaderCell.WidthF = columnWidth + 60;
                if (fieldName.Contains("Descriptivo"))
                    tableHeaderCell.WidthF = headerTableWidth + 150;
                //if (fieldName.Contains("Cuenta") || fieldName.Contains("vlrMdaLoc") || fieldName.Contains("vlrMdaExt"))
                //    tableHeaderCell.WidthF = columnWidth - 50;


                string resourceFieldID = (AppReports.inTransaccionManual).ToString() + "_" + fieldName;
                string tableColumnName = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, resourceFieldID);
                tableHeaderCell.Text = tableColumnName;

                tableHeaderRow.Controls.Add(tableHeaderCell);

                #endregion

                #region Report table detail
                tableDetailCell = new XRTableCell();
                tableDetailCell.WidthF = tableHeaderCell.WidthF;

                tableDetailCell = new XRTableCell();
                tableDetailCell.WidthF = tableHeaderCell.WidthF;
                if (fieldName.Contains("inReferenciaID"))
                {
                    tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    tableDetailCell.DataBindings.Add("Text", documentoData, "Footer." + fieldName);
                }
                if (fieldName.Contains("DocSoporte") || fieldName.Contains("DescripReferencia"))
                {
                    tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    tableDetailCell.DataBindings.Add("Text", documentoData, "Footer." + fieldName);
                }
                if (fieldName.Contains("CantidadUni"))
                {
                    tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    tableDetailCell.DataBindings.Add("Text", documentoData, "Footer." + fieldName);
                }
                if (fieldName.Contains("Serial"))
                {
                    tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    tableDetailCell.DataBindings.Add("Text", documentoData, "Footer." + fieldName);
                }
                tableDetailRow.Controls.Add(tableDetailCell);

                #endregion
            }

            tableHeader.Controls.Add(tableHeaderRow);
            tableDetail.Controls.Add(tableDetailRow);
            totalFooter.Controls.Add(totalFooterRow);

            documentoHeader.Controls.Add(tableHeader);
            documentoDetail.Controls.Add(tableDetail);
            //documentoFooterTotales.Controls.Add(totalFooter);

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

