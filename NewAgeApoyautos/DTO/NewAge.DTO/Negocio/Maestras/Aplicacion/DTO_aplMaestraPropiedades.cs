using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.Librerias.Project;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models aplMaestraParametro
    /// </summary>
    [DataContract]
    [Serializable]
    [KnownType(typeof(ModulesPrefix))]
    [KnownType(typeof(GrupoEmpresa))]
    public class DTO_aplMaestraPropiedades
    {
        #region Contructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_aplMaestraPropiedades(IDataReader dr)
        {
            this.InitCols();

            this.DocumentoID = Convert.ToInt32(dr["DocumentoID"]);
            this.NombreTabla = dr["NombreTabla"].ToString();
            this.DTOTipo = dr["DTOTipo"].ToString();
            this.ColumnaID = Convert.ToString(dr["ColumnaID"]);
            this.IDLongitudMax = Convert.ToInt16(dr["IDLongitudMax"]);
            this.ModuloID = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), dr["ModuloID"].ToString());
            this.GrupoEmpresaInd = Convert.ToBoolean(dr["GrupoEmpresaInd"]);
            this.TipoSeguridad = (GrupoEmpresa)Enum.Parse(typeof(GrupoEmpresa), dr["TipoSeguridad"].ToString());
        } 

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_aplMaestraPropiedades()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Campos = new List<DTO_aplMaestraCampo>();
        }

        #endregion
        
        /// <summary>
        /// Gets or sets the DocumentoID
        /// </summary>
        [DataMember]
        public int DocumentoID { get; set; }

        /// <summary>
        /// Gets or sets the NombreTabla
        /// </summary>
        [DataMember]
        public string NombreTabla { get; set; }

        /// <summary>
        /// Gets or sets the DTOTipo
        /// </summary>
        [DataMember]
        public string DTOTipo { get; set; }

        /// <summary>
        /// Gets or sets the ColumnaID
        /// </summary>
        [DataMember]
        public string ColumnaID { get; set; }

        /// <summary>
        /// Gets or sets the IDLongitudMax
        /// </summary>
        [DataMember]
        public int IDLongitudMax { get; set; }

        /// <summary>
        /// Gets or sets the ModuloID
        /// </summary>
        [DataMember]
        public ModulesPrefix ModuloID { get; set; }

        /// <summary>
        /// Gets or sets the EmpresaInd
        /// </summary>
        [DataMember]
        public bool GrupoEmpresaInd { get; set; }

        /// <summary>
        /// Gets or sets the GrupoEmpresaIndividualInd
        /// </summary>
        [DataMember]
        public GrupoEmpresa TipoSeguridad { get; set; }

        /// <summary>
        /// Gets or sets the list of extra fields
        /// </summary>
        [DataMember]
        public List<DTO_aplMaestraCampo> Campos { get; set; }
    }
}
