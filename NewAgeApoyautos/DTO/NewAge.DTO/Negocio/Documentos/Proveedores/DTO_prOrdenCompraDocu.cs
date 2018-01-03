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
    /// Models DTO_prOrdenCompraDocu
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_prOrdenCompraDocu
    {
        #region DTO_prOrdenCompraDocu

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_prOrdenCompraDocu(IDataReader dr)
        {
            InitCols();
            try
            {
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.ProveedorID.Value = dr["ProveedorID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["TipoOrden"].ToString()))
                    this.TipoOrden.Value = Convert.ToByte(dr["TipoOrden"]);
                if (!string.IsNullOrWhiteSpace(dr["Inconterm"].ToString()))
                    this.Inconterm.Value = Convert.ToByte(dr["Inconterm"]);
                if (!string.IsNullOrWhiteSpace(dr["ContratoNro"].ToString()))
                   this.ContratoNro.Value = Convert.ToInt32(dr["ContratoNro"]);
                this.MonedaOrden.Value = dr["MonedaOrden"].ToString();
                this.MonedaPago.Value = dr["MonedaPago"].ToString();
                this.LugarEntrega.Value = dr["LugarEntrega"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["FechaEntrega"].ToString()))
                    this.FechaEntrega.Value = Convert.ToDateTime(dr["FechaEntrega"]);
                if (!string.IsNullOrWhiteSpace(dr["PagoVariablend"].ToString()))
                    this.PagoVariablend.Value = Convert.ToBoolean(dr["PagoVariablend"]);
                if (!string.IsNullOrWhiteSpace(dr["TerminosInd"].ToString()))
                    this.TerminosInd.Value = Convert.ToBoolean(dr["TerminosInd"]);
                this.VlrAnticipo.Value = Convert.ToDecimal(dr["VlrAnticipo"]);
                if (!string.IsNullOrWhiteSpace(dr["DtoProntoPago"].ToString()))
                    this.DtoProntoPago.Value = Convert.ToDecimal(dr["DtoProntoPago"]);
                if (!string.IsNullOrWhiteSpace(dr["DiasPtoPago"].ToString()))
                    this.DiasPtoPago.Value = Convert.ToByte(dr["DiasPtoPago"]);
                this.FormaPago.Value = dr["FormaPago"].ToString();
                this.Instrucciones.Value = dr["Instrucciones"].ToString();
                this.Observaciones.Value = dr["Observaciones"].ToString();
                this.ObservRechazo.Value = dr["ObservRechazo"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["FechaERechazo"].ToString()))
                    this.FechaERechazo.Value = Convert.ToDateTime(dr["FechaERechazo"]); 
                if (!string.IsNullOrWhiteSpace(dr["TasaOrden"].ToString()))
                    this.TasaOrden.Value = Convert.ToDecimal(dr["TasaOrden"]); 
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
                this.DireccionEntrega.Value = dr["DireccionEntrega"].ToString();
                this.TelefonoEntrega.Value = dr["TelefonoEntrega"].ToString();
                this.Encargado.Value = dr["Encargado"].ToString();
                this.ContactoComercial.Value = dr["ContactoComercial"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_prOrdenCompraDocu()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.ProveedorID = new UDT_ProveedorID();
            this.TipoOrden = new UDTSQL_tinyint();
            this.Inconterm = new UDTSQL_tinyint();
            this.ContratoNro = new UDT_Consecutivo();
            this.MonedaOrden = new UDT_MonedaID();
            this.MonedaPago = new UDT_MonedaID();
            this.LugarEntrega = new UDT_LocFisicaID();
            this.FechaEntrega = new UDTSQL_smalldatetime();
            this.PagoVariablend = new UDT_SiNo();
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
            this.DireccionEntrega  = new UDT_DescripTExt();
            this.TelefonoEntrega  = new UDT_DescripTBase();
            this.Encargado = new UDT_DescripTBase();
            this.ContactoComercial = new UDTSQL_char(50);
            //Adicionales
            this.ProyectoID = new UDT_ProyectoID();
            this.DireccionEntrega = new UDT_DescripTExt();
            this.TelefonoEntrega = new UDT_DescripTBase();
            this.ProveedorDesc = new UDT_Descriptivo();
        }
        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_ProveedorID ProveedorID { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_tinyint TipoOrden { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_tinyint Inconterm { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Consecutivo ContratoNro { get; set; }

        [DataMember]
        public UDT_MonedaID MonedaOrden { get; set; }

        [DataMember]
        public UDT_MonedaID MonedaPago { get; set; }

        [DataMember]
        public UDT_LocFisicaID LugarEntrega { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_smalldatetime FechaEntrega { get; set; }

        [DataMember]
        public UDT_SiNo PagoVariablend { get; set; }

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
        [AllowNull]
        public UDT_DescripTExt ObservRechazo { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_datetime FechaERechazo { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_TasaID TasaOrden { get; set; }

        [DataMember]
        [AllowNull]
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
        [AllowNull]
        public UDT_UsuarioID UsuarioSolicita { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_tinyint Prioridad { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor IVA { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor VlrPagoMes { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_tinyint NroPagos { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_datetime FechaPago1 { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_datetime FechaVencimiento { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTBase Encargado { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTBase TelefonoEntrega { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTExt DireccionEntrega { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char ContactoComercial { get; set; }

        //Adicionales
        [DataMember]
        [AllowNull]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Descriptivo ProveedorDesc { get; set; }

        #endregion
    }
}
