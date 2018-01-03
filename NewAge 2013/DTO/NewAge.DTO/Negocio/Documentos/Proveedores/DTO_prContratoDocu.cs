using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;
using System.Reflection;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class recibidos de bienes y servicios:
    /// Models DTO_prContratoDocu
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_prContratoDocu
    {
        #region DTO_prContratoDocu

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_prContratoDocu(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.ProveedorID.Value = dr["ProveedorID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["TipoOrden"].ToString()))
                    this.TipoOrden.Value = Convert.ToByte(dr["TipoOrden"]);
                if (!string.IsNullOrWhiteSpace(dr["TipoDocumento"].ToString()))
                    this.TipoDocumento.Value = Convert.ToByte(dr["TipoDocumento"]);
                if (!string.IsNullOrWhiteSpace(dr["TipoOtroSi"].ToString()))
                    this.TipoOtroSi.Value = Convert.ToByte(dr["TipoOtroSi"]);
                if (!string.IsNullOrWhiteSpace(dr["DocContratoPRY"].ToString()))
                    this.DocContratoPRY.Value = Convert.ToInt32(dr["DocContratoPRY"]);                
                if (!string.IsNullOrWhiteSpace(dr["OrdenCompraNro"].ToString()))
                    this.OrdenCompraNro.Value = Convert.ToInt32(dr["OrdenCompraNro"]);
                if (!string.IsNullOrWhiteSpace(dr["ContratoMacroNro"].ToString()))
                    this.ContratoMacroNro.Value = Convert.ToInt32(dr["ContratoMacroNro"]);
                this.MonedaOrden.Value = dr["MonedaOrden"].ToString();
                this.MonedaPago.Value = dr["MonedaPago"].ToString();
                this.LugarEntrega.Value = dr["LugarEntrega"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["PagoVariableInd"].ToString()))
                    this.PagoVariableInd.Value = Convert.ToBoolean(dr["PagoVariableInd"]);
                this.TerminosInd.Value = Convert.ToBoolean(dr["TerminosInd"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrAnticipo"].ToString()))
                    this.VlrAnticipo.Value = Convert.ToDecimal(dr["VlrAnticipo"]);
                if (!string.IsNullOrWhiteSpace(dr["DtoProntoPago"].ToString()))
                    this.DtoProntoPago.Value = Convert.ToDecimal(dr["DtoProntoPago"]);
                if (!string.IsNullOrWhiteSpace(dr["DiasPtoPago"].ToString()))
                    this.DiasPtoPago.Value = Convert.ToByte(dr["DiasPtoPago"]);
                this.FormaPago.Value = dr["FormaPago"].ToString();
                this.Instrucciones.Value = dr["Instrucciones"].ToString();
                this.Observaciones.Value = dr["Observaciones"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["ContratoMacroNro"].ToString()))
                    this.ObservRechazo.Value = dr["ObservRechazo"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["FechaERechazo"].ToString()))
                    this.FechaERechazo.Value = Convert.ToDateTime(dr["FechaERechazo"]);
                if (!string.IsNullOrWhiteSpace(dr["TasaOrden"].ToString()))
                    this.TasaOrden.Value = Convert.ToDecimal(dr["TasaOrden"]);
                if (!string.IsNullOrWhiteSpace(dr["UsuarioRechaza"].ToString()))
                    this.UsuarioRechaza.Value = dr["UsuarioRechaza"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["PorcentAdministra"].ToString()))
                    this.PorcentAdministra.Value = Convert.ToDecimal(dr["PorcentAdministra"]);
                if (!string.IsNullOrWhiteSpace(dr["Porcentimprevisto"].ToString()))
                    this.Porcentimprevisto.Value = Convert.ToDecimal(dr["Porcentimprevisto"]);
                if (!string.IsNullOrWhiteSpace(dr["PorcentUtilidad"].ToString()))
                    this.PorcentUtilidad.Value = Convert.ToDecimal(dr["PorcentUtilidad"]);
                if (!string.IsNullOrWhiteSpace(dr["IncluyeAUICosto"].ToString()))
                    this.IncluyeAUICosto.Value = Convert.ToBoolean(dr["IncluyeAUICosto"]);
                if (!string.IsNullOrWhiteSpace(dr["PorcentHolgura"].ToString()))
                    this.PorcentHolgura.Value = Convert.ToDecimal(dr["PorcentHolgura"]);
                this.AreaAprobacion.Value = dr["AreaAprobacion"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["UsuarioSolicita"].ToString()))
                    this.UsuarioSolicita.Value = dr["UsuarioSolicita"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["Prioridad"].ToString()))
                    this.Prioridad.Value = Convert.ToByte(dr["Prioridad"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor"].ToString()))
                    this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                if (!string.IsNullOrWhiteSpace(dr["IVA"].ToString()))
                    this.IVA.Value = Convert.ToDecimal(dr["IVA"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrPagoMes"].ToString()))
                    this.VlrPagoMes.Value = Convert.ToDecimal(dr["VlrPagoMes"]);
                if (!string.IsNullOrWhiteSpace(dr["NroPagos"].ToString()))
                    this.NroPagos.Value = Convert.ToByte(dr["NroPagos"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaPago1"].ToString()))
                    this.FechaPago1.Value = Convert.ToDateTime(dr["FechaPago1"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaVencimiento"].ToString()))
                    this.FechaVencimiento.Value = Convert.ToDateTime(dr["FechaVencimiento"]);
                if (!string.IsNullOrWhiteSpace(dr["RteGarantiaPor"].ToString()))
                    this.RteGarantiaPor.Value = Convert.ToDecimal(dr["RteGarantiaPor"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_prContratoDocu()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.EmpresaID = new UDT_EmpresaID();
            this.ProveedorID = new UDT_ProveedorID();
            this.TipoOrden = new UDTSQL_tinyint();
            this.TipoDocumento = new UDTSQL_tinyint();
            this.TipoOtroSi = new UDTSQL_tinyint();
            this.DocContratoPRY = new UDT_Consecutivo();
            this.OrdenCompraNro = new UDT_Consecutivo();
            this.ContratoMacroNro = new UDT_Consecutivo();
            this.MonedaOrden = new UDT_MonedaID();
            this.MonedaPago = new UDT_MonedaID();
            this.LugarEntrega = new UDT_LocFisicaID();
            this.PagoVariableInd = new UDT_SiNo();
            this.TerminosInd = new UDT_SiNo();
            this.VlrAnticipo = new UDT_Valor();
            this.DtoProntoPago = new UDT_PorcentajeID();
            this.DiasPtoPago = new UDTSQL_tinyint();
            this.FormaPago = new UDT_DescripTExt();
            this.Instrucciones = new UDT_DescripTExt();
            this.Observaciones = new UDT_DescripTExt();
            this.ObservRechazo = new UDT_DescripTExt();
            this.FechaERechazo = new UDTSQL_datetime();
            this.TasaOrden = new UDT_TasaID();
            this.UsuarioRechaza = new UDT_UsuarioID();
            this.PorcentAdministra = new UDT_PorcentajeID();
            this.Porcentimprevisto = new UDT_PorcentajeID();
            this.PorcentUtilidad = new UDT_PorcentajeID();
            this.IncluyeAUICosto = new UDT_SiNo();
            this.PorcentHolgura = new UDT_PorcentajeID();
            this.AreaAprobacion = new UDT_AreaFuncionalID();
            this.UsuarioSolicita = new UDT_UsuarioID();
            this.Prioridad = new UDTSQL_tinyint();
            this.Valor = new UDT_Valor();
            this.IVA = new UDT_Valor();
            this.VlrPagoMes = new UDT_Valor();
            this.NroPagos = new UDTSQL_tinyint();
            this.FechaPago1 = new UDTSQL_datetime();
            this.FechaVencimiento = new UDTSQL_datetime();
            this.RteGarantiaPor = new UDT_PorcentajeID();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        [Filtrable]
        public UDT_ProveedorID ProveedorID { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoOrden { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_tinyint TipoDocumento { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoOtroSi { get; set; }

        [DataMember]
        public UDT_Consecutivo DocContratoPRY { get; set; }

        [DataMember]
        public UDT_Consecutivo OrdenCompraNro { get; set; }

        [DataMember]
        public UDT_Consecutivo ContratoMacroNro { get; set; }

        [AllowNull]
        [DataMember]
        public UDT_MonedaID MonedaOrden { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_MonedaID MonedaPago { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_LocFisicaID LugarEntrega { get; set; }

        [DataMember]
        public UDT_SiNo PagoVariableInd { get; set; }

        [DataMember]
        public UDT_SiNo TerminosInd { get; set; }

        [DataMember]
        public UDT_Valor VlrAnticipo { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_PorcentajeID DtoProntoPago { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_tinyint DiasPtoPago { get; set; }

        [DataMember]
        public UDT_DescripTExt FormaPago { get; set; }

        [DataMember]
        public UDT_DescripTExt Instrucciones { get; set; }

        [DataMember]
        public UDT_DescripTExt Observaciones { get; set; }

        [DataMember]
        public UDT_DescripTExt ObservRechazo { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaERechazo { get; set; }

        [DataMember]
        public UDT_TasaID TasaOrden { get; set; }

        [DataMember]
        public UDT_UsuarioID UsuarioRechaza { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_PorcentajeID PorcentAdministra { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_PorcentajeID Porcentimprevisto { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_PorcentajeID PorcentUtilidad { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_SiNo IncluyeAUICosto { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_PorcentajeID PorcentHolgura { get; set; }

        [DataMember]
        public UDT_AreaFuncionalID AreaAprobacion { get; set; }

        [DataMember]
        public UDT_UsuarioID UsuarioSolicita { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_tinyint Prioridad { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_Valor IVA { get; set; }

        [DataMember]
        public UDT_Valor VlrPagoMes { get; set; }

        [DataMember]
        public UDTSQL_tinyint NroPagos { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaPago1 { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaVencimiento { get; set; }

        [DataMember]
        public UDT_PorcentajeID RteGarantiaPor { get; set; }

        #endregion
    }
}
