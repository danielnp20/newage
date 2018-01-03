using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DevExpress.XtraReports.UI;

namespace NewAge.Librerias.Project
{
    public class ReportsBytes
    {
        /// <summary>
        /// Convierte Stream a bytes
        /// </summary>
        /// <param name="reporte">reporte en stream</param>
        /// <returns>arreglo de bytes</returns>
        public static byte[] ReportInBytes(XtraReport reporte)
        {
            MemoryStream stream = new MemoryStream();
            reporte.PrintingSystem.SaveDocument(stream);
            byte[] bytes = new byte[(int)stream.Length];
            stream.Write(bytes, 0, bytes.Length);
            return bytes;
        }

        /// <summary>
        /// Convierte arreglo de bytes a Reporte
        /// </summary>
        /// <param name="byteArrayIn">arreglo de bytes</param>
        /// <returns>Reporte</returns>
        public static XtraReport BytesInReport(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            XtraReport returnImage = XtraReport.FromStream(ms, false);
            return returnImage;
        }
    }
}
