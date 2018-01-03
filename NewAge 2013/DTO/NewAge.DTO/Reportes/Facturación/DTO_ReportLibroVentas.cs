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
    public class DTO_ReportLibroVentas
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ReportLibroVentas(IDataReader dr)
        {
            this.InitCols();
            try
            {
                if (!string.IsNullOrEmpty(dr["DocumentoNro"].ToString()))
                    this.DocumentoNro.Value = Convert.ToInt32(dr["DocumentoNro"]);
                if (!string.IsNullOrEmpty(dr["PrefijoID"].ToString()))
                    this.PrefijoID.Value = dr["PrefijoID"].ToString();
                
                if (!string.IsNullOrEmpty(dr["factura"].ToString()))
                    this.factura.Value = dr["factura"].ToString();
                if (!string.IsNullOrEmpty(dr["FechaDoc"].ToString()))
                    this.FechaDoc.Value = Convert.ToDateTime(dr["FechaDoc"]);
                if (!string.IsNullOrEmpty(dr["ClienteID"].ToString()))
                    this.ClienteID.Value = dr["ClienteID"].ToString();
                if (!string.IsNullOrEmpty(dr["NombreCliente"].ToString()))
                    this.NombreCliente.Value = dr["NombreCliente"].ToString();
                if (!string.IsNullOrEmpty(dr["Bruto"].ToString()))
                    this.Bruto.Value = Convert.ToDecimal(dr["Bruto"]);
                if (!string.IsNullOrEmpty(dr["Iva"].ToString()))
                    this.Iva.Value = Convert.ToDecimal(dr["Iva"]);
                if (!string.IsNullOrEmpty(dr["vlrTotal"].ToString()))
                    this.vlrTotal.Value = Convert.ToDecimal(dr["vlrTotal"]);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportLibroVentas()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.DocumentoNro = new UDTSQL_int();
            this.PrefijoID = new UDT_PrefijoID();
            this.factura = new UDTSQL_char(35);
            this.FechaDoc = new UDTSQL_smalldatetime();
            this.ClienteID = new UDT_ClienteID();
            this.NombreCliente = new UDT_Descriptivo();
            this.Bruto = new UDT_Valor();
            this.Iva = new UDT_Valor();
            this.vlrTotal = new UDT_Valor();
        }
        #endregion

        #region Propiedades

        [DataMember]
        public UDTSQL_int DocumentoNro { get; set; }

        [DataMember]
        public UDT_PrefijoID PrefijoID { get; set; }

        [DataMember]
        public UDTSQL_char factura { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaDoc { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        public UDT_Descriptivo NombreCliente { get; set; }

        [DataMember]
        public UDT_Valor Bruto { get; set; }

        [DataMember]
        public UDT_Valor Iva { get; set; }

        [DataMember]
        public UDT_Valor vlrTotal { get; set; }

        [DataMember]
        public List<DTO_ReportLibroVentas> Detalle { get; set; }
        #endregion

    }
}
