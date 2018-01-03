using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_MigracionGrupos
    {
        #region Contructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_MigracionGrupos()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.CapituloTareaID = new UDT_CodigoGrl10();
            this.CapituloGrupoID = new UDT_CodigoGrl();
            this.Descriptivo = new UDT_DescripTBase();
        }
        #endregion

        [DataMember]
        public UDT_CodigoGrl10 CapituloTareaID { get; set; }

        [DataMember]
        public UDT_CodigoGrl CapituloGrupoID { get; set; }
                
        [DataMember]
        public UDT_DescripTBase Descriptivo { get; set; }
    }
}


