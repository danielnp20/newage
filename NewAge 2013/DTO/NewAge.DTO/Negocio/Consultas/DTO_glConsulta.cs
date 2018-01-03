using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_glConsultas
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glConsulta
    {

        #region Contructora

        /// <summary>
        /// Constructor
        /// </summary>
        public DTO_glConsulta(IDataReader dr)
        {
            if (dr["glConsultaID"] != DBNull.Value)
                this.glConsultaID = Convert.ToInt32(dr["glConsultaID"]);

            if (dr["Nombre"] != DBNull.Value)
                this.Nombre = dr["Nombre"].ToString();

            if (dr["DocumentoID"] != DBNull.Value)
                this.DocumentoID = Convert.ToInt32(dr["DocumentoID"]);

            if (dr["FormaDesc"] != DBNull.Value)
                this.FormaDesc = dr["FormaDesc"].ToString();

            if (dr["seUsuarioID"] != DBNull.Value)
                this.seUsuarioID = Convert.ToInt32(dr["seUsuarioID"]);

            if (dr["UsuarioID"] != DBNull.Value)
                this.UsuarioID = dr["UsuarioID"].ToString();

            if (dr["Seleccion"] != DBNull.Value)
                this.Seleccion = dr["Seleccion"].ToString();

            if (dr["Filtro"] != DBNull.Value)
                this.Filtro = dr["Filtro"].ToString();

            if (dr["Distincion"] != DBNull.Value)
                this.Distincion = (bool?)dr["Distincion"];

            if (dr["Activo"] != DBNull.Value)
                this.Activo = (bool?)dr["Activo"];

            if (dr["CtrlVersion"] != DBNull.Value)
                this.CtrlVersion = Convert.ToInt32(dr["CtrlVersion"]);

            if (dr["FechaCreacion"] != DBNull.Value)
                this.FechaCreacion = (DateTime?)dr["FechaCreacion"];

            if (dr["UsuarioCreacion"] != DBNull.Value)
                this.UsuarioCreacion = Convert.ToInt32(dr["UsuarioCreacion"]);

            if (dr["FechaAct"] != DBNull.Value)
                this.FechaAct = (DateTime?)dr["FechaAct"];

            if (dr["UsuarioAct"] != DBNull.Value)
                this.UsuarioAct = Convert.ToInt32(dr["UsuarioAct"]);

            if (dr["Prefijada"] != DBNull.Value)
                this.Prefijada = (bool)dr["Prefijada"];
            
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glConsulta()
        {

        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Gets or sets the glConsultaID
        /// </summary>
        [DataMember]
        public int glConsultaID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Nombre
        /// </summary>
        [DataMember]
        public string Nombre
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Nombre
        /// </summary>
        [DataMember]
        public int DocumentoID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Nombre
        /// </summary>
        [DataMember]
        public string FormaDesc
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Nombre
        /// </summary>
        [DataMember]
        public int seUsuarioID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Nombre
        /// </summary>
        [DataMember]
        public string Seleccion
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Nombre
        /// </summary>
        [DataMember]
        public string Filtro
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Nombre
        /// </summary>
        [DataMember]
        public bool? Distincion
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Nombre
        /// </summary>
        [DataMember]
        public bool? Activo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Nombre
        /// </summary>
        [DataMember]
        public int CtrlVersion
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Nombre
        /// </summary>
        [DataMember]
        public string UsuarioID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Nombre
        /// </summary>
        [DataMember]
        public DateTime? FechaCreacion
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Nombre
        /// </summary>
        [DataMember]
        public int UsuarioCreacion
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Nombre
        /// </summary>
        [DataMember]
        public DateTime? FechaAct
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Nombre
        /// </summary>
        [DataMember]
        public int UsuarioAct
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Prefijada
        /// </summary>
        [DataMember]
        public bool Prefijada
        {
            get;
            set;
        }

        #endregion

        #region Colecciones

        /// <summary>
        /// Filtros
        /// </summary>
        [DataMember]
        public List<DTO_glConsultaFiltro> Filtros = new List<DTO_glConsultaFiltro>();

        /// <summary>
        /// Selecciones
        /// </summary>
        [DataMember]
        public List<DTO_glConsultaSeleccion> Selecciones = new List<DTO_glConsultaSeleccion>();

        #endregion
    }
}
