using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting.Shape;
using NewAge.DTO.Reportes;
using System.Collections.Generic;
using NewAge.Librerias.Project;
using NewAge.Cliente.GUI.WinApp.Clases;

namespace NewAge.Cliente.GUI.WinApp.Reports.Formularios
{
    public partial class FormularioRTEFTE : DevExpress.XtraReports.UI.XtraReport
    {
        #region Variables
        BaseController _bc = BaseController.GetInstance(); 
        #endregion

        #region Funciones Publicas
        /// <summary>
        /// Formulario "Declaracion mensual de retenciones en la fuente" Constructor
        /// </summary>
        /// <param name="formData">Data for the Formulario</param>
        /// <param name="Date">Period of the Formulario</param>
        public FormularioRTEFTE(DTO_Formularios formData, int yearFisc, int period, bool preInd)
        {
            #region Formulario bands
            InitializeComponent();

            this.Margins = new System.Drawing.Printing.Margins(50, 50, 50, 50);

            DetailReportBand formularioBand;
            formularioBand = new DetailReportBand();

            DetailBand formularioDetail;
            formularioDetail = new DetailBand();
            formularioDetail.Name = "formularioDetail";
            formularioDetail.HeightF = this.PageHeight - this.Margins.Top - this.Margins.Bottom;
            formularioBand.Bands.Add(formularioDetail);

            this.Bands.Add(formularioBand);
            #endregion

            #region Formulario styles

            XRControlStyle tableStyle = new XRControlStyle()
            {
                Name = "tableStyle",
                BackColor = Color.Transparent,
                ForeColor = Color.Black,
                Font = new Font("Arial", 6, FontStyle.Regular),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0)
            };

            this.StyleSheet.Add(tableStyle);

            #endregion

            float formularioWidth = this.PageWidth - this.Margins.Left - this.Margins.Right - 1;
            float frameShift = 3;
            float labelShift_thick = 2;
            float rowHeight = 15;
            Color oddRowColor = Color.FromArgb(0xE4, 0xBD, 0xBD);
            Color evenRowColor = Color.Transparent;

            #region Formulario part 1
            XRShape logoFrame = new XRShape();
            logoFrame.LocationF = new PointF(0, 0);
            logoFrame.SizeF = new SizeF(formularioWidth / 5, 50);
            logoFrame.LineWidth = 2;
            logoFrame.BackColor = Color.Transparent;
            logoFrame.ForeColor = Color.FromArgb(0xDB, 0x53, 0x53);
            logoFrame.Shape = new ShapeRectangle()
            {
                Fillet = 15,
            };
            //logoFrame.SendToBack();
            logoFrame.AnchorVertical = VerticalAnchorStyles.Top;
            formularioDetail.Controls.Add(logoFrame);

            XRLabel logoLabel = new XRLabel();
            logoLabel.LocationF = logoFrame.LocationF;
            logoLabel.SizeF = logoFrame.SizeF;
            logoLabel.ForeColor = Color.Black;
            logoLabel.BackColor = Color.Transparent;
            logoLabel.Font = new Font("Arial", 20, FontStyle.Bold);
            logoLabel.Text = "DIAN";
            logoLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            formularioDetail.Controls.Add(logoLabel);

            XRShape titleFrame = new XRShape();
            titleFrame.LocationF = new PointF(logoFrame.WidthF - frameShift, 0);
            titleFrame.SizeF = new SizeF(2 * formularioWidth / 5 + frameShift, 50);
            titleFrame.LineWidth = 2;
            titleFrame.BackColor = Color.Transparent;
            titleFrame.ForeColor = Color.FromArgb(0xDB, 0x53, 0x53);
            titleFrame.Shape = new ShapeRectangle()
            {
                Fillet = 14,
            };
            //titleFrame.SendToBack();
            titleFrame.AnchorVertical = VerticalAnchorStyles.Top;
            formularioDetail.Controls.Add(titleFrame);

            XRLabel titleLabel = new XRLabel();
            titleLabel.LocationF = titleFrame.LocationF;
            titleLabel.SizeF = titleFrame.SizeF;
            titleLabel.ForeColor = Color.Black;
            titleLabel.BackColor = Color.Transparent;
            titleLabel.Font = new Font("Arial Narrow", 8, FontStyle.Bold);
            titleLabel.Text = "Declaración Mensual de Retenciones en la Fuente";
            titleLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            titleLabel.BringToFront();
            formularioDetail.Controls.Add(titleLabel);

            XRShape privadaFrame = new XRShape();
            privadaFrame.LocationF = new PointF(titleFrame.LocationF.X + titleFrame.WidthF - frameShift, 0);
            privadaFrame.SizeF = new SizeF(formularioWidth / 5 + frameShift, 50);
            privadaFrame.LineWidth = 2;
            privadaFrame.BackColor = Color.Transparent;
            privadaFrame.ForeColor = Color.FromArgb(0xDB, 0x53, 0x53);
            privadaFrame.Shape = new ShapeRectangle()
            {
                Fillet = 15,
            };
            //privadaFrame.SendToBack();
            privadaFrame.AnchorVertical = VerticalAnchorStyles.Top;
            formularioDetail.Controls.Add(privadaFrame);

            XRLabel privadaLabel = new XRLabel();
            privadaLabel.LocationF = new PointF(privadaFrame.LocationF.X + labelShift_thick, privadaFrame.LocationF.Y + labelShift_thick);
            privadaLabel.SizeF = new SizeF(privadaFrame.WidthF - 2 * labelShift_thick, privadaFrame.HeightF - 2 * labelShift_thick);
            privadaLabel.ForeColor = Color.Black;
            privadaLabel.BackColor = Color.FromArgb(0xE7, 0xAB, 0xAB);
            privadaLabel.Font = new Font("Arial Narrow", 9, FontStyle.Bold);
            privadaLabel.Text = "Privada";
            privadaLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            privadaLabel.SendToBack();
            formularioDetail.Controls.Add(privadaLabel);

            XRShape numeroFrame = new XRShape();
            numeroFrame.LocationF = new PointF(privadaFrame.LocationF.X + privadaFrame.WidthF - frameShift, 0);
            numeroFrame.SizeF = new SizeF(formularioWidth / 5 + frameShift, 50);
            numeroFrame.LineWidth = 2;
            numeroFrame.BackColor = Color.Transparent;
            numeroFrame.ForeColor = Color.FromArgb(0xDB, 0x53, 0x53);
            numeroFrame.Shape = new ShapeRectangle()
            {
                Fillet = 15,

            };
            //numeroFrame.SendToBack();
            numeroFrame.AnchorVertical = VerticalAnchorStyles.Top;
            formularioDetail.Controls.Add(numeroFrame);

            XRShape numeroFrame_2 = new XRShape();
            numeroFrame_2.LocationF = new PointF(numeroFrame.LocationF.X + labelShift_thick, numeroFrame.LocationF.Y + labelShift_thick);
            numeroFrame_2.SizeF = new SizeF(numeroFrame.WidthF - 2 * labelShift_thick, numeroFrame.HeightF - 2 * labelShift_thick);
            numeroFrame_2.LineWidth = 2;
            numeroFrame_2.BackColor = Color.Transparent;
            numeroFrame_2.ForeColor = Color.White;
            numeroFrame_2.Shape = new ShapeRectangle()
            {
                Fillet = 15,

            };
            numeroFrame_2.BringToFront();
            numeroFrame_2.AnchorVertical = VerticalAnchorStyles.Top;
            formularioDetail.Controls.Add(numeroFrame_2);

            XRLabel numeroLabel = new XRLabel();
            numeroLabel.LocationF = numeroFrame_2.LocationF;
            numeroLabel.SizeF = numeroFrame_2.SizeF;
            numeroLabel.ForeColor = Color.White;
            numeroLabel.BackColor = Color.FromArgb(0xDB, 0x53, 0x53);
            numeroLabel.Font = new Font("Arial", 27, FontStyle.Bold);
            numeroLabel.Text = "350";
            numeroLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            numeroLabel.SendToBack();
            formularioDetail.Controls.Add(numeroLabel);
            #endregion

            #region Formulario part 2
            XRShape frame_1_4 = new XRShape();
            frame_1_4.LocationF = new PointF(0, logoFrame.HeightF - frameShift);
            frame_1_4.SizeF = new SizeF(formularioWidth, 140 + frameShift);
            frame_1_4.LineWidth = 2;
            frame_1_4.BackColor = Color.Transparent;
            frame_1_4.ForeColor = Color.FromArgb(0xDB, 0x53, 0x53);
            frame_1_4.Shape = new ShapeRectangle()
            {
                Fillet = 10,
            };
            //periodoFrame.SendToBack();
            frame_1_4.AnchorVertical = VerticalAnchorStyles.Top;
            formularioDetail.Controls.Add(frame_1_4);

            #region Left part

            XRPanel Panel_1_3 = new XRPanel();
            Panel_1_3.LocationF = new PointF(frame_1_4.LocationF.X + labelShift_thick, frame_1_4.LocationF.Y + labelShift_thick);
            Panel_1_3.SizeF = new SizeF(frame_1_4.WidthF / 2 - labelShift_thick, 20);
            Panel_1_3.Borders = DevExpress.XtraPrinting.BorderSide.None;
            Panel_1_3.SnapLineMargin = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0);
            Panel_1_3.BackColor = oddRowColor;
            Panel_1_3.ForeColor = Color.Transparent;
            formularioDetail.Controls.Add(Panel_1_3);

            XRLabel yearLabel = new XRLabel();
            yearLabel.LocationF = Panel_1_3.LocationF;
            yearLabel.SizeF = new SizeF(50, Panel_1_3.HeightF);
            yearLabel.ForeColor = Color.Black;
            yearLabel.BackColor = Color.Transparent;
            yearLabel.Font = new Font("Arial", 6, FontStyle.Regular);
            yearLabel.Text = "1. Año";
            yearLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            yearLabel.BringToFront();
            formularioDetail.Controls.Add(yearLabel);

            char[] year = yearFisc.ToString().ToCharArray();

            XRTable yearTable = new XRTable();
            XRTableRow yearTableRow;
            XRTableCell yearTableCell;
            yearTable.LocationF = new PointF(yearLabel.LocationF.X + yearLabel.WidthF, yearLabel.LocationF.Y + labelShift_thick);
            yearTable.HeightF = Panel_1_3.HeightF - 2 * labelShift_thick;
            yearTable.WidthF = 4 * yearTable.HeightF;
            yearTable.ForeColor = Color.Black;
            yearTable.BackColor = Color.White;
            yearTable.Borders = DevExpress.XtraPrinting.BorderSide.All;
            yearTable.BringToFront();
            yearTableRow = new XRTableRow();
            for (int i = 0; i < 4; i++)
            {
                yearTableCell = new XRTableCell();
                yearTableCell.Font = new System.Drawing.Font("Courier new", 8);
                yearTableCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                yearTableCell.Text = year[i].ToString();
                yearTableRow.Cells.Add(yearTableCell);
            }
            yearTable.Rows.Add(yearTableRow);
            formularioDetail.Controls.Add(yearTable);

            char[] month = (period.ToString().Length == 1) ? ("0" + period.ToString()).ToCharArray() : period.ToString().ToCharArray();

            XRTable periodTable = new XRTable();
            XRTableRow periodTableRow;
            XRTableCell periodTableCell;
            periodTable.HeightF = yearTable.HeightF;
            periodTable.WidthF = 2 * periodTable.HeightF;
            periodTable.LocationF = new PointF(Panel_1_3.WidthF - periodTable.WidthF - 10, yearTable.LocationF.Y);
            periodTable.ForeColor = Color.Black;
            periodTable.BackColor = Color.White;
            periodTable.Borders = DevExpress.XtraPrinting.BorderSide.All;
            periodTable.BringToFront();
            periodTableRow = new XRTableRow();
            for (int i = 0; i < 2; i++)
            {
                periodTableCell = new XRTableCell();
                periodTableCell.Font = new System.Drawing.Font("Courier new", 8);
                periodTableCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                periodTableCell.Text = month[i].ToString();
                periodTableRow.Cells.Add(periodTableCell);
            }
            periodTable.Rows.Add(periodTableRow);
            formularioDetail.Controls.Add(periodTable);

            XRLabel periodoLabel = new XRLabel();
            periodoLabel.SizeF = new SizeF(60, yearLabel.HeightF);
            periodoLabel.LocationF = new PointF(periodTable.LocationF.X - periodoLabel.WidthF, yearLabel.LocationF.Y);
            periodoLabel.ForeColor = Color.Black;
            periodoLabel.BackColor = Color.Transparent;
            periodoLabel.Font = new Font("Arial", 6, FontStyle.Regular);
            periodoLabel.Text = "3. Periodo";
            periodoLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            periodoLabel.BringToFront();
            formularioDetail.Controls.Add(periodoLabel);

            Panel_1_3.SendToBack();

            XRLabel leaLabel = new XRLabel();
            leaLabel.SizeF = new SizeF(Panel_1_3.WidthF, 20);
            leaLabel.LocationF = new PointF(yearLabel.LocationF.X, frame_1_4.LocationF.Y + frame_1_4.HeightF - leaLabel.HeightF);
            leaLabel.ForeColor = Color.Black;
            leaLabel.BackColor = Color.Transparent;
            leaLabel.Font = new Font("Arial", 7, FontStyle.Regular);
            leaLabel.Text = "Lea cuidadosamente las instrucciones";
            leaLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            formularioDetail.Controls.Add(leaLabel);

            XRLabel colombiaLabel = new XRLabel();
            colombiaLabel.LocationF = new PointF(yearLabel.LocationF.X, yearLabel.LocationF.Y + yearLabel.HeightF);
            colombiaLabel.SizeF = new SizeF(Panel_1_3.WidthF, frame_1_4.HeightF - Panel_1_3.HeightF - leaLabel.HeightF);
            colombiaLabel.ForeColor = Color.Black;
            colombiaLabel.BackColor = Color.Transparent;
            colombiaLabel.Font = new Font("Arial", 12, FontStyle.Bold);
            colombiaLabel.Multiline = true;
            colombiaLabel.Text = "Colombia\r\nun compromisoque no podemos evadir";
            colombiaLabel.Padding = new DevExpress.XtraPrinting.PaddingInfo(20, 20, 0, 0);
            colombiaLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            formularioDetail.Controls.Add(colombiaLabel);
            #endregion

            #region Right part
            XRPanel Panel_status = new XRPanel();
            Panel_status.LocationF = new PointF(Panel_1_3.LocationF.X + Panel_1_3.WidthF, Panel_1_3.LocationF.Y);
            Panel_status.SizeF = new SizeF(frame_1_4.WidthF / 2 - labelShift_thick, 20);
            Panel_status.Borders = DevExpress.XtraPrinting.BorderSide.Bottom | DevExpress.XtraPrinting.BorderSide.Left;
            Panel_status.BackColor = oddRowColor;
            Panel_status.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            Panel_status.BorderWidth = 2;
            formularioDetail.Controls.Add(Panel_status);

            XRLabel statusLabel = new XRLabel();
            statusLabel.LocationF = Panel_status.LocationF;
            statusLabel.SizeF = Panel_status.SizeF;
            statusLabel.ForeColor = Color.Black;
            statusLabel.BackColor = Color.Transparent;
            statusLabel.Font = new Font("Arial", 9, FontStyle.Bold | FontStyle.Italic);
            statusLabel.Text = preInd ? _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.coFormularios).ToString() + "_Preliminar") : _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.coFormularios).ToString() + "_Definitivo");
            statusLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            statusLabel.BringToFront();
            formularioDetail.Controls.Add(statusLabel);

            XRPanel Panel_4 = new XRPanel();
            Panel_4.LocationF = Panel_status.LocationF;
            Panel_4.SizeF = new SizeF(Panel_1_3.WidthF, frame_1_4.HeightF - 2 * labelShift_thick);
            Panel_4.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            Panel_4.BorderWidth = 1;
            Panel_4.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            Panel_4.SnapLineMargin = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0);
            Panel_4.BackColor = Color.Transparent;
            Panel_4.SendToBack();
            formularioDetail.Controls.Add(Panel_4);

            Panel_status.SendToBack();

            XRLabel numeroFormularioLabel = new XRLabel();
            numeroFormularioLabel.LocationF = new PointF(Panel_4.LocationF.X + 10, Panel_4.LocationF.Y + 30);
            numeroFormularioLabel.SizeF = new SizeF(150, 20);
            numeroFormularioLabel.CanShrink = true;
            numeroFormularioLabel.ForeColor = Color.Black;
            numeroFormularioLabel.BackColor = Color.Transparent;
            numeroFormularioLabel.Font = new Font("Arial", 7, FontStyle.Regular);
            numeroFormularioLabel.Text = "4. Número de formulario";
            numeroFormularioLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            formularioDetail.Controls.Add(numeroFormularioLabel);
            #endregion
            #endregion

            #region Formulario part 3
            XRShape frame_5_26 = new XRShape();
            frame_5_26.LocationF = new PointF(0, frame_1_4.LocationF.Y + frame_1_4.HeightF - frameShift);
            frame_5_26.SizeF = new SizeF(formularioWidth, 94);
            frame_5_26.LineWidth = 2;
            frame_5_26.BackColor = Color.Transparent;
            frame_5_26.ForeColor = Color.FromArgb(0xDB, 0x53, 0x53);
            frame_5_26.Shape = new ShapeRectangle()
            {
                Fillet = 10,
            };
            //datosDelDeclaranteFrame.SendToBack();
            frame_5_26.AnchorVertical = VerticalAnchorStyles.Top;
            formularioDetail.Controls.Add(frame_5_26);

            XRLabel datosLabel = new XRLabel();
            datosLabel.LocationF = frame_5_26.LocationF;
            datosLabel.SizeF = new SizeF(25, 5 * rowHeight);
            datosLabel.ForeColor = Color.Black;
            datosLabel.BackColor = Color.Transparent;
            datosLabel.Font = new Font("Arial", 6, FontStyle.Bold);
            datosLabel.Multiline = true;
            datosLabel.Text = "Datos del\r\ndeclarante";
            datosLabel.Angle = 90;
            datosLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            formularioDetail.Controls.Add(datosLabel);

            #region Table 1
            XRPanel Panel_5_6 = new XRPanel();
            Panel_5_6.LocationF = new PointF(datosLabel.LocationF.X + datosLabel.WidthF, datosLabel.LocationF.Y + labelShift_thick);
            Panel_5_6.SizeF = new SizeF(frame_5_26.WidthF / 3 - datosLabel.WidthF, 2 * rowHeight);
            Panel_5_6.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom;
            Panel_5_6.BorderWidth = 1;
            Panel_5_6.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            Panel_5_6.BackColor = Color.Transparent;
            Panel_5_6.BringToFront();
            formularioDetail.Controls.Add(Panel_5_6);

            XRTable table_5_6 = new XRTable();
            XRTableRow table_5_6_Row;
            XRTableCell table_5_6_Cell;
            table_5_6.LocationF = Panel_5_6.LocationF;
            table_5_6.SizeF = Panel_5_6.SizeF;
            table_5_6.StyleName = "tableStyle";
            table_5_6.BeginInit();
            table_5_6_Row = new XRTableRow();
            table_5_6_Row.HeightF = rowHeight;
            table_5_6_Row.BackColor = oddRowColor;
            table_5_6_Cell = new XRTableCell()
            {
                WidthF = table_5_6.WidthF - 30,
                Text = "5. Número de Identificación Tributaria (NIT)"
            };
            table_5_6_Row.Cells.Add(table_5_6_Cell);
            table_5_6_Cell = new XRTableCell()
            {
                WidthF = 30,
                Text = "6. DV",
                WordWrap = false,
                CanGrow = false
            };
            table_5_6_Row.Cells.Add(table_5_6_Cell);
            table_5_6.Rows.Add(table_5_6_Row);
            table_5_6_Row = new XRTableRow();
            table_5_6_Row.HeightF = rowHeight;
            table_5_6_Row.BackColor = evenRowColor;
            char[] NIT = formData.FormDecHeader.Nit.ToCharArray();
            int start = 14 - NIT.Length;
            for (int i = 0; i < 14; i++)
            {
                table_5_6_Cell = new XRTableCell()
                {
                    WidthF = (table_5_6.WidthF - 30)/14,
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                    Text = (i >= start) ? NIT[i - start].ToString() : string.Empty,
                    Font = new Font("Arial", 7)
                };
                table_5_6_Row.Cells.Add(table_5_6_Cell);
            }
            table_5_6_Cell = new XRTableCell()
            {
                WidthF = 30,
                Borders = DevExpress.XtraPrinting.BorderSide.Left,
                BorderColor = Color.FromArgb(0xDB, 0x53, 0x53),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                Text = formData.FormDecHeader.DV,
                Font = new Font("Arial", 7)
            };
            table_5_6_Row.Cells.Add(table_5_6_Cell);
            table_5_6.Rows.Add(table_5_6_Row);
            table_5_6.EndInit();
            table_5_6.SendToBack();
            formularioDetail.Controls.Add(table_5_6);

            XRTable table_5_6_division = new XRTable();
            XRTableRow table_5_6_division_Row;
            XRTableCell table_5_6_division_Cell;
            table_5_6_division.LocationF = new PointF(table_5_6.LocationF.X, table_5_6.LocationF.Y + 5 * rowHeight / 3);
            table_5_6_division.SizeF = new SizeF(table_5_6.WidthF - 30 - (table_5_6.WidthF-30) / 14, rowHeight / 3);
            table_5_6_division.BeginInit();
            table_5_6_division_Row = new XRTableRow();
            table_5_6_division_Row.HeightF = rowHeight / 3;
            table_5_6_division_Row.BackColor = evenRowColor;
            for (int i = 0; i < 13; i++)
            {
                table_5_6_division_Cell = new XRTableCell()
                {
                    WidthF = table_5_6.WidthF / 14,
                    Borders = DevExpress.XtraPrinting.BorderSide.Right,
                    BorderColor = Color.FromArgb(0xDB, 0x53, 0x53)
                };
                table_5_6_division_Row.Cells.Add(table_5_6_division_Cell);
            };

            table_5_6_division.Rows.Add(table_5_6_division_Row);
            table_5_6_division.EndInit();
            table_5_6_division.SendToBack();
            formularioDetail.Controls.Add(table_5_6_division);
            #endregion

            #region Table 2
            XRPanel Panel_7_10 = new XRPanel();
            Panel_7_10.LocationF = new PointF(table_5_6.LocationF.X + table_5_6.WidthF, table_5_6.LocationF.Y);
            Panel_7_10.SizeF = new SizeF(2 * frame_5_26.WidthF / 3 - labelShift_thick, 2 * rowHeight);
            Panel_7_10.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom;
            Panel_7_10.BorderWidth = 1;
            Panel_7_10.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            Panel_7_10.BackColor = Color.Transparent;
            Panel_7_10.BringToFront();
            formularioDetail.Controls.Add(Panel_7_10);

            XRTable table_7_10 = new XRTable();
            XRTableRow table_7_10_Row;
            XRTableCell table_7_10_Cell;
            table_7_10.LocationF = Panel_7_10.LocationF;
            table_7_10.SizeF = Panel_7_10.SizeF;
            table_7_10.StyleName = "tableStyle";
            table_7_10.BeginInit();
            table_7_10.CanGrow = false;
            table_7_10.AnchorVertical = VerticalAnchorStyles.Both;
            table_7_10_Row = new XRTableRow();
            table_7_10_Row.HeightF = rowHeight;
            table_7_10_Row.BackColor = oddRowColor;
            table_7_10_Cell = new XRTableCell()
            {
                WidthF = table_7_10.WidthF / 4,
                Text = "7. Primer apellido"
            };
            table_7_10_Row.Cells.Add(table_7_10_Cell);
            table_7_10_Cell = new XRTableCell()
            {
                WidthF = table_7_10.WidthF / 4,
                Text = "8. Segundo apellido"
            };
            table_7_10_Row.Cells.Add(table_7_10_Cell);
            table_7_10_Cell = new XRTableCell()
            {
                WidthF = table_7_10.WidthF / 4,
                Text = "9. Primer nombre"
            };
            table_7_10_Row.Cells.Add(table_7_10_Cell);
            table_7_10_Cell = new XRTableCell()
            {
                WidthF = table_7_10.WidthF / 4,
                Text = "10. Otros nombres"
            };
            table_7_10_Row.Cells.Add(table_7_10_Cell);
            table_7_10.Rows.Add(table_7_10_Row);
            table_7_10_Row = new XRTableRow();
            table_7_10_Row.HeightF = rowHeight;
            table_7_10_Row.BackColor = evenRowColor;
            table_7_10_Cell = new XRTableCell()
            {
                WidthF = table_7_10.WidthF / 4,
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                Text = formData.FormDecHeader.ApellidoPri,
                Font = new Font("Arial", 7)
            };
            table_7_10_Row.Cells.Add(table_7_10_Cell);
            table_7_10_Cell = new XRTableCell()
            {
                WidthF = table_7_10.WidthF / 4,
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                Text = formData.FormDecHeader.ApellidoSdo,
                Font = new Font("Arial", 7)
            };
            table_7_10_Row.Cells.Add(table_7_10_Cell);
            table_7_10_Cell = new XRTableCell()
            {
                WidthF = table_7_10.WidthF / 4,
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                Text = formData.FormDecHeader.NombrePri,
                Font = new Font("Arial", 7)
            };
            table_7_10_Row.Cells.Add(table_7_10_Cell);
            table_7_10_Cell = new XRTableCell()
            {
                WidthF = table_7_10.WidthF / 4,
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                Text = formData.FormDecHeader.NombreOtr,
                Font = new Font("Arial", 7)
            };
            table_7_10_Row.Cells.Add(table_7_10_Cell);
            table_7_10.Rows.Add(table_7_10_Row);
            table_7_10.EndInit();
            table_7_10.SendToBack();
            formularioDetail.Controls.Add(table_7_10);

            XRTable table_7_10_division = new XRTable();
            XRTableRow table_7_10_division_Row;
            XRTableCell table_7_10_division_Cell;
            table_7_10_division.LocationF = new PointF(table_7_10.LocationF.X, table_7_10.LocationF.Y + 3 * rowHeight / 2);
            table_7_10_division.SizeF = new SizeF(table_7_10.WidthF - table_7_10.WidthF / 4, rowHeight / 2);
            table_7_10_division.BeginInit();
            table_7_10_division_Row = new XRTableRow();
            table_7_10_division_Row.HeightF = rowHeight / 2;
            table_7_10_division_Row.BackColor = evenRowColor;
            for (int i = 0; i < 3; i++)
            {
                table_7_10_division_Cell = new XRTableCell()
                {
                    WidthF = table_7_10.WidthF / 4,
                    Borders = DevExpress.XtraPrinting.BorderSide.Right,
                    BorderColor = Color.FromArgb(0xDB, 0x53, 0x53)
                };
                table_7_10_division_Row.Cells.Add(table_7_10_division_Cell);
            };

            table_7_10_division.Rows.Add(table_7_10_division_Row);
            table_7_10_division.EndInit();
            table_7_10_division.SendToBack();
            formularioDetail.Controls.Add(table_7_10_division);
            #endregion

            #region Table 3
            XRPanel Panel_11 = new XRPanel();
            Panel_11.LocationF = new PointF(table_5_6.LocationF.X, table_5_6.LocationF.Y + table_5_6.HeightF);
            Panel_11.SizeF = new SizeF(9 * frame_5_26.WidthF / 10 - datosLabel.WidthF, 2 * rowHeight);
            Panel_11.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom;
            Panel_11.BorderWidth = 1;
            Panel_11.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            Panel_11.BackColor = Color.Transparent;
            Panel_11.BringToFront();
            formularioDetail.Controls.Add(Panel_11);

            XRTable table_11 = new XRTable();
            XRTableRow table_11_Row;
            XRTableCell table_11_Cell;
            table_11.LocationF = Panel_11.LocationF;
            table_11.SizeF = Panel_11.SizeF;
            table_11.StyleName = "tableStyle";
            table_11.BeginInit();
            table_11_Row = new XRTableRow();
            table_11_Row.HeightF = rowHeight;
            table_11_Row.BackColor = oddRowColor;
            table_11_Cell = new XRTableCell()
            {
                WidthF = table_11.WidthF,
                Text = "5. Razón social"
            };
            table_11_Row.Cells.Add(table_11_Cell);
            table_11.Rows.Add(table_11_Row);

            table_11_Row = new XRTableRow();
            table_11_Row.HeightF = rowHeight;
            table_11_Row.BackColor = evenRowColor;
            table_11_Cell = new XRTableCell()
            {
                WidthF = table_11.WidthF,
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                Text = formData.FormDecHeader.RazonSoc,
                Font = new Font("Arial", 7)
            };
            table_11_Row.Cells.Add(table_11_Cell);
            table_11.Rows.Add(table_11_Row);
            table_11.EndInit();
            table_11.SendToBack();
            formularioDetail.Controls.Add(table_11);
            #endregion

            #region Table 4
            XRPanel Panel_12 = new XRPanel();
            Panel_12.LocationF = new PointF(table_11.LocationF.X + table_11.WidthF, table_11.LocationF.Y);
            Panel_12.SizeF = new SizeF(frame_5_26.WidthF / 10 - labelShift_thick, 2 * rowHeight);
            Panel_12.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom;
            Panel_12.BorderWidth = 1;
            Panel_12.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            Panel_12.SnapLineMargin = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0);
            Panel_12.BackColor = Color.Transparent;
            Panel_12.SendToBack();
            formularioDetail.Controls.Add(Panel_12);

            XRTable table_12 = new XRTable();
            XRTableRow table_12_Row;
            XRTableCell table_12_Cell;
            table_12.LocationF = Panel_12.LocationF;
            table_12.SizeF = Panel_12.SizeF;
            table_12.StyleName = "tableStyle";
            table_12.BeginInit();
            table_12_Row = new XRTableRow();
            table_12_Row.HeightF = rowHeight;
            table_12_Row.BackColor = oddRowColor;
            table_12_Cell = new XRTableCell()
            {
                WidthF = table_12.WidthF,
                Text = "",
                Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular),
                CanGrow = false,
            };
            table_12_Row.Cells.Add(table_12_Cell);
            table_12.Rows.Add(table_12_Row);

            table_12_Row = new XRTableRow();
            table_12_Row.HeightF = rowHeight;
            table_12_Row.BackColor = evenRowColor;
            table_12_Cell = new XRTableCell()
            {
                WidthF = table_12.WidthF,
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                Text = formData.FormDecHeader.CodigoDir,
                Font = new Font("Arial", 7)
            };
            table_12_Row.Cells.Add(table_12_Cell);
            table_12.Rows.Add(table_12_Row);
            table_12.EndInit();
            formularioDetail.Controls.Add(table_12);

            XRLabel label_12 = new XRLabel();
            label_12.LocationF = new PointF(table_12.LocationF.X, table_12.LocationF.Y);
            label_12.SizeF = new SizeF(table_12.WidthF, rowHeight + 10);
            label_12.ForeColor = Color.Black;
            label_12.BackColor = Color.Transparent;
            label_12.Font = new Font("Arial", 5, FontStyle.Regular);
            label_12.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0);
            label_12.Multiline = true;
            label_12.Text = "12. Cód. Dirección\r\nseccional";
            label_12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_12.BringToFront();
            formularioDetail.Controls.Add(label_12);

            table_12.SendToBack();
            #endregion

            #region Table 5
            XRPanel Panel_24 = new XRPanel();
            Panel_24.LocationF = new PointF(table_5_6.LocationF.X, table_11.LocationF.Y + table_11.HeightF);
            Panel_24.SizeF = new SizeF(frame_5_26.WidthF - datosLabel.WidthF - labelShift_thick, rowHeight);
            Panel_24.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            Panel_24.BorderWidth = 1;
            Panel_24.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            Panel_24.SnapLineMargin = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0);
            Panel_24.BackColor = Color.Transparent;
            Panel_24.BringToFront();
            formularioDetail.Controls.Add(Panel_24);

            XRLabel label_24 = new XRLabel();
            label_24.LocationF = Panel_24.LocationF;
            label_24.SizeF = new SizeF(180, rowHeight);
            //label_24.ForeColor = Color.Black;
            //label_24.BackColor = Color.Transparent;
            //label_24.Font = new Font("Arial", 6, FontStyle.Regular);
            //label_24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            label_24.StyleName = "tableStyle";
            label_24.Text = "24. Si es gran contribuyente, marque \"X\"";
            formularioDetail.Controls.Add(label_24);

            XRLabel label_24_blank = new XRLabel();
            label_24_blank.LocationF = new PointF(label_24.LocationF.X + label_24.WidthF, label_24.LocationF.Y + 1);
            label_24_blank.SizeF = new SizeF(15, label_24.HeightF - 2);
            label_24_blank.BackColor = Color.White;
            label_24_blank.Borders = DevExpress.XtraPrinting.BorderSide.All;
            label_24_blank.BringToFront();
            formularioDetail.Controls.Add(label_24_blank);

            #endregion

            #region Table 6
            XRPanel Panel_25_26 = new XRPanel();
            Panel_25_26.LocationF = new PointF(frame_5_26.LocationF.X + labelShift_thick, Panel_24.LocationF.Y + Panel_24.HeightF);
            Panel_25_26.SizeF = new SizeF(2 * frame_5_26.WidthF / 3, rowHeight);
            Panel_25_26.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            Panel_25_26.BorderWidth = 1;
            Panel_25_26.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            Panel_25_26.BackColor = Color.Transparent;
            Panel_25_26.BringToFront();
            formularioDetail.Controls.Add(Panel_25_26);

            XRTable table_25_26 = new XRTable();
            XRTableRow table_25_26_Row;
            XRTableCell table_25_26_Cell;
            table_25_26.LocationF = Panel_25_26.LocationF;
            table_25_26.SizeF = Panel_25_26.SizeF;
            table_25_26.StyleName = "tableStyle";
            table_25_26.BeginInit();
            table_25_26_Row = new XRTableRow();
            table_25_26_Row.HeightF = rowHeight;
            table_25_26_Row.BackColor = evenRowColor;

            table_25_26_Cell = new XRTableCell()
            {
                WidthF = 180,
                Text = "Si es una corrección indique:",
                Font = new Font("Arial", 6, FontStyle.Bold)
            };
            table_25_26_Row.Cells.Add(table_25_26_Cell);

            table_25_26_Cell = new XRTableCell()
            {
                WidthF = 50,
                Text = "25. Cód."
            };
            table_25_26_Row.Cells.Add(table_25_26_Cell);

            table_25_26_Cell = new XRTableCell()
            {
                WidthF = 70,
                ///////////// Datos
            };
            table_25_26_Row.Cells.Add(table_25_26_Cell);

            table_25_26_Cell = new XRTableCell()
            {
                WidthF = 180,
                Text = "26. No. Formulario anterior"
            };
            table_25_26_Row.Cells.Add(table_25_26_Cell);

            table_25_26_Cell = new XRTableCell()
            {
                WidthF = table_25_26.WidthF - 300,
                ///////////// Datos
            };
            table_25_26_Row.Cells.Add(table_25_26_Cell);

            table_25_26.Rows.Add(table_25_26_Row);
            table_25_26.EndInit();
            table_25_26.SendToBack();
            formularioDetail.Controls.Add(table_25_26);

            XRPanel Panel_25_26_blank = new XRPanel();
            Panel_25_26_blank.LocationF = new PointF(Panel_25_26.LocationF.X + Panel_25_26.WidthF, Panel_25_26.LocationF.Y);
            Panel_25_26_blank.SizeF = new SizeF(frame_5_26.WidthF - Panel_25_26.WidthF - 2 * labelShift_thick, rowHeight);
            Panel_25_26_blank.Borders = DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Left;
            Panel_25_26_blank.BorderWidth = 1;
            Panel_25_26_blank.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            Panel_25_26_blank.BackColor = Color.FromArgb(0xE7, 0xAB, 0xAB);
            formularioDetail.Controls.Add(Panel_25_26_blank);
            #endregion
            #endregion

            #region Formulario part 4

            #region Left part
            XRShape frame_27_44 = new XRShape();
            frame_27_44.LocationF = new PointF(0, frame_5_26.LocationF.Y + frame_5_26.HeightF - frameShift);
            frame_27_44.SizeF = new SizeF(formularioWidth / 2, 19 * rowHeight + 2 * labelShift_thick);
            frame_27_44.CanGrow = false;
            frame_27_44.LineWidth = 2;
            frame_27_44.BackColor = Color.Transparent;
            frame_27_44.ForeColor = Color.FromArgb(0xDB, 0x53, 0x53);
            frame_27_44.Shape = new ShapeRectangle()
            {
                Fillet = 5,
            };
            frame_27_44.CanGrow = false;
            frame_27_44.AnchorVertical = VerticalAnchorStyles.Top;
            formularioDetail.Controls.Add(frame_27_44);

            XRLabel label_27_44 = new XRLabel();
            label_27_44.LocationF = new PointF(frame_27_44.LocationF.X + labelShift_thick, frame_27_44.LocationF.Y + labelShift_thick);
            label_27_44.SizeF = new SizeF(frame_27_44.WidthF - 2 * labelShift_thick, rowHeight);
            label_27_44.ForeColor = Color.Black;
            label_27_44.BackColor = Color.Transparent;
            label_27_44.Font = new Font("Arial", 6, FontStyle.Regular);
            label_27_44.Text = "Retenciones practicadas a título de renta y complementarios";
            label_27_44.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            label_27_44.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            label_27_44.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            label_27_44.SendToBack();
            formularioDetail.Controls.Add(label_27_44);

            #region Table

            XRTable table_27_44 = new XRTable();
            XRTableRow table_27_44_Row;
            XRTableCell table_27_44_Cell;
            table_27_44.Name = "table_27_44";
            table_27_44.LocationF = new PointF(label_27_44.LocationF.X, label_27_44.LocationF.Y + label_27_44.HeightF);
            table_27_44.SizeF = new SizeF(label_27_44.WidthF, 18 * rowHeight);
            table_27_44.StyleName = "tableStyle";
            table_27_44.BeginInit();

            for (int i = 27; i < 45; i++)
            {
                table_27_44_Row = new XRTableRow();
                table_27_44_Row.HeightF = rowHeight;
                table_27_44_Row.BackColor = (i % 2 != 0) ? oddRowColor : evenRowColor;

                table_27_44_Cell = new XRTableCell();
                table_27_44_Cell.WidthF = (table_27_44.WidthF - 20) / 2;
                switch (i)
                {
                    case 27:
                        table_27_44_Cell.Text = "Salarios y demás pagos laborales";
                        break;
                    case 28:
                        table_27_44_Cell.Text = "Dividendos i participaciones";
                        break;
                    case 29:
                        table_27_44_Cell.Text = "Rendimientos financieros";
                        break;
                    case 30:
                        table_27_44_Cell.Text = "Loterías, rifas, apuestas y similares";
                        break;
                    case 31:
                        table_27_44_Cell.Text = "Honorarios";
                        break;
                    case 32:
                        table_27_44_Cell.Text = "Comisiones";
                        break;
                    case 33:
                        table_27_44_Cell.Text = "Servisios";
                        break;
                    case 34:
                        table_27_44_Cell.Text = "Pagos al exterior renta (Pagos o abonos en"; ///////////////////
                        table_27_44_Cell.CanGrow = false;
                        table_27_44_Cell.Font = new Font("Arrial", 5);
                        table_27_44_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;
                    case 35:
                        table_27_44_Cell.Text = "Compras";
                        break;
                    case 36:
                        table_27_44_Cell.Text = "Arrendamientos (Muebles e inmuebles)";
                        break;
                    case 37:
                        table_27_44_Cell.Text = "Enajenaciónes de activos fijos de personas naturales"; /////////////////
                        table_27_44_Cell.CanGrow = false;
                        table_27_44_Cell.Font = new Font("Arrial", 5);
                        table_27_44_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;
                    case 38:
                        table_27_44_Cell.Text = "Retención sobre ingresos de tarjetas débito y crédito"; //////////////////////////
                        table_27_44_Cell.CanGrow = false;
                        table_27_44_Cell.Font = new Font("Arrial", 5);
                        break;
                    case 39:
                        table_27_44_Cell.Text = "Otras retenciones";
                        break;
                    case 40:
                        table_27_44_Cell.Text = "Ventas";
                        table_27_44_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(25, 0, 0, 0);
                        break;
                    case 41:
                        table_27_44_Cell.Text = "Servicios";
                        table_27_44_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(25, 0, 0, 0);
                        break;
                    case 42:
                        table_27_44_Cell.Text = "Rendimientos financieros";
                        table_27_44_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(25, 0, 0, 0);
                        break;
                    case 43:
                        table_27_44_Cell.Text = "Otros conceptos";
                        table_27_44_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(25, 0, 0, 0);
                        break;
                    case 44:
                        table_27_44_Cell.Text = "Total retenciones renta y complementarios";  ////////////////////
                        table_27_44_Cell.CanGrow = false;
                        table_27_44_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        table_27_44_Row.Font = new Font("Arial", 6, FontStyle.Bold);
                        table_27_44_Cell.Font = new Font("Arial", 5, FontStyle.Bold);
                        break;
                };
                table_27_44_Row.Cells.Add(table_27_44_Cell);

                table_27_44_Cell = new XRTableCell()
                            {
                                WidthF = 20,
                                Text = i.ToString(),
                                Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right,
                                BorderColor = Color.FromArgb(0xDB, 0x53, 0x53),
                            };
                table_27_44_Row.Cells.Add(table_27_44_Cell);

                table_27_44_Cell = new XRTableCell();
                table_27_44_Cell.WidthF = (table_27_44.WidthF - 20) / 2;
                table_27_44_Cell.CanGrow = false;
                foreach (DTO_FormDecDetail item in formData.FormDecDetail)
                {
                    if (Convert.ToInt32(item.Renglon.Trim()) == i)
                        table_27_44_Cell.Text = (Math.Round(item.ValorML, 0)).ToString("#,0.");
                };
                table_27_44_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                table_27_44_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0);
                table_27_44_Cell.Font = new System.Drawing.Font("Courier new", 7);
                table_27_44_Row.Cells.Add(table_27_44_Cell);
                table_27_44.Rows.Add(table_27_44_Row);
            };
            table_27_44.EndInit();
            formularioDetail.Controls.Add(table_27_44);

            XRLabel label_40_43 = new XRLabel();
            label_40_43.LocationF = new PointF(table_27_44.LocationF.X, table_27_44.LocationF.Y + 13 * rowHeight);
            label_40_43.SizeF = new SizeF(20, 4 * rowHeight);
            label_40_43.ForeColor = Color.Black;
            label_40_43.BackColor = Color.White;
            label_40_43.Font = new Font("Arial Narrow", 6, FontStyle.Regular);
            label_40_43.Text = "Autorretenciones";
            label_40_43.Angle = 90;
            label_40_43.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            label_40_43.SendToBack();
            formularioDetail.Controls.Add(label_40_43);

            XRLabel label_34 = new XRLabel();
            label_34.LocationF = new PointF(table_27_44.LocationF.X, table_27_44.LocationF.Y + 7 * rowHeight - 1);
            label_34.SizeF = new SizeF((table_27_44.WidthF - 20) / 2, 25);
            label_34.ForeColor = Color.Black;
            label_34.BackColor = Color.Transparent;
            label_34.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_34.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_34.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0);
            label_34.Multiline = true;
            label_34.Text = "\r\ncuenta a residentes o domiciliados en el exterior)";
            formularioDetail.Controls.Add(label_34);

            XRLabel label_37 = new XRLabel();
            label_37.LocationF = new PointF(table_27_44.LocationF.X, table_27_44.LocationF.Y + 10 * rowHeight - 1);
            label_37.SizeF = new SizeF((table_27_44.WidthF - 20) / 2, 25);
            label_37.ForeColor = Color.Black;
            label_37.BackColor = Color.Transparent;
            label_37.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_37.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_37.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0);
            label_37.Multiline = true;
            label_37.Text = "\r\nante notarios y autoridades de tránsito";
            formularioDetail.Controls.Add(label_37);

            //XRLabel label_38 = new XRLabel();
            //label_38.LocationF = new PointF(table_27_44.LocationF.X, table_27_44.LocationF.Y + 11 * rowHeight - 1);
            //label_38.SizeF = new SizeF((table_27_44.WidthF - 20) / 2, 25);
            //label_38.ForeColor = Color.Black;
            //label_38.BackColor = Color.Transparent;
            //label_38.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            //label_38.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            //label_38.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0);
            //label_38.Text = "Retención sobre ingresos de tarjetas débito y crédito";
            //formularioDetail.Controls.Add(label_38);

            XRLabel label_44 = new XRLabel();
            label_44.LocationF = new PointF(table_27_44.LocationF.X, table_27_44.LocationF.Y + 17 * rowHeight - 2);
            label_44.SizeF = new SizeF((table_27_44.WidthF - 20) / 2, 25);
            label_44.ForeColor = Color.Black;
            label_44.BackColor = Color.Transparent;
            label_44.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_44.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_44.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0);
            label_44.Multiline = true;
            label_44.Text = "\r\n(Sume 27 a 43)";
            formularioDetail.Controls.Add(label_44);

            table_27_44.SendToBack();
            #endregion

            #endregion

            #region Right part

            #region Table 1
            XRShape frame_45_51 = new XRShape();
            frame_45_51.LocationF = new PointF(frame_27_44.WidthF - frameShift, frame_27_44.LocationF.Y);
            frame_45_51.SizeF = new SizeF(formularioWidth / 2 + frameShift, 9 * rowHeight + frameShift);
            frame_45_51.AnchorVertical = VerticalAnchorStyles.Top;
            frame_45_51.LineWidth = 2;
            frame_45_51.BackColor = Color.Transparent;
            frame_45_51.ForeColor = Color.FromArgb(0xDB, 0x53, 0x53);
            frame_45_51.Shape = new ShapeRectangle()
            {
                Fillet = 10,
            };
            frame_45_51.CanGrow = false;
            frame_45_51.AnchorVertical = VerticalAnchorStyles.Top;
            formularioDetail.Controls.Add(frame_45_51);

            XRLabel label_45_48 = new XRLabel();
            label_45_48.LocationF = new PointF(frame_45_51.LocationF.X + labelShift_thick, frame_45_51.LocationF.Y + labelShift_thick);
            label_45_48.SizeF = new SizeF(frame_45_51.WidthF - 2 * labelShift_thick, rowHeight);
            label_45_48.ForeColor = Color.Black;
            label_45_48.BackColor = Color.Transparent;
            label_45_48.Font = new Font("Arial", 6, FontStyle.Regular);
            label_45_48.Text = "Retenciones practicadas a título de ventas (I.V.A.)";
            label_45_48.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            label_45_48.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            label_45_48.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            label_45_48.SendToBack();
            label_45_48.AnchorVertical = VerticalAnchorStyles.Top;
            formularioDetail.Controls.Add(label_45_48);

            XRTable table_45_48 = new XRTable();
            XRTableRow table_45_48_Row;
            XRTableCell table_45_48_Cell;
            table_45_48.LocationF = new PointF(label_45_48.LocationF.X, label_45_48.LocationF.Y + label_27_44.HeightF);
            table_45_48.SizeF = new SizeF(label_45_48.WidthF, 4 * rowHeight);
            table_45_48.StyleName = "tableStyle";
            table_45_48.BeginInit();

            for (int i = 45; i < 49; i++)
            {
                table_45_48_Row = new XRTableRow();
                table_45_48_Row.HeightF = rowHeight;
                table_45_48_Row.BackColor = (i % 2 != 0) ? oddRowColor : evenRowColor;

                table_45_48_Cell = new XRTableCell();
                table_45_48_Cell.WidthF = (table_45_48.WidthF - 20) / 2;
                switch (i)
                {
                    case 45:
                        table_45_48_Cell.Text = "A responsables de régimen común";
                        break;
                    case 46:
                        table_45_48_Cell.Text = "Por compras y/o servisios a responsables"; ///////////////////////
                        table_45_48_Cell.CanGrow = false;
                        table_45_48_Cell.Font = new Font("Arrial", 5);
                        table_45_48_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;
                    case 47:
                        table_45_48_Cell.Text = "Practicadas por servicios a no residentes";  /////////////////////////
                        table_45_48_Cell.CanGrow = false;
                        table_45_48_Cell.Font = new Font("Arrial", 5);
                        table_45_48_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;
                    case 48:
                        table_45_48_Cell.Text = "Total retenciones del I.V.A.";  ////////////////////////////////////
                        table_45_48_Cell.CanGrow = false;
                        table_45_48_Row.Font = new Font("Arial", 6, FontStyle.Bold);
                        break;

                };
                table_45_48_Row.Cells.Add(table_45_48_Cell);
                table_45_48_Cell = new XRTableCell()
                {
                    WidthF = 20,
                    Text = i.ToString(),
                    Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right,
                    BorderColor = Color.FromArgb(0xDB, 0x53, 0x53)
                };
                table_45_48_Row.Cells.Add(table_45_48_Cell);
                table_45_48_Cell = new XRTableCell();
                table_45_48_Cell.WidthF = (table_45_48.WidthF - 20) / 2;
                table_45_48_Cell.CanGrow = false;
                foreach (DTO_FormDecDetail item in formData.FormDecDetail)
                {
                    if (Convert.ToInt32(item.Renglon.Trim()) == i)
                        table_45_48_Cell.Text = (Math.Round(item.ValorML, 0)).ToString("#,0.");
                };
                table_45_48_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                table_45_48_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0);
                table_45_48_Cell.Font = new System.Drawing.Font("Courier new", 7);
                table_45_48_Row.Cells.Add(table_45_48_Cell);
                table_45_48.Rows.Add(table_45_48_Row);
            };
            table_45_48.EndInit();
            formularioDetail.Controls.Add(table_45_48);


            XRLabel label_46 = new XRLabel();
            label_46.LocationF = new PointF(table_45_48.LocationF.X, table_45_48.LocationF.Y + 1 * rowHeight - 1);
            label_46.SizeF = new SizeF((table_45_48.WidthF - 20) / 2, 25);
            label_46.ForeColor = Color.Black;
            label_46.BackColor = Color.Transparent;
            label_46.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_46.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_46.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0);
            label_46.Multiline = true;
            label_46.Text = "\r\ndel régimen simplificado";
            formularioDetail.Controls.Add(label_46);

            XRLabel label_47 = new XRLabel();
            label_47.LocationF = new PointF(table_45_48.LocationF.X, table_45_48.LocationF.Y + 2 * rowHeight - 1);
            label_47.SizeF = new SizeF((table_45_48.WidthF - 20) / 2, 25);
            label_47.ForeColor = Color.Black;
            label_47.BackColor = Color.Transparent;
            label_47.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_47.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_47.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0);
            label_47.Multiline = true;
            label_47.Text = "\r\no no domiciliados";
            formularioDetail.Controls.Add(label_47);

            XRLabel label_48 = new XRLabel();
            label_48.LocationF = new PointF(table_45_48.LocationF.X, table_45_48.LocationF.Y + 3 * rowHeight);
            label_48.SizeF = new SizeF((table_45_48.WidthF - 20) / 2, 15);
            label_48.ForeColor = Color.Black;
            label_48.BackColor = Color.Transparent;
            label_48.Font = new System.Drawing.Font("Arial", 6, FontStyle.Regular);
            label_48.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            label_48.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 7, 0, 0);
            label_48.Text = "(Sume 45 a 47)";
            formularioDetail.Controls.Add(label_48);

            table_45_48.SendToBack();
            #endregion

            #region Table 2
            XRLabel label_49_51 = new XRLabel();
            label_49_51.LocationF = new PointF(label_45_48.LocationF.X, label_45_48.LocationF.Y + 5 * rowHeight);
            label_49_51.SizeF = new SizeF(label_45_48.WidthF, rowHeight);
            label_49_51.ForeColor = Color.Black;
            label_49_51.BackColor = oddRowColor;
            label_49_51.Font = new Font("Arial", 6, FontStyle.Regular);
            label_49_51.Text = "Retenciones practicadas a título de timbre nacional";
            label_49_51.Borders = DevExpress.XtraPrinting.BorderSide.Bottom | DevExpress.XtraPrinting.BorderSide.Top;
            label_49_51.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            label_49_51.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            label_49_51.AnchorVertical = VerticalAnchorStyles.Top;
            label_49_51.SendToBack();
            formularioDetail.Controls.Add(label_49_51);

            XRTable table_49_51 = new XRTable();
            XRTableRow table_49_51_Row;
            XRTableCell table_49_51_Cell;
            table_49_51.LocationF = new PointF(label_49_51.LocationF.X, label_49_51.LocationF.Y + label_49_51.HeightF);
            table_49_51.SizeF = new SizeF(label_45_48.WidthF, 3 * rowHeight);
            table_49_51.StyleName = "tableStyle";
            table_49_51.BeginInit();

            for (int i = 49; i < 52; i++)
            {
                table_49_51_Row = new XRTableRow();
                table_49_51_Row.HeightF = rowHeight;
                table_49_51_Row.BackColor = (i % 2 == 0) ? oddRowColor : evenRowColor;
                table_49_51_Cell = new XRTableCell();
                table_49_51_Cell.WidthF = (table_49_51.WidthF - 20) / 2;
                switch (i)
                {
                    case 49:
                        table_49_51_Cell.Text = "A la tarifa general";
                        break;
                    case 50:
                        table_49_51_Cell.Text = "Otras tarifas";
                        break;
                    case 51:
                        table_49_51_Cell.Text = "Total retenciones timbre nacional";  //////////////////////
                        table_49_51_Row.Font = new Font("Arial", 6, FontStyle.Bold);
                        break;
                };

                table_49_51_Row.Cells.Add(table_49_51_Cell);
                table_49_51_Cell = new XRTableCell()
                {
                    WidthF = 20,
                    Text = i.ToString(),
                    Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right,
                    BorderColor = Color.FromArgb(0xDB, 0x53, 0x53)
                };
                table_49_51_Row.Cells.Add(table_49_51_Cell);
                table_49_51_Cell = new XRTableCell();
                table_49_51_Cell.WidthF = (table_49_51.WidthF - 20) / 2;
                table_49_51_Cell.CanGrow = false;
                foreach (DTO_FormDecDetail item in formData.FormDecDetail)
                {
                    if (Convert.ToInt32(item.Renglon.Trim()) == i)
                        table_49_51_Cell.Text = (Math.Round(item.ValorML, 0)).ToString("#,0.");
                };
                table_49_51_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                table_49_51_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0);
                table_49_51_Cell.Font = new System.Drawing.Font("Courier new", 7);
                table_49_51_Row.Cells.Add(table_49_51_Cell);
                table_49_51.Rows.Add(table_49_51_Row);
            };
            table_49_51.EndInit();
            formularioDetail.Controls.Add(table_49_51);

            XRLabel label_51 = new XRLabel();
            label_51.LocationF = new PointF(table_49_51.LocationF.X, table_49_51.LocationF.Y + 2 * rowHeight);
            label_51.SizeF = new SizeF((table_49_51.WidthF - 20) / 2, 15);
            label_51.ForeColor = Color.Black;
            label_51.BackColor = Color.Transparent;
            label_51.Font = new System.Drawing.Font("Arial", 6, FontStyle.Regular);
            label_51.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            label_51.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0);
            label_51.Text = "(49 + 50)";
            formularioDetail.Controls.Add(label_51);

            table_49_51.SendToBack();
            #endregion

            #region Table 3
            XRShape frame_52_54 = new XRShape();
            frame_52_54.LocationF = new PointF(frame_45_51.LocationF.X, frame_45_51.LocationF.Y + frame_45_51.HeightF - frameShift);
            frame_52_54.SizeF = new SizeF(frame_45_51.WidthF, 3 * rowHeight + frameShift);
            frame_52_54.LineWidth = 2;
            frame_52_54.BackColor = Color.Transparent;
            frame_52_54.ForeColor = Color.FromArgb(0xDB, 0x53, 0x53);
            frame_52_54.Shape = new ShapeRectangle()
            {
                Fillet = 14,
            };
            frame_52_54.CanGrow = false;
            frame_52_54.AnchorVertical = VerticalAnchorStyles.Top;
            formularioDetail.Controls.Add(frame_52_54);

            XRTable table_52_54 = new XRTable();
            XRTableRow table_52_54_Row;
            XRTableCell table_52_54_Cell;
            table_52_54.LocationF = new PointF(frame_52_54.LocationF.X + labelShift_thick, frame_52_54.LocationF.Y + labelShift_thick);
            table_52_54.SizeF = new SizeF(frame_52_54.WidthF - 2 * labelShift_thick, 3 * rowHeight);
            table_52_54.StyleName = "tableStyle";
            table_52_54.BeginInit();

            for (int i = 52; i < 55; i++)
            {
                table_52_54_Row = new XRTableRow();
                table_52_54_Row.HeightF = rowHeight;
                table_52_54_Row.BackColor = (i % 2 == 0) ? oddRowColor : evenRowColor;
                table_52_54_Cell = new XRTableCell();
                table_52_54_Cell.WidthF = (table_52_54.WidthF - 20) / 2;
                table_52_54_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(22, 0, 0, 0);
                switch (i)
                {
                    case 52:
                        table_52_54_Cell.Text = "Total retenciones"; /////////////////////////
                        table_52_54_Row.Font = new Font("Arial", 6, FontStyle.Bold);
                        break;
                    case 53:
                        table_52_54_Cell.Text = "Más: Sanciones";
                        break;
                    case 54:
                        table_52_54_Cell.Text = "Total retenciones más Sanciones";  //////////////////////////////
                        table_52_54_Cell.CanGrow = false;
                        table_52_54_Row.Font = new Font("Arial", 6, FontStyle.Bold);
                        table_52_54_Cell.Font = new Font("Arial", 5, FontStyle.Bold);
                        break;
                };

                table_52_54_Row.Cells.Add(table_52_54_Cell);
                table_52_54_Cell = new XRTableCell()
                {
                    WidthF = 20,
                    Text = i.ToString(),
                    Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right,
                    BorderColor = Color.FromArgb(0xDB, 0x53, 0x53)
                };
                table_52_54_Row.Cells.Add(table_52_54_Cell);
                table_52_54_Cell = new XRTableCell();
                table_52_54_Cell.WidthF = (table_52_54.WidthF - 20) / 2;
                table_52_54_Cell.CanGrow = false;
                foreach (DTO_FormDecDetail item in formData.FormDecDetail)
                {
                    if (Convert.ToInt32(item.Renglon.Trim()) == i)
                        table_52_54_Cell.Text = (Math.Round(item.ValorML, 0)).ToString("#,0.");
                };
                table_52_54_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                table_52_54_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0);
                table_52_54_Cell.Font = new System.Drawing.Font("Courier new", 7);
                table_52_54_Row.Cells.Add(table_52_54_Cell);
                table_52_54.Rows.Add(table_52_54_Row);
            };
            table_52_54.EndInit();
            formularioDetail.Controls.Add(table_52_54);

            XRLabel label_52_54 = new XRLabel();
            label_52_54.LocationF = new PointF(table_52_54.LocationF.X, table_52_54.LocationF.Y);
            label_52_54.SizeF = new SizeF(20, 3 * rowHeight);
            label_52_54.ForeColor = Color.Black;
            label_52_54.BackColor = Color.White;
            label_52_54.Font = new Font("Arial", 6, FontStyle.Bold);
            label_52_54.Text = "Total";
            label_52_54.Angle = 90;
            label_52_54.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            label_52_54.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            label_52_54.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            label_52_54.SendToBack();
            formularioDetail.Controls.Add(label_52_54);

            XRLabel label_52 = new XRLabel();
            label_52.LocationF = new PointF(table_52_54.LocationF.X, table_52_54.LocationF.Y);
            label_52.SizeF = new SizeF((table_52_54.WidthF - 20) / 2, 15);
            label_52.ForeColor = Color.Black;
            label_52.BackColor = Color.Transparent;
            label_52.Font = new System.Drawing.Font("Arial", 6, FontStyle.Regular);
            label_52.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            label_52.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 29, 0, 0);
            label_52.Text = "(44 + 48 + 51)";
            formularioDetail.Controls.Add(label_52);

            XRLabel label_54 = new XRLabel();
            label_54.LocationF = new PointF(table_52_54.LocationF.X, table_52_54.LocationF.Y + 2 * rowHeight);
            label_54.SizeF = new SizeF((table_52_54.WidthF - 20) / 2, 15);
            label_54.ForeColor = Color.Black;
            label_54.BackColor = Color.Transparent;
            label_54.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_54.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            label_54.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 12, 0, 0);
            label_54.Text = "(52 + 53)";
            formularioDetail.Controls.Add(label_54);

            table_52_54.SendToBack();
            #endregion

            #region Table 4
            XRShape frame_55_59 = new XRShape();
            frame_55_59.LocationF = new PointF(frame_45_51.LocationF.X, frame_52_54.LocationF.Y + frame_52_54.HeightF - frameShift);
            frame_55_59.SizeF = new SizeF(frame_45_51.WidthF, 7 * rowHeight + 2 * labelShift_thick);
            frame_55_59.LineWidth = 2;
            frame_55_59.BackColor = Color.Transparent;
            frame_55_59.ForeColor = Color.FromArgb(0xDB, 0x53, 0x53);
            frame_55_59.Shape = new ShapeRectangle()
            {
                Fillet = 13,
            };
            frame_55_59.CanGrow = false;
            frame_55_59.AnchorVertical = VerticalAnchorStyles.Top;
            formularioDetail.Controls.Add(frame_55_59);

            XRTable table_55_59 = new XRTable();
            XRTableRow table_55_59_Row;
            XRTableCell table_55_59_Cell;
            table_55_59.LocationF = new PointF(frame_55_59.LocationF.X + labelShift_thick, frame_55_59.LocationF.Y + labelShift_thick);
            table_55_59.SizeF = new SizeF(frame_55_59.WidthF - 2 * labelShift_thick, 5 * rowHeight);
            table_55_59.StyleName = "tableStyle";
            table_55_59.BeginInit();

            for (int i = 55; i < 60; i++)
            {
                table_55_59_Row = new XRTableRow();
                table_55_59_Row.HeightF = rowHeight;
                table_55_59_Row.BackColor = (i % 2 == 0) ? oddRowColor : evenRowColor;
                table_55_59_Row.Font = new Font("Arial", 6, FontStyle.Bold);
                table_55_59_Cell = new XRTableCell();
                table_55_59_Cell.WidthF = (table_55_59.WidthF - 20) / 2;
                table_55_59_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(22, 0, 0, 0);
                switch (i)
                {
                    case 55:
                        table_55_59_Cell.Text = "Valor pago sanciones";
                        break;
                    case 56:
                        table_55_59_Cell.Text = "Valor pago intereses de mora";
                        break;
                    case 57:
                        table_55_59_Cell.Text = "Valor pago retención renta";
                        break;
                    case 58:
                        table_55_59_Cell.Text = "Valor pago retención I.V.A.";
                        break;
                    case 59:
                        table_55_59_Cell.Text = "Valor pago retención timbre nacional";
                        break;
                };

                table_55_59_Row.Cells.Add(table_55_59_Cell);
                table_55_59_Cell = new XRTableCell()
                {
                    WidthF = 20,
                    Text = i.ToString(),
                    Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right,
                    BorderColor = Color.FromArgb(0xDB, 0x53, 0x53)
                };
                table_55_59_Row.Cells.Add(table_55_59_Cell);
                table_55_59_Cell = new XRTableCell();
                table_55_59_Cell.WidthF = (table_55_59.WidthF - 20) / 2;
                table_55_59_Cell.CanGrow = false;
                foreach (DTO_FormDecDetail item in formData.FormDecDetail)
                {
                    if (Convert.ToInt32(item.Renglon.Trim()) == i)
                        table_55_59_Cell.Text = (Math.Round(item.ValorML, 0)).ToString("#,0.");
                };
                table_55_59_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                table_55_59_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0);
                table_55_59_Cell.Font = new System.Drawing.Font("Courier new", 7);
                table_55_59_Row.Cells.Add(table_55_59_Cell);
                table_55_59.Rows.Add(table_55_59_Row);
            };
            table_55_59.EndInit();
            table_55_59.SendToBack();
            formularioDetail.Controls.Add(table_55_59);

            XRLabel label_55_59 = new XRLabel();
            label_55_59.LocationF = new PointF(table_55_59.LocationF.X, table_55_59.LocationF.Y);
            label_55_59.SizeF = new SizeF(20, 5 * rowHeight);
            label_55_59.ForeColor = Color.Black;
            label_55_59.BackColor = Color.White;
            label_55_59.Font = new Font("Arial", 6, FontStyle.Bold);
            label_55_59.Text = "Pagos";
            label_55_59.Angle = 90;
            label_55_59.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            label_55_59.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            label_55_59.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            label_55_59.SendToBack();
            formularioDetail.Controls.Add(label_55_59);

            table_55_59.SendToBack();
            #endregion

            XRPanel Panel_55_59_blank = new XRPanel();
            Panel_55_59_blank.LocationF = new PointF(frame_55_59.LocationF.X + labelShift_thick, table_55_59.LocationF.Y + 5 * rowHeight);
            Panel_55_59_blank.SizeF = new SizeF(frame_55_59.WidthF - 2 * labelShift_thick, 2 * rowHeight - 1);
            Panel_55_59_blank.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            Panel_55_59_blank.BorderWidth = 1;
            Panel_55_59_blank.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            Panel_55_59_blank.BackColor = Color.FromArgb(0xE7, 0xAB, 0xAB);
            Panel_55_59_blank.AnchorVertical = VerticalAnchorStyles.Top;
            Panel_55_59_blank.SendToBack();
            formularioDetail.Controls.Add(Panel_55_59_blank);
            #endregion
            #endregion

            #region Formulario part 5
            XRShape infoFrame = new XRShape();
            infoFrame.LocationF = new PointF(0, frame_27_44.LocationF.Y + frame_27_44.HeightF - frameShift);
            infoFrame.SizeF = new SizeF(formularioWidth, 120 + frameShift);
            infoFrame.LineWidth = 2;
            infoFrame.Borders = DevExpress.XtraPrinting.BorderSide.None;
            infoFrame.SnapLineMargin = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0);
            infoFrame.BackColor = Color.Transparent;
            infoFrame.ForeColor = Color.FromArgb(0xDB, 0x53, 0x53);
            infoFrame.Shape = new ShapeRectangle()
            {
                Fillet = 10,
            };
            infoFrame.SendToBack();
            formularioDetail.Controls.Add(infoFrame);

            XRLabel upperInfoLabel = new XRLabel();
            upperInfoLabel.LocationF = infoFrame.LocationF;
            upperInfoLabel.SizeF = new SizeF(infoFrame.WidthF, infoFrame.HeightF / 2);
            upperInfoLabel.ForeColor = Color.Black;
            upperInfoLabel.BackColor = Color.Transparent;
            upperInfoLabel.Font = new Font("Arial", 14, FontStyle.Bold);
            upperInfoLabel.Multiline = true;
            upperInfoLabel.Text = "Servicios Informáticos Electrónicos - ¡Más formas de servirle!";
            upperInfoLabel.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 20, 0);
            upperInfoLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            formularioDetail.Controls.Add(upperInfoLabel);

            XRLabel lowerInfoLabel = new XRLabel();
            lowerInfoLabel.LocationF = new PointF(upperInfoLabel.LocationF.X, upperInfoLabel.LocationF.Y + upperInfoLabel.HeightF);
            lowerInfoLabel.SizeF = new SizeF(infoFrame.WidthF, infoFrame.HeightF / 2);
            lowerInfoLabel.ForeColor = Color.Black;
            lowerInfoLabel.BackColor = Color.Transparent;
            lowerInfoLabel.Font = new Font("Arial", 12, FontStyle.Regular);
            lowerInfoLabel.Multiline = true;
            lowerInfoLabel.Text = "Este formulario también puede diligenciarlo a www.dian.gov.co\r\nAsistido,sin errores y de manera gratuita";
            lowerInfoLabel.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 15);
            lowerInfoLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            formularioDetail.Controls.Add(lowerInfoLabel);
            #endregion

            #region Formulario part 6
            XRShape frame_60_71 = new XRShape();
            frame_60_71.LocationF = new PointF(0, infoFrame.LocationF.Y + infoFrame.HeightF - frameShift);
            frame_60_71.SizeF = new SizeF(formularioWidth, 4 * rowHeight + 2 * labelShift_thick);
            frame_60_71.LineWidth = 2;
            frame_60_71.Borders = DevExpress.XtraPrinting.BorderSide.None;
            frame_60_71.SnapLineMargin = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0);
            frame_60_71.BackColor = Color.Transparent;
            frame_60_71.ForeColor = Color.FromArgb(0xDB, 0x53, 0x53);
            frame_60_71.Shape = new ShapeRectangle()
            {
                Fillet = 10,
            };
            frame_60_71.SendToBack();
            formularioDetail.Controls.Add(frame_60_71);

            XRLabel signatariosLabel = new XRLabel();
            signatariosLabel.LocationF = new PointF(frame_60_71.LocationF.X + labelShift_thick, frame_60_71.LocationF.Y + labelShift_thick);
            signatariosLabel.SizeF = new SizeF(25, 4 * rowHeight);
            signatariosLabel.ForeColor = Color.Black;
            signatariosLabel.BackColor = Color.Transparent;
            signatariosLabel.Font = new Font("Arial", 6, FontStyle.Bold);
            signatariosLabel.Multiline = true;
            signatariosLabel.Text = "Signatarios";
            signatariosLabel.Angle = 90;
            signatariosLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            formularioDetail.Controls.Add(signatariosLabel);

            #region Table 1
            XRPanel Panel_60_61 = new XRPanel();
            Panel_60_61.LocationF = new PointF(signatariosLabel.LocationF.X + signatariosLabel.WidthF, signatariosLabel.LocationF.Y);
            Panel_60_61.SizeF = new SizeF(frame_60_71.WidthF / 3 - signatariosLabel.WidthF, 2 * rowHeight);
            Panel_60_61.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom;
            Panel_60_61.BorderWidth = 1;
            Panel_60_61.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            Panel_60_61.BackColor = Color.Transparent;
            Panel_60_61.BringToFront();
            formularioDetail.Controls.Add(Panel_60_61);

            XRTable table_60_61 = new XRTable();
            XRTableRow table_60_61_Row;
            XRTableCell table_60_61_Cell;
            table_60_61.LocationF = Panel_60_61.LocationF;
            table_60_61.SizeF = Panel_60_61.SizeF;
            table_60_61.StyleName = "tableStyle";
            table_60_61.BeginInit();
            table_60_61_Row = new XRTableRow();
            table_60_61_Row.HeightF = rowHeight;
            table_60_61_Row.BackColor = evenRowColor;
            table_60_61_Cell = new XRTableCell()
            {
                WidthF = table_60_61.WidthF - 30,
                Text = "60. Número de Identificación Tributaria (NIT)"
            };
            table_60_61_Row.Cells.Add(table_60_61_Cell);
            table_60_61_Cell = new XRTableCell()
            {
                WidthF = 30,
                Text = "61. DV",
                Padding = new DevExpress.XtraPrinting.PaddingInfo(1, 0, 0, 0),
                WordWrap = false,
                CanGrow = false
            };
            table_60_61_Row.Cells.Add(table_60_61_Cell);
            table_60_61.Rows.Add(table_60_61_Row);
            table_60_61_Row = new XRTableRow();
            table_60_61_Row.HeightF = rowHeight;
            table_60_61_Row.BackColor = evenRowColor;
            table_60_61_Cell = new XRTableCell()
            {
                WidthF = table_60_61.WidthF - 30,
                ///////////// Datos
            };
            table_60_61_Row.Cells.Add(table_60_61_Cell);
            table_60_61_Cell = new XRTableCell()
            {
                WidthF = 30,
                Borders = DevExpress.XtraPrinting.BorderSide.Left,
                BorderColor = Color.FromArgb(0xDB, 0x53, 0x53)
                ///////////// Datos
            };
            table_60_61_Row.Cells.Add(table_60_61_Cell);
            table_60_61.Rows.Add(table_60_61_Row);
            table_60_61.EndInit();
            table_60_61.SendToBack();
            formularioDetail.Controls.Add(table_60_61);
            #endregion

            #region Table 2
            XRPanel Panel_62_65 = new XRPanel();
            Panel_62_65.LocationF = new PointF(table_60_61.LocationF.X + table_60_61.WidthF, table_60_61.LocationF.Y);
            Panel_62_65.SizeF = new SizeF(2 * frame_60_71.WidthF / 3 - 2 * labelShift_thick, 2 * rowHeight);
            Panel_62_65.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom;
            Panel_62_65.BorderWidth = 1;
            Panel_62_65.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            Panel_62_65.BackColor = Color.Transparent;
            Panel_62_65.BringToFront();
            formularioDetail.Controls.Add(Panel_62_65);

            XRLabel label_62_65 = new XRLabel();
            label_62_65.LocationF = Panel_62_65.LocationF;
            label_62_65.SizeF = new SizeF(Panel_62_65.WidthF, rowHeight);
            label_62_65.StyleName = "tableStyle";
            label_62_65.Text = "Apellidos y nombres de quien firma como representante del declarante";
            formularioDetail.Controls.Add(label_62_65);

            XRTable table_62_65 = new XRTable();
            XRTableRow table_62_65_Row;
            XRTableCell table_62_65_Cell;
            table_62_65.LocationF = Panel_62_65.LocationF;
            table_62_65.SizeF = Panel_62_65.SizeF;
            table_62_65.StyleName = "tableStyle";
            table_62_65.BeginInit();
            table_62_65_Row = new XRTableRow();
            table_62_65_Row.HeightF = rowHeight;
            table_62_65_Row.BackColor = evenRowColor;
            table_62_65_Cell = new XRTableCell()
            {
                WidthF = table_62_65.WidthF / 4,
                ///////////// Datos
            };
            table_62_65_Row.Cells.Add(table_62_65_Cell);
            table_62_65_Cell = new XRTableCell()
            {
                WidthF = table_62_65.WidthF / 4,
                ///////////// Datos
            };
            table_62_65_Row.Cells.Add(table_62_65_Cell);
            table_62_65_Cell = new XRTableCell()
            {
                WidthF = table_62_65.WidthF / 4,
                ///////////// Datos
            };
            table_62_65_Row.Cells.Add(table_62_65_Cell);
            table_62_65_Cell = new XRTableCell()
            {
                WidthF = table_62_65.WidthF / 4,
                ///////////// Datos
            };
            table_62_65_Row.Cells.Add(table_62_65_Cell);
            table_62_65.Rows.Add(table_62_65_Row);
            table_62_65_Row = new XRTableRow();
            table_62_65_Row.HeightF = rowHeight;
            table_62_65_Row.BackColor = evenRowColor;
            table_62_65_Row.ForeColor = Color.FromArgb(0xDB, 0x53, 0x53);
            table_62_65_Cell = new XRTableCell()
            {
                WidthF = table_62_65.WidthF / 4,
                Text = "62. Primer apellido"
            };
            table_62_65_Row.Cells.Add(table_62_65_Cell);
            table_62_65_Cell = new XRTableCell()
            {
                WidthF = table_62_65.WidthF / 4,
                Text = "63. Segundo apellido"
            };
            table_62_65_Row.Cells.Add(table_62_65_Cell);
            table_62_65_Cell = new XRTableCell()
            {
                WidthF = table_62_65.WidthF / 4,
                Text = "64. Primer nombre"
            };
            table_62_65_Row.Cells.Add(table_62_65_Cell);
            table_62_65_Cell = new XRTableCell()
            {
                WidthF = table_62_65.WidthF / 4,
                Text = "65. Otros nombres"
            };
            table_62_65_Row.Cells.Add(table_62_65_Cell);
            table_62_65.Rows.Add(table_62_65_Row);
            table_62_65.EndInit();
            table_62_65.SendToBack();
            formularioDetail.Controls.Add(table_62_65);

            XRTable table_62_65_division = new XRTable();
            XRTableRow table_62_65_division_Row;
            XRTableCell table_62_65_division_Cell;
            table_62_65_division.LocationF = new PointF(table_62_65.LocationF.X, table_62_65.LocationF.Y + 3 * rowHeight / 2);
            table_62_65_division.SizeF = new SizeF(table_62_65.WidthF - table_62_65.WidthF / 4, rowHeight / 2);
            table_62_65_division.BeginInit();
            table_62_65_division_Row = new XRTableRow();
            table_62_65_division_Row.HeightF = rowHeight / 2;
            table_62_65_division_Row.BackColor = evenRowColor;
            for (int i = 0; i < 3; i++)
            {
                table_62_65_division_Cell = new XRTableCell()
                {
                    WidthF = table_62_65.WidthF / 4,
                    Borders = DevExpress.XtraPrinting.BorderSide.Right,
                    BorderColor = Color.FromArgb(0xDB, 0x53, 0x53)
                };
                table_62_65_division_Row.Cells.Add(table_62_65_division_Cell);
            };

            table_62_65_division.Rows.Add(table_62_65_division_Row);
            table_62_65_division.EndInit();
            table_62_65_division.SendToBack();
            formularioDetail.Controls.Add(table_62_65_division);
            #endregion

            #region Table 3
            XRPanel Panel_66_67 = new XRPanel();
            Panel_66_67.LocationF = new PointF(signatariosLabel.LocationF.X + signatariosLabel.WidthF, Panel_60_61.LocationF.Y + Panel_60_61.HeightF);
            Panel_66_67.SizeF = new SizeF(frame_60_71.WidthF / 3 - signatariosLabel.WidthF, 2 * rowHeight);
            Panel_66_67.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            Panel_66_67.BorderWidth = 1;
            Panel_66_67.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            Panel_66_67.BackColor = Color.Transparent;
            Panel_66_67.BringToFront();
            formularioDetail.Controls.Add(Panel_66_67);

            XRTable table_66_67 = new XRTable();
            XRTableRow table_66_67_Row;
            XRTableCell table_66_67_Cell;
            table_66_67.LocationF = Panel_66_67.LocationF;
            table_66_67.SizeF = Panel_66_67.SizeF;
            table_66_67.StyleName = "tableStyle";
            table_66_67.BeginInit();

            table_66_67_Row = new XRTableRow();
            table_66_67_Row.HeightF = rowHeight;
            table_66_67_Row.BackColor = evenRowColor;

            table_66_67_Cell = new XRTableCell()
            {
                WidthF = table_66_67.WidthF - 30,
                Text = "66. Número NIT contrador o revisor fiscal"
            };
            table_66_67_Row.Cells.Add(table_66_67_Cell);
            table_66_67_Cell = new XRTableCell()
            {
                WidthF = 30,
                Text = "67. DV",
                Padding = new DevExpress.XtraPrinting.PaddingInfo(1, 0, 0, 0),
                WordWrap = false,
                CanGrow = false
            };
            table_66_67_Row.Cells.Add(table_66_67_Cell);
            table_66_67.Rows.Add(table_66_67_Row);

            table_66_67_Row = new XRTableRow();
            table_66_67_Row.HeightF = rowHeight;
            table_66_67_Row.BackColor = evenRowColor;
            table_66_67_Cell = new XRTableCell()
            {
                WidthF = table_66_67.WidthF - 30,
                ///////////// Datos
            };
            table_66_67_Row.Cells.Add(table_66_67_Cell);
            table_66_67_Cell = new XRTableCell()
            {
                WidthF = 30,
                Borders = DevExpress.XtraPrinting.BorderSide.Left,
                BorderColor = Color.FromArgb(0xDB, 0x53, 0x53)
                ///////////// Datos
            };
            table_66_67_Row.Cells.Add(table_66_67_Cell);
            table_66_67.Rows.Add(table_66_67_Row);
            table_66_67.EndInit();
            table_66_67.SendToBack();
            formularioDetail.Controls.Add(table_66_67);
            #endregion

            #region Table 4
            XRPanel Panel_68_71 = new XRPanel();
            Panel_68_71.LocationF = new PointF(table_66_67.LocationF.X + table_66_67.WidthF, table_66_67.LocationF.Y);
            Panel_68_71.SizeF = new SizeF(2 * frame_60_71.WidthF / 3 - 2 * labelShift_thick, 2 * rowHeight); ;
            Panel_68_71.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            Panel_68_71.BorderWidth = 1;
            Panel_68_71.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            Panel_68_71.BackColor = Color.Transparent;
            Panel_68_71.BringToFront();
            formularioDetail.Controls.Add(Panel_68_71);

            XRLabel label_68_71 = new XRLabel();
            label_68_71.LocationF = Panel_68_71.LocationF;
            label_68_71.SizeF = new SizeF(Panel_68_71.WidthF, rowHeight);
            label_68_71.StyleName = "tableStyle";
            label_68_71.Text = "Apellidos y nombres del contador o revisor fiscal";
            formularioDetail.Controls.Add(label_68_71);

            XRTable table_68_71 = new XRTable();
            XRTableRow table_68_71_Row;
            XRTableCell table_68_71_Cell;
            table_68_71.LocationF = Panel_68_71.LocationF;
            table_68_71.SizeF = Panel_68_71.SizeF;
            table_68_71.StyleName = "tableStyle";
            table_68_71.BeginInit();

            table_68_71_Row = new XRTableRow();
            table_68_71_Row.HeightF = rowHeight;
            table_68_71_Row.BackColor = evenRowColor;

            table_68_71_Cell = new XRTableCell()
            {
                WidthF = table_68_71.WidthF / 4,
                ///////////// Datos
            };
            table_68_71_Row.Cells.Add(table_68_71_Cell);
            table_68_71_Cell = new XRTableCell()
            {
                WidthF = table_68_71.WidthF / 4,
                ///////////// Datos
            };
            table_68_71_Row.Cells.Add(table_68_71_Cell);
            table_68_71_Cell = new XRTableCell()
            {
                WidthF = table_68_71.WidthF / 4,
                ///////////// Datos
            };
            table_68_71_Row.Cells.Add(table_68_71_Cell);
            table_68_71_Cell = new XRTableCell()
            {
                WidthF = table_68_71.WidthF / 4,
                ///////////// Datos
            };
            table_68_71_Row.Cells.Add(table_68_71_Cell);
            table_68_71.Rows.Add(table_68_71_Row);
            table_68_71_Row = new XRTableRow();
            table_68_71_Row.HeightF = rowHeight;
            table_68_71_Row.BackColor = evenRowColor;
            table_68_71_Row.ForeColor = Color.FromArgb(0xDB, 0x53, 0x53);
            table_68_71_Cell = new XRTableCell()
            {
                WidthF = table_68_71.WidthF / 4,
                Text = "68. Primer apellido"
            };
            table_68_71_Row.Cells.Add(table_68_71_Cell);
            table_68_71_Cell = new XRTableCell()
            {
                WidthF = table_68_71.WidthF / 4,
                Text = "69. Segundo apellido"
            };
            table_68_71_Row.Cells.Add(table_68_71_Cell);
            table_68_71_Cell = new XRTableCell()
            {
                WidthF = table_68_71.WidthF / 4,
                Text = "70. Primer nombre"
            };
            table_68_71_Row.Cells.Add(table_68_71_Cell);
            table_68_71_Cell = new XRTableCell()
            {
                WidthF = table_68_71.WidthF / 4,
                Text = "71. Otros nombres"
            };
            table_68_71_Row.Cells.Add(table_68_71_Cell);
            table_68_71.Rows.Add(table_68_71_Row);
            table_68_71.EndInit();
            table_68_71.SendToBack();
            formularioDetail.Controls.Add(table_68_71);

            XRTable table_68_71_division = new XRTable();
            XRTableRow table_68_71_division_Row;
            XRTableCell table_68_71_division_Cell;
            table_68_71_division.LocationF = new PointF(table_68_71.LocationF.X, table_68_71.LocationF.Y + 3 * rowHeight / 2);
            table_68_71_division.SizeF = new SizeF(table_68_71.WidthF - table_68_71.WidthF / 4, rowHeight / 2);
            table_68_71_division.BeginInit();
            table_68_71_division_Row = new XRTableRow();
            table_68_71_division_Row.HeightF = rowHeight / 2;
            table_68_71_division_Row.BackColor = evenRowColor;
            for (int i = 0; i < 3; i++)
            {
                table_68_71_division_Cell = new XRTableCell()
                {
                    WidthF = table_68_71.WidthF / 4,
                    Borders = DevExpress.XtraPrinting.BorderSide.Right,
                    BorderColor = Color.FromArgb(0xDB, 0x53, 0x53)
                };
                table_68_71_division_Row.Cells.Add(table_68_71_division_Cell);
            };

            table_68_71_division.Rows.Add(table_68_71_division_Row);
            table_68_71_division.EndInit();
            table_68_71_division.SendToBack();
            formularioDetail.Controls.Add(table_68_71_division);
            #endregion

            #endregion
        } 
        #endregion    
    }
}