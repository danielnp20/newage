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
    /// Clase del reporte Recibidos
    /// </summary>
    public class DTO_ReportProveedoresRecibidos
    {
        /// <summary>
        /// Constructor con DataReader
        /// </summary>
        public DTO_ReportProveedoresRecibidos(IDataReader dr)
        {
            InitCols();
            try
            {
                this.PrefDoc.Value = dr["PrefDoc"].ToString();
                if (!string.IsNullOrEmpty(dr["FechaDoc"].ToString()))
                    this.FechaDoc.Value = Convert.ToDateTime(dr["FechaDoc"]);
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
                if (!string.IsNullOrEmpty(dr["CantidadRec"].ToString()))
                    this.CantidadRec.Value = Convert.ToDecimal(dr["CantidadRec"]);
                if (!string.IsNullOrEmpty(dr["Estado"].ToString()))
                    this.Estado.Value = Convert.ToByte(dr["Estado"]);
                this.CodigoBSID.Value = dr["CodigoBSID"].ToString();
                this.inReferenciaID.Value = dr["inReferenciaID"].ToString();
                this.UnidadInvID.Value = dr["UnidadInvID"].ToString();
                this.ProyectoID.Value = dr["ProyectoID"].ToString();
                this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();             
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Constructor con DataReader
        /// </summary>
        public DTO_ReportProveedoresRecibidos(IDataReader dr, bool isDetallado, bool isFacturdo)
        {
            InitCols();
            try
            {
                if (!string.IsNullOrEmpty(dr["Recibido"].ToString()))
                    this.Recibido.Value = dr["Recibido"].ToString();
                if (!string.IsNullOrEmpty(dr["FechaDoc"].ToString()))
                    this.FechaDoc.Value = Convert.ToDateTime(dr["FechaDoc"]);
                if (!string.IsNullOrEmpty(dr["Proveedor"].ToString()))
                    this.Proveedor.Value = dr["Proveedor"].ToString();
                if (!string.IsNullOrEmpty(dr["CantidadRecidida"].ToString()))
                    this.CantidadRec.Value = Convert.ToDecimal(dr["CantidadRecidida"]);
                this.BodegaDesc.Value = dr["BodegaDesc"].ToString();

                if (isDetallado)
                {
                    if (!string.IsNullOrEmpty(dr["OrdenCompra"].ToString()))
                        this.OrdenCompra.Value = dr["OrdenCompra"].ToString();
                    if (!string.IsNullOrEmpty(dr["CodigoBSID"].ToString()))
                        this.CodigoBSID.Value = dr["CodigoBSID"].ToString();
                    if (!string.IsNullOrEmpty(dr["Descriptivo"].ToString()))
                        this.Descriptivo.Value = dr["Descriptivo"].ToString();

                    if (isFacturdo)
                    {
                        if (!string.IsNullOrEmpty(dr["CentroCostoID"].ToString()))
                            this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                        if (!string.IsNullOrEmpty(dr["Valor"].ToString()))
                            this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                    }
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
        public DTO_ReportProveedoresRecibidos()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.PrefDoc = new UDTSQL_char(30);
            this.Recibido = new UDTSQL_char(30);
            this.FechaDoc = new UDTSQL_datetime();
            this.Estado = new UDTSQL_tinyint();
            this.Proveedor = new UDTSQL_char(100);
            this.CantidadRec = new UDT_Cantidad();
            this.BodegaDesc = new UDT_Descriptivo();
            this.Elaboro = new UDT_Descriptivo();
            this.Aprobo = new UDT_Descriptivo();

            //Columnas para reporte detallado
            this.OrdenCompra = new UDTSQL_char(30);
            this.CodigoBSID = new UDT_CodigoBSID();
            this.inReferenciaID = new UDT_inReferenciaID();
            this.UnidadInvID = new UDT_UnidadInvID();
            this.Descriptivo = new UDT_DescripTExt();

            //Columnas para reporte detallado No Facturado
            this.ProyectoID = new UDT_ProyectoID();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.LineaPresupuestoID = new UDT_LineaPresupuestoID();
            this.Valor = new UDT_Valor();
        }

        #region Propiedades

        [DataMember]
        public UDTSQL_char PrefDoc { get; set; }

        [DataMember]
        public UDTSQL_char Recibido { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaDoc { get; set; }

        [DataMember]
        public UDTSQL_tinyint Estado { get; set; }

        [DataMember]
        public UDTSQL_char Proveedor { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadRec { get; set; }

        [DataMember]
        public UDT_Descriptivo BodegaDesc { get; set; }

        [DataMember]
        public UDT_Descriptivo Elaboro { get; set; }

        [DataMember]
        public UDT_Descriptivo Aprobo { get; set; }

        //Propiedade para Reporte detallado

        [DataMember]
        public UDTSQL_char OrdenCompra { get; set; }

        [DataMember]
        public UDT_CodigoBSID CodigoBSID { get; set; }

        [DataMember]
        public UDT_DescripTExt Descriptivo { get; set; }

        //Propiedades para el Reporte detallado No facturado

        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        public UDT_LineaPresupuestoID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_inReferenciaID inReferenciaID { get; set; }

        [DataMember]
        public UDT_UnidadInvID UnidadInvID { get; set; }

        #endregion
    }

}
