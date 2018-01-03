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
    public class DTO_ccPagaduriaIncoporacion
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccPagaduriaIncoporacion(IDataReader dr)
        {
            this.InitCols();
            try
            {
                if (!string.IsNullOrEmpty(dr["FechaDoc"].ToString()))
                    this.FechaDoc.Value = Convert.ToDateTime(dr["FechaDoc"]);
                if (!string.IsNullOrEmpty(dr["CodigoPagaduria"].ToString()))
                    this.CodigoPagaduria.Value = dr["CodigoPagaduria"].ToString();
                if (!string.IsNullOrEmpty(dr["Pagaduria"].ToString()))
                    this.Pagaduria.Value = dr["Pagaduria"].ToString();
                if (!string.IsNullOrEmpty(dr["Grado"].ToString()))
                    this.Grado.Value = dr["Grado"].ToString();
                if (!string.IsNullOrEmpty(dr["CodigoMilitar"].ToString()))
                    this.CodigoMilitar.Value = dr["CodigoMilitar"].ToString();
                if (!string.IsNullOrEmpty(dr["Tercero"].ToString()))
                    this.Tercero.Value = dr["Tercero"].ToString();
                if (!string.IsNullOrEmpty(dr["Nombre"].ToString()))
                    this.Nombre.Value = dr["Nombre"].ToString();
                if (!string.IsNullOrEmpty(dr["Libranza"].ToString()))
                    this.Libranza.Value = Convert.ToInt32(dr["Libranza"]);
                if (!string.IsNullOrEmpty(dr["FechaLibranza"].ToString()))
                    this.FechaLibranza.Value = Convert.ToDateTime(dr["FechaLibranza"]);
                if (!string.IsNullOrEmpty(dr["Cuota"].ToString()))
                    this.Cuota.Value = Convert.ToDecimal(dr["Cuota"]);
                if (!string.IsNullOrEmpty(dr["FechaInicio"].ToString()))
                    this.FechaInicio.Value = Convert.ToDateTime(dr["FechaInicio"]);
                if (!string.IsNullOrEmpty(dr["FechaTerminacion"].ToString()))
                    this.FechaTerminacion.Value = Convert.ToDateTime(dr["FechaTerminacion"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccPagaduriaIncoporacion()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.FechaDoc = new UDTSQL_datetime();
            this.CodigoPagaduria = new UDT_PagaduriaID();
            this.Pagaduria = new UDT_DescripTBase();
            this.Grado = new UDTSQL_char(25);
            this.CodigoMilitar = new UDTSQL_char(12);
            this.Tercero = new UDT_TerceroID();
            this.Nombre = new UDT_Descriptivo();
            this.Libranza = new UDTSQL_int();
            this.FechaLibranza = new UDTSQL_smalldatetime();
            this.Cuota = new UDT_Valor();
            this.FechaInicio = new UDTSQL_datetime();
            this.FechaTerminacion = new UDTSQL_datetime();
        }
        #endregion

        #region Propiedades

        [DataMember]
        public UDTSQL_datetime FechaDoc { get; set; }

        [DataMember]
        public UDT_PagaduriaID CodigoPagaduria { get; set; }

        [DataMember]
        public UDT_DescripTBase Pagaduria { get; set; }

        [DataMember]
        public UDTSQL_char Grado { get; set; }

        [DataMember]
        public UDTSQL_char CodigoMilitar { get; set; }

        [DataMember]
        public UDT_TerceroID Tercero { get; set; }

        [DataMember]
        public UDT_Descriptivo Nombre { get; set; }

        [DataMember]
        public UDTSQL_int Libranza { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaLibranza { get; set; }

        [DataMember]
        public UDT_Valor Cuota { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaInicio { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaTerminacion { get; set; }

        #endregion
    }
}
