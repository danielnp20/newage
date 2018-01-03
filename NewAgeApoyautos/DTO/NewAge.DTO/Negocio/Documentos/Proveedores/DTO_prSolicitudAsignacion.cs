using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class comprobante para aprobacion:
    /// Models DTO_prSolicitudAprobacion
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_prSolicitudAsignacion
    {
        #region DTO_prSolicitudAsignacion

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_prSolicitudAsignacion(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.PeriodoID.Value = Convert.ToDateTime(dr["PeriodoID"]);
                this.DocumentoID.Value = Convert.ToInt32(dr["DocumentoID"]);
                this.PrefijoID.Value = dr["PrefijoID"].ToString();
                this.DocumentoNro.Value = Convert.ToInt32(dr["DocumentoNro"]);
                this.ObservacionDoc.Value = dr["ObservacionDoc"].ToString();
                this.FechaEntrega.Value = Convert.ToDateTime(dr["FechaEntrega"]);
                this.LugarEntrega.Value = dr["LugarEntrega"].ToString();
                this.AreaAprobacion.Value = dr["AreaAprobacion"].ToString();
                this.UsuarioSolicita.Value = dr["UsuarioSolicita"].ToString();
                this.Prioridad.Value = Convert.ToByte(dr["Prioridad"]);
                this.UsuarioID.Value = dr["UsuarioID"].ToString();
                this.Asignado.Value = false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_prSolicitudAsignacion()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.Asignado = new UDT_SiNo();
            this.NumeroDoc = new UDT_Consecutivo();
            this.PeriodoID = new UDT_PeriodoID();
            this.DocumentoID = new UDT_DocumentoID();
            this.PrefijoID = new UDT_PrefijoID();
            this.DocumentoNro = new UDT_Consecutivo();
            this.ObservacionDoc = new UDT_DescripTExt();
            this.FechaEntrega = new UDTSQL_datetime();
            this.LugarEntrega = new UDT_LocFisicaID();
            this.AreaAprobacion = new UDT_AreaFuncionalID();
            this.UsuarioSolicita = new UDT_UsuarioID();
            this.Prioridad = new UDTSQL_tinyint();
            this.UsuarioID = new UDT_UsuarioID();
            this.FileUrl = "";
            this.SolicitudAsignDet = new List<DTO_prSolicitudAsignDet>();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_SiNo Asignado { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_PeriodoID PeriodoID { get; set; }

        [DataMember]
        public UDT_DocumentoID DocumentoID { get; set; }

        [DataMember]
        public UDT_PrefijoID PrefijoID { get; set; }
        
        [DataMember]
        public UDT_Consecutivo DocumentoNro { get; set; }

        [DataMember]
        public UDT_DescripTExt ObservacionDoc { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaEntrega { get; set; }

        [DataMember]
        public UDT_LocFisicaID LugarEntrega { get; set; }

        [DataMember]
        [Filtrable]
        public UDT_AreaFuncionalID AreaAprobacion { get; set; }

        [DataMember]
        public UDT_UsuarioID UsuarioSolicita { get; set; }

        [DataMember]
        public UDTSQL_tinyint Prioridad { get; set; }

        [DataMember]
        public UDT_UsuarioID UsuarioID { get; set; }

        [DataMember]
        public string FileUrl { get; set; }

        [DataMember]
        public List<DTO_prSolicitudAsignDet> SolicitudAsignDet { get; set; }
        #endregion
    }

    public class DTO_prSolicitudAsignDet
    {
        public DTO_prSolicitudAsignDet(IDataReader dr)
        {
            InitDetCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.ConsecutivoDetaID.Value = Convert.ToInt32(dr["ConsecutivoDetaID"]);
                this.CodigoBSID.Value = dr["CodigoBSID"].ToString();
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
                this.inReferenciaID.Value = dr["inReferenciaID"].ToString(); ;
                if (!string.IsNullOrWhiteSpace(dr["EstadoInv"].ToString()))
                    this.EstadoInv.Value = Convert.ToByte(dr["EstadoInv"]);
                this.Parametro1.Value = dr["Parametro1"].ToString();
                this.Parametro2.Value = dr["Parametro2"].ToString();
                this.UnidadInvID.Value = dr["UnidadInvID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["CantidadSol"].ToString()))
                    this.CantidadSol.Value = Convert.ToDecimal(dr["CantidadSol"]);
                this.DatoAdd1.Value = dr["DatoAdd1"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DTO_prSolicitudAsignDet()
        {
            InitDetCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitDetCols()
        {
            this.Asignado = new UDT_SiNo();
            this.NumeroDoc = new UDT_Consecutivo();
            this.ConsecutivoDetaID = new UDT_Consecutivo();
            this.CodigoBSID = new UDT_CodigoBSID();
            this.Descriptivo = new UDT_DescripTExt();
            this.inReferenciaID = new UDT_inReferenciaID();
            this.EstadoInv = new UDTSQL_tinyint();
            this.Parametro1 = new UDT_ParametroInvID();
            this.Parametro2 = new UDT_ParametroInvID();
            this.UnidadInvID = new UDT_UnidadInvID();
            this.CantidadSol = new UDT_Cantidad();
            this.DatoAdd1 = new UDTSQL_char(20);
            this.SolicitudCargos = new List<DTO_prSolicitudCargos>();
        }

        #region Propiedades

        [DataMember]
        public UDT_SiNo Asignado { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_Consecutivo ConsecutivoDetaID { get; set; }

        [DataMember]
        [Filtrable]
        public UDT_CodigoBSID CodigoBSID { get; set; }

        [DataMember]
        public UDT_DescripTExt Descriptivo { get; set; }

        [DataMember]
        [AllowNull]
        [Filtrable]
        public UDT_inReferenciaID inReferenciaID { get; set; }

        [DataMember]
        public UDTSQL_tinyint EstadoInv { get; set; }

        [DataMember]
        public UDT_ParametroInvID Parametro1 { get; set; }

        [DataMember]
        public UDT_ParametroInvID Parametro2 { get; set; }

        [DataMember]
        public UDT_UnidadInvID UnidadInvID { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadSol { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd1 { get; set; }

        [DataMember]
        public List<DTO_prSolicitudCargos> SolicitudCargos { get; set; }
        #endregion
    }
}
