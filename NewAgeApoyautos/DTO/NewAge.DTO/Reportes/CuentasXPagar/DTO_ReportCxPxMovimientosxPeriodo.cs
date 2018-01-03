using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.DTO.Attributes;
using NewAge.DTO.Negocio.Reportes;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Reportes
{
    [DataContract]
    [Serializable]

    // <summary>
    // Clase del reporte Cuentas por Pagar Movimientos por Periodo
    // </summary>
    public class DTO_ReportCxPxMovimientosxPeriodo : DTO_BasicReport
    {
        /// <summary>
        /// Constructor con DataReader
        /// </summary>
        public DTO_ReportCxPxMovimientosxPeriodo(IDataReader dr)
        {
            InitCols();
            try
            {
                this.DocumentoTercero.Value = dr["DocumentoTercero"].ToString();
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
                this.CuentaID.Value = dr["CuentaID"].ToString();
                this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                this.ComprobanteNro.Value = Convert.ToInt32(dr["ComprobanteNro"]);
                this.vlrMdaLoc.Value = Convert.ToInt32(dr["vlrMdaLoc"]);
                this.vlrMdaExt.Value = Convert.ToInt32(dr["vlrMdaExt"]);
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.SaldoMdaLoc.Value = Convert.ToInt32(dr["SaldoMdaLoc"]);
                this.SaldoMdaExt.Value = Convert.ToInt32(dr["SaldoMdaExt"]);
                this.TerceroID.Value = dr["TerceroID"].ToString();
                try
                { this.TerceroDesc.Value = dr["TerceroDesc"].ToString(); }catch (Exception e) { }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private DTO_ReportCxPxMovimientosxPeriodo()
        {
            this.InitCols();
        }

        // Inicializa las columnas
        // </summary>
        private void InitCols()
        {
            this.DocumentoTercero = new UDTSQL_char(20);
            this.Descriptivo = new UDT_DescripTExt();
            this.CuentaID = new UDT_CuentaID();
            this.Fecha = new UDTSQL_smalldatetime();
            this.ComprobanteNro = new UDT_Consecutivo();
            this.vlrMdaLoc = new UDT_Valor();
            this.vlrMdaExt = new UDT_Valor();
            this.TerceroID = new UDT_TerceroID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.SaldoMdaExt = new UDT_Valor();
            this.SaldoMdaLoc = new UDT_Valor();
            this.Codigo = new UDTSQL_char(20);
            this.TerceroDesc = new UDT_DescripTBase();
        }

        #region Propiedades
        
        [DataMember]
        [AllowNull]
        public UDTSQL_char DocumentoTercero { get; set; }

        [AllowNull]
        [DataMember]
        public UDT_DescripTExt Descriptivo { get; set; }

        [AllowNull]
        [DataMember]
        public UDT_CuentaID CuentaID { get; set; }

        [AllowNull]
        [DataMember]
        public UDTSQL_smalldatetime Fecha { get; set; }

        [AllowNull]
        [DataMember]
        public UDT_Consecutivo  ComprobanteNro { get; set; }

        [AllowNull]
        [DataMember]
        public UDT_Valor vlrMdaLoc { get; set; }

        [DataMember]
        public UDT_Valor vlrMdaExt { get; set; }

        [AllowNull]
        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [AllowNull]
        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [AllowNull]
        [DataMember]
        public UDT_Valor SaldoMdaLoc { get; set; }

        [DataMember]
        public UDT_Valor SaldoMdaExt { get; set; }

        /// <summary>
        /// TerceroID
        /// </summary>
        [DataMember]
        [AllowNull]
        public UDTSQL_char Codigo { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTBase TerceroDesc { get; set; }

        

        #endregion

    }
}


