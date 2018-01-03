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
    /// Class Pago Facturas:
    /// Models DTO_DetalleFactura
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ChequesGiradosDetaReport
    {
        #region DTO_ChequesGiradosDeta

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="dr"></param>
        public DTO_ChequesGiradosDetaReport(IDataReader dr)
        {
            this.InitCols();
            try
            {
                if (!string.IsNullOrWhiteSpace(dr["NumCheque"].ToString()))
                    this.NumCheque.Value = Convert.ToInt32(dr["NumCheque"]);
                this.Nit.Value =dr["Nit"].ToString();
                this.BancoCuentaId.Value = dr["BancoCuentaId"].ToString();
                this.Nombre.Value = dr["Nombre"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["Fecha"].ToString()))
                    this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrGirado"].ToString()))
                    this.VlrGirado.Value = Convert.ToDecimal(dr["VlrGirado"]);
                this.ComprobanteID.Value = dr["ComprobanteID"].ToString();
                this.NumeroFactura.Value = (dr["NroFactura"].ToString());
                this.BancoDescriptivo.Value = (dr["Descriptivo"].ToString());
                this.MdaTransacc.Value = (dr["MdaTransacc"].ToString());
                this.Beneficiario.Value = (dr["Beneficiario"].ToString());
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public DTO_ChequesGiradosDetaReport(IDataReader dr, bool 
            reporte)
        {
            this.InitCols();
            try
            {
               this.BancoCuentaId.Value = dr["BancoCuentaId"].ToString();
               this.BancoDescriptivo.Value = (dr["Descriptivo"].ToString());
               this.Nit.Value = dr["Nit"].ToString();
               this.Nombre.Value = dr["Nombre"].ToString();
               if (!string.IsNullOrWhiteSpace(dr["VlrGirado"].ToString()))
                    this.VlrGirado.Value = Convert.ToDecimal(dr["VlrGirado"]);
               this.MdaTransacc.Value = (dr["MdaTransacc"].ToString());
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ChequesGiradosDetaReport()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.NumCheque = new UDTSQL_int();
            this.Nit = new UDTSQL_char(50);
            this.BancoCuentaId = new UDT_BancoCuentaID();
            this.Nombre = new UDT_Descriptivo();
            this.Fecha = new UDTSQL_smalldatetime();
            this.VlrGirado = new UDT_Valor();
            this.ComprobanteID = new UDT_DescripTBase();
            this.NumeroFactura = new UDTSQL_char(20);
            this.BancoDescriptivo = new UDT_Descriptivo();
            this.MdaTransacc = new UDTSQL_char(3);
            this.Beneficiario = new UDTSQL_char(50);
        }

        #endregion

        #region Propiedades
        [DataMember]
        public UDTSQL_int NumCheque { get; set; }

        [DataMember]
        public UDTSQL_char Nit { get; set; }

        [DataMember]
        public UDT_BancoCuentaID BancoCuentaId { get; set; }

        [DataMember]
        public UDTSQL_char NumeroFactura { get; set; }
        
        [DataMember]
        public UDT_Descriptivo Nombre { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime Fecha { get; set; }
        
        [DataMember]
        public UDT_Valor VlrGirado { get; set; }

        [DataMember]
        public UDT_DescripTBase ComprobanteID { get; set; }

        [DataMember]
        public UDT_Descriptivo BancoDescriptivo { get; set; }

        [DataMember]
        public UDTSQL_char MdaTransacc { get; set; }

        [DataMember]
        public UDTSQL_char Beneficiario { get; set; }


        #endregion
    }
}
