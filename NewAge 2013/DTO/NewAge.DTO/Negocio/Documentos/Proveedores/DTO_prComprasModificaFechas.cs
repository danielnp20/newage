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
    /// Models DTO_prComprasModificaFechas
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_prComprasModificaFechas
    {
        #region DTO_prComprasModificaFechas

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_prComprasModificaFechas(IDataReader dr)
        {
            InitCols();
            try
            {
                if (!string.IsNullOrWhiteSpace(dr["NumeroDoc"].ToString())) 
                    this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.Codigo.Value = (dr["Codigo"]).ToString();
                if (!string.IsNullOrWhiteSpace(dr["ProyectoID"].ToString())) 
                    this.ProyectoID.Value = Convert.ToString(dr["ProyectoID"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaEntrega"].ToString())) 
                    this.FechaEntrega.Value = Convert.ToDateTime(dr["FechaEntrega"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaNueva"].ToString()))
                    this.FechaNueva.Value = Convert.ToDateTime(dr["FechaNueva"]);
                if (!string.IsNullOrWhiteSpace(dr["UsuarioDigita"].ToString()))
                    this.UsuarioDigita.Value = Convert.ToString(dr["UsuarioDigita"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaDigita"].ToString())) 
                    this.FechaDigita.Value = Convert.ToDateTime(dr["FechaDigita"]);
                if (!string.IsNullOrWhiteSpace(dr["UsuarioAprueba"].ToString())) 
                    this.UsuarioAprueba.Value = Convert.ToString(dr["UsuarioAprueba"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaAprueba"].ToString())) 
                    this.FechaAprueba.Value = Convert.ToDateTime(dr["FechaAprueba"]);
                if (!string.IsNullOrWhiteSpace(dr["ApruebaInd"].ToString())) 
                    this.ApruebaInd.Value = Convert.ToBoolean(dr["ApruebaInd"]);
                if (!string.IsNullOrWhiteSpace(dr["AprobadoDocInd"].ToString()))
                    this.AprobadoDocInd.Value = Convert.ToBoolean(dr["AprobadoDocInd"]);
                if (!string.IsNullOrWhiteSpace(dr["Observaciones"].ToString()))
                    this.Observaciones.Value = Convert.ToString(dr["Observaciones"]);
                if (!string.IsNullOrWhiteSpace(dr["Consecutivo"].ToString())) 
                    this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
            }
            catch (Exception e)
            {
                ;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_prComprasModificaFechas()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.ProyectoID = new UDT_ProyectoID();
            this.Codigo = new UDT_CodigoGrl10();
            this.FechaEntrega = new UDTSQL_smalldatetime();
            this.FechaNueva = new UDTSQL_smalldatetime();
            this.UsuarioDigita = new UDT_UsuarioID();
            this.FechaDigita = new UDTSQL_smalldatetime();
            this.UsuarioAprueba = new UDT_UsuarioID();
            this.FechaAprueba = new UDTSQL_smalldatetime();
            this.ApruebaInd = new UDT_SiNo();
            this.AprobadoDocInd = new UDT_SiNo();
            this.Observaciones = new UDT_DescripTExt();
            this.Consecutivo = new UDT_Consecutivo();
        }
        #endregion

        #region Propiedades

        
        [DataMember]
        public  UDT_Consecutivo NumeroDoc{get;set;}
        [DataMember]
        public UDT_CodigoGrl10 Codigo { get; set; }
        [DataMember]
        public  UDT_ProyectoID ProyectoID{get;set;}
        [DataMember]
        public  UDTSQL_smalldatetime FechaEntrega{get;set;}
        [DataMember]
        public UDTSQL_smalldatetime FechaNueva { get; set; }
        [DataMember]
        public  UDT_UsuarioID UsuarioDigita{get;set;}
        [DataMember]
        public UDTSQL_smalldatetime FechaDigita { get; set; }
        [DataMember]
        public  UDT_UsuarioID UsuarioAprueba{get;set;}
        [DataMember]
        public UDTSQL_smalldatetime FechaAprueba { get; set; }
        [DataMember]
        public UDT_SiNo ApruebaInd { get; set; }
        [DataMember]
        public UDT_SiNo AprobadoDocInd { get; set; }
        [DataMember]
        public  UDT_DescripTExt Observaciones{get;set;}
        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }
        #endregion
    }
}
