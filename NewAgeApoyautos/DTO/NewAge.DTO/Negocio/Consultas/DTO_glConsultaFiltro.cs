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
    /// Models DTO_glConsultaFiltro
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glConsultaFiltro
    {
        #region Contructora

        /// <summary>
        /// Constructor
        /// </summary>
        public DTO_glConsultaFiltro(IDataReader dr)
        {
            if (dr["glConsultaFiltroID"] != DBNull.Value)
                this.glConsultaFiltroID = Convert.ToInt32(dr["glConsultaFiltroID"]);

            if (dr["glConsultaFiltroGrupo"] != DBNull.Value)
                this.glConsultaFiltroGrupo = Convert.ToInt32(dr["glConsultaFiltroGrupo"]);
            
            if (dr["glConsultaID"] != DBNull.Value)
                this.glConsultaID = Convert.ToInt32(dr["glConsultaID"]);

            if (dr["CampoFisico"] != DBNull.Value)
                this.CampoFisico = dr["CampoFisico"].ToString();

            if (dr["CampoDesc"] != DBNull.Value)
                this.CampoDesc = dr["CampoDesc"].ToString();

            if (dr["OperadorFiltro"] != DBNull.Value)
                this.OperadorFiltro = dr["OperadorFiltro"].ToString();

            if (dr["ValorFiltro"] != DBNull.Value)
                this.ValorFiltro = dr["ValorFiltro"].ToString();

            if (dr["OperadorSentencia"] != DBNull.Value)
                this.OperadorSentencia = dr["OperadorSentencia"].ToString();

            if (dr["Idx"] != DBNull.Value)
                this.Idx = Convert.ToInt32(dr["Idx"]);

        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glConsultaFiltro()
        {
            this.glConsultaID = null;
            this.glConsultaFiltroID = null;
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Gets or sets the glConsultaID
        /// </summary>
        [DataMember]
        public int? glConsultaFiltroID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the glConsultaFiltroGrupo
        /// </summary>
        [DataMember]
        public int glConsultaFiltroGrupo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Nombre
        /// </summary>
        [DataMember]
        public int? glConsultaID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Nombre
        /// </summary>
        [DataMember]
        public string CampoFisico
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Nombre
        /// </summary>
        [DataMember]
        public string CampoDesc
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Nombre
        /// </summary>
        [DataMember]
        public string OperadorFiltro
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Nombre
        /// </summary>
        [DataMember]
        public string ValorFiltro
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Nombre
        /// </summary>
        [DataMember]
        public string OperadorSentencia
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Nombre
        /// </summary>
        [DataMember]
        public int Idx
        {
            get;
            set;
        }

        #endregion
    }

    /// <summary>
    /// Clase con definición de filtros complejos
    /// </summary>
    public class DTO_glConsultaFiltroComplejo : DTO_glConsultaFiltro
    {
        /// <summary>
        /// Crea un filtro complejo
        /// </summary>
        /// <param name="docId">Documento sobre el cual se esta aplicando el filtro</param>
        /// <param name="llavesFK">Diccionario con las Fks</param>
        /// <param name="filtrosDetalle">Lista de filtros</param>
        /// <param name="fieldTypes">Tipos de campos</param>
        public DTO_glConsultaFiltroComplejo(int docId, Dictionary<string, string> llavesFK, List<DTO_glConsultaFiltro> filtrosDetalle, Dictionary<string, Type> fieldTypes=null)
        {
            this.DocumentoIDFK = docId;
            this.LlavesFK = llavesFK;
            this.FiltrosDetalle = filtrosDetalle;
            this.FieldTypes = fieldTypes == null ? new Dictionary<string, Type>() : fieldTypes;
        }
        
        /// <summary>
        /// DocumentoID de la tabla a la que se hace referencia en el filtro complejo
        /// </summary>
        public int DocumentoIDFK
        {
            get;
            set;
        }

        /// <summary>
        /// Tipos de los campos. Solo es necesario para los que no se tomen automaticamente
        /// </summary>
        public Dictionary<string, Type> FieldTypes
        {
            get;
            set;
        }

        private Dictionary<string, string> _llavesFK = new Dictionary<string, string>();

        /// <summary>
        /// Llaves con las que se hace la relacion. La primera es la de la tabla desde la que se hace la relación 
        /// y la segunda la tabla a la que corresponde en la talba referenciada
        /// </summary>
        public Dictionary<string, string> LlavesFK
        {
            get { return _llavesFK; }
            set { _llavesFK = value; }
        }

        private List<DTO_glConsultaFiltro> _filtrosDetalle = new List<DTO_glConsultaFiltro>();

        /// <summary>
        /// Filtros que se le aplican a la tabla referenciada. Pueden ser normales o complejos
        /// </summary>
        public List<DTO_glConsultaFiltro> FiltrosDetalle
        {
            get { return _filtrosDetalle; }
            set { _filtrosDetalle = value; }
        }
    }
}
