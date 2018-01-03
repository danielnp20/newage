using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewAge.Cliente.GUI.WinApp.Componentes.ReportParameters
{
    public interface IMultiReportParameter
    {
        string[] GetSelectedValue();

        void SetItems(string name, List<ReportParameterListItem> items);
    }
}
