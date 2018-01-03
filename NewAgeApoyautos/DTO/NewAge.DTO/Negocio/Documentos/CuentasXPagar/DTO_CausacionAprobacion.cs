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
    /// Class comprobante para aprobacion:
    /// Models DTO_CausacionAprobacion
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_CausacionAprobacion : DTO_ComprobanteAprobacion
    {
        #region DTO_CausacionAprobacion

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_CausacionAprobacion(IDataReader dr) : base(dr)
        {
            InitCols();
            try
            {
                this.Descripcion.Value = dr["Descripcion"].ToString();
                this.TerceroID.Value = dr["TerceroID"].ToString();
                this.DescriptivoTercero.Value = dr["DescriptivoTercero"].ToString();
                this.DocumentoTercero.Value = dr["DocumentoTercero"].ToString();
                this.MonedaID.Value = dr["MonedaID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["TasaCambio"].ToString()))
                    this.TasaCambio.Value = Convert.ToDecimal(dr["TasaCambio"]);

                this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                this.FacturaFecha.Value = Convert.ToDateTime(dr["FacturaFecha"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_CausacionAprobacion()
            : base()
        {
            InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.Descripcion = new UDT_DescripTBase();
            this.TerceroID = new UDT_TerceroID();
            this.DescriptivoTercero = new UDT_DescripTBase();
            this.DocumentoTercero = new UDTSQL_char(20);
            this.MonedaID = new UDT_MonedaID();
            this.TasaCambio = new UDT_PorcentajeID();
            this.Valor = new UDT_Valor();
            this.FacturaFecha = new UDT_PeriodoID();
            this.FileUrl = "";
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_DescripTBase Descripcion { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_DescripTBase DescriptivoTercero { get; set; }

        [DataMember]
        public UDTSQL_char DocumentoTercero { get; set; }

        [DataMember]
        public UDT_MonedaID MonedaID { get; set; }

        [DataMember]
        public UDT_PorcentajeID TasaCambio { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_PeriodoID FacturaFecha { get; set; }

        [DataMember]
        public string FileUrl { get; set; }

        #endregion
    }
}
