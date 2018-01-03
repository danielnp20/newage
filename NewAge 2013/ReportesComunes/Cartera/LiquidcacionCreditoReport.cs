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
    public partial class LiquidcacionCreditoReport : BaseCommonReport
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
        /// <param name="fieldList">list of fields for report detail table</param>
        /// <param name="estadoInd">indicador de aprobacion del documneto (aprobado - false)</param> 
        /// <param name="supplier"> Interface que provee de informacion a un reporte comun</param>
        public LiquidcacionCreditoReport(int docId, List<DTO_ReportLiquidacionCredito2> documentoData, ArrayList detailFieldList, ArrayList footerFieldList, CommonReportDataSupplier supplier)
            : base(supplier)
        {
            this._supplier = supplier;
            this._docId = docId;
            this.lblReportName.Text = this._supplier.GetResource(Librerias.Project.LanguageTypes.Forms, this._docId.ToString());
            int totalsInd = 0;
            //this.lblUserName.Text = documentoData[0].Header.Cliente;
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
            documentoBand.DataMember = "Detail";

            GroupHeaderBand documentPeriodBand;
            documentPeriodBand = new GroupHeaderBand();
            documentPeriodBand.Level = 1;
            documentPeriodBand.HeightF = 10;
            documentoBand.Bands.Add(documentPeriodBand);

            GroupHeaderBand documentoHeader = new GroupHeaderBand();
            documentoHeader.HeightF = 50;
            documentoHeader.Level = 0;
            documentoBand.Bands.Add(documentoHeader);

            DetailBand documentoDetail;
            documentoDetail = new DetailBand();
            documentoDetail.HeightF = 10;
            documentoBand.Bands.Add(documentoDetail);

            GroupFooterBand documentoFooterTotales = new GroupFooterBand();
            documentoFooterTotales.HeightF = 30;
            documentoFooterTotales.Level = 0;
            documentoBand.Bands.Add(documentoFooterTotales);

            GroupFooterBand documentFooterBand;
            documentFooterBand = new GroupFooterBand();
            documentFooterBand.Level = 1;
            documentFooterBand.HeightF = 100;
            documentoBand.Bands.Add(documentFooterBand);

            this.Bands.Add(documentoBand);
            #endregion

            #region Documento field width
            float headerTableWidth = (this.PageWidth - (this.Margins.Right + this.Margins.Left));

            #endregion

            #region Documento elements
            
            #region Watermark

            //foreach (var VARIABLE in documentoData)
            //{
            //    isApro = VARIABLE.isApro;
            //}
            //if (!isApro)
            //{
            //    this.Watermark.Text = "Preliminar";
            //    this.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
            //    this.Watermark.Font = new Font("Arial", 100);
            //    this.Watermark.ForeColor = Color.LightGray;
            //    this.Watermark.TextTransparency = 150;
            //    this.Watermark.ShowBehind = true;
            //}
            #endregion

            #region Documento header
            #region Tabla 1
            XRTable documentoHeaderPeriodTable;
            XRTableRow documentoHeaderTableRow;
            XRTableCell documentoHeaderTableCell_Name;
            XRTableCell documentoHeaderTableCell_Value;

            documentoHeaderPeriodTable = new XRTable();
            documentoHeaderPeriodTable.BeginInit();
            documentoHeaderPeriodTable.LocationF = new PointF(0, 0);
            documentoHeaderPeriodTable.SizeF = new SizeF(140, 0);
            documentoHeaderPeriodTable.WidthF = headerTableWidth ;
            documentoHeaderPeriodTable.StyleName = "groupHeaderStyle";

            for (int i = 0; i < documentoData.Count; i++)
            {
                #region Row 1
                documentoHeaderTableRow = new XRTableRow();
                documentoHeaderTableRow.HeightF = documentoHeaderPeriodTable.HeightF;

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 5;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 8, FontStyle.Bold);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Fecha: ";//base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Para");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = headerTableWidth / 5;
                documentoHeaderTableCell_Value.Font = new Font("Arial", 8);
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "Fecha", "{0:dd/MMM/yyyy}");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 5;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 8, FontStyle.Bold);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = " ";//base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Para");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 5;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 10, FontStyle.Bold);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "OTROS COSTOS ";//base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Para");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 5;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 10, FontStyle.Bold);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "MENSUAL";//base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Para");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 5;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 10, FontStyle.Bold);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "TOTAL";//base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Para");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderPeriodTable.Rows.Add(documentoHeaderTableRow);
                #endregion

                #region Row 2
                documentoHeaderTableRow = new XRTableRow();
                documentoHeaderTableRow.HeightF = documentoHeaderPeriodTable.HeightF;

                #region Clienta
                
                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 4;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 8, FontStyle.Bold);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Cliente: ";//base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = headerTableWidth / 4;
                documentoHeaderTableCell_Value.Font = new Font("Arial", 8);
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "Cliente");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value); 
                #endregion

                #region Espacios
                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 4;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 8);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "      ";//base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 4;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 8, FontStyle.Bold);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = " ";//base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Para");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 4;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 8, FontStyle.Bold);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Aportes: ";//base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Para");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = headerTableWidth / 4;
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "AportesMes", "{0:C0}");
                documentoHeaderTableCell_Value.Font = new Font("Arial", 8);
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = headerTableWidth / 4;
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "Aportes", "{0:C0}");
                documentoHeaderTableCell_Value.Font = new Font("Arial", 8);
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);
                #endregion

                documentoHeaderPeriodTable.Rows.Add(documentoHeaderTableRow);
                #endregion

                #region Row 3
                documentoHeaderTableRow = new XRTableRow();
                documentoHeaderTableRow.HeightF = documentoHeaderPeriodTable.HeightF;

                #region Espacios
                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 4;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 8, FontStyle.Bold);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = " ";//base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Para");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 4;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 8, FontStyle.Bold);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = " ";//base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Para");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 4;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 8, FontStyle.Bold);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = " ";//base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Para");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);
                
                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 4;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 8, FontStyle.Bold);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "TOTAL: ";//base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Para");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                #endregion
                #region Totales
                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = headerTableWidth / 5;
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "TotalOtrosMes", "{0:C0}");
                documentoHeaderTableCell_Value.Font = new Font("Arial", 8, FontStyle.Underline);
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = headerTableWidth / 5;
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "Aportes", "{0:C0}");
                documentoHeaderTableCell_Value.Font = new Font("Arial", 8, FontStyle.Underline);
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = headerTableWidth / 5;
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "Aportes", "{0:C0}");
                documentoHeaderTableCell_Value.Font = new Font("Arial", 8, FontStyle.Underline);
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);
                #endregion

                documentoHeaderPeriodTable.Rows.Add(documentoHeaderTableRow);
                #endregion

                #region Row 4
                
                #endregion

                #region Row 5
                documentoHeaderTableRow = new XRTableRow();
                documentoHeaderTableRow.HeightF = documentoHeaderPeriodTable.HeightF;

                XRLabel lblAprobado_blank = new XRLabel();
                //lblAprobado_blank.LocationF = new PointF(lblAprobado.LocationF.X, lblAprobado.LocationF.Y + lblAprobado.HeightF);
                lblAprobado_blank.SizeF = new SizeF(headerTableWidth, 10);
                lblAprobado_blank.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                documentoHeaderTableCell_Value.Controls.Add(lblAprobado_blank);
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

                documentoHeaderPeriodTable.Rows.Add(documentoHeaderTableRow);
                #endregion

                #region Row 6

                documentoHeaderTableRow = new XRTableRow();
                documentoHeaderTableRow.HeightF = documentoHeaderPeriodTable.HeightF;

                #region Libranza
                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 8, FontStyle.Bold);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Libranza:";//base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "Libranza");
                documentoHeaderTableCell_Value.Font = new Font("Arial", 8);
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value); 
                #endregion

                #region Valor Solicitado
                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 8, FontStyle.Bold);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Valor Solicitado";//base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "VrSolicitado", "{0:C0}");
                documentoHeaderTableCell_Value.Font = new Font("Arial", 8);
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value); 
                #endregion

                #region Valor Descuento

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 8, FontStyle.Bold);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Valor Descuento:";//base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "VrDescuento", "{0:C0}");
                documentoHeaderTableCell_Value.Font = new Font("Arial", 8);
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);
               
                #endregion

                documentoHeaderPeriodTable.Rows.Add(documentoHeaderTableRow);

                #endregion

                #region Row 7
                documentoHeaderTableRow = new XRTableRow();
                documentoHeaderTableRow.HeightF = documentoHeaderPeriodTable.HeightF;

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 8, FontStyle.Bold);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Plazo:";//base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "NumeroCuotas");
                documentoHeaderTableCell_Value.Font = new Font("Arial", 8);
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 8, FontStyle.Bold);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Valor Adicional ";//base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "VrAdicional", "{0:C0}");
                documentoHeaderTableCell_Value.Font = new Font("Arial", 8);
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 8, FontStyle.Bold);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Valor Compra";//base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "VrCompra", "{0:C0}");
                documentoHeaderTableCell_Value.Font = new Font("Arial", 8);
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

                documentoHeaderPeriodTable.Rows.Add(documentoHeaderTableRow);
                #endregion

                #region Row 8
                documentoHeaderTableRow = new XRTableRow();
                documentoHeaderTableRow.HeightF = documentoHeaderPeriodTable.HeightF;

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 8, FontStyle.Bold);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Valor Cuota:";//base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "VrCuota", "{0:C0}");
                documentoHeaderTableCell_Value.Font = new Font("Arial", 8);
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 8, FontStyle.Bold);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Valor Crédito:";//base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "VrCredito", "{0:C0}");
                documentoHeaderTableCell_Value.Font = new Font("Arial", 8);
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 8, FontStyle.Bold);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Valor Giro:";//base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "VrGiro", "{0:C0}");
                documentoHeaderTableCell_Value.Font = new Font("Arial", 8);
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);
                
                documentoHeaderPeriodTable.Rows.Add(documentoHeaderTableRow);
                #endregion

                #region Row 9
                documentoHeaderTableRow = new XRTableRow();
                documentoHeaderTableRow.HeightF = documentoHeaderPeriodTable.HeightF;

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 8, FontStyle.Bold);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Valor Afiliación:";//base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "VrAfiliacionTotal", "{0:C0}");
                documentoHeaderTableCell_Value.Font = new Font("Arial", 8);
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 8, FontStyle.Bold);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Valor Seguro:";//base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = headerTableWidth / 6;
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "VrSeguro", "{0:C0}");
                documentoHeaderTableCell_Value.Font = new Font("Arial", 8);
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 8;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 8, FontStyle.Bold);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Tasa Interes";//base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = headerTableWidth / 8;
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "xInteres", "{0:P2}");
                documentoHeaderTableCell_Value.Font = new Font("Arial", 8);
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

                documentoHeaderTableCell_Name = new XRTableCell();
                documentoHeaderTableCell_Name.WidthF = headerTableWidth / 8;
                documentoHeaderTableCell_Name.Font = new Font("Arial", 8, FontStyle.Bold);
                documentoHeaderTableCell_Name.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Name.Text = "Tasa Seguro";//base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, docID.ToString() + "_Fecha");
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Name);

                documentoHeaderTableCell_Value = new XRTableCell();
                documentoHeaderTableCell_Value.WidthF = headerTableWidth / 8;
                documentoHeaderTableCell_Value.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                documentoHeaderTableCell_Value.DataBindings.Add("Text", documentoData[i].Header, "xSeguro", "{0:P2}");
                documentoHeaderTableCell_Value.Font = new Font("Arial", 8);
                documentoHeaderTableRow.Cells.Add(documentoHeaderTableCell_Value);

                documentoHeaderPeriodTable.Rows.Add(documentoHeaderTableRow); 
                #endregion
            }

            documentoHeaderPeriodTable.EndInit();
            documentPeriodBand.Controls.Add(documentoHeaderPeriodTable); 
            #endregion
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
                if (fieldName.Contains("Venc_Cta"))
                {
                    tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                    tableDetailCell.DataBindings.Add("Text", documentoData, "Detail." + fieldName, "{0:dd/MMM/yyyy}");
                }
                if (fieldName.Contains("CuotaID"))
                {
                    tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    tableDetailCell.DataBindings.Add("Text", documentoData, "Detail." + fieldName);
                }
                
                if (fieldName.Contains("VlrCuota"))
                {
                    tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    tableDetailCell.DataBindings.Add("Text", documentoData, "Detail." + fieldName, "{0:C0}");
                }
                if (fieldName.Contains("Capital"))
                {
                    tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    tableDetailCell.DataBindings.Add("Text", documentoData, "Detail." + fieldName, "{0:C0}");
                }
                if (fieldName.Contains("Seguro"))
                {
                    tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    tableDetailCell.DataBindings.Add("Text", documentoData, "Detail." + fieldName, "{0:C0}");
                }
                if (fieldName.Contains("Interes"))
                {
                    tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    tableDetailCell.DataBindings.Add("Text", documentoData, "Detail." + fieldName, "{0:C0}");
                }
                if (fieldName.Contains("Otros1"))
                {
                    tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    tableDetailCell.DataBindings.Add("Text", documentoData, "Detail." + fieldName, "{0:C0}");
                }
                if (fieldName.Contains("VlrOtros"))
                {
                    tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    tableDetailCell.DataBindings.Add("Text", documentoData, "Detail." + fieldName, "{0:C0}");
                }
                if (fieldName.Contains("SaldoCapital"))
                {
                    tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    tableDetailCell.DataBindings.Add("Text", documentoData, "Detail." + fieldName, "{0:C0}");
                }
                tableDetailRow.Controls.Add(tableDetailCell);

                #endregion

                #region Report table footer

                //if (fieldName.Contains("V"))
                //{
                //    if (totalsInd == 0)
                //    {
                //        totalFooterCell_Caption = new XRTableCell();
                //        totalFooterCell_Caption.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
                //        totalFooterCell_Caption.WidthF = headerTableWidth;
                //        totalFooterCell_Caption.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 20, 0, 0);
                //        totalFooterCell_Caption.Text = base._dataSupplier.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_Totals") + "Crédito";
                //        totalFooterRow.Controls.Add(totalFooterCell_Caption);
                //    }

                //    totalsInd = 1;

                //    totalFooterCell_Value = new XRTableCell();
                //    totalFooterCell_Value.WidthF = tableHeaderCell.WidthF;
                //    totalFooterCell_Value.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                //    totalFooterCell_Value.BorderWidth = 2;
                //    totalFooterCell_Value.Summary.Func = SummaryFunc.Sum;
                //    totalFooterCell_Value.Summary.Running = SummaryRunning.Report;
                //    totalFooterCell_Value.Summary.FormatString = "{0:C2}";
                //    totalFooterCell_Value.DataBindings.Add("Text", documentoData, "Detail." + fieldName);
                //    totalFooterRow.Controls.Add(totalFooterCell_Value);
                //}

                //if (totalsInd == 0)
                //    totalFieldLocation += tableHeaderCell.WidthF;

                #endregion
            }

            tableHeader.Controls.Add(tableHeaderRow);
            tableDetail.Controls.Add(tableDetailRow);
            totalFooter.Controls.Add(totalFooterRow);

            documentoHeader.Controls.Add(tableHeader);
            documentoDetail.Controls.Add(tableDetail);
            documentoFooterTotales.Controls.Add(totalFooter);

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

