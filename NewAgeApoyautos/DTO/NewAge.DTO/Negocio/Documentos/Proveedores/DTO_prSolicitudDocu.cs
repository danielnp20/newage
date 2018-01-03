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
    /// Models DTO_prSolicitudDocu
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_prSolicitudDocu
    {
        #region DTO_prSolicitudDocu

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_prSolicitudDocu(IDataReader dr)
        {
            InitCols();
            try
            {
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                if (!string.IsNullOrEmpty(dr["Ano"].ToString()))
                    this.Ano.Value = Convert.ToInt32(dr["Ano"]);
                if (!string.IsNullOrEmpty(dr["FechaEntrega"].ToString()))
                    this.FechaEntrega.Value = Convert.ToDateTime(dr["FechaEntrega"]);
                this.LugarEntrega.Value = dr["LugarEntrega"].ToString();
                this.AreaAprobacion.Value = dr["AreaAprobacion"].ToString();
                if (!string.IsNullOrEmpty(dr["Prioridad"].ToString()))
                    this.Prioridad.Value = Convert.ToByte(dr["Prioridad"]);
                this.UsuarioSolicita.Value = dr["UsuarioSolicita"].ToString();
                if (!string.IsNullOrEmpty(dr["Destino"].ToString()))
                    this.Destino.Value = Convert.ToByte(dr["Destino"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_prSolicitudDocu()
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
            this.Ano = new UDTSQL_int();
            this.FechaEntrega = new UDTSQL_datetime();
            this.LugarEntrega = new UDT_LocFisicaID();
            this.AreaAprobacion = new UDT_AreaFuncionalID();
            this.Prioridad = new UDTSQL_tinyint();
            this.UsuarioSolicita = new UDT_UsuarioID();
            this.Destino = new UDTSQL_tinyint();
        }
        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDTSQL_int Ano { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaEntrega { get; set; }
        
        [DataMember]
        public UDT_LocFisicaID LugarEntrega { get; set; }

        [DataMember]
        public UDT_AreaFuncionalID AreaAprobacion { get; set; }
      
        [DataMember]
        public UDTSQL_tinyint Prioridad { get; set; }
              
        [DataMember]
        public UDT_UsuarioID UsuarioSolicita { get; set; }

        [DataMember]
        public UDTSQL_tinyint Destino { get; set; }
        #endregion
    }
}
