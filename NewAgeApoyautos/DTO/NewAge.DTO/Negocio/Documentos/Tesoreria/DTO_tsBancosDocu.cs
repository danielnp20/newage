using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.DTO.UDT;
using System.Data;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{

    /// <summary>
    /// Class Pago Facturas:
    /// Models DTO_RegistroPagoFactura
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_tsBancosDocu
    {
        #region DTO_tsBancosDocu
        
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="dr"></param>
        public DTO_tsBancosDocu(IDataReader dr)
        {
            this.InitCols();
            try
            {
                this.EmpresaID.Value = Convert.ToString(dr["EmpresaID"]);
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.BancoCuentaID.Value = Convert.ToString(dr["BancoCuentaID"]);
                this.NroCheque.Value = Convert.ToInt32(dr["NroCheque"]);
                this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                this.IVA.Value = Convert.ToDecimal(dr["IVA"]);
                this.MonedaPago.Value = dr["MonedaPago"].ToString();
                this.ValorLocal.Value = Convert.ToDecimal(dr["ValorLocal"]);
                this.ValorExtra.Value = Convert.ToDecimal(dr["ValorExtra"]);
                if (!string.IsNullOrWhiteSpace(dr["NumeroDocCXP"].ToString()))
                    this.NumeroDocCXP.Value = Convert.ToInt32(dr["NumeroDocCXP"]);
                this.Dato1.Value = dr["Dato1"].ToString();
                this.Dato2.Value = dr["Dato2"].ToString();
                this.Dato3.Value = dr["Dato3"].ToString();
                this.Dato4.Value = dr["Dato4"].ToString();
                this.Dato5.Value = dr["Dato5"].ToString();
                this.Dato6.Value = dr["Dato6"].ToString();
                this.Dato7.Value = dr["Dato7"].ToString();
                this.Dato8.Value = dr["Dato8"].ToString();
                this.Dato9.Value = dr["Dato9"].ToString();
                this.Dato10.Value = dr["Dato10"].ToString();
                if (!string.IsNullOrEmpty(dr["Beneficiario"].ToString()))
                    this.Beneficiario.Value = dr["Beneficiario"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_tsBancosDocu()
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
            this.BancoCuentaID = new UDT_BancoCuentaID();
            this.NroCheque = new UDTSQL_int();
            this.Valor = new UDT_Valor();
            this.IVA = new UDT_Valor();
            this.MonedaPago = new UDT_MonedaID();
            this.ValorLocal = new UDT_Valor();
            this.ValorExtra = new UDT_Valor();
            this.NumeroDocCXP = new UDT_Consecutivo();
            this.Dato1 = new UDTSQL_char(20);
            this.Dato2 = new UDTSQL_char(20);
            this.Dato3 = new UDTSQL_char(20);
            this.Dato4 = new UDTSQL_char(20);
            this.Dato5 = new UDTSQL_char(20);
            this.Dato6 = new UDTSQL_char(20);
            this.Dato7 = new UDTSQL_char(20);
            this.Dato8 = new UDTSQL_char(20);
            this.Dato9 = new UDTSQL_char(20);
            this.Dato10 = new UDTSQL_char(20);
            this.Beneficiario = new UDTSQL_char(50);
        }


        #endregion

        #region Propiedades

        [DataMember]
        [NotImportable]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_BancoCuentaID BancoCuentaID { get; set; }

        [DataMember]
        public UDTSQL_int NroCheque { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_Valor IVA { get; set; }

        [DataMember]
        public UDT_MonedaID MonedaPago { get; set; }

        [DataMember]
        public UDT_Valor ValorLocal { get; set; }

        [DataMember]
        public UDT_Valor ValorExtra { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDocCXP { get; set; }

        [DataMember]
        public UDTSQL_char Dato1 { get; set; }

        [DataMember]
        public UDTSQL_char Dato2 { get; set; }

        [DataMember]
        public UDTSQL_char Dato3 { get; set; }

        [DataMember]
        public UDTSQL_char Dato4 { get; set; }

        [DataMember]
        public UDTSQL_char Dato5 { get; set; }

        [DataMember]
        public UDTSQL_char Dato6 { get; set; }

        [DataMember]
        public UDTSQL_char Dato7 { get; set; }

        [DataMember]
        public UDTSQL_char Dato8 { get; set; }

        [DataMember]
        public UDTSQL_char Dato9 { get; set; }

        [DataMember]
        public UDTSQL_char Dato10 { get; set; }

        [DataMember]
        public UDTSQL_char Beneficiario { get; set; }

        #endregion
    }
}
