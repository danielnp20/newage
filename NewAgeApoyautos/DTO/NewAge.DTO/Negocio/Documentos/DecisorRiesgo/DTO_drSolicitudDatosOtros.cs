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
    /// Models DTO_drSolicitudDatosOtros
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_drSolicitudDatosOtros
    {
        #region drSolicitudDatosOtros

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_drSolicitudDatosOtros(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value=Convert.ToInt32(dr["NumeroDoc"]);
                this.Version.Value = Convert.ToByte(dr["Version"]);


                if (!string.IsNullOrWhiteSpace(dr["VlrPoliza"].ToString()))
                    this.VlrPoliza.Value = Convert.ToDecimal(dr["VlrPoliza"]);
                if (!string.IsNullOrWhiteSpace(dr["CubriendoInd"].ToString()))
                    this.CubriendoInd.Value = Convert.ToBoolean(dr["CubriendoInd"]);
                if (!string.IsNullOrWhiteSpace(dr["AseguradoraID"].ToString()))
                    this.AseguradoraID.Value = Convert.ToString(dr["AseguradoraID"]);
                if (!string.IsNullOrWhiteSpace(dr["FinanciaPOLInd"].ToString()))
                    this.FinanciaPOLInd.Value = Convert.ToBoolean(dr["FinanciaPOLInd"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrMensualSV"].ToString()))
                    this.VlrMensualSV.Value = Convert.ToDecimal(dr["VlrMensualSV"]);
                if (!string.IsNullOrWhiteSpace(dr["TipoPrenda"].ToString()))
                    this.TipoPrenda.Value = Convert.ToByte(dr["TipoPrenda"]);
                if (!string.IsNullOrWhiteSpace(dr["PrefijoPrenda"].ToString()))
                    this.PrefijoPrenda.Value = Convert.ToString(dr["PrefijoPrenda"]);
                if (!string.IsNullOrWhiteSpace(dr["NumeroPrenda"].ToString()))
                    this.NumeroPrenda.Value = Convert.ToInt32(dr["NumeroPrenda"]);
                if (!string.IsNullOrWhiteSpace(dr["Registro"].ToString()))
                    this.Registro.Value = Convert.ToByte(dr["Registro"]);
                if (!string.IsNullOrWhiteSpace(dr["CedulaReg"].ToString()))
                    this.CedulaReg.Value = Convert.ToString(dr["CedulaReg"]);
                if (!string.IsNullOrWhiteSpace(dr["NombreREG"].ToString()))
                    this.NombreREG.Value = Convert.ToString(dr["NombreREG"]);
                if (!string.IsNullOrWhiteSpace(dr["DireccionREG"].ToString()))
                    this.DireccionREG.Value = Convert.ToString(dr["DireccionREG"]);
                if (!string.IsNullOrWhiteSpace(dr["Alternativa1"].ToString()))
                    this.Alternativa1.Value = Convert.ToString(dr["Alternativa1"]);
                if (!string.IsNullOrWhiteSpace(dr["Alternativa2"].ToString()))
                    this.Alternativa2.Value = Convert.ToString(dr["Alternativa2"]);
                if (!string.IsNullOrWhiteSpace(dr["Alternativa3"].ToString()))
                    this.Alternativa3.Value = Convert.ToString(dr["Alternativa3"]);
                if (!string.IsNullOrWhiteSpace(dr["Alternativa4"].ToString()))
                    this.Alternativa4.Value = Convert.ToString(dr["Alternativa4"]);
                if (!string.IsNullOrWhiteSpace(dr["Alternativa5"].ToString()))
                    this.Alternativa5.Value = Convert.ToString(dr["Alternativa5"]);
                if (!string.IsNullOrWhiteSpace(dr["Alternativa6"].ToString()))
                    this.Alternativa6.Value = Convert.ToString(dr["Alternativa6"]);
                if (!string.IsNullOrWhiteSpace(dr["Alternativa7"].ToString()))
                    this.Alternativa7.Value = Convert.ToString(dr["Alternativa7"]);
                if (!string.IsNullOrWhiteSpace(dr["FactorAlt1"].ToString()))
                    this.FactorAlt1.Value = Convert.ToDecimal(dr["FactorAlt1"]);
                if (!string.IsNullOrWhiteSpace(dr["FactorAlt2"].ToString()))
                    this.FactorAlt2.Value = Convert.ToDecimal(dr["FactorAlt2"]);
                if (!string.IsNullOrWhiteSpace(dr["FactorAlt3"].ToString()))
                    this.FactorAlt3.Value = Convert.ToDecimal(dr["FactorAlt3"]);
                if (!string.IsNullOrWhiteSpace(dr["FactorAlt4"].ToString()))
                    this.FactorAlt4.Value = Convert.ToDecimal(dr["FactorAlt4"]);
                if (!string.IsNullOrWhiteSpace(dr["FactorAlt5"].ToString()))
                    this.FactorAlt5.Value = Convert.ToDecimal(dr["FactorAlt5"]);
                if (!string.IsNullOrWhiteSpace(dr["FactorAlt6"].ToString()))
                    this.FactorAlt6.Value = Convert.ToDecimal(dr["FactorAlt6"]);
                if (!string.IsNullOrWhiteSpace(dr["FactorAlt7"].ToString()))
                    this.FactorAlt7.Value = Convert.ToDecimal(dr["FactorAlt7"]);
                if (!string.IsNullOrWhiteSpace(dr["MontoAlt1"].ToString()))
                    this.MontoAlt1.Value = Convert.ToDecimal(dr["MontoAlt1"]);
                if (!string.IsNullOrWhiteSpace(dr["MontoAlt2"].ToString()))
                    this.MontoAlt2.Value = Convert.ToDecimal(dr["MontoAlt2"]);
                if (!string.IsNullOrWhiteSpace(dr["MontoAlt3"].ToString()))
                    this.MontoAlt3.Value = Convert.ToDecimal(dr["MontoAlt3"]);
                if (!string.IsNullOrWhiteSpace(dr["MontoAlt4"].ToString()))
                    this.MontoAlt4.Value = Convert.ToDecimal(dr["MontoAlt4"]);
                if (!string.IsNullOrWhiteSpace(dr["MontoAlt5"].ToString()))
                    this.MontoAlt5.Value = Convert.ToDecimal(dr["MontoAlt5"]);
                if (!string.IsNullOrWhiteSpace(dr["MontoAlt6"].ToString()))
                    this.MontoAlt6.Value = Convert.ToDecimal(dr["MontoAlt6"]);
                if (!string.IsNullOrWhiteSpace(dr["MontoAlt7"].ToString()))
                    this.MontoAlt7.Value = Convert.ToDecimal(dr["MontoAlt7"]);
                if (!string.IsNullOrWhiteSpace(dr["GarantiaAlt1"].ToString()))
                    this.GarantiaAlt1.Value = Convert.ToDecimal(dr["GarantiaAlt1"]);
                if (!string.IsNullOrWhiteSpace(dr["GarantiaAlt2"].ToString()))
                    this.GarantiaAlt2.Value = Convert.ToDecimal(dr["GarantiaAlt2"]);
                if (!string.IsNullOrWhiteSpace(dr["GarantiaAlt3"].ToString()))
                    this.GarantiaAlt3.Value = Convert.ToDecimal(dr["GarantiaAlt3"]);
                if (!string.IsNullOrWhiteSpace(dr["GarantiaAlt4"].ToString()))
                    this.GarantiaAlt4.Value = Convert.ToDecimal(dr["GarantiaAlt4"]);
                if (!string.IsNullOrWhiteSpace(dr["GarantiaAlt5"].ToString()))
                    this.GarantiaAlt5.Value = Convert.ToDecimal(dr["GarantiaAlt5"]);
                if (!string.IsNullOrWhiteSpace(dr["GarantiaAlt6"].ToString()))
                    this.GarantiaAlt6.Value = Convert.ToDecimal(dr["GarantiaAlt6"]);
                if (!string.IsNullOrWhiteSpace(dr["GarantiaAlt7"].ToString()))
                    this.GarantiaAlt7.Value = Convert.ToDecimal(dr["GarantiaAlt7"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrGtiaEvaluacion"].ToString()))
                    this.VlrGtiaEvaluacion.Value = Convert.ToDecimal(dr["VlrGtiaEvaluacion"]);
                if (!string.IsNullOrWhiteSpace(dr["MontoMaximo"].ToString()))
                    this.MontoMaximo.Value = Convert.ToDecimal(dr["MontoMaximo"]);
                if (!string.IsNullOrWhiteSpace(dr["EstimadoSeguros"].ToString()))
                    this.EstimadoSeguros.Value = Convert.ToDecimal(dr["EstimadoSeguros"]);
                if (!string.IsNullOrWhiteSpace(dr["MaxFinanciacionAut"].ToString()))
                    this.MaxFinanciacionAut.Value = Convert.ToDecimal(dr["MaxFinanciacionAut"]);
                if (!string.IsNullOrWhiteSpace(dr["Plazo"].ToString()))
                    this.Plazo.Value = Convert.ToByte(dr["Plazo"]);

                if (!string.IsNullOrWhiteSpace(dr["EstimadoObl"].ToString()))
                    this.EstimadoObl.Value = Convert.ToDecimal(dr["EstimadoObl"]);
                if (!string.IsNullOrWhiteSpace(dr["CuotaFin"].ToString()))
                    this.CuotaFin.Value = Convert.ToDecimal(dr["CuotaFin"]);
                if (!string.IsNullOrWhiteSpace(dr["CuotaSeg"].ToString()))
                    this.CuotaSeg.Value = Convert.ToDecimal(dr["CuotaSeg"]);
                if (!string.IsNullOrWhiteSpace(dr["CuotaTotal"].ToString()))
                    this.CuotaTotal.Value = Convert.ToDecimal(dr["CuotaTotal"]);
                if (!string.IsNullOrWhiteSpace(dr["Estado"].ToString()))
                    this.Estado.Value = Convert.ToString(dr["Estado"]);
                if (!string.IsNullOrWhiteSpace(dr["Calificacion"].ToString()))
                    this.Calificacion.Value = Convert.ToString(dr["Calificacion"]);
                if (!string.IsNullOrWhiteSpace(dr["Revision"].ToString()))
                    this.Revision.Value = Convert.ToString(dr["Revision"]);
                if (!string.IsNullOrWhiteSpace(dr["CargoResp"].ToString()))
                    this.CargoResp.Value = Convert.ToString(dr["CargoResp"]);
                if (!string.IsNullOrWhiteSpace(dr["Firma1Ind"].ToString()))
                    this.Firma1Ind.Value = Convert.ToBoolean(dr["Firma1Ind"]);
                if (!string.IsNullOrWhiteSpace(dr["Firma2Ind"].ToString()))
                    this.Firma2Ind.Value = Convert.ToBoolean(dr["Firma2Ind"]);
                if (!string.IsNullOrWhiteSpace(dr["Firma3Ind"].ToString()))
                    this.Firma3Ind.Value = Convert.ToBoolean(dr["Firma3Ind"]);

                if (!string.IsNullOrWhiteSpace(dr["UsuarioResp"].ToString()))
                    this.UsuarioResp.Value = Convert.ToString(dr["UsuarioResp"]);
                if (!string.IsNullOrWhiteSpace(dr["UsuarioFirma1"].ToString()))
                    this.UsuarioFirma1.Value = Convert.ToString(dr["UsuarioFirma1"]);
                if (!string.IsNullOrWhiteSpace(dr["UsuarioFirma2"].ToString()))
                    this.UsuarioFirma2.Value = Convert.ToString(dr["UsuarioFirma2"]);
                if (!string.IsNullOrWhiteSpace(dr["UsuarioFirma3"].ToString()))
                    this.UsuarioFirma3.Value = Convert.ToString(dr["UsuarioFirma3"]);

                if (!string.IsNullOrWhiteSpace(dr["FechaFirmaResp"].ToString()))
                    this.FechaFirmaResp.Value = Convert.ToDateTime(dr["FechaFirmaResp"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaFirma1"].ToString()))
                    this.FechaFirma1.Value = Convert.ToDateTime(dr["FechaFirma1"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaFirma2"].ToString()))
                    this.FechaFirma2.Value = Convert.ToDateTime(dr["FechaFirma2"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaFirma3"].ToString()))
                    this.FechaFirma3.Value = Convert.ToDateTime(dr["FechaFirma3"]);

                if (!string.IsNullOrWhiteSpace(dr["FechaDatacredito"].ToString()))
                    this.FechaDatacredito.Value = Convert.ToDateTime(dr["FechaDatacredito"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaDesembolso"].ToString()))
                    this.FechaDesembolso.Value = Convert.ToDateTime(dr["FechaDesembolso"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaLegalizacion"].ToString()))
                    this.FechaLegalizacion.Value = Convert.ToDateTime(dr["FechaLegalizacion"]);
                
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);

                if (!string.IsNullOrWhiteSpace(dr["CartaAprobDirInd"].ToString()))
                    this.CartaAprobDirInd.Value = Convert.ToBoolean(dr["CartaAprobDirInd"]);
                if (!string.IsNullOrWhiteSpace(dr["CartaAprobDocInd"].ToString()))
                    this.CartaAprobDocInd.Value = Convert.ToBoolean(dr["CartaAprobDocInd"]);
                if (!string.IsNullOrWhiteSpace(dr["CartapreAprobInd"].ToString()))
                    this.CartapreAprobInd.Value = Convert.ToBoolean(dr["CartapreAprobInd"]);
                if (!string.IsNullOrWhiteSpace(dr["CartaNoViableInd"].ToString()))
                    this.CartaNoViableInd.Value = Convert.ToBoolean(dr["CartaNoViableInd"]);
                if (!string.IsNullOrWhiteSpace(dr["CartaRevocaInd"].ToString()))
                    this.CartaRevocaInd.Value = Convert.ToBoolean(dr["CartaRevocaInd"]);
                if (!string.IsNullOrWhiteSpace(dr["CartaRatificaInd"].ToString()))
                    this.CartaRatificaInd.Value = Convert.ToBoolean(dr["CartaRatificaInd"]);
                if (!string.IsNullOrWhiteSpace(dr["CartaAprobDirUsu"].ToString()))
                    this.CartaAprobDirUsu.Value = Convert.ToString(dr["CartaAprobDirUsu"]);
                if (!string.IsNullOrWhiteSpace(dr["CartaAprobDocUsu"].ToString()))
                    this.CartaAprobDocUsu.Value = Convert.ToString(dr["CartaAprobDocUsu"]);
                if (!string.IsNullOrWhiteSpace(dr["CartapreAprobUsu"].ToString()))
                    this.CartapreAprobUsu.Value = Convert.ToString(dr["CartapreAprobUsu"]);
                if (!string.IsNullOrWhiteSpace(dr["CartaNoViableUsu"].ToString()))
                    this.CartaNoViableUsu.Value = Convert.ToString(dr["CartaNoViableUsu"]);
                if (!string.IsNullOrWhiteSpace(dr["CartaRevocaUsu"].ToString()))
                    this.CartaRevocaUsu.Value = Convert.ToString(dr["CartaRevocaUsu"]);
                if (!string.IsNullOrWhiteSpace(dr["CartaRatificaUsu"].ToString()))
                    this.CartaRatificaUsu.Value = Convert.ToString(dr["CartaRatificaUsu"]);
                if (!string.IsNullOrWhiteSpace(dr["CartaAprobDirFecha"].ToString()))
                    this.CartaAprobDirFecha.Value = Convert.ToDateTime(dr["CartaAprobDirFecha"]);
                if (!string.IsNullOrWhiteSpace(dr["CartaAprobDocFecha"].ToString()))
                    this.CartaAprobDocFecha.Value = Convert.ToDateTime(dr["CartaAprobDocFecha"]);
                if (!string.IsNullOrWhiteSpace(dr["CartapreAprobFecha"].ToString()))
                    this.CartapreAprobFecha.Value = Convert.ToDateTime(dr["CartapreAprobFecha"]);
                if (!string.IsNullOrWhiteSpace(dr["CartaNoViableFecha"].ToString()))
                    this.CartaNoViableFecha.Value = Convert.ToDateTime(dr["CartaNoViableFecha"]);               
                if (!string.IsNullOrWhiteSpace(dr["CartaRevocaFecha"].ToString()))
                    this.CartaRevocaFecha.Value = Convert.ToDateTime(dr["CartaRevocaFecha"]);
                if (!string.IsNullOrWhiteSpace(dr["CartaRatificaFecha"].ToString()))
                    this.CartaRatificaFecha.Value = Convert.ToDateTime(dr["CartaRatificaFecha"]);
                if (!string.IsNullOrWhiteSpace(dr["PerfilUsuario"].ToString()))
                    this.PerfilUsuario.Value = Convert.ToString(dr["PerfilUsuario"]);
                if (!string.IsNullOrWhiteSpace(dr["PerfilFecha"].ToString()))
                    this.PerfilFecha.Value = Convert.ToDateTime(dr["PerfilFecha"]);
                if (!string.IsNullOrWhiteSpace(dr["EstActualFactor"].ToString()))
                    this.EstActualFactor.Value = Convert.ToDecimal(dr["EstActualFactor"]);

                if (!string.IsNullOrWhiteSpace(dr["VlrSolicitado"].ToString()))
                    this.VlrSolicitado.Value = Convert.ToDecimal(dr["VlrSolicitado"]);

                if (!string.IsNullOrWhiteSpace(dr["SMMLV"].ToString()))
                    this.SMMLV.Value = Convert.ToDecimal(dr["SMMLV"]);

                if (!string.IsNullOrWhiteSpace(dr["PF_PorIngrPagoCtas"].ToString())) 
                    this.PF_PorIngrPagoCtas.Value = Convert.ToDecimal(dr["PF_PorIngrPagoCtas"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngrDispApoyosDEU"].ToString())) 
                    this.PF_IngrDispApoyosDEU.Value = Convert.ToDecimal(dr["PF_IngrDispApoyosDEU"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngrDispApoyosCON"].ToString()))
                    this.PF_IngrDispApoyosCON.Value = Convert.ToDecimal(dr["PF_IngrDispApoyosCON"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngrDispApoyos"].ToString())) 
                    this.PF_IngrDispApoyos.Value = Convert.ToDecimal(dr["PF_IngrDispApoyos"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_VlrMontoSOL"].ToString())) 
                    this.PF_VlrMontoSOL.Value = Convert.ToDecimal(dr["PF_VlrMontoSOL"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_VlrMontoAJU"].ToString()))
                    this.PF_VlrMontoAJU.Value = Convert.ToDecimal(dr["PF_VlrMontoAJU"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_VlrMontoINC"].ToString()))
                    this.PF_VlrMontoINC.Value = Convert.ToDecimal(dr["PF_VlrMontoINC"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_VlrMontoFIN"].ToString()))
                    this.PF_VlrMontoFIN.Value = Convert.ToDecimal(dr["PF_VlrMontoFIN"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_CtaFinanciaSOL"].ToString())) 
                    this.PF_CtaFinanciaSOL.Value = Convert.ToDecimal(dr["PF_CtaFinanciaSOL"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_CtaFinanciaAJU"].ToString())) 
                    this.PF_CtaFinanciaAJU.Value = Convert.ToDecimal(dr["PF_CtaFinanciaAJU"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_CtaFinanciaINC"].ToString())) 
                    this.PF_CtaFinanciaINC.Value = Convert.ToDecimal(dr["PF_CtaFinanciaINC"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_CtaFinanciaFIN"].ToString())) 
                    this.PF_CtaFinanciaFIN.Value = Convert.ToDecimal(dr["PF_CtaFinanciaFIN"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_CtaSeguroSOL"].ToString())) 
                    this.PF_CtaSeguroSOL.Value = Convert.ToDecimal(dr["PF_CtaSeguroSOL"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_CtaSeguroAJU"].ToString())) 
                    this.PF_CtaSeguroAJU.Value = Convert.ToDecimal(dr["PF_CtaSeguroAJU"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_CtaSeguroINC"].ToString())) 
                    this.PF_CtaSeguroINC.Value = Convert.ToDecimal(dr["PF_CtaSeguroINC"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_CtaSeguroFIN"].ToString())) 
                    this.PF_CtaSeguroFIN.Value = Convert.ToDecimal(dr["PF_CtaSeguroFIN"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_CtaTotalSOL"].ToString())) 
                    this.PF_CtaTotalSOL.Value = Convert.ToDecimal(dr["PF_CtaTotalSOL"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_CtaTotalAJU"].ToString())) 
                    this.PF_CtaTotalAJU.Value = Convert.ToDecimal(dr["PF_CtaTotalAJU"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_CtaTotalINC"].ToString())) 
                    this.PF_CtaTotalINC.Value = Convert.ToDecimal(dr["PF_CtaTotalINC"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_CtaTotalFIN"].ToString())) 
                    this.PF_CtaTotalFIN.Value = Convert.ToDecimal(dr["PF_CtaTotalFIN"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_CtaApoyosDifSOL"].ToString())) 
                    this.PF_CtaApoyosDifSOL.Value = Convert.ToDecimal(dr["PF_CtaApoyosDifSOL"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_CtaApoyosDifAJU"].ToString())) 
                    this.PF_CtaApoyosDifAJU.Value = Convert.ToDecimal(dr["PF_CtaApoyosDifAJU"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_CtaApoyosDifINC"].ToString())) 
                    this.PF_CtaApoyosDifINC.Value = Convert.ToDecimal(dr["PF_CtaApoyosDifINC"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_CtaApoyosDifFIN"].ToString())) 
                    this.PF_CtaApoyosDifFIN.Value = Convert.ToDecimal(dr["PF_CtaApoyosDifFIN"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngDispApoyosSOL"].ToString()))
                    this.PF_IngDispApoyosSOL.Value = Convert.ToDecimal(dr["PF_IngDispApoyosSOL"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngDispApoyosAJU"].ToString()))
                    this.PF_IngDispApoyosAJU.Value = Convert.ToDecimal(dr["PF_IngDispApoyosAJU"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngDispApoyosINC"].ToString()))
                    this.PF_IngDispApoyosINC.Value = Convert.ToDecimal(dr["PF_IngDispApoyosINC"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngDispApoyosFIN"].ToString()))
                    this.PF_IngDispApoyosFIN.Value = Convert.ToDecimal(dr["PF_IngDispApoyosFIN"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_ReqSopIngrIndDEU"].ToString()))
                    this.PF_ReqSopIngrIndDEU.Value = Convert.ToBoolean(dr["PF_ReqSopIngrIndDEU"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_ReqSopIngrIndCON"].ToString()))
                    this.PF_ReqSopIngrIndCON.Value = Convert.ToBoolean(dr["PF_ReqSopIngrIndCON"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngReqDeu1SOL"].ToString())) 
                    this.PF_IngReqDeu1SOL.Value = Convert.ToDecimal(dr["PF_IngReqDeu1SOL"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngReqDeu1AJU"].ToString())) 
                    this.PF_IngReqDeu1AJU.Value = Convert.ToDecimal(dr["PF_IngReqDeu1AJU"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngReqDeu1INC"].ToString())) 
                    this.PF_IngReqDeu1INC.Value = Convert.ToDecimal(dr["PF_IngReqDeu1INC"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngReqDeu1FIN"].ToString())) 
                    this.PF_IngReqDeu1FIN.Value = Convert.ToDecimal(dr["PF_IngReqDeu1FIN"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngReqDeu2SOL"].ToString())) 
                    this.PF_IngReqDeu2SOL.Value = Convert.ToDecimal(dr["PF_IngReqDeu2SOL"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngReqDeu2AJU"].ToString())) 
                    this.PF_IngReqDeu2AJU.Value = Convert.ToDecimal(dr["PF_IngReqDeu2AJU"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngReqDeu2INC"].ToString())) 
                    this.PF_IngReqDeu2INC.Value = Convert.ToDecimal(dr["PF_IngReqDeu2INC"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngReqDeu2FIN"].ToString())) 
                    this.PF_IngReqDeu2FIN.Value = Convert.ToDecimal(dr["PF_IngReqDeu2FIN"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngReqDeu3SOL"].ToString())) 
                    this.PF_IngReqDeu3SOL.Value = Convert.ToDecimal(dr["PF_IngReqDeu3SOL"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngReqDeuAJU"].ToString())) 
                    this.PF_IngReqDeuAJU.Value = Convert.ToDecimal(dr["PF_IngReqDeuAJU"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngReqDeu3INC"].ToString())) 
                    this.PF_IngReqDeu3INC.Value = Convert.ToDecimal(dr["PF_IngReqDeu3INC"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngReqDeu3FIN"].ToString()))
                    this.PF_IngReqDeu3FIN.Value = Convert.ToDecimal(dr["PF_IngReqDeu3FIN"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngReqDeuFinSOL"].ToString())) 
                    this.PF_IngReqDeuFinSOL.Value = Convert.ToDecimal(dr["PF_IngReqDeuFinSOL"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngReqDeuFinAJU"].ToString())) 
                    this.PF_IngReqDeuFinAJU.Value = Convert.ToDecimal(dr["PF_IngReqDeuFinAJU"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngReqDeuFinINC"].ToString()))
                    this.PF_IngReqDeuFinINC.Value = Convert.ToDecimal(dr["PF_IngReqDeuFinINC"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngReqDeuFinFIN"].ToString()))
                    this.PF_IngReqDeuFinFIN.Value = Convert.ToDecimal(dr["PF_IngReqDeuFinFIN"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngReqCon1SOL"].ToString()))
                    this.PF_IngReqCon1SOL.Value = Convert.ToDecimal(dr["PF_IngReqCon1SOL"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngReqCon1AJU"].ToString())) 
                    this.PF_IngReqCon1AJU.Value = Convert.ToDecimal(dr["PF_IngReqCon1AJU"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngReqCon1INC"].ToString())) 
                    this.PF_IngReqCon1INC.Value = Convert.ToDecimal(dr["PF_IngReqCon1INC"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngReqCon1FIN"].ToString())) 
                    this.PF_IngReqCon1FIN.Value = Convert.ToDecimal(dr["PF_IngReqCon1FIN"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngReqCon2SOL"].ToString())) 
                    this.PF_IngReqCon2SOL.Value = Convert.ToDecimal(dr["PF_IngReqCon2SOL"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngReqCon2AJU"].ToString()))
                    this.PF_IngReqCon2AJU.Value = Convert.ToDecimal(dr["PF_IngReqCon2AJU"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngReqCon2INC"].ToString())) 
                    this.PF_IngReqCon2INC.Value = Convert.ToDecimal(dr["PF_IngReqCon2INC"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngReqCon2FIN"].ToString()))
                    this.PF_IngReqCon2FIN.Value = Convert.ToDecimal(dr["PF_IngReqCon2FIN"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngReqConFinSOL"].ToString())) 
                    this.PF_IngReqConFinSOL.Value = Convert.ToDecimal(dr["PF_IngReqConFinSOL"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngReqConFinAJU"].ToString())) 
                    this.PF_IngReqConFinAJU.Value = Convert.ToDecimal(dr["PF_IngReqConFinAJU"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngReqConFinINC"].ToString()))
                    this.PF_IngReqConFinINC.Value = Convert.ToDecimal(dr["PF_IngReqConFinINC"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_IngReqConFinFIN"].ToString())) 
                    this.PF_IngReqConFinFIN.Value = Convert.ToDecimal(dr["PF_IngReqConFinFIN"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_Cuantia"].ToString())) 
                    this.PF_Cuantia.Value = Convert.ToDecimal(dr["PF_Cuantia"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_TasaTablEva1"].ToString())) 
                    this.PF_TasaTablEva1.Value = Convert.ToDecimal(dr["PF_TasaTablEva1"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_FactTablEva"].ToString())) 
                    this.PF_FactTablEva.Value = Convert.ToDecimal(dr["PF_FactTablEva"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_TasaTablEva2"].ToString()))
                    this.PF_TasaTablEva2.Value = Convert.ToDecimal(dr["PF_TasaTablEva2"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_TasaPonderada"].ToString())) 
                    this.PF_TasaPonderada.Value = Convert.ToDecimal(dr["PF_TasaPonderada"]);

                ///////////
                if (!string.IsNullOrWhiteSpace(dr["PF_PolCubriendoInd"].ToString())) 
                    this.PF_PolCubriendoInd.Value = Convert.ToBoolean(dr["PF_PolCubriendoInd"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_PlazoFinal"].ToString()))
                    this.PF_PlazoFinal.Value = Convert.ToByte(dr["PF_PlazoFinal"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_VlrMinimoGar"].ToString()))
                    this.PF_VlrMinimoGar.Value = Convert.ToDecimal(dr["PF_VlrMinimoGar"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_VlrMinimoFirma2"].ToString()))
                    this.PF_VlrMinimoFirma2.Value = Convert.ToDecimal(dr["PF_VlrMinimoFirma2"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_VlrMinimoFirma3"].ToString()))
                    this.PF_VlrMinimoFirma3.Value = Convert.ToDecimal(dr["PF_VlrMinimoFirma3"]);

                if (!string.IsNullOrWhiteSpace(dr["PF_CtaFinanciacion"].ToString())) 
                    this.PF_CtaFinanciacion.Value = Convert.ToDecimal(dr["PF_CtaFinanciacion"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_CtaSeguros"].ToString())) 
                    this.PF_CtaSeguros.Value = Convert.ToDecimal(dr["PF_CtaSeguros"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_PorEstimado"].ToString()))
                    this.PF_PorEstimado.Value = Convert.ToDecimal(dr["PF_PorEstimado"]);
                if (!string.IsNullOrWhiteSpace(dr["Verificacion1"].ToString())) 
                    this.Verificacion1.Value = Convert.ToString(dr["Verificacion1"]);
                if (!string.IsNullOrWhiteSpace(dr["Verificacion2"].ToString())) 
                    this.Verificacion2.Value = Convert.ToString(dr["Verificacion2"]);
                if (!string.IsNullOrWhiteSpace(dr["Verificacion3"].ToString())) 
                    this.Verificacion3.Value = Convert.ToString(dr["Verificacion3"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_Resultado1SOL"].ToString())) 
                    this.AN_Resultado1SOL.Value = Convert.ToDecimal(dr["AN_Resultado1SOL"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_Resultado1CA1"].ToString())) 
                    this.AN_Resultado1CA1.Value = Convert.ToDecimal(dr["AN_Resultado1CA1"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_Resultado1CA2"].ToString())) 
                    this.AN_Resultado1CA2.Value = Convert.ToDecimal(dr["AN_Resultado1CA2"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_Resultado1CA3"].ToString())) 
                    this.AN_Resultado1CA3.Value = Convert.ToDecimal(dr["AN_Resultado1CA3"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_Resultado1AJ1"].ToString())) 
                    this.AN_Resultado1AJ1.Value = Convert.ToDecimal(dr["AN_Resultado1AJ1"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_Resultado1AJ2"].ToString())) 
                    this.AN_Resultado1AJ2.Value = Convert.ToDecimal(dr["AN_Resultado1AJ2"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_Resultado1V11"].ToString())) 
                    this.AN_Resultado1V11.Value = Convert.ToDecimal(dr["AN_Resultado1V11"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_Resultado1V12"].ToString())) 
                    this.AN_Resultado1V12.Value = Convert.ToDecimal(dr["AN_Resultado1V12"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_Resultado1V21"].ToString())) 
                    this.AN_Resultado1V21.Value = Convert.ToDecimal(dr["AN_Resultado1V21"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_Resultado1V22"].ToString())) 
                    this.AN_Resultado1V22.Value = Convert.ToDecimal(dr["AN_Resultado1V22"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_Resultado1V31"].ToString())) 
                    this.AN_Resultado1V31.Value = Convert.ToDecimal(dr["AN_Resultado1V31"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_Resultado1V32"].ToString())) 
                    this.AN_Resultado1V32.Value = Convert.ToDecimal(dr["AN_Resultado1V32"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_Resultado2SOL"].ToString())) 
                    this.AN_Resultado2SOL.Value = Convert.ToDecimal(dr["AN_Resultado2SOL"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_Resultado2CA1"].ToString())) 
                    this.AN_Resultado2CA1.Value = Convert.ToDecimal(dr["AN_Resultado2CA1"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_Resultado2CA2"].ToString())) 
                    this.AN_Resultado2CA2.Value = Convert.ToDecimal(dr["AN_Resultado2CA2"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_Resultado2CA3"].ToString())) 
                    this.AN_Resultado2CA3.Value = Convert.ToDecimal(dr["AN_Resultado2CA3"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_Resultado2AJ1"].ToString())) 
                    this.AN_Resultado2AJ1.Value = Convert.ToDecimal(dr["AN_Resultado2AJ1"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_Resultado2AJ2"].ToString())) 
                    this.AN_Resultado2AJ2.Value = Convert.ToDecimal(dr["AN_Resultado2AJ2"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_Resultado2V11"].ToString())) 
                    this.AN_Resultado2V11.Value = Convert.ToDecimal(dr["AN_Resultado2V11"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_Resultado2V12"].ToString())) 
                    this.AN_Resultado2V12.Value = Convert.ToDecimal(dr["AN_Resultado2V12"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_Resultado2V21"].ToString())) 
                    this.AN_Resultado2V21.Value = Convert.ToDecimal(dr["AN_Resultado2V21"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_Resultado2V22"].ToString())) 
                    this.AN_Resultado2V22.Value = Convert.ToDecimal(dr["AN_Resultado2V22"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_Resultado2V31"].ToString())) 
                    this.AN_Resultado2V31.Value = Convert.ToDecimal(dr["AN_Resultado2V31"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_Resultado2V32"].ToString())) 
                    this.AN_Resultado2V32.Value = Convert.ToDecimal(dr["AN_Resultado2V32"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrVenta"].ToString())) 
                    this.AN_VlrVenta.Value = Convert.ToDecimal(dr["AN_VlrVenta"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrFasecolda"].ToString()))
                    this.AN_VlrFasecolda.Value = Convert.ToDecimal(dr["AN_VlrFasecolda"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrCtaEvaSOL"].ToString()))
                    this.AN_VlrCtaEvaSOL.Value = Convert.ToDecimal(dr["AN_VlrCtaEvaSOL"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrCtaEvaCA1"].ToString()))
                    this.AN_VlrCtaEvaCA1.Value = Convert.ToDecimal(dr["AN_VlrCtaEvaCA1"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrCtaEvaCA2"].ToString()))
                    this.AN_VlrCtaEvaCA2.Value = Convert.ToDecimal(dr["AN_VlrCtaEvaCA2"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrCtaEvaCA3"].ToString()))
                    this.AN_VlrCtaEvaCA3.Value = Convert.ToDecimal(dr["AN_VlrCtaEvaCA3"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrCtaEvaAJ1"].ToString()))
                    this.AN_VlrCtaEvaAJ1.Value = Convert.ToDecimal(dr["AN_VlrCtaEvaAJ1"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrCtaEvaAJ2"].ToString()))
                    this.AN_VlrCtaEvaAJ2.Value = Convert.ToDecimal(dr["AN_VlrCtaEvaAJ2"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrCtaEvaV11"].ToString()))
                    this.AN_VlrCtaEvaV11.Value = Convert.ToDecimal(dr["AN_VlrCtaEvaV11"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrCtaEvaV12"].ToString()))
                    this.AN_VlrCtaEvaV12.Value = Convert.ToDecimal(dr["AN_VlrCtaEvaV12"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrCtaEvaV21"].ToString()))
                    this.AN_VlrCtaEvaV21.Value = Convert.ToDecimal(dr["AN_VlrCtaEvaV21"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrCtaEvaV22"].ToString()))
                    this.AN_VlrCtaEvaV22.Value = Convert.ToDecimal(dr["AN_VlrCtaEvaV22"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrCtaEvaV31"].ToString()))
                    this.AN_VlrCtaEvaV31.Value = Convert.ToDecimal(dr["AN_VlrCtaEvaV31"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrCtaEvaV32"].ToString()))
                    this.AN_VlrCtaEvaV32.Value = Convert.ToDecimal(dr["AN_VlrCtaEvaV32"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrMinLim"].ToString())) 
                    this.AN_VlrMinLim.Value = Convert.ToDecimal(dr["AN_VlrMinLim"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrMaxLim"].ToString())) 
                    this.AN_VlrMaxLim.Value = Convert.ToDecimal(dr["AN_VlrMaxLim"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrSolicitado"].ToString())) 
                    this.AN_VlrSolicitado.Value = Convert.ToDecimal(dr["AN_VlrSolicitado"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrAlternCA1"].ToString())) 
                    this.AN_VlrAlternCA1.Value = Convert.ToDecimal(dr["AN_VlrAlternCA1"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrAlternCA2"].ToString()))
                    this.AN_VlrAlternCA2.Value = Convert.ToDecimal(dr["AN_VlrAlternCA2"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrAlternCA3"].ToString()))
                    this.AN_VlrAlternCA3.Value = Convert.ToDecimal(dr["AN_VlrAlternCA3"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrAlternAJ1"].ToString()))
                    this.AN_VlrAlternAJ1.Value = Convert.ToDecimal(dr["AN_VlrAlternAJ1"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrAlternAJ2"].ToString()))
                    this.AN_VlrAlternAJ2.Value = Convert.ToDecimal(dr["AN_VlrAlternAJ2"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrAlternV11"].ToString()))
                    this.AN_VlrAlternV11.Value = Convert.ToDecimal(dr["AN_VlrAlternV11"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrAlternV12"].ToString()))
                    this.AN_VlrAlternV12.Value = Convert.ToDecimal(dr["AN_VlrAlternV12"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrAlternV21"].ToString()))
                    this.AN_VlrAlternV21.Value = Convert.ToDecimal(dr["AN_VlrAlternV21"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrAlternV22"].ToString()))
                    this.AN_VlrAlternV22.Value = Convert.ToDecimal(dr["AN_VlrAlternV22"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrAlternV31"].ToString()))
                    this.AN_VlrAlternV31.Value = Convert.ToDecimal(dr["AN_VlrAlternV31"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrAlternV32"].ToString()))
                    this.AN_VlrAlternV32.Value = Convert.ToDecimal(dr["AN_VlrAlternV32"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CtaInicial"].ToString())) 
                    this.AN_CtaInicial.Value = Convert.ToDecimal(dr["AN_CtaInicial"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrIncremSOL"].ToString()))
                    this.AN_VlrIncremSOL.Value = Convert.ToDecimal(dr["AN_VlrIncremSOL"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrIncremCA1"].ToString()))
                    this.AN_VlrIncremCA1.Value = Convert.ToDecimal(dr["AN_VlrIncremCA1"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrIncremCA2"].ToString()))
                    this.AN_VlrIncremCA2.Value = Convert.ToDecimal(dr["AN_VlrIncremCA2"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrIncremCA3"].ToString()))
                    this.AN_VlrIncremCA3.Value = Convert.ToDecimal(dr["AN_VlrIncremCA3"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrIncremAJ1"].ToString()))
                    this.AN_VlrIncremAJ1.Value = Convert.ToDecimal(dr["AN_VlrIncremAJ1"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrIncremAJ2"].ToString()))
                    this.AN_VlrIncremAJ2.Value = Convert.ToDecimal(dr["AN_VlrIncremAJ2"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrIncremV11"].ToString()))
                    this.AN_VlrIncremV11.Value = Convert.ToDecimal(dr["AN_VlrIncremV11"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrIncremV12"].ToString()))
                    this.AN_VlrIncremV12.Value = Convert.ToDecimal(dr["AN_VlrIncremV12"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrIncremV21"].ToString()))
                    this.AN_VlrIncremV21.Value = Convert.ToDecimal(dr["AN_VlrIncremV21"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrIncremV22"].ToString()))
                    this.AN_VlrIncremV22.Value = Convert.ToDecimal(dr["AN_VlrIncremV22"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrIncremV31"].ToString()))
                    this.AN_VlrIncremV31.Value = Convert.ToDecimal(dr["AN_VlrIncremV31"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrIncremV32"].ToString())) 
                    this.AN_VlrIncremV32.Value = Convert.ToDecimal(dr["AN_VlrIncremV32"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CtaIniAjuCA1"].ToString())) 
                    this.AN_CtaIniAjuCA1.Value = Convert.ToDecimal(dr["AN_CtaIniAjuCA1"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CtaIniAjuCA2"].ToString())) 
                    this.AN_CtaIniAjuCA2.Value = Convert.ToDecimal(dr["AN_CtaIniAjuCA2"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CtaIniAjuCA3"].ToString())) 
                    this.AN_CtaIniAjuCA3.Value = Convert.ToDecimal(dr["AN_CtaIniAjuCA3"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CtaIniAjuAJ1"].ToString())) 
                    this.AN_CtaIniAjuAJ1.Value = Convert.ToDecimal(dr["AN_CtaIniAjuAJ1"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CtaIniAjuAJ2"].ToString())) 
                    this.AN_CtaIniAjuAJ2.Value = Convert.ToDecimal(dr["AN_CtaIniAjuAJ2"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CtaIniAjuV11"].ToString())) 
                    this.AN_CtaIniAjuV11.Value = Convert.ToDecimal(dr["AN_CtaIniAjuV11"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CtaIniAjuV12"].ToString())) 
                    this.AN_CtaIniAjuV12.Value = Convert.ToDecimal(dr["AN_CtaIniAjuV12"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CtaIniAjuV21"].ToString())) 
                    this.AN_CtaIniAjuV21.Value = Convert.ToDecimal(dr["AN_CtaIniAjuV21"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CtaIniAjuV22"].ToString())) 
                    this.AN_CtaIniAjuV22.Value = Convert.ToDecimal(dr["AN_CtaIniAjuV22"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CtaIniAjuV31"].ToString())) 
                    this.AN_CtaIniAjuV31.Value = Convert.ToDecimal(dr["AN_CtaIniAjuV31"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CtaIniAjuV32"].ToString())) 
                    this.AN_CtaIniAjuV32.Value = Convert.ToDecimal(dr["AN_CtaIniAjuV32"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_porFinSoli"].ToString())) 
                    this.AN_porFinSoli.Value = Convert.ToDecimal(dr["AN_porFinSoli"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_porFinMaxSOL"].ToString()))
                    this.AN_porFinMaxSOL.Value = Convert.ToDecimal(dr["AN_porFinMaxSOL"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_porFinMaxCA1"].ToString()))
                    this.AN_porFinMaxCA1.Value = Convert.ToDecimal(dr["AN_porFinMaxCA1"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_porFinMaxCA2"].ToString()))
                    this.AN_porFinMaxCA2.Value = Convert.ToDecimal(dr["AN_porFinMaxCA2"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_porFinMaxCA3"].ToString()))
                    this.AN_porFinMaxCA3.Value = Convert.ToDecimal(dr["AN_porFinMaxCA3"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_porFinMaxAJ1"].ToString()))
                    this.AN_porFinMaxAJ1.Value = Convert.ToDecimal(dr["AN_porFinMaxAJ1"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_porFinMaxAJ2"].ToString()))
                    this.AN_porFinMaxAJ2.Value = Convert.ToDecimal(dr["AN_porFinMaxAJ2"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_porFinMaxV11"].ToString()))
                    this.AN_porFinMaxV11.Value = Convert.ToDecimal(dr["AN_porFinMaxV11"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_porFinMaxV12"].ToString()))
                    this.AN_porFinMaxV12.Value = Convert.ToDecimal(dr["AN_porFinMaxV12"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_porFinMaxV21"].ToString())) 
                    this.AN_porFinMaxV21.Value = Convert.ToDecimal(dr["AN_porFinMaxV21"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_porFinMaxV22"].ToString())) 
                    this.AN_porFinMaxV22.Value = Convert.ToDecimal(dr["AN_porFinMaxV22"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_porFinMaxV31"].ToString())) 
                    this.AN_porFinMaxV31.Value = Convert.ToDecimal(dr["AN_porFinMaxV31"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_porFinMaxV32"].ToString())) 
                    this.AN_porFinMaxV32.Value = Convert.ToDecimal(dr["AN_porFinMaxV32"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_porFinAltCA1"].ToString())) 
                    this.AN_porFinAltCA1.Value = Convert.ToDecimal(dr["AN_porFinAltCA1"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_porFinAltCA2"].ToString())) 
                    this.AN_porFinAltCA2.Value = Convert.ToDecimal(dr["AN_porFinAltCA2"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_porFinAltCA3"].ToString())) 
                    this.AN_porFinAltCA3.Value = Convert.ToDecimal(dr["AN_porFinAltCA3"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_porFinAltAJ1"].ToString())) 
                    this.AN_porFinAltAJ1.Value = Convert.ToDecimal(dr["AN_porFinAltAJ1"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_porFinAltAJ2"].ToString())) 
                    this.AN_porFinAltAJ2.Value = Convert.ToDecimal(dr["AN_porFinAltAJ2"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_porFinAltV11"].ToString())) 
                    this.AN_porFinAltV11.Value = Convert.ToDecimal(dr["AN_porFinAltV11"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_porFinAltV12"].ToString())) 
                    this.AN_porFinAltV12.Value = Convert.ToDecimal(dr["AN_porFinAltV12"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_porFinAltV21"].ToString())) 
                    this.AN_porFinAltV21.Value = Convert.ToDecimal(dr["AN_porFinAltV21"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_porFinAltV22"].ToString())) 
                    this.AN_porFinAltV22.Value = Convert.ToDecimal(dr["AN_porFinAltV22"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_porFinAltV31"].ToString())) 
                    this.AN_porFinAltV31.Value = Convert.ToDecimal(dr["AN_porFinAltV31"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_porFinAltV32"].ToString())) 
                    this.AN_porFinAltV32.Value = Convert.ToDecimal(dr["AN_porFinAltV32"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumParGarAJ1"].ToString())) 
                    this.AN_CumParGarAJ1.Value = Convert.ToBoolean(dr["AN_CumParGarAJ1"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumParGarAJ2"].ToString())) 
                    this.AN_CumParGarAJ2.Value = Convert.ToBoolean(dr["AN_CumParGarAJ2"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumParGarV11"].ToString())) 
                    this.AN_CumParGarV11.Value = Convert.ToBoolean(dr["AN_CumParGarV11"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumParGarV12"].ToString())) 
                    this.AN_CumParGarV12.Value = Convert.ToBoolean(dr["AN_CumParGarV12"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumParGarV21"].ToString())) 
                    this.AN_CumParGarV21.Value = Convert.ToBoolean(dr["AN_CumParGarV21"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumParGarV22"].ToString())) 
                    this.AN_CumParGarV22.Value = Convert.ToBoolean(dr["AN_CumParGarV22"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumParGarV31"].ToString())) 
                    this.AN_CumParGarV31.Value = Convert.ToBoolean(dr["AN_CumParGarV31"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumParGarV32"].ToString())) 
                    this.AN_CumParGarV32.Value = Convert.ToBoolean(dr["AN_CumParGarV32"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CapPagoSOL"].ToString())) 
                    this.AN_CapPagoSOL.Value = Convert.ToString(dr["AN_CapPagoSOL"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CapPagoCA1"].ToString())) 
                    this.AN_CapPagoCA1.Value = Convert.ToString(dr["AN_CapPagoCA1"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CapPagoCA2"].ToString())) 
                    this.AN_CapPagoCA2.Value = Convert.ToString(dr["AN_CapPagoCA2"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CapPagoCA3"].ToString())) 
                    this.AN_CapPagoCA3.Value = Convert.ToString(dr["AN_CapPagoCA3"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CapPagoAJ1"].ToString())) 
                    this.AN_CapPagoAJ1.Value = Convert.ToString(dr["AN_CapPagoAJ1"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CapPagoAJ2"].ToString())) 
                    this.AN_CapPagoAJ2.Value = Convert.ToString(dr["AN_CapPagoAJ2"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CapPagoV11"].ToString())) 
                    this.AN_CapPagoV11.Value = Convert.ToString(dr["AN_CapPagoV11"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CapPagoV12"].ToString())) 
                    this.AN_CapPagoV12.Value = Convert.ToString(dr["AN_CapPagoV12"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CapPagoV21"].ToString())) 
                    this.AN_CapPagoV21.Value = Convert.ToString(dr["AN_CapPagoV21"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CapPagoV22"].ToString())) 
                    this.AN_CapPagoV22.Value = Convert.ToString(dr["AN_CapPagoV22"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CapPagoV31"].ToString())) 
                    this.AN_CapPagoV31.Value = Convert.ToString(dr["AN_CapPagoV31"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CapPagoV32"].ToString())) 
                    this.AN_CapPagoV32.Value = Convert.ToString(dr["AN_CapPagoV32"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_PuedeIncCICA1"].ToString()))
                    this.AN_PuedeIncCICA1.Value = Convert.ToBoolean(dr["AN_PuedeIncCICA1"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_PuedeIncCICA2"].ToString()))
                    this.AN_PuedeIncCICA2.Value = Convert.ToBoolean(dr["AN_PuedeIncCICA2"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_PuedeIncCICA3"].ToString()))
                    this.AN_PuedeIncCICA3.Value = Convert.ToBoolean(dr["AN_PuedeIncCICA3"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_PuedeIncCIAJ1"].ToString()))
                    this.AN_PuedeIncCIAJ1.Value = Convert.ToBoolean(dr["AN_PuedeIncCIAJ1"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_PuedeIncCIAJ2"].ToString()))
                    this.AN_PuedeIncCIAJ2.Value = Convert.ToBoolean(dr["AN_PuedeIncCIAJ2"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_PuedeIncCIV11"].ToString()))
                    this.AN_PuedeIncCIV11.Value = Convert.ToBoolean(dr["AN_PuedeIncCIV11"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_PuedeIncCIV12"].ToString()))
                    this.AN_PuedeIncCIV12.Value = Convert.ToBoolean(dr["AN_PuedeIncCIV12"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_PuedeIncCIV21"].ToString()))
                    this.AN_PuedeIncCIV21.Value = Convert.ToBoolean(dr["AN_PuedeIncCIV21"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_PuedeIncCIV22"].ToString()))
                    this.AN_PuedeIncCIV22.Value = Convert.ToBoolean(dr["AN_PuedeIncCIV22"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_PuedeIncCIV31"].ToString()))
                    this.AN_PuedeIncCIV31.Value = Convert.ToBoolean(dr["AN_PuedeIncCIV31"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_PuedeIncCIV32"].ToString()))
                    this.AN_PuedeIncCIV32.Value = Convert.ToBoolean(dr["AN_PuedeIncCIV32"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumPorMaxSOL"].ToString())) 
                    this.AN_CumPorMaxSOL.Value = Convert.ToBoolean(dr["AN_CumPorMaxSOL"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumPorMaxCA1"].ToString())) 
                    this.AN_CumPorMaxCA1.Value = Convert.ToBoolean(dr["AN_CumPorMaxCA1"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumPorMaxCA2"].ToString())) 
                    this.AN_CumPorMaxCA2.Value = Convert.ToBoolean(dr["AN_CumPorMaxCA2"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumPorMaxCA3"].ToString())) 
                    this.AN_CumPorMaxCA3.Value = Convert.ToBoolean(dr["AN_CumPorMaxCA3"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumPorMaxAJ1"].ToString())) 
                    this.AN_CumPorMaxAJ1.Value = Convert.ToBoolean(dr["AN_CumPorMaxAJ1"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumPorMaxAJ2"].ToString())) 
                    this.AN_CumPorMaxAJ2.Value = Convert.ToBoolean(dr["AN_CumPorMaxAJ2"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumPorMaxV11"].ToString())) 
                    this.AN_CumPorMaxV11.Value = Convert.ToBoolean(dr["AN_CumPorMaxV11"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumPorMaxV12"].ToString())) 
                    this.AN_CumPorMaxV12.Value = Convert.ToBoolean(dr["AN_CumPorMaxV12"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumPorMaxV21"].ToString())) 
                    this.AN_CumPorMaxV21.Value = Convert.ToBoolean(dr["AN_CumPorMaxV21"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumPorMaxV22"].ToString())) 
                    this.AN_CumPorMaxV22.Value = Convert.ToBoolean(dr["AN_CumPorMaxV22"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumPorMaxV31"].ToString())) 
                    this.AN_CumPorMaxV31.Value = Convert.ToBoolean(dr["AN_CumPorMaxV31"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumPorMaxV32"].ToString())) 
                    this.AN_CumPorMaxV32.Value = Convert.ToBoolean(dr["AN_CumPorMaxV32"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumMtoMinCA1"].ToString())) 
                    this.AN_CumMtoMinCA1.Value = Convert.ToBoolean(dr["AN_CumMtoMinCA1"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumMtoMinCA2"].ToString())) 
                    this.AN_CumMtoMinCA2.Value = Convert.ToBoolean(dr["AN_CumMtoMinCA2"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumMtoMinCA3"].ToString())) 
                    this.AN_CumMtoMinCA3.Value = Convert.ToBoolean(dr["AN_CumMtoMinCA3"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumMtoMinAJ1"].ToString())) 
                    this.AN_CumMtoMinAJ1.Value = Convert.ToBoolean(dr["AN_CumMtoMinAJ1"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumMtoMinAJ2"].ToString())) 
                    this.AN_CumMtoMinAJ2.Value = Convert.ToBoolean(dr["AN_CumMtoMinAJ2"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumMtoMinV11"].ToString())) 
                    this.AN_CumMtoMinV11.Value = Convert.ToBoolean(dr["AN_CumMtoMinV11"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumMtoMinV12"].ToString())) 
                    this.AN_CumMtoMinV12.Value = Convert.ToBoolean(dr["AN_CumMtoMinV12"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumMtoMinV21"].ToString())) 
                    this.AN_CumMtoMinV21.Value = Convert.ToBoolean(dr["AN_CumMtoMinV21"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumMtoMinV22"].ToString())) 
                    this.AN_CumMtoMinV22.Value = Convert.ToBoolean(dr["AN_CumMtoMinV22"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumMtoMinV31"].ToString())) 
                    this.AN_CumMtoMinV31.Value = Convert.ToBoolean(dr["AN_CumMtoMinV31"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumMtoMinV32"].ToString())) 
                    this.AN_CumMtoMinV32.Value = Convert.ToBoolean(dr["AN_CumMtoMinV32"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumOtroSOL"].ToString())) 
                    this.AN_CumOtroSOL.Value = Convert.ToString(dr["AN_CumOtroSOL"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumOtroCA1"].ToString())) 
                    this.AN_CumOtroCA1.Value = Convert.ToString(dr["AN_CumOtroCA1"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumOtroCA2"].ToString())) 
                    this.AN_CumOtroCA2.Value = Convert.ToString(dr["AN_CumOtroCA2"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumOtroCA3"].ToString())) 
                    this.AN_CumOtroCA3.Value = Convert.ToString(dr["AN_CumOtroCA3"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumOtroAJ1"].ToString())) 
                    this.AN_CumOtroAJ1.Value = Convert.ToString(dr["AN_CumOtroAJ1"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumOtroAJ2"].ToString())) 
                    this.AN_CumOtroAJ2.Value = Convert.ToString(dr["AN_CumOtroAJ2"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumOtroV11"].ToString())) 
                    this.AN_CumOtroV11.Value = Convert.ToString(dr["AN_CumOtroV11"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumOtroV12"].ToString())) 
                    this.AN_CumOtroV12.Value = Convert.ToString(dr["AN_CumOtroV12"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumOtroV21"].ToString())) 
                    this.AN_CumOtroV21.Value = Convert.ToString(dr["AN_CumOtroV21"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumOtroV22"].ToString())) 
                    this.AN_CumOtroV22.Value = Convert.ToString(dr["AN_CumOtroV22"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumOtroV31"].ToString())) 
                    this.AN_CumOtroV31.Value = Convert.ToString(dr["AN_CumOtroV31"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_CumOtroV32"].ToString())) 
                    this.AN_CumOtroV32.Value = Convert.ToString(dr["AN_CumOtroV32"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrAutorizaSOL"].ToString()))
                    this.AN_VlrAutorizaSOL.Value = Convert.ToDecimal(dr["AN_VlrAutorizaSOL"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrAutorizaCA1"].ToString()))
                    this.AN_VlrAutorizaCA1.Value = Convert.ToDecimal(dr["AN_VlrAutorizaCA1"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrAutorizaCA2"].ToString()))
                    this.AN_VlrAutorizaCA2.Value = Convert.ToDecimal(dr["AN_VlrAutorizaCA2"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrAutorizaCA3"].ToString()))
                    this.AN_VlrAutorizaCA3.Value = Convert.ToDecimal(dr["AN_VlrAutorizaCA3"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrAutorizaAJ1"].ToString()))
                    this.AN_VlrAutorizaAJ1.Value = Convert.ToDecimal(dr["AN_VlrAutorizaAJ1"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrAutorizaAJ2"].ToString()))
                    this.AN_VlrAutorizaAJ2.Value = Convert.ToDecimal(dr["AN_VlrAutorizaAJ2"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrAutorizaV11"].ToString()))
                    this.AN_VlrAutorizaV11.Value = Convert.ToDecimal(dr["AN_VlrAutorizaV11"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrAutorizaV12"].ToString()))
                    this.AN_VlrAutorizaV12.Value = Convert.ToDecimal(dr["AN_VlrAutorizaV12"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrAutorizaV21"].ToString()))
                    this.AN_VlrAutorizaV21.Value = Convert.ToDecimal(dr["AN_VlrAutorizaV21"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrAutorizaV22"].ToString()))
                    this.AN_VlrAutorizaV22.Value = Convert.ToDecimal(dr["AN_VlrAutorizaV22"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrAutorizaV31"].ToString()))
                    this.AN_VlrAutorizaV31.Value = Convert.ToDecimal(dr["AN_VlrAutorizaV31"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrAutorizaV32"].ToString()))
                    this.AN_VlrAutorizaV32.Value = Convert.ToDecimal(dr["AN_VlrAutorizaV32"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrAutFinSOL"].ToString())) 
                    this.AN_VlrAutFinSOL.Value = Convert.ToDecimal(dr["AN_VlrAutFinSOL"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrAutFinCA2"].ToString()))
                    this.AN_VlrAutFinCA2.Value = Convert.ToDecimal(dr["AN_VlrAutFinCA2"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrAutFinAJ1"].ToString()))
                    this.AN_VlrAutFinAJ1.Value = Convert.ToDecimal(dr["AN_VlrAutFinAJ1"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrAutFinV12"].ToString()))
                    this.AN_VlrAutFinV12.Value = Convert.ToDecimal(dr["AN_VlrAutFinV12"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrAutFinV21"].ToString()))
                    this.AN_VlrAutFinV21.Value = Convert.ToDecimal(dr["AN_VlrAutFinV21"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_VlrAutFinV31"].ToString()))
                    this.AN_VlrAutFinV31.Value = Convert.ToDecimal(dr["AN_VlrAutFinV31"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_AltNoComAJ1"].ToString())) 
                    this.AN_AltNoComAJ1.Value = Convert.ToString(dr["AN_AltNoComAJ1"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_AltNoComAJ2"].ToString())) 
                    this.AN_AltNoComAJ2.Value = Convert.ToString(dr["AN_AltNoComAJ2"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_AltNoComV12"].ToString())) 
                    this.AN_AltNoComV12.Value = Convert.ToString(dr["AN_AltNoComV12"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_AltNoComV21"].ToString())) 
                    this.AN_AltNoComV21.Value = Convert.ToString(dr["AN_AltNoComV21"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_AltNoComV22"].ToString())) 
                    this.AN_AltNoComV22.Value = Convert.ToString(dr["AN_AltNoComV22"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_AltNoComV31"].ToString())) 
                    this.AN_AltNoComV31.Value = Convert.ToString(dr["AN_AltNoComV31"]);
                if (!string.IsNullOrWhiteSpace(dr["AN_AltNoComV32"].ToString())) 
                    this.AN_AltNoComV32.Value = Convert.ToString(dr["AN_AltNoComV32"]);

                if (!string.IsNullOrWhiteSpace(dr["Alternativa8"].ToString())) 
                    this.Alternativa8.Value = Convert.ToString(dr["Alternativa8"]);
                if (!string.IsNullOrWhiteSpace(dr["FactorAlt8"].ToString())) 
                    this.FactorAlt8.Value = Convert.ToDecimal(dr["FactorAlt8"]);
                if (!string.IsNullOrWhiteSpace(dr["MontoAlt8"].ToString())) 
                    this.MontoAlt8.Value = Convert.ToDecimal(dr["MontoAlt8"]);
                if (!string.IsNullOrWhiteSpace(dr["GarantiaAlt8"].ToString()))
                    this.GarantiaAlt8.Value = Convert.ToDecimal(dr["GarantiaAlt8"]);
                if (!string.IsNullOrWhiteSpace(dr["IngrMinSopDeu1"].ToString())) 
                    this.IngrMinSopDeu1.Value = Convert.ToDecimal(dr["IngrMinSopDeu1"]);
                if (!string.IsNullOrWhiteSpace(dr["IngrMinSopDeu2"].ToString())) 
                    this.IngrMinSopDeu2.Value = Convert.ToDecimal(dr["IngrMinSopDeu2"]);
                if (!string.IsNullOrWhiteSpace(dr["IngrMinSopDeu3"].ToString())) 
                    this.IngrMinSopDeu3.Value = Convert.ToDecimal(dr["IngrMinSopDeu3"]);
                if (!string.IsNullOrWhiteSpace(dr["IngrMinSopDeu4"].ToString())) 
                    this.IngrMinSopDeu4.Value = Convert.ToDecimal(dr["IngrMinSopDeu4"]);
                if (!string.IsNullOrWhiteSpace(dr["IngrMinSopDeu5"].ToString())) 
                    this.IngrMinSopDeu5.Value = Convert.ToDecimal(dr["IngrMinSopDeu5"]);
                if (!string.IsNullOrWhiteSpace(dr["IngrMinSopDeu6"].ToString())) 
                    this.IngrMinSopDeu6.Value = Convert.ToDecimal(dr["IngrMinSopDeu6"]);
                if (!string.IsNullOrWhiteSpace(dr["IngrMinSopDeu7"].ToString())) 
                    this.IngrMinSopDeu7.Value = Convert.ToDecimal(dr["IngrMinSopDeu7"]);
                if (!string.IsNullOrWhiteSpace(dr["IngrMinSopDeu8"].ToString())) 
                    this.IngrMinSopDeu8.Value = Convert.ToDecimal(dr["IngrMinSopDeu8"]);
                if (!string.IsNullOrWhiteSpace(dr["IngrMinSopCon1"].ToString())) 
                    this.IngrMinSopCon1.Value = Convert.ToDecimal(dr["IngrMinSopCon1"]);
                if (!string.IsNullOrWhiteSpace(dr["IngrMinSopCon2"].ToString())) 
                    this.IngrMinSopCon2.Value = Convert.ToDecimal(dr["IngrMinSopCon2"]);
                if (!string.IsNullOrWhiteSpace(dr["IngrMinSopCon3"].ToString())) 
                    this.IngrMinSopCon3.Value = Convert.ToDecimal(dr["IngrMinSopCon3"]);
                if (!string.IsNullOrWhiteSpace(dr["IngrMinSopCon4"].ToString())) 
                    this.IngrMinSopCon4.Value = Convert.ToDecimal(dr["IngrMinSopCon4"]);
                if (!string.IsNullOrWhiteSpace(dr["IngrMinSopCon5"].ToString())) 
                    this.IngrMinSopCon5.Value = Convert.ToDecimal(dr["IngrMinSopCon5"]);
                if (!string.IsNullOrWhiteSpace(dr["IngrMinSopCon6"].ToString())) 
                    this.IngrMinSopCon6.Value = Convert.ToDecimal(dr["IngrMinSopCon6"]);
                if (!string.IsNullOrWhiteSpace(dr["IngrMinSopCon7"].ToString())) 
                    this.IngrMinSopCon7.Value = Convert.ToDecimal(dr["IngrMinSopCon7"]);
                if (!string.IsNullOrWhiteSpace(dr["IngrMinSopCon8"].ToString())) 
                    this.IngrMinSopCon8.Value = Convert.ToDecimal(dr["IngrMinSopCon8"]);

                if (!string.IsNullOrWhiteSpace(dr["FechaFirmaDocumento"].ToString()))
                    this.FechaFirmaDocumento.Value = Convert.ToDateTime(dr["FechaFirmaDocumento"]);
                if (!string.IsNullOrWhiteSpace(dr["AccionSolicitud"].ToString()))
                    this.AccionSolicitud.Value = Convert.ToByte(dr["AccionSolicitud"]);
                if (!string.IsNullOrWhiteSpace(dr["Observacion"].ToString()))
                    this.Observacion.Value = Convert.ToString(dr["Observacion"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_Plazo"].ToString()))
                    this.PF_Plazo.Value = Convert.ToByte(dr["PF_Plazo"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_PorMaxEstadoActual"].ToString()))
                    this.PF_PorMaxEstadoActual.Value = Convert.ToDecimal(dr["PF_PorMaxEstadoActual"]);
                if (!string.IsNullOrWhiteSpace(dr["porMaximo"].ToString()))
                    this.porMaximo.Value = Convert.ToDecimal(dr["porMaximo"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrGarantia"].ToString()))
                    this.VlrGarantia.Value = Convert.ToDecimal(dr["VlrGarantia"]);
                if (!string.IsNullOrWhiteSpace(dr["AccionSolicitud1"].ToString()))
                    this.AccionSolicitud1.Value = Convert.ToByte(dr["AccionSolicitud1"]);
                if (!string.IsNullOrWhiteSpace(dr["AccionSolicitud2"].ToString()))
                    this.AccionSolicitud2.Value = Convert.ToByte(dr["AccionSolicitud2"]);
                if (!string.IsNullOrWhiteSpace(dr["AccionSolicitud3"].ToString()))
                    this.AccionSolicitud3.Value = Convert.ToByte(dr["AccionSolicitud3"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_PlazoFinal1"].ToString()))
                    this.PF_PlazoFinal1.Value = Convert.ToByte(dr["PF_PlazoFinal1"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_PlazoFinal2"].ToString()))
                    this.PF_PlazoFinal2.Value = Convert.ToByte(dr["PF_PlazoFinal2"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_PlazoFinal3"].ToString()))
                    this.PF_PlazoFinal3.Value = Convert.ToByte(dr["PF_PlazoFinal3"]);

                if (!string.IsNullOrWhiteSpace(dr["PF_VlrMontoFirma1"].ToString()))
                    this.PF_VlrMontoFirma1.Value = Convert.ToDecimal(dr["PF_VlrMontoFirma1"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_VlrMontoFirma2"].ToString()))
                    this.PF_VlrMontoFirma2.Value = Convert.ToDecimal(dr["PF_VlrMontoFirma2"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_VlrMontoFirma3"].ToString()))
                    this.PF_VlrMontoFirma3.Value = Convert.ToDecimal(dr["PF_VlrMontoFirma3"]);

                if (!string.IsNullOrWhiteSpace(dr["PF_TasaPerfilOBL"].ToString()))
                    this.PF_TasaPerfilOBL.Value = Convert.ToDecimal(dr["PF_TasaPerfilOBL"]);
                //if (!string.IsNullOrWhiteSpace(dr["PF_TasaFirma1OBL"].ToString()))
                //    this.PF_TasaFirma1OBL.Value = Convert.ToDecimal(dr["PF_TasaFirma1OBL"]);
                //if (!string.IsNullOrWhiteSpace(dr["PF_TasaFirma2OBL"].ToString()))
                //    this.PF_TasaFirma2OBL.Value = Convert.ToDecimal(dr["PF_TasaFirma2OBL"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_TasaFirma3OBL"].ToString()))
                    this.PF_TasaFirma3OBL.Value = Convert.ToDecimal(dr["PF_TasaFirma3OBL"]);

                if (!string.IsNullOrWhiteSpace(dr["PF_PorMaxFirma1"].ToString()))
                    this.PF_PorMaxFirma1.Value = Convert.ToDecimal(dr["PF_PorMaxFirma1"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_PorMaxFirma2"].ToString()))
                    this.PF_PorMaxFirma2.Value = Convert.ToDecimal(dr["PF_PorMaxFirma2"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_PorMaxFirma3"].ToString()))
                    this.PF_PorMaxFirma3.Value = Convert.ToDecimal(dr["PF_PorMaxFirma3"]);

                if (!string.IsNullOrWhiteSpace(dr["PF_VlrGarantiaPerfil"].ToString()))
                    this.PF_VlrGarantiaPerfil.Value = Convert.ToDecimal(dr["PF_VlrGarantiaPerfil"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_VlrGarantiaFirma1"].ToString()))
                    this.PF_VlrGarantiaFirma1.Value = Convert.ToDecimal(dr["PF_VlrGarantiaFirma1"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_VlrGarantiaFirma2"].ToString()))
                    this.PF_VlrGarantiaFirma2.Value = Convert.ToDecimal(dr["PF_VlrGarantiaFirma2"]);
                if (!string.IsNullOrWhiteSpace(dr["PF_VlrGarantiaFirma3"].ToString()))
                    this.PF_VlrGarantiaFirma3.Value = Convert.ToDecimal(dr["PF_VlrGarantiaFirma3"]);


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_drSolicitudDatosOtros()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {   
            this.NumeroDoc=new UDT_Consecutivo();
            this.Version=new UDTSQL_int();
            this.VlrPoliza=new UDT_Valor();
            this.CubriendoInd=new UDT_SiNo();
            this.AseguradoraID=new UDT_CodigoGrl10();
            this.FinanciaPOLInd=new UDT_SiNo();
            this.VlrMensualSV=new UDT_Valor();
            this.TipoPrenda=new UDTSQL_tinyint();
            this.PrefijoPrenda=new UDTSQL_varchar(5);
            this.NumeroPrenda=new UDTSQL_int();
            this.Registro=new UDTSQL_tinyint();
            this.CedulaReg=new UDT_TerceroID(); 
            this.NombreREG=new UDTSQL_varchar(50);
            this.DireccionREG=new UDTSQL_varchar(100);
            this.Alternativa1=new UDTSQL_varchar(15);
            this.Alternativa2=new UDTSQL_varchar(15);
            this.Alternativa3=new UDTSQL_varchar(15);
            this.Alternativa4=new UDTSQL_varchar(15);
            this.Alternativa5=new UDTSQL_varchar(15);
            this.Alternativa6=new UDTSQL_varchar(15);
            this.Alternativa7=new UDTSQL_varchar(15);
            this.FactorAlt1 = new UDT_PorcentajeID();
            this.FactorAlt2 = new UDT_PorcentajeID();
            this.FactorAlt3 = new UDT_PorcentajeID();
            this.FactorAlt4 = new UDT_PorcentajeID();
            this.FactorAlt5 = new UDT_PorcentajeID();
            this.FactorAlt6 = new UDT_PorcentajeID();
            this.FactorAlt7=new UDT_PorcentajeID();
            this.MontoAlt1=new UDT_Valor();
            this.MontoAlt2=new UDT_Valor();
            this.MontoAlt3=new UDT_Valor();
            this.MontoAlt4=new UDT_Valor();
            this.MontoAlt5=new UDT_Valor();
            this.MontoAlt6=new UDT_Valor();
            this.MontoAlt7=new UDT_Valor();
            this.GarantiaAlt1=new UDT_Valor();
            this.GarantiaAlt2=new UDT_Valor();
            this.GarantiaAlt3=new UDT_Valor();
            this.GarantiaAlt4=new UDT_Valor();
            this.GarantiaAlt5=new UDT_Valor();
            this.GarantiaAlt6=new UDT_Valor();
            this.GarantiaAlt7=new UDT_Valor();
            this.VlrGtiaEvaluacion=new UDT_Valor();
            this.MontoMaximo=new UDT_Valor();
            this.EstimadoSeguros=new UDTSQL_decimal();
            this.MaxFinanciacionAut=new UDTSQL_decimal();
            this.Plazo=new UDTSQL_tinyint();
            this.EstimadoObl=new UDTSQL_decimal();
            this.CuotaFin=new UDT_Valor();
            this.CuotaSeg=new UDT_Valor();
            this.CuotaTotal=new UDT_Valor();
            this.Estado=new UDTSQL_varchar(50);
            this.Calificacion=new UDTSQL_varchar(50);
            this.Revision=new UDTSQL_varchar(50);
            this.CargoResp=new UDTSQL_varchar(30);
            this.Firma1Ind=new UDT_SiNo();
            this.Firma2Ind = new UDT_SiNo();
            this.Firma3Ind = new UDT_SiNo();
            this.UsuarioResp=new UDT_UsuarioID();
            this.UsuarioFirma1=new UDT_UsuarioID();
            this.UsuarioFirma2=new UDT_UsuarioID();
            this.UsuarioFirma3 = new UDT_UsuarioID();
            this.FechaFirmaResp=new UDTSQL_smalldatetime();
            this.FechaFirma1=new UDTSQL_smalldatetime();
            this.FechaFirma2=new UDTSQL_smalldatetime();
            this.FechaFirma3 = new UDTSQL_smalldatetime();
            this.FechaDatacredito = new UDTSQL_smalldatetime();
            this.FechaLegalizacion = new UDTSQL_smalldatetime();
            this.FechaDesembolso = new UDTSQL_smalldatetime();

            this.Consecutivo=new UDT_Consecutivo();

            this.CartaAprobDirInd=new UDT_SiNo();
            this.CartaAprobDocInd=new UDT_SiNo();
            this.CartapreAprobInd=new UDT_SiNo();
            this.CartaNoViableInd=new UDT_SiNo();
            this.CartaRevocaInd=new UDT_SiNo();
            this.CartaRatificaInd=new UDT_SiNo();
            this.CartaAprobDirUsu=new UDT_UsuarioID();
            this.CartaAprobDocUsu=new UDT_UsuarioID();
            this.CartapreAprobUsu=new UDT_UsuarioID();
            this.CartaNoViableUsu=new UDT_UsuarioID();
            this.CartaRevocaUsu=new UDT_UsuarioID();
            this.CartaRatificaUsu=new UDT_UsuarioID();
            this.CartaAprobDirFecha=new UDTSQL_smalldatetime();
            this.CartaAprobDocFecha=new UDTSQL_smalldatetime();
            this.CartapreAprobFecha=new UDTSQL_smalldatetime();
            this.CartaNoViableFecha=new UDTSQL_smalldatetime();
            this.CartaRevocaFecha=new UDTSQL_smalldatetime();
            this.CartaRatificaFecha=new UDTSQL_smalldatetime();
            this.PerfilUsuario = new UDT_UsuarioID();
            this.PerfilFecha = new UDTSQL_smalldatetime();
            this.EstActualFactor = new UDT_PorcentajeID();

            this.VlrSolicitado = new UDT_Valor();

            this.SMMLV = new UDT_Valor();
            this.PF_PorIngrPagoCtas = new UDT_PorcentajeID();
            this.PF_IngrDispApoyosDEU = new UDT_Valor();
            this.PF_IngrDispApoyosCON = new UDT_Valor();
            this.PF_IngrDispApoyos = new UDT_Valor();
            this.PF_VlrMontoSOL = new UDT_Valor();
            this.PF_VlrMontoAJU = new UDT_Valor();
            this.PF_VlrMontoINC = new UDT_Valor();
            this.PF_VlrMontoFIN = new UDT_Valor();
            this.PF_CtaFinanciaSOL = new UDT_Valor();
            this.PF_CtaFinanciaAJU = new UDT_Valor();
            this.PF_CtaFinanciaINC = new UDT_Valor();
            this.PF_CtaFinanciaFIN = new UDT_Valor();
            this.PF_CtaSeguroSOL = new UDT_Valor();
            this.PF_CtaSeguroAJU = new UDT_Valor();
            this.PF_CtaSeguroINC = new UDT_Valor();
            this.PF_CtaSeguroFIN = new UDT_Valor();
            this.PF_CtaTotalSOL = new UDT_Valor();
            this.PF_CtaTotalAJU = new UDT_Valor();
            this.PF_CtaTotalINC = new UDT_Valor();
            this.PF_CtaTotalFIN = new UDT_Valor();
            this.PF_CtaApoyosDifSOL = new UDT_Valor();
            this.PF_CtaApoyosDifAJU = new UDT_Valor();
            this.PF_CtaApoyosDifINC = new UDT_Valor();
            this.PF_CtaApoyosDifFIN = new UDT_Valor();
            this.PF_IngDispApoyosSOL = new UDT_Valor();
            this.PF_IngDispApoyosAJU = new UDT_Valor();
            this.PF_IngDispApoyosINC = new UDT_Valor();
            this.PF_IngDispApoyosFIN = new UDT_Valor();
            this.PF_ReqSopIngrIndDEU = new UDT_SiNo();
            this.PF_ReqSopIngrIndCON = new UDT_SiNo();
            this.PF_IngReqDeu1SOL = new UDT_Valor();
            this.PF_IngReqDeu1AJU = new UDT_Valor();
            this.PF_IngReqDeu1INC = new UDT_Valor();
            this.PF_IngReqDeu1FIN = new UDT_Valor();
            this.PF_IngReqDeu2SOL = new UDT_Valor();
            this.PF_IngReqDeu2AJU = new UDT_Valor();
            this.PF_IngReqDeu2INC = new UDT_Valor();
            this.PF_IngReqDeu2FIN = new UDT_Valor();
            this.PF_IngReqDeu3SOL = new UDT_Valor();
            this.PF_IngReqDeuAJU = new UDT_Valor();
            this.PF_IngReqDeu3INC = new UDT_Valor();
            this.PF_IngReqDeu3FIN = new UDT_Valor();
            this.PF_IngReqDeuFinSOL = new UDT_Valor();
            this.PF_IngReqDeuFinAJU = new UDT_Valor();
            this.PF_IngReqDeuFinINC = new UDT_Valor();
            this.PF_IngReqDeuFinFIN = new UDT_Valor();
            this.PF_IngReqCon1SOL = new UDT_Valor();
            this.PF_IngReqCon1AJU = new UDT_Valor();
            this.PF_IngReqCon1INC = new UDT_Valor();
            this.PF_IngReqCon1FIN = new UDT_Valor();
            this.PF_IngReqCon2SOL = new UDT_Valor();
            this.PF_IngReqCon2AJU = new UDT_Valor();
            this.PF_IngReqCon2INC = new UDT_Valor();
            this.PF_IngReqCon2FIN = new UDT_Valor();
            this.PF_IngReqConFinSOL = new UDT_Valor();
            this.PF_IngReqConFinAJU = new UDT_Valor();
            this.PF_IngReqConFinINC = new UDT_Valor();
            this.PF_IngReqConFinFIN = new UDT_Valor();
            this.PF_Cuantia = new UDT_Valor();
            this.PF_TasaTablEva1 = new UDT_PorcentajeID();
            this.PF_FactTablEva = new UDT_PorcentajeID();
            this.PF_TasaTablEva2 = new UDT_PorcentajeID();
            this.PF_TasaPonderada = new UDT_PorcentajeID();
            
////
            this.PF_PolCubriendoInd = new UDT_SiNo();
            
            
            this.PF_PlazoFinal = new UDTSQL_tinyint();
            this.PF_VlrMinimoGar = new UDT_Valor();
            this.PF_VlrMinimoFirma2 = new UDT_Valor();
            this.PF_VlrMinimoFirma3 = new UDT_Valor();

            this.PF_CtaFinanciacion = new UDT_Valor();
            this.PF_CtaSeguros = new UDT_Valor();
            this.PF_PorEstimado = new UDT_PorcentajeID();
            this.Verificacion1 = new UDTSQL_varchar(50);
            this.Verificacion2 = new UDTSQL_varchar(50);
            this.Verificacion3 = new UDTSQL_varchar(50);
            this.AN_Resultado1SOL = new UDT_Valor();
            this.AN_Resultado1CA1 = new UDT_Valor();
            this.AN_Resultado1CA2 = new UDT_Valor();
            this.AN_Resultado1CA3 = new UDT_Valor();
            this.AN_Resultado1AJ1 = new UDT_Valor();
            this.AN_Resultado1AJ2 = new UDT_Valor();
            this.AN_Resultado1V11 = new UDT_Valor();
            this.AN_Resultado1V12 = new UDT_Valor();
            this.AN_Resultado1V21 = new UDT_Valor();
            this.AN_Resultado1V22 = new UDT_Valor();
            this.AN_Resultado1V31 = new UDT_Valor();
            this.AN_Resultado1V32 = new UDT_Valor();
            this.AN_Resultado2SOL = new UDT_Valor();
            this.AN_Resultado2CA1 = new UDT_Valor();
            this.AN_Resultado2CA2 = new UDT_Valor();
            this.AN_Resultado2CA3 = new UDT_Valor();
            this.AN_Resultado2AJ1 = new UDT_Valor();
            this.AN_Resultado2AJ2 = new UDT_Valor();
            this.AN_Resultado2V11 = new UDT_Valor();
            this.AN_Resultado2V12 = new UDT_Valor();
            this.AN_Resultado2V21 = new UDT_Valor();
            this.AN_Resultado2V22 = new UDT_Valor();
            this.AN_Resultado2V31 = new UDT_Valor();
            this.AN_Resultado2V32 = new UDT_Valor();
            this.AN_VlrVenta = new UDT_Valor();
            this.AN_VlrFasecolda = new UDT_Valor();
            this.AN_VlrCtaEvaSOL = new UDT_Valor();
            this.AN_VlrCtaEvaCA1 = new UDT_Valor();
            this.AN_VlrCtaEvaCA2 = new UDT_Valor();
            this.AN_VlrCtaEvaCA3 = new UDT_Valor();
            this.AN_VlrCtaEvaAJ1 = new UDT_Valor();
            this.AN_VlrCtaEvaAJ2 = new UDT_Valor();
            this.AN_VlrCtaEvaV11 = new UDT_Valor();
            this.AN_VlrCtaEvaV12 = new UDT_Valor();
            this.AN_VlrCtaEvaV21 = new UDT_Valor();
            this.AN_VlrCtaEvaV22 = new UDT_Valor();
            this.AN_VlrCtaEvaV31 = new UDT_Valor();
            this.AN_VlrCtaEvaV32 = new UDT_Valor();
            this.AN_VlrMinLim = new UDT_Valor();
            this.AN_VlrMaxLim = new UDT_Valor();
            this.AN_VlrSolicitado = new UDT_Valor();
            this.AN_VlrAlternCA1 = new UDT_Valor();
            this.AN_VlrAlternCA2 = new UDT_Valor();
            this.AN_VlrAlternCA3 = new UDT_Valor();
            this.AN_VlrAlternAJ1 = new UDT_Valor();
            this.AN_VlrAlternAJ2 = new UDT_Valor();
            this.AN_VlrAlternV11 = new UDT_Valor();
            this.AN_VlrAlternV12 = new UDT_Valor();
            this.AN_VlrAlternV21 = new UDT_Valor();
            this.AN_VlrAlternV22 = new UDT_Valor();
            this.AN_VlrAlternV31 = new UDT_Valor();
            this.AN_VlrAlternV32 = new UDT_Valor();
            this.AN_CtaInicial = new UDT_Valor();
            this.AN_VlrIncremSOL = new UDT_Valor();
            this.AN_VlrIncremCA1 = new UDT_Valor();
            this.AN_VlrIncremCA2 = new UDT_Valor();
            this.AN_VlrIncremCA3 = new UDT_Valor();
            this.AN_VlrIncremAJ1 = new UDT_Valor();
            this.AN_VlrIncremAJ2 = new UDT_Valor();
            this.AN_VlrIncremV11 = new UDT_Valor();
            this.AN_VlrIncremV12 = new UDT_Valor();
            this.AN_VlrIncremV21 = new UDT_Valor();
            this.AN_VlrIncremV22 = new UDT_Valor();
            this.AN_VlrIncremV31 = new UDT_Valor();
            this.AN_VlrIncremV32 = new UDT_Valor();
            this.AN_CtaIniAjuCA1 = new UDT_Valor();
            this.AN_CtaIniAjuCA2 = new UDT_Valor();
            this.AN_CtaIniAjuCA3 = new UDT_Valor();
            this.AN_CtaIniAjuAJ1 = new UDT_Valor();
            this.AN_CtaIniAjuAJ2 = new UDT_Valor();
            this.AN_CtaIniAjuV11 = new UDT_Valor();
            this.AN_CtaIniAjuV12 = new UDT_Valor();
            this.AN_CtaIniAjuV21 = new UDT_Valor();
            this.AN_CtaIniAjuV22 = new UDT_Valor();
            this.AN_CtaIniAjuV31 = new UDT_Valor();
            this.AN_CtaIniAjuV32 = new UDT_Valor();
            this.AN_porFinSoli = new UDT_PorcentajeID();
            this.AN_porFinMaxSOL = new UDT_PorcentajeID();
            this.AN_porFinMaxCA1 = new UDT_PorcentajeID();
            this.AN_porFinMaxCA2 = new UDT_PorcentajeID();
            this.AN_porFinMaxCA3 = new UDT_PorcentajeID();
            this.AN_porFinMaxAJ1 = new UDT_PorcentajeID();
            this.AN_porFinMaxAJ2 = new UDT_PorcentajeID();
            this.AN_porFinMaxV11 = new UDT_PorcentajeID();
            this.AN_porFinMaxV12 = new UDT_PorcentajeID();
            this.AN_porFinMaxV21 = new UDT_PorcentajeID();
            this.AN_porFinMaxV22 = new UDT_PorcentajeID();
            this.AN_porFinMaxV31 = new UDT_PorcentajeID();
            this.AN_porFinMaxV32 = new UDT_PorcentajeID();
            this.AN_porFinAltCA1 = new UDT_PorcentajeID();
            this.AN_porFinAltCA2 = new UDT_PorcentajeID();
            this.AN_porFinAltCA3 = new UDT_PorcentajeID();
            this.AN_porFinAltAJ1 = new UDT_PorcentajeID();
            this.AN_porFinAltAJ2 = new UDT_PorcentajeID();
            this.AN_porFinAltV11 = new UDT_PorcentajeID();
            this.AN_porFinAltV12 = new UDT_PorcentajeID();
            this.AN_porFinAltV21 = new UDT_PorcentajeID();
            this.AN_porFinAltV22 = new UDT_PorcentajeID();
            this.AN_porFinAltV31 = new UDT_PorcentajeID();
            this.AN_porFinAltV32 = new UDT_PorcentajeID();
            this.AN_CumParGarAJ1 = new UDT_SiNo();
            this.AN_CumParGarAJ2 = new UDT_SiNo();
            this.AN_CumParGarV11 = new UDT_SiNo();
            this.AN_CumParGarV12 = new UDT_SiNo();
            this.AN_CumParGarV21 = new UDT_SiNo();
            this.AN_CumParGarV22 = new UDT_SiNo();
            this.AN_CumParGarV31 = new UDT_SiNo();
            this.AN_CumParGarV32 = new UDT_SiNo();
            this.AN_CapPagoSOL = new UDTSQL_varchar(30);
            this.AN_CapPagoCA1 = new UDTSQL_varchar(30);
            this.AN_CapPagoCA2 = new UDTSQL_varchar(30);
            this.AN_CapPagoCA3 = new UDTSQL_varchar(30);
            this.AN_CapPagoAJ1 = new UDTSQL_varchar(30);
            this.AN_CapPagoAJ2 = new UDTSQL_varchar(30);
            this.AN_CapPagoV11 = new UDTSQL_varchar(30);
            this.AN_CapPagoV12 = new UDTSQL_varchar(30);
            this.AN_CapPagoV21 = new UDTSQL_varchar(30);
            this.AN_CapPagoV22 = new UDTSQL_varchar(30);
            this.AN_CapPagoV31 = new UDTSQL_varchar(30);
            this.AN_CapPagoV32 = new UDTSQL_varchar(30);
            this.AN_PuedeIncCICA1 = new UDT_SiNo();
            this.AN_PuedeIncCICA2 = new UDT_SiNo();
            this.AN_PuedeIncCICA3 = new UDT_SiNo();
            this.AN_PuedeIncCIAJ1 = new UDT_SiNo();
            this.AN_PuedeIncCIAJ2 = new UDT_SiNo();
            this.AN_PuedeIncCIV11 = new UDT_SiNo();
            this.AN_PuedeIncCIV12 = new UDT_SiNo();
            this.AN_PuedeIncCIV21 = new UDT_SiNo();
            this.AN_PuedeIncCIV22 = new UDT_SiNo();
            this.AN_PuedeIncCIV31 = new UDT_SiNo();
            this.AN_PuedeIncCIV32 = new UDT_SiNo();
            this.AN_CumPorMaxSOL = new UDT_SiNo();
            this.AN_CumPorMaxCA1 = new UDT_SiNo();
            this.AN_CumPorMaxCA2 = new UDT_SiNo();
            this.AN_CumPorMaxCA3 = new UDT_SiNo();
            this.AN_CumPorMaxAJ1 = new UDT_SiNo();
            this.AN_CumPorMaxAJ2 = new UDT_SiNo();
            this.AN_CumPorMaxV11 = new UDT_SiNo();
            this.AN_CumPorMaxV12 = new UDT_SiNo();
            this.AN_CumPorMaxV21 = new UDT_SiNo();
            this.AN_CumPorMaxV22 = new UDT_SiNo();
            this.AN_CumPorMaxV31 = new UDT_SiNo();
            this.AN_CumPorMaxV32 = new UDT_SiNo();
            this.AN_CumMtoMinCA1 = new UDT_SiNo();
            this.AN_CumMtoMinCA2 = new UDT_SiNo();
            this.AN_CumMtoMinCA3 = new UDT_SiNo();
            this.AN_CumMtoMinAJ1 = new UDT_SiNo();
            this.AN_CumMtoMinAJ2 = new UDT_SiNo();
            this.AN_CumMtoMinV11 = new UDT_SiNo();
            this.AN_CumMtoMinV12 = new UDT_SiNo();
            this.AN_CumMtoMinV21 = new UDT_SiNo();
            this.AN_CumMtoMinV22 = new UDT_SiNo();
            this.AN_CumMtoMinV31 = new UDT_SiNo();
            this.AN_CumMtoMinV32 = new UDT_SiNo();
            this.AN_CumOtroSOL = new UDTSQL_varchar(2);
            this.AN_CumOtroCA1 = new UDTSQL_varchar(2);
            this.AN_CumOtroCA2 = new UDTSQL_varchar(2);
            this.AN_CumOtroCA3 = new UDTSQL_varchar(2);
            this.AN_CumOtroAJ1 = new UDTSQL_varchar(2);
            this.AN_CumOtroAJ2 = new UDTSQL_varchar(2);
            this.AN_CumOtroV11 = new UDTSQL_varchar(2);
            this.AN_CumOtroV12 = new UDTSQL_varchar(2);
            this.AN_CumOtroV21 = new UDTSQL_varchar(2);
            this.AN_CumOtroV22 = new UDTSQL_varchar(2);
            this.AN_CumOtroV31 = new UDTSQL_varchar(2);
            this.AN_CumOtroV32 = new UDTSQL_varchar(2);
            this.AN_VlrAutorizaSOL = new UDT_Valor();
            this.AN_VlrAutorizaCA1 = new UDT_Valor();
            this.AN_VlrAutorizaCA2 = new UDT_Valor();
            this.AN_VlrAutorizaCA3 = new UDT_Valor();
            this.AN_VlrAutorizaAJ1 = new UDT_Valor();
            this.AN_VlrAutorizaAJ2 = new UDT_Valor();
            this.AN_VlrAutorizaV11 = new UDT_Valor();
            this.AN_VlrAutorizaV12 = new UDT_Valor();
            this.AN_VlrAutorizaV21 = new UDT_Valor();
            this.AN_VlrAutorizaV22 = new UDT_Valor();
            this.AN_VlrAutorizaV31 = new UDT_Valor();
            this.AN_VlrAutorizaV32 = new UDT_Valor();
            this.AN_VlrAutFinSOL = new UDT_Valor();
            this.AN_VlrAutFinCA2 = new UDT_Valor();
            this.AN_VlrAutFinAJ1 = new UDT_Valor();
            this.AN_VlrAutFinV12 = new UDT_Valor();
            this.AN_VlrAutFinV21 = new UDT_Valor();
            this.AN_VlrAutFinV31 = new UDT_Valor();
            this.AN_AltNoComAJ1 = new UDTSQL_varchar(2);
            this.AN_AltNoComAJ2 = new UDTSQL_varchar(2);
            this.AN_AltNoComV12 = new UDTSQL_varchar(2);
            this.AN_AltNoComV21 = new UDTSQL_varchar(2);
            this.AN_AltNoComV22 = new UDTSQL_varchar(2);
            this.AN_AltNoComV31 = new UDTSQL_varchar(2);
            this.AN_AltNoComV32 = new UDTSQL_varchar(2);

            this.Alternativa8 = new UDTSQL_varchar(15);
            this.FactorAlt8 = new UDT_PorcentajeID();
            this.MontoAlt8 = new UDT_Valor();
            this.GarantiaAlt8 = new UDT_Valor();
            this.IngrMinSopDeu1 = new UDT_Valor();
            this.IngrMinSopDeu2 = new UDT_Valor();
            this.IngrMinSopDeu3 = new UDT_Valor();
            this.IngrMinSopDeu4 = new UDT_Valor();
            this.IngrMinSopDeu5 = new UDT_Valor();
            this.IngrMinSopDeu6 = new UDT_Valor();
            this.IngrMinSopDeu7 = new UDT_Valor();
            this.IngrMinSopDeu8 = new UDT_Valor();
            this.IngrMinSopCon1 = new UDT_Valor();
            this.IngrMinSopCon2 = new UDT_Valor();
            this.IngrMinSopCon3 = new UDT_Valor();
            this.IngrMinSopCon4 = new UDT_Valor();
            this.IngrMinSopCon5 = new UDT_Valor();
            this.IngrMinSopCon6 = new UDT_Valor();
            this.IngrMinSopCon7 = new UDT_Valor();
            this.IngrMinSopCon8 = new UDT_Valor();

            this.FechaFirmaDocumento = new UDTSQL_smalldatetime();
            this.AccionSolicitud = new UDTSQL_tinyint();
            this.Observacion = new UDT_DescripTExt();
            this.PF_Plazo = new UDTSQL_tinyint();
            this.PF_PorMaxEstadoActual = new UDT_PorcentajeID();
            this.porMaximo = new UDT_PorcentajeID();
            this.VlrGarantia = new UDT_Valor();
            this.AccionSolicitud1 = new UDTSQL_tinyint();
            this.AccionSolicitud2 = new UDTSQL_tinyint();
            this.AccionSolicitud3 = new UDTSQL_tinyint();

            this.PF_PlazoFinal1 = new UDTSQL_tinyint();
            this.PF_PlazoFinal2 = new UDTSQL_tinyint();
            this.PF_PlazoFinal3 = new UDTSQL_tinyint();

            this.PF_VlrMontoFirma1 = new UDT_Valor();
            this.PF_VlrMontoFirma2 = new UDT_Valor();
            this.PF_VlrMontoFirma3 = new UDT_Valor();
            
            this.PF_TasaPerfilOBL = new UDT_PorcentajeID();
            //this.PF_TasaFirma1OBL = new UDT_PorcentajeID();
            //this.PF_TasaFirma2OBL = new UDT_PorcentajeID();
            this.PF_TasaFirma3OBL = new UDT_PorcentajeID();

            this.PF_PorMaxFirma1 = new UDT_PorcentajeID();
            this.PF_PorMaxFirma2 = new UDT_PorcentajeID();
            this.PF_PorMaxFirma3 = new UDT_PorcentajeID();

            this.PF_VlrGarantiaPerfil = new UDT_Valor();
            this.PF_VlrGarantiaFirma1 = new UDT_Valor();
            this.PF_VlrGarantiaFirma2 = new UDT_Valor();
            this.PF_VlrGarantiaFirma3 = new UDT_Valor();

        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }
        
        [DataMember]
        public UDTSQL_int Version { get; set; }

        [DataMember]
        public UDT_Valor VlrPoliza	{ get; set; }
        [DataMember]
        public UDT_SiNo CubriendoInd	{ get; set; }
        [DataMember]
        public UDT_CodigoGrl10 AseguradoraID	{ get; set; }
        [DataMember]
        public UDT_SiNo FinanciaPOLInd	{ get; set; }
        [DataMember]
        public UDT_Valor VlrMensualSV	{ get; set; }
        [DataMember]
        public UDTSQL_tinyint TipoPrenda	{ get; set; }
        [DataMember]
        public UDTSQL_varchar PrefijoPrenda	{ get; set; }
        [DataMember]
        public UDTSQL_int NumeroPrenda	{ get; set; }
        [DataMember]
        public UDTSQL_tinyint Registro	{ get; set; }
        [DataMember]
        public UDT_TerceroID CedulaReg	{ get; set; }
        [DataMember]
        public UDTSQL_varchar NombreREG	{ get; set; }
        [DataMember]
        public UDTSQL_varchar DireccionREG	{ get; set; }
        [DataMember]
        public UDTSQL_varchar Alternativa1	{ get; set; }
        [DataMember]
        public UDTSQL_varchar Alternativa2	{ get; set; }
        [DataMember]
        public UDTSQL_varchar Alternativa3	{ get; set; }
        [DataMember]
        public UDTSQL_varchar Alternativa4	{ get; set; }
        [DataMember]
        public UDTSQL_varchar Alternativa5	{ get; set; }
        [DataMember]
        public UDTSQL_varchar Alternativa6	{ get; set; }
        [DataMember]
        public UDTSQL_varchar Alternativa7	{ get; set; }
        [DataMember]
        public UDT_PorcentajeID FactorAlt1 { get; set; }
        [DataMember]
        public UDT_PorcentajeID FactorAlt2 { get; set; }
        [DataMember]
        public UDT_PorcentajeID FactorAlt3	{ get; set; }
        [DataMember]
        public UDT_PorcentajeID FactorAlt4 { get; set; }
        [DataMember]
        public UDT_PorcentajeID FactorAlt5 { get; set; }
        [DataMember]
        public UDT_PorcentajeID FactorAlt6 { get; set; }
        [DataMember]
        public UDT_PorcentajeID FactorAlt7 { get; set; }
        [DataMember]
        public UDT_Valor MontoAlt1	{ get; set; }
        [DataMember]
        public UDT_Valor MontoAlt2	{ get; set; }
        [DataMember]
        public UDT_Valor MontoAlt3	{ get; set; }
        [DataMember]
        public UDT_Valor MontoAlt4	{ get; set; }
        [DataMember]
        public UDT_Valor MontoAlt5	{ get; set; }
        [DataMember]
        public UDT_Valor MontoAlt6	{ get; set; }
        [DataMember]
        public UDT_Valor MontoAlt7	{ get; set; }
        [DataMember]
        public UDT_Valor GarantiaAlt1	{ get; set; }
        [DataMember]
        public UDT_Valor GarantiaAlt2	{ get; set; }
        [DataMember]
        public UDT_Valor GarantiaAlt3	{ get; set; }
        [DataMember]
        public UDT_Valor GarantiaAlt4	{ get; set; }
        [DataMember]
        public UDT_Valor GarantiaAlt5	{ get; set; }
        [DataMember]
        public UDT_Valor GarantiaAlt6	{ get; set; }
        [DataMember]
        public UDT_Valor GarantiaAlt7	{ get; set; }
        [DataMember]
        public UDT_Valor VlrGtiaEvaluacion	{ get; set; }
        [DataMember]
        public UDT_Valor MontoMaximo	{ get; set; }
        [DataMember]
        public UDTSQL_decimal EstimadoSeguros	{ get; set; }
        [DataMember]
        public UDTSQL_decimal MaxFinanciacionAut	{ get; set; }
        [DataMember]
        public UDTSQL_tinyint Plazo	{ get; set; }
        [DataMember]
        public UDTSQL_decimal EstimadoObl	{ get; set; }
        [DataMember]
        public UDT_Valor CuotaFin	{ get; set; }
        [DataMember]
        public UDT_Valor CuotaSeg	{ get; set; }
        [DataMember]
        public UDT_Valor CuotaTotal	{ get; set; }
        [DataMember]
        public UDTSQL_varchar Estado	{ get; set; }
        [DataMember]
        public UDTSQL_varchar Calificacion	{ get; set; }
        [DataMember]
        public UDTSQL_varchar Revision	{ get; set; }
        [DataMember]
        public UDTSQL_varchar CargoResp	{ get; set; }
        [DataMember]
        public UDT_SiNo Firma1Ind	{ get; set; }
        [DataMember]
        public UDT_SiNo Firma2Ind	{ get; set; }
        [DataMember]
        public UDT_SiNo Firma3Ind { get; set; }

        [DataMember]
        public UDT_UsuarioID UsuarioResp	{ get; set; }
        [DataMember]
        public UDT_UsuarioID UsuarioFirma1	{ get; set; }
        [DataMember]
        public UDT_UsuarioID UsuarioFirma2	{ get; set; }
        [DataMember]
        public UDT_UsuarioID UsuarioFirma3 { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaFirmaResp	{ get; set; }
        [DataMember]
        public UDTSQL_smalldatetime FechaFirma1	{ get; set; }
        [DataMember]
        public UDTSQL_smalldatetime FechaFirma2	{ get; set; }
        [DataMember]
        public UDTSQL_smalldatetime FechaFirma3 { get; set; }
        [DataMember]
        public UDTSQL_smalldatetime FechaDatacredito { get; set; }       
        [DataMember]
        public UDTSQL_smalldatetime FechaLegalizacion { get; set; }
        [DataMember]
        public UDTSQL_smalldatetime FechaDesembolso { get; set; }
        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }


    
        [DataMember]
        public UDT_SiNo CartaAprobDirInd { get; set; }
        [DataMember]
        public UDT_SiNo CartaAprobDocInd { get; set; }
        [DataMember]
        public UDT_SiNo CartapreAprobInd { get; set; }
        [DataMember]
        public UDT_SiNo CartaNoViableInd { get; set; }
        [DataMember]
        public UDT_SiNo CartaRevocaInd { get; set; }
        [DataMember]
        public UDT_SiNo CartaRatificaInd { get; set; } 

        [DataMember]
        public UDT_UsuarioID CartaAprobDirUsu { get; set; }
        [DataMember]
        public UDT_UsuarioID CartaAprobDocUsu { get; set; }
        [DataMember]
        public UDT_UsuarioID CartapreAprobUsu { get; set; }  
        [DataMember]
        public UDT_UsuarioID CartaNoViableUsu { get; set; }  
        [DataMember]
        public UDT_UsuarioID CartaRevocaUsu { get; set; }  
        [DataMember]
        public UDT_UsuarioID CartaRatificaUsu { get; set; }
        [DataMember]
        public UDTSQL_smalldatetime CartaAprobDirFecha { get; set; }
        [DataMember]
        public UDTSQL_smalldatetime CartaAprobDocFecha { get; set; }
        [DataMember]
        public UDTSQL_smalldatetime CartapreAprobFecha { get; set; }  
        [DataMember]
        public UDTSQL_smalldatetime CartaNoViableFecha { get; set; }  
        [DataMember]
        public UDTSQL_smalldatetime CartaRevocaFecha { get; set; } 
        [DataMember]
        public UDTSQL_smalldatetime CartaRatificaFecha { get; set; }
        [DataMember]
        public UDT_UsuarioID PerfilUsuario { get; set; }
        [DataMember]
        public UDTSQL_smalldatetime PerfilFecha { get; set; }
        [DataMember]
        public UDT_PorcentajeID EstActualFactor { get; set; }

        [DataMember]
        public UDT_Valor VlrSolicitado { get; set; }
        
        [DataMember]
        public  UDT_Valor SMMLV{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_PorIngrPagoCtas{get;set;}
        [DataMember]
        public  UDT_Valor PF_IngrDispApoyosDEU{get;set;}
        [DataMember]
        public  UDT_Valor PF_IngrDispApoyosCON{get;set;}
        [DataMember]
        public  UDT_Valor PF_IngrDispApoyos{get;set;}
        [DataMember]
        public  UDT_Valor PF_VlrMontoSOL{get;set;}
        [DataMember]
        public  UDT_Valor PF_VlrMontoAJU{get;set;}
        [DataMember]
        public  UDT_Valor PF_VlrMontoINC{get;set;}
        [DataMember]
        public  UDT_Valor PF_VlrMontoFIN{get;set;}
        [DataMember]
        public  UDT_Valor PF_CtaFinanciaSOL{get;set;}
        [DataMember]
        public  UDT_Valor PF_CtaFinanciaAJU{get;set;}
        [DataMember]
        public  UDT_Valor PF_CtaFinanciaINC{get;set;}
        [DataMember]
        public  UDT_Valor PF_CtaFinanciaFIN{get;set;}
        [DataMember]
        public  UDT_Valor PF_CtaSeguroSOL{get;set;}
        [DataMember]
        public  UDT_Valor PF_CtaSeguroAJU{get;set;}
        [DataMember]
        public  UDT_Valor PF_CtaSeguroINC{get;set;}
        [DataMember]
        public  UDT_Valor PF_CtaSeguroFIN{get;set;}
        [DataMember]
        public  UDT_Valor PF_CtaTotalSOL{get;set;}
        [DataMember]
        public  UDT_Valor PF_CtaTotalAJU{get;set;}
        [DataMember]
        public  UDT_Valor PF_CtaTotalINC{get;set;}
        [DataMember]
        public  UDT_Valor PF_CtaTotalFIN{get;set;}
        [DataMember]
        public  UDT_Valor PF_CtaApoyosDifSOL{get;set;}
        [DataMember]
        public  UDT_Valor PF_CtaApoyosDifAJU{get;set;}
        [DataMember]
        public  UDT_Valor PF_CtaApoyosDifINC{get;set;}
        [DataMember]
        public  UDT_Valor PF_CtaApoyosDifFIN{get;set;}
        [DataMember]
        public  UDT_Valor PF_IngDispApoyosSOL{get;set;}
        [DataMember]
        public  UDT_Valor PF_IngDispApoyosAJU{get;set;}
        [DataMember]
        public  UDT_Valor PF_IngDispApoyosINC{get;set;}
        [DataMember]
        public  UDT_Valor PF_IngDispApoyosFIN{get;set;}
        [DataMember]
        public  UDT_SiNo PF_ReqSopIngrIndDEU{get;set;}
        [DataMember]
        public  UDT_SiNo PF_ReqSopIngrIndCON{get;set;}
        [DataMember]
        public  UDT_Valor PF_IngReqDeu1SOL{get;set;}
        [DataMember]
        public  UDT_Valor PF_IngReqDeu1AJU{get;set;}
        [DataMember]
        public  UDT_Valor PF_IngReqDeu1INC{get;set;}
        [DataMember]
        public  UDT_Valor PF_IngReqDeu1FIN{get;set;}
        [DataMember]
        public  UDT_Valor PF_IngReqDeu2SOL{get;set;}
        [DataMember]
        public  UDT_Valor PF_IngReqDeu2AJU{get;set;}
        [DataMember]
        public  UDT_Valor PF_IngReqDeu2INC{get;set;}
        [DataMember]
        public  UDT_Valor PF_IngReqDeu2FIN{get;set;}
        [DataMember]
        public  UDT_Valor PF_IngReqDeu3SOL{get;set;}
        [DataMember]
        public  UDT_Valor PF_IngReqDeuAJU{get;set;}
        [DataMember]
        public  UDT_Valor PF_IngReqDeu3INC{get;set;}
        [DataMember]
        public  UDT_Valor PF_IngReqDeu3FIN{get;set;}
        [DataMember]
        public  UDT_Valor PF_IngReqDeuFinSOL{get;set;}
        [DataMember]
        public  UDT_Valor PF_IngReqDeuFinAJU{get;set;}
        [DataMember]
        public  UDT_Valor PF_IngReqDeuFinINC{get;set;}
        [DataMember]
        public  UDT_Valor PF_IngReqDeuFinFIN{get;set;}
        [DataMember]
        public  UDT_Valor PF_IngReqCon1SOL{get;set;}
        [DataMember]
        public  UDT_Valor PF_IngReqCon1AJU{get;set;}
        [DataMember]
        public  UDT_Valor PF_IngReqCon1INC{get;set;}
        [DataMember]
        public  UDT_Valor PF_IngReqCon1FIN{get;set;}
        [DataMember]
        public  UDT_Valor PF_IngReqCon2SOL{get;set;}
        [DataMember]
        public  UDT_Valor PF_IngReqCon2AJU{get;set;}
        [DataMember]
        public  UDT_Valor PF_IngReqCon2INC{get;set;}
        [DataMember]
        public  UDT_Valor PF_IngReqCon2FIN{get;set;}
        [DataMember]
        public  UDT_Valor PF_IngReqConFinSOL{get;set;}
        [DataMember]
        public  UDT_Valor PF_IngReqConFinAJU{get;set;}
        [DataMember]
        public  UDT_Valor PF_IngReqConFinINC{get;set;}
        [DataMember]
        public  UDT_Valor PF_IngReqConFinFIN{get;set;}
        [DataMember]
        public  UDT_Valor PF_Cuantia{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_TasaTablEva1{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_FactTablEva{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_TasaTablEva2{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_TasaPonderada{get;set;}

//////
        [DataMember]
        public  UDT_SiNo PF_PolCubriendoInd{get;set;}

        [DataMember]
        public UDTSQL_tinyint PF_PlazoFinal { get; set; }
        [DataMember]
        public UDT_Valor PF_VlrMinimoGar { get; set; }
        [DataMember]
        public UDT_Valor PF_VlrMinimoFirma2 { get; set; }
        [DataMember]
        public UDT_Valor PF_VlrMinimoFirma3 { get; set; }

        [DataMember]
        public  UDT_Valor PF_CtaFinanciacion{get;set;}
        [DataMember]
        public  UDT_Valor PF_CtaSeguros{get;set;}
        [DataMember]
        public  UDT_PorcentajeID PF_PorEstimado{get;set;}
        [DataMember]
        public  UDTSQL_varchar  Verificacion1{get;set;}
        [DataMember]
        public  UDTSQL_varchar Verificacion2{get;set;}
        [DataMember]
        public  UDTSQL_varchar Verificacion3{get;set;}
        [DataMember]
        public  UDT_Valor AN_Resultado1SOL{get;set;}
        [DataMember]
        public  UDT_Valor AN_Resultado1CA1{get;set;}
        [DataMember]
        public  UDT_Valor AN_Resultado1CA2{get;set;}
        [DataMember]
        public  UDT_Valor AN_Resultado1CA3{get;set;}
        [DataMember]
        public  UDT_Valor AN_Resultado1AJ1{get;set;}
        [DataMember]
        public  UDT_Valor AN_Resultado1AJ2{get;set;}
        [DataMember]
        public  UDT_Valor AN_Resultado1V11{get;set;}
        [DataMember]
        public  UDT_Valor AN_Resultado1V12{get;set;}
        [DataMember]
        public  UDT_Valor AN_Resultado1V21{get;set;}
        [DataMember]
        public  UDT_Valor AN_Resultado1V22{get;set;}
        [DataMember]
        public  UDT_Valor AN_Resultado1V31{get;set;}
        [DataMember]
        public  UDT_Valor AN_Resultado1V32{get;set;}
        [DataMember]
        public  UDT_Valor AN_Resultado2SOL{get;set;}
        [DataMember]
        public  UDT_Valor AN_Resultado2CA1{get;set;}
        [DataMember]
        public  UDT_Valor AN_Resultado2CA2{get;set;}
        [DataMember]
        public  UDT_Valor AN_Resultado2CA3{get;set;}
        [DataMember]
        public  UDT_Valor AN_Resultado2AJ1{get;set;}
        [DataMember]
        public  UDT_Valor AN_Resultado2AJ2{get;set;}
        [DataMember]
        public  UDT_Valor AN_Resultado2V11{get;set;}
        [DataMember]
        public  UDT_Valor AN_Resultado2V12{get;set;}
        [DataMember]
        public  UDT_Valor AN_Resultado2V21{get;set;}
        [DataMember]
        public  UDT_Valor AN_Resultado2V22{get;set;}
        [DataMember]
        public  UDT_Valor AN_Resultado2V31{get;set;}
        [DataMember]
        public  UDT_Valor AN_Resultado2V32{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrVenta{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrFasecolda{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrCtaEvaSOL{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrCtaEvaCA1{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrCtaEvaCA2{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrCtaEvaCA3{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrCtaEvaAJ1{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrCtaEvaAJ2{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrCtaEvaV11{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrCtaEvaV12{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrCtaEvaV21{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrCtaEvaV22{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrCtaEvaV31{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrCtaEvaV32{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrMinLim{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrMaxLim{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrSolicitado{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrAlternCA1{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrAlternCA2{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrAlternCA3{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrAlternAJ1{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrAlternAJ2{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrAlternV11{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrAlternV12{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrAlternV21{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrAlternV22{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrAlternV31{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrAlternV32{get;set;}
        [DataMember]
        public  UDT_Valor AN_CtaInicial{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrIncremSOL{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrIncremCA1{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrIncremCA2{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrIncremCA3{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrIncremAJ1{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrIncremAJ2{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrIncremV11{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrIncremV12{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrIncremV21{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrIncremV22{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrIncremV31{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrIncremV32{get;set;}
        [DataMember]
        public  UDT_Valor AN_CtaIniAjuCA1{get;set;}
        [DataMember]
        public  UDT_Valor AN_CtaIniAjuCA2{get;set;}
        [DataMember]
        public  UDT_Valor AN_CtaIniAjuCA3{get;set;}
        [DataMember]
        public  UDT_Valor AN_CtaIniAjuAJ1{get;set;}
        [DataMember]
        public  UDT_Valor AN_CtaIniAjuAJ2{get;set;}
        [DataMember]
        public  UDT_Valor AN_CtaIniAjuV11{get;set;}
        [DataMember]
        public  UDT_Valor AN_CtaIniAjuV12{get;set;}
        [DataMember]
        public  UDT_Valor AN_CtaIniAjuV21{get;set;}
        [DataMember]
        public  UDT_Valor AN_CtaIniAjuV22{get;set;}
        [DataMember]
        public  UDT_Valor AN_CtaIniAjuV31{get;set;}
        [DataMember]
        public  UDT_Valor AN_CtaIniAjuV32{get;set;}
        [DataMember]
        public  UDT_PorcentajeID AN_porFinSoli{get;set;}
        [DataMember]
        public  UDT_PorcentajeID AN_porFinMaxSOL{get;set;}
        [DataMember]
        public  UDT_PorcentajeID AN_porFinMaxCA1{get;set;}
        [DataMember]
        public  UDT_PorcentajeID AN_porFinMaxCA2{get;set;}
        [DataMember]
        public  UDT_PorcentajeID AN_porFinMaxCA3{get;set;}
        [DataMember]
        public  UDT_PorcentajeID AN_porFinMaxAJ1{get;set;}
        [DataMember]
        public  UDT_PorcentajeID AN_porFinMaxAJ2{get;set;}
        [DataMember]
        public  UDT_PorcentajeID AN_porFinMaxV11{get;set;}
        [DataMember]
        public  UDT_PorcentajeID AN_porFinMaxV12{get;set;}
        [DataMember]
        public  UDT_PorcentajeID AN_porFinMaxV21{get;set;}
        [DataMember]
        public  UDT_PorcentajeID AN_porFinMaxV22{get;set;}
        [DataMember]
        public  UDT_PorcentajeID AN_porFinMaxV31{get;set;}
        [DataMember]
        public  UDT_PorcentajeID AN_porFinMaxV32{get;set;}
        [DataMember]
        public  UDT_PorcentajeID AN_porFinAltCA1{get;set;}
        [DataMember]
        public  UDT_PorcentajeID AN_porFinAltCA2{get;set;}
        [DataMember]
        public  UDT_PorcentajeID AN_porFinAltCA3{get;set;}
        [DataMember]
        public  UDT_PorcentajeID AN_porFinAltAJ1{get;set;}
        [DataMember]
        public  UDT_PorcentajeID AN_porFinAltAJ2{get;set;}
        [DataMember]
        public  UDT_PorcentajeID AN_porFinAltV11{get;set;}
        [DataMember]
        public  UDT_PorcentajeID AN_porFinAltV12{get;set;}
        [DataMember]
        public  UDT_PorcentajeID AN_porFinAltV21{get;set;}
        [DataMember]
        public  UDT_PorcentajeID AN_porFinAltV22{get;set;}
        [DataMember]
        public  UDT_PorcentajeID AN_porFinAltV31{get;set;}
        [DataMember]
        public  UDT_PorcentajeID AN_porFinAltV32{get;set;}
        [DataMember]
        public  UDT_SiNo AN_CumParGarAJ1{get;set;}
        [DataMember]
        public  UDT_SiNo AN_CumParGarAJ2{get;set;}
        [DataMember]
        public  UDT_SiNo AN_CumParGarV11{get;set;}
        [DataMember]
        public  UDT_SiNo AN_CumParGarV12{get;set;}
        [DataMember]
        public  UDT_SiNo AN_CumParGarV21{get;set;}
        [DataMember]
        public  UDT_SiNo AN_CumParGarV22{get;set;}
        [DataMember]
        public  UDT_SiNo AN_CumParGarV31{get;set;}
        [DataMember]
        public  UDT_SiNo AN_CumParGarV32{get;set;}
        [DataMember]
        public  UDTSQL_varchar AN_CapPagoSOL{get;set;}
        [DataMember]
        public  UDTSQL_varchar AN_CapPagoCA1{get;set;}
        [DataMember]
        public  UDTSQL_varchar AN_CapPagoCA2{get;set;}
        [DataMember]
        public  UDTSQL_varchar AN_CapPagoCA3{get;set;}
        [DataMember]
        public  UDTSQL_varchar AN_CapPagoAJ1{get;set;}
        [DataMember]
        public  UDTSQL_varchar AN_CapPagoAJ2{get;set;}
        [DataMember]
        public  UDTSQL_varchar AN_CapPagoV11{get;set;}
        [DataMember]
        public  UDTSQL_varchar AN_CapPagoV12{get;set;}
        [DataMember]
        public  UDTSQL_varchar AN_CapPagoV21{get;set;}
        [DataMember]
        public  UDTSQL_varchar AN_CapPagoV22{get;set;}
        [DataMember]
        public  UDTSQL_varchar AN_CapPagoV31{get;set;}
        [DataMember]
        public  UDTSQL_varchar AN_CapPagoV32{get;set;}
        [DataMember]
        public  UDT_SiNo AN_PuedeIncCICA1{get;set;}
        [DataMember]
        public  UDT_SiNo AN_PuedeIncCICA2{get;set;}
        [DataMember]
        public  UDT_SiNo AN_PuedeIncCICA3{get;set;}
        [DataMember]
        public  UDT_SiNo AN_PuedeIncCIAJ1{get;set;}
        [DataMember]
        public  UDT_SiNo AN_PuedeIncCIAJ2{get;set;}
        [DataMember]
        public  UDT_SiNo AN_PuedeIncCIV11{get;set;}
        [DataMember]
        public  UDT_SiNo AN_PuedeIncCIV12{get;set;}
        [DataMember]
        public  UDT_SiNo AN_PuedeIncCIV21{get;set;}
        [DataMember]
        public  UDT_SiNo AN_PuedeIncCIV22{get;set;}
        [DataMember]
        public  UDT_SiNo AN_PuedeIncCIV31{get;set;}
        [DataMember]
        public  UDT_SiNo AN_PuedeIncCIV32{get;set;}
        [DataMember]
        public  UDT_SiNo AN_CumPorMaxSOL{get;set;}
        [DataMember]
        public  UDT_SiNo AN_CumPorMaxCA1{get;set;}
        [DataMember]
        public  UDT_SiNo AN_CumPorMaxCA2{get;set;}
        [DataMember]
        public  UDT_SiNo AN_CumPorMaxCA3{get;set;}
        [DataMember]
        public  UDT_SiNo AN_CumPorMaxAJ1{get;set;}
        [DataMember]
        public  UDT_SiNo AN_CumPorMaxAJ2{get;set;}
        [DataMember]
        public  UDT_SiNo AN_CumPorMaxV11{get;set;}
        [DataMember]
        public  UDT_SiNo AN_CumPorMaxV12{get;set;}
        [DataMember]
        public  UDT_SiNo AN_CumPorMaxV21{get;set;}
        [DataMember]
        public  UDT_SiNo AN_CumPorMaxV22{get;set;}
        [DataMember]
        public  UDT_SiNo AN_CumPorMaxV31{get;set;}
        [DataMember]
        public  UDT_SiNo AN_CumPorMaxV32{get;set;}
        [DataMember]
        public  UDT_SiNo AN_CumMtoMinCA1{get;set;}
        [DataMember]
        public  UDT_SiNo AN_CumMtoMinCA2{get;set;}
        [DataMember]
        public  UDT_SiNo AN_CumMtoMinCA3{get;set;}
        [DataMember]
        public  UDT_SiNo AN_CumMtoMinAJ1{get;set;}
        [DataMember]
        public  UDT_SiNo AN_CumMtoMinAJ2{get;set;}
        [DataMember]
        public  UDT_SiNo AN_CumMtoMinV11{get;set;}
        [DataMember]
        public  UDT_SiNo AN_CumMtoMinV12{get;set;}
        [DataMember]
        public  UDT_SiNo AN_CumMtoMinV21{get;set;}
        [DataMember]
        public  UDT_SiNo AN_CumMtoMinV22{get;set;}
        [DataMember]
        public  UDT_SiNo AN_CumMtoMinV31{get;set;}
        [DataMember]
        public  UDT_SiNo AN_CumMtoMinV32{get;set;}
        [DataMember]
        public  UDTSQL_varchar AN_CumOtroSOL{get;set;}
        [DataMember]
        public  UDTSQL_varchar AN_CumOtroCA1{get;set;}
        [DataMember]
        public  UDTSQL_varchar AN_CumOtroCA2{get;set;}
        [DataMember]
        public  UDTSQL_varchar AN_CumOtroCA3{get;set;}
        [DataMember]
        public  UDTSQL_varchar AN_CumOtroAJ1{get;set;}
        [DataMember]
        public  UDTSQL_varchar AN_CumOtroAJ2{get;set;}
        [DataMember]
        public  UDTSQL_varchar AN_CumOtroV11{get;set;}
        [DataMember]
        public  UDTSQL_varchar AN_CumOtroV12{get;set;}
        [DataMember]
        public  UDTSQL_varchar AN_CumOtroV21{get;set;}
        [DataMember]
        public  UDTSQL_varchar AN_CumOtroV22{get;set;}
        [DataMember]
        public  UDTSQL_varchar AN_CumOtroV31{get;set;}
        [DataMember]
        public  UDTSQL_varchar AN_CumOtroV32{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrAutorizaSOL{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrAutorizaCA1{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrAutorizaCA2{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrAutorizaCA3{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrAutorizaAJ1{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrAutorizaAJ2{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrAutorizaV11{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrAutorizaV12{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrAutorizaV21{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrAutorizaV22{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrAutorizaV31{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrAutorizaV32{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrAutFinSOL{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrAutFinCA2{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrAutFinAJ1{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrAutFinV12{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrAutFinV21{get;set;}
        [DataMember]
        public  UDT_Valor AN_VlrAutFinV31{get;set;}
        [DataMember]
        public  UDTSQL_varchar AN_AltNoComAJ1{get;set;}
        [DataMember]
        public  UDTSQL_varchar AN_AltNoComAJ2{get;set;}
        [DataMember]
        public  UDTSQL_varchar AN_AltNoComV12{get;set;}
        [DataMember]
        public  UDTSQL_varchar AN_AltNoComV21{get;set;}
        [DataMember]
        public  UDTSQL_varchar AN_AltNoComV22{get;set;}
        [DataMember]
        public  UDTSQL_varchar AN_AltNoComV31{get;set;}
        [DataMember]
        public  UDTSQL_varchar AN_AltNoComV32{get;set;}

        [DataMember]
        public  UDTSQL_varchar Alternativa8{get;set;}
        [DataMember]
        public  UDT_PorcentajeID FactorAlt8{get;set;}
        [DataMember]
        public  UDT_Valor MontoAlt8{get;set;}
        [DataMember]
        public  UDT_Valor GarantiaAlt8{get;set;}
        [DataMember]
        public  UDT_Valor IngrMinSopDeu1{get;set;}
        [DataMember]
        public  UDT_Valor IngrMinSopDeu2{get;set;}
        [DataMember]
        public  UDT_Valor IngrMinSopDeu3{get;set;}
        [DataMember]
        public  UDT_Valor IngrMinSopDeu4{get;set;}
        [DataMember]
        public  UDT_Valor IngrMinSopDeu5{get;set;}
        [DataMember]
        public  UDT_Valor IngrMinSopDeu6{get;set;}
        [DataMember]
        public  UDT_Valor IngrMinSopDeu7{get;set;}
        [DataMember]
        public  UDT_Valor IngrMinSopDeu8{get;set;}
        [DataMember]
        public  UDT_Valor IngrMinSopCon1{get;set;}
        [DataMember]
        public  UDT_Valor IngrMinSopCon2{get;set;}
        [DataMember]
        public  UDT_Valor IngrMinSopCon3{get;set;}
        [DataMember]
        public  UDT_Valor IngrMinSopCon4{get;set;}
        [DataMember]
        public  UDT_Valor IngrMinSopCon5{get;set;}
        [DataMember]
        public  UDT_Valor IngrMinSopCon6{get;set;}
        [DataMember]
        public  UDT_Valor IngrMinSopCon7{get;set;}
        [DataMember]
        public  UDT_Valor IngrMinSopCon8{get;set;}

        [DataMember]
        public UDTSQL_smalldatetime FechaFirmaDocumento { get; set; }
        [DataMember]
        public UDTSQL_tinyint AccionSolicitud       { get; set; }

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }
        [DataMember]
        public UDTSQL_tinyint PF_Plazo { get; set; }
        [DataMember]
        public UDT_PorcentajeID PF_PorMaxEstadoActual { get; set; }
        [DataMember]
        public UDT_PorcentajeID porMaximo { get; set; }
        [DataMember]
        public UDT_Valor VlrGarantia { get; set; }
        [DataMember]
        public UDTSQL_tinyint AccionSolicitud1 { get; set; }
        [DataMember]
        public UDTSQL_tinyint AccionSolicitud2 { get; set; }
        [DataMember]
        public UDTSQL_tinyint AccionSolicitud3 { get; set; }
        [DataMember]
        public UDTSQL_tinyint PF_PlazoFinal1 { get; set; }
        [DataMember]
        public UDTSQL_tinyint PF_PlazoFinal2 { get; set; }
        [DataMember]
        public UDTSQL_tinyint PF_PlazoFinal3 { get; set; }
        [DataMember]
        public UDT_Valor PF_VlrMontoFirma1 { get; set; }
        [DataMember]
        public UDT_Valor PF_VlrMontoFirma2 { get; set; }
        [DataMember]
        public UDT_Valor PF_VlrMontoFirma3 { get; set; }
        [DataMember]
        public UDT_PorcentajeID PF_TasaPerfilOBL { get; set; }
        //[DataMember]
        //public UDT_PorcentajeID PF_TasaFirma1OBL { get; set; }        
        //[DataMember]
        //public UDT_PorcentajeID PF_TasaFirma2OBL { get; set; }
        [DataMember]
        public UDT_PorcentajeID PF_TasaFirma3OBL { get; set; }
        [DataMember]
        public UDT_PorcentajeID PF_PorMaxFirma1 { get; set; }
        [DataMember]
        public UDT_PorcentajeID PF_PorMaxFirma2 { get; set; }
        [DataMember]
        public UDT_PorcentajeID PF_PorMaxFirma3 { get; set; }
        [DataMember]
        public UDT_Valor PF_VlrGarantiaPerfil { get; set; }
        [DataMember]
        public UDT_Valor PF_VlrGarantiaFirma1 { get; set; }

        [DataMember]
        public UDT_Valor PF_VlrGarantiaFirma2 { get; set; }
        [DataMember]
        public UDT_Valor PF_VlrGarantiaFirma3 { get; set; }

        #endregion
    }
}
