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
    /// DTO Tabla DTO_cpLegalizaDocu
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_cpLegalizaDocu
    {
        #region Constructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_cpLegalizaDocu(IDataReader dr)
        {
            InitCols();
            try
            {
                this.EmpresaID.Value = Convert.ToString(dr["EmpresaID"]);
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.CajaMenorID.Value = Convert.ToString(dr["CajaMenorID"]);
                this.FechaIni.Value = Convert.ToDateTime(dr["FechaIni"]);
                this.FechaFin.Value = Convert.ToDateTime(dr["FechaFin"]);
                this.FechaCont.Value = Convert.ToDateTime(dr["FechaCont"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorFondo"].ToString()))
                    this.ValorFondo.Value = Convert.ToDecimal(dr["ValorFondo"]); 
                if (!string.IsNullOrWhiteSpace(dr["ValorVales"].ToString()))
                    this.ValorVales.Value = Convert.ToDecimal(dr["ValorVales"]); 
                if (!string.IsNullOrWhiteSpace(dr["NumeroDocCXP"].ToString()))
                    this.NumeroDocCXP.Value = Convert.ToInt32(dr["NumeroDocCXP"]); 
                if (!string.IsNullOrWhiteSpace(dr["IdentificadorAnt1"].ToString()))
                    this.IdentificadorAnt1.Value = Convert.ToInt32(dr["IdentificadorAnt1"]); 
                if (!string.IsNullOrWhiteSpace(dr["ValorAnticipo1"].ToString()))
                    this.ValorAnticipo1.Value = Convert.ToDecimal(dr["ValorAnticipo1"]);
                if (!string.IsNullOrWhiteSpace(dr["IdentificadorAnt2"].ToString()))
                    this.IdentificadorAnt2.Value = Convert.ToInt32(dr["IdentificadorAnt2"]); 
                if (!string.IsNullOrWhiteSpace(dr["ValorAnticipo2"].ToString()))
                    this.ValorAnticipo2.Value = Convert.ToDecimal(dr["ValorAnticipo2"]);
                if (!string.IsNullOrWhiteSpace(dr["IdentificadorAnt3"].ToString()))
                    this.IdentificadorAnt3.Value = Convert.ToInt32(dr["IdentificadorAnt3"]); 
                if (!string.IsNullOrWhiteSpace(dr["ValorAnticipo3"].ToString()))
                    this.ValorAnticipo3.Value = Convert.ToDecimal(dr["ValorAnticipo3"]);
                if (!string.IsNullOrWhiteSpace(dr["IdentificadorAnt4"].ToString()))
                    this.IdentificadorAnt4.Value = Convert.ToInt32(dr["IdentificadorAnt4"]); 
                if (!string.IsNullOrWhiteSpace(dr["ValorAnticipo4"].ToString()))
                    this.ValorAnticipo4.Value = Convert.ToDecimal(dr["ValorAnticipo4"]);
                if (!string.IsNullOrWhiteSpace(dr["IdentificadorAnt5"].ToString()))
                    this.IdentificadorAnt5.Value = Convert.ToInt32(dr["IdentificadorAnt5"]); 
                if (!string.IsNullOrWhiteSpace(dr["ValorAnticipo5"].ToString()))
                    this.ValorAnticipo5.Value = Convert.ToDecimal(dr["ValorAnticipo5"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor"].ToString()))
                    this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                if (!string.IsNullOrWhiteSpace(dr["IVA"].ToString()))
                    this.IVA.Value = Convert.ToDecimal(dr["IVA"]);
                this.Estado.Value = Convert.ToByte(dr["Estado"]);
                this.UsuarioSolicita.Value = (dr["UsuarioSolicita"]).ToString();
                if (!string.IsNullOrWhiteSpace(dr["FechaSolicita"].ToString()))
                    this.FechaSolicita.Value = Convert.ToDateTime(dr["FechaSolicita"]);
                this.UsuarioRevisa.Value = (dr["UsuarioRevisa"]).ToString();
                if (!string.IsNullOrWhiteSpace(dr["FechaRevisa"].ToString()))
                    this.FechaRevisa.Value = Convert.ToDateTime(dr["FechaRevisa"]);
                this.UsuarioSupervisa.Value = (dr["UsuarioSupervisa"]).ToString();
                if (!string.IsNullOrWhiteSpace(dr["FechaSupervisa"].ToString()))
                    this.FechaSupervisa.Value = Convert.ToDateTime(dr["FechaSupervisa"]);
                this.UsuarioAprueba.Value = (dr["UsuarioAprueba"]).ToString();
                if (!string.IsNullOrWhiteSpace(dr["FechaAprueba"].ToString()))
                    this.FechaAprueba.Value = Convert.ToDateTime(dr["FechaAprueba"]);
                this.UsuarioContabiliza.Value = (dr["UsuarioContabiliza"]).ToString();
                if (!string.IsNullOrWhiteSpace(dr["FechaContabiliza"].ToString()))
                    this.FechaContabiliza.Value = Convert.ToDateTime(dr["FechaContabiliza"]);
                if (!string.IsNullOrWhiteSpace(dr["TarjetaCreditoID"].ToString()))
                    this.TarjetaCreditoID.Value = dr["TarjetaCreditoID"].ToString();
            }
            catch (Exception e)
            { 
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_cpLegalizaDocu()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.CajaMenorID = new UDT_CajaMenorID();
            this.FechaIni = new UDTSQL_smalldatetime();
            this.FechaFin = new UDTSQL_smalldatetime();
            this.FechaCont = new UDTSQL_smalldatetime();
            this.ValorFondo = new UDT_Valor();
            this.ValorVales = new UDT_Valor();
            this.NumeroDocCXP = new UDT_Consecutivo();
            this.IdentificadorAnt1 = new UDT_Consecutivo();
            this.ValorAnticipo1 = new UDT_Valor();
            this.IdentificadorAnt2 = new UDT_Consecutivo();
            this.ValorAnticipo2 = new UDT_Valor();
            this.IdentificadorAnt3 = new UDT_Consecutivo();
            this.ValorAnticipo3 = new UDT_Valor();
            this.IdentificadorAnt4 = new UDT_Consecutivo();
            this.ValorAnticipo4 = new UDT_Valor();
            this.IdentificadorAnt5 = new UDT_Consecutivo();
            this.ValorAnticipo5 = new UDT_Valor();
            this.Valor = new UDT_Valor();
            this.IVA = new UDT_Valor();
            this.Estado = new  UDTSQL_tinyint();
            this.UsuarioSolicita = new UDT_UsuarioID();
            this.FechaSolicita = new UDTSQL_smalldatetime();
            this.UsuarioRevisa = new UDT_UsuarioID();
            this.FechaRevisa = new UDTSQL_smalldatetime();
            this.UsuarioSupervisa = new UDT_UsuarioID();
            this.FechaSupervisa = new UDTSQL_smalldatetime();
            this.UsuarioAprueba = new UDT_UsuarioID();
            this.FechaAprueba = new UDTSQL_smalldatetime();
            this.UsuarioContabiliza = new UDT_UsuarioID();
            this.FechaContabiliza = new UDTSQL_smalldatetime();
            this.TarjetaCreditoID = new UDT_TarjetaCreditoID();
            this.Responsable = new UDT_TerceroID();
            this.ResponsableDesc = new UDT_Descriptivo();
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
        [AllowNull]
        public UDT_CajaMenorID CajaMenorID { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_smalldatetime FechaIni { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_smalldatetime FechaFin { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_smalldatetime FechaCont { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorFondo { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorVales { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Consecutivo NumeroDocCXP { get; set; }
        
        [DataMember]
        [AllowNull]
        public UDT_Consecutivo IdentificadorAnt1 { get; set; }

        [DataMember]
        public UDT_Valor ValorAnticipo1 { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Consecutivo IdentificadorAnt2 { get; set; }

        [DataMember]
        public UDT_Valor ValorAnticipo2 { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Consecutivo IdentificadorAnt3 { get; set; }

        [DataMember]
        public UDT_Valor ValorAnticipo3 { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Consecutivo IdentificadorAnt4 { get; set; }

        [DataMember]
        public UDT_Valor ValorAnticipo4 { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Consecutivo IdentificadorAnt5 { get; set; }

        [DataMember]
        public UDT_Valor ValorAnticipo5 { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor IVA { get; set; }

        [DataMember]
        public UDTSQL_tinyint Estado { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_UsuarioID UsuarioSolicita { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_smalldatetime FechaSolicita { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_UsuarioID UsuarioRevisa { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_smalldatetime FechaRevisa { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_UsuarioID UsuarioSupervisa { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_smalldatetime FechaSupervisa { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_UsuarioID UsuarioAprueba { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_smalldatetime FechaAprueba { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_UsuarioID UsuarioContabiliza { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_smalldatetime FechaContabiliza { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_TarjetaCreditoID TarjetaCreditoID { get; set; }

        //Reporte
        [DataMember]
        public UDT_TerceroID Responsable { get; set; }

        [DataMember]
        public UDT_Descriptivo ResponsableDesc { get; set; }


        #endregion
    }
}
