using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.Attributes;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// DTO Tabla DTO_inQuerySeriales
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_inQuerySeriales
    {
        #region Constructor

        public DTO_inQuerySeriales(IDataReader dr)
        {
            InitCols();
            try
            {
                if (!string.IsNullOrWhiteSpace(dr["SerialID"].ToString()))
                    this.SerialID.Value = dr["SerialID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["BodegaID"].ToString()))
                    this.BodegaID.Value = dr["BodegaID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["InReferenciaID"].ToString()))
                    this.InReferenciaID.Value = dr["InReferenciaID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["NumeroDoc"].ToString()))
                    this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                if (!string.IsNullOrWhiteSpace(dr["Documento"].ToString()))
                    this.Documento.Value = dr["Documento"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["Periodo"].ToString()))
                    this.Periodo.Value = Convert.ToDateTime(dr["Periodo"]);
                if (!string.IsNullOrWhiteSpace(dr["Tipo"].ToString()))
                    this.Tipo.Value = dr["Tipo"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["DocSoporte"].ToString()))
                    this.DocSoporte.Value = Convert.ToInt32(dr["DocSoporte"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_inQuerySeriales()
        {
            InitCols();
        }

        public void InitCols()
        {
            this.SerialID = new UDT_SerialID();
            this.BodegaID = new UDT_BodegaID();
            this.InReferenciaID = new UDT_inReferenciaID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.Documento = new UDTSQL_char(50);
            this.Periodo = new UDTSQL_smalldatetime();
            this.Tipo = new UDTSQL_char(20);
            this.DocSoporte = new UDT_Consecutivo();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_SerialID SerialID { get; set; }

        [DataMember]
        public UDT_BodegaID BodegaID { get; set; }

        [DataMember]
        public UDT_inReferenciaID  InReferenciaID { get; set; }

        [DataMember]
        public UDT_Consecutivo  NumeroDoc { get; set; }

        [DataMember]
        public UDTSQL_char Documento { get; set; }
        
        [DataMember]
        public UDT_Consecutivo DocSoporte { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime Periodo { get; set; }

        [DataMember]
        public UDTSQL_char Tipo { get; set; }

        #endregion
    }
}
