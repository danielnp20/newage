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
    public class DTO_ccEstadoDeCuenta
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccEstadoDeCuenta(IDataReader dr)
        {
            this.InitCols();
            try
            {
                if (!string.IsNullOrEmpty(dr["ClienteId"].ToString()))
                    this.ClienteId.Value = dr["ClienteId"].ToString();
                if (!string.IsNullOrEmpty(dr["Descriptivo"].ToString()))
                    this.Descriptivo.Value = dr["Descriptivo"].ToString();
                if (!string.IsNullOrEmpty(dr["Fecha"].ToString()))
                    this.Fecha = Convert.ToDateTime(dr["Fecha"]);
                if (!string.IsNullOrEmpty(dr["DocumentoTercero"].ToString()))
                    this.DocumentoTercero.Value = dr["DocumentoTercero"].ToString();
                if (!string.IsNullOrEmpty(dr["VlrMdaLoc"].ToString()))
                    this.VlrMdaLoc.Value = Convert.ToDecimal(dr["VlrMdaLoc"]);
                if (!string.IsNullOrEmpty(dr["Movimiento"].ToString()))
                    this.Movimiento.Value = dr["Movimiento"].ToString();
                if (!string.IsNullOrEmpty(dr["SaldoIni"].ToString()))
                    this.SaldoIni.Value = Convert.ToDecimal(dr["SaldoIni"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccEstadoDeCuenta()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ClienteId = new UDT_ClienteID();
            this.Descriptivo = new UDT_Descriptivo();
            this.DocumentoTercero = new UDT_DocTerceroID();
            this.Fecha = new DateTime();
            this.DocumentoTercero = new UDT_DocTerceroID();
            this.VlrMdaLoc = new UDT_Valor();
            this.Movimiento = new UDT_Descriptivo();
            this.SaldoIni = new UDT_Valor();
        }
        #endregion

        [DataMember]
        public UDT_ClienteID ClienteId { get; set; }

        [DataMember]
        public UDT_Descriptivo Descriptivo { get; set; }

        [DataMember]
        public DateTime Fecha { get; set; }

        [DataMember]
        public UDT_DocTerceroID DocumentoTercero { get; set; }

        [DataMember]
        public UDT_Valor VlrMdaLoc { get; set; }

        [DataMember]
        public UDT_Descriptivo Movimiento { get; set; }


        [DataMember]
        public UDT_Valor SaldoIni { get; set; }

        [DataMember]
        public decimal Recaudo { get; set; }

        [DataMember]
        public decimal Retiro { get; set; }

        [DataMember]
        public decimal SaldoFinDeta { get; set; }
    }
}
