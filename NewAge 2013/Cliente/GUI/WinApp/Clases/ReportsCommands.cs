using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraReports.UserDesigner;
using NewAge.DTO.Reportes;
using System.Windows.Forms;

namespace NewAge.Cliente.GUI.WinApp.Clases
{
    /// <summary>
    /// Utilidades para manejo de comandos en los reportes
    /// Sobreescribe el evento de comando de un reporte para el usuario final
    /// </summary>
    public class ReportsCommands : DevExpress.XtraReports.UserDesigner.ICommandHandler
    {
        BaseController _bc = BaseController.GetInstance();
        int documentID;
        XRDesignPanel panel;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="panel">Panel de diseño del reporte</param>
        /// <param name="docID">Documento que esta guardando la info</param>
        public ReportsCommands(XRDesignPanel panel, int docID)
        {
            this.panel = panel;
            this.documentID = docID;
        }

        /// <summary>
        /// Indica si se debe tomar el evento del comando o no
        /// </summary>
        /// <param name="command">Comando ejecutado</param>
        /// <returns>Retirna si se toma el evento o no</returns>
        public bool CanHandleCommand(ReportCommand command, ref bool useNextHandler)
        {
            useNextHandler = !(command == ReportCommand.SaveFile || command == ReportCommand.SaveFileAs ||
               command == ReportCommand.Closing);
            return !useNextHandler;
        }

        /// <summary>
        /// Maneja el evento asociado al comando del reporte
        /// </summary>
        /// <param name="command">Comando</param>
        /// <param name="args">Argumentos del evento</param>
        public void HandleCommand(ReportCommand command, object[] args)
        {
            if (command == ReportCommand.SaveFile)
            {
                try
                {
                    DTO_aplReporte report = new DTO_aplReporte();
                    report.DocumentoID.Value = this.documentID;

                    using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                    {
                        panel.Report.SaveLayout(memoryStream);
                        report.Data = memoryStream.ToArray();
                        //changesSaved = true;
                    }

                    panel.ReportState = ReportState.Saved;
                    _bc.AdministrationModel.aplReporte_Update(report);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReportsCommand.cs", "HandleCommand"));
                }
            }
        }
    }
}
