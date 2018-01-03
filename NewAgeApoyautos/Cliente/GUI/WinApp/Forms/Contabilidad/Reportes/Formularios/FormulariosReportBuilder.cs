using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using System.Collections;
using SentenceTransformer;
using System.Reflection;
using NewAge.DTO.Reportes;
using NewAge.DTO.Negocio.Reportes;
using NewAge.DTO.Negocio;
using NewAge.ReportesComunes;
using DevExpress.XtraReports.UI;

namespace NewAge.Cliente.GUI.WinApp.Reports.Formularios
{
    class FormulariosReportBuilder
    {
        #region Variables
        BaseController _bc = BaseController.GetInstance();
        protected DTO_glConsulta Consulta = null;
        #endregion

        #region Funciones Publicas

        /// <summary>
        ///  Builder for Formularios (collects and organises report data)
        /// </summary>
        /// <param name="Filtros">list of the filters that are applied by user</param>
        /// <param name="Date">formulario period</param>
        /// <param name="impType">Formulario type (Retefuenta,IVA,Renta etc.)</param>
        /// <param name="reportType">Type of report (Declaracion, por Cuenta, Datailed)</param>
        public FormulariosReportBuilder(List<DTO_glConsultaFiltro> Filtros, int year, int period, bool preInd, string impType, string reportType)
        {
            Consulta = new DTO_glConsulta();
            Consulta.Filtros.AddRange(Filtros);

            int declarType = 0;
            if (impType == this._bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoIVA))
                declarType = 2;
            if (impType == this._bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteFuente))
                declarType = 1;
            if (impType == this._bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteICA))
                declarType = 4;

            switch (reportType)
            {
                #region Formularios data (tipo "Cuenta")
                case "Cuenta":
                    List<DTO_FormulariosCuenta> formCuentaData = new List<DTO_FormulariosCuenta>();

                    List<DTO_BasicReport> dataFC = _bc.AdministrationModel.GetReportData(AppReports.coFormulariosCuenta, Consulta).ToList();
                    DTO_BasicReport.FillRompimiento(dataFC, "Renglon");
                    foreach (DTO_BasicReport itemFC in dataFC)
                    {
                        formCuentaData.Add((DTO_FormulariosCuenta)itemFC);
                    };

                    //if (declarType == "5")
                    //{
                    //    List<DTO_BasicReport> dataFC_bal = _bc.AdministrationModel.GetReportData(AppReports.FormulariosCuenta_balance, Consulta).ToList();
                    //    DTO_BasicReport.FillRompimiento(dataFC_bal, "Renglon");
                    //    foreach (DTO_BasicReport itemFC_bal in dataFC_bal)
                    //    {
                    //        formCuentaData.Add((DTO_FormulariosCuenta)itemFC_bal);
                    //    };
                    //};
                    formCuentaData = formCuentaData.OrderBy(x => x.CuentaID).ToList();
                    formCuentaData = formCuentaData.OrderBy(x => x.Renglon).ToList();

                    #region Report field list
                    ArrayList fieldListFC = new ArrayList();
                    fieldListFC.AddRange(ColumnsInfo.FormCuentaFields); 
                    #endregion

                    FormulariosCuentaReport formCuenta = new FormulariosCuentaReport(formCuentaData, fieldListFC, year, period, preInd, declarType.ToString());
                    formCuenta.ShowPreview();
                    break;
                #endregion

                #region Formularios data (tipo "Soporte")
                case "Soporte":
                    List<DTO_FormulariosDetail> formDetData = new List<DTO_FormulariosDetail>();

                    List<DTO_BasicReport> dataFD = _bc.AdministrationModel.GetReportData(AppReports.coFormulariosDetail, Consulta).ToList();
                    DTO_BasicReport.FillRompimiento(dataFD, "Renglon", "CuentaID");
                    foreach (DTO_BasicReport itemFD in dataFD)
                    {
                        formDetData.Add((DTO_FormulariosDetail)itemFD);
                    };

                    formDetData = formDetData.OrderBy(x => x.TerceroDesc).ToList();
                    formDetData = formDetData.OrderBy(x => x.TerceroID).ToList();
                    formDetData = formDetData.OrderBy(x => x.CuentaID).ToList();
                    formDetData = formDetData.OrderBy(x => x.Renglon).ToList();

                    #region Report field list
                    ArrayList fieldListFD = new ArrayList();
                    fieldListFD.AddRange(ColumnsInfo.FormSoporteFields); 
                    #endregion

                    FormulariosDetailReport formDet = new FormulariosDetailReport(formDetData, fieldListFD, year, period, preInd, declarType.ToString());
                    formDet.ShowPreview();
                    break;
                #endregion
            };
        } 
        #endregion
    }
}
