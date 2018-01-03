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
    /// DTO Tabla DTO_inDistribucionCosto
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_inDistribucionCosto
    {
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_inDistribucionCosto(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDocCto.Value = Convert.ToInt32(dr["NumeroDocCto"]);
                this.NumeroDocINV.Value = Convert.ToInt32(dr["NumeroDocINV"]);
                this.Valor.Value = Convert.ToDecimal(dr["Valor"]);               
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_inDistribucionCosto()
        {
            InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.NumeroDocCto = new UDT_Consecutivo();
            this.NumeroDocINV = new UDT_Consecutivo();
            this.ProveedorID = new UDT_ProveedorID();
            this.ProveedorDesc = new UDT_Descriptivo();
            this.FacturaNro = new UDT_DescripTBase();
            this.MonedaOrigen = new UDT_MonedaID();
            this.ValorML = new UDT_Valor();
            this.ValorME = new UDT_Valor();
            this.Valor = new UDT_Valor();
            this.Observacion = new UDT_DescripTExt();
            this.FechaFactura = new UDTSQL_datetime();
        }
        

        #region Propiedades

        [DataMember]
        [AllowNull]
        public UDT_Consecutivo NumeroDocINV { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Consecutivo NumeroDocCto { get; set; }

        [DataMember]
        [AllowNull]    
        public UDT_ProveedorID ProveedorID { get; set; }
       
        [DataMember]
        [AllowNull]
        public UDT_Descriptivo ProveedorDesc { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTBase FacturaNro { get; set; }
    
        [DataMember]
        [AllowNull]
        public UDT_MonedaID MonedaOrigen { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorML { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorME { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTExt Observacion { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_datetime FechaFactura { get; set; }

        #endregion
    }
}
