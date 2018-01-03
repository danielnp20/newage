using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace NewAge.Cliente.GUI.WinApp.Reports.Formularios
{
    public partial class FormatoRadDev : DevExpress.XtraReports.UI.XtraReport
    {
        #region Funciones Publicas
        /// <summary>
        /// Formulario "DEVOLUCIÓN FACTURAS PROVEEDORES" Constructor
        /// </summary>
        public FormatoRadDev()
        {
            #region Formato bands
            InitializeComponent();

            this.Margins = new System.Drawing.Printing.Margins(50, 50, 50, 50);

            DetailReportBand formatoBand;
            formatoBand = new DetailReportBand();

            DetailBand formatoDetail;
            formatoDetail = new DetailBand();
            formatoDetail.Name = "formularioDetail";
            formatoDetail.HeightF = this.PageHeight - this.Margins.Top - this.Margins.Bottom;
            formatoBand.Bands.Add(formatoDetail);

            this.Bands.Add(formatoBand);
            #endregion

            #region Formato styles

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

            float formatoWidth = this.PageWidth - this.Margins.Left - this.Margins.Right - 1;
            float rowHeight = 20;

            XRPanel formatoPanel = new XRPanel();
            formatoPanel.LocationF = new PointF(0, 0);
            formatoPanel.SizeF = new SizeF(formatoWidth, 920);
            formatoPanel.BackColor = Color.Transparent;
            formatoPanel.Borders = DevExpress.XtraPrinting.BorderSide.All;
            formatoPanel.BorderWidth = 3;
            formatoPanel.AnchorVertical = VerticalAnchorStyles.Top;
            formatoDetail.Controls.Add(formatoPanel);

            #region Part title
            XRPanel titlePanel = new XRPanel();
            titlePanel.LocationF = new PointF(0, 0);
            titlePanel.SizeF = new SizeF(formatoWidth, 160);
            titlePanel.BackColor = Color.Transparent;
            titlePanel.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            titlePanel.AnchorVertical = VerticalAnchorStyles.Top;
            formatoDetail.Controls.Add(titlePanel);

            XRLabel Label_EmpresaNombre = new XRLabel();
            Label_EmpresaNombre.LocationF = new PointF(formatoPanel.LocationF.X, formatoPanel.LocationF.Y + 25);
            Label_EmpresaNombre.SizeF = new SizeF(formatoWidth, 30);
            Label_EmpresaNombre.ForeColor = Color.Black;
            Label_EmpresaNombre.BackColor = Color.Transparent;
            Label_EmpresaNombre.Font = new Font("Arial", 14, FontStyle.Bold);
            Label_EmpresaNombre.Text = "NOMBRE DE LA EMPRESA";  ///////////  Data 
            Label_EmpresaNombre.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            formatoDetail.Controls.Add(Label_EmpresaNombre);

            XRLabel Label_EmpresaNIT = new XRLabel();
            Label_EmpresaNIT.LocationF = new PointF(Label_EmpresaNombre.LocationF.X, Label_EmpresaNombre.LocationF.Y + Label_EmpresaNombre.HeightF);
            Label_EmpresaNIT.SizeF = new SizeF(formatoWidth, 25);
            Label_EmpresaNIT.ForeColor = Color.Black;
            Label_EmpresaNIT.BackColor = Color.Transparent;
            Label_EmpresaNIT.Font = new Font("Arial", 12, FontStyle.Bold);
            Label_EmpresaNIT.Text = "NTI:  ";
            Label_EmpresaNIT.Text += "111111111111";  ///////////  Data
            Label_EmpresaNIT.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            formatoDetail.Controls.Add(Label_EmpresaNIT);

            XRLabel Label_FormatoNombre = new XRLabel();
            Label_FormatoNombre.LocationF = new PointF(Label_EmpresaNIT.LocationF.X, Label_EmpresaNIT.LocationF.Y + Label_EmpresaNIT.HeightF + 15);
            Label_FormatoNombre.SizeF = new SizeF(formatoWidth, 40);
            Label_FormatoNombre.ForeColor = Color.Black;
            Label_FormatoNombre.BackColor = Color.Transparent;
            Label_FormatoNombre.Font = new Font("Arial", 18, FontStyle.Bold);
            Label_FormatoNombre.Text = "DEVOLUCIÓN FACTURAS PROVEEDORES";
            Label_FormatoNombre.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            formatoDetail.Controls.Add(Label_FormatoNombre);
            #endregion

            #region Part Proveedor
            XRPanel proveedorPanel = new XRPanel();
            proveedorPanel.LocationF = new PointF(titlePanel.LocationF.X, titlePanel.LocationF.Y + titlePanel.HeightF);
            proveedorPanel.SizeF = new SizeF(formatoWidth, 95);
            proveedorPanel.BackColor = Color.Transparent;
            proveedorPanel.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            proveedorPanel.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
            proveedorPanel.AnchorVertical = VerticalAnchorStyles.Top;
            formatoDetail.Controls.Add(proveedorPanel);

            XRLabel Label_Proveedor = new XRLabel();
            Label_Proveedor.LocationF = new PointF(proveedorPanel.LocationF.X, proveedorPanel.LocationF.Y + 5);
            Label_Proveedor.SizeF = new SizeF(formatoWidth, 30);
            Label_Proveedor.ForeColor = Color.Black;
            Label_Proveedor.BackColor = Color.Transparent;
            Label_Proveedor.Font = new Font("Arial", 12, FontStyle.Bold | FontStyle.Underline);
            Label_Proveedor.Text = "PROVEEDOR: ";
            Label_Proveedor.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Label_Proveedor.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
            formatoDetail.Controls.Add(Label_Proveedor);

            XRLabel Label_NIT_Name = new XRLabel();
            Label_NIT_Name.LocationF = new PointF(Label_Proveedor.LocationF.X, Label_Proveedor.LocationF.Y + Label_Proveedor.HeightF + 5);
            Label_NIT_Name.SizeF = new SizeF(formatoWidth / 6, 25);
            Label_NIT_Name.ForeColor = Color.Black;
            Label_NIT_Name.BackColor = Color.Transparent;
            Label_NIT_Name.Font = new Font("Arial", 12, FontStyle.Bold);
            Label_NIT_Name.Text = "Nit: ";
            Label_NIT_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Label_NIT_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
            formatoDetail.Controls.Add(Label_NIT_Name);

            XRLabel Label_NIT_Value = new XRLabel();
            Label_NIT_Value.LocationF = new PointF(Label_NIT_Name.LocationF.X + Label_NIT_Name.WidthF, Label_NIT_Name.LocationF.Y);
            Label_NIT_Value.SizeF = new SizeF(5 * formatoWidth / 6, 25);
            Label_NIT_Value.ForeColor = Color.Black;
            Label_NIT_Value.BackColor = Color.Transparent;
            Label_NIT_Value.Font = new Font("Arial", 12);
            Label_NIT_Value.Text = "11111111";///////// Data
            Label_NIT_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Label_NIT_Value.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            formatoDetail.Controls.Add(Label_NIT_Value);

            XRLabel Label_Nombre_Name = new XRLabel();
            Label_Nombre_Name.LocationF = new PointF(Label_NIT_Name.LocationF.X, Label_NIT_Name.LocationF.Y + Label_NIT_Name.HeightF);
            Label_Nombre_Name.SizeF = Label_NIT_Name.SizeF;
            Label_Nombre_Name.ForeColor = Color.Black;
            Label_Nombre_Name.BackColor = Color.Transparent;
            Label_Nombre_Name.Font = new Font("Arial", 12, FontStyle.Bold);
            Label_Nombre_Name.Text = "Nombre: ";
            Label_Nombre_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Label_Nombre_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
            formatoDetail.Controls.Add(Label_Nombre_Name);

            XRLabel Label_Nombre_Value = new XRLabel();
            Label_Nombre_Value.LocationF = new PointF(Label_Nombre_Name.LocationF.X + Label_Nombre_Name.WidthF, Label_Nombre_Name.LocationF.Y);
            Label_Nombre_Value.SizeF = Label_NIT_Value.SizeF;
            Label_Nombre_Value.ForeColor = Color.Black;
            Label_Nombre_Value.BackColor = Color.Transparent;
            Label_Nombre_Value.Font = new Font("Arial", 12);
            Label_Nombre_Value.Text = "NOMBRE"; ///////// Data
            Label_Nombre_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Label_Nombre_Value.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            formatoDetail.Controls.Add(Label_Nombre_Value);
            #endregion

            #region Part Factura
            XRPanel facturaPanel = new XRPanel();
            facturaPanel.LocationF = new PointF(proveedorPanel.LocationF.X, proveedorPanel.LocationF.Y + proveedorPanel.HeightF);
            facturaPanel.SizeF = new SizeF(formatoWidth / 2, 120);
            facturaPanel.BackColor = Color.Transparent;
            facturaPanel.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            facturaPanel.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
            facturaPanel.AnchorVertical = VerticalAnchorStyles.Top;
            formatoDetail.Controls.Add(facturaPanel);

            XRLabel Label_Factura = new XRLabel();
            Label_Factura.LocationF = new PointF(facturaPanel.LocationF.X, facturaPanel.LocationF.Y + 5);
            Label_Factura.SizeF = new SizeF(formatoWidth / 2, 30);
            Label_Factura.ForeColor = Color.Black;
            Label_Factura.BackColor = Color.Transparent;
            Label_Factura.Font = new Font("Arial", 12, FontStyle.Bold | FontStyle.Underline);
            Label_Factura.Text = "FACTURA: ";
            Label_Factura.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Label_Factura.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
            formatoDetail.Controls.Add(Label_Factura);

            XRLabel Label_FacturaNumero_Name = new XRLabel();
            Label_FacturaNumero_Name.LocationF = new PointF(Label_Factura.LocationF.X, Label_Factura.LocationF.Y + Label_Factura.HeightF + 5);
            Label_FacturaNumero_Name.SizeF = new SizeF(formatoWidth / 6, 25);
            Label_FacturaNumero_Name.ForeColor = Color.Black;
            Label_FacturaNumero_Name.BackColor = Color.Transparent;
            Label_FacturaNumero_Name.Font = new Font("Arial", 12, FontStyle.Bold);
            Label_FacturaNumero_Name.Text = "Numero: ";
            Label_FacturaNumero_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Label_FacturaNumero_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
            formatoDetail.Controls.Add(Label_FacturaNumero_Name);

            XRLabel Label_FacturaNumero_Value = new XRLabel();
            Label_FacturaNumero_Value.LocationF = new PointF(Label_FacturaNumero_Name.LocationF.X + Label_FacturaNumero_Name.WidthF, Label_FacturaNumero_Name.LocationF.Y);
            Label_FacturaNumero_Value.SizeF = new SizeF(2 * formatoWidth / 6, 25);
            Label_FacturaNumero_Value.ForeColor = Color.Black;
            Label_FacturaNumero_Value.BackColor = Color.Transparent;
            Label_FacturaNumero_Value.Font = new Font("Arial", 12);
            Label_FacturaNumero_Value.Text = "11111111";///////// Data
            Label_FacturaNumero_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            Label_FacturaNumero_Value.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 50, 0, 0);
            formatoDetail.Controls.Add(Label_FacturaNumero_Value);

            XRLabel Label_FacturaFecha_Name = new XRLabel();
            Label_FacturaFecha_Name.LocationF = new PointF(Label_FacturaNumero_Name.LocationF.X, Label_FacturaNumero_Name.LocationF.Y + Label_FacturaNumero_Name.HeightF);
            Label_FacturaFecha_Name.SizeF = Label_FacturaNumero_Name.SizeF;
            Label_FacturaFecha_Name.ForeColor = Color.Black;
            Label_FacturaFecha_Name.BackColor = Color.Transparent;
            Label_FacturaFecha_Name.Font = new Font("Arial", 12, FontStyle.Bold);
            Label_FacturaFecha_Name.Text = "Fecha: ";
            Label_FacturaFecha_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Label_FacturaFecha_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
            formatoDetail.Controls.Add(Label_FacturaFecha_Name);

            XRLabel Label_FacturaFecha_Value = new XRLabel();
            Label_FacturaFecha_Value.LocationF = new PointF(Label_FacturaFecha_Name.LocationF.X + Label_FacturaFecha_Name.WidthF, Label_FacturaFecha_Name.LocationF.Y);
            Label_FacturaFecha_Value.SizeF = Label_FacturaNumero_Value.SizeF;
            Label_FacturaFecha_Value.ForeColor = Color.Black;
            Label_FacturaFecha_Value.BackColor = Color.Transparent;
            Label_FacturaFecha_Value.Font = new Font("Arial", 12);
            Label_FacturaFecha_Value.Text = "01/01/2012"; ///////// Data
            Label_FacturaFecha_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            Label_FacturaFecha_Value.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 50, 0, 0);
            formatoDetail.Controls.Add(Label_FacturaFecha_Value);

            XRLabel Label_Valor_Name = new XRLabel();
            Label_Valor_Name.LocationF = new PointF(Label_FacturaFecha_Name.LocationF.X, Label_FacturaFecha_Name.LocationF.Y + Label_FacturaFecha_Name.HeightF);
            Label_Valor_Name.SizeF = Label_FacturaFecha_Name.SizeF;
            Label_Valor_Name.ForeColor = Color.Black;
            Label_Valor_Name.BackColor = Color.Transparent;
            Label_Valor_Name.Font = new Font("Arial", 12, FontStyle.Bold);
            Label_Valor_Name.Text = "Valor: ";
            Label_Valor_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Label_Valor_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
            formatoDetail.Controls.Add(Label_Valor_Name);

            XRLabel Label_Valor_Value = new XRLabel();
            Label_Valor_Value.LocationF = new PointF(Label_Valor_Name.LocationF.X + Label_Valor_Name.WidthF, Label_Valor_Name.LocationF.Y);
            Label_Valor_Value.SizeF = Label_FacturaFecha_Value.SizeF;
            Label_Valor_Value.ForeColor = Color.Black;
            Label_Valor_Value.BackColor = Color.Transparent;
            Label_Valor_Value.Font = new Font("Arial", 12);
            Label_Valor_Value.Text = "01/01/2012"; ///////// Data
            Label_Valor_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            Label_Valor_Value.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 50, 0, 0);
            formatoDetail.Controls.Add(Label_Valor_Value);
            #endregion

            #region Part Redicado
            XRPanel redicadoPanel = new XRPanel();
            redicadoPanel.LocationF = new PointF(facturaPanel.LocationF.X + facturaPanel.WidthF, facturaPanel.LocationF.Y);
            redicadoPanel.SizeF = new SizeF(formatoWidth / 2, 120);
            redicadoPanel.BackColor = Color.Transparent;
            redicadoPanel.Borders = DevExpress.XtraPrinting.BorderSide.Bottom | DevExpress.XtraPrinting.BorderSide.Left;
            redicadoPanel.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
            redicadoPanel.AnchorVertical = VerticalAnchorStyles.Top;
            formatoDetail.Controls.Add(redicadoPanel);

            XRLabel Label_Redicado = new XRLabel();
            Label_Redicado.LocationF = new PointF(redicadoPanel.Location.X, redicadoPanel.LocationF.Y + 17);
            Label_Redicado.SizeF = new SizeF(formatoWidth / 2, 30);
            Label_Redicado.ForeColor = Color.Black;
            Label_Redicado.BackColor = Color.Transparent;
            Label_Redicado.Font = new Font("Arial", 12, FontStyle.Bold | FontStyle.Underline);
            Label_Redicado.Text = "REDICADO: ";
            Label_Redicado.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Label_Redicado.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
            formatoDetail.Controls.Add(Label_Redicado);

            XRLabel Label_RedicadoNumero_Name = new XRLabel();
            Label_RedicadoNumero_Name.LocationF = new PointF(Label_Redicado.LocationF.X, Label_Redicado.LocationF.Y + Label_Redicado.HeightF + 5);
            Label_RedicadoNumero_Name.SizeF = new SizeF(formatoWidth / 6, 25);
            Label_RedicadoNumero_Name.ForeColor = Color.Black;
            Label_RedicadoNumero_Name.BackColor = Color.Transparent;
            Label_RedicadoNumero_Name.Font = new Font("Arial", 12, FontStyle.Bold);
            Label_RedicadoNumero_Name.Text = "Numero: ";
            Label_RedicadoNumero_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Label_RedicadoNumero_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
            formatoDetail.Controls.Add(Label_RedicadoNumero_Name);

            XRLabel Label_RedicadoNumero_Value = new XRLabel();
            Label_RedicadoNumero_Value.LocationF = new PointF(Label_RedicadoNumero_Name.LocationF.X + Label_RedicadoNumero_Name.WidthF, Label_RedicadoNumero_Name.LocationF.Y);
            Label_RedicadoNumero_Value.SizeF = new SizeF(2 * formatoWidth / 6, 25);
            Label_RedicadoNumero_Value.ForeColor = Color.Black;
            Label_RedicadoNumero_Value.BackColor = Color.Transparent;
            Label_RedicadoNumero_Value.Font = new Font("Arial", 12);
            Label_RedicadoNumero_Value.Text = "22222222";///////// Data
            Label_RedicadoNumero_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            Label_RedicadoNumero_Value.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 50, 0, 0);
            formatoDetail.Controls.Add(Label_RedicadoNumero_Value);

            XRLabel Label_RedicadoFecha_Name = new XRLabel();
            Label_RedicadoFecha_Name.LocationF = new PointF(Label_RedicadoNumero_Name.LocationF.X, Label_RedicadoNumero_Name.LocationF.Y + Label_RedicadoNumero_Name.HeightF);
            Label_RedicadoFecha_Name.SizeF = Label_RedicadoNumero_Name.SizeF;
            Label_RedicadoFecha_Name.ForeColor = Color.Black;
            Label_RedicadoFecha_Name.BackColor = Color.Transparent;
            Label_RedicadoFecha_Name.Font = new Font("Arial", 12, FontStyle.Bold);
            Label_RedicadoFecha_Name.Text = "Fecha: ";
            Label_RedicadoFecha_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Label_RedicadoFecha_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
            formatoDetail.Controls.Add(Label_RedicadoFecha_Name);

            XRLabel Label_RedicadoFecha_Value = new XRLabel();
            Label_RedicadoFecha_Value.LocationF = new PointF(Label_RedicadoFecha_Name.LocationF.X + Label_RedicadoFecha_Name.WidthF, Label_RedicadoFecha_Name.LocationF.Y);
            Label_RedicadoFecha_Value.SizeF = Label_RedicadoNumero_Value.SizeF;
            Label_RedicadoFecha_Value.ForeColor = Color.Black;
            Label_RedicadoFecha_Value.BackColor = Color.Transparent;
            Label_RedicadoFecha_Value.Font = new Font("Arial", 12);
            Label_RedicadoFecha_Value.Text = "02/02/2012"; ///////// Data
            Label_RedicadoFecha_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            Label_RedicadoFecha_Value.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 50, 0, 0);
            formatoDetail.Controls.Add(Label_RedicadoFecha_Value);
            #endregion

            #region Part Motivo
            XRPanel motivoPanel = new XRPanel();
            motivoPanel.LocationF = new PointF(facturaPanel.LocationF.X, facturaPanel.LocationF.Y + facturaPanel.HeightF);
            motivoPanel.SizeF = new SizeF(formatoWidth, 405);
            motivoPanel.BackColor = Color.Transparent;
            motivoPanel.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            motivoPanel.AnchorVertical = VerticalAnchorStyles.Top;
            formatoDetail.Controls.Add(motivoPanel);

            XRLabel Label_Motivo = new XRLabel();
            Label_Motivo.LocationF = new PointF(motivoPanel.LocationF.X, motivoPanel.LocationF.Y + 5);
            Label_Motivo.SizeF = new SizeF(formatoWidth, 30);
            Label_Motivo.ForeColor = Color.Black;
            Label_Motivo.BackColor = Color.Transparent;
            Label_Motivo.Font = new Font("Arial", 12, FontStyle.Bold | FontStyle.Underline);
            Label_Motivo.Text = "MOTIVO DEVOLUCION: ";
            Label_Motivo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Label_Motivo.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
            formatoDetail.Controls.Add(Label_Motivo);

            XRTable Table_Motivo;
            XRTableRow Table_Motivo_Row;
            XRTableCell Table_MotivoMark_Cell;
            XRTableCell Table_MotivoDesc_Cell;
            Table_Motivo = new XRTable();
            Table_Motivo.BeginInit();
            Table_Motivo.LocationF = new PointF(Label_Motivo.LocationF.X, Label_Motivo.LocationF.Y + Label_Motivo.HeightF + 5);
            Table_Motivo.SizeF = new System.Drawing.SizeF(formatoWidth, 16 * rowHeight);
            Table_Motivo.StyleName = "tableStyle";

            for (int i = 1; i < 17; i++)
            {
                Table_Motivo_Row = new XRTableRow();
                Table_Motivo_Row.HeightF = rowHeight;

                Table_MotivoMark_Cell = new XRTableCell()
                {
                    WidthF = 30,
                    Borders = DevExpress.XtraPrinting.BorderSide.None
                };

                Table_MotivoDesc_Cell = new XRTableCell()
                {
                    WidthF = formatoWidth - 30,
                    Borders = DevExpress.XtraPrinting.BorderSide.None
                };

                switch (i)
                {
                    case 1:
                        Table_MotivoMark_Cell.Text = "";
                        Table_MotivoDesc_Cell.Text = "No está denominada expresamente con factura de venta.";
                        break;
                    case 2:
                        Table_MotivoMark_Cell.Text = "";
                        Table_MotivoDesc_Cell.Text = "Falta Nombre o Razón social del Proveedor.";
                        break;
                    case 3:
                        Table_MotivoMark_Cell.Text = "";
                        Table_MotivoDesc_Cell.Text = "Falta el número preimpreso de la factura";
                        break;
                    case 4:
                        Table_MotivoMark_Cell.Text = "";
                        Table_MotivoDesc_Cell.Text = "Falta fecha de expedición de la factura";
                        break;
                    case 5:
                        Table_MotivoMark_Cell.Text = "";
                        Table_MotivoDesc_Cell.Text = "Falta descripción espscifica de los articulos vendidios o servisios prestados.";
                        break;
                    case 6:
                        Table_MotivoMark_Cell.Text = "";
                        Table_MotivoDesc_Cell.Text = "Falta discriminación del IVA en renglón separado si es régimen común.";
                        break;
                    case 7:
                        Table_MotivoMark_Cell.Text = "";
                        Table_MotivoDesc_Cell.Text = "Presenta enmanedaduras o tachones.";
                        break;
                    case 8:
                        Table_MotivoMark_Cell.Text = "";
                        Table_MotivoDesc_Cell.Text = "Falta Nombre o Razón social y NIT del impresor de la factura.";
                        break;
                    case 9:
                        Table_MotivoMark_Cell.Text = "";
                        Table_MotivoDesc_Cell.Text = "Falta Indicación de la ciudad de retenedor del impuesto sobre las ventas.";
                        break;
                    case 10:
                        Table_MotivoMark_Cell.Text = "";
                        Table_MotivoDesc_Cell.Text = "Errores aritméticos.";
                        break;
                    case 11:
                        Table_MotivoMark_Cell.Text = "";
                        Table_MotivoDesc_Cell.Text = "Falta visto bueno de un funcionario de nuestra empresa.";
                        break;
                    case 12:
                        Table_MotivoMark_Cell.Text = "";
                        Table_MotivoDesc_Cell.Text = "No presenta suficientes soportes y/o estos no estan aprobados debidamente.";
                        break;
                    case 13:
                        Table_MotivoMark_Cell.Text = "";
                        Table_MotivoDesc_Cell.Text = "Presenta estemporaneidad para efectos de Retención en el Fuente.";
                        break;
                    case 14:
                        Table_MotivoMark_Cell.Text = "";
                        Table_MotivoDesc_Cell.Text = "No trae original de la Orden de Compra y/o servicios.";
                        break;
                    case 15:
                        Table_MotivoMark_Cell.Text = "";
                        Table_MotivoDesc_Cell.Text = "No indica la cantidad o el valor unitario.";
                        break;
                    case 16:
                        Table_MotivoMark_Cell.Text = "";
                        Table_MotivoDesc_Cell.Text = "Error en la tasa de cambio utilizada.";
                        break;
                };
                Table_Motivo_Row.Cells.Add(Table_MotivoMark_Cell);
                Table_Motivo_Row.Cells.Add(Table_MotivoDesc_Cell);

                Table_Motivo.Rows.Add(Table_Motivo_Row);
            }

            Table_Motivo.EndInit();
            formatoDetail.Controls.Add(Table_Motivo);

            XRLabel Label_Otros = new XRLabel();
            Label_Otros.LocationF = new PointF(Table_Motivo.LocationF.X, Table_Motivo.LocationF.Y + Table_Motivo.HeightF + 5);
            Label_Otros.SizeF = new SizeF(200, 20);
            Label_Otros.ForeColor = Color.Black;
            Label_Otros.BackColor = Color.Transparent;
            Label_Otros.Font = new Font("Arial", 7);
            Label_Otros.Text = "Otros: ";
            Label_Otros.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Label_Otros.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
            formatoDetail.Controls.Add(Label_Otros);

            XRLabel Label_REC = new XRLabel();
            Label_REC.LocationF = new PointF(Label_Otros.LocationF.X + Label_Otros.WidthF, Label_Otros.LocationF.Y);
            Label_REC.SizeF = new SizeF(formatoWidth - 200, 20);
            Label_REC.ForeColor = Color.Black;
            Label_REC.BackColor = Color.Transparent;
            Label_REC.Font = new Font("Arial", 7);
            Label_REC.Text = "REC: BOAD-2510";
            Label_REC.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            Label_REC.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 80, 0, 0);
            formatoDetail.Controls.Add(Label_REC);
            #endregion

            #region Part Footer
            XRLabel Label_Atentamente = new XRLabel();
            Label_Atentamente.LocationF = new PointF(motivoPanel.LocationF.X, motivoPanel.LocationF.Y + motivoPanel.HeightF + 15);
            Label_Atentamente.SizeF = new SizeF(formatoWidth, 30);
            Label_Atentamente.ForeColor = Color.Black;
            Label_Atentamente.BackColor = Color.Transparent;
            Label_Atentamente.Font = new Font("Arial", 8);
            Label_Atentamente.Text = "Atentamente: ";
            Label_Atentamente.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Label_Atentamente.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
            formatoDetail.Controls.Add(Label_Atentamente);

            XRLabel Label_Firma_blank = new XRLabel();
            Label_Firma_blank.LocationF = new PointF(Label_Atentamente.LocationF.X + 30, Label_Atentamente.LocationF.Y + Label_Atentamente.HeightF);
            Label_Firma_blank.SizeF = new SizeF(formatoWidth / 6, 50);
            Label_Firma_blank.ForeColor = Color.Black;
            Label_Firma_blank.BackColor = Color.Transparent;
            Label_Firma_blank.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            formatoDetail.Controls.Add(Label_Firma_blank);

            XRLabel Label_Footer_Bogota = new XRLabel();
            Label_Footer_Bogota.LocationF = new PointF(formatoPanel.LocationF.X, formatoPanel.LocationF.Y + formatoPanel.HeightF - 25);
            Label_Footer_Bogota.SizeF = new SizeF(formatoWidth / 6 - 20, 20);
            Label_Footer_Bogota.ForeColor = Color.Black;
            Label_Footer_Bogota.BackColor = Color.Transparent;
            Label_Footer_Bogota.Font = new Font("Arial", 8);
            Label_Footer_Bogota.Text = "Bogotá D.C. ,";
            Label_Footer_Bogota.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Label_Footer_Bogota.Padding = new DevExpress.XtraPrinting.PaddingInfo(30, 0, 0, 0);
            formatoDetail.Controls.Add(Label_Footer_Bogota);

            XRLabel Label_Footer_Date = new XRLabel();
            Label_Footer_Date.LocationF = new PointF(Label_Footer_Bogota.LocationF.X + Label_Footer_Bogota.WidthF, Label_Footer_Bogota.LocationF.Y);
            Label_Footer_Date.SizeF = new SizeF(formatoWidth / 6, 20);
            Label_Footer_Date.ForeColor = Color.Black;
            Label_Footer_Date.BackColor = Color.Transparent;
            Label_Footer_Date.Font = new Font("Arial", 8);
            Label_Footer_Date.Text = "27 Agosto 2012"; ///////// Data
            Label_Footer_Date.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            Label_Footer_Date.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 0, 0, 0);
            formatoDetail.Controls.Add(Label_Footer_Date);
            #endregion
        } 
        #endregion
    }
}

