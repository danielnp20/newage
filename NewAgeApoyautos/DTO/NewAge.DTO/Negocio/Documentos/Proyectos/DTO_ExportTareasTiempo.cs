using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_ExportTareasTiempos
    {
        #region Constructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ExportTareasTiempos()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.TareaCliente = new UDT_CodigoGrl();
            this.Descripcion = new UDT_DescripTExt();
            this.UnidadInv = new UDT_UnidadInvID();
            this.FechaInicio = new UDTSQL_smalldatetime();
            this.FechaFin = new UDTSQL_smalldatetime();
            // this.Borrar = new UDTSQL_char(1);
        }
        #endregion

        [DataMember]
        public UDT_CodigoGrl TareaCliente { get; set; }     

        [DataMember]
        public UDT_DescripTExt Descripcion { get; set; }

        [DataMember]
        public UDT_UnidadInvID UnidadInv { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaInicio { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaFin { get; set; }

    }
}


