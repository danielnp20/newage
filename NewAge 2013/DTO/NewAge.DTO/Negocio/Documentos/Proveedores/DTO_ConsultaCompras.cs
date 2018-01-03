using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using NewAge.DTO.Negocio.Reportes;
using NewAge.DTO.Reportes;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Models DTO_ConsultaCompras
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ConsultaCompras
    {
        #region DTO_ConsultaCompras

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ConsultaCompras(IDataReader dr)
        {
            this.InitCols();
            try
            {
                try { this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]); }   catch (Exception) { };
                try { this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]); }   catch (Exception) { };
                try { this.AreaFuncionalID.Value = dr["AreaFuncionalID"].ToString(); }   catch (Exception) { };
                try { this.CantidadSol.Value = Convert.ToDecimal(dr["CantidadSol"]); }   catch (Exception) { };
                try { this.CantidadOC.Value = Convert.ToDecimal(dr["CantidadOC"]); }   catch (Exception) { };
                try { this.CantidadPendOC.Value = Convert.ToDecimal(dr["CantidadPendOC"]); }   catch (Exception) { };
                try { this.CantidadRec.Value = Convert.ToDecimal(dr["CantidadRec"]); }   catch (Exception) { };
                try { this.CantidadPendRec.Value = Convert.ToDecimal(dr["CantidadPendRec"]); }   catch (Exception) { };
                try { this.ProveedorID.Value = dr["ProveedorID"].ToString(); }   catch (Exception) { };
                try { this.ProveedorNombre.Value = dr["ProveedorNombre"].ToString(); }  catch (Exception) { };
                try { this.MonedaPago.Value = dr["MonedaPago"].ToString(); }
                catch (Exception) { };
                try { this.Estado.Value = Convert.ToByte(dr["Estado"]); } catch (Exception) { };
                try
                {
                    if (!string.IsNullOrWhiteSpace(dr["Valor"].ToString()))
                        this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                }  catch (Exception) { };
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ConsultaCompras(): base()
        {
            InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.PrefDoc = new UDT_DescripTBase();
            this.Fecha = new UDTSQL_datetime();
            this.AreaFuncionalID = new UDT_AreaFuncionalID();
            this.Estado = new UDTSQL_smallint();
            this.CantidadSol = new UDT_Cantidad();
            this.CantidadOC = new UDT_Cantidad();
            this.CantidadPendOC = new UDT_Cantidad();
            this.CantidadRec = new UDT_Cantidad();
            this.CantidadFact = new UDT_Cantidad();
            this.CantidadPendRec = new UDT_Cantidad();
            this.ProveedorID = new UDT_ProveedorID();
            this.ProveedorNombre = new UDT_DescripTBase();
            this.Bodega = new UDT_DescripUnFormat();
            this.MonedaPago = new UDT_MonedaID();
            this.MonedaOC = new UDT_MonedaID();
            this.Valor = new UDT_Valor();
            this.Detalle = new List<DTO_ConsultaComprasDet>();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_DescripTBase PrefDoc { get; set; }

        [DataMember]
        public UDTSQL_datetime Fecha { get; set; }

        [AllowNull]
        [DataMember]
        public UDT_AreaFuncionalID AreaFuncionalID { get; set; }

        [AllowNull]
        [DataMember]
        public UDTSQL_smallint Estado { get; set; }

        [AllowNull]
        [DataMember]
        public UDT_Cantidad CantidadSol { get; set; }

        [AllowNull]
        [DataMember]
        public UDT_Cantidad CantidadOC { get; set; }

        [AllowNull]
        [DataMember]
        public UDT_Cantidad CantidadPendOC { get; set; }

        [AllowNull]
        [DataMember]
        public UDT_Cantidad CantidadRec { get; set; }

        [AllowNull]
        [DataMember]
        public UDT_Cantidad CantidadPendRec { get; set; }

        [AllowNull]
        [DataMember]
        public UDT_Cantidad CantidadFact { get; set; }

        [AllowNull]
        [DataMember]
        public UDT_ProveedorID ProveedorID { get; set; }

        [AllowNull]
        [DataMember]
        public UDT_DescripTBase ProveedorNombre { get; set; }

        [DataMember]
        public UDT_DescripUnFormat Bodega { get; set; }

        [AllowNull]
        [DataMember]
        public UDT_MonedaID MonedaOC { get; set; }

        [AllowNull]
        [DataMember]
        public UDT_MonedaID MonedaPago { get; set; }

        [AllowNull]
        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public List<DTO_ConsultaComprasDet> Detalle { get; set; }

        #endregion
    }

    /// <summary>
    /// Class Models DTO_ConsultaComprasDet
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ConsultaComprasDet
    {
        public DTO_ConsultaComprasDet(IDataReader dr)
        {
            InitDetCols();
            try
            {
                this.CodigoBSID.Value = dr["CodigoBSID"].ToString();
                this.inReferenciaID.Value = dr["inReferenciaID"].ToString();
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
                this.CantidadSol.Value = Convert.ToDecimal(dr["CantidadSol"]);
                this.CantidadOC.Value = Convert.ToDecimal(dr["CantidadOC"]);
                this.CantidadRec.Value = Convert.ToDecimal(dr["CantidadRec"]);
                this.ValorDet.Value = Convert.ToDecimal(dr["ValorDet"]);
                try { this.ProyectoID.Value = dr["ProyectoID"].ToString(); }    catch (Exception) { };
                try { this.CentroCostoID.Value = dr["CentroCostoID"].ToString(); }   catch (Exception) { };
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DTO_ConsultaComprasDet()
        {
            InitDetCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitDetCols()
        {
            this.CodigoBSID = new UDT_CodigoBSID();
            this.inReferenciaID = new UDT_inReferenciaID();
            this.MarcaInvID = new UDT_Descriptivo();
            this.RefProveedor = new UDT_CodigoGrl20();
            this.Descriptivo = new UDT_DescripTBase();
            this.SolicitudDocuID = new UDT_Consecutivo();
            this.SolicitudDetaID = new UDT_Consecutivo();
            this.OrdCompraDocuID = new UDT_Consecutivo();
            this.OrdCompraDetaID = new UDT_Consecutivo();
            this.RecibidoDocuID = new UDT_Consecutivo();
            this.RecibidoDetaID = new UDT_Consecutivo();
            this.CantidadSol = new UDT_Cantidad();
            this.CantidadOC = new UDT_Cantidad();
            this.CantidadRec = new UDT_Cantidad();
            this.CantidadFact = new UDT_Cantidad();
            this.FacturaDocuID = new UDT_Consecutivo();          
            this.ProyectoID = new UDT_ProyectoID();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.ValorDet = new UDT_Valor();
        }

        #region Propiedades

        [DataMember]
        public UDT_CodigoBSID CodigoBSID { get; set; }

        [DataMember]
        public UDT_inReferenciaID inReferenciaID { get; set; }

        [DataMember]
        public UDT_Descriptivo MarcaInvID { get; set; }

        [DataMember]
        public UDT_CodigoGrl20 RefProveedor { get; set; }

        [DataMember]
        public UDT_DescripTBase Descriptivo { get; set; }

        [DataMember]
        public UDT_Consecutivo SolicitudDocuID { get; set; }

        [DataMember]
        public UDT_Consecutivo SolicitudDetaID { get; set; }

        [AllowNull]
        [DataMember]
        public UDT_Consecutivo OrdCompraDocuID { get; set; }

        [AllowNull]
        [DataMember]
        public UDT_Consecutivo OrdCompraDetaID { get; set; }

        [AllowNull]
        [DataMember]
        public UDT_Consecutivo RecibidoDocuID { get; set; }

        [AllowNull]
        [DataMember]
        public UDT_Consecutivo RecibidoDetaID { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadSol { get; set; }

        [AllowNull]
        [DataMember]
        public UDT_Cantidad CantidadOC { get; set; }

        [AllowNull]
        [DataMember]
        public UDT_Cantidad CantidadRec { get; set; }

        [AllowNull]
        [DataMember]
        public UDT_Cantidad CantidadFact { get; set; }

        [DataMember]
        public UDT_Consecutivo FacturaDocuID { get; set; }

        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        public UDT_Valor ValorDet { get; set; }

        [DataMember]
        public string PrefDocOrig { get; set; }

        [DataMember]
        public DTO_ConsultaDocRelacion DetalleDocRelacion { get; set; }

        #endregion
    }

    /// <summary>
    /// Class Models DTO_ConsultaDocRelacion
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ConsultaDocRelacion
    {
        public DTO_ConsultaDocRelacion(IDataReader dr)
        {
            InitDetCols();
            try
            {
                this.PrefDoc.Value = dr["PrefDoc"].ToString();
                this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                this.OrdCompraDocuID.Value = Convert.ToInt32(dr["OrdCompraDocuID"]);
                this.RecibidoDocuID.Value = Convert.ToInt32(dr["RecibidoDocuID"]);
                this.FacturaDocuID.Value = Convert.ToInt32(dr["FacturaDocuID"]);
                this.ProveedorID.Value = dr["ProveedorID"].ToString();
                this.ProveedorNombre.Value = dr["ProveedorNombre"].ToString();
                this.Cantidad.Value = Convert.ToDecimal(dr["Cantidad"]);
                this.ValorUni.Value = Convert.ToDecimal(dr["ValorUni"]);
                this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                this.MonedaID.Value = dr["MonedaID"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DTO_ConsultaDocRelacion()
        {
            InitDetCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitDetCols()
        {
            this.PrefDoc = new UDT_DescripTBase();
            this.Fecha = new UDTSQL_datetime();
            this.OrdCompraDocuID = new UDT_Consecutivo();
            this.RecibidoDocuID = new UDT_Consecutivo();
            this.FacturaDocuID = new UDT_Consecutivo();
            this.ProveedorID = new UDT_ProveedorID();
            this.ProveedorNombre = new UDT_DescripTBase();
            this.Bodega = new UDT_DescripUnFormat();
            this.Cantidad = new UDT_Cantidad();
            this.ValorUni = new UDT_Valor();
            this.Valor = new UDT_Valor();
            this.MonedaID = new UDT_MonedaID();
            this.FacturaNro = new UDT_DescripTBase();
        }

        #region Propiedades

        [DataMember]
        public UDT_DescripTBase PrefDoc { get; set; }

        [DataMember]
        public UDTSQL_datetime Fecha { get; set; }

        [AllowNull]
        [DataMember]
        public UDT_Consecutivo OrdCompraDocuID { get; set; }

        [AllowNull]
        [DataMember]
        public UDT_Consecutivo RecibidoDocuID { get; set; }

        [AllowNull]
        [DataMember]
        public UDT_Consecutivo FacturaDocuID { get; set; }

        [AllowNull]
        [DataMember]
        public UDT_ProveedorID ProveedorID { get; set; }

        [AllowNull]
        [DataMember]
        public UDT_DescripTBase ProveedorNombre { get; set; }

        [DataMember]
        public UDT_DescripUnFormat Bodega { get; set; }

        [DataMember]
        public UDT_Cantidad Cantidad { get; set; }

        [DataMember]
        public UDT_Valor ValorUni { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_MonedaID MonedaID { get; set; }

        [DataMember]
        public UDT_DescripTBase FacturaNro { get; set; }

        #endregion
    }
}
       