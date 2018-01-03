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
    /// Clase del reporte Comrprobante Control
    /// </summary>
    public class DTO_ReportProveedoresSolicitudes 
    {
        /// <summary>
        /// Constructor con DataReader
        /// </summary>
        public DTO_ReportProveedoresSolicitudes(IDataReader dr)
        {
            InitCols();
            try
            {
                this.PrefDoc.Value = dr["PrefDoc"].ToString();
                if (!string.IsNullOrEmpty(dr["FechaDoc"].ToString()))
                    this.FechaDoc.Value = Convert.ToDateTime(dr["FechaDoc"]);
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
                if (!string.IsNullOrEmpty(dr["CantidadSol"].ToString()))
                    this.CantidadSol.Value = Convert.ToDecimal(dr["CantidadSol"]);
                if (!string.IsNullOrEmpty(dr["CantidadOC"].ToString()))
                    this.CantidadOC.Value = Convert.ToDecimal(dr["CantidadOC"]);
                if (!string.IsNullOrEmpty(dr["CantidadPendiente"].ToString()))
                    this.CantidadPendiente.Value = Convert.ToDecimal(dr["CantidadPendiente"]);
                this.Estado.Value = dr["Estado"].ToString();              
                this.UsuarioSolicita.Value = dr["UsuarioSolicita"].ToString();
                this.LugarEntrega.Value = dr["LugarEntrega"].ToString();
                if (!string.IsNullOrEmpty(dr["OrdCompraDocuID"].ToString()))
                    this.OrdCompraDocuID.Value = Convert.ToInt32(dr["OrdCompraDocuID"]);
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
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportProveedoresSolicitudes()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.PrefDoc = new UDTSQL_char(30);
            this.FechaDoc = new UDTSQL_datetime();
            this.Descriptivo = new UDT_Descriptivo();
            this.CantidadSol = new UDTSQL_numeric();
            this.CantidadOC = new UDTSQL_numeric();
            this.CantidadPendiente = new UDTSQL_numeric();
            this.Estado = new UDTSQL_char(15);
            this.UsuarioPreaprueba = new UDT_UsuarioID();
            this.UsuarioSolicita = new UDT_UsuarioID();
            this.GerenAprueba = new UDT_UsuarioID();
            this.FechaPreaprueba = new UDTSQL_datetime();
            this.UsuarioAprueba = new UDT_UsuarioID();
            this.fechaAprobacion = new UDTSQL_datetime();
            this.ProyectoID = new UDT_ProyectoID();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.LineaPresupuestoID = new UDT_LineaPresupuestoID();
            this.LugarEntrega = new UDT_LocFisicaID();
            this.OrdCompraDocuID = new UDT_Consecutivo();
            this.CodigoBSID = new UDT_CodigoBSID();
            this.inReferenciaID = new UDT_inReferenciaID();
            this.UnidadInvID = new UDT_UnidadInvID();
        }

        #region Propiedades

        [DataMember]
        public UDTSQL_char PrefDoc { get; set; }
        
        [DataMember]
        public UDTSQL_datetime FechaDoc { get; set; }

        [DataMember]
        public UDT_Descriptivo Descriptivo { get; set; }

        [DataMember]
        public UDTSQL_numeric CantidadSol { get; set; }

        [DataMember]
        public UDTSQL_numeric CantidadOC { get; set; }

        [DataMember]
        public UDTSQL_numeric CantidadPendiente { get; set; }

        [DataMember]
        public UDTSQL_char Estado { get; set; }

        [DataMember]
        public UDT_UsuarioID UsuarioPreaprueba { get; set; }

        [DataMember]
        public UDT_UsuarioID UsuarioSolicita { get; set; }

        [DataMember]
        public UDT_UsuarioID GerenAprueba { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaPreaprueba { get; set; }

        [DataMember]
        public UDT_UsuarioID UsuarioAprueba { get; set; }

        [DataMember]
        public UDTSQL_datetime fechaAprobacion { get; set; }

        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        public UDT_LineaPresupuestoID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_LocFisicaID LugarEntrega { get; set; }

        [DataMember]
        public UDT_Consecutivo OrdCompraDocuID { get; set; }

        [DataMember]
        public UDT_CodigoBSID CodigoBSID { get; set; }

        [DataMember]
        public UDT_inReferenciaID inReferenciaID { get; set; }

        [DataMember]
        public UDT_UnidadInvID UnidadInvID { get; set; }

        #endregion

    }

}
