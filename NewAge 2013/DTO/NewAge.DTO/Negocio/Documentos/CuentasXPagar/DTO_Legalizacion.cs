using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using NewAge.DTO.Negocio.Reportes;
using NewAge.DTO.Reportes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_cpLegalizacion
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_Legalizacion : DTO_BasicReport
    {
        [DataMember]
        public DTO_cpLegalizaDocu Header { get; set;}

        [DataMember]
        public List<DTO_cpLegalizaFooter> Footer { get; set;}

        [DataMember]
        public DTO_glDocumentoControl DocCtrl { get; set;}

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_Legalizacion()
        {
            this.Header = new DTO_cpLegalizaDocu();
            this.Footer = new List<DTO_cpLegalizaFooter>();
            this.DocCtrl = new DTO_glDocumentoControl();            
        }

        /// <summary>
        /// Crea el header de la legalizacion
        /// </summary>
        /// <param h>Header de la legalizacion</param>
        /// <param f>Detalle de la legalizacion</param>
        /// <param dc>Documento Control asociado</param>
        public void AddData(DTO_cpLegalizaDocu h, List<DTO_cpLegalizaFooter> f, DTO_glDocumentoControl dc)
        {
            this.Header = h;
            this.Footer = f;
            this.DocCtrl = dc;
        }
    }

}
