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
using NewAge.DTO.Negocio;
using System.Windows.Forms;
using DevExpress.XtraReports.Parameters;
using NewAge.Librerias.Project;
using NewAge.Cliente.GUI.WinApp.Reports;

namespace NewAge.Cliente.GUI.WinApp.Reports
{
    class BalanceDePruebaPorQReport : BaseReport
    {
        #region Variables
        private int _Q_ind;        
        private ArrayList _fieldList1;
        private ArrayList _fieldList;        
        private List<DTO_ReportBalanceDePruebaPorQ> _reportData;
        private TipoMoneda _MM;
        private float _tableWidth;
        private float _columnWidth;
        private float _headerRowHeight;
        private float _detailRowHeight;
        private DetailReportBand reportTableBand;
        private GroupHeaderBand reportPeriodBand;
        private GroupHeaderBand reportTableHeaderBand;
        private DetailBand reportTableDetailBand;
        private GroupFooterBand reportGroupFooterBand;                    
        private XRTable tableHeader;
        private XRTableRow tableHeaderRow;
        private XRTableCell tableHeaderCell;
        private XRTable upperTableHeader;
        private XRTableRow upperTableHeaderRow;
        private XRTableCell upperTableHeaderCell;
        private XRTable tableDetail;
        private XRTableRow tableDetailRow;
        private XRTableCell tableDetailCell;
        private XRCrossBandBox totalFrameQ;
        private XRCrossBandBox totalFrameY;
        
        BaseController _bc = BaseController.GetInstance();
        #endregion

        #region Funciones Publicas
        /// <summary>
        ///  Balance De Prueba por quarters Report Constructor
        /// </summary>
        /// <param name="reportData">data for the report</param>
        /// <param name="fieldList1">list of fields for report detail table</param>
        /// <param name="MM"> Moneda type of the report - local, foreign, both</param>
        /// <param name="balanceTipo">tipo de balance</param>
        /// <param name="Date">report period</param>
        /// <param name="selectedFiltersList">list of filters assigned by the user</param>
        /// <param name="cuentaFuncInd">indicador del tipo de la cuenta (true - cuenta Funcional;false - cuenta Alterna)</param>
        public BalanceDePruebaPorQReport(List<DTO_ReportBalanceDePruebaPorQ> reportData, ArrayList fieldList1, TipoMoneda MM, string balanceTipo, DateTime Date, List<string> selectedFiltersList)
        {
            _reportData = reportData;
            _fieldList1 = fieldList1;
            _MM = MM;

            _Q_ind = 1;

            _fieldList = CreateFieldList();

            this.lblReportName.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.coBalanceDePruebaPorQ).ToString()) + " (.000)";

            #region Report styles
            this.Landscape = true;
            XRControlStyles tableStyles = new XRControlStyles(this)
            {
                EvenStyle = new XRControlStyle()
                {
                    Name = "tableDetailEvenStyle",
                    BackColor = Color.White,
                    ForeColor = Color.Black,
                    //Font = new Font("Times New Roman", 9, FontStyle.Bold),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0)
                },
                OddStyle = new XRControlStyle()
                {
                    Name = "tableDetailOddStyle",
                    BackColor = Color.WhiteSmoke,
                    ForeColor = Color.Black,
                    //Font = new Font("Times New Roman", 9, FontStyle.Bold),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0)
                },
                Style = new XRControlStyle()
                {
                    Name = "tableHeaderStyle",
                    BackColor = Color.DimGray,
                    ForeColor = Color.White,
                    Font = new Font("Arial Narrow", 9, FontStyle.Bold),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                    Borders = DevExpress.XtraPrinting.BorderSide.None
                }
            };

            this.StyleSheet.Add(tableStyles.EvenStyle);
            this.StyleSheet.Add(tableStyles.OddStyle);
            this.StyleSheet.Add(tableStyles.Style);

            XRControlStyles tableShadedStyles = new XRControlStyles(this)
            {
                EvenStyle = new XRControlStyle()
                {
                    Name = "tableDetailEvenShadedStyle",
                    BackColor = Color.Gainsboro,
                    ForeColor = Color.Black,
                    //Font = new Font("Times New Roman", 9, FontStyle.Bold),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0)
                },
                OddStyle = new XRControlStyle()
                {
                    Name = "tableDetailOddShadedStyle",
                    BackColor = Color.LightGray,
                    ForeColor = Color.Black,
                    //Font = new Font("Times New Roman", 9, FontStyle.Bold),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0)
                },
                Style = new XRControlStyle()
                {
                    Name = "tableHeaderShadedStyle",
                    BackColor = Color.DarkSlateGray,
                    ForeColor = Color.White,
                    Font = new Font("Arial Narrow", 9, FontStyle.Bold),
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                    Borders = DevExpress.XtraPrinting.BorderSide.None
                }
            };

            this.StyleSheet.Add(tableShadedStyles.EvenStyle);
            this.StyleSheet.Add(tableShadedStyles.OddStyle);
            this.StyleSheet.Add(tableShadedStyles.Style);
            #endregion

            #region Report bands
            reportTableBand = new DetailReportBand();
            reportTableBand.DataSource = reportData;

            reportPeriodBand = new GroupHeaderBand();
            reportPeriodBand.Level = 1;
            reportPeriodBand.HeightF = (selectedFiltersList.Count > 0) ? 85 : 55;
            reportPeriodBand.RepeatEveryPage = true;
            reportTableBand.Bands.Add(reportPeriodBand);

            reportTableHeaderBand = new GroupHeaderBand();
            reportTableHeaderBand.Level = 0;
            reportTableHeaderBand.HeightF = 55;
            reportTableHeaderBand.RepeatEveryPage = true;
            reportTableBand.Bands.Add(reportTableHeaderBand);

            reportTableDetailBand = new DetailBand();
            reportTableDetailBand.HeightF = 20;
            reportTableBand.Bands.Add(reportTableDetailBand);

            reportGroupFooterBand = new GroupFooterBand();
            reportGroupFooterBand.Level = 0;
            reportGroupFooterBand.HeightF = 30;
            reportTableBand.Bands.Add(reportGroupFooterBand);

            this.Bands.Add(reportTableBand);
            #endregion

            #region Report dimensions
            _tableWidth = 0;
            _columnWidth = 0;

            _tableWidth = this.PageWidth - (this.Margins.Right + this.Margins.Left);

            if (_fieldList1.Contains("CuentaDesc"))
                _columnWidth = (_tableWidth - 50) / _fieldList.Count;
            else
                _columnWidth = _tableWidth / _fieldList.Count;

            _headerRowHeight = 0;
            _headerRowHeight = reportTableHeaderBand.HeightF - 30;

            _detailRowHeight = 0;
            _detailRowHeight = reportTableDetailBand.HeightF;
            #endregion

            #region Report elements
            #region Report period band
            
            XRShape periodFrame = new XRShape();
            periodFrame.LocationF = new PointF(0, 0);
            periodFrame.SizeF = new SizeF(_tableWidth, 35);
            periodFrame.LineWidth = 2;
            periodFrame.SendToBack();
            periodFrame.Shape = new ShapeRectangle()
            {
                Fillet = 50,
            };
            reportPeriodBand.Controls.Add(periodFrame);

            XRTable periodTable;
            XRTableRow periodTableRow;
            XRTableCell periodTableCell_Year;
            XRTableCell periodTableCell_Moneda;
            XRTableCell periodTableCell_BalanceTipo;
            periodTable = new XRTable();
            periodTable.LocationF = periodFrame.LocationF;
            periodTable.SizeF = periodFrame.SizeF;
            periodTable.Font = new Font("Times New Roman", 11, FontStyle.Bold);
            periodTable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            periodTableRow = new XRTableRow();

            float periodTableCellWidth = (_tableWidth) / 3;

            periodTableCell_Year = new XRTableCell();
            periodTableCell_Year.WidthF = periodTableCellWidth;
            periodTableCell_Year.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_lblYear") + ":   " + Date.ToString("yyyy");
            periodTableRow.Cells.Add(periodTableCell_Year);

            periodTableCell_Moneda = new XRTableCell();
            periodTableCell_Moneda.WidthF = periodTableCellWidth;
            //periodTableCell_Moneda.Text = "Moneda:   " + MM.ToString();
            switch (MM)
            {
                case TipoMoneda.Local:
                    periodTableCell_Moneda.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_Moneda") + ":   " + _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyLocal);
                    break;
                case TipoMoneda.Foreign:
                    periodTableCell_Moneda.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_Moneda") + ":   " + _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyForeign);
                    break;
                case TipoMoneda.Both:
                    periodTableCell_Moneda.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_Moneda") + ":   " + _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_CurrencyBoth);
                    break; ;
            };
            periodTableRow.Cells.Add(periodTableCell_Moneda);

            periodTableCell_BalanceTipo = new XRTableCell();
            periodTableCell_BalanceTipo.WidthF = periodTableCellWidth;
            periodTableCell_BalanceTipo.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_TipoBalance") + ":   " + balanceTipo;
            periodTableRow.Cells.Add(periodTableCell_BalanceTipo);

            periodTable.Rows.Add(periodTableRow);
            reportPeriodBand.Controls.Add(periodTable);

            XRLabel selectedFiltersLable = new XRLabel();
            selectedFiltersLable.LocationF = new PointF(0, periodFrame.HeightF + 10);
            selectedFiltersLable.SizeF = new SizeF(_tableWidth, 20);
            selectedFiltersLable.Font = new Font("Times New Roman", 9, FontStyle.Italic);
            selectedFiltersLable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            selectedFiltersLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(20, 0, 0, 0);
            selectedFiltersLable.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppForms.ReportForm).ToString() + "_FiltradoPor") + "   ";
            if (selectedFiltersList.Count > 0)
            {
                int filterCount = 0;
                foreach (string filter in selectedFiltersList)
                {
                    selectedFiltersLable.Text += filter;
                    filterCount++;
                    if (!(filterCount == (selectedFiltersList.Count))) selectedFiltersLable.Text += ",  ";
                };
                reportPeriodBand.Controls.Add(selectedFiltersLable);
            };
            #endregion

            #region Report footer band
            XRLine footerLowerLine_1 = new XRLine()
            {
                LineWidth = 1,
                SizeF = new SizeF(_tableWidth, 2),
                LocationF = new PointF(0, 12)
            };
            reportGroupFooterBand.Controls.Add(footerLowerLine_1);

            XRLine footerLowerLine_2 = new XRLine()
            {
                LineWidth = 1,
                SizeF = new SizeF(_tableWidth, 2),
                LocationF = new PointF(0, 15)
            };
            reportGroupFooterBand.Controls.Add(footerLowerLine_2);
            #endregion
            #endregion

            CreateQHeaderRow();

            CreateReportTable();
        }
        
        #endregion

        #region Funciones Privadas
        /// <summary>
        /// dynamically creates field list for the report 
        /// </summary>
        /// <returns>field list for the report </returns>
        private ArrayList CreateFieldList()
        {
            _fieldList = new ArrayList();
            foreach (string fieldName in _fieldList1)
            {
                if (fieldName.Contains("ML_Q" + _Q_ind.ToString()))
                {
                    _fieldList.Add("MovML_01_Q" + _Q_ind.ToString());
                    _fieldList.Add("MovML_02_Q" + _Q_ind.ToString());
                    _fieldList.Add("MovML_03_Q" + _Q_ind.ToString());
                    _fieldList.Add(fieldName);
                }
                else
                    if (fieldName.Contains("ME_Q" + _Q_ind.ToString()))
                    {
                        _fieldList.Add("MovME_01_Q" + _Q_ind.ToString());
                        _fieldList.Add("MovME_02_Q" + _Q_ind.ToString());
                        _fieldList.Add("MovME_03_Q" + _Q_ind.ToString());
                        _fieldList.Add(fieldName);
                    }
                    else
                        _fieldList.Add(fieldName);
            };

            return _fieldList;
        }

        /// <summary>
        /// creates part of the report table header (quarters division)
        /// </summary>
        private void CreateQHeaderRow()
        {
            #region Define Q Header Row
            upperTableHeader = new XRTable();
            upperTableHeader.HeightF = _headerRowHeight;
            if (_fieldList.Contains("CuentaDesc"))
            {
                upperTableHeader.WidthF = _tableWidth - 4 * _columnWidth - 50;
                upperTableHeader.LocationF = new PointF(3 * _columnWidth + 50, 0);
            }
            else
            {
                upperTableHeader.WidthF = _tableWidth - 3 * _columnWidth;
                upperTableHeader.LocationF = new PointF(2 * _columnWidth, 0);
            };
            if (_MM == TipoMoneda.Both)
            {
                upperTableHeader.WidthF -= 2 * _columnWidth;
                upperTableHeader.LocationF = new PointF(upperTableHeader.LocationF.X + _columnWidth, 0);
            };
            upperTableHeader.StyleName = "tableHeaderStyle";
            upperTableHeaderRow = new XRTableRow();
            upperTableHeaderRow.Borders = DevExpress.XtraPrinting.BorderSide.All;
            upperTableHeaderRow.BorderColor = Color.White;
            upperTableHeaderRow.BorderWidth = 1;
            #endregion

            #region Build Q Header Row
            for (int i = 1; i < 5; i++)
            {
                upperTableHeaderCell = new XRTableCell();
                upperTableHeaderCell.Text = "Q" + i.ToString();
                if (i == _Q_ind)
                {
                    upperTableHeaderCell.WidthF = 4 * _columnWidth;
                    upperTableHeaderCell.BackColor = Color.DarkSlateGray;
                }
                else
                {
                    upperTableHeaderCell.WidthF = _columnWidth;
                    upperTableHeaderCell.BackColor = Color.DimGray;
                };
                if (_MM == TipoMoneda.Both) upperTableHeaderCell.WidthF *= 2;
                upperTableHeaderRow.Cells.Add(upperTableHeaderCell);

                upperTableHeaderCell.PreviewClick += new PreviewMouseEventHandler(upperTableHeaderCell_PreviewClick);
            };
            upperTableHeader.Controls.Add(upperTableHeaderRow);
            reportTableHeaderBand.Controls.Add(upperTableHeader);
            #endregion
        }

        /// <summary>
        /// creates report table detail
        /// </summary>
        private void CreateReportTable()
        {
            #region Define Report table detail
            tableHeader = new XRTable();
            tableHeader.WidthF = _tableWidth;
            tableHeader.HeightF = _headerRowHeight;
            tableHeader.LocationF = new PointF(0, _headerRowHeight);
            tableHeader.StyleName = "tableHeaderStyle";
            tableHeaderRow = new XRTableRow();

            tableDetail = new XRTable();
            tableDetail.Name = "tableDetail";
            tableDetail.WidthF = _tableWidth;
            tableDetail.HeightF = _detailRowHeight;
            tableDetail.EvenStyleName = "tableDetailEvenStyle";
            tableDetail.OddStyleName = "tableDetailOddStyle";
            tableDetailRow = new XRTableRow();
            tableDetailRow.Name = "tableDetailRow";
            #endregion

            int Q_num = 1;
            float totalFrameLocation = 0;

            #region Build Report table detail
            foreach (string fieldName in _fieldList)
            {
                #region Report table header
                tableHeaderCell = new XRTableCell();
                if (fieldName.Contains("Desc"))
                    tableHeaderCell.WidthF = _columnWidth + 50;
                else
                    tableHeaderCell.WidthF = _columnWidth;

                tableHeaderCell.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.coBalanceDePruebaPorQ).ToString() + "_" + fieldName);

                tableHeaderCell.StyleName = (fieldName.Contains("Q" + _Q_ind.ToString())) ? "tableHeaderShadedStyle" : "tableHeaderStyle";

                tableHeaderRow.Controls.Add(tableHeaderCell);
                #endregion

                #region Report table detail
                tableDetailCell = new XRTableCell();
                tableDetailCell.WidthF = tableHeaderCell.WidthF;

                if (fieldName.Contains("Cuenta"))
                {
                    tableDetailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                    tableDetailCell.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0);
                    tableDetailCell.DataBindings.Add("Text", this.DataSource, fieldName);
                    if (fieldName.Contains("ID"))
                    {
                        tableDetailCell.BeforePrint += new System.Drawing.Printing.PrintEventHandler(tableDetailCell_BeforePrint);
                    };
                }
                else
                {
                    if (fieldName.Contains("Year") || fieldName.Contains("Inicial"))
                    {
                        totalFrameY = new XRCrossBandBox();
                        totalFrameY.StartBand = reportTableHeaderBand;
                        totalFrameY.EndBand = reportGroupFooterBand;
                        totalFrameY.Borders = DevExpress.XtraPrinting.BorderSide.All;
                        totalFrameY.BorderWidth = 1;
                        totalFrameY.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
                        totalFrameY.StartPointF = new PointF(totalFrameLocation, _headerRowHeight);
                        totalFrameY.EndPointF = new PointF(totalFrameLocation, 1);
                        totalFrameY.WidthF = _columnWidth - 1;
                        this.CrossBandControls.Add(totalFrameY);

                        tableDetailCell.DataBindings.Add("Text", this.DataSource, fieldName, "{0:#,0.}");
                    }
                    else
                    {

                        if (!fieldName.Contains("Q" + Q_num.ToString())) Q_num++;

                        if (Q_num == _Q_ind)
                        {
                            tableDetailCell.EvenStyleName = "tableDetailEvenShadedStyle";
                            tableDetailCell.OddStyleName = "tableDetailOddShadedStyle";
                            if (fieldName.Contains("Total"))
                            {
                                totalFrameQ = new XRCrossBandBox();
                                totalFrameQ.StartBand = reportTableHeaderBand;
                                totalFrameQ.EndBand = reportGroupFooterBand;
                                totalFrameQ.Borders = DevExpress.XtraPrinting.BorderSide.All;
                                totalFrameQ.BorderWidth = 1;
                                totalFrameQ.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
                                totalFrameQ.StartPointF = new PointF(totalFrameLocation, _headerRowHeight);
                                totalFrameQ.EndPointF = new PointF(totalFrameLocation, 1);
                                totalFrameQ.WidthF = _columnWidth - 1;
                                this.CrossBandControls.Add(totalFrameQ);
                            };
                        }
                        else
                        {
                            tableDetailCell.EvenStyleName = "tableDetailEvenStyle";
                            tableDetailCell.OddStyleName = "tableDetailOddStyle";
                        };

                        tableDetailCell.DataBindings.Add("Text", this.DataSource, "Q" + Q_num.ToString() + "." + fieldName, "{0:#,0.}");
                    };
                };

                totalFrameLocation += tableDetailCell.WidthF;

                tableDetailRow.Controls.Add(tableDetailCell);
                #endregion
            };
            tableHeader.Controls.Add(tableHeaderRow);
            tableDetailCell = new XRTableCell() { WidthF = 0, Visible = false, Name = "tableDetailCell_MaxLengthInd" };
            tableDetailCell.DataBindings.Add("Text", this.DataSource, "MaxLengthInd");
            tableDetailRow.Controls.Add(tableDetailCell);
            tableDetail.Controls.Add(tableDetailRow);

            reportTableHeaderBand.Controls.Add(tableHeader);
            reportTableDetailBand.Controls.Add(tableDetail);
            #endregion
        }
#endregion

        #region Eventos
        /// <summary>
        /// Changes font in the table detail row depnding on the data
        /// </summary>
        private void tableDetailCell_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell cell = (XRTableCell)sender;
            XRTableRow row = FindControl("tableDetailRow", true) as XRTableRow;
            XRTableCell cell_MaxLengthInd = FindControl("tableDetailCell_MaxLengthInd", true) as XRTableCell;

            //XRTableCell cell = (XRTableCell)sender;
            //XRTableRow row = FindControl("tableDetailRow", true) as XRTableRow;

            //string empGrupo = _bc.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta);
            //Tuple<int, string> tup = new Tuple<int, string>(AppMasters.coPlanCuenta, empGrupo);
            //DTO_glTabla table = _bc.AdministrationModel.Tables[tup];

            //string cellLength = (cell.Text.Trim()).Length.ToString();

            //if (cellLength == table.CodeLength(table.LevelsUsed()).ToString())

            if (!string.IsNullOrEmpty(cell_MaxLengthInd.Text.Trim()))
                if (Convert.ToInt16(cell_MaxLengthInd.Text) == 1)
                    row.Font = new Font("Times New Roman", 8, FontStyle.Regular);
                else
                    row.Font = new Font("Times New Roman", 8, FontStyle.Bold);
        }

        /// <summary>
        /// Handle mouse click on "quarter" part of the report table header
        /// </summary>
        private void upperTableHeaderCell_PreviewClick(object sender, PreviewMouseEventArgs e)
        {
            XRTableCell cell = (XRTableCell)sender;

            this.CrossBandControls.Clear();
            reportTableHeaderBand.Controls.Clear();
            reportTableDetailBand.Controls.Clear();

            _Q_ind = Convert.ToInt16((cell.Text).Remove(0, 1));

            _fieldList = CreateFieldList();

            CreateQHeaderRow();

            CreateReportTable();

            CreateDocument();
        } 
        #endregion
    }
}