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
    /// DTO Tabla DTO_MigrarFacturaVenta
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_MigrarFacturaVenta
    {
        #region Constructor

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_MigrarFacturaVenta()
        {
            InitCols();
            this.ContabilizaInd.Value = true;
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.PrefijoID = new UDT_PrefijoID();
            this.FacturaItem = new UDT_Consecutivo();
            this.FacturaTipoID = new UDT_FacturaTipoID();
            this.Fecha = new UDTSQL_smalldatetime();
            this.ClienteID = new UDT_ClienteID();
            this.ProyectoID = new UDT_ProyectoID();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.Moneda = new UDT_MonedaID();      
            this.ServicioID = new UDT_ServicioID();
            this.Descripcion = new UDT_DescripTBase();
            this.Cantidad = new UDT_Cantidad();
            this.Valor = new UDT_Valor();
            this.Iva = new UDT_Valor();
            this.TerceroID = new UDT_TerceroID();
            this.NumeroDocPadre = new UDT_Consecutivo();
            this.ContabilizaInd = new UDT_SiNo();
            #region Info Tercero
            this.Apellido1 = new UDT_DescripTBase();
            this.Apellido2 = new UDT_DescripTBase();
            this.Nombre1 = new UDT_DescripTBase();
            this.Nombre2 = new UDT_DescripTBase();
            this.Ciudad = new UDT_BasicID();
            this.RegFiscal = new UDT_BasicID();
            this.ActEconomicaID = new UDT_BasicID();
            this.TipoDocumento = new UDT_BasicID();
            this.Telefono = new UDTSQL_char(20);
            this.Direccion = new UDT_DescripTBase();
            this.CorreoElectronico = new UDTSQL_char(60);
            this.AutoRetenedorIVAInd = new UDT_SiNo();
            this.AutoRetenedorInd = new UDT_SiNo();
            this.DeclaraIVAInd = new UDT_SiNo();
            this.DeclaraRentaInd = new UDT_SiNo();
            this.IndependienteEMPInd = new UDT_SiNo();
            this.ExcluyeCREEInd = new UDT_SiNo();
        } 
            #endregion

        #endregion

        #region Propiedades

        [DataMember]
        [NotImportable]
        public UDT_FacturaTipoID FacturaTipoID { get; set; }

        [DataMember]
        public UDT_PrefijoID PrefijoID { get; set; }

        [DataMember]
        public UDT_Consecutivo FacturaItem { get; set; }
    
        [DataMember]
        public UDTSQL_smalldatetime Fecha { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        public UDT_MonedaID Moneda { get; set; }

        [DataMember]
        public UDT_ServicioID ServicioID { get; set; }

        [DataMember]
        public UDT_DescripTBase Descripcion { get; set; }

        [DataMember]
        public UDT_Cantidad Cantidad { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_Valor Iva { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDocPadre { get; set; }

        [DataMember]
        public UDT_SiNo ContabilizaInd { get; set; }

        #region Info Tercero

        [DataMember]
        public UDT_DescripTBase Apellido1 { get; set; }

        [DataMember]
        public UDT_DescripTBase Apellido2 { get; set; }

        [DataMember]
        public UDT_DescripTBase Nombre1 { get; set; }

        [DataMember]
        public UDT_DescripTBase Nombre2 { get; set; }

        [DataMember]
        public UDT_BasicID Ciudad { get; set; }

        [DataMember]
        public UDT_BasicID RegFiscal { get; set; }

        [DataMember]
        public UDT_BasicID ActEconomicaID { get; set; }

        [DataMember]
        public UDT_BasicID TipoDocumento { get; set; }

        [DataMember]
        public UDT_DescripTBase Direccion { get; set; }

        [DataMember]
        public UDTSQL_char Telefono { get; set; }

        [DataMember]
        public UDTSQL_char CorreoElectronico { get; set; }

        [DataMember]
        public UDT_SiNo AutoRetenedorInd { get; set; }

        [DataMember]
        public UDT_SiNo AutoRetenedorIVAInd { get; set; }

        [DataMember]
        public UDT_SiNo DeclaraIVAInd { get; set; }

        [DataMember]
        public UDT_SiNo IndependienteEMPInd { get; set; }

        [DataMember]
        public UDT_SiNo DeclaraRentaInd { get; set; }

        [DataMember]
        public UDT_SiNo ExcluyeCREEInd { get; set; } 
        #endregion

        #endregion

    }
}
