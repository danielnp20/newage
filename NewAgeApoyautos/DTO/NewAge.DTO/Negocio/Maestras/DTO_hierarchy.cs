using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// DTO para trasnportar la información de una jerarquía
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_hierarchy
    {
        /// <summary>
        /// Nivel al cual se encuantra la instancia de la jerarquia
        /// </summary>
        [DataMember]
        public int NivelInstancia
        {
            get;
            set;
        }

        /// <summary>
        /// Indica la cantidad total de niveles que tiene la jerarquia
        /// </summary>
        [DataMember]
        public int NivelesJerarquia
        {
            get;
            set;
        }

        /// <summary>
        /// Contiene los codigos de los niveles de la jerarquia con los que esta relacionado el dto
        /// </summary>
        [DataMember]
        public string[] Codigos = new string[DTO_glTabla.MaxLevels];

        /// <summary>
        /// Contiene las descripciones de los niveles de la jerarquia con los que esta relacionado el dto
        /// </summary>
        [DataMember]
        public string[] Descripciones = new string[DTO_glTabla.MaxLevels];
    }
}
