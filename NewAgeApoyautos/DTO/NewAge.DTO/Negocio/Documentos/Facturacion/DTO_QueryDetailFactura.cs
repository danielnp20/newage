using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.Attributes;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using NewAge.DTO.Negocio.Reportes;
using NewAge.DTO.Reportes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_QueryDetailFactura
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_QueryDetailFactura : DTO_BasicReport
    {
        #region Constructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_QueryDetailFactura(IDataReader dr)
        {
            InitCols();
            try
            {
                if (!string.IsNullOrWhiteSpace(dr["IdentificadorTR"].ToString()))
                    this.IdentificadorTr.Value = Convert.ToInt32(dr["IdentificadorTR"]);
                if (!string.IsNullOrWhiteSpace(dr["NumeroDoc"].ToString()))
                    this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                if (!string.IsNullOrWhiteSpace(dr["DocumentoTipo"].ToString()))
                    this.DocumentoTipo.Value = Convert.ToInt32(dr["DocumentoTipo"]);
                if (!string.IsNullOrWhiteSpace(dr["DocumentoCom"].ToString()))
                    this.DocumentoCom.Value = dr["DocumentoCom"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["ComprobanteNro"].ToString()))
                    this.ComprobanteNro.Value = Convert.ToInt32(dr["ComprobanteNro"]);
                if (!string.IsNullOrWhiteSpace(dr["ComprobanteID"].ToString()))
                    this.ComprobanteID.Value = dr["ComprobanteID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["Fecha"].ToString()))
                    this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                if (!string.IsNullOrWhiteSpace(dr["vlrMdaLoc"].ToString()))
                    this.vlrMdaLoc.Value = Convert.ToDecimal(dr["vlrMdaLoc"]);
                if (!string.IsNullOrWhiteSpace(dr["Estado"].ToString()))
                    this.Estado.Value = Convert.ToInt16(dr["Estado"]); 
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_QueryDetailFactura()
        {
            InitCols();
        }

        // Inicializa las columnas
        private void InitCols()
        {
            this.ComprobanteID = new UDT_ComprobanteID();
            this.ComprobanteNro = new UDT_Consecutivo();
            this.DocumentoCom = new UDTSQL_char(20);
            this.Fecha = new UDTSQL_datetime();
            this.vlrMdaLoc = new UDT_Valor();
            this.DocumentoTipo = new UDTSQL_int();
            this.NumeroDoc = new UDT_Consecutivo();
            this.IdentificadorTr = new UDT_Consecutivo();
            this.Banco = new UDT_Descriptivo();
            this.Estado = new UDTSQL_smallint();

        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_Consecutivo IdentificadorTr { get; set; }

        [DataMember]
        public UDTSQL_int DocumentoTipo { get; set; }

        [DataMember]
        public UDTSQL_char DocumentoCom { get; set; }

        [DataMember]
        public UDTSQL_datetime Fecha { get; set; }

        [DataMember]
        public UDT_ComprobanteID ComprobanteID { get; set; }

        [DataMember]
        public UDT_Consecutivo ComprobanteNro { get; set; }

        [DataMember]
        public UDT_Valor vlrMdaLoc { get; set; }

        //Extra
        [DataMember]
        public UDT_Descriptivo Banco { get; set; }

        [DataMember]
        public UDTSQL_smallint Estado { get; set; }

        #endregion
    }
}
