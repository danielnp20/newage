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
    public partial class FormularioRenta : DevExpress.XtraReports.UI.XtraReport
    {
        #region Variables
        BaseController _bc = BaseController.GetInstance(); 
        #endregion

        #region Funciones Publicas
        /// <summary>
        /// Formulario "Declaracion de Renta y Complementarios..." Constructor
        /// </summary>
        /// <param name="formData">Data for the Formulario</param>
        /// <param name="Date">Period of the Formulario</param>
        public FormularioRenta(DTO_Formularios formData, int yearFisc, int period, bool preInd)
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
            titleLabel.Multiline = true;
            titleLabel.Text = "Declaración de Renta y Complementarios o de Ingresos\r\ny Patrimonio para Personas Jurídicas y Asimiladas,\r\nPersonas Naturales y Asimiladas Oblagadas a llevar Contabilidad";
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
            numeroLabel.Text = "110";
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
            XRShape frame_5_27 = new XRShape();
            frame_5_27.LocationF = new PointF(0, frame_1_4.LocationF.Y + frame_1_4.HeightF - frameShift);
            frame_5_27.SizeF = new SizeF(formularioWidth, 94);
            frame_5_27.LineWidth = 2;
            frame_5_27.BackColor = Color.Transparent;
            frame_5_27.ForeColor = Color.FromArgb(0xDB, 0x53, 0x53);
            frame_5_27.Shape = new ShapeRectangle()
            {
                Fillet = 10,
            };
            //datosDelDeclaranteFrame.SendToBack();
            frame_5_27.AnchorVertical = VerticalAnchorStyles.Top;
            formularioDetail.Controls.Add(frame_5_27);

            XRLabel datosLabel = new XRLabel();
            datosLabel.LocationF = frame_5_27.LocationF;
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
            Panel_5_6.SizeF = new SizeF(frame_5_27.WidthF / 3 - datosLabel.WidthF, 2 * rowHeight);
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
            Panel_7_10.SizeF = new SizeF(2 * frame_5_27.WidthF / 3 - labelShift_thick, 2 * rowHeight);
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
            Panel_11.SizeF = new SizeF(9 * frame_5_27.WidthF / 10 - datosLabel.WidthF, 2 * rowHeight);
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
            Panel_12.SizeF = new SizeF(frame_5_27.WidthF / 10 - labelShift_thick, 2 * rowHeight);
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
            Panel_24.SizeF = new SizeF(frame_5_27.WidthF - datosLabel.WidthF - labelShift_thick, rowHeight);
            Panel_24.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            Panel_24.BorderWidth = 1;
            Panel_24.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            Panel_24.SnapLineMargin = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0);
            Panel_24.BackColor = Color.Transparent;
            Panel_24.BringToFront();
            formularioDetail.Controls.Add(Panel_24);

            XRLabel label_24 = new XRLabel();
            label_24.LocationF = Panel_24.LocationF;
            label_24.SizeF = new SizeF(110, rowHeight);
            //label_24.ForeColor = Color.Black;
            //label_24.BackColor = Color.Transparent;
            //label_24.Font = new Font("Arial", 6, FontStyle.Regular);
            //label_24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            label_24.StyleName = "tableStyle";
            label_24.Text = "24. Actividad Económica";
            formularioDetail.Controls.Add(label_24);

            XRTable table_24 = new XRTable();
            XRTableRow table_24_Row;
            XRTableCell table_24_Cell;
            table_24.LocationF = new PointF(label_24.LocationF.X + label_24.WidthF, label_24.LocationF.Y);
            table_24.HeightF = rowHeight;
            table_24.WidthF = 4 * table_24.HeightF;
            table_24.ForeColor = Color.Black;
            table_24.BackColor = Color.White;
            table_24.Borders = DevExpress.XtraPrinting.BorderSide.All;
            table_24.BringToFront();
            table_24_Row = new XRTableRow();
            for (int i = 0; i < 4; i++)
            {
                table_24_Cell = new XRTableCell();
                table_24_Row.Cells.Add(table_24_Cell);
            }
            table_24.Rows.Add(table_24_Row);
            formularioDetail.Controls.Add(table_24);

            XRLabel label_25 = new XRLabel();
            label_25.LocationF = new PointF(table_24.LocationF.X + table_24.WidthF + 120, table_24.LocationF.Y);
            label_25.SizeF = new SizeF(170, rowHeight);
            //label_24.ForeColor = Color.Black;
            //label_24.BackColor = Color.Transparent;
            //label_24.Font = new Font("Arial", 6, FontStyle.Regular);
            //label_24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            label_25.StyleName = "tableStyle";
            label_25.Text = "25. Si es gran contribuyente, marque \"X\"";
            formularioDetail.Controls.Add(label_25);

            XRLabel label_25_blank = new XRLabel();
            label_25_blank.LocationF = new PointF(label_25.LocationF.X + label_25.WidthF, label_25.LocationF.Y + 1);
            label_25_blank.SizeF = new SizeF(14, label_25.HeightF - 2);
            label_25_blank.BackColor = Color.White;
            label_25_blank.Borders = DevExpress.XtraPrinting.BorderSide.All;
            label_25_blank.BringToFront();
            formularioDetail.Controls.Add(label_25_blank);

            #endregion

            #region Table 6
            XRPanel Panel_26_27 = new XRPanel();
            Panel_26_27.LocationF = new PointF(frame_5_27.LocationF.X + labelShift_thick, Panel_24.LocationF.Y + Panel_24.HeightF);
            Panel_26_27.SizeF = new SizeF(2 * frame_5_27.WidthF / 3, rowHeight);
            Panel_26_27.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            Panel_26_27.BorderWidth = 1;
            Panel_26_27.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            Panel_26_27.BackColor = Color.Transparent;
            Panel_26_27.BringToFront();
            formularioDetail.Controls.Add(Panel_26_27);

            XRTable table_26_27 = new XRTable();
            XRTableRow table_26_27_Row;
            XRTableCell table_26_27_Cell;
            table_26_27.LocationF = Panel_26_27.LocationF;
            table_26_27.SizeF = Panel_26_27.SizeF;
            table_26_27.StyleName = "tableStyle";
            table_26_27.BeginInit();
            table_26_27_Row = new XRTableRow();
            table_26_27_Row.HeightF = rowHeight;
            table_26_27_Row.BackColor = evenRowColor;

            table_26_27_Cell = new XRTableCell()
            {
                WidthF = 180,
                Text = "Si es una corrección indique:",
                Font = new Font("Arial", 6, FontStyle.Bold)
            };
            table_26_27_Row.Cells.Add(table_26_27_Cell);

            table_26_27_Cell = new XRTableCell()
            {
                WidthF = 50,
                Text = "26. Cód."
            };
            table_26_27_Row.Cells.Add(table_26_27_Cell);

            table_26_27_Cell = new XRTableCell()
            {
                WidthF = 70,
                ///////////// Datos
            };
            table_26_27_Row.Cells.Add(table_26_27_Cell);

            table_26_27_Cell = new XRTableCell()
            {
                WidthF = 180,
                Text = "27. No. Formulario anterior"
            };
            table_26_27_Row.Cells.Add(table_26_27_Cell);

            table_26_27_Cell = new XRTableCell()
            {
                WidthF = table_26_27.WidthF - 300,
                ///////////// Datos
            };
            table_26_27_Row.Cells.Add(table_26_27_Cell);

            table_26_27.Rows.Add(table_26_27_Row);
            table_26_27.EndInit();
            table_26_27.SendToBack();
            formularioDetail.Controls.Add(table_26_27);

            XRPanel Panel_26_27_blank = new XRPanel();
            Panel_26_27_blank.LocationF = new PointF(Panel_26_27.LocationF.X + Panel_26_27.WidthF, Panel_26_27.LocationF.Y);
            Panel_26_27_blank.SizeF = new SizeF(frame_5_27.WidthF - Panel_26_27.WidthF - 2 * labelShift_thick, rowHeight);
            Panel_26_27_blank.Borders = DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Left;
            Panel_26_27_blank.BorderWidth = 1;
            Panel_26_27_blank.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            Panel_26_27_blank.BackColor = Color.FromArgb(0xE7, 0xAB, 0xAB);
            formularioDetail.Controls.Add(Panel_26_27_blank);
            #endregion
            #endregion

            #region Formulario part 4
            #region Left part
            XRShape frame_28_29 = new XRShape();
            frame_28_29.LocationF = new PointF(0, frame_5_27.LocationF.Y + frame_5_27.HeightF - frameShift);
            frame_28_29.SizeF = new SizeF(formularioWidth / 2, 2 * rowHeight + 2 * labelShift_thick);
            frame_28_29.CanGrow = false;
            frame_28_29.LineWidth = 2;
            frame_28_29.BackColor = Color.Transparent;
            frame_28_29.ForeColor = Color.FromArgb(0xDB, 0x53, 0x53);
            frame_28_29.Shape = new ShapeRectangle()
            {
                Fillet = 20,
            };
            frame_28_29.CanGrow = false;
            frame_28_29.AnchorVertical = VerticalAnchorStyles.Top;
            formularioDetail.Controls.Add(frame_28_29);

            #region Check list
            XRLabel label_28 = new XRLabel();
            label_28.LocationF = new PointF(frame_28_29.LocationF.X + labelShift_thick, frame_28_29.LocationF.Y + labelShift_thick);
            label_28.SizeF = new SizeF(210, rowHeight);
            label_28.StyleName = "tableStyle";
            label_28.Text = "28. Fracción año gravable 2012 (Marque \"X\")";
            formularioDetail.Controls.Add(label_28);

            XRLabel label_28_blank = new XRLabel();
            label_28_blank.LocationF = new PointF(label_28.LocationF.X + label_28.WidthF, label_28.LocationF.Y + 1);
            label_28_blank.SizeF = new SizeF(14, label_28.HeightF - 2);
            label_28_blank.BackColor = Color.White;
            label_28_blank.Borders = DevExpress.XtraPrinting.BorderSide.All;
            label_28_blank.BringToFront();
            formularioDetail.Controls.Add(label_28_blank);

            XRLabel label_29 = new XRLabel();
            label_29.LocationF = new PointF(label_28.LocationF.X, label_28.LocationF.Y + label_28.HeightF);
            label_29.SizeF = new SizeF(210, rowHeight);
            label_29.StyleName = "tableStyle";
            label_29.Text = "29. Cambio titular inversión extranjera (Marque \"X\")";
            formularioDetail.Controls.Add(label_29);

            XRLabel label_29_blank = new XRLabel();
            label_29_blank.LocationF = new PointF(label_29.LocationF.X + label_29.WidthF, label_29.LocationF.Y + 1);
            label_29_blank.SizeF = new SizeF(14, label_29.HeightF - 2);
            label_29_blank.BackColor = Color.White;
            label_29_blank.Borders = DevExpress.XtraPrinting.BorderSide.All;
            label_29_blank.BringToFront();
            formularioDetail.Controls.Add(label_29_blank);
            #endregion

            XRShape frame_30_56 = new XRShape();
            frame_30_56.LocationF = new PointF(0, frame_28_29.LocationF.Y + frame_28_29.HeightF - frameShift);
            frame_30_56.SizeF = new SizeF(formularioWidth / 2, 29 * rowHeight + 2 * labelShift_thick);
            frame_30_56.CanGrow = false;
            frame_30_56.LineWidth = 2;
            frame_30_56.BackColor = Color.Transparent;
            frame_30_56.ForeColor = Color.FromArgb(0xDB, 0x53, 0x53);
            frame_30_56.Shape = new ShapeRectangle()
            {
                Fillet = 3,
            };
            frame_30_56.CanGrow = false;
            frame_30_56.AnchorVertical = VerticalAnchorStyles.Top;
            formularioDetail.Controls.Add(frame_30_56);

            #region Table 1
            XRTable table_30_32 = new XRTable();
            XRTableRow table_30_32_Row;
            XRTableCell table_30_32_Cell;
            table_30_32.AnchorVertical = VerticalAnchorStyles.Top;
            table_30_32.LocationF = new PointF(frame_30_56.LocationF.X + labelShift_thick, frame_30_56.LocationF.Y + labelShift_thick);
            table_30_32.SizeF = new SizeF(frame_30_56.WidthF - 2 * labelShift_thick, 3 * rowHeight);
            table_30_32.StyleName = "tableStyle";
            table_30_32.BeginInit();

            for (int i = 30; i < 33; i++)
            {
                table_30_32_Row = new XRTableRow();
                table_30_32_Row.HeightF = rowHeight;
                table_30_32_Row.BackColor = (i % 2 == 0) ? oddRowColor : evenRowColor;

                table_30_32_Cell = new XRTableCell();
                table_30_32_Cell.WidthF = (table_30_32.WidthF - 20) / 2;
                table_30_32_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(18, 0, 0, 0);
                switch (i)
                {
                    case 30:
                        table_30_32_Cell.Text = "Total costos y gastos de nómina";
                        break;
                    case 31:
                        table_30_32_Cell.Text = "Aportes al sistema de seguridad social";
                        break;
                    case 32:
                        table_30_32_Cell.Text = "Aportes al SENA, ICBF, cajas de compensación";
                        table_30_32_Cell.CanGrow = false;
                        table_30_32_Cell.Font = new Font("Arrial", 5);
                        break;
                };
                table_30_32_Row.Cells.Add(table_30_32_Cell);

                table_30_32_Cell = new XRTableCell()
                {
                    WidthF = 20,
                    Text = i.ToString(),
                    Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right,
                    BorderColor = Color.FromArgb(0xDB, 0x53, 0x53),
                };
                table_30_32_Row.Cells.Add(table_30_32_Cell);

                table_30_32_Cell = new XRTableCell();
                table_30_32_Cell.WidthF = (table_30_32.WidthF - 20) / 2;
                table_30_32_Cell.CanGrow = false;
                foreach (DTO_FormDecDetail item in formData.FormDecDetail)
                {
                    if (Convert.ToInt32(item.Renglon.Trim()) == i)
                        table_30_32_Cell.Text = (Math.Round(item.ValorML, 0)).ToString("#,0.");
                };
                table_30_32_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                table_30_32_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0);
                table_30_32_Cell.Font = new System.Drawing.Font("Courier new", 7);
                table_30_32_Row.Cells.Add(table_30_32_Cell);
                table_30_32.Rows.Add(table_30_32_Row);
            };
            table_30_32.EndInit();
            formularioDetail.Controls.Add(table_30_32);

            XRLabel label_30_32 = new XRLabel();
            label_30_32.LocationF = table_30_32.LocationF;
            label_30_32.SizeF = new SizeF(17, 3 * rowHeight);
            label_30_32.CanGrow = false;
            label_30_32.ForeColor = Color.Black;
            label_30_32.BackColor = Color.White;
            label_30_32.Font = new Font("Arial Narrow", 6, FontStyle.Bold);
            label_30_32.Text = "Datos"; //informativos";
            label_30_32.Angle = 90;
            label_30_32.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            label_30_32.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            label_30_32.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            formularioDetail.Controls.Add(label_30_32);

            XRLabel label_30_32_rightPart = new XRLabel();
            label_30_32_rightPart.LocationF = label_30_32.LocationF;
            label_30_32_rightPart.SizeF = label_30_32.SizeF;
            label_30_32_rightPart.CanGrow = false;
            label_30_32_rightPart.ForeColor = Color.Black;
            label_30_32_rightPart.BackColor = Color.Transparent;
            label_30_32_rightPart.Font = new Font("Arial Narrow", 6, FontStyle.Bold);
            label_30_32_rightPart.Text = "informativos";
            label_30_32_rightPart.Angle = 90;
            label_30_32_rightPart.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            label_30_32_rightPart.Borders = DevExpress.XtraPrinting.BorderSide.None;
            label_30_32_rightPart.BringToFront();
            formularioDetail.Controls.Add(label_30_32_rightPart);

            label_30_32.SendToBack();

            table_30_32.SendToBack();
            #endregion

            #region Table 2
            XRPanel Panel_33_41 = new XRPanel();
            Panel_33_41.LocationF = new PointF(frame_30_56.LocationF.X + labelShift_thick, frame_30_56.LocationF.Y + labelShift_thick + 3 * rowHeight);
            Panel_33_41.SizeF = new SizeF(frame_30_56.WidthF - 2 * labelShift_thick, 9 * rowHeight);
            Panel_33_41.AnchorVertical = VerticalAnchorStyles.Top;
            Panel_33_41.Borders = DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom;
            Panel_33_41.BorderWidth = 1;
            Panel_33_41.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            Panel_33_41.BackColor = Color.Transparent;
            Panel_33_41.BringToFront();
            formularioDetail.Controls.Add(Panel_33_41);

            XRTable table_33_41 = new XRTable();
            XRTableRow table_33_41_Row;
            XRTableCell table_33_41_Cell;
            table_33_41.AnchorVertical = VerticalAnchorStyles.Top;
            table_33_41.LocationF = Panel_33_41.LocationF;
            table_33_41.SizeF = Panel_33_41.SizeF;
            table_33_41.StyleName = "tableStyle";
            table_33_41.BeginInit();

            for (int i = 33; i < 42; i++)
            {
                table_33_41_Row = new XRTableRow();
                table_33_41_Row.HeightF = rowHeight;
                table_33_41_Row.BackColor = (i % 2 == 0) ? oddRowColor : evenRowColor;

                table_33_41_Cell = new XRTableCell();
                table_33_41_Cell.WidthF = (table_33_41.WidthF - 20) / 2;
                table_33_41_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(18, 0, 0, 0);
                switch (i)
                {
                    case 33:
                        table_33_41_Cell.Text = "Efectivo, bancos, otras inversiones";
                        break;
                    case 34:
                        table_33_41_Cell.Text = "Cuentas por cobrar";
                        break;
                    case 35:
                        table_33_41_Cell.Text = "Acciones y aportes";
                        break;
                    case 36:
                        table_33_41_Cell.Text = "Invertarios";
                        break;
                    case 37:
                        table_33_41_Cell.Text = "Activos fijos";
                        break;
                    case 38:
                        table_33_41_Cell.Text = "Otros activos";
                        break;
                    case 39:
                        table_33_41_Cell.Text = "Total patrimonio bruto";
                        table_33_41_Row.Font = new Font("Arrial", 6, FontStyle.Bold);
                        break;
                    case 40:
                        table_33_41_Cell.Text = "Pasivos";
                        break;
                    case 41:
                        table_33_41_Cell.Text = "Total patrimonio líquido";
                        table_33_41_Cell.CanGrow = false;
                        table_33_41_Row.Font = new Font("Arrial", 6, FontStyle.Bold);
                        table_33_41_Cell.Font = new Font("Arrial", 5, FontStyle.Bold);
                        table_33_41_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;
                };
                table_33_41_Row.Cells.Add(table_33_41_Cell);

                table_33_41_Cell = new XRTableCell()
                {
                    WidthF = 20,
                    Text = i.ToString(),
                    Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right,
                    BorderColor = Color.FromArgb(0xDB, 0x53, 0x53),
                };
                table_33_41_Row.Cells.Add(table_33_41_Cell);

                table_33_41_Cell = new XRTableCell();
                table_33_41_Cell.WidthF = (table_33_41.WidthF - 20) / 2;
                table_33_41_Cell.CanGrow = false;
                foreach (DTO_FormDecDetail item in formData.FormDecDetail)
                {
                    if (Convert.ToInt32(item.Renglon.Trim()) == i)
                        table_33_41_Cell.Text = (Math.Round(item.ValorML, 0)).ToString("#,0.");
                };
                table_33_41_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                table_33_41_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0);
                table_33_41_Cell.Font = new System.Drawing.Font("Courier new", 7);
                table_33_41_Row.Cells.Add(table_33_41_Cell);
                table_33_41.Rows.Add(table_33_41_Row);
            };
            table_33_41.EndInit();
            formularioDetail.Controls.Add(table_33_41);

            XRLabel label_33_41 = new XRLabel();
            label_33_41.LocationF = Panel_33_41.LocationF;
            label_33_41.SizeF = new SizeF(17, 9 * rowHeight);
            label_33_41.CanGrow = false;
            label_33_41.ForeColor = Color.Black;
            label_33_41.BackColor = Color.White;
            label_33_41.Font = new Font("Arial Narrow", 6, FontStyle.Bold);
            label_33_41.Text = "Patrimonio";
            label_33_41.Angle = 90;
            label_33_41.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            label_33_41.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            label_33_41.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            label_33_41.SendToBack();
            formularioDetail.Controls.Add(label_33_41);

            XRLabel label_39 = new XRLabel();
            label_39.LocationF = new PointF(Panel_33_41.LocationF.X, Panel_33_41.LocationF.Y + 6 * rowHeight);
            label_39.SizeF = new SizeF((table_33_41.WidthF - 20) / 2, rowHeight);
            label_39.ForeColor = Color.Black;
            label_39.BackColor = Color.Transparent;
            label_39.Font = new System.Drawing.Font("Arial", 6, FontStyle.Regular);
            label_39.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            label_39.Padding = new DevExpress.XtraPrinting.PaddingInfo(18, 11, 0, 0);
            label_39.Text = "(Sume 33 a 38)";
            formularioDetail.Controls.Add(label_39);

            XRLabel label_41 = new XRLabel();
            label_41.LocationF = new PointF(Panel_33_41.LocationF.X, Panel_33_41.LocationF.Y + 8 * rowHeight - 1);
            label_41.SizeF = new SizeF((Panel_33_41.WidthF - 20) / 2, 25);
            label_41.ForeColor = Color.Black;
            label_41.BackColor = Color.Transparent;
            label_41.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_41.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_41.Padding = new DevExpress.XtraPrinting.PaddingInfo(18, 0, 0, 0);
            label_41.Multiline = true;
            label_41.Text = "\r\n(39-40, si el resultado es negativo escriba 0)";
            formularioDetail.Controls.Add(label_41);

            table_33_41.SendToBack();
            #endregion

            #region Table 3
            XRTable table_42_48 = new XRTable();
            XRTableRow table_42_48_Row;
            XRTableCell table_42_48_Cell;
            table_42_48.LocationF = new PointF(Panel_33_41.LocationF.X, Panel_33_41.LocationF.Y + Panel_33_41.HeightF);
            table_42_48.AnchorVertical = VerticalAnchorStyles.Top;
            table_42_48.SizeF = new SizeF(Panel_33_41.WidthF, 7 * rowHeight);
            table_42_48.StyleName = "tableStyle";
            table_42_48.BeginInit();

            for (int i = 42; i < 49; i++)
            {
                table_42_48_Row = new XRTableRow();
                table_42_48_Row.HeightF = rowHeight;
                table_42_48_Row.BackColor = (i % 2 == 0) ? oddRowColor : evenRowColor;

                table_42_48_Cell = new XRTableCell();
                table_42_48_Cell.WidthF = (table_42_48.WidthF - 20) / 2;
                table_42_48_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(18, 0, 0, 0);
                switch (i)
                {
                    case 42:
                        table_42_48_Cell.Text = "Ingresos brutos operacionales";
                        break;
                    case 43:
                        table_42_48_Cell.Text = "Ingresos brutos no operacionales";
                        break;
                    case 44:
                        table_42_48_Cell.Text = "Intereses y redimientos financieros";
                        break;
                    case 45:
                        table_42_48_Cell.Text = "Total ingresos brutos";
                        table_42_48_Row.Font = new Font("Arrial", 6, FontStyle.Bold);
                        break;
                    case 46:
                        table_42_48_Cell.Text = "Devoluciones, rebajas y descuentos en ventas";
                        table_42_48_Cell.Font = new Font("Arrial", 5);
                        break;
                    case 47:
                        table_42_48_Cell.Text = "Ingresos no constitutivos de renta ni ganancia";
                        table_42_48_Cell.CanGrow = false;
                        table_42_48_Cell.Font = new Font("Arrial", 5);
                        table_42_48_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;
                    case 48:
                        table_42_48_Cell.Text = "Total ingresos netos";
                        table_42_48_Row.Font = new Font("Arrial", 6, FontStyle.Bold);
                        break;
                };
                table_42_48_Row.Cells.Add(table_42_48_Cell);

                table_42_48_Cell = new XRTableCell()
                {
                    WidthF = 20,
                    Text = i.ToString(),
                    Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right,
                    BorderColor = Color.FromArgb(0xDB, 0x53, 0x53),
                };
                table_42_48_Row.Cells.Add(table_42_48_Cell);

                table_42_48_Cell = new XRTableCell();
                table_42_48_Cell.WidthF = (table_42_48.WidthF - 20) / 2;
                table_42_48_Cell.CanGrow = false;
                foreach (DTO_FormDecDetail item in formData.FormDecDetail)
                {
                    if (Convert.ToInt32(item.Renglon.Trim()) == i)
                        table_42_48_Cell.Text = (Math.Round(item.ValorML, 0)).ToString("#,0.");
                };
                table_42_48_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                table_42_48_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0);
                table_42_48_Cell.Font = new System.Drawing.Font("Courier new", 7);
                table_42_48_Row.Cells.Add(table_42_48_Cell);
                table_42_48.Rows.Add(table_42_48_Row);
            };
            table_42_48.EndInit();
            formularioDetail.Controls.Add(table_42_48);

            XRLabel label_42_48 = new XRLabel();
            label_42_48.LocationF = table_42_48.LocationF;
            label_42_48.SizeF = new SizeF(17, 7 * rowHeight);
            label_42_48.CanGrow = false;
            label_42_48.ForeColor = Color.Black;
            label_42_48.BackColor = Color.White;
            label_42_48.Font = new Font("Arial Narrow", 6, FontStyle.Bold);
            label_42_48.Text = "Ingresos";
            label_42_48.Angle = 90;
            label_42_48.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            label_42_48.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            label_42_48.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            label_42_48.SendToBack();
            formularioDetail.Controls.Add(label_42_48);

            XRLabel label_45 = new XRLabel();
            label_45.LocationF = new PointF(table_42_48.LocationF.X, table_42_48.LocationF.Y + 3 * rowHeight);
            label_45.SizeF = new SizeF((table_42_48.WidthF - 20) / 2, rowHeight);
            label_45.ForeColor = Color.Black;
            label_45.BackColor = Color.Transparent;
            label_45.Font = new System.Drawing.Font("Arial", 6, FontStyle.Regular);
            label_45.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            label_45.Padding = new DevExpress.XtraPrinting.PaddingInfo(18, 14, 0, 0);
            label_45.Text = "(Sume 42 a 44)";
            formularioDetail.Controls.Add(label_45);

            XRLabel label_47 = new XRLabel();
            label_47.LocationF = new PointF(table_42_48.LocationF.X, table_42_48.LocationF.Y + 5 * rowHeight - 1);
            label_47.SizeF = new SizeF((table_42_48.WidthF - 20) / 2, 25);
            label_47.ForeColor = Color.Black;
            label_47.BackColor = Color.Transparent;
            label_47.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_47.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_47.Padding = new DevExpress.XtraPrinting.PaddingInfo(18, 0, 0, 0);
            label_47.Multiline = true;
            label_47.Text = "\r\nocasional";
            formularioDetail.Controls.Add(label_47);

            XRLabel label_48 = new XRLabel();
            label_48.LocationF = new PointF(table_42_48.LocationF.X, table_42_48.LocationF.Y + 6 * rowHeight);
            label_48.SizeF = new SizeF((table_42_48.WidthF - 20) / 2, rowHeight);
            label_48.ForeColor = Color.Black;
            label_48.BackColor = Color.Transparent;
            label_48.Font = new System.Drawing.Font("Arial", 6, FontStyle.Regular);
            label_48.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            label_48.Padding = new DevExpress.XtraPrinting.PaddingInfo(18, 26, 0, 0);
            label_48.Text = "(45 - 46 - 47)";
            formularioDetail.Controls.Add(label_48);

            table_42_48.SendToBack();
            #endregion

            #region Table 4
            XRPanel Panel_49_51 = new XRPanel();
            Panel_49_51.LocationF = new PointF(Panel_33_41.LocationF.X, Panel_33_41.LocationF.Y + 16 * rowHeight);
            Panel_49_51.SizeF = new SizeF(Panel_33_41.WidthF, 3 * rowHeight);
            Panel_49_51.AnchorVertical = VerticalAnchorStyles.Top;
            Panel_49_51.Borders = DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom;
            Panel_49_51.BorderWidth = 1;
            Panel_49_51.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            Panel_49_51.BackColor = Color.Transparent;
            Panel_49_51.BringToFront();
            formularioDetail.Controls.Add(Panel_49_51);

            XRTable table_49_51 = new XRTable();
            XRTableRow table_49_51_Row;
            XRTableCell table_49_51_Cell;
            table_49_51.LocationF = Panel_49_51.LocationF;
            table_49_51.SizeF = Panel_49_51.SizeF;
            table_49_51.StyleName = "tableStyle";
            table_49_51.BeginInit();

            for (int i = 49; i < 52; i++)
            {
                table_49_51_Row = new XRTableRow();
                table_49_51_Row.HeightF = rowHeight;
                table_49_51_Row.BackColor = (i % 2 == 0) ? oddRowColor : evenRowColor;

                table_49_51_Cell = new XRTableCell();
                table_49_51_Cell.WidthF = (table_49_51.WidthF - 20) / 2;
                table_49_51_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(18, 0, 0, 0);
                switch (i)
                {
                    case 49:
                        table_49_51_Cell.Text = "Costo de ventas";
                        break;
                    case 50:
                        table_49_51_Cell.Text = "Otros costos";
                        break;
                    case 51:
                        table_49_51_Cell.Text = "Total costos";
                        table_49_51_Row.Font = new Font("Arrial", 6, FontStyle.Bold);
                        break;
                };
                table_49_51_Row.Cells.Add(table_49_51_Cell);

                table_49_51_Cell = new XRTableCell()
                {
                    WidthF = 20,
                    Text = i.ToString(),
                    Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right,
                    BorderColor = Color.FromArgb(0xDB, 0x53, 0x53),
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

            XRLabel label_49_51 = new XRLabel();
            label_49_51.LocationF = Panel_49_51.LocationF;
            label_49_51.SizeF = new SizeF(17, 3 * rowHeight);
            label_49_51.CanGrow = false;
            label_49_51.ForeColor = Color.Black;
            label_49_51.BackColor = Color.White;
            label_49_51.Font = new Font("Arial Narrow", 6, FontStyle.Bold);
            label_49_51.Text = "Costos";
            label_49_51.Angle = 90;
            label_49_51.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            label_49_51.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            label_49_51.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            label_49_51.SendToBack();
            formularioDetail.Controls.Add(label_49_51);

            XRLabel label_51 = new XRLabel();
            label_51.LocationF = new PointF(Panel_49_51.LocationF.X, Panel_49_51.LocationF.Y + 2 * rowHeight);
            label_51.SizeF = new SizeF((Panel_49_51.WidthF - 20) / 2, rowHeight);
            label_51.ForeColor = Color.Black;
            label_51.BackColor = Color.Transparent;
            label_51.Font = new System.Drawing.Font("Arial", 6, FontStyle.Regular);
            label_51.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            label_51.Padding = new DevExpress.XtraPrinting.PaddingInfo(18, 71, 0, 0);
            label_51.Text = "(49 + 50)";
            formularioDetail.Controls.Add(label_51);

            table_49_51.SendToBack();
            #endregion

            #region Table 5
            XRTable table_52_56 = new XRTable();
            XRTableRow table_52_56_Row;
            XRTableCell table_52_56_Cell;
            table_52_56.LocationF = new PointF(Panel_49_51.LocationF.X, Panel_49_51.LocationF.Y + Panel_49_51.HeightF);
            table_52_56.SizeF = new SizeF(Panel_49_51.WidthF, 5 * rowHeight);
            table_52_56.StyleName = "tableStyle";
            table_52_56.BeginInit();

            for (int i = 52; i < 57; i++)
            {
                table_52_56_Row = new XRTableRow();
                table_52_56_Row.HeightF = rowHeight;
                table_52_56_Row.BackColor = (i % 2 == 0) ? oddRowColor : evenRowColor;

                table_52_56_Cell = new XRTableCell();
                table_52_56_Cell.WidthF = (table_52_56.WidthF - 20) / 2;
                table_52_56_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(18, 0, 0, 0);
                switch (i)
                {
                    case 52:
                        table_52_56_Cell.Text = "Gastos operacionales de administración";
                        break;
                    case 53:
                        table_52_56_Cell.Text = "Gastos operacionales de ventas";
                        break;
                    case 54:
                        table_52_56_Cell.Text = "Deducción inversionales en activos fijos";
                        break;
                    case 55:
                        table_52_56_Cell.Text = "Otras deducciones";
                        break;
                    case 56:
                        table_52_56_Cell.Text = "Total deducciones";
                        table_52_56_Row.Font = new Font("Arrial", 6, FontStyle.Bold);
                        break;
                };
                table_52_56_Row.Cells.Add(table_52_56_Cell);

                table_52_56_Cell = new XRTableCell()
                {
                    WidthF = 20,
                    Text = i.ToString(),
                    Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right,
                    BorderColor = Color.FromArgb(0xDB, 0x53, 0x53),
                };
                table_52_56_Row.Cells.Add(table_52_56_Cell);

                table_52_56_Cell = new XRTableCell();
                table_52_56_Cell.WidthF = (table_52_56.WidthF - 20) / 2;
                table_52_56_Cell.CanGrow = false;
                foreach (DTO_FormDecDetail item in formData.FormDecDetail)
                {
                    if (Convert.ToInt32(item.Renglon.Trim()) == i)
                        table_52_56_Cell.Text = (Math.Round(item.ValorML, 0)).ToString("#,0.");
                };
                table_52_56_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                table_52_56_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0);
                table_52_56_Cell.Font = new System.Drawing.Font("Courier new", 7);
                table_52_56_Row.Cells.Add(table_52_56_Cell);
                table_52_56.Rows.Add(table_52_56_Row);
            };
            table_52_56.EndInit();
            formularioDetail.Controls.Add(table_52_56);

            XRLabel label_52_56 = new XRLabel();
            label_52_56.LocationF = table_52_56.LocationF;
            label_52_56.SizeF = new SizeF(17, 5 * rowHeight);
            label_52_56.CanGrow = false;
            label_52_56.ForeColor = Color.Black;
            label_52_56.BackColor = Color.White;
            label_52_56.Font = new Font("Arial Narrow", 6, FontStyle.Bold);
            label_52_56.Text = "Deducciones";
            label_52_56.Angle = 90;
            label_52_56.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            label_52_56.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            label_52_56.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            label_52_56.SendToBack();
            formularioDetail.Controls.Add(label_52_56);

            XRLabel label_56 = new XRLabel();
            label_56.LocationF = new PointF(table_52_56.LocationF.X, table_52_56.LocationF.Y + 4 * rowHeight);
            label_56.SizeF = new SizeF((table_52_56.WidthF - 20) / 2, rowHeight);
            label_56.ForeColor = Color.Black;
            label_56.BackColor = Color.Transparent;
            label_56.Font = new System.Drawing.Font("Arial", 6, FontStyle.Regular);
            label_56.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            label_56.Padding = new DevExpress.XtraPrinting.PaddingInfo(18, 24, 0, 0);
            label_56.Text = "(Sume 52 a 55)";
            formularioDetail.Controls.Add(label_56);

            table_52_56.SendToBack();
            #endregion

            XRPanel Panel_52_56_blank = new XRPanel();
            Panel_52_56_blank.LocationF = new PointF(Panel_49_51.LocationF.X, Panel_49_51.LocationF.Y + 8 * rowHeight);
            Panel_52_56_blank.SizeF = new SizeF(Panel_49_51.WidthF, 2 * rowHeight - 1);
            Panel_52_56_blank.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            Panel_52_56_blank.BorderWidth = 1;
            Panel_52_56_blank.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            Panel_52_56_blank.BackColor = Color.FromArgb(0xE7, 0xAB, 0xAB);
            Panel_52_56_blank.AnchorVertical = VerticalAnchorStyles.Top;
            Panel_52_56_blank.SendToBack();
            formularioDetail.Controls.Add(Panel_52_56_blank);
            #endregion

            #region Right part
            XRShape frame_57_84 = new XRShape();
            frame_57_84.LocationF = new PointF(frame_28_29.WidthF - frameShift, frame_28_29.LocationF.Y);
            frame_57_84.SizeF = new SizeF(formularioWidth / 2 + frameShift, 28 * rowHeight + 2 * labelShift_thick);
            frame_57_84.AnchorVertical = VerticalAnchorStyles.Top;
            frame_57_84.LineWidth = 2;
            frame_57_84.BackColor = Color.Transparent;
            frame_57_84.ForeColor = Color.FromArgb(0xDB, 0x53, 0x53);
            frame_57_84.Shape = new ShapeRectangle()
            {
                Fillet = 5,
            };
            frame_57_84.CanGrow = false;
            frame_57_84.AnchorVertical = VerticalAnchorStyles.Top;
            formularioDetail.Controls.Add(frame_57_84);

            #region Table 1
            XRTable table_57_64 = new XRTable();
            XRTableRow table_57_64_Row;
            XRTableCell table_57_64_Cell;
            table_57_64.LocationF = new PointF(frame_57_84.LocationF.X + labelShift_thick, frame_57_84.LocationF.Y + labelShift_thick);
            table_57_64.SizeF = new SizeF(frame_57_84.WidthF - 2 * labelShift_thick, 8 * rowHeight);
            table_57_64.StyleName = "tableStyle";
            table_57_64.BeginInit();

            for (int i = 57; i < 65; i++)
            {
                table_57_64_Row = new XRTableRow();
                table_57_64_Row.HeightF = rowHeight;
                table_57_64_Row.BackColor = (i % 2 == 0) ? oddRowColor : evenRowColor;

                table_57_64_Cell = new XRTableCell();
                table_57_64_Cell.WidthF = (table_57_64.WidthF - 20) / 2;
                table_57_64_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(18, 0, 0, 0);
                switch (i)
                {
                    case 57:
                        table_57_64_Cell.Text = "Renta líquida ordinaria del ejercicio";
                        table_57_64_Cell.CanGrow = false;
                        table_57_64_Row.Font = new Font("Arrial", 6, FontStyle.Bold);
                        table_57_64_Cell.Font = new Font("Arrial", 5, FontStyle.Bold);
                        table_57_64_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;
                    case 58:
                        table_57_64_Cell.Text = "o Perdida líquida del ejercicio";
                        table_57_64_Cell.CanGrow = false;
                        table_57_64_Row.Font = new Font("Arrial", 6, FontStyle.Bold);
                        table_57_64_Cell.Font = new Font("Arrial", 5, FontStyle.Bold);
                        table_57_64_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;
                    case 59:
                        table_57_64_Cell.Text = "Compensaciones";
                        break;
                    case 60:
                        table_57_64_Cell.Text = "Renta líquida";
                        table_57_64_Row.Font = new Font("Arrial", 6, FontStyle.Bold);
                        break;
                    case 61:
                        table_57_64_Cell.Text = "Renta presuntiva";
                        break;
                    case 62:
                        table_57_64_Cell.Text = "Renta exenta";
                        break;
                    case 63:
                        table_57_64_Cell.Text = "Rentas gravables";
                        break;
                    case 64:
                        table_57_64_Cell.Text = "Renta líquida gravable";
                        table_57_64_Cell.CanGrow = false;
                        table_57_64_Row.Font = new Font("Arrial", 6, FontStyle.Bold);
                        table_57_64_Cell.Font = new Font("Arrial", 5, FontStyle.Bold);
                        table_57_64_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;
                };
                table_57_64_Row.Cells.Add(table_57_64_Cell);
                table_57_64_Cell = new XRTableCell()
                {
                    WidthF = 20,
                    Text = i.ToString(),
                    Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right,
                    BorderColor = Color.FromArgb(0xDB, 0x53, 0x53)
                };
                table_57_64_Row.Cells.Add(table_57_64_Cell);
                table_57_64_Cell = new XRTableCell();
                table_57_64_Cell.WidthF = (table_57_64.WidthF - 20) / 2;
                table_57_64_Cell.CanGrow = false;
                foreach (DTO_FormDecDetail item in formData.FormDecDetail)
                {
                    if (Convert.ToInt32(item.Renglon.Trim()) == i)
                        table_57_64_Cell.Text = (Math.Round(item.ValorML, 0)).ToString("#,0.");
                };
                table_57_64_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                table_57_64_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0);
                table_57_64_Cell.Font = new System.Drawing.Font("Courier new", 7);
                table_57_64_Row.Cells.Add(table_57_64_Cell);
                table_57_64.Rows.Add(table_57_64_Row);
            };
            table_57_64.EndInit();
            formularioDetail.Controls.Add(table_57_64);

            XRLabel label_57_64 = new XRLabel();
            label_57_64.LocationF = table_57_64.LocationF;
            label_57_64.SizeF = new SizeF(17, 8 * rowHeight);
            label_57_64.CanGrow = false;
            label_57_64.ForeColor = Color.Black;
            label_57_64.BackColor = Color.White;
            label_57_64.Font = new Font("Arial Narrow", 6, FontStyle.Bold);
            label_57_64.Text = "Renta";
            label_57_64.Angle = 90;
            label_57_64.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            label_57_64.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            label_57_64.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            label_57_64.SendToBack();
            formularioDetail.Controls.Add(label_57_64);

            XRLabel label_57 = new XRLabel();
            label_57.LocationF = new PointF(table_57_64.LocationF.X, table_57_64.LocationF.Y - 1);
            label_57.SizeF = new SizeF((table_57_64.WidthF - 20) / 2, 25);
            label_57.ForeColor = Color.Black;
            label_57.BackColor = Color.Transparent;
            label_57.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_57.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_57.Padding = new DevExpress.XtraPrinting.PaddingInfo(18, 0, 0, 0);
            label_57.Multiline = true;
            label_57.Text = "\r\n(48-51-56, si el resultado es negativo escriba 0)";
            formularioDetail.Controls.Add(label_57);

            XRLabel label_58 = new XRLabel();
            label_58.LocationF = new PointF(table_57_64.LocationF.X, table_57_64.LocationF.Y + 1 * rowHeight - 1);
            label_58.SizeF = new SizeF((table_57_64.WidthF - 20) / 2, 25);
            label_58.ForeColor = Color.Black;
            label_58.BackColor = Color.Transparent;
            label_58.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_58.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_58.Padding = new DevExpress.XtraPrinting.PaddingInfo(18, 0, 0, 0);
            label_58.Multiline = true;
            label_58.Text = "\r\n(51+56-48, si el resultado es negativo escriba 0)";
            formularioDetail.Controls.Add(label_58);

            XRLabel label_60 = new XRLabel();
            label_60.LocationF = new PointF(table_57_64.LocationF.X, table_57_64.LocationF.Y + 3 * rowHeight);
            label_60.SizeF = new SizeF((table_57_64.WidthF - 20) / 2, rowHeight);
            label_60.ForeColor = Color.Black;
            label_60.BackColor = Color.Transparent;
            label_60.Font = new System.Drawing.Font("Arial", 6, FontStyle.Regular);
            label_60.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            label_60.Padding = new DevExpress.XtraPrinting.PaddingInfo(18, 69, 0, 0);
            label_60.Text = "(57 - 59)";
            formularioDetail.Controls.Add(label_60);

            XRLabel label_64 = new XRLabel();
            label_64.LocationF = new PointF(table_57_64.LocationF.X, table_57_64.LocationF.Y + 7 * rowHeight - 1);
            label_64.SizeF = new SizeF((table_57_64.WidthF - 20) / 2, 25);
            label_64.ForeColor = Color.Black;
            label_64.BackColor = Color.Transparent;
            label_64.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_64.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_64.Padding = new DevExpress.XtraPrinting.PaddingInfo(18, 0, 0, 0);
            label_64.Multiline = true;
            label_64.Text = "\r\n(Al mayor valor entre 60 y 61, reste 62 y sume 63)";
            formularioDetail.Controls.Add(label_64);

            table_57_64.SendToBack();
            #endregion

            #region Table 2
            XRPanel Panel_65_68 = new XRPanel();
            Panel_65_68.LocationF = new PointF(frame_57_84.LocationF.X + labelShift_thick, frame_57_84.LocationF.Y + labelShift_thick + 8 * rowHeight);
            Panel_65_68.SizeF = new SizeF(frame_57_84.WidthF - 2 * labelShift_thick, 4 * rowHeight);
            Panel_65_68.AnchorVertical = VerticalAnchorStyles.Top;
            Panel_65_68.Borders = DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom;
            Panel_65_68.BorderWidth = 1;
            Panel_65_68.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            Panel_65_68.BackColor = Color.Transparent;
            Panel_65_68.BringToFront();
            formularioDetail.Controls.Add(Panel_65_68);

            XRTable table_65_68 = new XRTable();
            XRTableRow table_65_68_Row;
            XRTableCell table_65_68_Cell;
            table_65_68.LocationF = Panel_65_68.LocationF;
            table_65_68.SizeF = Panel_65_68.SizeF;
            table_65_68.StyleName = "tableStyle";
            table_65_68.BeginInit();

            for (int i = 65; i < 69; i++)
            {
                table_65_68_Row = new XRTableRow();
                table_65_68_Row.HeightF = rowHeight;
                table_65_68_Row.BackColor = (i % 2 == 0) ? oddRowColor : evenRowColor;

                table_65_68_Cell = new XRTableCell();
                table_65_68_Cell.WidthF = (table_65_68.WidthF - 20) / 2;
                table_65_68_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(18, 0, 0, 0);
                switch (i)
                {
                    case 65:
                        table_65_68_Cell.Text = "Ingresos por ganacias ocasionales";
                        break;
                    case 66:
                        table_65_68_Cell.Text = "Costos por ganacias ocasionales";
                        break;
                    case 67:
                        table_65_68_Cell.Text = "Ganancias ocasionales no gravadas y exentas";
                        table_65_68_Cell.Font = new Font("Arrial", 5);
                        table_65_68_Cell.CanGrow = false;
                        break;
                    case 68:
                        table_65_68_Cell.Text = "Ganancias ocasionales ravables";
                        table_65_68_Cell.CanGrow = false;
                        table_65_68_Row.Font = new Font("Arrial", 6, FontStyle.Bold);
                        table_65_68_Cell.Font = new Font("Arrial", 5, FontStyle.Bold);
                        break;
                };
                table_65_68_Row.Cells.Add(table_65_68_Cell);
                table_65_68_Cell = new XRTableCell()
                {
                    WidthF = 20,
                    Text = i.ToString(),
                    Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right,
                    BorderColor = Color.FromArgb(0xDB, 0x53, 0x53)
                };
                table_65_68_Row.Cells.Add(table_65_68_Cell);
                table_65_68_Cell = new XRTableCell();
                table_65_68_Cell.WidthF = (table_65_68.WidthF - 20) / 2;
                table_65_68_Cell.CanGrow = false;
                foreach (DTO_FormDecDetail item in formData.FormDecDetail)
                {
                    if (Convert.ToInt32(item.Renglon.Trim()) == i)
                        table_65_68_Cell.Text = (Math.Round(item.ValorML, 0)).ToString("#,0.");
                };
                table_65_68_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                table_65_68_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0);
                table_65_68_Cell.Font = new System.Drawing.Font("Courier new", 7);
                table_65_68_Row.Cells.Add(table_65_68_Cell);
                table_65_68.Rows.Add(table_65_68_Row);
            };
            table_65_68.EndInit();
            formularioDetail.Controls.Add(table_65_68);

            XRLabel label_65_68 = new XRLabel();
            label_65_68.LocationF = Panel_65_68.LocationF;
            label_65_68.SizeF = new SizeF(17, 4 * rowHeight);
            label_65_68.CanGrow = false;
            label_65_68.ForeColor = Color.Black;
            label_65_68.BackColor = Color.White;
            label_65_68.Font = new Font("Arial Narrow", 6, FontStyle.Bold);
            label_65_68.Text = "Ganancias"; // ocasionales";
            label_65_68.Angle = 90;
            label_65_68.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            label_65_68.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            label_65_68.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            formularioDetail.Controls.Add(label_65_68);

            XRLabel label_65_68_rightPart = new XRLabel();
            label_65_68_rightPart.LocationF = label_65_68.LocationF;
            label_65_68_rightPart.SizeF = label_65_68.SizeF;
            label_65_68_rightPart.CanGrow = false;
            label_65_68_rightPart.ForeColor = Color.Black;
            label_65_68_rightPart.BackColor = Color.Transparent;
            label_65_68_rightPart.Font = new Font("Arial Narrow", 6, FontStyle.Bold);
            label_65_68_rightPart.Text = "ocasionales";
            label_65_68_rightPart.Angle = 90;
            label_65_68_rightPart.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            label_65_68_rightPart.Borders = DevExpress.XtraPrinting.BorderSide.None;
            label_65_68_rightPart.BringToFront();
            formularioDetail.Controls.Add(label_65_68_rightPart);

            label_65_68.SendToBack();

            XRLabel label_68 = new XRLabel();
            label_68.LocationF = new PointF(Panel_65_68.LocationF.X, Panel_65_68.LocationF.Y + 3 * rowHeight);
            label_68.SizeF = new SizeF((Panel_65_68.WidthF - 20) / 2, rowHeight);
            label_68.ForeColor = Color.Black;
            label_68.BackColor = Color.Transparent;
            label_68.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_68.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            label_68.Padding = new DevExpress.XtraPrinting.PaddingInfo(18, 10, 0, 0);
            label_68.Text = "(65 - 66 - 67)";
            formularioDetail.Controls.Add(label_68);

            table_65_68.SendToBack();
            #endregion

            #region Table 3
            XRTable table_69_84 = new XRTable();
            XRTableRow table_69_84_Row;
            XRTableCell table_69_84_Cell;
            table_69_84.LocationF = new PointF(Panel_65_68.LocationF.X, Panel_65_68.LocationF.Y + Panel_65_68.HeightF);
            table_69_84.SizeF = new SizeF(Panel_65_68.WidthF, 16 * rowHeight);
            table_69_84.StyleName = "tableStyle";
            table_69_84.BeginInit();

            for (int i = 69; i < 85; i++)
            {
                table_69_84_Row = new XRTableRow();
                table_69_84_Row.HeightF = rowHeight;
                table_69_84_Row.BackColor = (i % 2 == 0) ? oddRowColor : evenRowColor;
                table_69_84_Cell = new XRTableCell();
                table_69_84_Cell.WidthF = (table_69_84.WidthF - 20) / 2;
                table_69_84_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(18, 0, 0, 0);
                switch (i)
                {
                    case 69:
                        table_69_84_Cell.Text = "Impuesto sobre la renta líquida gravable";
                        table_69_84_Row.Font = new Font("Arrial", 6, FontStyle.Bold);
                        break;
                    case 70:
                        table_69_84_Cell.Text = "Descuentos tributarios";
                        break;
                    case 71:
                        table_69_84_Cell.Text = "Impuesto neto de renta";
                        table_69_84_Row.Font = new Font("Arrial", 6, FontStyle.Bold);
                        break;
                    case 72:
                        table_69_84_Cell.Text = "Impuesto de ganancias ocasionales";
                        break;
                    case 73:
                        table_69_84_Cell.Text = "Impuesto de remesas";
                        break;
                    case 74:
                        table_69_84_Cell.Text = "Total impuesto a cargo";
                        table_69_84_Row.Font = new Font("Arrial", 6, FontStyle.Bold);
                        break;
                    case 75:
                        table_69_84_Cell.Text = "Anticipo renta por el año gravable 2011";
                        table_69_84_Cell.CanGrow = false;
                        table_69_84_Cell.Font = new Font("Arrial", 5);
                        table_69_84_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;
                    case 76:
                        table_69_84_Cell.Text = "Saldo a favor año 2010 sin solicitud de devolución";
                        table_69_84_Cell.CanGrow = false;
                        table_69_84_Cell.Font = new Font("Arrial", 5);
                        table_69_84_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;
                    case 77:
                        table_69_84_Cell.Text = "Autorretenciones";
                        break;
                    case 78:
                        table_69_84_Cell.Text = "Otras retenciones";
                        break;
                    case 79:
                        table_69_84_Cell.Text = "Total retenciones año gravable 2011";
                        table_69_84_Cell.CanGrow = false;
                        table_69_84_Row.Font = new Font("Arrial", 6, FontStyle.Bold);
                        table_69_84_Cell.Font = new Font("Arrial", 5, FontStyle.Bold);
                        break;
                    case 80:
                        table_69_84_Cell.Text = "Anticipo renta por el año gravable 2012";
                        break;
                    case 81:
                        table_69_84_Cell.Text = "Saldo a pagar por impuesto";
                        table_69_84_Cell.CanGrow = false;
                        table_69_84_Row.Font = new Font("Arrial", 6, FontStyle.Bold);
                        table_69_84_Cell.Font = new Font("Arrial", 5, FontStyle.Bold);
                        table_69_84_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;
                    case 82:
                        table_69_84_Cell.Text = "Sanciones";
                        break;
                    case 83:
                        table_69_84_Cell.Text = "Total saldo a pagar";
                        table_69_84_Cell.CanGrow = false;
                        table_69_84_Row.Font = new Font("Arrial", 6, FontStyle.Bold);
                        table_69_84_Cell.Font = new Font("Arrial", 5, FontStyle.Bold);
                        table_69_84_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;
                    case 84:
                        table_69_84_Cell.Text = "o Total saldo a favor";
                        table_69_84_Cell.CanGrow = false;
                        table_69_84_Row.Font = new Font("Arrial", 6, FontStyle.Bold);
                        table_69_84_Cell.Font = new Font("Arrial", 5, FontStyle.Bold);
                        table_69_84_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
                        break;
                };

                table_69_84_Row.Cells.Add(table_69_84_Cell);
                table_69_84_Cell = new XRTableCell()
                {
                    WidthF = 20,
                    Text = i.ToString(),
                    Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right,
                    BorderColor = Color.FromArgb(0xDB, 0x53, 0x53)
                };
                table_69_84_Row.Cells.Add(table_69_84_Cell);
                table_69_84_Cell = new XRTableCell();
                table_69_84_Cell.WidthF = (table_69_84.WidthF - 20) / 2;
                table_69_84_Cell.CanGrow = false;
                foreach (DTO_FormDecDetail item in formData.FormDecDetail)
                {
                    if (Convert.ToInt32(item.Renglon.Trim()) == i)
                        table_69_84_Cell.Text = (Math.Round(item.ValorML, 0)).ToString("#,0.");
                };
                table_69_84_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                table_69_84_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0);
                table_69_84_Cell.Font = new System.Drawing.Font("Courier new", 7);
                table_69_84_Row.Cells.Add(table_69_84_Cell);
                table_69_84.Rows.Add(table_69_84_Row);
            };
            table_69_84.EndInit();
            table_69_84.SendToBack();
            formularioDetail.Controls.Add(table_69_84);

            XRLabel label_69_84 = new XRLabel();
            label_69_84.LocationF = table_69_84.LocationF;
            label_69_84.SizeF = new SizeF(17, 16 * rowHeight);
            label_69_84.CanGrow = false;
            label_69_84.ForeColor = Color.Black;
            label_69_84.BackColor = Color.White;
            label_69_84.Font = new Font("Arial", 6, FontStyle.Bold);
            label_69_84.Text = "Liquidación privada";
            label_69_84.Angle = 90;
            label_69_84.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            label_69_84.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            label_69_84.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            label_69_84.SendToBack();
            formularioDetail.Controls.Add(label_69_84);

            XRLabel label_71 = new XRLabel();
            label_71.LocationF = new PointF(table_69_84.LocationF.X, table_69_84.LocationF.Y + 2 * rowHeight);
            label_71.SizeF = new SizeF((table_69_84.WidthF - 20) / 2, rowHeight);
            label_71.ForeColor = Color.Black;
            label_71.BackColor = Color.Transparent;
            label_71.Font = new System.Drawing.Font("Arial", 6, FontStyle.Regular);
            label_71.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            label_71.Padding = new DevExpress.XtraPrinting.PaddingInfo(18, 32, 0, 0);
            label_71.Text = "(69 - 70)";
            formularioDetail.Controls.Add(label_71);

            XRLabel label_74 = new XRLabel();
            label_74.LocationF = new PointF(table_69_84.LocationF.X, table_69_84.LocationF.Y + 5 * rowHeight);
            label_74.SizeF = new SizeF((table_69_84.WidthF - 20) / 2, rowHeight);
            label_74.ForeColor = Color.Black;
            label_74.BackColor = Color.Transparent;
            label_74.Font = new System.Drawing.Font("Arial", 6, FontStyle.Regular);
            label_74.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            label_74.Padding = new DevExpress.XtraPrinting.PaddingInfo(18, 9, 0, 0);
            label_74.Text = "(Sume 71 a 73)";
            formularioDetail.Controls.Add(label_74);

            XRLabel label_75 = new XRLabel();
            label_75.LocationF = new PointF(table_69_84.LocationF.X, table_69_84.LocationF.Y + 6 * rowHeight - 1);
            label_75.SizeF = new SizeF((table_69_84.WidthF - 20) / 2, 25);
            label_75.ForeColor = Color.Black;
            label_75.BackColor = Color.Transparent;
            label_75.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_75.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_75.Padding = new DevExpress.XtraPrinting.PaddingInfo(18, 0, 0, 0);
            label_75.Multiline = true;
            label_75.Text = "\r\n(Casilla 80 declaración 2010)";
            formularioDetail.Controls.Add(label_75);

            XRLabel label_76 = new XRLabel();
            label_76.LocationF = new PointF(table_69_84.LocationF.X, table_69_84.LocationF.Y + 7 * rowHeight - 1);
            label_76.SizeF = new SizeF((table_69_84.WidthF - 20) / 2, 25);
            label_76.ForeColor = Color.Black;
            label_76.BackColor = Color.Transparent;
            label_76.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_76.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_76.Padding = new DevExpress.XtraPrinting.PaddingInfo(18, 0, 0, 0);
            label_76.Multiline = true;
            label_76.Text = "\r\no compensación (Casilla 80 declaración 2010)";
            formularioDetail.Controls.Add(label_76);

            XRLabel label_79 = new XRLabel();
            label_79.LocationF = new PointF(table_69_84.LocationF.X, table_69_84.LocationF.Y + 10 * rowHeight);
            label_79.SizeF = new SizeF((table_69_84.WidthF - 20) / 2, rowHeight);
            label_79.ForeColor = Color.Black;
            label_79.BackColor = Color.Transparent;
            label_79.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_79.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            label_79.Padding = new DevExpress.XtraPrinting.PaddingInfo(18, 13, 0, 0);
            label_79.Text = "(77+78)";
            formularioDetail.Controls.Add(label_79);

            XRLabel label_81_upper = new XRLabel();
            label_81_upper.LocationF = new PointF(table_69_84.LocationF.X, table_69_84.LocationF.Y + 12 * rowHeight);
            label_81_upper.SizeF = new SizeF((table_69_84.WidthF - 20) / 2, rowHeight);
            label_81_upper.ForeColor = Color.Black;
            label_81_upper.BackColor = Color.Transparent;
            label_81_upper.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_81_upper.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            label_81_upper.Padding = new DevExpress.XtraPrinting.PaddingInfo(18, 11, 0, 0);
            label_81_upper.Multiline = true;
            label_81_upper.Text = "(74+80-75-76-79,";
            formularioDetail.Controls.Add(label_81_upper);

            XRLabel label_81 = new XRLabel();
            label_81.LocationF = new PointF(table_69_84.LocationF.X, table_69_84.LocationF.Y + 12 * rowHeight - 1);
            label_81.SizeF = new SizeF((table_69_84.WidthF - 20) / 2, 25);
            label_81.ForeColor = Color.Black;
            label_81.BackColor = Color.Transparent;
            label_81.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_81.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_81.Padding = new DevExpress.XtraPrinting.PaddingInfo(18, 8, 0, 0);
            label_81.Multiline = true;
            label_81.Text = "\r\nsi el resultado es negativo escriba 0)";
            formularioDetail.Controls.Add(label_81);

            XRLabel label_83_upper = new XRLabel();
            label_83_upper.LocationF = new PointF(table_69_84.LocationF.X, table_69_84.LocationF.Y + 14 * rowHeight);
            label_83_upper.SizeF = new SizeF((table_69_84.WidthF - 20) / 2, rowHeight);
            label_83_upper.ForeColor = Color.Black;
            label_83_upper.BackColor = Color.Transparent;
            label_83_upper.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_83_upper.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            label_83_upper.Padding = new DevExpress.XtraPrinting.PaddingInfo(18, 25, 0, 0);
            label_83_upper.Multiline = true;
            label_83_upper.Text = "(74+80+82-75-76-79,";
            formularioDetail.Controls.Add(label_83_upper);

            XRLabel label_83 = new XRLabel();
            label_83.LocationF = new PointF(table_69_84.LocationF.X, table_69_84.LocationF.Y + 14 * rowHeight - 1);
            label_83.SizeF = new SizeF((table_69_84.WidthF - 20) / 2, 25);
            label_83.ForeColor = Color.Black;
            label_83.BackColor = Color.Transparent;
            label_83.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_83.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_83.Padding = new DevExpress.XtraPrinting.PaddingInfo(18, 5, 0, 0);
            label_83.Multiline = true;
            label_83.Text = "\r\nsi el resultado es negativo escriba 0)";
            formularioDetail.Controls.Add(label_83);

            XRLabel label_84_upper = new XRLabel();
            label_84_upper.LocationF = new PointF(table_69_84.LocationF.X, table_69_84.LocationF.Y + 15 * rowHeight);
            label_84_upper.SizeF = new SizeF((table_69_84.WidthF - 20) / 2, rowHeight);
            label_84_upper.ForeColor = Color.Black;
            label_84_upper.BackColor = Color.Transparent;
            label_84_upper.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_84_upper.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            label_84_upper.Padding = new DevExpress.XtraPrinting.PaddingInfo(18, 32, 0, 0);
            label_84_upper.Multiline = true;
            label_84_upper.Text = "(75+76+79-80-82,";
            formularioDetail.Controls.Add(label_84_upper);

            XRLabel label_84 = new XRLabel();
            label_84.LocationF = new PointF(table_69_84.LocationF.X, table_69_84.LocationF.Y + 15 * rowHeight - 1);
            label_84.SizeF = new SizeF((table_69_84.WidthF - 20) / 2, 25);
            label_84.ForeColor = Color.Black;
            label_84.BackColor = Color.Transparent;
            label_84.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
            label_84.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            label_84.Padding = new DevExpress.XtraPrinting.PaddingInfo(18, 5, 0, 0);
            label_84.Multiline = true;
            label_84.Text = "\r\nsi el resultado es negativo escriba 0)";
            formularioDetail.Controls.Add(label_84);

            table_69_84.SendToBack();
            #endregion

            #region Table 4
            XRShape frame_85_87 = new XRShape();
            frame_85_87.LocationF = new PointF(frame_57_84.LocationF.X, frame_57_84.LocationF.Y + frame_57_84.HeightF - frameShift);
            frame_85_87.SizeF = new SizeF(frame_57_84.WidthF, 3 * rowHeight + 2 * labelShift_thick);
            frame_85_87.LineWidth = 2;
            frame_85_87.BackColor = Color.Transparent;
            frame_85_87.ForeColor = Color.FromArgb(0xDB, 0x53, 0x53);
            frame_85_87.Shape = new ShapeRectangle()
            {
                Fillet = 15,
            };
            frame_85_87.CanGrow = false;
            frame_85_87.AnchorVertical = VerticalAnchorStyles.Top;
            formularioDetail.Controls.Add(frame_85_87);

            XRTable table_85_87 = new XRTable();
            XRTableRow table_85_87_Row;
            XRTableCell table_85_87_Cell;
            table_85_87.LocationF = new PointF(frame_85_87.LocationF.X + labelShift_thick, frame_85_87.LocationF.Y + labelShift_thick);
            table_85_87.SizeF = new SizeF(frame_85_87.WidthF - 2 * labelShift_thick, 3 * rowHeight);
            table_85_87.StyleName = "tableStyle";
            table_85_87.BeginInit();

            for (int i = 85; i < 88; i++)
            {
                table_85_87_Row = new XRTableRow();
                table_85_87_Row.HeightF = rowHeight;
                table_85_87_Row.BackColor = (i % 2 == 0) ? oddRowColor : evenRowColor;
                table_85_87_Row.Font = new Font("Arial", 6, FontStyle.Bold);
                table_85_87_Cell = new XRTableCell();
                table_85_87_Cell.WidthF = (table_85_87.WidthF - 20) / 2;
                table_85_87_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(18, 0, 0, 0);
                switch (i)
                {
                    case 85:
                        table_85_87_Cell.Text = "Valor pago sanciones";
                        break;
                    case 86:
                        table_85_87_Cell.Text = "Valor pago intereses de mora";
                        break;
                    case 87:
                        table_85_87_Cell.Text = "Valor pago impuesto";
                        break;

                };

                table_85_87_Row.Cells.Add(table_85_87_Cell);
                table_85_87_Cell = new XRTableCell()
                {
                    WidthF = 20,
                    Text = i.ToString(),
                    Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right,
                    BorderColor = Color.FromArgb(0xDB, 0x53, 0x53)
                };
                table_85_87_Row.Cells.Add(table_85_87_Cell);
                table_85_87_Cell = new XRTableCell();
                table_85_87_Cell.WidthF = (table_85_87.WidthF - 20) / 2;
                table_85_87_Cell.CanGrow = false;
                foreach (DTO_FormDecDetail item in formData.FormDecDetail)
                {
                    if (Convert.ToInt32(item.Renglon.Trim()) == i)
                        table_85_87_Cell.Text = (Math.Round(item.ValorML, 0)).ToString("#,0.");
                };
                table_85_87_Cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                table_85_87_Cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0);
                table_85_87_Cell.Font = new System.Drawing.Font("Courier new", 7);
                table_85_87_Row.Cells.Add(table_85_87_Cell);
                table_85_87.Rows.Add(table_85_87_Row);
            };
            table_85_87.EndInit();
            table_85_87.SendToBack();
            formularioDetail.Controls.Add(table_85_87);

            XRLabel label_85_87 = new XRLabel();
            label_85_87.LocationF = new PointF(table_85_87.LocationF.X, table_85_87.LocationF.Y);
            label_85_87.SizeF = new SizeF(17, 3 * rowHeight);
            label_85_87.CanGrow = false;
            label_85_87.ForeColor = Color.Black;
            label_85_87.BackColor = Color.White;
            label_85_87.Font = new Font("Arial", 6, FontStyle.Bold);
            label_85_87.Text = "Pagos";
            label_85_87.Angle = 90;
            label_85_87.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            label_85_87.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            label_85_87.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            label_85_87.SendToBack();
            formularioDetail.Controls.Add(label_85_87);

            table_85_87.SendToBack();
            #endregion
            #endregion
            #endregion

            #region Formulario part 5
            //XRShape infoFrame = new XRShape();
            //infoFrame.LocationF = new PointF(0, frame_30_56.LocationF.Y + frame_30_56.HeightF - frameShift);
            //infoFrame.SizeF = new SizeF(formularioWidth, 120 + frameShift);
            //infoFrame.LineWidth = 2;
            //infoFrame.Borders = DevExpress.XtraPrinting.BorderSide.None;
            //infoFrame.SnapLineMargin = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0);
            //infoFrame.BackColor = Color.Transparent;
            //infoFrame.ForeColor = Color.FromArgb(0xDB, 0x53, 0x53);
            //infoFrame.Shape = new ShapeRectangle()
            //{
            //    Fillet = 10,
            //};
            //infoFrame.SendToBack();
            //formularioDetail.Controls.Add(infoFrame);

            //XRLabel upperInfoLabel = new XRLabel();
            //upperInfoLabel.LocationF = infoFrame.LocationF;
            //upperInfoLabel.SizeF = new SizeF(infoFrame.WidthF, infoFrame.HeightF / 2);
            //upperInfoLabel.ForeColor = Color.Black;
            //upperInfoLabel.BackColor = Color.Transparent;
            //upperInfoLabel.Font = new Font("Arial", 14, FontStyle.Bold);
            //upperInfoLabel.Multiline = true;
            //upperInfoLabel.Text = "Servicios Informáticos Electrónicos - ¡Más formas de servirle!";
            //upperInfoLabel.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 20, 0);
            //upperInfoLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            //formularioDetail.Controls.Add(upperInfoLabel);

            //XRLabel lowerInfoLabel = new XRLabel();
            //lowerInfoLabel.LocationF = new PointF(upperInfoLabel.LocationF.X, upperInfoLabel.LocationF.Y + upperInfoLabel.HeightF);
            //lowerInfoLabel.SizeF = new SizeF(infoFrame.WidthF, infoFrame.HeightF / 2);
            //lowerInfoLabel.ForeColor = Color.Black;
            //lowerInfoLabel.BackColor = Color.Transparent;
            //lowerInfoLabel.Font = new Font("Arial", 12, FontStyle.Regular);
            //lowerInfoLabel.Multiline = true;
            //lowerInfoLabel.Text = "Este formulario también puede diligenciarlo a www.dian.gov.co\r\nAsistido,sin errores y de manera gratuita";
            //lowerInfoLabel.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 15);
            //lowerInfoLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            //formularioDetail.Controls.Add(lowerInfoLabel);
            #endregion

            #region Formulario part 6
            XRShape frame_88_99 = new XRShape();
            frame_88_99.LocationF = new PointF(0, frame_30_56.LocationF.Y + frame_30_56.HeightF - frameShift);
            frame_88_99.SizeF = new SizeF(formularioWidth, 4 * rowHeight + 2 * labelShift_thick);
            frame_88_99.LineWidth = 2;
            frame_88_99.Borders = DevExpress.XtraPrinting.BorderSide.None;
            frame_88_99.SnapLineMargin = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0);
            frame_88_99.BackColor = Color.Transparent;
            frame_88_99.ForeColor = Color.FromArgb(0xDB, 0x53, 0x53);
            frame_88_99.Shape = new ShapeRectangle()
            {
                Fillet = 10,
            };
            frame_88_99.SendToBack();
            formularioDetail.Controls.Add(frame_88_99);

            XRLabel signatariosLabel = new XRLabel();
            signatariosLabel.LocationF = new PointF(frame_88_99.LocationF.X + labelShift_thick, frame_88_99.LocationF.Y + labelShift_thick);
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
            XRPanel Panel_88_89 = new XRPanel();
            Panel_88_89.LocationF = new PointF(signatariosLabel.LocationF.X + signatariosLabel.WidthF, signatariosLabel.LocationF.Y);
            Panel_88_89.SizeF = new SizeF(frame_88_99.WidthF / 3 - signatariosLabel.WidthF, 2 * rowHeight);
            Panel_88_89.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom;
            Panel_88_89.BorderWidth = 1;
            Panel_88_89.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            Panel_88_89.BackColor = Color.Transparent;
            Panel_88_89.BringToFront();
            formularioDetail.Controls.Add(Panel_88_89);

            XRTable table_88_89 = new XRTable();
            XRTableRow table_88_89_Row;
            XRTableCell table_88_89_Cell;
            table_88_89.LocationF = Panel_88_89.LocationF;
            table_88_89.SizeF = Panel_88_89.SizeF;
            table_88_89.StyleName = "tableStyle";
            table_88_89.BeginInit();
            table_88_89_Row = new XRTableRow();
            table_88_89_Row.HeightF = rowHeight;
            table_88_89_Row.BackColor = evenRowColor;
            table_88_89_Cell = new XRTableCell()
            {
                WidthF = table_88_89.WidthF - 30,
                Text = "88. Número de Identificación Tributaria (NIT)"
            };
            table_88_89_Row.Cells.Add(table_88_89_Cell);
            table_88_89_Cell = new XRTableCell()
            {
                WidthF = 30,
                Text = "89. DV",
                Padding = new DevExpress.XtraPrinting.PaddingInfo(1, 0, 0, 0),
                WordWrap = false,
                CanGrow = false
            };
            table_88_89_Row.Cells.Add(table_88_89_Cell);
            table_88_89.Rows.Add(table_88_89_Row);
            table_88_89_Row = new XRTableRow();
            table_88_89_Row.HeightF = rowHeight;
            table_88_89_Row.BackColor = evenRowColor;
            table_88_89_Cell = new XRTableCell()
            {
                WidthF = table_88_89.WidthF - 30,
                ///////////// Datos
            };
            table_88_89_Row.Cells.Add(table_88_89_Cell);
            table_88_89_Cell = new XRTableCell()
            {
                WidthF = 30,
                Borders = DevExpress.XtraPrinting.BorderSide.Left,
                BorderColor = Color.FromArgb(0xDB, 0x53, 0x53)
                ///////////// Datos
            };
            table_88_89_Row.Cells.Add(table_88_89_Cell);
            table_88_89.Rows.Add(table_88_89_Row);
            table_88_89.EndInit();
            table_88_89.SendToBack();
            formularioDetail.Controls.Add(table_88_89);
            #endregion

            #region Table 2
            XRPanel Panel_90_93 = new XRPanel();
            Panel_90_93.LocationF = new PointF(table_88_89.LocationF.X + table_88_89.WidthF, table_88_89.LocationF.Y);
            Panel_90_93.SizeF = new SizeF(2 * frame_88_99.WidthF / 3 - 2 * labelShift_thick, 2 * rowHeight);
            Panel_90_93.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom;
            Panel_90_93.BorderWidth = 1;
            Panel_90_93.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            Panel_90_93.BackColor = Color.Transparent;
            Panel_90_93.BringToFront();
            formularioDetail.Controls.Add(Panel_90_93);

            XRLabel label_90_93 = new XRLabel();
            label_90_93.LocationF = Panel_90_93.LocationF;
            label_90_93.SizeF = new SizeF(Panel_90_93.WidthF, rowHeight);
            label_90_93.StyleName = "tableStyle";
            label_90_93.Text = "Apellidos y nombres de quien firma como representante del declarante";
            formularioDetail.Controls.Add(label_90_93);

            XRTable table_90_93 = new XRTable();
            XRTableRow table_90_93_Row;
            XRTableCell table_90_93_Cell;
            table_90_93.LocationF = Panel_90_93.LocationF;
            table_90_93.SizeF = Panel_90_93.SizeF;
            table_90_93.StyleName = "tableStyle";
            table_90_93.BeginInit();
            table_90_93_Row = new XRTableRow();
            table_90_93_Row.HeightF = rowHeight;
            table_90_93_Row.BackColor = evenRowColor;
            table_90_93_Cell = new XRTableCell()
            {
                WidthF = table_90_93.WidthF / 4,
                ///////////// Datos
            };
            table_90_93_Row.Cells.Add(table_90_93_Cell);
            table_90_93_Cell = new XRTableCell()
            {
                WidthF = table_90_93.WidthF / 4,
                ///////////// Datos
            };
            table_90_93_Row.Cells.Add(table_90_93_Cell);
            table_90_93_Cell = new XRTableCell()
            {
                WidthF = table_90_93.WidthF / 4,
                ///////////// Datos
            };
            table_90_93_Row.Cells.Add(table_90_93_Cell);
            table_90_93_Cell = new XRTableCell()
            {
                WidthF = table_90_93.WidthF / 4,
                ///////////// Datos
            };
            table_90_93_Row.Cells.Add(table_90_93_Cell);
            table_90_93.Rows.Add(table_90_93_Row);
            table_90_93_Row = new XRTableRow();
            table_90_93_Row.HeightF = rowHeight;
            table_90_93_Row.BackColor = evenRowColor;
            table_90_93_Row.ForeColor = Color.FromArgb(0xDB, 0x53, 0x53);
            table_90_93_Cell = new XRTableCell()
            {
                WidthF = table_90_93.WidthF / 4,
                Text = "90. Primer apellido"
            };
            table_90_93_Row.Cells.Add(table_90_93_Cell);
            table_90_93_Cell = new XRTableCell()
            {
                WidthF = table_90_93.WidthF / 4,
                Text = "91. Segundo apellido"
            };
            table_90_93_Row.Cells.Add(table_90_93_Cell);
            table_90_93_Cell = new XRTableCell()
            {
                WidthF = table_90_93.WidthF / 4,
                Text = "92. Primer nombre"
            };
            table_90_93_Row.Cells.Add(table_90_93_Cell);
            table_90_93_Cell = new XRTableCell()
            {
                WidthF = table_90_93.WidthF / 4,
                Text = "93. Otros nombres"
            };
            table_90_93_Row.Cells.Add(table_90_93_Cell);
            table_90_93.Rows.Add(table_90_93_Row);
            table_90_93.EndInit();
            table_90_93.SendToBack();
            formularioDetail.Controls.Add(table_90_93);

            XRTable table_71_74_division = new XRTable();
            XRTableRow table_71_74_division_Row;
            XRTableCell table_71_74_division_Cell;
            table_71_74_division.LocationF = new PointF(table_90_93.LocationF.X, table_90_93.LocationF.Y + 3 * rowHeight / 2);
            table_71_74_division.SizeF = new SizeF(table_90_93.WidthF - table_90_93.WidthF / 4, rowHeight / 2);
            table_71_74_division.BeginInit();
            table_71_74_division_Row = new XRTableRow();
            table_71_74_division_Row.HeightF = rowHeight / 2;
            table_71_74_division_Row.BackColor = evenRowColor;
            for (int i = 0; i < 3; i++)
            {
                table_71_74_division_Cell = new XRTableCell()
                {
                    WidthF = table_90_93.WidthF / 4,
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
            XRPanel Panel_94_95 = new XRPanel();
            Panel_94_95.LocationF = new PointF(signatariosLabel.LocationF.X + signatariosLabel.WidthF, Panel_88_89.LocationF.Y + Panel_88_89.HeightF);
            Panel_94_95.SizeF = new SizeF(frame_88_99.WidthF / 3 - signatariosLabel.WidthF, 2 * rowHeight);
            Panel_94_95.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            Panel_94_95.BorderWidth = 1;
            Panel_94_95.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            Panel_94_95.BackColor = Color.Transparent;
            Panel_94_95.BringToFront();
            formularioDetail.Controls.Add(Panel_94_95);

            XRTable table_94_95 = new XRTable();
            XRTableRow table_94_95_Row;
            XRTableCell table_94_95_Cell;
            table_94_95.LocationF = Panel_94_95.LocationF;
            table_94_95.SizeF = Panel_94_95.SizeF;
            table_94_95.StyleName = "tableStyle";
            table_94_95.BeginInit();

            table_94_95_Row = new XRTableRow();
            table_94_95_Row.HeightF = rowHeight;
            table_94_95_Row.BackColor = evenRowColor;

            table_94_95_Cell = new XRTableCell()
            {
                WidthF = table_94_95.WidthF - 30,
                Text = "94. Número NIT contrador o revisor fiscal"
            };
            table_94_95_Row.Cells.Add(table_94_95_Cell);
            table_94_95_Cell = new XRTableCell()
            {
                WidthF = 30,
                Text = "95. DV",
                Padding = new DevExpress.XtraPrinting.PaddingInfo(1, 0, 0, 0),
                WordWrap = false,
                CanGrow = false
            };
            table_94_95_Row.Cells.Add(table_94_95_Cell);
            table_94_95.Rows.Add(table_94_95_Row);

            table_94_95_Row = new XRTableRow();
            table_94_95_Row.HeightF = rowHeight;
            table_94_95_Row.BackColor = evenRowColor;
            table_94_95_Cell = new XRTableCell()
            {
                WidthF = table_94_95.WidthF - 30,
                ///////////// Datos
            };
            table_94_95_Row.Cells.Add(table_94_95_Cell);
            table_94_95_Cell = new XRTableCell()
            {
                WidthF = 30,
                Borders = DevExpress.XtraPrinting.BorderSide.Left,
                BorderColor = Color.FromArgb(0xDB, 0x53, 0x53)
                ///////////// Datos
            };
            table_94_95_Row.Cells.Add(table_94_95_Cell);
            table_94_95.Rows.Add(table_94_95_Row);
            table_94_95.EndInit();
            table_94_95.SendToBack();
            formularioDetail.Controls.Add(table_94_95);
            #endregion

            #region Table 4
            XRPanel Panel_96_99 = new XRPanel();
            Panel_96_99.LocationF = new PointF(table_94_95.LocationF.X + table_94_95.WidthF, table_94_95.LocationF.Y);
            Panel_96_99.SizeF = new SizeF(2 * frame_88_99.WidthF / 3 - 2 * labelShift_thick, 2 * rowHeight); ;
            Panel_96_99.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            Panel_96_99.BorderWidth = 1;
            Panel_96_99.BorderColor = Color.FromArgb(0xDB, 0x53, 0x53);
            Panel_96_99.BackColor = Color.Transparent;
            Panel_96_99.BringToFront();
            formularioDetail.Controls.Add(Panel_96_99);

            XRLabel label_96_99 = new XRLabel();
            label_96_99.LocationF = Panel_96_99.LocationF;
            label_96_99.SizeF = new SizeF(Panel_96_99.WidthF, rowHeight);
            label_96_99.StyleName = "tableStyle";
            label_96_99.Text = "Apellidos y nombres del contador o revisor fiscal";
            formularioDetail.Controls.Add(label_96_99);

            XRTable table_96_99 = new XRTable();
            XRTableRow table_96_99_Row;
            XRTableCell table_96_99_Cell;
            table_96_99.LocationF = Panel_96_99.LocationF;
            table_96_99.SizeF = Panel_96_99.SizeF;
            table_96_99.StyleName = "tableStyle";
            table_96_99.BeginInit();

            table_96_99_Row = new XRTableRow();
            table_96_99_Row.HeightF = rowHeight;
            table_96_99_Row.BackColor = evenRowColor;

            table_96_99_Cell = new XRTableCell()
            {
                WidthF = table_96_99.WidthF / 4,
                ///////////// Datos
            };
            table_96_99_Row.Cells.Add(table_96_99_Cell);
            table_96_99_Cell = new XRTableCell()
            {
                WidthF = table_96_99.WidthF / 4,
                ///////////// Datos
            };
            table_96_99_Row.Cells.Add(table_96_99_Cell);
            table_96_99_Cell = new XRTableCell()
            {
                WidthF = table_96_99.WidthF / 4,
                ///////////// Datos
            };
            table_96_99_Row.Cells.Add(table_96_99_Cell);
            table_96_99_Cell = new XRTableCell()
            {
                WidthF = table_96_99.WidthF / 4,
                ///////////// Datos
            };
            table_96_99_Row.Cells.Add(table_96_99_Cell);
            table_96_99.Rows.Add(table_96_99_Row);
            table_96_99_Row = new XRTableRow();
            table_96_99_Row.HeightF = rowHeight;
            table_96_99_Row.BackColor = evenRowColor;
            table_96_99_Row.ForeColor = Color.FromArgb(0xDB, 0x53, 0x53);
            table_96_99_Cell = new XRTableCell()
            {
                WidthF = table_96_99.WidthF / 4,
                Text = "96. Primer apellido"
            };
            table_96_99_Row.Cells.Add(table_96_99_Cell);
            table_96_99_Cell = new XRTableCell()
            {
                WidthF = table_96_99.WidthF / 4,
                Text = "97. Segundo apellido"
            };
            table_96_99_Row.Cells.Add(table_96_99_Cell);
            table_96_99_Cell = new XRTableCell()
            {
                WidthF = table_96_99.WidthF / 4,
                Text = "98. Primer nombre"
            };
            table_96_99_Row.Cells.Add(table_96_99_Cell);
            table_96_99_Cell = new XRTableCell()
            {
                WidthF = table_96_99.WidthF / 4,
                Text = "99. Otros nombres"
            };
            table_96_99_Row.Cells.Add(table_96_99_Cell);
            table_96_99.Rows.Add(table_96_99_Row);
            table_96_99.EndInit();
            table_96_99.SendToBack();
            formularioDetail.Controls.Add(table_96_99);

            XRTable table_96_99_division = new XRTable();
            XRTableRow table_77_80_division_Row;
            XRTableCell table_77_80_division_Cell;
            table_96_99_division.LocationF = new PointF(table_96_99.LocationF.X, table_96_99.LocationF.Y + 3 * rowHeight / 2);
            table_96_99_division.SizeF = new SizeF(table_96_99.WidthF - table_96_99.WidthF / 4, rowHeight / 2);
            table_96_99_division.BeginInit();
            table_77_80_division_Row = new XRTableRow();
            table_77_80_division_Row.HeightF = rowHeight / 2;
            table_77_80_division_Row.BackColor = evenRowColor;
            for (int i = 0; i < 3; i++)
            {
                table_77_80_division_Cell = new XRTableCell()
                {
                    WidthF = table_96_99.WidthF / 4,
                    Borders = DevExpress.XtraPrinting.BorderSide.Right,
                    BorderColor = Color.FromArgb(0xDB, 0x53, 0x53)
                };
                table_77_80_division_Row.Cells.Add(table_77_80_division_Cell);
            };

            table_96_99_division.Rows.Add(table_77_80_division_Row);
            table_96_99_division.EndInit();
            table_96_99_division.SendToBack();
            formularioDetail.Controls.Add(table_96_99_division);
            #endregion

            #endregion
        }
        
        #endregion
    }
}
