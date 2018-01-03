using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.DTO.Negocio;
using NewAge.Cliente.GUI.WinApp.Clases;
using SentenceTransformer;

namespace NewAge.Cliente.GUI.WinApp
{
    public interface IFiltrable
    {
        /// <summary>
        /// Asigna una consulta desde MasterQuery para hacer el filtrado de datos
        /// </summary>
        /// <param name="consulta"></param>
        /// <param name="fields"></param>
        void SetConsulta(DTO_glConsulta consulta, List<ConsultasFields> fields);
    }
}
