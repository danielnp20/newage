using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Reportes;
using System.Collections;
using DevExpress.XtraReports.UI;
using System.Drawing;
using DevExpress.XtraPrinting.Shape;
using NewAge.DTO.GlobalConfig;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Cliente.GUI.WinApp.Reports;
using System.Drawing.Printing;

namespace NewAge.Cliente.GUI.WinApp.Reports.Documentos
{
    class FacturaEquivalente : BaseReport //XtraReport//
    {
        BaseController _bc = BaseController.GetInstance();

        /// <summary>
        /// Factura equivalente Constructor
        /// </summary>
        /// <param name="documentoData"> data for the document </param>
        /// <param name="headerFieldList"> list of fields for document header table (general information about document)</param>
        /// <param name="detailFieldList"> list of fields for document detail table (detailed information about document)</param>
        /// <param name="Date">period for reported documents</param>
        public FacturaEquivalente(List<DTO_FacturaEquivalente> documentoData, ArrayList headerFieldList, ArrayList detailFieldList, DateTime Date)  
        {  

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
                Borders = DevExpress.XtraPrinting.BorderSide.Top|DevExpress.XtraPrinting.BorderSide.Bottom,
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
            documentoTitle1.HeightF = 100 ;
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

            #region Table 1

            DetailReportBand documentoBand1;
            documentoBand1 = new DetailReportBand();
            documentoBand1.DataSource = documentoData;
            documentoBand1.DataMember = "FacturaHeader";
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
            documentoBand2.DataMember = "FacturaDetail";

            GroupHeaderBand documentoHeader2 = new GroupHeaderBand();
            documentoHeader2.HeightF = 60;
            documentoHeader2.Level = 1;
            documentoBand2.Bands.Add(documentoHeader2);

            GroupHeaderBand documentoTable2Header = new GroupHeaderBand();
            documentoTable2Header.HeightF = 35;
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

            tableWidth2 = 3 * tableWidth1/4;

            columnWidth2 = (tableWidth2 - 50) / 4; //fieldList.Count;
            #endregion
            #endregion

            #region Documento elements
            #region Table 1
            #region Documento title
            this.ReportHeader.HeightF = 0;

            documentoTitle1.Controls.Add(this.imgLogoEmpresa);
            documentoTitle1.Controls.Add(this.lblNombreEmpresa);
            documentoTitle1.Controls.Add(this.lblReportName);

            this.lblNombreEmpresa.LocationF = new PointF(100, 40);
            this.lblReportName.LocationF = new PointF(0,this.lblNombreEmpresa.LocationF.Y+this.lblNombreEmpresa.HeightF+20);
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

            XRLabel lblTitleResponsable = new XRLabel();
            lblTitleResponsable.LocationF = new PointF(0, this.lblReportName.LocationF.Y+this.lblReportName.HeightF);
            lblTitleResponsable.SizeF = new SizeF(tableWidth1, 20);
            lblTitleResponsable.Font = new Font("Arial", 10);
            lblTitleResponsable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            lblTitleResponsable.Padding = new DevExpress.XtraPrinting.PaddingInfo(100, 0, 0, 0);
            lblTitleResponsable.DataBindings.Add("Text", this.DataSource, "RegimenDesc");
            documentoTitle1.Controls.Add(lblTitleResponsable);

            XRLabel lblTitleDocumentoName = new XRLabel();
            lblTitleDocumentoName.LocationF = new PointF(0, 10);
            lblTitleDocumentoName.SizeF = new SizeF(tableWidth1, 40);
            lblTitleDocumentoName.Font = new Font("Arial", 14,FontStyle.Bold);
            lblTitleDocumentoName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            //lblTitleDocumentoName.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.FacturaEquivalente).ToString());
            lblTitleDocumentoName.Text = "DOCUMENTO EQUIEVALENTE A FACTURA (art.3 dec.522 de 2003)";
            documentoTitle.Controls.Add(lblTitleDocumentoName);
            #endregion

            #region Documento header
            XRLabel lblHeaderPersona_Name = new XRLabel();
            lblHeaderPersona_Name.LocationF = new PointF(0, 0);
            lblHeaderPersona_Name.SizeF = new SizeF(tableWidth1/3 - 80, 30);
            lblHeaderPersona_Name.Font = new Font("Arial", 9);
            lblHeaderPersona_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            lblHeaderPersona_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0);
            lblHeaderPersona_Name.Multiline = true;
            lblHeaderPersona_Name.Text = "Persona natural de quien se adquieren\r\nlos bienes y/o servicios:";
            documentoHeader.Controls.Add(lblHeaderPersona_Name);

            XRLabel lblHeaderPersona_Value = new XRLabel();
            lblHeaderPersona_Value.LocationF = new PointF(lblHeaderPersona_Name.LocationF.X+lblHeaderPersona_Name.WidthF, lblHeaderPersona_Name.LocationF.Y);
            lblHeaderPersona_Value.SizeF = new SizeF(tableWidth1 / 3 + 80, 30);
            lblHeaderPersona_Value.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            lblHeaderPersona_Value.Font = new Font("Arial", 9);
            lblHeaderPersona_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            lblHeaderPersona_Value.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            lblHeaderPersona_Value.DataBindings.Add("Text", this.DataSource, "TerceroDesc");
            documentoHeader.Controls.Add(lblHeaderPersona_Value);

            XRLabel lblHeaderNit_Name = new XRLabel();
            lblHeaderNit_Name.LocationF = new PointF(0, lblHeaderPersona_Name.LocationF.Y + lblHeaderPersona_Name.HeightF);
            lblHeaderNit_Name.SizeF = new SizeF(tableWidth1 / 3 - 80, 30);
            lblHeaderNit_Name.Font = new Font("Arial", 9);
            lblHeaderNit_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            lblHeaderNit_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0);
            //lblHeaderNit_Name.Text = this.lblReportName.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.FacturaEquivalente).ToString() + "_Nit");
            lblHeaderNit_Name.Text = "Nit ";
            documentoHeader.Controls.Add(lblHeaderNit_Name);

            XRLabel lblHeaderNit_Value = new XRLabel();
            lblHeaderNit_Value.LocationF = new PointF(lblHeaderNit_Name.LocationF.X + lblHeaderNit_Name.WidthF, lblHeaderNit_Name.LocationF.Y);
            lblHeaderNit_Value.SizeF = new SizeF(tableWidth1 / 3 + 80, 30);
            lblHeaderNit_Value.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            lblHeaderNit_Value.Font = new Font("Arial", 9);
            lblHeaderNit_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            lblHeaderNit_Value.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            lblHeaderNit_Value.DataBindings.Add("Text", this.DataSource, "TerceroNit");
            documentoHeader.Controls.Add(lblHeaderNit_Value);

            XRLabel lblHeaderFecha_Name = new XRLabel();
            lblHeaderFecha_Name.LocationF = new PointF(0, lblHeaderNit_Name.LocationF.Y + lblHeaderNit_Name.HeightF);
            lblHeaderFecha_Name.SizeF = new SizeF(tableWidth1 / 3 - 80, 30);
            lblHeaderFecha_Name.Font = new Font("Arial", 9);
            lblHeaderFecha_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            lblHeaderFecha_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0);
            //lblHeaderFecha_Name.Text = this.lblReportName.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.FacturaEquivalente).ToString() + "_FechaDeOperacion"); 
            lblHeaderFecha_Name.Text = "Fecha de la Operación";
            documentoHeader.Controls.Add(lblHeaderFecha_Name);

            XRLabel lblHeaderFecha_Value = new XRLabel();
            lblHeaderFecha_Value.LocationF = new PointF(lblHeaderFecha_Name.LocationF.X + lblHeaderFecha_Name.WidthF, lblHeaderFecha_Name.LocationF.Y);
            lblHeaderFecha_Value.SizeF = new SizeF(tableWidth1 / 3 + 80, 30);
            lblHeaderFecha_Value.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
            lblHeaderFecha_Value.Font = new Font("Arial", 9);
            lblHeaderFecha_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            lblHeaderFecha_Value.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            lblHeaderFecha_Value.DataBindings.Add("Text", this.DataSource, "Fecha", "{0:dd/MM/yyyy}");
            documentoHeader.Controls.Add(lblHeaderFecha_Value);

            XRLabel lblHeaderNo_Name = new XRLabel();
            lblHeaderNo_Name.LocationF = new PointF(lblHeaderFecha_Value.LocationF.X + lblHeaderFecha_Value.WidthF, lblHeaderFecha_Value.LocationF.Y);
            lblHeaderNo_Name.SizeF = new SizeF(tableWidth1 / 9, 30);
            lblHeaderNo_Name.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0);
            lblHeaderNo_Name.Font = new Font("Arial", 9);
            lblHeaderNo_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            //lblHeaderNo_Name.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.FacturaEquivalente).ToString() + "_No");
            lblHeaderNo_Name.Text = "No.";
            documentoHeader.Controls.Add(lblHeaderNo_Name);

            XRLabel lblHeaderNo_Value = new XRLabel();
            lblHeaderNo_Value.LocationF = new PointF(lblHeaderNo_Name.LocationF.X + lblHeaderNo_Name.WidthF, lblHeaderNo_Name.LocationF.Y);
            lblHeaderNo_Value.SizeF = new SizeF(2*tableWidth1 / 9 - 10, 30);
            lblHeaderNo_Value.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0);
            lblHeaderNo_Value.Font = new Font("Arial", 9,FontStyle.Bold);
            lblHeaderNo_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            lblHeaderNo_Value.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            lblHeaderNo_Value.DataBindings.Add("Text", this.DataSource, "FactEquivalente");
            documentoHeader.Controls.Add(lblHeaderNo_Value);
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
            table1Footer = new XRTable();
            table1Footer.LocationF = new PointF(0, 2);
            table1Footer.SizeF = new SizeF(tableWidth1, 30);
            table1Footer.StyleName = "groupFooterStyle";
            table1FooterRow = new XRTableRow();
            table1FooterRow.Name = "table1FooterRow";
            table1FooterRow.HeightF = 30;
            #endregion
            #endregion

            #region Table 2
            #region Documento header
            XRLabel lblHeaderContab = new XRLabel();
            lblHeaderContab.LocationF = new PointF(0, 20);
            lblHeaderContab.SizeF = new SizeF(tableWidth2, 30);
            lblHeaderContab.Font = new Font("Arial", 10,FontStyle.Bold);
            lblHeaderContab.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            lblHeaderContab.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0);
            lblHeaderContab.Multiline = true;
            lblHeaderContab.Text = "Contabilizaciones";
            documentoHeader2.Controls.Add(lblHeaderContab);           
            #endregion

            #region Documento Table header
            XRTable table2Header;
            XRTableRow table2HeaderRow;
            XRTableCell table2HeaderCell;
            table2Header = new XRTable();
            table2Header.LocationF = new PointF(0, 0);
            table2Header.WidthF = tableWidth2;
            table2Header.HeightF = documentoTable1Header.HeightF - 5;
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
            XRLine footerLine = new XRLine()
            {
                LineWidth = 1,
                ForeColor = Color.Gray,
                SizeF = new SizeF(tableWidth2,1),
                LocationF = new PointF(0,0)
            };
            documentoTable2Footer.Controls.Add(footerLine);
            #endregion
            #endregion
            #endregion

            #region Table 1
            foreach (string fieldName in headerFieldList)
            {
                #region Documento table header
                table1HeaderCell = new XRTableCell();
                table1HeaderCell.WidthF = (fieldName.Contains("Descr")) ? columnWidth1 + 200 : columnWidth1;
                table1HeaderCell.BorderColor = Color.White;
                //string resourceId = (AppReports.FacturaEquivalente).ToString() + "_" + fieldName;
                //string columnname = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, resourceId);
                //table1HeaderCell.Text = columnname;
                switch (fieldName)
                {
                    case "DocumentoDesc":
                        table1HeaderCell.Text = "Descripción de la Operación";
                        break;
                    case "ValorMisma":
                        table1HeaderCell.Text = "Valor de la misma";
                        break;
                    case "IvaTeorico":
                        table1HeaderCell.Text = "Ive teórico generado en la operación";
                        break;
                    case "ValorImp":
                        table1HeaderCell.Text = "Tarifa de retención de Iva vigente";
                        break;
                    case "TarifaRet":
                        table1HeaderCell.Text = "Valor del \"impuesto asumido\" (retención de Iva asumida)";
                        break;
                };
                table1HeaderRow.Controls.Add(table1HeaderCell);
                #endregion

                #region Documento table detail
                table1DetailCell = new XRTableCell();
                table1DetailCell.WidthF = table1HeaderCell.WidthF;

                switch (fieldName)
                {
                    case "DocumentoDesc":
                        table1DetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                        table1DetailCell.DataBindings.Add("Text", this.DataSource, "FacturaHeader." + fieldName);
                        break;
                    case "ValorMisma":
                    case "IvaTeorico":
                    case "ValorImp":
                        table1DetailCell.DataBindings.Add("Text", this.DataSource, "FacturaHeader." + fieldName, "{0:#,0.}");
                        break;
                    case "TarifaRet":
                        table1DetailCell.DataBindings.Add("Text", this.DataSource, "FacturaHeader." + fieldName, "{0:0.0%}");
                        break;                        
                };
                table1DetailRow.Controls.Add(table1DetailCell);
                #endregion

                #region Documento table footer
                table1FooterCell = new XRTableCell();
                table1FooterCell.WidthF = table1HeaderCell.WidthF;

                switch (fieldName)
                {
                    case "DocumentoDesc":
                        table1FooterCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                        table1FooterCell.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 100, 0, 0);
                        table1FooterCell.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (FormCodes.ReportForm).ToString() + "_Totals");
                        break;
                    case "ValorMisma":
                    case "IvaTeorico":
                    case "ValorImp":
                        table1FooterCell.Summary.Func = SummaryFunc.Sum;
                        table1FooterCell.Summary.Running = SummaryRunning.Report;
                        table1FooterCell.Summary.FormatString = "{0:#,0.}";
                        table1FooterCell.DataBindings.Add("Text", documentoBand.DataSource, "FacturaHeader." + fieldName);
                        break;
                    case "TarifaRet":
                        table1FooterCell.Summary.Func = SummaryFunc.Sum;
                        table1FooterCell.Summary.Running = SummaryRunning.Report;
                        table1FooterCell.Summary.FormatString = "{0:0.0%}";
                        table1FooterCell.DataBindings.Add("Text", documentoBand.DataSource, "FacturaHeader." + fieldName);
                        break;
                };
                table1FooterRow.Controls.Add(table1FooterCell);
                #endregion
            };
            table1Header.Controls.Add(table1HeaderRow);
            table1Detail.Controls.Add(table1DetailRow);
            table1Footer.Controls.Add(table1FooterRow);

            documentoTable1Header.Controls.Add(table1Header);
            documentoTable1Detail.Controls.Add(table1Detail);
            documentoTable1Footer.Controls.Add(table1Footer);
            #endregion

            #region Table 2
            foreach (string fieldName in detailFieldList)
            {
                #region Documento table header
                table2HeaderCell = new XRTableCell();
                table2HeaderCell.WidthF = (fieldName.Contains("Desc")) ? columnWidth2 + 50 : columnWidth2;
                table2HeaderCell.BorderColor = Color.White;
                //string resourceId = (AppReports.FacturaEquivalente).ToString() + "_" + fieldName;
                //string columnname = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, resourceId);
                //table2HeaderCell.Text = columnname;
                switch (fieldName)
                {
                    case "CuentaID":
                        table2HeaderCell.Text = "Código PUC";
                        break;
                    case "CuentaDesc":
                        table2HeaderCell.Text = "Nombre Cuenta";
                        break;
                    case "DebitoML":
                        table2HeaderCell.Text = "Débito";
                        break;
                    case "CreditoML":
                        table2HeaderCell.Text = "Crédito";
                        break;
                };
                table2HeaderRow.Controls.Add(table2HeaderCell);
                #endregion

                #region Documento table detail
                table2DetailCell = new XRTableCell();
                table2DetailCell.WidthF = table2HeaderCell.WidthF;

                if (fieldName.Contains("Cuenta"))
                {
                    table2DetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                    table2DetailCell.DataBindings.Add("Text", this.DataSource, "FacturaDetail." + fieldName);
                }
                else
                    table2DetailCell.DataBindings.Add("Text", this.DataSource, "FacturaDetail." + fieldName, "{0:#,0.}");

                table2DetailRow.Controls.Add(table2DetailCell);
                #endregion
            };
            table2Header.Controls.Add(table2HeaderRow);
            table2Detail.Controls.Add(table2DetailRow);

            documentoTable2Header.Controls.Add(table2Header);
            documentoTable2Detail.Controls.Add(table2Detail);
            #endregion
        }
    }
}
