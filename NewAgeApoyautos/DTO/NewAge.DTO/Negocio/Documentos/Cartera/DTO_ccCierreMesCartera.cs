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
    /// Models DTO_ccCierreMesCartera
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccCierreMesCartera
    {
        #region DTO_ccCierreMesCartera

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccCierreMesCartera(IDataReader dr,bool isCoperativa=true)
        {
            InitCols();
            try
            {
                this.Periodo.Value = Convert.ToDateTime(dr["Periodo"]);
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);

                if (!string.IsNullOrWhiteSpace(dr["DocArrastre"].ToString()))
                    this.DocArrastre.Value = Convert.ToInt32(dr["DocArrastre"]);

                this.TipoEstado.Value = Convert.ToByte(dr["TipoEstado"]);
                this.EstadoDeuda.Value = Convert.ToByte(dr["EstadoDeuda"]);

                if (!string.IsNullOrWhiteSpace(dr["EtapaIncumplimiento"].ToString()))
                    this.EtapaIncumplimiento.Value = Convert.ToString(dr["EtapaIncumplimiento"]);
                if (!string.IsNullOrWhiteSpace(dr["CompradorCarteraID"].ToString()))
                    this.CompradorCarteraID.Value = dr["CompradorCarteraID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["ClaseCredito"].ToString()))
                    this.ClaseCredito.Value = dr["ClaseCredito"].ToString(); 
                if (!string.IsNullOrWhiteSpace(dr["VlrCuota"].ToString()))
                    this.VlrCuota.Value = Convert.ToDecimal(dr["VlrCuota"]);

                if (!string.IsNullOrWhiteSpace(dr["NominalTOT"].ToString()))
                    this.NominalTOT.Value = Convert.ToDecimal(dr["NominalTOT"]);
                if (!string.IsNullOrWhiteSpace(dr["CapitalTOT"].ToString()))
                    this.CapitalTOT.Value = Convert.ToDecimal(dr["CapitalTOT"]);
                if (!string.IsNullOrWhiteSpace(dr["CapitalSDO"].ToString()))
                    this.CapitalSDO.Value = Convert.ToDecimal(dr["CapitalSDO"]);
                if (!string.IsNullOrWhiteSpace(dr["CapitalVEN"].ToString()))
                    this.CapitalVEN.Value = Convert.ToDecimal(dr["CapitalVEN"]);

                if (!string.IsNullOrWhiteSpace(dr["InteresTOT"].ToString()))
                    this.InteresTOT.Value = Convert.ToDecimal(dr["InteresTOT"]);
                if (!string.IsNullOrWhiteSpace(dr["InteresSDO"].ToString()))
                    this.InteresSDO.Value = Convert.ToDecimal(dr["InteresSDO"]);
                if (!string.IsNullOrWhiteSpace(dr["InteresVEN"].ToString()))
                    this.InteresVEN.Value = Convert.ToDecimal(dr["InteresVEN"]);
                if (!string.IsNullOrWhiteSpace(dr["InteresCAU"].ToString()))
                    this.InteresCAU.Value = Convert.ToDecimal(dr["InteresCAU"]);
                if (!string.IsNullOrWhiteSpace(dr["InteresCAUMes"].ToString()))
                    this.InteresCAUMes.Value = Convert.ToDecimal(dr["InteresCAUMes"]);
                if (!string.IsNullOrWhiteSpace(dr["InteresPROMes"].ToString()))
                    this.InteresPROMes.Value = Convert.ToDecimal(dr["InteresPROMes"]);
                if (!string.IsNullOrWhiteSpace(dr["InteresMES"].ToString()))
                    this.InteresMES.Value = Convert.ToDecimal(dr["InteresMES"]);
                if (!string.IsNullOrWhiteSpace(dr["InteresCAUPag"].ToString()))
                    this.InteresCAUPag.Value = Convert.ToDecimal(dr["InteresCAUPag"]);
                if (!string.IsNullOrWhiteSpace(dr["InteresPROPag"].ToString()))
                    this.InteresPROPag.Value = Convert.ToDecimal(dr["InteresPROPag"]);
                if (!string.IsNullOrWhiteSpace(dr["InteresORDPag"].ToString()))
                    this.InteresORDPag.Value = Convert.ToDecimal(dr["InteresORDPag"]);
                if (!string.IsNullOrWhiteSpace(dr["InteresORD"].ToString()))
                    this.InteresORD.Value = Convert.ToDecimal(dr["InteresORD"]);
                if (!string.IsNullOrWhiteSpace(dr["InteresANTPag"].ToString()))
                    this.InteresANTPag.Value = Convert.ToDecimal(dr["InteresANTPag"]);
 
                if (!string.IsNullOrWhiteSpace(dr["SeguroTOT"].ToString()))
                    this.SeguroTOT.Value = Convert.ToDecimal(dr["SeguroTOT"]);
                if (!string.IsNullOrWhiteSpace(dr["SeguroSDO"].ToString()))
                    this.SeguroSDO.Value = Convert.ToDecimal(dr["SeguroSDO"]);
                if (!string.IsNullOrWhiteSpace(dr["SeguroVEN"].ToString()))
                    this.SeguroVEN.Value = Convert.ToDecimal(dr["SeguroVEN"]);

                if (!string.IsNullOrWhiteSpace(dr["Otros1TOT"].ToString()))
                    this.Otros1TOT.Value = Convert.ToDecimal(dr["Otros1TOT"]);
                if (!string.IsNullOrWhiteSpace(dr["Otros1SDO"].ToString()))
                    this.Otros1SDO.Value = Convert.ToDecimal(dr["Otros1SDO"]);
                if (!string.IsNullOrWhiteSpace(dr["Otros1VEN"].ToString()))
                    this.Otros1VEN.Value = Convert.ToDecimal(dr["Otros1VEN"]);

                if (!string.IsNullOrWhiteSpace(dr["Otros2TOT"].ToString()))
                    this.Otros2TOT.Value = Convert.ToDecimal(dr["Otros2TOT"]);
                if (!string.IsNullOrWhiteSpace(dr["Otros2SDO"].ToString()))
                    this.Otros2SDO.Value = Convert.ToDecimal(dr["Otros2SDO"]);
                if (!string.IsNullOrWhiteSpace(dr["Otros2VEN"].ToString()))
                    this.Otros2VEN.Value = Convert.ToDecimal(dr["Otros2VEN"]);

                if (!string.IsNullOrWhiteSpace(dr["Otros3TOT"].ToString()))
                    this.Otros3TOT.Value = Convert.ToDecimal(dr["Otros3TOT"]);
                if (!string.IsNullOrWhiteSpace(dr["Otros3SDO"].ToString()))
                    this.Otros3SDO.Value = Convert.ToDecimal(dr["Otros3SDO"]);
                if (!string.IsNullOrWhiteSpace(dr["Otros3VEN"].ToString()))
                    this.Otros3VEN.Value = Convert.ToDecimal(dr["Otros3VEN"]);

                if (!string.IsNullOrWhiteSpace(dr["OtrosFijoTOT"].ToString()))
                    this.OtrosFijoTOT.Value = Convert.ToDecimal(dr["OtrosFijoTOT"]);
                if (!string.IsNullOrWhiteSpace(dr["OtrosFijoSDO"].ToString()))
                    this.OtrosFijoSDO.Value = Convert.ToDecimal(dr["OtrosFijoSDO"]);
                if (!string.IsNullOrWhiteSpace(dr["OtrosFijoVEN"].ToString()))
                    this.OtrosFijoVEN.Value = Convert.ToDecimal(dr["OtrosFijoVEN"]);

                if (!string.IsNullOrWhiteSpace(dr["AportesVlr"].ToString()))
                    this.AportesVlr.Value = Convert.ToDecimal(dr["AportesVlr"]);
                if (!string.IsNullOrWhiteSpace(dr["TasaMora"].ToString()))
                    this.TasaMora.Value = Convert.ToDecimal(dr["TasaMora"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaVto"].ToString()))
                    this.FechaVto.Value = Convert.ToDateTime(dr["FechaVto"]);
                if (!string.IsNullOrWhiteSpace(dr["DiasMora"].ToString()))
                    this.DiasMora.Value = Convert.ToInt32(dr["DiasMora"]);

                if (!string.IsNullOrWhiteSpace(dr["CuotasTOT"].ToString()))
                    this.CuotasTOT.Value = Convert.ToInt32(dr["CuotasTOT"]);
                if (!string.IsNullOrWhiteSpace(dr["CuotasSDO"].ToString()))
                    this.CuotasSDO.Value = Convert.ToInt32(dr["CuotasSDO"]);
                if (!string.IsNullOrWhiteSpace(dr["CuotasVEN"].ToString()))
                    this.CuotasVEN.Value = Convert.ToInt32(dr["CuotasVEN"]);
                if (!string.IsNullOrWhiteSpace(dr["CuotasCAN"].ToString()))
                    this.CuotasCAN.Value = Convert.ToInt32(dr["CuotasCAN"]);

                if (!string.IsNullOrWhiteSpace(dr["Categoria"].ToString()))
                    this.Categoria.Value = dr["Categoria"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["CategoriaORI"].ToString()))
                    this.CategoriaORI.Value = dr["CategoriaORI"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["CreditoNUE"].ToString()))
                    this.CreditoNUE.Value = Convert.ToDecimal(dr["CreditoNUE"]);
                if (!string.IsNullOrWhiteSpace(dr["CreditoANU"].ToString()))
                    this.CreditoANU.Value = Convert.ToDecimal(dr["CreditoANU"]);
                if (!string.IsNullOrWhiteSpace(dr["RecaudoNUE"].ToString()))
                    this.RecaudoNUE.Value = Convert.ToDecimal(dr["RecaudoNUE"]);
                if (!string.IsNullOrWhiteSpace(dr["RecaudoANU"].ToString()))
                    this.RecaudoANU.Value = Convert.ToDecimal(dr["RecaudoANU"]);
                if (!string.IsNullOrWhiteSpace(dr["NominaNUE"].ToString()))
                    this.NominaNUE.Value = Convert.ToDecimal(dr["NominaNUE"]);
                if (!string.IsNullOrWhiteSpace(dr["NominaANU"].ToString()))
                    this.NominaANU.Value = Convert.ToDecimal(dr["NominaANU"]);
                if (!string.IsNullOrWhiteSpace(dr["PagototNUE"].ToString()))
                    this.PagototNUE.Value = Convert.ToDecimal(dr["PagototNUE"]);
                if (!string.IsNullOrWhiteSpace(dr["PagototANU"].ToString()))
                    this.PagototANU.Value = Convert.ToDecimal(dr["PagototANU"]);
                if (!string.IsNullOrWhiteSpace(dr["RefinanciaNUE"].ToString()))
                    this.RefinanciaNUE.Value = Convert.ToDecimal(dr["RefinanciaNUE"]);
                if (!string.IsNullOrWhiteSpace(dr["RefinanciaANU"].ToString()))
                    this.RefinanciaANU.Value = Convert.ToDecimal(dr["RefinanciaANU"]);

                if (!string.IsNullOrWhiteSpace(dr["SaldoNV"].ToString()))
                    this.SaldoNV.Value = Convert.ToDecimal(dr["SaldoNV"]);
                if (!string.IsNullOrWhiteSpace(dr["Saldo30"].ToString()))
                    this.Saldo30.Value = Convert.ToDecimal(dr["Saldo30"]);
                if (!string.IsNullOrWhiteSpace(dr["Saldo60"].ToString()))
                    this.Saldo60.Value = Convert.ToDecimal(dr["Saldo60"]);
                if (!string.IsNullOrWhiteSpace(dr["Saldo90"].ToString()))
                    this.Saldo90.Value = Convert.ToDecimal(dr["Saldo90"]);
                if (!string.IsNullOrWhiteSpace(dr["Saldo180"].ToString()))
                    this.Saldo180.Value = Convert.ToDecimal(dr["Saldo180"]);
                if (!string.IsNullOrWhiteSpace(dr["Saldo360"].ToString()))
                    this.Saldo360.Value = Convert.ToDecimal(dr["Saldo360"]);
                if (!string.IsNullOrWhiteSpace(dr["Saldo360m"].ToString()))
                    this.Saldo360m.Value = Convert.ToDecimal(dr["Saldo360m"]);

                if (!string.IsNullOrWhiteSpace(dr["CapitalNV"].ToString()))
                    this.CapitalNV.Value = Convert.ToDecimal(dr["CapitalNV"]);
                if (!string.IsNullOrWhiteSpace(dr["Capital30"].ToString()))
                    this.Capital30.Value = Convert.ToDecimal(dr["Capital30"]);
                if (!string.IsNullOrWhiteSpace(dr["Capital60"].ToString()))
                    this.Capital60.Value = Convert.ToDecimal(dr["Capital60"]);
                if (!string.IsNullOrWhiteSpace(dr["Capital90"].ToString()))
                    this.Capital90.Value = Convert.ToDecimal(dr["Capital90"]);
                if (!string.IsNullOrWhiteSpace(dr["Capital180"].ToString()))
                    this.Capital180.Value = Convert.ToDecimal(dr["Capital180"]);
                if (!string.IsNullOrWhiteSpace(dr["Capital360"].ToString()))
                    this.Capital360.Value = Convert.ToDecimal(dr["Capital360"]);
                if (!string.IsNullOrWhiteSpace(dr["Capital360m"].ToString()))
                    this.Capital360m.Value = Convert.ToDecimal(dr["Capital360m"]);

                if (!string.IsNullOrWhiteSpace(dr["ProvisionCAP"].ToString()))
                    this.ProvisionCAP.Value = Convert.ToDecimal(dr["ProvisionCAP"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaUltPago"].ToString()))
                    this.FechaUltPago.Value = Convert.ToDateTime(dr["FechaUltPago"]);
                if (!string.IsNullOrWhiteSpace(dr["NumeroDocVenta"].ToString()))
                    this.NumeroDocVenta.Value = Convert.ToInt32(dr["NumeroDocVenta"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorVenta"].ToString()))
                    this.ValorVenta.Value = Convert.ToDecimal(dr["ValorVenta"]);
                if (!string.IsNullOrWhiteSpace(dr["NumeroDocPrepago"].ToString()))
                    this.NumeroDocPrepago.Value = Convert.ToInt32(dr["NumeroDocPrepago"]);
                if (!string.IsNullOrWhiteSpace(dr["NumeroDocRecompra"].ToString()))
                    this.NumeroDocRecompra.Value = Convert.ToInt32(dr["NumeroDocRecompra"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorRecompra"].ToString()))
                    this.ValorRecompra.Value = Convert.ToDecimal(dr["ValorRecompra"]);
                if (!string.IsNullOrWhiteSpace(dr["SaldoFlujo"].ToString()))
                    this.SaldoFlujo.Value = Convert.ToDecimal(dr["SaldoFlujo"]);
                if (!string.IsNullOrWhiteSpace(dr["SaldoFlujoCAP"].ToString()))
                    this.SaldoFlujoCAP.Value = Convert.ToDecimal(dr["SaldoFlujoCAP"]);
                if (!string.IsNullOrWhiteSpace(dr["CuotasFlujo"].ToString()))
                    this.CuotasFlujo.Value = Convert.ToInt32(dr["CuotasFlujo"]);
                if (!string.IsNullOrWhiteSpace(dr["AlturaFlujo"].ToString()))
                    this.AlturaFlujo.Value = Convert.ToInt32(dr["AlturaFlujo"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccCierreMesCartera()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.Periodo = new UDTSQL_smalldatetime();
            this.NumeroDoc = new UDT_Consecutivo();
            this.DocArrastre = new UDT_Consecutivo();
            this.TipoEstado = new UDTSQL_tinyint();
            this.EstadoDeuda = new UDTSQL_tinyint();
            this.EtapaIncumplimiento = new UDT_CodigoGrl10();
            this.CompradorCarteraID = new UDT_CompradorCarteraID();
            this.ClaseCredito = new UDT_CodigoGrl5();
            this.VlrCuota = new UDT_Valor();
            this.NominalTOT = new UDT_Valor();
            this.CapitalTOT = new UDT_Valor();
            this.CapitalSDO = new UDT_Valor();
            this.CapitalVEN = new UDT_Valor();
            this.InteresTOT = new UDT_Valor();
            this.InteresSDO = new UDT_Valor(); 
            this.InteresVEN = new UDT_Valor();
            this.InteresCAU = new UDT_Valor();
            this.InteresCAUMes = new UDT_Valor();
            this.InteresPROMes = new UDT_Valor();
            this.InteresMES = new UDT_Valor();
            this.InteresCAUPag = new UDT_Valor();
            this.InteresPROPag = new UDT_Valor();
            this.InteresORDPag = new UDT_Valor();
            this.InteresORD = new UDT_Valor();
            this.InteresANTPag = new UDT_Valor();
            this.SeguroTOT = new UDT_Valor();
            this.SeguroSDO = new UDT_Valor();
            this.SeguroVEN = new UDT_Valor();
            this.Otros1TOT = new UDT_Valor();
            this.Otros1SDO = new UDT_Valor();
            this.Otros1VEN = new UDT_Valor();
            this.Otros2TOT = new UDT_Valor();
            this.Otros2SDO = new UDT_Valor();
            this.Otros2VEN = new UDT_Valor();
            this.Otros3TOT = new UDT_Valor();
            this.Otros3SDO = new UDT_Valor();
            this.Otros3VEN = new UDT_Valor();
            this.OtrosFijoTOT = new UDT_Valor();
            this.OtrosFijoSDO = new UDT_Valor();
            this.OtrosFijoVEN = new UDT_Valor();
            this.AportesVlr = new UDT_Valor();
            this.TasaMora = new UDT_TasaID();
            this.FechaVto = new UDTSQL_smalldatetime(); 
            this.DiasMora = new UDTSQL_int();
            this.CuotasTOT = new UDT_CuotaID();
            this.CuotasSDO = new UDT_CuotaID();
            this.CuotasVEN = new UDT_CuotaID();
            this.CuotasCAN = new UDT_CuotaID();
            this.Categoria = new UDTSQL_varchar(2);
            this.CategoriaORI = new UDTSQL_varchar(2);
            this.CreditoNUE = new UDT_Valor();
            this.CreditoANU = new UDT_Valor();
            this.RecaudoNUE = new UDT_Valor();
            this.RecaudoANU = new UDT_Valor();
            this.NominaNUE = new UDT_Valor();
            this.NominaANU = new UDT_Valor();
            this.PagototNUE = new UDT_Valor();
            this.PagototANU = new UDT_Valor();
            this.RefinanciaNUE = new UDT_Valor();
            this.RefinanciaANU = new UDT_Valor();
            this.SaldoNV = new UDT_Valor();
            this.Saldo30 = new UDT_Valor();
            this.Saldo60 = new UDT_Valor();
            this.Saldo90 = new UDT_Valor();
            this.Saldo180 = new UDT_Valor();
            this.Saldo360 = new UDT_Valor();
            this.Saldo360m = new UDT_Valor();
            this.CapitalNV = new UDT_Valor();
            this.Capital30 = new UDT_Valor();
            this.Capital60 = new UDT_Valor();
            this.Capital90 = new UDT_Valor();
            this.Capital180 = new UDT_Valor();
            this.Capital360 = new UDT_Valor();
            this.Capital360m = new UDT_Valor();
            this.ProvisionCAP = new UDT_Valor();
            this.FechaUltPago = new UDTSQL_smalldatetime();
            this.NumeroDocVenta = new UDT_Consecutivo(); 
            this.ValorVenta = new UDT_Valor();
            this.NumeroDocPrepago = new UDT_Consecutivo();
            this.NumeroDocRecompra = new UDT_Consecutivo();
            this.ValorRecompra = new UDT_Valor();
            this.SaldoFlujo = new UDT_Valor();
            this.SaldoFlujoCAP = new UDT_Valor();
            this.CuotasFlujo = new UDT_CuotaID();
            this.AlturaFlujo = new UDT_CuotaID();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDTSQL_smalldatetime Periodo { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

       	[DataMember]
        public UDT_Consecutivo DocArrastre { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoEstado { get; set; }

        [DataMember]
        public UDTSQL_tinyint EstadoDeuda { get; set; }

        [DataMember]
        public UDT_CodigoGrl10 EtapaIncumplimiento { get; set; }

        [DataMember]
        public UDT_CompradorCarteraID CompradorCarteraID { get; set; }

        [DataMember]
        public UDT_CodigoGrl5 ClaseCredito { get; set; }

        [DataMember]
        public UDT_Valor VlrCuota { get; set; }

        [DataMember]
        public UDT_Valor NominalTOT { get; set; }

        [DataMember]
        public UDT_Valor CapitalTOT { get; set; }

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
        public UDT_Valor InteresCAU { get; set; }

        [DataMember]
        public UDT_Valor InteresCAUMes { get; set; }

        [DataMember]
        public UDT_Valor InteresPROMes { get; set; }

        [DataMember]
        public UDT_Valor InteresMES { get; set; }

        [DataMember]
        public UDT_Valor InteresCAUPag { get; set; }

        [DataMember]
        public UDT_Valor InteresPROPag { get; set; }

        [DataMember]
        public UDT_Valor InteresORDPag { get; set; }

        [DataMember]
        public UDT_Valor InteresORD { get; set; }

        [DataMember]
        public UDT_Valor InteresANTPag { get; set; }

        [DataMember]
        public UDT_Valor SeguroTOT { get; set; }

        [DataMember]
        public UDT_Valor SeguroSDO { get; set; }

        [DataMember]
        public UDT_Valor SeguroVEN { get; set; }

        [DataMember]
        public UDT_Valor Otros1TOT { get; set; }

        [DataMember]
        public UDT_Valor Otros1SDO { get; set; }

        [DataMember]
        public UDT_Valor Otros1VEN { get; set; }

        [DataMember]
        public UDT_Valor Otros2TOT { get; set; }

        [DataMember]
        public UDT_Valor Otros2SDO { get; set; }

        [DataMember]
        public UDT_Valor Otros2VEN { get; set; }

        [DataMember]
        public UDT_Valor Otros3TOT { get; set; }

        [DataMember]
        public UDT_Valor Otros3SDO { get; set; }

        [DataMember]
        public UDT_Valor Otros3VEN { get; set; }

        [DataMember]
        public UDT_Valor OtrosFijoTOT { get; set; }

        [DataMember]
        public UDT_Valor OtrosFijoSDO { get; set; }

        [DataMember]
        public UDT_Valor OtrosFijoVEN { get; set; }

        [DataMember]
        public UDT_Valor AportesVlr { get; set; }

        [DataMember]
        public UDT_TasaID TasaMora { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime FechaVto { get; set; }

        [DataMember]
        public UDTSQL_int DiasMora { get; set; }

        [DataMember]
        public UDT_CuotaID CuotasTOT { get; set; }

        [DataMember]
        public UDT_CuotaID CuotasSDO { get; set; }

        [DataMember]
        public UDT_CuotaID CuotasVEN { get; set; }

        [DataMember]
        public UDT_CuotaID CuotasCAN { get; set; }

        [DataMember]
        public UDTSQL_varchar Categoria { get; set; }

        [DataMember]
        public UDTSQL_varchar CategoriaORI { get; set; }

        [DataMember]
        public UDT_Valor CreditoNUE { get; set; }

        [DataMember]
        public UDT_Valor CreditoANU { get; set; }

        [DataMember]
        public UDT_Valor RecaudoNUE { get; set; }

        [DataMember]
        public UDT_Valor RecaudoANU { get; set; }

        [DataMember]
        public UDT_Valor NominaNUE { get; set; }

        [DataMember]
        public UDT_Valor NominaANU { get; set; }

        [DataMember]
        public UDT_Valor PagototNUE { get; set; }

        [DataMember]
        public UDT_Valor PagototANU { get; set; }

        [DataMember]
        public UDT_Valor RefinanciaNUE { get; set; }

        [DataMember]
        public UDT_Valor RefinanciaANU { get; set; }

        [DataMember]
        public UDT_Valor SaldoNV { get; set; }

        [DataMember]
        public UDT_Valor Saldo30 { get; set; }

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
        public UDT_Valor CapitalNV { get; set; }

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
        public UDTSQL_smalldatetime FechaUltPago { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDocVenta { get; set; }

        [DataMember]
        public UDT_Valor ValorVenta { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDocPrepago { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDocRecompra { get; set; }

        [DataMember]
        public UDT_Valor ValorRecompra { get; set; }

        [DataMember]
        public UDT_Valor SaldoFlujo{ get; set; }

        [DataMember]
        public UDT_Valor SaldoFlujoCAP { get; set; }

        [DataMember]
        public UDT_CuotaID CuotasFlujo { get; set; }

        [DataMember]
        public UDT_CuotaID AlturaFlujo { get; set; }

        #endregion
    }
}
