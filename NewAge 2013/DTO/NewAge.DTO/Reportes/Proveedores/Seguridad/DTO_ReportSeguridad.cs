using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Runtime.Serialization;

namespace NewAge.DTO.Reportes
{
    [DataContract]
    [Serializable]

    /// <summary>
    /// Clase del reporte Seguridad
    /// </summary>
    public class DTO_ReportSeguridad
    { 
        /// <summary>
        /// Report de las acciones con los Masters
        /// </summary>
        public List<DTO_Masters> Master_Report{ get; set; }

        /// <summary>
        /// Report de las acciones con los Procesos
        /// </summary>
        public List<DTO_Process> Process_Report { get; set; }

        /// <summary>
        /// Report de las acciones con las Bitacoras
        /// </summary>
        public List<DTO_Bitacora> Bitacora_Report { get; set; }

        /// <summary>
        /// Report de las acciones con los Reportes
        /// </summary>
        public List<DTO_Reports> Report_Report { get; set; }

        /// <summary>
        /// Report de las acciones con Queries
        /// </summary>
        public List<DTO_Queries> Query_Report { get; set; }

        /// <summary>
        /// Report de las acciones con los Documentos
        /// </summary>
        public List<DTO_Documents> Document_Report { get; set; }

        /// <summary>
        /// Report de las acciones con los Documentos Aprobados
        /// </summary>
        public List<DTO_DocumentsAprob> DocumentAprob_Report { get; set; }
    }

    /// <summary>
    /// Clase del reporte Seguridad de los Masters
    /// </summary>
    public class DTO_Masters
    {
        /// <summary>
        /// Modulo ID
        /// </summary>
        public string ModuloID {get; set;}

        /// <summary>
        /// Documento ID
        /// </summary>
        public int DocumentoID { get; set; }

        /// <summary>
        /// Descripcion Documento
        /// </summary>
        public string DocumentoDesc { get; set; }

        /// <summary>
        /// Indicador de la accion "Get"
        /// </summary>
        public string Get { get; set; }

        /// <summary>
        /// Indicador de la accion "Add"
        /// </summary>
        public string Add { get; set; }

        /// <summary>
        /// Indicador de la accion "Edit"
        /// </summary>
        public string Edit { get; set; }

        /// <summary>
        /// Indicador de la accion "Delete"
        /// </summary>
        public string Delete { get; set; }

        /// <summary>
        /// Indicador de la accion "Print"
        /// </summary>
        public string Print { get; set; }

        /// <summary>
        /// Indicador de la accion "GenerateTemplate"
        /// </summary>
        public string GenerateTemplate { get; set; }

        /// <summary>
        /// Indicador de la accion "Import"
        /// </summary>
        public string Import { get; set; }

        /// <summary>
        /// Indicador de la accion "Export"
        /// </summary>
        public string Export { get; set; }

        /// <summary>
        /// Indicador de la accion "ResetPassword"
        /// </summary>
        public string ResetPassword { get; set; }
    }

    /// <summary>
    /// Clase del reporte Seguridad de los Procesos
    /// </summary>
    public class DTO_Process
    {
        /// <summary>
        /// Modulo ID
        /// </summary>
        public string ModuloID { get; set; }

        /// <summary>
        /// Documento ID
        /// </summary>
        public int DocumentoID { get; set; }

        /// <summary>
        /// Descripcion Documento
        /// </summary>
        public string DocumentoDesc { get; set; }

        /// <summary>
        /// Indicador de la accion "Get"
        /// </summary>
        public string Get { get; set; }
    }

    /// <summary>
    /// Clase del reporte Seguridad de las Bitacoras
    /// </summary>
    public class DTO_Bitacora
    {
        /// <summary>
        /// Modulo ID
        /// </summary>
        public string ModuloID { get; set; }

        /// <summary>
        /// Documento ID
        /// </summary>
        public int DocumentoID { get; set; }

        /// <summary>
        /// Descripcion Documento
        /// </summary>
        public string DocumentoDesc { get; set; }

        /// <summary>
        /// Indicador de la accion "Get"
        /// </summary>
        public string Get { get; set; }

        /// <summary>
        /// Indicador de la accion "Print"
        /// </summary>
        public string Print { get; set; }

        /// <summary>
        /// Indicador de la accion "Export"
        /// </summary>
        public string Export { get; set; }
    }

    /// <summary>
    /// Clase del reporte Seguridad de los Reportes
    /// </summary>
    public class DTO_Reports
    {
        /// <summary>
        /// Modulo ID
        /// </summary>
        public string ModuloID { get; set; }

        /// <summary>
        /// Documento ID
        /// </summary>
        public int DocumentoID { get; set; }

        /// <summary>
        /// Descripcion Documento
        /// </summary>
        public string DocumentoDesc { get; set; }

        /// <summary>
        /// Indicador de la accion "Get"
        /// </summary>
        public string Get { get; set; }
    }

    /// <summary>
    /// Clase del reporte Seguridad de los Queries
    /// </summary>
    public class DTO_Queries
    {
        /// <summary>
        /// Modulo ID
        /// </summary>
        public string ModuloID { get; set; }

        /// <summary>
        /// Documento ID
        /// </summary>
        public int DocumentoID { get; set; }

        /// <summary>
        /// Descripcion Documento
        /// </summary>
        public string DocumentoDesc { get; set; }

        /// <summary>
        /// Indicador de la accion "Get"
        /// </summary>
        public string Get { get; set; }
    }

    /// <summary>
    /// Clase del reporte Seguridad de los Documentos
    /// </summary>
    public class DTO_Documents
    {
        /// <summary>
        /// Modulo ID
        /// </summary>
        public string ModuloID { get; set; }

        /// <summary>
        /// Documento ID
        /// </summary>
        public int DocumentoID { get; set; }

        /// <summary>
        /// Descripcion Documento
        /// </summary>
        public string DocumentoDesc { get; set; }

        /// <summary>
        /// Indicador de la accion "Get"
        /// </summary>
        public string Get { get; set; }

        /// <summary>
        /// Indicador de la accion "Add"
        /// </summary>
        public string Add { get; set; }

        /// <summary>
        /// Indicador de la accion "Edit"
        /// </summary>
        public string Edit { get; set; }

        /// <summary>
        /// Indicador de la accion "Delete"
        /// </summary>
        public string Delete { get; set; }

        /// <summary>
        /// Indicador de la accion "Print"
        /// </summary>
        public string Print { get; set; }

        /// <summary>
        /// Indicador de la accion "GenerateTemplate"
        /// </summary>
        public string GenerateTemplate { get; set; }

        /// <summary>
        /// Indicador de la accion "Copy"
        /// </summary>
        public string Copy { get; set; }

        /// <summary>
        /// Indicador de la accion "Paste"
        /// </summary>
        public string Paste { get; set; }

        /// <summary>
        /// Indicador de la accion "Import"
        /// </summary>
        public string Import { get; set; }

        /// <summary>
        /// Indicador de la accion "Export"
        /// </summary>
        public string Export { get; set; }

        /// <summary>
        /// Indicador de la accion "Revert"
        /// </summary>
        public string Revert { get; set; }

        /// <summary>
        /// Indicador de la accion "CancelTx"
        /// </summary>
        public string CancelTx { get; set; }

        /// <summary>
        /// Indicador de la accion "SendtoAppr"
        /// </summary>
        public string SendtoAppr { get; set; }
    }

    /// <summary>
    /// Clase del reporte Seguridad de los Documentos Aprobados
    /// </summary>
    public class DTO_DocumentsAprob
    {
        /// <summary>
        /// Modulo ID
        /// </summary>
        public string ModuloID { get; set; }

        /// <summary>
        /// Documento ID
        /// </summary>
        public int DocumentoID { get; set; }

        /// <summary>
        /// Descripcion Documento
        /// </summary>
        public string DocumentoDesc { get; set; }

        /// <summary>
        /// Indicador de la accion "Get"
        /// </summary>
        public string Get { get; set; }
    }
}
