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
    public class DTO_ExportMovimiento
    {
        #region Contructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ExportMovimiento()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.DocumentoID = new UDT_DocumentoID();
            this.Fecha_Movimiento = new UDTSQL_smalldatetime();
            this.FechaAplicacion = new UDT_DescripTExt();
            this.NroCredito = new UDT_DocTerceroID();
            this.ClienteID = new UDT_ClienteID();
            this.Nom_Cliente = new UDT_Descriptivo();
            this.DOCUMENTO = new UDT_DescripTBase();
            this.COMPROBANTE = new UDT_DescripTBase();
            this.TotalDocumento = new UDT_Valor();
            this.TotalCuota = new UDT_Valor();
        }
        #endregion

        [DataMember]
        public UDT_DocumentoID DocumentoID { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime Fecha_Movimiento { get; set; }

        [DataMember]
        public UDT_DescripTExt FechaAplicacion { get; set; }

        [DataMember]
        public UDT_DocTerceroID NroCredito { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        public UDT_Descriptivo Nom_Cliente { get; set; }
       
        [DataMember]
        public UDT_DescripTBase DOCUMENTO { get; set; }

        [DataMember]
        public UDT_DescripTBase COMPROBANTE { get; set; }

        [DataMember]
        public UDT_Valor TotalDocumento { get; set; }

        [DataMember]
        public UDT_Valor TotalCuota { get; set; }

        //[DataMember]
        //public List<DTO_QueryCarteraMvtoDeta> Detalle { get; set; }

    }
}


