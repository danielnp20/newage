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
    public class DTO_ccSolicitudes
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccSolicitudes(IDataReader dr)
        {
            this.InitCols();
            try
            {
                if (!string.IsNullOrWhiteSpace(dr["Libranza"].ToString()))
                    this.Libranza.Value = Convert.ToInt32(dr["Libranza"]);
                this.cc.Value = dr["cc"].ToString();
                this.NombreCliente.Value = dr["NombreCliente"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["FechaDoc"].ToString()))
                    this.FechaDoc.Value = Convert.ToDateTime(dr["FechaDoc"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrSolicitado"].ToString()))
                    this.VlrSolicitado.Value = Convert.ToDecimal(dr["VlrSolicitado"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrPrestamo"].ToString()))
                    this.VlrPrestamo.Value = Convert.ToDecimal(dr["VlrPrestamo"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrLibranza"].ToString()))
                    this.VlrLibranza.Value = Convert.ToDecimal(dr["VlrLibranza"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrCuota"].ToString()))
                    this.VlrCuota.Value = Convert.ToDecimal(dr["VlrCuota"]);
                this.estado.Value = dr["estado"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccSolicitudes()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Libranza = new UDT_LibranzaID();
            this.cc = new UDT_ClienteID();
            this.NombreCliente = new UDT_Descriptivo();
            this.FechaDoc = new UDTSQL_smalldatetime();
            this.VlrSolicitado = new UDT_Valor();
            this.VlrPrestamo = new UDT_Valor();
            this.VlrLibranza = new UDT_Valor();
            this.VlrCuota = new UDT_Valor();
            this.estado = new UDT_Descriptivo();
        }
        #endregion

        #region Propiedades
        [DataMember]
        public UDT_LibranzaID Libranza { get; set; }

        [DataMember]
        public UDT_ClienteID cc { get; set; }

        [DataMember]
        public UDT_Descriptivo NombreCliente { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaDoc { get; set; }

        [DataMember]
        public UDT_Valor VlrSolicitado { get; set; }

        [DataMember]
        public UDT_Valor VlrPrestamo { get; set; }

        [DataMember]
        public UDT_Valor VlrLibranza { get; set; }

        [DataMember]
        public UDT_Valor VlrCuota { get; set; }

        [DataMember]
        public UDT_Descriptivo estado { get; set; }
        #endregion
    }
}
