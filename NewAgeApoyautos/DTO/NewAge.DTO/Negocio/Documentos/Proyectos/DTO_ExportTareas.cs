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
    public class DTO_ExportTareas
    {
        #region Contructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ExportTareas()
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
            this.UnidadInv = new UDT_Descriptivo();
            this.Cantidad = new UDT_Cantidad();
            this.CostoLocalCLI = new UDT_Valor();
            // this.Borrar = new UDTSQL_char(1);
        }
        #endregion

        [DataMember]
        public UDT_CodigoGrl TareaCliente { get; set; }     

        [DataMember]
        public UDT_DescripTExt Descripcion { get; set; }

        [DataMember]
        public UDT_Descriptivo UnidadInv { get; set; }

        [DataMember]
        public UDT_Cantidad Cantidad { get; set; }

        [DataMember]
        public UDT_Valor CostoLocalCLI { get; set; }

        //[DataMember]
        //public UDTSQL_char Borrar { get; set; }

    }
}


