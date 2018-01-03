using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class comprobante para aprobacion:
    /// Models DTO_AnticipoAprobacion
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_LegalizacionAprobacion : DTO_ComprobanteAprobacion
    {
        #region DTO_AnticipoAprobacion

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_LegalizacionAprobacion(IDataReader dr) : base(dr)
        {
            InitCols();
            try
            {
                this.CajaMenorID.Value = dr["CajaMenorID"].ToString();
                this.TarjetaCreditoID.Value = dr["TarjetaCreditoID"].ToString(); 
                this.TerceroID.Value = dr["TerceroID"].ToString(); 
                this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorTarjeta"].ToString()))
                    this.ValorTarjeta.Value = Convert.ToDecimal(dr["ValorTarjeta"]); 
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_LegalizacionAprobacion()
            : base()
        {
            InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.CajaMenorID = new UDT_CajaMenorID();
            this.TerceroID = new UDT_TerceroID();
            this.TarjetaCreditoID = new UDT_TarjetaCreditoID();
            this.NombreTercero = new UDT_DescripTBase();
            this.Fecha = new UDTSQL_smalldatetime();
            this.Valor = new UDT_Valor();
            this.ValorTarjeta = new UDT_Valor();
            this.NumeroDocCxP = new UDT_Consecutivo();
            this.FileUrl = "";
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_CajaMenorID CajaMenorID { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_TarjetaCreditoID TarjetaCreditoID { get; set; }

        [DataMember]
        public UDT_DescripTBase NombreTercero { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime Fecha { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorTarjeta { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDocCxP { get; set; }

        [DataMember]
        public string FileUrl { get; set; }

        #endregion
    }
}
