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
    public class DTO_ExportRecursosDeta
    {
        #region Contructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ExportRecursosDeta()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.TareaCliente = new UDT_CodigoGrl20();
            this.TareaID = new UDT_CodigoGrl();
            this.Descripcion = new UDT_DescripTExt();           
            this.RecursoID = new UDT_CodigoGrl();
            this.RecursoDesc = new UDT_Descriptivo();
            this.UnidadInvID = new UDT_UnidadInvID();
            this.CostoLocal = new UDT_Valor();
            this.FactorID = new UDT_Cantidad();
            this.Peso_Cantidad = new UDT_Cantidad();
            this.Distancia_Turnos = new UDT_Cantidad();
        }
        #endregion

        [DataMember]
        public UDT_CodigoGrl20 TareaCliente { get; set; }

        [DataMember]
        public UDT_CodigoGrl TareaID { get; set; }

        [DataMember]
        public UDT_DescripTExt Descripcion { get; set; }

        [DataMember]
        public UDT_CodigoGrl RecursoID { get; set; }

        [DataMember]
        public UDT_Descriptivo RecursoDesc { get; set; }

        [DataMember]
        public UDT_UnidadInvID UnidadInvID { get; set; }

        [DataMember]
        public UDT_Valor CostoLocal { get; set; }

        [DataMember]
        public UDT_Cantidad FactorID { get; set; }

        [DataMember]
        public UDT_Cantidad Peso_Cantidad { get; set; }

        [DataMember]
        public UDT_Cantidad Distancia_Turnos { get; set; }

    }
}


