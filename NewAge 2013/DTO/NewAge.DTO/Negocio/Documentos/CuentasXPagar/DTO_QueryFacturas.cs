using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.Attributes;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using NewAge.DTO.Negocio.Reportes;
using NewAge.DTO.Reportes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_QueryFacturas
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_QueryFacturas : DTO_BasicReport
    {
        #region Constructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_QueryFacturas(IDataReader dr)
        {
            InitCols();
            try
            {
                this.MonedaID.Value = dr["MonedaID"].ToString();
                this.numFac.Value = dr["numFac"].ToString();
                this.numIdentifica.Value = dr["numIdentifica"].ToString();
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
                if (!string.IsNullOrEmpty(dr["FacturaFecha"].ToString()))
                    this.FacturaFecha.Value = Convert.ToDateTime(dr["FacturaFecha"]);
                this.Observacion.Value = dr["Observacion"].ToString();      
                if (!string.IsNullOrEmpty(dr["Iva"].ToString()))
                    this.Iva.Value = Convert.ToDecimal(dr["Iva"]);
                if (!string.IsNullOrEmpty(dr["SaldoLoc"].ToString()))
                    this.SaldoLoc.Value = Convert.ToDecimal(dr["SaldoLoc"]);
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_QueryFacturas()
        {
            InitCols();
        }

        // Inicializa las columnas
        private void InitCols()
        {
            this.MonedaID = new UDT_MonedaID();        
            this.numFac = new UDTSQL_char(15);
            this.numIdentifica = new UDT_TerceroID();
            this.Descriptivo = new UDT_DescripTBase();
            this.FacturaFecha = new UDTSQL_datetime();
            this.Observacion = new UDT_DescripTExt();
            this.Valor = new UDT_Valor();
            this.Iva = new UDT_Valor();
            this.SaldoLoc = new UDT_Valor();
            this.NumeroDoc = new UDT_Consecutivo();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_MonedaID MonedaID { get; set; }

        [DataMember]
        public UDTSQL_char numFac { get; set; }

        [DataMember]
        public UDT_TerceroID numIdentifica { get; set; }

        [DataMember]
        public UDT_DescripTBase Descriptivo { get; set; }

        [DataMember]
        public UDTSQL_datetime FacturaFecha { get; set; }

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_Valor Iva { get; set; }

        [DataMember]
        public UDT_Valor SaldoLoc { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumeroDoc { get; set; }

        //Detalle
        [DataMember]
        public List<DTO_QueryFacturasDetail> Detalle { get; set; }
        #endregion
    }
}
