using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_ccReferenciacion
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccReferenciacion(IDataReader dr)
        {
            this.InitCols();
            try
            {
                if (!string.IsNullOrEmpty(dr["Libranza"].ToString()))
                    this.Libranza.Value = Convert.ToInt32(dr["Libranza"]);
                if (!string.IsNullOrEmpty(dr["ClienteID"].ToString()))
                    this.ClienteID.Value = dr["ClienteID"].ToString();
                if (!string.IsNullOrEmpty(dr["FechaDoc"].ToString()))
                    this.FechaDoc.Value = Convert.ToDateTime(dr["FechaDoc"]);
                if (!string.IsNullOrEmpty(dr["Nombre"].ToString()))
                    this.Nombre.Value = dr["Nombre"].ToString();
                if (!string.IsNullOrEmpty(dr["Telefono"].ToString()))
                    this.Telefono.Value = dr["Telefono"].ToString();
                if (!string.IsNullOrEmpty(dr["Direccion"].ToString()))
                    this.Direccion.Value = dr["Direccion"].ToString();
                if (!string.IsNullOrEmpty(dr["Correo"].ToString()))
                    this.Correo.Value = dr["Correo"].ToString();
                if (!string.IsNullOrEmpty(dr["Pregunta"].ToString()))
                    this.Pregunta.Value = dr["Pregunta"].ToString();
                if (!string.IsNullOrEmpty(dr["Observaciones"].ToString()))
                    this.Observaciones.Value = dr["Observaciones"].ToString();
                if (!string.IsNullOrEmpty(dr["TipoReferencia"].ToString()))
                    this.TipoReferencia.Value = dr["TipoReferencia"].ToString();
                if (!string.IsNullOrEmpty(dr["PersonaConsulta"].ToString()))
                    this.PersonaConsulta.Value = dr["PersonaConsulta"].ToString();
                if (!string.IsNullOrEmpty(dr["RelacionTitular"].ToString()))
                    this.RelacionTitular.Value = dr["RelacionTitular"].ToString();
                if (!string.IsNullOrEmpty(dr["nombreCliente"].ToString()))
                    this.nombreCliente.Value = dr["nombreCliente"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccReferenciacion()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Libranza = new UDT_LibranzaID();
            this.ClienteID = new UDT_ClienteID();
            this.FechaDoc = new UDTSQL_smalldatetime();
            this.Nombre = new UDTSQL_char(200);
            this.Telefono = new UDTSQL_char(50);
            this.Direccion = new UDTSQL_char(200);
            this.Correo = new UDTSQL_char(80);
            this.Pregunta = new UDT_DescripTBase();
            this.Observaciones = new UDT_DescripTExt();
            this.TipoReferencia = new UDTSQL_char(12);
            this.PersonaConsulta = new UDTSQL_char(50);
            this.RelacionTitular = new UDTSQL_char(50);
            this.nombreCliente = new UDTSQL_char(70);
        }
        #endregion

        #region Propiedades
        [DataMember]
        public UDT_LibranzaID Libranza { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaDoc { get; set; }

        [DataMember]
        public UDTSQL_char Nombre { get; set; }

        [DataMember]
        public UDTSQL_char Telefono { get; set; }

        [DataMember]
        public UDTSQL_char Direccion { get; set; }

        [DataMember]
        public UDTSQL_char Correo { get; set; }

        [DataMember]
        public UDT_DescripTBase Pregunta { get; set; }

        [DataMember]
        public UDT_DescripTExt Observaciones { get; set; }

        [DataMember]
        public UDTSQL_char TipoReferencia { get; set; }

        [DataMember]
        public UDTSQL_char PersonaConsulta { get; set; }

        [DataMember]
        public UDTSQL_char RelacionTitular { get; set; }

        [DataMember]
        public UDTSQL_char nombreCliente { get; set; }
        #endregion
    }
}
