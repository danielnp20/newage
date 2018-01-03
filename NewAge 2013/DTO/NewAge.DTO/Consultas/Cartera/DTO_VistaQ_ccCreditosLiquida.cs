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
    public class DTO_VistaQ_ccCreditosLiquida
    {
        #region DTO_ccCreditoLiquida

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        public DTO_VistaQ_ccCreditosLiquida(IDataReader dr)
        {
            this.InitCols();
            try
            {
                this.NumeroDoc.Value            = Convert.ToInt32(dr["NumeroDoc"]);
                this.EmpresaID.Value            = Convert.ToString(dr["EmpresaID"]);
                this.Libranza.Value             = Convert.ToString(dr["Libranza"]);
                this.Cliente.Value              = Convert.ToString(dr["Cliente"]);
                this.NomCliente.Value           = Convert.ToString(dr["NomCliente"]);
                this.TipoCredito.Value          = Convert.ToString(dr["TipoCredito"]);
                this.LineaCredito.Value         = Convert.ToString(dr["lineaCredito"]);
                this.Asesor.Value               = Convert.ToString(dr["Asesor"]);
                this.NomAsesor.Value            = Convert.ToString(dr["NomAsesor"]);
                this.Zona.Value                 = Convert.ToString(dr["Zona"]);
                this.Ciudad.Value               = Convert.ToString(dr["Ciudad"]);
                this.CentroPago.Value           = Convert.ToString(dr["CentroPago"]);
                this.NomCentroPago.Value        = Convert.ToString(dr["NomCentroPago"]);
                this.PagaduriaID.Value          = Convert.ToString(dr["PagaduriaID"]);
                this.ConcesionarioID.Value      = Convert.ToString(dr["ConcesionarioID"]);
                this.CooperativaID.Value        = Convert.ToString(dr["CooperativaID"]);
                this.AreaFuncionalID.Value      = Convert.ToString(dr["AreaFuncionalID"]);
                this.CentroCostoID.Value        = Convert.ToString(dr["CentroCostoID"]);
                this.LugarGeograficoID.Value    = Convert.ToString(dr["LugarGeograficoID"]);
                this.CuentaID.Value             = Convert.ToString(dr["CuentaID"]);
                this.VlrSolicitado.Value        = Convert.ToDecimal(dr["VlrSolicitado"]);
                this.VlrAdicional.Value         = Convert.ToDecimal(dr["VlrAdicional"]);
                this.VlrPrestamo.Value          = Convert.ToDecimal(dr["VlrPrestamo"]);
                this.VlrCompra.Value            = Convert.ToDecimal(dr["VlrCompra"]);
                this.VlrDescuento.Value         = Convert.ToDecimal(dr["VlrDescuento"]);
                this.VlrLibranza.Value          = Convert.ToDecimal(dr["VlrLibranza"]);
                this.VlrGiro.Value              = Convert.ToDecimal(dr["VlrGiro"]);
                this.VlrCuota.Value             = Convert.ToDecimal(dr["VlrCuota"]);
                this.Plazo.Value                = Convert.ToInt16(dr["Plazo"]);
                this.FechaDoc.Value             = Convert.ToDateTime(dr["FechaDoc"]);
                this.FechaLiquida.Value         = Convert.ToDateTime(dr["FechaLiquida"]);
                this.FechaCuota1.Value          = Convert.ToDateTime(dr["FechaCuota1"]);
                this.FechaVto.Value             = Convert.ToDateTime(dr["FechaVto"]);
                this.PorInteres.Value           = Convert.ToDecimal(dr["PorInteres"]);
                this.TasaEfectivaCredito.Value  = Convert.ToDecimal(dr["TasaEfectivaCredito"]);
                this.NumeroDocCXP.Value         = Convert.ToInt32(dr["NumeroDocCXP"]);
                this.Estado.Value               = Convert.ToByte(dr["Estado"]);
                this.ComprobanteID.Value        = Convert.ToString(dr["ComprobanteID"]);
                this.ComprobanteIDNro.Value     = Convert.ToInt32(dr["ComprobanteIDNro"]);
            }
            catch (Exception e)
            {                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_VistaQ_ccCreditosLiquida()
        {
            this.InitCols();
        }

        #endregion

        #region Inicializa las columnas

        /// <summary>
        /// Inicializa Columnas
        /// </summary>
        private void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.EmpresaID = new UDT_EmpresaID();
            this.Libranza = new UDT_DocTerceroID();
            this.Cliente = new UDT_ClienteID();
            this.NomCliente = new UDT_DescripTBase();
            this.TipoCredito = new UDT_CodigoGrl5();
            this.LineaCredito = new UDT_LineaCreditoID();
            this.Asesor = new UDT_AsesorID();
            this.NomAsesor = new UDT_DescripTBase();
            this.Zona = new UDT_ZonaID();
            this.Ciudad = new UDT_LugarGeograficoID();
            this.CentroPago = new UDT_CentroPagoID();
            this.NomCentroPago = new UDT_DescripTBase();
            this.PagaduriaID = new UDT_PagaduriaID();
            this.ConcesionarioID = new UDT_CodigoGrl10();
            this.CooperativaID = new UDT_CodigoGrl5();
            this.AreaFuncionalID = new UDT_AreaFuncionalID();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.LugarGeograficoID = new UDT_LugarGeograficoID();
            this.CuentaID = new UDT_CuentaID();
            this.VlrSolicitado = new UDT_Valor();
            this.VlrAdicional = new UDT_Valor();
            this.VlrPrestamo = new UDT_Valor();
            this.VlrCompra = new UDT_Valor();
            this.VlrDescuento = new UDT_Valor();
            this.VlrLibranza = new UDT_Valor();
            this.VlrGiro = new UDT_Valor();
            this.VlrCuota = new UDT_Valor();
            this.Plazo = new UDTSQL_smallint();
            this.FechaDoc = new UDTSQL_smalldatetime();
            this.FechaLiquida = new UDTSQL_smalldatetime();
            this.FechaCuota1 = new UDTSQL_smalldatetime();
            this.FechaVto = new UDTSQL_smalldatetime();
            this.PorInteres = new UDT_PorcentajeCarteraID();
            this.TasaEfectivaCredito = new UDT_PorcentajeCarteraID();
            this.NumeroDocCXP = new UDT_Consecutivo();
            this.Estado = new UDTSQL_tinyint();
            this.ComprobanteID = new UDT_ComprobanteID();
            this.ComprobanteIDNro = new UDTSQL_int();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_DocTerceroID Libranza { get; set; }

        [DataMember]
        public UDT_ClienteID Cliente { get; set; }

        [DataMember]
        public UDT_DescripTBase NomCliente { get; set; }

        [DataMember]
        public UDT_CodigoGrl5 TipoCredito { get; set; }

        [DataMember]
        public UDT_LineaCreditoID LineaCredito { get; set; }

        [DataMember]
        public UDT_AsesorID Asesor { get; set; }

        [DataMember]
        public UDT_DescripTBase NomAsesor { get; set; }

        [DataMember]
        public UDT_ZonaID Zona { get; set; }

        [DataMember]
        public UDT_LugarGeograficoID Ciudad { get; set; }

        [DataMember]
        public UDT_CentroPagoID CentroPago { get; set; }

        [DataMember]
        public UDT_DescripTBase NomCentroPago { get; set; }

        [DataMember]
        public UDT_PagaduriaID PagaduriaID { get; set; }

        [DataMember]
        public UDT_CodigoGrl10 ConcesionarioID { get; set; }

        [DataMember]
        public UDT_CodigoGrl5 CooperativaID { get; set; }

        [DataMember]
        public UDT_AreaFuncionalID AreaFuncionalID { get; set; }

        [DataMember]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        public UDT_LugarGeograficoID LugarGeograficoID { get; set; }

        [DataMember]
        public UDT_CuentaID CuentaID { get; set; }

        [DataMember]
        public UDT_Valor VlrSolicitado { get; set; }

        [DataMember]
        public UDT_Valor VlrAdicional { get; set; }

        [DataMember]
        public UDT_Valor VlrPrestamo { get; set; }

        [DataMember]
        public UDT_Valor VlrCompra { get; set; }

        [DataMember]
        public UDT_Valor VlrDescuento { get; set; }

        [DataMember]
        public UDT_Valor VlrLibranza { get; set; }

        [DataMember]
        public UDT_Valor VlrGiro { get; set; }

        [DataMember]
        public UDT_Valor VlrCuota { get; set; }
        
        [DataMember]
        public UDTSQL_smallint Plazo { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaDoc { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaLiquida { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaCuota1 { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaVto { get; set; }

        [DataMember]
        public UDT_PorcentajeCarteraID PorInteres { get; set; }

        [DataMember]
        public UDT_PorcentajeCarteraID TasaEfectivaCredito { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDocCXP { get; set; }

        [DataMember]
        public UDTSQL_tinyint Estado { get; set; }

        [DataMember]
        public UDT_ComprobanteID ComprobanteID { get; set; }

        [DataMember]
        public UDTSQL_int ComprobanteIDNro { get; set; }

        #endregion

    }
}
