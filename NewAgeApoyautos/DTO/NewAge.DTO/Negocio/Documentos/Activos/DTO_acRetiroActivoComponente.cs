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
    /// Class Error:
    /// Models DTO_acRetiroActivoComponente
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_acRetiroActivoComponente
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_acRetiroActivoComponente(IDataReader dr)
        {
            this.InitColums();
            try
            {
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.ComponenteActivoID.Value = dr["ComponenteActivoID"].ToString();
                this.ComponenteActivoDesc.Value = dr["Descriptivo"].ToString();
                this.CuentaSaldo = new DTO_coCuentaSaldo(dr);
                this.VlrSaldoML.Value = this.CuentaSaldo.DbOrigenLocML.Value + this.CuentaSaldo.DbOrigenExtML.Value + this.CuentaSaldo.CrOrigenLocML.Value + this.CuentaSaldo.CrOrigenExtML.Value
                                        + this.CuentaSaldo.DbSaldoIniLocML.Value + this.CuentaSaldo.DbSaldoIniExtML.Value + this.CuentaSaldo.CrSaldoIniLocML.Value + this.CuentaSaldo.CrSaldoIniExtML.Value;
                this.VlrSaldoME.Value = this.CuentaSaldo.DbOrigenLocME.Value + this.CuentaSaldo.DbOrigenExtME.Value + this.CuentaSaldo.CrOrigenLocME.Value + this.CuentaSaldo.CrOrigenExtME.Value
                                        + this.CuentaSaldo.DbSaldoIniLocME.Value + this.CuentaSaldo.DbSaldoIniExtME.Value + this.CuentaSaldo.CrSaldoIniLocME.Value + this.CuentaSaldo.CrSaldoIniExtME.Value;

            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        public void InitColums()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.ComponenteActivoID = new UDT_ComponenteActivoID();
            this.VlrSaldoML = new UDT_Valor();
            this.VlrSaldoME = new UDT_Valor();
            this.VlrSaldoIFRSML = new UDT_Valor();
            this.VlrSaldoIFRSME = new UDT_Valor();
            this.ComponenteActivoDesc = new UDT_DescripTBase();
        }

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_ComponenteActivoID ComponenteActivoID { get; set; }

        [DataMember]
        public UDT_DescripTBase ComponenteActivoDesc { get; set; }

        [DataMember]
        public DTO_coCuentaSaldo CuentaSaldo { get; set; }

        [DataMember]
        public UDT_Valor VlrSaldoML { get; set; }

        [DataMember]
        public UDT_Valor VlrSaldoIFRSML { get; set; }

        [DataMember]
        public UDT_Valor VlrSaldoME { get; set; }

        [DataMember]
        public UDT_Valor VlrSaldoIFRSME { get; set; }

    }
}
