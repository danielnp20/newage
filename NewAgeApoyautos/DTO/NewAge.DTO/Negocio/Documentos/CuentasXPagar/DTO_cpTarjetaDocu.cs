using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_cpTarjetaDocu
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_cpTarjetaDocu
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_cpTarjetaDocu(IDataReader dr)
        {
            this.InitCols();
            try
            {
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.TarjetaCreditoID.Value = dr["TarjetaCreditoID"].ToString();
                this.PeriodoPago.Value = Convert.ToDateTime(dr["PeriodoPago"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor"].ToString()))
                    this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                if (!string.IsNullOrWhiteSpace(dr["IVA"].ToString()))
                    this.IVA.Value = Convert.ToDecimal(dr["IVA"]);
                if (!string.IsNullOrWhiteSpace(dr["NumeroDocCXP"].ToString()))
                    this.NumeroDocCXP.Value = Convert.ToInt32(dr["NumeroDocCXP"]);

            }
            catch (Exception e)
            {                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_cpTarjetaDocu()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.TarjetaCreditoID = new UDT_TarjetaCreditoID();
            this.PeriodoPago = new UDTSQL_smalldatetime();
            this.Valor = new UDT_Valor();
            this.IVA = new UDT_Valor();
            this.NumeroDocCXP = new UDT_Consecutivo();
        }
        #endregion

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_TarjetaCreditoID TarjetaCreditoID { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime PeriodoPago  { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor IVA { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Consecutivo NumeroDocCXP { get; set; }

    }
}
