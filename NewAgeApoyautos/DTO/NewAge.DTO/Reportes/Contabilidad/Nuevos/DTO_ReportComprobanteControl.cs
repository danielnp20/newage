using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.DTO.Negocio.Reportes;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Reportes
{
    [DataContract]
    [Serializable]

    /// <summary>
    /// Clase del reporte Comrprobante Control
    /// </summary>
    public class DTO_ReportComprobanteControl : DTO_BasicReport
    {
        /// <summary>
        /// Constructor con DataReader
        /// </summary>
        public DTO_ReportComprobanteControl(IDataReader dr)
        {
            InitCols();
            try
            {
                if (!string.IsNullOrEmpty(dr["PeriodoID"].ToString()))
                    this.PeriodoID = Convert.ToDateTime(dr["PeriodoID"]);
                if (!string.IsNullOrEmpty(dr["ComprobanteID"].ToString()))
                    this.ComprobanteID.Value = dr["ComprobanteID"].ToString();
                if (!string.IsNullOrEmpty(dr["ComprobanteNro"].ToString()))
                    this.ComprobanteNro = Convert.ToInt32(dr["ComprobanteNro"]);
                if (!string.IsNullOrEmpty(dr["ComprobanteDesc"].ToString()))
                    this.ComprobanteDesc.Value = dr["ComprobanteDesc"].ToString();
                if (!string.IsNullOrEmpty(dr["RecordQty"].ToString()))
                    this.RecordQty = Convert.ToInt32(dr["RecordQty"]);

            }
            catch (Exception)
            {
                throw;
            }

        }

        public DTO_ReportComprobanteControl(IDataReader dr, bool isNullble)
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
        public DTO_ReportComprobanteControl()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.PeriodoID = new DateTime();
            this.ComprobanteID = new UDT_ComprobanteID();
            this.ComprobanteNro = new int();
            this.ComprobanteDesc = new UDT_Descriptivo();
            this.RecordQty = new int();

        }

        #region Propiedades

        [DataMember]
        public DateTime PeriodoID { get; set; }

        [DataMember]
        public UDT_ComprobanteID ComprobanteID { get; set; }

        [DataMember]
        public int ComprobanteNro { get; set; }

        [DataMember]
        public UDT_Descriptivo ComprobanteDesc { get; set; }

        [DataMember]
        public int RecordQty { get; set; }

        #endregion

    }

}
