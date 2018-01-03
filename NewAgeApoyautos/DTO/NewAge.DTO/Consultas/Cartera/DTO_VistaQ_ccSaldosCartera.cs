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
    public class DTO_VistaQ_ccSaldosCartera
    {
        #region DTO_ccCreditoLiquida

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        public DTO_VistaQ_ccSaldosCartera(IDataReader dr)
        {
            this.InitCols();
            try
            {
                this.EmpresaID.Value = Convert.ToString(dr["EmpresaID"]);
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.Periodo.Value = Convert.ToDateTime(dr["Periodo"]);
                this.Libranza.Value = Convert.ToString(dr["Libranza"]);
                this.ClienteID.Value = Convert.ToDecimal(dr["ClienteID"]);
                this.NombreCliente.Value = Convert.ToString(dr["NombreCliente"]);
                this.CompradorCarteraID.Value = Convert.ToString(dr["CompradorCarteraID"]);
                this.NombreCompradorcartera.Value = Convert.ToString(dr["NombreCompradorcartera"]);
                this.PagaduriaID.Value = Convert.ToString(dr["PagaduriaID"]);
                this.NombrePagaduria.Value = Convert.ToString(dr["NombrePagaduria"]);
                this.AsesorID.Value = Convert.ToString(dr["AsesorID"]);
                this.NombreAsesor.Value = Convert.ToString(dr["NombreAsesor"]);
                this.VlrCuota.Value = Convert.ToDecimal(dr["VlrCuota"]);
                this.CapitalSDO.Value = Convert.ToDecimal(dr["CapitalSDO"]);
                this.CapitalVEN.Value = Convert.ToDecimal(dr["CapitalVEN"]);
                this.InteresTOT.Value = Convert.ToDecimal(dr["InteresTOT"]);
                this.InteresSDO.Value = Convert.ToDecimal(dr["InteresSDO"]);
                this.InteresVEN.Value = Convert.ToDecimal(dr["InteresVEN"]);
                this.InteresMES.Value = Convert.ToDecimal(dr["InteresMES"]);
                this.SeguroTOT.Value = Convert.ToDecimal(dr["SeguroTOT"]);
                this.SeguroSDO.Value = Convert.ToDecimal(dr["SeguroSDO"]);
                this.SeguroVEN.Value = Convert.ToDecimal(dr["SeguroVEN"]);
                this.OtrosFijoTOT.Value = Convert.ToDecimal(dr["OtrosFijoTOT"]);
                this.OtrosFijoSDO.Value = Convert.ToDecimal(dr["OtrosFijoSDO"]);
                this.OtrosFijoVEN.Value = Convert.ToDecimal(dr["OtrosFijoVEN"]);
                this.DiasMora.Value = Convert.ToDecimal(dr["DiasMora"]);
                this.CuotasTOT.Value = Convert.ToInt32(dr["CuotasTOT"]);
                this.CuotasSDO.Value = Convert.ToInt32(dr["CuotasSDO"]);
                this.CuotasVEN.Value = Convert.ToInt32(dr["CuotasVEN"]);
                this.CuotasCAN.Value = Convert.ToInt32(dr["CuotasCAN"]);
                this.Categoria.Value = Convert.ToString(dr["Categoria"]);
                this.SaldoNV.Value = Convert.ToDecimal(dr["SaldoNV"]);
                this.saldo30.Value = Convert.ToDecimal(dr["saldo30"]);
                this.Saldo60.Value = Convert.ToDecimal(dr["Saldo60"]);
                this.Saldo90.Value = Convert.ToDecimal(dr["Saldo90"]);
                this.Saldo180.Value = Convert.ToDecimal(dr["Saldo180"]);
                this.Saldo360.Value = Convert.ToDecimal(dr["Saldo360"]);
                this.Saldo360m.Value = Convert.ToDecimal(dr["Saldo360m"]);
                this.Capital30.Value = Convert.ToDecimal(dr["Capital30"]);
                this.Capital60.Value = Convert.ToDecimal(dr["Capital60"]);
                this.Capital90.Value = Convert.ToDecimal(dr["Capital90"]);
                this.Capital180.Value = Convert.ToDecimal(dr["Capital180"]);
                this.Capital360.Value = Convert.ToDecimal(dr["Capital360"]);
                this.Capital360m.Value = Convert.ToDecimal(dr["Capital360m"]);
                this.ProvisionCAP.Value = Convert.ToDecimal(dr["ProvisionCAP"]);
                this.ValorVenta.Value = Convert.ToDecimal(dr["ValorVenta"]);
                this.SaldoFlujo.Value = Convert.ToDecimal(dr["SaldoFlujo"]);
                this.NumeroDocPrepago.Value = Convert.ToInt32(dr["NumeroDocPrepago"]);
                this.SaldoFlujoCAP.Value = Convert.ToDecimal(dr["SaldoFlujoCAP"]);
                this.TasaMora.Value = Convert.ToDecimal(dr["TasaMora"]);
                this.InteresORD.Value = Convert.ToDecimal(dr["InteresORD"]);
                this.AportesVlr.Value = Convert.ToDecimal(dr["AportesVlr"]);
                this.InteresCAU.Value = Convert.ToDecimal(dr["InteresCAU"]);
                this.InteresCAUMes.Value = Convert.ToDecimal(dr["InteresCAUMes"]);
                this.InteresPROMes.Value = Convert.ToDecimal(dr["InteresPROMes"]);
                this.InteresCAUPag.Value = Convert.ToDecimal(dr["InteresCAUPag"]);
                this.InteresPROPag.Value = Convert.ToDecimal(dr["InteresPROPag"]);
                this.InteresORDPag.Value = Convert.ToDecimal(dr["InteresORDPag"]);
                this.InteresANTPag.Value = Convert.ToDecimal(dr["InteresANTPag"]);
                this.ClaseCredito.Value = Convert.ToString(dr["ClaseCredito"]);
                this.EtapaIncumplimiento.Value = Convert.ToString(dr["EtapaIncumplimiento"]);
                this.LineaCreditoID.Value = Convert.ToString(dr["LineaCreditoID"]);
            }
            catch (Exception e)
            {                
                throw e;
            }
        }

        #region Inicializar Columnas

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_VistaQ_ccSaldosCartera()
        {
            this.InitCols();
        } 

        #endregion

        #endregion

        #region Inicializa las columnas

        /// <summary>
        /// Inicializa Columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.Periodo = new UDTSQL_smalldatetime();
            this.Libranza = new UDT_DocTerceroID();
            this.ClienteID = new UDT_Valor();
            this.NombreCliente = new UDT_DescripTBase();
            this.CompradorCarteraID = new UDT_CompradorCarteraID();
            this.NombreCompradorcartera = new UDT_DescripTBase();
            this.PagaduriaID = new UDT_PagaduriaID();
            this.NombrePagaduria = new UDT_DescripTBase();
            this.AsesorID = new UDT_AsesorID();
            this.NombreAsesor = new UDT_DescripTBase();
            this.VlrCuota = new UDT_Valor();
            this.CapitalSDO = new UDT_Valor();
            this.CapitalVEN = new UDT_Valor();
            this.InteresTOT = new UDT_Valor();
            this.InteresSDO = new UDT_Valor();
            this.InteresVEN = new UDT_Valor();
            this.InteresMES = new UDT_Valor();
            this.SeguroTOT = new UDT_Valor();
            this.SeguroSDO = new  UDT_Valor();
            this.SeguroVEN = new UDT_Valor();
            this.OtrosFijoTOT = new UDT_Valor();
            this.OtrosFijoSDO = new UDT_Valor();
            this.OtrosFijoVEN = new UDT_Valor();
            this.DiasMora = new UDT_Valor();
            this.CuotasTOT = new UDT_CuotaID();
            this.CuotasSDO = new UDT_CuotaID();
            this.CuotasVEN = new UDT_CuotaID();
            this.CuotasCAN = new UDT_CuotaID();
            this.Categoria = new UDTSQL_char(2);
            this.SaldoNV = new UDT_Valor();
            this.saldo30 = new UDT_Valor();
            this.Saldo60 = new UDT_Valor();
            this.Saldo90 = new UDT_Valor();
            this.Saldo180 = new UDT_Valor();
            this.Saldo360 = new UDT_Valor();
            this.Saldo360m = new UDT_Valor();
            this.Capital30 = new UDT_Valor();
            this.Capital60 = new UDT_Valor();
            this.Capital90 = new UDT_Valor();
            this.Capital180 = new UDT_Valor();
            this.Capital360 = new UDT_Valor();
            this.Capital360m = new UDT_Valor();
            this.ProvisionCAP = new UDT_Valor();
            this.ValorVenta = new UDT_Valor();
            this.SaldoFlujo = new UDT_Valor();
            this.NumeroDocPrepago = new UDT_Consecutivo();
            this.SaldoFlujoCAP = new UDT_Valor();
            this.TasaMora = new UDT_TasaID();
            this.InteresORD = new UDT_Valor();
            this.AportesVlr = new UDT_Valor();
            this.InteresCAU = new UDT_Valor();
            this.InteresCAUMes = new UDT_Valor();
            this.InteresPROMes = new UDT_Valor();
            this.InteresCAUPag = new UDT_Valor();
            this.InteresPROPag = new UDT_Valor();
            this.InteresORDPag = new UDT_Valor();
            this.InteresANTPag = new UDT_Valor();
            this.ClaseCredito = new UDT_CodigoGrl5();
            this.EtapaIncumplimiento = new UDT_CodigoGrl10();
            this.LineaCreditoID = new UDT_LineaCreditoID();          
        }

        #endregion

        #region Propiedades
        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime Periodo { get; set; }

        [DataMember]
        public UDT_DocTerceroID Libranza { get; set; }
        
        [DataMember]
        public UDT_Valor ClienteID { get; set; }
        
        [DataMember]
        public UDT_DescripTBase NombreCliente { get; set; }

        [DataMember]
        public UDT_CompradorCarteraID CompradorCarteraID { get; set; }

        [DataMember]
        public UDT_DescripTBase NombreCompradorcartera { get; set; }

        [DataMember]
        public UDT_PagaduriaID PagaduriaID { get; set; }

        [DataMember]
        public UDT_DescripTBase NombrePagaduria { get; set; }

        [DataMember]
        public UDT_AsesorID AsesorID { get; set; }

        [DataMember]
        public UDT_DescripTBase NombreAsesor { get; set; }

        [DataMember]
        public UDT_Valor VlrCuota { get; set; }

        [DataMember]
        public UDT_Valor CapitalSDO { get; set; }

        [DataMember]
        public UDT_Valor CapitalVEN { get; set; }

        [DataMember]
        public UDT_Valor InteresTOT { get; set; }

        [DataMember]
        public UDT_Valor InteresSDO { get; set; }

        [DataMember]
        public UDT_Valor InteresVEN { get; set; }

        [DataMember]
        public UDT_Valor InteresMES { get; set; }

        [DataMember]
        public UDT_Valor SeguroTOT { get; set; }

        [DataMember]
        public UDT_Valor SeguroSDO { get; set; }

        [DataMember]
        public UDT_Valor SeguroVEN { get; set; }

        [DataMember]
        public UDT_Valor OtrosFijoTOT { get; set; }

        [DataMember]
        public UDT_Valor OtrosFijoSDO { get; set; }

        [DataMember]
        public UDT_Valor OtrosFijoVEN { get; set; }

        [DataMember]
        public UDT_Valor DiasMora { get; set; }

        [DataMember]
        public UDT_CuotaID CuotasTOT { get; set; }

        [DataMember]
        public UDT_CuotaID CuotasSDO { get; set; }

        [DataMember]
        public UDT_CuotaID CuotasVEN { get; set; }

        [DataMember]
        public UDT_CuotaID CuotasCAN { get; set; }

        [DataMember]
        public UDTSQL_char Categoria { get; set; }

        [DataMember]
        public UDT_Valor SaldoNV { get; set; }
            
        [DataMember]
        public UDT_Valor saldo30 { get; set; }

        [DataMember]
        public UDT_Valor Saldo60 { get; set; }

        [DataMember]
        public UDT_Valor Saldo90 { get; set; }

        [DataMember]
        public UDT_Valor Saldo180 { get; set; }

        [DataMember]
        public UDT_Valor Saldo360 { get; set; }

        [DataMember]
        public UDT_Valor Saldo360m { get; set; }

        [DataMember]
        public UDT_Valor Capital30 { get; set; }

        [DataMember]
        public UDT_Valor Capital60 { get; set; }

        [DataMember]
        public UDT_Valor Capital90 { get; set; }

        [DataMember]
        public UDT_Valor Capital180 { get; set; }

        [DataMember]
        public UDT_Valor Capital360 { get; set; }

        [DataMember]
        public UDT_Valor Capital360m { get; set; }

        [DataMember]
        public UDT_Valor ProvisionCAP { get; set; }

        [DataMember]
        public UDT_Valor ValorVenta { get; set; }

        [DataMember]
        public UDT_Valor SaldoFlujo { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDocPrepago { get; set; }

        [DataMember]
        public UDT_Valor SaldoFlujoCAP { get; set; }

        [DataMember]
        public UDT_TasaID TasaMora { get; set; }

        [DataMember]
        public UDT_Valor InteresORD { get; set; }

        [DataMember]
        public UDT_Valor AportesVlr { get; set; }

        [DataMember]
        public UDT_Valor InteresCAU { get; set; }

        [DataMember]
        public UDT_Valor InteresCAUMes { get; set; }

        [DataMember]
        public UDT_Valor InteresPROMes { get; set; }

        [DataMember]
        public UDT_Valor InteresCAUPag { get; set; }
        
        [DataMember]
        public UDT_Valor InteresPROPag { get; set; }
        
        [DataMember]
        public UDT_Valor InteresORDPag { get; set; }

        [DataMember]
        public UDT_Valor InteresANTPag { get; set; }

        [DataMember]
        public UDT_CodigoGrl5 ClaseCredito { get; set; }

        [DataMember]
        public UDT_CodigoGrl10 EtapaIncumplimiento { get; set; }

        [DataMember]
        public UDT_LineaCreditoID LineaCreditoID { get; set; }

        #endregion

    }
}
