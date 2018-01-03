using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Reportes;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Models DTO_SolicitudLibranza
    [Serializable]
    [DataContract]
    public class DTO_SolicitudLibranza
    /// </summary>
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_SolicitudLibranza()
        {
            this.Header = new DTO_ccSolicitudDocu();
            this.DocCtrl = new DTO_glDocumentoControl();
            this.Anexos = new List<DTO_ccSolicitudAnexo>();
            this.TareasChequeos = new List<DTO_ccTareaChequeoLista>();
            this.Detalle = new List<DTO_SolicitudLibranza>();
            //this.PlantillaCarta = new UDTSQL_varcharMAX();
        }

        /// <summary>
        /// Crea el header
        /// </summary>
        /// <param h>Header </param>
        /// <param f>Detalle </param>
        public void AddData(DTO_glDocumentoControl dc,DTO_ccSolicitudDocu h, List<DTO_ccSolicitudAnexo> ane, List<DTO_ccTareaChequeoLista> tareas)
        {
            this.Header = h;
            this.Anexos = ane;
            this.DocCtrl = dc;
            this.TareasChequeos = tareas;
        }

        #region Propiedades

        [DataMember]
        public DTO_ccSolicitudDocu Header
        {
            get;
            set;
        }

        [DataMember]
        public DTO_glDocumentoControl DocCtrl
        {
            get;
            set;
        }

        [DataMember]
        public List<DTO_ccSolicitudAnexo> Anexos
        {
            get;
            set;
        }

        [DataMember]
        public List<DTO_ccTareaChequeoLista> TareasChequeos
        {
            get;
            set;
        }

        [DataMember]
        [NotImportable]
        public List<DTO_SolicitudLibranza> Detalle { get; set; }

        [DataMember]
        [NotImportable]
        public string PlantillaCarta { get; set; }

        #endregion
    }
}