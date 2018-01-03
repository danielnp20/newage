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
    public class DTO_ReportPagoFactura
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ReportPagoFactura(IDataReader dr)
        {
            this.InitCols();
            try
            {
                if (!string.IsNullOrEmpty(dr["CuentaID"].ToString()))
                    this.CuentaID.Value = dr["CuentaID"].ToString();
                if (!string.IsNullOrEmpty(dr["DocumentoCOM"].ToString()))
                    this.DocumentoCOM.Value = dr["DocumentoCOM"].ToString();
                if (!string.IsNullOrEmpty(dr["Descriptivo"].ToString()))
                    this.Descriptivo.Value = dr["Descriptivo"].ToString();
                if (!string.IsNullOrEmpty(dr["valor"].ToString()))
                    this.valor.Value = Convert.ToDecimal(dr["valor"]);
                if (!string.IsNullOrEmpty(dr["Documento"].ToString()))
                    this.Documento.Value = dr["Documento"].ToString();
                if (!string.IsNullOrEmpty(dr["PeriodoID"].ToString()))
                    this.PeriodoID.Value = Convert.ToDateTime(dr["PeriodoID"]);
                if (!string.IsNullOrEmpty(dr["ciudad"].ToString()))
                    this.ciudad.Value = dr["ciudad"].ToString();
                if (!string.IsNullOrEmpty(dr["TerceroID"].ToString()))
                    this.TerceroID.Value = dr["TerceroID"].ToString();
                if (!string.IsNullOrEmpty(dr["nombre"].ToString()))
                    this.nombre.Value = dr["nombre"].ToString();
                if (!string.IsNullOrEmpty(dr["banco"].ToString()))
                    this.banco.Value = dr["banco"].ToString();
                if (!string.IsNullOrEmpty(dr["CuentaNumero"].ToString()))
                    this.CuentaNumero.Value = dr["CuentaNumero"].ToString();                
                if (!string.IsNullOrEmpty(dr["NroCheque"].ToString()))
                    this.NroCheque.Value = Convert.ToInt32(dr["NroCheque"]);
                if (!string.IsNullOrEmpty(dr["Comprobante"].ToString()))
                    this.Comprobante.Value = dr["Comprobante"].ToString();
                if (!string.IsNullOrEmpty(dr["ComprobanteCxP"].ToString()))
                    this.ComprobanteCxP.Value = dr["ComprobanteCxP"].ToString();
                if (!string.IsNullOrEmpty(dr["CuentaContable"].ToString()))
                    this.CuentaContable.Value = dr["CuentaContable"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportPagoFactura()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.CuentaID = new UDT_CuentaID();
            this.DocumentoCOM = new UDTSQL_varchar(20);
            this.Descriptivo = new UDT_DescripTBase();
            this.valor = new UDT_Valor();
            this.DocumentoID = new UDT_DocumentoID();
            this.Documento = new UDT_DescripTBase();
            this.PeriodoID = new UDT_PeriodoID();
            this.ciudad = new UDT_DescripTBase();
            this.TerceroID = new UDT_TerceroID();
            this.nombre = new UDT_DescripTBase();
            this.banco = new UDT_DescripTBase();
            this.CuentaNumero = new UDTSQL_char(20);
            this.NroCheque = new UDTSQL_int();
            this.Comprobante = new UDTSQL_char(15);
            this.ComprobanteCxP = new UDTSQL_char(15);
            this.CuentaContable = new UDT_CuentaID();
        }
        #endregion

        #region Propiedades
        [DataMember]
        public UDT_CuentaID CuentaID { get; set; }

        [DataMember]
        public UDTSQL_varchar DocumentoCOM { get; set; }

        [DataMember]
        public UDT_DescripTBase Descriptivo { get; set; }

        [DataMember]
        public UDT_Valor valor { get; set; }

        [DataMember]
        public UDT_DocumentoID DocumentoID { get; set; }

        [DataMember]
        public UDT_DescripTBase Documento { get; set; }

        [DataMember]
        public UDT_PeriodoID PeriodoID { get; set; }

        [DataMember]
        public UDT_DescripTBase ciudad { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_DescripTBase nombre { get; set; }

        [DataMember]
        public UDT_DescripTBase banco { get; set; }

        [DataMember]
        public UDTSQL_char CuentaNumero { get; set; }

        [DataMember]
        public UDTSQL_int NroCheque { get; set; }

        [DataMember]
        public UDTSQL_char Comprobante { get; set; }

        [DataMember]
        public UDTSQL_char ComprobanteCxP { get; set; }

        [DataMember]
        public UDT_CuentaID CuentaContable { get; set; }

        #endregion

    }
}
