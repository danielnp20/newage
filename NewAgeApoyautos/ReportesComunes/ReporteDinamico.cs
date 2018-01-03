using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;
using NewAge.DTO.Reportes;
using DevExpress.XtraReports.UserDesigner;
using DevExpress.XtraPrinting;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;

namespace NewAge.ReportesComunes
{
    [Serializable]
    public partial class ReporteDinamico : BaseCommonReport
    {
        public ReporteDinamico(CommonReportDataSupplier supplier)
            : base(supplier)
        {
            InitializeComponent();

            try
            {
                PrintTool.MakeCommandResponsive(this.PrintingSystem);
                PrintingSystem.SetCommandVisibility(PrintingSystemCommand.Save, DevExpress.XtraPrinting.CommandVisibility.None);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
