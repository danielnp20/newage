using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.Attributes;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// DTO Tabla DTO_inMovimientoFooter
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_inMovimientoFooter
    {
        #region Constructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_inMovimientoFooter()
        {
            this.Movimiento = new DTO_glMovimientoDeta();
            this.ReferenciaIDP1P2 = new UDT_DescripTBase();
            this.ReferenciaIDP1P2Desc = new UDT_DescripTBase();
            this.UnidadRef = new UDT_UnidadInvID(); 
            this.RecibirInd = new UDT_SiNo();
            this.DevolverInd = new UDT_SiNo();
            this.CantidadPendiente = new UDT_Cantidad();
            this.CantidadTraslado = new UDT_Cantidad();
            this.CantidadDispon = new UDT_Cantidad();
            this.PorcDistribucion = new UDT_PorcentajeID();
            this.ValorDistribucionML = new UDT_Valor();
            this.ValorDistribucionME = new UDT_Valor();
            this.ValorItemTotalML = new UDT_Valor();
            this.ValorItemTotalME = new UDT_Valor();
            this.FacturaNro = new UDT_DescripTExt();
            this.Detalle = new List<DTO_glMovimientoDeta>();
            this.PorArancel = new UDT_PorcentajeID();
            this.ValorArancel = new UDT_Valor();
            this.ValorOtrosUS = new UDT_Valor();
            this.SelectInd = new UDT_SiNo();
            this.OrigenMonetario = new UDTSQL_tinyint();
            this.FechaSol = new UDTSQL_smalldatetime();
            this.DetalleSolicitud = new List<DTO_prSolicitudResumen>();
        }
        
        #endregion

        #region Propiedades

        [DataMember]
        public DTO_glMovimientoDeta Movimiento { get; set; }

        [DataMember]
        [AllowNull]    
        public UDT_DescripTBase ReferenciaIDP1P2 { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTBase ReferenciaIDP1P2Desc { get; set; }
       
        [DataMember]
        [AllowNull]
        public UDT_UnidadInvID UnidadRef { get; set; }

        [AllowNull]
        [DataMember]
        public UDT_SiNo RecibirInd { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_SiNo DevolverInd { get; set; }
    
        [DataMember]
        [AllowNull]
        public UDT_Cantidad CantidadPendiente { get; set; }

        [DataMember]
        [AllowNull]        
        public UDT_Cantidad CantidadTraslado { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Cantidad CantidadDispon { get; set; }

        [DataMember]
        [AllowNull]
        public List<DTO_prSolicitudResumen> DetalleSolicitud { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_SiNo SelectInd { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_smalldatetime FechaSol { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_tinyint OrigenMonetario { get; set; }

        [DataMember]
        [AllowNull]
        public string PrefDoc { get; set; }


        //Items Distribucion

        [DataMember]
        [AllowNull]
        public UDT_PorcentajeID PorcDistribucion { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorDistribucionML { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorDistribucionME { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorItemTotalML { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorItemTotalME { get; set; }

        //Items Importacion

        [DataMember]
        [AllowNull]
        public UDT_DescripTExt FacturaNro { get; set; }

        [DataMember]
        [AllowNull]
        public List<DTO_glMovimientoDeta> Detalle { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorOtrosUS { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorTotalUS { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorTotalPS { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_PorcentajeID PorArancel { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_PorcentajeID PorIVA { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorArancel { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorIVA { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorOtrosPS { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor CostoTotalUS { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor CostoTotalPS { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor PosArancelaria { get; set; }

        #endregion
    }
}
