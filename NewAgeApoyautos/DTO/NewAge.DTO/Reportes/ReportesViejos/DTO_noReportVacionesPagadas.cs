using System;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_noReportVacionesPagadas
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_noReportVacionesPagadas
    {
        #region Constructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noReportVacionesPagadas(IDataReader dr)
        {
            InitCols();
            try
            {
                if (!string.IsNullOrWhiteSpace(dr["ConceptoNOID"].ToString()))
                    this.Cedula.Value = dr["ConceptoNOID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["FechaFin1"].ToString()))
                    this.FechaFin1 = Convert.ToDateTime(dr["FechaFin1"].ToString());
                else
                    this.FechaFin1 = null;
                if (!string.IsNullOrWhiteSpace(dr["FechaFin2"].ToString()))
                    this.FechaFin2 = Convert.ToDateTime(dr["FechaFin2"].ToString());
                else
                    this.FechaFin2 = null;
                if (!string.IsNullOrWhiteSpace(dr["FechaIni1"].ToString()))
                    this.FechaIni1 = Convert.ToDateTime(dr["FechaIni1"].ToString());
                else
                    this.FechaIni1 = null;
                if (!string.IsNullOrWhiteSpace(dr["FechaIni2"].ToString()))
                    this.FechaIni2 = Convert.ToDateTime(dr["FechaIni2"].ToString());
                else
                    this.FechaIni2 = null;
                if (!string.IsNullOrWhiteSpace(dr["Fecha"].ToString()))
                    this.Fecha = Convert.ToDateTime(dr["Fecha"].ToString());
                else
                    this.Fecha = null;
                if (!string.IsNullOrWhiteSpace(dr["Descriptivo"].ToString()))
                    this.Descriptivo.Value = dr["Descriptivo"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["Dias1"].ToString()))
                    this.Dias1 = Convert.ToInt16(dr["Dias1"].ToString());
                if (!string.IsNullOrWhiteSpace(dr["Dias2"].ToString()))
                    this.Dias2 = Convert.ToInt16(dr["Dias2"].ToString());
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DTO_noReportVacionesPagadas(IDataReader dr, bool isNullble)
        {
            InitCols();
            try
            {

            }
            catch (Exception e)
            { ; }
        }
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noReportVacionesPagadas()
        {
            InitCols();
        }
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Cedula = new UDT_TerceroID();
            this.FechaFin1 = new DateTime();
            this.FechaFin2 = new DateTime();
            this.FechaIni1 = new DateTime();
            this.FechaIni2 = new DateTime();
            this.Fecha = new DateTime();
            this.Descriptivo = new UDT_DescripTBase();
        }
        #endregion
        #region Propiedades

        [DataMember]
        public DateTime? FechaIni1 { get; set; }
        [DataMember]
        public DateTime? FechaFin1 { get; set; }
        [DataMember]
        public DateTime? FechaIni2 { get; set; }
        [DataMember]
        public DateTime? FechaFin2 { get; set; }
        [DataMember]
        public UDT_TerceroID Cedula { get; set; }
        [DataMember]
        public UDT_DescripTBase Descriptivo { get; set; }
        [DataMember]
        public DateTime? Fecha { get; set; }

        [DataMember]
        public int Dias1 { get; set; }
        [DataMember]
        public int Dias2 { get; set; }

        [DataMember]
        public int DiasTotal { get; set; }
        [DataMember]
        public int DiasSubtotal { get; set; }

        #endregion
    }
}
