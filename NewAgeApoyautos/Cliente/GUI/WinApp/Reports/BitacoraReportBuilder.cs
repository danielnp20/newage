using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.DTO.Negocio;
using SentenceTransformer;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Cliente.GUI.WinApp.Forms;
using NewAge.Librerias.Project;
using System.Collections;
using DevExpress.XtraReports.UI;

namespace NewAge.Cliente.GUI.WinApp.Reports
{
    class BitacoraReportBuilder : IFiltrable
    {
        BaseController _bc = BaseController.GetInstance();

        protected DTO_glConsulta Consulta = null;

        /// <summary>
        /// Builder for Bitacora Report (collects and organises report data)
        /// </summary>
        /// <param name="documentId">ID of the current document allowing to get information about it</param>
        /// <param name="Acciones"></param>
        /// <param name="data">data for the report</param>
        public BitacoraReportBuilder(int documentId, Dictionary<short, string> Acciones, List<DTO_aplBitacora> data)
        {
            #region Filter form
            MasterQuery mq = new MasterQuery(this, documentId, 1, true, typeof(DTO_aplBitacora), new List<string> { "Actualizaciones", "Usuario", "Empresa", "Documento", "Accion" });
            mq.SetFK("DocumentoID", AppMasters.glDocumento, _bc.CreateFKConfig(AppMasters.glDocumento));
            mq.SetFK("EmpresaID", AppMasters.glEmpresa, _bc.CreateFKConfig(AppMasters.glEmpresa));
            mq.SetFK("seUsuarioID", AppMasters.seUsuario, _bc.CreateFKConfig(AppMasters.seUsuario));
            Dictionary<string, string> filterAcciones = new Dictionary<string, string>();
            foreach (KeyValuePair<short, string> kvp in Acciones)
                filterAcciones.Add(kvp.Key.ToString(), kvp.Value);

            mq.SetValueDictionary("AccionID", filterAcciones);
            mq.ShowDialog();
            #endregion

            #region List of fields for report
            #region Main table fieltered field list
            Dictionary<string, string> descFields = new Dictionary<string, string>();
            descFields.Add("seUsuarioID", "Usuario");
            descFields.Add("EmpresaID", "Empresa");
            descFields.Add("DocumentoID", "Documento");
            descFields.Add("AccionID", "Accion");

            ArrayList filteredFieldList = new ArrayList();

            if (this.Consulta != null && this.Consulta.Selecciones != null)
            {
                foreach (DTO_glConsultaSeleccion sel in this.Consulta.Selecciones)
                {
                    filteredFieldList.Add(sel.CampoFisico);
                    if (descFields.ContainsKey(sel.CampoFisico))
                    {
                        filteredFieldList.Add(descFields[sel.CampoFisico]);
                    };
                };
            };

            ArrayList fieldList = new ArrayList()
            { 
               "BitacoraID",               
               "EmpresaID", "Empresa",
               "DocumentoID", "Documento",
               "AccionID", "Accion",
               "Fecha",
               "seUsuarioID", "Usuario",
               "llp01","llp02","llp03","llp04","llp05","llp06",
               "BitacoraOrigenID", 
               "BitacoraPadreID", 
               "BitacoraAnulacionID"
            };

            for (int i = 0; i < fieldList.Count; i++)
            {
                if (!filteredFieldList.Contains(fieldList[i]))
                {
                    fieldList.Remove(fieldList[i]);
                    i--;
                };
            };

            #endregion
            #region Sub Table field list
            ArrayList subFieldList = new ArrayList()
            {            
            //"BitacoraID",
            //"DocumentoID",
            "NombreCampo",
            "Valor"
            };
            #endregion
            #endregion

            BitacoraReport report = new BitacoraReport(documentId, data, fieldList, subFieldList);

            report.ShowPreview();
        }

        /// <summary>
        /// Asigna una consulta desde MasterQuery para hacer el filtrado de datos
        /// </summary>
        /// <param name="consulta"></param>
        /// <param name="fields"></param>
        void IFiltrable.SetConsulta(DTO_glConsulta consulta, List<ConsultasFields> fields)
        {
            try
            {
                this.Consulta = consulta;
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }        
        }

        
    }    
}
