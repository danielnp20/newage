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
    /// Models DTO_ccEstadoCuentaComponentes
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccEstadoCuentaComponentes
    {
        #region DTO_ccEstadoCuentaComponentes

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccEstadoCuentaComponentes(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.ComponenteCarteraID.Value = dr["ComponenteCarteraID"].ToString();
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
                this.SaldoValor.Value = Convert.ToDecimal(dr["SaldoValor"]);
                this.PagoValor.Value = Convert.ToDecimal(dr["PagoValor"]);
                this.AbonoValor.Value = Convert.ToDecimal(dr["AbonoValor"]);
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);

                this.PagoInd.Value = true;
                this.Editable.Value = false;
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
        public DTO_ccEstadoCuentaComponentes()
        {
            InitCols();
        }

        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.ComponenteCarteraID = new UDT_BasicID();
            this.SaldoValor = new UDT_Valor();
            this.PagoValor = new UDT_Valor();
            this.AbonoValor = new UDT_Valor();
            this.Consecutivo = new UDT_Consecutivo();

            //Campos Adicionales
            this.PagoInd = new UDT_SiNo();
            this.Descriptivo = new UDT_Descriptivo();
            this.VlrPagar = new UDT_Valor();
            this.VlrAbonoPrevio = new UDT_Valor();

            this.PagoInd.Value = true;
            this.AbonoValor.Value = 0;
            this.VlrPagar.Value = 0;
            this.Editable = new UDT_SiNo();
        }

        #endregion

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_BasicID ComponenteCarteraID { get; set; }

        [DataMember]
        public UDT_Valor SaldoValor{ get; set; }

        [DataMember]
        public UDT_Valor PagoValor { get; set; }

        [DataMember]
        public UDT_Valor AbonoValor{ get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        //Campos Adicionales

        [DataMember]
        public UDT_SiNo PagoInd { get; set; }

        [DataMember]
        public UDT_Descriptivo Descriptivo { get; set; }

        [DataMember]
        public UDT_Valor VlrPagar { get; set; }

        [DataMember]
        public UDT_Valor VlrAbonoPrevio { get; set; }

        [DataMember]
        public UDT_SiNo Editable { get; set; }
    }
}
