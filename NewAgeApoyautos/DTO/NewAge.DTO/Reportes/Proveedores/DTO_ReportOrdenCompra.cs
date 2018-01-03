using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.DTO.Negocio.Reportes;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Reportes
{
    [DataContract]
    [Serializable]

    /// <summary>
    /// Clase del Ordenes de compra
    /// </summary>
    public class DTO_ReportOrdenCompra
    {
        /// <summary>
        /// Constructor con DataReader
        /// </summary>
        public DTO_ReportOrdenCompra(IDataReader dr, bool isDetallado)
        {
            InitCols();
            try
            {
                //Datos Generales
                if (!string.IsNullOrEmpty(dr["PrefijoID"].ToString()))
                    this.PrefijoID.Value = dr["PrefijoID"].ToString();
                if (!string.IsNullOrEmpty(dr["OrdenCompra"].ToString()))
                    this.OrdenCompra.Value = dr["OrdenCompra"].ToString();
                if (!string.IsNullOrEmpty(dr["FechaDoc"].ToString()))
                    this.FechaDoc.Value = Convert.ToDateTime(dr["FechaDoc"]);
                if (!string.IsNullOrEmpty(dr["Proveedor"].ToString()))
                    this.Proveedor.Value = dr["Proveedor"].ToString();
                if (!string.IsNullOrEmpty(dr["Valor"].ToString()))
                    this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                if (!string.IsNullOrEmpty(dr["Iva"].ToString()))
                    this.Iva.Value = Convert.ToDecimal(dr["Iva"]);

                //Datos Orden Compra Resumida
                if (!isDetallado)
                {
                    if (!string.IsNullOrEmpty(dr["AreaFuncional"].ToString()))
                        this.AreaFuncional.Value = dr["AreaFuncional"].ToString();
                    if (!string.IsNullOrEmpty(dr["FechaAprueba"].ToString()))
                        this.FechaAprueba.Value = Convert.ToDateTime(dr["FechaAprueba"]);
                    if (!string.IsNullOrEmpty(dr["usuarioAprueba"].ToString()))
                        this.usuarioAprueba.Value = dr["usuarioAprueba"].ToString();
                }

                //Datos Orden Compra Detallada
                else
                {
                    if (!string.IsNullOrEmpty(dr["ProyectoID"].ToString()))
                        this.ProyectoID.Value = dr["ProyectoID"].ToString();
                    if (!string.IsNullOrEmpty(dr["CodigoBSID"].ToString()))
                        this.CodigoBSID.Value = dr["CodigoBSID"].ToString();
                    if (!string.IsNullOrEmpty(dr["Descriptivo"].ToString()))
                        this.Descriptivo.Value = dr["Descriptivo"].ToString();
                    if (!string.IsNullOrEmpty(dr["CantidadSol"].ToString()))
                        this.CantidadSol.Value = Convert.ToDecimal(dr["CantidadSol"]);
                    if (!string.IsNullOrEmpty(dr["ValorUni"].ToString()))
                        this.ValorUni.Value = Convert.ToDecimal(dr["ValorUni"]);
                    if (!string.IsNullOrEmpty(dr["ValorTotML"].ToString()))
                        this.ValorTotML.Value = Convert.ToDecimal(dr["ValorTotML"]);
                    if (!string.IsNullOrEmpty(dr["ValorTotME"].ToString()))
                        this.ValorTotME.Value = Convert.ToDecimal(dr["ValorTotME"]);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportOrdenCompra()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Propiedades Genericas
            this.PrefijoID = new UDT_PrefijoID();
            this.OrdenCompra = new UDTSQL_char(20);
            this.FechaDoc = new UDTSQL_datetime();
            this.Proveedor = new UDTSQL_char(100);
            this.Valor = new UDT_Valor();
            this.Iva = new UDT_Valor();
            //Propiedades Orden Compra Resumida
            this.AreaFuncional = new UDTSQL_char(100);
            this.FechaAprueba = new UDTSQL_datetime();
            this.usuarioAprueba = new UDT_UsuarioID();
            //Propiedades Orden de Compra Detallada
            this.ProyectoID = new UDT_ProyectoID();
            this.CodigoBSID = new UDT_CodigoBSID();
            this.Descriptivo = new UDT_DescripTExt();
            this.CantidadSol = new UDT_Cantidad();
            this.ValorUni = new UDT_Valor();
            this.ValorTotML = new UDT_Valor();
            this.ValorTotME = new UDT_Valor();
        }

        #region Propiedades

        //Propiedades Generiacas
        [DataMember]
        public UDT_PrefijoID PrefijoID { get; set; }

        [DataMember]
        public UDTSQL_char OrdenCompra { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaDoc { get; set; }

        [DataMember]
        public UDTSQL_char Proveedor { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_Valor Iva { get; set; }

        //Propiedades Orden Compra Resumida
        [DataMember]
        public UDTSQL_char AreaFuncional { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaAprueba { get; set; }

        [DataMember]
        public UDT_UsuarioID usuarioAprueba { get; set; }

        //Propiedades Orden Compra Detallada
        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        public UDT_CodigoBSID CodigoBSID { get; set; }

        [DataMember]
        public UDT_DescripTExt Descriptivo { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadSol { get; set; }

        [DataMember]
        public UDT_Valor ValorUni { get; set; }

        [DataMember]
        public UDT_Valor ValorTotML { get; set; }

        [DataMember]
        public UDT_Valor ValorTotME { get; set; }

        #endregion

    }

}
