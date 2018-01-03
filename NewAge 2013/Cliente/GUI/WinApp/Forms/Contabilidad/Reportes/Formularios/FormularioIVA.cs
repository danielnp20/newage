using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting.Shape;
using NewAge.DTO.Reportes;
using System.Collections.Generic;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;

namespace NewAge.Cliente.GUI.WinApp.Reports.Formularios
{
    public partial class FormularioIVA : DevExpress.XtraReports.UI.XtraReport
    {
        #region Variables
        BaseController _bc = BaseController.GetInstance(); 
        #endregion

        #region Funciones Publicas
        /// <summary>
        /// Formulario "Declaracion bimestral del impuesto sobre las ventas - IVA" Constructor
        /// </summary>
        /// <param name="formData">Data for the Formulario</param>
        /// <param name="Date">Period of the Formulario</param>
        public FormularioIVA(DTO_Formularios formData, int yearFisc, int period, bool preInd)
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
            titleLabel.Text = "Declaración Bimestral del Impuesto sobre las Ventas - IVA";
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
            numeroLabel.Text = "300";
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
 
            char[] month = (period.ToString().Length == 1)? ("0" + period.ToString()).ToCharArray(): period.ToString().ToCharArray();

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
            numeroFormularioLabel.LocationF = new PointF(Panel_4.LocationF.X + 10, Panel_4.LocationF.Y + 20);
            numeroFormularioLabel.SizeF = new SizeF(140, 20);
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
                    WidthF = (table_5_6.WidthF - 30) / 14,
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
            table_5_6_division.SizeF = new SizeF(table_5_6.WidthF - 30 - table_5_6.WidthF / 14, rowHeight / 3);
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
            label_24.SizeF = new SizeF(160, rowHeight);
            label_24.StyleName = "tableStyle";
            label_24.Text = "24. Periodicidad de la declaración:";
            formularioDetail.Controls.Add(label_24);
            
            XRLabel label_Bimest = new XRLabel();
            label_Bimest.LocationF = new PointF(label_24.LocationF.X + label_24.WidthF, label_24.LocationF.Y);
            label_Bimest.HeightF = rowHeight;
            label_Bimest.WidthF = 40;
            label_Bimest.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            label_Bimest.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0);
            label_Bimest.StyleName = "tableStyle";
            label_Bimest.Text = "Bimestral";
            formularioDetail.Controls.Add(label_Bimest);

            XRLabel label_Bimest_blank = new XRLabel();
            label_Bimest_blank.LocationF = new PointF(label_Bimest.LocationF.X + label_Bimest.WidthF + 10, label_24.LocationF.Y + 1);
            label_Bimest_blank.SizeF = new SizeF(14, label_24.HeightF - 2);
            label_Bimest_blank.BackColor = Color.White;
            label_Bimest_blank.Borders = DevExpress.XtraPrinting.BorderSide.All;
            label_Bimest_blank.BringToFront();
            formularioDetail.Controls.Add(label_Bimest_blank);

            XRLabel label_Cuatrimest = new XRLabel();
            label_Cuatrimest.LocationF = new PointF(label_Bimest_blank.LocationF.X + label_Bimest_blank.WidthF + 30, label_24.LocationF.Y);
            label_Cuatrimest.HeightF = rowHeight;
            label_Cuatrimest.WidthF = 65;
            label_Cuatrimest.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            label_Cuatrimest.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0);
            label_Cuatrimest.StyleName = "tableStyle";
            label_Cuatrimest.Text = "Cuatrimestral";
            formularioDetail.Controls.Add(label_Cuatrimest);

            XRLabel label_Cuatrimest_blank = new XRLabel();
            label_Cuatrimest_blank.LocationF = new PointF(label_Cuatrimest.LocationF.X + label_Cuatrimest.WidthF + 10, label_24.LocationF.Y + 1);
            label_Cuatrimest_blank.SizeF = new SizeF(14, label_24.HeightF - 2);
            label_Cuatrimest_blank.BackColor = Color.White;
            label_Cuatrimest_blank.Borders = DevExpress.XtraPrinting.BorderSide.All;
            label_Cuatrimest_blank.BringToFront();
            formularioDetail.Controls.Add(label_Cuatrimest_blank);

            XRLabel label_Anual = new XRLabel();
            label_Anual.LocationF = new PointF(label_Cuatrimest_blank.LocationF.X + label_Cuatrimest_blank.WidthF +30, label_24.LocationF.Y);
            label_Anual.HeightF = rowHeight;
            label_Anual.WidthF = 35;
            label_Anual.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            label_Anual.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0);
            label_Anual.StyleName = "tableStyle";
            label_Anual.Text = "Anual";
            formularioDetail.Controls.Add(label_Anual);

            XRLabel label_Anual_blank = new XRLabel();
            label_Anual_blank.LocationF = new PointF(label_Anual.LocationF.X + label_Anual.WidthF +10, label_24.LocationF.Y + 1);
            label_Anual_blank.SizeF = new SizeF(14, label_24.HeightF - 2);
            label_Anual_blank.BackColor = Color.White;
            label_Anual_blank.Borders = DevExpress.XtraPrinting.BorderSide.All;
            label_Anual_blank.BringToFront();
            formularioDetail.Controls.Add(label_Anual_blank);

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
            XRShape frame_27_60 = new XRShape();
            frame_27_60.LocationF = new PointF(0, frame_5_26.LocationF.Y + frame_5_26.HeightF - frameShift);
            frame_27_60.SizeF = new SizeF(formularioWidth / 2, 34 * rowHeight + 2 * labelShift_thick);
            frame_27_60.LineWidth = 2;
            frame_27_60.BackColor = Color.Transparent;
            frame_27_60.ForeColor = Color.FromArgb(0xDB, 0x53, 0x53);
            frame_27_60.Shape = new ShapeRectangle()
            {
                Fillet = 3,
            };
            frame_27_60.AnchorVertical = VerticalAnchorStyles.Both;
            frame_27_60.CanGrow = false;
            frame_27_60.AnchorVertical = VerticalAnchorStyles.Top;
            formularioDetail.Controls.Add(frame_27_60);

            #region Table 1
            XRTable table_27_41 = new XRTable();
            XRTableRow table_27_41_Row;
            XRTableCell table_27_41_Cell;
            table_27_41.BeginInit();
            table_27_41.LocationF = new PointF(frame_27_60.LocationF.X + labelShift_thick, frame_27_60.LocationF.Y + labelShift_thick);
            table_27_41.SizeF = new SizeF(frame_27_60.WidthF - 2 * labelShift_thick, 15 * rowHeight);
            table_27_41.StyleName = "tableStyle";
            //table_27_41.AnchorVertical = VerticalAnchorStyles.Both;

            for (int i = 27; i < 42; i++)
            {
                table_27_41_Row = new XRTableRow();
                table_27_41_Row.HeightF = rowHeight;
                table_27_41_Row.BackColor = (i % 2 != 0) ? evenRowColor : oddRowColor;
                table_27_41_Row.CanGrow = false;

                table_27_41_Cell = new XRTableCell();
                table_27_41_Cell.WidthF = (table_27_41.WidthF - 20) / 2;
                table_27_41_Cell.CanGrow = false;
                switch (i)
                {
                    case 27:
                        table_27_41_Cell.Text = "Por operaciones gravadas al 5%";
                        table_27_41_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 0, 0, 0);
                        break;
                    case 28:
                        table_27_41_Cell.Text = "Por operaciones gravadas a la tarifa general";
                        table_27_41_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 0, 0, 0);
                        table_27_41_Cell.CanGrow = false;
                        table_27_41_Cell.Font = new Font("Arial Narrow", 6);
                        break;
                    case 29:
                        table_27_41_Cell.Text = "A.I.U por operaciones gravadas"; /// label
                        table_27_41_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 0, 0, 0);
                        table_27_41_Cell.CanGrow = false;
                        table_27_41_Cell.Font = new Font("Arial", 5);
                        table_27_41_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;
                    case 30:
                        table_27_41_Cell.Text = "Por exportación de bienes";
                        table_27_41_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 0, 0, 0);
                        //table_27_41_Cell.CanGrow = false;
                        //table_27_41_Cell.Font = new Font("Arial", 5);
                        break;
                    case 31:
                        table_27_41_Cell.Text = "Por exportación de servicios";
                        table_27_41_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 0, 0, 0);
                        //table_27_41_Cell.CanGrow = false;
                        //table_27_41_Cell.Font = new Font("Arial", 5);
                        break;
                    case 32:
                        table_27_41_Cell.Text = "Por ventas a sociedades de comercialización"; /// label
                        table_27_41_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 0, 0, 0);
                        table_27_41_Cell.CanGrow = false;
                        table_27_41_Cell.Font = new Font("Arial", 5);
                        table_27_41_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;
                    case 33:
                        table_27_41_Cell.Text = "Por ventas a Zonas Francas";
                        table_27_41_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 0, 0, 0);
                        break;
                    case 34:
                        table_27_41_Cell.Text = "Por juegos de suerte y azar";
                        table_27_41_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 0, 0, 0);
                        break;
                    case 35:
                        table_27_41_Cell.Text = "Por operaciones exentas"; /// label
                        table_27_41_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 0, 0, 0);
                        table_27_41_Cell.CanGrow = false;
                        table_27_41_Cell.Font = new Font("Arial", 5);
                        table_27_41_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;
                    case 36:
                        table_27_41_Cell.Text = "Por venta de cerveza de producción nacional o"; /// label
                        table_27_41_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 0, 0, 0);
                        table_27_41_Cell.CanGrow = false;
                        table_27_41_Cell.Font = new Font("Arial", 5);
                        table_27_41_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;
                    case 37:
                        table_27_41_Cell.Text = "Por operaciones excluidas";
                        table_27_41_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 0, 0, 0);
                        break;
                    case 38:
                        table_27_41_Cell.Text = "Por operaciones no gravadas";
                        table_27_41_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 0, 0, 0);
                        break;
                    case 39:
                        table_27_41_Cell.Text = "Total Ingresos brutos";
                        table_27_41_Row.Font = new System.Drawing.Font("Arial", 6, FontStyle.Bold);
                        table_27_41_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 0, 0, 0);
                        break;
                    case 40:
                        table_27_41_Cell.Text = "Devoluciones en ventas anuladas, rescindidas o";//label
                        table_27_41_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 0, 0, 0);
                        table_27_41_Cell.CanGrow = false;
                        table_27_41_Cell.Font = new Font("Arial", 5);
                        table_27_41_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;
                    case 41:
                        table_27_41_Cell.Text = "Total Ingresos netos recibidos durante el período";
                        table_27_41_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 0, 0, 0);
                        table_27_41_Cell.CanGrow = false;
                        table_27_41_Row.Font = new System.Drawing.Font("Arial", 6, FontStyle.Bold);
                        table_27_41_Cell.Font = new Font("Arial Narrow", 6, FontStyle.Bold);
                        break;
                };
                table_27_41_Row.Cells.Add(table_27_41_Cell);

                table_27_41_Cell = new XRTableCell()
                {
                    WidthF = 20,
                    Text = i.ToString(),
                    Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right,
                    BorderColor = Color.FromArgb(0xDB, 0x53, 0x53),
                };
                table_27_41_Row.Cells.Add(table_27_41_Cell);

                table_27_41_Cell = new XRTableCell();
                table_27_41_Cell.WidthF = (table_27_41.WidthF - 20) / 2;
                table_27_41_Cell.CanGrow = false;
                foreach (DTO_FormDecDetail item in formData.FormDecDetail)
                {
                    if (Convert.ToInt32(item.Renglon.Trim()) == i)
                        table_27_41_Cell.Text = (Math.Round(item.ValorML, 0)).ToString("#,0.");
                };
                table_27_41_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                table_27_41_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0);
                table_27_41_Cell.Font = new System.Drawing.Font("Courier new", 7);
                table_27_41_Row.Cells.Add(table_27_41_Cell);
                table_27_41.Rows.Add(table_27_41_Row);
            };
            table_27_41.EndInit();
            formularioDetail.Controls.Add(table_27_41);

            XRLabel label_27_41 = new XRLabel();
            label_27_41.LocationF = table_27_41.LocationF;
            label_27_41.SizeF = new SizeF(14, 15 * rowHeight);
            label_27_41.AnchorVertical = VerticalAnchorStyles.Both;
            label_27_41.CanGrow = false;
            label_27_41.ForeColor = Color.Black;
            label_27_41.BackColor = Color.White;
            label_27_41.Font = new Font("Arial Narrow", 6, FontStyle.Bold);
            label_27_41.Text = "Ingresos";
            label_27_41.Angle = 90;
            label_27_41.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            label_27_41.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            label_27_41.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            label_27_41.SendToBack();
            formularioDetail.Controls.Add(label_27_41);

            XRLabel label_29 = new XRLabel();
            label_29.LocationF = new PointF(table_27_41.LocationF.X, table_27_41.LocationF.Y + 2 * rowHeight - 1);
            label_29.SizeF = new SizeF((table_27_41.WidthF - 20) / 2, 25);
            label_29.ForeColor = Color.Black;
            label_29.BackColor = Color.Transparent;
            label_29.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_29.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_29.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 0, 0, 0);
            label_29.Multiline = true;
            label_29.Text = "\r\n(base gravable especial)";
            formularioDetail.Controls.Add(label_29);

            XRLabel label_32 = new XRLabel();
            label_32.LocationF = new PointF(table_27_41.LocationF.X, table_27_41.LocationF.Y + 5 * rowHeight - 1);
            label_32.SizeF = new SizeF((table_27_41.WidthF - 20) / 2, 25);
            label_32.ForeColor = Color.Black;
            label_32.BackColor = Color.Transparent;
            label_32.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_32.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_32.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 0, 0, 0);
            label_32.Multiline = true;
            label_32.Text = "\r\ninternacional";
            formularioDetail.Controls.Add(label_32);

            XRLabel label_35 = new XRLabel();
            label_35.LocationF = new PointF(table_27_41.LocationF.X, table_27_41.LocationF.Y + 8 * rowHeight - 1);
            label_35.SizeF = new SizeF((table_27_41.WidthF - 20) / 2, 25);
            label_35.ForeColor = Color.Black;
            label_35.BackColor = Color.Transparent;
            label_35.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_35.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_35.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 0, 0, 0);
            label_35.Multiline = true;
            label_35.Text = "\r\n(Arts.477,478 y 481 del ET)";
            formularioDetail.Controls.Add(label_35);

            XRLabel label_36 = new XRLabel();
            label_36.LocationF = new PointF(table_27_41.LocationF.X, table_27_41.LocationF.Y + 9 * rowHeight - 1);
            label_36.SizeF = new SizeF((table_27_41.WidthF - 20) / 2, 25);
            label_36.ForeColor = Color.Black;
            label_36.BackColor = Color.Transparent;
            label_36.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_36.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_36.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 0, 0, 0);
            label_36.Multiline = true;
            label_36.Text = "\r\nimportada";
            formularioDetail.Controls.Add(label_36);

            XRLabel label_40 = new XRLabel();
            label_40.LocationF = new PointF(table_27_41.LocationF.X, table_27_41.LocationF.Y + 13 * rowHeight - 1);
            label_40.SizeF = new SizeF((table_27_41.WidthF - 20) / 2, 25);
            label_40.ForeColor = Color.Black;
            label_40.BackColor = Color.Transparent;
            label_40.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_40.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_40.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 0, 0, 0);
            label_40.Multiline = true;
            label_40.Text = "\r\nresueltas";
            formularioDetail.Controls.Add(label_40);

            table_27_41.SendToBack();
            #endregion

            #region Table 2
            XRPanel Panel_42_55 = new XRPanel();
            Panel_42_55.LocationF = new PointF(frame_27_60.LocationF.X + labelShift_thick, frame_27_60.LocationF.Y + labelShift_thick + 15 * rowHeight);
            Panel_42_55.SizeF = new SizeF(frame_27_60.WidthF - 2 * labelShift_thick, 14 * rowHeight);
            Panel_42_55.AnchorVertical = VerticalAnchorStyles.Top;
            Panel_42_55.Borders = DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom;
            Panel_42_55.BorderWidth = 1;
            Panel_42_55.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            Panel_42_55.BackColor = Color.Transparent;
            Panel_42_55.BringToFront();
            formularioDetail.Controls.Add(Panel_42_55);

            #region Sub-table 1
            XRTable table_42_47 = new XRTable();
            XRTableRow table_42_47_Row;
            XRTableCell table_42_47_Cell;
            table_42_47.LocationF = Panel_42_55.LocationF;
            table_42_47.SizeF = new SizeF(Panel_42_55.WidthF, 6 * rowHeight);
            table_42_47.StyleName = "tableStyle";
            table_42_47.BeginInit();

            for (int i = 42; i < 48; i++)
            {
                table_42_47_Row = new XRTableRow();
                table_42_47_Row.HeightF = rowHeight;
                table_42_47_Row.BackColor = (i % 2 != 0) ? evenRowColor : oddRowColor;

                table_42_47_Cell = new XRTableCell();
                table_42_47_Cell.WidthF = (table_42_47.WidthF - 20) / 2;
                switch (i)
                {
                    case 42:
                        table_42_47_Cell.Text = "De bienes gravados a la tarifa del 5%";
                        table_42_47_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
                        break;
                    case 43:
                        table_42_47_Cell.Text = "De bienes gravados a la tarifa general";
                        table_42_47_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
                        break;
                    case 44:
                        table_42_47_Cell.Text = "De bienes y servicios gravados provenientes";//label
                        table_42_47_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
                        table_42_47_Cell.CanGrow = false;
                        table_42_47_Cell.Font = new Font("Arial", 5);
                        table_42_47_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;
                    case 45:
                        table_42_47_Cell.Text = "De bienes no gravados";
                        table_42_47_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
                        break;
                    case 46:
                        table_42_47_Cell.Text = "De bienes y servicios no gravados";
                        table_42_47_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
                        table_42_47_Cell.CanGrow = false;
                        table_42_47_Cell.Font = new Font("Arial", 5);
                        table_42_47_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;
                    case 47:
                        table_42_47_Cell.Text = "De servicios";
                        table_42_47_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
                        break;                
                };
                table_42_47_Row.Cells.Add(table_42_47_Cell);

                table_42_47_Cell = new XRTableCell()
                {
                    WidthF = 20,
                    Text = i.ToString(),
                    Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right,
                    BorderColor = Color.FromArgb(0xDB, 0x53, 0x53),
                };
                table_42_47_Row.Cells.Add(table_42_47_Cell);

                table_42_47_Cell = new XRTableCell();
                table_42_47_Cell.WidthF = (table_42_47.WidthF - 20) / 2;
                table_42_47_Cell.CanGrow = false;
                foreach (DTO_FormDecDetail item in formData.FormDecDetail)
                {
                    if (Convert.ToInt32(item.Renglon.Trim()) == i)
                        table_42_47_Cell.Text = (Math.Round(item.ValorML, 0)).ToString("#,0.");
                };
                table_42_47_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                table_42_47_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0);
                table_42_47_Cell.Font = new System.Drawing.Font("Courier new", 7);
                table_42_47_Row.Cells.Add(table_42_47_Cell);
                table_42_47.Rows.Add(table_42_47_Row);
            };
            table_42_47.EndInit();
            formularioDetail.Controls.Add(table_42_47);

            XRLabel label_44 = new XRLabel();
            label_44.LocationF = new PointF(table_42_47.LocationF.X, table_42_47.LocationF.Y + 2 * rowHeight - 1);
            label_44.SizeF = new SizeF((table_42_47.WidthF - 20) / 2, 25);
            label_44.ForeColor = Color.Black;
            label_44.BackColor = Color.Transparent;
            label_44.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_44.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_44.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
            label_44.Multiline = true;
            label_44.Text = "\r\nde Zonas Francas";
            formularioDetail.Controls.Add(label_44);

            XRLabel label_46 = new XRLabel();
            label_46.LocationF = new PointF(table_42_47.LocationF.X, table_42_47.LocationF.Y + 4 * rowHeight - 1);
            label_46.SizeF = new SizeF((table_42_47.WidthF - 20) / 2, 25);
            label_46.ForeColor = Color.Black;
            label_46.BackColor = Color.Transparent;
            label_46.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_46.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_46.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
            label_46.Multiline = true;
            label_46.Text = "\r\nprovenientes de Zonas Francas";
            formularioDetail.Controls.Add(label_46);
            #endregion
            #region Sub-table 2
            XRPanel Panel_48_52 = new XRPanel();
            Panel_48_52.LocationF = new PointF(frame_27_60.LocationF.X + labelShift_thick + 14, frame_27_60.LocationF.Y + labelShift_thick + 21 * rowHeight);
            Panel_48_52.SizeF = new SizeF(frame_27_60.WidthF - 2 * labelShift_thick - 14, 5 * rowHeight);
            Panel_48_52.AnchorVertical = VerticalAnchorStyles.Top;
            Panel_48_52.Borders = DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom;
            Panel_48_52.BorderWidth = 1;
            Panel_48_52.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            Panel_48_52.BackColor = Color.Transparent;
            Panel_48_52.BringToFront();
            formularioDetail.Controls.Add(Panel_48_52);
            
            XRTable table_48_52 = new XRTable();
            XRTableRow table_48_52_Row;
            XRTableCell table_48_52_Cell;
            table_48_52.LocationF = new PointF(Panel_48_52.LocationF.X - 14, Panel_48_52.LocationF.Y);
            table_48_52.SizeF = new SizeF(Panel_48_52.WidthF + 14, Panel_48_52.HeightF);
            table_48_52.StyleName = "tableStyle";
            table_48_52.BeginInit();

            for (int i = 48; i < 53; i++)
            {
                table_48_52_Row = new XRTableRow();
                table_48_52_Row.HeightF = rowHeight;
                table_48_52_Row.BackColor = (i % 2 != 0) ? evenRowColor : oddRowColor;

                table_48_52_Cell = new XRTableCell();
                table_48_52_Cell.WidthF = (table_48_52.WidthF - 20) / 2;
                switch (i)
                {
                    case 48:
                        table_48_52_Cell.Text = "De bienes gravados a la tarifa del 5%";
                        table_48_52_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
                        break;
                    case 49:
                        table_48_52_Cell.Text = "De bienes gravados a la tarifa general";
                        table_48_52_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
                        break;
                    case 50:
                        table_48_52_Cell.Text = "De servicios gravados a la tarifa del 5%";
                        table_48_52_Cell.Font = new Font("Arial Narrow", 6);
                        table_48_52_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
                        break;
                    case 51:
                        table_48_52_Cell.Text = "De servicios gravados a la tarifa general";
                        table_48_52_Cell.Font = new Font("Arial Narrow", 6);
                        table_48_52_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
                        break;
                    case 52:
                        table_48_52_Cell.Text = "De bienes y servicios no gravados";
                        table_48_52_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
                        break;
                };
                table_48_52_Row.Cells.Add(table_48_52_Cell);

                table_48_52_Cell = new XRTableCell()
                {
                    WidthF = 20,
                    Text = i.ToString(),
                    Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right,
                    BorderColor = Color.FromArgb(0xDB, 0x53, 0x53),
                };
                table_48_52_Row.Cells.Add(table_48_52_Cell);

                table_48_52_Cell = new XRTableCell();
                table_48_52_Cell.WidthF = (table_48_52.WidthF - 20) / 2;
                table_48_52_Cell.CanGrow = false;
                foreach (DTO_FormDecDetail item in formData.FormDecDetail)
                {
                    if (Convert.ToInt32(item.Renglon.Trim()) == i)
                        table_48_52_Cell.Text = (Math.Round(item.ValorML, 0)).ToString("#,0.");
                };
                table_48_52_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                table_48_52_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0);
                table_48_52_Cell.Font = new System.Drawing.Font("Courier new", 7);
                table_48_52_Row.Cells.Add(table_48_52_Cell);
                table_48_52.Rows.Add(table_48_52_Row);
            };
            table_48_52.EndInit();
            formularioDetail.Controls.Add(table_48_52);
            #endregion
            #region Sub-table 3
            XRTable table_53_55 = new XRTable();
            XRTableRow table_53_55_Row;
            XRTableCell table_53_55_Cell;
            table_53_55.LocationF = new PointF(frame_27_60.LocationF.X + labelShift_thick, frame_27_60.LocationF.Y + labelShift_thick + 26 * rowHeight);
            table_53_55.SizeF = new SizeF(frame_27_60.WidthF - 2 * labelShift_thick, 3 * rowHeight);
            table_53_55.StyleName = "tableStyle";
            table_53_55.BeginInit();

            for (int i = 53; i < 56; i++)
            {
                table_53_55_Row = new XRTableRow();
                table_53_55_Row.HeightF = rowHeight;
                table_53_55_Row.BackColor = (i % 2 != 0) ? evenRowColor : oddRowColor;

                table_53_55_Cell = new XRTableCell();
                table_53_55_Cell.WidthF = (table_53_55.WidthF - 20) / 2;
                switch (i)
                {
                    case 53:
                        table_53_55_Cell.Text = "Total compras e importaciones brutas";
                        table_53_55_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 0, 0, 0);
                        table_53_55_Cell.CanGrow = false;
                        table_53_55_Row.Font = new System.Drawing.Font("Arial", 6, FontStyle.Bold);
                        table_53_55_Cell.Font = new Font("Arial", 5, FontStyle.Bold);
                        break;
                    case 54:
                        table_53_55_Cell.Text = "Devoluciones en compras anuladas, rescididas";//label
                        table_53_55_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 0, 0, 0);
                        table_53_55_Cell.CanGrow = false;
                        table_53_55_Cell.Font = new Font("Arial", 5);
                        table_53_55_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;
                    case 55:
                        table_53_55_Cell.Text = "Total compras netas realizadas durante el período";
                        table_53_55_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 0, 0, 0);
                        table_53_55_Cell.CanGrow = false;
                        table_53_55_Row.Font = new System.Drawing.Font("Arial", 6, FontStyle.Bold);
                        table_53_55_Cell.Font = new Font("Arial", 5, FontStyle.Bold);
                        table_53_55_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;                   
                };
                table_53_55_Row.Cells.Add(table_53_55_Cell);

                table_53_55_Cell = new XRTableCell()
                {
                    WidthF = 20,
                    Text = i.ToString(),
                    Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right,
                    BorderColor = Color.FromArgb(0xDB, 0x53, 0x53),
                };
                table_53_55_Row.Cells.Add(table_53_55_Cell);

                table_53_55_Cell = new XRTableCell();
                table_53_55_Cell.WidthF = (table_53_55.WidthF - 20) / 2;
                table_53_55_Cell.CanGrow = false;
                foreach (DTO_FormDecDetail item in formData.FormDecDetail)
                {
                    if (Convert.ToInt32(item.Renglon.Trim()) == i)
                        table_53_55_Cell.Text = (Math.Round(item.ValorML, 0)).ToString("#,0.");
                };
                table_53_55_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                table_53_55_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0);
                table_53_55_Cell.Font = new System.Drawing.Font("Courier new", 7);
                table_53_55_Row.Cells.Add(table_53_55_Cell);
                table_53_55.Rows.Add(table_53_55_Row);
            };
            table_53_55.EndInit();
            formularioDetail.Controls.Add(table_53_55);

            XRLabel label_54 = new XRLabel();
            label_54.LocationF = new PointF(table_53_55.LocationF.X, table_53_55.LocationF.Y + 1 * rowHeight - 1);
            label_54.SizeF = new SizeF((table_53_55.WidthF - 20) / 2, 25);
            label_54.ForeColor = Color.Black;
            label_54.BackColor = Color.Transparent;
            label_54.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_54.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_54.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 0, 0, 0);
            label_54.Multiline = true;
            label_54.Text = "\r\nrescindidas o resueltas en este período";
            formularioDetail.Controls.Add(label_54);
            #endregion

            XRLabel label_42_55 = new XRLabel();
            label_42_55.LocationF = table_42_47.LocationF;
            label_42_55.SizeF = new SizeF(14, 14 * rowHeight);
            label_42_55.CanGrow = false;
            label_42_55.ForeColor = Color.Black;
            label_42_55.BackColor = Color.White;
            label_42_55.Font = new Font("Arial Narrow", 6, FontStyle.Bold);
            label_42_55.Text = "Compras";
            label_42_55.Angle = 90;
            label_42_55.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            label_42_55.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            label_42_55.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            label_42_55.SendToBack();
            formularioDetail.Controls.Add(label_42_55);

            XRLabel label_42_47 = new XRLabel();
            label_42_47.LocationF = new PointF(label_42_55.LocationF.X + label_42_55.WidthF, label_42_55.LocationF.Y);
            label_42_47.SizeF = new SizeF(14, 6 * rowHeight);
            label_42_47.CanGrow = false;
            label_42_47.ForeColor = Color.Black;
            label_42_47.BackColor = Color.White;
            label_42_47.Font = new Font("Arial Narrow", 6, FontStyle.Bold);
            label_42_47.Text = "Importación";
            label_42_47.Angle = 90;
            label_42_47.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            label_42_47.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            label_42_47.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            label_42_47.SendToBack();
            formularioDetail.Controls.Add(label_42_47);

            XRLabel label_48_52 = new XRLabel();
            label_48_52.LocationF = new PointF(label_42_55.LocationF.X + label_42_55.WidthF, label_42_47.LocationF.Y + label_42_47.HeightF);
            label_48_52.SizeF = new SizeF(14, 5 * rowHeight);
            label_48_52.CanGrow = false;
            label_48_52.ForeColor = Color.Black;
            label_48_52.BackColor = Color.White;
            label_48_52.Font = new Font("Arial Narrow", 6, FontStyle.Bold);
            label_48_52.Text = "Nacionales";
            label_48_52.Angle = 90;
            label_48_52.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            label_48_52.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            label_48_52.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            label_48_52.SendToBack();
            formularioDetail.Controls.Add(label_48_52);

            table_42_47.SendToBack();
            table_48_52.SendToBack();
            table_53_55.SendToBack();
            #endregion

            #region Table 3
            XRTable table_56_60 = new XRTable();
            XRTableRow table_56_60_Row;
            XRTableCell table_56_60_Cell;
            table_56_60.LocationF = new PointF(Panel_42_55.LocationF.X, Panel_42_55.LocationF.Y + Panel_42_55.HeightF);
            table_56_60.SizeF = new SizeF(Panel_42_55.WidthF, 5 * rowHeight);
            table_56_60.StyleName = "tableStyle";
            table_56_60.BeginInit();

            for (int i = 56; i < 61; i++)
            {
                table_56_60_Row = new XRTableRow();
                table_56_60_Row.HeightF = rowHeight;
                table_56_60_Row.BackColor = (i % 2 != 0) ? evenRowColor : oddRowColor;

                table_56_60_Cell = new XRTableCell();
                table_56_60_Cell.WidthF = (table_56_60.WidthF - 20) / 2;
                switch (i)
                {
                    case 56:
                        table_56_60_Cell.Text = "A la tarifa del 5%";
                        table_56_60_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
                        break;
                    case 57:
                        table_56_60_Cell.Text = "A la tarifa general";
                        table_56_60_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
                        table_56_60_Cell.CanGrow = false;
                        table_56_60_Cell.Font = new Font("Arial", 5);
                        table_56_60_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;
                    case 58:
                        table_56_60_Cell.Text = "Sobre A.I.U en operaciones gravadas"; // label
                        table_56_60_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
                        table_56_60_Cell.CanGrow = false;
                        table_56_60_Cell.Font = new Font("Arial", 5);
                        table_56_60_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;
                    case 59:
                        table_56_60_Cell.Text = "Por venta de juegos de suerte y azar";
                        table_56_60_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
                        break;
                    case 60:
                        table_56_60_Cell.Text = "En venta serveza de producción nacional"; // label
                        table_56_60_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
                        table_56_60_Cell.CanGrow = false;
                        table_56_60_Cell.Font = new Font("Arial", 5);
                        table_56_60_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;                 
                };
                table_56_60_Row.Cells.Add(table_56_60_Cell);

                table_56_60_Cell = new XRTableCell()
                {
                    WidthF = 20,
                    Text = i.ToString(),
                    Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right,
                    BorderColor = Color.FromArgb(0xDB, 0x53, 0x53),
                };
                table_56_60_Row.Cells.Add(table_56_60_Cell);

                table_56_60_Cell = new XRTableCell();
                table_56_60_Cell.WidthF = (table_56_60.WidthF - 20) / 2;
                table_56_60_Cell.CanGrow = false;
                foreach (DTO_FormDecDetail item in formData.FormDecDetail)
                {
                    if (Convert.ToInt32(item.Renglon.Trim()) == i)
                        table_56_60_Cell.Text = (Math.Round(item.ValorML, 0)).ToString("#,0.");
                };
                table_56_60_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                table_56_60_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0);
                table_56_60_Cell.Font = new System.Drawing.Font("Courier new", 7);
                table_56_60_Row.Cells.Add(table_56_60_Cell);
                table_56_60.Rows.Add(table_56_60_Row);
            };
            table_56_60.EndInit();
            formularioDetail.Controls.Add(table_56_60);

            XRLabel label_56_60 = new XRLabel();
            label_56_60.LocationF = table_56_60.LocationF;
            label_56_60.SizeF = new SizeF(14, 5 * rowHeight);
            label_56_60.CanGrow = false;
            label_56_60.ForeColor = Color.Black;
            label_56_60.BackColor = Color.White;
            label_56_60.Font = new Font("Arial", 5, FontStyle.Bold);
            label_56_60.Text = "Liquidación";
            label_56_60.Angle = 90;
            label_56_60.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            label_56_60.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            label_56_60.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            label_56_60.SendToBack();
            formularioDetail.Controls.Add(label_56_60);

            XRLabel label_56_60_add = new XRLabel();
            label_56_60_add.LocationF = new PointF(label_56_60.LocationF.X - 2, label_56_60.LocationF.Y);
            label_56_60_add.SizeF = new SizeF(20, 5 * rowHeight);
            label_56_60_add.ForeColor = Color.Black;
            label_56_60_add.BackColor = Color.Transparent;
            label_56_60_add.Font = new Font("Arial", 5, FontStyle.Bold);
            label_56_60_add.Multiline = true;
            label_56_60_add.Text = "\r\nprivada";
            label_56_60_add.Angle = 90;
            label_56_60_add.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            formularioDetail.Controls.Add(label_56_60_add);
            label_56_60_add.BringToFront();

            XRLabel label_56_60_ = new XRLabel();
            label_56_60_.LocationF = new PointF(label_56_60.LocationF.X + label_56_60.WidthF, label_56_60.LocationF.Y);
            label_56_60_.SizeF = new SizeF(14, 5 * rowHeight);
            label_56_60_.CanGrow = false;
            label_56_60_.ForeColor = Color.Black;
            label_56_60_.BackColor = Color.White;
            label_56_60_.Font = new Font("Arial", 5, FontStyle.Bold);
            label_56_60_.Text = "Impuesto";
            label_56_60_.Angle = 90;
            label_56_60_.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            label_56_60_.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            label_56_60_.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            formularioDetail.Controls.Add(label_56_60_);
            label_56_60_.SendToBack();

            XRLabel label_56_60__add = new XRLabel();
            label_56_60__add.LocationF = new PointF(label_56_60_.LocationF.X - 2, label_56_60_.LocationF.Y);
            label_56_60__add.SizeF = new SizeF(20, 5 * rowHeight);
            label_56_60__add.ForeColor = Color.Black;
            label_56_60__add.BackColor = Color.Transparent;
            label_56_60__add.Font = new Font("Arial", 5, FontStyle.Bold);
            label_56_60__add.Multiline = true;
            label_56_60__add.Text = "\r\nGenerado";
            label_56_60__add.Angle = 90;
            label_56_60__add.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            formularioDetail.Controls.Add(label_56_60__add);
            label_56_60__add.BringToFront();

            XRLabel label_43 = new XRLabel();
            label_43.LocationF = new PointF(table_56_60.LocationF.X, table_56_60.LocationF.Y + 1 * rowHeight - 1);
            label_43.SizeF = new SizeF((table_56_60.WidthF - 20) / 2, 25);
            label_43.ForeColor = Color.Black;
            label_43.BackColor = Color.Transparent;
            label_43.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_43.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_43.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
            label_43.Multiline = true;
            label_43.Text = "\r\nde producción nacional o importada";
            formularioDetail.Controls.Add(label_43);

            XRLabel label_58 = new XRLabel();
            label_58.LocationF = new PointF(table_56_60.LocationF.X, table_56_60.LocationF.Y + 2 * rowHeight - 1);
            label_58.SizeF = new SizeF((table_56_60.WidthF - 20) / 2, 25);
            label_58.ForeColor = Color.Black;
            label_58.BackColor = Color.Transparent;
            label_58.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_58.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_58.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
            label_58.Multiline = true;
            label_58.Text = "\r\n(base gravable especial)";
            formularioDetail.Controls.Add(label_58);

            XRLabel label_60 = new XRLabel();
            label_60.LocationF = new PointF(table_56_60.LocationF.X, table_56_60.LocationF.Y + 4 * rowHeight - 1);
            label_60.SizeF = new SizeF((table_56_60.WidthF - 20) / 2, 25);
            label_60.ForeColor = Color.Black;
            label_60.BackColor = Color.Transparent;
            label_60.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_60.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_60.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
            label_60.Multiline = true;
            label_60.Text = "\r\no importada";
            formularioDetail.Controls.Add(label_60);

            table_56_60.SendToBack();
            #endregion
            #endregion

            #region Right part
            XRShape frame_61_94 = new XRShape();
            frame_61_94.LocationF = new PointF(frame_27_60.WidthF - frameShift, frame_27_60.LocationF.Y);
            frame_61_94.SizeF = new SizeF(formularioWidth / 2 + frameShift, 34 * rowHeight + 2 * labelShift_thick);
            frame_61_94.AnchorVertical = VerticalAnchorStyles.Top;
            frame_61_94.LineWidth = 2;
            frame_61_94.BackColor = Color.Transparent;
            frame_61_94.ForeColor = Color.FromArgb(0xDB, 0x53, 0x53);
            frame_61_94.Shape = new ShapeRectangle()
            {
                Fillet = 3,
            };
            frame_61_94.CanGrow = false;
            frame_61_94.AnchorVertical = VerticalAnchorStyles.Top;
            formularioDetail.Controls.Add(frame_61_94);
            
            #region Table 1
            #region Sub-table 1
            XRTable table_61_63 = new XRTable();
            XRTableRow table_61_63_Row;
            XRTableCell table_61_63_Cell;
            table_61_63.LocationF = new PointF(frame_61_94.LocationF.X + labelShift_thick, frame_61_94.LocationF.Y + labelShift_thick);
            table_61_63.SizeF = new SizeF(frame_61_94.WidthF - 2 * labelShift_thick, 3 * rowHeight);
            table_61_63.StyleName = "tableStyle";
            table_61_63.BeginInit();

            for (int i = 61; i < 64; i++)
            {
                table_61_63_Row = new XRTableRow();
                table_61_63_Row.HeightF = rowHeight;
                table_61_63_Row.BackColor = (i % 2 != 0) ? oddRowColor : evenRowColor;

                table_61_63_Cell = new XRTableCell();
                table_61_63_Cell.WidthF = (table_61_63.WidthF - 20) / 2;
                switch (i)
                {
                    case 61: // label
                        table_61_63_Cell.Text = "En retiro inventar. para activ. fijos, consumo,"; 
                        table_61_63_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
                        table_61_63_Cell.CanGrow = false;
                        table_61_63_Cell.Font = new Font("Arial", 5);
                        table_61_63_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;
                    case 62:
                        table_61_63_Cell.Text = "IVA recuperando en devoluciones en compras";
                        table_61_63_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
                        table_61_63_Cell.CanGrow = false;
                        table_61_63_Cell.Font = new Font("Arial", 5);
                        table_61_63_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;
                    case 63:
                        table_61_63_Cell.Text = "Total impuesto generado por operaciones";
                        table_61_63_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);                        
                        table_61_63_Cell.CanGrow = false;
                        table_61_63_Row.Font = new Font("Arial", 6, FontStyle.Bold);
                        table_61_63_Cell.Font = new Font("Arial", 5, FontStyle.Bold);
                        table_61_63_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;               
                };
                table_61_63_Row.Cells.Add(table_61_63_Cell);
                table_61_63_Cell = new XRTableCell()
                {
                    WidthF = 20,
                    Text = i.ToString(),
                    Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right,
                    BorderColor = Color.FromArgb(0xDB, 0x53, 0x53)
                };
                table_61_63_Row.Cells.Add(table_61_63_Cell);
                table_61_63_Cell = new XRTableCell();
                table_61_63_Cell.WidthF = (table_61_63.WidthF - 20) / 2;
                table_61_63_Cell.CanGrow = false;
                foreach (DTO_FormDecDetail item in formData.FormDecDetail)
                {
                    if (Convert.ToInt32(item.Renglon.Trim()) == i)
                        table_61_63_Cell.Text = (Math.Round(item.ValorML, 0)).ToString("#,0.");
                };
                table_61_63_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                table_61_63_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0);
                table_61_63_Cell.Font = new System.Drawing.Font("Courier new", 7);
                table_61_63_Row.Cells.Add(table_61_63_Cell);
                table_61_63.Rows.Add(table_61_63_Row);
            };
            table_61_63.EndInit();
            formularioDetail.Controls.Add(table_61_63);

            XRLabel label_61 = new XRLabel();
            label_61.LocationF = new PointF(table_61_63.LocationF.X, table_61_63.LocationF.Y - 1);
            label_61.SizeF = new SizeF((table_61_63.WidthF - 20) / 2, 25);
            label_61.ForeColor = Color.Black;
            label_61.BackColor = Color.Transparent;
            label_61.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_61.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_61.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
            label_61.Multiline = true;
            label_61.Text = "\r\nmuestras gratis o donaciones";
            formularioDetail.Controls.Add(label_61);

            XRLabel label_62 = new XRLabel();
            label_62.LocationF = new PointF(table_61_63.LocationF.X, table_61_63.LocationF.Y + 1 * rowHeight - 1);
            label_62.SizeF = new SizeF((table_61_63.WidthF - 20) / 2, 25);
            label_62.ForeColor = Color.Black;
            label_62.BackColor = Color.Transparent;
            label_62.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_62.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_62.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
            label_62.Multiline = true;
            label_62.Text = "\r\nanuladas, rescinadas o resueltas";
            formularioDetail.Controls.Add(label_62);

            XRLabel label_63 = new XRLabel();
            label_63.LocationF = new PointF(table_61_63.LocationF.X, table_61_63.LocationF.Y + 2 * rowHeight - 1);
            label_63.SizeF = new SizeF((table_61_63.WidthF - 20) / 2, 25);
            label_63.ForeColor = Color.Black;
            label_63.BackColor = Color.Transparent;
            label_63.Font = new System.Drawing.Font("Arial", 5, FontStyle.Bold);
            label_63.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_63.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
            label_63.Multiline = true;
            label_63.Text = "\r\ngravadas";
            formularioDetail.Controls.Add(label_63);
            #endregion
            #region Sub-table 2
            XRPanel Panel_64_77 = new XRPanel();
            Panel_64_77.LocationF = new PointF(frame_61_94.LocationF.X + labelShift_thick + 14, frame_61_94.LocationF.Y + labelShift_thick + 3 * rowHeight);
            Panel_64_77.SizeF = new SizeF(frame_61_94.WidthF - 2 * labelShift_thick - 14, 14 * rowHeight);
            Panel_64_77.AnchorVertical = VerticalAnchorStyles.Top;
            Panel_64_77.Borders = DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom;
            Panel_64_77.BorderWidth = 1;
            Panel_64_77.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            Panel_64_77.BackColor = Color.Transparent;
            Panel_64_77.BringToFront();
            formularioDetail.Controls.Add(Panel_64_77);

            XRTable table_64_77 = new XRTable();
            XRTableRow table_64_77_Row;
            XRTableCell table_64_77_Cell;
            table_64_77.LocationF = new PointF(Panel_64_77.LocationF.X - 14, Panel_64_77.LocationF.Y);
            table_64_77.SizeF = new SizeF(Panel_64_77.WidthF + 14, Panel_64_77.HeightF);
            table_64_77.StyleName = "tableStyle";
            table_64_77.BeginInit();

            for (int i = 64; i < 78; i++)
            {
                table_64_77_Row = new XRTableRow();
                table_64_77_Row.HeightF = rowHeight;
                table_64_77_Row.BackColor = (i % 2 != 0) ? oddRowColor : evenRowColor;

                table_64_77_Cell = new XRTableCell();
                table_64_77_Cell.WidthF = (table_64_77.WidthF - 20) / 2;
                switch (i)
                {
                    case 64:
                        table_64_77_Cell.Text = "Por importaciones gravadas a tarifa del 5%";
                        table_64_77_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
                        table_64_77_Cell.Font = new Font("Arial Narrow", 6);
                        break;
                    case 65:
                        table_64_77_Cell.Text = "Por importaciones gravadas a tarifa general";
                        table_64_77_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
                        table_64_77_Cell.Font = new Font("Arial Narrow", 6);
                        break;
                    case 66:
                        table_64_77_Cell.Text = "De bienes y servicios gravados provenientales"; // label
                        table_64_77_Cell.CanGrow = false;
                        table_64_77_Cell.Font = new Font("Arial", 5);
                        table_64_77_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        table_64_77_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
                        break;
                    case 67:
                        table_64_77_Cell.Text = "Por compras de bienes gravados a la tarifa 5%";
                        table_64_77_Cell.CanGrow = false;
                        table_64_77_Cell.Font = new Font("Arial Narrow", 6);
                        table_64_77_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
                        break;
                    case 68:
                        table_64_77_Cell.Text = "Por compras de bienes gravados a tarifa"; // label
                        table_64_77_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
                        table_64_77_Cell.CanGrow = false;
                        table_64_77_Cell.Font = new Font("Arial",5);
                        table_64_77_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;
                    case 69:
                        table_64_77_Cell.Text = "Por servicios gravados a la tarifa del 5%";// label
                        table_64_77_Cell.CanGrow = false;
                        table_64_77_Cell.Font = new Font("Arial", 6);
                        table_64_77_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
                        table_64_77_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;
                    case 70:
                        table_64_77_Cell.Text = "Por servicios gravados a la tarifa general";
                        table_64_77_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
                        table_64_77_Cell.Font = new Font("Arial Narrow", 6);
                        break;
                    case 71:
                        table_64_77_Cell.Text = "Total Impuesto pagado o facturado";
                        table_64_77_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
                        table_64_77_Row.Font = new Font("Arial", 6, FontStyle.Bold);
                        break;                   
                    case 72:
                        table_64_77_Cell.Text = "IVA retenido en operaciones con régimen";
                        table_64_77_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
                        table_64_77_Cell.CanGrow = false;
                        table_64_77_Cell.Font = new Font("Arial", 5);
                        table_64_77_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;
                    case 73:
                        table_64_77_Cell.Text = "IVA retenido por servicios prestados en";// label
                        table_64_77_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
                        table_64_77_Cell.CanGrow = false;
                        table_64_77_Cell.Font = new Font("Arial", 5);
                        table_64_77_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;
                    case 74:
                        table_64_77_Cell.Text = "IVA resultante por devoluciones en ventas";// label
                        table_64_77_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
                        table_64_77_Cell.CanGrow = false;
                        table_64_77_Cell.Font = new Font("Arial", 5);
                        table_64_77_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;
                    case 75:
                        table_64_77_Cell.Text = "IVA descontable por impuesto nacional a la"; // label
                        table_64_77_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
                        table_64_77_Cell.CanGrow = false;
                        table_64_77_Cell.Font = new Font("Arial", 5);
                        table_64_77_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;
                    case 76:
                        table_64_77_Cell.Text = "Ajuste impuestos descontables (perdidas,";
                        table_64_77_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
                        table_64_77_Cell.CanGrow = false;
                        table_64_77_Cell.Font = new Font("Arial", 5);
                        table_64_77_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;
                    case 77:
                        table_64_77_Cell.Text = "Total impuestos descontables";
                        table_64_77_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
                        table_64_77_Row.Font = new Font("Arial", 6, FontStyle.Bold);
                        break;
                };
                table_64_77_Row.Cells.Add(table_64_77_Cell);
                table_64_77_Cell = new XRTableCell()
                {
                    WidthF = 20,
                    Text = i.ToString(),
                    Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right,
                    BorderColor = Color.FromArgb(0xDB, 0x53, 0x53)
                };
                table_64_77_Row.Cells.Add(table_64_77_Cell);
                table_64_77_Cell = new XRTableCell();
                table_64_77_Cell.WidthF = (table_64_77.WidthF - 20) / 2;
                table_64_77_Cell.CanGrow = false;
                foreach (DTO_FormDecDetail item in formData.FormDecDetail)
                {
                    if (Convert.ToInt32(item.Renglon.Trim()) == i)
                        table_64_77_Cell.Text = (Math.Round(item.ValorML, 0)).ToString("#,0.");
                };
                table_64_77_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                table_64_77_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0);
                table_64_77_Cell.Font = new System.Drawing.Font("Courier new", 7);
                table_64_77_Row.Cells.Add(table_64_77_Cell);
                table_64_77.Rows.Add(table_64_77_Row);
            };
            table_64_77.EndInit();
            formularioDetail.Controls.Add(table_64_77);

            XRLabel label_66 = new XRLabel();
            label_66.LocationF = new PointF(table_64_77.LocationF.X, table_64_77.LocationF.Y + 2 * rowHeight - 1);
            label_66.SizeF = new SizeF((table_64_77.WidthF - 20) / 2, 25);
            label_66.ForeColor = Color.Black;
            label_66.BackColor = Color.Transparent;
            label_66.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_66.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_66.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
            label_66.Multiline = true;
            label_66.Text = "\r\nde Zonas Francas";
            formularioDetail.Controls.Add(label_66);

            XRLabel label_68 = new XRLabel();
            label_68.LocationF = new PointF(table_64_77.LocationF.X, table_64_77.LocationF.Y + 4 * rowHeight - 1);
            label_68.SizeF = new SizeF((table_64_77.WidthF - 20) / 2, 25);
            label_68.ForeColor = Color.Black;
            label_68.BackColor = Color.Transparent;
            label_68.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_68.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_68.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
            label_68.Multiline = true;
            label_68.Text = "\r\ngeneral";
            formularioDetail.Controls.Add(label_68);

            XRLabel label_72 = new XRLabel();
            label_72.LocationF = new PointF(table_64_77.LocationF.X, table_64_77.LocationF.Y + 8 * rowHeight - 1);
            label_72.SizeF = new SizeF((table_64_77.WidthF - 20) / 2, 25);
            label_72.ForeColor = Color.Black;
            label_72.BackColor = Color.Transparent;
            label_72.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_72.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_72.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
            label_72.Multiline = true;
            label_72.Text = "\r\nsimplificado";
            formularioDetail.Controls.Add(label_72);

            XRLabel label_73 = new XRLabel();
            label_73.LocationF = new PointF(table_64_77.LocationF.X, table_64_77.LocationF.Y + 9 * rowHeight - 1);
            label_73.SizeF = new SizeF((table_64_77.WidthF - 20) / 2, 25);
            label_73.ForeColor = Color.Black;
            label_73.BackColor = Color.Transparent;
            label_73.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_73.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_73.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
            label_73.Multiline = true;
            label_73.Text = "\r\nColombia por no domicilios y no residentes";
            formularioDetail.Controls.Add(label_73);

            XRLabel label_74 = new XRLabel();
            label_74.LocationF = new PointF(table_64_77.LocationF.X, table_64_77.LocationF.Y + 10 * rowHeight - 1);
            label_74.SizeF = new SizeF((table_64_77.WidthF - 20) / 2, 25);
            label_74.ForeColor = Color.Black;
            label_74.BackColor = Color.Transparent;
            label_74.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_74.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_74.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
            label_74.Multiline = true;
            label_74.Text = "\r\nanuladas, rescindidas o resueltas";
            formularioDetail.Controls.Add(label_74);

            XRLabel label_75 = new XRLabel();
            label_75.LocationF = new PointF(table_64_77.LocationF.X, table_64_77.LocationF.Y + 11 * rowHeight - 1);
            label_75.SizeF = new SizeF((table_64_77.WidthF - 20) / 2, 25);
            label_75.ForeColor = Color.Black;
            label_75.BackColor = Color.Transparent;
            label_75.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_75.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_75.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
            label_75.Multiline = true;
            label_75.Text = "\r\ngasolina y ACPM";
            formularioDetail.Controls.Add(label_75);

            XRLabel label_76 = new XRLabel();
            label_76.LocationF = new PointF(table_64_77.LocationF.X, table_64_77.LocationF.Y + 12 * rowHeight - 1);
            label_76.SizeF = new SizeF((table_64_77.WidthF - 20) / 2, 25);
            label_76.ForeColor = Color.Black;
            label_76.BackColor = Color.Transparent;
            label_76.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_76.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_76.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
            label_76.Multiline = true;
            label_76.Text = "\r\nhurto o castigo de inventarios)";
            formularioDetail.Controls.Add(label_76);
            #endregion
            #region Sub-table 3
            XRTable table_78_85 = new XRTable();
            XRTableRow table_78_85_Row;
            XRTableCell table_78_85_Cell;
            table_78_85.LocationF = new PointF(frame_61_94.LocationF.X + labelShift_thick, frame_61_94.LocationF.Y + labelShift_thick + 17 * rowHeight);
            table_78_85.SizeF = new SizeF(frame_61_94.WidthF - 2 * labelShift_thick, 8 * rowHeight);
            table_78_85.StyleName = "tableStyle";
            table_78_85.BeginInit();

            for (int i = 78; i < 86; i++)
            {
                table_78_85_Row = new XRTableRow();
                table_78_85_Row.HeightF = rowHeight;
                table_78_85_Row.BackColor = (i % 2 != 0) ? oddRowColor : evenRowColor;

                table_78_85_Cell = new XRTableCell();
                table_78_85_Cell.WidthF = (table_78_85.WidthF - 20) / 2;
                switch (i)
                {
                    case 78:
                        table_78_85_Cell.Text = "Saldo a pagar por el período fiscal";
                        table_78_85_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 0, 0, 0);
                        table_78_85_Row.Font = new Font("Arial", 6, FontStyle.Bold);
                        break;
                    case 79:
                        table_78_85_Cell.Text = "Saldo a favor del período fiscal";
                        table_78_85_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 0, 0, 0);
                        table_78_85_Row.Font = new Font("Arial", 6, FontStyle.Bold);
                        break;
                    case 80:
                        table_78_85_Cell.Text = "Saldo a favor del período fiscal anterior";
                        table_78_85_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 0, 0, 0);
                        break;
                    case 81:
                        table_78_85_Cell.Text = "Retenciónes por IVA que le practicaron";
                        table_78_85_Cell.CanGrow = false;
                        table_78_85_Cell.Font = new Font("Arial Narrow", 6);
                        table_78_85_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 0, 0, 0);
                        break;
                    case 82:
                        table_78_85_Cell.Text = "Saldo a pagar por impuesto";
                        table_78_85_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 0, 0, 0);
                        table_78_85_Row.Font = new Font("Arial", 6, FontStyle.Bold);
                        break;
                    case 83:
                        table_78_85_Cell.Text = "Senciones";
                        table_78_85_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 0, 0, 0);
                        break;
                    case 84:
                        table_78_85_Cell.Text = "Total saldo a pagar por este periodo";
                        table_78_85_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 0, 0, 0);
                        table_78_85_Row.Font = new Font("Arial", 6, FontStyle.Bold);
                        break;
                    case 85:
                        table_78_85_Cell.Text = "o Total saldo a favor por este periodo";
                        table_78_85_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 0, 0, 0);
                        table_78_85_Row.Font = new Font("Arial", 6, FontStyle.Bold);                       
                        break;
                };
                table_78_85_Row.Cells.Add(table_78_85_Cell);
                table_78_85_Cell = new XRTableCell()
                {
                    WidthF = 20,
                    Text = i.ToString(),
                    Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right,
                    BorderColor = Color.FromArgb(0xDB, 0x53, 0x53)
                };
                table_78_85_Row.Cells.Add(table_78_85_Cell);
                table_78_85_Cell = new XRTableCell();
                table_78_85_Cell.WidthF = (table_78_85.WidthF - 20) / 2;
                table_78_85_Cell.CanGrow = false;
                foreach (DTO_FormDecDetail item in formData.FormDecDetail)
                {
                    if (Convert.ToInt32(item.Renglon.Trim()) == i)
                        table_78_85_Cell.Text = (Math.Round(item.ValorML, 0)).ToString("#,0.");
                };
                table_78_85_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                table_78_85_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0);
                table_78_85_Cell.Font = new System.Drawing.Font("Courier new", 7);
                table_78_85_Row.Cells.Add(table_78_85_Cell);
                table_78_85.Rows.Add(table_78_85_Row);
            };
            table_78_85.EndInit();
            formularioDetail.Controls.Add(table_78_85);
            #endregion

            XRLabel label_61_77 = new XRLabel();
            label_61_77.LocationF = table_61_63.LocationF;
            label_61_77.SizeF = new SizeF(14, 25 * rowHeight);
            label_61_77.CanGrow = false;
            label_61_77.ForeColor = Color.Black;
            label_61_77.BackColor = Color.White;
            label_61_77.Font = new Font("Arial Narrow", 6, FontStyle.Bold);
            label_61_77.Text = "Liquidación privada (Continuación)";
            label_61_77.Angle = 90;
            label_61_77.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            label_61_77.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            label_61_77.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            label_61_77.SendToBack();
            formularioDetail.Controls.Add(label_61_77);

            XRLabel label_61_63 = new XRLabel();
            label_61_63.LocationF = new PointF(label_61_77.LocationF.X + label_61_77.WidthF, label_61_77.LocationF.Y);
            label_61_63.SizeF = new SizeF(14, 3 * rowHeight);
            label_61_63.CanGrow = false;
            label_61_63.ForeColor = Color.Black;
            label_61_63.BackColor = Color.White;
            label_61_63.Font = new Font("Arial", 5, FontStyle.Bold);
            label_61_63.Text = "Impuesto";
            label_61_63.Angle = 90;
            label_61_63.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            label_61_63.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            label_61_63.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            label_61_63.SendToBack();
            formularioDetail.Controls.Add(label_61_63);

            XRLabel label_61_63_add = new XRLabel();
            label_61_63_add.LocationF = new PointF(label_61_63.LocationF.X - 2, label_61_63.LocationF.Y);
            label_61_63_add.SizeF = new SizeF(20, 3 * rowHeight);
            label_61_63_add.ForeColor = Color.Black;
            label_61_63_add.BackColor = Color.Transparent;
            label_61_63_add.Font = new Font("Arial", 5, FontStyle.Bold);
            label_61_63_add.Multiline = true;
            label_61_63_add.Text = "\r\nGenerado";
            label_61_63_add.Angle = 90;
            label_61_63_add.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            formularioDetail.Controls.Add(label_61_63_add);
            label_61_63_add.BringToFront();

            XRLabel label_64_77 = new XRLabel();
            label_64_77.LocationF = new PointF(label_61_77.LocationF.X + label_61_77.WidthF, label_61_63.LocationF.Y + label_61_63.HeightF);
            label_64_77.SizeF = new SizeF(14, 14 * rowHeight);
            label_64_77.CanGrow = false;
            label_64_77.ForeColor = Color.Black;
            label_64_77.BackColor = Color.White;
            label_64_77.Font = new Font("Arial", 6, FontStyle.Bold);
            label_64_77.Text = "Impuesto descontable";
            label_64_77.Angle = 90;
            label_64_77.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            label_64_77.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            label_64_77.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            label_64_77.SendToBack();
            formularioDetail.Controls.Add(label_64_77);

            table_61_63.SendToBack();
            table_64_77.SendToBack();
            table_78_85.SendToBack();
            #endregion

            #region Table 2
            XRPanel Panel_86_94 = new XRPanel();
            Panel_86_94.LocationF = new PointF(frame_61_94.LocationF.X + labelShift_thick, frame_61_94.LocationF.Y + labelShift_thick + 25 * rowHeight);
            Panel_86_94.SizeF = new SizeF(frame_61_94.WidthF - 2 * labelShift_thick, 9 * rowHeight);
            Panel_86_94.AnchorVertical = VerticalAnchorStyles.Top;
            Panel_86_94.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            Panel_86_94.BorderWidth = 1;
            Panel_86_94.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            Panel_86_94.BackColor = Color.Transparent;
            Panel_86_94.BringToFront();
            formularioDetail.Controls.Add(Panel_86_94);

            #region Sub-table 1
            XRTable table_86_88 = new XRTable();
            XRTableRow table_86_88_Row;
            XRTableCell table_86_88_Cell;
            table_86_88.LocationF = Panel_86_94.LocationF;
            table_86_88.SizeF = new SizeF(Panel_86_94.WidthF, 3 * rowHeight);
            table_86_88.StyleName = "tableStyle";
            table_86_88.BeginInit();

            for (int i = 86; i < 89; i++)
            {
                table_86_88_Row = new XRTableRow();
                table_86_88_Row.HeightF = rowHeight;
                table_86_88_Row.BackColor = (i % 2 != 0) ? evenRowColor : oddRowColor;

                table_86_88_Cell = new XRTableCell();
                table_86_88_Cell.WidthF = (table_86_88.WidthF - 20) / 2;
                switch (i)
                {
                    case 86:
                        table_86_88_Cell.Text = "Por diferencia de tarifa en este período si";//label
                        table_86_88_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
                        table_86_88_Cell.CanGrow = false;
                        table_86_88_Cell.Font = new Font("Arial", 5);
                        table_86_88_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;
                    case 87:
                        table_86_88_Cell.Text = "Por diferencia de tarifa en acumulado en períodos";//label
                        table_86_88_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
                        table_86_88_Cell.CanGrow = false;
                        table_86_88_Cell.Font = new Font("Arial Narrow", 5);
                        table_86_88_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;
                    case 88:
                        table_86_88_Cell.Text = "Por diferencia de tarifa susceptible de ser";//label
                        table_86_88_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
                        table_86_88_Cell.CanGrow = false;
                        table_86_88_Row.Font = new Font("Arial", 6, FontStyle.Bold);
                        table_86_88_Cell.Font = new Font("Arial", 5, FontStyle.Bold);
                        table_86_88_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;                
                };
                table_86_88_Row.Cells.Add(table_86_88_Cell);

                table_86_88_Cell = new XRTableCell()
                {
                    WidthF = 20,
                    Text = i.ToString(),
                    Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right,
                    BorderColor = Color.FromArgb(0xDB, 0x53, 0x53),
                };
                table_86_88_Row.Cells.Add(table_86_88_Cell);

                table_86_88_Cell = new XRTableCell();
                table_86_88_Cell.WidthF = (table_86_88.WidthF - 20) / 2;
                table_86_88_Cell.CanGrow = false;
                foreach (DTO_FormDecDetail item in formData.FormDecDetail)
                {
                    if (Convert.ToInt32(item.Renglon.Trim()) == i)
                        table_86_88_Cell.Text = (Math.Round(item.ValorML, 0)).ToString("#,0.");
                };
                table_86_88_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                table_86_88_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0);
                table_86_88_Cell.Font = new System.Drawing.Font("Courier new", 7);
                table_86_88_Row.Cells.Add(table_86_88_Cell);
                table_86_88.Rows.Add(table_86_88_Row);
            };
            table_86_88.EndInit();
            formularioDetail.Controls.Add(table_86_88);

            XRLabel label_86 = new XRLabel();
            label_86.LocationF = new PointF(table_86_88.LocationF.X, table_86_88.LocationF.Y + 0 * rowHeight - 1);
            label_86.SizeF = new SizeF((table_86_88.WidthF - 20) / 2, 25);
            label_86.ForeColor = Color.Black;
            label_86.BackColor = Color.Transparent;
            label_86.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_86.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_86.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
            label_86.Multiline = true;
            label_86.Text = "\r\npresenta saldo a favor";
            formularioDetail.Controls.Add(label_86);

            XRLabel label_87 = new XRLabel();
            label_87.LocationF = new PointF(table_86_88.LocationF.X, table_86_88.LocationF.Y + 1 * rowHeight - 1);
            label_87.SizeF = new SizeF((table_86_88.WidthF - 20) / 2, 25);
            label_87.ForeColor = Color.Black;
            label_87.BackColor = Color.Transparent;
            label_87.Font = new System.Drawing.Font("Arial Narrow", 5, FontStyle.Regular);
            label_87.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_87.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
            label_87.Multiline = true;
            label_87.Text = "\r\nanteriores pendiente de aplicar";
            formularioDetail.Controls.Add(label_87);

            XRLabel label_88 = new XRLabel();
            label_88.LocationF = new PointF(table_86_88.LocationF.X, table_86_88.LocationF.Y + 2 * rowHeight - 1);
            label_88.SizeF = new SizeF((table_86_88.WidthF - 20) / 2, 25);
            label_88.ForeColor = Color.Black;
            label_88.BackColor = Color.Transparent;
            label_88.Font = new System.Drawing.Font("Arial", 5, FontStyle.Bold);
            label_88.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_88.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
            label_88.Multiline = true;
            label_88.Text = "\r\naplicado al siguiente período";
            formularioDetail.Controls.Add(label_88);
            #endregion
            #region Sub-table 2
            XRPanel Panel_89_91 = new XRPanel();
            Panel_89_91.LocationF = new PointF(frame_61_94.LocationF.X + labelShift_thick + 14, frame_61_94.LocationF.Y + labelShift_thick + 28 * rowHeight);
            Panel_89_91.SizeF = new SizeF(frame_61_94.WidthF - 2 * labelShift_thick - 14, 3 * rowHeight);
            Panel_89_91.AnchorVertical = VerticalAnchorStyles.Top;
            Panel_89_91.Borders = DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom;
            Panel_89_91.BorderWidth = 1;
            Panel_89_91.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            Panel_89_91.BackColor = Color.Transparent;
            Panel_89_91.BringToFront();
            formularioDetail.Controls.Add(Panel_89_91);

            XRTable table_89_91 = new XRTable();
            XRTableRow table_89_91_Row;
            XRTableCell table_89_91_Cell;
            table_89_91.LocationF = new PointF(Panel_89_91.LocationF.X - 14, Panel_89_91.LocationF.Y);
            table_89_91.SizeF = new SizeF(Panel_89_91.WidthF + 14, Panel_89_91.HeightF);
            table_89_91.StyleName = "tableStyle";
            table_89_91.BeginInit();

            for (int i = 89; i < 92; i++)
            {
                table_89_91_Row = new XRTableRow();
                table_89_91_Row.HeightF = rowHeight;
                table_89_91_Row.BackColor = (i % 2 != 0) ? evenRowColor : oddRowColor;

                table_89_91_Cell = new XRTableCell();
                table_89_91_Cell.WidthF = (table_89_91.WidthF - 20) / 2;
                switch (i)
                {
                    case 89:
                        table_89_91_Cell.Text = "Que le practicaron en este período no aplicada";//label
                        table_89_91_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
                        table_89_91_Cell.Font = new Font("Arial", 5);
                        break;
                    case 90:
                        table_89_91_Cell.Text = "Que le practicaron en períodos anteriores, no";//label
                        table_89_91_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
                        table_89_91_Cell.CanGrow = false;
                        table_89_91_Cell.Font = new Font("Arial", 5);
                        table_89_91_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;
                    case 91:
                        table_89_91_Cell.Text = "Susceptible de ser aplicadaal siguiente";//label
                        table_89_91_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
                        table_89_91_Cell.CanGrow = false;
                        table_89_91_Row.Font = new Font("Arial", 6, FontStyle.Bold);
                        table_89_91_Cell.Font = new Font("Arial", 5, FontStyle.Bold);
                        table_89_91_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;
                };
                table_89_91_Row.Cells.Add(table_89_91_Cell);

                table_89_91_Cell = new XRTableCell()
                {
                    WidthF = 20,
                    Text = i.ToString(),
                    Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right,
                    BorderColor = Color.FromArgb(0xDB, 0x53, 0x53),
                };
                table_89_91_Row.Cells.Add(table_89_91_Cell);

                table_89_91_Cell = new XRTableCell();
                table_89_91_Cell.WidthF = (table_89_91.WidthF - 20) / 2;
                table_89_91_Cell.CanGrow = false;
                foreach (DTO_FormDecDetail item in formData.FormDecDetail)
                {
                    if (Convert.ToInt32(item.Renglon.Trim()) == i)
                        table_89_91_Cell.Text = (Math.Round(item.ValorML, 0)).ToString("#,0.");
                };
                table_89_91_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                table_89_91_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0);
                table_89_91_Cell.Font = new System.Drawing.Font("Courier new", 7);
                table_89_91_Row.Cells.Add(table_89_91_Cell);
                table_89_91.Rows.Add(table_89_91_Row);
            };
            table_89_91.EndInit();
            formularioDetail.Controls.Add(table_89_91);

            XRLabel label_90 = new XRLabel();
            label_90.LocationF = new PointF(table_89_91.LocationF.X, table_89_91.LocationF.Y + 1 * rowHeight - 1);
            label_90.SizeF = new SizeF((table_89_91.WidthF - 20) / 2, 25);
            label_90.ForeColor = Color.Black;
            label_90.BackColor = Color.Transparent;
            label_90.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_90.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_90.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
            label_90.Multiline = true;
            label_90.Text = "\r\naplicada";
            formularioDetail.Controls.Add(label_90);

            XRLabel label_91 = new XRLabel();
            label_91.LocationF = new PointF(table_89_91.LocationF.X, table_89_91.LocationF.Y + 2 * rowHeight - 1);
            label_91.SizeF = new SizeF((table_89_91.WidthF - 20) / 2, 25);
            label_91.ForeColor = Color.Black;
            label_91.BackColor = Color.Transparent;
            label_91.Font = new System.Drawing.Font("Arial", 5, FontStyle.Bold);
            label_91.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_91.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
            label_91.Multiline = true;
            label_91.Text = "\r\nperíodo";
            formularioDetail.Controls.Add(label_91);
            #endregion
            #region Sub-table 3
            XRTable table_92_94 = new XRTable();
            XRTableRow table_92_94_Row;
            XRTableCell table_92_94_Cell;
            table_92_94.LocationF = new PointF(frame_61_94.LocationF.X + labelShift_thick, frame_61_94.LocationF.Y + labelShift_thick + 31 * rowHeight);
            table_92_94.SizeF = new SizeF(frame_61_94.WidthF - 2 * labelShift_thick, 3 * rowHeight);
            table_92_94.StyleName = "tableStyle";
            table_92_94.BeginInit();

            for (int i = 92; i < 95; i++)
            {
                table_92_94_Row = new XRTableRow();
                table_92_94_Row.HeightF = rowHeight;
                table_92_94_Row.BackColor = (i % 2 != 0) ? evenRowColor : oddRowColor;

                table_92_94_Cell = new XRTableCell();
                table_92_94_Cell.WidthF = (table_92_94.WidthF - 20) / 2;
                switch (i)
                {
                    case 92:
                        table_92_94_Cell.Text = "Descontable por ventas del período";
                        table_92_94_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 0, 0, 0);
                        break;
                    case 93:
                        table_92_94_Cell.Text = "Descontable períodos anteriores sobre las ventas";//label
                        table_92_94_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 0, 0, 0);
                        table_92_94_Cell.CanGrow = false;
                        table_92_94_Cell.Font = new Font("Arial", 5);
                        table_92_94_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;
                    case 94:
                        table_92_94_Cell.Text = "Exceso impuesto descontable no susceptible de";//label
                        table_92_94_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 0, 0, 0);
                        table_92_94_Cell.CanGrow = false;
                        table_92_94_Row.Font = new Font("Arial", 6, FontStyle.Bold);
                        table_92_94_Cell.Font = new Font("Arial", 5, FontStyle.Bold);
                        table_92_94_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        table_92_94_Row.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                        table_92_94_Row.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
                        break;       
                };
                table_92_94_Row.Cells.Add(table_92_94_Cell);

                table_92_94_Cell = new XRTableCell()
                {
                    WidthF = 20,
                    Text = i.ToString(),
                    Borders = (i==94)? DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Top :
                         DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right,
                    BorderColor = Color.FromArgb(0xDB, 0x53, 0x53),
                };
                table_92_94_Row.Cells.Add(table_92_94_Cell);

                table_92_94_Cell = new XRTableCell();
                table_92_94_Cell.WidthF = (table_92_94.WidthF - 20) / 2;
                table_92_94_Cell.CanGrow = false;
                foreach (DTO_FormDecDetail item in formData.FormDecDetail)
                {
                    if (Convert.ToInt32(item.Renglon.Trim()) == i)
                        table_92_94_Cell.Text = (Math.Round(item.ValorML, 0)).ToString("#,0.");
                };
                table_92_94_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                table_92_94_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0);
                table_92_94_Cell.Font = new System.Drawing.Font("Courier new", 7);
                table_92_94_Row.Cells.Add(table_92_94_Cell);
                table_92_94.Rows.Add(table_92_94_Row);
            };
            table_92_94.EndInit();
            formularioDetail.Controls.Add(table_92_94);

            XRLabel label_93 = new XRLabel();
            label_93.LocationF = new PointF(table_92_94.LocationF.X, table_92_94.LocationF.Y + 1 * rowHeight - 1);
            label_93.SizeF = new SizeF((table_92_94.WidthF - 20) / 2, 25);
            label_93.ForeColor = Color.Black;
            label_93.BackColor = Color.Transparent;
            label_93.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_93.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_93.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 0, 0, 0);
            label_93.Multiline = true;
            label_93.Text = "\r\ndel período";
            formularioDetail.Controls.Add(label_93);

            XRLabel label_94 = new XRLabel();
            label_94.LocationF = new PointF(table_92_94.LocationF.X, table_92_94.LocationF.Y + 2 * rowHeight - 1);
            label_94.SizeF = new SizeF((table_92_94.WidthF - 20) / 2, 25);
            label_94.ForeColor = Color.Black;
            label_94.BackColor = Color.Transparent;
            label_94.Font = new System.Drawing.Font("Arial", 5, FontStyle.Bold);
            label_94.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_94.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 0, 0, 0);
            label_94.Multiline = true;
            label_94.Text = "\r\nsolisitarse en devolución y/o Compensación";
            formularioDetail.Controls.Add(label_94);
            #endregion

            XRLabel label_86_94 = new XRLabel();
            label_86_94.LocationF = Panel_86_94.LocationF;
            label_86_94.SizeF = new SizeF(14, 9 * rowHeight);
            label_86_94.CanGrow = false;
            label_86_94.ForeColor = Color.Black;
            label_86_94.BackColor = Color.White;
            label_86_94.Font = new Font("Arial Narrow", 6, FontStyle.Bold);
            label_86_94.Text = "Control de saldos";
            label_86_94.Angle = 90;
            label_86_94.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            label_86_94.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            label_86_94.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            formularioDetail.Controls.Add(label_86_94);
            label_86_94.SendToBack();

            XRLabel label_86_88 = new XRLabel();
            label_86_88.LocationF = new PointF(label_86_94.LocationF.X + label_86_94.WidthF, label_86_94.LocationF.Y);
            label_86_88.SizeF = new SizeF(14, 3 * rowHeight);
            label_86_88.CanGrow = false;
            label_86_88.ForeColor = Color.Black;
            label_86_88.BackColor = Color.White;
            label_86_88.Font = new Font("Arial", 5, FontStyle.Bold);
            label_86_88.Text = "Exceso";
            label_86_88.Angle = 90;
            label_86_88.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            label_86_88.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            label_86_88.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            label_86_88.SendToBack();
            formularioDetail.Controls.Add(label_86_88);

            XRLabel label_86_88_add = new XRLabel();
            label_86_88_add.LocationF = new PointF(label_86_88.LocationF.X - 2, label_86_88.LocationF.Y);
            label_86_88_add.SizeF = new SizeF(20, 3 * rowHeight);
            label_86_88_add.ForeColor = Color.Black;
            label_86_88_add.BackColor = Color.Transparent;
            label_86_88_add.Font = new Font("Arial", 5, FontStyle.Bold);
            label_86_88_add.Multiline = true;
            label_86_88_add.Text = "\r\ndescontable";
            label_86_88_add.Angle = 90;
            label_86_88_add.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            formularioDetail.Controls.Add(label_86_88_add);
            label_86_88_add.BringToFront();
            
            XRLabel label_88_91 = new XRLabel();
            label_88_91.LocationF = new PointF(label_86_94.LocationF.X + label_86_94.WidthF, label_86_88.LocationF.Y + label_86_88.HeightF);
            label_88_91.SizeF = new SizeF(14, 3 * rowHeight);
            label_88_91.CanGrow = false;
            label_88_91.ForeColor = Color.Black;
            label_88_91.BackColor = Color.White;
            label_88_91.Font = new Font("Arial Narrow", 5, FontStyle.Bold);
            label_88_91.Text = "Retencion";
            label_88_91.Angle = 90;
            label_88_91.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            label_88_91.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            label_88_91.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            formularioDetail.Controls.Add(label_88_91);
            label_88_91.SendToBack();

            XRLabel label_88_91_add = new XRLabel();
            label_88_91_add.LocationF = new PointF(label_88_91.LocationF.X - 2, label_88_91.LocationF.Y);
            label_88_91_add.SizeF = new SizeF(20, 3 * rowHeight);
            label_88_91_add.CanGrow = false;
            label_88_91_add.ForeColor = Color.Black;
            label_88_91_add.BackColor = Color.Transparent;
            label_88_91_add.Font = new Font("Arial", 5, FontStyle.Bold);
            label_88_91_add.Multiline = true;
            label_88_91_add.Text = "\r\nIVA";
            label_88_91_add.Angle = 90;
            label_88_91_add.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            formularioDetail.Controls.Add(label_88_91_add);
            label_88_91_add.BringToFront();

            table_86_88.SendToBack();
            table_89_91.SendToBack();
            table_92_94.SendToBack();
            #endregion
            #endregion
            #endregion

            #region Formulario part 5
            XRShape infoFrame = new XRShape();
            infoFrame.LocationF = new PointF(0, frame_27_60.LocationF.Y + frame_27_60.HeightF - frameShift);
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
            XRShape frame_69_80 = new XRShape();
            frame_69_80.LocationF = new PointF(0, infoFrame.LocationF.Y + infoFrame.HeightF - frameShift);
            frame_69_80.SizeF = new SizeF(formularioWidth, 4 * rowHeight + 2 * labelShift_thick);
            frame_69_80.LineWidth = 2;
            frame_69_80.Borders = DevExpress.XtraPrinting.BorderSide.None;
            frame_69_80.SnapLineMargin = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0);
            frame_69_80.BackColor = Color.Transparent;
            frame_69_80.ForeColor = Color.FromArgb(0xDB, 0x53, 0x53);
            frame_69_80.Shape = new ShapeRectangle()
            {
                Fillet = 10,
            };
            frame_69_80.SendToBack();
            formularioDetail.Controls.Add(frame_69_80);

            XRLabel signatariosLabel = new XRLabel();
            signatariosLabel.LocationF = new PointF(frame_69_80.LocationF.X + labelShift_thick, frame_69_80.LocationF.Y + labelShift_thick);
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
            XRPanel Panel_69_70 = new XRPanel();
            Panel_69_70.LocationF = new PointF(signatariosLabel.LocationF.X + signatariosLabel.WidthF, signatariosLabel.LocationF.Y);
            Panel_69_70.SizeF = new SizeF(frame_69_80.WidthF / 3 - signatariosLabel.WidthF, 2 * rowHeight);
            Panel_69_70.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom;
            Panel_69_70.BorderWidth = 1;
            Panel_69_70.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            Panel_69_70.BackColor = Color.Transparent;
            Panel_69_70.BringToFront();
            formularioDetail.Controls.Add(Panel_69_70);

            XRTable table_69_70 = new XRTable();
            XRTableRow table_69_70_Row;
            XRTableCell table_69_70_Cell;
            table_69_70.LocationF = Panel_69_70.LocationF;
            table_69_70.SizeF = Panel_69_70.SizeF;
            table_69_70.StyleName = "tableStyle";
            table_69_70.BeginInit();
            table_69_70_Row = new XRTableRow();
            table_69_70_Row.HeightF = rowHeight;
            table_69_70_Row.BackColor = evenRowColor;
            table_69_70_Cell = new XRTableCell()
            {
                WidthF = table_69_70.WidthF - 30,
                Text = "69. Número de Identificación Tributaria (NIT)"
            };
            table_69_70_Row.Cells.Add(table_69_70_Cell);
            table_69_70_Cell = new XRTableCell()
            {
                WidthF = 30,
                Text = "70. DV",
                Padding = new DevExpress.XtraPrinting.PaddingInfo(1, 0, 0, 0),
                WordWrap = false,
                CanGrow = false
            };
            table_69_70_Row.Cells.Add(table_69_70_Cell);
            table_69_70.Rows.Add(table_69_70_Row);
            table_69_70_Row = new XRTableRow();
            table_69_70_Row.HeightF = rowHeight;
            table_69_70_Row.BackColor = evenRowColor;
            table_69_70_Cell = new XRTableCell()
            {
                WidthF = table_69_70.WidthF - 30,
                ///////////// Datos
            };
            table_69_70_Row.Cells.Add(table_69_70_Cell);
            table_69_70_Cell = new XRTableCell()
            {
                WidthF = 30,
                Borders = DevExpress.XtraPrinting.BorderSide.Left,
                BorderColor = Color.FromArgb(0xDB, 0x53, 0x53)
                ///////////// Datos
            };
            table_69_70_Row.Cells.Add(table_69_70_Cell);
            table_69_70.Rows.Add(table_69_70_Row);
            table_69_70.EndInit();
            table_69_70.SendToBack();
            formularioDetail.Controls.Add(table_69_70);
            #endregion

            #region Table 2
            XRPanel Panel_71_74 = new XRPanel();
            Panel_71_74.LocationF = new PointF(table_69_70.LocationF.X + table_69_70.WidthF, table_69_70.LocationF.Y);
            Panel_71_74.SizeF = new SizeF(2 * frame_69_80.WidthF / 3 - 2 * labelShift_thick, 2 * rowHeight);
            Panel_71_74.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom;
            Panel_71_74.BorderWidth = 1;
            Panel_71_74.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            Panel_71_74.BackColor = Color.Transparent;
            Panel_71_74.BringToFront();
            formularioDetail.Controls.Add(Panel_71_74);

            XRLabel label_71_74 = new XRLabel();
            label_71_74.LocationF = Panel_71_74.LocationF;
            label_71_74.SizeF = new SizeF(Panel_71_74.WidthF, rowHeight);
            label_71_74.StyleName = "tableStyle";
            label_71_74.Text = "Apellidos y nombres de quien firma como representante del declarante";
            formularioDetail.Controls.Add(label_71_74);

            XRTable table_71_74 = new XRTable();
            XRTableRow table_71_74_Row;
            XRTableCell table_71_74_Cell;
            table_71_74.LocationF = Panel_71_74.LocationF;
            table_71_74.SizeF = Panel_71_74.SizeF;
            table_71_74.StyleName = "tableStyle";
            table_71_74.BeginInit();
            table_71_74_Row = new XRTableRow();
            table_71_74_Row.HeightF = rowHeight;
            table_71_74_Row.BackColor = evenRowColor;
            table_71_74_Cell = new XRTableCell()
            {
                WidthF = table_71_74.WidthF / 4,
                ///////////// Datos
            };
            table_71_74_Row.Cells.Add(table_71_74_Cell);
            table_71_74_Cell = new XRTableCell()
            {
                WidthF = table_71_74.WidthF / 4,
                ///////////// Datos
            };
            table_71_74_Row.Cells.Add(table_71_74_Cell);
            table_71_74_Cell = new XRTableCell()
            {
                WidthF = table_71_74.WidthF / 4,
                ///////////// Datos
            };
            table_71_74_Row.Cells.Add(table_71_74_Cell);
            table_71_74_Cell = new XRTableCell()
            {
                WidthF = table_71_74.WidthF / 4,
                ///////////// Datos
            };
            table_71_74_Row.Cells.Add(table_71_74_Cell);
            table_71_74.Rows.Add(table_71_74_Row);
            table_71_74_Row = new XRTableRow();
            table_71_74_Row.HeightF = rowHeight;
            table_71_74_Row.BackColor = evenRowColor;
            table_71_74_Row.ForeColor = Color.FromArgb(0xDB, 0x53, 0x53);
            table_71_74_Cell = new XRTableCell()
            {
                WidthF = table_71_74.WidthF / 4,
                Text = "71. Primer apellido"
            };
            table_71_74_Row.Cells.Add(table_71_74_Cell);
            table_71_74_Cell = new XRTableCell()
            {
                WidthF = table_71_74.WidthF / 4,
                Text = "72. Segundo apellido"
            };
            table_71_74_Row.Cells.Add(table_71_74_Cell);
            table_71_74_Cell = new XRTableCell()
            {
                WidthF = table_71_74.WidthF / 4,
                Text = "73. Primer nombre"
            };
            table_71_74_Row.Cells.Add(table_71_74_Cell);
            table_71_74_Cell = new XRTableCell()
            {
                WidthF = table_71_74.WidthF / 4,
                Text = "74. Otros nombres"
            };
            table_71_74_Row.Cells.Add(table_71_74_Cell);
            table_71_74.Rows.Add(table_71_74_Row);
            table_71_74.EndInit();
            table_71_74.SendToBack();
            formularioDetail.Controls.Add(table_71_74);

            XRTable table_71_74_division = new XRTable();
            XRTableRow table_71_74_division_Row;
            XRTableCell table_71_74_division_Cell;
            table_71_74_division.LocationF = new PointF(table_71_74.LocationF.X, table_71_74.LocationF.Y + 3 * rowHeight / 2);
            table_71_74_division.SizeF = new SizeF(table_71_74.WidthF - table_71_74.WidthF / 4, rowHeight / 2);
            table_71_74_division.BeginInit();
            table_71_74_division_Row = new XRTableRow();
            table_71_74_division_Row.HeightF = rowHeight / 2;
            table_71_74_division_Row.BackColor = evenRowColor;
            for (int i = 0; i < 3; i++)
            {
                table_71_74_division_Cell = new XRTableCell()
                {
                    WidthF = table_71_74.WidthF / 4,
                    Borders = DevExpress.XtraPrinting.BorderSide.Right,
                    BorderColor = Color.FromArgb(0xDB, 0x53, 0x53)
                };
                table_71_74_division_Row.Cells.Add(table_71_74_division_Cell);
            };

            table_71_74_division.Rows.Add(table_71_74_division_Row);
            table_71_74_division.EndInit();
            table_71_74_division.SendToBack();
            formularioDetail.Controls.Add(table_71_74_division);
            #endregion

            #region Table 3
            XRPanel Panel_75_76 = new XRPanel();
            Panel_75_76.LocationF = new PointF(signatariosLabel.LocationF.X + signatariosLabel.WidthF, Panel_69_70.LocationF.Y + Panel_69_70.HeightF);
            Panel_75_76.SizeF = new SizeF(frame_69_80.WidthF / 3 - signatariosLabel.WidthF, 2 * rowHeight);
            Panel_75_76.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            Panel_75_76.BorderWidth = 1;
            Panel_75_76.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            Panel_75_76.BackColor = Color.Transparent;
            Panel_75_76.BringToFront();
            formularioDetail.Controls.Add(Panel_75_76);

            XRTable table_75_76 = new XRTable();
            XRTableRow table_75_76_Row;
            XRTableCell table_75_76_Cell;
            table_75_76.LocationF = Panel_75_76.LocationF;
            table_75_76.SizeF = Panel_75_76.SizeF;
            table_75_76.StyleName = "tableStyle";
            table_75_76.BeginInit();

            table_75_76_Row = new XRTableRow();
            table_75_76_Row.HeightF = rowHeight;
            table_75_76_Row.BackColor = evenRowColor;

            table_75_76_Cell = new XRTableCell()
            {
                WidthF = table_75_76.WidthF - 30,
                Text = "75. Número NIT contrador o revisor fiscal"
            };
            table_75_76_Row.Cells.Add(table_75_76_Cell);
            table_75_76_Cell = new XRTableCell()
            {
                WidthF = 30,
                Text = "76. DV",
                Padding = new DevExpress.XtraPrinting.PaddingInfo(1, 0, 0, 0),
                WordWrap = false,
                CanGrow = false
            };
            table_75_76_Row.Cells.Add(table_75_76_Cell);
            table_75_76.Rows.Add(table_75_76_Row);

            table_75_76_Row = new XRTableRow();
            table_75_76_Row.HeightF = rowHeight;
            table_75_76_Row.BackColor = evenRowColor;
            table_75_76_Cell = new XRTableCell()
            {
                WidthF = table_75_76.WidthF - 30,
                ///////////// Datos
            };
            table_75_76_Row.Cells.Add(table_75_76_Cell);
            table_75_76_Cell = new XRTableCell()
            {
                WidthF = 30,
                Borders = DevExpress.XtraPrinting.BorderSide.Left,
                BorderColor = Color.FromArgb(0xDB, 0x53, 0x53)
                ///////////// Datos
            };
            table_75_76_Row.Cells.Add(table_75_76_Cell);
            table_75_76.Rows.Add(table_75_76_Row);
            table_75_76.EndInit();
            table_75_76.SendToBack();
            formularioDetail.Controls.Add(table_75_76);
            #endregion

            #region Table 4
            XRPanel Panel_77_80 = new XRPanel();
            Panel_77_80.LocationF = new PointF(table_75_76.LocationF.X + table_75_76.WidthF, table_75_76.LocationF.Y);
            Panel_77_80.SizeF = new SizeF(2 * frame_69_80.WidthF / 3 - 2 * labelShift_thick, 2 * rowHeight); ;
            Panel_77_80.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            Panel_77_80.BorderWidth = 1;
            Panel_77_80.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            Panel_77_80.BackColor = Color.Transparent;
            Panel_77_80.BringToFront();
            formularioDetail.Controls.Add(Panel_77_80);

            XRLabel label_77_80 = new XRLabel();
            label_77_80.LocationF = Panel_77_80.LocationF;
            label_77_80.SizeF = new SizeF(Panel_77_80.WidthF, rowHeight);
            label_77_80.StyleName = "tableStyle";
            label_77_80.Text = "Apellidos y nombres del contador o revisor fiscal";
            formularioDetail.Controls.Add(label_77_80);

            XRTable table_77_80 = new XRTable();
            XRTableRow table_77_80_Row;
            XRTableCell table_77_80_Cell;
            table_77_80.LocationF = Panel_77_80.LocationF;
            table_77_80.SizeF = Panel_77_80.SizeF;
            table_77_80.StyleName = "tableStyle";
            table_77_80.BeginInit();

            table_77_80_Row = new XRTableRow();
            table_77_80_Row.HeightF = rowHeight;
            table_77_80_Row.BackColor = evenRowColor;

            table_77_80_Cell = new XRTableCell()
            {
                WidthF = table_77_80.WidthF / 4,
                ///////////// Datos
            };
            table_77_80_Row.Cells.Add(table_77_80_Cell);
            table_77_80_Cell = new XRTableCell()
            {
                WidthF = table_77_80.WidthF / 4,
                ///////////// Datos
            };
            table_77_80_Row.Cells.Add(table_77_80_Cell);
            table_77_80_Cell = new XRTableCell()
            {
                WidthF = table_77_80.WidthF / 4,
                ///////////// Datos
            };
            table_77_80_Row.Cells.Add(table_77_80_Cell);
            table_77_80_Cell = new XRTableCell()
            {
                WidthF = table_77_80.WidthF / 4,
                ///////////// Datos
            };
            table_77_80_Row.Cells.Add(table_77_80_Cell);
            table_77_80.Rows.Add(table_77_80_Row);
            table_77_80_Row = new XRTableRow();
            table_77_80_Row.HeightF = rowHeight;
            table_77_80_Row.BackColor = evenRowColor;
            table_77_80_Row.ForeColor = Color.FromArgb(0xDB, 0x53, 0x53);
            table_77_80_Cell = new XRTableCell()
            {
                WidthF = table_77_80.WidthF / 4,
                Text = "77. Primer apellido"
            };
            table_77_80_Row.Cells.Add(table_77_80_Cell);
            table_77_80_Cell = new XRTableCell()
            {
                WidthF = table_77_80.WidthF / 4,
                Text = "78. Segundo apellido"
            };
            table_77_80_Row.Cells.Add(table_77_80_Cell);
            table_77_80_Cell = new XRTableCell()
            {
                WidthF = table_77_80.WidthF / 4,
                Text = "79. Primer nombre"
            };
            table_77_80_Row.Cells.Add(table_77_80_Cell);
            table_77_80_Cell = new XRTableCell()
            {
                WidthF = table_77_80.WidthF / 4,
                Text = "80. Otros nombres"
            };
            table_77_80_Row.Cells.Add(table_77_80_Cell);
            table_77_80.Rows.Add(table_77_80_Row);
            table_77_80.EndInit();
            table_77_80.SendToBack();
            formularioDetail.Controls.Add(table_77_80);

            XRTable table_77_80_division = new XRTable();
            XRTableRow table_77_80_division_Row;
            XRTableCell table_77_80_division_Cell;
            table_77_80_division.LocationF = new PointF(table_77_80.LocationF.X, table_77_80.LocationF.Y + 3 * rowHeight / 2);
            table_77_80_division.SizeF = new SizeF(table_77_80.WidthF - table_77_80.WidthF / 4, rowHeight / 2);
            table_77_80_division.BeginInit();
            table_77_80_division_Row = new XRTableRow();
            table_77_80_division_Row.HeightF = rowHeight / 2;
            table_77_80_division_Row.BackColor = evenRowColor;
            for (int i = 0; i < 3; i++)
            {
                table_77_80_division_Cell = new XRTableCell()
                {
                    WidthF = table_77_80.WidthF / 4,
                    Borders = DevExpress.XtraPrinting.BorderSide.Right,
                    BorderColor = Color.FromArgb(0xDB, 0x53, 0x53)
                };
                table_77_80_division_Row.Cells.Add(table_77_80_division_Cell);
            };

            table_77_80_division.Rows.Add(table_77_80_division_Row);
            table_77_80_division.EndInit();
            table_77_80_division.SendToBack();
            formularioDetail.Controls.Add(table_77_80_division);
            #endregion

            #endregion
        }
        
        #endregion
    }
}
