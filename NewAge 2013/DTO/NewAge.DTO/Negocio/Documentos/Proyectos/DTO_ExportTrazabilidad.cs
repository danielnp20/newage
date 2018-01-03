using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// 
    /// Models DTO_ExportTrazabilidad
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ExportTrazabilidad
    {
        #region DTO_ExportTrazabilidad

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ExportTrazabilidad()
        {
            this.InitCols();
            this.CantPresupuestado.Value = 0;
            this.VlrPresupuestado.Value = 0;
            this.CantSolicitado.Value = 0;
            this.VlrSolicitado.Value = 0;
            this.CantComprado.Value = 0;
            this.VlrComprado.Value = 0;
            this.CantRecibido.Value = 0;
            this.VlrRecibido.Value = 0;
            this.CantConsumido.Value = 0;
            this.VlrConsumido.Value = 0;
            this.CantFacturado.Value = 0;
            this.VlrFacturado.Value = 0;
        }

        public void InitCols()
        {           
            this.RecursoID = new UDT_CodigoGrl();
            this.RecursoDesc = new UDT_Descriptivo();
            this.UnidadInvID = new UDT_UnidadInvID();
            this.MarcaInvID = new UDT_CodigoGrl();
            this.RefProveedor = new UDT_CodigoGrl20();
            this.TareaCliente = new UDT_CodigoGrl20();
            this.TareaDesc = new UDT_DescripTExt();    
            this.CantPresupuestado = new UDT_Cantidad();
            this.VlrPresupuestado = new UDT_Valor(); 
            this.CantSolicitado = new UDT_Cantidad();
            this.VlrSolicitado = new UDT_Valor();
            this.CantComprado = new UDT_Cantidad();
            this.VlrComprado = new UDT_Valor();
            this.CantRecibido = new UDT_Cantidad();
            this.VlrRecibido = new UDT_Valor();
            this.CantConsumido = new UDT_Cantidad();
            this.VlrConsumido = new UDT_Valor();
            this.CantFacturado = new UDT_Cantidad();
            this.VlrFacturado = new UDT_Valor();           
        }
        #endregion

        #region Propiedades

        [DataMember]
        public UDT_CodigoGrl20 TareaCliente { get; set; }

        [DataMember]
        public UDT_DescripTExt TareaDesc { get; set; }

        [DataMember]
        public UDT_CodigoGrl RecursoID { get; set; }

        [DataMember]
        public UDT_Descriptivo RecursoDesc { get; set; }

        [DataMember]
        public UDT_CodigoGrl MarcaInvID { get; set; }

        [DataMember]
        public UDT_CodigoGrl20 RefProveedor { get; set; }

        [DataMember]
        public UDT_UnidadInvID UnidadInvID { get; set; }

        [DataMember]
        public UDT_Cantidad CantPresupuestado { get; set; }

        [DataMember]
        public UDT_Valor VlrPresupuestado { get; set; }

        [DataMember]
        public UDT_Cantidad CantSolicitado { get; set; }

        [DataMember]
        public UDT_Valor VlrSolicitado { get; set; }

        [DataMember]
        public UDT_Cantidad CantComprado { get; set; }

        [DataMember]
        public UDT_Valor VlrComprado { get; set; }

        [DataMember]
        public UDT_Cantidad CantRecibido { get; set; }

        [DataMember]
        public UDT_Valor VlrRecibido { get; set; }

        [DataMember]
        public UDT_Cantidad CantConsumido { get; set; }

        [DataMember]
        public UDT_Valor VlrConsumido { get; set; }

        [DataMember]
        public UDT_Cantidad CantFacturado { get; set; }

        [DataMember]
        public UDT_Valor VlrFacturado { get; set; }
        #endregion
    }
}
