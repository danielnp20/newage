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
    /// Class comprobante para aprobacion:
    /// Models DTO_prSolicitudAprobacion
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_prSolicitudAprobacion
    {
        #region DTO_prSolicitudAprobacion

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_prSolicitudAprobacion(IDataReader dr)
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
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_prSolicitudAprobacion()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.Aprobado = new UDT_SiNo();
            this.Rechazado = new UDT_SiNo();
            this.Observacion = new UDT_DescripTExt();
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
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_SiNo Aprobado { get; set; }

        [DataMember]
        public UDT_SiNo Rechazado { get; set; }

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }

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
        public UDT_AreaFuncionalID AreaAprobacion { get; set; }

        [DataMember]
        public UDT_UsuarioID UsuarioSolicita { get; set; }

        [DataMember]
        public UDTSQL_tinyint Prioridad { get; set; }

        [DataMember]
        public UDT_UsuarioID UsuarioID { get; set; }

        [DataMember]
        public string FileUrl { get; set; }

        #endregion
    }
}
