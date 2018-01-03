using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class comprobante para aprobacion:
    /// Models DTO_ccCertificadoDeuda.
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccCertificadoDeuda
    {
        #region DTO_ccCertificadoDeuda

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccCertificadoDeuda(IDataReader dr)
        {
            InitCols();
            try
            {
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
                this.ClienteId.Value = dr["ClienteId"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["NumeroDoc"].ToString()))
                    this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                //if (!string.IsNullOrWhiteSpace(dr["NroPagare"].ToString()))
                //    this.NroPagare.Value = Convert.ToInt32(dr["NroPagare"]);
                this.Pagadria.Value = dr["Pagaduria"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["Plazo"].ToString()))
                    this.Plazo.Value = Convert.ToInt32(dr["Plazo"]);
                if (!string.IsNullOrWhiteSpace(dr["EC_SaldoPend"].ToString()))
                    this.EC_SaldoPend.Value = Convert.ToDecimal(dr["EC_SaldoPend"]);
                if (!string.IsNullOrWhiteSpace(dr["EC_FechaLimite"].ToString()))
                    this.EC_FechaLimite.Value = Convert.ToDateTime(dr["EC_FechaLimite"]);
                this.Correo.Value = dr["Correo"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["FechaLiquida"].ToString()))
                    this.FechaLiquida.Value = Convert.ToDateTime(dr["FechaLiquida"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrCuota"].ToString()))
                    this.VlrCuota.Value = Convert.ToDecimal(dr["VlrCuota"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrLibranza"].ToString()))
                    this.VlrLibranza.Value = Convert.ToDecimal(dr["VlrLibranza"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccCertificadoDeuda()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            
            this.Descriptivo = new UDT_Descriptivo();
            this.ClienteId = new UDT_ClienteID();
            this.NroPagare = new UDTSQL_int();
            this.Pagadria = new UDT_Descriptivo();
            this.EC_SaldoPend = new UDT_Valor();
            this.EC_FechaLimite = new UDTSQL_smalldatetime();
            this.Correo = new UDTSQL_char(60);
            this.FechaLiquida = new UDTSQL_smalldatetime();
            this.VlrLibranza = new UDT_Valor();
            this.Plazo = new UDTSQL_int();
            this.VlrCuota = new UDT_Valor();
            this.NumeroDoc = new UDT_Consecutivo();
            //Extra
            this.isPazYSalvo = new UDT_SiNo();
        }

        #endregion

        #region Propiedades
        
        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_Descriptivo Descriptivo { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteId { get; set; }

        [DataMember]
        public UDTSQL_int NroPagare { get; set; }

        [DataMember]
        public UDT_Descriptivo Pagadria { get; set; }

        [DataMember]
        public UDT_Valor EC_SaldoPend { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime EC_FechaLimite { get; set; }

        [DataMember]
        public UDTSQL_char Correo { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaLiquida { get; set; }

        [DataMember]
        public UDT_Valor VlrLibranza { get; set; }

        [DataMember]
        public UDTSQL_int Plazo { get; set; }

        [DataMember]
        public UDT_Valor VlrCuota { get; set; }

        //Campos Extra
        [DataMember]
        public UDT_SiNo isPazYSalvo { get; set; }
        
        [DataMember]
        public string mensaje { get; set; }

        #endregion
    }
}
