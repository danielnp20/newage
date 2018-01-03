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
    /// Models DTO_coAuxiliar
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_Comprobante : DTO_BasicReport
    {
        [DataMember]
        public DTO_ComprobanteHeader Header
        {
            get;
            set;
        }

        [DataMember]
        public List<DTO_ComprobanteFooter> Footer
        {
            get;
            set;
        }

        #region CamposExtra

        [DataMember]
        public DocumentoTipo TipoDoc = DocumentoTipo.DocInterno;
        [DataMember]
        public string ValorDoc = null;
        [DataMember]
        public string coDocumentoID = null;
        [DataMember]
        public string TerceroID = null;
        [DataMember]
        public string DocumentoTercero = null;
        [DataMember]
        public string CuentaID = null;
        [DataMember]
        public string ProyectoID = null;
        [DataMember]
        public string CentroCostoID = null;
        [DataMember]
        public string LineaPresupuestoID = null;
        [DataMember]
        public string LugarGeograficoID = null;
        [DataMember]
        public string Observacion = string.Empty;
        [DataMember]
        public string PrefijoID = null;
        [DataMember]
        public int DocumentoNro = 0;
        #endregion

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_Comprobante()
        {
            this.Header = new DTO_ComprobanteHeader();
            this.Footer = new List<DTO_ComprobanteFooter>();
        }

        /// <summary>
        /// Crea el header del comprobante
        /// </summary>
        /// <param h>Header del comprobante</param>
        /// <param f>Detalle del comprobante</param>
        public void AddData(DTO_ComprobanteHeader h, List<DTO_ComprobanteFooter> f)
        {
            this.Header = h;
            this.Footer = f;
        }
    }

}
