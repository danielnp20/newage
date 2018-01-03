using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using NewAge.DTO.Reportes;
using System.Collections.Generic;

namespace NewAge.Cliente.GUI.WinApp.Reports.Formularios
{
    public partial class FormularioICA : DevExpress.XtraReports.UI.XtraReport
    {
        #region Funciones Privadas
        /// <summary>
        /// Formulario "Formulario para declaración del impuesto industria comercio, avisos y tableros" Constructor
        /// </summary>
        /// <param name="formData">Data for the Formulario</param>
        /// <param name="Date">Period of the Formulario</param>        
        public FormularioICA(DTO_Formularios formData, int yearFisc, int period)
        {
            #region Formulario bands
            InitializeComponent();

            this.Margins = new System.Drawing.Printing.Margins(50, 80, 100, 50);

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
                Font = new Font("Arial", 7, FontStyle.Regular),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft,
                Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0)
            };

            this.StyleSheet.Add(tableStyle);

            #endregion

            float formularioWidth = this.PageWidth - this.Margins.Left - this.Margins.Right - 1;
            float frameShift = 2;
            float rowHeight = 15;
            string currentRenglon = "";


            #region Formulario title part
            XRPanel logoPanel = new XRPanel();
            logoPanel.LocationF = new PointF(0, 0);
            logoPanel.SizeF = new SizeF(formularioWidth / 9, 90);
            logoPanel.BackColor = Color.Transparent;
            logoPanel.AnchorVertical = VerticalAnchorStyles.Top;
            formularioDetail.Controls.Add(logoPanel);

            XRLabel logoLabel = new XRLabel();
            logoLabel.LocationF = logoPanel.LocationF;
            logoLabel.SizeF = logoPanel.SizeF;
            logoLabel.ForeColor = Color.Black;
            logoLabel.BackColor = Color.Transparent;
            logoLabel.Font = new Font("Arial", 6, FontStyle.Bold);
            logoLabel.Multiline = true;
            logoLabel.Text = "ALCADÍA MAYOR\r\nDE BOGOTÁ D.C.";
            logoLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            formularioDetail.Controls.Add(logoLabel);

            XRPanel titlePanel = new XRPanel();
            titlePanel.LocationF = new PointF(logoPanel.WidthF, 0);
            titlePanel.SizeF = new SizeF(3 * formularioWidth / 9, 90);
            titlePanel.BackColor = Color.Transparent;
            titlePanel.AnchorVertical = VerticalAnchorStyles.Top;
            formularioDetail.Controls.Add(titlePanel);

            XRLabel titleLabel = new XRLabel();
            titleLabel.LocationF = titlePanel.LocationF;
            titleLabel.SizeF = titlePanel.SizeF;
            titleLabel.ForeColor = Color.Black;
            titleLabel.BackColor = Color.Transparent;
            titleLabel.Font = new Font("Arial", 10, FontStyle.Bold);
            titleLabel.Multiline = true;
            titleLabel.Text = "Formulario para declaración\r\ndel impuesto industria\r\ncomercio, avisos y tableros";
            titleLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            titleLabel.BringToFront();
            formularioDetail.Controls.Add(titleLabel);

            XRPanel barPanel = new XRPanel();
            barPanel.LocationF = new PointF(titlePanel.LocationF.X + titlePanel.WidthF, 0);
            barPanel.SizeF = new SizeF(formularioWidth / 9, 90);
            barPanel.BackColor = Color.Transparent;
            barPanel.AnchorVertical = VerticalAnchorStyles.Top;
            formularioDetail.Controls.Add(barPanel);

            XRPanel fromNroPanel = new XRPanel();
            fromNroPanel.LocationF = new PointF(barPanel.LocationF.X + barPanel.WidthF, 0);
            fromNroPanel.SizeF = new SizeF(3 * formularioWidth / 9, 90);
            fromNroPanel.BackColor = Color.Transparent;
            fromNroPanel.Borders = DevExpress.XtraPrinting.BorderSide.All;
            fromNroPanel.BorderWidth = 2;
            fromNroPanel.AnchorVertical = VerticalAnchorStyles.Top;
            formularioDetail.Controls.Add(fromNroPanel);

            XRLabel fromNroLabel_Name = new XRLabel();
            fromNroLabel_Name.LocationF = fromNroPanel.LocationF;
            fromNroLabel_Name.SizeF = new System.Drawing.SizeF(fromNroPanel.SizeF.Width, 20);
            fromNroLabel_Name.ForeColor = Color.Black;
            fromNroLabel_Name.BackColor = Color.Transparent;
            fromNroLabel_Name.Font = new Font("Arial", 8);
            fromNroLabel_Name.Text = "Formulario No.";
            fromNroLabel_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            fromNroLabel_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            formularioDetail.Controls.Add(fromNroLabel_Name);

            XRLabel fromNroLabel_Value = new XRLabel();
            fromNroLabel_Value.LocationF = new PointF(fromNroLabel_Name.LocationF.X, fromNroLabel_Name.LocationF.Y + fromNroLabel_Name.WidthF);
            fromNroLabel_Value.SizeF = new System.Drawing.SizeF(fromNroPanel.SizeF.Width, fromNroPanel.HeightF - fromNroLabel_Name.HeightF);
            fromNroLabel_Value.ForeColor = Color.Black;
            fromNroLabel_Value.BackColor = Color.Transparent;
            fromNroLabel_Value.Font = new Font("Arial", 9);
            /////// Data
            fromNroLabel_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            formularioDetail.Controls.Add(fromNroLabel_Value);

            XRPanel numberPanel = new XRPanel();
            numberPanel.LocationF = new PointF(fromNroPanel.LocationF.X + fromNroPanel.WidthF - frameShift, 0);
            numberPanel.SizeF = new SizeF(formularioWidth / 9 + frameShift, 90);
            numberPanel.BackColor = Color.Transparent;
            numberPanel.Borders = DevExpress.XtraPrinting.BorderSide.All;
            numberPanel.BorderWidth = 2;
            numberPanel.AnchorVertical = VerticalAnchorStyles.Top;
            formularioDetail.Controls.Add(numberPanel);

            XRLabel numberLabel = new XRLabel();
            numberLabel.LocationF = numberPanel.LocationF;
            numberLabel.SizeF = numberPanel.SizeF;
            numberLabel.ForeColor = Color.White;
            numberLabel.BackColor = Color.Gray;
            numberLabel.Font = new Font("Arial", 20, FontStyle.Bold);
            numberLabel.Text = "302";
            numberLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            numberLabel.SendToBack();
            formularioDetail.Controls.Add(numberLabel);
            #endregion

            #region Formulario periodo part
            XRPanel yearPanel = new XRPanel();
            yearPanel.LocationF = new PointF(0, logoPanel.HeightF + 5);
            yearPanel.SizeF = new SizeF(formularioWidth / 4, 30);
            yearPanel.BackColor = Color.Transparent;
            yearPanel.Borders = DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right;
            yearPanel.BorderWidth = 2;
            yearPanel.AnchorVertical = VerticalAnchorStyles.Top;
            formularioDetail.Controls.Add(yearPanel);

            XRLabel yearLable_Name = new XRLabel();
            yearLable_Name.LocationF = yearPanel.LocationF;
            yearLable_Name.SizeF = new SizeF(yearPanel.WidthF / 2, yearPanel.HeightF);
            yearLable_Name.ForeColor = Color.Black;
            yearLable_Name.BackColor = Color.Transparent;
            yearLable_Name.Font = new Font("Arial", 7, FontStyle.Bold);
            yearLable_Name.Text = "AÑO GRAVABLE";
            yearLable_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            formularioDetail.Controls.Add(yearLable_Name);

            XRLabel yearLable_Value = new XRLabel();
            yearLable_Value.LocationF = new PointF(yearLable_Name.LocationF.X + yearLable_Name.WidthF, yearLable_Name.LocationF.Y);
            yearLable_Value.SizeF = new SizeF(yearPanel.WidthF - yearLable_Name.WidthF, yearPanel.HeightF);
            yearLable_Value.ForeColor = Color.Black;
            yearLable_Value.BackColor = Color.Transparent;
            yearLable_Value.Font = new Font("Arial", 7);
            yearLable_Value.Text = yearFisc.ToString();
            yearLable_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            formularioDetail.Controls.Add(yearLable_Value);

            XRPanel periodoPanel = new XRPanel();
            periodoPanel.LocationF = new PointF(yearPanel.WidthF - frameShift, yearPanel.LocationF.Y);
            periodoPanel.SizeF = new SizeF(2 * formularioWidth / 4 + 70 + frameShift, yearPanel.HeightF);
            periodoPanel.BackColor = Color.Transparent;
            periodoPanel.Borders = DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right; ;
            periodoPanel.BorderWidth = 2;
            periodoPanel.AnchorVertical = VerticalAnchorStyles.Top;
            formularioDetail.Controls.Add(periodoPanel);

            XRLabel periodoLable_Name = new XRLabel();
            periodoLable_Name.LocationF = periodoPanel.LocationF;
            periodoLable_Name.SizeF = new SizeF(periodoPanel.WidthF / 3, periodoPanel.HeightF);
            periodoLable_Name.ForeColor = Color.Black;
            periodoLable_Name.BackColor = Color.Transparent;
            periodoLable_Name.Font = new Font("Arial", 7, FontStyle.Bold);
            periodoLable_Name.Text = "PERIODO GRAVABLE";
            periodoLable_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            formularioDetail.Controls.Add(periodoLable_Name);

            XRLabel periodoLable_Regimen = new XRLabel();
            periodoLable_Regimen.LocationF = new PointF(periodoLable_Name.LocationF.X + periodoLable_Name.WidthF, periodoLable_Name.LocationF.Y);
            periodoLable_Regimen.SizeF = new SizeF(periodoPanel.WidthF / 6, periodoPanel.HeightF);
            periodoLable_Regimen.ForeColor = Color.Black;
            periodoLable_Regimen.BackColor = Color.Transparent;
            periodoLable_Regimen.Font = new Font("Arial", 5, FontStyle.Bold);
            periodoLable_Regimen.Text = "Régimen común";
            periodoLable_Regimen.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            formularioDetail.Controls.Add(periodoLable_Regimen);

            XRTable periodoTable = new XRTable();
            periodoTable.LocationF = new PointF(periodoLable_Regimen.LocationF.X + periodoLable_Regimen.WidthF, periodoLable_Regimen.LocationF.Y + frameShift);
            periodoTable.SizeF = new SizeF(periodoPanel.WidthF / 2 - 10, 23);
            XRTableRow periodoTableRow = new XRTableRow();
            periodoTableRow.HeightF = 8;
            XRTableCell periodoTableCell;
            for (int i = 1; i < 7; i++)
            {
                periodoTableCell = new XRTableCell()
                {
                    WidthF = periodoTable.WidthF / 6,
                    Font = new Font("Arial", 4),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight
                };
                switch (i)
                {
                    case 1:
                        periodoTableCell.Text = "ene-feb";
                        break;
                    case 2:
                        periodoTableCell.Text = "mar-abr";
                        break;
                    case 3:
                        periodoTableCell.Text = "may-jun";
                        break;
                    case 4:
                        periodoTableCell.Text = "jul-ago";
                        break;
                    case 5:
                        periodoTableCell.Text = "sep-oct";
                        break;
                    case 6:
                        periodoTableCell.Text = "nov-dec";
                        break;
                };
                periodoTableRow.Cells.Add(periodoTableCell);
            };
            periodoTable.Rows.Add(periodoTableRow);
            periodoTableRow = new XRTableRow();
            periodoTableRow.HeightF = 15;
            for (int i = 1; i < 7; i++)
            {
                periodoTableCell = new XRTableCell()
                {
                    WidthF = periodoTable.WidthF / 6 - 18,
                    Font = new Font("Arial", 7),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 3, 0, 0),
                    Text = i.ToString()
                };
                periodoTableRow.Cells.Add(periodoTableCell);

                periodoTableCell = new XRTableCell()
                {
                    WidthF = 18,
                    Borders = DevExpress.XtraPrinting.BorderSide.All,
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                    Text = (i==period)? "X":string.Empty, 
                };
                periodoTableRow.Cells.Add(periodoTableCell);
            };
            periodoTable.Rows.Add(periodoTableRow);
            formularioDetail.Controls.Add(periodoTable);

            XRPanel periodoPanel_simpl = new XRPanel();
            periodoPanel_simpl.LocationF = new PointF(periodoPanel.LocationF.X + periodoPanel.WidthF - frameShift, periodoPanel.LocationF.Y);
            periodoPanel_simpl.SizeF = new SizeF(formularioWidth / 4 - 70 + frameShift, yearPanel.HeightF);
            periodoPanel_simpl.BackColor = Color.Transparent;
            periodoPanel_simpl.Borders = DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right; ;
            periodoPanel_simpl.BorderWidth = 2;
            periodoPanel_simpl.AnchorVertical = VerticalAnchorStyles.Top;
            formularioDetail.Controls.Add(periodoPanel_simpl);

            XRLabel periodoPanel_simplRegimen = new XRLabel();
            periodoPanel_simplRegimen.LocationF = periodoPanel_simpl.LocationF;
            periodoPanel_simplRegimen.SizeF = new SizeF(periodoPanel_simpl.WidthF - 25, periodoPanel_simpl.HeightF);
            periodoPanel_simplRegimen.ForeColor = Color.Black;
            periodoPanel_simplRegimen.BackColor = Color.Transparent;
            periodoPanel_simplRegimen.Font = new Font("Arial", 5, FontStyle.Bold);
            periodoPanel_simplRegimen.Text = "Régimen simplificado";
            periodoPanel_simplRegimen.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            formularioDetail.Controls.Add(periodoPanel_simplRegimen);

            XRLabel periodoPanel_simplRegimen_blank = new XRLabel();
            periodoPanel_simplRegimen_blank.LocationF = new PointF(periodoPanel_simplRegimen.LocationF.X + periodoPanel_simplRegimen.WidthF, periodoPanel_simplRegimen.LocationF.Y + 8);
            periodoPanel_simplRegimen_blank.SizeF = new SizeF(18, 15);
            periodoPanel_simplRegimen_blank.ForeColor = Color.Black;
            periodoPanel_simplRegimen_blank.BackColor = Color.Transparent;
            periodoPanel_simplRegimen_blank.Borders = DevExpress.XtraPrinting.BorderSide.All;
            formularioDetail.Controls.Add(periodoPanel_simplRegimen_blank);
            #endregion

            #region Formulario A part
            XRPanel Panel_A = new XRPanel();
            Panel_A.LocationF = new PointF(0, yearPanel.LocationF.Y + yearPanel.HeightF);
            Panel_A.SizeF = new SizeF(formularioWidth, 80);
            Panel_A.BackColor = Color.Transparent;
            Panel_A.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right;
            Panel_A.BorderWidth = 2;
            Panel_A.AnchorVertical = VerticalAnchorStyles.Top;
            formularioDetail.Controls.Add(Panel_A);

            XRLabel Lable_A = new XRLabel();
            Lable_A.LocationF = Panel_A.LocationF;
            Lable_A.SizeF = new SizeF(formularioWidth, 20);
            Lable_A.ForeColor = Color.White;
            Lable_A.BackColor = Color.Black;
            Lable_A.Font = new Font("Arial", 7, FontStyle.Bold);
            Lable_A.Text = "A. INFORMACIÓN DEL CONTRIBUYENTE";
            Lable_A.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Lable_A.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            formularioDetail.Controls.Add(Lable_A);

            XRLabel Lable_1_Name = new XRLabel();
            Lable_1_Name.LocationF = new PointF(Lable_A.LocationF.X, Lable_A.LocationF.Y + Lable_A.HeightF);
            Lable_1_Name.SizeF = new SizeF(formularioWidth / 3, 20);
            Lable_1_Name.ForeColor = Color.Black;
            Lable_1_Name.BackColor = Color.Transparent;
            Lable_1_Name.Font = new Font("Arial", 7);
            Lable_1_Name.Text = "1. APELLIDOS Y NOMBRES O RAZÓN SOCIAL: ";
            Lable_1_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Lable_1_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            Lable_1_Name.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            formularioDetail.Controls.Add(Lable_1_Name);

            XRLabel Lable_1_Value = new XRLabel();
            Lable_1_Value.LocationF = new PointF(Lable_1_Name.LocationF.X + Lable_1_Name.WidthF, Lable_1_Name.LocationF.Y);
            Lable_1_Value.SizeF = new SizeF(2 * formularioWidth / 3, 20);
            Lable_1_Value.ForeColor = Color.Black;
            Lable_1_Value.BackColor = Color.Transparent;
            Lable_1_Value.Font = new Font("Arial", 8);
            Lable_1_Value.Text = !string.IsNullOrEmpty(formData.FormDecHeader.RazonSoc) ? formData.FormDecHeader.RazonSoc : 
                formData.FormDecHeader.NombrePri + " " + formData.FormDecHeader.NombreOtr + " " + 
                formData.FormDecHeader.ApellidoPri + " " + formData.FormDecHeader.ApellidoSdo;
            Lable_1_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Lable_1_Value.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            Lable_1_Value.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            formularioDetail.Controls.Add(Lable_1_Value);

            XRLabel Lable_2_Ident = new XRLabel();
            Lable_2_Ident.LocationF = new PointF(Lable_1_Name.LocationF.X, Lable_1_Name.LocationF.Y + Lable_1_Name.HeightF);
            Lable_2_Ident.SizeF = new SizeF(formularioWidth / 6 - 10, 20);
            Lable_2_Ident.ForeColor = Color.Black;
            Lable_2_Ident.BackColor = Color.Transparent;
            Lable_2_Ident.Font = new Font("Arial", 7);
            Lable_2_Ident.Text = "2. IDENTIFICACIÓN";
            Lable_2_Ident.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Lable_2_Ident.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            Lable_2_Ident.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            formularioDetail.Controls.Add(Lable_2_Ident);

            XRTable Table_2 = new XRTable();
            Table_2.LocationF = new PointF(Lable_2_Ident.LocationF.X + Lable_2_Ident.WidthF, Lable_2_Ident.LocationF.Y + 4);
            Table_2.SizeF = new SizeF(formularioWidth / 6 + 10, 12);
            XRTableRow Table_2_Row = new XRTableRow();
            XRTableCell Table_2_Cell;
            for (int i = 1; i < 5; i++)
            {
                bool marked = false;
                Table_2_Cell = new XRTableCell()
                {
                    WidthF = Table_2.WidthF / 4 - 15,
                    Font = new Font("Arial", 5),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 3, 0, 0)
                };
                switch (i)
                {
                    case 1:
                        Table_2_Cell.Text = "CC";
                        if (formData.FormDecHeader.NitTipoDoc.ToUpper() == "CC") marked = true;
                        break;
                    case 2:
                        Table_2_Cell.Text = "NIT";
                        if (formData.FormDecHeader.NitTipoDoc.ToUpper() == "NIT") marked = true;
                        break;
                    case 3:
                        Table_2_Cell.Text = "TI";
                        if (formData.FormDecHeader.NitTipoDoc.ToUpper() == "TI") marked = true;
                        break;
                    case 4:
                        Table_2_Cell.Text = "CE";
                        if (formData.FormDecHeader.NitTipoDoc.ToUpper() == "CE") marked = true;
                        break;
                };
                Table_2_Row.Cells.Add(Table_2_Cell);

                Table_2_Cell = new XRTableCell()
                {
                    WidthF = 15,
                    Borders = DevExpress.XtraPrinting.BorderSide.All,
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                    Text = (marked)?"X":string.Empty
                };
                Table_2_Row.Cells.Add(Table_2_Cell);
            };
            Table_2.Rows.Add(Table_2_Row);
            formularioDetail.Controls.Add(Table_2);

            XRLabel Lable_2_Numero_Value = new XRLabel();
            Lable_2_Numero_Value.LocationF = new PointF(Table_2.LocationF.X + Table_2.WidthF, Lable_2_Ident.LocationF.Y);
            Lable_2_Numero_Value.SizeF = new SizeF(Panel_A.WidthF / 6, 20);
            Lable_2_Numero_Value.ForeColor = Color.Black;
            Lable_2_Numero_Value.BackColor = Color.Transparent;
            Lable_2_Numero_Value.Font = new Font("Arial", 7);
            Lable_2_Numero_Value.Text = formData.FormDecHeader.Nit;
            Lable_2_Numero_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            Lable_2_Numero_Value.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 10, 0, 0);
            formularioDetail.Controls.Add(Lable_2_Numero_Value);

            XRLabel Lable_2_DV_Name = new XRLabel();
            Lable_2_DV_Name.LocationF = new PointF(Lable_2_Numero_Value.LocationF.X + Lable_2_Numero_Value.WidthF, Lable_2_Numero_Value.LocationF.Y);
            Lable_2_DV_Name.SizeF = new SizeF(25, 20);
            Lable_2_DV_Name.ForeColor = Color.Black;
            Lable_2_DV_Name.BackColor = Color.Transparent;
            Lable_2_DV_Name.Font = new Font("Arial", 3, FontStyle.Bold);
            Lable_2_DV_Name.Text = "DV";
            Lable_2_DV_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            Lable_2_DV_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 1, 0);
            formularioDetail.Controls.Add(Lable_2_DV_Name);

            XRLabel Lable_2_DV_Value = new XRLabel();
            Lable_2_DV_Value.LocationF = Lable_2_DV_Name.LocationF;
            Lable_2_DV_Value.SizeF = Lable_2_DV_Name.SizeF;
            Lable_2_DV_Value.ForeColor = Color.Black;
            Lable_2_DV_Value.BackColor = Color.Transparent;
            Lable_2_DV_Value.Font = new Font("Arial", 7);
            Lable_2_DV_Value.Text = formData.FormDecHeader.DV;
            Lable_2_DV_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            formularioDetail.Controls.Add(Lable_2_DV_Value);

            XRLabel Lable_3_Name = new XRLabel();
            Lable_3_Name.LocationF = new PointF(Lable_2_DV_Value.LocationF.X + Lable_2_DV_Value.WidthF, Lable_2_DV_Value.LocationF.Y);
            Lable_3_Name.SizeF = new SizeF((formularioWidth / 2 - 25) / 2, 20);
            Lable_3_Name.ForeColor = Color.Black;
            Lable_3_Name.BackColor = Color.Transparent;
            Lable_3_Name.Font = new Font("Arial", 7);
            Lable_3_Name.Text = "3. TELÉFONO FIJO O MÓVIL: ";
            Lable_3_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Lable_3_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            Lable_3_Name.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            formularioDetail.Controls.Add(Lable_3_Name);

            XRLabel Lable_3_Value = new XRLabel();
            Lable_3_Value.LocationF = new PointF(Lable_3_Name.LocationF.X + Lable_3_Name.WidthF, Lable_3_Name.LocationF.Y);
            Lable_3_Value.SizeF = Lable_3_Name.SizeF;
            Lable_3_Value.ForeColor = Color.Black;
            Lable_3_Value.BackColor = Color.Transparent;
            Lable_3_Value.Font = new Font("Arial", 7);
            //////// Data
            Lable_3_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Lable_3_Value.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            formularioDetail.Controls.Add(Lable_3_Value);

            XRLabel Lable_4_Direccion_Name = new XRLabel();
            Lable_4_Direccion_Name.LocationF = new PointF(Lable_2_Ident.LocationF.X, Lable_2_Ident.LocationF.Y + Lable_2_Ident.HeightF);
            Lable_4_Direccion_Name.SizeF = new SizeF(formularioWidth / 3 - 70, 20);
            Lable_4_Direccion_Name.ForeColor = Color.Black;
            Lable_4_Direccion_Name.BackColor = Color.Transparent;
            Lable_4_Direccion_Name.Font = new Font("Arial", 7);
            Lable_4_Direccion_Name.Text = "4. DIRECCIÓN DE NOTIFICACIÓN: ";
            Lable_4_Direccion_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Lable_4_Direccion_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            Lable_4_Direccion_Name.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            formularioDetail.Controls.Add(Lable_4_Direccion_Name);

            XRLabel Lable_4_Direccion_Value = new XRLabel();
            Lable_4_Direccion_Value.LocationF = new PointF(Lable_4_Direccion_Name.LocationF.X + Lable_4_Direccion_Name.WidthF, Lable_4_Direccion_Name.LocationF.Y);
            Lable_4_Direccion_Value.SizeF = new SizeF(formularioWidth / 3 + 70, 20);
            Lable_4_Direccion_Value.ForeColor = Color.Black;
            Lable_4_Direccion_Value.BackColor = Color.Transparent;
            Lable_4_Direccion_Value.Font = new Font("Arial", 7);
            ///// Data
            Lable_4_Direccion_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Lable_4_Direccion_Value.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            Lable_4_Direccion_Value.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            formularioDetail.Controls.Add(Lable_4_Direccion_Value);

            XRLabel Lable_5_Municipio_Name = new XRLabel();
            Lable_5_Municipio_Name.LocationF = new PointF(Lable_4_Direccion_Value.LocationF.X + Lable_4_Direccion_Value.WidthF, Lable_4_Direccion_Value.LocationF.Y);
            Lable_5_Municipio_Name.SizeF = new SizeF(formularioWidth / 9, 20);
            Lable_5_Municipio_Name.ForeColor = Color.Black;
            Lable_5_Municipio_Name.BackColor = Color.Transparent;
            Lable_5_Municipio_Name.Font = new Font("Arial", 7);
            Lable_5_Municipio_Name.Text = "5. MUNICIPIO";
            Lable_5_Municipio_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Lable_5_Municipio_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            Lable_5_Municipio_Name.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top;
            formularioDetail.Controls.Add(Lable_5_Municipio_Name);

            XRLabel Lable_5_Municipio_Value = new XRLabel();
            Lable_5_Municipio_Value.LocationF = new PointF(Lable_5_Municipio_Name.LocationF.X + Lable_5_Municipio_Name.WidthF, Lable_5_Municipio_Name.LocationF.Y);
            Lable_5_Municipio_Value.SizeF = new SizeF(formularioWidth / 9, 20);
            Lable_5_Municipio_Value.ForeColor = Color.Black;
            Lable_5_Municipio_Value.BackColor = Color.Transparent;
            Lable_5_Municipio_Value.Font = new Font("Arial", 7);
            //////// Data
            Lable_5_Municipio_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Lable_5_Municipio_Value.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            Lable_5_Municipio_Value.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            formularioDetail.Controls.Add(Lable_5_Municipio_Value);

            XRLabel Lable_5_Municipio_Bogota = new XRLabel();
            Lable_5_Municipio_Bogota.LocationF = new PointF(Lable_5_Municipio_Value.LocationF.X + Lable_5_Municipio_Value.WidthF, Lable_5_Municipio_Value.LocationF.Y);
            Lable_5_Municipio_Bogota.SizeF = new SizeF(formularioWidth / 9, 20);
            Lable_5_Municipio_Bogota.ForeColor = Color.Black;
            Lable_5_Municipio_Bogota.BackColor = Color.Transparent;
            Lable_5_Municipio_Bogota.Font = new Font("Arial", 7);
            Lable_5_Municipio_Bogota.Text = "BOGOTÁ D.C.";
            Lable_5_Municipio_Bogota.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Lable_5_Municipio_Bogota.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            Lable_5_Municipio_Bogota.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            formularioDetail.Controls.Add(Lable_5_Municipio_Bogota);
            #endregion

            #region Formulario B part
            XRPanel Panel_B = new XRPanel();
            Panel_B.LocationF = new PointF(0, Panel_A.LocationF.Y + Panel_A.HeightF);
            Panel_B.SizeF = new SizeF(formularioWidth, 195);
            Panel_B.BackColor = Color.Transparent;
            Panel_B.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right;
            Panel_B.BorderWidth = 2;
            Panel_B.AnchorVertical = VerticalAnchorStyles.Top;
            formularioDetail.Controls.Add(Panel_B);

            XRLabel Lable_B = new XRLabel();
            Lable_B.LocationF = Panel_B.LocationF;
            Lable_B.SizeF = new SizeF(formularioWidth, rowHeight);
            Lable_B.AnchorVertical = VerticalAnchorStyles.Top;
            Lable_B.ForeColor = Color.White;
            Lable_B.BackColor = Color.Black;
            Lable_B.Font = new Font("Arial", 7, FontStyle.Bold);
            Lable_B.Text = "B. BASE GRAVABLE";
            Lable_B.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Lable_B.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            formularioDetail.Controls.Add(Lable_B);

            XRLabel Lable_6_Actividad_Name = new XRLabel();
            Lable_6_Actividad_Name.LocationF = new PointF(Lable_B.LocationF.X, Lable_B.LocationF.Y + Lable_B.HeightF);
            Lable_6_Actividad_Name.SizeF = new SizeF(formularioWidth / 3, rowHeight);
            Lable_6_Actividad_Name.ForeColor = Color.Black;
            Lable_6_Actividad_Name.BackColor = Color.Transparent;
            Lable_6_Actividad_Name.Font = new Font("Arial", 7);
            Lable_6_Actividad_Name.Text = "6. ACTIVIDAD ECONÓMICA PRINCIPAL";
            Lable_6_Actividad_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Lable_6_Actividad_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            formularioDetail.Controls.Add(Lable_6_Actividad_Name);

            XRLabel Lable_6_Actividad_Value = new XRLabel();
            Lable_6_Actividad_Value.LocationF = new PointF(Lable_6_Actividad_Name.LocationF.X, Lable_6_Actividad_Name.LocationF.Y + Lable_6_Actividad_Name.HeightF);
            Lable_6_Actividad_Value.SizeF = new SizeF(formularioWidth / 3, 25);
            Lable_6_Actividad_Value.ForeColor = Color.Black;
            Lable_6_Actividad_Value.BackColor = Color.Transparent;
            Lable_6_Actividad_Value.Font = new Font("Arial", 8);
            //////// Data
            Lable_6_Actividad_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            formularioDetail.Controls.Add(Lable_6_Actividad_Value);

            XRLabel Lable_6_Base_Name = new XRLabel();
            Lable_6_Base_Name.LocationF = new PointF(Lable_6_Actividad_Name.LocationF.X + Lable_6_Actividad_Name.WidthF, Lable_6_Actividad_Name.LocationF.Y);
            Lable_6_Base_Name.SizeF = new SizeF(formularioWidth / 3, rowHeight);
            Lable_6_Base_Name.ForeColor = Color.Black;
            Lable_6_Base_Name.BackColor = Color.Transparent;
            Lable_6_Base_Name.Font = new Font("Arial", 7);
            Lable_6_Base_Name.Text = "BASE GRAVABLE ACTIVIDAD PRINCIPAL";
            Lable_6_Base_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Lable_6_Base_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            Lable_6_Base_Name.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            formularioDetail.Controls.Add(Lable_6_Base_Name);

            XRLabel Lable_6_Base_Value = new XRLabel();
            Lable_6_Base_Value.LocationF = new PointF(Lable_6_Base_Name.LocationF.X, Lable_6_Base_Name.LocationF.Y + Lable_6_Base_Name.HeightF);
            Lable_6_Base_Value.SizeF = new SizeF(formularioWidth / 3, 25);
            Lable_6_Base_Value.ForeColor = Color.Black;
            Lable_6_Base_Value.BackColor = Color.Transparent;
            Lable_6_Base_Value.Font = new Font("Arial", 8);
            //////// Data
            Lable_6_Base_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            Lable_6_Base_Value.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            formularioDetail.Controls.Add(Lable_6_Base_Value);

            XRLabel Lable_7_Name = new XRLabel();
            Lable_7_Name.LocationF = new PointF(Lable_6_Base_Name.LocationF.X + Lable_6_Base_Name.WidthF, Lable_6_Base_Name.LocationF.Y);
            Lable_7_Name.SizeF = new SizeF(formularioWidth / 3, rowHeight);
            Lable_7_Name.ForeColor = Color.Black;
            Lable_7_Name.BackColor = Color.Transparent;
            Lable_7_Name.Font = new Font("Arial", 7);
            Lable_7_Name.Text = "7. NÚMERO DE ESTABLECIMIENTOS";
            Lable_7_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Lable_7_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            Lable_7_Name.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            formularioDetail.Controls.Add(Lable_7_Name);

            XRLabel Lable_7_Value = new XRLabel();
            Lable_7_Value.LocationF = new PointF(Lable_7_Name.LocationF.X, Lable_7_Name.LocationF.Y + Lable_7_Name.HeightF);
            Lable_7_Value.SizeF = new SizeF(formularioWidth / 3, 25);
            Lable_7_Value.ForeColor = Color.Black;
            Lable_7_Value.BackColor = Color.Transparent;
            Lable_7_Value.Font = new Font("Arial", 8);
            //////// Data
            Lable_7_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            Lable_7_Value.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            formularioDetail.Controls.Add(Lable_7_Value);

            XRLabel Lable_8_Name = new XRLabel();
            Lable_8_Name.LocationF = new PointF(Lable_6_Actividad_Value.LocationF.X, Lable_6_Actividad_Value.LocationF.Y + Lable_6_Actividad_Value.HeightF);
            Lable_8_Name.SizeF = new SizeF(2 * formularioWidth / 3, rowHeight);
            Lable_8_Name.ForeColor = Color.Black;
            Lable_8_Name.BackColor = Color.Transparent;
            Lable_8_Name.Font = new Font("Arial", 7);
            Lable_8_Name.Text = "8. ACTIVIDADES ECONOMICAS SECUNDARIAS";
            Lable_8_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Lable_8_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            Lable_8_Name.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            formularioDetail.Controls.Add(Lable_8_Name);

            XRLabel Lable_8_Value = new XRLabel();
            Lable_8_Value.LocationF = new PointF(Lable_8_Name.LocationF.X, Lable_8_Name.LocationF.Y + Lable_8_Name.HeightF);
            Lable_8_Value.SizeF = new SizeF(formularioWidth / 3, 35);
            Lable_8_Value.ForeColor = Color.Black;
            Lable_8_Value.BackColor = Color.Transparent;
            Lable_8_Value.Font = new Font("Arial", 8);
            //////// Data
            Lable_8_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            Lable_8_Value.Multiline = true;
            formularioDetail.Controls.Add(Lable_8_Value);

            XRLabel Lable_8_Fecha_Name = new XRLabel();
            Lable_8_Fecha_Name.LocationF = new PointF(Lable_8_Name.LocationF.X + Lable_8_Name.WidthF, Lable_8_Name.LocationF.Y);
            Lable_8_Fecha_Name.SizeF = new SizeF(formularioWidth / 3, 25);
            Lable_8_Fecha_Name.ForeColor = Color.Black;
            Lable_8_Fecha_Name.BackColor = Color.Transparent;
            Lable_8_Fecha_Name.Font = new Font("Arial", 7, FontStyle.Bold);
            Lable_8_Fecha_Name.Text = "FECHA DE PRESENTACIÓN Y/O PAGO";
            Lable_8_Fecha_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            Lable_8_Fecha_Name.Borders = DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Left;
            formularioDetail.Controls.Add(Lable_8_Fecha_Name);

            XRLabel Lable_8_Fecha_Value = new XRLabel();
            Lable_8_Fecha_Value.LocationF = new PointF(Lable_8_Fecha_Name.LocationF.X, Lable_8_Fecha_Name.LocationF.Y + Lable_8_Fecha_Name.HeightF);
            Lable_8_Fecha_Value.SizeF = new SizeF(formularioWidth / 3, 25);
            Lable_8_Fecha_Value.ForeColor = Color.Black;
            Lable_8_Fecha_Value.BackColor = Color.Transparent;
            Lable_8_Fecha_Value.Font = new Font("Arial", 7, FontStyle.Bold);
            //////// Data
            Lable_8_Fecha_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            Lable_8_Fecha_Value.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            formularioDetail.Controls.Add(Lable_8_Fecha_Value);

            XRTable Table_B;
            XRTableRow Table_B_Row;
            XRTableCell Table_B_Cell;
            Table_B = new XRTable();
            Table_B.BeginInit();
            Table_B.LocationF = new PointF(Lable_8_Value.LocationF.X, Lable_8_Value.LocationF.Y + Lable_8_Value.HeightF);
            Table_B.SizeF = new System.Drawing.SizeF(formularioWidth, 6 * rowHeight);
            Table_B.StyleName = "tableStyle";

            for (int i = 9; i < 15; i++)
            {
                Table_B_Row = new XRTableRow();
                Table_B_Row.HeightF = rowHeight;

                Table_B_Cell = new XRTableCell()
                {
                    WidthF = 3 * formularioWidth / 4 - 20,
                    Borders = DevExpress.XtraPrinting.BorderSide.Top
                };
                switch (i)
                {
                    case 9:
                        Table_B_Cell.Text = i.ToString() + ". TOTAL INGRESOS ORDINARIOS Y EXTRAORDINARIOS DEL PERIODO";
                        break;
                    case 10:
                        Table_B_Cell.Text = i.ToString() + ". TOTAL INGRESOS OBTENIDOS FUERA DEL DISTRITO CAPITAL";
                        break;
                    case 11:
                        Table_B_Cell.Text = i.ToString() + ". TOTAL INGRESOS OBTENIDOS EN EL DISTRITO CAPITAL (Renglón 9 - 10)";
                        Table_B_Cell.Font = new Font("Arial", 7, FontStyle.Bold);
                        break;
                    case 12:
                        Table_B_Cell.Text = i.ToString() + ". DEVOLUCIONES, REBAJAS Y DESCUENTOS";
                        break;
                    case 13:
                        Table_B_Cell.Text = i.ToString() + ". DEDUCCIONES, EXENCIONES Y ACTIVIDADES NO SUJETAS";
                        break;
                    case 14:
                        Table_B_Cell.Text = i.ToString() + ". TOTAL INGRESOS NETOS GRAVABLES (Renglón 11 - 12 - 13)";
                        Table_B_Cell.Font = new Font("Arial", 7, FontStyle.Bold);
                        break;
                };
                Table_B_Row.Cells.Add(Table_B_Cell);

                Table_B_Cell = new XRTableCell()
                {
                    WidthF = 20,
                    Font = new Font("Arial", 7, FontStyle.Bold),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                    Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top
                };
                switch (i)
                {
                    case 9:
                        Table_B_Cell.Text = "BA";
                        currentRenglon = "BA";
                        break;
                    case 10:
                        Table_B_Cell.Text = "BC";
                        currentRenglon = "BC";
                        break;
                    case 11:
                        Table_B_Cell.Text = "BT";
                        currentRenglon = "BT";
                        break;
                    case 12:
                        Table_B_Cell.Text = "BB";
                        currentRenglon = "BB";
                        break;
                    case 13:
                        Table_B_Cell.Text = "BD";
                        currentRenglon = "BD";
                        break;
                    case 14:
                        Table_B_Cell.Text = "BE";
                        currentRenglon = "BE";
                        break;
                };
                Table_B_Row.Cells.Add(Table_B_Cell);

                Table_B_Cell = new XRTableCell()
                {
                    WidthF = formularioWidth / 4,
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0),
                    Font = new Font("Arial", 7, FontStyle.Regular),
                    Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top
                };
                foreach (DTO_FormDecDetail item in formData.FormDecDetail)
                {
                    if (item.Renglon.Trim() == currentRenglon)
                        Table_B_Cell.Text = (Math.Round(item.ValorML, 0)).ToString("#,0.");
                };
                Table_B_Row.Cells.Add(Table_B_Cell);
                Table_B.Rows.Add(Table_B_Row);
            }

            Table_B.EndInit();
            formularioDetail.Controls.Add(Table_B);
            #endregion

            #region Formulario C part
            XRPanel Panel_C = new XRPanel();
            Panel_C.LocationF = new PointF(0, Panel_B.LocationF.Y + Panel_B.HeightF);
            Panel_C.SizeF = new SizeF(formularioWidth, 8 * rowHeight);
            Panel_C.BackColor = Color.Transparent;
            Panel_C.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right;
            Panel_C.BorderWidth = 2;
            Panel_C.AnchorVertical = VerticalAnchorStyles.Top;
            formularioDetail.Controls.Add(Panel_C);

            XRLabel Lable_C = new XRLabel();
            Lable_C.LocationF = Panel_C.LocationF;
            Lable_C.SizeF = new SizeF(formularioWidth, rowHeight);
            Lable_C.AnchorVertical = VerticalAnchorStyles.Top;
            Lable_C.ForeColor = Color.White;
            Lable_C.BackColor = Color.Black;
            Lable_C.Font = new Font("Arial", 7, FontStyle.Bold);
            Lable_C.Text = "C. LIQUIDACIÓN PRIVADA";
            Lable_C.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Lable_C.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            formularioDetail.Controls.Add(Lable_C);

            XRTable Table_C;
            XRTableRow Table_C_Row;
            XRTableCell Table_C_Cell;
            Table_C = new XRTable();
            Table_C.BeginInit();
            Table_C.LocationF = new PointF(Lable_C.LocationF.X, Lable_C.LocationF.Y + Lable_C.HeightF);
            Table_C.SizeF = new System.Drawing.SizeF(formularioWidth, 7 * rowHeight);
            Table_C.StyleName = "tableStyle";
            for (int i = 15; i < 22; i++)
            {
                Table_C_Row = new XRTableRow();
                Table_C_Row.HeightF = rowHeight;

                Table_C_Cell = new XRTableCell()
                {
                    WidthF = 3 * formularioWidth / 4 - 20,
                    Borders = DevExpress.XtraPrinting.BorderSide.Top
                };
                switch (i)
                {
                    case 15:
                        Table_C_Cell.Text = i.ToString() + ". IMPUESTO DE INDUSTRIA Y COMERCIO";
                        break;
                    case 16:
                        Table_C_Cell.Text = i.ToString() + ". IMPUESTO DE AVISOS Y TABLEROS (15% DE  Renglón 15)";
                        break;
                    case 17:
                        Table_C_Cell.Text = i.ToString() + ". VALOR TOTAL DE UNIDADES COMERCIALES ADICIONALES";
                        break;
                    case 18:
                        Table_C_Cell.Text = i.ToString() + ". TOTAL IMPUESTO A CARGO (Renglón 15 + Renglón 16 + Renglón 17)";
                        break;
                    case 19:
                        Table_C_Cell.Text = i.ToString() + ". VALOR RETENIDO A TITULO DE IMPUESTO DE INDUSTRIA Y COMERCIO";
                        break;
                    case 20:
                        Table_C_Cell.Text = i.ToString() + ". SANCIONES    Código de sanción ";
                        break;
                    case 21:
                        Table_C_Cell.Text = i.ToString() + ". TOTAL SALDO A CARGO (Renglón 18 - Renglón 19 + Renglón 20)";
                        break;
                };
                Table_C_Row.Cells.Add(Table_C_Cell);

                Table_C_Cell = new XRTableCell()
                {
                    WidthF = 20,
                    Font = new Font("Arial", 7, FontStyle.Bold),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                    Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top
                };
                switch (i)
                {
                    case 15:
                        Table_C_Cell.Text = "IC";
                        currentRenglon = "IC";
                        break;
                    case 16:
                        Table_C_Cell.Text = "BF";
                        currentRenglon = "BF";
                        break;
                    case 17:
                        Table_C_Cell.Text = "BG";
                        currentRenglon = "BG";
                        break;
                    case 18:
                        Table_C_Cell.Text = "FU";
                        currentRenglon = "FU";
                        break;
                    case 19:
                        Table_C_Cell.Text = "BI";
                        currentRenglon = "BI";
                        break;
                    case 20:
                        Table_C_Cell.Text = "VS";
                        currentRenglon = "VS";
                        break;
                    case 21:
                        Table_C_Cell.Text = "HA";
                        currentRenglon = "HA";
                        break;
                };
                Table_C_Row.Cells.Add(Table_C_Cell);

                Table_C_Cell = new XRTableCell()
                {
                    WidthF = formularioWidth / 4,
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0),
                    Font = new Font("Arial", 7, FontStyle.Regular),
                    Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top
                };
                foreach (DTO_FormDecDetail item in formData.FormDecDetail)
                {
                    if (item.Renglon.Trim() == currentRenglon)
                        Table_C_Cell.Text = (Math.Round(item.ValorML, 0)).ToString("#,0.");
                };
                Table_C_Row.Cells.Add(Table_C_Cell);
                Table_C.Rows.Add(Table_C_Row);
            };

            Table_C.EndInit();
            formularioDetail.Controls.Add(Table_C);
            #endregion

            #region Formulario D part
            XRPanel Panel_D = new XRPanel();
            Panel_D.LocationF = new PointF(0, Panel_C.LocationF.Y + Panel_C.HeightF);
            Panel_D.SizeF = new SizeF(formularioWidth, 4 * rowHeight);
            Panel_D.BackColor = Color.Transparent;
            Panel_D.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right;
            Panel_D.BorderWidth = 2;
            Panel_D.AnchorVertical = VerticalAnchorStyles.Top;
            formularioDetail.Controls.Add(Panel_D);

            XRLabel Lable_D = new XRLabel();
            Lable_D.LocationF = Panel_D.LocationF;
            Lable_D.SizeF = new SizeF(formularioWidth, rowHeight);
            Lable_D.AnchorVertical = VerticalAnchorStyles.Top;
            Lable_D.ForeColor = Color.White;
            Lable_D.BackColor = Color.Black;
            Lable_D.Font = new Font("Arial", 7, FontStyle.Bold);
            Lable_D.Text = "D. PAGO";
            Lable_D.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Lable_D.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            formularioDetail.Controls.Add(Lable_D);

            XRTable Table_D;
            XRTableRow Table_D_Row;
            XRTableCell Table_D_Cell;
            Table_D = new XRTable();
            Table_D.BeginInit();
            Table_D.LocationF = new PointF(Lable_D.LocationF.X, Lable_D.LocationF.Y + Lable_D.HeightF);
            Table_D.SizeF = new System.Drawing.SizeF(formularioWidth, 3 * rowHeight);
            Table_D.StyleName = "tableStyle";
            for (int i = 22; i < 25; i++)
            {
                Table_D_Row = new XRTableRow();
                Table_D_Row.HeightF = rowHeight;

                Table_D_Cell = new XRTableCell()
                {
                    WidthF = 3 * formularioWidth / 4 - 20,
                    Borders = DevExpress.XtraPrinting.BorderSide.Top
                };
                switch (i)
                {
                    case 22:
                        Table_D_Cell.Text = i.ToString() + ". VALOR A PAGAR";
                        break;
                    case 23:
                        Table_D_Cell.Text = i.ToString() + ". INTERESES DE MORA";
                        break;
                    case 24:
                        Table_D_Cell.Text = i.ToString() + ". TOTAL A PAGAR (Renglón 22 + Renglón 23)";
                        break;
                };
                Table_D_Row.Cells.Add(Table_D_Cell);

                Table_D_Cell = new XRTableCell()
                {
                    WidthF = 20,
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                    Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top,
                    Font = new Font("Arial", 7, FontStyle.Bold)
                };
                switch (i)
                {
                    case 22:
                        Table_D_Cell.Text = "VP";
                        currentRenglon = "VP";
                        break;
                    case 23:
                        Table_D_Cell.Text = "IM";
                        currentRenglon = "IM";
                        break;
                    case 24:
                        Table_D_Cell.Text = "TP";
                        currentRenglon = "TP";
                        break;
                };
                Table_D_Row.Cells.Add(Table_D_Cell);

                Table_D_Cell = new XRTableCell()
                {
                    WidthF = formularioWidth / 4,
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0),
                    Font = new Font("Arial", 7, FontStyle.Regular),
                    Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top
                };
                foreach (DTO_FormDecDetail item in formData.FormDecDetail)
                {
                    if (item.Renglon.Trim() == currentRenglon)
                        Table_D_Cell.Text = (Math.Round(item.ValorML, 0)).ToString("#,0.");
                };
                Table_D_Row.Cells.Add(Table_D_Cell);
                Table_D.Rows.Add(Table_D_Row);
            };

            Table_D.EndInit();
            formularioDetail.Controls.Add(Table_D);
            #endregion

            #region Formulario E part
            XRPanel Panel_E = new XRPanel();
            Panel_E.LocationF = new PointF(0, Panel_D.LocationF.Y + Panel_D.HeightF);
            Panel_E.SizeF = new SizeF(formularioWidth, 65);
            Panel_E.BackColor = Color.Transparent;
            Panel_E.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right;
            Panel_E.BorderWidth = 2;
            Panel_E.AnchorVertical = VerticalAnchorStyles.Top;
            formularioDetail.Controls.Add(Panel_E);

            XRLabel Lable_E = new XRLabel();
            Lable_E.LocationF = Panel_E.LocationF;
            Lable_E.SizeF = new SizeF(formularioWidth, rowHeight);
            Lable_E.AnchorVertical = VerticalAnchorStyles.Top;
            Lable_E.ForeColor = Color.White;
            Lable_E.BackColor = Color.Black;
            Lable_E.Font = new Font("Arial", 7, FontStyle.Bold);
            Lable_E.Text = "E. PAGO ADICIONAL VOLUNTARIO";
            Lable_E.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Lable_E.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            formularioDetail.Controls.Add(Lable_E);

            XRLabel Lable_E_1 = new XRLabel();
            Lable_E_1.LocationF = new PointF(Lable_E.LocationF.X, Lable_E.LocationF.Y + Lable_E.HeightF);
            Lable_E_1.SizeF = new SizeF(formularioWidth / 3, 20);
            Lable_E_1.AnchorVertical = VerticalAnchorStyles.Top;
            Lable_E_1.ForeColor = Color.Black;
            Lable_E_1.BackColor = Color.White;
            Lable_E_1.Font = new Font("Arial", 6);
            Lable_E_1.Text = "Aporto voluntariamente un 10% adicional al desarollo de";
            Lable_E_1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Lable_E_1.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            formularioDetail.Controls.Add(Lable_E_1);

            XRLabel Lable_E_Si = new XRLabel();
            Lable_E_Si.LocationF = new PointF(Lable_E_1.LocationF.X + Lable_E_1.WidthF, Lable_E_1.LocationF.Y);
            Lable_E_Si.SizeF = new SizeF(formularioWidth / 24, 20);
            Lable_E_Si.AnchorVertical = VerticalAnchorStyles.Top;
            Lable_E_Si.ForeColor = Color.Black;
            Lable_E_Si.BackColor = Color.White;
            Lable_E_Si.Font = new Font("Arial", 7);
            Lable_E_Si.Text = "Si";
            Lable_E_Si.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            formularioDetail.Controls.Add(Lable_E_Si);

            XRLabel Lable_E_Si_blank = new XRLabel();
            Lable_E_Si_blank.LocationF = new PointF(Lable_E_Si.LocationF.X + Lable_E_Si.WidthF + (formularioWidth / 24 - 18) / 2, Lable_E_Si.LocationF.Y + 2);
            Lable_E_Si_blank.SizeF = new SizeF(18, 15);
            Lable_E_Si_blank.ForeColor = Color.Black;
            Lable_E_Si_blank.BackColor = Color.White;
            Lable_E_Si_blank.Borders = DevExpress.XtraPrinting.BorderSide.All;
            formularioDetail.Controls.Add(Lable_E_Si_blank);

            XRLabel Lable_E_No = new XRLabel();
            Lable_E_No.LocationF = new PointF(Lable_E_Si.LocationF.X + 2 * Lable_E_Si.WidthF, Lable_E_Si.LocationF.Y);
            Lable_E_No.SizeF = new SizeF(formularioWidth / 24, 20);
            Lable_E_No.AnchorVertical = VerticalAnchorStyles.Top;
            Lable_E_No.ForeColor = Color.Black;
            Lable_E_No.BackColor = Color.White;
            Lable_E_No.Font = new Font("Arial", 7);
            Lable_E_No.Text = "No";
            Lable_E_No.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            formularioDetail.Controls.Add(Lable_E_No);

            XRLabel Lable_E_No_blank = new XRLabel();
            Lable_E_No_blank.LocationF = new PointF(Lable_E_No.LocationF.X + Lable_E_No.WidthF + (formularioWidth / 24 - 18) / 2, Lable_E_No.LocationF.Y + 2);
            Lable_E_No_blank.SizeF = new SizeF(18, 15);
            Lable_E_No_blank.ForeColor = Color.Black;
            Lable_E_No_blank.BackColor = Color.White;
            Lable_E_No_blank.Borders = DevExpress.XtraPrinting.BorderSide.All;
            formularioDetail.Controls.Add(Lable_E_No_blank);

            XRLabel Lable_E_2 = new XRLabel();
            Lable_E_2.LocationF = new PointF(Lable_E_No.LocationF.X + 2 * Lable_E_No.WidthF, Lable_E_No.LocationF.Y);
            Lable_E_2.SizeF = new SizeF(formularioWidth / 2, 20);
            Lable_E_2.AnchorVertical = VerticalAnchorStyles.Top;
            Lable_E_2.ForeColor = Color.Black;
            Lable_E_2.BackColor = Color.White;
            Lable_E_2.Font = new Font("Arial", 6);
            Lable_E_2.Text = "Mi aporte debe destinarse al proyecto";
            Lable_E_2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Lable_E_2.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            formularioDetail.Controls.Add(Lable_E_2);

            XRTable Table_E;
            XRTableRow Table_E_Row;
            XRTableCell Table_E_Cell;
            Table_E = new XRTable();
            Table_E.BeginInit();
            Table_E.LocationF = new PointF(Lable_E_1.LocationF.X, Lable_E_1.LocationF.Y + Lable_E_1.HeightF);
            Table_E.SizeF = new System.Drawing.SizeF(formularioWidth, 2 * rowHeight);
            Table_E.StyleName = "tableStyle";
            for (int i = 25; i < 27; i++)
            {
                Table_E_Row = new XRTableRow();
                Table_E_Row.HeightF = rowHeight;

                Table_E_Cell = new XRTableCell()
                {
                    WidthF = 3 * formularioWidth / 4 - 20,
                    Borders = DevExpress.XtraPrinting.BorderSide.Top
                };
                switch (i)
                {
                    case 25:
                        Table_E_Cell.Text = i.ToString() + ". PAGO VOLUNTARIO (10% del Renglón 18)";
                        break;
                    case 26:
                        Table_E_Cell.Text = i.ToString() + ". TOTAL A PAGAR CON PAGO VOLUNTARIO (Renglón 24 + Renglón 25)";
                        break;
                };
                Table_E_Row.Cells.Add(Table_E_Cell);

                Table_E_Cell = new XRTableCell()
                {
                    WidthF = 20,
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0),
                    Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top,
                    Font = new Font("Arial", 7, FontStyle.Bold)
                };
                switch (i)
                {
                    case 25:
                        Table_E_Cell.Text = "AV";
                        currentRenglon = "AV";
                        break;
                    case 26:
                        Table_E_Cell.Text = "TA";
                        currentRenglon = "TA";
                        break;
                };
                Table_E_Row.Cells.Add(Table_E_Cell);

                Table_E_Cell = new XRTableCell()
                {
                    WidthF = formularioWidth / 4,
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0),
                    Font = new Font("Arial", 7, FontStyle.Regular),
                    Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top
                };
                foreach (DTO_FormDecDetail item in formData.FormDecDetail)
                {
                    if (item.Renglon.Trim() == currentRenglon)
                        Table_E_Cell.Text = (Math.Round(item.ValorML, 0)).ToString("#,0.");
                };
                Table_E_Row.Cells.Add(Table_E_Cell);
                Table_E.Rows.Add(Table_E_Row);
            };

            Table_E.EndInit();
            formularioDetail.Controls.Add(Table_E);
            #endregion

            #region Formulario F part
            XRPanel Panel_F = new XRPanel();
            Panel_F.LocationF = new PointF(0, Panel_E.LocationF.Y + Panel_E.HeightF);
            Panel_F.SizeF = new SizeF(formularioWidth, 190 + frameShift);
            Panel_F.BackColor = Color.Transparent;
            Panel_F.Borders = DevExpress.XtraPrinting.BorderSide.Bottom | DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right;
            Panel_F.BorderWidth = 2;
            Panel_F.AnchorVertical = VerticalAnchorStyles.Top;
            formularioDetail.Controls.Add(Panel_F);

            XRLabel Lable_F = new XRLabel();
            Lable_F.LocationF = Panel_F.LocationF;
            Lable_F.SizeF = new SizeF(formularioWidth / 2, rowHeight);
            Lable_F.AnchorVertical = VerticalAnchorStyles.Top;
            Lable_F.ForeColor = Color.White;
            Lable_F.BackColor = Color.Black;
            Lable_F.Font = new Font("Arial", 7, FontStyle.Bold);
            Lable_F.Text = "E. FIRMA";
            Lable_F.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Lable_F.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            formularioDetail.Controls.Add(Lable_F);

            XRLabel Lable_F_1 = new XRLabel();
            Lable_F_1.LocationF = new PointF(Lable_F.LocationF.X + Lable_F.WidthF, Lable_F.LocationF.Y);
            Lable_F_1.SizeF = new SizeF(formularioWidth / 2, rowHeight);
            Lable_F_1.AnchorVertical = VerticalAnchorStyles.Top;
            Lable_F_1.ForeColor = Color.White;
            Lable_F_1.BackColor = Color.Black;
            Lable_F_1.Font = new Font("Arial", 7, FontStyle.Bold);
            Lable_F_1.Text = "ESPACIO RESERVADO PARA LA ENTIDAD RECAUDADORA";
            Lable_F_1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            formularioDetail.Controls.Add(Lable_F_1);

            #region Right part
            XRLabel Lable_F_FirmaDeclarante = new XRLabel();
            Lable_F_FirmaDeclarante.LocationF = new PointF(Lable_F.LocationF.X, Lable_F.LocationF.Y + Lable_F.HeightF);
            Lable_F_FirmaDeclarante.SizeF = new SizeF(formularioWidth / 2, 50);
            Lable_F_FirmaDeclarante.ForeColor = Color.Black;
            Lable_F_FirmaDeclarante.BackColor = Color.White;
            Lable_F_FirmaDeclarante.Font = new Font("Arial", 6, FontStyle.Bold);
            Lable_F_FirmaDeclarante.Text = "FIRMA DEL DECLARANTE";
            Lable_F_FirmaDeclarante.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            Lable_F_FirmaDeclarante.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 5, 0);
            Lable_F_FirmaDeclarante.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            formularioDetail.Controls.Add(Lable_F_FirmaDeclarante);

            XRLabel Lable_F_NombresApellidos_Name = new XRLabel();
            Lable_F_NombresApellidos_Name.LocationF = new PointF(Lable_F_FirmaDeclarante.LocationF.X, Lable_F_FirmaDeclarante.LocationF.Y + Lable_F_FirmaDeclarante.HeightF);
            Lable_F_NombresApellidos_Name.SizeF = new SizeF(formularioWidth / 4, rowHeight);
            Lable_F_NombresApellidos_Name.ForeColor = Color.Black;
            Lable_F_NombresApellidos_Name.BackColor = Color.White;
            Lable_F_NombresApellidos_Name.Font = new Font("Arial", 6, FontStyle.Bold);
            Lable_F_NombresApellidos_Name.Text = "NOMBRES Y APELLIDOS";
            Lable_F_NombresApellidos_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Lable_F_NombresApellidos_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            Lable_F_NombresApellidos_Name.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            formularioDetail.Controls.Add(Lable_F_NombresApellidos_Name);

            XRLabel Lable_F_NombresApellidos_Value = new XRLabel();
            Lable_F_NombresApellidos_Value.LocationF = new PointF(Lable_F_NombresApellidos_Name.LocationF.X + Lable_F_NombresApellidos_Name.WidthF, Lable_F_NombresApellidos_Name.LocationF.Y);
            Lable_F_NombresApellidos_Value.SizeF = Lable_F_NombresApellidos_Name.SizeF;
            Lable_F_NombresApellidos_Value.ForeColor = Color.Black;
            Lable_F_NombresApellidos_Value.BackColor = Color.White;
            Lable_F_NombresApellidos_Value.Font = new Font("Arial", 6, FontStyle.Bold);
            ///// Data
            Lable_F_NombresApellidos_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Lable_F_NombresApellidos_Value.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            Lable_F_NombresApellidos_Value.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            formularioDetail.Controls.Add(Lable_F_NombresApellidos_Value);

            XRTable Table_F_Ident = new XRTable();
            Table_F_Ident.LocationF = new PointF(Lable_F_NombresApellidos_Name.LocationF.X, Lable_F_NombresApellidos_Name.LocationF.Y + Lable_F_NombresApellidos_Name.HeightF + 2);
            Table_F_Ident.SizeF = new SizeF(formularioWidth / 4 - 10, 15);
            XRTableRow Table_F_Ident_Row = new XRTableRow();
            XRTableCell Table_F_Ident_Cell;
            for (int i = 1; i < 4; i++)
            {
                Table_F_Ident_Cell = new XRTableCell()
                {
                    WidthF = Table_F_Ident.WidthF / 3 - 18,
                    Font = new Font("Arial", 6),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 3, 0, 0)
                };
                switch (i)
                {
                    case 1:
                        Table_F_Ident_Cell.Text = "CC";
                        break;
                    case 2:
                        Table_F_Ident_Cell.Text = "CE";
                        break;
                    case 3:
                        Table_F_Ident_Cell.Text = "TI";
                        break;
                };
                Table_F_Ident_Row.Cells.Add(Table_F_Ident_Cell);

                Table_F_Ident_Cell = new XRTableCell()
                {
                    WidthF = 18,
                    Borders = DevExpress.XtraPrinting.BorderSide.All,
                };
                Table_F_Ident_Row.Cells.Add(Table_F_Ident_Cell);
            };
            Table_F_Ident.Rows.Add(Table_F_Ident_Row);
            formularioDetail.Controls.Add(Table_F_Ident);

            XRLabel Lable_F_IdentNro = new XRLabel();
            Lable_F_IdentNro.LocationF = new PointF(Lable_F_NombresApellidos_Value.LocationF.X, Lable_F_NombresApellidos_Value.LocationF.Y + Lable_F_NombresApellidos_Value.HeightF);
            Lable_F_IdentNro.SizeF = new System.Drawing.SizeF(Lable_F_NombresApellidos_Value.WidthF, 20);
            Lable_F_IdentNro.ForeColor = Color.Black;
            Lable_F_IdentNro.BackColor = Color.White;
            Lable_F_IdentNro.Font = new Font("Arial", 6, FontStyle.Bold);
            ///// Data
            Lable_F_IdentNro.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Lable_F_IdentNro.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            Lable_F_IdentNro.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            formularioDetail.Controls.Add(Lable_F_IdentNro);

            XRPanel Panel_F_Firma = new XRPanel();
            Panel_F_Firma.LocationF = new PointF(Table_F_Ident.LocationF.X, Lable_F_IdentNro.LocationF.Y + Lable_F_IdentNro.HeightF);
            Panel_F_Firma.SizeF = new SizeF(formularioWidth / 6, 40);
            Panel_F_Firma.BackColor = Color.Transparent;
            Panel_F_Firma.Borders = DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom | DevExpress.XtraPrinting.BorderSide.Right;
            Panel_F_Firma.AnchorVertical = VerticalAnchorStyles.Top;
            formularioDetail.Controls.Add(Panel_F_Firma);

            XRLabel Lable_F_FirmaContador = new XRLabel();
            Lable_F_FirmaContador.LocationF = Panel_F_Firma.LocationF;
            Lable_F_FirmaContador.SizeF = new SizeF(Panel_F_Firma.WidthF - 20, 20);
            Lable_F_FirmaContador.ForeColor = Color.Black;
            Lable_F_FirmaContador.BackColor = Color.White;
            Lable_F_FirmaContador.Font = new Font("Arial", 6, FontStyle.Bold);
            Lable_F_FirmaContador.Text = "FIRMA DE CONTADOR";
            Lable_F_FirmaContador.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Lable_F_FirmaContador.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0);
            formularioDetail.Controls.Add(Lable_F_FirmaContador);

            XRLabel Lable_F_FirmaContador_blank = new XRLabel();
            Lable_F_FirmaContador_blank.LocationF = new PointF(Lable_F_FirmaContador.LocationF.X + Lable_F_FirmaContador.WidthF, Lable_F_FirmaContador.LocationF.Y + 2);
            Lable_F_FirmaContador_blank.SizeF = new SizeF(15, 15);
            Lable_F_FirmaContador_blank.Borders = DevExpress.XtraPrinting.BorderSide.All;
            formularioDetail.Controls.Add(Lable_F_FirmaContador_blank);

            XRLabel Lable_F_FirmaRevisor = new XRLabel();
            Lable_F_FirmaRevisor.LocationF = new PointF(Lable_F_FirmaContador.LocationF.X, Lable_F_FirmaContador.LocationF.Y + Lable_F_FirmaContador.HeightF);
            Lable_F_FirmaRevisor.SizeF = Lable_F_FirmaContador.SizeF;
            Lable_F_FirmaRevisor.ForeColor = Color.Black;
            Lable_F_FirmaRevisor.BackColor = Color.White;
            Lable_F_FirmaRevisor.Font = new Font("Arial", 6, FontStyle.Bold);
            Lable_F_FirmaRevisor.Text = "O REVISOR FISCAL";
            Lable_F_FirmaRevisor.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Lable_F_FirmaRevisor.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0);
            formularioDetail.Controls.Add(Lable_F_FirmaRevisor);

            XRLabel Lable_F_FirmaRevisor_blank = new XRLabel();
            Lable_F_FirmaRevisor_blank.LocationF = new PointF(Lable_F_FirmaRevisor.LocationF.X + Lable_F_FirmaRevisor.WidthF, Lable_F_FirmaRevisor.LocationF.Y + 2);
            Lable_F_FirmaRevisor_blank.SizeF = new SizeF(15, 15);
            Lable_F_FirmaRevisor_blank.Borders = DevExpress.XtraPrinting.BorderSide.All;
            formularioDetail.Controls.Add(Lable_F_FirmaRevisor_blank);

            XRLabel Lable_F_Firma = new XRLabel();
            Lable_F_Firma.LocationF = new PointF(Panel_F_Firma.LocationF.X + Panel_F_Firma.WidthF, Panel_F_Firma.LocationF.Y);
            Lable_F_Firma.SizeF = new SizeF(2 * formularioWidth / 6, 40);
            Lable_F_Firma.ForeColor = Color.Black;
            Lable_F_Firma.BackColor = Color.White;
            Lable_F_Firma.Font = new Font("Arial", 6, FontStyle.Bold);
            Lable_F_Firma.Text = "FIRMA";
            Lable_F_Firma.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            Lable_F_Firma.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 3, 0);
            Lable_F_Firma.Borders = DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom;
            formularioDetail.Controls.Add(Lable_F_Firma);

            XRLabel Lable_F_NombresApellidos1_Name = new XRLabel();
            Lable_F_NombresApellidos1_Name.LocationF = new PointF(Panel_F_Firma.LocationF.X, Panel_F_Firma.LocationF.Y + Panel_F_Firma.HeightF);
            Lable_F_NombresApellidos1_Name.SizeF = new SizeF(formularioWidth / 6, rowHeight);
            Lable_F_NombresApellidos1_Name.ForeColor = Color.Black;
            Lable_F_NombresApellidos1_Name.BackColor = Color.White;
            Lable_F_NombresApellidos1_Name.Font = new Font("Arial", 6, FontStyle.Bold);
            Lable_F_NombresApellidos1_Name.Text = "NOMBRES Y APELLIDOS";
            Lable_F_NombresApellidos1_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Lable_F_NombresApellidos1_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            Lable_F_NombresApellidos1_Name.Borders = DevExpress.XtraPrinting.BorderSide.Bottom | DevExpress.XtraPrinting.BorderSide.Right;
            formularioDetail.Controls.Add(Lable_F_NombresApellidos1_Name);

            XRLabel Lable_F_NombresApellidos1_Value = new XRLabel();
            Lable_F_NombresApellidos1_Value.LocationF = new PointF(Lable_F_NombresApellidos1_Name.LocationF.X + Lable_F_NombresApellidos1_Name.WidthF, Lable_F_NombresApellidos1_Name.LocationF.Y);
            Lable_F_NombresApellidos1_Value.SizeF = new SizeF(2 * formularioWidth / 6, rowHeight);
            Lable_F_NombresApellidos1_Value.ForeColor = Color.Black;
            Lable_F_NombresApellidos1_Value.BackColor = Color.White;
            Lable_F_NombresApellidos1_Value.Font = new Font("Arial", 6, FontStyle.Bold);
            ///// Data
            Lable_F_NombresApellidos1_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Lable_F_NombresApellidos1_Value.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            Lable_F_NombresApellidos1_Value.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            formularioDetail.Controls.Add(Lable_F_NombresApellidos1_Value);

            XRTable Table_F_Ident1 = new XRTable();
            Table_F_Ident1.LocationF = new PointF(Lable_F_NombresApellidos1_Name.LocationF.X, Lable_F_NombresApellidos1_Name.LocationF.Y + Lable_F_NombresApellidos1_Name.HeightF + 2);
            Table_F_Ident1.SizeF = new SizeF(formularioWidth / 6 - 10, 15);
            XRTableRow Table_F_Ident1_Row = new XRTableRow();
            XRTableCell Table_F_Ident1_Cell;
            for (int i = 1; i < 3; i++)
            {
                Table_F_Ident1_Cell = new XRTableCell()
                {
                    WidthF = Table_F_Ident1.WidthF / 2 - 18,
                    Font = new Font("Arial", 6),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 3, 0, 0)
                };
                switch (i)
                {
                    case 1:
                        Table_F_Ident1_Cell.Text = "CC";
                        break;
                    case 2:
                        Table_F_Ident1_Cell.Text = "CE";
                        break;
                };
                Table_F_Ident1_Row.Cells.Add(Table_F_Ident1_Cell);

                Table_F_Ident1_Cell = new XRTableCell()
                {
                    WidthF = 18,
                    Borders = DevExpress.XtraPrinting.BorderSide.All,
                };
                Table_F_Ident1_Row.Cells.Add(Table_F_Ident1_Cell);
            };
            Table_F_Ident1.Rows.Add(Table_F_Ident1_Row);
            formularioDetail.Controls.Add(Table_F_Ident1);

            XRLabel Lable_F_IdentNro1 = new XRLabel();
            Lable_F_IdentNro1.LocationF = new PointF(Lable_F_NombresApellidos1_Value.LocationF.X - 1, Lable_F_NombresApellidos1_Value.LocationF.Y + Lable_F_NombresApellidos1_Value.HeightF);
            Lable_F_IdentNro1.SizeF = new SizeF(2 * formularioWidth / 6 + 1, 20);
            Lable_F_IdentNro1.ForeColor = Color.Black;
            Lable_F_IdentNro1.BackColor = Color.White;
            Lable_F_IdentNro1.Font = new Font("Arial", 6, FontStyle.Bold);
            ///// Data
            Lable_F_IdentNro1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Lable_F_IdentNro1.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            Lable_F_IdentNro1.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            formularioDetail.Controls.Add(Lable_F_IdentNro1);

            XRLabel Lable_F_Tarjeta = new XRLabel();
            Lable_F_Tarjeta.LocationF = new PointF(Table_F_Ident1.LocationF.X, Lable_F_IdentNro1.LocationF.Y + Lable_F_IdentNro1.HeightF);
            Lable_F_Tarjeta.SizeF = new SizeF(formularioWidth / 2, rowHeight);
            Lable_F_Tarjeta.ForeColor = Color.Black;
            Lable_F_Tarjeta.BackColor = Color.White;
            Lable_F_Tarjeta.Font = new Font("Arial", 6, FontStyle.Bold);
            Lable_F_Tarjeta.Text = "TARJETA PROFESIONAL";
            Lable_F_Tarjeta.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Lable_F_Tarjeta.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            Lable_F_Tarjeta.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            formularioDetail.Controls.Add(Lable_F_Tarjeta);
            #endregion

            #region Left part
            XRPanel Panel_F_Autoadhesivo = new XRPanel();
            Panel_F_Autoadhesivo.LocationF = new PointF(formularioWidth / 2, Panel_F.LocationF.Y + Lable_F.HeightF);
            Panel_F_Autoadhesivo.SizeF = new SizeF(formularioWidth / 2, 90);
            Panel_F_Autoadhesivo.BackColor = Color.Transparent;
            Panel_F_Autoadhesivo.Borders = DevExpress.XtraPrinting.BorderSide.Bottom | DevExpress.XtraPrinting.BorderSide.Left;
            Panel_F_Autoadhesivo.AnchorVertical = VerticalAnchorStyles.Top;
            formularioDetail.Controls.Add(Panel_F_Autoadhesivo);

            XRLabel Lable_F_Autoadhesivo = new XRLabel();
            Lable_F_Autoadhesivo.LocationF = Panel_F_Autoadhesivo.LocationF;
            Lable_F_Autoadhesivo.SizeF = new SizeF(25, Panel_F_Autoadhesivo.HeightF);
            Lable_F_Autoadhesivo.ForeColor = Color.Black;
            Lable_F_Autoadhesivo.BackColor = Color.White;
            Lable_F_Autoadhesivo.Font = new Font("Arial", 6, FontStyle.Bold);
            Lable_F_Autoadhesivo.Text = "AUTOADHESIVO";
            Lable_F_Autoadhesivo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            Lable_F_Autoadhesivo.Angle = 90;
            formularioDetail.Controls.Add(Lable_F_Autoadhesivo);

            //XRLabel Lable_F_Espacio = new XRLabel();
            //Lable_F_Espacio.LocationF = new PointF(Lable_F_Autoadhesivo.LocationF.X + Lable_F_Autoadhesivo.WidthF, Lable_F_Autoadhesivo.LocationF.Y);
            //Lable_F_Espacio.SizeF = new SizeF(Panel_F_Autoadhesivo.WidthF - 25, 25);
            //Lable_F_Espacio.ForeColor = Color.Black;
            //Lable_F_Espacio.BackColor = Color.White;
            //Lable_F_Espacio.Font = new Font("Arial", 6, FontStyle.Bold);
            //Lable_F_Espacio.Text = "ESPACIO RESERVADO PARA LA ENTIDAD RECAUDADORA";
            //Lable_F_Espacio.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            //formularioDetail.Controls.Add(Lable_F_Espacio);

            XRPanel Panel_F_Sello = new XRPanel();
            Panel_F_Sello.LocationF = new PointF(Panel_F_Autoadhesivo.LocationF.X, Panel_F_Autoadhesivo.LocationF.Y + Panel_F_Autoadhesivo.HeightF);
            Panel_F_Sello.SizeF = new SizeF(formularioWidth / 2, 85);
            Panel_F_Sello.BackColor = Color.Transparent;
            Panel_F_Sello.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            Panel_F_Sello.AnchorVertical = VerticalAnchorStyles.Top;
            formularioDetail.Controls.Add(Panel_F_Sello);

            XRLabel Label_F_Sello = new XRLabel();
            Label_F_Sello.LocationF = Panel_F_Sello.LocationF;
            Label_F_Sello.SizeF = new SizeF(25, Panel_F_Sello.HeightF);
            Label_F_Sello.ForeColor = Color.Black;
            Label_F_Sello.BackColor = Color.White;
            Label_F_Sello.Font = new Font("Arial", 6, FontStyle.Bold);
            Label_F_Sello.Text = "SELLO O TIMBRE";
            Label_F_Sello.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            Label_F_Sello.Angle = 90;
            formularioDetail.Controls.Add(Label_F_Sello);

            //XRLabel Lable_F_Espacio1 = new XRLabel();
            //Lable_F_Espacio1.LocationF = new PointF(Label_F_Sello.LocationF.X + Label_F_Sello.WidthF, Label_F_Sello.LocationF.Y);
            //Lable_F_Espacio1.SizeF = new SizeF(Panel_F_Sello.WidthF - 25, 25);
            //Lable_F_Espacio1.ForeColor = Color.Black;
            //Lable_F_Espacio1.BackColor = Color.White;
            //Lable_F_Espacio1.Font = new Font("Arial", 6, FontStyle.Bold);
            //Lable_F_Espacio1.Text = "ESPACIO RESERVADO PARA LA ENTIDAD RECAUDADORA";
            //Lable_F_Espacio1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            //formularioDetail.Controls.Add(Lable_F_Espacio1);
            #endregion
            #endregion
        }
        
        #endregion
    }
}
