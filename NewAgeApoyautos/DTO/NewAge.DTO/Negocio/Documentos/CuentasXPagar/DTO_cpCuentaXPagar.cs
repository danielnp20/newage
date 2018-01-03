using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.Attributes;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// DTO Tabla Cuentas X Pagar
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_cpCuentaXPagar
    {
        #region Constructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_cpCuentaXPagar(IDataReader dr)
        {
            InitCols();
            try
            {
                this.EmpresaID.Value = Convert.ToString(dr["EmpresaID"]);
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.RadicaFecha.Value = Convert.ToDateTime(dr["RadicaFecha"]);
                this.ConceptoCxPID.Value = Convert.ToString(dr["ConceptoCxPID"]);
                this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                this.IVA.Value = Convert.ToDecimal(dr["IVA"]);
                this.MonedaPago.Value = Convert.ToString(dr["MonedaPago"]);
                this.FacturaFecha.Value = Convert.ToDateTime(dr["FacturaFecha"]);
                this.VtoFecha.Value = Convert.ToDateTime(dr["VtoFecha"]);
                if (!string.IsNullOrWhiteSpace(dr["ContabFecha"].ToString()))
                    this.ContabFecha.Value = Convert.ToDateTime(dr["ContabFecha"]);
                this.CausalDevID.Value = Convert.ToString(dr["CausalDevID"]);
                this.DistribuyeImpLocalInd.Value = Convert.ToBoolean(dr["DistribuyeImpLocalInd"]);
                if (!string.IsNullOrWhiteSpace(dr["RadicaCodigo"].ToString()))
                    this.RadicaCodigo.Value = Convert.ToString(dr["RadicaCodigo"]);
                if (!string.IsNullOrWhiteSpace(dr["NumeroRadica"].ToString()))
                    this.NumeroRadica.Value = Convert.ToInt32(dr["NumeroRadica"]);
                this.FacturaEquivalente.Value = dr["FactEquivalente"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["ValorLocal"].ToString()))
                    this.ValorLocal.Value = Convert.ToDecimal(dr["ValorLocal"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorExtra"].ToString()))
                    this.ValorExtra.Value = Convert.ToDecimal(dr["ValorExtra"]);
                if (!string.IsNullOrWhiteSpace(dr["PagoInd"].ToString()))
                    this.PagoInd.Value = Convert.ToBoolean(dr["PagoInd"]);
                if (!string.IsNullOrWhiteSpace(dr["PagoAprobacionInd"].ToString()))
                    this.PagoAprobacionInd.Value = Convert.ToBoolean(dr["PagoAprobacionInd"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorPago"].ToString()))
                    this.ValorPago.Value = Convert.ToDecimal(dr["ValorPago"]);
                if (!string.IsNullOrWhiteSpace(dr["BancoCuentaID"].ToString()))
                    this.BancoCuentaID.Value = dr["BancoCuentaID"].ToString();
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
                if (!string.IsNullOrEmpty(dr["NumeroDocPadre"].ToString()))
                    this.NumeroDocPadre.Value = Convert.ToInt32(dr["NumeroDocPadre"]);

                if (!string.IsNullOrEmpty(dr["ProvisionInd"].ToString()))
                    this.ProvisionInd.Value = Convert.ToBoolean(dr["ProvisionInd"]);
                if (!string.IsNullOrEmpty(dr["PeriComprRevierteProv"].ToString()))
                    this.PeriComprRevierteProv.Value = Convert.ToDateTime(dr["PeriComprRevierteProv"]);
                if (!string.IsNullOrEmpty(dr["TipoComprRevierteProv"].ToString()))
                    this.TipoComprRevierteProv.Value = dr["TipoComprRevierteProv"].ToString();
                if (!string.IsNullOrEmpty(dr["NumeComprRevierteProv"].ToString()))
                    this.NumeComprRevierteProv.Value = Convert.ToInt32(dr["NumeComprRevierteProv"]);
                if (!string.IsNullOrEmpty(dr["ValorTercero"].ToString()))
                    this.ValorTercero.Value = Convert.ToDecimal(dr["ValorTercero"]);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_cpCuentaXPagar()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.NumeroDocPadre = new UDT_Consecutivo();
            this.RadicaFecha = new UDTSQL_datetime();
            this.RadicaCodigo = new UDTSQL_char(20);
            this.ConceptoCxPID = new UDT_ConceptoCxPID();
            this.Valor = new UDT_Valor();
            this.IVA = new UDT_Valor();
            this.MonedaPago = new UDT_MonedaID();
            this.FacturaFecha = new UDTSQL_datetime();
            this.VtoFecha = new UDTSQL_datetime();
            this.ContabFecha = new UDTSQL_datetime();
            this.CausalDevID = new UDTSQL_char(3);
            this.DistribuyeImpLocalInd = new UDT_SiNo();
            this.RadicaCodigo = new UDTSQL_char(20);
            this.NumeroRadica = new UDT_Consecutivo();
            this.FacturaEquivalente = new UDTSQL_char(20);
            this.ValorLocal = new UDT_Valor();
            this.ValorExtra = new UDT_Valor();
            this.PagoInd = new UDT_SiNo();
            this.PagoAprobacionInd = new UDT_SiNo();
            this.ValorPago = new UDT_Valor();
            this.BancoCuentaID = new UDT_BancoCuentaID();
            this.Dato1 = new UDT_DescripTBase();
            this.Dato2 = new UDT_DescripTBase();
            this.Dato3 = new UDT_DescripTBase();
            this.Dato4 = new UDT_DescripTBase();
            this.Dato5 = new UDT_DescripTBase();
            this.Dato6 = new UDT_DescripTBase();
            this.Dato7 = new UDT_DescripTBase();
            this.Dato8 = new UDT_DescripTBase();
            this.Dato9 = new UDT_DescripTBase();
            this.Dato10 = new UDT_DescripTBase();
            this.ProvisionInd = new UDT_SiNo();
            this.PeriComprRevierteProv = new UDT_PeriodoID();
            this.TipoComprRevierteProv = new UDT_ComprobanteID();
            this.NumeComprRevierteProv = new UDTSQL_int();
            this.ValorTercero = new UDT_Valor();
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
        [NotImportable]
        public UDT_Consecutivo NumeroDocPadre { get; set; }

        [DataMember]
        public UDTSQL_datetime RadicaFecha { get; set; }

        [DataMember]
        public UDT_ConceptoCxPID ConceptoCxPID { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor IVA { get; set; }

        [DataMember]
        public UDT_MonedaID MonedaPago { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_datetime FacturaFecha { get; set; }

        [DataMember]
        public UDTSQL_datetime VtoFecha { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_datetime ContabFecha { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char CausalDevID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_SiNo DistribuyeImpLocalInd { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDTSQL_char RadicaCodigo { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_Consecutivo NumeroRadica { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDTSQL_char FacturaEquivalente { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor ValorLocal { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor ValorExtra { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_SiNo PagoInd { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_SiNo PagoAprobacionInd { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_Valor ValorPago { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_BancoCuentaID BancoCuentaID { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_DescripTBase Dato1 { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_DescripTBase Dato2 { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_DescripTBase Dato3 { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_DescripTBase Dato4 { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_DescripTBase Dato5 { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_DescripTBase Dato6 { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_DescripTBase Dato7 { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_DescripTBase Dato8 { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_DescripTBase Dato9 { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_DescripTBase Dato10 { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_SiNo ProvisionInd { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_PeriodoID PeriComprRevierteProv { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_ComprobanteID TipoComprRevierteProv { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDTSQL_int NumeComprRevierteProv { get; set; }
        
        [DataMember]
        [NotImportable]
        public UDT_Valor ValorTercero { get; set; }

        #endregion
    }
}
