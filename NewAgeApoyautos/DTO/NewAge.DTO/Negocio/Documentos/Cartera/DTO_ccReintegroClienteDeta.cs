using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class comprobante para aprobacion:
    /// Models DTO_ccIncorporacionDeta
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccReintegroClienteDeta
    {
        #region DTO_ccReintegroClienteDeta

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        public DTO_ccReintegroClienteDeta(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                if (!String.IsNullOrWhiteSpace(dr["NumDocCredito"].ToString()))
                    this.NumDocCredito.Value = Convert.ToInt32(dr["NumDocCredito"]);
                this.ClienteID.Value = dr["ClienteID"].ToString();
                this.TerceroID.Value = dr["TerceroID"].ToString();
                this.CuentaID.Value = dr["CuentaID"].ToString();
                this.Nombre.Value = dr["Nombre"].ToString();
                this.ComponenteCarteraID.Value = dr["ComponenteCarteraID"].ToString();
                this.FechaReintegro.Value = Convert.ToDateTime(dr["FechaReintegro"]);
                if(!String.IsNullOrWhiteSpace(dr["FechaAprobacionReintegro"].ToString()))
                    this.FechaAprobacionReintegro.Value = Convert.ToDateTime(dr["FechaAprobacionReintegro"]);
                this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                this.Observacion.Value = dr["Observacion"].ToString();
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccReintegroClienteDeta()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.NumDocCredito = new UDT_Consecutivo();
            this.ClienteID = new UDT_ClienteID();
            this.TerceroID = new UDT_TerceroID();
            this.CuentaID = new UDT_CuentaID();
            this.Nombre = new UDT_DescripTBase();
            this.ComponenteCarteraID = new UDT_BasicID();
            this.FechaReintegro = new UDTSQL_smalldatetime();
            this.FechaAprobacionReintegro = new UDTSQL_smalldatetime();
            this.Valor = new UDT_Valor();
            this.Observacion = new UDT_DescripTExt();
            this.Consecutivo = new UDT_Consecutivo();

            //Campos Adicionales
            this.Aprobado = new UDT_SiNo();
            this.AsesorID = new UDT_AsesorID();
            this.Rechazado = new UDT_SiNo();
            this.Libranza = new UDT_LibranzaID();
            this.HasDetalle = new UDT_SiNo();
            this.Detalle = new List<DTO_ccReintegroClienteDeta>();
            this.NumDocCxP = new UDT_Consecutivo();
            this.ValorMax = new UDT_Valor();
            this.ValorPago = new UDT_Valor();
            this.ValorAjuste = new UDT_Valor();
            this.CuentaReintegroID = new UDT_CuentaID();
        }
        
        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_Consecutivo NumDocCredito { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_CuentaID CuentaID { get; set; }

        [DataMember]
        public UDT_DescripTBase Nombre { get; set; }

        [DataMember]
        public UDT_BasicID ComponenteCarteraID { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaReintegro { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaAprobacionReintegro { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        //Campos Adicionales

        [DataMember]
        public UDT_SiNo Aprobado { get; set; }

        [DataMember]
        public UDT_AsesorID AsesorID { get; set; }

        [DataMember]
        public UDT_SiNo Rechazado { get; set; }

        [DataMember]
        public UDT_LibranzaID Libranza { get; set; }

        [DataMember]
        public UDT_SiNo HasDetalle { get; set; }

        [DataMember]
        public List<DTO_ccReintegroClienteDeta> Detalle { get; set; }

        [DataMember]
        public UDT_Consecutivo NumDocCxP { get; set; }

        [DataMember]
        public UDT_Valor ValorMax { get; set; }

        [DataMember]
        public UDT_Valor ValorPago { get; set; }

        [DataMember]
        public UDT_Valor ValorAjuste { get; set; }

        [DataMember]
        public UDT_CuentaID CuentaReintegroID { get; set; }

        #endregion
    }
}
