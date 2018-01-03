using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.DTO.UDT;
using System.Data;

namespace NewAge.DTO.Negocio
{

    /// <summary>
    /// Class Pagos Electronicos:
    /// Models DTO_PagosElectronicos
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_PagosElectronicos
    {
        #region DTO_PagosElectronicos
        
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="dr"></param>
        public DTO_PagosElectronicos(IDataReader dr)
        {
            this.InitCols();
            try
            {
                this.BancoCuentaID.Value = dr["BancoCuentaID"].ToString();
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"].ToString());
                this.TerceroID.Value = dr["TerceroID"].ToString();
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
                this.Banco.Value = dr["Banco"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["TipoCuenta"].ToString()))
                    this.TipoCuenta.Value = Convert.ToByte(dr["TipoCuenta"].ToString());
                this.CuentaNro.Value = dr["CuentaNro"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["Fecha"].ToString()))
                    this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                this.ComprobanteID.Value = dr["ComprobanteID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["ComprobanteIDNro"].ToString()))
                    this.ComprobanteIDNro.Value = Convert.ToInt32(dr["ComprobanteIDNro"]);
                this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaTransmision"].ToString()))
                    this.FechaTransmicion.Value = Convert.ToDateTime(dr["FechaTransmision"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_PagosElectronicos()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.PagosElectronicosInd = new UDT_SiNo();
            this.DevolverTransmicionInd = new UDT_SiNo();
            this.BancoCuentaID = new UDT_BancoCuentaID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.TerceroID = new UDT_TerceroID();
            this.Descriptivo = new UDT_DescripTBase();
            this.Banco = new UDT_BancoID();
            this.TipoCuenta = new UDTSQL_tinyint();
            this.CuentaNro = new UDTSQL_varchar(15);
            this.Fecha = new UDTSQL_smalldatetime();
            this.ComprobanteID = new UDT_ComprobanteID();
            this.ComprobanteIDNro = new UDTSQL_int();
            this.Valor = new UDT_Valor();
            this.FechaTransmicion = new UDTSQL_smalldatetime();
        }


        #endregion

        #region Propiedades

        [DataMember]
        public int Index { get; set; }

        [DataMember]
        public UDT_SiNo PagosElectronicosInd { get; set; }

        [DataMember]
        public UDT_SiNo DevolverTransmicionInd { get; set; }

        [DataMember]
        public UDT_BancoCuentaID BancoCuentaID { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_DescripTBase Descriptivo { get; set; }

        [DataMember]
        public UDT_BancoID Banco { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoCuenta { get; set; }

        [DataMember]
        public UDTSQL_varchar CuentaNro { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime Fecha { get; set; }

        [DataMember]
        public UDT_ComprobanteID ComprobanteID { get; set; }

        [DataMember]
        public UDTSQL_int ComprobanteIDNro { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaTransmicion { get; set; }

        #endregion
    }
}
