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
    /// Class detalle para Busqueda de Documentos - tab Saldos:
    /// Models DTO_BitacoraSaldo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_BitacoraSaldo
    {
        #region DTO_BitacoraSaldo

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_BitacoraSaldo(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.PeriodoID.Value = Convert.ToDateTime(dr["PeriodoID"]);
                this.ComprobanteID.Value = dr["ComprobanteID"].ToString();
                this.ComprobanteNro.Value = Convert.ToInt32(dr["ComprobanteNro"]);
                this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
                this.vlrMdaLoc.Value = Convert.ToDecimal(dr["vlrMdaLoc"]);
                this.vlrMdaExt.Value = Convert.ToDecimal(dr["vlrMdaExt"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_BitacoraSaldo()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.PeriodoID = new UDT_PeriodoID();
            this.ComprobanteID = new UDT_ComprobanteID();
            this.ComprobanteNro = new UDT_Consecutivo();
            this.Fecha = new UDTSQL_smalldatetime();
            this.Descriptivo = new UDT_DescripTBase();
            this.vlrMdaLoc = new UDT_Valor();
            this.vlrMdaExt = new UDT_Valor();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_PeriodoID PeriodoID { get; set; }

        [DataMember]
        public UDT_ComprobanteID ComprobanteID { get; set; }

        [DataMember]
        public UDT_Consecutivo ComprobanteNro { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime Fecha { get; set; }

        [DataMember]
        public UDT_DescripTBase Descriptivo { get; set; }

        [DataMember]
        public UDT_Valor vlrMdaLoc { get; set; }

        [DataMember]
        public UDT_Valor vlrMdaExt { get; set; }

        #endregion
    }
}
