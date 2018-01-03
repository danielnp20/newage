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
    /// Models DTO_DataCreditoMes
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_CentralRiesgoMes
    {
        #region DTO_DataCreditoMes

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_CentralRiesgoMes(IDataReader dr)
        {
            InitCols();
            try
            {
                this.Obligacion.Value = dr["Obligacion"].ToString();
                this.NumeroID.Value = Convert.ToInt32(dr["NumeroID"]);
                if (!string.IsNullOrWhiteSpace(dr["TipoID"].ToString()))
                    this.TipoID.Value = Convert.ToByte(dr["TipoID"]);
                this.Nombre.Value = dr["Nombre"].ToString();
                this.Apertura.Value = Convert.ToDateTime(dr["Apertura"]);
                this.Vencimiento.Value = Convert.ToDateTime(dr["Vencimiento"]);
                if (!string.IsNullOrWhiteSpace(dr["FecEstado"].ToString()))
                    this.FecEstado.Value = Convert.ToDateTime(dr["FecEstado"]);
                if (!string.IsNullOrWhiteSpace(dr["FormaPago"].ToString()))
                    this.FormaPago.Value = Convert.ToByte(dr["FormaPago"]);
                if (!string.IsNullOrWhiteSpace(dr["CAdjetivo"].ToString()))
                    this.CAdjetivo.Value = Convert.ToByte(dr["CAdjetivo"]);
                this.Garante.Value = dr["Garante"].ToString();
                this.ValorIni.Value = Convert.ToDecimal(dr["ValorIni"]);
                this.CiudadRe.Value = dr["CiudadRe"].ToString();
                this.DiasMora.Value = Convert.ToInt32(dr["DiasMora"]);
                this.DireccionCli.Value = dr["DireccionCli"].ToString();
                this.TelefonoCli.Value = dr["TelefonoCli"].ToString();
                this.CiudadCli.Value = dr["CiudadCli"].ToString();
                this.CiudadCliDesc.Value = dr["CiudadCliDesc"].ToString();
                this.CiudadLabor.Value = dr["CiudadLabor"].ToString();
                this.TelefonoLabor.Value = dr["TelefonoLabor"].ToString();
                this.CuotasTOT.Value = Convert.ToInt32(dr["CuotasTOT"]);
                this.CuotasCAN.Value = Convert.ToInt32(dr["CuotasCAN"]);
                this.CuotasVEN.Value = Convert.ToInt32(dr["CuotasVEN"]);
                this.VlrCuota.Value = Convert.ToDecimal(dr["VlrCuota"]);
                this.NovedadFin.Value = dr["NovedadFin"].ToString();
                this.Adjetivo.Value = dr["Adjetivo"].ToString();
                this.EstadoCta.Value = dr["EstadoCta"].ToString();
                this.SaldoDeuda.Value = Convert.ToDecimal(dr["SaldoDeuda"]);
                this.SaldoMora.Value = Convert.ToDecimal(dr["SaldoMora"]);
                if (!string.IsNullOrWhiteSpace(dr["Moneda"].ToString()))
                    this.Moneda.Value = Convert.ToByte(dr["Moneda"]);
                this.Calificacion.Value = dr["Calificacion"].ToString();
                this.CTipoCta.Value = dr["CTipoCta"].ToString();
                this.EdadMora.Value = dr["EdadMora"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["FechaCorte"].ToString()))
                    this.FechaCorte.Value = Convert.ToDateTime(dr["FechaCorte"]);
                this.CEstrato.Value = dr["CEstrato"].ToString();
                this.Codeudor1.Value = dr["Codeudor1"].ToString();
                this.Codeudor2.Value = dr["Codeudor2"].ToString();
                this.Codeudor3.Value = dr["Codeudor3"].ToString();
                this.Codeudor4.Value = dr["Codeudor4"].ToString();
                this.Codeudor5.Value = dr["Codeudor5"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["CtasMLV"].ToString()))
                    this.CtasMLV.Value = Convert.ToDecimal(dr["CtasMLV"]);                   
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_CentralRiesgoMes()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.Obligacion = new UDT_DescripTBase();
            this.NumeroID = new UDTSQL_int();
            this.TipoID = new UDTSQL_tinyint();
            this.Nombre = new UDT_Descriptivo();
            this.Apertura = new UDTSQL_datetime();
            this.Vencimiento = new UDTSQL_datetime();
            this.FecEstado = new UDTSQL_datetime();
            this.FormaPago = new UDTSQL_tinyint();
            this.CAdjetivo  = new UDTSQL_tinyint();
            this.Garante = new UDTSQL_char(2);
            this.ValorIni = new UDT_Valor();
            this.CiudadRe = new UDT_LugarGeograficoID();
            this.DiasMora = new UDTSQL_int();
            this.DireccionCli = new UDT_DescripTBase();
            this.TelefonoCli = new UDT_DescripTBase();
            this.CiudadCli = new UDT_LugarGeograficoID();
            this.CiudadCliDesc = new UDT_Descriptivo();
            this.CiudadLabor = new UDT_LugarGeograficoID();
            this.TelefonoLabor = new UDT_DescripTBase();
            this.CuotasTOT = new UDT_CuotaID();
            this.CuotasCAN = new UDT_CuotaID();
            this.CuotasVEN = new UDT_CuotaID();
            this.VlrCuota = new UDT_Valor();
            this.NovedadFin = new UDTSQL_char(2);
            this.Adjetivo = new UDTSQL_char(2);
            this.EstadoCta = new UDTSQL_char(2);
            this.SaldoDeuda = new UDT_Valor();
            this.SaldoMora = new UDT_Valor();
            this.Moneda = new UDTSQL_tinyint();
            this.Calificacion = new UDTSQL_char(1);
            this.CTipoCta = new UDTSQL_char(1);
            this.EdadMora = new UDTSQL_char(3);
            this.FechaCorte = new UDTSQL_datetime();
            this.CEstrato = new UDTSQL_char(3);
            this.Codeudor1 = new UDT_ClienteID();
            this.Codeudor2 = new UDT_ClienteID();
            this.Codeudor3 = new UDT_ClienteID();
            this.Codeudor4 = new UDT_ClienteID();
            this.Codeudor5 = new UDT_ClienteID();
            this.CtasMLV = new UDT_Valor();
        }

        #endregion

        #region Propiedades

       	[DataMember]
        public UDT_DescripTBase Obligacion { get; set; }

        [DataMember]
        public UDTSQL_int NumeroID { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoID { get; set; }

        [DataMember]
        public UDT_Descriptivo Nombre { get; set; }

        [DataMember]
        public UDTSQL_datetime Apertura { get; set; }

        [DataMember]
        public UDTSQL_datetime Vencimiento { get; set; }

        [DataMember]
        public UDTSQL_datetime FecEstado { get; set; }

        [DataMember]
        public UDTSQL_tinyint FormaPago { get; set; }

        [DataMember]
        public UDTSQL_tinyint CAdjetivo { get; set; }

        [DataMember]
        public UDTSQL_char Garante { get; set; }

        [DataMember]
        public UDT_Valor ValorIni { get; set; }

        [DataMember]
        public UDT_LugarGeograficoID CiudadRe { get; set; }

        [DataMember]
        public UDTSQL_int DiasMora { get; set; }

        [DataMember]
        public UDT_DescripTBase DireccionCli { get; set; }

        [DataMember]
        public UDT_DescripTBase TelefonoCli { get; set; }

        [DataMember]
        public UDT_LugarGeograficoID CiudadCli { get; set; }

        [DataMember]
        public UDT_Descriptivo CiudadCliDesc { get; set; }

        [DataMember]
        public UDT_LugarGeograficoID CiudadLabor { get; set; }

        [DataMember]
        public UDT_DescripTBase TelefonoLabor { get; set; }

        [DataMember]
        public UDT_CuotaID CuotasTOT { get; set; }

        [DataMember]
        public UDT_CuotaID CuotasCAN { get; set; }

         [DataMember]
        public UDT_CuotaID CuotasVEN { get; set; }

        [DataMember]
        public UDT_Valor VlrCuota { get; set; }

        [DataMember]
        public UDTSQL_char NovedadFin { get; set; }

        [DataMember]
        public UDTSQL_char Adjetivo { get; set; }

        [DataMember]
        public UDTSQL_char EstadoCta { get; set; }

        [DataMember]
        public UDT_Valor SaldoDeuda { get; set; }

        [DataMember]
        public UDT_Valor SaldoMora { get; set; }

        [DataMember]
        public UDTSQL_tinyint Moneda { get; set; }

        [DataMember]
        public UDTSQL_char Calificacion { get; set; }

        [DataMember]
        public UDTSQL_char CTipoCta { get; set; }

        [DataMember]
        public UDTSQL_char EdadMora { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaCorte { get; set; }

        [DataMember]
        public UDTSQL_char CEstrato { get; set; }

        [DataMember]
        public UDT_ClienteID Codeudor1 { get; set; }

        [DataMember]
        public UDT_ClienteID Codeudor2 { get; set; }

        [DataMember]
        public UDT_ClienteID Codeudor3 { get; set; }

        [DataMember]
        public UDT_ClienteID Codeudor4 { get; set; }

        [DataMember]
        public UDT_ClienteID Codeudor5 { get; set; }

        [DataMember]
        public UDT_Valor CtasMLV { get; set; }
        #endregion
    }
}
	