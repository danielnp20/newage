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
    public class DTO_MigrarMvtoInventario
    {
        #region Contructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_MigrarMvtoInventario()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.inReferenciaID = new UDT_inReferenciaID();
            this.EstadoInv = new UDTSQL_tinyint();
            this.Parametro1 = new UDT_ParametroInvID();
            this.Parametro2 = new UDT_ParametroInvID();
            this.ActivoID = new UDT_ActivoID();
            this.IdentificadorTr = new UDT_Consecutivo();
            this.SerialID = new UDT_SerialID();
            this.EmpaqueInvID = new UDT_EmpaqueInvID();
            this.CantidadDoc = new UDT_Cantidad();
            this.CantidadEMP = new UDT_Cantidad();
            this.CantidadUNI = new UDT_Cantidad();
            this.EntradaSalida = new UDTSQL_tinyint();
            this.DescripTExt = new UDT_Descriptivo();
            this.ValorUNI = new UDT_Valor();
            this.DocSoporte = new UDT_Consecutivo();
            this.DocSoporteTER = new UDTSQL_char(20);
            this.Valor1LOC = new UDT_Valor();
            this.Valor2LOC = new UDT_Valor();
            this.Valor1EXT = new UDT_Valor();
            this.Valor2EXT = new UDT_Valor();
        }
        #endregion

        [DataMember]
        public UDT_inReferenciaID inReferenciaID { get; set; }        

        [DataMember]
        public UDTSQL_tinyint EstadoInv { get; set; }

        [DataMember]
        public UDT_ParametroInvID Parametro1 { get; set; }

        [DataMember]
        public UDT_ParametroInvID Parametro2 { get; set; }

        [DataMember]
        public UDT_ActivoID ActivoID { get; set; }

        [DataMember]
        public UDT_Consecutivo IdentificadorTr { get; set; }

        [DataMember]
        public UDT_SerialID SerialID { get; set; }

        [DataMember]
        public UDT_EmpaqueInvID EmpaqueInvID { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadDoc { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadEMP { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadUNI { get; set; }

        [DataMember]
        public UDTSQL_tinyint EntradaSalida { get; set; }          
 
        [DataMember]
        public UDT_Descriptivo DescripTExt { get; set; }  

        [DataMember]
        public UDT_Valor ValorUNI { get; set; }

        [DataMember]
        public UDT_Consecutivo DocSoporte { get; set; }

        [DataMember]
        public UDTSQL_char DocSoporteTER { get; set; } 

        [DataMember]
        public UDT_Valor Valor1LOC { get; set; }

        [DataMember]
        public UDT_Valor Valor2LOC { get; set; }

        [DataMember]
        public UDT_Valor Valor1EXT { get; set; }

        [DataMember]
        public UDT_Valor Valor2EXT { get; set; }

    }
}


