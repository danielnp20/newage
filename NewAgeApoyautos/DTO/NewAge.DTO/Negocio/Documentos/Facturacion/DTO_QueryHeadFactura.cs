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
    /// Models DTO_QueryHeadFactura
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_QueryHeadFactura : DTO_BasicReport
    {
        #region Constructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_QueryHeadFactura(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value=Convert.ToInt32(dr["NumeroDoc"]);
                this.PrefDoc.Value = dr["PrefDoc"].ToString();
                if (!string.IsNullOrEmpty(dr["TerceroID"].ToString()))
                    this.TerceroID.Value = dr["TerceroID"].ToString();
                if (!string.IsNullOrEmpty(dr["Nombre"].ToString()))
                    this.Nombre.Value = dr["Nombre"].ToString();
                if (!string.IsNullOrEmpty(dr["ValorBruto"].ToString()))
                    this.ValorBruto.Value = Convert.ToDecimal(dr["ValorBruto"]);
                if (!string.IsNullOrEmpty(dr["MdaPago"].ToString()))
                    this.MdaPago.Value = dr["MdaPago"].ToString();
                if (!string.IsNullOrEmpty(dr["IVA"].ToString()))
                    this.IVA.Value = Convert.ToDecimal(dr["IVA"]);
                if (!string.IsNullOrEmpty(dr["Fecha"].ToString()))
                    this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                if (!string.IsNullOrEmpty(dr["Observacion"].ToString()))
                    this.Observacion.Value = dr["Observacion"].ToString();
                if (!string.IsNullOrEmpty(dr["SaldoLoc"].ToString()))
                    this.SaldoLoc.Value = Convert.ToDecimal(dr["SaldoLoc"]);
                if (!string.IsNullOrEmpty(dr["Valor"].ToString()))
                    this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                if (!string.IsNullOrEmpty(dr["Prefijo"].ToString()))
                    this.Prefijo.Value = dr["Prefijo"].ToString();
                this.DatoAdd1.Value = dr["DatoAdd1"].ToString();
                if (!string.IsNullOrEmpty(dr["FacturaFijaInd"].ToString()))
                    this.FacturaFijaInd.Value = Convert.ToBoolean(dr["FacturaFijaInd"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_QueryHeadFactura()
        {
            InitCols();
        }

        // Inicializa las columnas
        private void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.PrefDoc = new UDT_DescripTBase();
            this.TerceroID = new UDT_TerceroID();
            this.Nombre = new UDT_DescripTExt();
            this.ValorBruto = new UDT_Valor();
            this.MdaPago = new UDT_MonedaID();
            this.IVA = new UDT_Valor();
            this.Fecha = new UDTSQL_datetime();
            this.Observacion = new UDT_DescripTExt();
            this.SaldoLoc = new UDT_Valor();
            this.Valor = new UDT_Valor();
            this.Prefijo = new UDT_PrefijoID();
            this.DatoAdd1 = new UDTSQL_char(20);
            this.FacturaFijaInd = new UDT_SiNo();
        }

        #endregion

        #region Propiedades

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_DescripTBase PrefDoc { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_DescripTExt Nombre { get; set; }

        [DataMember]
        public UDT_Valor ValorBruto { get; set; }

        [DataMember]
        public UDT_MonedaID MdaPago { get; set; }

        [DataMember]
        public UDT_Valor IVA { get; set; }

        [DataMember]
        public UDTSQL_datetime Fecha { get; set; }

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }

        [DataMember]
        public UDT_Valor SaldoLoc { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_PrefijoID Prefijo { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char DatoAdd1 { get; set; }         

        [DataMember]
        [AllowNull]
        public UDT_SiNo FacturaFijaInd { get; set; }

        //Detalle
        [DataMember]
        public List<DTO_QueryDetailFactura> Detalle { get; set; }
        #endregion
    }
}
