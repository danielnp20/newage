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
    /// Models DTO_Convenios
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_Convenios : DTO_BasicReport
    {
        #region Propiedades
        [DataMember]
        public DTO_prConvenioSolicitudDocu Header { get; set; }

        [DataMember]
        public List<DTO_SolicitudDespachoFooter> FooterSolDespacho { get; set; }

        [DataMember]
        public List<DTO_prDetalleDocu> DetalleDocu { get; set; }

        [DataMember]
        public List<DTO_prConvenioConsumoDirecto> FooterConsumo { get; set; }

        [DataMember]
        public DTO_glDocumentoControl DocCtrl { get; set; }
        #endregion

        #region Constructores

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_Convenios()
        {
            this.Header = new DTO_prConvenioSolicitudDocu(); //Encabezado para Solicitud Despacho
            this.FooterSolDespacho = new List<DTO_SolicitudDespachoFooter>(); //Detalle de importacion Solicitud Despacho
            this.DetalleDocu = new List<DTO_prDetalleDocu>();//Detalle interno Solicitud Despacho
            this.FooterConsumo = new List<DTO_prConvenioConsumoDirecto>(); //Detalle Importacion Planilla Consumo Proy
            this.DocCtrl = new DTO_glDocumentoControl(); //Doc Control
        }
        #endregion
    }

}
