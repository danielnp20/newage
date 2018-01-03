using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// 
    /// Models DTO_ccEstadoCuentaHistoria
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccEstadoCuentaHistoria
    {
        #region DTO_ccEstadoCuentaHistoria

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccEstadoCuentaHistoria(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.NumDocCredito.Value = Convert.ToInt32(dr["NumDocCredito"]);
                if (!string.IsNullOrWhiteSpace(dr["NumDocProceso"].ToString())) 
                    this.NumDocProceso.Value = Convert.ToInt32(dr["NumDocProceso"]);
                if (!string.IsNullOrWhiteSpace(dr["TasaMora"].ToString()))
                    this.TasaMora.Value = Convert.ToDecimal(dr["TasaMora"]);

                if (!string.IsNullOrWhiteSpace(dr["EC_EstadoDeuda"].ToString()))
                    this.EC_EstadoDeuda.Value = Convert.ToByte(dr["EC_EstadoDeuda"]);

                if(!string.IsNullOrWhiteSpace(dr["EC_Proposito"].ToString()))
                    this.EC_Proposito.Value = Convert.ToByte(dr["EC_Proposito"]);
                if (!string.IsNullOrWhiteSpace(dr["EC_Fecha"].ToString()))
                    this.EC_Fecha.Value = Convert.ToDateTime(dr["EC_Fecha"]);
                if (!string.IsNullOrWhiteSpace(dr["EC_FechaLimite"].ToString()))
                    this.EC_FechaLimite.Value = Convert.ToDateTime(dr["EC_FechaLimite"]);
                if (!string.IsNullOrWhiteSpace(dr["EC_Altura"].ToString())) 
                    this.EC_Altura.Value = Convert.ToInt32(dr["EC_Altura"]);
                if (!string.IsNullOrWhiteSpace(dr["EC_CuotasMora"].ToString())) 
                    this.EC_CuotasMora.Value = Convert.ToInt32(dr["EC_CuotasMora"]);
                if (!string.IsNullOrWhiteSpace(dr["EC_PrimeraCtaPagada"].ToString()))
                    this.EC_PrimeraCtaPagada.Value = Convert.ToInt32(dr["EC_PrimeraCtaPagada"]);
                if (!string.IsNullOrWhiteSpace(dr["EC_SaldoPend"].ToString())) 
                    this.EC_SaldoPend.Value = Convert.ToDecimal(dr["EC_SaldoPend"]);
                if (!string.IsNullOrWhiteSpace(dr["EC_SaldoMora"].ToString()))
                    this.EC_SaldoMora.Value = Convert.ToDecimal(dr["EC_SaldoMora"]);
                if (!string.IsNullOrWhiteSpace(dr["EC_SaldoTotal"].ToString()))
                    this.EC_SaldoTotal.Value = Convert.ToDecimal(dr["EC_SaldoTotal"]);
                if (!string.IsNullOrWhiteSpace(dr["EC_ValorPago"].ToString()))
                    this.EC_ValorPago.Value = Convert.ToDecimal(dr["EC_ValorPago"]);
                if (!string.IsNullOrWhiteSpace(dr["EC_ValorAbono"].ToString()))
                    this.EC_ValorAbono.Value = Convert.ToDecimal(dr["EC_ValorAbono"]);
                if (!string.IsNullOrWhiteSpace(dr["EC_SeguroVida"].ToString()))
                    this.EC_SeguroVida.Value = Convert.ToDecimal(dr["EC_SeguroVida"]);
                if (!string.IsNullOrWhiteSpace(dr["EC_PolizaMvto"].ToString()))
                    this.EC_PolizaMvto.Value = Convert.ToByte(dr["EC_PolizaMvto"]);
                if (!string.IsNullOrWhiteSpace(dr["EC_FijadoInd"].ToString())) 
                    this.EC_FijadoInd.Value = Convert.ToBoolean(dr["EC_FijadoInd"]);
                if (!string.IsNullOrWhiteSpace(dr["EC_NormalizaInd"].ToString())) 
                    this.EC_NormalizaInd.Value = Convert.ToBoolean(dr["EC_NormalizaInd"]);
                if (!string.IsNullOrWhiteSpace(dr["NumDocRevoca"].ToString()))
                    this.NumDocRevoca.Value = Convert.ToInt32(dr["NumDocRevoca"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaInicialFNC"].ToString()))
                    this.FechaInicialFNC.Value = Convert.ToDateTime(dr["FechaInicialFNC"]);
                if (!string.IsNullOrWhiteSpace(dr["DiasFNC"].ToString()))
                    this.DiasFNC.Value = Convert.ToByte(dr["DiasFNC"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccEstadoCuentaHistoria()
        {
            InitCols();
        }

        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.NumDocCredito = new UDT_Consecutivo();
            this.NumDocProceso = new UDT_Consecutivo();
            this.TasaMora = new UDT_TasaID();

            this.EC_EstadoDeuda = new UDTSQL_tinyint();

            this.EC_Proposito = new UDTSQL_tinyint();
            this.EC_Fecha = new UDTSQL_smalldatetime();
            this.EC_FechaLimite = new UDTSQL_smalldatetime();
            this.EC_Altura = new UDT_CuotaID();
            this.EC_CuotasMora = new UDT_CuotaID();
            this.EC_PrimeraCtaPagada = new UDT_CuotaID();
            this.EC_SaldoPend = new UDT_Valor();
            this.EC_SaldoMora = new UDT_Valor();
            this.EC_SaldoTotal = new UDT_Valor();
            this.EC_ValorPago = new UDT_Valor();
            this.EC_ValorAbono = new UDT_Valor();
            this.EC_SeguroVida = new UDT_Valor();
            this.EC_PolizaMvto = new UDTSQL_tinyint();
            this.EC_FijadoInd = new UDT_SiNo();
            this.EC_NormalizaInd = new UDT_SiNo();
            this.NumDocRevoca = new UDT_Consecutivo();
            this.FechaInicialFNC = new UDTSQL_smalldatetime();
            this.DiasFNC = new UDTSQL_tinyint();
        }

        #endregion

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_Consecutivo NumDocCredito { get; set; }

        [DataMember]
        public UDT_Consecutivo NumDocProceso { get; set; }

        [DataMember]
        public UDT_TasaID TasaMora { get; set; }

        [DataMember]
        public UDTSQL_tinyint EC_EstadoDeuda { get; set; }

        [DataMember]
        public UDTSQL_tinyint EC_Proposito { get; set; }
        
        [DataMember]
        public UDTSQL_smalldatetime EC_Fecha { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime EC_FechaLimite { get; set; }

        [DataMember]
        public UDT_CuotaID EC_Altura { get; set; }

        [DataMember]
        public UDT_CuotaID EC_CuotasMora { get; set; }

        [DataMember]
        public UDT_CuotaID EC_PrimeraCtaPagada { get; set; }

        [DataMember]
        public UDT_Valor EC_SaldoPend { get; set; }

        [DataMember]
        public UDT_Valor EC_SaldoMora { get; set; }

        [DataMember]
        public UDT_Valor EC_SaldoTotal { get; set; }

        [DataMember]
        public UDT_Valor EC_ValorPago { get; set; }

        [DataMember]
        public UDT_Valor EC_ValorAbono { get; set; }

        [DataMember]
        public UDT_Valor EC_SeguroVida { get; set; }

        [DataMember]
        public UDT_SiNo EC_FijadoInd { get; set; }

        [DataMember]
        public UDT_SiNo EC_NormalizaInd { get; set; }

        [DataMember]
        public UDTSQL_tinyint EC_PolizaMvto { get; set; }

        [DataMember]
        public UDT_Consecutivo NumDocRevoca { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaInicialFNC { get; set; }

        [DataMember]
        public UDTSQL_tinyint DiasFNC { get; set; }

    }
}
