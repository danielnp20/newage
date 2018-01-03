using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.DTO.UDT;
using System.Data;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Models DTO_DeclaracionImpuesto
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_DeclaracionImpuesto
    {
        #region DTO_Declaracion

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_DeclaracionImpuesto(IDataReader dr)
        {
            InitCols();
            try
            {
                this.Seleccionar.Value = false;
                this.ImpuestoDeclID.Value = dr["ImpuestoDeclID"].ToString();
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
                this.AñoFiscal.Value = Convert.ToByte(dr["AñoFiscal"]);
                this.PeriodoCalendario.Value = Convert.ToByte(dr["Periodo"]);
                this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                if (!string.IsNullOrWhiteSpace(dr["NumeroDoc"].ToString()))
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
        public DTO_DeclaracionImpuesto()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Seleccionar = new UDT_SiNo();
            this.ImpuestoDeclID = new UDT_ImpuestoDeclID();
            this.Descriptivo = new UDT_DescripTBase();
            this.AñoFiscal = new UDTSQL_tinyint();
            this.PeriodoCalendario = new UDTSQL_tinyint();
            this.PeriodoConsulta = new UDTSQL_tinyint();
            this.Fecha = new UDTSQL_smalldatetime();
            this.NumeroDoc = new UDT_Consecutivo();
            this.Estado = new UDTSQL_tinyint();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_SiNo Seleccionar { get; set; }

        [DataMember]
        public UDT_ImpuestoDeclID ImpuestoDeclID { get; set; }

        [DataMember]
        public UDT_DescripTBase Descriptivo { get; set; }

        [DataMember]
        public UDTSQL_tinyint AñoFiscal { get; set; }

        [DataMember]
        public UDTSQL_tinyint PeriodoCalendario { get; set; }

        [DataMember]
        public UDTSQL_tinyint PeriodoConsulta { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime Fecha { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDTSQL_tinyint Estado { get; set; }

        [DataMember]
        public string EstadoRsx { get; set; }

        #endregion

    }
}
