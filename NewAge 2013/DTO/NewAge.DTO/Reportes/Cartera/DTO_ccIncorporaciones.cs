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
    public class DTO_ccIncorporaciones
    {
        #region Contructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccIncorporaciones(IDataReader dr)
        {
            this.InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                if (!string.IsNullOrEmpty(dr["ClienteID"].ToString()))
                    this.ClienteID.Value = dr["ClienteID"].ToString();
                if (!string.IsNullOrEmpty(dr["Nombre"].ToString()))
                    this.Nombre.Value = dr["Nombre"].ToString();
                if (!string.IsNullOrEmpty(dr["VlrLibranza"].ToString()))
                    this.VlrLibranza.Value = Convert.ToDecimal(dr["VlrLibranza"]);
                if (!string.IsNullOrEmpty(dr["VlrCuota"].ToString()))
                    this.VlrCuota.Value = Convert.ToDecimal(dr["VlrCuota"]);
                if (!string.IsNullOrEmpty(dr["FechaCuota1"].ToString()))
                    this.FechaCuota1.Value = Convert.ToDateTime(dr["FechaCuota1"]);
                if (!string.IsNullOrEmpty(dr["Libranza"].ToString()))
                    this.Libranza.Value = Convert.ToInt32(dr["Libranza"]);
                this.TipoDoc = dr["tipoDoc"].ToString();
                if (!string.IsNullOrEmpty(dr["nomPagaduria"].ToString()))
                    this.NomPagaduria.Value = dr["nomPagaduria"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccIncorporaciones()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Inicializa los datos basicos
            this.NumeroDoc = new UDT_Consecutivo();
            this.Libranza = new UDT_LibranzaID();
            this.ClienteID = new UDT_ClienteID();
            this.Nombre = new UDT_Descriptivo();
            this.VlrLibranza = new UDT_Valor();
            this.VlrCuota = new UDT_Valor();
            this.FechaCuota1 = new UDTSQL_datetime();
            this.NomPagaduria = new UDT_Descriptivo();
            this.RepresentateLegal = new UDT_Descriptivo();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_LibranzaID Libranza { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        public UDT_Descriptivo Nombre { get; set; }

        [DataMember]
        public UDT_Valor VlrLibranza { get; set; }

        [DataMember]
        public UDT_Valor VlrCuota { get; set; } 

        [DataMember]
        public UDTSQL_datetime FechaCuota1 { get; set; }

        [DataMember]
        public string TipoDoc { get; set; }

        [DataMember]
        public UDT_Descriptivo NomPagaduria { get; set; }

        [DataMember]
        public string DesGeneral { get; set; }

        [DataMember]
        public UDT_Descriptivo RepresentateLegal { get; set; }

        #endregion
    }
}
