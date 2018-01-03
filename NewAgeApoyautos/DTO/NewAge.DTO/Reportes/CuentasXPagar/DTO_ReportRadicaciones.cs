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
    public class DTO_ReportRadicaciones
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ReportRadicaciones(IDataReader dr)
        {
            this.InitCols();
            try
            {
                if (!string.IsNullOrEmpty(dr["Item"].ToString()))
                    this.Item.Value = Convert.ToInt32(dr["Item"]);
                if (!string.IsNullOrEmpty(dr["RadicaFecha"].ToString()))
                    this.RadicaFecha.Value = Convert.ToDateTime(dr["RadicaFecha"]);
                if (!string.IsNullOrEmpty(dr["TerceroId"].ToString()))
                    this.TerceroId.Value = dr["TerceroId"].ToString();
                if (!string.IsNullOrEmpty(dr["NombreTer"].ToString()))
                    this.NombreTer.Value = dr["NombreTer"].ToString();
                if (!string.IsNullOrEmpty(dr["DocumentoTercero"].ToString()))
                    this.DocumentoTercero.Value = dr["DocumentoTercero"].ToString();
                if (!string.IsNullOrEmpty(dr["FacturaFecha"].ToString()))
                    this.FacturaFecha.Value = Convert.ToDateTime(dr["FacturaFecha"]);
                if (!string.IsNullOrEmpty(dr["Valor"].ToString()))
                    this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                if (!string.IsNullOrEmpty(dr["Iva"].ToString()))
                    this.Iva.Value = Convert.ToDecimal(dr["Iva"]);
                if (!string.IsNullOrEmpty(dr["Total"].ToString()))
                    this.Total.Value = Convert.ToDecimal(dr["Total"]);
                if (!string.IsNullOrEmpty(dr["Estado"].ToString()))
                    this.Estado.Value = dr["Estado"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportRadicaciones()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Item = new UDTSQL_int();
            this.RadicaFecha = new UDTSQL_datetime();
            this.TerceroId = new UDT_TerceroID();
            this.NombreTer=new UDT_Descriptivo();
            this.DocumentoTercero = new UDTSQL_char(20);
            this.FacturaFecha = new UDTSQL_datetime();
            this.Valor = new UDT_Valor();
            this.Iva = new UDT_Valor();
            this.Total = new UDT_Valor();
            this.Estado = new UDTSQL_char(10);
        }
        #endregion

        [DataMember]
        public  UDTSQL_int Item{ get; set; }

        [DataMember]
        public UDTSQL_datetime RadicaFecha { get; set; }

        [DataMember]
        public UDTSQL_char DocumentoTercero { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroId { get; set; }

        [DataMember]
        public UDT_Descriptivo NombreTer { get; set; }

        [DataMember]
        public UDTSQL_datetime FacturaFecha { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_Valor Iva { get; set; }

        [DataMember]
        public UDT_Valor Total { get; set; }

        [DataMember]
        public UDTSQL_char Estado { get; set; }
    }
}
