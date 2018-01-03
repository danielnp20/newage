using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_ReportCuentasPorCobrar
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ReportCuentasPorCobrar(IDataReader dr)
        {
            this.InitCols();
            try
            {
                if (!string.IsNullOrEmpty(dr["CuentaId"].ToString()))
                    this.CuentaId.Value = dr["CuentaId"].ToString();
                //if (!string.IsNullOrEmpty(dr["CuentaDesc"].ToString()))
                //    this.CuentaDesc.Value = dr["CuentaDesc"].ToString();
                if (!string.IsNullOrEmpty(dr["Descriptivo"].ToString()))
                    this.Descriptivo.Value = dr["Descriptivo"].ToString();
                if (!string.IsNullOrEmpty(dr["Comprobante"].ToString()))
                    this.Comprobante.Value = Convert.ToString(dr["Comprobante"]);
                //if (!string.IsNullOrEmpty(dr["MdaOrigen"].ToString()))
                //    this.MdaOrigen.Value = Convert.ToString(dr["MdaOrigen"]);
                if (!string.IsNullOrEmpty(dr["Observacion"].ToString()))
                    this.Observacion.Value = dr["Observacion"].ToString();
                if (!string.IsNullOrEmpty(dr["Factura"].ToString()))
                    this.Factura.Value = dr["Factura"].ToString();
                if (!string.IsNullOrEmpty(dr["FacturaFecha"].ToString()))
                    this.FacturaFecha.Value = Convert.ToDateTime(dr["FacturaFecha"]);
                if (!string.IsNullOrEmpty(dr["FechaVto"].ToString()))
                    this.FechaVto.Value = Convert.ToDateTime(dr["FechaVto"]);
                if (!string.IsNullOrEmpty(dr["ValorBruto"].ToString()))
                    this.ValorBruto.Value = Convert.ToDecimal(dr["ValorBruto"].ToString());
                if (!string.IsNullOrEmpty(dr["ValorNeto"].ToString()))
                    this.ValorNeto.Value = Convert.ToDecimal(dr["ValorNeto"].ToString());
                if (!string.IsNullOrEmpty(dr["VlrAbono"].ToString()))
                    this.VlrAbono.Value = Convert.ToDecimal(dr["VlrAbono"]);
                if (!string.IsNullOrEmpty(dr["SaldoTotal"].ToString()))
                    this.SaldoTotal.Value = Convert.ToDecimal(dr["SaldoTotal"]);
                //if (!string.IsNullOrEmpty(dr["ValorBrutoEXT"].ToString()))
                //    this.ValorBrutoEXT.Value = Convert.ToDecimal(dr["ValorBrutoEXT"].ToString());
                //if (!string.IsNullOrEmpty(dr["ValorNetoEXT"].ToString()))
                //    this.ValorNetoEXT.Value = Convert.ToDecimal(dr["ValorNetoEXT"].ToString());
                //if (!string.IsNullOrEmpty(dr["VlrAbonoEXT"].ToString()))
                //    this.VlrAbonoEXT.Value = Convert.ToDecimal(dr["VlrAbonoEXT"]);
                //if (!string.IsNullOrEmpty(dr["SaldoTotalEXT"].ToString()))
                //    this.SaldoTotalEXT.Value = Convert.ToDecimal(dr["SaldoTotalEXT"]);
                if (!string.IsNullOrEmpty(dr["TerceroID"].ToString()))
                    this.TerceroID.Value = dr["TerceroID"].ToString();                
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportCuentasPorCobrar()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.TerceroID = new UDT_TerceroID();
            this.Descriptivo = new UDT_Descriptivo();
            this.CuentaId = new UDT_CuentaID();
            //this.CuentaDesc = new UDT_Descriptivo();
            this.Observacion = new UDT_DescripTExt();
            this.Factura = new UDTSQL_char(20);
            this.FacturaFecha = new UDTSQL_datetime();
            this.FechaVto = new UDTSQL_datetime();
            this.Comprobante = new UDTSQL_char(10);
            //this.MdaOrigen = new UDTSQL_char(15);
            this.ValorBruto = new UDT_Valor();
            this.ValorNeto = new UDT_Valor();
            this.VlrAbono = new UDT_Valor();
            this.SaldoTotal = new UDT_Valor();
            //this.ValorBrutoEXT = new UDT_Valor();
            //this.ValorNetoEXT = new UDT_Valor();
            //this.VlrAbonoEXT = new UDT_Valor();
            //this.SaldoTotalEXT = new UDT_Valor();
            
        }
        #endregion

        #region Propiedades
        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_Descriptivo Descriptivo { get; set; }

        [DataMember]
        public UDT_CuentaID CuentaId { get; set; }

        //[DataMember]
        //public UDT_Descriptivo CuentaDesc { get; set; }

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }

        [DataMember]
        public UDTSQL_char Factura { get; set; }

        [DataMember]
        public UDTSQL_datetime FacturaFecha { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaVto { get; set; }

        [DataMember]
        public UDTSQL_char Comprobante { get; set; }

        //[DataMember]
        //public UDTSQL_char MdaOrigen { get; set; }

        [DataMember]
        public UDT_Valor ValorBruto { get; set; }

        [DataMember]
        public UDT_Valor ValorNeto { get; set; }

        [DataMember]
        public UDT_Valor VlrAbono { get; set; }

        [DataMember]
        public UDT_Valor SaldoTotal { get; set; }

        //[DataMember]
        //public UDT_Valor ValorBrutoEXT { get; set; }

        //[DataMember]
        //public UDT_Valor ValorNetoEXT { get; set; }

        //[DataMember]
        //public UDT_Valor VlrAbonoEXT { get; set; }

        //[DataMember]
        //public UDT_Valor SaldoTotalEXT { get; set; } 
        #endregion
    }
}
