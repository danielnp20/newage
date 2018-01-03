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
    /// Models DTO_ccSaldosComponentes
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccSaldosComponentes
    {
        #region DTO_ccSaldosComponentes

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccSaldosComponentes(IDataReader dr)
        {

            this.InitCols();
            try
            {
                this.CuotaID.Value = Convert.ToInt32(dr["CuotaID"].ToString());
                this.ComponenteCarteraID.Value = dr["ComponenteCarteraID"].ToString();
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
                this.TipoComponente.Value = Convert.ToByte(dr["TipoComponente"]); 
                this.ComponenteFijo.Value = Convert.ToBoolean(dr["ComponenteFijo"]);
                this.PagoTotalInd.Value = Convert.ToBoolean(dr["PagoTotalInd"]);
                this.CuotaInicial.Value = Convert.ToDecimal(dr["CuotaInicial"]);
                this.TotalInicial.Value = Convert.ToDecimal(dr["TotalInicial"]);
                this.CuotaSaldo.Value = Convert.ToDecimal(dr["CuotaSaldo"]);

                if (!string.IsNullOrWhiteSpace(dr["TipoPago"].ToString()))
                    this.TipoPago.Value = Convert.ToByte(dr["TipoPago"]);

                //if (string.IsNullOrWhiteSpace(dr["TotalSaldo"].ToString()))
                    //this.TotalSaldo.Value = 0;
                //else
                    this.TotalSaldo.Value = Convert.ToDecimal(dr["TotalSaldo"].ToString());
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
        public DTO_ccSaldosComponentes()
        {
            this.InitCols();
        }

        private void InitCols()
        {
            this.CuotaID = new UDT_CuotaID();
            this.ComponenteCarteraID = new UDT_BasicID();
            this.Descriptivo = new UDT_Descriptivo();
            this.TipoComponente = new UDTSQL_tinyint();
            this.ComponenteFijo = new UDT_SiNo();
            this.PagoTotalInd = new UDT_SiNo();
            this.TipoPago = new UDTSQL_tinyint();
            this.CuotaInicial = new UDT_Valor();
            this.TotalInicial = new UDT_Valor();
            this.CuotaSaldo = new UDT_Valor();
            this.TotalSaldo = new UDT_Valor();

            //Campos Adicionales
            this.AbonoValor = new UDT_Valor();
            this.AbonoSaldo = new UDT_Valor();
            this.CuentaID = new UDT_CuentaID();
            this.Editable = new UDT_SiNo();
            this.NumDocCredito = new UDT_Consecutivo();
            this.VlrNotaCredito = new UDT_Valor();
            this.VlrNuevoSaldo = new UDT_Valor();

            this.AbonoValor.Value = 0;
            this.AbonoSaldo.Value = 0;
            this.VlrNotaCredito.Value = 0;
            this.VlrNuevoSaldo.Value = 0;
        }

        #endregion

        [DataMember]
        public UDT_CuotaID CuotaID { get; set; }

        [DataMember]
        public UDT_BasicID ComponenteCarteraID { get; set; }

        [DataMember]
        public UDT_Descriptivo Descriptivo { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoComponente { get; set; }

        [DataMember]
        public UDT_SiNo ComponenteFijo { get; set; }

        [DataMember]
        public UDT_SiNo PagoTotalInd { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoPago { get; set; }

        [DataMember]
        public UDT_Valor CuotaInicial { get; set; }

        [DataMember]
        public UDT_Valor TotalInicial { get; set; }

        [DataMember]
        public UDT_Valor CuotaSaldo { get; set; }

        [DataMember]
        public UDT_Valor TotalSaldo { get; set; }


        //Campos Adicionales

        [DataMember]
        public UDT_Valor AbonoValor { get; set; }

        [DataMember]
        public UDT_Valor AbonoSaldo { get; set; }

        [DataMember]
        public UDT_CuentaID CuentaID { get; set; }

        [DataMember]
        public UDT_SiNo Editable { get; set; }

        [DataMember]
        public UDT_Consecutivo NumDocCredito { get; set; }

        [DataMember]
        public UDT_Valor VlrNotaCredito { get; set; }

        [DataMember]
        public UDT_Valor VlrNuevoSaldo { get; set; }
    }
}
