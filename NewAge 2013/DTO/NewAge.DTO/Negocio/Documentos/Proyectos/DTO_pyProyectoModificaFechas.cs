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
    /// Models DTO_pyProyectoModificaFechas
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_pyProyectoModificaFechas
    {
        #region DTO_pyProyectoModificaFechas

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_pyProyectoModificaFechas(IDataReader dr)
        {
            InitCols();
            try
            {
                //              this.DescMod.Value = dr["DescMod"].ToString();  

                if (!string.IsNullOrWhiteSpace(dr["NumeroDoc"].ToString()))
                    this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                if (!string.IsNullOrWhiteSpace(dr["TareaID"].ToString()))
                    this.TareaID.Value = (dr["TareaID"]).ToString();
                if (!string.IsNullOrWhiteSpace(dr["ProyectoID"].ToString()))
                    this.ProyectoID.Value = Convert.ToString(dr["ProyectoID"]);
                if (!string.IsNullOrWhiteSpace(dr["TipoAjuste"].ToString()))
                    this.TipoAjuste.Value = Convert.ToByte(dr["TipoAjuste"]);
                if (!string.IsNullOrWhiteSpace(dr["Codigo"].ToString()))
                    this.Codigo.Value = (dr["Codigo"]).ToString();
                if (!string.IsNullOrWhiteSpace(dr["FechaActual"].ToString()))
                    this.FechaActual.Value = Convert.ToDateTime(dr["FechaActual"]);
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
        public DTO_pyProyectoModificaFechas()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.TareaID = new UDT_CodigoGrl10();
            this.ProyectoID = new UDT_ProyectoID();
            this.TipoAjuste = new UDTSQL_tinyint();
            this.Codigo = new UDT_CodigoGrl();
            //          this.DescMod = new UDT_Descriptivo();
            this.FechaActual = new UDTSQL_smalldatetime();
            this.FechaNueva = new UDTSQL_smalldatetime();
            this.UsuarioDigita = new UDT_UsuarioID();
            this.FechaDigita = new UDTSQL_smalldatetime();
            this.UsuarioAprueba = new UDT_UsuarioID();
            this.FechaAprueba = new UDTSQL_smalldatetime();
            this.ApruebaInd = new UDT_SiNo();
            this.Observaciones = new UDT_DescripTExt();
            this.Consecutivo = new UDT_Consecutivo();
        }
        #endregion

        #region Propiedades


        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }
        [DataMember]
        public UDT_CodigoGrl10 TareaID { get; set; }
        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }
        [DataMember]
        public UDTSQL_tinyint TipoAjuste { get; set; }
        [DataMember]
        public UDT_CodigoGrl Codigo { get; set; }

        //[DataMember]
        //public UDT_Descriptivo DescMod { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaActual { get; set; }
        [DataMember]
        public UDTSQL_smalldatetime FechaNueva { get; set; }
        [DataMember]
        public UDT_UsuarioID UsuarioDigita { get; set; }
        [DataMember]
        public UDTSQL_smalldatetime FechaDigita { get; set; }
        [DataMember]
        public UDT_UsuarioID UsuarioAprueba { get; set; }
        [DataMember]
        public UDTSQL_smalldatetime FechaAprueba { get; set; }
        [DataMember]
        public UDT_SiNo ApruebaInd { get; set; }
        [DataMember]
        public UDT_DescripTExt Observaciones { get; set; }
        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }
        #endregion
    }
}
