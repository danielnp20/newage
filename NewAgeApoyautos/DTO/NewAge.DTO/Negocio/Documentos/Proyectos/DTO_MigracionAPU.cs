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
    public class DTO_MigracionAPU
    {
        #region Contructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_MigracionAPU()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.TareaID = new UDT_CodigoGrl();
            this.RecursoID = new UDT_CodigoGrl();
            this.Factor = new UDT_Cantidad();       
        }
        #endregion

        [DataMember]
        public UDT_CodigoGrl TareaID { get; set; }

        [DataMember]
        public UDT_CodigoGrl RecursoID { get; set; }

        [DataMember]
        public UDT_Cantidad Factor { get; set; }
    }
}