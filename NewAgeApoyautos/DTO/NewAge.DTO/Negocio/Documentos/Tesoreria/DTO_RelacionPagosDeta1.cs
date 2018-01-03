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
    /// Models DTO_RelacionPagosHeader
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_RelacionPagosDeta1
    {
        #region DTO_RelacionPagos

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="dr"></param>
        public DTO_RelacionPagosDeta1(IDataReader dr)
        {
            this.InitCols();
            try
            {
                if (!string.IsNullOrWhiteSpace(dr["NroCheque"].ToString()))
                    this.NroCheque.Value = Convert.ToInt32(dr["NroCheque"]);
                if (!string.IsNullOrWhiteSpace(dr["Nit"].ToString()))
                    this.Nit.Value = (dr["Nit"]).ToString();
                if (!string.IsNullOrWhiteSpace(dr["Descriptivo"].ToString()))
                    this.Descriptivo.Value = (dr["Descriptivo"]).ToString();
                if (!string.IsNullOrWhiteSpace(dr["Fecha"].ToString()))
                    this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrGirado"].ToString()))
                    this.VlrGirado.Value = Convert.ToDecimal(dr["VlrGirado"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrFactura"].ToString()))
                    this.VlrFactura.Value = Convert.ToDecimal(dr["VlrFactura"]);
                if (!string.IsNullOrWhiteSpace(dr["NroFactura"].ToString()))
                    this.NroFactura.Value = (dr["NroFactura"]).ToString();
                if (!string.IsNullOrWhiteSpace(dr["Valor"].ToString()))
                    this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                if (!string.IsNullOrWhiteSpace(dr["Observacion"].ToString()))
                    this.Observacion.Value = (dr["Observacion"]).ToString();
                if (!string.IsNullOrWhiteSpace(dr["MonedaId"].ToString()))
                    this.MonedaId.Value = (dr["MonedaId"]).ToString();
                if (!string.IsNullOrWhiteSpace(dr["BancoCuentaID"].ToString()))
                    this.BancoCuentaID.Value = (dr["BancoCuentaID"]).ToString();
                if (!string.IsNullOrWhiteSpace(dr["BancoDesc"].ToString()))
                    this.BancoDesc.Value = (dr["BancoDesc"]).ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_RelacionPagosDeta1()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.NroCheque = new UDTSQL_int();
            this.Nit = new UDT_TerceroID();
            this.Descriptivo = new UDT_Descriptivo();
            this.Fecha = new UDTSQL_smalldatetime();
            this.VlrFactura = new UDT_Valor();
            this.VlrGirado = new UDT_Valor();
            this.NroFactura = new UDTSQL_char(20);
            this.Valor = new UDT_Valor();
            this.Observacion = new UDT_DescripTExt();
            this.MonedaId = new UDTSQL_char(3);
            this.BancoCuentaID = new UDT_BancoCuentaID();
            this.BancoDesc = new UDT_Descriptivo();
        }
    

        #endregion

        #region Propiedades


        [DataMember]
        public UDTSQL_int NroCheque { get; set; }

        [DataMember]
        public UDT_TerceroID Nit { get; set; }

        [DataMember]
        public UDT_Descriptivo Descriptivo { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime Fecha { get; set; }

        [DataMember]
        public UDT_Valor VlrGirado { get; set; }

        [DataMember]
        public UDT_Valor VlrFactura { get; set; }

        [DataMember]
        public UDTSQL_char NroFactura { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }

        [DataMember]
        public UDTSQL_char MonedaId { get; set; }

        [DataMember]
        public UDT_BancoCuentaID BancoCuentaID { get; set; }

        [DataMember]
        public UDT_Descriptivo BancoDesc { get; set; }

        #endregion
    }
}
