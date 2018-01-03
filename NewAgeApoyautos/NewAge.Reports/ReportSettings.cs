using DevExpress.XtraReports.UI;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Negocio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NewAge.Reports
{
    public class ReportSettings
    {
        #region Variables

        ModuloGlobal _moduloGlobal;
        ReportProvider _reportProvider;
        XRPictureBox _logo;
   
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public ReportSettings(SqlConnection conn, SqlTransaction tx, DTO_glEmpresa emp, int userID, string loggerConn)
        {
            this._moduloGlobal = new ModuloGlobal(conn, tx, emp, userID, loggerConn);
            this._reportProvider = new ReportProvider(conn, tx, emp, userID, loggerConn);
        }

        #region Metodos
        
        /// <summary>
        /// Crea el Reporte de acuerdo al formato
        /// </summary>
        /// <param name="report">reporte</param>
        /// <param name="formaType">formato</param>
        public void CreateReport(XtraReport report, ExportFormatType formaType)
        {
            this.SetLogo();
            this.ReportName = Guid.NewGuid().ToString();
            this._reportProvider.LoadResources(report.AllControls<XRControl>());
            string filesPath = this._moduloGlobal.GetControlValue(AppControl.RutaFisicaArchivos);
            string docsPath = this._moduloGlobal.GetControlValue(AppControl.RutaTemporales);
            string Path = filesPath + docsPath + ReportName + '.' + formaType.ToString();

            switch (formaType)
            {
                case ExportFormatType.pdf:
                    report.ExportToPdf(Path);
                    break;
                case ExportFormatType.xls:
                    report.ExportToXls(Path);
                    break;
                case ExportFormatType.xlsx:
                    report.ExportToXlsx(Path);
                    break;
                case ExportFormatType.html:
                    report.ExportToHtml(Path);
                    break;
            }
        }

        /// <summary>
        /// Carga el logo de la Compañia
        /// </summary>
        public void SetLogo()
        {
            byte[] logo = this._moduloGlobal.glEmpresaLogo();
            try
            {
                object img = Utility.ByteArrayToObject(logo);
                Image logoImage = (Image)img;
                this.Logo = new XRPictureBox();
                this.Logo.Image = logoImage;                
            }
            catch { }
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Retorna el nombre del Reporte
        /// </summary>
        public string ReportName { get; set; }
                
        /// <summary>
        /// Logo Empresa
        /// </summary>
        public XRPictureBox Logo { get; set; }
             

        #endregion

    }
}

