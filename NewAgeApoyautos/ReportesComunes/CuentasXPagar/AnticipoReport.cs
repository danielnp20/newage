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
    public partial class AnticipoReport : BaseCommonReport
    {
        #region Funciones Publicas
        /// <summary>
        /// Anticipo Report Constructor
        /// </summary>
        /// <param name="docId">Report ID (from AppReport)</param>
        /// <param name="documentoData">data for the report</param>
        /// <param name="supplier"> Interface que provee de informacion a un reporte comun</param>
        public AnticipoReport(int docID, List<DTO_ReportAnticipo> documentoData, bool estadoInd, CommonReportDataSupplier supplier)
        //public AnticipoReport(int docID, CommonReportDataSupplier supplier)
            : base(supplier)
        {
            this.lblReportName.Text = "SOLICITUD DE GIRO"; //base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString());
            this.lblReportName.Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);

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

            GroupHeaderBand documentoHeader = new GroupHeaderBand();
            documentoHeader.HeightF = 180;
            documentoHeader.Level = 0;
            documentoBand.Bands.Add(documentoHeader);

            DetailBand documentoDetail;
            documentoDetail = new DetailBand();
            documentoDetail.HeightF = 180;
            documentoBand.Bands.Add(documentoDetail);

            GroupFooterBand documentoFooter = new GroupFooterBand();
            documentoFooter.HeightF = 250;
            documentoFooter.Level = 0;
            documentoBand.Bands.Add(documentoFooter);

            this.Bands.Add(documentoBand);
            #endregion

            #region Documento field width
            float headerTableWidth = (this.PageWidth - (this.Margins.Right + this.Margins.Left)) / 2;
            float headerCellWidth_Name = headerTableWidth / 4 + 10;
            float headerCellWidth_Value = 3 * headerTableWidth / 4 - 10;

            float detailTableWidth = (this.PageWidth - (this.Margins.Right + this.Margins.Left)) - 60;
            float detailCellWidth_Name = headerTableWidth / 5;
            float detailCellWidth_Value = 4 * headerTableWidth / 5;
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
            XRTable documentoHeaderTable;
            XRTableRow documentoHeaderTableRow;
            XRTableCell documentoHeaderTableCell_Name;
            XRTableCell documentoHeaderTableCell_Value;

            documentoHeaderTable = new XRTable();
            documentoHeaderTable.BeginInit();
            documentoHeaderTable.LocationF = new PointF(headerTableWidth / 2, 0);
            documentoHeaderTable.SizeF = new SizeF(headerTableWidth, 90);
            documentoHeaderTable.StyleName = "groupHeaderStyle";

            #region Row 1
            documentoHeaderTableRow = new XRTableRow();
            documentoHeaderTableRow.HeightF = documentoHeaderTable.HeightF / 3;

            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = headerCellWidth_Name;
            documentoHeaderTableCell_Name.Font = new Font("Arial", 10);
            documentoHeaderTableCell_Name.Text = "PARA:";//base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Para");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = headerCellWidth_Value;
            documentoHeaderTableCell_Value.Text = "DEPARETAMENTO DE CONTABILIDAD";
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTable.Rows.Add(documentoHeaderTableRow);
            #endregion

            #region Row 2
            documentoHeaderTableRow = new XRTableRow();
            documentoHeaderTableRow.HeightF = documentoHeaderTable.HeightF / 3;

            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = headerCellWidth_Name;
            documentoHeaderTableCell_Name.Font = new Font("Arial", 10);
            documentoHeaderTableCell_Name.Text = "FECHA:";//base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = headerCellWidth_Value;
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "Fecha", "{0:dd/MMMM/yyyy}");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTable.Rows.Add(documentoHeaderTableRow);
            #endregion

            #region Row 3

            documentoHeaderTableRow = new XRTableRow();
            documentoHeaderTableRow.HeightF = documentoHeaderTable.HeightF / 3;

            documentoHeaderTableCell_Name = new XRTableCell();
            documentoHeaderTableCell_Name.WidthF = headerCellWidth_Name;
            documentoHeaderTableCell_Name.Font = new Font("Arial", 10);
            documentoHeaderTableCell_Name.Text = "REFERENCIA:"; //base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Referencia");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

            documentoHeaderTableCell_Value = new XRTableCell();
            documentoHeaderTableCell_Value.WidthF = headerCellWidth_Value;
            documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData, "TipoAnticipoDesc");
            documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

            documentoHeaderTable.Rows.Add(documentoHeaderTableRow);
            #endregion

            documentoHeaderTable.EndInit();
            documentoHeader.Controls.Add(documentoHeaderTable);

            XRLabel headerLable_Atentament = new XRLabel();
            headerLable_Atentament.LocationF = new PointF(30, documentoHeaderTable.LocationF.Y + documentoHeaderTable.HeightF + 40);
            headerLable_Atentament.SizeF = new System.Drawing.SizeF(detailTableWidth, 30);
            headerLable_Atentament.Font = new System.Drawing.Font("Arial", 10);
            headerLable_Atentament.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            headerLable_Atentament.Multiline = true;
            headerLable_Atentament.Text = "Atentamente solicitamos a usted se sirva con la causación y entrega a Tesorería para el pago, de lo relacionado a continuación:";
            documentoHeader.Controls.Add(headerLable_Atentament);
            #endregion

            #region Documento detail
            XRTable documentoDetailTable;
            XRTableRow documentoDetailTableRow;
            XRTableCell documentoDetailTableCell_Name;
            XRTableCell documentoDetailTableCell_Value;

            documentoDetailTable = new XRTable();
            documentoDetailTable.BeginInit();
            documentoDetailTable.LocationF = new PointF(50, 20);
            documentoDetailTable.SizeF = new SizeF(detailTableWidth, 120);
            documentoDetailTable.StyleName = "groupHeaderStyle";

            #region Row 1
            documentoDetailTableRow = new XRTableRow();
            documentoDetailTableRow.HeightF = documentoHeaderTable.HeightF / 4;

            documentoDetailTableCell_Name = new XRTableCell();
            documentoDetailTableCell_Name.WidthF = detailCellWidth_Name;
            documentoDetailTableCell_Name.Text = "BENEFICIARIO:"; //base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Beneficiario");
            documentoDetailTableRow.Cells.Add(documentoDetailTableCell_Name);

            documentoDetailTableCell_Value = new XRTableCell();
            documentoDetailTableCell_Value.WidthF = detailCellWidth_Value;
            documentoDetailTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoDetailTableCell_Value.DataBindings.Add("Text", documentoData, "TerceroDesc");
            documentoDetailTableRow.Cells.Add(documentoDetailTableCell_Value);

            documentoDetailTable.Rows.Add(documentoDetailTableRow);
            #endregion

            #region Row 2
            documentoDetailTableRow = new XRTableRow();
            documentoDetailTableRow.HeightF = documentoHeaderTable.HeightF / 4;

            documentoDetailTableCell_Name = new XRTableCell();
            documentoDetailTableCell_Name.WidthF = detailCellWidth_Name;
            documentoDetailTableCell_Name.Text = "C.C./NIT:"; // base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_CC_Nit");
            documentoDetailTableRow.Cells.Add(documentoDetailTableCell_Name);

            documentoDetailTableCell_Value = new XRTableCell();
            documentoDetailTableCell_Value.WidthF = detailCellWidth_Value;
            documentoDetailTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoDetailTableCell_Value.DataBindings.Add("Text", documentoData, "TerceroID");
            documentoDetailTableRow.Cells.Add(documentoDetailTableCell_Value);

            documentoDetailTable.Rows.Add(documentoDetailTableRow);
            #endregion

            #region Row 3

            documentoDetailTableRow = new XRTableRow();
            documentoDetailTableRow.HeightF = documentoHeaderTable.HeightF / 4;

            documentoDetailTableCell_Name = new XRTableCell();
            documentoDetailTableCell_Name.WidthF = detailCellWidth_Name;
            documentoDetailTableCell_Name.Text = "CONCEPTO:"; //base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Concepto");
            documentoDetailTableRow.Cells.Add(documentoDetailTableCell_Name);

            documentoDetailTableCell_Value = new XRTableCell();
            documentoDetailTableCell_Value.WidthF = detailCellWidth_Value;
            documentoDetailTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoDetailTableCell_Value.DataBindings.Add("Text", documentoData, "Observacion");
            documentoDetailTableRow.Cells.Add(documentoDetailTableCell_Value);

            documentoDetailTable.Rows.Add(documentoDetailTableRow);
            #endregion

            #region Row 4

            documentoDetailTableRow = new XRTableRow();
            documentoDetailTableRow.HeightF = documentoHeaderTable.HeightF / 4;

            documentoDetailTableCell_Name = new XRTableCell();
            documentoDetailTableCell_Name.WidthF = detailCellWidth_Name;
            documentoDetailTableCell_Name.Text = "VALOR";// base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Valor");
            documentoDetailTableRow.Cells.Add(documentoDetailTableCell_Name);

            documentoDetailTableCell_Value = new XRTableCell();
            documentoDetailTableCell_Value.WidthF = detailCellWidth_Value;
            documentoDetailTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            documentoDetailTableCell_Value.DataBindings.Add("Text", documentoData, "Valor","{0:#,0.00}");
            documentoDetailTableRow.Cells.Add(documentoDetailTableCell_Value);

            documentoDetailTable.Rows.Add(documentoDetailTableRow);
            #endregion

            #region Row 5 blank

            //documentoDetailTableRow = new XRTableRow();
            //documentoDetailTableRow.HeightF = documentoHeaderTable.HeightF / 4;

            //documentoDetailTableCell_Name = new XRTableCell();
            //documentoDetailTableCell_Name.WidthF = detailTableWidth;
            //documentoDetailTableRow.Cells.Add(documentoDetailTableCell_Name);

            //documentoDetailTable.Rows.Add(documentoDetailTableRow);
            #endregion

            #region Row 6

            //documentoDetailTableRow = new XRTableRow();
            //documentoDetailTableRow.HeightF = documentoHeaderTable.HeightF / 4;

            //documentoDetailTableCell_Name = new XRTableCell();
            //documentoDetailTableCell_Name.WidthF = detailCellWidth_Name;
            //documentoDetailTableCell_Name.Text = "OBSERVACIONES:"; // base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Observacion");
            //documentoDetailTableRow.Cells.Add(documentoDetailTableCell_Name);

            //documentoDetailTableCell_Value = new XRTableCell();
            //documentoDetailTableCell_Value.WidthF = detailCellWidth_Value;
            //documentoDetailTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            ////documentoDetailTableCell_Value.DataBindings.Add("Text", documentoData, "");
            //documentoDetailTableRow.Cells.Add(documentoDetailTableCell_Value);

            //documentoDetailTable.Rows.Add(documentoDetailTableRow);
            #endregion

            documentoDetailTable.EndInit();
            documentoDetail.Controls.Add(documentoDetailTable);
            #endregion

            #region Documento footer
            XRLine footerLine = new XRLine()
            {
                LineWidth = 1,
                SizeF = new SizeF(detailTableWidth, 2),
                LocationF = new PointF(30, 0),
                LineStyle = System.Drawing.Drawing2D.DashStyle.Dash
            };
            documentoFooter.Controls.Add(footerLine);

            //XRLabel footerLable_Agradezco = new XRLabel();
            //footerLable_Agradezco.LocationF = new PointF(30, footerLine.LocationF.Y + 25);
            //footerLable_Agradezco.SizeF = new SizeF(detailTableWidth, 30);
            //footerLable_Agradezco.Font = new Font("Arial", 10);
            //footerLable_Agradezco.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            //footerLable_Agradezco.Multiline = true;
            //footerLable_Agradezco.Text = "Agradezco de antemano su colaboración y gestión quedando a la espera de su pronta y positiva respuesta.";
            //documentoFooter.Controls.Add(footerLable_Agradezco);

            XRLabel lblSolicita = new XRLabel();
            lblSolicita.LocationF = new PointF(30, footerLine.LocationF.Y + 100);
            lblSolicita.SizeF = new SizeF(detailTableWidth / 3, 30);
            lblSolicita.Font = new Font("Arial", 10);
            lblSolicita.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            lblSolicita.Text = "SOLICITA";
            documentoFooter.Controls.Add(lblSolicita);

            XRLabel lblSolicita_blank = new XRLabel();
            lblSolicita_blank.LocationF = new PointF(lblSolicita.LocationF.X , lblSolicita.LocationF.Y + lblSolicita.HeightF);
            lblSolicita_blank.SizeF = new SizeF(detailTableWidth / 3, 50);
            lblSolicita_blank.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            documentoFooter.Controls.Add(lblSolicita_blank);

            XRLabel lblAprobado = new XRLabel();
            lblAprobado.LocationF = new PointF(2 * detailTableWidth / 3, footerLine.LocationF.Y + 100);
            lblAprobado.SizeF = new SizeF(detailTableWidth / 3, 30);
            lblAprobado.Font = new Font("Arial", 10);
            lblAprobado.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            lblAprobado.Text = "APROBADO";
            documentoFooter.Controls.Add(lblAprobado);

            XRLabel lblAprobado_blank = new XRLabel();
            lblAprobado_blank.LocationF = new PointF(lblAprobado.LocationF.X, lblAprobado.LocationF.Y + lblAprobado.HeightF);
            lblAprobado_blank.SizeF = new SizeF(detailTableWidth / 3, 50);
            lblAprobado_blank.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            documentoFooter.Controls.Add(lblAprobado_blank);

            //XRLabel footerLable_Autorizacion = new XRLabel();
            //footerLable_Autorizacion.LocationF = new PointF(2 * detailTableWidth / 3, footerLable_Atentamente.LocationF.Y);
            //footerLable_Autorizacion.SizeF = new SizeF(detailTableWidth / 3, 30);
            //footerLable_Autorizacion.Font = new Font("Arial", 10, FontStyle.Bold);
            //footerLable_Autorizacion.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            //footerLable_Autorizacion.Text = "AUTORIZACIÓN GERENCIA";
            //documentoFooter.Controls.Add(footerLable_Autorizacion);

            //XRLabel lblJefe_Value = new XRLabel();
            //lblJefe_Value.LocationF = new PointF(30, footerLable_Atentamente.LocationF.Y + footerLable_Atentamente.HeightF + 50);
            //lblJefe_Value.SizeF = new SizeF(detailTableWidth / 3, 30);
            //lblJefe_Value.Font = new Font("Arial", 10, FontStyle.Bold);
            //lblJefe_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
            ////lblJefe_Value.DataBindings.Add();
            //documentoFooter.Controls.Add(lblJefe_Value);

            //XRLabel lblJefe_Name = new XRLabel();
            //lblJefe_Name.LocationF = new PointF(lblJefe_Value.LocationF.X, lblJefe_Value.LocationF.Y + lblJefe_Value.HeightF);
            //lblJefe_Name.SizeF = lblJefe_Value.SizeF;
            //lblJefe_Name.Font = new Font("Arial", 10, FontStyle.Bold);
            //lblJefe_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            //lblJefe_Name.Text = "Jefe Administrativa";
            //documentoFooter.Controls.Add(lblJefe_Name);

            //XRLabel lblGerente_Value = new XRLabel();
            //lblGerente_Value.LocationF = new PointF(2 * detailTableWidth / 3, lblJefe_Value.LocationF.Y);
            //lblGerente_Value.SizeF = new SizeF(detailTableWidth / 3, 30);
            //lblGerente_Value.Font = new Font("Arial", 10, FontStyle.Bold);
            //lblGerente_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
            ////lblGerente_Value.DataBindings.Add();
            //documentoFooter.Controls.Add(lblGerente_Value);

            //XRLabel lblGerente_Name = new XRLabel();
            //lblGerente_Name.LocationF = new PointF(lblGerente_Value.LocationF.X, lblGerente_Value.LocationF.Y + lblGerente_Value.HeightF);
            //lblGerente_Name.SizeF = lblGerente_Value.SizeF;
            //lblGerente_Name.Font = new Font("Arial", 10, FontStyle.Bold);
            //lblGerente_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            //lblGerente_Name.Text = "Gerente";
            //documentoFooter.Controls.Add(lblGerente_Name);
            #endregion
            #endregion

        }       
        #endregion
    }
}

