using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.DTO.Reportes;
using System.Collections;
using DevExpress.XtraReports.UI;
using System.Drawing;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.ReportesComunes;

namespace NewAge.Cliente.GUI.WinApp.Reports
{
    public class ComprobanteReportSigned : ComprobanteReport
    {
        #region Funciones Publicas
        /// <summary>
        /// Comprobante Report Constructor (with the place for the approbation mark)
        /// </summary>
        /// <param name="documentId">ID of the current document allowing to get information about it</param>
        /// <param name="reportList">data for the report</param>
        /// <param name="multiMonedaInd">MultiMoneda property of the document (true - MultiMoneda; false - not MultiMoneda) </param>
        /// <param name="fieldList">list of fields for report detail table</param>
        /// <param name="supplier"> Interface que provee de informacion a un reporte comun</param>
        /// <param name="selectedFiltersList">list of filters applied by user</param>
        public ComprobanteReportSigned(int documentID, List<DTO_ReportComprobante2> reportList, bool multiMonedaInd, ArrayList fieldList,bool isPre, List<string> selectedFiltersList)
            : base(documentID, reportList, multiMonedaInd, fieldList, isPre, BaseController.GetInstance(), selectedFiltersList)
        {
            #region Create "Aprobado" Lable
            XRLabel aprobarCaptionLable = new XRLabel();
            aprobarCaptionLable.LocationF = new PointF((this.PageWidth - (this.Margins.Left + this.Margins.Right)) / 10, 70);
            aprobarCaptionLable.Name = "lblAprobarCaption";
            aprobarCaptionLable.Text = "Aprobado:";
            aprobarCaptionLable.HeightF = 15;
            aprobarCaptionLable.WidthF = 60F;
            aprobarCaptionLable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            aprobarCaptionLable.Font = new Font("Times New Roman", 9F, FontStyle.Bold);

            XRLabel aprobarLable = new XRLabel();
            aprobarLable.LocationF = new PointF(aprobarCaptionLable.LocationF.X + aprobarCaptionLable.WidthF + 15, aprobarCaptionLable.LocationF.Y);
            aprobarLable.Name = "lblAprobar";
            aprobarLable.Text = "_responsiblePerson";
            aprobarLable.HeightF = aprobarCaptionLable.HeightF;
            aprobarLable.WidthF = 150;
            aprobarLable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            aprobarLable.Font = new Font("Times New Roman", 9F, FontStyle.Italic);
            #endregion

            #region Add decorative elements
            XRLine aprobarLine = new XRLine();
            aprobarLine.SizeF = new System.Drawing.SizeF(aprobarLable.WidthF, 2);
            aprobarLine.LocationF = new PointF(aprobarLable.LocationF.X, aprobarLable.LocationF.Y + aprobarCaptionLable.HeightF + 1);
            aprobarLine.LineWidth = 2;
            #endregion

            this.ReportFooter.Controls.Add(aprobarCaptionLable);
            this.ReportFooter.Controls.Add(aprobarLable);
            this.ReportFooter.Controls.Add(aprobarLine);
        } 
        #endregion
    }
}
