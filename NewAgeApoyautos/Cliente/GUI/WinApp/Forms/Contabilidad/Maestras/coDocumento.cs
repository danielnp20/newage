using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using NewAge.DTO.Negocio;
using NewAge.Cliente.GUI.WinApp.Clases;
using System.Windows.Forms;
using NewAge.Librerias.Project;
using NewAge.DTO.Resultados;
using NewAge.DTO.UDT;
using SentenceTransformer;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    class coDocumento : MasterSimpleForm
    {
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        
        ///<summary>
        /// Constructor 
        /// </summary>
        public coDocumento() : base() { }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.DocumentID = AppMasters.coDocumento;
            base.InitForm();
        }

        /// <summary>
        /// Sobrecargar para modificar alguna configuración de campo de la maestra
        /// </summary>
        public override void CustomizeFieldsConfig()
        {
            string imp = string.Empty;
            try
            {
                ButtonEditFKConfiguration fc = (ButtonEditFKConfiguration)this.GetFieldConfigByFieldName("DocumentoID");
                DTO_glConsultaFiltro fil = new DTO_glConsultaFiltro()
                {
                    CampoFisico = "DocumentoTipo",
                    OperadorFiltro = OperadorFiltro.Igual,
                    ValorFiltro = "6"
                };
                List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();
                filtros.Add(fil);
                fc.Filtros = filtros;
            }
            catch (Exception)
            {
                ;
            }
        }
    }
}
