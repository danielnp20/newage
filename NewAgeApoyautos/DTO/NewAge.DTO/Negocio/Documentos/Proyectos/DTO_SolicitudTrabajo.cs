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
    /// Models DTO_SolicitudTrabajo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_SolicitudTrabajo : DTO_BasicReport
    {
        #region Propiedades

        [DataMember]
        public DTO_glDocumentoControl DocCtrl { get; set; }

        #region PreProyecto
        [DataMember]
        public DTO_pyPreProyectoDocu Header { get; set; }

        [DataMember]
        public List<DTO_pyPreProyectoTarea> Detalle { get; set; }

        [DataMember]
        public List<DTO_pyPreProyectoTarea> DetalleTareasAdic { get; set; } 
        #endregion

        #region Proyecto
        [DataMember]
        public DTO_pyProyectoDocu HeaderProyecto { get; set; }

        [DataMember]
        public List<DTO_pyProyectoTarea> DetalleProyecto { get; set; }

        [DataMember]
        public List<DTO_pyProyectoTarea> DetalleProyectoTareaAdic { get; set; }

        [DataMember]
        public List<DTO_pyProyectoMvto> Movimientos { get; set; } 
        #endregion

        [DataMember]
        public List<DTO_pyActaTrabajoDeta> ActaTrabajosDeta { get; set; }

        [DataMember]
        public List<DTO_QueryTrazabilidad> ResumenTrazabilidad { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaDoc { get; set; }

        [DataMember]
        public UDT_DescripTBase CorreoUsuario { get; set; }

        [DataMember]
        public UDT_DescripTBase TelefonoUsuario { get; set; }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_SolicitudTrabajo()
        {
            this.Header = new DTO_pyPreProyectoDocu();
            this.Detalle = new List<DTO_pyPreProyectoTarea>();
            this.DetalleTareasAdic = new List<DTO_pyPreProyectoTarea>();
            this.HeaderProyecto = new DTO_pyProyectoDocu();
            this.DetalleProyecto = new List<DTO_pyProyectoTarea>();
            this.DetalleProyectoTareaAdic = new List<DTO_pyProyectoTarea>();
            this.DocCtrl = new DTO_glDocumentoControl();
            this.Movimientos = new List<DTO_pyProyectoMvto>();
            this.ActaTrabajosDeta = new List<DTO_pyActaTrabajoDeta>();
            this.ResumenTrazabilidad = new List<DTO_QueryTrazabilidad>();
            this.FechaDoc = new UDTSQL_smalldatetime();
            this.TelefonoUsuario = new UDT_DescripTBase();
            this.CorreoUsuario = new UDT_DescripTBase();
        }
        #endregion
    }

}
