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
    /// Models DTO_glDocumentoAprueba
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glDocumentoAprueba
    {
        #region DTO_glDocumentoAprueba

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glDocumentoAprueba(IDataReader dr)
        {
            InitCols();
            try
            {
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                if (!string.IsNullOrWhiteSpace(dr["UsuarioAprueba"].ToString()))
                    this.UsuarioAprueba.Value = Convert.ToInt32(dr["UsuarioAprueba"]);
                if (!string.IsNullOrWhiteSpace(dr["UsuarioAprueba1"].ToString()))
                    this.UsuarioAprueba1.Value = Convert.ToInt32(dr["UsuarioAprueba1"]);
                if (!string.IsNullOrWhiteSpace(dr["UsuarioAprueba2"].ToString()))
                    this.UsuarioAprueba2.Value = Convert.ToInt32(dr["UsuarioAprueba2"]);
                if (!string.IsNullOrWhiteSpace(dr["UsuarioAprueba3"].ToString()))
                    this.UsuarioAprueba3.Value = Convert.ToInt32(dr["UsuarioAprueba3"]);
                if (!string.IsNullOrWhiteSpace(dr["UsuarioAprueba4"].ToString()))
                    this.UsuarioAprueba4.Value = Convert.ToInt32(dr["UsuarioAprueba4"]);
                if (!string.IsNullOrWhiteSpace(dr["UsuarioAprueba5"].ToString()))
                    this.UsuarioAprueba5.Value = Convert.ToInt32(dr["UsuarioAprueba5"]);
                if (!string.IsNullOrWhiteSpace(dr["UsuarioAprueba6"].ToString()))
                    this.UsuarioAprueba6.Value = Convert.ToInt32(dr["UsuarioAprueba6"]);
                if (!string.IsNullOrWhiteSpace(dr["UsuarioAprueba7"].ToString()))
                    this.UsuarioAprueba7.Value = Convert.ToInt32(dr["UsuarioAprueba7"]);
                if (!string.IsNullOrWhiteSpace(dr["UsuarioAprueba8"].ToString()))
                    this.UsuarioAprueba8.Value = Convert.ToInt32(dr["UsuarioAprueba8"]);
                if (!string.IsNullOrWhiteSpace(dr["UsuarioAprueba9"].ToString()))
                    this.UsuarioAprueba9.Value = Convert.ToInt32(dr["UsuarioAprueba9"]);
                if (!string.IsNullOrWhiteSpace(dr["UsuarioAprueba10"].ToString()))
                    this.UsuarioAprueba10.Value = Convert.ToInt32(dr["UsuarioAprueba10"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaAprueba1"].ToString()))
                    this.FechaAprueba1.Value = Convert.ToDateTime(dr["FechaAprueba1"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaAprueba2"].ToString()))
                    this.FechaAprueba2.Value = Convert.ToDateTime(dr["FechaAprueba2"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaAprueba3"].ToString()))
                    this.FechaAprueba3.Value = Convert.ToDateTime(dr["FechaAprueba3"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaAprueba4"].ToString()))
                    this.FechaAprueba4.Value = Convert.ToDateTime(dr["FechaAprueba4"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaAprueba5"].ToString()))
                    this.FechaAprueba5.Value = Convert.ToDateTime(dr["FechaAprueba5"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaAprueba6"].ToString()))
                    this.FechaAprueba6.Value = Convert.ToDateTime(dr["FechaAprueba6"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaAprueba7"].ToString()))
                    this.FechaAprueba7.Value = Convert.ToDateTime(dr["FechaAprueba7"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaAprueba8"].ToString()))
                    this.FechaAprueba8.Value = Convert.ToDateTime(dr["FechaAprueba8"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaAprueba9"].ToString()))
                    this.FechaAprueba9.Value = Convert.ToDateTime(dr["FechaAprueba9"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaAprueba10"].ToString()))
                    this.FechaAprueba10.Value = Convert.ToDateTime(dr["FechaAprueba10"]);   
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glDocumentoAprueba()
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
            this.UsuarioAprueba = new UDT_seUsuarioID();
            this.UsuarioAprueba1 = new UDT_seUsuarioID();
            this.UsuarioAprueba2 = new UDT_seUsuarioID();
            this.UsuarioAprueba3 = new UDT_seUsuarioID();
            this.UsuarioAprueba4 = new UDT_seUsuarioID();
            this.UsuarioAprueba5 = new UDT_seUsuarioID();
            this.UsuarioAprueba6 = new UDT_seUsuarioID();
            this.UsuarioAprueba7 = new UDT_seUsuarioID();
            this.UsuarioAprueba8 = new UDT_seUsuarioID();
            this.UsuarioAprueba9 = new UDT_seUsuarioID();
            this.UsuarioAprueba10 = new UDT_seUsuarioID();
            this.FechaAprueba1 = new UDTSQL_datetime();
            this.FechaAprueba2 = new UDTSQL_datetime();
            this.FechaAprueba3 = new UDTSQL_datetime();
            this.FechaAprueba4 = new UDTSQL_datetime();
            this.FechaAprueba5 = new UDTSQL_datetime();
            this.FechaAprueba6 = new UDTSQL_datetime();
            this.FechaAprueba7 = new UDTSQL_datetime();
            this.FechaAprueba8 = new UDTSQL_datetime();
            this.FechaAprueba9 = new UDTSQL_datetime();
            this.FechaAprueba10 = new UDTSQL_datetime();
        }
        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_seUsuarioID UsuarioAprueba { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_seUsuarioID UsuarioAprueba1 { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_seUsuarioID UsuarioAprueba2 { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_seUsuarioID UsuarioAprueba3 { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_seUsuarioID UsuarioAprueba4 { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_seUsuarioID UsuarioAprueba5 { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_seUsuarioID UsuarioAprueba6 { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_seUsuarioID UsuarioAprueba7 { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_seUsuarioID UsuarioAprueba8 { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_seUsuarioID UsuarioAprueba9 { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_seUsuarioID UsuarioAprueba10 { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_datetime FechaAprueba1 { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_datetime FechaAprueba2 { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_datetime FechaAprueba3 { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_datetime FechaAprueba4 { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_datetime FechaAprueba5 { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_datetime FechaAprueba6 { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_datetime FechaAprueba7 { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_datetime FechaAprueba8 { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_datetime FechaAprueba9 { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_datetime FechaAprueba10 { get; set; }

        #endregion
    }
}
